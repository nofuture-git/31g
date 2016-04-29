using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Shared;

namespace NoFuture.Util
{
    /// <summary>
    /// Location of reuseable functionality related to NoFuture console apps.
    /// </summary>
    public class ConsoleCmd
    {
        /// <summary>
        /// The per item function used by <see cref="ArgHash"/>
        /// </summary>
        /// <param name="cmdLineArg"></param>
        /// <returns></returns>
        public static DictionaryEntry? ParseArgKey2StringHash(String cmdLineArg)
        {
            if (cmdLineArg.StartsWith(Constants.CMD_LINE_ARG_SWITCH))
                cmdLineArg = cmdLineArg.Remove(0, 1); //remove dash

            var key = new StringBuilder();
            var val = new StringBuilder();
            var switchToVal = false;
            foreach (var c in cmdLineArg.ToCharArray())
            {
                if (!switchToVal && c == Constants.CMD_LINE_ARG_ASSIGN)
                {
                    switchToVal = true;
                    continue;
                }
                    
                if (!switchToVal)
                    key.Append(c);
                else
                    val.Append(c);
            }
            if (key.Length == 0)
                return null;
            var arg = new DictionaryEntry
            {
                Key = key.ToString(),
                Value = val.Length <= 0 ? bool.TrueString : val.ToString()
            };
            return arg;
        }

        /// <summary>
        /// A reusable function for parsing a Main statement's args string
        /// array into a hashtable of key value pairs using the delimiters 
        /// <see cref="Constants.CMD_LINE_ARG_SWITCH"/> and <see cref="Constants.CMD_LINE_ARG_ASSIGN"/>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Hashtable ArgHash(string[] args)
        {
            //parse command line switches
            var argHash = new Hashtable();
            foreach (var entry in args.Select(ParseArgKey2StringHash).Where(entry => entry != null))
            {
                argHash.Add(entry.Value.Key, entry.Value.Value);
            }
            return argHash;
        }

        /// <summary>
        /// Constructs a command line arg in the form expected herein.
        /// </summary>
        /// <param name="argName"></param>
        /// <param name="argValue"></param>
        /// <returns></returns>
        public static string ConstructCmdLineArgs(string argName, string argValue)
        {
            var cmdArg = new StringBuilder();
            cmdArg.Append(Constants.CMD_LINE_ARG_SWITCH);
            cmdArg.Append(argName);
            if (string.IsNullOrWhiteSpace(argValue))
                return cmdArg.ToString();

            cmdArg.Append(Constants.CMD_LINE_ARG_ASSIGN);
            cmdArg.Append(argValue.Contains(" ")
                ? string.Format("\"{0}\"", argValue)
                : string.Format("{0}", argValue));
            return cmdArg.ToString();
        }

        public static void SetConsoleAsTransparent(bool setOnParentProc = false)
        {
            var myproc = System.Diagnostics.Process.GetCurrentProcess();
            if (setOnParentProc)
                myproc = GetParentProcessById(myproc.Id);
            var hwnd = myproc.MainWindowHandle;
            var margin = new Aero.MARGINS { top = -1, left = -1 };
            Aero.DwmExtendFrameIntoClientArea(hwnd, ref margin);
        }

        /// <summary>
        /// Get the parent process for the given process id by <see cref="pid"/>
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static System.Diagnostics.Process GetParentProcessById(int pid)
        {
            var qry = "SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = " + pid;
            var search = new System.Management.ManagementObjectSearcher("root\\CIMV2", qry);
            
            var rslts = search.Get().GetEnumerator();
            rslts.MoveNext();
            var qryObj = rslts.Current;
            var parentId = (uint) qryObj["ParentProcessId"];
            return System.Diagnostics.Process.GetProcessById((int) parentId);
        }

        /// <summary>
        /// Will parse the results of an invocation to 'tList.exe' with the '-c' switch.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static List<Tuple<int, string, string>> ParseTListExeOutput(string buffer)
        {

            const string lnI = @"([0-9]+)\W(.+?\x2E[Ee][Xx][Ee])\W";
            const string lnIplus1 = @"\WCommand\sLine\:\W(.*)";
            var procs = new List<Tuple<int, string, string>>();

            var stdOut = buffer.Split('\n');

            for (var i = 0; i < stdOut.Length; i += 2)
            {
                if (string.IsNullOrWhiteSpace(stdOut[i]) || stdOut[i].Split(' ').Length == 0 || i % 2 != 0)
                    continue;
                var pid = string.Empty;
                var proc = string.Empty;
                var cmdln = string.Empty;
                if (!RegexCatalog.IsRegexMatch(stdOut[i], lnI, out pid, 1))
                    pid = "0";
                if (!RegexCatalog.IsRegexMatch(stdOut[i], lnI, out proc, 2))
                    proc = stdOut[i];
                if (!RegexCatalog.IsRegexMatch(stdOut[i + 1], lnIplus1, out cmdln, 1))
                    cmdln = stdOut[i + 1];

                int iPid = 0;
                int.TryParse(pid, out iPid);
                procs.Add(new Tuple<int, string, string>(iPid, proc.Trim(), cmdln.Trim()));
            }

            return procs;
        }
    }
}
