using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace NoFuture.Console
{
    public class MyMarshalled : IDisposable
    {
        public string MyManagedString { get; set; }
        public IntPtr MyNativeMemBlock { get; set; }
        public IntPtr MyNativeString { get; set; }
        public string MyReversedString { get; set; }

        public MyMarshalled()
        {
            MyManagedString = Util.Etc.LoremIpsumEightParagraphs;
            MyNativeString = Marshal.StringToHGlobalAnsi(MyManagedString);
            MyNativeMemBlock = Marshal.AllocHGlobal(MyManagedString.Length + 1);

            unsafe
            {
                byte* myUnsafeString = (byte*)MyNativeString.ToPointer();
                byte* myUnsafeBlockOfMem = (byte*)MyNativeMemBlock.ToPointer();

                int mycount = MyManagedString.Length;
                for (int i = 0; i < mycount; i++)
                {
                    byte valAtIndex = myUnsafeString[i];
                    valAtIndex += (byte)1;

                    myUnsafeBlockOfMem[i] = valAtIndex;
                }

                myUnsafeBlockOfMem[mycount] = (byte)'\0';
            }

            MyReversedString = Marshal.PtrToStringAnsi(MyNativeString);

        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(MyNativeMemBlock);
            
            Marshal.FreeHGlobal(MyNativeString);
        }
    }
}
