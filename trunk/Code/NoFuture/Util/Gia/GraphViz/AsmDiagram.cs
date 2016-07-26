using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NoFuture.Shared;
using NoFuture.Util.Binary;

namespace NoFuture.Util.Gia.GraphViz
{
    [Serializable]
    public class AsmDiagram
    {
        #region inner types
        [Serializable]
        public class AsmDiagramEdge
        {
            private readonly string _node1Name;
            private readonly string _node2Name;

            public bool IsTwoWay { get; set; }
            public bool IsEnumerable { get;}
            public string Node1 { get; }
            public string Node2 { get; }
            public AsmDiagramEdge(Tuple<FlattenedItem, FlattenedItem> f)
            {
                _node1Name = NfTypeName.SafeDotNetIdentifier(f.Item1.TypeFullName.Replace(".", "_"), false, 128);
                _node2Name = NfTypeName.SafeDotNetIdentifier(f.Item2.TypeFullName.Replace(".", "_"), false, 128);

                Node1 = f.Item1.TypeFullName;
                Node2 = f.Item2.TypeFullName;

                IsEnumerable = f.Item2.IsEnumerable;
            }
            public string[] NodeName => new[] {_node1Name, _node2Name};

            public bool AreSame(AsmDiagramEdge ed)
            {
                if (ed == null)
                    return false;
                return ed.Node1 == Node1 && ed.Node2 == Node2;
            }

            public bool AreCounterparts(AsmDiagramEdge ed)
            {
                if(ed == null)
                    return false;
                return ed.Node2 == Node1 && ed.Node1 == Node2;
            }

            public override string ToString()
            {
                var edge = new StringBuilder();
                edge.Append($"{_node1Name} -> {_node2Name}");
                var ls = new List<string>();
                if (IsEnumerable)
                {
                    ls.Add("arrowhead=\"diamond\"");
                    ls.Add("arrowsize=0.35");
                }

                if (IsTwoWay)
                    ls.Add("color=\"#000000\"");

                if (ls.Any())
                    edge.Append(" [" + string.Join(",", ls.Where(x => !string.IsNullOrWhiteSpace(x))) + "]");

                edge.Append(";");
                return edge.ToString();

            }
        }
        [Serializable]
        public class AsmDiagramNode
        {
            private readonly string _origText;
            private readonly int _edgeCount;

            public string NodeNamespace { get; }

            public AsmDiagramNode(string txt, int countOfEdges)
            {
                _origText = txt;
                _edgeCount = countOfEdges;
                var nameParts = _origText.Split('_');
                NodeNamespace = string.Join("_", nameParts.Take(nameParts.Length - 1));
            }

            public override string ToString()
            {
                switch (_edgeCount)
                {
                    case 0:
                    case 1:
                    case 2:
                        return $"{_origText} [label=\"-¦-\"];";
                    case 3:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.25, width=0.25, fontsize=6, fixedsize=\"true\"];";
                    case 4:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.30, width=0.30, fontsize=6, fixedsize=\"true\"];";
                    case 5:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.35, width=0.35, fontsize=6, fixedsize=\"true\"];";
                    case 6:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.40, width=0.40, fontsize=10, fixedsize=\"true\"];";
                    case 7:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.45, width=0.45, fontsize=12, fixedsize=\"true\"];";
                    case 8:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.5, width=0.5, fontsize=12, fixedsize=\"true\"];";
                    case 9:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.55, width=0.55, fontsize=12, fixedsize=\"true\"];";
                    case 10:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.6, width=0.6, fontsize=14, fixedsize=\"true\"];";
                    case 11:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.7, width=0.7, fontsize=14, fixedsize=\"true\"];";
                    case 12:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.75, width=0.75, fontsize=16, fixedsize=\"true\"];";
                    case 13:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.8, width=0.8, fontsize=16, fixedsize=\"true\"];";
                    case 14:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.85, width=0.85, fontsize=18, fixedsize=\"true\"];";
                    case 15:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=0.95, width=0.95, fontsize=20, fixedsize=\"true\"];";
                    default:
                        return $"{_origText} [label=\"-¦-\", shape=circle, height=1.0, width=1.0, fontsize=22, fixedsize=\"true\"];";
                }
            }
        }
        [Serializable]
        public class AsmDiagramSubGraph
        {
            private readonly List<AsmDiagramNode> _nsNodes;
            private readonly string _ns;

            public AsmDiagramSubGraph(string ns, List<AsmDiagramNode> allNodes)
            {
                _ns = ns;
                _nsNodes = allNodes.Where(x => x.NodeNamespace == ns).ToList();
            }

            public override string ToString()
            {
                if (_nsNodes == null || _nsNodes.Count <= 0)
                    return string.Empty;
                var graphViz = new StringBuilder();
                graphViz.Append($"subgraph cluster_{NfTypeName.SafeDotNetIdentifier(_ns)}");
                graphViz.AppendLine("{");
                graphViz.AppendLine("\tlabel=\"\";");
                graphViz.AppendLine("\tcolor=red;");
                foreach (var item in _nsNodes)
                {
                    graphViz.AppendLine($"\t{item}");
                }
                graphViz.AppendLine("}");

                return graphViz.ToString();
            }
        }
        #endregion

        #region fields
        private readonly List<AsmDiagramNode> _nodes;
        private readonly List<AsmDiagramSubGraph> _nsSubGraphs = new List<AsmDiagramSubGraph>();
        private readonly string _asmName;
        private readonly bool _withNsSubgraphs;
        #endregion

        #region properties
        public List<AsmDiagramEdge> Edges { get; } = new List<AsmDiagramEdge>();

        #endregion

        #region ctor
        public AsmDiagram(Assembly asm, bool withNamespaceSubGraphs = false)
        {
            _withNsSubgraphs = withNamespaceSubGraphs;
            _asmName = NfTypeName.SafeDotNetIdentifier(asm.GetName().Name);
            foreach (var asmType in asm.NfGetExportedTypes())
            {
                var item1 = new FlattenedItem(asmType) {FlName = asmType.Name};
                if (item1.IsTerminalNode || NfTypeName.IsEnumType(asmType))
                    continue;

                foreach (var p in asmType.GetProperties(NfConfig.DefaultFlags))
                {
                    if (NfTypeName.IsEnumType(p.PropertyType))
                        continue;
                    var item2 = new FlattenedItem(p.PropertyType) {FlName = p.Name};
                    if(item2.IsTerminalNode)
                        continue;
                    var tupleOfItems = new Tuple<FlattenedItem, FlattenedItem>(item1, item2);
                    var itemEv = new AsmDiagramEdge(tupleOfItems);
                    if (Edges.Any(x => x.AreSame(itemEv)))
                        continue;
                    Edges.Add(itemEv);
                }
            }

            Edges = RemoveDuplicates(Edges);

            var names = Edges.SelectMany(x => x.NodeName).ToList();
            var uqNames = names.Distinct().ToList();
            _nodes = uqNames.Select(x => new AsmDiagramNode(x, GetCountOfEdgesOn(x))).ToList();

            if (_withNsSubgraphs)
            {
                var uqNamespaces = _nodes.Select(x => x.NodeNamespace).Distinct().ToList();
                foreach (var ns in uqNamespaces)
                {
                    _nsSubGraphs.Add(new AsmDiagramSubGraph(ns, _nodes));
                }
            }

        }
        #endregion

        #region methods
        internal List<AsmDiagramEdge> RemoveDuplicates(List<AsmDiagramEdge> dataIn)
        {
            var dupIdx = GetDupIndices();
            var redux = new List<AsmDiagramEdge>();
            var skipThese = new List<int>();
            for (var i = 0; i < dupIdx.GetLongLength(0); i++)
            {
                var idx0 = dupIdx[i, 0];
                var idx1 = dupIdx[i, 1];
                skipThese.AddRange(new [] {idx0, idx1});
                if (idx0 == idx1)
                {
                    redux.Add(dataIn[idx0]);
                    continue;
                }
                var exbA = dataIn[idx0];
                var exbB = dataIn[idx1];

                var toAdd = exbA.IsEnumerable ? exbA : exbB;
                toAdd.IsTwoWay = true;
                redux.Add(toAdd);
            }
            for (var i = 0; i < dataIn.Count; i++)
            {
                if (skipThese.Any(x => x == i))
                    continue;
                if (redux.Any(x => x.AreSame(dataIn[i])))
                    continue;
                if (redux.Any(x => x.AreCounterparts(dataIn[i])))
                    continue;
                redux.Add(dataIn[i]);
            }

            return redux;
        }

        internal int[,] GetDupIndices()
        {
            var dataIn = Edges;
            var idxMatches = new List<Tuple<int, int>>();
            for (var i = 0; i < dataIn.Count; i++)
            {
                //we already matched this
                if (idxMatches.Any(x => x.Item2 == i))
                    continue;
                //foward only look-ahead
                var remaining = dataIn.Skip(i + 1).ToList();

                for (var j = 0; j < remaining.Count; j++)
                {
                    
                    if (!dataIn[i].AreCounterparts(remaining[j]))
                        continue;
                    var idxMatch = new Tuple<int, int>(i, j);
                    if (idxMatches.Any(x => x.Equals(idxMatch)))
                        continue;
                    if (idxMatches.Any(x => x.Equals(new Tuple<int, int>(j, i))))
                        continue;

                    idxMatches.Add(idxMatch);
                }
            }
            var idx = new int[idxMatches.Count, 2];
            for (var i = 0; i < idxMatches.Count; i++)
            {
                idx[i, 0] = idxMatches[i].Item1;
                idx[i, 1] = idxMatches[i].Item2;
            }
            return idx;
        }

        internal int GetCountOfEdgesOn(string nodeName)
        {
            return Edges.Sum(x => x.NodeName.Contains(nodeName) ? 1 : 0);
        }

        public string ToGraphVizString()
        {
            var gviz = new StringBuilder();
            gviz.AppendLine($"digraph {_asmName} ");
            gviz.AppendLine("{");
            gviz.AppendLine("graph [rankdir=\"LR\", splines=ortho, nodesep=0.06];");
            gviz.AppendLine("node [shape=circle, height=0.2, width=0.2, fontsize=6, fixedsize=\"true\"];");
            gviz.AppendLine("edge [arrowhead=none, color=\"#808080\"]");
            gviz.AppendLine(GetGraphContents());
            gviz.AppendLine("}");
            return gviz.ToString();
        }

        private string GetGraphContents()
        {
            var itemsOut = new List<string>();
            if (_withNsSubgraphs)
            {
                foreach (var nodeTxt in _nsSubGraphs.Select(x => x.ToString()))
                {
                    if (itemsOut.Any(x => x == nodeTxt))
                        continue;
                    itemsOut.Add(nodeTxt);
                }
            }
            else
            {
                foreach (var node in _nodes)
                {
                    if (itemsOut.Any(x => x == node.ToString()))
                        continue;
                    itemsOut.Add(node.ToString());
                }
            }
            itemsOut.Add("");
            foreach (var item in Edges)
            {
                //finaly check to remove exact duplicates
                if(itemsOut.Any(x => x == item.ToString()))
                    continue;
                itemsOut.Add(item.ToString());
            }
            return string.Join("\n", itemsOut);
        }
        #endregion
    }
}
