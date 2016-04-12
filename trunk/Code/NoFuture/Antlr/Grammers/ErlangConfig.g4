grammar ErlangConfig;

/*
 * Parser Rules
 * http://erlang.org/doc/man/config.html
 */

erlConfigFile
	: '[' erlApplication (',' erlApplication)*? ('\r\n')? '].' ('\r\n')? EOF ;

erlApplication  : '{' erlName ',' erlArray '}' ;

erlNameValuePair : '{' erlName ',' erlAtomValue '}'       # NameValue2Value
                 | '{' erlName ',' erlNameValuePair '}'   # NameValue2NameValue
				 | '{' erlName ',' erlArray '}'           # NameValue2Array
				 ;

erlArray : '[' ( erlAtomValue | erlNameValuePair | erlArray ) (',' ( erlAtomValue | erlNameValuePair | erlArray ))*? ']' 
         | erlEmptyArray 
		 ;

erlName  : DBL_QUOTE_ATOM
         | SNGL_QUOTE_ATOM
		 | ATOM
		 ;

erlEmptyArray : '[' ']' ;

erlAtomValue : DBL_QUOTE_ATOM
             | SNGL_QUOTE_ATOM
		     | ATOM
		     ;

/*
 * Lexer Rules
 */

DBL_QUOTE_ATOM : '"' ( ~['\u007B' '\u007D' '\u005B' '\u005DB' '\u002C' '\u0022' '\u0027' '"'] | ' ')*  '"' ;
SNGL_QUOTE_ATOM : '\'' ( ~['\u007B' '\u007D' '\u005B' '\u005DB' '\u002C' '\u0022' '\u0027' '\''] | ' ')*  '\'' ;
ATOM : ~['\u007B' '\u007D' '\u005B' '\u005DB' '\u002C' '\u0022' '\u0027']+ ;

WS : ['\t' '\r' '\n' ' ' ]+ -> skip ;

