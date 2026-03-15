// $ANTLR 3.2 Sep 23, 2009 12:02:23 J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g 2010-11-18 12:12:13

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
* FUZZY grammar                                                                                        *
*                                                                                                     *
* Copyright (C) 2010 Atesion GmbH. All rights reserved.                             *
*******************************************************************************************************/
public partial class FUZZYParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"IF", 
		"THEN", 
		"WENN", 
		"DANN", 
		"AND", 
		"OR", 
		"UND", 
		"ODER", 
		"STRING_LITERAL_UNI", 
		"COMMENT", 
		"LINE_COMMENT", 
		"WS", 
		"'('", 
		"')'", 
		"';'"
    };

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
    public const int COMMENT = 13;
    public const int AND = 8;
    public const int ODER = 11;
    public const int EOF = -1;
    public const int DANN = 7;
    public const int IF = 4;

    // delegates
    // delegators



        public FUZZYParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public FUZZYParser(ITokenStream input, RecognizerSharedState state)
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
		get { return FUZZYParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g"; }
    }


    public class rule_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "rule"
    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:30:1: rule : ( ( IF | WENN ) expression result )* EOF ;
    public FUZZYParser.rule_return rule() // throws RecognitionException [1]
    {   
        FUZZYParser.rule_return retval = new FUZZYParser.rule_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set1 = null;
        IToken EOF4 = null;
        FUZZYParser.expression_return expression2 = default(FUZZYParser.expression_return);

        FUZZYParser.result_return result3 = default(FUZZYParser.result_return);


        object set1_tree=null;
        object EOF4_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:33:6: ( ( ( IF | WENN ) expression result )* EOF )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:33:8: ( ( IF | WENN ) expression result )* EOF
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:33:8: ( ( IF | WENN ) expression result )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( (LA1_0 == IF || LA1_0 == WENN) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:33:9: ( IF | WENN ) expression result
            			    {
            			    	set1 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == IF || input.LA(1) == WENN ) 
            			    	{
            			    	    input.Consume();
            			    	    adaptor.AddChild(root_0, (object)adaptor.Create(set1));
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_expression_in_rule205);
            			    	expression2 = expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, expression2.Tree);
            			    	PushFollow(FOLLOW_result_in_rule208);
            			    	result3 = result();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, result3.Tree);

            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

            	EOF4=(IToken)Match(input,EOF,FOLLOW_EOF_in_rule214); 

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
    // $ANTLR end "rule"

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
    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:35:1: expression : and_expression ( ( OR | ODER ) and_expression )* ;
    public FUZZYParser.expression_return expression() // throws RecognitionException [1]
    {   
        FUZZYParser.expression_return retval = new FUZZYParser.expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set6 = null;
        FUZZYParser.and_expression_return and_expression5 = default(FUZZYParser.and_expression_return);

        FUZZYParser.and_expression_return and_expression7 = default(FUZZYParser.and_expression_return);


        object set6_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:35:12: ( and_expression ( ( OR | ODER ) and_expression )* )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:35:14: and_expression ( ( OR | ODER ) and_expression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_and_expression_in_expression223);
            	and_expression5 = and_expression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, and_expression5.Tree);
            	// J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:35:29: ( ( OR | ODER ) and_expression )*
            	do 
            	{
            	    int alt2 = 2;
            	    int LA2_0 = input.LA(1);

            	    if ( (LA2_0 == OR || LA2_0 == ODER) )
            	    {
            	        alt2 = 1;
            	    }


            	    switch (alt2) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:35:30: ( OR | ODER ) and_expression
            			    {
            			    	set6=(IToken)input.LT(1);
            			    	set6 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == OR || input.LA(1) == ODER ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set6), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_and_expression_in_expression233);
            			    	and_expression7 = and_expression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, and_expression7.Tree);

            			    }
            			    break;

            			default:
            			    goto loop2;
            	    }
            	} while (true);

            	loop2:
            		;	// Stops C# compiler whining that label 'loop2' has no statements


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
    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:37:1: and_expression : comparison ( ( AND | UND ) comparison )* ;
    public FUZZYParser.and_expression_return and_expression() // throws RecognitionException [1]
    {   
        FUZZYParser.and_expression_return retval = new FUZZYParser.and_expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set9 = null;
        FUZZYParser.comparison_return comparison8 = default(FUZZYParser.comparison_return);

        FUZZYParser.comparison_return comparison10 = default(FUZZYParser.comparison_return);


        object set9_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:37:16: ( comparison ( ( AND | UND ) comparison )* )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:37:18: comparison ( ( AND | UND ) comparison )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_comparison_in_and_expression243);
            	comparison8 = comparison();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, comparison8.Tree);
            	// J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:37:29: ( ( AND | UND ) comparison )*
            	do 
            	{
            	    int alt3 = 2;
            	    int LA3_0 = input.LA(1);

            	    if ( (LA3_0 == AND || LA3_0 == UND) )
            	    {
            	        alt3 = 1;
            	    }


            	    switch (alt3) 
            		{
            			case 1 :
            			    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:37:30: ( AND | UND ) comparison
            			    {
            			    	set9=(IToken)input.LT(1);
            			    	set9 = (IToken)input.LT(1);
            			    	if ( input.LA(1) == AND || input.LA(1) == UND ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set9), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_comparison_in_and_expression253);
            			    	comparison10 = comparison();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, comparison10.Tree);

            			    }
            			    break;

            			default:
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements


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
    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:39:1: comparison : ( STRING_LITERAL_UNI | '(' expression ')' );
    public FUZZYParser.comparison_return comparison() // throws RecognitionException [1]
    {   
        FUZZYParser.comparison_return retval = new FUZZYParser.comparison_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken STRING_LITERAL_UNI11 = null;
        IToken char_literal12 = null;
        IToken char_literal14 = null;
        FUZZYParser.expression_return expression13 = default(FUZZYParser.expression_return);


        object STRING_LITERAL_UNI11_tree=null;
        object char_literal12_tree=null;
        object char_literal14_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:39:12: ( STRING_LITERAL_UNI | '(' expression ')' )
            int alt4 = 2;
            int LA4_0 = input.LA(1);

            if ( (LA4_0 == STRING_LITERAL_UNI) )
            {
                alt4 = 1;
            }
            else if ( (LA4_0 == 16) )
            {
                alt4 = 2;
            }
            else 
            {
                NoViableAltException nvae_d4s0 =
                    new NoViableAltException("", 4, 0, input);

                throw nvae_d4s0;
            }
            switch (alt4) 
            {
                case 1 :
                    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:39:14: STRING_LITERAL_UNI
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	STRING_LITERAL_UNI11=(IToken)Match(input,STRING_LITERAL_UNI,FOLLOW_STRING_LITERAL_UNI_in_comparison263); 
                    		STRING_LITERAL_UNI11_tree = (object)adaptor.Create(STRING_LITERAL_UNI11);
                    		adaptor.AddChild(root_0, STRING_LITERAL_UNI11_tree);


                    }
                    break;
                case 2 :
                    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:39:36: '(' expression ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal12=(IToken)Match(input,16,FOLLOW_16_in_comparison268); 
                    	PushFollow(FOLLOW_expression_in_comparison271);
                    	expression13 = expression();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, expression13.Tree);
                    	char_literal14=(IToken)Match(input,17,FOLLOW_17_in_comparison273); 

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
    // $ANTLR end "comparison"

    public class result_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "result"
    // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:41:1: result : ( THEN | DANN ) STRING_LITERAL_UNI ';' ;
    public FUZZYParser.result_return result() // throws RecognitionException [1]
    {   
        FUZZYParser.result_return retval = new FUZZYParser.result_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set15 = null;
        IToken STRING_LITERAL_UNI16 = null;
        IToken char_literal17 = null;

        object set15_tree=null;
        object STRING_LITERAL_UNI16_tree=null;
        object char_literal17_tree=null;

        try 
    	{
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:41:9: ( ( THEN | DANN ) STRING_LITERAL_UNI ';' )
            // J:\\Projekte\\Mea\\MeaProcess\\mp-windows\\Mp.Controller\\FUZZY.g:41:12: ( THEN | DANN ) STRING_LITERAL_UNI ';'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set15=(IToken)input.LT(1);
            	set15 = (IToken)input.LT(1);
            	if ( input.LA(1) == THEN || input.LA(1) == DANN ) 
            	{
            	    input.Consume();
            	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set15), root_0);
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}

            	STRING_LITERAL_UNI16=(IToken)Match(input,STRING_LITERAL_UNI,FOLLOW_STRING_LITERAL_UNI_in_result293); 
            		STRING_LITERAL_UNI16_tree = (object)adaptor.Create(STRING_LITERAL_UNI16);
            		adaptor.AddChild(root_0, STRING_LITERAL_UNI16_tree);

            	char_literal17=(IToken)Match(input,18,FOLLOW_18_in_result295); 

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
    // $ANTLR end "result"

    // Delegated rules


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_set_in_rule199 = new BitSet(new ulong[]{0x0000000000011000UL});
    public static readonly BitSet FOLLOW_expression_in_rule205 = new BitSet(new ulong[]{0x00000000000000A0UL});
    public static readonly BitSet FOLLOW_result_in_rule208 = new BitSet(new ulong[]{0x0000000000000050UL});
    public static readonly BitSet FOLLOW_EOF_in_rule214 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_and_expression_in_expression223 = new BitSet(new ulong[]{0x0000000000000A02UL});
    public static readonly BitSet FOLLOW_set_in_expression226 = new BitSet(new ulong[]{0x0000000000011000UL});
    public static readonly BitSet FOLLOW_and_expression_in_expression233 = new BitSet(new ulong[]{0x0000000000000A02UL});
    public static readonly BitSet FOLLOW_comparison_in_and_expression243 = new BitSet(new ulong[]{0x0000000000000502UL});
    public static readonly BitSet FOLLOW_set_in_and_expression246 = new BitSet(new ulong[]{0x0000000000011000UL});
    public static readonly BitSet FOLLOW_comparison_in_and_expression253 = new BitSet(new ulong[]{0x0000000000000502UL});
    public static readonly BitSet FOLLOW_STRING_LITERAL_UNI_in_comparison263 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_16_in_comparison268 = new BitSet(new ulong[]{0x0000000000011000UL});
    public static readonly BitSet FOLLOW_expression_in_comparison271 = new BitSet(new ulong[]{0x0000000000020000UL});
    public static readonly BitSet FOLLOW_17_in_comparison273 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_result286 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_STRING_LITERAL_UNI_in_result293 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_18_in_result295 = new BitSet(new ulong[]{0x0000000000000002UL});

}
