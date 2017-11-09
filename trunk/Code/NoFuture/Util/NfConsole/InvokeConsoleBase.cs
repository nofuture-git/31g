using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NoFuture.Shared;
using NoFuture.Shared.Core;

namespace NoFuture.Util.NfConsole
{
    public abstract class InvokeConsoleBase : IDisposable
    {

        protected Process MyProcess;
        protected static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        protected static string ConvertJsonFromBuffer(byte[] buffer)
        {
            var decoder = Encoding.UTF8.GetDecoder();

            var jsonChars = new char[decoder.GetCharCount(buffer, 0, buffer.Length)];
            decoder.GetChars(buffer, 0, buffer.Length, jsonChars, 0);

            var jsonStringBldr = new StringBuilder();
            jsonStringBldr.Append(jsonChars);

            return jsonStringBldr.ToString();
        }

        protected Process StartRemoteProcess(string exe, string args)
        {
            var si = !string.IsNullOrWhiteSpace(args)
                ? new ProcessStartInfo(exe, args)
                : new ProcessStartInfo(exe);

            si.CreateNoWindow = false;
            si.UseShellExecute = true;
            si.RedirectStandardOutput = false;
            si.RedirectStandardError = false;

            return StartRemoteProcess(si);
        }

        protected Process StartRemoteProcess(ProcessStartInfo si)
        {
            if (string.IsNullOrWhiteSpace(si?.FileName) || !File.Exists(si.FileName))
            {
                throw new ArgumentException(nameof(si));
            }
            var myProcess = new Process { StartInfo = si };

            myProcess.Start();
            Thread.Sleep(NfConfig.ThreadSleepTime);

            return myProcess;

        }

        public bool IsMyProcessRunning
        {
            get
            {
                if (MyProcess == null)
                    return false;
                MyProcess.Refresh();
                return !MyProcess.HasExited;
            }
        }

        public int MyProcessId
        {
            get
            {
                if (!IsMyProcessRunning)
                    return -1;
                return MyProcess.Id;
            }
        }

        public void Dispose()
        {
            if (!IsMyProcessRunning)
                return;
            MyProcess.CloseMainWindow();
            MyProcess.Close();
        }
    }
}
