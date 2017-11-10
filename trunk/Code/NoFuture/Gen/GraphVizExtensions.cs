using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Util;
using NoFuture.Util.Core;
using NoFuture.Util.NfType;

namespace NoFuture.Gen
{
    public static class GraphVizExtensions
    {
        /// <summary>
        /// Returns string specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public static string ToGraphVizString(this CgMember cgMem)
        {
            var graphViz = new StringBuilder();
            graphViz.Append("<tr><td align=\"left\">");
            graphViz.Append(cgMem.Name);
            graphViz.Append(" ");
            var typeColor = Etc.ValueTypesList.Contains(NfReflect.GetLastTypeNameFromArrayAndGeneric(cgMem.TypeName.Trim(), "<")) ? "blue" : "grey";
            if (cgMem.HasGetter || cgMem.HasSetter)
            {
                graphViz.Append("(");
                graphViz.Append(string.Join(", ", cgMem.Args.Select(x => x.ToGraphVizString())));
                graphViz.Append(")");
            }
            graphViz.Append(": <font color=\"");
            graphViz.Append(typeColor);
            graphViz.Append("\">");
            graphViz.Append(NfReflect.GetLastTypeNameFromArrayAndGeneric(cgMem.TypeName, "<"));
            if (cgMem.IsEnumerableType)
                graphViz.Append("[*]");
            graphViz.Append("</font></td></tr>");
            return graphViz.ToString();
        }

        /// <summary>
        /// Returns edge definitions specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public static string ToGraphVizEdge(this CgType cgType)
        {
            var graphViz = new StringBuilder();
            var myName = Util.Core.Etc.SafeDotNetIdentifier(cgType.FullName);
            var edges = new List<string>();
            foreach (
                var property in
                    cgType.Properties.Where(
                        x => !Etc.ValueTypesList.Contains(NfReflect.GetLastTypeNameFromArrayAndGeneric(x.TypeName, "<")))
                )
            {
                var toName =
                    Util.Core.Etc.SafeDotNetIdentifier(NfReflect.GetLastTypeNameFromArrayAndGeneric(property.TypeName, "<"));
                var edg = new StringBuilder();
                edg.AppendFormat("{0} -> {1}", myName, toName);
                edg.Append(property.IsEnumerableType ? " [arrowhead=odiamond]" : " [arrowhead=vee]");
                edg.Append(";");
                if (!edges.Contains(edg.ToString()))
                    edges.Add(edg.ToString());
            }
            foreach (var edge in edges)
            {
                graphViz.AppendLine(edge);
            }
            return graphViz.ToString();
        }

        /// <summary>
        /// Returns node definition specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public static string ToGraphVizNode(this CgType cgType)
        {
            var graphViz = new StringBuilder();
            graphViz.Append(Util.Core.Etc.SafeDotNetIdentifier(cgType.FullName));
            graphViz.AppendLine(" [shape=Mrecord, label=<<table bgcolor=\"white\" border=\"0\" >");
            graphViz.AppendLine("<th>");
            graphViz.AppendLine("<td bgcolor=\"grey\" align=\"center\">");
            graphViz.Append("<font color=\"white\">");
            graphViz.AppendFormat("{0} :: {1}", cgType.Name, string.IsNullOrWhiteSpace(cgType.Namespace) ? "global" : cgType.Namespace);
            graphViz.AppendLine("</font></td></th>");

            foreach (var prop in cgType.Properties)
                graphViz.AppendLine(prop.ToGraphVizString());
            foreach (var me in cgType.SortedMethods)
                graphViz.AppendLine(me.ToGraphVizString());
            graphViz.AppendLine("</table>> ];");

            return graphViz.ToString();
        }

        public static string ToGraphVizString(this CgArg cgArg)
        {
            return NfReflect.GetTypeNameWithoutNamespace(cgArg.ArgType);
        }

        /// <summary>
        /// Returns a node definition with just the type name's header.
        /// This is specific to graph-viz (ver. 2.38+)
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <param name="enumValues">Optional values to be listed as line items with no type specifiers nor .gv port ids.</param>
        /// <returns></returns>
        public static string EmptyGraphVizClassNode(string typeFullName, string[] enumValues)
        {
            var className = NfReflect.GetTypeNameWithoutNamespace(typeFullName);
            var ns = NfReflect.GetNamespaceWithoutTypeName(typeFullName);
            var fullName = string.Format("{0}{1}", string.IsNullOrWhiteSpace(ns) ? string.Empty : ns + ".", className);
            var graphViz = new StringBuilder();
            graphViz.Append(Util.Core.Etc.SafeDotNetIdentifier(fullName));
            graphViz.AppendLine(" [shape=Mrecord, label=<<table bgcolor=\"white\" border=\"0\" >");
            graphViz.AppendLine("<th>");
            graphViz.AppendLine("<td bgcolor=\"grey\" align=\"center\">");
            graphViz.Append("<font color=\"white\">");
            graphViz.AppendFormat("{0} :: {1}", className, string.IsNullOrWhiteSpace(ns) ? "global" : ns);
            graphViz.AppendLine("</font></td></th>");
            if (enumValues != null && enumValues.Length > 0)
            {
                foreach (var enumVal in enumValues)
                {
                    graphViz.Append("<tr><td><font color=\"blue\">");
                    graphViz.Append(enumVal);
                    graphViz.AppendLine("</font></td></tr>");
                }
            }
            else
            {
                graphViz.AppendLine("<tr><td></td></tr>");
                graphViz.AppendLine("<tr><td></td></tr>");
            }
            graphViz.AppendLine("</table>> ];");

            return graphViz.ToString();
        }
    }
}
