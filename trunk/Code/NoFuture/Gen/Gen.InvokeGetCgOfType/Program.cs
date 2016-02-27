using System;
using System.IO;
using System.Text;
using System.Threading;
using NoFuture.Util.Binary;
using CfgMgr = System.Configuration.ConfigurationManager;

namespace NoFuture.Gen.InvokeGetCgOfType
{
    public class Program
    {
        internal static class AppSettingKeys
        {
            internal const string UseReflectionOnlyLoad = "NoFuture.Shared.Constants.UseReflectionOnlyLoad";
            internal const string DebugTempDir = "NoFuture.TempDirectories.Debug";
        }

        static void Main(string[] args)
        {
            try
            {
                //used during ReflectionOnlyAssemblyResolve event handler
                bool useReflectionOnly;
                var p = CfgMgr.AppSettings[AppSettingKeys.UseReflectionOnlyLoad];
                if (!string.IsNullOrWhiteSpace(p) && bool.TryParse(p, out useReflectionOnly))
                {
                    Shared.Constants.UseReflectionOnlyLoad = useReflectionOnly;
                }
                else
                {
                    Shared.Constants.UseReflectionOnlyLoad = false;
                }
                Util.FxPointers.AddResolveAsmEventHandlerToDomain();

                //test if there are any args, if not just leave with no message
                if (args.Length == 0 || args[0] == "-h" || args[0] == "-help" || args[0] == "/?")
                {
                    Console.WriteLine(Help());
                    return;
                }

                //get and test the cmd line arg key\values
                var argHash = Util.ConsoleCmd.ArgHash(args);

                if (argHash == null || argHash.Keys.Count <= 0)
                {
                    var msg = string.Format("could not parse cmd line arg \n{0}", string.Join(" ", args));
                    Console.WriteLine(msg);
                    return;
                }

                TempDirectories.Debug = CfgMgr.AppSettings[AppSettingKeys.DebugTempDir];

                if (!argHash.ContainsKey(Settings.INVOKE_ASM_PATH_SWITCH) ||
                    argHash[Settings.INVOKE_ASM_PATH_SWITCH] == null)
                {
                    var msg = string.Format("the switch '{0}' could be parsed from cmd line arg \n{1}",
                        Settings.INVOKE_ASM_PATH_SWITCH, string.Join(" ", args));
                    Console.WriteLine(msg);
                    return;
                }
                if (!argHash.ContainsKey(Settings.INVOKE_FULL_TYPE_NAME_SWITCH) ||
                    argHash[Settings.INVOKE_FULL_TYPE_NAME_SWITCH] == null)
                {
                    var msg = string.Format("the switch '{0}' could be parsed from cmd line arg \n{1}",
                        Settings.INVOKE_FULL_TYPE_NAME_SWITCH, string.Join(" ", args));
                    Console.WriteLine(msg);
                    return;
                }

                var resolveDependencies = argHash.ContainsKey(Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES) &&
                                           string.Equals(argHash[Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES].ToString(), bool.TrueString,
                                               StringComparison.OrdinalIgnoreCase);

                var asmPath = argHash[Settings.INVOKE_ASM_PATH_SWITCH].ToString();
                if (!File.Exists(asmPath))
                {
                    var msg = string.Format("There is no assembly at '{0}'.", asmPath);
                    Console.WriteLine(msg);
                    return;
                }
                
                Shared.Constants.AssemblySearchPaths.Add(Path.GetDirectoryName(asmPath));
                var asm = Shared.Constants.UseReflectionOnlyLoad
                    ? Asm.NfReflectionOnlyLoadFrom(asmPath)
                    : Asm.NfLoadFrom(asmPath);
                if (asm == null)
                {
                    var msg =
                        string.Format("The assembly at '{0}' could not be loaded, see the log at '{1}' for more info.",
                            asmPath, Asm.ResolveAsmLog);
                    Console.WriteLine(msg);
                    return;
                }

                var typeName = argHash[Settings.INVOKE_FULL_TYPE_NAME_SWITCH].ToString();
                var cgType = Etc.GetCgOfType(asm, typeName, false, resolveDependencies);

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
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Thread.Sleep(20);//slight pause
        }

        public static String Help()
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
