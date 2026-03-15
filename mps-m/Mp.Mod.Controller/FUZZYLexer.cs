// $ANTLR 3.2 Sep 23, 2009 12:02:23 J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g 2010-11-18 12:12:14

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


public partial class FUZZYLexer : Lexer {
    public const int UND = 10;
    public const int WS = 15;
    public const int T__16 = 16;
    public const int THEN = 5;
    public const int T__18 = 18;
    public const int T__17 = 17;
    public const int LINE_COMMENT = 14;
    public const int WENN = 6;
    public const int STRING_LITERAL_UNI = 12;
    public const int OR = 9;
    public const int AND = 8;
    public const int COMMENT = 13;
    public const int ODER = 11;
    public const int EOF = -1;
    public const int DANN = 7;
    public const int IF = 4;

    // delegates
    // delegators

    public FUZZYLexer() 
    {
		InitializeCyclicDFAs();
    }
    public FUZZYLexer(ICharStream input)
		: this(input, null) {
    }
    public FUZZYLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g";} 
    }

    // $ANTLR start "IF"
    public void mIF() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IF;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:7:4: ( 'IF' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:7:6: 'IF'
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

    // $ANTLR start "THEN"
    public void mTHEN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = THEN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:8:6: ( 'THEN' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:8:8: 'THEN'
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

    // $ANTLR start "WENN"
    public void mWENN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WENN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:9:6: ( 'WENN' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:9:8: 'WENN'
            {
            	Match("WENN"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WENN"

    // $ANTLR start "DANN"
    public void mDANN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DANN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:10:6: ( 'DANN' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:10:8: 'DANN'
            {
            	Match("DANN"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DANN"

    // $ANTLR start "AND"
    public void mAND() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AND;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:11:5: ( 'AND' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:11:7: 'AND'
            {
            	Match("AND"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AND"

    // $ANTLR start "OR"
    public void mOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:12:4: ( 'OR' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:12:6: 'OR'
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

    // $ANTLR start "UND"
    public void mUND() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = UND;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:13:5: ( 'UND' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:13:7: 'UND'
            {
            	Match("UND"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "UND"

    // $ANTLR start "ODER"
    public void mODER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ODER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:14:6: ( 'ODER' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:14:8: 'ODER'
            {
            	Match("ODER"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ODER"

    // $ANTLR start "T__16"
    public void mT__16() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__16;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:15:7: ( '(' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:15:9: '('
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
    // $ANTLR end "T__16"

    // $ANTLR start "T__17"
    public void mT__17() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__17;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:16:7: ( ')' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:16:9: ')'
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
    // $ANTLR end "T__17"

    // $ANTLR start "T__18"
    public void mT__18() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__18;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:17:7: ( ';' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:17:9: ';'
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
    // $ANTLR end "T__18"

    // $ANTLR start "STRING_LITERAL_UNI"
    public void mSTRING_LITERAL_UNI() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STRING_LITERAL_UNI;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:47:20: ( '\"' (~ ( '\\\\' | '\"' ) )* '\"' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:47:22: '\"' (~ ( '\\\\' | '\"' ) )* '\"'
            {
            	Match('\"'); 
            	// J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:47:26: (~ ( '\\\\' | '\"' ) )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( ((LA1_0 >= '\u0000' && LA1_0 <= '!') || (LA1_0 >= '#' && LA1_0 <= '[') || (LA1_0 >= ']' && LA1_0 <= '\uFFFF')) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:47:27: ~ ( '\\\\' | '\"' )
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
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

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

    // $ANTLR start "COMMENT"
    public void mCOMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:50:9: ( '(*' ( options {greedy=false; } : . )* '*)' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:50:11: '(*' ( options {greedy=false; } : . )* '*)'
            {
            	Match("(*"); 

            	// J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:50:16: ( options {greedy=false; } : . )*
            	do 
            	{
            	    int alt2 = 2;
            	    int LA2_0 = input.LA(1);

            	    if ( (LA2_0 == '*') )
            	    {
            	        int LA2_1 = input.LA(2);

            	        if ( (LA2_1 == ')') )
            	        {
            	            alt2 = 2;
            	        }
            	        else if ( ((LA2_1 >= '\u0000' && LA2_1 <= '(') || (LA2_1 >= '*' && LA2_1 <= '\uFFFF')) )
            	        {
            	            alt2 = 1;
            	        }


            	    }
            	    else if ( ((LA2_0 >= '\u0000' && LA2_0 <= ')') || (LA2_0 >= '+' && LA2_0 <= '\uFFFF')) )
            	    {
            	        alt2 = 1;
            	    }


            	    switch (alt2) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:50:44: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop2;
            	    }
            	} while (true);

            	loop2:
            		;	// Stops C# compiler whining that label 'loop2' has no statements

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

    // $ANTLR start "LINE_COMMENT"
    public void mLINE_COMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LINE_COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:53:14: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:53:16: '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
            {
            	Match("//"); 

            	// J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:53:21: (~ ( '\\n' | '\\r' ) )*
            	do 
            	{
            	    int alt3 = 2;
            	    int LA3_0 = input.LA(1);

            	    if ( ((LA3_0 >= '\u0000' && LA3_0 <= '\t') || (LA3_0 >= '\u000B' && LA3_0 <= '\f') || (LA3_0 >= '\u000E' && LA3_0 <= '\uFFFF')) )
            	    {
            	        alt3 = 1;
            	    }


            	    switch (alt3) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:53:21: ~ ( '\\n' | '\\r' )
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
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements

            	// J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:53:35: ( '\\r' )?
            	int alt4 = 2;
            	int LA4_0 = input.LA(1);

            	if ( (LA4_0 == '\r') )
            	{
            	    alt4 = 1;
            	}
            	switch (alt4) 
            	{
            	    case 1 :
            	        // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:53:35: '\\r'
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
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:55:5: ( ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' ) )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:55:8: ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' )
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
        // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:8: ( IF | THEN | WENN | DANN | AND | OR | UND | ODER | T__16 | T__17 | T__18 | STRING_LITERAL_UNI | COMMENT | LINE_COMMENT | WS )
        int alt5 = 15;
        alt5 = dfa5.Predict(input);
        switch (alt5) 
        {
            case 1 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:10: IF
                {
                	mIF(); 

                }
                break;
            case 2 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:13: THEN
                {
                	mTHEN(); 

                }
                break;
            case 3 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:18: WENN
                {
                	mWENN(); 

                }
                break;
            case 4 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:23: DANN
                {
                	mDANN(); 

                }
                break;
            case 5 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:28: AND
                {
                	mAND(); 

                }
                break;
            case 6 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:32: OR
                {
                	mOR(); 

                }
                break;
            case 7 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:35: UND
                {
                	mUND(); 

                }
                break;
            case 8 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:39: ODER
                {
                	mODER(); 

                }
                break;
            case 9 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:44: T__16
                {
                	mT__16(); 

                }
                break;
            case 10 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:50: T__17
                {
                	mT__17(); 

                }
                break;
            case 11 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:56: T__18
                {
                	mT__18(); 

                }
                break;
            case 12 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:62: STRING_LITERAL_UNI
                {
                	mSTRING_LITERAL_UNI(); 

                }
                break;
            case 13 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:81: COMMENT
                {
                	mCOMMENT(); 

                }
                break;
            case 14 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:89: LINE_COMMENT
                {
                	mLINE_COMMENT(); 

                }
                break;
            case 15 :
                // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:1:102: WS
                {
                	mWS(); 

                }
                break;

        }

    }


    protected DFA5 dfa5;
	private void InitializeCyclicDFAs()
	{
	    this.dfa5 = new DFA5(this);
	}

    const string DFA5_eotS =
        "\x08\uffff\x01\x11\x09\uffff";
    const string DFA5_eofS =
        "\x12\uffff";
    const string DFA5_minS =
        "\x01\x09\x05\uffff\x01\x44\x01\uffff\x01\x2a\x09\uffff";
    const string DFA5_maxS =
        "\x01\x57\x05\uffff\x01\x52\x01\uffff\x01\x2a\x09\uffff";
    const string DFA5_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\uffff\x01"+
        "\x07\x01\uffff\x01\x0a\x01\x0b\x01\x0c\x01\x0e\x01\x0f\x01\x06\x01"+
        "\x08\x01\x0d\x01\x09";
    const string DFA5_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA5_transitionS = {
            "\x02\x0d\x01\uffff\x02\x0d\x12\uffff\x01\x0d\x01\uffff\x01"+
            "\x0b\x05\uffff\x01\x08\x01\x09\x05\uffff\x01\x0c\x0b\uffff\x01"+
            "\x0a\x05\uffff\x01\x05\x02\uffff\x01\x04\x04\uffff\x01\x01\x05"+
            "\uffff\x01\x06\x04\uffff\x01\x02\x01\x07\x01\uffff\x01\x03",
            "",
            "",
            "",
            "",
            "",
            "\x01\x0f\x0d\uffff\x01\x0e",
            "",
            "\x01\x10",
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
            get { return "1:1: Tokens : ( IF | THEN | WENN | DANN | AND | OR | UND | ODER | T__16 | T__17 | T__18 | STRING_LITERAL_UNI | COMMENT | LINE_COMMENT | WS );"; }
        }

    }

 
    
}
