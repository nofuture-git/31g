using System;

namespace NoFuture.Util.Core.Math.Matrix
{
    /// <summary>
    /// Chiang, Alpha C. "Fundamental Methods Of Mathematical Economics" New York: McGraw-Hill, Inc.
    ///     1984. Print
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// REFERENCE:
    ///- not commutative multiplication
    ///  AB != BA
    ///
    ///- associative multiplication
    /// A*(B*C) = (A*B)*C
    /// (a*b)*C = a*(b*C)
    /// a*(B*C) = (a*B)*C = B*(a*C)
    ///
    ///- distributive law
    /// A*(B + C) = A*B + A*C
    /// (B + C)*A = B*A + C*A
    /// A*v + B*v = (A + B)*v
    /// [A + B]ᵀ = Aᵀ + Bᵀ
    /// A*(u + v) = A*u + A*v
    ///
    ///- commutative addition
    /// A + B = B + A
    ///
    ///- associative addition
    /// A + (B + C) = (A + B) + C
    ///
    ///- symmetric matrix is 
    ///  A = Aᵀ
    ///- dyadic matrix
    ///  A*Aᵀ
    ///- idempotent matrix
    ///  A = A*A
    ///
    ///- other multiplication rules
    /// (A*B)ᵀ = Bᵀ*Aᵀ
    /// det(A * B) = det(A) * det(B)
    ///
    ///- exponent rules
    ///  A^(r+s) = A^r * A^s
    ///  A^(r*s) = (A^r)^s
    ///  A^0 = I
    ///
    ///- inversion rules
    /// A^-1*A = I
    /// A*A^-1 = I
    /// I^-1 = I
    /// (A^-1)^-1 = A
    /// (A^-1)ᵀ = (Aᵀ)^-1
    /// ]]>
    /// </remarks>
    public class MatrixOps
    {
        public static double[,] Sum(double[,] a, double[,] b)
        {
            return Arithmetic(a, b, (d1, d2) => d1 + d2);
        }

        public static double[,] Difference(double[,] a, double[,] b)
        {
            return Arithmetic(a, b, (d1, d2) => d1 - d2);
        }

        public static double[,] Arithmetic(double[,] a, double[,] b, Func<double, double, double> expr)
        {
            a = a ?? new double[,] { };
            b = b ?? new double[,] { };
            expr = expr ?? ((d1, d2) => d1 + d2);
            if (a.CountOfRows() != b.CountOfRows()) //equal rows
                throw new NonConformableException(
                    $"The number of rows in matrix 'a' must match the number of rows in matrix 'b' " +
                    $"to solve for the {expr}.");
            if (a.CountOfColumns() != b.CountOfColumns()) //equal columns
                throw new NonConformableException(
                    $"The number of columns in matrix 'a' must match the number of columns in matrix 'b' " +
                    $"to solve for the {expr}.");

            var iLength = a.CountOfRows();
            var jLength = a.CountOfColumns();

            var sum = new double[iLength,jLength];

            for (var i = 0L; i < iLength; i++)
            {
                for (var j = 0L; j < jLength; j++)
                {
                    sum[i, j] = expr(a[i, j], b[i, j]);
                }
            }
            return sum;
        }

        public static double[,] Product(double[,] a, double scalar)
        {
            a = a ?? new double[,] { };
            var iLength = a.CountOfRows();
            var jLength = a.CountOfColumns();
            var vout = new double[iLength, jLength];
            for (var i = 0L; i < iLength; i++)
            {
                for(var j=0L;j<jLength;j++)
                {
                    vout[i,j] = a[i,j] * scalar;
                }
            }
            return vout;

        }

        public static double[,] Product(double[,] a, double[,] b)
        {
            a = a ?? new double[,] { };
            b = b ?? new double[,] { };
            var dimensions = new[,] {{a.CountOfRows(), a.CountOfColumns()}, {b.CountOfRows(), b.CountOfColumns()}};
            var m = dimensions[0, 0];
            var n = dimensions[0, 1];
            var p = dimensions[1, 0];
            var q = dimensions[1, 1];

            if (n != p) //equal rows
                throw new NonConformableException(
                    "The number of columns in matrix 'a' must match the number of rows in matrix 'b' " +
                    "in order to solve for the product of the two.");

            var product = new double[m,q];
            for (var productRows = 0L; productRows < m; productRows++)
            for (var productColumns = 0L; productColumns < q; productColumns++)
            for (var i = 0L; i < n; i++)
                product[productRows, productColumns] += a[productRows, i] * b[i, productColumns];

            return product;

        }

        public static double[,] GetIdentity(long sqrSize)
        {
            sqrSize = sqrSize < 0 ? 0 : sqrSize;
            var identity = new double[sqrSize,sqrSize];
            for (var i = 0L; i < sqrSize; i++)
                identity[i, i] = 1L;

            return identity;
        }

        public static double[,] GetAllOnesMatrix(long rows, long columns)
        {
            rows = rows < 0 ? 0 : rows;
            columns = columns < 0 ? 0 : columns;
            var s = new double[rows, columns];
            for (var i = 0; i < s.CountOfRows(); i++)
            {
                for (var j = 0; j < s.CountOfColumns(); j++)
                    s[i, j] = 1D;
            }
            return s;
        }

        public static bool AreEqual(double[,] a, double[,] b, double tolerance = 0.0000001D)
        {
            a = a ?? new double[,] { };
            b = b ?? new double[,] { };
            if (a.CountOfRows() != b.CountOfRows() || a.CountOfColumns()!=b.CountOfColumns())
                return false;
            for (var i = 0; i < a.CountOfRows(); i++)
            {
                for (var j = 0; j < a.CountOfColumns(); j++)
                {
                    if (System.Math.Abs(a[i, j] - b[i, j]) > tolerance)
                        return false;

                }
            }
            return true;
        }

        /// <summary>
        /// Returns a matrix of 1 X <see cref="length"/> where all
        /// values are 0 expect for one at <see cref="index"/>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static double[,] OneHotVector(long index, long length)
        {
            index = index < 0 ? 0 : index;
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
        /// <param name="expr">Optional, apply some expression to each value</param>
        /// <returns></returns>
        public static double[,] RandomMatrix(long numOfRows, long numOfColumns, Func<double, double> expr = null)
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
                    if (expr != null)
                        m[i, j] = expr(m[i, j]);
                }
            }

            return m;
        }
    }
}