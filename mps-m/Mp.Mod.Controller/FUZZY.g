/******************************************************************************************************
* FUZZY grammar                                                                                        *
*                                                                                                     *
* Copyright (C) 2010 Atesion GmbH. All rights reserved.                             *
*******************************************************************************************************/
grammar FUZZY;

options 
{
    output=AST;
//    language=CSharp2;

    language=Java;
    k =3;
}

tokens 
{
    IF			= 'IF' ;
    THEN		= 'THEN';
    WENN 		= 'WENN' ;
    DANN		= 'DANN';
    AND		        = 'AND';
    OR                  = 'OR';
    UND                 = 'UND';
    ODER		= 'ODER';
    
}

/******************************************************************************************************
* Parser rules                                                                                        *
******************************************************************************************************/
rule : ((IF|WENN) expression  result )*  EOF!;

expression : and_expression ((OR|ODER)^ and_expression)*;

and_expression : comparison ((AND|UND)^ comparison)*;

comparison :	STRING_LITERAL_UNI |  '('! expression ')'! ;
	
result 	:  (THEN|DANN)^ STRING_LITERAL_UNI ';'!;	

/******************************************************************************************************
* Lexer rules                                                                                        *
******************************************************************************************************/

STRING_LITERAL_UNI : '"' (~('\\'|'"'))* '"';


COMMENT : '(*' ( options {greedy=false;} : . )* '*)' {$channel=HIDDEN;} ;


LINE_COMMENT : '//' ~('\n'|'\r')* '\r'? '\n'   {$channel=HIDDEN;} ;

WS  :  (' '|'\r'|'\t'|'\u000C'|'\n') {$channel=HIDDEN;} ;
