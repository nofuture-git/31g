using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util.Binary;
using NoFuture.Util.NfConsole;

namespace NoFuture.Gen.InvokeGetCgOfType
{
    public class CgProgram : Program
    {
        public CgProgram(string[] args) :base(args, false) { }

        public bool ResolveDependencies { get; set; }
        public string TypeName { get; set; }
        public string AsmPath { get; set; }
        public System.Reflection.Assembly Assembly { get; set; }

        public static void Main(string[] args)
        {
            var p = new CgProgram(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp()) return;

                p.ParseProgramArgs();

                var cgType = Etc.GetCgOfType(p.Assembly, p.TypeName, false, p.ResolveDependencies);

                var dcs = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof (CgType));
                using (var std = Console.OpenStandardOutput())
                {
                    dcs.WriteObject(std, cgType);
                    std.Flush();
                    std.Close();
                }
            }
            catch (Exception ex)
            {
                p.PrintToConsole(ex);
            }
            Thread.Sleep(NfConfig.ThreadSleepTime);//slight pause
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);
            if (!argHash.ContainsKey(Settings.INVOKE_ASM_PATH_SWITCH) ||
                argHash[Settings.INVOKE_ASM_PATH_SWITCH] == null)
            {
                throw new RahRowRagee(
                    $"the switch '{Settings.INVOKE_ASM_PATH_SWITCH}' could not be " +
                    $"parsed from cmd line arg \n{string.Join(" ", _args)}");
            }
            if (!argHash.ContainsKey(Settings.INVOKE_FULL_TYPE_NAME_SWITCH) ||
                argHash[Settings.INVOKE_FULL_TYPE_NAME_SWITCH] == null)
            {
                throw new RahRowRagee(
                    $"the switch '{Settings.INVOKE_FULL_TYPE_NAME_SWITCH}' could not " +
                    $"be parsed from cmd line arg \n{string.Join(" ", _args)}");
            }

            ResolveDependencies = argHash.ContainsKey(Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES) &&
                                  string.Equals(argHash[Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES].ToString(),
                                      bool.TrueString,
                                      StringComparison.OrdinalIgnoreCase);

            AsmPath = argHash[Settings.INVOKE_ASM_PATH_SWITCH].ToString();
            if (!File.Exists(AsmPath))
            {
                throw new RahRowRagee($"There is no assembly at '{AsmPath}'.");
            }

            NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(AsmPath));
            Assembly = NfConfig.UseReflectionOnlyLoad
                ? Asm.NfReflectionOnlyLoadFrom(AsmPath)
                : Asm.NfLoadFrom(AsmPath);
            if (Assembly == null)
            {
                throw new RahRowRagee(
                    $"The assembly at '{AsmPath}' could not be loaded, " +
                    $"see the log at '{Asm.ResolveAsmLog}' for more info.");
            }

            TypeName = argHash[Settings.INVOKE_FULL_TYPE_NAME_SWITCH].ToString();

            NfConfig.CustomTools.InvokeNfTypeName = NoFuture.Util.NfPath.GetAppCfgSetting("NoFuture.ToolsCustomTools.InvokeNfTypeName");

        }

        protected override string MyName
        {
            get { return "InvokeGetCgOfType"; }
        }

        protected override String GetHelpText()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine($" [{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}] ");
            help.AppendLine("");
            help.AppendLine(" This executable is specific to getting a serialized instance of ");
            help.AppendLine(" NoFuture.Gen.CgType on the standard output.");
            help.AppendLine(" It is intended to operate as an isolated process whose AppDomain");
            help.AppendLine(" is independent of the rest of calling assemblies runtime.");
            help.AppendLine(" This has two benifits, one, the ReflectionOnlyLoadFrom ");
            help.AppendLine(" assemblies are only locked during the timespan of this exe, and two,");
            help.AppendLine(" the calling assembly will not have its AppDomain loaded with ");
            help.AppendLine(" with assemblies which were only needed for Code Generation.");
            help.AppendLine(" ");
            help.AppendLine("Usage: options ");
            help.AppendLine("Options:");
            help.AppendLine(" -h | -help             Will print this help.");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]      The full directory path ",
                NfConfig.CmdLineArgSwitch, Settings.INVOKE_ASM_PATH_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                        to a valid .NET assembly. ");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]      The types full name ",
                NfConfig.CmdLineArgSwitch, Settings.INVOKE_FULL_TYPE_NAME_SWITCH,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                        (i.e. the namespace and class name).");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[True|False]      Optional switch to have ",
                NfConfig.CmdLineArgSwitch, Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES,
                NfConfig.CmdLineArgAssign));
            help.AppendLine("                        the IL of every member inspected for  ");
            help.AppendLine("                        dependencies to other assemblies.  ");
            help.AppendLine("");

            return help.ToString();
        }
    }
}
