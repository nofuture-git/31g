using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using TestAnythingHere;
using Win32;

/*
.NET and COM: The Complete Interoperability Guide
By: Adam Nathan
Publisher: Sams
Pub. Date: January 31, 2002
Print ISBN-10: 0-672-32170-X
Print ISBN-13: 978-0-672-32170-2
Web ISBN-10: 0-672-33358-9
Web ISBN-13: 978-0-672-33358-3
*/

/*
 Misc .NET related
 
  System.Diagnostics.Process, Modules, BaseAddress, Threads
  System.Reflection.Module.ModuleHandle
  System.Runtime.TypeHandle
  System.Runtime.InteropServices.RuntimeEnvironment
 */

/*
 use dumpbin.exe to get the exposed WIN32 functions 
 . "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\bin\amd64\dumpbin.exe" /exports C:\Windows\System32\advapi32.dll
 */

namespace TestAnythingHere
{
    public class Notes_WIN32_Marshal
    {
        //BOOL QueryPerformanceCounter(LARGE_INTEGER *lpPerformanceCount)
        [DllImport("kernal32.dll"), SuppressUnmanagedCodeSecurity]
        public static extern int QueryPerformanceCounter(out long lpPerformance);

        //BOOL QueryPerformanceFrequency(LARGE_INTEGER *lpFrequency)
        [DllImport("kernal32.dll"), SuppressUnmanagedCodeSecurity]
        public static extern int QueryPerformanceFrequency(out long lpFrequency);

        /*Customizing Dll import functions*/

        //give the function a different name than its entrypoint name
        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter")]
        public static extern bool GetTimerValue(out long lpPerformanceCount);

        //use the ordinal instead of the name
        [DllImport("kernel32.dll", EntryPoint = "#556")]
        public static extern bool OrdinalGetTimerValue(out long lpPerformanceCount);

        //win32 functions taking strings differentiate using ANSI or Unicode with 'A' and 'W' respectively
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        //having 'Exact' means the 'A' must be added
        public static extern bool SetVolumeLabelA(string lpRootPathName, string lpVolumeName);

        //these function and are of some practical use
        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "OpenThread", CharSet = CharSet.Auto)]
        public static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "OpenProcess", CharSet = CharSet.Auto)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        //direct calls to WIN32's get last error through p/invoke is not to be used, .NET provides another means
        private int lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();

        private const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
        private const ushort LANG_NEUTRAL = 0x0;
        private const ushort SUBLANG_DEFAULT = 0x1;

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern uint FormatMessage(uint dwFlags,
                                                IntPtr lpSource,
                                                int dwMessageId,
                                                int dwLanguageId,
                                                [Out] StringBuilder lpBuffer,
                                                int nSize,
                                                IntPtr Arguments);

        public static int MakeLangId(ushort p, ushort s)
        {
            return (s << 10) | p;
        }

        public static string GetErrorMessage(int errorCode)
        {
            var sb = new StringBuilder(1024);
            if (
                FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM,
                              IntPtr.Zero,
                              errorCode,
                              MakeLangId(LANG_NEUTRAL, SUBLANG_DEFAULT),
                              sb,
                              sb.Capacity,
                              IntPtr.Zero) != 0)
            {
                return sb.ToString();
            }
            else
            {
                return "<Unrecognized error>";
            }
        }

        private string lastErrorMessage = GetErrorMessage(System.Runtime.InteropServices.Marshal.GetLastWin32Error());

        //it is possible to have WIN32 error turned into a thrown exceptions using the PreserveSig member of DllImport
        [DllImport("ole32.dll", CharSet = CharSet.Unicode, EntryPoint = "ProgIDFromCLSID", PreserveSig = false)]
        public static extern void ProgIDFromCLSID([In] ref Guid clsid, out string lplpszProgID);

        public static void ExceptionHandlingAndWIN32Errors()
        {
            string progId;
            Guid newGuid = Guid.NewGuid();
            try
            {
                ProgIDFromCLSID(ref newGuid, out progId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /*
         WIN32 types to .NET DataTypes
         BOOL, BOOLEAN ~ System.Int32, System.Boolean
         BYTE ~ System.Byte
         COLORREF ~ System.UInt32, System.Int32
         DOUBLE ~ System.Double
         DWORD ~ System.UInt32, System.Int32
         DWORD_PTR ~ System.UIntPtr, System.IntPtr
         DWORD32 ~ System.UInt32, System.Int32
         DWORD64 ~ System.UInt64, System.Int64
         FLOAT ~ System.Single
         HRESULT ~ System.UInt32, System.Int32
         INT ~ System.Int32
         INT64 ~ System.Int64
         LANGID ~ System.UInt16, System.Int16
         LARGE_INTEGER ~ System.Int64
         LCID ~ System.UInt32, System.Int32
         LCTYPE ~ System.UInt32, System.Int32
         LONG ~ System.Int32
         LONG_PTR ~ System.IntPtr
         LONG32 ~ System.Int32
         LONG64 ~ System.Int64
         LONGLONG ~ System.Int64
         LPARAM ~ System.UIntPtr, System.IntPtr, System.Object
         LPCTSTR ~ System.String
         LPDWORD ~ System.IntPtr
         LPTSTR ~ System.String
         LPVOID(void*) ~ System.UIntPtr, System.IntPtr, System.Object
         LPCVOID ~ System.IntPtr
         LRESULT ~ System.IntPtr
         PDWORD ~ System.Int32
         SHORT ~ System.Int16
         SIZE_T ~ System.UIntPtr, System.IntPtr
         SSIZE_T ~ System.IntPtr
         TBYTE,TCHAR ~ System.Char
         UCHAR ~ System.SByte
         UINT ~ System.UInt32, System.Int32
         UINT_PTR ~ System.UIntPtr, System.IntPtr
         UINT32 ~ System.UInt32, System.Int32
         UINT64 ~ System.UInt64, System.Int64
         ULARGE_INTEGER ~ System.Int64
         ULONG ~ System.UInt32, System.Int32
         ULONG32 ~ System.UInt32, System.Int32
         ULONG64 ~ System.UInt64, System.Int64
         ULONGLONG ~ System.UInt64, System.Int64
         USHORT ~ System.UInt16, System.Int16
         WORD ~ System.UInt16, System.Int16
         WPARAM ~ System.UIntPtr, System.IntPtr, System.Object
         
         BSTR ~ System.String (UnmanagedType.BStr)
         CHAR ~ System.Char (UnmanagedType.U1)
         LPCSTR ~ System.String (UnmanagedType.LPStr)
         LPCWSTR ~ System.String (UnmanagedType.LPWStr)
         LPWSTR ~ System.String (UnmanagedType.LPWStr)
         SAFEARRAY ~ System.Array (UnmanagedType.SafeArray)
         VARIANT ~ System.Object (UnmanagedType.Struct)
         VARIANT_BOOL ~ System.Boolean (UnmanagedType.VariantBool)
         WCHAR ~ System.Char (UnmanagedType.U2)
         
         HANDLE, HBITMAP, HBRUSH, HCURSOR, HDC,  ~ System.UIntPtr, System.IntPtr, System.Runtime.InteropServices.HandleRef
         HFONT, HGLOBAL, HICON, HINSTANCE, HKEY, 
         HMENU, HWND,... 
          
         WINAPI appears to be the calling convention which is the same value by default
         for example
         HANDLE WINAPI OpenThread(...) 
          translates into 
         public static extern bool OpenThread(...)
         */

        //MarshalAs Examples
        /*
         may be marked on parameters, return types and fields
         */
        private Decimal _decimal;

        public Decimal MyMarshalDec
        {
            [return: MarshalAs(UnmanagedType.Currency)]
            get { return _decimal; }
            [param: MarshalAs(UnmanagedType.Currency)]
            set { _decimal = value; }
        }

        [return: MarshalAs(UnmanagedType.AnsiBStr)]
        public string GetAnsiBstr()
        {
            return "";
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        public bool GetWin32Bool()
        {
            //false is zero, all else is true
            return false;
        }

        [return: MarshalAs(UnmanagedType.BStr)]
        public string GetBstr()
        {
            return "";
        }

        [return: MarshalAs(UnmanagedType.Interface)]
        public IList GetSomething([In, MarshalAs(UnmanagedType.BStr)] string aStringVar)
        {
            var myInterfaceType = new System.Collections.ArrayList();
            return (IList) myInterfaceType;
        }

        [return: MarshalAs(UnmanagedType.Error)]
        public System.Int32 MyHRESULT()
        {
            return 0;
        }

        [return: MarshalAs(UnmanagedType.IDispatch)]
        public object IDispatchIsAnObjectInVb6()
        {
            return new object();
        }

        public delegate void MyFunctionPointer();

        public void GetAPointer([In, MarshalAs(UnmanagedType.FunctionPtr)] MyFunctionPointer arg)
        {
            return;
        }

        public delegate uint CopyProgressDelegate(
            long TotalFileSize,
            long TotalBytesTransferred,
            long StreamSize,
            long StreamBytesTransfered,
            uint dwStreamNumber,
            uint dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CopyFileEx(string lpExistingFileName,
                                             string lpNewFileName,
                                             CopyProgressDelegate lpProgressRoutine,
                                             IntPtr lpData,
                                             [In] ref bool pbCancel,
                                             uint dwCopyFlags);

        //make this static to prevent GC' on it prior to the unmanaged code being done
        //expect the classic 'Access Violation' error
        public static CopyProgressDelegate del;

        [return: MarshalAs(UnmanagedType.IUnknown)]
        public object GetAnUnknown()
        {
            return new object();
        }

        //this tells the marshaller to get five total elements
        [return: MarshalAs(UnmanagedType.LPArray, SizeConst = 5)]
        public char[] GetPointerToFirstElementInArray()
        {
            return new char[] {'h', 'e', 'l', 'l', 'o'};
        }

        //this tells the marshaller to get up to the fourth index
        [return: MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
        public char[] GetPointerByLastIndexVal()
        {
            return new char[] {'h', 'e', 'l', 'l', 'o'};
        }

        [return: MarshalAs(UnmanagedType.LPStr)]
        public string GetNullTermAnsiString()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("hello");
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        [return: MarshalAs(UnmanagedType.LPWStr)]
        public string GetNullTermUnicodeString()
        {
            var bytes = System.Text.Encoding.Unicode.GetBytes("hello");
            return System.Text.Encoding.Unicode.GetString(bytes);
        }

        //not sure this works on generics, the SafeArraySubType is to specify the type therein and its a different enum
        [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
        public System.Array GetSafeArray()
        {
            return new[] {"string", "string", "string"};
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MyPerson
        {
            public String first;
            public String last;
        }

        //int and BOOL in Win32 are the same size in bytes
        /*
         * integers are 'blittable' id est, types whose underlying representation is the same 
         * in both managed and unmanaged form.
         * 
         * Blittable types
         * Byte
         * SByte
         * Int16
         * Int32
         * UInt32
         * Int64
         * UInt64
         * IntPtr
         * UIntPtr
         * Single
         * Double
        */

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        //with the use of delegates function pointers may be passed to unmanged Win32 functions
        //the reverse, however, is not true - this is a viable workaround
        public static void AdvancedFunctionPointerExample()
        {
            IntPtr hModule = LoadLibrary("user32.dll");
            IntPtr functionPointer = GetProcAddress(hModule, "CountClipboardFormats");

            var returnVal = (int) InvokeUnmanagedFunctionPointerWithNoParams(functionPointer, typeof (int));
        }

        /// <summary>
        /// Dynamic assembly which invokes a function pointer which was returned from 
        /// an unmanaged, marshaled function call.
        /// </summary>
        /// <param name="functionPointer"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static object InvokeUnmanagedFunctionPointerWithNoParams(IntPtr functionPointer, Type returnType)
        {
            const string DYNAMIC_METHOD = "MyDynamicMethod";

            //create dynamic assembly and dynamic module
            var asmName = new AssemblyName();
            asmName.Name = "tempAssembly";
            var dynamicAsm = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            var dynamicMod = dynamicAsm.DefineDynamicModule("tempModule");

            //create a global method capable of invoking the function pointer
            var dynamicMethod = dynamicMod.DefineGlobalMethod(DYNAMIC_METHOD,
                                                              MethodAttributes.Public | MethodAttributes.Static,
                                                              returnType,
                                                              null);
            var generator = dynamicMethod.GetILGenerator();

            if (IntPtr.Size == 4)
            {
                generator.Emit(OpCodes.Ldc_I4, functionPointer.ToInt32());
            }
            else if (IntPtr.Size == 8)
            {
                generator.Emit(OpCodes.Ldc_I8, functionPointer.ToInt64());
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            //if the method has parameters then they would need to be pushed onto the stack first
            generator.EmitCalli(OpCodes.Calli, CallingConvention.StdCall, returnType, new Type[] {});
            generator.Emit(OpCodes.Ret);

            dynamicMod.CreateGlobalFunctions();

            var mi = dynamicMod.GetMethod(DYNAMIC_METHOD);

            return mi.Invoke(null, null);
        }
    }

    //BOOL GetConsoleScreenBufferInfo(_In_ HANDLE hConsoleOutput, _Out_ PCONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo)
    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms683171(v=vs.85).aspx
	
	//PCONSOLE_SCREEN_BUFFER_INFO is a pointer to the struct CONSOLE_SCREEN_BUFFER_INFO
    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms682093(v=vs.85).aspx

    //CONSOLE_SCREEN_BUFFER_INFO has a another two structs itself, SMALL_RECT and COORD
    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms686311(v=vs.85).aspx
    //http://msdn.microsoft.com/en-us/library/windows/desktop/ms682119(v=vs.85).aspx

    //all marshaled structs not marked with a layout attribute are set to LayoutKind.Sequential


	internal struct SMALL_RECT
	{
		internal short Left;
		internal short Top;
		internal short Right;
		internal short Bottom;
	}
	
	internal struct COORD
	{
		internal short X;
		internal short Y;
	}
	internal struct CONSOLE_SCREEN_BUFFER_INFO
	{
		internal COORD dwSize;
		internal COORD dwCursorPosition;
		internal ushort wAttributes;
		internal SMALL_RECT srWindow;
		internal COORD dwMaximumWindowSize;
	}
    internal class Constants
    {
        internal const ushort FOREGROUND_BLUE = 0x01;
        internal const ushort FOREGROUND_GREEN = 0x02;
        internal const ushort FOREGROUND_RED = 0x04;
        internal const ushort FOREGROUND_INTENSITY = 0x08;

        internal const ushort BACKGROUND_BLUE = 0x10;
        internal const ushort BACKGROUND_GREEN = 0x20;
        internal const ushort BACKGROUND_RED = 0x40;
        internal const ushort BACKGROUND_INTENSITY = 0x80;

        internal const int STD_INPUT_HANDLE = -10;
        internal const int STD_OUTPUT_HANDLE = -11;
        internal const int STD_ERROR_HANDLE = -12;

        internal static readonly System.IntPtr INVALID_HANDLE_VALUE = new System.IntPtr(-1);
    }

    public class ColorfulConsole
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool GetConsoleScreenBufferInfo(System.IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool SetConsoleTextAttribute(System.IntPtr hConsoleOutput, ushort wAttributes);

        private static CONSOLE_SCREEN_BUFFER_INFO csbi = new CONSOLE_SCREEN_BUFFER_INFO();

        public static void WriteLine(string text, ForegroundColors foreColor, BackgroundColors backColor)
        {
            var stdout = GetStdHandle(Constants.STD_OUTPUT_HANDLE);

            if (stdout == Constants.INVALID_HANDLE_VALUE)
            {
                throw new System.Exception("Unable to get standard output handle");
            }

            if (!GetConsoleScreenBufferInfo(stdout, out csbi))
            {
                throw new System.Exception("Unable to get existing console settings");
            }

            if (!SetConsoleTextAttribute(stdout, (ushort) ((ushort) foreColor | (ushort) backColor)))
            {
                throw new System.Exception("Unable to set console colors");
            }

            System.Console.WriteLine(text);

            if (!SetConsoleTextAttribute(stdout, csbi.wAttributes))
            {
                throw new System.Exception("Unable to restore normal console colors");
            }
        }

        public static void WriteLine(string text, ForegroundColors foreColor)
        {
            System.IntPtr stdout = GetStdHandle(Constants.STD_OUTPUT_HANDLE);

            if (stdout == Constants.INVALID_HANDLE_VALUE)
            {
                throw new System.Exception("Unable to get standard output handle.");
            }

            if (!GetConsoleScreenBufferInfo(stdout, out csbi))
            {
                throw new System.Exception("Unable to get existing console settings");
            }

            BackgroundColors defaultBackground = (BackgroundColors) (csbi.wAttributes & 0xF0);

            System.Console.WriteLine(text, foreColor, defaultBackground);
        }
    }

    // Enumeration of foreground colors
    public enum ForegroundColors : ushort
    {
        Black = 0,
        Blue = Constants.FOREGROUND_BLUE,
        Green = Constants.FOREGROUND_GREEN,
        Cyan = Constants.FOREGROUND_BLUE | Constants.FOREGROUND_GREEN,
        Red = Constants.FOREGROUND_RED,
        Magenta = Constants.FOREGROUND_RED | Constants.FOREGROUND_BLUE,
        Brown = Constants.FOREGROUND_RED | Constants.FOREGROUND_GREEN,

        Gray = Constants.FOREGROUND_RED | Constants.FOREGROUND_BLUE |
               Constants.FOREGROUND_GREEN,
        DarkGray = Constants.FOREGROUND_INTENSITY,
        LightBlue = Constants.FOREGROUND_BLUE | Constants.FOREGROUND_INTENSITY,

        LightGreen = Constants.FOREGROUND_GREEN |
                     Constants.FOREGROUND_INTENSITY,

        LightCyan = Constants.FOREGROUND_BLUE | Constants.FOREGROUND_GREEN |
                    Constants.FOREGROUND_INTENSITY,
        LightRed = Constants.FOREGROUND_RED | Constants.FOREGROUND_INTENSITY,

        LightMagenta = Constants.FOREGROUND_RED | Constants.FOREGROUND_BLUE |
                       Constants.FOREGROUND_INTENSITY,

        Yellow = Constants.FOREGROUND_RED | Constants.FOREGROUND_GREEN |
                 Constants.FOREGROUND_INTENSITY,

        White = Constants.FOREGROUND_RED | Constants.FOREGROUND_BLUE |
                Constants.FOREGROUND_GREEN | Constants.FOREGROUND_INTENSITY
    }

    // Enumeration of background colors
    public enum BackgroundColors : ushort
    {
        Black = 0,
        Blue = Constants.BACKGROUND_BLUE,
        Green = Constants.BACKGROUND_GREEN,
        Cyan = Constants.BACKGROUND_BLUE | Constants.BACKGROUND_GREEN,
        Red = Constants.BACKGROUND_RED,
        Magenta = Constants.BACKGROUND_RED | Constants.BACKGROUND_BLUE,
        Brown = Constants.BACKGROUND_RED | Constants.BACKGROUND_GREEN,

        Gray = Constants.BACKGROUND_RED | Constants.BACKGROUND_BLUE |
               Constants.BACKGROUND_GREEN,
        DarkGray = Constants.BACKGROUND_INTENSITY,
        LightBlue = Constants.BACKGROUND_BLUE | Constants.BACKGROUND_INTENSITY,

        LightGreen = Constants.BACKGROUND_GREEN |
                     Constants.BACKGROUND_INTENSITY,

        LightCyan = Constants.BACKGROUND_BLUE | Constants.BACKGROUND_GREEN |
                    Constants.BACKGROUND_INTENSITY,
        LightRed = Constants.BACKGROUND_RED | Constants.BACKGROUND_INTENSITY,

        LightMagenta = Constants.BACKGROUND_RED | Constants.BACKGROUND_BLUE |
                       Constants.BACKGROUND_INTENSITY,

        Yellow = Constants.BACKGROUND_RED | Constants.BACKGROUND_GREEN |
                 Constants.BACKGROUND_INTENSITY,

        White = Constants.BACKGROUND_RED | Constants.BACKGROUND_BLUE |
                Constants.BACKGROUND_GREEN | Constants.BACKGROUND_INTENSITY
    }

    public class CustomizedStructLayout
    {
        //packing and packing boundary
        // idea of a marker in serial layout of bytes

        //which Pack set to 8 the double 'two' would begin at index 8
        //even though the int 'one' fits into half that size
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        internal struct PackingExample
        {
            internal int one;
            internal double two;
            internal int three;
        }

        //this only impacts the unmanaged representation, managed side still does it its way

        //also possible to define a strict memory layout as such
        [StructLayout(LayoutKind.Explicit)]
        internal struct MyStrictLayout
        {
            //here is a hole of size 1 byte

            [FieldOffset(1)] //begins as index 1 
                internal int One;

            //here is a hole of size 2 bytes

            [FieldOffset(7)] //begins at index 7
                internal double Two;

            //here is a hole of size 3 bytes

            [FieldOffset(18)] //begins at index 15
                internal int Three;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct MarshalPassingInStructs
        {
            //this is the only way to pass an array as a field of a struct
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            internal int[] MyArray;

            [MarshalAs(UnmanagedType.Interface)]
            internal object MyObject;

            //use this for getting strings 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
            internal string ForString;

            //use this for char arrays
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            internal char[] ForCharArray;
        }

        /*
        typedef struct _OSVERSIONINFO{ 
            DWORD dwOSVersionInfoSize;
            DWORD dwMajorVersion;
            DWORD dwMinorVersion;
            DWORD dwBuildNumber;
            DWORD dwPlatformId;
            TCHAR szCSDVersion[128];
        } OSVERSIONINFO; 
        http://msdn.microsoft.com/en-us/library/windows/desktop/ms724834(v=vs.85).aspx
         
         */

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct OSVERSIONINFO_1
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct OSVERSIONINFO_2
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] szCSDVersion;
        }

        //decorating the class with the struct layout means it will get marshaled as a struct irrespective
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class clsOSVERSIONINFO_1
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetVersionEx(ref OSVERSIONINFO_1 lpVersionInfo);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetVersionEx(ref OSVERSIONINFO_2 lpVersionInfo);

        //now that the parameter is a class, its needs to be decorated with the marshal In, Out attributes
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetVersionEx([In, Out] clsOSVERSIONINFO_1 lpVersionInfo);

        public static void MyGetOsVersionExample()
        {
            OSVERSIONINFO_1 os1 = new OSVERSIONINFO_1();
            os1.dwOSVersionInfoSize = Marshal.SizeOf(os1); //determines the unmanaged memory size
            os1.szCSDVersion = new string(' ', 128); //only 127 chars are actually available, our string is far less

            if (GetVersionEx(ref os1))
            {
                Console.WriteLine("os1.szCSDVersion == '" + os1.szCSDVersion + "'");
            }

            OSVERSIONINFO_2 os2 = new OSVERSIONINFO_2();
            os2.dwOSVersionInfoSize = Marshal.SizeOf(os2);
            os2.szCSDVersion = new char[128];

            if (GetVersionEx(ref os2))
            {
                string expectedResult = new string(os2.szCSDVersion).Trim('\0');
                Console.WriteLine("expected result == '" + expectedResult + "'");
            }
        }

        //more complex examples
        //MSVCRT.DLL and its 'localtime' function

        //tm* localtime(const time_t* timer);

        public struct tm
        {
            public int tm_sec; // seconds after the minute
            public int tm_min; // minutes after the hour
            public int tm_hour; // hours since midnight
            public int tm_mday; // day of the month
            public int tm_mon; // months since January
            public int tm_year; // years since 1900
            public int tm_wday; // days since Sunday
            public int tm_yday; // days since January 1
            public int tm_isdst; // daylight savings time flag
        }

        [DllImport("msvcrt.dll")]
        public static extern IntPtr localtime(ref int timer);

        public static void GetLocalTimeExample()
        {
            int myTimer = 1;
            //have the structure assigned to get its size 
            tm myTm = new tm();

            //get a pointer to the block of unmanaged memory
            IntPtr toBeMarshalled = Marshal.AllocHGlobal(Marshal.SizeOf(myTm));

            //Marshal.AllocCoTaskMem is from the COM task memory allocator
            //Marshal.AllocHGlobal is from the Win32 one
            try
            {
                //perform the unmanaged function call
                toBeMarshalled = localtime(ref myTimer);

                //marshal the pointer back to your structure as such
                myTm = (tm) Marshal.PtrToStructure(toBeMarshalled, typeof (tm));

                Console.WriteLine("myTm.tm_hour = '" + myTm.tm_hour + "'");
            }
            finally
            {
                // Marshal.FreeHGlobal(toBeMarshalled);    
            }
        }

        public static void DisplayStruct(object o)
        {
            int totalBytes = Marshal.SizeOf(o);
            Console.WriteLine("Total Bytes = " + totalBytes);

            IntPtr ptr = IntPtr.Zero;
            try
            {
                // Allocate unmanaged memory
                ptr = Marshal.AllocCoTaskMem(totalBytes);
                // Marshal the type to its unmanaged representation
                Marshal.StructureToPtr(o, ptr, false);

                byte[] bytes = new byte[4];

                for (int i = 0; i < totalBytes; i += 4)
                {
                    // Print each byte in hexadecimal format
                    for (int j = 0; j < 4; j++)
                    {
                        if (i + j < totalBytes)
                        {
                            bytes[j] = Marshal.ReadByte(ptr, i + j);
                            Console.Write("{0:X2} ", bytes[j]);
                        }
                        else
                        {
                            Console.Write("   ");
                        }
                    }
                    Console.Write("  ");

                    // Print each byte as if it's a character
                    for (int j = 0; j < 4 && i + j < totalBytes; j++)
                    {
                        if (bytes[j] == 0)
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            Console.Write(Convert.ToChar(bytes[j]));
                        }
                    }
                    Console.WriteLine("");
                }
            }
            finally
            {
                // Free the unmanaged memory
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(ptr);
                }
            }
        }

        //using the kernel's function through P/Invoke
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        //if a dll is not found in a PATH variable then prior in a P/Invoke call the lib must be loaded
        public void NeedDllInNonPATHFolder()
        {
            //to, in turn, load other dll's to call more p/invoke functions on
            LoadLibrary(@"C:\WinDDK\7600.16385.1\Debuggers\dbghelp.dll");
        }

        public static void GeneralMarshalExamples()
        {
            //given a string
            string myString = "here is my string";
            
            //get pointer to it
            IntPtr myStringPtr = Marshal.StringToHGlobalAnsi(myString);

            //get the mem-block to hold it 
            IntPtr myStringMemBlockPtr = Marshal.AllocHGlobal(myString.Length + 1);

            unsafe
            {
                //turn an IntPtr into a pointer to a type
                byte *myUnsafeString = (byte*) myStringPtr.ToPointer();

                //use the block of memory to create a new, modified if needed, co
                byte *myUnsafeBlockOfMem = (byte*) myStringMemBlockPtr.ToPointer();

                int myStringLen = myString.Length;
                for(int i=0;i<myStringLen;i++)
                {
                    //recall "a[i]" is simply "*(a+i)"
                    byte valAtIndex = myUnsafeString[i];
                    valAtIndex += (byte)1;

                    myUnsafeBlockOfMem[i] = valAtIndex;
                }

                myUnsafeBlockOfMem[myStringLen] = (byte) '\0';

            }
            string myresultString = Marshal.PtrToStringAnsi(myStringMemBlockPtr);

            Console.WriteLine(myresultString);

            Marshal.FreeHGlobal(myStringMemBlockPtr);
            Marshal.FreeHGlobal(myStringPtr);

        }

        public void DotNetObjectsOfInterest()
        {
            //the Windows container 
            var proc = System.Diagnostics.Process.GetProcessById(1234);

            IntPtr procHandle = proc.Handle;
            IntPtr procEntryPoint = proc.MainModule.EntryPointAddress;


            //loaded dlls and exe's found with
            foreach(System.Diagnostics.ProcessModule module in proc.Modules)
            {
                IntPtr baseAddr = module.BaseAddress;
                IntPtr entryPt = module.EntryPointAddress;
                
            }

            foreach(System.Diagnostics.ProcessThread thread in proc.Threads)
            {
                IntPtr startAddr = thread.StartAddress;
            }

        }
        
		//dealing with struct union
		[DllImport("Ws2_32.dll", CharSet = CharSet.Ansi)]
        public static extern Int32 inet_pton(Int32 family, String pszAddrString, out IntPtr pAddrBuf);

		/*original struct as defined in inaddr.h
		
		typedef struct in_addr {
			union {
					struct { UCHAR s_b1,s_b2,s_b3,s_b4; } S_un_b;
					struct { USHORT s_w1,s_w2; } S_un_w;
					ULONG S_addr;
			} S_un;
		} 		
		
		*/
		
        [StructLayout(LayoutKind.Sequential)]
        public struct InAddr
        {
            public SUnion S_un;
        }

		//requires an explicit memory layout based on host being x86 or x64
        [StructLayout(LayoutKind.Explicit)]
        public struct SUnion
        {
            [FieldOffset(0)]
            public SUnB S_un_b;

            [FieldOffset(32)]
            public SUnW S_un_w;

            [FieldOffset(64)]
            public ulong S_addr;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct SUnB
        {
            [FieldOffset(0)]
            public sbyte s_b1;
            [FieldOffset(8)]
            public sbyte s_b2;
            [FieldOffset(16)]
            public sbyte s_b3;
            [FieldOffset(24)]
            public sbyte s_b4;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct SUnW
        {
            [FieldOffset(0)]
            public UInt16 s_w1;
            [FieldOffset(16)]
            public UInt16 s_w2;
        }

    }
}