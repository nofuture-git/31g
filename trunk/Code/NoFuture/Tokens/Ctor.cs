using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Tokens
{
    public class Ctor
    {
        public class StringLiteral
        {
            public int Index { get; set; }
            public string Value { get; set; }
        }

        public string RawSignature { get; set; }
        public List<StringLiteral> StringLiterals { get; set; }
        public string AfterStringRem { get; set; }
        public string AfterFirstTerminator { get; set; }
        public string LeftSplit { get; set; }
        public string RightSplit { get; set; }
        public string Declaration { get; set; }
        public string ConstructorArgs { get; set; }
        public string InstanceType { get; set; }
        public string Namespace { get; set; }
        public string TType { get; set; }

        public static Ctor DisassembleConstructor(string signature)
        {
            var psSignature = new Ctor {RawSignature = signature};

            var ws = signature;

            //remove string literals
            if (ws.Contains('"'))
            {
                var nfXDoc = new XDocFrame('"', '"');

                var xdocItems = (nfXDoc.FindEnclosingTokens(signature));
                if (xdocItems != null)
                {
                    for (var i = 0; i < xdocItems.Count; i++)
                    {
                        var strToken = xdocItems[i];
                        psSignature.StringLiterals.Add(new StringLiteral()
                        {
                            Index = i,
                            Value = signature.Substring(strToken.Start, strToken.Span)
                        });
                        ws = ws.Replace((signature.Substring(strToken.Start, strToken.Span)), string.Format("%{0}", i));
                    }
                    psSignature.AfterStringRem = ws;
                }
            }
            //terminate at end of first statement
            if (ws.Contains(";"))
            {
                ws = ws.Substring(0, (ws.IndexOf(";", StringComparison.Ordinal)));
                psSignature.AfterFirstTerminator = ws;
            }

            //extract assignment 
            if (ws.Contains("="))
            {
                var assign = ws.Substring(0, (ws.IndexOf("=", StringComparison.Ordinal))).Trim();
                psSignature.LeftSplit = assign;
                ws =
                    ws.Substring(ws.IndexOf("=", StringComparison.Ordinal),
                        ws.Length - ws.IndexOf("=", StringComparison.Ordinal)).Trim();
                psSignature.RightSplit = ws;
                if (assign.Contains(" "))
                {
                    assign =
                        assign.Substring(assign.LastIndexOf(" ", StringComparison.Ordinal),
                            (assign.Length - assign.LastIndexOf(" ", StringComparison.Ordinal))).Trim();
                    psSignature.Declaration = assign;
                }
            }


            //back the working sig up to open parenth.
            if (ws.IndexOf("(", StringComparison.Ordinal) != -1)
            {
                var ctorArgs =
                    ws.Substring(ws.IndexOf("(", StringComparison.Ordinal),
                        (ws.Length - ws.IndexOf("(", StringComparison.Ordinal))).Trim();
                psSignature.ConstructorArgs = ctorArgs;
                ws = ws.Substring(0, (ws.IndexOf("(", StringComparison.Ordinal)));
                psSignature.InstanceType = ws;
            }


            //determine namespace
            if (ws.Contains("."))
            {
                var ns = (ws.Substring(0, ws.LastIndexOf(".", StringComparison.Ordinal)));
                if (ns.Contains(" "))
                {
                    ns =
                        ns.Substring(ns.LastIndexOf(" ", StringComparison.Ordinal),
                            (ns.Length - ns.LastIndexOf(" ", StringComparison.Ordinal))).Trim();
                }
                else
                {
                    ns = ns.Replace("=", string.Empty);
                }
                psSignature.Namespace = ns;
                ws =
                    ws.Substring(ws.LastIndexOf(".", StringComparison.Ordinal),
                        (ws.Length - ws.LastIndexOf(".", StringComparison.Ordinal))).Trim();
                if (ws.StartsWith("."))
                {
                    ws = ws.Substring(1, ws.Length - 1);
                }
                psSignature.TType = ws;
            }
            else
            {
                if (ws.Contains(" "))
                {
                    ws =
                        ws.Substring(ws.LastIndexOf(" ", StringComparison.Ordinal),
                            (ws.Length - ws.LastIndexOf(" ", StringComparison.Ordinal))).Trim();
                }
                else
                {
                    ws = ws.Replace("=", "");
                }
                psSignature.TType = ws;

            }

            return psSignature;
        }
    }
}
