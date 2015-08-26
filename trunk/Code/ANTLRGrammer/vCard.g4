grammar vCard ; 

//http://tools.ietf.org/html/rfc2426

vcfile : vCardProfile* EOF ;

vCardProfile : NL? 'BEGIN' ':' 'VCARD' NL (vCardExpr)* 'END' ':' 'VCARD' NL?;

vCardExpr : vCardVer NL
		  | vCardFn NL
		  | vCardN NL
          | vCardTel NL
		  | vCardAdr NL
		  | vCardNote NL
		  | vCardClass NL
		  | vCardBday NL
		  | vCardEmail NL
		  | vCardOrg NL
		  | vCardUrl NL
		  | NL
		  ;

vCardVer   : 'VERSION:' NUMBER  ;
vCardFn    : 'FN:' .*?  ;
vCardN     : 'N:' .*?  ;
vCardTel   : 'TEL' (vCardType)? (vCardEncode)? ':' naPhNum  ;
vCardAdr   : 'ADR' (vCardType)? (vCardEncode)? ':' .*?  ;
vCardNote  : 'NOTE:' .*?  ;
vCardClass : ('X-')? 'CLASS:' .*?  ;
vCardBday  : 'BDAY:' YYYYMMDD ;
vCardEmail : 'EMAIL' (vCardType)? (vCardEncode)? ':' .*? ;
vCardOrg   : 'ORG:' .*? ;
vCardUrl   : 'URL:' ('http://')? .*? ;

wordOrNum    :  ( NUMBER | WORD )   ;
vCardType    :  ';' ('TYPE' '=')? ANY_TYPE ( (';' 'TYPE' '=' ANY_TYPE) | (',' ANY_TYPE) )*  ;
naPhNum      :  ( '('? NPA ')'? NXX '-'? XXXX ) | ( PHNUM ) ;
vCardEncode  : ';ENCODING=QUOTED-PRINTABLE' ;

ANY_TYPE   : ( DOM_TYPE | INTL_TYPE | POSTAL_TYPE | PARCEL_TYPE | HOME_TYPE | WORK_TYPE | E_INTERNET | E_X400 | PREF | CELL_TYPE )  ;
DOM_TYPE    : [Dd][Oo][Mm] ;
INTL_TYPE   : [Ii][Nn][Tt][Ll] ;
POSTAL_TYPE : [Pp][Oo][Ss][Tt][Aa][Ll] ;
PARCEL_TYPE : [Pp][Aa][Rr][Cc][Ee][Ll] ;
HOME_TYPE   : [Hh][Oo][Mm][Ee] ;
WORK_TYPE   : [Ww][Oo][Rr][Kk] ;
CELL_TYPE   : [Cc][Ee][Ll][Ll] ;

E_INTERNET  : [Ii][Nn][Tt][Ee][Rr][Nn][Ee][Tt] ;
E_X400      : [Xx]'400' ;

PREF        : [Pp][Ee][Rr][Ff] ;


// north american only
PHNUM       : '1'? [2-9] [0-9] [0-9] [2-9] ('0' '0' | [1-9] [2-9] | [2-9] [1-9] ) [0-9] [0-9] [0-9] [0-9] ;

YYYYMMDD    : [1-2][0-9][0-9][0-9][0-1][1-9][0-3][0-9] ; //yyyyMMdd
		  
NPA         : [2-9] [0-9] [0-9] ;                            // numbering plan area code
NXX         : [2-9] ('0' '0' | [1-9] [2-9] | [2-9] [1-9] ) ; // central office exchange
XXXX        : [0-9] [0-9] [0-9] [0-9] ;                      // subscriber name
		  
NUMBER    :   '-'? ('.' DIGIT+ | DIGIT+ ('.' DIGIT*)? ) ;
		   
NL : '\r'? '\n' ; 

WORD : (LETTER | '.' | ',' )+ ;
DIGIT  :  [0-9] ;

fragment LETTER  : [a-zA-Z] ;

WS :   [ \t]+ -> skip ;