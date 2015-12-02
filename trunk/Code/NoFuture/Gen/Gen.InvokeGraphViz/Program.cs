using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NoFuture.Util.Binary;
using NoFuture.Tools;
using NoFuture.Util.Gia.Args;

namespace NoFuture.Gen.InvokeGraphViz
{
    public class Program
    {
        private static string[] _implementedDiagrams = {Settings.CLASS_DIAGRAM, Settings.FLATTENED_DIAGRAM};

        public static void Main(string[] args)
        {
            try
            {
                Util.FxPointers.AddResolveAsmEventHandlerToDomain();

                //test if there are any args, if not just leave with no message
                if (args.Length == 0 || args[0] == "-h" || args[0] == "-help" || args[0] == "/?")
                {
                    Console.WriteLine(Help());
                    return;
                }

                TempDirectories.Debug = ConfigurationManager.AppSettings["NoFuture.TempDirectories.Debug"];

                BinDirectories.Root =
                    ConfigurationManager.AppSettings["NoFuture.BinDirectories.Root"];

                if (string.IsNullOrWhiteSpace(BinDirectories.Root) || !Directory.Exists(BinDirectories.Root))
                {
                    var msg =
                        string.Format(
                            "the root bin directory is not present at '{0}' - change the config file settings",
                            BinDirectories.Root);
                    Console.WriteLine(msg);
                    return;
                }

                var f = ConfigurationManager.AppSettings["NoFuture.Tools.X86.DotExe"];
                if (string.IsNullOrWhiteSpace(f))
                {
                    var msg =
                        string.Format(
                            "the path to Dot.exe is not present at '{0}' - change the config file settings", f);
                    Console.WriteLine(msg);
                    return;
                }

                X86.DotExe = Path.Combine(BinDirectories.Root, f);

                if (!File.Exists(X86.DotExe))
                {
                    var msg =
                        string.Format(
                            "the path to Dot.exe is not present at '{0}' - change the config file settings", X86.DotExe);
                    Console.WriteLine(msg);
                    return;
                }

                TempDirectories.Graph = ConfigurationManager.AppSettings["NoFuture.TempDirectories.Graph"];

                if (string.IsNullOrWhiteSpace(TempDirectories.Graph))
                {
                    Console.WriteLine(
                        @"assign a config file's appSettings for  'NoFuture.TempDirectories.Graph' to a valid directory");
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

                var asmPath = argHash[Settings.INVOKE_ASM_PATH_SWITCH].ToString();
                if (!File.Exists(asmPath))
                {
                    var msg = string.Format("There is no assembly at '{0}'.", asmPath);
                    Console.WriteLine(msg);
                    return;
                }

                Shared.Constants.AssemblySearchPath = Path.GetDirectoryName(asmPath);

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

                var diagramType = argHash[Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE].ToString();

                if (string.IsNullOrWhiteSpace(diagramType) || !_implementedDiagrams.Contains(diagramType))
                {
                    Console.WriteLine("choose a valid type of diagram listed in the help ");
                    return;
                }

                var graphText = string.Empty;
                var outputFileName = string.Empty;
                switch (diagramType)
                {
                    case Settings.FLATTENED_DIAGRAM:
                        var limitOn = argHash[Settings.INVOKE_GRAPHVIZ_FLATTENED_LIMIT_TYPE];
                        var displayEnums = argHash[Settings.INVOKE_GRAPHVIZ_DISPLAY_ENUMS];
                        var ft =
                            Util.Gia.Flatten.FlattenType(new FlattenTypeArgs()
                            {
                                Assembly = asm,
                                Depth = 16,
                                Separator = "-",
                                UseTypeNames = false,
                                TypeFullName = typeName,
                                LimitOnThisType = limitOn == null ? null : limitOn.ToString(),
                                DisplayEnums = displayEnums != null
                            });
                        graphText = ft.ToGraphVizString();
                        outputFileName = string.Format("{0}Flattened.gv", typeName.Replace(".", ""));
                        break;
                    case Settings.CLASS_DIAGRAM:
                        graphText = Etc.GetClassDiagram(asm, typeName);
                        outputFileName = string.Format("{0}ClassDiagram.gv", typeName.Replace(".", ""));
                        break;
                }

                outputFileName = Path.Combine(TempDirectories.Graph, outputFileName);
                File.WriteAllText(outputFileName, graphText);

                using (var sw = new StreamWriter(Console.OpenStandardOutput(), Encoding.UTF8))
                {
                    sw.Write(outputFileName);
                    sw.Flush();
                    sw.Close();
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
            help.AppendLine(" This executable is specific to creating GraphViz (ver. 2.38+) ");
            help.AppendLine(" for types defined in the NoFuture namespace.");
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
            help.AppendLine(string.Format(" [{0}{1}]      Optional switch to have Enum's values displayed",
                Shared.Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_GRAPHVIZ_DISPLAY_ENUMS));
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[{3}]      The kind of diagram to create.",
                Shared.Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE, Shared.Constants.CMD_LINE_ARG_ASSIGN,
                string.Join(" | ", _implementedDiagrams)));
            help.AppendLine("");
            help.AppendLine(string.Format(" [{0}{1}{2}[\n  {3}]]              Optional and specific to Flattened.",
                Shared.Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_GRAPHVIZ_FLATTENED_LIMIT_TYPE,
                Shared.Constants.CMD_LINE_ARG_ASSIGN,
                string.Join("\n| ", Etc.ValueTypesList)));
            help.AppendLine("");
            return help.ToString();
        }
    }
}
