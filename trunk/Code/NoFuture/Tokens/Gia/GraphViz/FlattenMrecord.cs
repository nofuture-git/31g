﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Tokens.Gia.GraphViz
{
    /// <summary>
    /// Specific to graph-viz (ver. 2.38+)
    /// see [http://www.graphviz.org/]
    /// </summary>
    /// <returns></returns>
    public class FlattenMrecord
    {
        public const string NAME_PORT = "nm";
        public const string PROPERTY_PORT_PREFIX = "p";

        private string _labelName;

        public List<FlattenedItem> Entries { get; set; }
        public string NodeName { get; private set; }

        public static string MrecordName(string labelName)
        {
            var safeLabelName = NfString.SafeDotNetIdentifier(labelName);
            return string.Format("{0}", safeLabelName);
        }

        public FlattenMrecord(string labelName)
        {
            if(string.IsNullOrWhiteSpace(labelName))
                throw new ArgumentNullException("labelName");

            _labelName = labelName;
            NodeName = MrecordName(labelName);
            Entries = new List<FlattenedItem>();
        }

        public string ToGraphVizString()
        {
            var nodeText = new StringBuilder();
            nodeText.Append(NodeName);
            nodeText.Append(" [label=\"");
            nodeText.AppendFormat("<{0}> {1}", NAME_PORT, _labelName);

            if (Entries == null)
            {
                nodeText.Append("];");
                return nodeText.ToString();
            }
            nodeText.Append(" | ");
            nodeText.Append(string.Join(" | ", Entries.Select(x => x.ToGraphVizString())));

            nodeText.Append("\"];");
            return nodeText.ToString();
        }
    }

    /// <summary>
    /// Specific to graph-viz (ver. 2.38+)
    /// see [http://www.graphviz.org/]
    /// </summary>
    /// <returns></returns>
    public class FlattenMrecordEdge
    {
        public FlattenedItem Left { get; private set; }
        public FlattenedItem Right { get; private set; }

        public FlattenMrecordEdge(FlattenedItem left, FlattenedItem right)
        {
            Left = left;
            Right = right;
        }

        public string ToGraphVizString()
        {
            if (Left.IsTerminalNode || Right.IsTerminalNode)
                return string.Empty;
            var edgeString = new StringBuilder();
            edgeString.AppendFormat("{0}:{1}{2} -> ", FlattenMrecord.MrecordName(Left.TypeFullName), FlattenMrecord.PROPERTY_PORT_PREFIX, Right.FlName);
            edgeString.AppendFormat("{0}:{1}", FlattenMrecord.MrecordName(Right.TypeFullName), FlattenMrecord.NAME_PORT);

            edgeString.Append(Right.IsEnumerable ? " [arrowhead=odiamond];" : " [arrowhead=vee];");

            return edgeString.ToString();
        }

        public bool Equals(FlattenMrecordEdge edge)
        {
            if (edge == null)
                return false;

            return string.Equals(edge.ToGraphVizString(), ToGraphVizString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
