using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NoFuture.Util.Core.Math.Matrix
{
    public class MatrixExpressions
    {

        /// <summary>
        /// This expression is used calculate a Determinant by simply passing 
        /// in '0' for the second parameter of the compiled expression.
        /// It can also be used to test for eigen values based on this same second parameter 
        /// being anything other than '0'.
        /// </summary>
        /// <param name="is3x3">
        /// Only works for 3X3 and 2X2 since anything greater than 3X3 needs Laplace expansion.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Is one that simply gets "scaled up" by some value (the eigen value).
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
        public static Expression<Func<double[,], double, double>> DeterminantExpression(bool is3x3 = true)
        {
            var myArray = Expression.Parameter(typeof(double[,]), "a");
            var myParam = Expression.Parameter(typeof (double), "r");
            var zeroIdx = Expression.Constant(0);
            var oneIdx = Expression.Constant(1);
            var len = is3x3 ? 3 : 2;
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

        /// <summary>
        /// The expression is used to calculate Gauss Elimination. The resulting expression
        /// allows for the results to be considered in a kind-of algebraic form.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        /// var input = new double[,]
        /// {
        ///     { 1,2,3 },
        ///     { 4,5,6 },
        ///     { 1,0,1 }
        /// };
        ///
        /// var gaussExpr = MatrixExpressions.GaussEliminationExpression(input);
        /// //to see the actual algebra
        /// Console.WriteLine(gaussExpr.ToString())
        /// 
        /// var gaussFx = gaussExpr.Compile();
        /// var results = gaussFx(new double[] { 1, 1, 1 });
        /// ]]>
        /// </example>
        public static Expression<Func<double[], double[]>> GaussEliminationExpression(double[,] a)
        {
            var n = a.GetLongLength(0);
            var cols = a.GetLongLength(1);

            var AExpr = new Expression[n, cols + 1];
            var vExpr = Expression.Parameter(typeof(double[]), "v");

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < a.GetLongLength(1); j++)
                {
                    AExpr[i, j] = Expression.Constant(a[i, j], typeof(double));
                }

                AExpr[i,cols] = Expression.ArrayAccess(vExpr, Expression.Constant(i));
            }

            for (var i = 0; i < n; i++)
            {
                // Search for maximum in this column
                var maxEl = a[i, i] < 0 ? -1 * a[i, i] : a[i, i];

                var maxRow = i;
                for (var k = i + 1; k < n; k++)
                {
                    var aikAbs = a[k, i] < 0 ? -1 * a[k, i] : a[k, i];
                    if (aikAbs > maxEl)
                    {
                        maxEl = aikAbs;
                        maxRow = k;
                    }
                }

                // Swap maximum row with current row (column by column)
                for (var j = 0; j < a.GetLongLength(1)+1; j++)
                {
                    var row1JExpr = AExpr[i, j];
                    var row2JExpr = AExpr[maxRow, j];
                    AExpr[i, j] = row2JExpr;
                    AExpr[maxRow, j] = row1JExpr;
                }


                // Make all rows below this one 0 in current column
                for (var k = i + 1; k < n; k++)
                {
                    var negAki = Expression.Multiply(Expression.Constant(-1D), AExpr[k, i]);
                    var cExprDiv = Expression.Divide(negAki, AExpr[i, i]);
                    for (var j = i; j < n + 1; j++)
                    {
                        if (i == j)
                        {
                            AExpr[k, j] = Expression.Constant(0D);
                        }
                        else
                        {
                            var akj = AExpr[k, j];
                            var aij = AExpr[i, j];
                            var cTimesAij = Expression.Multiply(cExprDiv, aij);
                            var plusAik = Expression.Add(akj, cTimesAij);
                            AExpr[k, j] = plusAik;
                        }
                    }
                }
            }

            // Solve equation Ax=b for an upper triangular matrix A
            var xExpr = new Expression[n];
            for (var i = n - 1; i >= 0; i--)
            {
                var cExprDiv = Expression.Divide(AExpr[i, n], AExpr[i, i]);
                xExpr[i] = cExprDiv;
                for (var k = i - 1; k >= 0; k--)
                {
                    var akn = AExpr[k, n];
                    var aki = AExpr[k, i];
                    var cTimesAki = Expression.Multiply(aki, cExprDiv);
                    var aknMinusCtimesAki = Expression.Subtract(akn, cTimesAki);
                    AExpr[k, n] = aknMinusCtimesAki;
                }
            }

            var xExprExpr = Expression.NewArrayInit(typeof(double), xExpr);
            return Expression.Lambda<Func<double[], double[]>>(xExprExpr, vExpr);
        }
    }
}