using System;

namespace NoFuture.Domain
{
    /// <summary>
    /// Text file logging specific to the <see cref="NoFuture.Domain.Engine"/>
    /// </summary>
    public class Log
    {
        #region fields
        private static int _maxColumnLength = 64; 
        static readonly Object MYLOCK = new Object();
        public static string RootPath { get; set; }
        #endregion

        public static void Write(string msg)
        {
            lock (MYLOCK)
            {
                try
                {
                    var printMsg = string.Format("{0:yyyyMMdd HH:mm:ss.fffff} - {1}\n", DateTime.Now, msg);
                    Console.Write(printMsg);
                    System.IO.File.AppendAllText(GenerateLogFileName(RootPath),printMsg);
                }
                catch (Exception ex)
                {
                    var printEx = string.Format("Logging exception :: {0}\n{1}\n", ex.Message, ex.StackTrace);
                    Console.Write(printEx);
                    System.IO.File.AppendAllText(GenerateLogFileName(RootPath), printEx);
                    if (ex.InnerException != null)
                    {
                        var printInnerEx = string.Format("inner exception :: {0}\n{1}\n",
                                                         ex.InnerException.Message,
                                                         ex.InnerException.StackTrace);
                        
                        Console.Write(printInnerEx);
                        System.IO.File.AppendAllText(GenerateLogFileName(RootPath), printInnerEx);
                    }
                }
            }
        }
        public static void ExceptionLogging(Exception ex)
        {
            if (ex == null)
                return;
            Write(String.Format("Logging exception :: {0}\n{1}", ex.Message, ex.StackTrace));
            if (ex.InnerException != null)
            {
                Write(String.Format("inner exception :: {0}\n{1}", ex.InnerException.Message, ex.InnerException.StackTrace));
            }
        }

        #region helpers
        public static void StringArrayLogging(string[] strings)
        {
            if (strings == null)
                return;
            foreach(var val in strings)
            {
                Write(string.Format("{0}",val));
            }
        }

        public static void NameValueLogging(System.Collections.Specialized.NameValueCollection webHeaderCollection, string label)
        {
            if(webHeaderCollection == null)
                return;
            Write(string.Format("--{0}--",label));
            foreach (var key in webHeaderCollection.AllKeys)
            {
                Write(string.Format("{0,-40}{1}",key,webHeaderCollection[key]));
            }
            Write("----");
        }

        public static string GetColumnFormat(string leftColumn, string rightColumn)
        {
            try
            {
                if(leftColumn == null || rightColumn == null)
                    return string.Format("{0,-64}{1}", leftColumn, rightColumn);

                if (leftColumn.Length > _maxColumnLength)
                    _maxColumnLength = leftColumn.Length;

                var format = "{0,-" + _maxColumnLength.ToString() + "}{1}";

                return string.Format(format, leftColumn, rightColumn);

            }
            catch
            {

                return string.Format("{0} {1}", leftColumn, rightColumn);
            }
        }
        public static string GenerateBinFileName(string path, string classname)
        {
            if (path == null)
                path = ".\\";
            classname = string.Format("{0:yyyyMMdd-HHmmss-fffff}-{1}.bin", DateTime.Now, classname);
            return System.IO.Path.Combine(path, classname);
        }
        public static string GenerateLogFileName(string path)
        {
            if (path == null)
                path = ".\\";

            return System.IO.Path.Combine(path, string.Format("{0:yyyyMMdd}_AppDomain.log", DateTime.Today));
        }
        #endregion
    }//end Log

}//end NoFuture.Domain
