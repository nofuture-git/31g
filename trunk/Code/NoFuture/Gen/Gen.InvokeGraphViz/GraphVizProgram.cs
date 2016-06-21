﻿using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Tools;
using NoFuture.Util.Binary;
using NoFuture.Util.Gia;
using NoFuture.Util.Gia.Args;
using NoFuture.Util.NfConsole;

namespace NoFuture.Gen.InvokeGraphViz
{
    public class GraphVizProgram : Program
    {
        private static readonly string[] _implementedDiagrams = {Settings.CLASS_DIAGRAM, Settings.FLATTENED_DIAGRAM};

        public GraphVizProgram(string[] args) : base(args, false)
        {
        }

        public string AsmPath { get; set; }
        public Assembly Assembly { get; set; }
        public string TypeName { get; set; }
        public string DiagramType { get; set; }
        public string GraphText { get; set; }
        public string OutputFileName { get; set; }
        public object LimitOn { get; set; }
        public bool DisplayEnums { get; set; }

        protected override string MyName => "InvokeGraphViz";

        public static void Main(string[] args)
        {
            var p = new GraphVizProgram(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp())
                    return;

                p.ParseProgramArgs();

                ValidateBinDir();

                switch (p.DiagramType)
                {
                    case Settings.FLATTENED_DIAGRAM:

                        var ft =
                            Flatten.FlattenType(new FlattenTypeArgs
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
                        p.OutputFileName = $"{p.TypeName.Replace(".", "")}Flattened.gv";
                        break;
                    case Settings.CLASS_DIAGRAM:
                        p.GraphText = Etc.GetClassDiagram(p.Assembly, p.TypeName);
                        p.OutputFileName = $"{p.TypeName.Replace(".", "")}ClassDiagram.gv";
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
            Thread.Sleep(20); //slight pause
        }

        protected static void ValidateBinDir()
        {
            BinDirectories.Root =
                ConfigurationManager.AppSettings[AppSettingKeys.RootBinDir];

            if (string.IsNullOrWhiteSpace(BinDirectories.Root) || !Directory.Exists(BinDirectories.Root))
            {
                throw new ItsDeadJim(
                    $"the root bin directory is not present at '{BinDirectories.Root}' " +
                    "- change the config file settings");
            }

            var f = ConfigurationManager.AppSettings[AppSettingKeys.DotExe];
            if (string.IsNullOrWhiteSpace(f))
            {
                throw new ItsDeadJim(
                    $"the path to Dot.exe is not present at '{f}' " +
                    $"- change the config file settings");
            }

            X86.DotExe = Path.Combine(BinDirectories.Root, f);

            if (!File.Exists(X86.DotExe))
            {
                throw new ItsDeadJim(
                    $"the path to Dot.exe is not present at '{X86.DotExe}' " +
                    $"- change the config file settings");
            }

            TempDirectories.Graph = ConfigurationManager.AppSettings[AppSettingKeys.GraphTempDir];

            if (string.IsNullOrWhiteSpace(TempDirectories.Graph))
            {
                throw new ItsDeadJim(
                    "assign a config file's appSettings for " +
                    "'NoFuture.TempDirectories.Graph' to a valid directory");
            }
        }

        protected override string Help()
        {
            var help = new StringBuilder();
            help.AppendLine(" ----");
            help.AppendLine($" [{Assembly.GetExecutingAssembly().GetName().Name}] ");
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
                Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_ASM_PATH_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                        to a valid .NET assembly. ");
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[STRING]      The types full name ",
                Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_FULL_TYPE_NAME_SWITCH,
                Constants.CMD_LINE_ARG_ASSIGN));
            help.AppendLine("                        (i.e. the namespace and class name).");
            help.AppendLine("");
            help.AppendLine(string.Format(" [{0}{1}]      Optional switch to have Enum's values displayed",
                Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_GRAPHVIZ_DISPLAY_ENUMS));
            help.AppendLine("");
            help.AppendLine(string.Format(" {0}{1}{2}[{3}]      The kind of diagram to create.",
                Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE, Constants.CMD_LINE_ARG_ASSIGN,
                string.Join(" | ", _implementedDiagrams)));
            help.AppendLine("");
            help.AppendLine(string.Format(" [{0}{1}{2}[\n  {3}]]              Optional and specific to Flattened.",
                Constants.CMD_LINE_ARG_SWITCH, Settings.INVOKE_GRAPHVIZ_FLATTENED_LIMIT_TYPE,
                Constants.CMD_LINE_ARG_ASSIGN,
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
                throw new RahRowRagee($"could not parse cmd line arg \n{string.Join(" ", _args)}");
            }

            if (!argHash.ContainsKey(Settings.INVOKE_ASM_PATH_SWITCH) ||
                argHash[Settings.INVOKE_ASM_PATH_SWITCH] == null)
            {
                throw new RahRowRagee(
                    $"the switch '{Settings.INVOKE_ASM_PATH_SWITCH}' could not " +
                    $"be parsed from cmd line arg \n{string.Join(" ", _args)}");
            }
            if (!argHash.ContainsKey(Settings.INVOKE_FULL_TYPE_NAME_SWITCH) ||
                argHash[Settings.INVOKE_FULL_TYPE_NAME_SWITCH] == null)
            {
                throw new RahRowRagee(
                    $"the switch '{Settings.INVOKE_FULL_TYPE_NAME_SWITCH}' could not " +
                    $"be parsed from cmd line arg \n{string.Join(" ", _args)}");
            }

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
                    $"The assembly at '{AsmPath}' could not " +
                    $"be loaded, see the log at '{Asm.ResolveAsmLog}' for more info.");
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

        internal static class AppSettingKeys
        {
            internal const string UseReflectionOnlyLoad = "NoFuture.Shared.Constants.UseReflectionOnlyLoad";
            internal const string DebugTempDir = "NoFuture.TempDirectories.Debug";
            internal const string RootBinDir = "NoFuture.BinDirectories.Root";
            internal const string GraphTempDir = "NoFuture.TempDirectories.Graph";
            internal const string DotExe = "NoFuture.Tools.X86.DotExe";
        }
    }
}