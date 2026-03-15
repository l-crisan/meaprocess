/******************************************************************************************************
* MPAL grammar                                                                                        *
*                                                                                                     *
* Copyright (C) 2006-2010 Atesion GmbH. All rights reserved.                             *
*******************************************************************************************************/
grammar MPAL;

options 
{
    output=AST;
    language=CSharp2;

//    language=Java;
    k =3;
}

tokens 
{
    PROGRAM			= 'PROGRAM' ;
    FUNCTION			= 'FUNCTION';
    VAR				= 'VAR';
    VAR_TEMP			= 'VAR_TEMP';
    CONSTANT			= 'CONSTANT';
    VAR_INPUT      		= 'VAR_INPUT';
    VAR_IN_OUT     		= 'VAR_IN_OUT';
    STRING         		= 'STRING';
    WSTRING        		= 'WSTRING';
    VAR_OUTPUT     		= 'VAR_OUTPUT';
    ARRAY          		= 'ARRAY';
    OF             		= 'OF';
    END_FUNCTION   		= 'END_FUNCTION';
    FUNCTION_BLOCK 		= 'FUNCTION_BLOCK';
    END_FUNCTION_BLOCK 		= 'END_FUNCTION_BLOCK';
    NOT 	       		= 'NOT';
    EXIT 			= 'EXIT';
    ASSIGN        		= ':=' ;
    ASSIGN2			= '=>';
    COLON          		= ':';	
    LBRACKED       		= '[';
    LRBRACKED      		= '(';
    COMMA	       		= ',';
    DIV	       			= '/';
    MOD	       			= 'MOD';
    MUL 	      		= '*';
    OR	       			= 'OR';
    XOR	       			= 'XOR';
    EQU	       			= '=';
    VOID	       		= 'VOID';
    SINT	       		= 'SINT';
    INT	       			= 'INT';
    DINT           		= 'DINT';
    LINT           		= 'LINT';
    USINT          		= 'USINT';
    UINT           		= 'UINT';
    UDINT          		= 'UDINT';
    ULINT          		= 'ULINT';
    COUNTER			= 'COUNTER';
    TIMER			= 'TIMER';
    TIME           		= 'TIME';
    DATE_LIT       		= 'DATE';
    TIME_OF_DAY_LIT  		= 'TIME_OF_DAY';
    TOD             	 	= 'TOD';
    DATE_AND_TIME    		= 'DATE_AND_TIME';
    DT		 		= 'DT';	 
    BOOL         	   	= 'BOOL';
    BYTE 			= 'BYTE';
    WORD			= 'WORD';
    DWORD			= 'DWORD';
    LWORD			= 'LWORD';
    REAL			= 'REAL';
    LREAL			= 'LREAL';
    CHAR		 	= 'CHAR' ;
    WCHAR		 	= 'WCHAR';
    R_EDGE			= 'R_EDGE';
    F_EDGE			= 'F_EDGE';
    STRUCT           		= 'STRUCT';
    LEQ              		= '<=';
    GEQ		 		= '>=';
    NEQ 			= '<>';
    IF				= 'IF';
    CASE			= 'CASE';
    FOR				= 'FOR';
    TO				= 'TO';
    BY				= 'BY';
    DO				= 'DO';
    WHILE			= 'WHILE';
    REPEAT			= 'REPEAT';
    CONTINUE			= 'CONTINUE';
    TRUE			= 'TRUE';
    FALSE			= 'FALSE';
    ELSE			= 'ELSE';
    ELSIF			= 'ELSIF';
    USES			= 'USES' ; 
    TYPE			= 'TYPE';
    UNTIL			= 'UNTIL';
    THEN			= 'THEN';
    LS				= '<';
    GR				= '>';
    POW				= '**';
    RETURN			= 'RETURN';
    SHARP			= '#';
    PLUS			= '+';
    NEG				= '-';
    DOTDOT			= '..'; 
    END_PROGRAM                 = 'END_PROGRAM'; 
    END_FUNCTION		= 'END_FUNCTION';	
    END_FUNCTION_BLOCK          = 'END_FUNCTION_BLOCK';
}

/******************************************************************************************************
* Parser rules                                                                                        *
******************************************************************************************************/
mpal : (program_declaration | data_type_declaration| function_declaration  | function_block_declaration )+ EOF ;
	 
program	: PROGRAM^ IDENTIFIER (io_var_declarations  | other_var_declarations | data_type_declaration)* function_body END_PROGRAM;

program_declaration  : PROGRAM^ IDENTIFIER  (io_var_declarations | other_var_declarations)* function_body END_PROGRAM ;

function_declaration : FUNCTION^ function_type  (io_var_declarations | var_declarations)* function_body END_FUNCTION;

function_type : IDENTIFIER ':'^ simple_specification ; 
	
function_block_declaration :  FUNCTION_BLOCK^ IDENTIFIER ( io_var_declarations | other_var_declarations )* function_body  END_FUNCTION_BLOCK;

string_type_size : LBRACKED^ INTEGER ']'!;

character_string :  STRING_LITERAL_UNI | STRING_LITERAL;

string_type_declaration :  (STRING | WSTRING)^ string_type_size string_type_decl_init? ;

string_type_decl_init :	(ASSIGN^ character_string);

real_literal :	(real_type_name SHARP^)? real_literal_body;

real_literal_body  : (PLUS^|NEG^)?  REAL_CONSTANT;

integer_literal  :  ((PLUS^|NEG^)? INTEGER) | BINARY_INTEGER | OCTAL_INTEGER | HEX_INTEGER;

bit_string_literal : ((BYTE | WORD | DWORD | LWORD) SHARP^ )? integer_literal;

spec_integer_literal : ( integer_type_name SHARP^) (integer_literal);

constant_literal : real_literal| spec_integer_literal | character_string | bool_literal | bit_string_literal;	 

simple_spec_init : simple_specification^ (ASSIGN! (constant_literal| IDENTIFIER))? ;
	
enumerated_value : IDENTIFIER SHARP IDENTIFIER ;

enumerated_specification : ( '('^ (enumerated_value|IDENTIFIER ) (COMMA! (enumerated_value|IDENTIFIER))* ')'! );

enumerated_spec_init : enumerated_specification^ enumerated_init? ;

enumerated_init : ASSIGN^ (enumerated_value|IDENTIFIER);

single_element_type_declaration : ( simple_spec_init | subrange_spec_init  | enumerated_spec_init) ;

subrange_spec_init :  subrange_specification (':=' INTEGER)?;

subrange_specification : integer_type_name '(' subrange ')';

array_spec_init : (array_specification)  (':='! array_initialization)? ;

array_range  : LBRACKED^ subrange (COMMA! subrange)* ']'! ;

udt_array_spec_init :	IDENTIFIER ASSIGN! array_initialization;

non_generic_type_name : elemetary_type_name | IDENTIFIER  | structure_declaration| string_type_declaration;

array_specification : (ARRAY^ array_range OF! non_generic_type_name);

structure_element_declaration : IDENTIFIER (COMMA! IDENTIFIER)* ':'^ ( simple_spec_init | subrange_spec_init | enumerated_spec_init | array_spec_init | string_spec| initialized_structure | structure_declaration);

structure_declaration : STRUCT^ structure_element_declaration ';'!(structure_element_declaration ';'!)* 'END_STRUCT'!;

array_initial_element : constant_literal | enumerated_value | IDENTIFIER | structure_initialization | array_initialization;

array_initial_elements : array_initial_element | INTEGER '(' (array_initial_element)? ')' ;

array_initialization : LBRACKED^ array_initial_elements (COMMA! array_initial_elements)* ']'! ;

structure_element_initialization : IDENTIFIER ':='^ (enumerated_value| IDENTIFIER | constant_literal | array_initialization | structure_initialization);

structure_initialization : LRBRACKED^ structure_element_initialization (COMMA! structure_element_initialization)* ')'!;

initialized_structure : IDENTIFIER ':='!  structure_initialization;

structure_specification : structure_declaration | initialized_structure;

type_declaration  : IDENTIFIER ':'^ ( string_type_declaration  | single_element_type_declaration | structure_specification  | array_spec_init );

data_type_declaration : TYPE^ type_declaration ';'! (type_declaration ';'!)* 'END_TYPE'!;

io_var_declarations :  input_output_declarations | input_declarations  | output_declarations ;

input_output_declarations : VAR_IN_OUT^ ( var_declaration ';'!)+ 'END_VAR'!;

input_declarations : VAR_INPUT^ (var_init_decl ';'!)+  'END_VAR'!;

output_declarations : VAR_OUTPUT^ (var_init_decl ';'!)+ 'END_VAR'!;

var_init_decl : var_list ':'^ (string_spec |  simple_spec_init|  (BOOL (R_EDGE | F_EDGE))| array_spec_init| structure_specification | subrange_spec_init | enumerated_spec_init| udt_array_spec_init) ;

var_declaration : var_list ':'^ (structure_specification| string_spec|  array_specification|  simple_specification | subrange_specification | enumerated_specification );

var_list : IDENTIFIER (COMMA! IDENTIFIER)*;

string_spec :  (STRING^ | WSTRING^) (LBRACKED! INTEGER ']'!) (':='! character_string )?;

other_var_declarations : var_declarations | temp_var_decls;

var_declarations : VAR^ (CONSTANT)? var_init_decl ';'! ((var_init_decl ';'!))* 'END_VAR'!;

temp_var_decls : VAR_TEMP^ (CONSTANT)? var_init_decl ';'! ((var_init_decl ';'!))* 'END_VAR'!;

function_body : statement_list ;

statement_list : (statement ';'!)+;

do_statement : DO^ statement_list;

for_statement : FOR^ for_assign  for_to (for_by)? do_statement 'END_FOR'!;

for_assign : IDENTIFIER ':='^ expression;

for_to : TO^ expression;

for_by 	: BY^ expression;

while_statement : WHILE^ expression do_statement 'END_WHILE'!;

repeat_statement : REPEAT^ statement_list repeat_until 'END_REPEAT'!;

repeat_until : UNTIL^ expression;

iteration_statement : for_statement  | while_statement | repeat_statement | EXIT| CONTINUE;

statement : ';' | fb_invoke_or_assigment | selection_statement | iteration_statement| RETURN;

fb_invoke_or_assigment : variable (ASSIGN^ expression | ('('^ (param_assignment (COMMA! param_assignment)*)? ')'!)) ;

//fb_invoke_or_assigment options{ k=4;} : assignment_statement; //A lookahead from 4 tokens.

selection_statement : if_statement | case_statement;

if_statement : IF^ expression if_then (if_elif)* (else_statement)? 'END_IF'!;
        
if_then : THEN^ statement_list;

if_elif : ELSIF^ expression if_then;

else_statement : ELSE^ (':'?)! statement_list ;

case_statement : CASE^ expression case_of else_statement? 'END_CASE'!;

case_of : OF^ (case_element)+;

case_element : case_list ':'^ statement_list;

case_list : case_list_element (COMMA^ case_list_element)*;

case_list_element : number_literal| subrange |  enumerated_value | IDENTIFIER ;


param_assignment : ((IDENTIFIER ASSIGN^)? expression) | (NOT? IDENTIFIER ASSIGN2^ variable) ;

expression : xor_expression (OR^ xor_expression)*;

xor_expression : and_expression (XOR^ and_expression)*;

and_expression : comparison (AND^ comparison)*;

comparison : equ_expression ( (EQU | NEQ)^ equ_expression)*;

equ_expression : add_expression (comparison_operators^ add_expression)*;
         
add_expression : term ( (PLUS|NEG)^ term )*;

term : power_expression (multiply_operator^ power_expression)*;

power_expression : unary_expression (POW^ unary_expression)*;

unary_expression : (unary_operator^)? primary_expression;

primary_expression : function_call | variable| constant_literal| enumerated_value|  '('! expression ')'! ;

function_call : (IDENTIFIER '('^ param_assignment (COMMA! param_assignment)*  ')'! ) ;
 
unary_operator : (PLUS^ |NEG^ | NOT^);
         
multiply_operator : MUL^ | DIV^ | MOD; 

number_literal : bit_string_literal ;

comparison_operators : LS | GR | LEQ | GEQ ;

variable : multi_element_variable ;

multi_element_variable : IDENTIFIER ( DOT^ IDENTIFIER | subscript_list^)*;

subscript_list : LBRACKED^  subscript (COMMA! subscript)* ']'!;

subscript : expression ;

//fb_invocation : (IDENTIFIER | IDENTIFIER DOT^ IDENTIFIER ) '('^ (param_assignment (COMMA! param_assignment)*)? ')'!; 
//fb_invocation : '('^ (param_assignment (COMMA! param_assignment)*)? ')'!; 


simple_specification : elemetary_type_name  | IDENTIFIER  | IDENTIFIER DOT IDENTIFIER;

elemetary_type_name : USINT | UINT | UDINT | ULINT | SINT | INT | DINT | LINT | 
                       TIME | VOID | DATE_LIT | TIME_OF_DAY_LIT | TOD | DATE_AND_TIME| DT | 
                       BOOL | BYTE | WORD  | DWORD | LWORD | REAL | LREAL | STRING | WSTRING| CHAR | WCHAR | COUNTER | TIMER ;

real_type_name : REAL | LREAL ;

integer_type_name : SINT | INT | DINT | LINT | USINT | UINT | UDINT | ULINT;

bool_literal : ( BOOL SHARP^ )? (TRUE | FALSE);

subrange  : range_index DOTDOT^ range_index;

range_index :  (integer_literal| IDENTIFIER);

/******************************************************************************************************
* Lexer rules                                                                                         *
******************************************************************************************************/

DOT : '.' ('.' {$type=DOTDOT;})?;

AND : '&' |'AND';

INTEGER : ('0'..'9')+;

REAL_CONSTANT :	INTEGER  ( ( {
					if(input.LA(2)=='.')
					{//If follow .. than is only an integer.
					        state.type = INTEGER;
				                state.channel = _channel;
				                return;
					}
					
				} '.'  ('0'..'9')+ (EXPONENT)? )?  |  EXPONENT );
	 

EXPONENT  : ('E' | 'e') (PLUS|NEG)? INTEGER;	

IDENTIFIER :   ('a'..'z' | 'A'..'Z' | '_' ) ( 'a'..'z' | 'A'..'Z' | '_'  |'0'..'9')* ;

STRING_LITERAL : '\'' ('\'\'' | ~('\''))* '\'' ;

STRING_LITERAL_UNI : '"' (~('\\'|'"'))* '"';

BINARY_INTEGER :  '2#'  ('1' | '0')* ;

OCTAL_INTEGER : '8#' ('0'..'7')*;

HEX_INTEGER : '16#' ('0'..'9'| 'A'..'F')*;

COMMENT : '(*' ( options {greedy=false;} : . )* '*)' {$channel=HIDDEN;} ;

PRAGMAS : '{' ( options {greedy=false;} : . )* '}' {$channel=HIDDEN;} ;

LINE_COMMENT : '//' ~('\n'|'\r')* '\r'? '\n'   {$channel=HIDDEN;} ;

WS  :  (' '|'\r'|'\t'|'\u000C'|'\n') {$channel=HIDDEN;} ;
