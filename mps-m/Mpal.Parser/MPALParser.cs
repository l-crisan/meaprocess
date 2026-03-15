// $ANTLR 3.2 Sep 23, 2009 12:02:23 J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g 2010-10-26 14:53:58

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



using Antlr.Runtime.Tree;

/******************************************************************************************************
* MPAL grammar                                                                                        *
*                                                                                                     *
* Copyright (C) 2006-2010 Atesion GmbH. All rights reserved.                             *
*******************************************************************************************************/
public partial class MPALParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"PROGRAM", 
		"FUNCTION", 
		"VAR", 
		"VAR_TEMP", 
		"CONSTANT", 
		"VAR_INPUT", 
		"VAR_IN_OUT", 
		"STRING", 
		"WSTRING", 
		"VAR_OUTPUT", 
		"ARRAY", 
		"OF", 
		"END_FUNCTION", 
		"FUNCTION_BLOCK", 
		"END_FUNCTION_BLOCK", 
		"NOT", 
		"EXIT", 
		"ASSIGN", 
		"ASSIGN2", 
		"COLON", 
		"LBRACKED", 
		"LRBRACKED", 
		"COMMA", 
		"DIV", 
		"MOD", 
		"MUL", 
		"OR", 
		"XOR", 
		"EQU", 
		"VOID", 
		"SINT", 
		"INT", 
		"DINT", 
		"LINT", 
		"USINT", 
		"UINT", 
		"UDINT", 
		"ULINT", 
		"COUNTER", 
		"TIMER", 
		"TIME", 
		"DATE_LIT", 
		"TIME_OF_DAY_LIT", 
		"TOD", 
		"DATE_AND_TIME", 
		"DT", 
		"BOOL", 
		"BYTE", 
		"WORD", 
		"DWORD", 
		"LWORD", 
		"REAL", 
		"LREAL", 
		"CHAR", 
		"WCHAR", 
		"R_EDGE", 
		"F_EDGE", 
		"STRUCT", 
		"LEQ", 
		"GEQ", 
		"NEQ", 
		"IF", 
		"CASE", 
		"FOR", 
		"TO", 
		"BY", 
		"DO", 
		"WHILE", 
		"REPEAT", 
		"CONTINUE", 
		"TRUE", 
		"FALSE", 
		"ELSE", 
		"ELSIF", 
		"USES", 
		"TYPE", 
		"UNTIL", 
		"THEN", 
		"LS", 
		"GR", 
		"POW", 
		"RETURN", 
		"SHARP", 
		"PLUS", 
		"NEG", 
		"DOTDOT", 
		"END_PROGRAM", 
		"IDENTIFIER", 
		"INTEGER", 
		"STRING_LITERAL_UNI", 
		"STRING_LITERAL", 
		"REAL_CONSTANT", 
		"BINARY_INTEGER", 
		"OCTAL_INTEGER", 
		"HEX_INTEGER", 
		"AND", 
		"DOT", 
		"EXPONENT", 
		"COMMENT", 
		"PRAGMAS", 
		"LINE_COMMENT", 
		"WS", 
		"']'", 
		"')'", 
		"';'", 
		"'END_STRUCT'", 
		"'END_TYPE'", 
		"'END_VAR'", 
		"'END_FOR'", 
		"'END_WHILE'", 
		"'END_REPEAT'", 
		"'END_IF'", 
		"'END_CASE'"
    };

    public const int FUNCTION = 5;
    public const int EXPONENT = 101;
    public const int WHILE = 71;
    public const int DT = 49;
    public const int LREAL = 56;
    public const int MOD = 28;
    public const int STRING_LITERAL_UNI = 93;
    public const int LS = 82;
    public const int F_EDGE = 60;
    public const int CASE = 66;
    public const int DATE_AND_TIME = 48;
    public const int CHAR = 57;
    public const int DO = 70;
    public const int SINT = 34;
    public const int NOT = 19;
    public const int EOF = -1;
    public const int WCHAR = 58;
    public const int TYPE = 79;
    public const int WORD = 52;
    public const int STRING_LITERAL = 94;
    public const int POW = 84;
    public const int RETURN = 85;
    public const int LBRACKED = 24;
    public const int USINT = 38;
    public const int VAR = 6;
    public const int VOID = 33;
    public const int GEQ = 63;
    public const int COMMENT = 102;
    public const int ARRAY = 14;
    public const int OCTAL_INTEGER = 97;
    public const int EXIT = 20;
    public const int VAR_OUTPUT = 13;
    public const int SHARP = 86;
    public const int LINE_COMMENT = 104;
    public const int ELSE = 76;
    public const int BOOL = 50;
    public const int INT = 35;
    public const int LRBRACKED = 25;
    public const int LWORD = 54;
    public const int OF = 15;
    public const int MUL = 29;
    public const int REAL = 55;
    public const int BINARY_INTEGER = 96;
    public const int WS = 105;
    public const int USES = 78;
    public const int UNTIL = 80;
    public const int OR = 30;
    public const int CONSTANT = 8;
    public const int GR = 83;
    public const int REPEAT = 72;
    public const int ELSIF = 77;
    public const int DINT = 36;
    public const int FALSE = 75;
    public const int ULINT = 41;
    public const int VAR_TEMP = 7;
    public const int TIME_OF_DAY_LIT = 46;
    public const int UINT = 39;
    public const int T__116 = 116;
    public const int T__114 = 114;
    public const int ASSIGN2 = 22;
    public const int T__115 = 115;
    public const int DATE_LIT = 45;
    public const int FOR = 67;
    public const int DOTDOT = 89;
    public const int AND = 99;
    public const int REAL_CONSTANT = 95;
    public const int IF = 65;
    public const int TIME = 44;
    public const int THEN = 81;
    public const int END_FUNCTION = 16;
    public const int T__107 = 107;
    public const int DWORD = 53;
    public const int CONTINUE = 73;
    public const int COMMA = 26;
    public const int T__108 = 108;
    public const int T__109 = 109;
    public const int UDINT = 40;
    public const int IDENTIFIER = 91;
    public const int T__106 = 106;
    public const int T__111 = 111;
    public const int T__110 = 110;
    public const int T__113 = 113;
    public const int PLUS = 87;
    public const int T__112 = 112;
    public const int WSTRING = 12;
    public const int DOT = 100;
    public const int PRAGMAS = 103;
    public const int FUNCTION_BLOCK = 17;
    public const int INTEGER = 92;
    public const int BYTE = 51;
    public const int BY = 69;
    public const int XOR = 31;
    public const int TO = 68;
    public const int LINT = 37;
    public const int STRUCT = 61;
    public const int TRUE = 74;
    public const int COLON = 23;
    public const int COUNTER = 42;
    public const int TOD = 47;
    public const int NEQ = 64;
    public const int END_PROGRAM = 90;
    public const int HEX_INTEGER = 98;
    public const int VAR_INPUT = 9;
    public const int NEG = 88;
    public const int ASSIGN = 21;
    public const int R_EDGE = 59;
    public const int EQU = 32;
    public const int PROGRAM = 4;
    public const int DIV = 27;
    public const int END_FUNCTION_BLOCK = 18;
    public const int VAR_IN_OUT = 10;
    public const int TIMER = 43;
    public const int STRING = 11;
    public const int LEQ = 62;

    // delegates
    // delegators



        public MPALParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public MPALParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();

             
        }
        
    protected ITreeAdaptor adaptor = new CommonTreeAdaptor();

    public ITreeAdaptor TreeAdaptor
    {
        get { return this.adaptor; }
        set {
    	this.adaptor = value;
    	}
    }

    override public string[] TokenNames {
		get { return MPALParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g"; }
    }


    public class mpal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "mpal"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:110:1: mpal : ( program_declaration | data_type_declaration | function_declaration | function_block_declaration )+ EOF ;
    public MPALParser.mpal_return mpal() // throws RecognitionException [1]
    {   
        MPALParser.mpal_return retval = new MPALParser.mpal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken EOF5 = null;
        MPALParser.program_declaration_return program_declaration1 = default(MPALParser.program_declaration_return);

        MPALParser.data_type_declaration_return data_type_declaration2 = default(MPALParser.data_type_declaration_return);

        MPALParser.function_declaration_return function_declaration3 = default(MPALParser.function_declaration_return);

        MPALParser.function_block_declaration_return function_block_declaration4 = default(MPALParser.function_block_declaration_return);


        object EOF5_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:113:6: ( ( program_declaration | data_type_declaration | function_declaration | function_block_declaration )+ EOF )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:113:8: ( program_declaration | data_type_declaration | function_declaration | function_block_declaration )+ EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:113:8: ( program_declaration | data_type_declaration | function_declaration | function_block_declaration )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 5;
            	    switch ( input.LA(1) ) 
            	    {
            	    case PROGRAM:
            	    	{
            	        alt1 = 1;
            	        }
            	        break;
            	    case TYPE:
            	    	{
            	        alt1 = 2;
            	        }
            	        break;
            	    case FUNCTION:
            	    	{
            	        alt1 = 3;
            	        }
            	        break;
            	    case FUNCTION_BLOCK:
            	    	{
            	        alt1 = 4;
            	        }
            	        break;

            	    }

            	    switch (alt1) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:113:9: program_declaration
            			    {
            			    	PushFollow(FOLLOW_program_declaration_in_mpal1548);
            			    	program_declaration1 = program_declaration();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, program_declaration1.Tree);

            			    }
            			    break;
            			case 2 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:113:31: data_type_declaration
            			    {
            			    	PushFollow(FOLLOW_data_type_declaration_in_mpal1552);
            			    	data_type_declaration2 = data_type_declaration();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, data_type_declaration2.Tree);

            			    }
            			    break;
            			case 3 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:113:54: function_declaration
            			    {
            			    	PushFollow(FOLLOW_function_declaration_in_mpal1555);
            			    	function_declaration3 = function_declaration();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, function_declaration3.Tree);

            			    }
            			    break;
            			case 4 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:113:78: function_block_declaration
            			    {
            			    	PushFollow(FOLLOW_function_block_declaration_in_mpal1560);
            			    	function_block_declaration4 = function_block_declaration();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, function_block_declaration4.Tree);

            			    }
            			    break;

            			default:
            			    if ( cnt1 >= 1 ) goto loop1;
            		            EarlyExitException eee1 =
            		                new EarlyExitException(1, input);
            		            throw eee1;
            	    }
            	    cnt1++;
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

            	EOF5=(IToken)Match(input,EOF,FOLLOW_EOF_in_mpal1565); 
            		EOF5_tree = (object)adaptor.Create(EOF5);
            		adaptor.AddChild(root_0, EOF5_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "mpal"

    public class program_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "program"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:115:1: program : PROGRAM IDENTIFIER ( io_var_declarations | other_var_declarations | data_type_declaration )* function_body END_PROGRAM ;
    public MPALParser.program_return program() // throws RecognitionException [1]
    {   
        MPALParser.program_return retval = new MPALParser.program_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken PROGRAM6 = null;
        IToken IDENTIFIER7 = null;
        IToken END_PROGRAM12 = null;
        MPALParser.io_var_declarations_return io_var_declarations8 = default(MPALParser.io_var_declarations_return);

        MPALParser.other_var_declarations_return other_var_declarations9 = default(MPALParser.other_var_declarations_return);

        MPALParser.data_type_declaration_return data_type_declaration10 = default(MPALParser.data_type_declaration_return);

        MPALParser.function_body_return function_body11 = default(MPALParser.function_body_return);


        object PROGRAM6_tree=null;
        object IDENTIFIER7_tree=null;
        object END_PROGRAM12_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:115:9: ( PROGRAM IDENTIFIER ( io_var_declarations | other_var_declarations | data_type_declaration )* function_body END_PROGRAM )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:115:11: PROGRAM IDENTIFIER ( io_var_declarations | other_var_declarations | data_type_declaration )* function_body END_PROGRAM
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PROGRAM6=(IToken)Match(input,PROGRAM,FOLLOW_PROGRAM_in_program1576); 
            		PROGRAM6_tree = (object)adaptor.Create(PROGRAM6);
            		root_0 = (object)adaptor.BecomeRoot(PROGRAM6_tree, root_0);

            	IDENTIFIER7=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_program1579); 
            		IDENTIFIER7_tree = (object)adaptor.Create(IDENTIFIER7);
            		adaptor.AddChild(root_0, IDENTIFIER7_tree);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:115:31: ( io_var_declarations | other_var_declarations | data_type_declaration )*
            	do 
            	{
            	    int alt2 = 4;
            	    alt2 = dfa2.Predict(input);
            	    switch (alt2) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:115:32: io_var_declarations
            			    {
            			    	PushFollow(FOLLOW_io_var_declarations_in_program1582);
            			    	io_var_declarations8 = io_var_declarations();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, io_var_declarations8.Tree);

            			    }
            			    break;
            			case 2 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:115:55: other_var_declarations
            			    {
            			    	PushFollow(FOLLOW_other_var_declarations_in_program1587);
            			    	other_var_declarations9 = other_var_declarations();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, other_var_declarations9.Tree);

            			    }
            			    break;
            			case 3 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:115:80: data_type_declaration
            			    {
            			    	PushFollow(FOLLOW_data_type_declaration_in_program1591);
            			    	data_type_declaration10 = data_type_declaration();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, data_type_declaration10.Tree);

            			    }
            			    break;

            			default:
            			    goto loop2;
            	    }
            	} while (true);

            	loop2:
            		;	// Stops C# compiler whining that label 'loop2' has no statements

            	PushFollow(FOLLOW_function_body_in_program1595);
            	function_body11 = function_body();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, function_body11.Tree);
            	END_PROGRAM12=(IToken)Match(input,END_PROGRAM,FOLLOW_END_PROGRAM_in_program1597); 
            		END_PROGRAM12_tree = (object)adaptor.Create(END_PROGRAM12);
            		adaptor.AddChild(root_0, END_PROGRAM12_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "program"

    public class program_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "program_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:117:1: program_declaration : PROGRAM IDENTIFIER ( io_var_declarations | other_var_declarations )* function_body END_PROGRAM ;
    public MPALParser.program_declaration_return program_declaration() // throws RecognitionException [1]
    {   
        MPALParser.program_declaration_return retval = new MPALParser.program_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken PROGRAM13 = null;
        IToken IDENTIFIER14 = null;
        IToken END_PROGRAM18 = null;
        MPALParser.io_var_declarations_return io_var_declarations15 = default(MPALParser.io_var_declarations_return);

        MPALParser.other_var_declarations_return other_var_declarations16 = default(MPALParser.other_var_declarations_return);

        MPALParser.function_body_return function_body17 = default(MPALParser.function_body_return);


        object PROGRAM13_tree=null;
        object IDENTIFIER14_tree=null;
        object END_PROGRAM18_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:117:22: ( PROGRAM IDENTIFIER ( io_var_declarations | other_var_declarations )* function_body END_PROGRAM )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:117:24: PROGRAM IDENTIFIER ( io_var_declarations | other_var_declarations )* function_body END_PROGRAM
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PROGRAM13=(IToken)Match(input,PROGRAM,FOLLOW_PROGRAM_in_program_declaration1606); 
            		PROGRAM13_tree = (object)adaptor.Create(PROGRAM13);
            		root_0 = (object)adaptor.BecomeRoot(PROGRAM13_tree, root_0);

            	IDENTIFIER14=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_program_declaration1609); 
            		IDENTIFIER14_tree = (object)adaptor.Create(IDENTIFIER14);
            		adaptor.AddChild(root_0, IDENTIFIER14_tree);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:117:45: ( io_var_declarations | other_var_declarations )*
            	do 
            	{
            	    int alt3 = 3;
            	    alt3 = dfa3.Predict(input);
            	    switch (alt3) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:117:46: io_var_declarations
            			    {
            			    	PushFollow(FOLLOW_io_var_declarations_in_program_declaration1613);
            			    	io_var_declarations15 = io_var_declarations();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, io_var_declarations15.Tree);

            			    }
            			    break;
            			case 2 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:117:68: other_var_declarations
            			    {
            			    	PushFollow(FOLLOW_other_var_declarations_in_program_declaration1617);
            			    	other_var_declarations16 = other_var_declarations();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, other_var_declarations16.Tree);

            			    }
            			    break;

            			default:
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements

            	PushFollow(FOLLOW_function_body_in_program_declaration1621);
            	function_body17 = function_body();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, function_body17.Tree);
            	END_PROGRAM18=(IToken)Match(input,END_PROGRAM,FOLLOW_END_PROGRAM_in_program_declaration1623); 
            		END_PROGRAM18_tree = (object)adaptor.Create(END_PROGRAM18);
            		adaptor.AddChild(root_0, END_PROGRAM18_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "program_declaration"

    public class function_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "function_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:119:1: function_declaration : FUNCTION function_type ( io_var_declarations | var_declarations )* function_body END_FUNCTION ;
    public MPALParser.function_declaration_return function_declaration() // throws RecognitionException [1]
    {   
        MPALParser.function_declaration_return retval = new MPALParser.function_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken FUNCTION19 = null;
        IToken END_FUNCTION24 = null;
        MPALParser.function_type_return function_type20 = default(MPALParser.function_type_return);

        MPALParser.io_var_declarations_return io_var_declarations21 = default(MPALParser.io_var_declarations_return);

        MPALParser.var_declarations_return var_declarations22 = default(MPALParser.var_declarations_return);

        MPALParser.function_body_return function_body23 = default(MPALParser.function_body_return);


        object FUNCTION19_tree=null;
        object END_FUNCTION24_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:119:22: ( FUNCTION function_type ( io_var_declarations | var_declarations )* function_body END_FUNCTION )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:119:24: FUNCTION function_type ( io_var_declarations | var_declarations )* function_body END_FUNCTION
            {
            	root_0 = (object)adaptor.GetNilNode();

            	FUNCTION19=(IToken)Match(input,FUNCTION,FOLLOW_FUNCTION_in_function_declaration1632); 
            		FUNCTION19_tree = (object)adaptor.Create(FUNCTION19);
            		root_0 = (object)adaptor.BecomeRoot(FUNCTION19_tree, root_0);

            	PushFollow(FOLLOW_function_type_in_function_declaration1635);
            	function_type20 = function_type();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, function_type20.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:119:49: ( io_var_declarations | var_declarations )*
            	do 
            	{
            	    int alt4 = 3;
            	    alt4 = dfa4.Predict(input);
            	    switch (alt4) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:119:50: io_var_declarations
            			    {
            			    	PushFollow(FOLLOW_io_var_declarations_in_function_declaration1639);
            			    	io_var_declarations21 = io_var_declarations();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, io_var_declarations21.Tree);

            			    }
            			    break;
            			case 2 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:119:72: var_declarations
            			    {
            			    	PushFollow(FOLLOW_var_declarations_in_function_declaration1643);
            			    	var_declarations22 = var_declarations();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, var_declarations22.Tree);

            			    }
            			    break;

            			default:
            			    goto loop4;
            	    }
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements

            	PushFollow(FOLLOW_function_body_in_function_declaration1647);
            	function_body23 = function_body();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, function_body23.Tree);
            	END_FUNCTION24=(IToken)Match(input,END_FUNCTION,FOLLOW_END_FUNCTION_in_function_declaration1649); 
            		END_FUNCTION24_tree = (object)adaptor.Create(END_FUNCTION24);
            		adaptor.AddChild(root_0, END_FUNCTION24_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "function_declaration"

    public class function_type_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "function_type"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:121:1: function_type : IDENTIFIER ':' simple_specification ;
    public MPALParser.function_type_return function_type() // throws RecognitionException [1]
    {   
        MPALParser.function_type_return retval = new MPALParser.function_type_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER25 = null;
        IToken char_literal26 = null;
        MPALParser.simple_specification_return simple_specification27 = default(MPALParser.simple_specification_return);


        object IDENTIFIER25_tree=null;
        object char_literal26_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:121:15: ( IDENTIFIER ':' simple_specification )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:121:17: IDENTIFIER ':' simple_specification
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER25=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_function_type1657); 
            		IDENTIFIER25_tree = (object)adaptor.Create(IDENTIFIER25);
            		adaptor.AddChild(root_0, IDENTIFIER25_tree);

            	char_literal26=(IToken)Match(input,COLON,FOLLOW_COLON_in_function_type1659); 
            		char_literal26_tree = (object)adaptor.Create(char_literal26);
            		root_0 = (object)adaptor.BecomeRoot(char_literal26_tree, root_0);

            	PushFollow(FOLLOW_simple_specification_in_function_type1662);
            	simple_specification27 = simple_specification();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, simple_specification27.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "function_type"

    public class function_block_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "function_block_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:123:1: function_block_declaration : FUNCTION_BLOCK IDENTIFIER ( io_var_declarations | other_var_declarations )* function_body END_FUNCTION_BLOCK ;
    public MPALParser.function_block_declaration_return function_block_declaration() // throws RecognitionException [1]
    {   
        MPALParser.function_block_declaration_return retval = new MPALParser.function_block_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken FUNCTION_BLOCK28 = null;
        IToken IDENTIFIER29 = null;
        IToken END_FUNCTION_BLOCK33 = null;
        MPALParser.io_var_declarations_return io_var_declarations30 = default(MPALParser.io_var_declarations_return);

        MPALParser.other_var_declarations_return other_var_declarations31 = default(MPALParser.other_var_declarations_return);

        MPALParser.function_body_return function_body32 = default(MPALParser.function_body_return);


        object FUNCTION_BLOCK28_tree=null;
        object IDENTIFIER29_tree=null;
        object END_FUNCTION_BLOCK33_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:123:28: ( FUNCTION_BLOCK IDENTIFIER ( io_var_declarations | other_var_declarations )* function_body END_FUNCTION_BLOCK )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:123:31: FUNCTION_BLOCK IDENTIFIER ( io_var_declarations | other_var_declarations )* function_body END_FUNCTION_BLOCK
            {
            	root_0 = (object)adaptor.GetNilNode();

            	FUNCTION_BLOCK28=(IToken)Match(input,FUNCTION_BLOCK,FOLLOW_FUNCTION_BLOCK_in_function_block_declaration1674); 
            		FUNCTION_BLOCK28_tree = (object)adaptor.Create(FUNCTION_BLOCK28);
            		root_0 = (object)adaptor.BecomeRoot(FUNCTION_BLOCK28_tree, root_0);

            	IDENTIFIER29=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_function_block_declaration1677); 
            		IDENTIFIER29_tree = (object)adaptor.Create(IDENTIFIER29);
            		adaptor.AddChild(root_0, IDENTIFIER29_tree);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:123:58: ( io_var_declarations | other_var_declarations )*
            	do 
            	{
            	    int alt5 = 3;
            	    alt5 = dfa5.Predict(input);
            	    switch (alt5) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:123:60: io_var_declarations
            			    {
            			    	PushFollow(FOLLOW_io_var_declarations_in_function_block_declaration1681);
            			    	io_var_declarations30 = io_var_declarations();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, io_var_declarations30.Tree);

            			    }
            			    break;
            			case 2 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:123:82: other_var_declarations
            			    {
            			    	PushFollow(FOLLOW_other_var_declarations_in_function_block_declaration1685);
            			    	other_var_declarations31 = other_var_declarations();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, other_var_declarations31.Tree);

            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements

            	PushFollow(FOLLOW_function_body_in_function_block_declaration1690);
            	function_body32 = function_body();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, function_body32.Tree);
            	END_FUNCTION_BLOCK33=(IToken)Match(input,END_FUNCTION_BLOCK,FOLLOW_END_FUNCTION_BLOCK_in_function_block_declaration1693); 
            		END_FUNCTION_BLOCK33_tree = (object)adaptor.Create(END_FUNCTION_BLOCK33);
            		adaptor.AddChild(root_0, END_FUNCTION_BLOCK33_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "function_block_declaration"

    public class string_type_size_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "string_type_size"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:125:1: string_type_size : LBRACKED INTEGER ']' ;
    public MPALParser.string_type_size_return string_type_size() // throws RecognitionException [1]
    {   
        MPALParser.string_type_size_return retval = new MPALParser.string_type_size_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LBRACKED34 = null;
        IToken INTEGER35 = null;
        IToken char_literal36 = null;

        object LBRACKED34_tree=null;
        object INTEGER35_tree=null;
        object char_literal36_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:125:18: ( LBRACKED INTEGER ']' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:125:20: LBRACKED INTEGER ']'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LBRACKED34=(IToken)Match(input,LBRACKED,FOLLOW_LBRACKED_in_string_type_size1701); 
            		LBRACKED34_tree = (object)adaptor.Create(LBRACKED34);
            		root_0 = (object)adaptor.BecomeRoot(LBRACKED34_tree, root_0);

            	INTEGER35=(IToken)Match(input,INTEGER,FOLLOW_INTEGER_in_string_type_size1704); 
            		INTEGER35_tree = (object)adaptor.Create(INTEGER35);
            		adaptor.AddChild(root_0, INTEGER35_tree);

            	char_literal36=(IToken)Match(input,106,FOLLOW_106_in_string_type_size1706); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "string_type_size"

    public class character_string_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "character_string"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:127:1: character_string : ( STRING_LITERAL_UNI | STRING_LITERAL );
    public MPALParser.character_string_return character_string() // throws RecognitionException [1]
    {   
        MPALParser.character_string_return retval = new MPALParser.character_string_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set37 = null;

        object set37_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:127:18: ( STRING_LITERAL_UNI | STRING_LITERAL )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set37 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= STRING_LITERAL_UNI && input.LA(1) <= STRING_LITERAL) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set37));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "character_string"

    public class string_type_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "string_type_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:129:1: string_type_declaration : ( STRING | WSTRING ) string_type_size ( string_type_decl_init )? ;
    public MPALParser.string_type_declaration_return string_type_declaration() // throws RecognitionException [1]
    {   
        MPALParser.string_type_declaration_return retval = new MPALParser.string_type_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set38 = null;
        MPALParser.string_type_size_return string_type_size39 = default(MPALParser.string_type_size_return);

        MPALParser.string_type_decl_init_return string_type_decl_init40 = default(MPALParser.string_type_decl_init_return);


        object set38_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:129:25: ( ( STRING | WSTRING ) string_type_size ( string_type_decl_init )? )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:129:28: ( STRING | WSTRING ) string_type_size ( string_type_decl_init )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set38=(IToken)input.LT(1);
            	set38 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= STRING && input.LA(1) <= WSTRING) ) 
            	{
            	    input.Consume();
            	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set38), root_0);
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}

            	PushFollow(FOLLOW_string_type_size_in_string_type_declaration1738);
            	string_type_size39 = string_type_size();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, string_type_size39.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:129:65: ( string_type_decl_init )?
            	int alt6 = 2;
            	int LA6_0 = input.LA(1);

            	if ( (LA6_0 == ASSIGN) )
            	{
            	    int LA6_1 = input.LA(2);

            	    if ( ((LA6_1 >= STRING_LITERAL_UNI && LA6_1 <= STRING_LITERAL)) )
            	    {
            	        alt6 = 1;
            	    }
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:129:65: string_type_decl_init
            	        {
            	        	PushFollow(FOLLOW_string_type_decl_init_in_string_type_declaration1740);
            	        	string_type_decl_init40 = string_type_decl_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, string_type_decl_init40.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "string_type_declaration"

    public class string_type_decl_init_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "string_type_decl_init"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:131:1: string_type_decl_init : ( ASSIGN character_string ) ;
    public MPALParser.string_type_decl_init_return string_type_decl_init() // throws RecognitionException [1]
    {   
        MPALParser.string_type_decl_init_return retval = new MPALParser.string_type_decl_init_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ASSIGN41 = null;
        MPALParser.character_string_return character_string42 = default(MPALParser.character_string_return);


        object ASSIGN41_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:131:23: ( ( ASSIGN character_string ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:131:25: ( ASSIGN character_string )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:131:25: ( ASSIGN character_string )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:131:26: ASSIGN character_string
            	{
            		ASSIGN41=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_string_type_decl_init1751); 
            			ASSIGN41_tree = (object)adaptor.Create(ASSIGN41);
            			root_0 = (object)adaptor.BecomeRoot(ASSIGN41_tree, root_0);

            		PushFollow(FOLLOW_character_string_in_string_type_decl_init1754);
            		character_string42 = character_string();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, character_string42.Tree);

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "string_type_decl_init"

    public class real_literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "real_literal"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:133:1: real_literal : ( real_type_name SHARP )? real_literal_body ;
    public MPALParser.real_literal_return real_literal() // throws RecognitionException [1]
    {   
        MPALParser.real_literal_return retval = new MPALParser.real_literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken SHARP44 = null;
        MPALParser.real_type_name_return real_type_name43 = default(MPALParser.real_type_name_return);

        MPALParser.real_literal_body_return real_literal_body45 = default(MPALParser.real_literal_body_return);


        object SHARP44_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:133:14: ( ( real_type_name SHARP )? real_literal_body )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:133:16: ( real_type_name SHARP )? real_literal_body
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:133:16: ( real_type_name SHARP )?
            	int alt7 = 2;
            	int LA7_0 = input.LA(1);

            	if ( ((LA7_0 >= REAL && LA7_0 <= LREAL)) )
            	{
            	    alt7 = 1;
            	}
            	switch (alt7) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:133:17: real_type_name SHARP
            	        {
            	        	PushFollow(FOLLOW_real_type_name_in_real_literal1764);
            	        	real_type_name43 = real_type_name();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, real_type_name43.Tree);
            	        	SHARP44=(IToken)Match(input,SHARP,FOLLOW_SHARP_in_real_literal1766); 
            	        		SHARP44_tree = (object)adaptor.Create(SHARP44);
            	        		root_0 = (object)adaptor.BecomeRoot(SHARP44_tree, root_0);


            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_real_literal_body_in_real_literal1771);
            	real_literal_body45 = real_literal_body();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, real_literal_body45.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "real_literal"

    public class real_literal_body_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "real_literal_body"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:135:1: real_literal_body : ( PLUS | NEG )? REAL_CONSTANT ;
    public MPALParser.real_literal_body_return real_literal_body() // throws RecognitionException [1]
    {   
        MPALParser.real_literal_body_return retval = new MPALParser.real_literal_body_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken PLUS46 = null;
        IToken NEG47 = null;
        IToken REAL_CONSTANT48 = null;

        object PLUS46_tree=null;
        object NEG47_tree=null;
        object REAL_CONSTANT48_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:135:20: ( ( PLUS | NEG )? REAL_CONSTANT )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:135:22: ( PLUS | NEG )? REAL_CONSTANT
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:135:22: ( PLUS | NEG )?
            	int alt8 = 3;
            	int LA8_0 = input.LA(1);

            	if ( (LA8_0 == PLUS) )
            	{
            	    alt8 = 1;
            	}
            	else if ( (LA8_0 == NEG) )
            	{
            	    alt8 = 2;
            	}
            	switch (alt8) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:135:23: PLUS
            	        {
            	        	PLUS46=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_real_literal_body1781); 
            	        		PLUS46_tree = (object)adaptor.Create(PLUS46);
            	        		root_0 = (object)adaptor.BecomeRoot(PLUS46_tree, root_0);


            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:135:29: NEG
            	        {
            	        	NEG47=(IToken)Match(input,NEG,FOLLOW_NEG_in_real_literal_body1784); 
            	        		NEG47_tree = (object)adaptor.Create(NEG47);
            	        		root_0 = (object)adaptor.BecomeRoot(NEG47_tree, root_0);


            	        }
            	        break;

            	}

            	REAL_CONSTANT48=(IToken)Match(input,REAL_CONSTANT,FOLLOW_REAL_CONSTANT_in_real_literal_body1790); 
            		REAL_CONSTANT48_tree = (object)adaptor.Create(REAL_CONSTANT48);
            		adaptor.AddChild(root_0, REAL_CONSTANT48_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "real_literal_body"

    public class integer_literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "integer_literal"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:1: integer_literal : ( ( ( PLUS | NEG )? INTEGER ) | BINARY_INTEGER | OCTAL_INTEGER | HEX_INTEGER );
    public MPALParser.integer_literal_return integer_literal() // throws RecognitionException [1]
    {   
        MPALParser.integer_literal_return retval = new MPALParser.integer_literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken PLUS49 = null;
        IToken NEG50 = null;
        IToken INTEGER51 = null;
        IToken BINARY_INTEGER52 = null;
        IToken OCTAL_INTEGER53 = null;
        IToken HEX_INTEGER54 = null;

        object PLUS49_tree=null;
        object NEG50_tree=null;
        object INTEGER51_tree=null;
        object BINARY_INTEGER52_tree=null;
        object OCTAL_INTEGER53_tree=null;
        object HEX_INTEGER54_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:18: ( ( ( PLUS | NEG )? INTEGER ) | BINARY_INTEGER | OCTAL_INTEGER | HEX_INTEGER )
            int alt10 = 4;
            switch ( input.LA(1) ) 
            {
            case PLUS:
            case NEG:
            case INTEGER:
            	{
                alt10 = 1;
                }
                break;
            case BINARY_INTEGER:
            	{
                alt10 = 2;
                }
                break;
            case OCTAL_INTEGER:
            	{
                alt10 = 3;
                }
                break;
            case HEX_INTEGER:
            	{
                alt10 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d10s0 =
            	        new NoViableAltException("", 10, 0, input);

            	    throw nvae_d10s0;
            }

            switch (alt10) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:21: ( ( PLUS | NEG )? INTEGER )
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:21: ( ( PLUS | NEG )? INTEGER )
                    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:22: ( PLUS | NEG )? INTEGER
                    	{
                    		// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:22: ( PLUS | NEG )?
                    		int alt9 = 3;
                    		int LA9_0 = input.LA(1);

                    		if ( (LA9_0 == PLUS) )
                    		{
                    		    alt9 = 1;
                    		}
                    		else if ( (LA9_0 == NEG) )
                    		{
                    		    alt9 = 2;
                    		}
                    		switch (alt9) 
                    		{
                    		    case 1 :
                    		        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:23: PLUS
                    		        {
                    		        	PLUS49=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_integer_literal1802); 
                    		        		PLUS49_tree = (object)adaptor.Create(PLUS49);
                    		        		root_0 = (object)adaptor.BecomeRoot(PLUS49_tree, root_0);


                    		        }
                    		        break;
                    		    case 2 :
                    		        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:29: NEG
                    		        {
                    		        	NEG50=(IToken)Match(input,NEG,FOLLOW_NEG_in_integer_literal1805); 
                    		        		NEG50_tree = (object)adaptor.Create(NEG50);
                    		        		root_0 = (object)adaptor.BecomeRoot(NEG50_tree, root_0);


                    		        }
                    		        break;

                    		}

                    		INTEGER51=(IToken)Match(input,INTEGER,FOLLOW_INTEGER_in_integer_literal1810); 
                    			INTEGER51_tree = (object)adaptor.Create(INTEGER51);
                    			adaptor.AddChild(root_0, INTEGER51_tree);


                    	}


                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:47: BINARY_INTEGER
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	BINARY_INTEGER52=(IToken)Match(input,BINARY_INTEGER,FOLLOW_BINARY_INTEGER_in_integer_literal1815); 
                    		BINARY_INTEGER52_tree = (object)adaptor.Create(BINARY_INTEGER52);
                    		adaptor.AddChild(root_0, BINARY_INTEGER52_tree);


                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:64: OCTAL_INTEGER
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	OCTAL_INTEGER53=(IToken)Match(input,OCTAL_INTEGER,FOLLOW_OCTAL_INTEGER_in_integer_literal1819); 
                    		OCTAL_INTEGER53_tree = (object)adaptor.Create(OCTAL_INTEGER53);
                    		adaptor.AddChild(root_0, OCTAL_INTEGER53_tree);


                    }
                    break;
                case 4 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:137:80: HEX_INTEGER
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	HEX_INTEGER54=(IToken)Match(input,HEX_INTEGER,FOLLOW_HEX_INTEGER_in_integer_literal1823); 
                    		HEX_INTEGER54_tree = (object)adaptor.Create(HEX_INTEGER54);
                    		adaptor.AddChild(root_0, HEX_INTEGER54_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "integer_literal"

    public class bit_string_literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "bit_string_literal"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:139:1: bit_string_literal : ( ( BYTE | WORD | DWORD | LWORD ) SHARP )? integer_literal ;
    public MPALParser.bit_string_literal_return bit_string_literal() // throws RecognitionException [1]
    {   
        MPALParser.bit_string_literal_return retval = new MPALParser.bit_string_literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set55 = null;
        IToken SHARP56 = null;
        MPALParser.integer_literal_return integer_literal57 = default(MPALParser.integer_literal_return);


        object set55_tree=null;
        object SHARP56_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:139:20: ( ( ( BYTE | WORD | DWORD | LWORD ) SHARP )? integer_literal )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:139:22: ( ( BYTE | WORD | DWORD | LWORD ) SHARP )? integer_literal
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:139:22: ( ( BYTE | WORD | DWORD | LWORD ) SHARP )?
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);

            	if ( ((LA11_0 >= BYTE && LA11_0 <= LWORD)) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:139:23: ( BYTE | WORD | DWORD | LWORD ) SHARP
            	        {
            	        	set55 = (IToken)input.LT(1);
            	        	if ( (input.LA(1) >= BYTE && input.LA(1) <= LWORD) ) 
            	        	{
            	        	    input.Consume();
            	        	    adaptor.AddChild(root_0, (object)adaptor.Create(set55));
            	        	    state.errorRecovery = false;
            	        	}
            	        	else 
            	        	{
            	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	        	    throw mse;
            	        	}

            	        	SHARP56=(IToken)Match(input,SHARP,FOLLOW_SHARP_in_bit_string_literal1848); 
            	        		SHARP56_tree = (object)adaptor.Create(SHARP56);
            	        		root_0 = (object)adaptor.BecomeRoot(SHARP56_tree, root_0);


            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_integer_literal_in_bit_string_literal1854);
            	integer_literal57 = integer_literal();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, integer_literal57.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "bit_string_literal"

    public class spec_integer_literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "spec_integer_literal"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:141:1: spec_integer_literal : ( integer_type_name SHARP ) ( integer_literal ) ;
    public MPALParser.spec_integer_literal_return spec_integer_literal() // throws RecognitionException [1]
    {   
        MPALParser.spec_integer_literal_return retval = new MPALParser.spec_integer_literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken SHARP59 = null;
        MPALParser.integer_type_name_return integer_type_name58 = default(MPALParser.integer_type_name_return);

        MPALParser.integer_literal_return integer_literal60 = default(MPALParser.integer_literal_return);


        object SHARP59_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:141:22: ( ( integer_type_name SHARP ) ( integer_literal ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:141:24: ( integer_type_name SHARP ) ( integer_literal )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:141:24: ( integer_type_name SHARP )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:141:26: integer_type_name SHARP
            	{
            		PushFollow(FOLLOW_integer_type_name_in_spec_integer_literal1864);
            		integer_type_name58 = integer_type_name();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, integer_type_name58.Tree);
            		SHARP59=(IToken)Match(input,SHARP,FOLLOW_SHARP_in_spec_integer_literal1866); 
            			SHARP59_tree = (object)adaptor.Create(SHARP59);
            			root_0 = (object)adaptor.BecomeRoot(SHARP59_tree, root_0);


            	}

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:141:52: ( integer_literal )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:141:53: integer_literal
            	{
            		PushFollow(FOLLOW_integer_literal_in_spec_integer_literal1871);
            		integer_literal60 = integer_literal();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, integer_literal60.Tree);

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "spec_integer_literal"

    public class constant_literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "constant_literal"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:143:1: constant_literal : ( real_literal | spec_integer_literal | character_string | bool_literal | bit_string_literal );
    public MPALParser.constant_literal_return constant_literal() // throws RecognitionException [1]
    {   
        MPALParser.constant_literal_return retval = new MPALParser.constant_literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.real_literal_return real_literal61 = default(MPALParser.real_literal_return);

        MPALParser.spec_integer_literal_return spec_integer_literal62 = default(MPALParser.spec_integer_literal_return);

        MPALParser.character_string_return character_string63 = default(MPALParser.character_string_return);

        MPALParser.bool_literal_return bool_literal64 = default(MPALParser.bool_literal_return);

        MPALParser.bit_string_literal_return bit_string_literal65 = default(MPALParser.bit_string_literal_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:143:18: ( real_literal | spec_integer_literal | character_string | bool_literal | bit_string_literal )
            int alt12 = 5;
            alt12 = dfa12.Predict(input);
            switch (alt12) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:143:20: real_literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_real_literal_in_constant_literal1880);
                    	real_literal61 = real_literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, real_literal61.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:143:34: spec_integer_literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_spec_integer_literal_in_constant_literal1883);
                    	spec_integer_literal62 = spec_integer_literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, spec_integer_literal62.Tree);

                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:143:57: character_string
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_character_string_in_constant_literal1887);
                    	character_string63 = character_string();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, character_string63.Tree);

                    }
                    break;
                case 4 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:143:76: bool_literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_bool_literal_in_constant_literal1891);
                    	bool_literal64 = bool_literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, bool_literal64.Tree);

                    }
                    break;
                case 5 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:143:91: bit_string_literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_bit_string_literal_in_constant_literal1895);
                    	bit_string_literal65 = bit_string_literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, bit_string_literal65.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "constant_literal"

    public class simple_spec_init_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "simple_spec_init"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:145:1: simple_spec_init : simple_specification ( ASSIGN ( constant_literal | IDENTIFIER ) )? ;
    public MPALParser.simple_spec_init_return simple_spec_init() // throws RecognitionException [1]
    {   
        MPALParser.simple_spec_init_return retval = new MPALParser.simple_spec_init_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ASSIGN67 = null;
        IToken IDENTIFIER69 = null;
        MPALParser.simple_specification_return simple_specification66 = default(MPALParser.simple_specification_return);

        MPALParser.constant_literal_return constant_literal68 = default(MPALParser.constant_literal_return);


        object ASSIGN67_tree=null;
        object IDENTIFIER69_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:145:18: ( simple_specification ( ASSIGN ( constant_literal | IDENTIFIER ) )? )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:145:20: simple_specification ( ASSIGN ( constant_literal | IDENTIFIER ) )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_simple_specification_in_simple_spec_init1905);
            	simple_specification66 = simple_specification();
            	state.followingStackPointer--;

            	root_0 = (object)adaptor.BecomeRoot(simple_specification66.Tree, root_0);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:145:42: ( ASSIGN ( constant_literal | IDENTIFIER ) )?
            	int alt14 = 2;
            	int LA14_0 = input.LA(1);

            	if ( (LA14_0 == ASSIGN) )
            	{
            	    alt14 = 1;
            	}
            	switch (alt14) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:145:43: ASSIGN ( constant_literal | IDENTIFIER )
            	        {
            	        	ASSIGN67=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_simple_spec_init1909); 
            	        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:145:51: ( constant_literal | IDENTIFIER )
            	        	int alt13 = 2;
            	        	alt13 = dfa13.Predict(input);
            	        	switch (alt13) 
            	        	{
            	        	    case 1 :
            	        	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:145:52: constant_literal
            	        	        {
            	        	        	PushFollow(FOLLOW_constant_literal_in_simple_spec_init1913);
            	        	        	constant_literal68 = constant_literal();
            	        	        	state.followingStackPointer--;

            	        	        	adaptor.AddChild(root_0, constant_literal68.Tree);

            	        	        }
            	        	        break;
            	        	    case 2 :
            	        	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:145:70: IDENTIFIER
            	        	        {
            	        	        	IDENTIFIER69=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_simple_spec_init1916); 
            	        	        		IDENTIFIER69_tree = (object)adaptor.Create(IDENTIFIER69);
            	        	        		adaptor.AddChild(root_0, IDENTIFIER69_tree);


            	        	        }
            	        	        break;

            	        	}


            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "simple_spec_init"

    public class enumerated_value_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "enumerated_value"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:147:1: enumerated_value : IDENTIFIER SHARP IDENTIFIER ;
    public MPALParser.enumerated_value_return enumerated_value() // throws RecognitionException [1]
    {   
        MPALParser.enumerated_value_return retval = new MPALParser.enumerated_value_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER70 = null;
        IToken SHARP71 = null;
        IToken IDENTIFIER72 = null;

        object IDENTIFIER70_tree=null;
        object SHARP71_tree=null;
        object IDENTIFIER72_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:147:18: ( IDENTIFIER SHARP IDENTIFIER )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:147:20: IDENTIFIER SHARP IDENTIFIER
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER70=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_enumerated_value1929); 
            		IDENTIFIER70_tree = (object)adaptor.Create(IDENTIFIER70);
            		adaptor.AddChild(root_0, IDENTIFIER70_tree);

            	SHARP71=(IToken)Match(input,SHARP,FOLLOW_SHARP_in_enumerated_value1931); 
            		SHARP71_tree = (object)adaptor.Create(SHARP71);
            		adaptor.AddChild(root_0, SHARP71_tree);

            	IDENTIFIER72=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_enumerated_value1933); 
            		IDENTIFIER72_tree = (object)adaptor.Create(IDENTIFIER72);
            		adaptor.AddChild(root_0, IDENTIFIER72_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "enumerated_value"

    public class enumerated_specification_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "enumerated_specification"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:1: enumerated_specification : ( '(' ( enumerated_value | IDENTIFIER ) ( COMMA ( enumerated_value | IDENTIFIER ) )* ')' ) ;
    public MPALParser.enumerated_specification_return enumerated_specification() // throws RecognitionException [1]
    {   
        MPALParser.enumerated_specification_return retval = new MPALParser.enumerated_specification_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal73 = null;
        IToken IDENTIFIER75 = null;
        IToken COMMA76 = null;
        IToken IDENTIFIER78 = null;
        IToken char_literal79 = null;
        MPALParser.enumerated_value_return enumerated_value74 = default(MPALParser.enumerated_value_return);

        MPALParser.enumerated_value_return enumerated_value77 = default(MPALParser.enumerated_value_return);


        object char_literal73_tree=null;
        object IDENTIFIER75_tree=null;
        object COMMA76_tree=null;
        object IDENTIFIER78_tree=null;
        object char_literal79_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:26: ( ( '(' ( enumerated_value | IDENTIFIER ) ( COMMA ( enumerated_value | IDENTIFIER ) )* ')' ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:28: ( '(' ( enumerated_value | IDENTIFIER ) ( COMMA ( enumerated_value | IDENTIFIER ) )* ')' )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:28: ( '(' ( enumerated_value | IDENTIFIER ) ( COMMA ( enumerated_value | IDENTIFIER ) )* ')' )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:30: '(' ( enumerated_value | IDENTIFIER ) ( COMMA ( enumerated_value | IDENTIFIER ) )* ')'
            	{
            		char_literal73=(IToken)Match(input,LRBRACKED,FOLLOW_LRBRACKED_in_enumerated_specification1944); 
            			char_literal73_tree = (object)adaptor.Create(char_literal73);
            			root_0 = (object)adaptor.BecomeRoot(char_literal73_tree, root_0);

            		// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:35: ( enumerated_value | IDENTIFIER )
            		int alt15 = 2;
            		int LA15_0 = input.LA(1);

            		if ( (LA15_0 == IDENTIFIER) )
            		{
            		    int LA15_1 = input.LA(2);

            		    if ( (LA15_1 == SHARP) )
            		    {
            		        alt15 = 1;
            		    }
            		    else if ( (LA15_1 == COMMA || LA15_1 == 107) )
            		    {
            		        alt15 = 2;
            		    }
            		    else 
            		    {
            		        NoViableAltException nvae_d15s1 =
            		            new NoViableAltException("", 15, 1, input);

            		        throw nvae_d15s1;
            		    }
            		}
            		else 
            		{
            		    NoViableAltException nvae_d15s0 =
            		        new NoViableAltException("", 15, 0, input);

            		    throw nvae_d15s0;
            		}
            		switch (alt15) 
            		{
            		    case 1 :
            		        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:36: enumerated_value
            		        {
            		        	PushFollow(FOLLOW_enumerated_value_in_enumerated_specification1948);
            		        	enumerated_value74 = enumerated_value();
            		        	state.followingStackPointer--;

            		        	adaptor.AddChild(root_0, enumerated_value74.Tree);

            		        }
            		        break;
            		    case 2 :
            		        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:53: IDENTIFIER
            		        {
            		        	IDENTIFIER75=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_enumerated_specification1950); 
            		        		IDENTIFIER75_tree = (object)adaptor.Create(IDENTIFIER75);
            		        		adaptor.AddChild(root_0, IDENTIFIER75_tree);


            		        }
            		        break;

            		}

            		// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:66: ( COMMA ( enumerated_value | IDENTIFIER ) )*
            		do 
            		{
            		    int alt17 = 2;
            		    int LA17_0 = input.LA(1);

            		    if ( (LA17_0 == COMMA) )
            		    {
            		        alt17 = 1;
            		    }


            		    switch (alt17) 
            			{
            				case 1 :
            				    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:67: COMMA ( enumerated_value | IDENTIFIER )
            				    {
            				    	COMMA76=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_enumerated_specification1955); 
            				    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:74: ( enumerated_value | IDENTIFIER )
            				    	int alt16 = 2;
            				    	int LA16_0 = input.LA(1);

            				    	if ( (LA16_0 == IDENTIFIER) )
            				    	{
            				    	    int LA16_1 = input.LA(2);

            				    	    if ( (LA16_1 == SHARP) )
            				    	    {
            				    	        alt16 = 1;
            				    	    }
            				    	    else if ( (LA16_1 == COMMA || LA16_1 == 107) )
            				    	    {
            				    	        alt16 = 2;
            				    	    }
            				    	    else 
            				    	    {
            				    	        NoViableAltException nvae_d16s1 =
            				    	            new NoViableAltException("", 16, 1, input);

            				    	        throw nvae_d16s1;
            				    	    }
            				    	}
            				    	else 
            				    	{
            				    	    NoViableAltException nvae_d16s0 =
            				    	        new NoViableAltException("", 16, 0, input);

            				    	    throw nvae_d16s0;
            				    	}
            				    	switch (alt16) 
            				    	{
            				    	    case 1 :
            				    	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:75: enumerated_value
            				    	        {
            				    	        	PushFollow(FOLLOW_enumerated_value_in_enumerated_specification1959);
            				    	        	enumerated_value77 = enumerated_value();
            				    	        	state.followingStackPointer--;

            				    	        	adaptor.AddChild(root_0, enumerated_value77.Tree);

            				    	        }
            				    	        break;
            				    	    case 2 :
            				    	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:149:92: IDENTIFIER
            				    	        {
            				    	        	IDENTIFIER78=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_enumerated_specification1961); 
            				    	        		IDENTIFIER78_tree = (object)adaptor.Create(IDENTIFIER78);
            				    	        		adaptor.AddChild(root_0, IDENTIFIER78_tree);


            				    	        }
            				    	        break;

            				    	}


            				    }
            				    break;

            				default:
            				    goto loop17;
            		    }
            		} while (true);

            		loop17:
            			;	// Stops C# compiler whining that label 'loop17' has no statements

            		char_literal79=(IToken)Match(input,107,FOLLOW_107_in_enumerated_specification1966); 

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "enumerated_specification"

    public class enumerated_spec_init_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "enumerated_spec_init"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:151:1: enumerated_spec_init : enumerated_specification ( enumerated_init )? ;
    public MPALParser.enumerated_spec_init_return enumerated_spec_init() // throws RecognitionException [1]
    {   
        MPALParser.enumerated_spec_init_return retval = new MPALParser.enumerated_spec_init_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.enumerated_specification_return enumerated_specification80 = default(MPALParser.enumerated_specification_return);

        MPALParser.enumerated_init_return enumerated_init81 = default(MPALParser.enumerated_init_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:151:22: ( enumerated_specification ( enumerated_init )? )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:151:24: enumerated_specification ( enumerated_init )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_enumerated_specification_in_enumerated_spec_init1977);
            	enumerated_specification80 = enumerated_specification();
            	state.followingStackPointer--;

            	root_0 = (object)adaptor.BecomeRoot(enumerated_specification80.Tree, root_0);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:151:50: ( enumerated_init )?
            	int alt18 = 2;
            	int LA18_0 = input.LA(1);

            	if ( (LA18_0 == ASSIGN) )
            	{
            	    alt18 = 1;
            	}
            	switch (alt18) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:151:50: enumerated_init
            	        {
            	        	PushFollow(FOLLOW_enumerated_init_in_enumerated_spec_init1980);
            	        	enumerated_init81 = enumerated_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, enumerated_init81.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "enumerated_spec_init"

    public class enumerated_init_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "enumerated_init"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:153:1: enumerated_init : ASSIGN ( enumerated_value | IDENTIFIER ) ;
    public MPALParser.enumerated_init_return enumerated_init() // throws RecognitionException [1]
    {   
        MPALParser.enumerated_init_return retval = new MPALParser.enumerated_init_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ASSIGN82 = null;
        IToken IDENTIFIER84 = null;
        MPALParser.enumerated_value_return enumerated_value83 = default(MPALParser.enumerated_value_return);


        object ASSIGN82_tree=null;
        object IDENTIFIER84_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:153:17: ( ASSIGN ( enumerated_value | IDENTIFIER ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:153:19: ASSIGN ( enumerated_value | IDENTIFIER )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	ASSIGN82=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_enumerated_init1990); 
            		ASSIGN82_tree = (object)adaptor.Create(ASSIGN82);
            		root_0 = (object)adaptor.BecomeRoot(ASSIGN82_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:153:27: ( enumerated_value | IDENTIFIER )
            	int alt19 = 2;
            	int LA19_0 = input.LA(1);

            	if ( (LA19_0 == IDENTIFIER) )
            	{
            	    int LA19_1 = input.LA(2);

            	    if ( (LA19_1 == SHARP) )
            	    {
            	        alt19 = 1;
            	    }
            	    else if ( (LA19_1 == 108) )
            	    {
            	        alt19 = 2;
            	    }
            	    else 
            	    {
            	        NoViableAltException nvae_d19s1 =
            	            new NoViableAltException("", 19, 1, input);

            	        throw nvae_d19s1;
            	    }
            	}
            	else 
            	{
            	    NoViableAltException nvae_d19s0 =
            	        new NoViableAltException("", 19, 0, input);

            	    throw nvae_d19s0;
            	}
            	switch (alt19) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:153:28: enumerated_value
            	        {
            	        	PushFollow(FOLLOW_enumerated_value_in_enumerated_init1994);
            	        	enumerated_value83 = enumerated_value();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, enumerated_value83.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:153:45: IDENTIFIER
            	        {
            	        	IDENTIFIER84=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_enumerated_init1996); 
            	        		IDENTIFIER84_tree = (object)adaptor.Create(IDENTIFIER84);
            	        		adaptor.AddChild(root_0, IDENTIFIER84_tree);


            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "enumerated_init"

    public class single_element_type_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "single_element_type_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:155:1: single_element_type_declaration : ( simple_spec_init | subrange_spec_init | enumerated_spec_init ) ;
    public MPALParser.single_element_type_declaration_return single_element_type_declaration() // throws RecognitionException [1]
    {   
        MPALParser.single_element_type_declaration_return retval = new MPALParser.single_element_type_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.simple_spec_init_return simple_spec_init85 = default(MPALParser.simple_spec_init_return);

        MPALParser.subrange_spec_init_return subrange_spec_init86 = default(MPALParser.subrange_spec_init_return);

        MPALParser.enumerated_spec_init_return enumerated_spec_init87 = default(MPALParser.enumerated_spec_init_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:155:33: ( ( simple_spec_init | subrange_spec_init | enumerated_spec_init ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:155:35: ( simple_spec_init | subrange_spec_init | enumerated_spec_init )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:155:35: ( simple_spec_init | subrange_spec_init | enumerated_spec_init )
            	int alt20 = 3;
            	switch ( input.LA(1) ) 
            	{
            	case SINT:
            	case INT:
            	case DINT:
            	case LINT:
            	case USINT:
            	case UINT:
            	case UDINT:
            	case ULINT:
            		{
            	    int LA20_1 = input.LA(2);

            	    if ( (LA20_1 == LRBRACKED) )
            	    {
            	        alt20 = 2;
            	    }
            	    else if ( (LA20_1 == ASSIGN || LA20_1 == 108) )
            	    {
            	        alt20 = 1;
            	    }
            	    else 
            	    {
            	        NoViableAltException nvae_d20s1 =
            	            new NoViableAltException("", 20, 1, input);

            	        throw nvae_d20s1;
            	    }
            	    }
            	    break;
            	case STRING:
            	case WSTRING:
            	case VOID:
            	case COUNTER:
            	case TIMER:
            	case TIME:
            	case DATE_LIT:
            	case TIME_OF_DAY_LIT:
            	case TOD:
            	case DATE_AND_TIME:
            	case DT:
            	case BOOL:
            	case BYTE:
            	case WORD:
            	case DWORD:
            	case LWORD:
            	case REAL:
            	case LREAL:
            	case CHAR:
            	case WCHAR:
            	case IDENTIFIER:
            		{
            	    alt20 = 1;
            	    }
            	    break;
            	case LRBRACKED:
            		{
            	    alt20 = 3;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d20s0 =
            		        new NoViableAltException("", 20, 0, input);

            		    throw nvae_d20s0;
            	}

            	switch (alt20) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:155:37: simple_spec_init
            	        {
            	        	PushFollow(FOLLOW_simple_spec_init_in_single_element_type_declaration2007);
            	        	simple_spec_init85 = simple_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, simple_spec_init85.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:155:56: subrange_spec_init
            	        {
            	        	PushFollow(FOLLOW_subrange_spec_init_in_single_element_type_declaration2011);
            	        	subrange_spec_init86 = subrange_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, subrange_spec_init86.Tree);

            	        }
            	        break;
            	    case 3 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:155:78: enumerated_spec_init
            	        {
            	        	PushFollow(FOLLOW_enumerated_spec_init_in_single_element_type_declaration2016);
            	        	enumerated_spec_init87 = enumerated_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, enumerated_spec_init87.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "single_element_type_declaration"

    public class subrange_spec_init_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "subrange_spec_init"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:157:1: subrange_spec_init : subrange_specification ( ':=' INTEGER )? ;
    public MPALParser.subrange_spec_init_return subrange_spec_init() // throws RecognitionException [1]
    {   
        MPALParser.subrange_spec_init_return retval = new MPALParser.subrange_spec_init_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal89 = null;
        IToken INTEGER90 = null;
        MPALParser.subrange_specification_return subrange_specification88 = default(MPALParser.subrange_specification_return);


        object string_literal89_tree=null;
        object INTEGER90_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:157:20: ( subrange_specification ( ':=' INTEGER )? )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:157:23: subrange_specification ( ':=' INTEGER )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_subrange_specification_in_subrange_spec_init2027);
            	subrange_specification88 = subrange_specification();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, subrange_specification88.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:157:46: ( ':=' INTEGER )?
            	int alt21 = 2;
            	int LA21_0 = input.LA(1);

            	if ( (LA21_0 == ASSIGN) )
            	{
            	    alt21 = 1;
            	}
            	switch (alt21) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:157:47: ':=' INTEGER
            	        {
            	        	string_literal89=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_subrange_spec_init2030); 
            	        		string_literal89_tree = (object)adaptor.Create(string_literal89);
            	        		adaptor.AddChild(root_0, string_literal89_tree);

            	        	INTEGER90=(IToken)Match(input,INTEGER,FOLLOW_INTEGER_in_subrange_spec_init2032); 
            	        		INTEGER90_tree = (object)adaptor.Create(INTEGER90);
            	        		adaptor.AddChild(root_0, INTEGER90_tree);


            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "subrange_spec_init"

    public class subrange_specification_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "subrange_specification"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:159:1: subrange_specification : integer_type_name '(' subrange ')' ;
    public MPALParser.subrange_specification_return subrange_specification() // throws RecognitionException [1]
    {   
        MPALParser.subrange_specification_return retval = new MPALParser.subrange_specification_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal92 = null;
        IToken char_literal94 = null;
        MPALParser.integer_type_name_return integer_type_name91 = default(MPALParser.integer_type_name_return);

        MPALParser.subrange_return subrange93 = default(MPALParser.subrange_return);


        object char_literal92_tree=null;
        object char_literal94_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:159:24: ( integer_type_name '(' subrange ')' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:159:26: integer_type_name '(' subrange ')'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_integer_type_name_in_subrange_specification2042);
            	integer_type_name91 = integer_type_name();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, integer_type_name91.Tree);
            	char_literal92=(IToken)Match(input,LRBRACKED,FOLLOW_LRBRACKED_in_subrange_specification2044); 
            		char_literal92_tree = (object)adaptor.Create(char_literal92);
            		adaptor.AddChild(root_0, char_literal92_tree);

            	PushFollow(FOLLOW_subrange_in_subrange_specification2046);
            	subrange93 = subrange();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, subrange93.Tree);
            	char_literal94=(IToken)Match(input,107,FOLLOW_107_in_subrange_specification2048); 
            		char_literal94_tree = (object)adaptor.Create(char_literal94);
            		adaptor.AddChild(root_0, char_literal94_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "subrange_specification"

    public class array_spec_init_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "array_spec_init"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:161:1: array_spec_init : ( array_specification ) ( ':=' array_initialization )? ;
    public MPALParser.array_spec_init_return array_spec_init() // throws RecognitionException [1]
    {   
        MPALParser.array_spec_init_return retval = new MPALParser.array_spec_init_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal96 = null;
        MPALParser.array_specification_return array_specification95 = default(MPALParser.array_specification_return);

        MPALParser.array_initialization_return array_initialization97 = default(MPALParser.array_initialization_return);


        object string_literal96_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:161:17: ( ( array_specification ) ( ':=' array_initialization )? )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:161:19: ( array_specification ) ( ':=' array_initialization )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:161:19: ( array_specification )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:161:20: array_specification
            	{
            		PushFollow(FOLLOW_array_specification_in_array_spec_init2057);
            		array_specification95 = array_specification();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, array_specification95.Tree);

            	}

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:161:42: ( ':=' array_initialization )?
            	int alt22 = 2;
            	int LA22_0 = input.LA(1);

            	if ( (LA22_0 == ASSIGN) )
            	{
            	    alt22 = 1;
            	}
            	switch (alt22) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:161:43: ':=' array_initialization
            	        {
            	        	string_literal96=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_array_spec_init2062); 
            	        	PushFollow(FOLLOW_array_initialization_in_array_spec_init2065);
            	        	array_initialization97 = array_initialization();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, array_initialization97.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "array_spec_init"

    public class array_range_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "array_range"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:163:1: array_range : LBRACKED subrange ( COMMA subrange )* ']' ;
    public MPALParser.array_range_return array_range() // throws RecognitionException [1]
    {   
        MPALParser.array_range_return retval = new MPALParser.array_range_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LBRACKED98 = null;
        IToken COMMA100 = null;
        IToken char_literal102 = null;
        MPALParser.subrange_return subrange99 = default(MPALParser.subrange_return);

        MPALParser.subrange_return subrange101 = default(MPALParser.subrange_return);


        object LBRACKED98_tree=null;
        object COMMA100_tree=null;
        object char_literal102_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:163:14: ( LBRACKED subrange ( COMMA subrange )* ']' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:163:16: LBRACKED subrange ( COMMA subrange )* ']'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LBRACKED98=(IToken)Match(input,LBRACKED,FOLLOW_LBRACKED_in_array_range2077); 
            		LBRACKED98_tree = (object)adaptor.Create(LBRACKED98);
            		root_0 = (object)adaptor.BecomeRoot(LBRACKED98_tree, root_0);

            	PushFollow(FOLLOW_subrange_in_array_range2080);
            	subrange99 = subrange();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, subrange99.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:163:35: ( COMMA subrange )*
            	do 
            	{
            	    int alt23 = 2;
            	    int LA23_0 = input.LA(1);

            	    if ( (LA23_0 == COMMA) )
            	    {
            	        alt23 = 1;
            	    }


            	    switch (alt23) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:163:36: COMMA subrange
            			    {
            			    	COMMA100=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_array_range2083); 
            			    	PushFollow(FOLLOW_subrange_in_array_range2086);
            			    	subrange101 = subrange();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, subrange101.Tree);

            			    }
            			    break;

            			default:
            			    goto loop23;
            	    }
            	} while (true);

            	loop23:
            		;	// Stops C# compiler whining that label 'loop23' has no statements

            	char_literal102=(IToken)Match(input,106,FOLLOW_106_in_array_range2090); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "array_range"

    public class udt_array_spec_init_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "udt_array_spec_init"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:165:1: udt_array_spec_init : IDENTIFIER ASSIGN array_initialization ;
    public MPALParser.udt_array_spec_init_return udt_array_spec_init() // throws RecognitionException [1]
    {   
        MPALParser.udt_array_spec_init_return retval = new MPALParser.udt_array_spec_init_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER103 = null;
        IToken ASSIGN104 = null;
        MPALParser.array_initialization_return array_initialization105 = default(MPALParser.array_initialization_return);


        object IDENTIFIER103_tree=null;
        object ASSIGN104_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:165:21: ( IDENTIFIER ASSIGN array_initialization )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:165:23: IDENTIFIER ASSIGN array_initialization
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER103=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_udt_array_spec_init2100); 
            		IDENTIFIER103_tree = (object)adaptor.Create(IDENTIFIER103);
            		adaptor.AddChild(root_0, IDENTIFIER103_tree);

            	ASSIGN104=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_udt_array_spec_init2102); 
            	PushFollow(FOLLOW_array_initialization_in_udt_array_spec_init2105);
            	array_initialization105 = array_initialization();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, array_initialization105.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "udt_array_spec_init"

    public class non_generic_type_name_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "non_generic_type_name"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:167:1: non_generic_type_name : ( elemetary_type_name | IDENTIFIER | structure_declaration | string_type_declaration );
    public MPALParser.non_generic_type_name_return non_generic_type_name() // throws RecognitionException [1]
    {   
        MPALParser.non_generic_type_name_return retval = new MPALParser.non_generic_type_name_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER107 = null;
        MPALParser.elemetary_type_name_return elemetary_type_name106 = default(MPALParser.elemetary_type_name_return);

        MPALParser.structure_declaration_return structure_declaration108 = default(MPALParser.structure_declaration_return);

        MPALParser.string_type_declaration_return string_type_declaration109 = default(MPALParser.string_type_declaration_return);


        object IDENTIFIER107_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:167:23: ( elemetary_type_name | IDENTIFIER | structure_declaration | string_type_declaration )
            int alt24 = 4;
            switch ( input.LA(1) ) 
            {
            case STRING:
            case WSTRING:
            	{
                int LA24_1 = input.LA(2);

                if ( (LA24_1 == ASSIGN || LA24_1 == 108) )
                {
                    alt24 = 1;
                }
                else if ( (LA24_1 == LBRACKED) )
                {
                    alt24 = 4;
                }
                else 
                {
                    NoViableAltException nvae_d24s1 =
                        new NoViableAltException("", 24, 1, input);

                    throw nvae_d24s1;
                }
                }
                break;
            case IDENTIFIER:
            	{
                alt24 = 2;
                }
                break;
            case STRUCT:
            	{
                alt24 = 3;
                }
                break;
            case VOID:
            case SINT:
            case INT:
            case DINT:
            case LINT:
            case USINT:
            case UINT:
            case UDINT:
            case ULINT:
            case COUNTER:
            case TIMER:
            case TIME:
            case DATE_LIT:
            case TIME_OF_DAY_LIT:
            case TOD:
            case DATE_AND_TIME:
            case DT:
            case BOOL:
            case BYTE:
            case WORD:
            case DWORD:
            case LWORD:
            case REAL:
            case LREAL:
            case CHAR:
            case WCHAR:
            	{
                alt24 = 1;
                }
                break;
            	default:
            	    NoViableAltException nvae_d24s0 =
            	        new NoViableAltException("", 24, 0, input);

            	    throw nvae_d24s0;
            }

            switch (alt24) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:167:25: elemetary_type_name
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_elemetary_type_name_in_non_generic_type_name2113);
                    	elemetary_type_name106 = elemetary_type_name();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, elemetary_type_name106.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:167:47: IDENTIFIER
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	IDENTIFIER107=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_non_generic_type_name2117); 
                    		IDENTIFIER107_tree = (object)adaptor.Create(IDENTIFIER107);
                    		adaptor.AddChild(root_0, IDENTIFIER107_tree);


                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:167:61: structure_declaration
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_structure_declaration_in_non_generic_type_name2122);
                    	structure_declaration108 = structure_declaration();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, structure_declaration108.Tree);

                    }
                    break;
                case 4 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:167:84: string_type_declaration
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_string_type_declaration_in_non_generic_type_name2125);
                    	string_type_declaration109 = string_type_declaration();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, string_type_declaration109.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "non_generic_type_name"

    public class array_specification_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "array_specification"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:169:1: array_specification : ( ARRAY array_range OF non_generic_type_name ) ;
    public MPALParser.array_specification_return array_specification() // throws RecognitionException [1]
    {   
        MPALParser.array_specification_return retval = new MPALParser.array_specification_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ARRAY110 = null;
        IToken OF112 = null;
        MPALParser.array_range_return array_range111 = default(MPALParser.array_range_return);

        MPALParser.non_generic_type_name_return non_generic_type_name113 = default(MPALParser.non_generic_type_name_return);


        object ARRAY110_tree=null;
        object OF112_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:169:21: ( ( ARRAY array_range OF non_generic_type_name ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:169:23: ( ARRAY array_range OF non_generic_type_name )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:169:23: ( ARRAY array_range OF non_generic_type_name )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:169:24: ARRAY array_range OF non_generic_type_name
            	{
            		ARRAY110=(IToken)Match(input,ARRAY,FOLLOW_ARRAY_in_array_specification2134); 
            			ARRAY110_tree = (object)adaptor.Create(ARRAY110);
            			root_0 = (object)adaptor.BecomeRoot(ARRAY110_tree, root_0);

            		PushFollow(FOLLOW_array_range_in_array_specification2137);
            		array_range111 = array_range();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, array_range111.Tree);
            		OF112=(IToken)Match(input,OF,FOLLOW_OF_in_array_specification2139); 
            		PushFollow(FOLLOW_non_generic_type_name_in_array_specification2142);
            		non_generic_type_name113 = non_generic_type_name();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, non_generic_type_name113.Tree);

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "array_specification"

    public class structure_element_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "structure_element_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:1: structure_element_declaration : IDENTIFIER ( COMMA IDENTIFIER )* ':' ( simple_spec_init | subrange_spec_init | enumerated_spec_init | array_spec_init | string_spec | initialized_structure | structure_declaration ) ;
    public MPALParser.structure_element_declaration_return structure_element_declaration() // throws RecognitionException [1]
    {   
        MPALParser.structure_element_declaration_return retval = new MPALParser.structure_element_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER114 = null;
        IToken COMMA115 = null;
        IToken IDENTIFIER116 = null;
        IToken char_literal117 = null;
        MPALParser.simple_spec_init_return simple_spec_init118 = default(MPALParser.simple_spec_init_return);

        MPALParser.subrange_spec_init_return subrange_spec_init119 = default(MPALParser.subrange_spec_init_return);

        MPALParser.enumerated_spec_init_return enumerated_spec_init120 = default(MPALParser.enumerated_spec_init_return);

        MPALParser.array_spec_init_return array_spec_init121 = default(MPALParser.array_spec_init_return);

        MPALParser.string_spec_return string_spec122 = default(MPALParser.string_spec_return);

        MPALParser.initialized_structure_return initialized_structure123 = default(MPALParser.initialized_structure_return);

        MPALParser.structure_declaration_return structure_declaration124 = default(MPALParser.structure_declaration_return);


        object IDENTIFIER114_tree=null;
        object COMMA115_tree=null;
        object IDENTIFIER116_tree=null;
        object char_literal117_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:31: ( IDENTIFIER ( COMMA IDENTIFIER )* ':' ( simple_spec_init | subrange_spec_init | enumerated_spec_init | array_spec_init | string_spec | initialized_structure | structure_declaration ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:33: IDENTIFIER ( COMMA IDENTIFIER )* ':' ( simple_spec_init | subrange_spec_init | enumerated_spec_init | array_spec_init | string_spec | initialized_structure | structure_declaration )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER114=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_structure_element_declaration2151); 
            		IDENTIFIER114_tree = (object)adaptor.Create(IDENTIFIER114);
            		adaptor.AddChild(root_0, IDENTIFIER114_tree);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:44: ( COMMA IDENTIFIER )*
            	do 
            	{
            	    int alt25 = 2;
            	    int LA25_0 = input.LA(1);

            	    if ( (LA25_0 == COMMA) )
            	    {
            	        alt25 = 1;
            	    }


            	    switch (alt25) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:45: COMMA IDENTIFIER
            			    {
            			    	COMMA115=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_structure_element_declaration2154); 
            			    	IDENTIFIER116=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_structure_element_declaration2157); 
            			    		IDENTIFIER116_tree = (object)adaptor.Create(IDENTIFIER116);
            			    		adaptor.AddChild(root_0, IDENTIFIER116_tree);


            			    }
            			    break;

            			default:
            			    goto loop25;
            	    }
            	} while (true);

            	loop25:
            		;	// Stops C# compiler whining that label 'loop25' has no statements

            	char_literal117=(IToken)Match(input,COLON,FOLLOW_COLON_in_structure_element_declaration2161); 
            		char_literal117_tree = (object)adaptor.Create(char_literal117);
            		root_0 = (object)adaptor.BecomeRoot(char_literal117_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:70: ( simple_spec_init | subrange_spec_init | enumerated_spec_init | array_spec_init | string_spec | initialized_structure | structure_declaration )
            	int alt26 = 7;
            	alt26 = dfa26.Predict(input);
            	switch (alt26) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:72: simple_spec_init
            	        {
            	        	PushFollow(FOLLOW_simple_spec_init_in_structure_element_declaration2166);
            	        	simple_spec_init118 = simple_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, simple_spec_init118.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:91: subrange_spec_init
            	        {
            	        	PushFollow(FOLLOW_subrange_spec_init_in_structure_element_declaration2170);
            	        	subrange_spec_init119 = subrange_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, subrange_spec_init119.Tree);

            	        }
            	        break;
            	    case 3 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:112: enumerated_spec_init
            	        {
            	        	PushFollow(FOLLOW_enumerated_spec_init_in_structure_element_declaration2174);
            	        	enumerated_spec_init120 = enumerated_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, enumerated_spec_init120.Tree);

            	        }
            	        break;
            	    case 4 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:135: array_spec_init
            	        {
            	        	PushFollow(FOLLOW_array_spec_init_in_structure_element_declaration2178);
            	        	array_spec_init121 = array_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, array_spec_init121.Tree);

            	        }
            	        break;
            	    case 5 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:153: string_spec
            	        {
            	        	PushFollow(FOLLOW_string_spec_in_structure_element_declaration2182);
            	        	string_spec122 = string_spec();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, string_spec122.Tree);

            	        }
            	        break;
            	    case 6 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:166: initialized_structure
            	        {
            	        	PushFollow(FOLLOW_initialized_structure_in_structure_element_declaration2185);
            	        	initialized_structure123 = initialized_structure();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, initialized_structure123.Tree);

            	        }
            	        break;
            	    case 7 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:171:190: structure_declaration
            	        {
            	        	PushFollow(FOLLOW_structure_declaration_in_structure_element_declaration2189);
            	        	structure_declaration124 = structure_declaration();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, structure_declaration124.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "structure_element_declaration"

    public class structure_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "structure_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:173:1: structure_declaration : STRUCT structure_element_declaration ';' ( structure_element_declaration ';' )* 'END_STRUCT' ;
    public MPALParser.structure_declaration_return structure_declaration() // throws RecognitionException [1]
    {   
        MPALParser.structure_declaration_return retval = new MPALParser.structure_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken STRUCT125 = null;
        IToken char_literal127 = null;
        IToken char_literal129 = null;
        IToken string_literal130 = null;
        MPALParser.structure_element_declaration_return structure_element_declaration126 = default(MPALParser.structure_element_declaration_return);

        MPALParser.structure_element_declaration_return structure_element_declaration128 = default(MPALParser.structure_element_declaration_return);


        object STRUCT125_tree=null;
        object char_literal127_tree=null;
        object char_literal129_tree=null;
        object string_literal130_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:173:23: ( STRUCT structure_element_declaration ';' ( structure_element_declaration ';' )* 'END_STRUCT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:173:25: STRUCT structure_element_declaration ';' ( structure_element_declaration ';' )* 'END_STRUCT'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	STRUCT125=(IToken)Match(input,STRUCT,FOLLOW_STRUCT_in_structure_declaration2198); 
            		STRUCT125_tree = (object)adaptor.Create(STRUCT125);
            		root_0 = (object)adaptor.BecomeRoot(STRUCT125_tree, root_0);

            	PushFollow(FOLLOW_structure_element_declaration_in_structure_declaration2201);
            	structure_element_declaration126 = structure_element_declaration();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, structure_element_declaration126.Tree);
            	char_literal127=(IToken)Match(input,108,FOLLOW_108_in_structure_declaration2203); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:173:67: ( structure_element_declaration ';' )*
            	do 
            	{
            	    int alt27 = 2;
            	    int LA27_0 = input.LA(1);

            	    if ( (LA27_0 == IDENTIFIER) )
            	    {
            	        alt27 = 1;
            	    }


            	    switch (alt27) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:173:68: structure_element_declaration ';'
            			    {
            			    	PushFollow(FOLLOW_structure_element_declaration_in_structure_declaration2206);
            			    	structure_element_declaration128 = structure_element_declaration();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, structure_element_declaration128.Tree);
            			    	char_literal129=(IToken)Match(input,108,FOLLOW_108_in_structure_declaration2208); 

            			    }
            			    break;

            			default:
            			    goto loop27;
            	    }
            	} while (true);

            	loop27:
            		;	// Stops C# compiler whining that label 'loop27' has no statements

            	string_literal130=(IToken)Match(input,109,FOLLOW_109_in_structure_declaration2213); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "structure_declaration"

    public class array_initial_element_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "array_initial_element"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:175:1: array_initial_element : ( constant_literal | enumerated_value | IDENTIFIER | structure_initialization | array_initialization );
    public MPALParser.array_initial_element_return array_initial_element() // throws RecognitionException [1]
    {   
        MPALParser.array_initial_element_return retval = new MPALParser.array_initial_element_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER133 = null;
        MPALParser.constant_literal_return constant_literal131 = default(MPALParser.constant_literal_return);

        MPALParser.enumerated_value_return enumerated_value132 = default(MPALParser.enumerated_value_return);

        MPALParser.structure_initialization_return structure_initialization134 = default(MPALParser.structure_initialization_return);

        MPALParser.array_initialization_return array_initialization135 = default(MPALParser.array_initialization_return);


        object IDENTIFIER133_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:175:23: ( constant_literal | enumerated_value | IDENTIFIER | structure_initialization | array_initialization )
            int alt28 = 5;
            alt28 = dfa28.Predict(input);
            switch (alt28) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:175:25: constant_literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_constant_literal_in_array_initial_element2222);
                    	constant_literal131 = constant_literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, constant_literal131.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:175:44: enumerated_value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_enumerated_value_in_array_initial_element2226);
                    	enumerated_value132 = enumerated_value();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, enumerated_value132.Tree);

                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:175:63: IDENTIFIER
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	IDENTIFIER133=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_array_initial_element2230); 
                    		IDENTIFIER133_tree = (object)adaptor.Create(IDENTIFIER133);
                    		adaptor.AddChild(root_0, IDENTIFIER133_tree);


                    }
                    break;
                case 4 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:175:76: structure_initialization
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_structure_initialization_in_array_initial_element2234);
                    	structure_initialization134 = structure_initialization();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, structure_initialization134.Tree);

                    }
                    break;
                case 5 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:175:103: array_initialization
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_array_initialization_in_array_initial_element2238);
                    	array_initialization135 = array_initialization();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, array_initialization135.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "array_initial_element"

    public class array_initial_elements_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "array_initial_elements"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:177:1: array_initial_elements : ( array_initial_element | INTEGER '(' ( array_initial_element )? ')' );
    public MPALParser.array_initial_elements_return array_initial_elements() // throws RecognitionException [1]
    {   
        MPALParser.array_initial_elements_return retval = new MPALParser.array_initial_elements_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken INTEGER137 = null;
        IToken char_literal138 = null;
        IToken char_literal140 = null;
        MPALParser.array_initial_element_return array_initial_element136 = default(MPALParser.array_initial_element_return);

        MPALParser.array_initial_element_return array_initial_element139 = default(MPALParser.array_initial_element_return);


        object INTEGER137_tree=null;
        object char_literal138_tree=null;
        object char_literal140_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:177:24: ( array_initial_element | INTEGER '(' ( array_initial_element )? ')' )
            int alt30 = 2;
            alt30 = dfa30.Predict(input);
            switch (alt30) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:177:26: array_initial_element
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_array_initial_element_in_array_initial_elements2246);
                    	array_initial_element136 = array_initial_element();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, array_initial_element136.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:177:50: INTEGER '(' ( array_initial_element )? ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	INTEGER137=(IToken)Match(input,INTEGER,FOLLOW_INTEGER_in_array_initial_elements2250); 
                    		INTEGER137_tree = (object)adaptor.Create(INTEGER137);
                    		adaptor.AddChild(root_0, INTEGER137_tree);

                    	char_literal138=(IToken)Match(input,LRBRACKED,FOLLOW_LRBRACKED_in_array_initial_elements2252); 
                    		char_literal138_tree = (object)adaptor.Create(char_literal138);
                    		adaptor.AddChild(root_0, char_literal138_tree);

                    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:177:62: ( array_initial_element )?
                    	int alt29 = 2;
                    	alt29 = dfa29.Predict(input);
                    	switch (alt29) 
                    	{
                    	    case 1 :
                    	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:177:63: array_initial_element
                    	        {
                    	        	PushFollow(FOLLOW_array_initial_element_in_array_initial_elements2255);
                    	        	array_initial_element139 = array_initial_element();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, array_initial_element139.Tree);

                    	        }
                    	        break;

                    	}

                    	char_literal140=(IToken)Match(input,107,FOLLOW_107_in_array_initial_elements2259); 
                    		char_literal140_tree = (object)adaptor.Create(char_literal140);
                    		adaptor.AddChild(root_0, char_literal140_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "array_initial_elements"

    public class array_initialization_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "array_initialization"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:179:1: array_initialization : LBRACKED array_initial_elements ( COMMA array_initial_elements )* ']' ;
    public MPALParser.array_initialization_return array_initialization() // throws RecognitionException [1]
    {   
        MPALParser.array_initialization_return retval = new MPALParser.array_initialization_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LBRACKED141 = null;
        IToken COMMA143 = null;
        IToken char_literal145 = null;
        MPALParser.array_initial_elements_return array_initial_elements142 = default(MPALParser.array_initial_elements_return);

        MPALParser.array_initial_elements_return array_initial_elements144 = default(MPALParser.array_initial_elements_return);


        object LBRACKED141_tree=null;
        object COMMA143_tree=null;
        object char_literal145_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:179:22: ( LBRACKED array_initial_elements ( COMMA array_initial_elements )* ']' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:179:24: LBRACKED array_initial_elements ( COMMA array_initial_elements )* ']'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LBRACKED141=(IToken)Match(input,LBRACKED,FOLLOW_LBRACKED_in_array_initialization2268); 
            		LBRACKED141_tree = (object)adaptor.Create(LBRACKED141);
            		root_0 = (object)adaptor.BecomeRoot(LBRACKED141_tree, root_0);

            	PushFollow(FOLLOW_array_initial_elements_in_array_initialization2271);
            	array_initial_elements142 = array_initial_elements();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, array_initial_elements142.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:179:57: ( COMMA array_initial_elements )*
            	do 
            	{
            	    int alt31 = 2;
            	    int LA31_0 = input.LA(1);

            	    if ( (LA31_0 == COMMA) )
            	    {
            	        alt31 = 1;
            	    }


            	    switch (alt31) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:179:58: COMMA array_initial_elements
            			    {
            			    	COMMA143=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_array_initialization2274); 
            			    	PushFollow(FOLLOW_array_initial_elements_in_array_initialization2277);
            			    	array_initial_elements144 = array_initial_elements();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, array_initial_elements144.Tree);

            			    }
            			    break;

            			default:
            			    goto loop31;
            	    }
            	} while (true);

            	loop31:
            		;	// Stops C# compiler whining that label 'loop31' has no statements

            	char_literal145=(IToken)Match(input,106,FOLLOW_106_in_array_initialization2281); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "array_initialization"

    public class structure_element_initialization_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "structure_element_initialization"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:1: structure_element_initialization : IDENTIFIER ':=' ( enumerated_value | IDENTIFIER | constant_literal | array_initialization | structure_initialization ) ;
    public MPALParser.structure_element_initialization_return structure_element_initialization() // throws RecognitionException [1]
    {   
        MPALParser.structure_element_initialization_return retval = new MPALParser.structure_element_initialization_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER146 = null;
        IToken string_literal147 = null;
        IToken IDENTIFIER149 = null;
        MPALParser.enumerated_value_return enumerated_value148 = default(MPALParser.enumerated_value_return);

        MPALParser.constant_literal_return constant_literal150 = default(MPALParser.constant_literal_return);

        MPALParser.array_initialization_return array_initialization151 = default(MPALParser.array_initialization_return);

        MPALParser.structure_initialization_return structure_initialization152 = default(MPALParser.structure_initialization_return);


        object IDENTIFIER146_tree=null;
        object string_literal147_tree=null;
        object IDENTIFIER149_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:34: ( IDENTIFIER ':=' ( enumerated_value | IDENTIFIER | constant_literal | array_initialization | structure_initialization ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:36: IDENTIFIER ':=' ( enumerated_value | IDENTIFIER | constant_literal | array_initialization | structure_initialization )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER146=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_structure_element_initialization2291); 
            		IDENTIFIER146_tree = (object)adaptor.Create(IDENTIFIER146);
            		adaptor.AddChild(root_0, IDENTIFIER146_tree);

            	string_literal147=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_structure_element_initialization2293); 
            		string_literal147_tree = (object)adaptor.Create(string_literal147);
            		root_0 = (object)adaptor.BecomeRoot(string_literal147_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:53: ( enumerated_value | IDENTIFIER | constant_literal | array_initialization | structure_initialization )
            	int alt32 = 5;
            	alt32 = dfa32.Predict(input);
            	switch (alt32) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:54: enumerated_value
            	        {
            	        	PushFollow(FOLLOW_enumerated_value_in_structure_element_initialization2297);
            	        	enumerated_value148 = enumerated_value();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, enumerated_value148.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:72: IDENTIFIER
            	        {
            	        	IDENTIFIER149=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_structure_element_initialization2300); 
            	        		IDENTIFIER149_tree = (object)adaptor.Create(IDENTIFIER149);
            	        		adaptor.AddChild(root_0, IDENTIFIER149_tree);


            	        }
            	        break;
            	    case 3 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:85: constant_literal
            	        {
            	        	PushFollow(FOLLOW_constant_literal_in_structure_element_initialization2304);
            	        	constant_literal150 = constant_literal();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, constant_literal150.Tree);

            	        }
            	        break;
            	    case 4 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:104: array_initialization
            	        {
            	        	PushFollow(FOLLOW_array_initialization_in_structure_element_initialization2308);
            	        	array_initialization151 = array_initialization();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, array_initialization151.Tree);

            	        }
            	        break;
            	    case 5 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:181:127: structure_initialization
            	        {
            	        	PushFollow(FOLLOW_structure_initialization_in_structure_element_initialization2312);
            	        	structure_initialization152 = structure_initialization();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, structure_initialization152.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "structure_element_initialization"

    public class structure_initialization_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "structure_initialization"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:183:1: structure_initialization : LRBRACKED structure_element_initialization ( COMMA structure_element_initialization )* ')' ;
    public MPALParser.structure_initialization_return structure_initialization() // throws RecognitionException [1]
    {   
        MPALParser.structure_initialization_return retval = new MPALParser.structure_initialization_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LRBRACKED153 = null;
        IToken COMMA155 = null;
        IToken char_literal157 = null;
        MPALParser.structure_element_initialization_return structure_element_initialization154 = default(MPALParser.structure_element_initialization_return);

        MPALParser.structure_element_initialization_return structure_element_initialization156 = default(MPALParser.structure_element_initialization_return);


        object LRBRACKED153_tree=null;
        object COMMA155_tree=null;
        object char_literal157_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:183:26: ( LRBRACKED structure_element_initialization ( COMMA structure_element_initialization )* ')' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:183:28: LRBRACKED structure_element_initialization ( COMMA structure_element_initialization )* ')'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LRBRACKED153=(IToken)Match(input,LRBRACKED,FOLLOW_LRBRACKED_in_structure_initialization2321); 
            		LRBRACKED153_tree = (object)adaptor.Create(LRBRACKED153);
            		root_0 = (object)adaptor.BecomeRoot(LRBRACKED153_tree, root_0);

            	PushFollow(FOLLOW_structure_element_initialization_in_structure_initialization2324);
            	structure_element_initialization154 = structure_element_initialization();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, structure_element_initialization154.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:183:72: ( COMMA structure_element_initialization )*
            	do 
            	{
            	    int alt33 = 2;
            	    int LA33_0 = input.LA(1);

            	    if ( (LA33_0 == COMMA) )
            	    {
            	        alt33 = 1;
            	    }


            	    switch (alt33) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:183:73: COMMA structure_element_initialization
            			    {
            			    	COMMA155=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_structure_initialization2327); 
            			    	PushFollow(FOLLOW_structure_element_initialization_in_structure_initialization2330);
            			    	structure_element_initialization156 = structure_element_initialization();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, structure_element_initialization156.Tree);

            			    }
            			    break;

            			default:
            			    goto loop33;
            	    }
            	} while (true);

            	loop33:
            		;	// Stops C# compiler whining that label 'loop33' has no statements

            	char_literal157=(IToken)Match(input,107,FOLLOW_107_in_structure_initialization2334); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "structure_initialization"

    public class initialized_structure_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "initialized_structure"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:185:1: initialized_structure : IDENTIFIER ':=' structure_initialization ;
    public MPALParser.initialized_structure_return initialized_structure() // throws RecognitionException [1]
    {   
        MPALParser.initialized_structure_return retval = new MPALParser.initialized_structure_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER158 = null;
        IToken string_literal159 = null;
        MPALParser.structure_initialization_return structure_initialization160 = default(MPALParser.structure_initialization_return);


        object IDENTIFIER158_tree=null;
        object string_literal159_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:185:23: ( IDENTIFIER ':=' structure_initialization )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:185:25: IDENTIFIER ':=' structure_initialization
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER158=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_initialized_structure2343); 
            		IDENTIFIER158_tree = (object)adaptor.Create(IDENTIFIER158);
            		adaptor.AddChild(root_0, IDENTIFIER158_tree);

            	string_literal159=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_initialized_structure2345); 
            	PushFollow(FOLLOW_structure_initialization_in_initialized_structure2349);
            	structure_initialization160 = structure_initialization();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, structure_initialization160.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "initialized_structure"

    public class structure_specification_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "structure_specification"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:187:1: structure_specification : ( structure_declaration | initialized_structure );
    public MPALParser.structure_specification_return structure_specification() // throws RecognitionException [1]
    {   
        MPALParser.structure_specification_return retval = new MPALParser.structure_specification_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.structure_declaration_return structure_declaration161 = default(MPALParser.structure_declaration_return);

        MPALParser.initialized_structure_return initialized_structure162 = default(MPALParser.initialized_structure_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:187:25: ( structure_declaration | initialized_structure )
            int alt34 = 2;
            int LA34_0 = input.LA(1);

            if ( (LA34_0 == STRUCT) )
            {
                alt34 = 1;
            }
            else if ( (LA34_0 == IDENTIFIER) )
            {
                alt34 = 2;
            }
            else 
            {
                NoViableAltException nvae_d34s0 =
                    new NoViableAltException("", 34, 0, input);

                throw nvae_d34s0;
            }
            switch (alt34) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:187:27: structure_declaration
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_structure_declaration_in_structure_specification2357);
                    	structure_declaration161 = structure_declaration();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, structure_declaration161.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:187:51: initialized_structure
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_initialized_structure_in_structure_specification2361);
                    	initialized_structure162 = initialized_structure();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, initialized_structure162.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "structure_specification"

    public class type_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "type_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:189:1: type_declaration : IDENTIFIER ':' ( string_type_declaration | single_element_type_declaration | structure_specification | array_spec_init ) ;
    public MPALParser.type_declaration_return type_declaration() // throws RecognitionException [1]
    {   
        MPALParser.type_declaration_return retval = new MPALParser.type_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER163 = null;
        IToken char_literal164 = null;
        MPALParser.string_type_declaration_return string_type_declaration165 = default(MPALParser.string_type_declaration_return);

        MPALParser.single_element_type_declaration_return single_element_type_declaration166 = default(MPALParser.single_element_type_declaration_return);

        MPALParser.structure_specification_return structure_specification167 = default(MPALParser.structure_specification_return);

        MPALParser.array_spec_init_return array_spec_init168 = default(MPALParser.array_spec_init_return);


        object IDENTIFIER163_tree=null;
        object char_literal164_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:189:19: ( IDENTIFIER ':' ( string_type_declaration | single_element_type_declaration | structure_specification | array_spec_init ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:189:21: IDENTIFIER ':' ( string_type_declaration | single_element_type_declaration | structure_specification | array_spec_init )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER163=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_type_declaration2370); 
            		IDENTIFIER163_tree = (object)adaptor.Create(IDENTIFIER163);
            		adaptor.AddChild(root_0, IDENTIFIER163_tree);

            	char_literal164=(IToken)Match(input,COLON,FOLLOW_COLON_in_type_declaration2372); 
            		char_literal164_tree = (object)adaptor.Create(char_literal164);
            		root_0 = (object)adaptor.BecomeRoot(char_literal164_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:189:37: ( string_type_declaration | single_element_type_declaration | structure_specification | array_spec_init )
            	int alt35 = 4;
            	alt35 = dfa35.Predict(input);
            	switch (alt35) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:189:39: string_type_declaration
            	        {
            	        	PushFollow(FOLLOW_string_type_declaration_in_type_declaration2377);
            	        	string_type_declaration165 = string_type_declaration();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, string_type_declaration165.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:189:66: single_element_type_declaration
            	        {
            	        	PushFollow(FOLLOW_single_element_type_declaration_in_type_declaration2382);
            	        	single_element_type_declaration166 = single_element_type_declaration();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, single_element_type_declaration166.Tree);

            	        }
            	        break;
            	    case 3 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:189:100: structure_specification
            	        {
            	        	PushFollow(FOLLOW_structure_specification_in_type_declaration2386);
            	        	structure_specification167 = structure_specification();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, structure_specification167.Tree);

            	        }
            	        break;
            	    case 4 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:189:127: array_spec_init
            	        {
            	        	PushFollow(FOLLOW_array_spec_init_in_type_declaration2391);
            	        	array_spec_init168 = array_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, array_spec_init168.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "type_declaration"

    public class data_type_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "data_type_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:191:1: data_type_declaration : TYPE type_declaration ';' ( type_declaration ';' )* 'END_TYPE' ;
    public MPALParser.data_type_declaration_return data_type_declaration() // throws RecognitionException [1]
    {   
        MPALParser.data_type_declaration_return retval = new MPALParser.data_type_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken TYPE169 = null;
        IToken char_literal171 = null;
        IToken char_literal173 = null;
        IToken string_literal174 = null;
        MPALParser.type_declaration_return type_declaration170 = default(MPALParser.type_declaration_return);

        MPALParser.type_declaration_return type_declaration172 = default(MPALParser.type_declaration_return);


        object TYPE169_tree=null;
        object char_literal171_tree=null;
        object char_literal173_tree=null;
        object string_literal174_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:191:23: ( TYPE type_declaration ';' ( type_declaration ';' )* 'END_TYPE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:191:25: TYPE type_declaration ';' ( type_declaration ';' )* 'END_TYPE'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	TYPE169=(IToken)Match(input,TYPE,FOLLOW_TYPE_in_data_type_declaration2401); 
            		TYPE169_tree = (object)adaptor.Create(TYPE169);
            		root_0 = (object)adaptor.BecomeRoot(TYPE169_tree, root_0);

            	PushFollow(FOLLOW_type_declaration_in_data_type_declaration2404);
            	type_declaration170 = type_declaration();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, type_declaration170.Tree);
            	char_literal171=(IToken)Match(input,108,FOLLOW_108_in_data_type_declaration2406); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:191:53: ( type_declaration ';' )*
            	do 
            	{
            	    int alt36 = 2;
            	    int LA36_0 = input.LA(1);

            	    if ( (LA36_0 == IDENTIFIER) )
            	    {
            	        alt36 = 1;
            	    }


            	    switch (alt36) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:191:54: type_declaration ';'
            			    {
            			    	PushFollow(FOLLOW_type_declaration_in_data_type_declaration2410);
            			    	type_declaration172 = type_declaration();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, type_declaration172.Tree);
            			    	char_literal173=(IToken)Match(input,108,FOLLOW_108_in_data_type_declaration2412); 

            			    }
            			    break;

            			default:
            			    goto loop36;
            	    }
            	} while (true);

            	loop36:
            		;	// Stops C# compiler whining that label 'loop36' has no statements

            	string_literal174=(IToken)Match(input,110,FOLLOW_110_in_data_type_declaration2417); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "data_type_declaration"

    public class io_var_declarations_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "io_var_declarations"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:193:1: io_var_declarations : ( input_output_declarations | input_declarations | output_declarations );
    public MPALParser.io_var_declarations_return io_var_declarations() // throws RecognitionException [1]
    {   
        MPALParser.io_var_declarations_return retval = new MPALParser.io_var_declarations_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.input_output_declarations_return input_output_declarations175 = default(MPALParser.input_output_declarations_return);

        MPALParser.input_declarations_return input_declarations176 = default(MPALParser.input_declarations_return);

        MPALParser.output_declarations_return output_declarations177 = default(MPALParser.output_declarations_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:193:21: ( input_output_declarations | input_declarations | output_declarations )
            int alt37 = 3;
            switch ( input.LA(1) ) 
            {
            case VAR_IN_OUT:
            	{
                alt37 = 1;
                }
                break;
            case VAR_INPUT:
            	{
                alt37 = 2;
                }
                break;
            case VAR_OUTPUT:
            	{
                alt37 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d37s0 =
            	        new NoViableAltException("", 37, 0, input);

            	    throw nvae_d37s0;
            }

            switch (alt37) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:193:24: input_output_declarations
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_input_output_declarations_in_io_var_declarations2427);
                    	input_output_declarations175 = input_output_declarations();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, input_output_declarations175.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:193:52: input_declarations
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_input_declarations_in_io_var_declarations2431);
                    	input_declarations176 = input_declarations();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, input_declarations176.Tree);

                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:193:74: output_declarations
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_output_declarations_in_io_var_declarations2436);
                    	output_declarations177 = output_declarations();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, output_declarations177.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "io_var_declarations"

    public class input_output_declarations_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "input_output_declarations"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:195:1: input_output_declarations : VAR_IN_OUT ( var_declaration ';' )+ 'END_VAR' ;
    public MPALParser.input_output_declarations_return input_output_declarations() // throws RecognitionException [1]
    {   
        MPALParser.input_output_declarations_return retval = new MPALParser.input_output_declarations_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken VAR_IN_OUT178 = null;
        IToken char_literal180 = null;
        IToken string_literal181 = null;
        MPALParser.var_declaration_return var_declaration179 = default(MPALParser.var_declaration_return);


        object VAR_IN_OUT178_tree=null;
        object char_literal180_tree=null;
        object string_literal181_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:195:27: ( VAR_IN_OUT ( var_declaration ';' )+ 'END_VAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:195:29: VAR_IN_OUT ( var_declaration ';' )+ 'END_VAR'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	VAR_IN_OUT178=(IToken)Match(input,VAR_IN_OUT,FOLLOW_VAR_IN_OUT_in_input_output_declarations2445); 
            		VAR_IN_OUT178_tree = (object)adaptor.Create(VAR_IN_OUT178);
            		root_0 = (object)adaptor.BecomeRoot(VAR_IN_OUT178_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:195:41: ( var_declaration ';' )+
            	int cnt38 = 0;
            	do 
            	{
            	    int alt38 = 2;
            	    int LA38_0 = input.LA(1);

            	    if ( (LA38_0 == IDENTIFIER) )
            	    {
            	        alt38 = 1;
            	    }


            	    switch (alt38) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:195:43: var_declaration ';'
            			    {
            			    	PushFollow(FOLLOW_var_declaration_in_input_output_declarations2450);
            			    	var_declaration179 = var_declaration();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, var_declaration179.Tree);
            			    	char_literal180=(IToken)Match(input,108,FOLLOW_108_in_input_output_declarations2452); 

            			    }
            			    break;

            			default:
            			    if ( cnt38 >= 1 ) goto loop38;
            		            EarlyExitException eee38 =
            		                new EarlyExitException(38, input);
            		            throw eee38;
            	    }
            	    cnt38++;
            	} while (true);

            	loop38:
            		;	// Stops C# compiler whining that label 'loop38' has no statements

            	string_literal181=(IToken)Match(input,111,FOLLOW_111_in_input_output_declarations2457); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "input_output_declarations"

    public class input_declarations_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "input_declarations"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:197:1: input_declarations : VAR_INPUT ( var_init_decl ';' )+ 'END_VAR' ;
    public MPALParser.input_declarations_return input_declarations() // throws RecognitionException [1]
    {   
        MPALParser.input_declarations_return retval = new MPALParser.input_declarations_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken VAR_INPUT182 = null;
        IToken char_literal184 = null;
        IToken string_literal185 = null;
        MPALParser.var_init_decl_return var_init_decl183 = default(MPALParser.var_init_decl_return);


        object VAR_INPUT182_tree=null;
        object char_literal184_tree=null;
        object string_literal185_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:197:20: ( VAR_INPUT ( var_init_decl ';' )+ 'END_VAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:197:22: VAR_INPUT ( var_init_decl ';' )+ 'END_VAR'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	VAR_INPUT182=(IToken)Match(input,VAR_INPUT,FOLLOW_VAR_INPUT_in_input_declarations2466); 
            		VAR_INPUT182_tree = (object)adaptor.Create(VAR_INPUT182);
            		root_0 = (object)adaptor.BecomeRoot(VAR_INPUT182_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:197:33: ( var_init_decl ';' )+
            	int cnt39 = 0;
            	do 
            	{
            	    int alt39 = 2;
            	    int LA39_0 = input.LA(1);

            	    if ( (LA39_0 == IDENTIFIER) )
            	    {
            	        alt39 = 1;
            	    }


            	    switch (alt39) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:197:34: var_init_decl ';'
            			    {
            			    	PushFollow(FOLLOW_var_init_decl_in_input_declarations2470);
            			    	var_init_decl183 = var_init_decl();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, var_init_decl183.Tree);
            			    	char_literal184=(IToken)Match(input,108,FOLLOW_108_in_input_declarations2472); 

            			    }
            			    break;

            			default:
            			    if ( cnt39 >= 1 ) goto loop39;
            		            EarlyExitException eee39 =
            		                new EarlyExitException(39, input);
            		            throw eee39;
            	    }
            	    cnt39++;
            	} while (true);

            	loop39:
            		;	// Stops C# compiler whining that label 'loop39' has no statements

            	string_literal185=(IToken)Match(input,111,FOLLOW_111_in_input_declarations2478); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "input_declarations"

    public class output_declarations_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "output_declarations"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:199:1: output_declarations : VAR_OUTPUT ( var_init_decl ';' )+ 'END_VAR' ;
    public MPALParser.output_declarations_return output_declarations() // throws RecognitionException [1]
    {   
        MPALParser.output_declarations_return retval = new MPALParser.output_declarations_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken VAR_OUTPUT186 = null;
        IToken char_literal188 = null;
        IToken string_literal189 = null;
        MPALParser.var_init_decl_return var_init_decl187 = default(MPALParser.var_init_decl_return);


        object VAR_OUTPUT186_tree=null;
        object char_literal188_tree=null;
        object string_literal189_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:199:21: ( VAR_OUTPUT ( var_init_decl ';' )+ 'END_VAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:199:23: VAR_OUTPUT ( var_init_decl ';' )+ 'END_VAR'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	VAR_OUTPUT186=(IToken)Match(input,VAR_OUTPUT,FOLLOW_VAR_OUTPUT_in_output_declarations2487); 
            		VAR_OUTPUT186_tree = (object)adaptor.Create(VAR_OUTPUT186);
            		root_0 = (object)adaptor.BecomeRoot(VAR_OUTPUT186_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:199:35: ( var_init_decl ';' )+
            	int cnt40 = 0;
            	do 
            	{
            	    int alt40 = 2;
            	    int LA40_0 = input.LA(1);

            	    if ( (LA40_0 == IDENTIFIER) )
            	    {
            	        alt40 = 1;
            	    }


            	    switch (alt40) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:199:36: var_init_decl ';'
            			    {
            			    	PushFollow(FOLLOW_var_init_decl_in_output_declarations2491);
            			    	var_init_decl187 = var_init_decl();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, var_init_decl187.Tree);
            			    	char_literal188=(IToken)Match(input,108,FOLLOW_108_in_output_declarations2493); 

            			    }
            			    break;

            			default:
            			    if ( cnt40 >= 1 ) goto loop40;
            		            EarlyExitException eee40 =
            		                new EarlyExitException(40, input);
            		            throw eee40;
            	    }
            	    cnt40++;
            	} while (true);

            	loop40:
            		;	// Stops C# compiler whining that label 'loop40' has no statements

            	string_literal189=(IToken)Match(input,111,FOLLOW_111_in_output_declarations2498); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "output_declarations"

    public class var_init_decl_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "var_init_decl"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:1: var_init_decl : var_list ':' ( string_spec | simple_spec_init | ( BOOL ( R_EDGE | F_EDGE ) ) | array_spec_init | structure_specification | subrange_spec_init | enumerated_spec_init | udt_array_spec_init ) ;
    public MPALParser.var_init_decl_return var_init_decl() // throws RecognitionException [1]
    {   
        MPALParser.var_init_decl_return retval = new MPALParser.var_init_decl_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal191 = null;
        IToken BOOL194 = null;
        IToken set195 = null;
        MPALParser.var_list_return var_list190 = default(MPALParser.var_list_return);

        MPALParser.string_spec_return string_spec192 = default(MPALParser.string_spec_return);

        MPALParser.simple_spec_init_return simple_spec_init193 = default(MPALParser.simple_spec_init_return);

        MPALParser.array_spec_init_return array_spec_init196 = default(MPALParser.array_spec_init_return);

        MPALParser.structure_specification_return structure_specification197 = default(MPALParser.structure_specification_return);

        MPALParser.subrange_spec_init_return subrange_spec_init198 = default(MPALParser.subrange_spec_init_return);

        MPALParser.enumerated_spec_init_return enumerated_spec_init199 = default(MPALParser.enumerated_spec_init_return);

        MPALParser.udt_array_spec_init_return udt_array_spec_init200 = default(MPALParser.udt_array_spec_init_return);


        object char_literal191_tree=null;
        object BOOL194_tree=null;
        object set195_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:15: ( var_list ':' ( string_spec | simple_spec_init | ( BOOL ( R_EDGE | F_EDGE ) ) | array_spec_init | structure_specification | subrange_spec_init | enumerated_spec_init | udt_array_spec_init ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:17: var_list ':' ( string_spec | simple_spec_init | ( BOOL ( R_EDGE | F_EDGE ) ) | array_spec_init | structure_specification | subrange_spec_init | enumerated_spec_init | udt_array_spec_init )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_var_list_in_var_init_decl2507);
            	var_list190 = var_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, var_list190.Tree);
            	char_literal191=(IToken)Match(input,COLON,FOLLOW_COLON_in_var_init_decl2509); 
            		char_literal191_tree = (object)adaptor.Create(char_literal191);
            		root_0 = (object)adaptor.BecomeRoot(char_literal191_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:31: ( string_spec | simple_spec_init | ( BOOL ( R_EDGE | F_EDGE ) ) | array_spec_init | structure_specification | subrange_spec_init | enumerated_spec_init | udt_array_spec_init )
            	int alt41 = 8;
            	alt41 = dfa41.Predict(input);
            	switch (alt41) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:32: string_spec
            	        {
            	        	PushFollow(FOLLOW_string_spec_in_var_init_decl2513);
            	        	string_spec192 = string_spec();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, string_spec192.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:47: simple_spec_init
            	        {
            	        	PushFollow(FOLLOW_simple_spec_init_in_var_init_decl2518);
            	        	simple_spec_init193 = simple_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, simple_spec_init193.Tree);

            	        }
            	        break;
            	    case 3 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:66: ( BOOL ( R_EDGE | F_EDGE ) )
            	        {
            	        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:66: ( BOOL ( R_EDGE | F_EDGE ) )
            	        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:67: BOOL ( R_EDGE | F_EDGE )
            	        	{
            	        		BOOL194=(IToken)Match(input,BOOL,FOLLOW_BOOL_in_var_init_decl2523); 
            	        			BOOL194_tree = (object)adaptor.Create(BOOL194);
            	        			adaptor.AddChild(root_0, BOOL194_tree);

            	        		set195 = (IToken)input.LT(1);
            	        		if ( (input.LA(1) >= R_EDGE && input.LA(1) <= F_EDGE) ) 
            	        		{
            	        		    input.Consume();
            	        		    adaptor.AddChild(root_0, (object)adaptor.Create(set195));
            	        		    state.errorRecovery = false;
            	        		}
            	        		else 
            	        		{
            	        		    MismatchedSetException mse = new MismatchedSetException(null,input);
            	        		    throw mse;
            	        		}


            	        	}


            	        }
            	        break;
            	    case 4 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:92: array_spec_init
            	        {
            	        	PushFollow(FOLLOW_array_spec_init_in_var_init_decl2535);
            	        	array_spec_init196 = array_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, array_spec_init196.Tree);

            	        }
            	        break;
            	    case 5 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:109: structure_specification
            	        {
            	        	PushFollow(FOLLOW_structure_specification_in_var_init_decl2538);
            	        	structure_specification197 = structure_specification();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, structure_specification197.Tree);

            	        }
            	        break;
            	    case 6 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:135: subrange_spec_init
            	        {
            	        	PushFollow(FOLLOW_subrange_spec_init_in_var_init_decl2542);
            	        	subrange_spec_init198 = subrange_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, subrange_spec_init198.Tree);

            	        }
            	        break;
            	    case 7 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:156: enumerated_spec_init
            	        {
            	        	PushFollow(FOLLOW_enumerated_spec_init_in_var_init_decl2546);
            	        	enumerated_spec_init199 = enumerated_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, enumerated_spec_init199.Tree);

            	        }
            	        break;
            	    case 8 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:201:178: udt_array_spec_init
            	        {
            	        	PushFollow(FOLLOW_udt_array_spec_init_in_var_init_decl2549);
            	        	udt_array_spec_init200 = udt_array_spec_init();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, udt_array_spec_init200.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "var_init_decl"

    public class var_declaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "var_declaration"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:1: var_declaration : var_list ':' ( structure_specification | string_spec | array_specification | simple_specification | subrange_specification | enumerated_specification ) ;
    public MPALParser.var_declaration_return var_declaration() // throws RecognitionException [1]
    {   
        MPALParser.var_declaration_return retval = new MPALParser.var_declaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal202 = null;
        MPALParser.var_list_return var_list201 = default(MPALParser.var_list_return);

        MPALParser.structure_specification_return structure_specification203 = default(MPALParser.structure_specification_return);

        MPALParser.string_spec_return string_spec204 = default(MPALParser.string_spec_return);

        MPALParser.array_specification_return array_specification205 = default(MPALParser.array_specification_return);

        MPALParser.simple_specification_return simple_specification206 = default(MPALParser.simple_specification_return);

        MPALParser.subrange_specification_return subrange_specification207 = default(MPALParser.subrange_specification_return);

        MPALParser.enumerated_specification_return enumerated_specification208 = default(MPALParser.enumerated_specification_return);


        object char_literal202_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:17: ( var_list ':' ( structure_specification | string_spec | array_specification | simple_specification | subrange_specification | enumerated_specification ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:19: var_list ':' ( structure_specification | string_spec | array_specification | simple_specification | subrange_specification | enumerated_specification )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_var_list_in_var_declaration2559);
            	var_list201 = var_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, var_list201.Tree);
            	char_literal202=(IToken)Match(input,COLON,FOLLOW_COLON_in_var_declaration2561); 
            		char_literal202_tree = (object)adaptor.Create(char_literal202);
            		root_0 = (object)adaptor.BecomeRoot(char_literal202_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:33: ( structure_specification | string_spec | array_specification | simple_specification | subrange_specification | enumerated_specification )
            	int alt42 = 6;
            	alt42 = dfa42.Predict(input);
            	switch (alt42) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:34: structure_specification
            	        {
            	        	PushFollow(FOLLOW_structure_specification_in_var_declaration2565);
            	        	structure_specification203 = structure_specification();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, structure_specification203.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:59: string_spec
            	        {
            	        	PushFollow(FOLLOW_string_spec_in_var_declaration2568);
            	        	string_spec204 = string_spec();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, string_spec204.Tree);

            	        }
            	        break;
            	    case 3 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:73: array_specification
            	        {
            	        	PushFollow(FOLLOW_array_specification_in_var_declaration2572);
            	        	array_specification205 = array_specification();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, array_specification205.Tree);

            	        }
            	        break;
            	    case 4 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:95: simple_specification
            	        {
            	        	PushFollow(FOLLOW_simple_specification_in_var_declaration2576);
            	        	simple_specification206 = simple_specification();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, simple_specification206.Tree);

            	        }
            	        break;
            	    case 5 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:118: subrange_specification
            	        {
            	        	PushFollow(FOLLOW_subrange_specification_in_var_declaration2580);
            	        	subrange_specification207 = subrange_specification();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, subrange_specification207.Tree);

            	        }
            	        break;
            	    case 6 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:203:143: enumerated_specification
            	        {
            	        	PushFollow(FOLLOW_enumerated_specification_in_var_declaration2584);
            	        	enumerated_specification208 = enumerated_specification();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, enumerated_specification208.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "var_declaration"

    public class var_list_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "var_list"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:205:1: var_list : IDENTIFIER ( COMMA IDENTIFIER )* ;
    public MPALParser.var_list_return var_list() // throws RecognitionException [1]
    {   
        MPALParser.var_list_return retval = new MPALParser.var_list_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER209 = null;
        IToken COMMA210 = null;
        IToken IDENTIFIER211 = null;

        object IDENTIFIER209_tree=null;
        object COMMA210_tree=null;
        object IDENTIFIER211_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:205:10: ( IDENTIFIER ( COMMA IDENTIFIER )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:205:12: IDENTIFIER ( COMMA IDENTIFIER )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER209=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_var_list2594); 
            		IDENTIFIER209_tree = (object)adaptor.Create(IDENTIFIER209);
            		adaptor.AddChild(root_0, IDENTIFIER209_tree);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:205:23: ( COMMA IDENTIFIER )*
            	do 
            	{
            	    int alt43 = 2;
            	    int LA43_0 = input.LA(1);

            	    if ( (LA43_0 == COMMA) )
            	    {
            	        alt43 = 1;
            	    }


            	    switch (alt43) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:205:24: COMMA IDENTIFIER
            			    {
            			    	COMMA210=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_var_list2597); 
            			    	IDENTIFIER211=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_var_list2600); 
            			    		IDENTIFIER211_tree = (object)adaptor.Create(IDENTIFIER211);
            			    		adaptor.AddChild(root_0, IDENTIFIER211_tree);


            			    }
            			    break;

            			default:
            			    goto loop43;
            	    }
            	} while (true);

            	loop43:
            		;	// Stops C# compiler whining that label 'loop43' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "var_list"

    public class string_spec_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "string_spec"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:1: string_spec : ( STRING | WSTRING ) ( LBRACKED INTEGER ']' ) ( ':=' character_string )? ;
    public MPALParser.string_spec_return string_spec() // throws RecognitionException [1]
    {   
        MPALParser.string_spec_return retval = new MPALParser.string_spec_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken STRING212 = null;
        IToken WSTRING213 = null;
        IToken LBRACKED214 = null;
        IToken INTEGER215 = null;
        IToken char_literal216 = null;
        IToken string_literal217 = null;
        MPALParser.character_string_return character_string218 = default(MPALParser.character_string_return);


        object STRING212_tree=null;
        object WSTRING213_tree=null;
        object LBRACKED214_tree=null;
        object INTEGER215_tree=null;
        object char_literal216_tree=null;
        object string_literal217_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:13: ( ( STRING | WSTRING ) ( LBRACKED INTEGER ']' ) ( ':=' character_string )? )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:16: ( STRING | WSTRING ) ( LBRACKED INTEGER ']' ) ( ':=' character_string )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:16: ( STRING | WSTRING )
            	int alt44 = 2;
            	int LA44_0 = input.LA(1);

            	if ( (LA44_0 == STRING) )
            	{
            	    alt44 = 1;
            	}
            	else if ( (LA44_0 == WSTRING) )
            	{
            	    alt44 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d44s0 =
            	        new NoViableAltException("", 44, 0, input);

            	    throw nvae_d44s0;
            	}
            	switch (alt44) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:17: STRING
            	        {
            	        	STRING212=(IToken)Match(input,STRING,FOLLOW_STRING_in_string_spec2612); 
            	        		STRING212_tree = (object)adaptor.Create(STRING212);
            	        		root_0 = (object)adaptor.BecomeRoot(STRING212_tree, root_0);


            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:27: WSTRING
            	        {
            	        	WSTRING213=(IToken)Match(input,WSTRING,FOLLOW_WSTRING_in_string_spec2617); 
            	        		WSTRING213_tree = (object)adaptor.Create(WSTRING213);
            	        		root_0 = (object)adaptor.BecomeRoot(WSTRING213_tree, root_0);


            	        }
            	        break;

            	}

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:37: ( LBRACKED INTEGER ']' )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:38: LBRACKED INTEGER ']'
            	{
            		LBRACKED214=(IToken)Match(input,LBRACKED,FOLLOW_LBRACKED_in_string_spec2622); 
            		INTEGER215=(IToken)Match(input,INTEGER,FOLLOW_INTEGER_in_string_spec2625); 
            			INTEGER215_tree = (object)adaptor.Create(INTEGER215);
            			adaptor.AddChild(root_0, INTEGER215_tree);

            		char_literal216=(IToken)Match(input,106,FOLLOW_106_in_string_spec2627); 

            	}

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:62: ( ':=' character_string )?
            	int alt45 = 2;
            	int LA45_0 = input.LA(1);

            	if ( (LA45_0 == ASSIGN) )
            	{
            	    alt45 = 1;
            	}
            	switch (alt45) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:207:63: ':=' character_string
            	        {
            	        	string_literal217=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_string_spec2632); 
            	        	PushFollow(FOLLOW_character_string_in_string_spec2635);
            	        	character_string218 = character_string();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, character_string218.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "string_spec"

    public class other_var_declarations_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "other_var_declarations"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:209:1: other_var_declarations : ( var_declarations | temp_var_decls );
    public MPALParser.other_var_declarations_return other_var_declarations() // throws RecognitionException [1]
    {   
        MPALParser.other_var_declarations_return retval = new MPALParser.other_var_declarations_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.var_declarations_return var_declarations219 = default(MPALParser.var_declarations_return);

        MPALParser.temp_var_decls_return temp_var_decls220 = default(MPALParser.temp_var_decls_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:209:24: ( var_declarations | temp_var_decls )
            int alt46 = 2;
            int LA46_0 = input.LA(1);

            if ( (LA46_0 == VAR) )
            {
                alt46 = 1;
            }
            else if ( (LA46_0 == VAR_TEMP) )
            {
                alt46 = 2;
            }
            else 
            {
                NoViableAltException nvae_d46s0 =
                    new NoViableAltException("", 46, 0, input);

                throw nvae_d46s0;
            }
            switch (alt46) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:209:26: var_declarations
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_var_declarations_in_other_var_declarations2646);
                    	var_declarations219 = var_declarations();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, var_declarations219.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:209:45: temp_var_decls
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_temp_var_decls_in_other_var_declarations2650);
                    	temp_var_decls220 = temp_var_decls();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, temp_var_decls220.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "other_var_declarations"

    public class var_declarations_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "var_declarations"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:1: var_declarations : VAR ( CONSTANT )? var_init_decl ';' ( ( var_init_decl ';' ) )* 'END_VAR' ;
    public MPALParser.var_declarations_return var_declarations() // throws RecognitionException [1]
    {   
        MPALParser.var_declarations_return retval = new MPALParser.var_declarations_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken VAR221 = null;
        IToken CONSTANT222 = null;
        IToken char_literal224 = null;
        IToken char_literal226 = null;
        IToken string_literal227 = null;
        MPALParser.var_init_decl_return var_init_decl223 = default(MPALParser.var_init_decl_return);

        MPALParser.var_init_decl_return var_init_decl225 = default(MPALParser.var_init_decl_return);


        object VAR221_tree=null;
        object CONSTANT222_tree=null;
        object char_literal224_tree=null;
        object char_literal226_tree=null;
        object string_literal227_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:18: ( VAR ( CONSTANT )? var_init_decl ';' ( ( var_init_decl ';' ) )* 'END_VAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:20: VAR ( CONSTANT )? var_init_decl ';' ( ( var_init_decl ';' ) )* 'END_VAR'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	VAR221=(IToken)Match(input,VAR,FOLLOW_VAR_in_var_declarations2658); 
            		VAR221_tree = (object)adaptor.Create(VAR221);
            		root_0 = (object)adaptor.BecomeRoot(VAR221_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:25: ( CONSTANT )?
            	int alt47 = 2;
            	int LA47_0 = input.LA(1);

            	if ( (LA47_0 == CONSTANT) )
            	{
            	    alt47 = 1;
            	}
            	switch (alt47) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:26: CONSTANT
            	        {
            	        	CONSTANT222=(IToken)Match(input,CONSTANT,FOLLOW_CONSTANT_in_var_declarations2662); 
            	        		CONSTANT222_tree = (object)adaptor.Create(CONSTANT222);
            	        		adaptor.AddChild(root_0, CONSTANT222_tree);


            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_var_init_decl_in_var_declarations2666);
            	var_init_decl223 = var_init_decl();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, var_init_decl223.Tree);
            	char_literal224=(IToken)Match(input,108,FOLLOW_108_in_var_declarations2668); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:56: ( ( var_init_decl ';' ) )*
            	do 
            	{
            	    int alt48 = 2;
            	    int LA48_0 = input.LA(1);

            	    if ( (LA48_0 == IDENTIFIER) )
            	    {
            	        alt48 = 1;
            	    }


            	    switch (alt48) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:57: ( var_init_decl ';' )
            			    {
            			    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:57: ( var_init_decl ';' )
            			    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:211:58: var_init_decl ';'
            			    	{
            			    		PushFollow(FOLLOW_var_init_decl_in_var_declarations2673);
            			    		var_init_decl225 = var_init_decl();
            			    		state.followingStackPointer--;

            			    		adaptor.AddChild(root_0, var_init_decl225.Tree);
            			    		char_literal226=(IToken)Match(input,108,FOLLOW_108_in_var_declarations2675); 

            			    	}


            			    }
            			    break;

            			default:
            			    goto loop48;
            	    }
            	} while (true);

            	loop48:
            		;	// Stops C# compiler whining that label 'loop48' has no statements

            	string_literal227=(IToken)Match(input,111,FOLLOW_111_in_var_declarations2681); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "var_declarations"

    public class temp_var_decls_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "temp_var_decls"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:1: temp_var_decls : VAR_TEMP ( CONSTANT )? var_init_decl ';' ( ( var_init_decl ';' ) )* 'END_VAR' ;
    public MPALParser.temp_var_decls_return temp_var_decls() // throws RecognitionException [1]
    {   
        MPALParser.temp_var_decls_return retval = new MPALParser.temp_var_decls_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken VAR_TEMP228 = null;
        IToken CONSTANT229 = null;
        IToken char_literal231 = null;
        IToken char_literal233 = null;
        IToken string_literal234 = null;
        MPALParser.var_init_decl_return var_init_decl230 = default(MPALParser.var_init_decl_return);

        MPALParser.var_init_decl_return var_init_decl232 = default(MPALParser.var_init_decl_return);


        object VAR_TEMP228_tree=null;
        object CONSTANT229_tree=null;
        object char_literal231_tree=null;
        object char_literal233_tree=null;
        object string_literal234_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:16: ( VAR_TEMP ( CONSTANT )? var_init_decl ';' ( ( var_init_decl ';' ) )* 'END_VAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:18: VAR_TEMP ( CONSTANT )? var_init_decl ';' ( ( var_init_decl ';' ) )* 'END_VAR'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	VAR_TEMP228=(IToken)Match(input,VAR_TEMP,FOLLOW_VAR_TEMP_in_temp_var_decls2690); 
            		VAR_TEMP228_tree = (object)adaptor.Create(VAR_TEMP228);
            		root_0 = (object)adaptor.BecomeRoot(VAR_TEMP228_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:28: ( CONSTANT )?
            	int alt49 = 2;
            	int LA49_0 = input.LA(1);

            	if ( (LA49_0 == CONSTANT) )
            	{
            	    alt49 = 1;
            	}
            	switch (alt49) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:29: CONSTANT
            	        {
            	        	CONSTANT229=(IToken)Match(input,CONSTANT,FOLLOW_CONSTANT_in_temp_var_decls2694); 
            	        		CONSTANT229_tree = (object)adaptor.Create(CONSTANT229);
            	        		adaptor.AddChild(root_0, CONSTANT229_tree);


            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_var_init_decl_in_temp_var_decls2698);
            	var_init_decl230 = var_init_decl();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, var_init_decl230.Tree);
            	char_literal231=(IToken)Match(input,108,FOLLOW_108_in_temp_var_decls2700); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:59: ( ( var_init_decl ';' ) )*
            	do 
            	{
            	    int alt50 = 2;
            	    int LA50_0 = input.LA(1);

            	    if ( (LA50_0 == IDENTIFIER) )
            	    {
            	        alt50 = 1;
            	    }


            	    switch (alt50) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:60: ( var_init_decl ';' )
            			    {
            			    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:60: ( var_init_decl ';' )
            			    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:213:61: var_init_decl ';'
            			    	{
            			    		PushFollow(FOLLOW_var_init_decl_in_temp_var_decls2705);
            			    		var_init_decl232 = var_init_decl();
            			    		state.followingStackPointer--;

            			    		adaptor.AddChild(root_0, var_init_decl232.Tree);
            			    		char_literal233=(IToken)Match(input,108,FOLLOW_108_in_temp_var_decls2707); 

            			    	}


            			    }
            			    break;

            			default:
            			    goto loop50;
            	    }
            	} while (true);

            	loop50:
            		;	// Stops C# compiler whining that label 'loop50' has no statements

            	string_literal234=(IToken)Match(input,111,FOLLOW_111_in_temp_var_decls2713); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "temp_var_decls"

    public class function_body_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "function_body"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:215:1: function_body : statement_list ;
    public MPALParser.function_body_return function_body() // throws RecognitionException [1]
    {   
        MPALParser.function_body_return retval = new MPALParser.function_body_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.statement_list_return statement_list235 = default(MPALParser.statement_list_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:215:15: ( statement_list )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:215:17: statement_list
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_statement_list_in_function_body2722);
            	statement_list235 = statement_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, statement_list235.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "function_body"

    public class statement_list_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "statement_list"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:217:1: statement_list : ( statement ';' )+ ;
    public MPALParser.statement_list_return statement_list() // throws RecognitionException [1]
    {   
        MPALParser.statement_list_return retval = new MPALParser.statement_list_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal237 = null;
        MPALParser.statement_return statement236 = default(MPALParser.statement_return);


        object char_literal237_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:217:16: ( ( statement ';' )+ )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:217:18: ( statement ';' )+
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:217:18: ( statement ';' )+
            	int cnt51 = 0;
            	do 
            	{
            	    int alt51 = 2;
            	    alt51 = dfa51.Predict(input);
            	    switch (alt51) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:217:19: statement ';'
            			    {
            			    	PushFollow(FOLLOW_statement_in_statement_list2732);
            			    	statement236 = statement();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, statement236.Tree);
            			    	char_literal237=(IToken)Match(input,108,FOLLOW_108_in_statement_list2734); 

            			    }
            			    break;

            			default:
            			    if ( cnt51 >= 1 ) goto loop51;
            		            EarlyExitException eee51 =
            		                new EarlyExitException(51, input);
            		            throw eee51;
            	    }
            	    cnt51++;
            	} while (true);

            	loop51:
            		;	// Stops C# compiler whining that label 'loop51' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "statement_list"

    public class do_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "do_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:219:1: do_statement : DO statement_list ;
    public MPALParser.do_statement_return do_statement() // throws RecognitionException [1]
    {   
        MPALParser.do_statement_return retval = new MPALParser.do_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken DO238 = null;
        MPALParser.statement_list_return statement_list239 = default(MPALParser.statement_list_return);


        object DO238_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:219:14: ( DO statement_list )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:219:16: DO statement_list
            {
            	root_0 = (object)adaptor.GetNilNode();

            	DO238=(IToken)Match(input,DO,FOLLOW_DO_in_do_statement2745); 
            		DO238_tree = (object)adaptor.Create(DO238);
            		root_0 = (object)adaptor.BecomeRoot(DO238_tree, root_0);

            	PushFollow(FOLLOW_statement_list_in_do_statement2748);
            	statement_list239 = statement_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, statement_list239.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "do_statement"

    public class for_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "for_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:221:1: for_statement : FOR for_assign for_to ( for_by )? do_statement 'END_FOR' ;
    public MPALParser.for_statement_return for_statement() // throws RecognitionException [1]
    {   
        MPALParser.for_statement_return retval = new MPALParser.for_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken FOR240 = null;
        IToken string_literal245 = null;
        MPALParser.for_assign_return for_assign241 = default(MPALParser.for_assign_return);

        MPALParser.for_to_return for_to242 = default(MPALParser.for_to_return);

        MPALParser.for_by_return for_by243 = default(MPALParser.for_by_return);

        MPALParser.do_statement_return do_statement244 = default(MPALParser.do_statement_return);


        object FOR240_tree=null;
        object string_literal245_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:221:15: ( FOR for_assign for_to ( for_by )? do_statement 'END_FOR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:221:17: FOR for_assign for_to ( for_by )? do_statement 'END_FOR'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	FOR240=(IToken)Match(input,FOR,FOLLOW_FOR_in_for_statement2756); 
            		FOR240_tree = (object)adaptor.Create(FOR240);
            		root_0 = (object)adaptor.BecomeRoot(FOR240_tree, root_0);

            	PushFollow(FOLLOW_for_assign_in_for_statement2759);
            	for_assign241 = for_assign();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, for_assign241.Tree);
            	PushFollow(FOLLOW_for_to_in_for_statement2762);
            	for_to242 = for_to();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, for_to242.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:221:41: ( for_by )?
            	int alt52 = 2;
            	int LA52_0 = input.LA(1);

            	if ( (LA52_0 == BY) )
            	{
            	    alt52 = 1;
            	}
            	switch (alt52) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:221:42: for_by
            	        {
            	        	PushFollow(FOLLOW_for_by_in_for_statement2765);
            	        	for_by243 = for_by();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, for_by243.Tree);

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_do_statement_in_for_statement2769);
            	do_statement244 = do_statement();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, do_statement244.Tree);
            	string_literal245=(IToken)Match(input,112,FOLLOW_112_in_for_statement2771); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "for_statement"

    public class for_assign_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "for_assign"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:223:1: for_assign : IDENTIFIER ':=' expression ;
    public MPALParser.for_assign_return for_assign() // throws RecognitionException [1]
    {   
        MPALParser.for_assign_return retval = new MPALParser.for_assign_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER246 = null;
        IToken string_literal247 = null;
        MPALParser.expression_return expression248 = default(MPALParser.expression_return);


        object IDENTIFIER246_tree=null;
        object string_literal247_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:223:12: ( IDENTIFIER ':=' expression )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:223:14: IDENTIFIER ':=' expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER246=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_for_assign2780); 
            		IDENTIFIER246_tree = (object)adaptor.Create(IDENTIFIER246);
            		adaptor.AddChild(root_0, IDENTIFIER246_tree);

            	string_literal247=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_for_assign2782); 
            		string_literal247_tree = (object)adaptor.Create(string_literal247);
            		root_0 = (object)adaptor.BecomeRoot(string_literal247_tree, root_0);

            	PushFollow(FOLLOW_expression_in_for_assign2785);
            	expression248 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression248.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "for_assign"

    public class for_to_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "for_to"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:225:1: for_to : TO expression ;
    public MPALParser.for_to_return for_to() // throws RecognitionException [1]
    {   
        MPALParser.for_to_return retval = new MPALParser.for_to_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken TO249 = null;
        MPALParser.expression_return expression250 = default(MPALParser.expression_return);


        object TO249_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:225:8: ( TO expression )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:225:10: TO expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	TO249=(IToken)Match(input,TO,FOLLOW_TO_in_for_to2793); 
            		TO249_tree = (object)adaptor.Create(TO249);
            		root_0 = (object)adaptor.BecomeRoot(TO249_tree, root_0);

            	PushFollow(FOLLOW_expression_in_for_to2796);
            	expression250 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression250.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "for_to"

    public class for_by_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "for_by"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:227:1: for_by : BY expression ;
    public MPALParser.for_by_return for_by() // throws RecognitionException [1]
    {   
        MPALParser.for_by_return retval = new MPALParser.for_by_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken BY251 = null;
        MPALParser.expression_return expression252 = default(MPALParser.expression_return);


        object BY251_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:227:9: ( BY expression )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:227:11: BY expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	BY251=(IToken)Match(input,BY,FOLLOW_BY_in_for_by2805); 
            		BY251_tree = (object)adaptor.Create(BY251);
            		root_0 = (object)adaptor.BecomeRoot(BY251_tree, root_0);

            	PushFollow(FOLLOW_expression_in_for_by2808);
            	expression252 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression252.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "for_by"

    public class while_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "while_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:229:1: while_statement : WHILE expression do_statement 'END_WHILE' ;
    public MPALParser.while_statement_return while_statement() // throws RecognitionException [1]
    {   
        MPALParser.while_statement_return retval = new MPALParser.while_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken WHILE253 = null;
        IToken string_literal256 = null;
        MPALParser.expression_return expression254 = default(MPALParser.expression_return);

        MPALParser.do_statement_return do_statement255 = default(MPALParser.do_statement_return);


        object WHILE253_tree=null;
        object string_literal256_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:229:17: ( WHILE expression do_statement 'END_WHILE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:229:19: WHILE expression do_statement 'END_WHILE'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	WHILE253=(IToken)Match(input,WHILE,FOLLOW_WHILE_in_while_statement2816); 
            		WHILE253_tree = (object)adaptor.Create(WHILE253);
            		root_0 = (object)adaptor.BecomeRoot(WHILE253_tree, root_0);

            	PushFollow(FOLLOW_expression_in_while_statement2819);
            	expression254 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression254.Tree);
            	PushFollow(FOLLOW_do_statement_in_while_statement2821);
            	do_statement255 = do_statement();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, do_statement255.Tree);
            	string_literal256=(IToken)Match(input,113,FOLLOW_113_in_while_statement2823); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "while_statement"

    public class repeat_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "repeat_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:231:1: repeat_statement : REPEAT statement_list repeat_until 'END_REPEAT' ;
    public MPALParser.repeat_statement_return repeat_statement() // throws RecognitionException [1]
    {   
        MPALParser.repeat_statement_return retval = new MPALParser.repeat_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken REPEAT257 = null;
        IToken string_literal260 = null;
        MPALParser.statement_list_return statement_list258 = default(MPALParser.statement_list_return);

        MPALParser.repeat_until_return repeat_until259 = default(MPALParser.repeat_until_return);


        object REPEAT257_tree=null;
        object string_literal260_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:231:18: ( REPEAT statement_list repeat_until 'END_REPEAT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:231:20: REPEAT statement_list repeat_until 'END_REPEAT'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	REPEAT257=(IToken)Match(input,REPEAT,FOLLOW_REPEAT_in_repeat_statement2832); 
            		REPEAT257_tree = (object)adaptor.Create(REPEAT257);
            		root_0 = (object)adaptor.BecomeRoot(REPEAT257_tree, root_0);

            	PushFollow(FOLLOW_statement_list_in_repeat_statement2835);
            	statement_list258 = statement_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, statement_list258.Tree);
            	PushFollow(FOLLOW_repeat_until_in_repeat_statement2837);
            	repeat_until259 = repeat_until();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, repeat_until259.Tree);
            	string_literal260=(IToken)Match(input,114,FOLLOW_114_in_repeat_statement2839); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "repeat_statement"

    public class repeat_until_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "repeat_until"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:233:1: repeat_until : UNTIL expression ;
    public MPALParser.repeat_until_return repeat_until() // throws RecognitionException [1]
    {   
        MPALParser.repeat_until_return retval = new MPALParser.repeat_until_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken UNTIL261 = null;
        MPALParser.expression_return expression262 = default(MPALParser.expression_return);


        object UNTIL261_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:233:14: ( UNTIL expression )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:233:16: UNTIL expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	UNTIL261=(IToken)Match(input,UNTIL,FOLLOW_UNTIL_in_repeat_until2848); 
            		UNTIL261_tree = (object)adaptor.Create(UNTIL261);
            		root_0 = (object)adaptor.BecomeRoot(UNTIL261_tree, root_0);

            	PushFollow(FOLLOW_expression_in_repeat_until2851);
            	expression262 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression262.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "repeat_until"

    public class iteration_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "iteration_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:235:1: iteration_statement : ( for_statement | while_statement | repeat_statement | EXIT | CONTINUE );
    public MPALParser.iteration_statement_return iteration_statement() // throws RecognitionException [1]
    {   
        MPALParser.iteration_statement_return retval = new MPALParser.iteration_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken EXIT266 = null;
        IToken CONTINUE267 = null;
        MPALParser.for_statement_return for_statement263 = default(MPALParser.for_statement_return);

        MPALParser.while_statement_return while_statement264 = default(MPALParser.while_statement_return);

        MPALParser.repeat_statement_return repeat_statement265 = default(MPALParser.repeat_statement_return);


        object EXIT266_tree=null;
        object CONTINUE267_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:235:21: ( for_statement | while_statement | repeat_statement | EXIT | CONTINUE )
            int alt53 = 5;
            switch ( input.LA(1) ) 
            {
            case FOR:
            	{
                alt53 = 1;
                }
                break;
            case WHILE:
            	{
                alt53 = 2;
                }
                break;
            case REPEAT:
            	{
                alt53 = 3;
                }
                break;
            case EXIT:
            	{
                alt53 = 4;
                }
                break;
            case CONTINUE:
            	{
                alt53 = 5;
                }
                break;
            	default:
            	    NoViableAltException nvae_d53s0 =
            	        new NoViableAltException("", 53, 0, input);

            	    throw nvae_d53s0;
            }

            switch (alt53) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:235:23: for_statement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_for_statement_in_iteration_statement2859);
                    	for_statement263 = for_statement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, for_statement263.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:235:40: while_statement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_while_statement_in_iteration_statement2864);
                    	while_statement264 = while_statement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, while_statement264.Tree);

                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:235:58: repeat_statement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_repeat_statement_in_iteration_statement2868);
                    	repeat_statement265 = repeat_statement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, repeat_statement265.Tree);

                    }
                    break;
                case 4 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:235:77: EXIT
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	EXIT266=(IToken)Match(input,EXIT,FOLLOW_EXIT_in_iteration_statement2872); 
                    		EXIT266_tree = (object)adaptor.Create(EXIT266);
                    		adaptor.AddChild(root_0, EXIT266_tree);


                    }
                    break;
                case 5 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:235:83: CONTINUE
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	CONTINUE267=(IToken)Match(input,CONTINUE,FOLLOW_CONTINUE_in_iteration_statement2875); 
                    		CONTINUE267_tree = (object)adaptor.Create(CONTINUE267);
                    		adaptor.AddChild(root_0, CONTINUE267_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "iteration_statement"

    public class statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:237:1: statement : ( ';' | fb_invoke_or_assigment | selection_statement | iteration_statement | RETURN );
    public MPALParser.statement_return statement() // throws RecognitionException [1]
    {   
        MPALParser.statement_return retval = new MPALParser.statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal268 = null;
        IToken RETURN272 = null;
        MPALParser.fb_invoke_or_assigment_return fb_invoke_or_assigment269 = default(MPALParser.fb_invoke_or_assigment_return);

        MPALParser.selection_statement_return selection_statement270 = default(MPALParser.selection_statement_return);

        MPALParser.iteration_statement_return iteration_statement271 = default(MPALParser.iteration_statement_return);


        object char_literal268_tree=null;
        object RETURN272_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:237:11: ( ';' | fb_invoke_or_assigment | selection_statement | iteration_statement | RETURN )
            int alt54 = 5;
            alt54 = dfa54.Predict(input);
            switch (alt54) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:237:13: ';'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal268=(IToken)Match(input,108,FOLLOW_108_in_statement2883); 
                    		char_literal268_tree = (object)adaptor.Create(char_literal268);
                    		adaptor.AddChild(root_0, char_literal268_tree);


                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:237:19: fb_invoke_or_assigment
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_fb_invoke_or_assigment_in_statement2887);
                    	fb_invoke_or_assigment269 = fb_invoke_or_assigment();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, fb_invoke_or_assigment269.Tree);

                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:237:44: selection_statement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_selection_statement_in_statement2891);
                    	selection_statement270 = selection_statement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, selection_statement270.Tree);

                    }
                    break;
                case 4 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:237:66: iteration_statement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_iteration_statement_in_statement2895);
                    	iteration_statement271 = iteration_statement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, iteration_statement271.Tree);

                    }
                    break;
                case 5 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:237:87: RETURN
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	RETURN272=(IToken)Match(input,RETURN,FOLLOW_RETURN_in_statement2898); 
                    		RETURN272_tree = (object)adaptor.Create(RETURN272);
                    		adaptor.AddChild(root_0, RETURN272_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "statement"

    public class fb_invoke_or_assigment_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "fb_invoke_or_assigment"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:1: fb_invoke_or_assigment : variable ( ASSIGN expression | ( '(' ( param_assignment ( COMMA param_assignment )* )? ')' ) ) ;
    public MPALParser.fb_invoke_or_assigment_return fb_invoke_or_assigment() // throws RecognitionException [1]
    {   
        MPALParser.fb_invoke_or_assigment_return retval = new MPALParser.fb_invoke_or_assigment_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ASSIGN274 = null;
        IToken char_literal276 = null;
        IToken COMMA278 = null;
        IToken char_literal280 = null;
        MPALParser.variable_return variable273 = default(MPALParser.variable_return);

        MPALParser.expression_return expression275 = default(MPALParser.expression_return);

        MPALParser.param_assignment_return param_assignment277 = default(MPALParser.param_assignment_return);

        MPALParser.param_assignment_return param_assignment279 = default(MPALParser.param_assignment_return);


        object ASSIGN274_tree=null;
        object char_literal276_tree=null;
        object COMMA278_tree=null;
        object char_literal280_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:24: ( variable ( ASSIGN expression | ( '(' ( param_assignment ( COMMA param_assignment )* )? ')' ) ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:26: variable ( ASSIGN expression | ( '(' ( param_assignment ( COMMA param_assignment )* )? ')' ) )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_variable_in_fb_invoke_or_assigment2906);
            	variable273 = variable();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, variable273.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:35: ( ASSIGN expression | ( '(' ( param_assignment ( COMMA param_assignment )* )? ')' ) )
            	int alt57 = 2;
            	int LA57_0 = input.LA(1);

            	if ( (LA57_0 == ASSIGN) )
            	{
            	    alt57 = 1;
            	}
            	else if ( (LA57_0 == LRBRACKED) )
            	{
            	    alt57 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d57s0 =
            	        new NoViableAltException("", 57, 0, input);

            	    throw nvae_d57s0;
            	}
            	switch (alt57) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:36: ASSIGN expression
            	        {
            	        	ASSIGN274=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_fb_invoke_or_assigment2909); 
            	        		ASSIGN274_tree = (object)adaptor.Create(ASSIGN274);
            	        		root_0 = (object)adaptor.BecomeRoot(ASSIGN274_tree, root_0);

            	        	PushFollow(FOLLOW_expression_in_fb_invoke_or_assigment2912);
            	        	expression275 = expression();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, expression275.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:57: ( '(' ( param_assignment ( COMMA param_assignment )* )? ')' )
            	        {
            	        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:57: ( '(' ( param_assignment ( COMMA param_assignment )* )? ')' )
            	        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:58: '(' ( param_assignment ( COMMA param_assignment )* )? ')'
            	        	{
            	        		char_literal276=(IToken)Match(input,LRBRACKED,FOLLOW_LRBRACKED_in_fb_invoke_or_assigment2917); 
            	        			char_literal276_tree = (object)adaptor.Create(char_literal276);
            	        			root_0 = (object)adaptor.BecomeRoot(char_literal276_tree, root_0);

            	        		// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:63: ( param_assignment ( COMMA param_assignment )* )?
            	        		int alt56 = 2;
            	        		alt56 = dfa56.Predict(input);
            	        		switch (alt56) 
            	        		{
            	        		    case 1 :
            	        		        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:64: param_assignment ( COMMA param_assignment )*
            	        		        {
            	        		        	PushFollow(FOLLOW_param_assignment_in_fb_invoke_or_assigment2921);
            	        		        	param_assignment277 = param_assignment();
            	        		        	state.followingStackPointer--;

            	        		        	adaptor.AddChild(root_0, param_assignment277.Tree);
            	        		        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:81: ( COMMA param_assignment )*
            	        		        	do 
            	        		        	{
            	        		        	    int alt55 = 2;
            	        		        	    int LA55_0 = input.LA(1);

            	        		        	    if ( (LA55_0 == COMMA) )
            	        		        	    {
            	        		        	        alt55 = 1;
            	        		        	    }


            	        		        	    switch (alt55) 
            	        		        		{
            	        		        			case 1 :
            	        		        			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:239:82: COMMA param_assignment
            	        		        			    {
            	        		        			    	COMMA278=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_fb_invoke_or_assigment2924); 
            	        		        			    	PushFollow(FOLLOW_param_assignment_in_fb_invoke_or_assigment2927);
            	        		        			    	param_assignment279 = param_assignment();
            	        		        			    	state.followingStackPointer--;

            	        		        			    	adaptor.AddChild(root_0, param_assignment279.Tree);

            	        		        			    }
            	        		        			    break;

            	        		        			default:
            	        		        			    goto loop55;
            	        		        	    }
            	        		        	} while (true);

            	        		        	loop55:
            	        		        		;	// Stops C# compiler whining that label 'loop55' has no statements


            	        		        }
            	        		        break;

            	        		}

            	        		char_literal280=(IToken)Match(input,107,FOLLOW_107_in_fb_invoke_or_assigment2933); 

            	        	}


            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "fb_invoke_or_assigment"

    public class selection_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "selection_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:243:1: selection_statement : ( if_statement | case_statement );
    public MPALParser.selection_statement_return selection_statement() // throws RecognitionException [1]
    {   
        MPALParser.selection_statement_return retval = new MPALParser.selection_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.if_statement_return if_statement281 = default(MPALParser.if_statement_return);

        MPALParser.case_statement_return case_statement282 = default(MPALParser.case_statement_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:243:21: ( if_statement | case_statement )
            int alt58 = 2;
            int LA58_0 = input.LA(1);

            if ( (LA58_0 == IF) )
            {
                alt58 = 1;
            }
            else if ( (LA58_0 == CASE) )
            {
                alt58 = 2;
            }
            else 
            {
                NoViableAltException nvae_d58s0 =
                    new NoViableAltException("", 58, 0, input);

                throw nvae_d58s0;
            }
            switch (alt58) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:243:23: if_statement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_if_statement_in_selection_statement2947);
                    	if_statement281 = if_statement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, if_statement281.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:243:38: case_statement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_case_statement_in_selection_statement2951);
                    	case_statement282 = case_statement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, case_statement282.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "selection_statement"

    public class if_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "if_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:245:1: if_statement : IF expression if_then ( if_elif )* ( else_statement )? 'END_IF' ;
    public MPALParser.if_statement_return if_statement() // throws RecognitionException [1]
    {   
        MPALParser.if_statement_return retval = new MPALParser.if_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IF283 = null;
        IToken string_literal288 = null;
        MPALParser.expression_return expression284 = default(MPALParser.expression_return);

        MPALParser.if_then_return if_then285 = default(MPALParser.if_then_return);

        MPALParser.if_elif_return if_elif286 = default(MPALParser.if_elif_return);

        MPALParser.else_statement_return else_statement287 = default(MPALParser.else_statement_return);


        object IF283_tree=null;
        object string_literal288_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:245:14: ( IF expression if_then ( if_elif )* ( else_statement )? 'END_IF' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:245:16: IF expression if_then ( if_elif )* ( else_statement )? 'END_IF'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IF283=(IToken)Match(input,IF,FOLLOW_IF_in_if_statement2959); 
            		IF283_tree = (object)adaptor.Create(IF283);
            		root_0 = (object)adaptor.BecomeRoot(IF283_tree, root_0);

            	PushFollow(FOLLOW_expression_in_if_statement2962);
            	expression284 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression284.Tree);
            	PushFollow(FOLLOW_if_then_in_if_statement2964);
            	if_then285 = if_then();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, if_then285.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:245:39: ( if_elif )*
            	do 
            	{
            	    int alt59 = 2;
            	    int LA59_0 = input.LA(1);

            	    if ( (LA59_0 == ELSIF) )
            	    {
            	        alt59 = 1;
            	    }


            	    switch (alt59) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:245:40: if_elif
            			    {
            			    	PushFollow(FOLLOW_if_elif_in_if_statement2967);
            			    	if_elif286 = if_elif();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, if_elif286.Tree);

            			    }
            			    break;

            			default:
            			    goto loop59;
            	    }
            	} while (true);

            	loop59:
            		;	// Stops C# compiler whining that label 'loop59' has no statements

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:245:50: ( else_statement )?
            	int alt60 = 2;
            	int LA60_0 = input.LA(1);

            	if ( (LA60_0 == ELSE) )
            	{
            	    alt60 = 1;
            	}
            	switch (alt60) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:245:51: else_statement
            	        {
            	        	PushFollow(FOLLOW_else_statement_in_if_statement2972);
            	        	else_statement287 = else_statement();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, else_statement287.Tree);

            	        }
            	        break;

            	}

            	string_literal288=(IToken)Match(input,115,FOLLOW_115_in_if_statement2976); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "if_statement"

    public class if_then_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "if_then"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:247:1: if_then : THEN statement_list ;
    public MPALParser.if_then_return if_then() // throws RecognitionException [1]
    {   
        MPALParser.if_then_return retval = new MPALParser.if_then_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken THEN289 = null;
        MPALParser.statement_list_return statement_list290 = default(MPALParser.statement_list_return);


        object THEN289_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:247:9: ( THEN statement_list )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:247:11: THEN statement_list
            {
            	root_0 = (object)adaptor.GetNilNode();

            	THEN289=(IToken)Match(input,THEN,FOLLOW_THEN_in_if_then2993); 
            		THEN289_tree = (object)adaptor.Create(THEN289);
            		root_0 = (object)adaptor.BecomeRoot(THEN289_tree, root_0);

            	PushFollow(FOLLOW_statement_list_in_if_then2996);
            	statement_list290 = statement_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, statement_list290.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "if_then"

    public class if_elif_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "if_elif"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:249:1: if_elif : ELSIF expression if_then ;
    public MPALParser.if_elif_return if_elif() // throws RecognitionException [1]
    {   
        MPALParser.if_elif_return retval = new MPALParser.if_elif_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ELSIF291 = null;
        MPALParser.expression_return expression292 = default(MPALParser.expression_return);

        MPALParser.if_then_return if_then293 = default(MPALParser.if_then_return);


        object ELSIF291_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:249:9: ( ELSIF expression if_then )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:249:11: ELSIF expression if_then
            {
            	root_0 = (object)adaptor.GetNilNode();

            	ELSIF291=(IToken)Match(input,ELSIF,FOLLOW_ELSIF_in_if_elif3004); 
            		ELSIF291_tree = (object)adaptor.Create(ELSIF291);
            		root_0 = (object)adaptor.BecomeRoot(ELSIF291_tree, root_0);

            	PushFollow(FOLLOW_expression_in_if_elif3007);
            	expression292 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression292.Tree);
            	PushFollow(FOLLOW_if_then_in_if_elif3009);
            	if_then293 = if_then();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, if_then293.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "if_elif"

    public class else_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "else_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:251:1: else_statement : ELSE ( ( ':' )? ) statement_list ;
    public MPALParser.else_statement_return else_statement() // throws RecognitionException [1]
    {   
        MPALParser.else_statement_return retval = new MPALParser.else_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ELSE294 = null;
        IToken char_literal295 = null;
        MPALParser.statement_list_return statement_list296 = default(MPALParser.statement_list_return);


        object ELSE294_tree=null;
        object char_literal295_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:251:16: ( ELSE ( ( ':' )? ) statement_list )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:251:18: ELSE ( ( ':' )? ) statement_list
            {
            	root_0 = (object)adaptor.GetNilNode();

            	ELSE294=(IToken)Match(input,ELSE,FOLLOW_ELSE_in_else_statement3017); 
            		ELSE294_tree = (object)adaptor.Create(ELSE294);
            		root_0 = (object)adaptor.BecomeRoot(ELSE294_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:251:24: ( ( ':' )? )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:251:25: ( ':' )?
            	{
            		// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:251:25: ( ':' )?
            		int alt61 = 2;
            		alt61 = dfa61.Predict(input);
            		switch (alt61) 
            		{
            		    case 1 :
            		        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:251:25: ':'
            		        {
            		        	char_literal295=(IToken)Match(input,COLON,FOLLOW_COLON_in_else_statement3021); 
            		        		char_literal295_tree = (object)adaptor.Create(char_literal295);
            		        		adaptor.AddChild(root_0, char_literal295_tree);


            		        }
            		        break;

            		}


            	}

            	PushFollow(FOLLOW_statement_list_in_else_statement3026);
            	statement_list296 = statement_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, statement_list296.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "else_statement"

    public class case_statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "case_statement"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:253:1: case_statement : CASE expression case_of ( else_statement )? 'END_CASE' ;
    public MPALParser.case_statement_return case_statement() // throws RecognitionException [1]
    {   
        MPALParser.case_statement_return retval = new MPALParser.case_statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken CASE297 = null;
        IToken string_literal301 = null;
        MPALParser.expression_return expression298 = default(MPALParser.expression_return);

        MPALParser.case_of_return case_of299 = default(MPALParser.case_of_return);

        MPALParser.else_statement_return else_statement300 = default(MPALParser.else_statement_return);


        object CASE297_tree=null;
        object string_literal301_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:253:16: ( CASE expression case_of ( else_statement )? 'END_CASE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:253:18: CASE expression case_of ( else_statement )? 'END_CASE'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	CASE297=(IToken)Match(input,CASE,FOLLOW_CASE_in_case_statement3035); 
            		CASE297_tree = (object)adaptor.Create(CASE297);
            		root_0 = (object)adaptor.BecomeRoot(CASE297_tree, root_0);

            	PushFollow(FOLLOW_expression_in_case_statement3038);
            	expression298 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression298.Tree);
            	PushFollow(FOLLOW_case_of_in_case_statement3040);
            	case_of299 = case_of();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, case_of299.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:253:43: ( else_statement )?
            	int alt62 = 2;
            	int LA62_0 = input.LA(1);

            	if ( (LA62_0 == ELSE) )
            	{
            	    alt62 = 1;
            	}
            	switch (alt62) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:253:43: else_statement
            	        {
            	        	PushFollow(FOLLOW_else_statement_in_case_statement3042);
            	        	else_statement300 = else_statement();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, else_statement300.Tree);

            	        }
            	        break;

            	}

            	string_literal301=(IToken)Match(input,116,FOLLOW_116_in_case_statement3045); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "case_statement"

    public class case_of_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "case_of"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:255:1: case_of : OF ( case_element )+ ;
    public MPALParser.case_of_return case_of() // throws RecognitionException [1]
    {   
        MPALParser.case_of_return retval = new MPALParser.case_of_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken OF302 = null;
        MPALParser.case_element_return case_element303 = default(MPALParser.case_element_return);


        object OF302_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:255:9: ( OF ( case_element )+ )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:255:11: OF ( case_element )+
            {
            	root_0 = (object)adaptor.GetNilNode();

            	OF302=(IToken)Match(input,OF,FOLLOW_OF_in_case_of3054); 
            		OF302_tree = (object)adaptor.Create(OF302);
            		root_0 = (object)adaptor.BecomeRoot(OF302_tree, root_0);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:255:15: ( case_element )+
            	int cnt63 = 0;
            	do 
            	{
            	    int alt63 = 2;
            	    alt63 = dfa63.Predict(input);
            	    switch (alt63) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:255:16: case_element
            			    {
            			    	PushFollow(FOLLOW_case_element_in_case_of3058);
            			    	case_element303 = case_element();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, case_element303.Tree);

            			    }
            			    break;

            			default:
            			    if ( cnt63 >= 1 ) goto loop63;
            		            EarlyExitException eee63 =
            		                new EarlyExitException(63, input);
            		            throw eee63;
            	    }
            	    cnt63++;
            	} while (true);

            	loop63:
            		;	// Stops C# compiler whining that label 'loop63' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "case_of"

    public class case_element_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "case_element"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:257:1: case_element : case_list ':' statement_list ;
    public MPALParser.case_element_return case_element() // throws RecognitionException [1]
    {   
        MPALParser.case_element_return retval = new MPALParser.case_element_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal305 = null;
        MPALParser.case_list_return case_list304 = default(MPALParser.case_list_return);

        MPALParser.statement_list_return statement_list306 = default(MPALParser.statement_list_return);


        object char_literal305_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:257:14: ( case_list ':' statement_list )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:257:16: case_list ':' statement_list
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_case_list_in_case_element3068);
            	case_list304 = case_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, case_list304.Tree);
            	char_literal305=(IToken)Match(input,COLON,FOLLOW_COLON_in_case_element3070); 
            		char_literal305_tree = (object)adaptor.Create(char_literal305);
            		root_0 = (object)adaptor.BecomeRoot(char_literal305_tree, root_0);

            	PushFollow(FOLLOW_statement_list_in_case_element3073);
            	statement_list306 = statement_list();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, statement_list306.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "case_element"

    public class case_list_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "case_list"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:259:1: case_list : case_list_element ( COMMA case_list_element )* ;
    public MPALParser.case_list_return case_list() // throws RecognitionException [1]
    {   
        MPALParser.case_list_return retval = new MPALParser.case_list_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken COMMA308 = null;
        MPALParser.case_list_element_return case_list_element307 = default(MPALParser.case_list_element_return);

        MPALParser.case_list_element_return case_list_element309 = default(MPALParser.case_list_element_return);


        object COMMA308_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:259:11: ( case_list_element ( COMMA case_list_element )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:259:13: case_list_element ( COMMA case_list_element )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_case_list_element_in_case_list3081);
            	case_list_element307 = case_list_element();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, case_list_element307.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:259:31: ( COMMA case_list_element )*
            	do 
            	{
            	    int alt64 = 2;
            	    int LA64_0 = input.LA(1);

            	    if ( (LA64_0 == COMMA) )
            	    {
            	        alt64 = 1;
            	    }


            	    switch (alt64) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:259:32: COMMA case_list_element
            			    {
            			    	COMMA308=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_case_list3084); 
            			    		COMMA308_tree = (object)adaptor.Create(COMMA308);
            			    		root_0 = (object)adaptor.BecomeRoot(COMMA308_tree, root_0);

            			    	PushFollow(FOLLOW_case_list_element_in_case_list3087);
            			    	case_list_element309 = case_list_element();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, case_list_element309.Tree);

            			    }
            			    break;

            			default:
            			    goto loop64;
            	    }
            	} while (true);

            	loop64:
            		;	// Stops C# compiler whining that label 'loop64' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "case_list"

    public class case_list_element_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "case_list_element"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:261:1: case_list_element : ( number_literal | subrange | enumerated_value | IDENTIFIER );
    public MPALParser.case_list_element_return case_list_element() // throws RecognitionException [1]
    {   
        MPALParser.case_list_element_return retval = new MPALParser.case_list_element_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER313 = null;
        MPALParser.number_literal_return number_literal310 = default(MPALParser.number_literal_return);

        MPALParser.subrange_return subrange311 = default(MPALParser.subrange_return);

        MPALParser.enumerated_value_return enumerated_value312 = default(MPALParser.enumerated_value_return);


        object IDENTIFIER313_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:261:19: ( number_literal | subrange | enumerated_value | IDENTIFIER )
            int alt65 = 4;
            alt65 = dfa65.Predict(input);
            switch (alt65) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:261:21: number_literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_number_literal_in_case_list_element3097);
                    	number_literal310 = number_literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, number_literal310.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:261:37: subrange
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_subrange_in_case_list_element3100);
                    	subrange311 = subrange();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, subrange311.Tree);

                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:261:49: enumerated_value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_enumerated_value_in_case_list_element3105);
                    	enumerated_value312 = enumerated_value();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, enumerated_value312.Tree);

                    }
                    break;
                case 4 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:261:68: IDENTIFIER
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	IDENTIFIER313=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_case_list_element3109); 
                    		IDENTIFIER313_tree = (object)adaptor.Create(IDENTIFIER313);
                    		adaptor.AddChild(root_0, IDENTIFIER313_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "case_list_element"

    public class param_assignment_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "param_assignment"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:1: param_assignment : ( ( ( IDENTIFIER ASSIGN )? expression ) | ( ( NOT )? IDENTIFIER ASSIGN2 variable ) );
    public MPALParser.param_assignment_return param_assignment() // throws RecognitionException [1]
    {   
        MPALParser.param_assignment_return retval = new MPALParser.param_assignment_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER314 = null;
        IToken ASSIGN315 = null;
        IToken NOT317 = null;
        IToken IDENTIFIER318 = null;
        IToken ASSIGN2319 = null;
        MPALParser.expression_return expression316 = default(MPALParser.expression_return);

        MPALParser.variable_return variable320 = default(MPALParser.variable_return);


        object IDENTIFIER314_tree=null;
        object ASSIGN315_tree=null;
        object NOT317_tree=null;
        object IDENTIFIER318_tree=null;
        object ASSIGN2319_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:18: ( ( ( IDENTIFIER ASSIGN )? expression ) | ( ( NOT )? IDENTIFIER ASSIGN2 variable ) )
            int alt68 = 2;
            alt68 = dfa68.Predict(input);
            switch (alt68) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:20: ( ( IDENTIFIER ASSIGN )? expression )
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:20: ( ( IDENTIFIER ASSIGN )? expression )
                    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:21: ( IDENTIFIER ASSIGN )? expression
                    	{
                    		// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:21: ( IDENTIFIER ASSIGN )?
                    		int alt66 = 2;
                    		alt66 = dfa66.Predict(input);
                    		switch (alt66) 
                    		{
                    		    case 1 :
                    		        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:22: IDENTIFIER ASSIGN
                    		        {
                    		        	IDENTIFIER314=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_param_assignment3121); 
                    		        		IDENTIFIER314_tree = (object)adaptor.Create(IDENTIFIER314);
                    		        		adaptor.AddChild(root_0, IDENTIFIER314_tree);

                    		        	ASSIGN315=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_param_assignment3123); 
                    		        		ASSIGN315_tree = (object)adaptor.Create(ASSIGN315);
                    		        		root_0 = (object)adaptor.BecomeRoot(ASSIGN315_tree, root_0);


                    		        }
                    		        break;

                    		}

                    		PushFollow(FOLLOW_expression_in_param_assignment3128);
                    		expression316 = expression();
                    		state.followingStackPointer--;

                    		adaptor.AddChild(root_0, expression316.Tree);

                    	}


                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:57: ( ( NOT )? IDENTIFIER ASSIGN2 variable )
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:57: ( ( NOT )? IDENTIFIER ASSIGN2 variable )
                    	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:58: ( NOT )? IDENTIFIER ASSIGN2 variable
                    	{
                    		// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:58: ( NOT )?
                    		int alt67 = 2;
                    		int LA67_0 = input.LA(1);

                    		if ( (LA67_0 == NOT) )
                    		{
                    		    alt67 = 1;
                    		}
                    		switch (alt67) 
                    		{
                    		    case 1 :
                    		        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:264:58: NOT
                    		        {
                    		        	NOT317=(IToken)Match(input,NOT,FOLLOW_NOT_in_param_assignment3134); 
                    		        		NOT317_tree = (object)adaptor.Create(NOT317);
                    		        		adaptor.AddChild(root_0, NOT317_tree);


                    		        }
                    		        break;

                    		}

                    		IDENTIFIER318=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_param_assignment3137); 
                    			IDENTIFIER318_tree = (object)adaptor.Create(IDENTIFIER318);
                    			adaptor.AddChild(root_0, IDENTIFIER318_tree);

                    		ASSIGN2319=(IToken)Match(input,ASSIGN2,FOLLOW_ASSIGN2_in_param_assignment3139); 
                    			ASSIGN2319_tree = (object)adaptor.Create(ASSIGN2319);
                    			root_0 = (object)adaptor.BecomeRoot(ASSIGN2319_tree, root_0);

                    		PushFollow(FOLLOW_variable_in_param_assignment3142);
                    		variable320 = variable();
                    		state.followingStackPointer--;

                    		adaptor.AddChild(root_0, variable320.Tree);

                    	}


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "param_assignment"

    public class expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expression"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:266:1: expression : xor_expression ( OR xor_expression )* ;
    public MPALParser.expression_return expression() // throws RecognitionException [1]
    {   
        MPALParser.expression_return retval = new MPALParser.expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken OR322 = null;
        MPALParser.xor_expression_return xor_expression321 = default(MPALParser.xor_expression_return);

        MPALParser.xor_expression_return xor_expression323 = default(MPALParser.xor_expression_return);


        object OR322_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:266:12: ( xor_expression ( OR xor_expression )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:266:14: xor_expression ( OR xor_expression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_xor_expression_in_expression3152);
            	xor_expression321 = xor_expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, xor_expression321.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:266:29: ( OR xor_expression )*
            	do 
            	{
            	    int alt69 = 2;
            	    alt69 = dfa69.Predict(input);
            	    switch (alt69) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:266:30: OR xor_expression
            			    {
            			    	OR322=(IToken)Match(input,OR,FOLLOW_OR_in_expression3155); 
            			    		OR322_tree = (object)adaptor.Create(OR322);
            			    		root_0 = (object)adaptor.BecomeRoot(OR322_tree, root_0);

            			    	PushFollow(FOLLOW_xor_expression_in_expression3158);
            			    	xor_expression323 = xor_expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, xor_expression323.Tree);

            			    }
            			    break;

            			default:
            			    goto loop69;
            	    }
            	} while (true);

            	loop69:
            		;	// Stops C# compiler whining that label 'loop69' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "expression"

    public class xor_expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "xor_expression"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:268:1: xor_expression : and_expression ( XOR and_expression )* ;
    public MPALParser.xor_expression_return xor_expression() // throws RecognitionException [1]
    {   
        MPALParser.xor_expression_return retval = new MPALParser.xor_expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken XOR325 = null;
        MPALParser.and_expression_return and_expression324 = default(MPALParser.and_expression_return);

        MPALParser.and_expression_return and_expression326 = default(MPALParser.and_expression_return);


        object XOR325_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:268:16: ( and_expression ( XOR and_expression )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:268:18: and_expression ( XOR and_expression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_and_expression_in_xor_expression3168);
            	and_expression324 = and_expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, and_expression324.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:268:33: ( XOR and_expression )*
            	do 
            	{
            	    int alt70 = 2;
            	    alt70 = dfa70.Predict(input);
            	    switch (alt70) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:268:34: XOR and_expression
            			    {
            			    	XOR325=(IToken)Match(input,XOR,FOLLOW_XOR_in_xor_expression3171); 
            			    		XOR325_tree = (object)adaptor.Create(XOR325);
            			    		root_0 = (object)adaptor.BecomeRoot(XOR325_tree, root_0);

            			    	PushFollow(FOLLOW_and_expression_in_xor_expression3174);
            			    	and_expression326 = and_expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, and_expression326.Tree);

            			    }
            			    break;

            			default:
            			    goto loop70;
            	    }
            	} while (true);

            	loop70:
            		;	// Stops C# compiler whining that label 'loop70' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "xor_expression"

    public class and_expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "and_expression"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:270:1: and_expression : comparison ( AND comparison )* ;
    public MPALParser.and_expression_return and_expression() // throws RecognitionException [1]
    {   
        MPALParser.and_expression_return retval = new MPALParser.and_expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken AND328 = null;
        MPALParser.comparison_return comparison327 = default(MPALParser.comparison_return);

        MPALParser.comparison_return comparison329 = default(MPALParser.comparison_return);


        object AND328_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:270:16: ( comparison ( AND comparison )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:270:18: comparison ( AND comparison )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_comparison_in_and_expression3184);
            	comparison327 = comparison();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, comparison327.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:270:29: ( AND comparison )*
            	do 
            	{
            	    int alt71 = 2;
            	    alt71 = dfa71.Predict(input);
            	    switch (alt71) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:270:30: AND comparison
            			    {
            			    	AND328=(IToken)Match(input,AND,FOLLOW_AND_in_and_expression3187); 
            			    		AND328_tree = (object)adaptor.Create(AND328);
            			    		root_0 = (object)adaptor.BecomeRoot(AND328_tree, root_0);

            			    	PushFollow(FOLLOW_comparison_in_and_expression3190);
            			    	comparison329 = comparison();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, comparison329.Tree);

            			    }
            			    break;

            			default:
            			    goto loop71;
            	    }
            	} while (true);

            	loop71:
            		;	// Stops C# compiler whining that label 'loop71' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "and_expression"

    public class comparison_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "comparison"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:272:1: comparison : equ_expression ( ( EQU | NEQ ) equ_expression )* ;
    public MPALParser.comparison_return comparison() // throws RecognitionException [1]
    {   
        MPALParser.comparison_return retval = new MPALParser.comparison_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set331 = null;
        MPALParser.equ_expression_return equ_expression330 = default(MPALParser.equ_expression_return);

        MPALParser.equ_expression_return equ_expression332 = default(MPALParser.equ_expression_return);


        object set331_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:272:12: ( equ_expression ( ( EQU | NEQ ) equ_expression )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:272:14: equ_expression ( ( EQU | NEQ ) equ_expression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_equ_expression_in_comparison3200);
            	equ_expression330 = equ_expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, equ_expression330.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:272:29: ( ( EQU | NEQ ) equ_expression )*
            	do 
            	{
            	    int alt72 = 2;
            	    alt72 = dfa72.Predict(input);
            	    switch (alt72) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:272:31: ( EQU | NEQ ) equ_expression
            			    {
            			    	set331=(IToken)input.LT(1);
            			    	set331 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == EQU || input.LA(1) == NEQ ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set331), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_equ_expression_in_comparison3213);
            			    	equ_expression332 = equ_expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, equ_expression332.Tree);

            			    }
            			    break;

            			default:
            			    goto loop72;
            	    }
            	} while (true);

            	loop72:
            		;	// Stops C# compiler whining that label 'loop72' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "comparison"

    public class equ_expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "equ_expression"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:274:1: equ_expression : add_expression ( comparison_operators add_expression )* ;
    public MPALParser.equ_expression_return equ_expression() // throws RecognitionException [1]
    {   
        MPALParser.equ_expression_return retval = new MPALParser.equ_expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.add_expression_return add_expression333 = default(MPALParser.add_expression_return);

        MPALParser.comparison_operators_return comparison_operators334 = default(MPALParser.comparison_operators_return);

        MPALParser.add_expression_return add_expression335 = default(MPALParser.add_expression_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:274:16: ( add_expression ( comparison_operators add_expression )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:274:18: add_expression ( comparison_operators add_expression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_add_expression_in_equ_expression3223);
            	add_expression333 = add_expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, add_expression333.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:274:33: ( comparison_operators add_expression )*
            	do 
            	{
            	    int alt73 = 2;
            	    alt73 = dfa73.Predict(input);
            	    switch (alt73) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:274:34: comparison_operators add_expression
            			    {
            			    	PushFollow(FOLLOW_comparison_operators_in_equ_expression3226);
            			    	comparison_operators334 = comparison_operators();
            			    	state.followingStackPointer--;

            			    	root_0 = (object)adaptor.BecomeRoot(comparison_operators334.Tree, root_0);
            			    	PushFollow(FOLLOW_add_expression_in_equ_expression3229);
            			    	add_expression335 = add_expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, add_expression335.Tree);

            			    }
            			    break;

            			default:
            			    goto loop73;
            	    }
            	} while (true);

            	loop73:
            		;	// Stops C# compiler whining that label 'loop73' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "equ_expression"

    public class add_expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "add_expression"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:276:1: add_expression : term ( ( PLUS | NEG ) term )* ;
    public MPALParser.add_expression_return add_expression() // throws RecognitionException [1]
    {   
        MPALParser.add_expression_return retval = new MPALParser.add_expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set337 = null;
        MPALParser.term_return term336 = default(MPALParser.term_return);

        MPALParser.term_return term338 = default(MPALParser.term_return);


        object set337_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:276:16: ( term ( ( PLUS | NEG ) term )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:276:18: term ( ( PLUS | NEG ) term )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_term_in_add_expression3248);
            	term336 = term();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, term336.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:276:23: ( ( PLUS | NEG ) term )*
            	do 
            	{
            	    int alt74 = 2;
            	    alt74 = dfa74.Predict(input);
            	    switch (alt74) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:276:25: ( PLUS | NEG ) term
            			    {
            			    	set337=(IToken)input.LT(1);
            			    	set337 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= PLUS && input.LA(1) <= NEG) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set337), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_term_in_add_expression3259);
            			    	term338 = term();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, term338.Tree);

            			    }
            			    break;

            			default:
            			    goto loop74;
            	    }
            	} while (true);

            	loop74:
            		;	// Stops C# compiler whining that label 'loop74' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "add_expression"

    public class term_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "term"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:278:1: term : power_expression ( multiply_operator power_expression )* ;
    public MPALParser.term_return term() // throws RecognitionException [1]
    {   
        MPALParser.term_return retval = new MPALParser.term_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.power_expression_return power_expression339 = default(MPALParser.power_expression_return);

        MPALParser.multiply_operator_return multiply_operator340 = default(MPALParser.multiply_operator_return);

        MPALParser.power_expression_return power_expression341 = default(MPALParser.power_expression_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:278:6: ( power_expression ( multiply_operator power_expression )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:278:8: power_expression ( multiply_operator power_expression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_power_expression_in_term3270);
            	power_expression339 = power_expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, power_expression339.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:278:25: ( multiply_operator power_expression )*
            	do 
            	{
            	    int alt75 = 2;
            	    alt75 = dfa75.Predict(input);
            	    switch (alt75) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:278:26: multiply_operator power_expression
            			    {
            			    	PushFollow(FOLLOW_multiply_operator_in_term3273);
            			    	multiply_operator340 = multiply_operator();
            			    	state.followingStackPointer--;

            			    	root_0 = (object)adaptor.BecomeRoot(multiply_operator340.Tree, root_0);
            			    	PushFollow(FOLLOW_power_expression_in_term3276);
            			    	power_expression341 = power_expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, power_expression341.Tree);

            			    }
            			    break;

            			default:
            			    goto loop75;
            	    }
            	} while (true);

            	loop75:
            		;	// Stops C# compiler whining that label 'loop75' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "term"

    public class power_expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "power_expression"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:280:1: power_expression : unary_expression ( POW unary_expression )* ;
    public MPALParser.power_expression_return power_expression() // throws RecognitionException [1]
    {   
        MPALParser.power_expression_return retval = new MPALParser.power_expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken POW343 = null;
        MPALParser.unary_expression_return unary_expression342 = default(MPALParser.unary_expression_return);

        MPALParser.unary_expression_return unary_expression344 = default(MPALParser.unary_expression_return);


        object POW343_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:280:18: ( unary_expression ( POW unary_expression )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:280:20: unary_expression ( POW unary_expression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unary_expression_in_power_expression3286);
            	unary_expression342 = unary_expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, unary_expression342.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:280:37: ( POW unary_expression )*
            	do 
            	{
            	    int alt76 = 2;
            	    alt76 = dfa76.Predict(input);
            	    switch (alt76) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:280:38: POW unary_expression
            			    {
            			    	POW343=(IToken)Match(input,POW,FOLLOW_POW_in_power_expression3289); 
            			    		POW343_tree = (object)adaptor.Create(POW343);
            			    		root_0 = (object)adaptor.BecomeRoot(POW343_tree, root_0);

            			    	PushFollow(FOLLOW_unary_expression_in_power_expression3292);
            			    	unary_expression344 = unary_expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, unary_expression344.Tree);

            			    }
            			    break;

            			default:
            			    goto loop76;
            	    }
            	} while (true);

            	loop76:
            		;	// Stops C# compiler whining that label 'loop76' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "power_expression"

    public class unary_expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unary_expression"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:282:1: unary_expression : ( unary_operator )? primary_expression ;
    public MPALParser.unary_expression_return unary_expression() // throws RecognitionException [1]
    {   
        MPALParser.unary_expression_return retval = new MPALParser.unary_expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.unary_operator_return unary_operator345 = default(MPALParser.unary_operator_return);

        MPALParser.primary_expression_return primary_expression346 = default(MPALParser.primary_expression_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:282:18: ( ( unary_operator )? primary_expression )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:282:20: ( unary_operator )? primary_expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:282:20: ( unary_operator )?
            	int alt77 = 2;
            	alt77 = dfa77.Predict(input);
            	switch (alt77) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:282:21: unary_operator
            	        {
            	        	PushFollow(FOLLOW_unary_operator_in_unary_expression3303);
            	        	unary_operator345 = unary_operator();
            	        	state.followingStackPointer--;

            	        	root_0 = (object)adaptor.BecomeRoot(unary_operator345.Tree, root_0);

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_primary_expression_in_unary_expression3308);
            	primary_expression346 = primary_expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, primary_expression346.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "unary_expression"

    public class primary_expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "primary_expression"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:284:1: primary_expression : ( function_call | variable | constant_literal | enumerated_value | '(' expression ')' );
    public MPALParser.primary_expression_return primary_expression() // throws RecognitionException [1]
    {   
        MPALParser.primary_expression_return retval = new MPALParser.primary_expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal351 = null;
        IToken char_literal353 = null;
        MPALParser.function_call_return function_call347 = default(MPALParser.function_call_return);

        MPALParser.variable_return variable348 = default(MPALParser.variable_return);

        MPALParser.constant_literal_return constant_literal349 = default(MPALParser.constant_literal_return);

        MPALParser.enumerated_value_return enumerated_value350 = default(MPALParser.enumerated_value_return);

        MPALParser.expression_return expression352 = default(MPALParser.expression_return);


        object char_literal351_tree=null;
        object char_literal353_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:284:20: ( function_call | variable | constant_literal | enumerated_value | '(' expression ')' )
            int alt78 = 5;
            alt78 = dfa78.Predict(input);
            switch (alt78) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:284:22: function_call
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_function_call_in_primary_expression3316);
                    	function_call347 = function_call();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, function_call347.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:284:38: variable
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_primary_expression3320);
                    	variable348 = variable();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, variable348.Tree);

                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:284:48: constant_literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_constant_literal_in_primary_expression3323);
                    	constant_literal349 = constant_literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, constant_literal349.Tree);

                    }
                    break;
                case 4 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:284:66: enumerated_value
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_enumerated_value_in_primary_expression3326);
                    	enumerated_value350 = enumerated_value();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, enumerated_value350.Tree);

                    }
                    break;
                case 5 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:284:85: '(' expression ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal351=(IToken)Match(input,LRBRACKED,FOLLOW_LRBRACKED_in_primary_expression3330); 
                    	PushFollow(FOLLOW_expression_in_primary_expression3333);
                    	expression352 = expression();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, expression352.Tree);
                    	char_literal353=(IToken)Match(input,107,FOLLOW_107_in_primary_expression3335); 

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "primary_expression"

    public class function_call_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "function_call"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:286:1: function_call : ( IDENTIFIER '(' param_assignment ( COMMA param_assignment )* ')' ) ;
    public MPALParser.function_call_return function_call() // throws RecognitionException [1]
    {   
        MPALParser.function_call_return retval = new MPALParser.function_call_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER354 = null;
        IToken char_literal355 = null;
        IToken COMMA357 = null;
        IToken char_literal359 = null;
        MPALParser.param_assignment_return param_assignment356 = default(MPALParser.param_assignment_return);

        MPALParser.param_assignment_return param_assignment358 = default(MPALParser.param_assignment_return);


        object IDENTIFIER354_tree=null;
        object char_literal355_tree=null;
        object COMMA357_tree=null;
        object char_literal359_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:286:15: ( ( IDENTIFIER '(' param_assignment ( COMMA param_assignment )* ')' ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:286:17: ( IDENTIFIER '(' param_assignment ( COMMA param_assignment )* ')' )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:286:17: ( IDENTIFIER '(' param_assignment ( COMMA param_assignment )* ')' )
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:286:18: IDENTIFIER '(' param_assignment ( COMMA param_assignment )* ')'
            	{
            		IDENTIFIER354=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_function_call3346); 
            			IDENTIFIER354_tree = (object)adaptor.Create(IDENTIFIER354);
            			adaptor.AddChild(root_0, IDENTIFIER354_tree);

            		char_literal355=(IToken)Match(input,LRBRACKED,FOLLOW_LRBRACKED_in_function_call3348); 
            			char_literal355_tree = (object)adaptor.Create(char_literal355);
            			root_0 = (object)adaptor.BecomeRoot(char_literal355_tree, root_0);

            		PushFollow(FOLLOW_param_assignment_in_function_call3351);
            		param_assignment356 = param_assignment();
            		state.followingStackPointer--;

            		adaptor.AddChild(root_0, param_assignment356.Tree);
            		// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:286:51: ( COMMA param_assignment )*
            		do 
            		{
            		    int alt79 = 2;
            		    int LA79_0 = input.LA(1);

            		    if ( (LA79_0 == COMMA) )
            		    {
            		        alt79 = 1;
            		    }


            		    switch (alt79) 
            			{
            				case 1 :
            				    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:286:52: COMMA param_assignment
            				    {
            				    	COMMA357=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_function_call3354); 
            				    	PushFollow(FOLLOW_param_assignment_in_function_call3357);
            				    	param_assignment358 = param_assignment();
            				    	state.followingStackPointer--;

            				    	adaptor.AddChild(root_0, param_assignment358.Tree);

            				    }
            				    break;

            				default:
            				    goto loop79;
            		    }
            		} while (true);

            		loop79:
            			;	// Stops C# compiler whining that label 'loop79' has no statements

            		char_literal359=(IToken)Match(input,107,FOLLOW_107_in_function_call3362); 

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "function_call"

    public class unary_operator_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unary_operator"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:288:1: unary_operator : ( PLUS | NEG | NOT ) ;
    public MPALParser.unary_operator_return unary_operator() // throws RecognitionException [1]
    {   
        MPALParser.unary_operator_return retval = new MPALParser.unary_operator_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken PLUS360 = null;
        IToken NEG361 = null;
        IToken NOT362 = null;

        object PLUS360_tree=null;
        object NEG361_tree=null;
        object NOT362_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:288:16: ( ( PLUS | NEG | NOT ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:288:18: ( PLUS | NEG | NOT )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:288:18: ( PLUS | NEG | NOT )
            	int alt80 = 3;
            	switch ( input.LA(1) ) 
            	{
            	case PLUS:
            		{
            	    alt80 = 1;
            	    }
            	    break;
            	case NEG:
            		{
            	    alt80 = 2;
            	    }
            	    break;
            	case NOT:
            		{
            	    alt80 = 3;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d80s0 =
            		        new NoViableAltException("", 80, 0, input);

            		    throw nvae_d80s0;
            	}

            	switch (alt80) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:288:19: PLUS
            	        {
            	        	PLUS360=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_unary_operator3376); 
            	        		PLUS360_tree = (object)adaptor.Create(PLUS360);
            	        		root_0 = (object)adaptor.BecomeRoot(PLUS360_tree, root_0);


            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:288:26: NEG
            	        {
            	        	NEG361=(IToken)Match(input,NEG,FOLLOW_NEG_in_unary_operator3380); 
            	        		NEG361_tree = (object)adaptor.Create(NEG361);
            	        		root_0 = (object)adaptor.BecomeRoot(NEG361_tree, root_0);


            	        }
            	        break;
            	    case 3 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:288:33: NOT
            	        {
            	        	NOT362=(IToken)Match(input,NOT,FOLLOW_NOT_in_unary_operator3385); 
            	        		NOT362_tree = (object)adaptor.Create(NOT362);
            	        		root_0 = (object)adaptor.BecomeRoot(NOT362_tree, root_0);


            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "unary_operator"

    public class multiply_operator_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "multiply_operator"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:290:1: multiply_operator : ( MUL | DIV | MOD );
    public MPALParser.multiply_operator_return multiply_operator() // throws RecognitionException [1]
    {   
        MPALParser.multiply_operator_return retval = new MPALParser.multiply_operator_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken MUL363 = null;
        IToken DIV364 = null;
        IToken MOD365 = null;

        object MUL363_tree=null;
        object DIV364_tree=null;
        object MOD365_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:290:19: ( MUL | DIV | MOD )
            int alt81 = 3;
            switch ( input.LA(1) ) 
            {
            case MUL:
            	{
                alt81 = 1;
                }
                break;
            case DIV:
            	{
                alt81 = 2;
                }
                break;
            case MOD:
            	{
                alt81 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d81s0 =
            	        new NoViableAltException("", 81, 0, input);

            	    throw nvae_d81s0;
            }

            switch (alt81) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:290:21: MUL
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	MUL363=(IToken)Match(input,MUL,FOLLOW_MUL_in_multiply_operator3404); 
                    		MUL363_tree = (object)adaptor.Create(MUL363);
                    		root_0 = (object)adaptor.BecomeRoot(MUL363_tree, root_0);


                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:290:28: DIV
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	DIV364=(IToken)Match(input,DIV,FOLLOW_DIV_in_multiply_operator3409); 
                    		DIV364_tree = (object)adaptor.Create(DIV364);
                    		root_0 = (object)adaptor.BecomeRoot(DIV364_tree, root_0);


                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:290:35: MOD
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	MOD365=(IToken)Match(input,MOD,FOLLOW_MOD_in_multiply_operator3414); 
                    		MOD365_tree = (object)adaptor.Create(MOD365);
                    		adaptor.AddChild(root_0, MOD365_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "multiply_operator"

    public class number_literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "number_literal"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:292:1: number_literal : bit_string_literal ;
    public MPALParser.number_literal_return number_literal() // throws RecognitionException [1]
    {   
        MPALParser.number_literal_return retval = new MPALParser.number_literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.bit_string_literal_return bit_string_literal366 = default(MPALParser.bit_string_literal_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:292:16: ( bit_string_literal )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:292:18: bit_string_literal
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_bit_string_literal_in_number_literal3423);
            	bit_string_literal366 = bit_string_literal();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, bit_string_literal366.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "number_literal"

    public class comparison_operators_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "comparison_operators"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:294:1: comparison_operators : ( LS | GR | LEQ | GEQ );
    public MPALParser.comparison_operators_return comparison_operators() // throws RecognitionException [1]
    {   
        MPALParser.comparison_operators_return retval = new MPALParser.comparison_operators_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set367 = null;

        object set367_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:294:22: ( LS | GR | LEQ | GEQ )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set367 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= LEQ && input.LA(1) <= GEQ) || (input.LA(1) >= LS && input.LA(1) <= GR) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set367));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "comparison_operators"

    public class variable_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variable"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:296:1: variable : multi_element_variable ;
    public MPALParser.variable_return variable() // throws RecognitionException [1]
    {   
        MPALParser.variable_return retval = new MPALParser.variable_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.multi_element_variable_return multi_element_variable368 = default(MPALParser.multi_element_variable_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:296:10: ( multi_element_variable )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:296:12: multi_element_variable
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multi_element_variable_in_variable3453);
            	multi_element_variable368 = multi_element_variable();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, multi_element_variable368.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "variable"

    public class multi_element_variable_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "multi_element_variable"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:298:1: multi_element_variable : IDENTIFIER ( DOT IDENTIFIER | subscript_list )* ;
    public MPALParser.multi_element_variable_return multi_element_variable() // throws RecognitionException [1]
    {   
        MPALParser.multi_element_variable_return retval = new MPALParser.multi_element_variable_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER369 = null;
        IToken DOT370 = null;
        IToken IDENTIFIER371 = null;
        MPALParser.subscript_list_return subscript_list372 = default(MPALParser.subscript_list_return);


        object IDENTIFIER369_tree=null;
        object DOT370_tree=null;
        object IDENTIFIER371_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:298:24: ( IDENTIFIER ( DOT IDENTIFIER | subscript_list )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:298:26: IDENTIFIER ( DOT IDENTIFIER | subscript_list )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	IDENTIFIER369=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_multi_element_variable3462); 
            		IDENTIFIER369_tree = (object)adaptor.Create(IDENTIFIER369);
            		adaptor.AddChild(root_0, IDENTIFIER369_tree);

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:298:37: ( DOT IDENTIFIER | subscript_list )*
            	do 
            	{
            	    int alt82 = 3;
            	    alt82 = dfa82.Predict(input);
            	    switch (alt82) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:298:39: DOT IDENTIFIER
            			    {
            			    	DOT370=(IToken)Match(input,DOT,FOLLOW_DOT_in_multi_element_variable3466); 
            			    		DOT370_tree = (object)adaptor.Create(DOT370);
            			    		root_0 = (object)adaptor.BecomeRoot(DOT370_tree, root_0);

            			    	IDENTIFIER371=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_multi_element_variable3469); 
            			    		IDENTIFIER371_tree = (object)adaptor.Create(IDENTIFIER371);
            			    		adaptor.AddChild(root_0, IDENTIFIER371_tree);


            			    }
            			    break;
            			case 2 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:298:57: subscript_list
            			    {
            			    	PushFollow(FOLLOW_subscript_list_in_multi_element_variable3473);
            			    	subscript_list372 = subscript_list();
            			    	state.followingStackPointer--;

            			    	root_0 = (object)adaptor.BecomeRoot(subscript_list372.Tree, root_0);

            			    }
            			    break;

            			default:
            			    goto loop82;
            	    }
            	} while (true);

            	loop82:
            		;	// Stops C# compiler whining that label 'loop82' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "multi_element_variable"

    public class subscript_list_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "subscript_list"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:300:1: subscript_list : LBRACKED subscript ( COMMA subscript )* ']' ;
    public MPALParser.subscript_list_return subscript_list() // throws RecognitionException [1]
    {   
        MPALParser.subscript_list_return retval = new MPALParser.subscript_list_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LBRACKED373 = null;
        IToken COMMA375 = null;
        IToken char_literal377 = null;
        MPALParser.subscript_return subscript374 = default(MPALParser.subscript_return);

        MPALParser.subscript_return subscript376 = default(MPALParser.subscript_return);


        object LBRACKED373_tree=null;
        object COMMA375_tree=null;
        object char_literal377_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:300:16: ( LBRACKED subscript ( COMMA subscript )* ']' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:300:18: LBRACKED subscript ( COMMA subscript )* ']'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	LBRACKED373=(IToken)Match(input,LBRACKED,FOLLOW_LBRACKED_in_subscript_list3484); 
            		LBRACKED373_tree = (object)adaptor.Create(LBRACKED373);
            		root_0 = (object)adaptor.BecomeRoot(LBRACKED373_tree, root_0);

            	PushFollow(FOLLOW_subscript_in_subscript_list3488);
            	subscript374 = subscript();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, subscript374.Tree);
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:300:39: ( COMMA subscript )*
            	do 
            	{
            	    int alt83 = 2;
            	    int LA83_0 = input.LA(1);

            	    if ( (LA83_0 == COMMA) )
            	    {
            	        alt83 = 1;
            	    }


            	    switch (alt83) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:300:40: COMMA subscript
            			    {
            			    	COMMA375=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_subscript_list3491); 
            			    	PushFollow(FOLLOW_subscript_in_subscript_list3494);
            			    	subscript376 = subscript();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, subscript376.Tree);

            			    }
            			    break;

            			default:
            			    goto loop83;
            	    }
            	} while (true);

            	loop83:
            		;	// Stops C# compiler whining that label 'loop83' has no statements

            	char_literal377=(IToken)Match(input,106,FOLLOW_106_in_subscript_list3498); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "subscript_list"

    public class subscript_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "subscript"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:302:1: subscript : expression ;
    public MPALParser.subscript_return subscript() // throws RecognitionException [1]
    {   
        MPALParser.subscript_return retval = new MPALParser.subscript_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MPALParser.expression_return expression378 = default(MPALParser.expression_return);



        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:302:11: ( expression )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:302:13: expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_expression_in_subscript3507);
            	expression378 = expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, expression378.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "subscript"

    public class simple_specification_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "simple_specification"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:308:1: simple_specification : ( elemetary_type_name | IDENTIFIER | IDENTIFIER DOT IDENTIFIER );
    public MPALParser.simple_specification_return simple_specification() // throws RecognitionException [1]
    {   
        MPALParser.simple_specification_return retval = new MPALParser.simple_specification_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER380 = null;
        IToken IDENTIFIER381 = null;
        IToken DOT382 = null;
        IToken IDENTIFIER383 = null;
        MPALParser.elemetary_type_name_return elemetary_type_name379 = default(MPALParser.elemetary_type_name_return);


        object IDENTIFIER380_tree=null;
        object IDENTIFIER381_tree=null;
        object DOT382_tree=null;
        object IDENTIFIER383_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:308:22: ( elemetary_type_name | IDENTIFIER | IDENTIFIER DOT IDENTIFIER )
            int alt84 = 3;
            alt84 = dfa84.Predict(input);
            switch (alt84) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:308:24: elemetary_type_name
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_elemetary_type_name_in_simple_specification3520);
                    	elemetary_type_name379 = elemetary_type_name();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, elemetary_type_name379.Tree);

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:308:47: IDENTIFIER
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	IDENTIFIER380=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_simple_specification3525); 
                    		IDENTIFIER380_tree = (object)adaptor.Create(IDENTIFIER380);
                    		adaptor.AddChild(root_0, IDENTIFIER380_tree);


                    }
                    break;
                case 3 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:308:61: IDENTIFIER DOT IDENTIFIER
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	IDENTIFIER381=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_simple_specification3530); 
                    		IDENTIFIER381_tree = (object)adaptor.Create(IDENTIFIER381);
                    		adaptor.AddChild(root_0, IDENTIFIER381_tree);

                    	DOT382=(IToken)Match(input,DOT,FOLLOW_DOT_in_simple_specification3532); 
                    		DOT382_tree = (object)adaptor.Create(DOT382);
                    		adaptor.AddChild(root_0, DOT382_tree);

                    	IDENTIFIER383=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_simple_specification3534); 
                    		IDENTIFIER383_tree = (object)adaptor.Create(IDENTIFIER383);
                    		adaptor.AddChild(root_0, IDENTIFIER383_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "simple_specification"

    public class elemetary_type_name_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "elemetary_type_name"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:310:1: elemetary_type_name : ( USINT | UINT | UDINT | ULINT | SINT | INT | DINT | LINT | TIME | VOID | DATE_LIT | TIME_OF_DAY_LIT | TOD | DATE_AND_TIME | DT | BOOL | BYTE | WORD | DWORD | LWORD | REAL | LREAL | STRING | WSTRING | CHAR | WCHAR | COUNTER | TIMER );
    public MPALParser.elemetary_type_name_return elemetary_type_name() // throws RecognitionException [1]
    {   
        MPALParser.elemetary_type_name_return retval = new MPALParser.elemetary_type_name_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set384 = null;

        object set384_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:310:21: ( USINT | UINT | UDINT | ULINT | SINT | INT | DINT | LINT | TIME | VOID | DATE_LIT | TIME_OF_DAY_LIT | TOD | DATE_AND_TIME | DT | BOOL | BYTE | WORD | DWORD | LWORD | REAL | LREAL | STRING | WSTRING | CHAR | WCHAR | COUNTER | TIMER )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set384 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= STRING && input.LA(1) <= WSTRING) || (input.LA(1) >= VOID && input.LA(1) <= WCHAR) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set384));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "elemetary_type_name"

    public class real_type_name_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "real_type_name"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:314:1: real_type_name : ( REAL | LREAL );
    public MPALParser.real_type_name_return real_type_name() // throws RecognitionException [1]
    {   
        MPALParser.real_type_name_return retval = new MPALParser.real_type_name_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set385 = null;

        object set385_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:314:16: ( REAL | LREAL )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set385 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= REAL && input.LA(1) <= LREAL) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set385));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "real_type_name"

    public class integer_type_name_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "integer_type_name"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:316:1: integer_type_name : ( SINT | INT | DINT | LINT | USINT | UINT | UDINT | ULINT );
    public MPALParser.integer_type_name_return integer_type_name() // throws RecognitionException [1]
    {   
        MPALParser.integer_type_name_return retval = new MPALParser.integer_type_name_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set386 = null;

        object set386_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:316:19: ( SINT | INT | DINT | LINT | USINT | UINT | UDINT | ULINT )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set386 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= SINT && input.LA(1) <= ULINT) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set386));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "integer_type_name"

    public class bool_literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "bool_literal"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:318:1: bool_literal : ( BOOL SHARP )? ( TRUE | FALSE ) ;
    public MPALParser.bool_literal_return bool_literal() // throws RecognitionException [1]
    {   
        MPALParser.bool_literal_return retval = new MPALParser.bool_literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken BOOL387 = null;
        IToken SHARP388 = null;
        IToken set389 = null;

        object BOOL387_tree=null;
        object SHARP388_tree=null;
        object set389_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:318:14: ( ( BOOL SHARP )? ( TRUE | FALSE ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:318:16: ( BOOL SHARP )? ( TRUE | FALSE )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:318:16: ( BOOL SHARP )?
            	int alt85 = 2;
            	int LA85_0 = input.LA(1);

            	if ( (LA85_0 == BOOL) )
            	{
            	    alt85 = 1;
            	}
            	switch (alt85) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:318:18: BOOL SHARP
            	        {
            	        	BOOL387=(IToken)Match(input,BOOL,FOLLOW_BOOL_in_bool_literal3757); 
            	        		BOOL387_tree = (object)adaptor.Create(BOOL387);
            	        		adaptor.AddChild(root_0, BOOL387_tree);

            	        	SHARP388=(IToken)Match(input,SHARP,FOLLOW_SHARP_in_bool_literal3759); 
            	        		SHARP388_tree = (object)adaptor.Create(SHARP388);
            	        		root_0 = (object)adaptor.BecomeRoot(SHARP388_tree, root_0);


            	        }
            	        break;

            	}

            	set389 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= TRUE && input.LA(1) <= FALSE) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set389));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "bool_literal"

    public class subrange_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "subrange"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:320:1: subrange : range_index DOTDOT range_index ;
    public MPALParser.subrange_return subrange() // throws RecognitionException [1]
    {   
        MPALParser.subrange_return retval = new MPALParser.subrange_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken DOTDOT391 = null;
        MPALParser.range_index_return range_index390 = default(MPALParser.range_index_return);

        MPALParser.range_index_return range_index392 = default(MPALParser.range_index_return);


        object DOTDOT391_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:320:11: ( range_index DOTDOT range_index )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:320:13: range_index DOTDOT range_index
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_range_index_in_subrange3780);
            	range_index390 = range_index();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, range_index390.Tree);
            	DOTDOT391=(IToken)Match(input,DOTDOT,FOLLOW_DOTDOT_in_subrange3782); 
            		DOTDOT391_tree = (object)adaptor.Create(DOTDOT391);
            		root_0 = (object)adaptor.BecomeRoot(DOTDOT391_tree, root_0);

            	PushFollow(FOLLOW_range_index_in_subrange3785);
            	range_index392 = range_index();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, range_index392.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "subrange"

    public class range_index_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "range_index"
    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:322:1: range_index : ( integer_literal | IDENTIFIER ) ;
    public MPALParser.range_index_return range_index() // throws RecognitionException [1]
    {   
        MPALParser.range_index_return retval = new MPALParser.range_index_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IDENTIFIER394 = null;
        MPALParser.integer_literal_return integer_literal393 = default(MPALParser.integer_literal_return);


        object IDENTIFIER394_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:322:13: ( ( integer_literal | IDENTIFIER ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:322:16: ( integer_literal | IDENTIFIER )
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:322:16: ( integer_literal | IDENTIFIER )
            	int alt86 = 2;
            	int LA86_0 = input.LA(1);

            	if ( ((LA86_0 >= PLUS && LA86_0 <= NEG) || LA86_0 == INTEGER || (LA86_0 >= BINARY_INTEGER && LA86_0 <= HEX_INTEGER)) )
            	{
            	    alt86 = 1;
            	}
            	else if ( (LA86_0 == IDENTIFIER) )
            	{
            	    alt86 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d86s0 =
            	        new NoViableAltException("", 86, 0, input);

            	    throw nvae_d86s0;
            	}
            	switch (alt86) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:322:17: integer_literal
            	        {
            	        	PushFollow(FOLLOW_integer_literal_in_range_index3795);
            	        	integer_literal393 = integer_literal();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, integer_literal393.Tree);

            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:322:34: IDENTIFIER
            	        {
            	        	IDENTIFIER394=(IToken)Match(input,IDENTIFIER,FOLLOW_IDENTIFIER_in_range_index3798); 
            	        		IDENTIFIER394_tree = (object)adaptor.Create(IDENTIFIER394);
            	        		adaptor.AddChild(root_0, IDENTIFIER394_tree);


            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "range_index"

    // Delegated rules


   	protected DFA2 dfa2;
   	protected DFA3 dfa3;
   	protected DFA4 dfa4;
   	protected DFA5 dfa5;
   	protected DFA12 dfa12;
   	protected DFA13 dfa13;
   	protected DFA26 dfa26;
   	protected DFA28 dfa28;
   	protected DFA30 dfa30;
   	protected DFA29 dfa29;
   	protected DFA32 dfa32;
   	protected DFA35 dfa35;
   	protected DFA41 dfa41;
   	protected DFA42 dfa42;
   	protected DFA51 dfa51;
   	protected DFA54 dfa54;
   	protected DFA56 dfa56;
   	protected DFA61 dfa61;
   	protected DFA63 dfa63;
   	protected DFA65 dfa65;
   	protected DFA68 dfa68;
   	protected DFA66 dfa66;
   	protected DFA69 dfa69;
   	protected DFA70 dfa70;
   	protected DFA71 dfa71;
   	protected DFA72 dfa72;
   	protected DFA73 dfa73;
   	protected DFA74 dfa74;
   	protected DFA75 dfa75;
   	protected DFA76 dfa76;
   	protected DFA77 dfa77;
   	protected DFA78 dfa78;
   	protected DFA82 dfa82;
   	protected DFA84 dfa84;
	private void InitializeCyclicDFAs()
	{
    	this.dfa2 = new DFA2(this);
    	this.dfa3 = new DFA3(this);
    	this.dfa4 = new DFA4(this);
    	this.dfa5 = new DFA5(this);
    	this.dfa12 = new DFA12(this);
    	this.dfa13 = new DFA13(this);
    	this.dfa26 = new DFA26(this);
    	this.dfa28 = new DFA28(this);
    	this.dfa30 = new DFA30(this);
    	this.dfa29 = new DFA29(this);
    	this.dfa32 = new DFA32(this);
    	this.dfa35 = new DFA35(this);
    	this.dfa41 = new DFA41(this);
    	this.dfa42 = new DFA42(this);
    	this.dfa51 = new DFA51(this);
    	this.dfa54 = new DFA54(this);
    	this.dfa56 = new DFA56(this);
    	this.dfa61 = new DFA61(this);
    	this.dfa63 = new DFA63(this);
    	this.dfa65 = new DFA65(this);
    	this.dfa68 = new DFA68(this);
    	this.dfa66 = new DFA66(this);
    	this.dfa69 = new DFA69(this);
    	this.dfa70 = new DFA70(this);
    	this.dfa71 = new DFA71(this);
    	this.dfa72 = new DFA72(this);
    	this.dfa73 = new DFA73(this);
    	this.dfa74 = new DFA74(this);
    	this.dfa75 = new DFA75(this);
    	this.dfa76 = new DFA76(this);
    	this.dfa77 = new DFA77(this);
    	this.dfa78 = new DFA78(this);
    	this.dfa82 = new DFA82(this);
    	this.dfa84 = new DFA84(this);
	}

    const string DFA2_eotS =
        "\x11\uffff";
    const string DFA2_eofS =
        "\x11\uffff";
    const string DFA2_minS =
        "\x01\x06\x10\uffff";
    const string DFA2_maxS =
        "\x01\x6c\x10\uffff";
    const string DFA2_acceptS =
        "\x01\uffff\x01\x04\x09\uffff\x01\x01\x02\uffff\x01\x02\x01\uffff"+
        "\x01\x03";
    const string DFA2_specialS =
        "\x11\uffff}>";
    static readonly string[] DFA2_transitionS = {
            "\x02\x0e\x01\uffff\x02\x0b\x02\uffff\x01\x0b\x06\uffff\x01"+
            "\x01\x2c\uffff\x03\x01\x03\uffff\x03\x01\x05\uffff\x01\x10\x05"+
            "\uffff\x01\x01\x05\uffff\x01\x01\x10\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA2_eot = DFA.UnpackEncodedString(DFA2_eotS);
    static readonly short[] DFA2_eof = DFA.UnpackEncodedString(DFA2_eofS);
    static readonly char[] DFA2_min = DFA.UnpackEncodedStringToUnsignedChars(DFA2_minS);
    static readonly char[] DFA2_max = DFA.UnpackEncodedStringToUnsignedChars(DFA2_maxS);
    static readonly short[] DFA2_accept = DFA.UnpackEncodedString(DFA2_acceptS);
    static readonly short[] DFA2_special = DFA.UnpackEncodedString(DFA2_specialS);
    static readonly short[][] DFA2_transition = DFA.UnpackEncodedStringArray(DFA2_transitionS);

    protected class DFA2 : DFA
    {
        public DFA2(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 2;
            this.eot = DFA2_eot;
            this.eof = DFA2_eof;
            this.min = DFA2_min;
            this.max = DFA2_max;
            this.accept = DFA2_accept;
            this.special = DFA2_special;
            this.transition = DFA2_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 115:31: ( io_var_declarations | other_var_declarations | data_type_declaration )*"; }
        }

    }

    const string DFA3_eotS =
        "\x10\uffff";
    const string DFA3_eofS =
        "\x10\uffff";
    const string DFA3_minS =
        "\x01\x06\x0f\uffff";
    const string DFA3_maxS =
        "\x01\x6c\x0f\uffff";
    const string DFA3_acceptS =
        "\x01\uffff\x01\x03\x09\uffff\x01\x01\x02\uffff\x01\x02\x01\uffff";
    const string DFA3_specialS =
        "\x10\uffff}>";
    static readonly string[] DFA3_transitionS = {
            "\x02\x0e\x01\uffff\x02\x0b\x02\uffff\x01\x0b\x06\uffff\x01"+
            "\x01\x2c\uffff\x03\x01\x03\uffff\x03\x01\x0b\uffff\x01\x01\x05"+
            "\uffff\x01\x01\x10\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA3_eot = DFA.UnpackEncodedString(DFA3_eotS);
    static readonly short[] DFA3_eof = DFA.UnpackEncodedString(DFA3_eofS);
    static readonly char[] DFA3_min = DFA.UnpackEncodedStringToUnsignedChars(DFA3_minS);
    static readonly char[] DFA3_max = DFA.UnpackEncodedStringToUnsignedChars(DFA3_maxS);
    static readonly short[] DFA3_accept = DFA.UnpackEncodedString(DFA3_acceptS);
    static readonly short[] DFA3_special = DFA.UnpackEncodedString(DFA3_specialS);
    static readonly short[][] DFA3_transition = DFA.UnpackEncodedStringArray(DFA3_transitionS);

    protected class DFA3 : DFA
    {
        public DFA3(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 3;
            this.eot = DFA3_eot;
            this.eof = DFA3_eof;
            this.min = DFA3_min;
            this.max = DFA3_max;
            this.accept = DFA3_accept;
            this.special = DFA3_special;
            this.transition = DFA3_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 117:45: ( io_var_declarations | other_var_declarations )*"; }
        }

    }

    const string DFA4_eotS =
        "\x0f\uffff";
    const string DFA4_eofS =
        "\x0f\uffff";
    const string DFA4_minS =
        "\x01\x06\x0e\uffff";
    const string DFA4_maxS =
        "\x01\x6c\x0e\uffff";
    const string DFA4_acceptS =
        "\x01\uffff\x01\x03\x09\uffff\x01\x01\x02\uffff\x01\x02";
    const string DFA4_specialS =
        "\x0f\uffff}>";
    static readonly string[] DFA4_transitionS = {
            "\x01\x0e\x02\uffff\x02\x0b\x02\uffff\x01\x0b\x06\uffff\x01"+
            "\x01\x2c\uffff\x03\x01\x03\uffff\x03\x01\x0b\uffff\x01\x01\x05"+
            "\uffff\x01\x01\x10\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA4_eot = DFA.UnpackEncodedString(DFA4_eotS);
    static readonly short[] DFA4_eof = DFA.UnpackEncodedString(DFA4_eofS);
    static readonly char[] DFA4_min = DFA.UnpackEncodedStringToUnsignedChars(DFA4_minS);
    static readonly char[] DFA4_max = DFA.UnpackEncodedStringToUnsignedChars(DFA4_maxS);
    static readonly short[] DFA4_accept = DFA.UnpackEncodedString(DFA4_acceptS);
    static readonly short[] DFA4_special = DFA.UnpackEncodedString(DFA4_specialS);
    static readonly short[][] DFA4_transition = DFA.UnpackEncodedStringArray(DFA4_transitionS);

    protected class DFA4 : DFA
    {
        public DFA4(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 4;
            this.eot = DFA4_eot;
            this.eof = DFA4_eof;
            this.min = DFA4_min;
            this.max = DFA4_max;
            this.accept = DFA4_accept;
            this.special = DFA4_special;
            this.transition = DFA4_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 119:49: ( io_var_declarations | var_declarations )*"; }
        }

    }

    const string DFA5_eotS =
        "\x10\uffff";
    const string DFA5_eofS =
        "\x10\uffff";
    const string DFA5_minS =
        "\x01\x06\x0f\uffff";
    const string DFA5_maxS =
        "\x01\x6c\x0f\uffff";
    const string DFA5_acceptS =
        "\x01\uffff\x01\x03\x09\uffff\x01\x01\x02\uffff\x01\x02\x01\uffff";
    const string DFA5_specialS =
        "\x10\uffff}>";
    static readonly string[] DFA5_transitionS = {
            "\x02\x0e\x01\uffff\x02\x0b\x02\uffff\x01\x0b\x06\uffff\x01"+
            "\x01\x2c\uffff\x03\x01\x03\uffff\x03\x01\x0b\uffff\x01\x01\x05"+
            "\uffff\x01\x01\x10\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA5_eot = DFA.UnpackEncodedString(DFA5_eotS);
    static readonly short[] DFA5_eof = DFA.UnpackEncodedString(DFA5_eofS);
    static readonly char[] DFA5_min = DFA.UnpackEncodedStringToUnsignedChars(DFA5_minS);
    static readonly char[] DFA5_max = DFA.UnpackEncodedStringToUnsignedChars(DFA5_maxS);
    static readonly short[] DFA5_accept = DFA.UnpackEncodedString(DFA5_acceptS);
    static readonly short[] DFA5_special = DFA.UnpackEncodedString(DFA5_specialS);
    static readonly short[][] DFA5_transition = DFA.UnpackEncodedStringArray(DFA5_transitionS);

    protected class DFA5 : DFA
    {
        public DFA5(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 5;
            this.eot = DFA5_eot;
            this.eof = DFA5_eof;
            this.min = DFA5_min;
            this.max = DFA5_max;
            this.accept = DFA5_accept;
            this.special = DFA5_special;
            this.transition = DFA5_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 123:58: ( io_var_declarations | other_var_declarations )*"; }
        }

    }

    const string DFA12_eotS =
        "\x12\uffff";
    const string DFA12_eofS =
        "\x12\uffff";
    const string DFA12_minS =
        "\x01\x22\x01\uffff\x02\x5c\x0e\uffff";
    const string DFA12_maxS =
        "\x01\x62\x01\uffff\x02\x5f\x0e\uffff";
    const string DFA12_acceptS =
        "\x01\uffff\x01\x01\x03\uffff\x01\x02\x01\x03\x01\x04\x01\uffff"+
        "\x01\x05\x08\uffff";
    const string DFA12_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA12_transitionS = {
            "\x08\x05\x08\uffff\x01\x07\x04\x09\x02\x01\x11\uffff\x02\x07"+
            "\x0b\uffff\x01\x02\x01\x03\x03\uffff\x01\x09\x02\x06\x01\x01"+
            "\x03\x09",
            "",
            "\x01\x09\x02\uffff\x01\x01",
            "\x01\x09\x02\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA12_eot = DFA.UnpackEncodedString(DFA12_eotS);
    static readonly short[] DFA12_eof = DFA.UnpackEncodedString(DFA12_eofS);
    static readonly char[] DFA12_min = DFA.UnpackEncodedStringToUnsignedChars(DFA12_minS);
    static readonly char[] DFA12_max = DFA.UnpackEncodedStringToUnsignedChars(DFA12_maxS);
    static readonly short[] DFA12_accept = DFA.UnpackEncodedString(DFA12_acceptS);
    static readonly short[] DFA12_special = DFA.UnpackEncodedString(DFA12_specialS);
    static readonly short[][] DFA12_transition = DFA.UnpackEncodedStringArray(DFA12_transitionS);

    protected class DFA12 : DFA
    {
        public DFA12(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 12;
            this.eot = DFA12_eot;
            this.eof = DFA12_eof;
            this.min = DFA12_min;
            this.max = DFA12_max;
            this.accept = DFA12_accept;
            this.special = DFA12_special;
            this.transition = DFA12_transition;

        }

        override public string Description
        {
            get { return "143:1: constant_literal : ( real_literal | spec_integer_literal | character_string | bool_literal | bit_string_literal );"; }
        }

    }

    const string DFA13_eotS =
        "\x0f\uffff";
    const string DFA13_eofS =
        "\x0f\uffff";
    const string DFA13_minS =
        "\x01\x22\x0e\uffff";
    const string DFA13_maxS =
        "\x01\x62\x0e\uffff";
    const string DFA13_acceptS =
        "\x01\uffff\x01\x01\x0c\uffff\x01\x02";
    const string DFA13_specialS =
        "\x0f\uffff}>";
    static readonly string[] DFA13_transitionS = {
            "\x08\x01\x08\uffff\x07\x01\x11\uffff\x02\x01\x0b\uffff\x02"+
            "\x01\x02\uffff\x01\x0e\x07\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA13_eot = DFA.UnpackEncodedString(DFA13_eotS);
    static readonly short[] DFA13_eof = DFA.UnpackEncodedString(DFA13_eofS);
    static readonly char[] DFA13_min = DFA.UnpackEncodedStringToUnsignedChars(DFA13_minS);
    static readonly char[] DFA13_max = DFA.UnpackEncodedStringToUnsignedChars(DFA13_maxS);
    static readonly short[] DFA13_accept = DFA.UnpackEncodedString(DFA13_acceptS);
    static readonly short[] DFA13_special = DFA.UnpackEncodedString(DFA13_specialS);
    static readonly short[][] DFA13_transition = DFA.UnpackEncodedStringArray(DFA13_transitionS);

    protected class DFA13 : DFA
    {
        public DFA13(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 13;
            this.eot = DFA13_eot;
            this.eof = DFA13_eof;
            this.min = DFA13_min;
            this.max = DFA13_max;
            this.accept = DFA13_accept;
            this.special = DFA13_special;
            this.transition = DFA13_transition;

        }

        override public string Description
        {
            get { return "145:51: ( constant_literal | IDENTIFIER )"; }
        }

    }

    const string DFA26_eotS =
        "\x24\uffff";
    const string DFA26_eofS =
        "\x24\uffff";
    const string DFA26_minS =
        "\x01\x0b\x03\x15\x02\uffff\x01\x15\x06\uffff\x01\x19\x16\uffff";
    const string DFA26_maxS =
        "\x01\x5b\x03\x6c\x02\uffff\x01\x6c\x06\uffff\x01\x62\x16\uffff";
    const string DFA26_acceptS =
        "\x04\uffff\x01\x03\x01\x04\x01\uffff\x01\x01\x01\x07\x01\x02\x05"+
        "\uffff\x01\x05\x13\uffff\x01\x06";
    const string DFA26_specialS =
        "\x24\uffff}>";
    static readonly string[] DFA26_transitionS = {
            "\x01\x03\x01\x06\x01\uffff\x01\x05\x0a\uffff\x01\x04\x07\uffff"+
            "\x01\x07\x08\x01\x11\x07\x02\uffff\x01\x08\x1d\uffff\x01\x02",
            "\x01\x07\x03\uffff\x01\x09\x52\uffff\x01\x07",
            "\x01\x0d\x4e\uffff\x01\x07\x07\uffff\x01\x07",
            "\x01\x07\x02\uffff\x01\x0f\x53\uffff\x01\x07",
            "",
            "",
            "\x01\x07\x02\uffff\x01\x0f\x53\uffff\x01\x07",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x23\x08\uffff\x08\x07\x08\uffff\x07\x07\x11\uffff\x02"+
            "\x07\x0b\uffff\x02\x07\x02\uffff\x08\x07",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA26_eot = DFA.UnpackEncodedString(DFA26_eotS);
    static readonly short[] DFA26_eof = DFA.UnpackEncodedString(DFA26_eofS);
    static readonly char[] DFA26_min = DFA.UnpackEncodedStringToUnsignedChars(DFA26_minS);
    static readonly char[] DFA26_max = DFA.UnpackEncodedStringToUnsignedChars(DFA26_maxS);
    static readonly short[] DFA26_accept = DFA.UnpackEncodedString(DFA26_acceptS);
    static readonly short[] DFA26_special = DFA.UnpackEncodedString(DFA26_specialS);
    static readonly short[][] DFA26_transition = DFA.UnpackEncodedStringArray(DFA26_transitionS);

    protected class DFA26 : DFA
    {
        public DFA26(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 26;
            this.eot = DFA26_eot;
            this.eof = DFA26_eof;
            this.min = DFA26_min;
            this.max = DFA26_max;
            this.accept = DFA26_accept;
            this.special = DFA26_special;
            this.transition = DFA26_transition;

        }

        override public string Description
        {
            get { return "171:70: ( simple_spec_init | subrange_spec_init | enumerated_spec_init | array_spec_init | string_spec | initialized_structure | structure_declaration )"; }
        }

    }

    const string DFA28_eotS =
        "\x15\uffff";
    const string DFA28_eofS =
        "\x15\uffff";
    const string DFA28_minS =
        "\x01\x18\x0d\uffff\x01\x1a\x06\uffff";
    const string DFA28_maxS =
        "\x01\x62\x0d\uffff\x01\x6b\x06\uffff";
    const string DFA28_acceptS =
        "\x01\uffff\x01\x01\x0d\uffff\x01\x04\x01\x05\x01\x02\x01\x03\x02"+
        "\uffff";
    const string DFA28_specialS =
        "\x15\uffff}>";
    static readonly string[] DFA28_transitionS = {
            "\x01\x10\x01\x0f\x08\uffff\x08\x01\x08\uffff\x07\x01\x11\uffff"+
            "\x02\x01\x0b\uffff\x02\x01\x02\uffff\x01\x0e\x07\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x12\x3b\uffff\x01\x11\x13\uffff\x02\x12",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA28_eot = DFA.UnpackEncodedString(DFA28_eotS);
    static readonly short[] DFA28_eof = DFA.UnpackEncodedString(DFA28_eofS);
    static readonly char[] DFA28_min = DFA.UnpackEncodedStringToUnsignedChars(DFA28_minS);
    static readonly char[] DFA28_max = DFA.UnpackEncodedStringToUnsignedChars(DFA28_maxS);
    static readonly short[] DFA28_accept = DFA.UnpackEncodedString(DFA28_acceptS);
    static readonly short[] DFA28_special = DFA.UnpackEncodedString(DFA28_specialS);
    static readonly short[][] DFA28_transition = DFA.UnpackEncodedStringArray(DFA28_transitionS);

    protected class DFA28 : DFA
    {
        public DFA28(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 28;
            this.eot = DFA28_eot;
            this.eof = DFA28_eof;
            this.min = DFA28_min;
            this.max = DFA28_max;
            this.accept = DFA28_accept;
            this.special = DFA28_special;
            this.transition = DFA28_transition;

        }

        override public string Description
        {
            get { return "175:1: array_initial_element : ( constant_literal | enumerated_value | IDENTIFIER | structure_initialization | array_initialization );"; }
        }

    }

    const string DFA30_eotS =
        "\x14\uffff";
    const string DFA30_eofS =
        "\x14\uffff";
    const string DFA30_minS =
        "\x01\x18\x09\uffff\x01\x19\x09\uffff";
    const string DFA30_maxS =
        "\x01\x62\x09\uffff\x01\x6a\x09\uffff";
    const string DFA30_acceptS =
        "\x01\uffff\x01\x01\x0f\uffff\x01\x02\x02\uffff";
    const string DFA30_specialS =
        "\x14\uffff}>";
    static readonly string[] DFA30_transitionS = {
            "\x02\x01\x08\uffff\x08\x01\x08\uffff\x07\x01\x11\uffff\x02"+
            "\x01\x0b\uffff\x02\x01\x02\uffff\x01\x01\x01\x0a\x06\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x11\x01\x01\x4f\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA30_eot = DFA.UnpackEncodedString(DFA30_eotS);
    static readonly short[] DFA30_eof = DFA.UnpackEncodedString(DFA30_eofS);
    static readonly char[] DFA30_min = DFA.UnpackEncodedStringToUnsignedChars(DFA30_minS);
    static readonly char[] DFA30_max = DFA.UnpackEncodedStringToUnsignedChars(DFA30_maxS);
    static readonly short[] DFA30_accept = DFA.UnpackEncodedString(DFA30_acceptS);
    static readonly short[] DFA30_special = DFA.UnpackEncodedString(DFA30_specialS);
    static readonly short[][] DFA30_transition = DFA.UnpackEncodedStringArray(DFA30_transitionS);

    protected class DFA30 : DFA
    {
        public DFA30(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 30;
            this.eot = DFA30_eot;
            this.eof = DFA30_eof;
            this.min = DFA30_min;
            this.max = DFA30_max;
            this.accept = DFA30_accept;
            this.special = DFA30_special;
            this.transition = DFA30_transition;

        }

        override public string Description
        {
            get { return "177:1: array_initial_elements : ( array_initial_element | INTEGER '(' ( array_initial_element )? ')' );"; }
        }

    }

    const string DFA29_eotS =
        "\x12\uffff";
    const string DFA29_eofS =
        "\x12\uffff";
    const string DFA29_minS =
        "\x01\x18\x11\uffff";
    const string DFA29_maxS =
        "\x01\x6b\x11\uffff";
    const string DFA29_acceptS =
        "\x01\uffff\x01\x01\x0f\uffff\x01\x02";
    const string DFA29_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA29_transitionS = {
            "\x02\x01\x08\uffff\x08\x01\x08\uffff\x07\x01\x11\uffff\x02"+
            "\x01\x0b\uffff\x02\x01\x02\uffff\x08\x01\x08\uffff\x01\x11",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA29_eot = DFA.UnpackEncodedString(DFA29_eotS);
    static readonly short[] DFA29_eof = DFA.UnpackEncodedString(DFA29_eofS);
    static readonly char[] DFA29_min = DFA.UnpackEncodedStringToUnsignedChars(DFA29_minS);
    static readonly char[] DFA29_max = DFA.UnpackEncodedStringToUnsignedChars(DFA29_maxS);
    static readonly short[] DFA29_accept = DFA.UnpackEncodedString(DFA29_acceptS);
    static readonly short[] DFA29_special = DFA.UnpackEncodedString(DFA29_specialS);
    static readonly short[][] DFA29_transition = DFA.UnpackEncodedStringArray(DFA29_transitionS);

    protected class DFA29 : DFA
    {
        public DFA29(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 29;
            this.eot = DFA29_eot;
            this.eof = DFA29_eof;
            this.min = DFA29_min;
            this.max = DFA29_max;
            this.accept = DFA29_accept;
            this.special = DFA29_special;
            this.transition = DFA29_transition;

        }

        override public string Description
        {
            get { return "177:62: ( array_initial_element )?"; }
        }

    }

    const string DFA32_eotS =
        "\x14\uffff";
    const string DFA32_eofS =
        "\x14\uffff";
    const string DFA32_minS =
        "\x01\x18\x01\x1a\x12\uffff";
    const string DFA32_maxS =
        "\x01\x62\x01\x6b\x12\uffff";
    const string DFA32_acceptS =
        "\x02\uffff\x01\x03\x0c\uffff\x01\x04\x01\x05\x01\x01\x01\x02\x01"+
        "\uffff";
    const string DFA32_specialS =
        "\x14\uffff}>";
    static readonly string[] DFA32_transitionS = {
            "\x01\x0f\x01\x10\x08\uffff\x08\x02\x08\uffff\x07\x02\x11\uffff"+
            "\x02\x02\x0b\uffff\x02\x02\x02\uffff\x01\x01\x07\x02",
            "\x01\x12\x3b\uffff\x01\x11\x14\uffff\x01\x12",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA32_eot = DFA.UnpackEncodedString(DFA32_eotS);
    static readonly short[] DFA32_eof = DFA.UnpackEncodedString(DFA32_eofS);
    static readonly char[] DFA32_min = DFA.UnpackEncodedStringToUnsignedChars(DFA32_minS);
    static readonly char[] DFA32_max = DFA.UnpackEncodedStringToUnsignedChars(DFA32_maxS);
    static readonly short[] DFA32_accept = DFA.UnpackEncodedString(DFA32_acceptS);
    static readonly short[] DFA32_special = DFA.UnpackEncodedString(DFA32_specialS);
    static readonly short[][] DFA32_transition = DFA.UnpackEncodedStringArray(DFA32_transitionS);

    protected class DFA32 : DFA
    {
        public DFA32(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 32;
            this.eot = DFA32_eot;
            this.eof = DFA32_eof;
            this.min = DFA32_min;
            this.max = DFA32_max;
            this.accept = DFA32_accept;
            this.special = DFA32_special;
            this.transition = DFA32_transition;

        }

        override public string Description
        {
            get { return "181:53: ( enumerated_value | IDENTIFIER | constant_literal | array_initialization | structure_initialization )"; }
        }

    }

    const string DFA35_eotS =
        "\x1d\uffff";
    const string DFA35_eofS =
        "\x1d\uffff";
    const string DFA35_minS =
        "\x01\x0b\x01\x15\x01\uffff\x01\x15\x08\uffff\x01\x19\x10\uffff";
    const string DFA35_maxS =
        "\x01\x5b\x01\x6c\x01\uffff\x01\x6c\x08\uffff\x01\x62\x10\uffff";
    const string DFA35_acceptS =
        "\x02\uffff\x01\x02\x03\uffff\x01\x03\x01\x04\x02\uffff\x01\x01"+
        "\x12\uffff";
    const string DFA35_specialS =
        "\x1d\uffff}>";
    static readonly string[] DFA35_transitionS = {
            "\x02\x01\x01\uffff\x01\x07\x0a\uffff\x01\x02\x07\uffff\x1a"+
            "\x02\x02\uffff\x01\x06\x1d\uffff\x01\x03",
            "\x01\x02\x02\uffff\x01\x0a\x53\uffff\x01\x02",
            "",
            "\x01\x0c\x4e\uffff\x01\x02\x07\uffff\x01\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x06\x08\uffff\x08\x02\x08\uffff\x07\x02\x11\uffff\x02"+
            "\x02\x0b\uffff\x02\x02\x02\uffff\x08\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA35_eot = DFA.UnpackEncodedString(DFA35_eotS);
    static readonly short[] DFA35_eof = DFA.UnpackEncodedString(DFA35_eofS);
    static readonly char[] DFA35_min = DFA.UnpackEncodedStringToUnsignedChars(DFA35_minS);
    static readonly char[] DFA35_max = DFA.UnpackEncodedStringToUnsignedChars(DFA35_maxS);
    static readonly short[] DFA35_accept = DFA.UnpackEncodedString(DFA35_acceptS);
    static readonly short[] DFA35_special = DFA.UnpackEncodedString(DFA35_specialS);
    static readonly short[][] DFA35_transition = DFA.UnpackEncodedStringArray(DFA35_transitionS);

    protected class DFA35 : DFA
    {
        public DFA35(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 35;
            this.eot = DFA35_eot;
            this.eof = DFA35_eof;
            this.min = DFA35_min;
            this.max = DFA35_max;
            this.accept = DFA35_accept;
            this.special = DFA35_special;
            this.transition = DFA35_transition;

        }

        override public string Description
        {
            get { return "189:37: ( string_type_declaration | single_element_type_declaration | structure_specification | array_spec_init )"; }
        }

    }

    const string DFA41_eotS =
        "\x29\uffff";
    const string DFA41_eofS =
        "\x29\uffff";
    const string DFA41_minS =
        "\x01\x0b\x05\x15\x0e\uffff\x01\x18\x14\uffff";
    const string DFA41_maxS =
        "\x01\x5b\x05\x6c\x0e\uffff\x01\x62\x14\uffff";
    const string DFA41_acceptS =
        "\x06\uffff\x01\x04\x01\x05\x01\x02\x01\x07\x01\x01\x05\uffff\x01"+
        "\x03\x07\uffff\x01\x06\x0e\uffff\x01\x08\x01\uffff";
    const string DFA41_specialS =
        "\x29\uffff}>";
    static readonly string[] DFA41_transitionS = {
            "\x01\x01\x01\x02\x01\uffff\x01\x06\x0a\uffff\x01\x09\x07\uffff"+
            "\x01\x08\x08\x05\x08\x08\x01\x03\x08\x08\x02\uffff\x01\x07\x1d"+
            "\uffff\x01\x04",
            "\x01\x08\x02\uffff\x01\x0a\x53\uffff\x01\x08",
            "\x01\x08\x02\uffff\x01\x0a\x53\uffff\x01\x08",
            "\x01\x08\x25\uffff\x02\x10\x2f\uffff\x01\x08",
            "\x01\x14\x4e\uffff\x01\x08\x07\uffff\x01\x08",
            "\x01\x08\x03\uffff\x01\x18\x52\uffff\x01\x08",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x27\x01\x07\x08\uffff\x08\x08\x08\uffff\x07\x08\x11\uffff"+
            "\x02\x08\x0b\uffff\x02\x08\x02\uffff\x08\x08",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA41_eot = DFA.UnpackEncodedString(DFA41_eotS);
    static readonly short[] DFA41_eof = DFA.UnpackEncodedString(DFA41_eofS);
    static readonly char[] DFA41_min = DFA.UnpackEncodedStringToUnsignedChars(DFA41_minS);
    static readonly char[] DFA41_max = DFA.UnpackEncodedStringToUnsignedChars(DFA41_maxS);
    static readonly short[] DFA41_accept = DFA.UnpackEncodedString(DFA41_acceptS);
    static readonly short[] DFA41_special = DFA.UnpackEncodedString(DFA41_specialS);
    static readonly short[][] DFA41_transition = DFA.UnpackEncodedStringArray(DFA41_transitionS);

    protected class DFA41 : DFA
    {
        public DFA41(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 41;
            this.eot = DFA41_eot;
            this.eof = DFA41_eof;
            this.min = DFA41_min;
            this.max = DFA41_max;
            this.accept = DFA41_accept;
            this.special = DFA41_special;
            this.transition = DFA41_transition;

        }

        override public string Description
        {
            get { return "201:31: ( string_spec | simple_spec_init | ( BOOL ( R_EDGE | F_EDGE ) ) | array_spec_init | structure_specification | subrange_spec_init | enumerated_spec_init | udt_array_spec_init )"; }
        }

    }

    const string DFA42_eotS =
        "\x12\uffff";
    const string DFA42_eofS =
        "\x12\uffff";
    const string DFA42_minS =
        "\x01\x0b\x01\uffff\x01\x15\x02\x18\x01\uffff\x01\x19\x0b\uffff";
    const string DFA42_maxS =
        "\x01\x5b\x01\uffff\x03\x6c\x01\uffff\x01\x6c\x0b\uffff";
    const string DFA42_acceptS =
        "\x01\uffff\x01\x01\x03\uffff\x01\x03\x01\uffff\x01\x04\x01\x06"+
        "\x04\uffff\x01\x02\x03\uffff\x01\x05";
    const string DFA42_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA42_transitionS = {
            "\x01\x03\x01\x04\x01\uffff\x01\x05\x0a\uffff\x01\x08\x07\uffff"+
            "\x01\x07\x08\x06\x11\x07\x02\uffff\x01\x01\x1d\uffff\x01\x02",
            "",
            "\x01\x01\x4e\uffff\x01\x07\x07\uffff\x01\x07",
            "\x01\x0d\x53\uffff\x01\x07",
            "\x01\x0d\x53\uffff\x01\x07",
            "",
            "\x01\x11\x52\uffff\x01\x07",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA42_eot = DFA.UnpackEncodedString(DFA42_eotS);
    static readonly short[] DFA42_eof = DFA.UnpackEncodedString(DFA42_eofS);
    static readonly char[] DFA42_min = DFA.UnpackEncodedStringToUnsignedChars(DFA42_minS);
    static readonly char[] DFA42_max = DFA.UnpackEncodedStringToUnsignedChars(DFA42_maxS);
    static readonly short[] DFA42_accept = DFA.UnpackEncodedString(DFA42_acceptS);
    static readonly short[] DFA42_special = DFA.UnpackEncodedString(DFA42_specialS);
    static readonly short[][] DFA42_transition = DFA.UnpackEncodedStringArray(DFA42_transitionS);

    protected class DFA42 : DFA
    {
        public DFA42(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 42;
            this.eot = DFA42_eot;
            this.eof = DFA42_eof;
            this.min = DFA42_min;
            this.max = DFA42_max;
            this.accept = DFA42_accept;
            this.special = DFA42_special;
            this.transition = DFA42_transition;

        }

        override public string Description
        {
            get { return "203:33: ( structure_specification | string_spec | array_specification | simple_specification | subrange_specification | enumerated_specification )"; }
        }

    }

    const string DFA51_eotS =
        "\x24\uffff";
    const string DFA51_eofS =
        "\x24\uffff";
    const string DFA51_minS =
        "\x01\x10\x11\uffff\x01\x15\x11\uffff";
    const string DFA51_maxS =
        "\x01\x74\x11\uffff\x01\x64\x11\uffff";
    const string DFA51_acceptS =
        "\x01\uffff\x01\x02\x11\uffff\x01\x01\x10\uffff";
    const string DFA51_specialS =
        "\x24\uffff}>";
    static readonly string[] DFA51_transitionS = {
            "\x01\x01\x01\uffff\x01\x01\x01\uffff\x01\x13\x1e\uffff\x04"+
            "\x01\x0a\uffff\x03\x13\x03\uffff\x03\x13\x02\uffff\x02\x01\x02"+
            "\uffff\x01\x01\x04\uffff\x01\x13\x01\uffff\x02\x01\x01\uffff"+
            "\x01\x01\x01\x12\x01\x01\x03\uffff\x03\x01\x09\uffff\x01\x13"+
            "\x03\uffff\x02\x01\x01\uffff\x02\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x13\x01\uffff\x01\x01\x02\x13\x01\x01\x3b\uffff\x01\x01"+
            "\x02\uffff\x01\x01\x0a\uffff\x01\x13",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA51_eot = DFA.UnpackEncodedString(DFA51_eotS);
    static readonly short[] DFA51_eof = DFA.UnpackEncodedString(DFA51_eofS);
    static readonly char[] DFA51_min = DFA.UnpackEncodedStringToUnsignedChars(DFA51_minS);
    static readonly char[] DFA51_max = DFA.UnpackEncodedStringToUnsignedChars(DFA51_maxS);
    static readonly short[] DFA51_accept = DFA.UnpackEncodedString(DFA51_acceptS);
    static readonly short[] DFA51_special = DFA.UnpackEncodedString(DFA51_specialS);
    static readonly short[][] DFA51_transition = DFA.UnpackEncodedStringArray(DFA51_transitionS);

    protected class DFA51 : DFA
    {
        public DFA51(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 51;
            this.eot = DFA51_eot;
            this.eof = DFA51_eof;
            this.min = DFA51_min;
            this.max = DFA51_max;
            this.accept = DFA51_accept;
            this.special = DFA51_special;
            this.transition = DFA51_transition;

        }

        override public string Description
        {
            get { return "()+ loopback of 217:18: ( statement ';' )+"; }
        }

    }

    const string DFA54_eotS =
        "\x0b\uffff";
    const string DFA54_eofS =
        "\x0b\uffff";
    const string DFA54_minS =
        "\x01\x14\x0a\uffff";
    const string DFA54_maxS =
        "\x01\x6c\x0a\uffff";
    const string DFA54_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\uffff\x01\x04\x04\uffff"+
        "\x01\x05";
    const string DFA54_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA54_transitionS = {
            "\x01\x05\x2c\uffff\x02\x03\x01\x05\x03\uffff\x03\x05\x0b\uffff"+
            "\x01\x0a\x05\uffff\x01\x02\x10\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA54_eot = DFA.UnpackEncodedString(DFA54_eotS);
    static readonly short[] DFA54_eof = DFA.UnpackEncodedString(DFA54_eofS);
    static readonly char[] DFA54_min = DFA.UnpackEncodedStringToUnsignedChars(DFA54_minS);
    static readonly char[] DFA54_max = DFA.UnpackEncodedStringToUnsignedChars(DFA54_maxS);
    static readonly short[] DFA54_accept = DFA.UnpackEncodedString(DFA54_acceptS);
    static readonly short[] DFA54_special = DFA.UnpackEncodedString(DFA54_specialS);
    static readonly short[][] DFA54_transition = DFA.UnpackEncodedStringArray(DFA54_transitionS);

    protected class DFA54 : DFA
    {
        public DFA54(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 54;
            this.eot = DFA54_eot;
            this.eof = DFA54_eof;
            this.min = DFA54_min;
            this.max = DFA54_max;
            this.accept = DFA54_accept;
            this.special = DFA54_special;
            this.transition = DFA54_transition;

        }

        override public string Description
        {
            get { return "237:1: statement : ( ';' | fb_invoke_or_assigment | selection_statement | iteration_statement | RETURN );"; }
        }

    }

    const string DFA56_eotS =
        "\x12\uffff";
    const string DFA56_eofS =
        "\x12\uffff";
    const string DFA56_minS =
        "\x01\x13\x11\uffff";
    const string DFA56_maxS =
        "\x01\x6b\x11\uffff";
    const string DFA56_acceptS =
        "\x01\uffff\x01\x01\x0f\uffff\x01\x02";
    const string DFA56_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA56_transitionS = {
            "\x01\x01\x05\uffff\x01\x01\x08\uffff\x08\x01\x08\uffff\x07"+
            "\x01\x11\uffff\x02\x01\x0b\uffff\x02\x01\x02\uffff\x08\x01\x08"+
            "\uffff\x01\x11",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA56_eot = DFA.UnpackEncodedString(DFA56_eotS);
    static readonly short[] DFA56_eof = DFA.UnpackEncodedString(DFA56_eofS);
    static readonly char[] DFA56_min = DFA.UnpackEncodedStringToUnsignedChars(DFA56_minS);
    static readonly char[] DFA56_max = DFA.UnpackEncodedStringToUnsignedChars(DFA56_maxS);
    static readonly short[] DFA56_accept = DFA.UnpackEncodedString(DFA56_acceptS);
    static readonly short[] DFA56_special = DFA.UnpackEncodedString(DFA56_specialS);
    static readonly short[][] DFA56_transition = DFA.UnpackEncodedStringArray(DFA56_transitionS);

    protected class DFA56 : DFA
    {
        public DFA56(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 56;
            this.eot = DFA56_eot;
            this.eof = DFA56_eof;
            this.min = DFA56_min;
            this.max = DFA56_max;
            this.accept = DFA56_accept;
            this.special = DFA56_special;
            this.transition = DFA56_transition;

        }

        override public string Description
        {
            get { return "239:63: ( param_assignment ( COMMA param_assignment )* )?"; }
        }

    }

    const string DFA61_eotS =
        "\x0c\uffff";
    const string DFA61_eofS =
        "\x0c\uffff";
    const string DFA61_minS =
        "\x01\x14\x0b\uffff";
    const string DFA61_maxS =
        "\x01\x6c\x0b\uffff";
    const string DFA61_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x09\uffff";
    const string DFA61_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA61_transitionS = {
            "\x01\x02\x02\uffff\x01\x01\x29\uffff\x03\x02\x03\uffff\x03"+
            "\x02\x0b\uffff\x01\x02\x05\uffff\x01\x02\x10\uffff\x01\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA61_eot = DFA.UnpackEncodedString(DFA61_eotS);
    static readonly short[] DFA61_eof = DFA.UnpackEncodedString(DFA61_eofS);
    static readonly char[] DFA61_min = DFA.UnpackEncodedStringToUnsignedChars(DFA61_minS);
    static readonly char[] DFA61_max = DFA.UnpackEncodedStringToUnsignedChars(DFA61_maxS);
    static readonly short[] DFA61_accept = DFA.UnpackEncodedString(DFA61_acceptS);
    static readonly short[] DFA61_special = DFA.UnpackEncodedString(DFA61_specialS);
    static readonly short[][] DFA61_transition = DFA.UnpackEncodedStringArray(DFA61_transitionS);

    protected class DFA61 : DFA
    {
        public DFA61(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 61;
            this.eot = DFA61_eot;
            this.eof = DFA61_eof;
            this.min = DFA61_min;
            this.max = DFA61_max;
            this.accept = DFA61_accept;
            this.special = DFA61_special;
            this.transition = DFA61_transition;

        }

        override public string Description
        {
            get { return "251:25: ( ':' )?"; }
        }

    }

    const string DFA63_eotS =
        "\x0b\uffff";
    const string DFA63_eofS =
        "\x0b\uffff";
    const string DFA63_minS =
        "\x01\x33\x0a\uffff";
    const string DFA63_maxS =
        "\x01\x74\x0a\uffff";
    const string DFA63_acceptS =
        "\x01\uffff\x01\x02\x01\uffff\x01\x01\x07\uffff";
    const string DFA63_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA63_transitionS = {
            "\x04\x03\x15\uffff\x01\x01\x0a\uffff\x02\x03\x02\uffff\x02"+
            "\x03\x03\uffff\x03\x03\x11\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA63_eot = DFA.UnpackEncodedString(DFA63_eotS);
    static readonly short[] DFA63_eof = DFA.UnpackEncodedString(DFA63_eofS);
    static readonly char[] DFA63_min = DFA.UnpackEncodedStringToUnsignedChars(DFA63_minS);
    static readonly char[] DFA63_max = DFA.UnpackEncodedStringToUnsignedChars(DFA63_maxS);
    static readonly short[] DFA63_accept = DFA.UnpackEncodedString(DFA63_acceptS);
    static readonly short[] DFA63_special = DFA.UnpackEncodedString(DFA63_specialS);
    static readonly short[][] DFA63_transition = DFA.UnpackEncodedStringArray(DFA63_transitionS);

    protected class DFA63 : DFA
    {
        public DFA63(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 63;
            this.eot = DFA63_eot;
            this.eof = DFA63_eof;
            this.min = DFA63_min;
            this.max = DFA63_max;
            this.accept = DFA63_accept;
            this.special = DFA63_special;
            this.transition = DFA63_transition;

        }

        override public string Description
        {
            get { return "()+ loopback of 255:15: ( case_element )+"; }
        }

    }

    const string DFA65_eotS =
        "\x21\uffff";
    const string DFA65_eofS =
        "\x21\uffff";
    const string DFA65_minS =
        "\x01\x33\x01\uffff\x02\x5c\x07\x17\x16\uffff";
    const string DFA65_maxS =
        "\x01\x62\x01\uffff\x02\x5c\x07\x59\x16\uffff";
    const string DFA65_acceptS =
        "\x01\uffff\x01\x01\x0b\uffff\x01\x02\x09\uffff\x01\x03\x01\uffff"+
        "\x01\x04\x07\uffff";
    const string DFA65_specialS =
        "\x21\uffff}>";
    static readonly string[] DFA65_transitionS = {
            "\x04\x01\x20\uffff\x01\x02\x01\x03\x02\uffff\x01\x08\x01\x04"+
            "\x03\uffff\x01\x05\x01\x06\x01\x07",
            "",
            "\x01\x09",
            "\x01\x0a",
            "\x01\x01\x02\uffff\x01\x01\x3e\uffff\x01\x0d",
            "\x01\x01\x02\uffff\x01\x01\x3e\uffff\x01\x0d",
            "\x01\x01\x02\uffff\x01\x01\x3e\uffff\x01\x0d",
            "\x01\x01\x02\uffff\x01\x01\x3e\uffff\x01\x0d",
            "\x01\x19\x02\uffff\x01\x19\x3b\uffff\x01\x17\x02\uffff\x01"+
            "\x0d",
            "\x01\x01\x02\uffff\x01\x01\x3e\uffff\x01\x0d",
            "\x01\x01\x02\uffff\x01\x01\x3e\uffff\x01\x0d",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA65_eot = DFA.UnpackEncodedString(DFA65_eotS);
    static readonly short[] DFA65_eof = DFA.UnpackEncodedString(DFA65_eofS);
    static readonly char[] DFA65_min = DFA.UnpackEncodedStringToUnsignedChars(DFA65_minS);
    static readonly char[] DFA65_max = DFA.UnpackEncodedStringToUnsignedChars(DFA65_maxS);
    static readonly short[] DFA65_accept = DFA.UnpackEncodedString(DFA65_acceptS);
    static readonly short[] DFA65_special = DFA.UnpackEncodedString(DFA65_specialS);
    static readonly short[][] DFA65_transition = DFA.UnpackEncodedStringArray(DFA65_transitionS);

    protected class DFA65 : DFA
    {
        public DFA65(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 65;
            this.eot = DFA65_eot;
            this.eof = DFA65_eof;
            this.min = DFA65_min;
            this.max = DFA65_max;
            this.accept = DFA65_accept;
            this.special = DFA65_special;
            this.transition = DFA65_transition;

        }

        override public string Description
        {
            get { return "261:1: case_list_element : ( number_literal | subrange | enumerated_value | IDENTIFIER );"; }
        }

    }

    const string DFA68_eotS =
        "\x43\uffff";
    const string DFA68_eofS =
        "\x43\uffff";
    const string DFA68_minS =
        "\x01\x13\x01\x15\x02\uffff\x01\x19\x1e\uffff\x01\x16\x1f\uffff";
    const string DFA68_maxS =
        "\x01\x62\x01\x6b\x02\uffff\x01\x62\x1e\uffff\x01\x6b\x1f\uffff";
    const string DFA68_acceptS =
        "\x02\uffff\x01\x01\x11\uffff\x01\x02\x2e\uffff";
    const string DFA68_specialS =
        "\x43\uffff}>";
    static readonly string[] DFA68_transitionS = {
            "\x01\x04\x05\uffff\x01\x02\x08\uffff\x08\x02\x08\uffff\x07"+
            "\x02\x11\uffff\x02\x02\x0b\uffff\x02\x02\x02\uffff\x01\x01\x07"+
            "\x02",
            "\x01\x02\x01\x14\x01\uffff\x09\x02\x1d\uffff\x03\x02\x11\uffff"+
            "\x03\x02\x01\uffff\x03\x02\x0a\uffff\x02\x02\x06\uffff\x01\x02",
            "",
            "",
            "\x01\x02\x08\uffff\x08\x02\x08\uffff\x07\x02\x11\uffff\x02"+
            "\x02\x0b\uffff\x02\x02\x02\uffff\x01\x23\x07\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x14\x01\uffff\x09\x02\x1d\uffff\x03\x02\x11\uffff\x03"+
            "\x02\x01\uffff\x03\x02\x0a\uffff\x02\x02\x06\uffff\x01\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA68_eot = DFA.UnpackEncodedString(DFA68_eotS);
    static readonly short[] DFA68_eof = DFA.UnpackEncodedString(DFA68_eofS);
    static readonly char[] DFA68_min = DFA.UnpackEncodedStringToUnsignedChars(DFA68_minS);
    static readonly char[] DFA68_max = DFA.UnpackEncodedStringToUnsignedChars(DFA68_maxS);
    static readonly short[] DFA68_accept = DFA.UnpackEncodedString(DFA68_acceptS);
    static readonly short[] DFA68_special = DFA.UnpackEncodedString(DFA68_specialS);
    static readonly short[][] DFA68_transition = DFA.UnpackEncodedStringArray(DFA68_transitionS);

    protected class DFA68 : DFA
    {
        public DFA68(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 68;
            this.eot = DFA68_eot;
            this.eof = DFA68_eof;
            this.min = DFA68_min;
            this.max = DFA68_max;
            this.accept = DFA68_accept;
            this.special = DFA68_special;
            this.transition = DFA68_transition;

        }

        override public string Description
        {
            get { return "264:1: param_assignment : ( ( ( IDENTIFIER ASSIGN )? expression ) | ( ( NOT )? IDENTIFIER ASSIGN2 variable ) );"; }
        }

    }

    const string DFA66_eotS =
        "\x22\uffff";
    const string DFA66_eofS =
        "\x22\uffff";
    const string DFA66_minS =
        "\x01\x13\x01\x15\x20\uffff";
    const string DFA66_maxS =
        "\x01\x62\x01\x6b\x20\uffff";
    const string DFA66_acceptS =
        "\x02\uffff\x01\x02\x0e\uffff\x01\x01\x10\uffff";
    const string DFA66_specialS =
        "\x22\uffff}>";
    static readonly string[] DFA66_transitionS = {
            "\x01\x02\x05\uffff\x01\x02\x08\uffff\x08\x02\x08\uffff\x07"+
            "\x02\x11\uffff\x02\x02\x0b\uffff\x02\x02\x02\uffff\x01\x01\x07"+
            "\x02",
            "\x01\x11\x02\uffff\x09\x02\x1d\uffff\x03\x02\x11\uffff\x03"+
            "\x02\x01\uffff\x03\x02\x0a\uffff\x02\x02\x06\uffff\x01\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA66_eot = DFA.UnpackEncodedString(DFA66_eotS);
    static readonly short[] DFA66_eof = DFA.UnpackEncodedString(DFA66_eofS);
    static readonly char[] DFA66_min = DFA.UnpackEncodedStringToUnsignedChars(DFA66_minS);
    static readonly char[] DFA66_max = DFA.UnpackEncodedStringToUnsignedChars(DFA66_maxS);
    static readonly short[] DFA66_accept = DFA.UnpackEncodedString(DFA66_acceptS);
    static readonly short[] DFA66_special = DFA.UnpackEncodedString(DFA66_specialS);
    static readonly short[][] DFA66_transition = DFA.UnpackEncodedStringArray(DFA66_transitionS);

    protected class DFA66 : DFA
    {
        public DFA66(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 66;
            this.eot = DFA66_eot;
            this.eof = DFA66_eof;
            this.min = DFA66_min;
            this.max = DFA66_max;
            this.accept = DFA66_accept;
            this.special = DFA66_special;
            this.transition = DFA66_transition;

        }

        override public string Description
        {
            get { return "264:21: ( IDENTIFIER ASSIGN )?"; }
        }

    }

    const string DFA69_eotS =
        "\x0c\uffff";
    const string DFA69_eofS =
        "\x0c\uffff";
    const string DFA69_minS =
        "\x01\x0f\x0b\uffff";
    const string DFA69_maxS =
        "\x01\x72\x0b\uffff";
    const string DFA69_acceptS =
        "\x01\uffff\x01\x02\x09\uffff\x01\x01";
    const string DFA69_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA69_transitionS = {
            "\x01\x01\x0a\uffff\x01\x01\x03\uffff\x01\x0b\x25\uffff\x03"+
            "\x01\x0a\uffff\x01\x01\x18\uffff\x03\x01\x05\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA69_eot = DFA.UnpackEncodedString(DFA69_eotS);
    static readonly short[] DFA69_eof = DFA.UnpackEncodedString(DFA69_eofS);
    static readonly char[] DFA69_min = DFA.UnpackEncodedStringToUnsignedChars(DFA69_minS);
    static readonly char[] DFA69_max = DFA.UnpackEncodedStringToUnsignedChars(DFA69_maxS);
    static readonly short[] DFA69_accept = DFA.UnpackEncodedString(DFA69_acceptS);
    static readonly short[] DFA69_special = DFA.UnpackEncodedString(DFA69_specialS);
    static readonly short[][] DFA69_transition = DFA.UnpackEncodedStringArray(DFA69_transitionS);

    protected class DFA69 : DFA
    {
        public DFA69(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 69;
            this.eot = DFA69_eot;
            this.eof = DFA69_eof;
            this.min = DFA69_min;
            this.max = DFA69_max;
            this.accept = DFA69_accept;
            this.special = DFA69_special;
            this.transition = DFA69_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 266:29: ( OR xor_expression )*"; }
        }

    }

    const string DFA70_eotS =
        "\x0d\uffff";
    const string DFA70_eofS =
        "\x0d\uffff";
    const string DFA70_minS =
        "\x01\x0f\x0c\uffff";
    const string DFA70_maxS =
        "\x01\x72\x0c\uffff";
    const string DFA70_acceptS =
        "\x01\uffff\x01\x02\x0a\uffff\x01\x01";
    const string DFA70_specialS =
        "\x0d\uffff}>";
    static readonly string[] DFA70_transitionS = {
            "\x01\x01\x0a\uffff\x01\x01\x03\uffff\x01\x01\x01\x0c\x24\uffff"+
            "\x03\x01\x0a\uffff\x01\x01\x18\uffff\x03\x01\x05\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA70_eot = DFA.UnpackEncodedString(DFA70_eotS);
    static readonly short[] DFA70_eof = DFA.UnpackEncodedString(DFA70_eofS);
    static readonly char[] DFA70_min = DFA.UnpackEncodedStringToUnsignedChars(DFA70_minS);
    static readonly char[] DFA70_max = DFA.UnpackEncodedStringToUnsignedChars(DFA70_maxS);
    static readonly short[] DFA70_accept = DFA.UnpackEncodedString(DFA70_acceptS);
    static readonly short[] DFA70_special = DFA.UnpackEncodedString(DFA70_specialS);
    static readonly short[][] DFA70_transition = DFA.UnpackEncodedStringArray(DFA70_transitionS);

    protected class DFA70 : DFA
    {
        public DFA70(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 70;
            this.eot = DFA70_eot;
            this.eof = DFA70_eof;
            this.min = DFA70_min;
            this.max = DFA70_max;
            this.accept = DFA70_accept;
            this.special = DFA70_special;
            this.transition = DFA70_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 268:33: ( XOR and_expression )*"; }
        }

    }

    const string DFA71_eotS =
        "\x0e\uffff";
    const string DFA71_eofS =
        "\x0e\uffff";
    const string DFA71_minS =
        "\x01\x0f\x0d\uffff";
    const string DFA71_maxS =
        "\x01\x72\x0d\uffff";
    const string DFA71_acceptS =
        "\x01\uffff\x01\x02\x0b\uffff\x01\x01";
    const string DFA71_specialS =
        "\x0e\uffff}>";
    static readonly string[] DFA71_transitionS = {
            "\x01\x01\x0a\uffff\x01\x01\x03\uffff\x02\x01\x24\uffff\x03"+
            "\x01\x0a\uffff\x01\x01\x11\uffff\x01\x0d\x06\uffff\x03\x01\x05"+
            "\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA71_eot = DFA.UnpackEncodedString(DFA71_eotS);
    static readonly short[] DFA71_eof = DFA.UnpackEncodedString(DFA71_eofS);
    static readonly char[] DFA71_min = DFA.UnpackEncodedStringToUnsignedChars(DFA71_minS);
    static readonly char[] DFA71_max = DFA.UnpackEncodedStringToUnsignedChars(DFA71_maxS);
    static readonly short[] DFA71_accept = DFA.UnpackEncodedString(DFA71_acceptS);
    static readonly short[] DFA71_special = DFA.UnpackEncodedString(DFA71_specialS);
    static readonly short[][] DFA71_transition = DFA.UnpackEncodedStringArray(DFA71_transitionS);

    protected class DFA71 : DFA
    {
        public DFA71(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 71;
            this.eot = DFA71_eot;
            this.eof = DFA71_eof;
            this.min = DFA71_min;
            this.max = DFA71_max;
            this.accept = DFA71_accept;
            this.special = DFA71_special;
            this.transition = DFA71_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 270:29: ( AND comparison )*"; }
        }

    }

    const string DFA72_eotS =
        "\x0f\uffff";
    const string DFA72_eofS =
        "\x0f\uffff";
    const string DFA72_minS =
        "\x01\x0f\x0e\uffff";
    const string DFA72_maxS =
        "\x01\x72\x0e\uffff";
    const string DFA72_acceptS =
        "\x01\uffff\x01\x02\x0c\uffff\x01\x01";
    const string DFA72_specialS =
        "\x0f\uffff}>";
    static readonly string[] DFA72_transitionS = {
            "\x01\x01\x0a\uffff\x01\x01\x03\uffff\x02\x01\x01\x0e\x1f\uffff"+
            "\x01\x0e\x03\uffff\x03\x01\x0a\uffff\x01\x01\x11\uffff\x01\x01"+
            "\x06\uffff\x03\x01\x05\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA72_eot = DFA.UnpackEncodedString(DFA72_eotS);
    static readonly short[] DFA72_eof = DFA.UnpackEncodedString(DFA72_eofS);
    static readonly char[] DFA72_min = DFA.UnpackEncodedStringToUnsignedChars(DFA72_minS);
    static readonly char[] DFA72_max = DFA.UnpackEncodedStringToUnsignedChars(DFA72_maxS);
    static readonly short[] DFA72_accept = DFA.UnpackEncodedString(DFA72_acceptS);
    static readonly short[] DFA72_special = DFA.UnpackEncodedString(DFA72_specialS);
    static readonly short[][] DFA72_transition = DFA.UnpackEncodedStringArray(DFA72_transitionS);

    protected class DFA72 : DFA
    {
        public DFA72(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 72;
            this.eot = DFA72_eot;
            this.eof = DFA72_eof;
            this.min = DFA72_min;
            this.max = DFA72_max;
            this.accept = DFA72_accept;
            this.special = DFA72_special;
            this.transition = DFA72_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 272:29: ( ( EQU | NEQ ) equ_expression )*"; }
        }

    }

    const string DFA73_eotS =
        "\x10\uffff";
    const string DFA73_eofS =
        "\x10\uffff";
    const string DFA73_minS =
        "\x01\x0f\x0f\uffff";
    const string DFA73_maxS =
        "\x01\x72\x0f\uffff";
    const string DFA73_acceptS =
        "\x01\uffff\x01\x02\x0d\uffff\x01\x01";
    const string DFA73_specialS =
        "\x10\uffff}>";
    static readonly string[] DFA73_transitionS = {
            "\x01\x01\x0a\uffff\x01\x01\x03\uffff\x03\x01\x1d\uffff\x02"+
            "\x0f\x01\x01\x03\uffff\x03\x01\x0a\uffff\x01\x01\x02\x0f\x0f"+
            "\uffff\x01\x01\x06\uffff\x03\x01\x05\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA73_eot = DFA.UnpackEncodedString(DFA73_eotS);
    static readonly short[] DFA73_eof = DFA.UnpackEncodedString(DFA73_eofS);
    static readonly char[] DFA73_min = DFA.UnpackEncodedStringToUnsignedChars(DFA73_minS);
    static readonly char[] DFA73_max = DFA.UnpackEncodedStringToUnsignedChars(DFA73_maxS);
    static readonly short[] DFA73_accept = DFA.UnpackEncodedString(DFA73_acceptS);
    static readonly short[] DFA73_special = DFA.UnpackEncodedString(DFA73_specialS);
    static readonly short[][] DFA73_transition = DFA.UnpackEncodedStringArray(DFA73_transitionS);

    protected class DFA73 : DFA
    {
        public DFA73(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 73;
            this.eot = DFA73_eot;
            this.eof = DFA73_eof;
            this.min = DFA73_min;
            this.max = DFA73_max;
            this.accept = DFA73_accept;
            this.special = DFA73_special;
            this.transition = DFA73_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 274:33: ( comparison_operators add_expression )*"; }
        }

    }

    const string DFA74_eotS =
        "\x11\uffff";
    const string DFA74_eofS =
        "\x11\uffff";
    const string DFA74_minS =
        "\x01\x0f\x10\uffff";
    const string DFA74_maxS =
        "\x01\x72\x10\uffff";
    const string DFA74_acceptS =
        "\x01\uffff\x01\x02\x0e\uffff\x01\x01";
    const string DFA74_specialS =
        "\x11\uffff}>";
    static readonly string[] DFA74_transitionS = {
            "\x01\x01\x0a\uffff\x01\x01\x03\uffff\x03\x01\x1d\uffff\x03"+
            "\x01\x03\uffff\x03\x01\x0a\uffff\x03\x01\x03\uffff\x02\x10\x0a"+
            "\uffff\x01\x01\x06\uffff\x03\x01\x05\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA74_eot = DFA.UnpackEncodedString(DFA74_eotS);
    static readonly short[] DFA74_eof = DFA.UnpackEncodedString(DFA74_eofS);
    static readonly char[] DFA74_min = DFA.UnpackEncodedStringToUnsignedChars(DFA74_minS);
    static readonly char[] DFA74_max = DFA.UnpackEncodedStringToUnsignedChars(DFA74_maxS);
    static readonly short[] DFA74_accept = DFA.UnpackEncodedString(DFA74_acceptS);
    static readonly short[] DFA74_special = DFA.UnpackEncodedString(DFA74_specialS);
    static readonly short[][] DFA74_transition = DFA.UnpackEncodedStringArray(DFA74_transitionS);

    protected class DFA74 : DFA
    {
        public DFA74(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 74;
            this.eot = DFA74_eot;
            this.eof = DFA74_eof;
            this.min = DFA74_min;
            this.max = DFA74_max;
            this.accept = DFA74_accept;
            this.special = DFA74_special;
            this.transition = DFA74_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 276:23: ( ( PLUS | NEG ) term )*"; }
        }

    }

    const string DFA75_eotS =
        "\x14\uffff";
    const string DFA75_eofS =
        "\x14\uffff";
    const string DFA75_minS =
        "\x01\x0f\x13\uffff";
    const string DFA75_maxS =
        "\x01\x72\x13\uffff";
    const string DFA75_acceptS =
        "\x01\uffff\x01\x02\x0f\uffff\x01\x01\x02\uffff";
    const string DFA75_specialS =
        "\x14\uffff}>";
    static readonly string[] DFA75_transitionS = {
            "\x01\x01\x0a\uffff\x01\x01\x03\x11\x03\x01\x1d\uffff\x03\x01"+
            "\x03\uffff\x03\x01\x0a\uffff\x03\x01\x03\uffff\x02\x01\x0a\uffff"+
            "\x01\x01\x06\uffff\x03\x01\x05\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA75_eot = DFA.UnpackEncodedString(DFA75_eotS);
    static readonly short[] DFA75_eof = DFA.UnpackEncodedString(DFA75_eofS);
    static readonly char[] DFA75_min = DFA.UnpackEncodedStringToUnsignedChars(DFA75_minS);
    static readonly char[] DFA75_max = DFA.UnpackEncodedStringToUnsignedChars(DFA75_maxS);
    static readonly short[] DFA75_accept = DFA.UnpackEncodedString(DFA75_acceptS);
    static readonly short[] DFA75_special = DFA.UnpackEncodedString(DFA75_specialS);
    static readonly short[][] DFA75_transition = DFA.UnpackEncodedStringArray(DFA75_transitionS);

    protected class DFA75 : DFA
    {
        public DFA75(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 75;
            this.eot = DFA75_eot;
            this.eof = DFA75_eof;
            this.min = DFA75_min;
            this.max = DFA75_max;
            this.accept = DFA75_accept;
            this.special = DFA75_special;
            this.transition = DFA75_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 278:25: ( multiply_operator power_expression )*"; }
        }

    }

    const string DFA76_eotS =
        "\x15\uffff";
    const string DFA76_eofS =
        "\x15\uffff";
    const string DFA76_minS =
        "\x01\x0f\x14\uffff";
    const string DFA76_maxS =
        "\x01\x72\x14\uffff";
    const string DFA76_acceptS =
        "\x01\uffff\x01\x02\x12\uffff\x01\x01";
    const string DFA76_specialS =
        "\x15\uffff}>";
    static readonly string[] DFA76_transitionS = {
            "\x01\x01\x0a\uffff\x07\x01\x1d\uffff\x03\x01\x03\uffff\x03"+
            "\x01\x0a\uffff\x03\x01\x01\x14\x02\uffff\x02\x01\x0a\uffff\x01"+
            "\x01\x06\uffff\x03\x01\x05\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA76_eot = DFA.UnpackEncodedString(DFA76_eotS);
    static readonly short[] DFA76_eof = DFA.UnpackEncodedString(DFA76_eofS);
    static readonly char[] DFA76_min = DFA.UnpackEncodedStringToUnsignedChars(DFA76_minS);
    static readonly char[] DFA76_max = DFA.UnpackEncodedStringToUnsignedChars(DFA76_maxS);
    static readonly short[] DFA76_accept = DFA.UnpackEncodedString(DFA76_acceptS);
    static readonly short[] DFA76_special = DFA.UnpackEncodedString(DFA76_specialS);
    static readonly short[][] DFA76_transition = DFA.UnpackEncodedStringArray(DFA76_transitionS);

    protected class DFA76 : DFA
    {
        public DFA76(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 76;
            this.eot = DFA76_eot;
            this.eof = DFA76_eof;
            this.min = DFA76_min;
            this.max = DFA76_max;
            this.accept = DFA76_accept;
            this.special = DFA76_special;
            this.transition = DFA76_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 280:37: ( POW unary_expression )*"; }
        }

    }

    const string DFA77_eotS =
        "\x11\uffff";
    const string DFA77_eofS =
        "\x11\uffff";
    const string DFA77_minS =
        "\x01\x13\x10\uffff";
    const string DFA77_maxS =
        "\x01\x62\x10\uffff";
    const string DFA77_acceptS =
        "\x01\uffff\x02\x01\x01\uffff\x01\x02\x0c\uffff";
    const string DFA77_specialS =
        "\x11\uffff}>";
    static readonly string[] DFA77_transitionS = {
            "\x01\x02\x05\uffff\x01\x04\x08\uffff\x08\x04\x08\uffff\x07"+
            "\x04\x11\uffff\x02\x04\x0b\uffff\x01\x01\x01\x02\x02\uffff\x08"+
            "\x04",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA77_eot = DFA.UnpackEncodedString(DFA77_eotS);
    static readonly short[] DFA77_eof = DFA.UnpackEncodedString(DFA77_eofS);
    static readonly char[] DFA77_min = DFA.UnpackEncodedStringToUnsignedChars(DFA77_minS);
    static readonly char[] DFA77_max = DFA.UnpackEncodedStringToUnsignedChars(DFA77_maxS);
    static readonly short[] DFA77_accept = DFA.UnpackEncodedString(DFA77_acceptS);
    static readonly short[] DFA77_special = DFA.UnpackEncodedString(DFA77_specialS);
    static readonly short[][] DFA77_transition = DFA.UnpackEncodedStringArray(DFA77_transitionS);

    protected class DFA77 : DFA
    {
        public DFA77(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 77;
            this.eot = DFA77_eot;
            this.eof = DFA77_eof;
            this.min = DFA77_min;
            this.max = DFA77_max;
            this.accept = DFA77_accept;
            this.special = DFA77_special;
            this.transition = DFA77_transition;

        }

        override public string Description
        {
            get { return "282:20: ( unary_operator )?"; }
        }

    }

    const string DFA78_eotS =
        "\x28\uffff";
    const string DFA78_eofS =
        "\x28\uffff";
    const string DFA78_minS =
        "\x01\x19\x01\x0f\x26\uffff";
    const string DFA78_maxS =
        "\x01\x62\x01\x72\x26\uffff";
    const string DFA78_acceptS =
        "\x02\uffff\x01\x03\x0c\uffff\x01\x05\x01\x01\x01\x04\x01\x02\x15"+
        "\uffff";
    const string DFA78_specialS =
        "\x28\uffff}>";
    static readonly string[] DFA78_transitionS = {
            "\x01\x0f\x08\uffff\x08\x02\x08\uffff\x07\x02\x11\uffff\x02"+
            "\x02\x0b\uffff\x02\x02\x02\uffff\x01\x01\x07\x02",
            "\x01\x12\x08\uffff\x01\x12\x01\x10\x07\x12\x1d\uffff\x03\x12"+
            "\x03\uffff\x03\x12\x0a\uffff\x04\x12\x01\uffff\x01\x11\x02\x12"+
            "\x0a\uffff\x02\x12\x05\uffff\x03\x12\x05\uffff\x01\x12",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA78_eot = DFA.UnpackEncodedString(DFA78_eotS);
    static readonly short[] DFA78_eof = DFA.UnpackEncodedString(DFA78_eofS);
    static readonly char[] DFA78_min = DFA.UnpackEncodedStringToUnsignedChars(DFA78_minS);
    static readonly char[] DFA78_max = DFA.UnpackEncodedStringToUnsignedChars(DFA78_maxS);
    static readonly short[] DFA78_accept = DFA.UnpackEncodedString(DFA78_acceptS);
    static readonly short[] DFA78_special = DFA.UnpackEncodedString(DFA78_specialS);
    static readonly short[][] DFA78_transition = DFA.UnpackEncodedStringArray(DFA78_transitionS);

    protected class DFA78 : DFA
    {
        public DFA78(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 78;
            this.eot = DFA78_eot;
            this.eof = DFA78_eof;
            this.min = DFA78_min;
            this.max = DFA78_max;
            this.accept = DFA78_accept;
            this.special = DFA78_special;
            this.transition = DFA78_transition;

        }

        override public string Description
        {
            get { return "284:1: primary_expression : ( function_call | variable | constant_literal | enumerated_value | '(' expression ')' );"; }
        }

    }

    const string DFA82_eotS =
        "\x19\uffff";
    const string DFA82_eofS =
        "\x19\uffff";
    const string DFA82_minS =
        "\x01\x0f\x18\uffff";
    const string DFA82_maxS =
        "\x01\x72\x18\uffff";
    const string DFA82_acceptS =
        "\x01\uffff\x01\x03\x15\uffff\x01\x01\x01\x02";
    const string DFA82_specialS =
        "\x19\uffff}>";
    static readonly string[] DFA82_transitionS = {
            "\x01\x01\x05\uffff\x01\x01\x02\uffff\x01\x18\x08\x01\x1d\uffff"+
            "\x03\x01\x03\uffff\x03\x01\x0a\uffff\x04\x01\x02\uffff\x02\x01"+
            "\x0a\uffff\x01\x01\x01\x17\x05\uffff\x03\x01\x05\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA82_eot = DFA.UnpackEncodedString(DFA82_eotS);
    static readonly short[] DFA82_eof = DFA.UnpackEncodedString(DFA82_eofS);
    static readonly char[] DFA82_min = DFA.UnpackEncodedStringToUnsignedChars(DFA82_minS);
    static readonly char[] DFA82_max = DFA.UnpackEncodedStringToUnsignedChars(DFA82_maxS);
    static readonly short[] DFA82_accept = DFA.UnpackEncodedString(DFA82_acceptS);
    static readonly short[] DFA82_special = DFA.UnpackEncodedString(DFA82_specialS);
    static readonly short[][] DFA82_transition = DFA.UnpackEncodedStringArray(DFA82_transitionS);

    protected class DFA82 : DFA
    {
        public DFA82(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 82;
            this.eot = DFA82_eot;
            this.eof = DFA82_eof;
            this.min = DFA82_min;
            this.max = DFA82_max;
            this.accept = DFA82_accept;
            this.special = DFA82_special;
            this.transition = DFA82_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 298:37: ( DOT IDENTIFIER | subscript_list )*"; }
        }

    }

    const string DFA84_eotS =
        "\x13\uffff";
    const string DFA84_eofS =
        "\x13\uffff";
    const string DFA84_minS =
        "\x01\x0b\x01\uffff\x01\x06\x10\uffff";
    const string DFA84_maxS =
        "\x01\x5b\x01\uffff\x01\x6c\x10\uffff";
    const string DFA84_acceptS =
        "\x01\uffff\x01\x01\x01\uffff\x01\x03\x01\x02\x0e\uffff";
    const string DFA84_specialS =
        "\x13\uffff}>";
    static readonly string[] DFA84_transitionS = {
            "\x02\x01\x14\uffff\x1a\x01\x20\uffff\x01\x02",
            "",
            "\x01\x04\x02\uffff\x02\x04\x02\uffff\x01\x04\x06\uffff\x02"+
            "\x04\x2b\uffff\x03\x04\x03\uffff\x03\x04\x0b\uffff\x01\x04\x05"+
            "\uffff\x01\x04\x08\uffff\x01\x03\x07\uffff\x01\x04",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA84_eot = DFA.UnpackEncodedString(DFA84_eotS);
    static readonly short[] DFA84_eof = DFA.UnpackEncodedString(DFA84_eofS);
    static readonly char[] DFA84_min = DFA.UnpackEncodedStringToUnsignedChars(DFA84_minS);
    static readonly char[] DFA84_max = DFA.UnpackEncodedStringToUnsignedChars(DFA84_maxS);
    static readonly short[] DFA84_accept = DFA.UnpackEncodedString(DFA84_acceptS);
    static readonly short[] DFA84_special = DFA.UnpackEncodedString(DFA84_specialS);
    static readonly short[][] DFA84_transition = DFA.UnpackEncodedStringArray(DFA84_transitionS);

    protected class DFA84 : DFA
    {
        public DFA84(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 84;
            this.eot = DFA84_eot;
            this.eof = DFA84_eof;
            this.min = DFA84_min;
            this.max = DFA84_max;
            this.accept = DFA84_accept;
            this.special = DFA84_special;
            this.transition = DFA84_transition;

        }

        override public string Description
        {
            get { return "308:1: simple_specification : ( elemetary_type_name | IDENTIFIER | IDENTIFIER DOT IDENTIFIER );"; }
        }

    }

 

    public static readonly BitSet FOLLOW_program_declaration_in_mpal1548 = new BitSet(new ulong[]{0x0000000000020030UL,0x0000000000008000UL});
    public static readonly BitSet FOLLOW_data_type_declaration_in_mpal1552 = new BitSet(new ulong[]{0x0000000000020030UL,0x0000000000008000UL});
    public static readonly BitSet FOLLOW_function_declaration_in_mpal1555 = new BitSet(new ulong[]{0x0000000000020030UL,0x0000000000008000UL});
    public static readonly BitSet FOLLOW_function_block_declaration_in_mpal1560 = new BitSet(new ulong[]{0x0000000000020030UL,0x0000000000008000UL});
    public static readonly BitSet FOLLOW_EOF_in_mpal1565 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PROGRAM_in_program1576 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_program1579 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_io_var_declarations_in_program1582 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_other_var_declarations_in_program1587 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_data_type_declaration_in_program1591 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_function_body_in_program1595 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_END_PROGRAM_in_program1597 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PROGRAM_in_program_declaration1606 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_program_declaration1609 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_io_var_declarations_in_program_declaration1613 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_other_var_declarations_in_program_declaration1617 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_function_body_in_program_declaration1621 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000004000000UL});
    public static readonly BitSet FOLLOW_END_PROGRAM_in_program_declaration1623 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FUNCTION_in_function_declaration1632 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_function_type_in_function_declaration1635 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_io_var_declarations_in_function_declaration1639 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_var_declarations_in_function_declaration1643 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_function_body_in_function_declaration1647 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_END_FUNCTION_in_function_declaration1649 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_function_type1657 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COLON_in_function_type1659 = new BitSet(new ulong[]{0x07FFFFFE00001800UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_simple_specification_in_function_type1662 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FUNCTION_BLOCK_in_function_block_declaration1674 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_function_block_declaration1677 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_io_var_declarations_in_function_block_declaration1681 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_other_var_declarations_in_function_block_declaration1685 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_function_body_in_function_block_declaration1690 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_END_FUNCTION_BLOCK_in_function_block_declaration1693 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRACKED_in_string_type_size1701 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_INTEGER_in_string_type_size1704 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000040000000000UL});
    public static readonly BitSet FOLLOW_106_in_string_type_size1706 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_character_string0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_string_type_declaration1729 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_string_type_size_in_string_type_declaration1738 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_string_type_decl_init_in_string_type_declaration1740 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_string_type_decl_init1751 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000060000000UL});
    public static readonly BitSet FOLLOW_character_string_in_string_type_decl_init1754 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_real_type_name_in_real_literal1764 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SHARP_in_real_literal1766 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000081800000UL});
    public static readonly BitSet FOLLOW_real_literal_body_in_real_literal1771 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PLUS_in_real_literal_body1781 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000080000000UL});
    public static readonly BitSet FOLLOW_NEG_in_real_literal_body1784 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000080000000UL});
    public static readonly BitSet FOLLOW_REAL_CONSTANT_in_real_literal_body1790 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PLUS_in_integer_literal1802 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_NEG_in_integer_literal1805 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_INTEGER_in_integer_literal1810 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BINARY_INTEGER_in_integer_literal1815 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OCTAL_INTEGER_in_integer_literal1819 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_HEX_INTEGER_in_integer_literal1823 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_bit_string_literal1832 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SHARP_in_bit_string_literal1848 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000711800000UL});
    public static readonly BitSet FOLLOW_integer_literal_in_bit_string_literal1854 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_integer_type_name_in_spec_integer_literal1864 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SHARP_in_spec_integer_literal1866 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000711800000UL});
    public static readonly BitSet FOLLOW_integer_literal_in_spec_integer_literal1871 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_real_literal_in_constant_literal1880 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_spec_integer_literal_in_constant_literal1883 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_character_string_in_constant_literal1887 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_bool_literal_in_constant_literal1891 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_bit_string_literal_in_constant_literal1895 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_simple_specification_in_simple_spec_init1905 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_simple_spec_init1909 = new BitSet(new ulong[]{0x01FC03FC00000000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_constant_literal_in_simple_spec_init1913 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_simple_spec_init1916 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_enumerated_value1929 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SHARP_in_enumerated_value1931 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_enumerated_value1933 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LRBRACKED_in_enumerated_specification1944 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_enumerated_value_in_enumerated_specification1948 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_enumerated_specification1950 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_enumerated_specification1955 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_enumerated_value_in_enumerated_specification1959 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_enumerated_specification1961 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_107_in_enumerated_specification1966 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_enumerated_specification_in_enumerated_spec_init1977 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_enumerated_init_in_enumerated_spec_init1980 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_enumerated_init1990 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_enumerated_value_in_enumerated_init1994 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_enumerated_init1996 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_simple_spec_init_in_single_element_type_declaration2007 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_subrange_spec_init_in_single_element_type_declaration2011 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_enumerated_spec_init_in_single_element_type_declaration2016 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_subrange_specification_in_subrange_spec_init2027 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_subrange_spec_init2030 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_INTEGER_in_subrange_spec_init2032 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_integer_type_name_in_subrange_specification2042 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_LRBRACKED_in_subrange_specification2044 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000719800000UL});
    public static readonly BitSet FOLLOW_subrange_in_subrange_specification2046 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_107_in_subrange_specification2048 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_array_specification_in_array_spec_init2057 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_array_spec_init2062 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_array_initialization_in_array_spec_init2065 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRACKED_in_array_range2077 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000719800000UL});
    public static readonly BitSet FOLLOW_subrange_in_array_range2080 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000040000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_array_range2083 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000719800000UL});
    public static readonly BitSet FOLLOW_subrange_in_array_range2086 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000040000000000UL});
    public static readonly BitSet FOLLOW_106_in_array_range2090 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_udt_array_spec_init2100 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_udt_array_spec_init2102 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_array_initialization_in_udt_array_spec_init2105 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_elemetary_type_name_in_non_generic_type_name2113 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_non_generic_type_name2117 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_structure_declaration_in_non_generic_type_name2122 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_string_type_declaration_in_non_generic_type_name2125 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ARRAY_in_array_specification2134 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_array_range_in_array_specification2137 = new BitSet(new ulong[]{0x0000000000008000UL});
    public static readonly BitSet FOLLOW_OF_in_array_specification2139 = new BitSet(new ulong[]{0x27FFFFFE00001800UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_non_generic_type_name_in_array_specification2142 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_structure_element_declaration2151 = new BitSet(new ulong[]{0x0000000004800000UL});
    public static readonly BitSet FOLLOW_COMMA_in_structure_element_declaration2154 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_structure_element_declaration2157 = new BitSet(new ulong[]{0x0000000004800000UL});
    public static readonly BitSet FOLLOW_COLON_in_structure_element_declaration2161 = new BitSet(new ulong[]{0x27FFFFFE02005800UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_simple_spec_init_in_structure_element_declaration2166 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_subrange_spec_init_in_structure_element_declaration2170 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_enumerated_spec_init_in_structure_element_declaration2174 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_array_spec_init_in_structure_element_declaration2178 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_string_spec_in_structure_element_declaration2182 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_initialized_structure_in_structure_element_declaration2185 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_structure_declaration_in_structure_element_declaration2189 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRUCT_in_structure_declaration2198 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_structure_element_declaration_in_structure_declaration2201 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_structure_declaration2203 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000200008000000UL});
    public static readonly BitSet FOLLOW_structure_element_declaration_in_structure_declaration2206 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_structure_declaration2208 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000200008000000UL});
    public static readonly BitSet FOLLOW_109_in_structure_declaration2213 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constant_literal_in_array_initial_element2222 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_enumerated_value_in_array_initial_element2226 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_array_initial_element2230 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_structure_initialization_in_array_initial_element2234 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_array_initialization_in_array_initial_element2238 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_array_initial_element_in_array_initial_elements2246 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INTEGER_in_array_initial_elements2250 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_LRBRACKED_in_array_initial_elements2252 = new BitSet(new ulong[]{0x01FC03FC03000000UL,0x00000807F9800C00UL});
    public static readonly BitSet FOLLOW_array_initial_element_in_array_initial_elements2255 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_107_in_array_initial_elements2259 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRACKED_in_array_initialization2268 = new BitSet(new ulong[]{0x01FC03FC03000000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_array_initial_elements_in_array_initialization2271 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000040000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_array_initialization2274 = new BitSet(new ulong[]{0x01FC03FC03000000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_array_initial_elements_in_array_initialization2277 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000040000000000UL});
    public static readonly BitSet FOLLOW_106_in_array_initialization2281 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_structure_element_initialization2291 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_structure_element_initialization2293 = new BitSet(new ulong[]{0x01FC03FC03000000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_enumerated_value_in_structure_element_initialization2297 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_structure_element_initialization2300 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constant_literal_in_structure_element_initialization2304 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_array_initialization_in_structure_element_initialization2308 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_structure_initialization_in_structure_element_initialization2312 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LRBRACKED_in_structure_initialization2321 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_structure_element_initialization_in_structure_initialization2324 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_structure_initialization2327 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_structure_element_initialization_in_structure_initialization2330 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_107_in_structure_initialization2334 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_initialized_structure2343 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_initialized_structure2345 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_structure_initialization_in_initialized_structure2349 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_structure_declaration_in_structure_specification2357 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_initialized_structure_in_structure_specification2361 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_type_declaration2370 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COLON_in_type_declaration2372 = new BitSet(new ulong[]{0x27FFFFFE02005800UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_string_type_declaration_in_type_declaration2377 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_single_element_type_declaration_in_type_declaration2382 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_structure_specification_in_type_declaration2386 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_array_spec_init_in_type_declaration2391 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TYPE_in_data_type_declaration2401 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_type_declaration_in_data_type_declaration2404 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_data_type_declaration2406 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000400008000000UL});
    public static readonly BitSet FOLLOW_type_declaration_in_data_type_declaration2410 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_data_type_declaration2412 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000400008000000UL});
    public static readonly BitSet FOLLOW_110_in_data_type_declaration2417 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_input_output_declarations_in_io_var_declarations2427 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_input_declarations_in_io_var_declarations2431 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_output_declarations_in_io_var_declarations2436 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VAR_IN_OUT_in_input_output_declarations2445 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_var_declaration_in_input_output_declarations2450 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_input_output_declarations2452 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000800008000000UL});
    public static readonly BitSet FOLLOW_111_in_input_output_declarations2457 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VAR_INPUT_in_input_declarations2466 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_var_init_decl_in_input_declarations2470 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_input_declarations2472 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000800008000000UL});
    public static readonly BitSet FOLLOW_111_in_input_declarations2478 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VAR_OUTPUT_in_output_declarations2487 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_var_init_decl_in_output_declarations2491 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_output_declarations2493 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000800008000000UL});
    public static readonly BitSet FOLLOW_111_in_output_declarations2498 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_var_list_in_var_init_decl2507 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COLON_in_var_init_decl2509 = new BitSet(new ulong[]{0x27FFFFFE02005800UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_string_spec_in_var_init_decl2513 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_simple_spec_init_in_var_init_decl2518 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BOOL_in_var_init_decl2523 = new BitSet(new ulong[]{0x1800000000000000UL});
    public static readonly BitSet FOLLOW_set_in_var_init_decl2525 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_array_spec_init_in_var_init_decl2535 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_structure_specification_in_var_init_decl2538 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_subrange_spec_init_in_var_init_decl2542 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_enumerated_spec_init_in_var_init_decl2546 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_udt_array_spec_init_in_var_init_decl2549 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_var_list_in_var_declaration2559 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COLON_in_var_declaration2561 = new BitSet(new ulong[]{0x27FFFFFE02005800UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_structure_specification_in_var_declaration2565 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_string_spec_in_var_declaration2568 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_array_specification_in_var_declaration2572 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_simple_specification_in_var_declaration2576 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_subrange_specification_in_var_declaration2580 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_enumerated_specification_in_var_declaration2584 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_var_list2594 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FOLLOW_COMMA_in_var_list2597 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_var_list2600 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FOLLOW_STRING_in_string_spec2612 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_WSTRING_in_string_spec2617 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_LBRACKED_in_string_spec2622 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000010000000UL});
    public static readonly BitSet FOLLOW_INTEGER_in_string_spec2625 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000040000000000UL});
    public static readonly BitSet FOLLOW_106_in_string_spec2627 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_string_spec2632 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000060000000UL});
    public static readonly BitSet FOLLOW_character_string_in_string_spec2635 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_var_declarations_in_other_var_declarations2646 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_temp_var_decls_in_other_var_declarations2650 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VAR_in_var_declarations2658 = new BitSet(new ulong[]{0x0000000000000100UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_CONSTANT_in_var_declarations2662 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_var_init_decl_in_var_declarations2666 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_var_declarations2668 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000800008000000UL});
    public static readonly BitSet FOLLOW_var_init_decl_in_var_declarations2673 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_var_declarations2675 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000800008000000UL});
    public static readonly BitSet FOLLOW_111_in_var_declarations2681 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VAR_TEMP_in_temp_var_decls2690 = new BitSet(new ulong[]{0x0000000000000100UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_CONSTANT_in_temp_var_decls2694 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_var_init_decl_in_temp_var_decls2698 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_temp_var_decls2700 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000800008000000UL});
    public static readonly BitSet FOLLOW_var_init_decl_in_temp_var_decls2705 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_temp_var_decls2707 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000800008000000UL});
    public static readonly BitSet FOLLOW_111_in_temp_var_decls2713 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_statement_list_in_function_body2722 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_statement_in_statement_list2732 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000100000000000UL});
    public static readonly BitSet FOLLOW_108_in_statement_list2734 = new BitSet(new ulong[]{0x00000000001026C2UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_DO_in_do_statement2745 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_statement_list_in_do_statement2748 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FOR_in_for_statement2756 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_for_assign_in_for_statement2759 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000010UL});
    public static readonly BitSet FOLLOW_for_to_in_for_statement2762 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000060UL});
    public static readonly BitSet FOLLOW_for_by_in_for_statement2765 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000060UL});
    public static readonly BitSet FOLLOW_do_statement_in_for_statement2769 = new BitSet(new ulong[]{0x0000000000000000UL,0x0001000000000000UL});
    public static readonly BitSet FOLLOW_112_in_for_statement2771 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_for_assign2780 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_for_assign2782 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_for_assign2785 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TO_in_for_to2793 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_for_to2796 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BY_in_for_by2805 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_for_by2808 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHILE_in_while_statement2816 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_while_statement2819 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000060UL});
    public static readonly BitSet FOLLOW_do_statement_in_while_statement2821 = new BitSet(new ulong[]{0x0000000000000000UL,0x0002000000000000UL});
    public static readonly BitSet FOLLOW_113_in_while_statement2823 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_REPEAT_in_repeat_statement2832 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_statement_list_in_repeat_statement2835 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000010000UL});
    public static readonly BitSet FOLLOW_repeat_until_in_repeat_statement2837 = new BitSet(new ulong[]{0x0000000000000000UL,0x0004000000000000UL});
    public static readonly BitSet FOLLOW_114_in_repeat_statement2839 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_UNTIL_in_repeat_until2848 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_repeat_until2851 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_for_statement_in_iteration_statement2859 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_while_statement_in_iteration_statement2864 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_repeat_statement_in_iteration_statement2868 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_EXIT_in_iteration_statement2872 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CONTINUE_in_iteration_statement2875 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_108_in_statement2883 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_fb_invoke_or_assigment_in_statement2887 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selection_statement_in_statement2891 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_iteration_statement_in_statement2895 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RETURN_in_statement2898 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_fb_invoke_or_assigment2906 = new BitSet(new ulong[]{0x0000000002200000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_fb_invoke_or_assigment2909 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_fb_invoke_or_assigment2912 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LRBRACKED_in_fb_invoke_or_assigment2917 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000807F9800C00UL});
    public static readonly BitSet FOLLOW_param_assignment_in_fb_invoke_or_assigment2921 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_fb_invoke_or_assigment2924 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_param_assignment_in_fb_invoke_or_assigment2927 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_107_in_fb_invoke_or_assigment2933 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_if_statement_in_selection_statement2947 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_case_statement_in_selection_statement2951 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IF_in_if_statement2959 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_if_statement2962 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000020000UL});
    public static readonly BitSet FOLLOW_if_then_in_if_statement2964 = new BitSet(new ulong[]{0x0000000000000000UL,0x0008000000003000UL});
    public static readonly BitSet FOLLOW_if_elif_in_if_statement2967 = new BitSet(new ulong[]{0x0000000000000000UL,0x0008000000003000UL});
    public static readonly BitSet FOLLOW_else_statement_in_if_statement2972 = new BitSet(new ulong[]{0x0000000000000000UL,0x0008000000000000UL});
    public static readonly BitSet FOLLOW_115_in_if_statement2976 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_THEN_in_if_then2993 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_statement_list_in_if_then2996 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ELSIF_in_if_elif3004 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_if_elif3007 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000020000UL});
    public static readonly BitSet FOLLOW_if_then_in_if_elif3009 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ELSE_in_else_statement3017 = new BitSet(new ulong[]{0x00000000009026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_COLON_in_else_statement3021 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_statement_list_in_else_statement3026 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CASE_in_case_statement3035 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_case_statement3038 = new BitSet(new ulong[]{0x0000000000008000UL});
    public static readonly BitSet FOLLOW_case_of_in_case_statement3040 = new BitSet(new ulong[]{0x0000000000000000UL,0x0010000000001000UL});
    public static readonly BitSet FOLLOW_else_statement_in_case_statement3042 = new BitSet(new ulong[]{0x0000000000000000UL,0x0010000000000000UL});
    public static readonly BitSet FOLLOW_116_in_case_statement3045 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OF_in_case_of3054 = new BitSet(new ulong[]{0x01FC03FC00000000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_case_element_in_case_of3058 = new BitSet(new ulong[]{0x01FC03FC00000002UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_case_list_in_case_element3068 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COLON_in_case_element3070 = new BitSet(new ulong[]{0x00000000001026C0UL,0x000010000820838EUL});
    public static readonly BitSet FOLLOW_statement_list_in_case_element3073 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_case_list_element_in_case_list3081 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FOLLOW_COMMA_in_case_list3084 = new BitSet(new ulong[]{0x01FC03FC00000000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_case_list_element_in_case_list3087 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FOLLOW_number_literal_in_case_list_element3097 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_subrange_in_case_list_element3100 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_enumerated_value_in_case_list_element3105 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_case_list_element3109 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_param_assignment3121 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_param_assignment3123 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_param_assignment3128 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOT_in_param_assignment3134 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_param_assignment3137 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_ASSIGN2_in_param_assignment3139 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_variable_in_param_assignment3142 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_xor_expression_in_expression3152 = new BitSet(new ulong[]{0x0000000040000002UL});
    public static readonly BitSet FOLLOW_OR_in_expression3155 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_xor_expression_in_expression3158 = new BitSet(new ulong[]{0x0000000040000002UL});
    public static readonly BitSet FOLLOW_and_expression_in_xor_expression3168 = new BitSet(new ulong[]{0x0000000080000002UL});
    public static readonly BitSet FOLLOW_XOR_in_xor_expression3171 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_and_expression_in_xor_expression3174 = new BitSet(new ulong[]{0x0000000080000002UL});
    public static readonly BitSet FOLLOW_comparison_in_and_expression3184 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000800000000UL});
    public static readonly BitSet FOLLOW_AND_in_and_expression3187 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_comparison_in_and_expression3190 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000800000000UL});
    public static readonly BitSet FOLLOW_equ_expression_in_comparison3200 = new BitSet(new ulong[]{0x0000000100000002UL,0x0000000000000001UL});
    public static readonly BitSet FOLLOW_set_in_comparison3204 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_equ_expression_in_comparison3213 = new BitSet(new ulong[]{0x0000000100000002UL,0x0000000000000001UL});
    public static readonly BitSet FOLLOW_add_expression_in_equ_expression3223 = new BitSet(new ulong[]{0xC000000000000002UL,0x00000000000C0000UL});
    public static readonly BitSet FOLLOW_comparison_operators_in_equ_expression3226 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_add_expression_in_equ_expression3229 = new BitSet(new ulong[]{0xC000000000000002UL,0x00000000000C0000UL});
    public static readonly BitSet FOLLOW_term_in_add_expression3248 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000001800000UL});
    public static readonly BitSet FOLLOW_set_in_add_expression3252 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_term_in_add_expression3259 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000001800000UL});
    public static readonly BitSet FOLLOW_power_expression_in_term3270 = new BitSet(new ulong[]{0x0000000038000002UL});
    public static readonly BitSet FOLLOW_multiply_operator_in_term3273 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_power_expression_in_term3276 = new BitSet(new ulong[]{0x0000000038000002UL});
    public static readonly BitSet FOLLOW_unary_expression_in_power_expression3286 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000100000UL});
    public static readonly BitSet FOLLOW_POW_in_power_expression3289 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_unary_expression_in_power_expression3292 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000100000UL});
    public static readonly BitSet FOLLOW_unary_operator_in_unary_expression3303 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_primary_expression_in_unary_expression3308 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_call_in_primary_expression3316 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_primary_expression3320 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constant_literal_in_primary_expression3323 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_enumerated_value_in_primary_expression3326 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LRBRACKED_in_primary_expression3330 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_expression_in_primary_expression3333 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_107_in_primary_expression3335 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_function_call3346 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_LRBRACKED_in_function_call3348 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_param_assignment_in_function_call3351 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_function_call3354 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_param_assignment_in_function_call3357 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000080000000000UL});
    public static readonly BitSet FOLLOW_107_in_function_call3362 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PLUS_in_unary_operator3376 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NEG_in_unary_operator3380 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOT_in_unary_operator3385 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MUL_in_multiply_operator3404 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DIV_in_multiply_operator3409 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MOD_in_multiply_operator3414 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_bit_string_literal_in_number_literal3423 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_comparison_operators0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_multi_element_variable_in_variable3453 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_multi_element_variable3462 = new BitSet(new ulong[]{0x0000000001000002UL,0x0000001000000000UL});
    public static readonly BitSet FOLLOW_DOT_in_multi_element_variable3466 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_multi_element_variable3469 = new BitSet(new ulong[]{0x0000000001000002UL,0x0000001000000000UL});
    public static readonly BitSet FOLLOW_subscript_list_in_multi_element_variable3473 = new BitSet(new ulong[]{0x0000000001000002UL,0x0000001000000000UL});
    public static readonly BitSet FOLLOW_LBRACKED_in_subscript_list3484 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_subscript_in_subscript_list3488 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000040000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_subscript_list3491 = new BitSet(new ulong[]{0x01FC03FC02080000UL,0x00000007F9800C00UL});
    public static readonly BitSet FOLLOW_subscript_in_subscript_list3494 = new BitSet(new ulong[]{0x0000000004000000UL,0x0000040000000000UL});
    public static readonly BitSet FOLLOW_106_in_subscript_list3498 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_subscript3507 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_elemetary_type_name_in_simple_specification3520 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_simple_specification3525 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_simple_specification3530 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000001000000000UL});
    public static readonly BitSet FOLLOW_DOT_in_simple_specification3532 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000008000000UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_simple_specification3534 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_elemetary_type_name0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_real_type_name0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_integer_type_name0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BOOL_in_bool_literal3757 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SHARP_in_bool_literal3759 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000C00UL});
    public static readonly BitSet FOLLOW_set_in_bool_literal3765 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_range_index_in_subrange3780 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_DOTDOT_in_subrange3782 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000719800000UL});
    public static readonly BitSet FOLLOW_range_index_in_subrange3785 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_integer_literal_in_range_index3795 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENTIFIER_in_range_index3798 = new BitSet(new ulong[]{0x0000000000000002UL});

}
