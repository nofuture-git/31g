using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Shared;

namespace NoFuture.Util.Gia
{
    public class FlattenAssembly
    {
        public List<FlattenedLine> AllLines { get; set; }
        public string AssemblyName { get; set; }
        public string Path { get; set; }

        public string ToGraphVizString(params string[] regexPatterns)
        {
            var asmName = new System.Reflection.AssemblyName(AssemblyName);
            var gviz = new StringBuilder();
            gviz.AppendLine($"digraph {NfTypeName.SafeDotNetIdentifier(asmName.Name)} ");
            gviz.AppendLine("{");
            gviz.AppendLine("graph [rankdir=\"LR\", splines=ortho, nodesep=0.06];");
            gviz.AppendLine("node [shape=circle, height=0.5, width=0.5, fontsize=10, fixedsize=\"true\"];");
            gviz.AppendLine("edge [arrowhead=none, color=\"#808080\"]");
            gviz.AppendLine(AllLinesToGraphViz(regexPatterns));
            gviz.AppendLine("}");
            return gviz.ToString();
        }

        private string AllLinesToGraphViz(params string[] regexPatterns)
        {
            if (AllLines == null || AllLines.Count <= 0)
                return string.Empty;
            var nodes = new List<string>();
            var edges = new List<string>();
            foreach (var ln in AllLines)
            {
                if (ln.Items == null || ln.Items.Count <= 0)
                    continue;
                for (var i = 0; i < ln.Items.Count - 1; i++)
                {
                    var item0 = ln.Items[i];
                    var item1 = ln.Items[i + 1];
                    if (item0 == null || item1 == null || item0.IsTerminalNode ||
                        item1.IsTerminalNode)
                        continue;
                    
                    var leftToken = item0.SimpleTypeName.Replace("`","");
                    var rightToken = item1.SimpleTypeName.Replace("`", "");

                    if (regexPatterns?.Length > 0)
                    {
                        if (!RegexCatalog.AreAnyRegexMatch(leftToken, regexPatterns) &&
                            !RegexCatalog.AreAnyRegexMatch(rightToken, regexPatterns))
                            continue;
                    }
                    var e = item1.IsEnumerable
                        ? $"{leftToken} -> {rightToken} [arrowhead=\"diamond\", arrowsize=0.26];"
                        : $"{leftToken} -> {rightToken};";
                    
                    var n0 = $"{leftToken} [label=\"-¦-\"];";
                    var n1 = $"{rightToken} [label=\"-¦-\"];";

                    if (!nodes.Any(x => x.Equals(n0)))
                        nodes.Add(n0);

                    if (!nodes.Any(x => x.Equals(n1)))
                        nodes.Add(n1);

                    var existingEdge =
                        edges.FirstOrDefault(
                            x =>
                                x.StartsWith($"{rightToken} -> {leftToken}") ||
                                x.StartsWith($"{leftToken} -> {rightToken}"));

                    //when there is already a edge for these two and its more detailed
                    if (existingEdge != null && existingEdge.Length >= e.Length)
                        continue;

                    //when the new edge is more detailed than the existing add it and remove the old one
                    if (existingEdge != null && e.Length > existingEdge.Length)
                    {
                        edges.Add(e);
                        edges.Remove(existingEdge);
                    }
                    //for everything else
                    else if (edges.All(x => x != e))
                    {
                        edges.Add(e);
                    }

                }
            }

            nodes.AddRange(edges);

            return string.Join("\n", nodes);
        }
    }
}
