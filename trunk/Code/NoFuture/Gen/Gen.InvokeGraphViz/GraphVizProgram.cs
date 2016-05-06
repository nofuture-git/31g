using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NoFuture.Exceptions;
using NoFuture.Util.Binary;
using NoFuture.Tools;
using NoFuture.Util.Gia.Args;
using NoFuture.Util.NfConsole;

namespace NoFuture.Gen.InvokeGraphViz
{
    public class GraphVizProgram : NoFuture.Util.NfConsole.Program
    {
        private static readonly string[] _implementedDiagrams = {Settings.CLASS_DIAGRAM, Settings.FLATTENED_DIAGRAM};

        public string AsmPath { get; set; }
        public System.Reflection.Assembly Assembly { get; set; }
        public string TypeName { get; set; }
        public string DiagramType { get; set; }
        public string GraphText { get; set; }
        public string OutputFileName { get; set; }
        public object LimitOn { get; set; }
        public bool DisplayEnums { get; set; }

        public GraphVizProgram(string[] args) : base(args, false) { }

        internal static class AppSettingKeys
        {
            internal const string UseReflectionOnlyLoad = "NoFuture.Shared.Constants.UseReflectionOnlyLoad";
            internal const string DebugTempDir = "NoFuture.TempDirectories.Debug";
            internal const string RootBinDir = "NoFuture.BinDirectories.Root";
            internal const string GraphTempDir = "NoFuture.TempDirectories.Graph";
            internal const string DotExe = "NoFuture.Tools.X86.DotExe";
        }

        public static void Main(string[] args)
        {
            var p = new GraphVizProgram(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp()) return;

                p.ParseProgramArgs(); 
                
                ValidateBinDir();

                switch (p.DiagramType)
                {
                    case Settings.FLATTENED_DIAGRAM:

                        var ft =
                            Util.Gia.Flatten.FlattenType(new FlattenTypeArgs()
                            {
                                Assembly = p.Assembly,
                                Depth = 16,
                                Separator = "-",
                                UseTypeNames = false,
                                TypeFullName = p.TypeName,
                                LimitOnThisType = p.LimitOn == null ? null : p.LimitOn.ToString(),
                                DisplayEnums = p.DisplayEnums
                            });
                        p.GraphText = ft.ToGraphVizString();
                        p.OutputFileName = string.Format("{0}Flattened.gv", p.TypeName.Replace(".", ""));
                        break;
                    case Settings.CLASS_DIAGRAM:
                        p.GraphText = Etc.GetClassDiagram(p.Assembly, p.TypeName);
                        p.OutputFileName = string.Format("{0}ClassDiagram.gv", p.TypeName.Replace(".", ""));
                        break;
                }

                p.OutputFileName = Path.Combine(TempDirectories.Graph, p.OutputFileName);
                File.WriteAllText(p.OutputFileName, p.GraphText);

                using (var sw = new StreamWriter(Console.OpenStandardOutput(), Encoding.UTF8))
                {
                    sw.Write(p.OutputFileName);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                p.PrintToConsole(ex);
            }
            Thread.Sleep(20);//slight pause
        }

        protected static void ValidateBinDir()
        {
            BinDirectories.Root =
                ConfigurationManager.AppSettings[AppSettingKeys.RootBinDir];

            if (string.IsNullOrWhiteSpace(BinDirectories.Root) || !Directory.Exists(BinDirectories.Root))
            {
                throw new ItsDeadJim(
                    string.Format(
                        "the root bin directory is not present at '{0}' - change the config file settings",
                        BinDirectories.Root));
            }

            var f = ConfigurationManager.AppSettings[AppSettingKeys.DotExe];
            if (string.IsNullOrWhiteSpace(f))
            {
                throw new ItsDeadJim(
                    string.Format(
                        "the path to Dot.exe is not present at '{0}' - change the config file settings", f));
            }

            X86.DotExe = Path.Combine(BinDirectories.Root, f);

            if (!File.Exists(X86.DotExe))
            {
                throw new ItsDeadJim(
                    string.Format(
                        "the path to Dot.exe is not present at '{0}' - change the config file settings", X86.DotExe));
            }

            TempDirectories.Graph = ConfigurationManager.AppSettings[AppSettingKeys.GraphTempDir];

            if (string.IsNullOrWhiteSpace(TempDirectories.Graph))
            {
                throw new ItsDeadJim(
                    @"assign a config file's appSettings for  'NoFuture.TempDirectories.Graph' to a valid directory");
            }
        }

        protected override string MyName
        {
            get { return "InvokeGraphViz"; }
        }

        protected override String Help()
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

        protected override void ParseProgramArgs()
        {
            //get and test the cmd line arg key\values
            var argHash = ConsoleCmd.ArgHash(_args);

            if (argHash == null || argHash.Keys.Count <= 0)
            {
                throw new RahRowRagee(string.Format("could not parse cmd line arg \n{0}", string.Join(" ", _args)));
            }

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

            AsmPath = argHash[Settings.INVOKE_ASM_PATH_SWITCH].ToString();
            if (!File.Exists(AsmPath))
            {
                throw new RahRowRagee(string.Format("There is no assembly at '{0}'.", AsmPath));
            }

            Shared.Constants.AssemblySearchPaths.Add(Path.GetDirectoryName(AsmPath));

            Assembly = Shared.Constants.UseReflectionOnlyLoad
                ? Asm.NfReflectionOnlyLoadFrom(AsmPath)
                : Asm.NfLoadFrom(AsmPath);
            if (Assembly == null)
            {
                throw new RahRowRagee(
                    string.Format("The assembly at '{0}' could not be loaded, see the log at '{1}' for more info.",
                        AsmPath, Asm.ResolveAsmLog));
            }

            TypeName = argHash[Settings.INVOKE_FULL_TYPE_NAME_SWITCH].ToString();

            DiagramType = argHash[Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE].ToString();

            if (string.IsNullOrWhiteSpace(DiagramType) || !_implementedDiagrams.Contains(DiagramType))
            {
                throw new RahRowRagee("choose a valid type of diagram listed in the help ");
            }
            LimitOn = argHash[Settings.INVOKE_GRAPHVIZ_FLATTENED_LIMIT_TYPE];
            DisplayEnums = argHash.ContainsKey(Settings.INVOKE_GRAPHVIZ_DISPLAY_ENUMS);
            GraphText = string.Empty;
            OutputFileName = string.Empty;
        }
    }
}
