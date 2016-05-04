using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace NoFuture.Util.Math
{
    public class NonConformable : System.Exception
    {
        public NonConformable()
            : base()
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
            return Arithmetic(a, b, false);
        }//end Sum

        public static double[,] Difference(double[,] a, double[,] b)
        {
            return Arithmetic(a, b, true);
        }//end Difference

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        private static double[,] Arithmetic(double[,] a, double[,] b, bool asDiff)
        {

            if (a.GetLongLength(0) != b.GetLongLength(0)) //equal rows
                throw new NonConformable(
                    string.Format(
                        "The number of rows in matrix 'a' must match the number of rows in matrix 'b' to solve for the {0}.",
                        asDiff ? "difference" : "sum"));
            if (a.GetLongLength(1) != b.GetLongLength(1)) //equal columns
                throw new NonConformable(
                    string.Format(
                        "The number of columns in matrix 'a' must match the number of columns in matrix 'b' to solve for the {0}.",
                        asDiff ? "difference" : "sum"));

            var iLength = a.GetLongLength(0);
            var jLength = a.GetLongLength(1);

            var sum = new double[iLength,jLength];

            for (var i = 0L; i < iLength; i++)
            {
                for (var j = 0L; j < jLength; j++)
                {
                    if(asDiff)
                        sum[i,j] = a[i,j] - b[i,j];
                    else
                        sum[i,j] = a[i,j] + b[i,j];
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
            var dimensions = new[,] { { a.GetLongLength(0), a.GetLongLength(1) }, { b.GetLongLength(0), b.GetLongLength(1) } };
            var m = dimensions[0, 0];
            var n = dimensions[0, 1];
            var p = dimensions[1, 0];
            var q = dimensions[1, 1];

            if (n != p) //equal rows
                throw new NonConformable(
                    "The number of columns in matrix 'a' must match the number of rows in matrix 'b' in order to solve for the product of the two.");

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
            for (var i = 0; i < s.Rows(); i++)
            {
                for (var j = 0; j < s.Columns(); j++)
                    s[i, j] = 1D;
            }
            return s;
        }//end GetAllOnesMatrix

        public static bool AreEqual(double[,] a, double[,] b)
        {
            if(a.Rows() != b.Rows() || a.Columns()!=b.Columns())
                throw new NonConformable(
                    "The diminsions of a and b must be the same.");
            for (var i = 0; i < a.Rows(); i++)
            {
                for (var j = 0; j < a.Columns(); j++)
                {
                    if (System.Math.Abs(a[i, j] - b[i, j]) > 0.000000D)
                        return false;

                }
            }
            return true;
        }//end AreEqual
    }

    public static class MatrixExtensions
    {
        public static long Rows(this double[,] a)
        {
            return a.GetLongLength(0);
        }//end Rows

        public static long Columns(this double[,] a)
        {
            return a.GetLongLength(1);
        }//end Columns

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
            var l = Matrix.GetAllOnesMatrix(a.Rows(), 1L);
            var lTick = l.Transpose();
            var lOverRows = 1D/a.Rows();

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

            return Matrix.Product(aDevXaDevTick, (1D/a.Rows()));
        }//end Covariance

        public static string Print(this double[,] a)
        {
            var str = new StringBuilder();

            //find the max len
            var maxLen = 0;
            for (var i = 0; i < a.Rows(); i++)
                for (var j = 0; j < a.Columns(); j++)
                    if (a[i, j].ToString(CultureInfo.InvariantCulture).Length > maxLen)
                        maxLen = a[i, j].ToString(CultureInfo.InvariantCulture).Length;

            maxLen += 2;

            for (var i = 0; i < a.Rows(); i++)
            {
                for (var j = 0; j < a.Columns(); j++)
                {
                    str.AppendFormat(("{0,-" + maxLen + "}"), a[i, j]);
                }
                str.AppendLine();
            }


            return str.ToString();
        }//end Print

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
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
                var a00 = Expression.ArrayAccess(myArray, new Expression[] { zeroIdx, zeroIdx });
                var a11 = Expression.ArrayAccess(myArray, new Expression[] { oneIdx, oneIdx });
                var a01 = Expression.ArrayAccess(myArray, new Expression[] { zeroIdx, oneIdx });
                var a10 = Expression.ArrayAccess(myArray, new Expression[] { oneIdx, zeroIdx });

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

                    var aIjExpr = Expression.ArrayAccess(myArray, new Expression[] { iExpr, jExpr });
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
                    var aIjExpr = Expression.ArrayAccess(myArray, new Expression[] { iExpr, jExpr });
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
