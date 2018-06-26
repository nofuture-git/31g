using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NoFuture.Util.Core.Math
{
    public class NonConformable : System.Exception
    {
        public NonConformable()
        {

        }
        public NonConformable(string message)
            : base(message)
        {

        }

    }
    /// <summary>
    /// Chiang, Alpha C. "Fundamental Methods Of Mathematical Economics" New York: McGraw-Hill, Inc.
    ///     1984. Print
    /// </summary>
    public class Matrix
    {
        public static double[,] Sum(double[,] a, double[,] b)
        {
            return Arithmetic(a, b, (d1, d2) => d1 + d2);
        }//end Sum

        public static double[,] Difference(double[,] a, double[,] b)
        {
            return Arithmetic(a, b, (d1, d2) => d1 - d2);
        }//end Difference

        public static double[,] Arithmetic(double[,] a, double[,] b, Func<double, double, double> expr)
        {
            expr = expr ?? ((d1, d2) => d1 + d2);
            if (a.GetLongLength(0) != b.GetLongLength(0)) //equal rows
                throw new NonConformable(
                    $"The number of rows in matrix 'a' must match the number of rows in matrix 'b' " +
                    $"to solve for the {expr}.");
            if (a.GetLongLength(1) != b.GetLongLength(1)) //equal columns
                throw new NonConformable(
                    $"The number of columns in matrix 'a' must match the number of columns in matrix 'b' " +
                    $"to solve for the {expr}.");

            var iLength = a.GetLongLength(0);
            var jLength = a.GetLongLength(1);

            var sum = new double[iLength,jLength];

            for (var i = 0L; i < iLength; i++)
            {
                for (var j = 0L; j < jLength; j++)
                {
                    sum[i, j] = expr(a[i, j], b[i, j]);
                }
            }
            return sum;
        }//end Arthmetic

        public static double[,] Product(double[,] a, double scalar)
        {
            var iLength = a.GetLongLength(0);
            var jLength = a.GetLongLength(1);
            for (var i = 0L; i < iLength; i++)
            {
                for(var j=0L;j<jLength;j++)
                {
                    a[i,j] = a[i,j] * scalar;
                }
            }
            return a;

        }//end Product

        public static double[,] Product(double[,] a, double[,] b)
        {
            var dimensions = new[,] {{a.GetLongLength(0), a.GetLongLength(1)}, {b.GetLongLength(0), b.GetLongLength(1)}};
            var m = dimensions[0, 0];
            var n = dimensions[0, 1];
            var p = dimensions[1, 0];
            var q = dimensions[1, 1];

            if (n != p) //equal rows
                throw new NonConformable(
                    "The number of columns in matrix 'a' must match the number of rows in matrix 'b' " +
                    "in order to solve for the product of the two.");

            var product = new double[m,q];
            for (var productRows = 0L; productRows < m; productRows++)
                for (var productColumns = 0L; productColumns < q; productColumns++)
                    for (var i = 0L; i < n; i++)
                        product[productRows, productColumns] += a[productRows, i] * b[i, productColumns];

            return product;

        }//end Product

        public static double[,] GetIdentity(long sqrSize)
        {
            var identity = new double[sqrSize,sqrSize];
            for (var i = 0L; i < sqrSize; i++)
                identity[i, i] = 1L;

            return identity;
        }//end GetIdentity

        public static double[,] GetNull(long rows, long columns)
        {
            return new double[rows,columns];
        }//end GetNull

        public static double[,] GetAllOnesMatrix(long rows, long columns)
        {
            var s = new double[rows, columns];
            for (var i = 0; i < s.CountOfRows(); i++)
            {
                for (var j = 0; j < s.CountOfColumns(); j++)
                    s[i, j] = 1D;
            }
            return s;
        }//end GetAllOnesMatrix

        public static bool AreEqual(double[,] a, double[,] b)
        {
            if(a.CountOfRows() != b.CountOfRows() || a.CountOfColumns()!=b.CountOfColumns())
                throw new NonConformable(
                    "The diminsions of a and b must be the same.");
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                for (var j = 0; j < a.CountOfColumns(); j++)
                {
                    if (System.Math.Abs(a[i, j] - b[i, j]) > 0.0000001D)
                        return false;

                }
            }
            return true;
        }//end AreEqual

        /// <summary>
        /// Returns a matrix of 1 X <see cref="length"/> where all
        /// values are 0 expect for one at <see cref="index"/>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static double[,] OneHotVector(int index, int length)
        {
            if(index >= length)
                throw new ArgumentException("index must be less than the length");
            var m = new double[1, length];
            for (var j = 0; j < length; j++)
            {
                m[0, j] = j == index ? 1 : 0;
            }

            return m;
        }

        /// <summary>
        /// Returns a matrix of dimension size <see cref="numOfRows"/> by <see cref="numOfColumns"/>
        /// having all random, less-than 1, double values.
        /// </summary>
        /// <param name="numOfRows"></param>
        /// <param name="numOfColumns"></param>
        /// <returns></returns>
        public static double[,] RandomMatrix(int numOfRows, int numOfColumns)
        {
            var myRand = new Random(Convert.ToInt32($"{DateTime.Now:ffffff}"));
            numOfRows = numOfRows <= 0 ? myRand.Next(4, 8) : numOfRows;
            numOfColumns = numOfColumns <= 0 ? myRand.Next(4, 8) : numOfColumns;
            var m = new double[numOfRows, numOfColumns];
            for (var i = 0; i < numOfRows; i++)
            {
                for (var j = 0; j < numOfColumns; j++)
                {
                    m[i, j] = myRand.NextDouble();
                }
            }

            return m;
        }
    }

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

        public static double[,] Add(this double[,] a, double[,] b)
        {
            return Matrix.Sum(a, b);
        }

        public static double[,] Subtract(this double[,] a, double[,] b)
        {
            return Matrix.Difference(a, b);
        }

        public static double[,] DotProduct(this double[,] a, double[,] b)
        {
            return Matrix.Product(a, b);
        }

        public static double[,] ApplyAtRow(this double[,] a, double[] b, int atRow, Func<double, double, double> expr)
        {
            if(atRow >= a.CountOfRows())
                throw new NonConformable($"{atRow} exceeds the number of rows present in a");
            if (expr == null)
                return a;
            for (var j = 0; j < a.CountOfColumns(); j++)
            {
                var v = b.Length <= j ? 0 : b[j];
                a[atRow, j] = expr(a[atRow, j], v);
            }

            return a;
        }

        public static double[,] ApplyAtColumn(this double[,] a, double[] b, int atColumn, Func<double, double, double> expr)
        {
            if (atColumn >= a.CountOfColumns())
                throw new NonConformable($"{atColumn} exceeds the number of columns present in a");
            if (expr == null)
                return a;
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                var v = b.Length <= i ? 0 : b[i];
                a[i, atColumn] = expr(a[i, atColumn], v);
            }

            return a;
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
            var dout = new double[a.CountOfRows()];

            for (var i = 0; i < dout.Length; i++)
            {
                var rowI = a.SelectRow(i);
                dout[i] = expr(rowI);
            }

            return dout;
        }

        public static double[,] ToMatrix(this double[] a, int numOfColumns = 1, bool truncEnding = false)
        {
            if (a == null || !a.Any() || numOfColumns <= 0)
                return new double[,] { };
            if (numOfColumns == 1)
            {
                var aw = new double[1, a.Length];
                for (var i = 0; i < a.Length; i++)
                    aw[0, i] = a[i];
                return aw;
            }

            if(!truncEnding && a.Length % numOfColumns != 0)
                throw new NonConformable("the number of items in " +
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

        public static double[,] ApplyToEach(this double[,] a, Func<double, double> expr)
        {
            if (expr == null)
                return a;
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                for (var j = 0; j < a.CountOfColumns(); j++)
                {
                    a[i, j] = expr(a[i, j]);
                }
            }

            return a;
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
            var m = a.MeanByColumn();
            var mul = Matrix.Arithmetic(a, a, (d, d1) => d * d1);
            return mul.MeanByColumn().Select(System.Math.Sqrt).ToArray();
        }

        /// <summary>
        /// https://github.com/ronxin/wevi/blob/master/js/vector_math.js @ function scale(x, center, scale)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="toCenter"></param>
        /// <param name="toScale"></param>
        /// <returns></returns>
        public static double[,] Scale(this double[,] a, bool toCenter = true, bool toScale = true)
        {
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

            return a;
        }

        public static double[,] Transpose(this double[,] a)
        {
            var transpose = new double[a.GetLongLength(1), a.GetLongLength(0)];
            for (var i = 0L; i < a.GetLongLength(0); i++)
                for (var j = 0L; j < a.GetLongLength(1); j++)
                    transpose[j, i] = a[i, j];

            return transpose;
        }//end Transpose

        public static double[,] Deviation(this double[,] a)
        {
            var l = Matrix.GetAllOnesMatrix(a.CountOfRows(), 1L);
            var lTick = l.Transpose();
            var lOverRows = 1D/a.CountOfRows();

            var lxlTick = Matrix.Product(l, lTick);
            var lXa = Matrix.Product(lxlTick, a);
            var aXr = Matrix.Product(lXa, lOverRows);

            return Matrix.Difference(a, aXr);
        }//end Deviation

        public static double[,] Inverse(this double[,] a)
        {
            var determinant = Determinant(a);
            var len = a.GetLongLength(0);
            if (determinant.Equals(0D))
                throw new NonConformable("The given matrix is linear dependent.");

            var cofactor = Cofactor(a);
            var adjCofactor = cofactor.Transpose();

            var inverse = new double[len, len];
            for (var i = 0L; i < len; i++)
                for (var j = 0L; j < len; j++)
                    inverse[i, j] = ((double)adjCofactor[i, j] / (double)determinant);

            return inverse;

        }//end Inverse

        public static bool IsLinearDependent(this double[,] a)
        {
            return (Determinant(a).Equals(0D));
        }//end IsLinearDependent

        public static double Determinant(this double[,] a)
        {
            if (a.GetLongLength(0) != a.GetLongLength(1))
                throw new NonConformable("A Determinant requires a square matrix (num-of-Rows = num-of-Columns).");

            var iLen = a.GetLength(0);

            var eigenExpr = MatrixExpressions.EigenvalueExpression(iLen);
            var determinantFunc = eigenExpr.Compile();

            return determinantFunc(a, 0);
        }//end Determinant

        public static double[,] Cofactor(this double[,] a)
        {
            if (a.GetLongLength(0) != a.GetLongLength(1))
                throw new NonConformable("A Cofactor requires a square matrix (num-of-Rows = num-of-Columns).");

            var len = a.GetLongLength(0);
            var cofactor = new double[len, len];

            for (var i = 0L; i < len; i++)
            {
                var iIndices = GetApplicableCofactorIndices(i, len);
                for (var j = 0L; j < len; j++)
                {
                    var jIndices = GetApplicableCofactorIndices(j, len);

                    var ic = new double[len - 1, len - 1];
                    for (var k = 0L; k < iIndices.LongLength; k++)
                    {
                        for (var l = 0L; l < jIndices.LongLength; l++)
                        {
                            ic[k, l] = a[iIndices[k], jIndices[l]];
                        }

                    }
                    if ((i + j) % 2 == 1)
                        cofactor[i, j] = Determinant(ic) * -1;
                    else
                        cofactor[i, j] = Determinant(ic);
                }
            }
            return cofactor;

        }//end Cofactor

        public static double[,] Covariance(this double[,] a)
        {
            var aDev = a.Deviation();
            var aDevXaDevTick = Matrix.Product(aDev.Transpose(), aDev);

            return Matrix.Product(aDevXaDevTick, (1D/a.CountOfRows()));
        }//end Covariance

        public static string Print(this double[,] a)
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
        }//end Print

        public static double[,] GetSoftmax(this double[,] matrix)
        {
            var mout = new double[matrix.CountOfRows(), matrix.CountOfColumns()];
            for (var i = 0; i < matrix.GetLongLength(0); i++)
            {
                var z = new List<double>();
                for (var j = 0; j < matrix.GetLongLength(1); j++)
                {
                    z.Add(matrix[i,j]);
                }

                var zExp = z.Select(m => System.Math.Pow(System.Math.E, m)).ToArray();
                var sumZExp = zExp.Sum();
                var softmaxAtI = zExp.Select(m => System.Math.Round(m / sumZExp, 5)).ToArray();

                for (var j = 0; j < mout.GetLongLength(1); j++)
                {
                    mout[i, j] = softmaxAtI[j];
                }
            }

            return mout;
        }//GetSoftmax

        public static double[] SelectRow(this double[,] matrix, int index)
        {
            var select = new List<double>();
            if (index < 0)
                return select.ToArray();

            if (matrix.GetLongLength(0) <= index)
                return select.ToArray();
            for (var j = 0; j < matrix.GetLongLength(1); j++)
            {
                select.Add(matrix[index, j]);
            }

            return select.ToArray();

        }//SelectRow

        public static double[] SelectColumn(this double[,] matrix, int index)
        {
            var select = new List<double>();
            if (index < 0)
                return select.ToArray();

            if (matrix.GetLongLength(1) <= index)
                return select.ToArray();
            for (var i = 0; i < matrix.GetLongLength(0); i++)
            {
                select.Add(matrix[i,index]);
            }
            return select.ToArray();
        }//SelectColumn

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private static long[] GetApplicableCofactorIndices(long currentIndex, long originalLength)
        {
            if (originalLength == 0)
                return null;

            if (currentIndex >= originalLength)
                return null;

            var applicableIndices = new long[originalLength - 1];
            var runningCount = 0;
            for (var i = 0L; i < originalLength; i++)
            {
                if (i == currentIndex)
                    continue;

                applicableIndices[runningCount] = i;
                runningCount += 1;
            }
            return applicableIndices;
        }//end GetApplicableCofactorIndices
    }

    public class MatrixExpressions
    {
        /// <summary>
        /// This expression may be used to discover Eigenvalues.  Also it may be used as a Determinant 
        /// Expression by simply passing in '0' for the second parameter of the compiled expression.
        /// </summary>
        /// <param name="len">Determinants and eigenvalues can only be calculated on square matrices.</param>
        /// <returns></returns>
        /// <remarks>
        /// Is pronounced "I-G-in" vector - is one that simply gets "scaled up" by some value (the eigen value).
        /// Dx = rx -> (D -rI)x = 0
        /// 
        /// λ is the eigen value, v` is the eigen vector
        /// T(v`) = Av` = λv`
        /// </remarks>
        /// <example>
        /// <![CDATA[
        ///     var typicalMatrix = new[,]
        ///     {
        ///         {2D, 2D},
        ///         {2D, -1D}
        ///     };
        ///     var eigenvalueExpression = MatrixExpressions.EigenvalueExpression(2);
        ///     var eigenvalueFunc = eigenvalueExpression.Compile();
        /// 
        ///     //discover the eigenvalues by solving the f(x) = 0 
        ///     var myEigenvalues = new List<double>();
        ///     for (var i = 10; i >= -10; i--)
        ///     {
        ///          if (eigenvalueFunc(typicalMatrix, i).Equals(0D))
        ///             myEigenvalues.Add(i); //will contain '3' & '-2'
        ///     }
        /// ]]>
        /// </example>
        public static Expression<Func<double[,], double, double>> EigenvalueExpression(int len)
        {
            var myArray = Expression.Parameter(typeof(double[,]), "a");
            var myParam = Expression.Parameter(typeof (double), "r");
            var zeroIdx = Expression.Constant(0);
            var oneIdx = Expression.Constant(1);

            //(a[0, 0] * a[1, 1]) - (a[0, 1] * a[1, 0])
            if (len == 2)
            {
                var a00 = Expression.ArrayAccess(myArray, zeroIdx, zeroIdx);
                var a11 = Expression.ArrayAccess(myArray, oneIdx, oneIdx);
                var a01 = Expression.ArrayAccess(myArray, zeroIdx, oneIdx);
                var a10 = Expression.ArrayAccess(myArray, oneIdx, zeroIdx);

                var a00Xa11 = Expression.Multiply(Expression.Subtract(a00,myParam), Expression.Subtract(a11, myParam));
                var a01Xa10 = Expression.Multiply(a01, a10);

                var diff = Expression.Subtract(a00Xa11, a01Xa10);
                return Expression.Lambda<Func<double[,], double, double>>(diff, myArray, myParam);
            }

            var d = new ConstantExpression[len, len, 2];

            //get positive cross-diagonial indices
            for (var i = 0; i < len; i++)
            {
                for (var j = 0; j < len; j++)
                {
                    var left = i;
                    var right = (j + i) % len;
                    d[i, j, 0] = Expression.Constant(left);
                    d[i, j, 1] = Expression.Constant(right);
                }
            }

            BinaryExpression addDeterminantExpr = null;

            //get product of positive diagonials and add to sum
            for (var j = 0; j < len; j++)
            {
                BinaryExpression productExpr = null;
                for (var i = 0; i < len; i++)
                {
                    var iExpr = d[i, j, 0];
                    var jExpr = d[i, j, 1];

                    var aIjExpr = Expression.ArrayAccess(myArray, iExpr, jExpr);
                    if (iExpr.Value.Equals(jExpr.Value))
                    {
                        productExpr = productExpr == null
                            ? Expression.Multiply(Expression.Constant(1D), Expression.Subtract(aIjExpr, myParam))
                            : Expression.Multiply(productExpr, Expression.Subtract(aIjExpr, myParam));
                    }
                    else
                    {
                        productExpr = productExpr == null
                            ? Expression.Multiply(Expression.Constant(1D), aIjExpr)
                            : Expression.Multiply(productExpr, aIjExpr);
                    }
                }

                addDeterminantExpr = addDeterminantExpr == null
                    ? productExpr
                    : Expression.Add(addDeterminantExpr, productExpr);
            }

            //get negative cross-diagonial indices
            for (var i = len; i > 0; i--)
            {
                for (var j = (len - i); j < (len + (len - i)); j++)
                {
                    var left = Expression.Constant(i - 1);
                    var right = Expression.Constant(j % len);
                    var diIndex = (len - i);
                    var djIndex = System.Math.Abs(len - i - j);
                    d[diIndex, djIndex, 0] = left;
                    d[diIndex, djIndex, 1] = right;
                }
            }

            BinaryExpression minusDeterminantExpr = null;

            //get product of negative diagonials and subtract from sum
            for (var j = 0; j < len; j++)
            {
                BinaryExpression productExpr = null;
                for (var i = 0; i < len; i++)
                {
                    var iExpr = d[i, j, 0];
                    var jExpr = d[i, j, 1];
                    var aIjExpr = Expression.ArrayAccess(myArray, iExpr, jExpr);
                    if (iExpr.Value.Equals(jExpr.Value))
                    {
                        productExpr = productExpr == null
                            ? Expression.Multiply(Expression.Constant(1D), Expression.Subtract(aIjExpr, myParam))
                            : Expression.Multiply(productExpr, Expression.Subtract(aIjExpr, myParam));
                    }
                    else
                    {
                        productExpr = productExpr == null
                            ? Expression.Multiply(Expression.Constant(1D), aIjExpr)
                            : Expression.Multiply(productExpr, aIjExpr);
                    }
                }
                minusDeterminantExpr = minusDeterminantExpr == null
                    ? productExpr
                    : Expression.Add(minusDeterminantExpr, productExpr);
            }

            var newdiff = Expression.Subtract(addDeterminantExpr, minusDeterminantExpr);

            return Expression.Lambda<Func<double[,], double, double>>(newdiff, myArray, myParam);
        }
    }
}
