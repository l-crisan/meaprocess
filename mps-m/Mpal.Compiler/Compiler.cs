//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Collections;

using System.Text;
using System.IO;
using System.Globalization;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

using Mpal.Parser;
using Mpal.Model;

namespace Mpal.Compiler
{
    /// <summary>
    /// MPAL Compiler.
    /// </summary>
    public class Compiler
    {
        private string       _binModSearchPath;
        private string       _srcModSearchPath;
        private string       _currentPath;
        private string       _file;
        private const ulong  COMP_VERSION = 3;
        private bool        _parseError = false;
        private CompilerOptions _options;
        
        /// <summary>
        /// Construct a new MPAL compiler object.
        /// </summary>
        /// <param name="binModSearchPath">The search path for binary MPAL units.</param>
        /// <param name="srcModSearchPath">The search path for source MPAL units.</param>
        public Compiler(string binModSearchPath, string srcModSearchPath)
        {
            _binModSearchPath = binModSearchPath;
            _srcModSearchPath = srcModSearchPath;
        }

        /// <summary>
        /// On message event. Emited by the compilation.
        /// </summary>
        public event Message OnMessage;
        
        /// <summary>
        /// Compile a unit.
        /// </summary>
        /// <param name="file">The unit file.</param>
        /// <param name="units">The compiled units.</param>
        /// <param name="vmAddressSize">The virtual machine address size in byte</param>
        /// <returns>The current compiled unit.</returns>
        /// <remarks>The current compiled unit is added into units too.</remarks>
        public bool Compile(string file, Unit unit, CompilerOptions options)
        {
            unit.Clear();
            _options = options;
            try
            {
                _currentPath = Path.GetDirectoryName(file);
                _file = file;
                _parseError = false;

                if (!File.Exists(_file))
                {
                    OutputMessage(": error S1001: File not found");
                    return false;
                }

                Preprocessor pp = new Preprocessor();
                pp.ScanFile(file, unit);

                BaseTree tree = ParseFile(file);

                if (tree == null || _parseError)
                    return false;

                return CompileTree(tree, unit);
            }
            catch (Exception ex)
            {
                OnMessage(ex.Message);
                return false;
            }
        }

        public bool CompileText(string text, Unit unit, CompilerOptions options)
        {
            try
            {
                _options = options;
                
                unit.Clear();

                Preprocessor pp = new Preprocessor();
                pp.ScanText(text, unit);

                _parseError = false;

                BaseTree tree = ParseText(text);

                if (tree == null || _parseError)
                    return false;

                return CompileTree(tree, unit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            
        }

        public bool ScanInterface(string text, Unit unit)
        {
            try
            {
                unit.Clear();
                BaseTree tree = ParseText(text);

                if (tree == null)
                    return false;

                unit.Version = COMP_VERSION;

                //Scan the interface.
                IfaceScaner ifaceScaner = new IfaceScaner();
                ifaceScaner.OnMessage += new Message(OutputMessage);

                ifaceScaner.Scan(unit, tree, false, _options);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }            
        }

        /// <summary>
        /// Gets the compiler version.
        /// </summary>
        public ulong Version
        {
            get{ return COMP_VERSION;}
        }

        private BaseTree ParseFile(string file)
        {
            ANTLRFileStream inputStream = new ANTLRFileStream(file, System.Text.Encoding.UTF8);

            int size = inputStream.Size();

            Mpal.Parser.Lexer lexer = new Mpal.Parser.Lexer(inputStream);
            lexer.OnNewError += new ErrorMessage(ScanerParserOnMessage);

            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            
            Parser.Parser parser = new Parser.Parser(tokenStream);            
            parser.OnNewError += new ErrorMessage(ScanerParserOnMessage);

            //Start parsing.        
            Parser.Parser.mpal_return module = parser.mpal();
            BaseTree tree = (BaseTree)module.Tree;
                       
            return tree;
        }


        private BaseTree ParseText(string text)
        {
            ANTLRStringStream inputStream = new ANTLRStringStream(text);
            Mpal.Parser.Lexer lexer = new Mpal.Parser.Lexer(inputStream);
            lexer.OnNewError += new ErrorMessage(ScanerParserOnMessage);

            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            Parser.Parser parser = new Parser.Parser(tokenStream);
            parser.OnNewError += new ErrorMessage(ScanerParserOnMessage);

            BaseTree tree = null;

            //Start parsing.
            Parser.Parser.mpal_return module = parser.mpal();

            tree = (BaseTree)module.Tree;

            return tree;
        }

        private bool CompileTree(BaseTree tree, Unit unit)
        {
            unit.Version = COMP_VERSION;
            unit.UserVersion[0] = _options.VmAddressSize;

            //Scan the interface.
            IfaceScaner ifaceScaner = new IfaceScaner();
            ifaceScaner.OnMessage += new Message(OutputMessage);

            if (!ifaceScaner.Scan(unit, tree, true, _options))
                return false;

            //Compile the interface.
            IfaceCompiler ifaceCompiler = new IfaceCompiler();
            ifaceCompiler.OnMessage += new Message(OutputMessage);
            
            if (!ifaceCompiler.Compile(unit, _options))
                return false;

            STCodeGenerator generator = new STCodeGenerator(_options);
            generator.OnMessage += new Message(OutputMessage);
           
            //Generate code.
            foreach (DictionaryEntry entry in unit.Functions)
                generator.Generate(unit, (Function) entry.Value);

            return true;
        }

        private void ScanerParserOnMessage(string msg)
        {
            _parseError = true;
            OutputMessage(msg);
        }

        private void OutputMessage(string msg)
        {
            if (OnMessage != null)
                OnMessage(_file + msg);
        }
    }
}
