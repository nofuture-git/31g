grammar NfUri;

//RFC 2396 Section A.
uriReference : ( uriAboslute | uriRelative ) ('#' uriFragment)? ;
uriAboslute : uriScheme ':' ( hierPart | opaquePart ) ;
uriRelative : ( netPath | absPath | relPath ) ('?' uriQuery)? ;

hierPart : ( netPath | absPath ) ('?' uriQuery )? ;
opaquePart : uricNoSlash URIC* ;

uricNoSlash : UNRESERVED | ESCAPED 
         | '\u003B' //semicolon
		 | '\u003F' //question mark
		 | '\u003A' //colon
		 | '\u0040' //at symbol
		 | '\u0026' //ampersand
		 | '\u003D' //equal sign
		 | '\u002B' //plus sign
		 | '\u0024' //dollar sign
		 | '\u002C' //comma
		 ;

netPath : '//' authority ( absPath )? ;
absPath : '/' pathSegments ;
relPath : relSegment (absPath)? ;
relSegment : (UNRESERVED | ESCAPED | '\u003B' | '\u0040' | '\u0026' | '\u003D' | '\u002B' | '\u0024' | '\u002C')+ ;

uriScheme : ALPHA (ALPHA | DIGIT | '\u002B' | '\u002D' | '\u002E' )* ;
authority : uriServer | regName ;
regName : (UNRESERVED | ESCAPED | '\u0024' | '\u002C' | '\u003B' | '\u003A' | '\u0040' | '\u0026' | '\u003D' | '\u002B')+  ;
uriServer : (( userInfo '@')? hostAndPort)? ;
userInfo : (UNRESERVED | ESCAPED | '\u003B' | '\u003A' | '\u0026' | '\u003D' | '\u002B' | '\u0024' | '\u002C')* ;

hostAndPort : uriHost (':' uriPort )? ;
uriHost : hostName | ipv4Address | ipv6Address ;
hostName : (domainLabel '.')* topLabel ('.')? ;
domainLabel : ALPHA_NUM | ALPHA_NUM ( ALPHA_NUM | '-')* ALPHA_NUM ;
topLabel : ALPHA | ALPHA (ALPHA_NUM | '-')* ALPHA_NUM ;


//----------------------from RFC 5954 Sec. 4.1
ipv6Address : '[' ( 
              HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' leastSig32
              | '::' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' leastSig32
			  | (HEX_FOUR)? '::' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' leastSig32
			  | ( (HEX_FOUR ':' )? HEX_FOUR)? '::' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' leastSig32
			  | ( (HEX_FOUR ':' HEX_FOUR ':')? HEX_FOUR )? '::' HEX_FOUR ':' HEX_FOUR ':' leastSig32
			  | ( (HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':')? HEX_FOUR)? '::' HEX_FOUR ':' leastSig32
			  | ( (HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':')? HEX_FOUR)? '::' leastSig32
			  | ( (HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':')? HEX_FOUR)? '::' HEX_FOUR
			  | ( (HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':' HEX_FOUR ':') HEX_FOUR)? '::' 
			  ) ']'
			;

leastSig32 : ( HEX_FOUR ':' HEX_FOUR ) | ipv4Address ;
ipv4Address : DEC_OCTET '.' DEC_OCTET '.' DEC_OCTET '.' DEC_OCTET ;

//----------------------

uriPort : DIGIT+ ;

pathSegments : uriSegment ( '/' uriSegment)* ;
uriSegment : PCHAR* (';' uriParam)* ;
uriParam : PCHAR* ;

uriQuery : URIC* ;

uriFragment : URIC* ;

PCHAR : UNRESERVED | ESCAPED | '\u003A'  | '\u0040' | '\u0026' | '\u003D' | '\u002B' | '\u0024' | '\u002C' ;

URIC : RESERVED | UNRESERVED | ESCAPED ; 
fragment RESERVED : '\u003B' //semicolon
         | '\u002F' //foward-slash
		 | '\u003F' //question mark
		 | '\u003A' //colon
		 | '\u0040' //at symbol
		 | '\u0026' //ampersand
		 | '\u003D' //equal sign
		 | '\u002B' //plus sign
		 | '\u0024' //dollar sign
		 | '\u002C' //comma
		 ;
		 
UNRESERVED : ALPHA_NUM | MARK ;
fragment MARK : '\u002D'  //minus sign
     | '\u005F'  //underscore
     | '\u002E'  //period
     | '\u0021'  //exclaimation
     | '\u007E'  //tilde
     | '\u002A'  //star
     | '\u0027'  //single quote
     | '\u0028'  //open parenth
     | '\u0029'  //close parenth
     ;	
		 
ESCAPED : '%' HEX HEX ;

//--- RFC 5954
DEC_OCTET : [0-9]
         | [1-9] [0-9]
		 | '1' [0-9] [0-9]
		 | '2' [0-4] [0-9]
		 | '25' [0-5]
		 ;

HEX_FOUR : HEX (HEX (HEX (HEX)?)?)? ;
//----

HEX : DIGIT | [a-fA-F] ;

ALPHA_NUM : ALPHA | DIGIT ;

ALPHA  : [a-zA-Z] ;
DIGIT  :  [0-9] ;

WS : (' '|'\t'|'\r'|'\n')+ -> skip ;