inline int myDebugBreak( int ){
    DebugBreak();
    return 0;
}
#define MAXELEMS(x)     (sizeof(x)/sizeof(x[0]))
#define SafeDRef(a, i)  ((i < MAXELEMS(a)) ? a[i] : a[myDebugBreak(i)])

#define MAX_TYPE_IN_DETAIL 5
#define MAX_RVA_LINES_BYTES_RANGE 0x100

extern const wchar_t * const rgBaseType[];
extern const wchar_t * const rgTags[];
extern const wchar_t * const rgFloatPackageStrings[];
extern const wchar_t * const rgProcessorStrings[];
extern const wchar_t * const rgDataKind[];
extern const wchar_t * const rgUdtKind[];
extern const wchar_t * const rgAccess[];
extern const wchar_t * const rgCallingConvention[];
extern const wchar_t * const rgLanguage[];
extern const wchar_t * const rgLocationTypeString[];

void PrintPublicSymbol( IDiaSymbol* );
void PrintGlobalSymbol( IDiaSymbol* );
void PrintSymbol( IDiaSymbol*, DWORD, bool );
void PrintSymTag( DWORD );
void PrintName( IDiaSymbol* );
void PrintUndName( IDiaSymbol* );
void PrintThunk( IDiaSymbol* );
void PrintCompilandDetails( IDiaSymbol*, DWORD );
void PrintCompilandEnv( IDiaSymbol*, DWORD );
void PrintLocation( IDiaSymbol*, DWORD );
void PrintConst( IDiaSymbol*, DWORD );
void PrintUDT( IDiaSymbol*, DWORD );
void PrintSymbolType( IDiaSymbol*, DWORD );
void PrintType( IDiaSymbol*, DWORD );
void PrintBound( IDiaSymbol* );
void PrintData( IDiaSymbol* , DWORD );
void PrintVariant( VARIANT );
void PrintUdtKind( IDiaSymbol* );
void PrintTypeInDetail( IDiaSymbol* , DWORD );
void PrintFunctionType( IDiaSymbol*, DWORD );
void PrintSourceFile( IDiaSourceFile* );
void PrintLines( IDiaSession* , IDiaSymbol* );
void PrintLines( IDiaEnumLineNumbers*, ULONGLONG, DWORD );
void PrintSource( IDiaSourceFile* );
void PrintSecContribs( IDiaSectionContrib* );
void PrintStreamData( IDiaEnumDebugStreamData* );
void PrintFrameData( IDiaFrameData* );

void PrintPropertyStorage( IDiaPropertyStorage* );

template<class T> void PrintGeneric( T t ){
  IDiaPropertyStorage* pPropertyStorage;
  
  if(t->QueryInterface( __uuidof(IDiaPropertyStorage), (void **)&pPropertyStorage ) == S_OK){
    PrintPropertyStorage(pPropertyStorage);
    pPropertyStorage->Release();
  }
}
