﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Tokens.Gia.GraphViz;
using NoFuture.Util.Core;

namespace NoFuture.Tokens.Gia
{
    /// <summary>
    /// Represents the targeted type of <see cref="Flatten"/>
    /// </summary>
    public class FlattenedType
    {
        public string Separator { get; set; }
        public bool UseTypeNames { get; set; }

        public string TypeFullName => NfReflect.GetLastTypeNameFromArrayAndGeneric(RootType);
        public string SimpleTypeName => NfReflect.GetTypeNameWithoutNamespace(TypeFullName);

        public Type RootType { get; set; }
        public List<FlattenedLine> Lines { get; set; }

        public List<string> PrintLines()
        {
            if(Lines == null || Lines.Count <= 0)
                return null;
            foreach (var ln in Lines)
            {
                ln.Separator = Separator;
            }
            return Lines.Select(x => x.ToFlattenedString(Separator, UseTypeNames)).ToList();
        }

        public string[] DistinctTypeNames
        {
            get
            {
                var l =
                    Lines?.SelectMany(x => x.Items.Select(y => y.TypeFullName))
                        .Where(s => !FlattenedItem.ValueTypesList.Contains(s))
                        .Distinct()
                        .ToList() ?? new List<string>();
                l.Add(TypeFullName);
                return l.ToArray();
            }
        }

        /// <summary>
        /// Specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public List<FlattenMrecord> GetGraphVizMrecords
        {
            get
            {
                //add all types 
                var mRecordDict = new Dictionary<string, FlattenMrecord>();
                foreach (var ty in DistinctTypeNames)
                {
                    if (mRecordDict.ContainsKey(ty))
                        continue;
                    mRecordDict.Add(ty, new FlattenMrecord(ty));
                }

                foreach (var rec in mRecordDict.Keys)
                {
                    var typeName = rec;
                    var mrecord = mRecordDict[typeName];
                    foreach (var ln in Lines)
                    {
                        foreach (var fi in ln.Items.Where(f => f.TypeFullName == typeName))
                        {
                            var propThereof = ln.FirstOnRight(fi);
                            if (propThereof == null || mrecord.Entries.Any(x => x.Equals(propThereof)))
                                continue;
                            mrecord.Entries.Add(propThereof);
                        }
                    }
                }

                return mRecordDict.Values.ToList();
            }
        }

        /// <summary>
        /// Specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public List<FlattenMrecordEdge> GetGraphVizEdges
        {
            get
            {
                var edges = new List<FlattenMrecordEdge>();
                foreach (var ln in Lines)
                {
                    for (var i = 0; i < ln.Items.Count; i++)
                    {
                        if (i + 1 >= ln.Items.Count)
                            break;
                        var left = ln.Items[i];
                        var right = ln.Items[i + 1];
                        if (right.IsTerminalNode)
                            break;
                        var edge = new FlattenMrecordEdge(left, right);
                        if(!edges.Any(x => x.Equals(edge)))
                            edges.Add(edge);
                    }
                }
                return edges;
            }
        }

        /// <summary>
        /// Specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public string ToGraphVizString()
        {
            var gviz = new StringBuilder();
            gviz.AppendLine(" digraph myTable{");
            gviz.AppendLine(" graph [");
            gviz.AppendLine(" rankdir=\"LR\"");
            gviz.AppendLine("];");
            gviz.AppendLine(" node [shape=Mrecord fontname=\"Consolas\"];");
            gviz.AppendLine("");
            foreach (var mrec in GetGraphVizMrecords)
                gviz.AppendLine(mrec.ToGraphVizString());

            gviz.AppendLine();
            gviz.AppendLine();

            foreach (var mEdge in GetGraphVizEdges)
                gviz.AppendLine(mEdge.ToGraphVizString());

            gviz.AppendLine("}");

            return gviz.ToString();
        }
    }
}