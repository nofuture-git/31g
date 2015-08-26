using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Console
{
    public class CallCounts
    {
        public int Exception { get; set; }
        public int Lock { get; set; }
        public int Thread { get; set; }
        public int Marshal { get; set; }
        public int Loh { get; set; }
        public int Poco { get; set; }
        public int Pinned { get; set; }
        public const string PRINT_FORMAT = "{0,-32}{1}";
        public const int THREAD_SLEEP_MILLI_SECONDS = 5000;
        public const int LARGE_OBJECT_BYTE_ARRAY_SIZE = 1048576;

        public string Print()
        {
            var str = new System.Text.StringBuilder();
            str.AppendLine("------------------------------- ----");
            str.AppendFormat(PRINT_FORMAT, "Exceptions", this.Exception);
            str.AppendLine();
            str.AppendFormat(PRINT_FORMAT, "Locks", this.Lock);
            str.AppendLine();
            str.AppendFormat(PRINT_FORMAT, "Threads", this.Thread);
            str.AppendLine();
            str.AppendFormat(PRINT_FORMAT, "GC Pinned Objects", this.Pinned);
            str.AppendLine();
            str.AppendFormat(PRINT_FORMAT, "Marshal Objects", this.Marshal);
            str.AppendLine();
            str.AppendFormat(PRINT_FORMAT, "Large Object Heap", this.Loh);
            str.AppendLine();
            str.AppendFormat(PRINT_FORMAT, "Plain Old CLR Objects", this.Poco);
            str.AppendLine();
            str.AppendFormat(PRINT_FORMAT, "", "----");
            str.AppendLine();
            str.AppendFormat(PRINT_FORMAT, "Total", Exception + Lock + Thread + Marshal + Loh + Poco + Pinned);
            str.AppendLine();
            str.AppendLine("------------------------------- ----");
            return str.ToString();
        }
        public class PropertyNames
        {
            public static string Exception { get { return "Exception"; } }
            public static string Lock { get { return "Lock"; } }
            public static string Thread { get { return "Thread"; } }
            public static string Marshal { get { return "Marshal"; } }
            public static string Loh { get { return "Loh"; } }
            public static string Poco { get { return "Poco"; } }
            public static string Pinned { get { return "Pinned"; } }
        }
    }
}
