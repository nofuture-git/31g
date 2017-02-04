grammar DotNetIlTypeName;

utPrius 
   : dotNetGenericName
   | dotNetAsmTypeName
   | dotNetAsmName
   | dotNetName
   ;

dotNetGenericName : dotNetName GENERIC_COUNTER ( '[' dotNetGenericArg (',' dotNetGenericArg)*? ']' )? (', ' dotNetAsmName)? ;

dotNetGenericArg 
     : '[' dotNetGenericName ']'
	 | '[' dotNetAsmTypeName ']'
	 | dotNetName 
	 ;

dotNetAsmTypeName :  dotNetName ', ' dotNetAsmName ;

dotNetAsmName : dotNetName ', ' asmVersion ', ' asmCulture ', ' asmPubToken (', ' asmProArch)? ;

dotNetName :  IDENTIFIER (ID_SEPARATOR IDENTIFIER)*? ;

asmVersion : VERSION '=' FOUR_SET_VER ;

asmCulture : CULTURE '=neutral' ;

asmPubToken : PUBLIC_KEY_TOKEN '=' tokenValue ;

asmProArch : PROCESSOR_ARCHITECTURE '=' CHIP_SETS ;

tokenValue 
  : HEX_VALUE
  | 'null'
  ;

GENERIC_COUNTER : '`' DIGIT ;

FOUR_SET_VER : DIGIT+ '.' DIGIT+ '.' DIGIT+ '.' DIGIT+ ;

VERSION : 'Version' ;
CULTURE : 'Culture' ;
PUBLIC_KEY_TOKEN : 'PublicKeyToken' ;
PROCESSOR_ARCHITECTURE : [Pp] 'rocessorArchitecture' ;

HEX_VALUE : HEX_DIGIT+ ;

CHIP_SETS 
	: 'MSIL'
	| 'Arm'
	| 'Amd64'
	| 'IA64'
	| 'None'
	| 'X86'
	;

IDENTIFIER
  : ID_FIRST_CHAR ID_ANY_OTHER*
  ;
  
ID_SEPARATOR
 : '.'
 | '+'
 ; 
 
NL : '\r'? '\n' ;

fragment HEX_DIGIT 
  : '0'..'9'
  | 'A'..'F'
  | 'a'..'f'
  ;
 
fragment ID_FIRST_CHAR
  : LETTER
  | '_'
  ;
  
fragment ID_ANY_OTHER
  : LETTER
  | DIGIT
  | '_'
  ;
 
fragment LETTER
   : '\u0041'..'\u005A'
   | '\u0061'..'\u007A'
   ;   

fragment DIGIT
  : '\u0030' // DIGIT ZERO
  | '\u0031' // DIGIT ONE
  | '\u0032' // DIGIT TWO
  | '\u0033' // DIGIT THREE
  | '\u0034' // DIGIT FOUR
  | '\u0035' // DIGIT FIVE
  | '\u0036' // DIGIT SIX
  | '\u0037' // DIGIT SEVEN
  | '\u0038' // DIGIT EIGHT
  | '\u0039' // DIGIT NINE
  ;  
