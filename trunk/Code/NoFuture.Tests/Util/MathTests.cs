using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using com.sun.org.apache.bcel.@internal.generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data;
using NoFuture.Timeline;
using NoFuture.Util.Math;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class MathTests
    {
        [TestMethod]
        public void TestAreEqual()
        {
            var typicalMatrix = new[,]
            {
                {2D, 2D},
                {2D, -1D}
            };

            Assert.IsTrue(Matrix.AreEqual(typicalMatrix, typicalMatrix));

        }

        [TestMethod]
        public void TestEigenvalueExpression()
        {
            var typicalMatrix = new[,]
            {
                {2D, 2D},
                {2D, -1D}
            };
            var eigenvalueExpression = MatrixExpressions.EigenvalueExpression(2);

            var eigenvalueFunc = eigenvalueExpression.Compile();
            System.Diagnostics.Debug.WriteLine(eigenvalueExpression.ToString());
            var myEigenvalues = new List<double>();
            for (var i = 10; i >= -10; i--)
            {
                if (eigenvalueFunc(typicalMatrix, i).Equals(0D))
                    myEigenvalues.Add(i);
            }

            foreach (var eigenVal in myEigenvalues)
            {
                System.Diagnostics.Debug.WriteLine(eigenVal);
            }

            Assert.AreNotEqual(0, myEigenvalues.Count);
            Assert.AreEqual(3,myEigenvalues[0]);
            Assert.AreEqual(-2,myEigenvalues[1]);
        }

        [TestMethod]
        public void TestEigenVectors()
        {
            var typicalMatrix = new[,]
            {
                {0.8D, 0.3D},
                {0.2D, 0.7D}
            };

            var eigenval00 = 1D;
            //var eigenval01 = 0.5D;
            //pg 326 Dx = rx (where 'x' is a nX1 matrix, 'n' being dim's of D
            //so for a 3x3 matrix the eigenvector is 3x1

            //(D -rI)x = 0
            //so all the eigenvalues do is get you closer to guessing what 'x' is

            //this is the property of eigenvectors that is being sought
            var really = Matrix.Product(typicalMatrix, new double[,] {{0.6D}, {0.4D}});
            System.Diagnostics.Debug.WriteLine(really.Print());//yep, this really is the {{0.6D}, {0.4D}}

            typicalMatrix = new[,]
            {
                {2D, 2D},
                {2D, -1D}
            };
            eigenval00 = 3D;
            //eigenval01 = -2D;

            //so you get this matrix
            var usedToGuessEigenVector = Matrix.Difference(typicalMatrix,
                Matrix.Product(Matrix.GetIdentity(typicalMatrix.GetLongLength(0)), eigenval00));

            //then you simply have to try various 2x1 matricies which when taken time 'usedToGuessEigenVector' 
            //  result in a 2x1 matrix of zeros
        }

        [TestMethod]
        public void TestToleranceEqualityOps()
        {
            var dbl1 = 5.675D;
            var dbl2 = 5.67468923D;

            var equalityOpWithThreshold = System.Math.Abs(dbl1 - dbl2) < 0.01;
            
            Assert.IsTrue(equalityOpWithThreshold);
        }

        [TestMethod]
        public void TestDeterminant()
        {
            var typicalMatrix = new[,]
            {
                {3D, 5D, 1D},
                {9D, 1D, 4D},
                {11D, 13D, 60D}
            };

            var testResult = MatrixExtensions.Determinant(typicalMatrix);
            Assert.AreEqual(-2350, testResult);
        }

        [TestMethod]
        public void TestExtensionMethods()
        {
            var aSqrMatrix = new[,]
            {
                {0D, 1D}, 
                {4D, 5D}, 
                {8D, 9D}
            };

            var numOfRows = aSqrMatrix.Rows();
            var numOfColumns = aSqrMatrix.Columns();

            Assert.AreEqual(3,numOfRows);
            Assert.AreEqual(2, numOfColumns);

        }

        [TestMethod]
        public void TestGetaAllOnesMatrix()
        {
            var testResult = NoFuture.Util.Math.Matrix.GetAllOnesMatrix(2, 1);
            var numOfRows = testResult.GetLongLength(0);
            var numOfColumns = testResult.GetLongLength(1);

            Assert.AreEqual(2,numOfRows);
            Assert.AreEqual(1,numOfColumns);

            Assert.AreEqual(1D, testResult[0, 0]);
            Assert.AreEqual(1D, testResult[1, 0]);

        }

        [TestMethod]
        public void TestMatrixProduct()
        {
            var col = new double[,] {{1}, {1}};
            var row = new double[,] {{1, 1}};
            var testResult = NoFuture.Util.Math.Matrix.Product(col, row);

            var lenTop = testResult.GetLongLength(0);
            var lenBottom = testResult.GetLongLength(1);

            System.Diagnostics.Debug.WriteLine("{0}X{1}",lenTop, lenBottom);

            Assert.AreEqual(1D, testResult[0, 0]);
            Assert.AreEqual(1D, testResult[0, 1]);
            Assert.AreEqual(1D, testResult[1, 0]);
            Assert.AreEqual(1D, testResult[1, 1]);

            var matrixOfOnes = new[,]
            {
                {1D, 1D}, 
                {1D, 1D}
            };

            var typicalMatrix = new[,]
            {
                {3D, 5D, 1D}, 
                {9D, 1D, 4D}
            };

            testResult = Matrix.Product(matrixOfOnes, typicalMatrix);
            Assert.AreEqual(12D, testResult[0, 0]);
            Assert.AreEqual(6D, testResult[0, 1]);
            Assert.AreEqual(5D, testResult[0, 2]);

            Assert.AreEqual(12D, testResult[1, 0]);
            Assert.AreEqual(6D, testResult[1, 1]);
            Assert.AreEqual(5D, testResult[1, 2]);
        }

        [TestMethod]
        public void TestMatrixTranspose()
        {
            var testInput = new[,]
            {
                {1D}, 
                {1D}
            };

            var testResult = testInput.Transpose();

            //one row, two columns
            Assert.AreEqual(1D, testResult[0, 0]);
            Assert.AreEqual(1D, testResult[0, 1]);
        }

        [TestMethod]
        public void TestMatrixDeviation()
        {
            var typicalMatrix = new[,]
            {
                {3D, 5D, 1D}, 
                {9D, 1D, 4D}
            };

            var testResult = typicalMatrix.Deviation();
            Assert.AreEqual(-3D, testResult[0, 0]);
            Assert.AreEqual(2D, testResult[0, 1]);
            Assert.AreEqual(-1.5D, testResult[0, 2]);

            Assert.AreEqual(3D, testResult[1, 0]);
            Assert.AreEqual(-2D, testResult[1, 1]);
            Assert.AreEqual(1.5D, testResult[1, 2]);

            System.Diagnostics.Debug.WriteLine(testResult.Print());
           
        }

        [TestMethod]
        public void TestMatrixPrint()
        {
            var testInput = new[,]
            {
                {90D, 60D, 90D},
                {90D, 90D, 30D},
                {60D, 60D, 60D},
                {60D, 60D, 90D},
                {30D, 30D, 30D}
            };

            System.Diagnostics.Debug.WriteLine(testInput.Print());
            
        }

        [TestMethod]
        public void TestMatrixCovariance()
        {
            var testInput = new[,]
            {
                {90D, 60D, 90D},
                {90D, 90D, 30D},
                {60D, 60D, 60D},
                {60D, 60D, 90D},
                {30D, 30D, 30D}
            };

            var testResult = testInput.Covariance();

            Assert.AreEqual(504D, testResult[0, 0]);
            Assert.AreEqual(360D, testResult[0, 1]);
            Assert.AreEqual(180D, testResult[0, 2]);

            Assert.AreEqual(360D, testResult[1, 0]);
            Assert.AreEqual(360D, testResult[1, 1]);
            Assert.AreEqual(0, testResult[1, 2]);

            Assert.AreEqual(180D, testResult[2, 0]);
            Assert.AreEqual(0D, testResult[2, 1]);
            Assert.AreEqual(720D, testResult[2, 2]);
        }

        [TestMethod]
        public void TestRomanNumberConversion()
        {
            var testSubject = "XLIX";
            var testResult = testSubject.ParseRomanNumeral();
            Assert.AreEqual(49,testResult);
        }

        [TestMethod]
        public void TestLinearEquation()
        {
            //y = 0.1056x - 181.45
            var myEq = new LinearEquation {Intercept = -181.45, Slope = 0.1056};
            var dob = 1974.477451;
            var x = myEq.SolveForY(dob);
            System.Diagnostics.Debug.WriteLine(x);
            Assert.IsTrue(x > 27 && x < 28);

            var y = myEq.SolveForX(27.0548188256);
            System.Diagnostics.Debug.WriteLine(y);
            Assert.AreEqual(dob, y);

        }

        [TestMethod]
        public void TestPerDiemInterest()
        {
            var testResult = 1541M.PerDiemInterest(0.13f, 30);
            Assert.AreEqual(1557.54M,testResult);

            testResult = 36000M.PerDiemInterest(0.035f, NoFuture.Shared.Constants.TropicalYear.TotalDays*5);
            Assert.AreEqual(42884.5M, testResult);

            testResult = 36000M.PerDiemInterest(0f, NoFuture.Shared.Constants.TropicalYear.TotalDays*5);
            Assert.AreEqual(36000M, testResult);

            testResult = 48000M.PerDiemInterest(-0.15f, NoFuture.Shared.Constants.TropicalYear.TotalDays*20);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

    }
}
