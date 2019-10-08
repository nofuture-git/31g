using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Core.Math.Matrix
{
    public static class MatrixPrint
    {
        public static string Print(this decimal[] v, string style = null)
        {
            if (v == null)
                return "";
            return Print(v.Select(x => (double)x).ToArray(), style);
        }

        public static string Print(this long[] v, string style = null)
        {
            if (v == null)
                return "";
            return Print(v.Select(x => (double)x).ToArray(), style);
        }

        public static string Print(this int[] v, string style = null)
        {
            if (v == null)
                return "";
            return Print(v.Select(x => (double)x).ToArray(), style);
        }

        public static string Print(this double[] v, string style = null)
        {
            if (v == null)
                return "";
            return style == null ? PrintRstyle(v.ToMatrix()) : PrintCodeStyle(v.ToMatrix(), style);
        }

        public static string Print(this double[,] a, string style = null)
        {
            if (a == null)
                return "";
            return style == null
                ? PrintRstyle(a)
                : PrintCodeStyle(a, style);
        }

        internal static string PrintCodeStyle(double[,] a, string style = "js")
        {
            var open = "[";
            var close = "]";
            var str = new StringBuilder();
            var isRstyle = new[] { "r" }.Any(v => String.Equals(v, style, StringComparison.OrdinalIgnoreCase));
            if (new[] { "cs", "c#" }.Any(v => String.Equals(style, v, StringComparison.OrdinalIgnoreCase)))
            {
                open = "{";
                close = "}";
                str.Append("new[,] ");
            }

            if (new[] { "ps", "powershell" }.Any(v => String.Equals(v, style, StringComparison.OrdinalIgnoreCase)))
            {
                open = "@(";
                close = ")";
            }

            if (new[] { "java" }.Any(v => String.Equals(v, style, StringComparison.OrdinalIgnoreCase)))
            {
                open = "{";
                close = "}";
                str.Append("new Double[][] ");
            }

            if (isRstyle)
            {
                open = "";
                close = "";
                str.Append("matrix(c(");
            }

            str.AppendLine(open);
            var lines = new List<string>();
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                var strLn = new StringBuilder();
                strLn.Append(open);
                var vals = new List<string>();
                for (var j = 0; j < a.CountOfColumns(); j++)
                {
                    vals.Add(a[i, j].ToString(CultureInfo.InvariantCulture));
                }

                strLn.Append(String.Join(",", vals));
                strLn.Append(close);
                lines.Add(strLn.ToString());
            }

            str.Append(String.Join($",{Environment.NewLine}", lines));
            str.AppendLine("");
            if (!isRstyle)
            {
                str.Append(close);
            }
            else
            {
                str.Append($"), ncol={a.CountOfColumns()}, byrow=TRUE)");
            }
            return str.ToString();
        }

        internal static string PrintRstyle(double[,] a)
        {
            var str = new StringBuilder();
            const int ROUND_TO = 6;
            //find the max len
            var maxLen = 0;
            var anyNeg = false;
            for (var i = 0; i < a.CountOfRows(); i++)
            for (var j = 0; j < a.CountOfColumns(); j++)
            {
                var aij = a[i, j];
                var aijString = System.Math.Round(aij, ROUND_TO).ToString(CultureInfo.InvariantCulture);
                if (aijString.Length > maxLen)
                    maxLen = aijString.Length;
                if (aij < 0)
                    anyNeg = true;
            }

            maxLen += 2;

            for (var i = 0; i < a.CountOfColumns(); i++)
            {
                var headerStr = $"[,{i}]";
                var padding = (maxLen);

                if (i == 0)
                {
                    var hdrFill = maxLen <= 2 ? 2 : 4;
                    str.Append(new String(' ', a.CountOfRows().ToString().Length + hdrFill));
                }

                str.AppendFormat("{0,-" + padding + "}", headerStr);
            }

            str.AppendLine();
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                str.Append($"[{i},] ");
                for (var j = 0; j < a.CountOfColumns(); j++)
                {
                    var aij = a[i, j];
                    var aijString = System.Math.Round(aij, ROUND_TO).ToString(CultureInfo.InvariantCulture);

                    var format = aij >= 0 && anyNeg ? " {0,-" + (maxLen - 1) + "}" : "{0,-" + maxLen + "}";
                    str.AppendFormat(format, aijString);
                }
                str.AppendLine();
            }

            return str.ToString();
        }
    }
}
