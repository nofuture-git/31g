using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Core.Math.Matrix
{
    public static class MatrixExtensions
    {
        public static long CountOfRows(this double[,] a)
        {
            return a.GetLongLength(0);
        }

        public static long CountOfColumns(this double[,] a)
        {
            return a.GetLongLength(1);
        }

        public static double[,] Plus(this double[,] a, double[,] b)
        {
            return MatrixOps.Sum(a, b);
        }

        public static double[,] Minus(this double[,] a, double[,] b)
        {
            return MatrixOps.Difference(a, b);
        }

        public static double[,] DotProduct(this double[,] a, double[,] b)
        {
            return MatrixOps.Product(a, b);
        }

        public static double[,] DotScalar(this double[,] a, double scalar)
        {
            return MatrixOps.Product(a, scalar);
        }

        public static void SwapRow(this double[,] a, long row1, long row2)
        {
            a = a ?? new double[,] { };
            var len = a.CountOfRows();
            if (row1 >= len || row2 >= len)
                return;

            for (var j = 0; j < a.CountOfColumns(); j++)
            {
                var row1J = a[row1, j];
                var row2J = a[row2, j];
                a[row1, j] = row2J;
                a[row2, j] = row1J;
            }
        }

        public static void SwapColumn(this double[,] a, long column1, long column2)
        {
            a = a ?? new double[,] { };
            var len = a.CountOfColumns();
            if (column1 >= len || column2 >= len)
                return;

            for (var i = 0; i < a.CountOfColumns(); i++)
            {
                var column1J = a[i, column1];
                var column2J = a[i, column2];
                a[i, column1] = column2J;
                a[i, column2] = column1J;
            }
        }

        public static void ApplyAtRow(this double[,] a, double b, long atRow, Func<double, double, double> expr)
        {
            a = a ?? new double[,] { };
            var bArr = Enumerable.Repeat(b, (int) a.CountOfRows()).ToArray();
            ApplyAtRow(a, bArr, atRow, expr);
        }

        public static void ApplyAtColumn(this double[,] a, double b, long atColumn, Func<double, double, double> expr)
        {
            a = a ?? new double[,] { };
            var bArr = Enumerable.Repeat(b, (int) a.CountOfColumns()).ToArray();
            ApplyAtColumn(a, bArr, atColumn, expr);
        }

        public static void ApplyAtRow(this double[,] a, double[] b, long atRow, Func<double, double, double> expr)
        {
            a = a ?? new double[,] { };
            if (atRow >= a.CountOfRows())
                throw new NonConformableException($"{atRow} exceeds the number of rows present in a");
            if (expr == null)
                return;
            for (var j = 0; j < a.CountOfColumns(); j++)
            {
                var v = b.Length <= j ? 0 : b[j];
                a[atRow, j] = expr(a[atRow, j], v);
            }
        }

        public static void ApplyAtColumn(this double[,] a, double[] b, long atColumn, Func<double, double, double> expr)
        {
            a = a ?? new double[,] { };
            if (atColumn >= a.CountOfColumns())
                throw new NonConformableException($"{atColumn} exceeds the number of columns present in a");
            if (expr == null)
                return;
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                var v = b.Length <= i ? 0 : b[i];
                a[i, atColumn] = expr(a[i, atColumn], v);
            }
        }

        public static double[,] AppendColumns(this double[,] a, double[,] b)
        {
            var leftColumnCount = a.CountOfColumns();
            var rightColumnCount = b.CountOfColumns();

            var rows = a.CountOfRows();
            var cols = leftColumnCount + rightColumnCount;
            var vout = new double[rows, cols];
            var rightRowCount = b.CountOfRows();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < leftColumnCount; j++)
                {
                    vout[i, j] = a[i, j];
                }

                for (var j = 0; j < rightColumnCount; j++)
                {
                    if(j >= cols || i >= rightRowCount)
                        continue;
                    vout[i, j + leftColumnCount] = b[i, j];
                }
            }

            return vout;
        }

        public static double[,] AppendRows(this double[,] a, double[,] b)
        {
            var at = a.Transpose();
            var vout = at.AppendColumns(b.Transpose());
            return vout.Transpose();
        }

        /// <summary>
        /// https://rosettacode.org/wiki/Reduced_row_echelon_form
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[,] ReducedRowEchelonForm(this double[,] matrix)
        {
            const double TOLERANCE = 0.00000001D;
            var lead = 0;
            var rowCount = matrix.CountOfRows();
            var columnCount = matrix.CountOfColumns();
            for (var r = 0; r < rowCount; r++)
            {
                if (columnCount <= lead) break;
                var i = r;
                while (System.Math.Abs(matrix[i, lead]) < TOLERANCE)
                {
                    i++;
                    if (i == rowCount)
                    {
                        i = r;
                        lead++;
                        if (columnCount == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }
                for (var j = 0; j < columnCount; j++)
                {
                    var temp = matrix[r, j];
                    matrix[r, j] = matrix[i, j];
                    matrix[i, j] = temp;
                }
                var div = matrix[r, lead];
                if (System.Math.Abs(div) > TOLERANCE)
                    for (var j = 0; j < columnCount; j++) matrix[r, j] /= div;
                for (var j = 0; j < rowCount; j++)
                {
                    if (j != r)
                    {
                        var sub = matrix[j, lead];
                        for (var k = 0; k < columnCount; k++) matrix[j, k] -= (sub * matrix[r, k]);
                    }
                }
                lead++;
            }
            return matrix;
        }

        /// <summary>
        /// https://martin-thoma.com/solving-linear-equations-with-gaussian-elimination/
        /// </summary>
        /// <param name="a"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double[] GaussElimination(this double[,] a, double[] v)
        {
            var n = a.CountOfRows();
            var A = a.AppendColumns(v.ToMatrix().Transpose());
            for (var i = 0; i < n; i++)
            {
                // Search for maximum in this column
                var maxEl = System.Math.Abs(A[i, i]);
                var maxRow = i;
                for (var k = i + 1; k < n; k++)
                {
                    if (System.Math.Abs(A[k, i]) > maxEl)
                    {
                        maxEl = System.Math.Abs(A[k, i]);
                        maxRow = k;
                    }
                }

                // Swap maximum row with current row (column by column)
                A.SwapRow(i,maxRow);
                
                // Make all rows below this one 0 in current column
                for (var k = i + 1; k < n; k++)
                {
                    var c = -A[k, i] / A[i, i];
                    if (c.Equals(double.NaN))
                        c = 0;
                    for (var j = i; j < n + 1; j++)
                    {
                        if (i == j)
                        {
                            A[k, j] = 0;
                        }
                        else
                        {
                            A[k, j] += c * A[i, j];
                        }
                    }
                }
            }

            // Solve equation Ax=b for an upper triangular matrix A
            var x = new double[n];
            for (var i = n - 1; i >= 0; i--)
            {
                var c = A[i, n] / A[i, i];
                if (c.Equals(double.NaN))
                    c = 0;
                x[i] = c;
                for (var k = i - 1; k >= 0; k--)
                {
                    A[k, n] -= A[k, i] * x[i];
                }
            }

            return x;
        }

        /// <summary>
        /// Puts <see cref="a"/> into a matrix at the diagonals
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double[,] Diag(this double[] a)
        {
            a = a ?? new double[] { };
            var o = new double[a.Length, a.Length];
            for (var i = 0; i < o.CountOfRows(); i++)
            {
                for (var j = 0; j < o.CountOfColumns(); j++)
                {
                    o[i, j] = i == j ? a[i] : 0;
                }
            }

            return o;
        }

        public static double[] Flatten(this double[,] a)
        {
            a = a ?? new double[,] { };
            var r = a.CountOfRows();
            var c = a.CountOfColumns();
            var d = new double[r * c];
            var counter = 0;
            for (var i = 0; i < r; i++)
            {
                for (var j = 0; j < c; j++)
                {
                    var v = a[i, j];
                    d[counter] = v;
                    counter++;
                }
            }

            return d;
        }

        /// <summary>
        /// Collapses the matrix by performing <see cref="expr"/> on each column
        /// </summary>
        /// <param name="a"></param>
        /// <param name="expr">The expression of how to coalese an array into a single value</param>
        /// <returns></returns>
        public static double[] CollapseTop2Bottom(this double[,] a, Func<double[], double> expr)
        {
            a = a ?? new double[,] { };
            var dout = new double[a.CountOfColumns()];

            for (var i = 0; i < dout.Length; i++)
            {
                var rowI = a.SelectColumn(i);
                dout[i] = expr(rowI);
            }

            return dout;
        }

        /// <summary>
        /// Collapses the matrix by performing <see cref="expr"/> on each row
        /// </summary>
        /// <param name="a"></param>
        /// <param name="expr">The expression of how to coalese an array into a single value</param>
        /// <returns></returns>
        public static double[] CollapseLeft2Right(this double[,] a, Func<double[], double> expr)
        {
            a = a ?? new double[,] { };
            var dout = new double[a.CountOfRows()];

            for (var i = 0; i < dout.Length; i++)
            {
                var rowI = a.SelectRow(i);
                dout[i] = expr(rowI);
            }

            return dout;
        }

        /// <summary>
        /// Sections out <see cref="a"/> into blocks of equal size from left to right thereby
        /// creating a matrix.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="numOfColumns"></param>
        /// <param name="truncEnding">
        /// If <see cref="a"/> cannot be evenly divided and this is false an exception will be thrown
        /// </param>
        /// <returns></returns>
        public static double[,] ToMatrix(this double[] a, long numOfColumns = 1, bool truncEnding = false)
        {
            a = a ?? new double[] { };
            if (!a.Any() || numOfColumns <= 0)
                return new double[,] { };
            if (numOfColumns == 1)
            {
                var aw = new double[1, a.Length];
                for (var i = 0; i < a.Length; i++)
                    aw[0, i] = a[i];
                return aw;
            }

            if(!truncEnding && a.Length % numOfColumns != 0)
                throw new NonConformableException("the number of items in " +
                                                  $"the array {a.Length} will not fit evenly into a matrix");

            var numOfRows = (int)System.Math.Round(a.Length / (double) numOfColumns);
            var matrix = new double[numOfRows, numOfColumns];
            for (var i = 0; i < numOfRows; i++)
            {
                for (var j = 0; j < numOfColumns; j++)
                {
                    var vIdx = (i * numOfColumns) + j;
                    matrix[i, j] = a[vIdx];
                }
            }

            return matrix;
        }

        public static void ApplyToEach(this double[,] a, Func<double, double> expr)
        {
            a = a ?? new double[,] { };
            if (expr == null)
                return;
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                for (var j = 0; j < a.CountOfColumns(); j++)
                {
                    a[i, j] = expr(a[i, j]);
                }
            }
        }

        /// <summary>
        /// https://github.com/ronxin/wevi/blob/master/js/vector_math.js @ function mean(X)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double[] MeanByColumn(this double[,] a)
        {
            return a.CollapseTop2Bottom(row => row.Sum() / a.CountOfColumns());
        }

        /// <summary>
        /// https://github.com/ronxin/wevi/blob/master/js/vector_math.js @ function std(X)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double[] StdByColumn(this double[,] a)
        {
            var mul = MatrixOps.Arithmetic(a, a, (d, d1) => d * d1);
            return mul.MeanByColumn().Select(System.Math.Sqrt).ToArray();
        }

        /// <summary>
        /// https://github.com/ronxin/wevi/blob/master/js/vector_math.js @ function scale(x, center, scale)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="toCenter"></param>
        /// <param name="toScale"></param>
        /// <returns></returns>
        public static void Scale(this double[,] a, bool toCenter = true, bool toScale = true)
        {
            a = a ?? new double[,] { };
            if (toCenter)
            {
                var m = a.MeanByColumn();
                for (var i = 0; i < a.CountOfRows(); i++)
                {
                    a.ApplyAtRow(m, i, (d, d1) => d - d1);
                }
            }

            if (toScale)
            {
                var s = a.StdByColumn();
                for (var i = 0; i < a.CountOfRows(); i++)
                {
                    a.ApplyAtRow(s, i, (d, d1) => d / (d1 == 0D ? 0.0000001D : d1));
                }
            }
        }

        public struct SvdOutput
        {
            public double[,] U;
            public double[] D;
            public double[,] V;
        }

        /// <summary>
        /// https://github.com/ronxin/wevi/blob/master/js/vector_math.js @ function svd(A)
        /// which is a copy of
        /// http://stitchpanorama.sourceforge.net/Python/svd.py
        /// which is cited on 
        /// Compute the thin SVD from G. H. Golub and C. Reinsch, Numer. Math. 14, 403-420 (1970)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static SvdOutput SingularValueDecomp(this double[,] a)
        {
            a = a ?? new double[,] { };
            var temp = 0D;
            var prec = System.Math.Pow(2, -52);
            var tolerance = 1e-64 / prec;
            var itmax = 50D;
            var c = 0D;
            var i = 0;
            var j = 0;
            var k = 0;
            var l = 0;

            var u = a.Copy();
            var m = u.CountOfRows();
            var n = u.CountOfColumns();

            var e = new double[n];
            var q = new double[n];
            var v = new double[n, n];

            Func<double, double, double> pythag = (v1, v2) =>
            {
                v1 = System.Math.Abs(v1);
                v2 = System.Math.Abs(v2);
                if (v1 > v2)
                    return v1 * System.Math.Sqrt(1D + v2 * v2 / v1 / v1);
                if (v2 == 0D)
                    return v1;
                return v2 * System.Math.Sqrt(1D + v1 * v1 / v2 / v2);
            };

            // Householder's reduction to bidiagonal form
            var f = 0D;
            var g = 0D;
            var h = 0D;
            var x = 0D;
            var y = 0D;
            var z = 0D;
            var s = 0D;

            for (i = 0; i < n; i++)
            {
                e[i] = g;
                s = 0.0D;
                l = i + 1;
                for (j = i; j < m; j++)
                    s += (u[j,i] * u[j,i]);
                if (s <= tolerance)
                    g = 0.0;
                else
                {
                    f = u[i,i];
                    g = System.Math.Sqrt(s);
                    if (f >= 0.0) g = -g;
                    h = f * g - s;
                    u[i,i] = f - g;
                    for (j = l; j < n; j++)
                    {
                        s = 0.0;
                        for (k = i; k < m; k++)
                            s += u[k, i] * u[k, j];
                        f = s / h;
                        for (k = i; k < m; k++)
                            u[k, j] += f * u[k, i];
                    }
                }

                q[i] = g;
                s = 0.0;
                for (j = l; j < n; j++)
                    s = s + u[i, j] * u[i, j];
                if (s <= tolerance)
                    g = 0.0;
                else
                {
                    f = u[i,i + 1];
                    g = System.Math.Sqrt(s);
                    if (f >= 0.0) g = -g;
                    h = f * g - s;
                    u[i,i + 1] = f - g;
                    for (j = l; j < n; j++) e[j] = u[i,j] / h;
                    for (j = l; j < m; j++)
                    {
                        s = 0.0;
                        for (k = l; k < n; k++)
                            s += (u[j, k] * u[i, k]);
                        for (k = l; k < n; k++)
                            u[j, k] += s * e[k];
                    }
                }

                y = System.Math.Abs(q[i]) + System.Math.Abs(e[i]);
                if (y > x)
                    x = y;
            }

            // accumulation of right hand gtransformations
            for (i = (int)n - 1; i != -1; i += -1)
            {
                if (g != 0.0D)
                {
                    h = g * u[i, i + 1];
                    for (j = l; j < n; j++)
                        v[j, i] = u[i, j] / h;
                    for (j = l; j < n; j++)
                    {
                        s = 0.0;
                        for (k = l; k < n; k++)
                            s += u[i, k] * v[k, j];
                        for (k = l; k < n; k++)
                            v[k, j] += (s * v[k, i]);
                    }
                }
                for (j = l; j < n; j++)
                {
                    v[i,j] = 0;
                    v[j,i] = 0;
                }
                v[i,i] = 1;
                g = e[i];
                l = i;
            }

            // accumulation of left hand transformations
            for (i = (int)n - 1; i != -1; i += -1)
            {
                l = i + 1;
                g = q[i];
                for (j = l; j < n; j++)
                    u[i,j] = 0;
                if (g != 0.0)
                {
                    h = u[i, i] * g;
                    for (j = l; j < n; j++)
                    {
                        s = 0.0;
                        for (k = l; k < m; k++) s += u[k,i] * u[k,j];
                        f = s / h;
                        for (k = i; k < m; k++) u[k,j] += f * u[k,i];
                    }
                    for (j = i; j < m; j++) u[j,i] = u[j,i] / g;
                }
                else
                    for (j = i; j < m; j++) u[j,i] = 0;
                u[i,i] += 1;
            }

            // diagonalization of the bidiagonal form
            prec = prec * x;
            for (k = (int) n - 1; k != -1; k += -1)
            {
                for (var iteration = 0; iteration < itmax; iteration++)
                {
                    // test f splitting
                    var test_convergence = false;
                    for (l = k; l != -1; l += -1)
                    {
                        if (System.Math.Abs(e[l]) <= prec)
                        {
                            test_convergence = true;
                            break;
                        }

                        if (System.Math.Abs(q[l - 1]) <= prec)
                            break;
                    }

                    if (!test_convergence)
                    {
                        // cancellation of e[l] if l>0
                        c = 0.0;
                        s = 1.0;
                        var l1 = l - 1;
                        for (i = l; i < k + 1; i++)
                        {
                            f = s * e[i];
                            e[i] = c * e[i];
                            if (System.Math.Abs(f) <= prec)
                                break;
                            g = q[i];
                            h = pythag(f, g);
                            q[i] = h;
                            c = g / h;
                            s = -f / h;
                            for (j = 0; j < m; j++)
                            {
                                y = u[j, l1];
                                z = u[j, i];
                                u[j, l1] = y * c + (z * s);
                                u[j, i] = -y * s + (z * c);
                            }
                        }
                    }

                    // test f convergence
                    z = q[k];
                    if (l == k)
                    {
                        //convergence
                        if (z < 0.0)
                        {
                            //q[k] is made non-negative
                            q[k] = -z;
                            for (j = 0; j < n; j++)
                                v[j, k] = -v[j, k];
                        }

                        break; //break out of iteration loop and move on to next k value
                    }

                    //console.assert(iteration < itmax - 1, 'Error: no convergence.');

                    // shift from bottom 2x2 minor
                    x = q[l];
                    y = q[k - 1];
                    g = e[k - 1];
                    h = e[k];
                    f = ((y - z) * (y + z) + (g - h) * (g + h)) / (2.0 * h * y);
                    g = pythag(f, 1.0);
                    if (f < 0.0)
                        f = ((x - z) * (x + z) + h * (y / (f - g) - h)) / x;
                    else
                        f = ((x - z) * (x + z) + h * (y / (f + g) - h)) / x;
                    // next QR transformation
                    c = 1.0;
                    s = 1.0;
                    for (i = l + 1; i < k + 1; i++)
                    {
                        g = e[i];
                        y = q[i];
                        h = s * g;
                        g = c * g;
                        z = pythag(f, h);
                        e[i - 1] = z;
                        c = f / z;
                        s = h / z;
                        f = x * c + g * s;
                        g = -x * s + g * c;
                        h = y * s;
                        y = y * c;
                        for (j = 0; j < n; j++)
                        {
                            x = v[j, i - 1];
                            z = v[j, i];
                            v[j, i - 1] = x * c + z * s;
                            v[j, i] = -x * s + z * c;
                        }

                        z = pythag(f, h);
                        q[i - 1] = z;
                        c = f / z;
                        s = h / z;
                        f = c * g + s * y;
                        x = -s * g + c * y;
                        for (j = 0; j < m; j++)
                        {
                            y = u[j, i - 1];
                            z = u[j, i];
                            u[j, i - 1] = y * c + z * s;
                            u[j, i] = -y * s + z * c;
                        }
                    }

                    e[l] = 0.0;
                    e[k] = f;
                    q[k] = x;
                }
            }

            // vt = transpose(v)
            // return (u,q,vt)
            for (i = 0; i < q.Length; i++)
                if (q[i] < prec)
                    q[i] = 0;

            // sort eigenvalues
            for (i = 0; i < n; i++)
            {
                // writeln(q)
                for (j = i - 1; j >= 0; j--)
                {
                    if (q[j] < q[i])
                    {
                        // writeln(i,'-',j)
                        c = q[j];
                        q[j] = q[i];
                        q[i] = c;
                        for (k = 0; k < u.GetLength(0); k++)
                        {
                            temp = u[k,i];
                            u[k,i] = u[k,j];
                            u[k,j] = temp;
                        }
                        for (k = 0; k < v.GetLength(0); k++)
                        {
                            temp = v[k,i];
                            v[k,i] = v[k,j];
                            v[k,j] = temp;
                        }

                        i = j;
                    }
                }
            }

            return new SvdOutput {U = u, D = q, V = v};
        }

        /// <summary>
        /// Gets the upper or lower triangle of the given matrix <see cref="a"/> 
        /// where the split is along the diagonal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="upperRight"></param>
        /// <returns></returns>
        public static double[,] GetTriangle(this double[,] a, bool upperRight = true)
        {
            a = a ?? new double[,] { };
            var rowsLen = a.CountOfRows();
            var colLen = a.CountOfColumns();
            var vout = new double[rowsLen, colLen];
            Func<long, long, bool> contExpr = (l, l1) => l > l1;

            if (!upperRight)
            {
                contExpr = (l, l1) => l < l1;
            }

            for (var i = 0; i < rowsLen; i++)
            {
                for (var j = 0; j < colLen; j++)
                {
                    if (contExpr(i, j))
                        continue;
                    vout[i, j] = a[i, j];
                }
            }

            return vout;
        }

        /// <summary>
        /// Apply the Cocke-Kasami-Younger algo to the matrix.  This algo is a special way to &quot;walk&quot; a 
        /// matrix&apos;s upper-right triangle.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="expr">
        /// The first arg in <see cref="expr"/> is the one-to-the-left, second arg is the the one-below and the 
        /// third arg is the current position.
        /// </param>
        public static double[,] CockeKasamiYounger(this double[,] a, Func<double, double, double, double> expr = null)
        {
            a = a ?? new double[,] { };
            var rowsLen = a.CountOfRows();
            var colLen = a.CountOfColumns();
            var vout = a.GetTriangle();
            if (expr == null)
                return vout;
            for (var j = 2; j < a.CountOfRows() + 1; j++)
            {
                for (var i = j - 2; i > -1; i--)
                {
                    for (var k = i + 1; k < j; k++)
                    {
                        if (i >= rowsLen)
                            continue;
                        if (k - 1 >= colLen)
                            continue;
                        if (k >= rowsLen)
                            continue;
                        if (j - 1 >= colLen)
                            continue;

                        var toTheLeft = vout[i, k - 1];
                        var directBelow = vout[k, j - 1];
                        var currentPos = vout[i, j - 1];
                        var rslt = expr(toTheLeft, directBelow, currentPos);

                        vout[i, j - 1] = rslt;
                    }
                }
            }

            return vout;
        }

        public static double[,] Transpose(this double[,] a)
        {
            a = a ?? new double[,] { };
            var transpose = new double[a.CountOfColumns(), a.CountOfRows()];
            for (var i = 0L; i < a.CountOfRows(); i++)
            for (var j = 0L; j < a.CountOfColumns(); j++)
                transpose[j, i] = a[i, j];

            return transpose;
        }

        public static double[,] CrossProduct(this double[,] a)
        {
            a = a ?? new double[,] { };
            return a.Transpose().DotProduct(a);
        }

        public static double[,] Deviation(this double[,] a)
        {
            a = a ?? new double[,] { };
            var l = MatrixOps.GetAllOnesMatrix(a.CountOfRows(), 1L);
            var lTick = l.Transpose();
            var lOverRows = 1D/a.CountOfRows();

            var lxlTick = MatrixOps.Product(l, lTick);
            var lXa = MatrixOps.Product(lxlTick, a);
            var aXr = MatrixOps.Product(lXa, lOverRows);

            return MatrixOps.Difference(a, aXr);
        }

        public static double[,] Inverse(this double[,] a)
        {
            a = a ?? new double[,] { };
            var len = a.CountOfRows();
            var determinant = Determinant(a);
            if (determinant.Equals(0D))
                throw new NonConformableException("The given matrix is linear dependent.");

            var adjCofactor = Cofactor(a).Transpose();

            var inverse = new double[len, len];
            for (var i = 0L; i < len; i++)
            for (var j = 0L; j < len; j++)
                inverse[i, j] = adjCofactor[i, j] / determinant;

            return inverse;

        }

        public static bool IsLinearDependent(this double[,] a)
        {
            return (Determinant(a).Equals(0D));
        }

        public static double Determinant(this double[,] a)
        {
            a = a ?? new double[,] { };
            var rows = a.CountOfRows();
            var cols = a.CountOfColumns();
            if (rows != cols)
                throw new NonConformableException("A Determinant requires a square matrix (num-of-Rows = num-of-Columns).");

            if (rows == 1)
                return a[0, 0];

            var val = 0D;
            for (var j = 0L; j < cols; j++)
            {
                var aOj = a[0, j];
                if (aOj == 0D)
                    continue;
                var minor = a.SelectMinor(0, j);
                var v = rows - 1 == 3 ? Determinant3X3(minor,0) : Determinant(minor);
                v = aOj * v;
                if (j % 2 == 1)
                    v *= -1;
                val += v;
            }

            return val;

        }

        public static double[,] Cofactor(this double[,] a)
        {
            a = a ?? new double[,] { };
            var len = a.CountOfRows();
            var cols = a.CountOfColumns();
            if (len != cols)
                throw new NonConformableException("A Cofactor requires a square matrix (num-of-Rows = num-of-Columns).");

            //use multithread for non-trival size matrix
            if (len >= 8)
            {
                return new CofactorSupervisor(a).CalcCofactor();
            }

            var cofactor = new double[len, len];

            for (var i = 0L; i < len; i++)
            {
                for (var j = 0L; j < len; j++)
                {
                    var ic = a.SelectMinor(i, j);
                    var det = Determinant(ic);
                    if ((i + j) % 2 == 1)
                        det *= -1;
                    cofactor[i, j] = det;
                }
            }
            return cofactor;
        }

        public static double[,] Covariance(this double[,] a)
        {
            a = a ?? new double[,] { };
            var aDev = a.Deviation();
            var aDevXaDevTick = MatrixOps.Product(aDev.Transpose(), aDev);

            return MatrixOps.Product(aDevXaDevTick, (1D/a.CountOfRows()));
        }

        public static double[,] ProjectionMatrix(this double[,] a)
        {
            a = a ?? new double[,] { };
            var ata = a.CrossProduct();
            var ataInverse = ata.Inverse();
            return a.DotProduct(ataInverse).DotProduct(a.Transpose());
        }

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
            var isRstyle = new[] {"r"}.Any(v => string.Equals(v, style, StringComparison.OrdinalIgnoreCase));
            if (new[] { "cs", "c#" }.Any(v => string.Equals(style, v, StringComparison.OrdinalIgnoreCase)))
            {
                open = "{";
                close = "}";
                str.Append("new[,] ");
            }

            if (new[] {"ps", "powershell"}.Any(v => string.Equals(v, style, StringComparison.OrdinalIgnoreCase)))
            {
                open = "@(";
                close = ")";
            }

            if (new[] {"java"}.Any(v => string.Equals(v, style, StringComparison.OrdinalIgnoreCase)))
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
                    vals.Add(a[i,j].ToString(CultureInfo.InvariantCulture));
                }

                strLn.Append(string.Join(",", vals));
                strLn.Append(close);
                lines.Add(strLn.ToString());
            }

            str.Append(string.Join($",{Environment.NewLine}", lines));
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

            //find the max len
            var maxLen = 0;
            var anyNeg = false;
            for (var i = 0; i < a.CountOfRows(); i++)
            for (var j = 0; j < a.CountOfColumns(); j++)
            {
                if (a[i, j].ToString(CultureInfo.InvariantCulture).Length > maxLen)
                    maxLen = a[i, j].ToString(CultureInfo.InvariantCulture).Length;
                if (a[i, j] < 0)
                    anyNeg = true;
            }

            maxLen += 2;

            for (var i = 0; i < a.CountOfColumns(); i++)
            {
                var headerStr = $"[,{i}]";
                var padding = (maxLen);

                if (i == 0)
                {
                    str.Append(new String(' ', a.CountOfRows().ToString().Length + 2));
                }

                str.AppendFormat("{0," + padding + "}", headerStr);
            }

            str.AppendLine();
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                str.Append($"[{i},] ");
                for (var j = 0; j < a.CountOfColumns(); j++)
                {
                    var aij = a[i, j];
                    var format = aij >= 0 && anyNeg ? " {0,-" + (maxLen - 1) + "}" : "{0,-" + maxLen + "}";
                    str.AppendFormat(format, aij);
                }
                str.AppendLine();
            }

            return str.ToString();
        }

        public static double[,] GetSoftmax(this double[,] a)
        {
            a = a ?? new double[,] { };
            var mout = new double[a.CountOfRows(), a.CountOfColumns()];
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                var z = new List<double>();
                for (var j = 0; j < a.CountOfColumns(); j++)
                {
                    z.Add(a[i,j]);
                }

                var zExp = z.Select(m => System.Math.Pow(System.Math.E, m)).ToArray();
                var sumZExp = zExp.Sum();
                var softmaxAtI = zExp.Select(m => m / sumZExp).ToArray();

                for (var j = 0; j < mout.CountOfColumns(); j++)
                {
                    mout[i, j] = softmaxAtI[j];
                }
            }

            return mout;
        }

        public static double[] SelectRow(this double[,] a, long index)
        {
            a = a ?? new double[,] { };
            var select = new List<double>();
            if (index < 0)
                return select.ToArray();

            if (a.CountOfRows() <= index)
                return select.ToArray();
            for (var j = 0; j < a.CountOfColumns(); j++)
            {
                select.Add(a[index, j]);
            }

            return select.ToArray();

        }

        public static double[] SelectColumn(this double[,] a, long index)
        {
            a = a ?? new double[,] { };
            var select = new List<double>();
            if (index < 0)
                return select.ToArray();

            if (a.CountOfColumns() <= index)
                return select.ToArray();
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                select.Add(a[i,index]);
            }
            return select.ToArray();
        }

        /// <summary>
        /// Get the matrix less the row at <see cref="rowIndex"/> and less the column at <see cref="columnIndex"/>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <remarks>
        /// Chiang, Alpha C. @ page 95
        /// </remarks>
        public static double[,] SelectMinor(this double[,] a, long rowIndex, long columnIndex)
        {
            a = a ?? new double[,] { };
            var rows = a.CountOfRows();
            var columns = a.CountOfColumns();
            if (rowIndex > rows - 1 || columnIndex > columns - 1)
                return a;

            var vout = new double[rows - 1, columns - 1];
            for (var i = 0; i < rows; i++)
            {
                if(i == rowIndex)
                    continue;
                var vi = i > rowIndex ? i - 1 : i;
                for (var j = 0; j < columns; j++)
                {
                    if(j == columnIndex)
                        continue;
                    var vj = j > columnIndex ? j - 1 : j;
                    vout[vi, vj] = a[i, j];

                }
            }
            return vout;
        }

        public static double[,] Copy(this double[,] a)
        {
            a = a ?? new double[,] { };
            var dest = new double[a.CountOfRows(), a.CountOfColumns()];
            for (var i = 0; i < dest.CountOfRows(); i++)
            {
                for (var j = 0; j < dest.CountOfColumns(); j++)
                {
                    dest[i, j] = a[i, j];
                }
            }

            return dest;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        internal static bool IsSqrBy(long dim, double[,] a)
        {
            a = a ?? new double[,] { };
            return a.CountOfRows() == a.CountOfColumns() && a.CountOfRows() == dim;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private static Func<double[,], double, double> _ex;

        /// <summary>
        /// Determinant runs a little faster with this.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        internal static Func<double[,], double, double> Determinant3X3
        {
            get
            {
                if (_ex != null)
                    return _ex;
                var expr = MatrixExpressions.EigenvalueExpression(3);
                _ex = expr.Compile();
                return _ex;
            }
        }
    }
}