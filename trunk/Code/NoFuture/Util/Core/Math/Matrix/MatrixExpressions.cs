using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Here 'r' is the eigen value we want.
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
        /// <param name="allowStringCompare">
        /// Causes the resulting expressions to be much more concise.
        /// </param>
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
        public static Expression<Func<double[], double[]>> GaussEliminationExpression(double[,] a, bool allowStringCompare = true)
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
                if (i >= cols)
                    continue;

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
                    var aki = AExpr[k, i];
                    var aii = AExpr[i, i];
                    var cExprDiv = GetDivideExpression(Expression.Negate(aki), aii, allowStringCompare);

                    for (var j = i; j < cols + 1; j++)
                    {
                        if (i == j)
                        {
                            AExpr[k, j] = Expression.Constant(0D);
                        }
                        else
                        {
                            var akj = AExpr[k, j];
                            var aij = AExpr[i, j];

                            var isAijConst = TryGetDblConstValue(aij, out var aijVal);
                            var isAkjConst = TryGetDblConstValue(akj, out var akjVal);
                            var isCexprDivConst = TryGetDblConstValue(cExprDiv, out var cExprDivVal);
                            Expression plusAik;

                            if (isAkjConst && isAijConst && isCexprDivConst)
                            {
                                var cTimesAijVal = cExprDivVal * aijVal;
                                var plusAikVal = akjVal + cTimesAijVal;
                                plusAik = Expression.Constant(plusAikVal);
                            }
                            else
                            {
                                var cTimesAij = GetMultipleExpression(cExprDiv, aij, allowStringCompare);
                                plusAik = GetAddSubtractExpression(ExpressionType.Add, akj, cTimesAij,
                                    allowStringCompare);
                            }
                            AExpr[k, j] = plusAik;
                        }
                    }
                }
            }

            // Solve equation Ax=b for an upper triangular matrix A
            var xExpr = new Expression[n];
            for (var i = n - 1; i >= 0; i--)
            {
                var lastIdx = AExpr.GetLongLength(1) - 1;
                var ain = AExpr[i, lastIdx];
                var aii = AExpr[i, i];

                var cExprDiv = GetDivideExpression(ain, aii, allowStringCompare);

                xExpr[i] = cExprDiv;
                for (var k = i - 1; k >= 0; k--)
                {
                    var akn = AExpr[k, lastIdx];
                    var aki = AExpr[k, i];

                    var isAkiConst = TryGetDblConstValue(aki, out var akiVal);
                    var isCexprDivConst = TryGetDblConstValue(cExprDiv, out var cExprDivVal);
                    var isAknConst = TryGetDblConstValue(akn, out var aknVal);

                    Expression cTimesAki;
                    if (isAkiConst && isCexprDivConst)
                    {
                        cTimesAki = Expression.Constant(akiVal * cExprDivVal);
                    }
                    else
                    {
                        cTimesAki = GetMultipleExpression(aki, cExprDiv, allowStringCompare);
                    }

                    Expression aknMinusCtimesAki;

                    if (isAknConst && TryGetDblConstValue(cTimesAki, out var cTimesAkiVal))
                    {
                        aknMinusCtimesAki = Expression.Constant(aknVal - cTimesAkiVal);
                    }
                    else
                    {
                        aknMinusCtimesAki = GetAddSubtractExpression(ExpressionType.Subtract, akn, cTimesAki,
                            allowStringCompare);
                    }
                    
                    AExpr[k, lastIdx] = aknMinusCtimesAki;
                }
            }

            var xExprExpr = Expression.NewArrayInit(typeof(double), xExpr);
            var guassExpr = Expression.Lambda<Func<double[], double[]>>(xExprExpr, vExpr);
            return guassExpr;
        }

        internal static Expression GetMultipleExpression(Expression left, Expression right, bool allowStringCompare = true)
        {
            left = left ?? Expression.Constant(0D);
            right = right ?? Expression.Constant(0D);

            var isLeftConst = TryGetDblConstValue(left, out var leftVal);
            var isRightConst = TryGetDblConstValue(right, out var rightVal);

            //nothing is a variable so just reduce
            if (isLeftConst && isRightConst)
                return Expression.Constant(leftVal * rightVal);

            //distribute left const value to each side of right expression
            if (isLeftConst)
            {
                if (leftVal == 0D)
                    return Expression.Constant(0D);
                if (leftVal == 1D)
                    return right;

                if(right is BinaryExpression rightBin)
                    return GetMultipleExpression(leftVal, rightBin, allowStringCompare);
            }

            //distribute right const value to each side of left expression
            if (isRightConst)
            {
                if (rightVal == 0D)
                    return Expression.Constant(0D);

                if (rightVal == 1D)
                    return left;

                if(left is BinaryExpression leftBin)
                    return GetMultipleExpression(rightVal, leftBin, allowStringCompare);
            }
            return GetBinaryExpression(ExpressionType.Multiply, left, right, allowStringCompare);
        }

        internal static Expression GetMultipleExpression(double value, BinaryExpression exprBin, bool allowStringCompare = true)
        {
            if (new[] {ExpressionType.Add, ExpressionType.Subtract}.Contains(exprBin.NodeType))
            {
                var valConstExpr = Expression.Constant(value);
                var recurLeft = GetMultipleExpression(valConstExpr, exprBin.Left, allowStringCompare);
                var recurRight = GetMultipleExpression(valConstExpr, exprBin.Right, allowStringCompare);
                return GetBinaryExpression(exprBin.NodeType, recurLeft,recurRight,allowStringCompare);
            }

            if (exprBin.NodeType == ExpressionType.Multiply 
                && TryIsBinaryExpressWithOneConst(exprBin, out var innerVal, out var innerExpr))
            {
                var distVal = value * innerVal;
                if (distVal == 0D)
                    return Expression.Constant(0D);
                if (distVal == 1D)
                    return innerExpr;
                return GetBinaryExpression(ExpressionType.Multiply,Expression.Constant(distVal), innerExpr, allowStringCompare);
            }

            if (value == 0D)
                return Expression.Constant(0D);
            if (value == 1D)
                return exprBin;
            return GetBinaryExpression(ExpressionType.Multiply,Expression.Constant(value), exprBin, allowStringCompare);
        }

        internal static Expression GetDivideExpression(Expression left, Expression right,
            bool allowStringCompare = true)
        {
            left = left ?? Expression.Constant(0D);
            right = right ?? Expression.Constant(0D);

            var isLeftConst = TryGetDblConstValue(left, out var leftVal);
            var isRightConst = TryGetDblConstValue(right, out var rightVal);

            if (isLeftConst && isRightConst)
            {
                var div = leftVal / rightVal;
                if (div.Equals(double.NaN))
                    div = 0D;
                return Expression.Constant(div);
            }
            
            if (isLeftConst)
            {
                if (leftVal == 0D)
                    return Expression.Constant(0D);
                if (right is BinaryExpression rightBin)
                    return GetDivideExpression(leftVal, rightBin, false, allowStringCompare);
            }

            if (isRightConst)
            {
                if(rightVal == 0D)
                    return Expression.Constant(0D);
                if (rightVal == 1D)
                    return left;
                if (left is BinaryExpression leftBin)
                    return GetDivideExpression(rightVal, leftBin, true, allowStringCompare);
            }

            return GetBinaryExpression(ExpressionType.Divide, left, right, allowStringCompare);
        }

        internal static Expression GetDivideExpression(double value, BinaryExpression exprBin, bool isValueDenominator, bool allowStringCompare = true)
        {

            if (new[] { ExpressionType.Add, ExpressionType.Subtract }.Contains(exprBin.NodeType))
            {
                //(a + c) / b = (a/b) + (c/b)
                var recurLeft = isValueDenominator
                    ? GetDivideExpression(exprBin.Left, Expression.Constant(value), allowStringCompare)
                    : GetDivideExpression(Expression.Constant(value), exprBin.Left, allowStringCompare);

                var recurRight = isValueDenominator
                    ? GetDivideExpression(exprBin.Right, Expression.Constant(value), allowStringCompare)
                    : GetDivideExpression(Expression.Constant(value), exprBin.Right, allowStringCompare);

                return GetBinaryExpression(exprBin.NodeType, recurLeft, recurRight, allowStringCompare);
            }

            if (exprBin.NodeType == ExpressionType.Multiply &&
                TryIsBinaryExpressWithOneConst(exprBin, out var innerVal, out var innerExpr))
            {
                var distVal = isValueDenominator ? innerVal / value : value / innerVal;
                if (distVal.Equals(double.NaN))
                    distVal = 0D;
                if (distVal == 0D)
                    return Expression.Constant(0D);
                if (distVal == 1D)
                    return innerExpr;
                return isValueDenominator
                    ? GetBinaryExpression(ExpressionType.Multiply, innerExpr, Expression.Constant(distVal),
                        allowStringCompare)
                    : GetBinaryExpression(ExpressionType.Multiply, Expression.Constant(distVal), innerExpr,
                        allowStringCompare);
            }

            var expr = isValueDenominator
                ? GetBinaryExpression(ExpressionType.Divide, exprBin, Expression.Constant(value), allowStringCompare)
                : GetBinaryExpression(ExpressionType.Divide, Expression.Constant(value), exprBin, allowStringCompare);
            return expr;
        }

        internal static bool TryGetDblConstValue(Expression expr, out double value)
        {
            value = 0D;
            if (expr == null)
                return false;

            if (expr is UnaryExpression uniExpr && TryGetDblConstValue(uniExpr.Operand, out value))
            {
                value = -1D * value;
                return true;
            }
            if (!(expr is ConstantExpression conExpr))
                return false;

            var conExprVal = conExpr.Value;

            if (!(conExprVal is double d))
                return false;
            value = d;
            return true;
        }

        private static Expression GetAddSubtractExpression(ExpressionType nodeType,  Expression expr1, Expression expr2, bool allowStringCompare = true)
        {
            expr1 = expr1 ?? Expression.Constant(0D);
            expr2 = expr2 ?? Expression.Constant(0D);

            var isExpr1BinWithConst = 
                TryIsBinaryExpressWithOneConst(expr1, out var expr1InnerVal, out var expr1InnerExpr)
                && expr1.NodeType == ExpressionType.Multiply;
            var isExpr2BinWithConst =
                TryIsBinaryExpressWithOneConst(expr2, out var expr2InnerVal, out var expr2InnerExpr)
                && expr2.NodeType == ExpressionType.Multiply;

            if (!isExpr1BinWithConst || !isExpr2BinWithConst)
                return GetBinaryExpression(nodeType, expr1, expr2, allowStringCompare);

            if (IsAlgebraVariableEsque(expr1InnerExpr)
                && IsAlgebraVariableEsque(expr2InnerExpr)
                && allowStringCompare
                && string.Equals(expr1InnerExpr.ToString(), expr2InnerExpr.ToString()))
            {
                var coeff = expr1InnerVal + expr2InnerVal;
                return GetBinaryExpression(ExpressionType.Multiply,Expression.Constant(coeff), expr1InnerExpr);
            }

            return GetBinaryExpression(nodeType, expr1, expr2, allowStringCompare);
        }

        private static bool IsAlgebraVariableEsque(Expression expr)
        {
            if (expr == null)
                return false;
            if (expr is IndexExpression)
                return true;
            if (expr is ParameterExpression)
                return true;
            return false;
        }

        private static bool TryIsBinaryExpressWithOneConst(Expression expr, out double val, out Expression otherSide)
        {
            return TryIsBinaryExpressWithOneConst(expr, out val, out otherSide, out var dontCare);
        }

        private static bool TryIsBinaryExpressWithOneConst(Expression expr, out double val, out Expression otherSide, out bool isRightSideTheValue)
        {
            expr = expr ?? Expression.Constant(0D);
            val = 0D;
            otherSide = Expression.Constant(0D);
            isRightSideTheValue = false;
            if (!(expr is BinaryExpression binExpr))
                return false;

            if (TryGetDblConstValue(binExpr.Left, out var leftVal))
            {
                val = leftVal;
                otherSide = binExpr.Right;
                return true;
            }

            if (TryGetDblConstValue(binExpr.Right, out var rightVal))
            {
                val = rightVal;
                otherSide = binExpr.Left;
                isRightSideTheValue = true;
                return true;
            }

            return false;
        }

        private static Expression GetBinaryExpression(ExpressionType nodeType, Expression left, Expression right, bool allowStringCompare = true)
        {
            left = left ?? Expression.Constant(0D);
            right = right ?? Expression.Constant(0D);

            var areExactSameString = string.Equals(left.ToString(), right.ToString());

            switch (nodeType)
            {
                case ExpressionType.Add:
                    return Expression.Add(left, right);
                case ExpressionType.Subtract:
                    return areExactSameString && allowStringCompare
                        ? Expression.Constant(0D) as Expression 
                        : Expression.Subtract(left, right);
                case ExpressionType.Multiply:
                    return areExactSameString && allowStringCompare
                        ? Expression.Power(left,Expression.Constant(2)) 
                        : Expression.Multiply(left, right);
                case ExpressionType.Divide:
                    return areExactSameString && allowStringCompare
                        ? Expression.Constant(1D) as Expression 
                        : Expression.Divide(left, right);
            }

            throw new NotImplementedException();
        }
    }
}