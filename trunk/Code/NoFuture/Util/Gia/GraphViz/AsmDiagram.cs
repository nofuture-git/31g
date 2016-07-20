using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NoFuture.Shared;
using NoFuture.Util.Binary;

namespace NoFuture.Util.Gia.GraphViz
{
    public class AsmDiagram
    {
        public class EvData
        {
            private readonly string _node1Name;
            private readonly string _node2Name;

            public bool IsTwoWay { get; set; }
            public bool IsEnumerable { get;}
            public string Node1 { get; }
            public string Node2 { get; }
            public EvData(Tuple<FlattenedItem, FlattenedItem> f)
            {
                _node1Name = NfTypeName.SafeDotNetIdentifier(f.Item1.SimpleTypeName);
                _node2Name = NfTypeName.SafeDotNetIdentifier(f.Item2.SimpleTypeName);

                Node1 = f.Item1.TypeFullName;
                Node2 = f.Item2.TypeFullName;

                IsEnumerable = f.Item2.IsEnumerable;
            }
            public string[] NodeName => new[] {_node1Name, _node2Name};

            public bool AreSame(EvData ed)
            {
                if (ed == null)
                    return false;
                return ed.Node1 == Node1 && ed.Node2 == Node2;
            }

            public bool AreCounterparts(EvData ed)
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
                    ls.Add("arrowsize=0.26");
                }

                if (IsTwoWay)
                    ls.Add("color=\"#000000\"");

                if (ls.Any())
                    edge.Append(" [" + string.Join(",", ls.Where(x => !string.IsNullOrWhiteSpace(x))) + "]");

                edge.Append(";");
                return edge.ToString();

            }
        }
        #region fields

        private readonly List<string> _nodes;
        private readonly string _asmName;
        #endregion

        public List<EvData> Items { get; } = new List<EvData>();

        #region ctor
        public AsmDiagram(Assembly asm)
        {
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
                    var itemEv = new EvData(tupleOfItems);
                    if (Items.Any(x => x.AreSame(itemEv)))
                        continue;
                    Items.Add(itemEv);
                }
            }

            Items = RemoveDuplicates(Items);

            var names = Items.SelectMany(x => x.NodeName).ToList();
            _nodes = names.Distinct().ToList();
        }
        #endregion

        internal List<EvData> RemoveDuplicates(List<EvData> dataIn)
        {
            var dupIdx = GetDupIndices();
            var redux = new List<EvData>();
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
            var dataIn = Items;
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

        #region methods
        public string ToGraphVizString()
        {
            var gviz = new StringBuilder();
            gviz.AppendLine($"digraph {_asmName} ");
            gviz.AppendLine("{");
            gviz.AppendLine("graph [rankdir=\"LR\", splines=ortho, nodesep=0.06];");
            gviz.AppendLine("node [shape=circle, height=0.5, width=0.5, fontsize=10, fixedsize=\"true\"];");
            gviz.AppendLine("edge [arrowhead=none, color=\"#808080\"]");
            gviz.AppendLine(GetGraphContents());
            gviz.AppendLine("}");
            return gviz.ToString();
        }

        private string GetGraphContents()
        {
            var itemsOut = new List<string>();
            itemsOut.AddRange(_nodes.Select(x => $"{x} [label=\"-¦-\"];"));
            itemsOut.Add("");
            itemsOut.AddRange(Items.Select(x => x.ToString()));
            return string.Join("\n", itemsOut);
        }
        #endregion
    }
}
