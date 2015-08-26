using System;
using System.Runtime.InteropServices;

namespace NoFuture.Console
{
    public class MyPinned : IDisposable
    {
        public GCHandle GarbageHandle { get; set; }
        public IntPtr PointerToGcHandle { get; set; }
        public string Something { get; set; }
        public byte[] Data { get; set; }

        public MyPinned()
        {
            Something = Util.Etc.LoremIpsumEightParagraphs;
            Data = System.Text.Encoding.UTF8.GetBytes(Something);
            GarbageHandle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            PointerToGcHandle = GarbageHandle.AddrOfPinnedObject();
        }

        public void Dispose()
        {
            GarbageHandle.Free();
        }

    }
}
