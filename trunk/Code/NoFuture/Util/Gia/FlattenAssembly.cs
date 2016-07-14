using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Gia
{
    public class FlattenAssembly
    {
        public List<FlattenedLine> AllLines { get; set; }
        public string AssemblyName { get; set; }
        public string Path { get; set; }

        public string ToGraphVizString()
        {
            var asmName = new System.Reflection.AssemblyName(AssemblyName);
            var gviz = new StringBuilder();
            gviz.AppendLine($" digraph {NfTypeName.SafeDotNetIdentifier(asmName.Name)} ");
            gviz.AppendLine("{");
            gviz.AppendLine("node [fontname=\"Consolas\", shape=Mrecord];");
            gviz.AppendLine(AllLinesToGraphViz());
            gviz.AppendLine("}");
            return gviz.ToString();
        }

        private string AllLinesToGraphViz()
        {
            if (AllLines == null || AllLines.Count <= 0)
                return string.Empty;

            var listOut = new List<string>();
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
                    var leftToken = item0.FlName.Replace("`","");
                    var rightToken = item1.FlName.Replace("`", "");

                    var t = string.Join("->", leftToken, rightToken);
                    
                    if (listOut.Any(x => x.Equals(t)))
                        continue;
                    listOut.Add(t);
                }
            }
            return string.Join("\n", listOut);
        }
    }
}
