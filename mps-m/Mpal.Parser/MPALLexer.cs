// $ANTLR 3.2 Sep 23, 2009 12:02:23 J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g 2010-10-26 14:53:59

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


public partial class MPALLexer : Lexer {
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
    public const int BY = 69;
    public const int BYTE = 51;
    public const int XOR = 31;
    public const int TO = 68;
    public const int LINT = 37;
    public const int STRUCT = 61;
    public const int TRUE = 74;
    public const int COLON = 23;
    public const int COUNTER = 42;
    public const int NEQ = 64;
    public const int TOD = 47;
    public const int END_PROGRAM = 90;
    public const int HEX_INTEGER = 98;
    public const int VAR_INPUT = 9;
    public const int NEG = 88;
    public const int R_EDGE = 59;
    public const int ASSIGN = 21;
    public const int EQU = 32;
    public const int PROGRAM = 4;
    public const int DIV = 27;
    public const int VAR_IN_OUT = 10;
    public const int END_FUNCTION_BLOCK = 18;
    public const int TIMER = 43;
    public const int STRING = 11;
    public const int LEQ = 62;

    // delegates
    // delegators

    public MPALLexer() 
    {
		InitializeCyclicDFAs();
    }
    public MPALLexer(ICharStream input)
		: this(input, null) {
    }
    public MPALLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g";} 
    }

    // $ANTLR start "PROGRAM"
    public void mPROGRAM() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PROGRAM;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:7:9: ( 'PROGRAM' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:7:11: 'PROGRAM'
            {
            	Match("PROGRAM"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "PROGRAM"

    // $ANTLR start "FUNCTION"
    public void mFUNCTION() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FUNCTION;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:8:10: ( 'FUNCTION' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:8:12: 'FUNCTION'
            {
            	Match("FUNCTION"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FUNCTION"

    // $ANTLR start "VAR"
    public void mVAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:9:5: ( 'VAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:9:7: 'VAR'
            {
            	Match("VAR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VAR"

    // $ANTLR start "VAR_TEMP"
    public void mVAR_TEMP() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VAR_TEMP;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:10:10: ( 'VAR_TEMP' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:10:12: 'VAR_TEMP'
            {
            	Match("VAR_TEMP"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VAR_TEMP"

    // $ANTLR start "CONSTANT"
    public void mCONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = CONSTANT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:11:10: ( 'CONSTANT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:11:12: 'CONSTANT'
            {
            	Match("CONSTANT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "CONSTANT"

    // $ANTLR start "VAR_INPUT"
    public void mVAR_INPUT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VAR_INPUT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:12:11: ( 'VAR_INPUT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:12:13: 'VAR_INPUT'
            {
            	Match("VAR_INPUT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VAR_INPUT"

    // $ANTLR start "VAR_IN_OUT"
    public void mVAR_IN_OUT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VAR_IN_OUT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:13:12: ( 'VAR_IN_OUT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:13:14: 'VAR_IN_OUT'
            {
            	Match("VAR_IN_OUT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VAR_IN_OUT"

    // $ANTLR start "STRING"
    public void mSTRING() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STRING;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:14:8: ( 'STRING' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:14:10: 'STRING'
            {
            	Match("STRING"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "STRING"

    // $ANTLR start "WSTRING"
    public void mWSTRING() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WSTRING;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:15:9: ( 'WSTRING' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:15:11: 'WSTRING'
            {
            	Match("WSTRING"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WSTRING"

    // $ANTLR start "VAR_OUTPUT"
    public void mVAR_OUTPUT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VAR_OUTPUT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:16:12: ( 'VAR_OUTPUT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:16:14: 'VAR_OUTPUT'
            {
            	Match("VAR_OUTPUT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VAR_OUTPUT"

    // $ANTLR start "ARRAY"
    public void mARRAY() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ARRAY;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:17:7: ( 'ARRAY' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:17:9: 'ARRAY'
            {
            	Match("ARRAY"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ARRAY"

    // $ANTLR start "OF"
    public void mOF() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OF;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:18:4: ( 'OF' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:18:6: 'OF'
            {
            	Match("OF"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OF"

    // $ANTLR start "END_FUNCTION"
    public void mEND_FUNCTION() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = END_FUNCTION;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:19:14: ( 'END_FUNCTION' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:19:16: 'END_FUNCTION'
            {
            	Match("END_FUNCTION"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "END_FUNCTION"

    // $ANTLR start "FUNCTION_BLOCK"
    public void mFUNCTION_BLOCK() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FUNCTION_BLOCK;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:20:16: ( 'FUNCTION_BLOCK' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:20:18: 'FUNCTION_BLOCK'
            {
            	Match("FUNCTION_BLOCK"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FUNCTION_BLOCK"

    // $ANTLR start "END_FUNCTION_BLOCK"
    public void mEND_FUNCTION_BLOCK() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = END_FUNCTION_BLOCK;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:21:20: ( 'END_FUNCTION_BLOCK' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:21:22: 'END_FUNCTION_BLOCK'
            {
            	Match("END_FUNCTION_BLOCK"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "END_FUNCTION_BLOCK"

    // $ANTLR start "NOT"
    public void mNOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:22:5: ( 'NOT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:22:7: 'NOT'
            {
            	Match("NOT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NOT"

    // $ANTLR start "EXIT"
    public void mEXIT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EXIT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:23:6: ( 'EXIT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:23:8: 'EXIT'
            {
            	Match("EXIT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EXIT"

    // $ANTLR start "ASSIGN"
    public void mASSIGN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ASSIGN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:24:8: ( ':=' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:24:10: ':='
            {
            	Match(":="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ASSIGN"

    // $ANTLR start "ASSIGN2"
    public void mASSIGN2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ASSIGN2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:25:9: ( '=>' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:25:11: '=>'
            {
            	Match("=>"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ASSIGN2"

    // $ANTLR start "COLON"
    public void mCOLON() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COLON;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:26:7: ( ':' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:26:9: ':'
            {
            	Match(':'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COLON"

    // $ANTLR start "LBRACKED"
    public void mLBRACKED() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LBRACKED;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:27:10: ( '[' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:27:12: '['
            {
            	Match('['); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LBRACKED"

    // $ANTLR start "LRBRACKED"
    public void mLRBRACKED() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LRBRACKED;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:28:11: ( '(' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:28:13: '('
            {
            	Match('('); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LRBRACKED"

    // $ANTLR start "COMMA"
    public void mCOMMA() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMA;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:29:7: ( ',' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:29:9: ','
            {
            	Match(','); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMA"

    // $ANTLR start "DIV"
    public void mDIV() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DIV;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:30:5: ( '/' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:30:7: '/'
            {
            	Match('/'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIV"

    // $ANTLR start "MOD"
    public void mMOD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MOD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:31:5: ( 'MOD' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:31:7: 'MOD'
            {
            	Match("MOD"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MOD"

    // $ANTLR start "MUL"
    public void mMUL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MUL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:32:5: ( '*' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:32:7: '*'
            {
            	Match('*'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MUL"

    // $ANTLR start "OR"
    public void mOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:33:4: ( 'OR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:33:6: 'OR'
            {
            	Match("OR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OR"

    // $ANTLR start "XOR"
    public void mXOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = XOR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:34:5: ( 'XOR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:34:7: 'XOR'
            {
            	Match("XOR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "XOR"

    // $ANTLR start "EQU"
    public void mEQU() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQU;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:35:5: ( '=' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:35:7: '='
            {
            	Match('='); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EQU"

    // $ANTLR start "VOID"
    public void mVOID() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VOID;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:36:6: ( 'VOID' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:36:8: 'VOID'
            {
            	Match("VOID"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VOID"

    // $ANTLR start "SINT"
    public void mSINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SINT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:37:6: ( 'SINT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:37:8: 'SINT'
            {
            	Match("SINT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SINT"

    // $ANTLR start "INT"
    public void mINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = INT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:38:5: ( 'INT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:38:7: 'INT'
            {
            	Match("INT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "INT"

    // $ANTLR start "DINT"
    public void mDINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DINT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:39:6: ( 'DINT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:39:8: 'DINT'
            {
            	Match("DINT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DINT"

    // $ANTLR start "LINT"
    public void mLINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LINT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:40:6: ( 'LINT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:40:8: 'LINT'
            {
            	Match("LINT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LINT"

    // $ANTLR start "USINT"
    public void mUSINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = USINT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:41:7: ( 'USINT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:41:9: 'USINT'
            {
            	Match("USINT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "USINT"

    // $ANTLR start "UINT"
    public void mUINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = UINT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:42:6: ( 'UINT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:42:8: 'UINT'
            {
            	Match("UINT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "UINT"

    // $ANTLR start "UDINT"
    public void mUDINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = UDINT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:43:7: ( 'UDINT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:43:9: 'UDINT'
            {
            	Match("UDINT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "UDINT"

    // $ANTLR start "ULINT"
    public void mULINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ULINT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:44:7: ( 'ULINT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:44:9: 'ULINT'
            {
            	Match("ULINT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ULINT"

    // $ANTLR start "COUNTER"
    public void mCOUNTER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COUNTER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:45:9: ( 'COUNTER' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:45:11: 'COUNTER'
            {
            	Match("COUNTER"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COUNTER"

    // $ANTLR start "TIMER"
    public void mTIMER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TIMER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:46:7: ( 'TIMER' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:46:9: 'TIMER'
            {
            	Match("TIMER"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TIMER"

    // $ANTLR start "TIME"
    public void mTIME() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TIME;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:47:6: ( 'TIME' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:47:8: 'TIME'
            {
            	Match("TIME"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TIME"

    // $ANTLR start "DATE_LIT"
    public void mDATE_LIT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DATE_LIT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:48:10: ( 'DATE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:48:12: 'DATE'
            {
            	Match("DATE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DATE_LIT"

    // $ANTLR start "TIME_OF_DAY_LIT"
    public void mTIME_OF_DAY_LIT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TIME_OF_DAY_LIT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:49:17: ( 'TIME_OF_DAY' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:49:19: 'TIME_OF_DAY'
            {
            	Match("TIME_OF_DAY"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TIME_OF_DAY_LIT"

    // $ANTLR start "TOD"
    public void mTOD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TOD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:50:5: ( 'TOD' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:50:7: 'TOD'
            {
            	Match("TOD"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TOD"

    // $ANTLR start "DATE_AND_TIME"
    public void mDATE_AND_TIME() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DATE_AND_TIME;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:51:15: ( 'DATE_AND_TIME' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:51:17: 'DATE_AND_TIME'
            {
            	Match("DATE_AND_TIME"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DATE_AND_TIME"

    // $ANTLR start "DT"
    public void mDT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:52:4: ( 'DT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:52:6: 'DT'
            {
            	Match("DT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DT"

    // $ANTLR start "BOOL"
    public void mBOOL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BOOL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:53:6: ( 'BOOL' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:53:8: 'BOOL'
            {
            	Match("BOOL"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BOOL"

    // $ANTLR start "BYTE"
    public void mBYTE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BYTE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:54:6: ( 'BYTE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:54:8: 'BYTE'
            {
            	Match("BYTE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BYTE"

    // $ANTLR start "WORD"
    public void mWORD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WORD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:55:6: ( 'WORD' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:55:8: 'WORD'
            {
            	Match("WORD"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WORD"

    // $ANTLR start "DWORD"
    public void mDWORD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DWORD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:56:7: ( 'DWORD' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:56:9: 'DWORD'
            {
            	Match("DWORD"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DWORD"

    // $ANTLR start "LWORD"
    public void mLWORD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LWORD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:57:7: ( 'LWORD' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:57:9: 'LWORD'
            {
            	Match("LWORD"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LWORD"

    // $ANTLR start "REAL"
    public void mREAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = REAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:58:6: ( 'REAL' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:58:8: 'REAL'
            {
            	Match("REAL"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "REAL"

    // $ANTLR start "LREAL"
    public void mLREAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LREAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:59:7: ( 'LREAL' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:59:9: 'LREAL'
            {
            	Match("LREAL"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LREAL"

    // $ANTLR start "CHAR"
    public void mCHAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = CHAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:60:6: ( 'CHAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:60:8: 'CHAR'
            {
            	Match("CHAR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "CHAR"

    // $ANTLR start "WCHAR"
    public void mWCHAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WCHAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:61:7: ( 'WCHAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:61:9: 'WCHAR'
            {
            	Match("WCHAR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WCHAR"

    // $ANTLR start "R_EDGE"
    public void mR_EDGE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = R_EDGE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:62:8: ( 'R_EDGE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:62:10: 'R_EDGE'
            {
            	Match("R_EDGE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "R_EDGE"

    // $ANTLR start "F_EDGE"
    public void mF_EDGE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = F_EDGE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:63:8: ( 'F_EDGE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:63:10: 'F_EDGE'
            {
            	Match("F_EDGE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "F_EDGE"

    // $ANTLR start "STRUCT"
    public void mSTRUCT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STRUCT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:64:8: ( 'STRUCT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:64:10: 'STRUCT'
            {
            	Match("STRUCT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "STRUCT"

    // $ANTLR start "LEQ"
    public void mLEQ() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LEQ;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:65:5: ( '<=' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:65:7: '<='
            {
            	Match("<="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LEQ"

    // $ANTLR start "GEQ"
    public void mGEQ() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GEQ;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:66:5: ( '>=' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:66:7: '>='
            {
            	Match(">="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GEQ"

    // $ANTLR start "NEQ"
    public void mNEQ() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEQ;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:67:5: ( '<>' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:67:7: '<>'
            {
            	Match("<>"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEQ"

    // $ANTLR start "IF"
    public void mIF() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IF;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:68:4: ( 'IF' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:68:6: 'IF'
            {
            	Match("IF"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IF"

    // $ANTLR start "CASE"
    public void mCASE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = CASE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:69:6: ( 'CASE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:69:8: 'CASE'
            {
            	Match("CASE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "CASE"

    // $ANTLR start "FOR"
    public void mFOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FOR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:70:5: ( 'FOR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:70:7: 'FOR'
            {
            	Match("FOR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FOR"

    // $ANTLR start "TO"
    public void mTO() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TO;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:71:4: ( 'TO' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:71:6: 'TO'
            {
            	Match("TO"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TO"

    // $ANTLR start "BY"
    public void mBY() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BY;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:72:4: ( 'BY' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:72:6: 'BY'
            {
            	Match("BY"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BY"

    // $ANTLR start "DO"
    public void mDO() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DO;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:73:4: ( 'DO' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:73:6: 'DO'
            {
            	Match("DO"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DO"

    // $ANTLR start "WHILE"
    public void mWHILE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WHILE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:74:7: ( 'WHILE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:74:9: 'WHILE'
            {
            	Match("WHILE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WHILE"

    // $ANTLR start "REPEAT"
    public void mREPEAT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = REPEAT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:75:8: ( 'REPEAT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:75:10: 'REPEAT'
            {
            	Match("REPEAT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "REPEAT"

    // $ANTLR start "CONTINUE"
    public void mCONTINUE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = CONTINUE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:76:10: ( 'CONTINUE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:76:12: 'CONTINUE'
            {
            	Match("CONTINUE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "CONTINUE"

    // $ANTLR start "TRUE"
    public void mTRUE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TRUE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:77:6: ( 'TRUE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:77:8: 'TRUE'
            {
            	Match("TRUE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TRUE"

    // $ANTLR start "FALSE"
    public void mFALSE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FALSE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:78:7: ( 'FALSE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:78:9: 'FALSE'
            {
            	Match("FALSE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FALSE"

    // $ANTLR start "ELSE"
    public void mELSE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ELSE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:79:6: ( 'ELSE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:79:8: 'ELSE'
            {
            	Match("ELSE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ELSE"

    // $ANTLR start "ELSIF"
    public void mELSIF() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ELSIF;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:80:7: ( 'ELSIF' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:80:9: 'ELSIF'
            {
            	Match("ELSIF"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ELSIF"

    // $ANTLR start "USES"
    public void mUSES() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = USES;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:81:6: ( 'USES' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:81:8: 'USES'
            {
            	Match("USES"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "USES"

    // $ANTLR start "TYPE"
    public void mTYPE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TYPE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:82:6: ( 'TYPE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:82:8: 'TYPE'
            {
            	Match("TYPE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TYPE"

    // $ANTLR start "UNTIL"
    public void mUNTIL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = UNTIL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:83:7: ( 'UNTIL' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:83:9: 'UNTIL'
            {
            	Match("UNTIL"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "UNTIL"

    // $ANTLR start "THEN"
    public void mTHEN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = THEN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:84:6: ( 'THEN' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:84:8: 'THEN'
            {
            	Match("THEN"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "THEN"

    // $ANTLR start "LS"
    public void mLS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:85:4: ( '<' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:85:6: '<'
            {
            	Match('<'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LS"

    // $ANTLR start "GR"
    public void mGR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:86:4: ( '>' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:86:6: '>'
            {
            	Match('>'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GR"

    // $ANTLR start "POW"
    public void mPOW() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = POW;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:87:5: ( '**' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:87:7: '**'
            {
            	Match("**"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "POW"

    // $ANTLR start "RETURN"
    public void mRETURN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RETURN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:88:8: ( 'RETURN' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:88:10: 'RETURN'
            {
            	Match("RETURN"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RETURN"

    // $ANTLR start "SHARP"
    public void mSHARP() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SHARP;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:89:7: ( '#' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:89:9: '#'
            {
            	Match('#'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SHARP"

    // $ANTLR start "PLUS"
    public void mPLUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PLUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:90:6: ( '+' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:90:8: '+'
            {
            	Match('+'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "PLUS"

    // $ANTLR start "NEG"
    public void mNEG() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEG;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:91:5: ( '-' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:91:7: '-'
            {
            	Match('-'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEG"

    // $ANTLR start "DOTDOT"
    public void mDOTDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOTDOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:92:8: ( '..' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:92:10: '..'
            {
            	Match(".."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOTDOT"

    // $ANTLR start "END_PROGRAM"
    public void mEND_PROGRAM() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = END_PROGRAM;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:93:13: ( 'END_PROGRAM' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:93:15: 'END_PROGRAM'
            {
            	Match("END_PROGRAM"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "END_PROGRAM"

    // $ANTLR start "T__106"
    public void mT__106() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__106;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:94:8: ( ']' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:94:10: ']'
            {
            	Match(']'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__106"

    // $ANTLR start "T__107"
    public void mT__107() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__107;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:95:8: ( ')' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:95:10: ')'
            {
            	Match(')'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__107"

    // $ANTLR start "T__108"
    public void mT__108() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__108;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:96:8: ( ';' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:96:10: ';'
            {
            	Match(';'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__108"

    // $ANTLR start "T__109"
    public void mT__109() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__109;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:97:8: ( 'END_STRUCT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:97:10: 'END_STRUCT'
            {
            	Match("END_STRUCT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__109"

    // $ANTLR start "T__110"
    public void mT__110() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__110;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:98:8: ( 'END_TYPE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:98:10: 'END_TYPE'
            {
            	Match("END_TYPE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__110"

    // $ANTLR start "T__111"
    public void mT__111() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__111;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:99:8: ( 'END_VAR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:99:10: 'END_VAR'
            {
            	Match("END_VAR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__111"

    // $ANTLR start "T__112"
    public void mT__112() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__112;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:100:8: ( 'END_FOR' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:100:10: 'END_FOR'
            {
            	Match("END_FOR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__112"

    // $ANTLR start "T__113"
    public void mT__113() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__113;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:101:8: ( 'END_WHILE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:101:10: 'END_WHILE'
            {
            	Match("END_WHILE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__113"

    // $ANTLR start "T__114"
    public void mT__114() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__114;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:102:8: ( 'END_REPEAT' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:102:10: 'END_REPEAT'
            {
            	Match("END_REPEAT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__114"

    // $ANTLR start "T__115"
    public void mT__115() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__115;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:103:8: ( 'END_IF' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:103:10: 'END_IF'
            {
            	Match("END_IF"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__115"

    // $ANTLR start "T__116"
    public void mT__116() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__116;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:104:8: ( 'END_CASE' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:104:10: 'END_CASE'
            {
            	Match("END_CASE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__116"

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:328:5: ( '.' ( '.' )? )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:328:7: '.' ( '.' )?
            {
            	Match('.'); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:328:11: ( '.' )?
            	int alt1 = 2;
            	int LA1_0 = input.LA(1);

            	if ( (LA1_0 == '.') )
            	{
            	    alt1 = 1;
            	}
            	switch (alt1) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:328:12: '.'
            	        {
            	        	Match('.'); 
            	        	_type=DOTDOT;

            	        }
            	        break;

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOT"

    // $ANTLR start "AND"
    public void mAND() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AND;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:330:5: ( '&' | 'AND' )
            int alt2 = 2;
            int LA2_0 = input.LA(1);

            if ( (LA2_0 == '&') )
            {
                alt2 = 1;
            }
            else if ( (LA2_0 == 'A') )
            {
                alt2 = 2;
            }
            else 
            {
                NoViableAltException nvae_d2s0 =
                    new NoViableAltException("", 2, 0, input);

                throw nvae_d2s0;
            }
            switch (alt2) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:330:7: '&'
                    {
                    	Match('&'); 

                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:330:12: 'AND'
                    {
                    	Match("AND"); 


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AND"

    // $ANTLR start "INTEGER"
    public void mINTEGER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = INTEGER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:332:9: ( ( '0' .. '9' )+ )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:332:11: ( '0' .. '9' )+
            {
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:332:11: ( '0' .. '9' )+
            	int cnt3 = 0;
            	do 
            	{
            	    int alt3 = 2;
            	    int LA3_0 = input.LA(1);

            	    if ( ((LA3_0 >= '0' && LA3_0 <= '9')) )
            	    {
            	        alt3 = 1;
            	    }


            	    switch (alt3) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:332:12: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 

            			    }
            			    break;

            			default:
            			    if ( cnt3 >= 1 ) goto loop3;
            		            EarlyExitException eee3 =
            		                new EarlyExitException(3, input);
            		            throw eee3;
            	    }
            	    cnt3++;
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "INTEGER"

    // $ANTLR start "REAL_CONSTANT"
    public void mREAL_CONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = REAL_CONSTANT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:334:15: ( INTEGER ( ( '.' ( '0' .. '9' )+ ( EXPONENT )? )? | EXPONENT ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:334:17: INTEGER ( ( '.' ( '0' .. '9' )+ ( EXPONENT )? )? | EXPONENT )
            {
            	mINTEGER(); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:334:26: ( ( '.' ( '0' .. '9' )+ ( EXPONENT )? )? | EXPONENT )
            	int alt7 = 2;
            	int LA7_0 = input.LA(1);

            	if ( (LA7_0 == 'E' || LA7_0 == 'e') )
            	{
            	    alt7 = 2;
            	}
            	else 
            	{
            	    alt7 = 1;}
            	switch (alt7) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:334:28: ( '.' ( '0' .. '9' )+ ( EXPONENT )? )?
            	        {
            	        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:334:28: ( '.' ( '0' .. '9' )+ ( EXPONENT )? )?
            	        	int alt6 = 2;
            	        	int LA6_0 = input.LA(1);

            	        	if ( (LA6_0 == '.') )
            	        	{
            	        	    alt6 = 1;
            	        	}
            	        	switch (alt6) 
            	        	{
            	        	    case 1 :
            	        	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:334:30: '.' ( '0' .. '9' )+ ( EXPONENT )?
            	        	        {

            	        	        						if(input.LA(2)=='.')
            	        	        						{//If follow .. than is only an integer.
            	        	        						        state.type = INTEGER;
            	        	        					                state.channel = _channel;
            	        	        					                return;
            	        	        						}
            	        	        						
            	        	        					
            	        	        	Match('.'); 
            	        	        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:342:12: ( '0' .. '9' )+
            	        	        	int cnt4 = 0;
            	        	        	do 
            	        	        	{
            	        	        	    int alt4 = 2;
            	        	        	    int LA4_0 = input.LA(1);

            	        	        	    if ( ((LA4_0 >= '0' && LA4_0 <= '9')) )
            	        	        	    {
            	        	        	        alt4 = 1;
            	        	        	    }


            	        	        	    switch (alt4) 
            	        	        		{
            	        	        			case 1 :
            	        	        			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:342:13: '0' .. '9'
            	        	        			    {
            	        	        			    	MatchRange('0','9'); 

            	        	        			    }
            	        	        			    break;

            	        	        			default:
            	        	        			    if ( cnt4 >= 1 ) goto loop4;
            	        	        		            EarlyExitException eee4 =
            	        	        		                new EarlyExitException(4, input);
            	        	        		            throw eee4;
            	        	        	    }
            	        	        	    cnt4++;
            	        	        	} while (true);

            	        	        	loop4:
            	        	        		;	// Stops C# compiler whining that label 'loop4' has no statements

            	        	        	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:342:24: ( EXPONENT )?
            	        	        	int alt5 = 2;
            	        	        	int LA5_0 = input.LA(1);

            	        	        	if ( (LA5_0 == 'E' || LA5_0 == 'e') )
            	        	        	{
            	        	        	    alt5 = 1;
            	        	        	}
            	        	        	switch (alt5) 
            	        	        	{
            	        	        	    case 1 :
            	        	        	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:342:25: EXPONENT
            	        	        	        {
            	        	        	        	mEXPONENT(); 

            	        	        	        }
            	        	        	        break;

            	        	        	}


            	        	        }
            	        	        break;

            	        	}


            	        }
            	        break;
            	    case 2 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:342:43: EXPONENT
            	        {
            	        	mEXPONENT(); 

            	        }
            	        break;

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "REAL_CONSTANT"

    // $ANTLR start "EXPONENT"
    public void mEXPONENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EXPONENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:345:11: ( ( 'E' | 'e' ) ( PLUS | NEG )? INTEGER )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:345:13: ( 'E' | 'e' ) ( PLUS | NEG )? INTEGER
            {
            	if ( input.LA(1) == 'E' || input.LA(1) == 'e' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:345:25: ( PLUS | NEG )?
            	int alt8 = 2;
            	int LA8_0 = input.LA(1);

            	if ( (LA8_0 == '+' || LA8_0 == '-') )
            	{
            	    alt8 = 1;
            	}
            	switch (alt8) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            	        {
            	        	if ( input.LA(1) == '+' || input.LA(1) == '-' ) 
            	        	{
            	        	    input.Consume();

            	        	}
            	        	else 
            	        	{
            	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	        	    Recover(mse);
            	        	    throw mse;}


            	        }
            	        break;

            	}

            	mINTEGER(); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EXPONENT"

    // $ANTLR start "IDENTIFIER"
    public void mIDENTIFIER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IDENTIFIER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:347:12: ( ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '_' | '0' .. '9' )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:347:16: ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '_' | '0' .. '9' )*
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:347:45: ( 'a' .. 'z' | 'A' .. 'Z' | '_' | '0' .. '9' )*
            	do 
            	{
            	    int alt9 = 2;
            	    int LA9_0 = input.LA(1);

            	    if ( ((LA9_0 >= '0' && LA9_0 <= '9') || (LA9_0 >= 'A' && LA9_0 <= 'Z') || LA9_0 == '_' || (LA9_0 >= 'a' && LA9_0 <= 'z')) )
            	    {
            	        alt9 = 1;
            	    }


            	    switch (alt9) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            			    {
            			    	if ( (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop9;
            	    }
            	} while (true);

            	loop9:
            		;	// Stops C# compiler whining that label 'loop9' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IDENTIFIER"

    // $ANTLR start "STRING_LITERAL"
    public void mSTRING_LITERAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STRING_LITERAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:349:16: ( '\\'' ( '\\'\\'' | ~ ( '\\'' ) )* '\\'' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:349:18: '\\'' ( '\\'\\'' | ~ ( '\\'' ) )* '\\''
            {
            	Match('\''); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:349:23: ( '\\'\\'' | ~ ( '\\'' ) )*
            	do 
            	{
            	    int alt10 = 3;
            	    int LA10_0 = input.LA(1);

            	    if ( (LA10_0 == '\'') )
            	    {
            	        int LA10_1 = input.LA(2);

            	        if ( (LA10_1 == '\'') )
            	        {
            	            alt10 = 1;
            	        }


            	    }
            	    else if ( ((LA10_0 >= '\u0000' && LA10_0 <= '&') || (LA10_0 >= '(' && LA10_0 <= '\uFFFF')) )
            	    {
            	        alt10 = 2;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:349:24: '\\'\\''
            			    {
            			    	Match("''"); 


            			    }
            			    break;
            			case 2 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:349:33: ~ ( '\\'' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '&') || (input.LA(1) >= '(' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop10;
            	    }
            	} while (true);

            	loop10:
            		;	// Stops C# compiler whining that label 'loop10' has no statements

            	Match('\''); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "STRING_LITERAL"

    // $ANTLR start "STRING_LITERAL_UNI"
    public void mSTRING_LITERAL_UNI() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STRING_LITERAL_UNI;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:351:20: ( '\"' (~ ( '\\\\' | '\"' ) )* '\"' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:351:22: '\"' (~ ( '\\\\' | '\"' ) )* '\"'
            {
            	Match('\"'); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:351:26: (~ ( '\\\\' | '\"' ) )*
            	do 
            	{
            	    int alt11 = 2;
            	    int LA11_0 = input.LA(1);

            	    if ( ((LA11_0 >= '\u0000' && LA11_0 <= '!') || (LA11_0 >= '#' && LA11_0 <= '[') || (LA11_0 >= ']' && LA11_0 <= '\uFFFF')) )
            	    {
            	        alt11 = 1;
            	    }


            	    switch (alt11) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:351:27: ~ ( '\\\\' | '\"' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop11;
            	    }
            	} while (true);

            	loop11:
            		;	// Stops C# compiler whining that label 'loop11' has no statements

            	Match('\"'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "STRING_LITERAL_UNI"

    // $ANTLR start "BINARY_INTEGER"
    public void mBINARY_INTEGER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BINARY_INTEGER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:353:16: ( '2#' ( '1' | '0' )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:353:19: '2#' ( '1' | '0' )*
            {
            	Match("2#"); 

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:353:25: ( '1' | '0' )*
            	do 
            	{
            	    int alt12 = 2;
            	    int LA12_0 = input.LA(1);

            	    if ( ((LA12_0 >= '0' && LA12_0 <= '1')) )
            	    {
            	        alt12 = 1;
            	    }


            	    switch (alt12) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            			    {
            			    	if ( (input.LA(1) >= '0' && input.LA(1) <= '1') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop12;
            	    }
            	} while (true);

            	loop12:
            		;	// Stops C# compiler whining that label 'loop12' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BINARY_INTEGER"

    // $ANTLR start "OCTAL_INTEGER"
    public void mOCTAL_INTEGER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OCTAL_INTEGER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:355:15: ( '8#' ( '0' .. '7' )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:355:17: '8#' ( '0' .. '7' )*
            {
            	Match("8#"); 

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:355:22: ( '0' .. '7' )*
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);

            	    if ( ((LA13_0 >= '0' && LA13_0 <= '7')) )
            	    {
            	        alt13 = 1;
            	    }


            	    switch (alt13) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:355:23: '0' .. '7'
            			    {
            			    	MatchRange('0','7'); 

            			    }
            			    break;

            			default:
            			    goto loop13;
            	    }
            	} while (true);

            	loop13:
            		;	// Stops C# compiler whining that label 'loop13' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OCTAL_INTEGER"

    // $ANTLR start "HEX_INTEGER"
    public void mHEX_INTEGER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = HEX_INTEGER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:357:13: ( '16#' ( '0' .. '9' | 'A' .. 'F' )* )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:357:15: '16#' ( '0' .. '9' | 'A' .. 'F' )*
            {
            	Match("16#"); 

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:357:21: ( '0' .. '9' | 'A' .. 'F' )*
            	do 
            	{
            	    int alt14 = 2;
            	    int LA14_0 = input.LA(1);

            	    if ( ((LA14_0 >= '0' && LA14_0 <= '9') || (LA14_0 >= 'A' && LA14_0 <= 'F')) )
            	    {
            	        alt14 = 1;
            	    }


            	    switch (alt14) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:
            			    {
            			    	if ( (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'F') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop14;
            	    }
            	} while (true);

            	loop14:
            		;	// Stops C# compiler whining that label 'loop14' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "HEX_INTEGER"

    // $ANTLR start "COMMENT"
    public void mCOMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:359:9: ( '(*' ( options {greedy=false; } : . )* '*)' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:359:11: '(*' ( options {greedy=false; } : . )* '*)'
            {
            	Match("(*"); 

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:359:16: ( options {greedy=false; } : . )*
            	do 
            	{
            	    int alt15 = 2;
            	    int LA15_0 = input.LA(1);

            	    if ( (LA15_0 == '*') )
            	    {
            	        int LA15_1 = input.LA(2);

            	        if ( (LA15_1 == ')') )
            	        {
            	            alt15 = 2;
            	        }
            	        else if ( ((LA15_1 >= '\u0000' && LA15_1 <= '(') || (LA15_1 >= '*' && LA15_1 <= '\uFFFF')) )
            	        {
            	            alt15 = 1;
            	        }


            	    }
            	    else if ( ((LA15_0 >= '\u0000' && LA15_0 <= ')') || (LA15_0 >= '+' && LA15_0 <= '\uFFFF')) )
            	    {
            	        alt15 = 1;
            	    }


            	    switch (alt15) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:359:44: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop15;
            	    }
            	} while (true);

            	loop15:
            		;	// Stops C# compiler whining that label 'loop15' has no statements

            	Match("*)"); 

            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMENT"

    // $ANTLR start "PRAGMAS"
    public void mPRAGMAS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PRAGMAS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:361:9: ( '{' ( options {greedy=false; } : . )* '}' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:361:11: '{' ( options {greedy=false; } : . )* '}'
            {
            	Match('{'); 
            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:361:15: ( options {greedy=false; } : . )*
            	do 
            	{
            	    int alt16 = 2;
            	    int LA16_0 = input.LA(1);

            	    if ( (LA16_0 == '}') )
            	    {
            	        alt16 = 2;
            	    }
            	    else if ( ((LA16_0 >= '\u0000' && LA16_0 <= '|') || (LA16_0 >= '~' && LA16_0 <= '\uFFFF')) )
            	    {
            	        alt16 = 1;
            	    }


            	    switch (alt16) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:361:43: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop16;
            	    }
            	} while (true);

            	loop16:
            		;	// Stops C# compiler whining that label 'loop16' has no statements

            	Match('}'); 
            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "PRAGMAS"

    // $ANTLR start "LINE_COMMENT"
    public void mLINE_COMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LINE_COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:363:14: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:363:16: '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
            {
            	Match("//"); 

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:363:21: (~ ( '\\n' | '\\r' ) )*
            	do 
            	{
            	    int alt17 = 2;
            	    int LA17_0 = input.LA(1);

            	    if ( ((LA17_0 >= '\u0000' && LA17_0 <= '\t') || (LA17_0 >= '\u000B' && LA17_0 <= '\f') || (LA17_0 >= '\u000E' && LA17_0 <= '\uFFFF')) )
            	    {
            	        alt17 = 1;
            	    }


            	    switch (alt17) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:363:21: ~ ( '\\n' | '\\r' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop17;
            	    }
            	} while (true);

            	loop17:
            		;	// Stops C# compiler whining that label 'loop17' has no statements

            	// J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:363:35: ( '\\r' )?
            	int alt18 = 2;
            	int LA18_0 = input.LA(1);

            	if ( (LA18_0 == '\r') )
            	{
            	    alt18 = 1;
            	}
            	switch (alt18) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:363:35: '\\r'
            	        {
            	        	Match('\r'); 

            	        }
            	        break;

            	}

            	Match('\n'); 
            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LINE_COMMENT"

    // $ANTLR start "WS"
    public void mWS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:365:5: ( ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' ) )
            // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:365:8: ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' )
            {
            	if ( (input.LA(1) >= '\t' && input.LA(1) <= '\n') || (input.LA(1) >= '\f' && input.LA(1) <= '\r') || input.LA(1) == ' ' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WS"

    override public void mTokens() // throws RecognitionException 
    {
        // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:8: ( PROGRAM | FUNCTION | VAR | VAR_TEMP | CONSTANT | VAR_INPUT | VAR_IN_OUT | STRING | WSTRING | VAR_OUTPUT | ARRAY | OF | END_FUNCTION | FUNCTION_BLOCK | END_FUNCTION_BLOCK | NOT | EXIT | ASSIGN | ASSIGN2 | COLON | LBRACKED | LRBRACKED | COMMA | DIV | MOD | MUL | OR | XOR | EQU | VOID | SINT | INT | DINT | LINT | USINT | UINT | UDINT | ULINT | COUNTER | TIMER | TIME | DATE_LIT | TIME_OF_DAY_LIT | TOD | DATE_AND_TIME | DT | BOOL | BYTE | WORD | DWORD | LWORD | REAL | LREAL | CHAR | WCHAR | R_EDGE | F_EDGE | STRUCT | LEQ | GEQ | NEQ | IF | CASE | FOR | TO | BY | DO | WHILE | REPEAT | CONTINUE | TRUE | FALSE | ELSE | ELSIF | USES | TYPE | UNTIL | THEN | LS | GR | POW | RETURN | SHARP | PLUS | NEG | DOTDOT | END_PROGRAM | T__106 | T__107 | T__108 | T__109 | T__110 | T__111 | T__112 | T__113 | T__114 | T__115 | T__116 | DOT | AND | INTEGER | REAL_CONSTANT | EXPONENT | IDENTIFIER | STRING_LITERAL | STRING_LITERAL_UNI | BINARY_INTEGER | OCTAL_INTEGER | HEX_INTEGER | COMMENT | PRAGMAS | LINE_COMMENT | WS )
        int alt19 = 113;
        alt19 = dfa19.Predict(input);
        switch (alt19) 
        {
            case 1 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:10: PROGRAM
                {
                	mPROGRAM(); 

                }
                break;
            case 2 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:18: FUNCTION
                {
                	mFUNCTION(); 

                }
                break;
            case 3 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:27: VAR
                {
                	mVAR(); 

                }
                break;
            case 4 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:31: VAR_TEMP
                {
                	mVAR_TEMP(); 

                }
                break;
            case 5 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:40: CONSTANT
                {
                	mCONSTANT(); 

                }
                break;
            case 6 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:49: VAR_INPUT
                {
                	mVAR_INPUT(); 

                }
                break;
            case 7 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:59: VAR_IN_OUT
                {
                	mVAR_IN_OUT(); 

                }
                break;
            case 8 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:70: STRING
                {
                	mSTRING(); 

                }
                break;
            case 9 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:77: WSTRING
                {
                	mWSTRING(); 

                }
                break;
            case 10 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:85: VAR_OUTPUT
                {
                	mVAR_OUTPUT(); 

                }
                break;
            case 11 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:96: ARRAY
                {
                	mARRAY(); 

                }
                break;
            case 12 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:102: OF
                {
                	mOF(); 

                }
                break;
            case 13 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:105: END_FUNCTION
                {
                	mEND_FUNCTION(); 

                }
                break;
            case 14 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:118: FUNCTION_BLOCK
                {
                	mFUNCTION_BLOCK(); 

                }
                break;
            case 15 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:133: END_FUNCTION_BLOCK
                {
                	mEND_FUNCTION_BLOCK(); 

                }
                break;
            case 16 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:152: NOT
                {
                	mNOT(); 

                }
                break;
            case 17 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:156: EXIT
                {
                	mEXIT(); 

                }
                break;
            case 18 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:161: ASSIGN
                {
                	mASSIGN(); 

                }
                break;
            case 19 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:168: ASSIGN2
                {
                	mASSIGN2(); 

                }
                break;
            case 20 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:176: COLON
                {
                	mCOLON(); 

                }
                break;
            case 21 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:182: LBRACKED
                {
                	mLBRACKED(); 

                }
                break;
            case 22 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:191: LRBRACKED
                {
                	mLRBRACKED(); 

                }
                break;
            case 23 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:201: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 24 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:207: DIV
                {
                	mDIV(); 

                }
                break;
            case 25 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:211: MOD
                {
                	mMOD(); 

                }
                break;
            case 26 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:215: MUL
                {
                	mMUL(); 

                }
                break;
            case 27 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:219: OR
                {
                	mOR(); 

                }
                break;
            case 28 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:222: XOR
                {
                	mXOR(); 

                }
                break;
            case 29 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:226: EQU
                {
                	mEQU(); 

                }
                break;
            case 30 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:230: VOID
                {
                	mVOID(); 

                }
                break;
            case 31 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:235: SINT
                {
                	mSINT(); 

                }
                break;
            case 32 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:240: INT
                {
                	mINT(); 

                }
                break;
            case 33 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:244: DINT
                {
                	mDINT(); 

                }
                break;
            case 34 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:249: LINT
                {
                	mLINT(); 

                }
                break;
            case 35 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:254: USINT
                {
                	mUSINT(); 

                }
                break;
            case 36 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:260: UINT
                {
                	mUINT(); 

                }
                break;
            case 37 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:265: UDINT
                {
                	mUDINT(); 

                }
                break;
            case 38 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:271: ULINT
                {
                	mULINT(); 

                }
                break;
            case 39 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:277: COUNTER
                {
                	mCOUNTER(); 

                }
                break;
            case 40 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:285: TIMER
                {
                	mTIMER(); 

                }
                break;
            case 41 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:291: TIME
                {
                	mTIME(); 

                }
                break;
            case 42 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:296: DATE_LIT
                {
                	mDATE_LIT(); 

                }
                break;
            case 43 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:305: TIME_OF_DAY_LIT
                {
                	mTIME_OF_DAY_LIT(); 

                }
                break;
            case 44 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:321: TOD
                {
                	mTOD(); 

                }
                break;
            case 45 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:325: DATE_AND_TIME
                {
                	mDATE_AND_TIME(); 

                }
                break;
            case 46 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:339: DT
                {
                	mDT(); 

                }
                break;
            case 47 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:342: BOOL
                {
                	mBOOL(); 

                }
                break;
            case 48 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:347: BYTE
                {
                	mBYTE(); 

                }
                break;
            case 49 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:352: WORD
                {
                	mWORD(); 

                }
                break;
            case 50 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:357: DWORD
                {
                	mDWORD(); 

                }
                break;
            case 51 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:363: LWORD
                {
                	mLWORD(); 

                }
                break;
            case 52 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:369: REAL
                {
                	mREAL(); 

                }
                break;
            case 53 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:374: LREAL
                {
                	mLREAL(); 

                }
                break;
            case 54 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:380: CHAR
                {
                	mCHAR(); 

                }
                break;
            case 55 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:385: WCHAR
                {
                	mWCHAR(); 

                }
                break;
            case 56 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:391: R_EDGE
                {
                	mR_EDGE(); 

                }
                break;
            case 57 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:398: F_EDGE
                {
                	mF_EDGE(); 

                }
                break;
            case 58 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:405: STRUCT
                {
                	mSTRUCT(); 

                }
                break;
            case 59 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:412: LEQ
                {
                	mLEQ(); 

                }
                break;
            case 60 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:416: GEQ
                {
                	mGEQ(); 

                }
                break;
            case 61 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:420: NEQ
                {
                	mNEQ(); 

                }
                break;
            case 62 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:424: IF
                {
                	mIF(); 

                }
                break;
            case 63 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:427: CASE
                {
                	mCASE(); 

                }
                break;
            case 64 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:432: FOR
                {
                	mFOR(); 

                }
                break;
            case 65 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:436: TO
                {
                	mTO(); 

                }
                break;
            case 66 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:439: BY
                {
                	mBY(); 

                }
                break;
            case 67 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:442: DO
                {
                	mDO(); 

                }
                break;
            case 68 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:445: WHILE
                {
                	mWHILE(); 

                }
                break;
            case 69 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:451: REPEAT
                {
                	mREPEAT(); 

                }
                break;
            case 70 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:458: CONTINUE
                {
                	mCONTINUE(); 

                }
                break;
            case 71 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:467: TRUE
                {
                	mTRUE(); 

                }
                break;
            case 72 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:472: FALSE
                {
                	mFALSE(); 

                }
                break;
            case 73 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:478: ELSE
                {
                	mELSE(); 

                }
                break;
            case 74 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:483: ELSIF
                {
                	mELSIF(); 

                }
                break;
            case 75 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:489: USES
                {
                	mUSES(); 

                }
                break;
            case 76 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:494: TYPE
                {
                	mTYPE(); 

                }
                break;
            case 77 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:499: UNTIL
                {
                	mUNTIL(); 

                }
                break;
            case 78 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:505: THEN
                {
                	mTHEN(); 

                }
                break;
            case 79 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:510: LS
                {
                	mLS(); 

                }
                break;
            case 80 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:513: GR
                {
                	mGR(); 

                }
                break;
            case 81 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:516: POW
                {
                	mPOW(); 

                }
                break;
            case 82 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:520: RETURN
                {
                	mRETURN(); 

                }
                break;
            case 83 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:527: SHARP
                {
                	mSHARP(); 

                }
                break;
            case 84 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:533: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 85 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:538: NEG
                {
                	mNEG(); 

                }
                break;
            case 86 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:542: DOTDOT
                {
                	mDOTDOT(); 

                }
                break;
            case 87 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:549: END_PROGRAM
                {
                	mEND_PROGRAM(); 

                }
                break;
            case 88 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:561: T__106
                {
                	mT__106(); 

                }
                break;
            case 89 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:568: T__107
                {
                	mT__107(); 

                }
                break;
            case 90 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:575: T__108
                {
                	mT__108(); 

                }
                break;
            case 91 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:582: T__109
                {
                	mT__109(); 

                }
                break;
            case 92 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:589: T__110
                {
                	mT__110(); 

                }
                break;
            case 93 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:596: T__111
                {
                	mT__111(); 

                }
                break;
            case 94 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:603: T__112
                {
                	mT__112(); 

                }
                break;
            case 95 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:610: T__113
                {
                	mT__113(); 

                }
                break;
            case 96 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:617: T__114
                {
                	mT__114(); 

                }
                break;
            case 97 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:624: T__115
                {
                	mT__115(); 

                }
                break;
            case 98 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:631: T__116
                {
                	mT__116(); 

                }
                break;
            case 99 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:638: DOT
                {
                	mDOT(); 

                }
                break;
            case 100 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:642: AND
                {
                	mAND(); 

                }
                break;
            case 101 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:646: INTEGER
                {
                	mINTEGER(); 

                }
                break;
            case 102 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:654: REAL_CONSTANT
                {
                	mREAL_CONSTANT(); 

                }
                break;
            case 103 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:668: EXPONENT
                {
                	mEXPONENT(); 

                }
                break;
            case 104 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:677: IDENTIFIER
                {
                	mIDENTIFIER(); 

                }
                break;
            case 105 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:688: STRING_LITERAL
                {
                	mSTRING_LITERAL(); 

                }
                break;
            case 106 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:703: STRING_LITERAL_UNI
                {
                	mSTRING_LITERAL_UNI(); 

                }
                break;
            case 107 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:722: BINARY_INTEGER
                {
                	mBINARY_INTEGER(); 

                }
                break;
            case 108 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:737: OCTAL_INTEGER
                {
                	mOCTAL_INTEGER(); 

                }
                break;
            case 109 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:751: HEX_INTEGER
                {
                	mHEX_INTEGER(); 

                }
                break;
            case 110 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:763: COMMENT
                {
                	mCOMMENT(); 

                }
                break;
            case 111 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:771: PRAGMAS
                {
                	mPRAGMAS(); 

                }
                break;
            case 112 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:779: LINE_COMMENT
                {
                	mLINE_COMMENT(); 

                }
                break;
            case 113 :
                // J:\\Projekte\\Mea\\MPAL\\mpal-comp\\Mpal.Parser\\MPAL.g:1:792: WS
                {
                	mWS(); 

                }
                break;

        }

    }


    protected DFA19 dfa19;
	private void InitializeCyclicDFAs()
	{
	    this.dfa19 = new DFA19(this);
	}

    const string DFA19_eotS =
        "\x01\uffff\x0a\x27\x01\x4a\x01\x4c\x01\uffff\x01\x4e\x01\uffff"+
        "\x01\x50\x01\x27\x01\x53\x08\x27\x01\x6f\x01\x71\x03\uffff\x01\x73"+
        "\x04\uffff\x01\x75\x01\x27\x03\uffff\x03\x75\x02\uffff\x12\x27\x01"+
        "\u008c\x01\u008d\x03\x27\x01\x47\x01\uffff\x01\x27\x08\uffff\x01"+
        "\x27\x02\uffff\x02\x27\x01\u0095\x02\x27\x01\u0098\x01\x27\x01\u009a"+
        "\x09\x27\x01\u00a6\x04\x27\x01\u00ac\x02\x27\x0b\uffff\x01\x75\x03"+
        "\x27\x01\u00b6\x01\x27\x01\u00b9\x0c\x27\x01\x24\x02\uffff\x03\x27"+
        "\x01\u00cc\x01\u00cd\x01\u00ce\x01\u00cf\x01\uffff\x02\x27\x01\uffff"+
        "\x01\x27\x01\uffff\x0a\x27\x01\u00dd\x01\uffff\x05\x27\x01\uffff"+
        "\x04\x27\x02\uffff\x03\x27\x01\uffff\x02\x27\x01\uffff\x01\u00ee"+
        "\x03\x27\x01\u00f2\x01\u00f3\x02\x27\x01\u00f6\x01\x27\x01\u00f8"+
        "\x04\x27\x01\u0105\x01\u0106\x01\x27\x04\uffff\x01\u0108\x01\u010a"+
        "\x01\x27\x01\u010c\x03\x27\x01\u0110\x01\u0111\x03\x27\x01\u0117"+
        "\x01\uffff\x01\u0118\x01\u0119\x01\u011a\x01\u011b\x01\u011c\x01"+
        "\u011d\x06\x27\x01\u0124\x03\x27\x01\uffff\x03\x27\x02\uffff\x02"+
        "\x27\x01\uffff\x01\x27\x01\uffff\x01\u012e\x01\u012f\x01\u0130\x09"+
        "\x27\x02\uffff\x01\u013b\x01\uffff\x01\x27\x01\uffff\x01\u013d\x01"+
        "\uffff\x01\u013e\x01\u013f\x01\u0140\x02\uffff\x01\u0141\x01\u0142"+
        "\x01\u0143\x01\u0144\x01\x27\x07\uffff\x05\x27\x01\u014b\x01\uffff"+
        "\x06\x27\x01\u0153\x01\u0154\x01\x27\x03\uffff\x08\x27\x01\u015e"+
        "\x01\x27\x01\uffff\x01\x27\x08\uffff\x01\x27\x01\u0162\x01\u0163"+
        "\x01\u0164\x01\u0165\x01\x27\x01\uffff\x06\x27\x01\u016d\x02\uffff"+
        "\x01\u016e\x01\x27\x01\u0170\x03\x27\x01\u0174\x02\x27\x01\uffff"+
        "\x03\x27\x04\uffff\x01\u017b\x01\u017c\x03\x27\x01\u0180\x01\u0181"+
        "\x02\uffff\x01\x27\x01\uffff\x02\x27\x01\u0185\x01\uffff\x02\x27"+
        "\x01\u0188\x03\x27\x02\uffff\x01\u018c\x02\x27\x02\uffff\x03\x27"+
        "\x01\uffff\x01\u0192\x01\x27\x01\uffff\x03\x27\x01\uffff\x01\u0197"+
        "\x01\u0198\x02\x27\x01\u019b\x01\uffff\x01\u019c\x03\x27\x02\uffff"+
        "\x01\x27\x01\u01a1\x02\uffff\x01\x27\x01\u01a3\x01\x27\x01\u01a6"+
        "\x01\uffff\x01\x27\x01\uffff\x02\x27\x01\uffff\x01\u01aa\x01\u01ab"+
        "\x01\x27\x02\uffff\x03\x27\x01\u01b0\x01\uffff";
    const string DFA19_eofS =
        "\u01b1\uffff";
    const string DFA19_minS =
        "\x01\x09\x01\x52\x03\x41\x01\x49\x01\x43\x01\x4e\x01\x46\x01\x2b"+
        "\x01\x4f\x01\x3d\x01\x3e\x01\uffff\x01\x2a\x01\uffff\x01\x2f\x01"+
        "\x4f\x01\x2a\x01\x4f\x01\x46\x01\x41\x01\x49\x01\x44\x01\x48\x01"+
        "\x4f\x01\x45\x02\x3d\x03\uffff\x01\x2e\x04\uffff\x01\x23\x01\x2b"+
        "\x03\uffff\x01\x23\x02\x2e\x02\uffff\x01\x4f\x01\x4e\x01\x45\x01"+
        "\x52\x01\x4c\x01\x52\x01\x49\x01\x4e\x01\x41\x01\x53\x01\x52\x01"+
        "\x4e\x01\x54\x01\x52\x01\x48\x01\x49\x01\x52\x01\x44\x02\x30\x01"+
        "\x44\x01\x49\x01\x53\x01\x30\x01\uffff\x01\x54\x08\uffff\x01\x44"+
        "\x02\uffff\x01\x52\x01\x54\x01\x30\x01\x4e\x01\x54\x01\x30\x01\x4f"+
        "\x01\x30\x01\x4e\x01\x4f\x02\x45\x01\x4e\x02\x49\x01\x54\x01\x4d"+
        "\x01\x30\x01\x55\x01\x50\x01\x45\x01\x4f\x01\x30\x01\x41\x01\x45"+
        "\x0b\uffff\x01\x23\x01\x47\x01\x43\x01\x44\x01\x30\x01\x53\x01\x30"+
        "\x01\x44\x01\x53\x01\x4e\x01\x52\x01\x45\x01\x49\x01\x54\x01\x52"+
        "\x01\x44\x01\x41\x01\x4c\x01\x41\x01\x30\x02\uffff\x01\x5f\x01\x54"+
        "\x01\x45\x04\x30\x01\uffff\x01\x54\x01\x45\x01\uffff\x01\x52\x01"+
        "\uffff\x01\x54\x01\x52\x01\x41\x01\x4e\x01\x53\x01\x54\x02\x4e\x01"+
        "\x49\x01\x45\x01\x30\x01\uffff\x02\x45\x01\x4e\x01\x4c\x01\x45\x01"+
        "\uffff\x01\x4c\x01\x45\x01\x55\x01\x44\x02\uffff\x01\x52\x01\x54"+
        "\x01\x47\x01\uffff\x01\x45\x01\x49\x01\uffff\x01\x30\x01\x54\x01"+
        "\x49\x01\x54\x02\x30\x01\x4e\x01\x43\x01\x30\x01\x49\x01\x30\x01"+
        "\x52\x01\x45\x01\x59\x01\x43\x02\x30\x01\x46\x04\uffff\x02\x30\x01"+
        "\x44\x01\x30\x01\x44\x01\x4c\x01\x54\x02\x30\x02\x54\x01\x4c\x01"+
        "\x30\x01\uffff\x06\x30\x01\x41\x01\x52\x01\x47\x01\x41\x01\x49\x01"+
        "\x45\x01\x30\x01\x45\x01\x4e\x01\x55\x01\uffff\x01\x41\x01\x4e\x01"+
        "\x45\x02\uffff\x01\x47\x01\x54\x01\uffff\x01\x4e\x01\uffff\x03\x30"+
        "\x01\x4f\x01\x52\x01\x54\x01\x59\x01\x41\x01\x48\x01\x45\x01\x46"+
        "\x01\x41\x02\uffff\x01\x30\x01\uffff\x01\x41\x01\uffff\x01\x30\x01"+
        "\uffff\x03\x30\x02\uffff\x04\x30\x01\x4f\x07\uffff\x01\x54\x01\x4e"+
        "\x01\x45\x01\x4d\x01\x4f\x01\x30\x01\uffff\x01\x4d\x01\x50\x01\x54"+
        "\x01\x4e\x01\x55\x01\x52\x02\x30\x01\x47\x03\uffff\x01\x4e\x01\x52"+
        "\x01\x4f\x01\x52\x01\x50\x01\x52\x01\x49\x01\x50\x01\x30\x01\x53"+
        "\x01\uffff\x01\x4e\x08\uffff\x01\x46\x04\x30\x01\x4e\x01\uffff\x01"+
        "\x50\x01\x55\x01\x4f\x01\x50\x01\x54\x01\x45\x01\x30\x02\uffff\x01"+
        "\x30\x01\x43\x01\x30\x01\x47\x01\x55\x01\x45\x01\x30\x01\x4c\x01"+
        "\x45\x01\uffff\x01\x45\x01\x44\x01\x5f\x04\uffff\x02\x30\x01\x54"+
        "\x02\x55\x02\x30\x02\uffff\x01\x54\x01\uffff\x01\x52\x01\x43\x01"+
        "\x30\x01\uffff\x01\x45\x01\x41\x01\x30\x01\x5f\x01\x44\x01\x42\x02"+
        "\uffff\x01\x30\x02\x54\x02\uffff\x01\x49\x01\x41\x01\x54\x01\uffff"+
        "\x01\x30\x01\x54\x01\uffff\x01\x54\x01\x41\x01\x4c\x01\uffff\x02"+
        "\x30\x01\x4f\x01\x4d\x01\x30\x01\uffff\x01\x30\x01\x49\x01\x59\x01"+
        "\x4f\x02\uffff\x01\x4e\x01\x30\x02\uffff\x01\x4d\x01\x30\x01\x43"+
        "\x01\x30\x01\uffff\x01\x45\x01\uffff\x01\x4b\x01\x42\x01\uffff\x02"+
        "\x30\x01\x4c\x02\uffff\x01\x4f\x01\x43\x01\x4b\x01\x30\x01\uffff";
    const string DFA19_maxS =
        "\x01\x7b\x01\x52\x01\x5f\x02\x4f\x01\x54\x01\x53\x02\x52\x01\x58"+
        "\x01\x4f\x01\x3d\x01\x3e\x01\uffff\x01\x2a\x01\uffff\x01\x2f\x01"+
        "\x4f\x01\x2a\x01\x4f\x01\x4e\x02\x57\x01\x53\x02\x59\x01\x5f\x01"+
        "\x3e\x01\x3d\x03\uffff\x01\x2e\x04\uffff\x01\x65\x01\x39\x03\uffff"+
        "\x03\x65\x02\uffff\x01\x4f\x01\x4e\x01\x45\x01\x52\x01\x4c\x01\x52"+
        "\x01\x49\x01\x55\x01\x41\x01\x53\x01\x52\x01\x4e\x01\x54\x01\x52"+
        "\x01\x48\x01\x49\x01\x52\x01\x44\x02\x7a\x01\x44\x01\x49\x01\x53"+
        "\x01\x7a\x01\uffff\x01\x54\x08\uffff\x01\x44\x02\uffff\x01\x52\x01"+
        "\x54\x01\x7a\x01\x4e\x01\x54\x01\x7a\x01\x4f\x01\x7a\x01\x4e\x01"+
        "\x4f\x01\x45\x01\x49\x01\x4e\x02\x49\x01\x54\x01\x4d\x01\x7a\x01"+
        "\x55\x01\x50\x01\x45\x01\x4f\x01\x7a\x01\x54\x01\x45\x0b\uffff\x01"+
        "\x65\x01\x47\x01\x43\x01\x44\x01\x7a\x01\x53\x01\x7a\x01\x44\x01"+
        "\x54\x01\x4e\x01\x52\x01\x45\x01\x55\x01\x54\x01\x52\x01\x44\x01"+
        "\x41\x01\x4c\x01\x41\x01\x7a\x02\uffff\x01\x5f\x01\x54\x01\x49\x04"+
        "\x7a\x01\uffff\x01\x54\x01\x45\x01\uffff\x01\x52\x01\uffff\x01\x54"+
        "\x01\x52\x01\x41\x01\x4e\x01\x53\x01\x54\x02\x4e\x01\x49\x01\x45"+
        "\x01\x7a\x01\uffff\x02\x45\x01\x4e\x01\x4c\x01\x45\x01\uffff\x01"+
        "\x4c\x01\x45\x01\x55\x01\x44\x02\uffff\x01\x52\x01\x54\x01\x47\x01"+
        "\uffff\x01\x45\x01\x54\x01\uffff\x01\x7a\x01\x54\x01\x49\x01\x54"+
        "\x02\x7a\x01\x4e\x01\x43\x01\x7a\x01\x49\x01\x7a\x01\x52\x01\x45"+
        "\x01\x59\x01\x57\x02\x7a\x01\x46\x04\uffff\x02\x7a\x01\x44\x01\x7a"+
        "\x01\x44\x01\x4c\x01\x54\x02\x7a\x02\x54\x01\x4c\x01\x7a\x01\uffff"+
        "\x06\x7a\x01\x41\x01\x52\x01\x47\x01\x41\x01\x49\x01\x45\x01\x7a"+
        "\x01\x45\x01\x4e\x01\x55\x01\uffff\x01\x41\x01\x4e\x01\x45\x02\uffff"+
        "\x01\x47\x01\x54\x01\uffff\x01\x4e\x01\uffff\x03\x7a\x01\x55\x01"+
        "\x52\x01\x54\x01\x59\x01\x41\x01\x48\x01\x45\x01\x46\x01\x41\x02"+
        "\uffff\x01\x7a\x01\uffff\x01\x41\x01\uffff\x01\x7a\x01\uffff\x03"+
        "\x7a\x02\uffff\x04\x7a\x01\x4f\x07\uffff\x01\x54\x01\x4e\x01\x45"+
        "\x01\x4d\x01\x4f\x01\x7a\x01\uffff\x01\x4d\x01\x5f\x01\x54\x01\x4e"+
        "\x01\x55\x01\x52\x02\x7a\x01\x47\x03\uffff\x01\x4e\x01\x52\x01\x4f"+
        "\x01\x52\x01\x50\x01\x52\x01\x49\x01\x50\x01\x7a\x01\x53\x01\uffff"+
        "\x01\x4e\x08\uffff\x01\x46\x04\x7a\x01\x4e\x01\uffff\x01\x50\x01"+
        "\x55\x01\x4f\x01\x50\x01\x54\x01\x45\x01\x7a\x02\uffff\x01\x7a\x01"+
        "\x43\x01\x7a\x01\x47\x01\x55\x01\x45\x01\x7a\x01\x4c\x01\x45\x01"+
        "\uffff\x01\x45\x01\x44\x01\x5f\x04\uffff\x02\x7a\x01\x54\x02\x55"+
        "\x02\x7a\x02\uffff\x01\x54\x01\uffff\x01\x52\x01\x43\x01\x7a\x01"+
        "\uffff\x01\x45\x01\x41\x01\x7a\x01\x5f\x01\x44\x01\x42\x02\uffff"+
        "\x01\x7a\x02\x54\x02\uffff\x01\x49\x01\x41\x01\x54\x01\uffff\x01"+
        "\x7a\x01\x54\x01\uffff\x01\x54\x01\x41\x01\x4c\x01\uffff\x02\x7a"+
        "\x01\x4f\x01\x4d\x01\x7a\x01\uffff\x01\x7a\x01\x49\x01\x59\x01\x4f"+
        "\x02\uffff\x01\x4e\x01\x7a\x02\uffff\x01\x4d\x01\x7a\x01\x43\x01"+
        "\x7a\x01\uffff\x01\x45\x01\uffff\x01\x4b\x01\x42\x01\uffff\x02\x7a"+
        "\x01\x4c\x02\uffff\x01\x4f\x01\x43\x01\x4b\x01\x7a\x01\uffff";
    const string DFA19_acceptS =
        "\x0d\uffff\x01\x15\x01\uffff\x01\x17\x0d\uffff\x01\x53\x01\x54"+
        "\x01\x55\x01\uffff\x01\x58\x01\x59\x01\x5a\x01\x64\x02\uffff\x01"+
        "\x68\x01\x69\x01\x6a\x03\uffff\x01\x6f\x01\x71\x18\uffff\x01\x67"+
        "\x01\uffff\x01\x12\x01\x14\x01\x13\x01\x1d\x01\x6e\x01\x16\x01\x70"+
        "\x01\x18\x01\uffff\x01\x51\x01\x1a\x19\uffff\x01\x3b\x01\x3d\x01"+
        "\x4f\x01\x3c\x01\x50\x01\x56\x01\x63\x01\x6b\x01\x65\x01\x66\x01"+
        "\x6c\x14\uffff\x01\x0c\x01\x1b\x07\uffff\x01\x3e\x02\uffff\x01\x2e"+
        "\x01\uffff\x01\x43\x0b\uffff\x01\x41\x05\uffff\x01\x42\x04\uffff"+
        "\x01\x56\x01\x6d\x03\uffff\x01\x40\x02\uffff\x01\x03\x12\uffff\x01"+
        "\x10\x01\x19\x01\x1c\x01\x20\x0d\uffff\x01\x2c\x10\uffff\x01\x1e"+
        "\x03\uffff\x01\x36\x01\x3f\x02\uffff\x01\x1f\x01\uffff\x01\x31\x0c"+
        "\uffff\x01\x11\x01\x49\x01\uffff\x01\x21\x01\uffff\x01\x2a\x01\uffff"+
        "\x01\x22\x03\uffff\x01\x4b\x01\x24\x05\uffff\x01\x29\x01\x47\x01"+
        "\x4c\x01\x4e\x01\x2f\x01\x30\x01\x34\x06\uffff\x01\x48\x09\uffff"+
        "\x01\x37\x01\x44\x01\x0b\x0a\uffff\x01\x4a\x01\uffff\x01\x32\x01"+
        "\x33\x01\x35\x01\x23\x01\x25\x01\x26\x01\x4d\x01\x28\x06\uffff\x01"+
        "\x39\x07\uffff\x01\x08\x01\x3a\x09\uffff\x01\x61\x03\uffff\x01\x45"+
        "\x01\x52\x01\x38\x01\x01\x07\uffff\x01\x27\x01\x09\x01\uffff\x01"+
        "\x5e\x03\uffff\x01\x5d\x06\uffff\x01\x02\x01\x04\x03\uffff\x01\x05"+
        "\x01\x46\x03\uffff\x01\x5c\x02\uffff\x01\x62\x03\uffff\x01\x06\x05"+
        "\uffff\x01\x5f\x04\uffff\x01\x07\x01\x0a\x02\uffff\x01\x5b\x01\x60"+
        "\x04\uffff\x01\x57\x01\uffff\x01\x2b\x02\uffff\x01\x0d\x03\uffff"+
        "\x01\x2d\x01\x0e\x04\uffff\x01\x0f";
    const string DFA19_specialS =
        "\u01b1\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x02\x2e\x01\uffff\x02\x2e\x12\uffff\x01\x2e\x01\uffff\x01"+
            "\x29\x01\x1d\x02\uffff\x01\x24\x01\x28\x01\x0e\x01\x22\x01\x12"+
            "\x01\x1e\x01\x0f\x01\x1f\x01\x20\x01\x10\x01\x2c\x01\x2b\x01"+
            "\x25\x05\x2c\x01\x2a\x01\x2c\x01\x0b\x01\x23\x01\x1b\x01\x0c"+
            "\x01\x1c\x02\uffff\x01\x07\x01\x19\x01\x04\x01\x15\x01\x09\x01"+
            "\x02\x02\x27\x01\x14\x02\x27\x01\x16\x01\x11\x01\x0a\x01\x08"+
            "\x01\x01\x01\x27\x01\x1a\x01\x05\x01\x18\x01\x17\x01\x03\x01"+
            "\x06\x01\x13\x02\x27\x01\x0d\x01\uffff\x01\x21\x01\uffff\x01"+
            "\x27\x01\uffff\x04\x27\x01\x26\x15\x27\x01\x2d",
            "\x01\x2f",
            "\x01\x33\x0d\uffff\x01\x32\x05\uffff\x01\x30\x09\uffff\x01"+
            "\x31",
            "\x01\x34\x0d\uffff\x01\x35",
            "\x01\x38\x06\uffff\x01\x37\x06\uffff\x01\x36",
            "\x01\x3a\x0a\uffff\x01\x39",
            "\x01\x3d\x04\uffff\x01\x3e\x06\uffff\x01\x3c\x03\uffff\x01"+
            "\x3b",
            "\x01\x40\x03\uffff\x01\x3f",
            "\x01\x41\x0b\uffff\x01\x42",
            "\x01\x47\x01\uffff\x01\x47\x02\uffff\x0a\x46\x12\uffff\x01"+
            "\x45\x01\uffff\x01\x43\x09\uffff\x01\x44",
            "\x01\x48",
            "\x01\x49",
            "\x01\x4b",
            "",
            "\x01\x4d",
            "",
            "\x01\x4f",
            "\x01\x51",
            "\x01\x52",
            "\x01\x54",
            "\x01\x56\x07\uffff\x01\x55",
            "\x01\x58\x07\uffff\x01\x57\x05\uffff\x01\x5b\x04\uffff\x01"+
            "\x59\x02\uffff\x01\x5a",
            "\x01\x5c\x08\uffff\x01\x5e\x04\uffff\x01\x5d",
            "\x01\x61\x04\uffff\x01\x60\x02\uffff\x01\x62\x01\uffff\x01"+
            "\x63\x04\uffff\x01\x5f",
            "\x01\x68\x01\x64\x05\uffff\x01\x65\x02\uffff\x01\x66\x06\uffff"+
            "\x01\x67",
            "\x01\x69\x09\uffff\x01\x6a",
            "\x01\x6b\x19\uffff\x01\x6c",
            "\x01\x6d\x01\x6e",
            "\x01\x70",
            "",
            "",
            "",
            "\x01\x72",
            "",
            "",
            "",
            "",
            "\x01\x74\x0a\uffff\x01\x76\x01\uffff\x0a\x2c\x0b\uffff\x01"+
            "\x76\x1f\uffff\x01\x76",
            "\x01\x47\x01\uffff\x01\x47\x02\uffff\x0a\x46",
            "",
            "",
            "",
            "\x01\x77\x0a\uffff\x01\x76\x01\uffff\x0a\x2c\x0b\uffff\x01"+
            "\x76\x1f\uffff\x01\x76",
            "\x01\x76\x01\uffff\x06\x2c\x01\x78\x03\x2c\x0b\uffff\x01\x76"+
            "\x1f\uffff\x01\x76",
            "\x01\x76\x01\uffff\x0a\x2c\x0b\uffff\x01\x76\x1f\uffff\x01"+
            "\x76",
            "",
            "",
            "\x01\x79",
            "\x01\x7a",
            "\x01\x7b",
            "\x01\x7c",
            "\x01\x7d",
            "\x01\x7e",
            "\x01\x7f",
            "\x01\u0080\x06\uffff\x01\u0081",
            "\x01\u0082",
            "\x01\u0083",
            "\x01\u0084",
            "\x01\u0085",
            "\x01\u0086",
            "\x01\u0087",
            "\x01\u0088",
            "\x01\u0089",
            "\x01\u008a",
            "\x01\u008b",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u008e",
            "\x01\u008f",
            "\x01\u0090",
            "\x0a\x46\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "\x01\u0091",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\u0092",
            "",
            "",
            "\x01\u0093",
            "\x01\u0094",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0096",
            "\x01\u0097",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0099",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u009b",
            "\x01\u009c",
            "\x01\u009d",
            "\x01\u009f\x03\uffff\x01\u009e",
            "\x01\u00a0",
            "\x01\u00a1",
            "\x01\u00a2",
            "\x01\u00a3",
            "\x01\u00a4",
            "\x0a\x27\x07\uffff\x03\x27\x01\u00a5\x16\x27\x04\uffff\x01"+
            "\x27\x01\uffff\x1a\x27",
            "\x01\u00a7",
            "\x01\u00a8",
            "\x01\u00a9",
            "\x01\u00aa",
            "\x0a\x27\x07\uffff\x13\x27\x01\u00ab\x06\x27\x04\uffff\x01"+
            "\x27\x01\uffff\x1a\x27",
            "\x01\u00ad\x0e\uffff\x01\u00ae\x03\uffff\x01\u00af",
            "\x01\u00b0",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\u00b2\x0a\uffff\x01\x76\x01\uffff\x0a\x2c\x0b\uffff\x01"+
            "\x76\x1f\uffff\x01\x76",
            "\x01\u00b3",
            "\x01\u00b4",
            "\x01\u00b5",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u00b7",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\u00b8\x01\uffff\x1a"+
            "\x27",
            "\x01\u00ba",
            "\x01\u00bb\x01\u00bc",
            "\x01\u00bd",
            "\x01\u00be",
            "\x01\u00bf",
            "\x01\u00c0\x0b\uffff\x01\u00c1",
            "\x01\u00c2",
            "\x01\u00c3",
            "\x01\u00c4",
            "\x01\u00c5",
            "\x01\u00c6",
            "\x01\u00c7",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "",
            "\x01\u00c8",
            "\x01\u00c9",
            "\x01\u00ca\x03\uffff\x01\u00cb",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "\x01\u00d0",
            "\x01\u00d1",
            "",
            "\x01\u00d2",
            "",
            "\x01\u00d3",
            "\x01\u00d4",
            "\x01\u00d5",
            "\x01\u00d6",
            "\x01\u00d7",
            "\x01\u00d8",
            "\x01\u00d9",
            "\x01\u00da",
            "\x01\u00db",
            "\x01\u00dc",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "\x01\u00de",
            "\x01\u00df",
            "\x01\u00e0",
            "\x01\u00e1",
            "\x01\u00e2",
            "",
            "\x01\u00e3",
            "\x01\u00e4",
            "\x01\u00e5",
            "\x01\u00e6",
            "",
            "",
            "\x01\u00e7",
            "\x01\u00e8",
            "\x01\u00e9",
            "",
            "\x01\u00ea",
            "\x01\u00ec\x05\uffff\x01\u00ed\x04\uffff\x01\u00eb",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u00ef",
            "\x01\u00f0",
            "\x01\u00f1",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u00f4",
            "\x01\u00f5",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u00f7",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u00f9",
            "\x01\u00fa",
            "\x01\u00fb",
            "\x01\u0104\x02\uffff\x01\u00fc\x02\uffff\x01\u0103\x06\uffff"+
            "\x01\u00fd\x01\uffff\x01\u0102\x01\u00fe\x01\u00ff\x01\uffff"+
            "\x01\u0100\x01\u0101",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0107",
            "",
            "",
            "",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\u0109\x01\uffff\x1a"+
            "\x27",
            "\x01\u010b",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u010d",
            "\x01\u010e",
            "\x01\u010f",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0112",
            "\x01\u0113",
            "\x01\u0114",
            "\x0a\x27\x07\uffff\x11\x27\x01\u0115\x08\x27\x04\uffff\x01"+
            "\u0116\x01\uffff\x1a\x27",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u011e",
            "\x01\u011f",
            "\x01\u0120",
            "\x01\u0121",
            "\x01\u0122",
            "\x01\u0123",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0125",
            "\x01\u0126",
            "\x01\u0127",
            "",
            "\x01\u0128",
            "\x01\u0129",
            "\x01\u012a",
            "",
            "",
            "\x01\u012b",
            "\x01\u012c",
            "",
            "\x01\u012d",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0132\x05\uffff\x01\u0131",
            "\x01\u0133",
            "\x01\u0134",
            "\x01\u0135",
            "\x01\u0136",
            "\x01\u0137",
            "\x01\u0138",
            "\x01\u0139",
            "\x01\u013a",
            "",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "\x01\u013c",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0145",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\u0146",
            "\x01\u0147",
            "\x01\u0148",
            "\x01\u0149",
            "\x01\u014a",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "\x01\u014c",
            "\x01\u014d\x0e\uffff\x01\u014e",
            "\x01\u014f",
            "\x01\u0150",
            "\x01\u0151",
            "\x01\u0152",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0155",
            "",
            "",
            "",
            "\x01\u0156",
            "\x01\u0157",
            "\x01\u0158",
            "\x01\u0159",
            "\x01\u015a",
            "\x01\u015b",
            "\x01\u015c",
            "\x01\u015d",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u015f",
            "",
            "\x01\u0160",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\u0161",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0166",
            "",
            "\x01\u0167",
            "\x01\u0168",
            "\x01\u0169",
            "\x01\u016a",
            "\x01\u016b",
            "\x01\u016c",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u016f",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0171",
            "\x01\u0172",
            "\x01\u0173",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0175",
            "\x01\u0176",
            "",
            "\x01\u0177",
            "\x01\u0178",
            "\x01\u0179",
            "",
            "",
            "",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\u017a\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u017d",
            "\x01\u017e",
            "\x01\u017f",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "",
            "\x01\u0182",
            "",
            "\x01\u0183",
            "\x01\u0184",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "\x01\u0186",
            "\x01\u0187",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0189",
            "\x01\u018a",
            "\x01\u018b",
            "",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u018d",
            "\x01\u018e",
            "",
            "",
            "\x01\u018f",
            "\x01\u0190",
            "\x01\u0191",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0193",
            "",
            "\x01\u0194",
            "\x01\u0195",
            "\x01\u0196",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u0199",
            "\x01\u019a",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u019d",
            "\x01\u019e",
            "\x01\u019f",
            "",
            "",
            "\x01\u01a0",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "",
            "",
            "\x01\u01a2",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u01a4",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\u01a5\x01\uffff\x1a"+
            "\x27",
            "",
            "\x01\u01a7",
            "",
            "\x01\u01a8",
            "\x01\u01a9",
            "",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            "\x01\u01ac",
            "",
            "",
            "\x01\u01ad",
            "\x01\u01ae",
            "\x01\u01af",
            "\x0a\x27\x07\uffff\x1a\x27\x04\uffff\x01\x27\x01\uffff\x1a"+
            "\x27",
            ""
    };

    static readonly short[] DFA19_eot = DFA.UnpackEncodedString(DFA19_eotS);
    static readonly short[] DFA19_eof = DFA.UnpackEncodedString(DFA19_eofS);
    static readonly char[] DFA19_min = DFA.UnpackEncodedStringToUnsignedChars(DFA19_minS);
    static readonly char[] DFA19_max = DFA.UnpackEncodedStringToUnsignedChars(DFA19_maxS);
    static readonly short[] DFA19_accept = DFA.UnpackEncodedString(DFA19_acceptS);
    static readonly short[] DFA19_special = DFA.UnpackEncodedString(DFA19_specialS);
    static readonly short[][] DFA19_transition = DFA.UnpackEncodedStringArray(DFA19_transitionS);

    protected class DFA19 : DFA
    {
        public DFA19(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 19;
            this.eot = DFA19_eot;
            this.eof = DFA19_eof;
            this.min = DFA19_min;
            this.max = DFA19_max;
            this.accept = DFA19_accept;
            this.special = DFA19_special;
            this.transition = DFA19_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( PROGRAM | FUNCTION | VAR | VAR_TEMP | CONSTANT | VAR_INPUT | VAR_IN_OUT | STRING | WSTRING | VAR_OUTPUT | ARRAY | OF | END_FUNCTION | FUNCTION_BLOCK | END_FUNCTION_BLOCK | NOT | EXIT | ASSIGN | ASSIGN2 | COLON | LBRACKED | LRBRACKED | COMMA | DIV | MOD | MUL | OR | XOR | EQU | VOID | SINT | INT | DINT | LINT | USINT | UINT | UDINT | ULINT | COUNTER | TIMER | TIME | DATE_LIT | TIME_OF_DAY_LIT | TOD | DATE_AND_TIME | DT | BOOL | BYTE | WORD | DWORD | LWORD | REAL | LREAL | CHAR | WCHAR | R_EDGE | F_EDGE | STRUCT | LEQ | GEQ | NEQ | IF | CASE | FOR | TO | BY | DO | WHILE | REPEAT | CONTINUE | TRUE | FALSE | ELSE | ELSIF | USES | TYPE | UNTIL | THEN | LS | GR | POW | RETURN | SHARP | PLUS | NEG | DOTDOT | END_PROGRAM | T__106 | T__107 | T__108 | T__109 | T__110 | T__111 | T__112 | T__113 | T__114 | T__115 | T__116 | DOT | AND | INTEGER | REAL_CONSTANT | EXPONENT | IDENTIFIER | STRING_LITERAL | STRING_LITERAL_UNI | BINARY_INTEGER | OCTAL_INTEGER | HEX_INTEGER | COMMENT | PRAGMAS | LINE_COMMENT | WS );"; }
        }

    }

 
    
}
