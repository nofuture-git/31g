grammar AspClassic;

aspPage     : aspHeader aspBody EOF;

aspHeader   : vbDirective* ;
aspBody     : ( vbBlock | vbInclude | NL )* ;

vbDirective : '<%@' expr '%>' ;
vbInclude   : '<!--' vbIncludeFile '-->' ;
vbIncludeFile : '#' VB_INCLUDE VB_FILE '=' STRING ;

vbBlock  : '<%' vbBody '%>' ;
vbBody   : ( stmt* | expr ) ;

stmt : vbClass
     | vbFunction
	 | vbSub
	 | vbGetProperty
	 | vbSetProperty
	 | vbWith
     | vbDim 
	 | vbReDim
     | vbConst
	 | vbExecute
     | vbDoLoop 
	 | vbForEachLoop
	 | vbForLoop
	 | vbWhile
	 | vbSelect
	 | vbExit
	 | vbIfThen
	 | vbErase
	 | expr '=' vbLiteral
	 | expr NL
	 | NL
	 ;

vbClass       : VB_CLASS ID vbBody VB_END VB_CLASS ;
vbFunction    : (VB_PUBLIC (VB_DEFAULT)? | VB_PRIVATE)? VB_FUNCTION ID exprList vbBodyWithExit VB_END VB_FUNCTION ;
vbSub         : (VB_PUBLIC (VB_DEFAULT)? | VB_PRIVATE)? VB_SUB ID ( exprList )? vbBodyWithExit VB_END VB_SUB ;
vbGetProperty : (VB_PUBLIC (VB_DEFAULT)? | VB_PRIVATE)? VB_PROPERTY VB_GET ID ( exprList )? vbBodyWithExit VB_END VB_PROPERTY ;
vbSetProperty : (VB_PUBLIC | VB_PRIVATE)? VB_PROPERTY (VB_SET | VB_GET) ID exprList vbBodyWithExit VB_END VB_PROPERTY ;
vbDoLoop      : VB_DO ( (VB_WHILE | VB_UNTIL) expr)? vbBodyWithExit VB_LOOP ( (VB_WHILE | VB_UNTIL) expr)? ;
vbWhile       : VB_WHILE expr vbBody VB_WEND  ;
vbForEachLoop : VB_FOR VB_EACH ID VB_IN expr vbBodyWithExit VB_NEXT (ID)? ;
vbForLoop     : VB_FOR ID '=' expr VB_TO expr (VB_STEP expr)? vbBodyWithExit VB_NEXT (expr)?  ;
vbIfThen      : VB_IF expr VB_THEN vbBodyWithExit (VB_ELSEIF expr VB_THEN vbBodyWithExit )* (VB_ELSE vbBodyWithExit)? VB_ENDIF   ;
vbWith        : VB_WITH ID vbBody VB_END VB_WITH ;
vbSelect      : VB_SELECT vbSelectBody VB_END VB_SELECT ;
vbExit        : VB_EXIT (VB_DO | VB_FOR | VB_FUNCTION | VB_PROPERTY | VB_SUB) ;
vbDim         : (VB_PUBLIC | VB_PRIVATE | VB_DIM) ID VB_ARRAYINIT? (',' NL ID VB_ARRAYINIT? )* NL;
vbErase       : VB_ERASE ID NL ;
vbReDim       : VB_REDIM (VB_PRESERVE)? ID ('(' INT ')')? (',' NL ID ('(' INT ')')?)* NL ;
vbConst       : (VB_PUBLIC | VB_PRIVATE)? VB_CONST ID '=' (STRING | INT) ('+' (STRING | INT))* NL ;
vbExecute     : (VB_EXECUTE | VB_EXECUTE_GLOBAL) stmt NL;

vbBodyWithExit : stmt (vbExit | stmt)* ;
vbSelectBody   : VB_CASE expr ( VB_CASE expr stmt* )* (VB_CASE VB_ELSE stmt*)?  ;
	 
expr : VB_SET ID 
	 | ID
	 | ID ('(' exprList? ')' | exprList )
	 | vbGlobalFx
	 | vbCast
	 | exprArg
	 | expr VB_IS expr
	 | expr '<' expr
	 | expr '<=' expr
	 | expr '=<' expr
	 | expr '>' expr
	 | expr '>=' expr
	 | expr '=>' expr
	 | expr '<>' expr
	 | expr '><' expr
	 | expr '^' expr
	 | expr '/' expr
	 | expr '*' expr
	 | expr '\\' expr
	 | expr VB_MOD expr
	 | expr '+' expr
	 | expr '-' expr
	 | expr '&' expr
	 | VB_NOT expr
	 | expr VB_AND expr
	 | expr VB_OR expr
	 | expr VB_XOR expr
	 | expr VB_EQV expr
	 | expr VB_IMP expr
	 | 'vbCr'
	 | 'vbCrLf'
	 | 'vbTab'
	 ;

vbLiteral : VB_NOTHING
	 | VB_DATE_LITERAL
	 | INT
	 | STRING
	 | VB_TRUE
	 | VB_FALSE
	 ;
	 
vbGlobalFx   : vbCtor 
             | vbNowFx
			 | vbTimeFx
			 | vbLenFx
			 | vbOnErr
			 | vbErrorRaise
			 | vbIsNullFx
			 | vbIsEmptyFx
			 | vbIsArrayFx
			 | vbIsObjectFx
			 | vbIsNumericFx
			 | vbArrayInitFx
			 | vbFilterArrayFx
			 | vbJoinArrayFx
			 | vbLBoundArrayFx 
			 | vbSplitFx
			 | vbDateAddFx
			 | vbDateDiffFx
			 | vbDatePartFx
			 | vbDateSerialFx
			 | vbDateValueFx
			 | vbDayFx
			 | vbHourFx
			 | vbMinuteFx
			 | vbMonthFx
			 | vbMonthNameFx
			 | vbSecondFx
			 | vbTimerFx
			 | vbTimeValueFx
			 | vbWeekdayFx
			 | vbWeekdayNameFx
			 | vbYearFx
			 | vbTypeNameFx
			 | vbVarTypeFx 
			 | vbAbsFx       
			 | vbCosFx       
			 | vbExpFx       
			 | vbFixFx       
			 | vbIntFx       
			 | vbLogFx       
			 | vbRandomizeFx 
			 | vbRndFx       
			 | vbRoundFx     
			 | vbSgnFx       
			 | vbSinFx       
			 | vbSqrFx       
			 | vbTanFx       
			 | vbEvalFx        
			 | vbLoadPictureFx
			 | vbGetObjectFx
			 | vbGetRefFx
			 | vbFormatCurrencyFx
			 | vbFormatNumberFx  
			 | vbFormatPercentFx
			 | vbFormatDateTimeFx
			 | vbInStrFx
			 | vbInStrBFx
			 | vbInStrRevFx
			 | vbLCaseFx
			 | vbLeftFx
			 | vbLeftBFx
			 | vbLTrimFx
			 | vbMidFx
			 | vbMidBFx
			 | vbReplaceFx
			 | vbRightFx
			 | vbRightBFx
			 | vbRTrimFx
			 | vbSpaceFx
			 | vbStrCompFx
			 | vbStringFx
			 | vbStringReverseFx
			 | vbTrimFx
			 | vbUCaseFx
             | vbChrFx
             ;	 
	 
vbCtor           : (VB_SERVER '.' | VB_WSCRIPT '.')? VB_CREATEOBJECT '(' STRING ')' NL ;
vbNowFx          : VB_NOW ('('')')?  ;
vbTimeFx         : VB_TIME ('('')')? ;
	 
vbIsNullFx       : VB_ISNULL exprArg  ;
vbIsEmptyFx      : VB_ISEMPTY exprArg ;	 
vbIsArrayFx      : VB_ISARRAY exprArg ;
vbIsObjectFx     : VB_ISOBJECT exprArg ;
vbIsNumericFx    : VB_ISNUMERIC exprArg ;
vbLenFx          : VB_LEN exprArg  ;
vbLenBFx         : VB_LENB exprArg  ;
vbFilterArrayFx  : VB_FILTER '(' ID ',' STRING (',' (VB_TRUE | VB_FALSE) (',' INT)? )? ')'  ;
vbArrayInitFx    : VB_ARRAY '(' expr (',' expr)* ')'   ;
vbJoinArrayFx    : VB_JOIN '(' ID (',' STRING)? ')'   ;
vbLBoundArrayFx  : VB_LBOUND '(' ID (',' INT)? ')'    ;
vbSplitFx        : VB_SPLIT '(' expr (',' STRING (',' INT (',' ('0' | '1' | 'vbBinaryCompare' | 'vbTextCompare'))? )? )? ')'  ;	
vbDateAddFx      : VB_DATEADD '(' vbDatePart ',' expr ',' expr ')' ;
vbDateDiffFx     : VB_DATEDIFF '(' vbDatePart ',' expr ',' expr (',' (vbDayOfWeek | INT) (',' (vbFirstWeek | INT) )? )? ')' ;
vbDatePartFx     : VB_DATEPART '(' vbDatePart ',' expr (',' (vbDayOfWeek | INT) (',' (vbFirstWeek | INT) )? )? ')' ;
vbDateSerialFx   : VB_DATESERIAL '(' expr ',' expr ',' expr ')'  ;
vbDateValueFx    : VB_DATEVALUE exprArg  ;
vbDayFx          : VB_DAY exprArg ;
vbHourFx         : VB_HOUR exprArg ;
vbMinuteFx       : VB_MINUTE exprArg ;
vbMonthFx        : VB_MONTH  exprArg ;
vbMonthNameFx    : VB_MONTHNAME ('1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12') (',' (VB_TRUE | VB_FALSE) )?  ;
vbSecondFx       : VB_SECOND exprArg ;
vbTimerFx        : VB_TIMER '(' ')'  ;
vbTimeValueFx    : VB_TIMEVALUE exprArg ;
vbWeekdayFx      : VB_WEEKDAY '(' expr (',' (vbDayOfWeek | INT) )?  ')' ;
vbWeekdayNameFx  : VB_WEEKDAYNAME '(' expr (',' (VB_TRUE | VB_FALSE) (',' (vbDayOfWeek | INT))? )? ')' ;
vbYearFx         : VB_YEAR exprArg ;
vbOnErr          : VB_ON VB_ERR ;
vbErrorRaise     : VB_ERRORRAISE exprList ;
vbTypeNameFx     : VB_TYPENAME exprArg ; 
vbVarTypeFx      : VB_VARTYPE exprArg ;
vbAbsFx          : VB_ABS exprArg ;
vbCosFx          : VB_COS exprArg ;
vbExpFx          : VB_EXP exprArg ;
vbFixFx          : VB_FIX exprArg ;
vbIntFx          : VB_INT exprArg ;
vbLogFx          : VB_LOG exprArg ;
vbRandomizeFx    : VB_RANDOMIZE exprArg ;
vbRndFx          : VB_RND exprArg;
vbRoundFx        : VB_ROUND '(' expr (',' INT)? ')' ;
vbSgnFx          : VB_SGN exprArg ;
vbSinFx          : VB_SIN exprArg ;
vbSqrFx          : VB_SQR exprArg ;
vbTanFx          : VB_TAN exprArg ;
vbEvalFx         : VB_EVAL exprArg ;
vbLoadPictureFx  : VB_LOADPICTURE '(' STRING  ')' ;
vbGetObjectFx    : VB_GET_OBJECT '(' STRING (',' ID) ')' ;
vbGetRefFx       : VB_GET_REF '(' STRING ')' ;
vbFormatCurrencyFx: VB_FORMATCURRENCY '(' exprList  ')' ;
vbFormatNumberFx : VB_FORMATNUMBER '(' exprList ')' ;
vbFormatPercentFx: VB_FORMATPERCENT '(' exprList ')' ;
vbFormatDateTimeFx : VB_FORMATDATETIME '(' exprList ')' ;
vbInStrFx        : VB_INSTR '(' exprList ')' ;
vbInStrBFx       : VB_INSTRB '(' exprList ')' ;
vbInStrRevFx     : VB_INSTRREV '(' exprList ')' ;
vbLCaseFx        : VB_LCASE exprArg ;
vbLeftFx         : VB_LEFT '(' STRING ',' INT ')' ;
vbLeftBFx        : VB_LEFTB '(' STRING ',' INT ')' ;
vbLTrimFx        : VB_LTRIM exprArg ;
vbMidFx          : VB_MID '(' exprList ')' ;
vbMidBFx         : VB_MIDB '(' exprList ')' ;
vbReplaceFx      : VB_REPLACE '(' exprList ')' ;
vbRightFx        : VB_RIGHT '(' exprList ')' ;
vbRightBFx       : VB_RIGHTB '(' exprList ')' ;
vbRTrimFx        : VB_RTRIM exprArg ;
vbSpaceFx        : VB_SPACE exprArg ;
vbStrCompFx      : VB_STRCOMP '(' exprList ')' ;  
vbStringFx       : VB_STRING '(' exprList ')' ;
vbStringReverseFx: VB_STRREVERSE exprArg ;
vbTrimFx         : VB_TRIM exprArg ;
vbUCaseFx        : VB_UCASE exprArg ;
vbChrFx          : VB_CHR exprArg ;


vbCast : 'Asc' '(' STRING ')'
       | 'AscW' '(' STRING ')'
	   | 'CBool' exprArg
	   | 'CByte' exprArg
	   | 'CCur' exprArg
	   | 'CDate' exprArg
	   | 'CDbl' exprArg
	   | 'Chr' exprArg
	   | 'ChrW' exprArg
	   | 'CInt' exprArg
	   | 'CLng' exprArg
	   | 'CSng' exprArg
	   | 'CStr' exprArg
	   ;

vbDatePart : ('"yyyy"' | '"q"' | '"m"' | '"y"' | '"d"' | '"w"' | '"ww"' | '"h"' | '"n"' | '"s"' )   ;
vbDayOfWeek : ('vbUseSystem' | 'vbSunday' | 'vbMonday' | 'vbTuesday' | 'vbWednesday' | 'vbThursday' | 'vbFriday' | 'vbSaturday' ) ;
vbFirstWeek : ('vbUseSystem' | 'vbFirstJan1' | 'vbFirstFourDays' | 'vbFirstFullWeek' ) ;
exprArg : '(' expr ')' ;
	 
exprList : expr (',' expr)*               ;   // arg list

VB_ISNULL      :  [Ii]'s'[Nn]'ull'     ;
VB_ISEMPTY     :  [Ii]'s'[Ee]'mpty'    ;
VB_ISARRAY     :  [Ii]'s'[Aa]'rray'    ;
VB_ISOBJECT    :  [Ii]'s'[Oo]'bject'   ;
VB_ISNUMERIC   :  [Ii]'s'[Nn]'umeric'  ;
VB_LEN         :  [Ll]'en'             ;
VB_LENB        :  [Ll]'en'[Bb]         ;
VB_ERASE       :  [Ee]'rase'           ;
VB_ARRAY       :  [Aa]'rray'           ;
VB_FILTER      :  [Ff]'ilter'          ;
VB_JOIN        :  [Jj]'oin'            ;
VB_LBOUND      :  [Ll][Bb]'ound'       ;
VB_PRESERVE    :  [Pp]'reserve'        ;
VB_SPLIT       :  [Ss]'plit'           ;
VB_DATEADD     :  [Dd]'ate'[Aa]'dd'    ;
VB_DATEDIFF    :  [Dd]'ate'[Dd]'iff'   ;
VB_DATEPART    :  [Dd]'ate'[Pp]'art'   ;
VB_DATESERIAL  :  [Dd]'ate'[Ss]'erial' ;
VB_DATEVALUE   :  [Dd]'ate'[Vv]'alue'  ;
VB_DAY         :  [Dd]'ay'             ;
VB_HOUR        :  [Hh]'our'            ;
VB_MINUTE      :  [Mm]'inute'          ;
VB_MONTH       :  [Mm]'onth'           ;
VB_MONTHNAME   :  [Mm]'onth'[Nn]'ame'  ;
VB_NOW         :  [Nn]'ow'             ;
VB_SECOND      :  [Ss]'econd'          ;
VB_TIME        :  [Tt]'ime'            ;
VB_TIMER       :  [Tt]'imer'           ;
VB_TIMESERIAL  :  [Tt]'ime'[Ss]'erial' ;
VB_TIMEVALUE   :  [Tt]'ime'[Vv]'alue'  ;
VB_WEEKDAY     :  [Ww]'eekday'         ;
VB_WEEKDAYNAME :  [Ww]'eekdayName'     ;
VB_YEAR        :  [Yy]'ear'            ;
VB_ERR         :  [Ee]'rr'             ;
VB_ON          :  [Oo]'n'              ;
VB_ERRORRAISE  :  [Ee]'rror.'[Rr]'aise';
VB_ARRAYINIT   :  '(' DIGIT+ ')'       ;
VB_TYPENAME    :  [Tt]'ype'[Nn]'ame'   ;
VB_VARTYPE     :  [Vv]'ar'[Tt]'ype'    ;

//math
VB_ABS         :  [Aa]'bs'             ;
VB_COS         :  [Cc]'os'             ;
VB_EXP         :  [Ee]'xp'             ;
VB_FIX         :  [Ff]'ix'             ;
VB_INT         :  [Ii]'nt'             ;
VB_LOG         :  [Ll]'og'             ;
VB_RANDOMIZE   :  [Rr]'andomize'       ;
VB_RND         :  [Rr]'nd'             ;
VB_ROUND       :  [Rr]'ound'           ;
VB_SGN         :  [Ss]'gn'             ;
VB_SIN         :  [Ss]'in'             ;
VB_SQR         :  [Ss]'qr'             ;
VB_TAN         :  [Tt]'an'             ;

//misc
VB_EVAL            : [Ee]'val'                     ;
VB_LOADPICTURE     : [Ll]'oad'[Pp]'icture'         ;
VB_EXECUTE         : [Ee]'xecute'                  ;
VB_EXECUTE_GLOBAL  : [Ee]'xecute'[Gg]'lobal'       ;
VB_GET_OBJECT      : [Gg]'et'[Oo]'bject'           ;
VB_GET_REF         : [Gg]'et'[Rr]'ef'              ;
VB_SERVER          : [Ss]'ever'                    ;
VB_WSCRIPT         : [Ww][Ss]'cript'               ;
VB_CREATEOBJECT    : [Cc]'reate'[Oo]'bject'        ;

VB_FORMATCURRENCY  : [Ff]'ormat'[Cc]'urrency'      ;
VB_FORMATNUMBER    : [Ff]'ormat'[Nn]'umber'        ;
VB_FORMATPERCENT   : [Ff]'ormat'[Pp]'ercent'       ;
VB_FORMATDATETIME  : [Ff]'ormat'[Dd]'ate'[Tt]'ime' ;
VB_INSTR           : [Ii]'n'[Ss]'tr'               ;
VB_INSTRB          : [Ii]'n'[Ss]'tr'[Bb]           ;
VB_INSTRREV        : [Ii]'n'[Ss]'tr'[Rr]'ev'       ;
VB_LCASE           : [Ll][Cc]'ase'                 ;
VB_LEFT            : [Ll]'eft'                     ;
VB_LEFTB           : [Ll]'eft'[Bb]                 ;
VB_LTRIM           : [Ll][Tt]'rim'                 ;
VB_MID             : [Mm]'id'                      ;
VB_MIDB            : [Mm]'id'[Bb]                  ;
VB_REPLACE         : [Rr]'eplace'                  ;
VB_RIGHT           : [Rr]'ight'                    ;
VB_RIGHTB          : [Rr]'ight'[Bb]                ;
VB_RTRIM           : [Rr][Tt]'rim'                 ;
VB_SPACE           : [Ss]'pace'                    ;
VB_STRCOMP         : [Ss]'tr'[Cc]'omp'             ;
VB_STRING          : [Ss]'tring'                   ;
VB_STRREVERSE      : [Ss]'tr'[Rr]'everse'          ;
VB_TRIM            : [Tt]'rim'                     ;
VB_UCASE           : [Uu][Cc]'ase'                 ;
VB_CHR             : [Cc]'hr'                      ;


VB_CLASS        : [Cc][Ll][Aa][Ss][Ss]              ;
VB_FUNCTION     : [Ff][Uu][Nn][Cc][Tt][Ii][Oo][Nn]  ;
VB_SUB          : [Ss][Uu][Bb]                      ;
VB_PROPERTY     : [Pp][Rr][Oo][Pp][Ee][Rr][Tt][Yy]  ;
VB_DEFAULT      : [Dd][Ee][Ff][Aa][Uu][Ll][Tt]      ;

VB_END    :   [Ee]'nd'                 ;
VB_DO     :   [Dd]'o'                  ;
VB_WHILE  :   [Ww]'hile'               ;
VB_UNTIL  :   [Uu]'ntil'               ;
VB_LOOP   :   [Ll]'oop'                ;
VB_EXIT   :   [Ee]'xit'                ;
VB_FOR    :   [Ff]'or'                 ;
VB_EACH   :   [Ee]'ach'                ;
VB_IN     :   [Ii]'n'                  ;
VB_NEXT   :   [Nn]'ext'                ;
VB_IF     :   [Ii]'f'                  ;
VB_THEN   :   [Tt]'hen'                ;
VB_ELSE   :   [Ee]'lse'                ;
VB_ELSEIF :   [Ee]'lse'[Ii]'f'         ;
VB_ENDIF  :   [Ee]'nd '[Ii]'f'         ;
VB_PUBLIC :   [Pp]'ublic'              ;
VB_PRIVATE:   [Pp]'rivate'             ;
VB_CONST  :   [Cc]'onst'               ;
VB_WITH   :   [Ww]'ith'                ;
VB_STEP   :   [Ss]'tep'                ;
VB_SELECT :   [Ss]'elct'               ;
VB_CASE   :   [Cc]'ase'                ;
VB_WEND   :   [Ww]'end'                ;

VB_INCLUDE : [Ii]'nclude' ;
VB_FILE    : [Ff]'ile'    ;

VB_REDIM  :   [Rr][Ee][Dd][Ii][Mm]         ;
VB_DIM    :   [Dd][Ii][Mm]                 ;
VB_IS     :   [Ii][Ss]                     ;
VB_SET    :   [Ss][Ee][Tt]                 ;
VB_LET    :   [Ll][Ee][Tt]                 ;
VB_GET    :   [Gg][Ee][Tt]                 ;
VB_NOT    :   [Nn][Oo][Tt]                 ;
VB_AND    :   [Aa][Nn][Dd]                 ;
VB_OR     :   [Oo][Rr]                     ;
VB_NOTHING:   [Nn][Oo][Tt][Hh][Ii][Nn][Gg] ;
VB_MOD    :   [Mm][Oo][Dd]                 ;
VB_XOR    :   [Xx][Oo][Rr]                 ;
VB_EQV    :   [Ee][Qq][Vv]                 ;
VB_IMP    :   [Ii][Mm][Pp]                 ;
VB_TRUE   :   [Tt][Rr][Uu][Ee]             ;
VB_FALSE  :   [Ff][Aa][Ll][Ss][Ee]         ;
VB_TO     :   [Tt][Oo]                     ;

VB_DATE_LITERAL : '#' (DIGIT | '-')* '#'   ;

STRING :   '"' ('\\"'|.)*? '"' ;
INT    :   DIGIT+ ;
ID     :   '.' (LETTER|'_'|'.') (LETTER|DIGIT|'_'|'.')*
	   |   LETTER (LETTER|DIGIT|'_'|'.')*
	   ;
fragment LETTER  : [a-zA-Z] ;

DIGIT  :  '0'..'9' ;

LINE_COMMENT : '\'' .*? '\r'? '\n' -> type(NL) ;

NL      :   '\r'? '\n' ;
WS :   [ \t]+ -> skip ;
