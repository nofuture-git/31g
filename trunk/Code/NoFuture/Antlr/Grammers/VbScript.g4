grammar VbScript;

vbsBody   : ( stmt* | expr ) EOF;

stmt : vbsClass
     | vbsFunction
	 | vbsSub
	 | vbsGetProperty
	 | vbsSetProperty
	 | vbsWith
     | vbsDim 
	 | vbsReDim
     | vbsConst
	 | vbsExecute
     | vbsDoLoop 
	 | vbsForEachLoop
	 | vbsForLoop
	 | vbsWhile
	 | vbsSelect
	 | vbsExit
	 | vbsIfThen
	 | vbsErase
	 | expr NL
	 | NL
	 ;

vbsClass       : VBS_CLASS ID vbsBody VBS_END VBS_CLASS ;
vbsFunction    : (VBS_PUBLIC (VBS_DEFAULT)? | VBS_PRIVATE)? VBS_FUNCTION ID exprList vbsBodyWithExit VBS_END VBS_FUNCTION ;
vbsSub         : (VBS_PUBLIC (VBS_DEFAULT)? | VBS_PRIVATE)? VBS_SUB ID ( exprList )? vbsBodyWithExit VBS_END VBS_SUB ;
vbsGetProperty : (VBS_PUBLIC (VBS_DEFAULT)? | VBS_PRIVATE)? VBS_PROPERTY VBS_GET ID ( exprList )? vbsBodyWithExit VBS_END VBS_PROPERTY ;
vbsSetProperty : (VBS_PUBLIC | VBS_PRIVATE)? VBS_PROPERTY (VBS_SET | VBS_GET) ID exprList vbsBodyWithExit VBS_END VBS_PROPERTY ;
vbsDoLoop      : VBS_DO ( (VBS_WHILE | VBS_UNTIL) expr)? vbsBodyWithExit VBS_LOOP ( (VBS_WHILE | VBS_UNTIL) expr)? ;
vbsWhile       : VBS_WHILE expr vbsBody VBS_WEND  ;
vbsForEachLoop : VBS_FOR VBS_EACH ID VBS_IN expr vbsBodyWithExit VBS_NEXT (ID)? ;
vbsForLoop     : VBS_FOR ID '=' expr VBS_TO expr (VBS_STEP expr)? vbsBodyWithExit VBS_NEXT (expr)?  ;
vbsIfThen      : VBS_IF expr VBS_THEN vbsBodyWithExit (VBS_ELSEIF expr VBS_THEN vbsBodyWithExit )* (VBS_ELSE vbsBodyWithExit)? VBS_ENDIF   ;
vbsWith        : VBS_WITH ID vbsBody VBS_END VBS_WITH ;
vbsSelect      : VBS_SELECT vbsSelectBody VBS_END VBS_SELECT ;
vbsExit        : VBS_EXIT (VBS_DO | VBS_FOR | VBS_FUNCTION | VBS_PROPERTY | VBS_SUB) ;
vbsDim         : (VBS_PUBLIC | VBS_PRIVATE | VBS_DIM) ID VBS_ARRAYINIT? (',' NL ID VBS_ARRAYINIT? )* NL;
vbsErase       : VBS_ERASE ID NL ;
vbsReDim       : VBS_REDIM (VBS_PRESERVE)? ID ('(' INT ')')? (',' NL ID ('(' INT ')')?)* NL ;
vbsConst       : (VBS_PUBLIC | VBS_PRIVATE)? VBS_CONST ID '=' (STRING | INT) ('+' (STRING | INT))* NL ;
vbsExecute     : (VBS_EXECUTE | VBS_EXECUTE_GLOBAL) stmt NL;

vbsBodyWithExit : stmt (vbsExit | stmt)* ;
vbsSelectBody   : VBS_CASE expr ( VBS_CASE expr stmt* )* (VBS_CASE VBS_ELSE stmt*)?  ;
	 
expr : VBS_SET ID 
	 | ID
	 | ID '=' expr 
	 | ID ('(' exprList? ')' | exprList )
	 | vbsGlobalFx
	 | vbsCast
	 | exprArg
	 | expr VBS_IS expr
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
	 | expr VBS_MOD expr
	 | expr '+' expr
	 | expr '-' expr
	 | expr '&' expr
	 | VBS_NOT expr
	 | expr VBS_AND expr
	 | expr VBS_OR expr
	 | expr VBS_XOR expr
	 | expr VBS_EQV expr
	 | expr VBS_IMP expr
	 | VBS_NOTHING
	 | VBS_DATE_LITERAL
	 | INT
	 | STRING
	 | VBS_TRUE
	 | VBS_FALSE
	 | 'vbCr'
	 | 'vbCrLf'
	 | 'vbTab'
	 ;
	 
vbsGlobalFx   : vbsCtor 
             | vbsNowFx
			 | vbsTimeFx
			 | vbsLenFx
			 | vbsOnErr
			 | vbsErrorRaise
			 | vbsIsNullFx
			 | vbsIsEmptyFx
			 | vbsIsArrayFx
			 | vbsIsObjectFx
			 | vbsIsNumericFx
			 | vbsArrayInitFx
			 | vbsFilterArrayFx
			 | vbsJoinArrayFx
			 | vbsLBoundArrayFx 
			 | vbsSplitFx
			 | vbsDateAddFx
			 | vbsDateDiffFx
			 | vbsDatePartFx
			 | vbsDateSerialFx
			 | vbsDateValueFx
			 | vbsDayFx
			 | vbsHourFx
			 | vbsMinuteFx
			 | vbsMonthFx
			 | vbsMonthNameFx
			 | vbsSecondFx
			 | vbsTimerFx
			 | vbsTimeValueFx
			 | vbsWeekdayFx
			 | vbsWeekdayNameFx
			 | vbsYearFx
			 | vbsTypeNameFx
			 | vbsVarTypeFx 
			 | vbsAbsFx       
			 | vbsCosFx       
			 | vbsExpFx       
			 | vbsFixFx       
			 | vbsIntFx       
			 | vbsLogFx       
			 | vbsRandomizeFx 
			 | vbsRndFx       
			 | vbsRoundFx     
			 | vbsSgnFx       
			 | vbsSinFx       
			 | vbsSqrFx       
			 | vbsTanFx       
			 | vbsEvalFx        
			 | vbsLoadPictureFx
			 | vbsGetObjectFx
			 | vbsGetRefFx
			 | vbsFormatCurrencyFx
			 | vbsFormatNumberFx  
			 | vbsFormatPercentFx
			 | vbsFormatDateTimeFx
			 | vbsInStrFx
			 | vbsInStrBFx
			 | vbsInStrRevFx
			 | vbsLCaseFx
			 | vbsLeftFx
			 | vbsLeftBFx
			 | vbsLTrimFx
			 | vbsMidFx
			 | vbsMidBFx
			 | vbsReplaceFx
			 | vbsRightFx
			 | vbsRightBFx
			 | vbsRTrimFx
			 | vbsSpaceFx
			 | vbsStrCompFx
			 | vbsStringFx
			 | vbsStringReverseFx
			 | vbsTrimFx
			 | vbsUCaseFx
             | vbsChrFx
             ;	 
	 
vbsCtor           : (VBS_SERVER '.' | VBS_WSCRIPT '.')? VBS_CREATEOBJECT '(' STRING ')' NL ;
vbsNowFx          : VBS_NOW ('('')')?  ;
vbsTimeFx         : VBS_TIME ('('')')? ;
	 
vbsIsNullFx       : VBS_ISNULL exprArg  ;
vbsIsEmptyFx      : VBS_ISEMPTY exprArg ;	 
vbsIsArrayFx      : VBS_ISARRAY exprArg ;
vbsIsObjectFx     : VBS_ISOBJECT exprArg ;
vbsIsNumericFx    : VBS_ISNUMERIC exprArg ;
vbsLenFx          : VBS_LEN exprArg  ;
vbsLenBFx         : VBS_LENB exprArg  ;
vbsFilterArrayFx  : VBS_FILTER '(' ID ',' STRING (',' (VBS_TRUE | VBS_FALSE) (',' INT)? )? ')'  ;
vbsArrayInitFx    : VBS_ARRAY '(' expr (',' expr)* ')'   ;
vbsJoinArrayFx    : VBS_JOIN '(' ID (',' STRING)? ')'   ;
vbsLBoundArrayFx  : VBS_LBOUND '(' ID (',' INT)? ')'    ;
vbsSplitFx        : VBS_SPLIT '(' expr (',' STRING (',' INT (',' ('0' | '1' | 'vbBinaryCompare' | 'vbTextCompare'))? )? )? ')'  ;	
vbsDateAddFx      : VBS_DATEADD '(' vbsDatePart ',' expr ',' expr ')' ;
vbsDateDiffFx     : VBS_DATEDIFF '(' vbsDatePart ',' expr ',' expr (',' (vbsDayOfWeek | INT) (',' (vbsFirstWeek | INT) )? )? ')' ;
vbsDatePartFx     : VBS_DATEPART '(' vbsDatePart ',' expr (',' (vbsDayOfWeek | INT) (',' (vbsFirstWeek | INT) )? )? ')' ;
vbsDateSerialFx   : VBS_DATESERIAL '(' expr ',' expr ',' expr ')'  ;
vbsDateValueFx    : VBS_DATEVALUE exprArg  ;
vbsDayFx          : VBS_DAY exprArg ;
vbsHourFx         : VBS_HOUR exprArg ;
vbsMinuteFx       : VBS_MINUTE exprArg ;
vbsMonthFx        : VBS_MONTH  exprArg ;
vbsMonthNameFx    : VBS_MONTHNAME ('1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12') (',' (VBS_TRUE | VBS_FALSE) )?  ;
vbsSecondFx       : VBS_SECOND exprArg ;
vbsTimerFx        : VBS_TIMER '(' ')'  ;
vbsTimeValueFx    : VBS_TIMEVALUE exprArg ;
vbsWeekdayFx      : VBS_WEEKDAY '(' expr (',' (vbsDayOfWeek | INT) )?  ')' ;
vbsWeekdayNameFx  : VBS_WEEKDAYNAME '(' expr (',' (VBS_TRUE | VBS_FALSE) (',' (vbsDayOfWeek | INT))? )? ')' ;
vbsYearFx         : VBS_YEAR exprArg ;
vbsOnErr          : VBS_ON VBS_ERR ;
vbsErrorRaise     : VBS_ERRORRAISE exprList ;
vbsTypeNameFx     : VBS_TYPENAME exprArg ; 
vbsVarTypeFx      : VBS_VARTYPE exprArg ;
vbsAbsFx          : VBS_ABS exprArg ;
vbsCosFx          : VBS_COS exprArg ;
vbsExpFx          : VBS_EXP exprArg ;
vbsFixFx          : VBS_FIX exprArg ;
vbsIntFx          : VBS_INT exprArg ;
vbsLogFx          : VBS_LOG exprArg ;
vbsRandomizeFx    : VBS_RANDOMIZE exprArg ;
vbsRndFx          : VBS_RND exprArg;
vbsRoundFx        : VBS_ROUND '(' expr (',' INT)? ')' ;
vbsSgnFx          : VBS_SGN exprArg ;
vbsSinFx          : VBS_SIN exprArg ;
vbsSqrFx          : VBS_SQR exprArg ;
vbsTanFx          : VBS_TAN exprArg ;
vbsEvalFx         : VBS_EVAL exprArg ;
vbsLoadPictureFx  : VBS_LOADPICTURE '(' STRING  ')' ;
vbsGetObjectFx    : VBS_GET_OBJECT '(' STRING (',' ID) ')' ;
vbsGetRefFx       : VBS_GET_REF '(' STRING ')' ;
vbsFormatCurrencyFx: VBS_FORMATCURRENCY '(' exprList  ')' ;
vbsFormatNumberFx : VBS_FORMATNUMBER '(' exprList ')' ;
vbsFormatPercentFx: VBS_FORMATPERCENT '(' exprList ')' ;
vbsFormatDateTimeFx : VBS_FORMATDATETIME '(' exprList ')' ;
vbsInStrFx        : VBS_INSTR '(' exprList ')' ;
vbsInStrBFx       : VBS_INSTRB '(' exprList ')' ;
vbsInStrRevFx     : VBS_INSTRREV '(' exprList ')' ;
vbsLCaseFx        : VBS_LCASE exprArg ;
vbsLeftFx         : VBS_LEFT '(' STRING ',' INT ')' ;
vbsLeftBFx        : VBS_LEFTB '(' STRING ',' INT ')' ;
vbsLTrimFx        : VBS_LTRIM exprArg ;
vbsMidFx          : VBS_MID '(' exprList ')' ;
vbsMidBFx         : VBS_MIDB '(' exprList ')' ;
vbsReplaceFx      : VBS_REPLACE '(' exprList ')' ;
vbsRightFx        : VBS_RIGHT '(' exprList ')' ;
vbsRightBFx       : VBS_RIGHTB '(' exprList ')' ;
vbsRTrimFx        : VBS_RTRIM exprArg ;
vbsSpaceFx        : VBS_SPACE exprArg ;
vbsStrCompFx      : VBS_STRCOMP '(' exprList ')' ;  
vbsStringFx       : VBS_STRING '(' exprList ')' ;
vbsStringReverseFx: VBS_STRREVERSE exprArg ;
vbsTrimFx         : VBS_TRIM exprArg ;
vbsUCaseFx        : VBS_UCASE exprArg ;
vbsChrFx          : VBS_CHR exprArg ;


vbsCast : 'Asc' '(' STRING ')'
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

vbsDatePart : ('"yyyy"' | '"q"' | '"m"' | '"y"' | '"d"' | '"w"' | '"ww"' | '"h"' | '"n"' | '"s"' )   ;
vbsDayOfWeek : ('vbUseSystem' | 'vbSunday' | 'vbMonday' | 'vbTuesday' | 'vbWednesday' | 'vbThursday' | 'vbFriday' | 'vbSaturday' ) ;
vbsFirstWeek : ('vbUseSystem' | 'vbFirstJan1' | 'vbFirstFourDays' | 'vbFirstFullWeek' ) ;
exprArg : '(' expr ')' ;
	 
exprList : expr (',' expr)*               ;   // arg list

VBS_ISNULL      :  [Ii]'s'[Nn]'ull'     ;
VBS_ISEMPTY     :  [Ii]'s'[Ee]'mpty'    ;
VBS_ISARRAY     :  [Ii]'s'[Aa]'rray'    ;
VBS_ISOBJECT    :  [Ii]'s'[Oo]'bject'   ;
VBS_ISNUMERIC   :  [Ii]'s'[Nn]'umeric'  ;
VBS_LEN         :  [Ll]'en'             ;
VBS_LENB        :  [Ll]'en'[Bb]         ;
VBS_ERASE       :  [Ee]'rase'           ;
VBS_ARRAY       :  [Aa]'rray'           ;
VBS_FILTER      :  [Ff]'ilter'          ;
VBS_JOIN        :  [Jj]'oin'            ;
VBS_LBOUND      :  [Ll][Bb]'ound'       ;
VBS_PRESERVE    :  [Pp]'reserve'        ;
VBS_SPLIT       :  [Ss]'plit'           ;
VBS_DATEADD     :  [Dd]'ate'[Aa]'dd'    ;
VBS_DATEDIFF    :  [Dd]'ate'[Dd]'iff'   ;
VBS_DATEPART    :  [Dd]'ate'[Pp]'art'   ;
VBS_DATESERIAL  :  [Dd]'ate'[Ss]'erial' ;
VBS_DATEVALUE   :  [Dd]'ate'[Vv]'alue'  ;
VBS_DAY         :  [Dd]'ay'             ;
VBS_HOUR        :  [Hh]'our'            ;
VBS_MINUTE      :  [Mm]'inute'          ;
VBS_MONTH       :  [Mm]'onth'           ;
VBS_MONTHNAME   :  [Mm]'onth'[Nn]'ame'  ;
VBS_NOW         :  [Nn]'ow'             ;
VBS_SECOND      :  [Ss]'econd'          ;
VBS_TIME        :  [Tt]'ime'            ;
VBS_TIMER       :  [Tt]'imer'           ;
VBS_TIMESERIAL  :  [Tt]'ime'[Ss]'erial' ;
VBS_TIMEVALUE   :  [Tt]'ime'[Vv]'alue'  ;
VBS_WEEKDAY     :  [Ww]'eekday'         ;
VBS_WEEKDAYNAME :  [Ww]'eekdayName'     ;
VBS_YEAR        :  [Yy]'ear'            ;
VBS_ERR         :  [Ee]'rr'             ;
VBS_ON          :  [Oo]'n'              ;
VBS_ERRORRAISE  :  [Ee]'rror.'[Rr]'aise';
VBS_ARRAYINIT   :  '(' DIGIT+ ')'       ;
VBS_TYPENAME    :  [Tt]'ype'[Nn]'ame'   ;
VBS_VARTYPE     :  [Vv]'ar'[Tt]'ype'    ;

//math
VBS_ABS         :  [Aa]'bs'             ;
VBS_COS         :  [Cc]'os'             ;
VBS_EXP         :  [Ee]'xp'             ;
VBS_FIX         :  [Ff]'ix'             ;
VBS_INT         :  [Ii]'nt'             ;
VBS_LOG         :  [Ll]'og'             ;
VBS_RANDOMIZE   :  [Rr]'andomize'       ;
VBS_RND         :  [Rr]'nd'             ;
VBS_ROUND       :  [Rr]'ound'           ;
VBS_SGN         :  [Ss]'gn'             ;
VBS_SIN         :  [Ss]'in'             ;
VBS_SQR         :  [Ss]'qr'             ;
VBS_TAN         :  [Tt]'an'             ;

//misc
VBS_EVAL            : [Ee]'val'                     ;
VBS_LOADPICTURE     : [Ll]'oad'[Pp]'icture'         ;
VBS_EXECUTE         : [Ee]'xecute'                  ;
VBS_EXECUTE_GLOBAL  : [Ee]'xecute'[Gg]'lobal'       ;
VBS_GET_OBJECT      : [Gg]'et'[Oo]'bject'           ;
VBS_GET_REF         : [Gg]'et'[Rr]'ef'              ;
VBS_SERVER          : [Ss]'ever'                    ;
VBS_WSCRIPT         : [Ww][Ss]'cript'               ;
VBS_CREATEOBJECT    : [Cc]'reate'[Oo]'bject'        ;

VBS_FORMATCURRENCY  : [Ff]'ormat'[Cc]'urrency'      ;
VBS_FORMATNUMBER    : [Ff]'ormat'[Nn]'umber'        ;
VBS_FORMATPERCENT   : [Ff]'ormat'[Pp]'ercent'       ;
VBS_FORMATDATETIME  : [Ff]'ormat'[Dd]'ate'[Tt]'ime' ;
VBS_INSTR           : [Ii]'n'[Ss]'tr'               ;
VBS_INSTRB          : [Ii]'n'[Ss]'tr'[Bb]           ;
VBS_INSTRREV        : [Ii]'n'[Ss]'tr'[Rr]'ev'       ;
VBS_LCASE           : [Ll][Cc]'ase'                 ;
VBS_LEFT            : [Ll]'eft'                     ;
VBS_LEFTB           : [Ll]'eft'[Bb]                 ;
VBS_LTRIM           : [Ll][Tt]'rim'                 ;
VBS_MID             : [Mm]'id'                      ;
VBS_MIDB            : [Mm]'id'[Bb]                  ;
VBS_REPLACE         : [Rr]'eplace'                  ;
VBS_RIGHT           : [Rr]'ight'                    ;
VBS_RIGHTB          : [Rr]'ight'[Bb]                ;
VBS_RTRIM           : [Rr][Tt]'rim'                 ;
VBS_SPACE           : [Ss]'pace'                    ;
VBS_STRCOMP         : [Ss]'tr'[Cc]'omp'             ;
VBS_STRING          : [Ss]'tring'                   ;
VBS_STRREVERSE      : [Ss]'tr'[Rr]'everse'          ;
VBS_TRIM            : [Tt]'rim'                     ;
VBS_UCASE           : [Uu][Cc]'ase'                 ;
VBS_CHR             : [Cc]'hr'                      ;


VBS_CLASS        : [Cc][Ll][Aa][Ss][Ss]              ;
VBS_FUNCTION     : [Ff][Uu][Nn][Cc][Tt][Ii][Oo][Nn]  ;
VBS_SUB          : [Ss][Uu][Bb]                      ;
VBS_PROPERTY     : [Pp][Rr][Oo][Pp][Ee][Rr][Tt][Yy]  ;
VBS_DEFAULT      : [Dd][Ee][Ff][Aa][Uu][Ll][Tt]      ;

VBS_END    :   [Ee]'nd'                 ;
VBS_DO     :   [Dd]'o'                  ;
VBS_WHILE  :   [Ww]'hile'               ;
VBS_UNTIL  :   [Uu]'ntil'               ;
VBS_LOOP   :   [Ll]'oop'                ;
VBS_EXIT   :   [Ee]'xit'                ;
VBS_FOR    :   [Ff]'or'                 ;
VBS_EACH   :   [Ee]'ach'                ;
VBS_IN     :   [Ii]'n'                  ;
VBS_NEXT   :   [Nn]'ext'                ;
VBS_IF     :   [Ii]'f'                  ;
VBS_THEN   :   [Tt]'hen'                ;
VBS_ELSE   :   [Ee]'lse'                ;
VBS_ELSEIF :   [Ee]'lse'[Ii]'f'         ;
VBS_ENDIF  :   [Ee]'nd '[Ii]'f'         ;
VBS_PUBLIC :   [Pp]'ublic'              ;
VBS_PRIVATE:   [Pp]'rivate'             ;
VBS_CONST  :   [Cc]'onst'               ;
VBS_WITH   :   [Ww]'ith'                ;
VBS_STEP   :   [Ss]'tep'                ;
VBS_SELECT :   [Ss]'elct'               ;
VBS_CASE   :   [Cc]'ase'                ;
VBS_WEND   :   [Ww]'end'                ;

VBS_INCLUDE : [Ii]'nclude' ;
VBS_FILE    : [Ff]'ile'    ;

VBS_REDIM  :   [Rr][Ee][Dd][Ii][Mm]         ;
VBS_DIM    :   [Dd][Ii][Mm]                 ;
VBS_IS     :   [Ii][Ss]                     ;
VBS_SET    :   [Ss][Ee][Tt]                 ;
VBS_LET    :   [Ll][Ee][Tt]                 ;
VBS_GET    :   [Gg][Ee][Tt]                 ;
VBS_NOT    :   [Nn][Oo][Tt]                 ;
VBS_AND    :   [Aa][Nn][Dd]                 ;
VBS_OR     :   [Oo][Rr]                     ;
VBS_NOTHING:   [Nn][Oo][Tt][Hh][Ii][Nn][Gg] ;
VBS_MOD    :   [Mm][Oo][Dd]                 ;
VBS_XOR    :   [Xx][Oo][Rr]                 ;
VBS_EQV    :   [Ee][Qq][Vv]                 ;
VBS_IMP    :   [Ii][Mm][Pp]                 ;
VBS_TRUE   :   [Tt][Rr][Uu][Ee]             ;
VBS_FALSE  :   [Ff][Aa][Ll][Ss][Ee]         ;
VBS_TO     :   [Tt][Oo]                     ;

VBS_DATE_LITERAL : '#' (DIGIT | '-')* '#'   ;

STRING :   '"' ('\\"'|.)*? '"' ;
INT    :  DIGIT+  ;
ID     :   '.' (LETTER|'_'|'.') (LETTER|DIGIT|'_'|'.')*
	   |   LETTER (LETTER|DIGIT|'_'|'.')*
	   ;
fragment LETTER  : [a-zA-Z] ;

fragment DIGIT  :  [0-9] ;

LINE_COMMENT : '\'' .*? '\r'? '\n' -> type(NL) ;

NL      :   '\r'? '\n' ;
WS :   [ \t]+ -> skip ;
