using System;
using System.IO;
using System.Text;
using System.Threading;
using NoFuture.Exceptions;
using NoFuture.Globals;
using NoFuture.Util.Binary;
using NoFuture.Util.NfConsole;
using CfgMgr = System.Configuration.ConfigurationManager;

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
            Thread.Sleep(20);//slight pause
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);
            if (!argHash.ContainsKey(Settings.INVOKE_ASM_PATH_SWITCH) ||
                argHash[Settings.INVOKE_ASM_PATH_SWITCH] == null)
            {
                throw new RahRowRagee(string.Format("the switch '{0}' could be parsed from cmd line arg \n{1}",
                    Settings.INVOKE_ASM_PATH_SWITCH, string.Join(" ", _args)));
            }
            if (!argHash.ContainsKey(Settings.INVOKE_FULL_TYPE_NAME_SWITCH) ||
                argHash[Settings.INVOKE_FULL_TYPE_NAME_SWITCH] == null)
            {
                throw new RahRowRagee(string.Format("the switch '{0}' could be parsed from cmd line arg \n{1}",
                    Settings.INVOKE_FULL_TYPE_NAME_SWITCH, string.Join(" ", _args)));
            }

            ResolveDependencies = argHash.ContainsKey(Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES) &&
                                       string.Equals(argHash[Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES].ToString(), bool.TrueString,
                                           StringComparison.OrdinalIgnoreCase);

            AsmPath = argHash[Settings.INVOKE_ASM_PATH_SWITCH].ToString();
            if (!File.Exists(AsmPath))
            {
                throw new RahRowRagee(string.Format("There is no assembly at '{0}'.", AsmPath));
            }

            NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(AsmPath));
            Assembly = NfConfig.UseReflectionOnlyLoad
                ? Asm.NfReflectionOnlyLoadFrom(AsmPath)
                : Asm.NfLoadFrom(AsmPath);
            if (Assembly == null)
            {
                throw new RahRowRagee(
                    string.Format("The assembly at '{0}' could not be loaded, see the log at '{1}' for more info.",
                        AsmPath, Asm.ResolveAsmLog));
            }

            TypeName = argHash[Settings.INVOKE_FULL_TYPE_NAME_SWITCH].ToString();            
        }

        protected override string MyName
        {
            get { return "InvokeGetCgOfType"; }
        }

        protected override String Help()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine(string.Format(" [{0}] ", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name));
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
                Shared.Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_ASM_PATH_SWITCH,
                Shared.Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                        to a valid .NET assembly. ");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]      The types full name ",
                Shared.Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_FULL_TYPE_NAME_SWITCH,
                Shared.Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                        (i.e. the namespace and class name).");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[True|False]      Optional switch to have ",
                Shared.Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES,
                Shared.Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                        the IL of every member inspected for  ");
            help.AppendLine("                        dependencies to other assemblies.  ");
            help.AppendLine("");

            return help.ToString();
        }
    }
}
