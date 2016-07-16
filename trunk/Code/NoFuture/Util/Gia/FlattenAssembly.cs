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
            gviz.AppendLine("edge [arrowhead=none, color=\"red;0.06:#808080:blue;0.06\"]");
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

                    var e = string.Join("->", leftToken, rightToken);
                    var n0 = $"{leftToken} [label=\"-¦-\"];";
                    var n1 = $"{rightToken} [label=\"-¦-\"];";

                    if (!edges.Any(x => x.Equals(e)))
                        edges.Add(e);

                    if (!nodes.Any(x => x.Equals(n0)))
                        nodes.Add(n0);

                    if (!nodes.Any(x => x.Equals(n1)))
                        nodes.Add(n1);
                }
            }

            nodes.AddRange(edges);

            return string.Join("\n", nodes);
        }
    }
}
