using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NoFuture.Shared.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Tests.Util
{
    [TestFixture]
    public class MathTests
    {
        [Test]
        public void TestAreEqual()
        {
            var typicalMatrix = new[,]
            {
                {2D, 2D},
                {2D, -1D}
            };

            Assert.IsTrue(Matrix.AreEqual(typicalMatrix, typicalMatrix));

        }

        [Test]
        public void TestEigenvalueExpression()
        {
            var typicalMatrix = new[,]
            {
                {2D, 2D},
                {2D, -1D}
            };
            var eigenvalueExpression = MatrixExpressions.EigenvalueExpression(2);

            var eigenvalueFunc = eigenvalueExpression.Compile();
            Console.WriteLine(eigenvalueExpression.ToString());
            var myEigenvalues = new List<double>();
            for (var i = 10; i >= -10; i--)
            {
                if (eigenvalueFunc(typicalMatrix, i).Equals(0D))
                    myEigenvalues.Add(i);
            }

            foreach (var eigenVal in myEigenvalues)
            {
                Console.WriteLine(eigenVal);
            }

            Assert.AreNotEqual(0, myEigenvalues.Count);
            Assert.AreEqual(3,myEigenvalues[0]);
            Assert.AreEqual(-2,myEigenvalues[1]);


            


        }

        [Test]
        public void TestEigenVector3X3()
        {
            var eigenvalueExpression = MatrixExpressions.EigenvalueExpression(3);

            var eigenvalueFunc = eigenvalueExpression.Compile();
            Console.WriteLine(eigenvalueExpression.ToString());
            var myEigenvalues = new List<double>();

            var threeByThree = new[,]
            {
                {-1D, 2D, 2D},
                {2D, 2D, -1D},
                {2D, -1D, 2D}
            };
            myEigenvalues = new List<double>();
            for (var i = 10; i >= -10; i--)
            {
                if (eigenvalueFunc(threeByThree, i).Equals(0D))
                    myEigenvalues.Add(i);
            }
            foreach (var eigenVal in myEigenvalues)
            {
                Console.WriteLine(eigenVal);
            }
        }

        [Test]
        public void TestEigenVectors()
        {
            var typicalMatrix = new[,]
            {
                {0.8D, 0.3D},
                {0.2D, 0.7D}
            };

            var eigenval00 = 1D;
            //var eigenval01 = 0.5D;
            //pg 326 Dx = rx (where 'x' is a nX1 matrix, 'n' being dim's of D so for a 3x3 matrix the eigenvector is 3x1)

            //(D -rI)x = 0
            //so all the eigenvalues do is get you closer to guessing what 'x' is

            //this is the property of eigenvectors that is being sought
            //https://www.khanacademy.org/math/linear-algebra/alternate_bases/eigen_everything/v/linear-algebra-introduction-to-eigenvalues-and-eigenvectors
            // " the line that they span will not change" its just extended in the same direction
            var really = Matrix.Product(typicalMatrix, new double[,] {{0.6D}, {0.4D}});
            Console.WriteLine(really.Print());//yep, this really is the {{0.6D}, {0.4D}}

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

        [Test]
        public void TestToleranceEqualityOps()
        {
            var dbl1 = 5.675D;
            var dbl2 = 5.67468923D;

            var equalityOpWithThreshold = System.Math.Abs(dbl1 - dbl2) < 0.01;
            
            Assert.IsTrue(equalityOpWithThreshold);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void TestGetaAllOnesMatrix()
        {
            var testResult = Matrix.GetAllOnesMatrix(2, 1);
            var numOfRows = testResult.GetLongLength(0);
            var numOfColumns = testResult.GetLongLength(1);

            Assert.AreEqual(2,numOfRows);
            Assert.AreEqual(1,numOfColumns);

            Assert.AreEqual(1D, testResult[0, 0]);
            Assert.AreEqual(1D, testResult[1, 0]);

        }

        [Test]
        public void TestMatrixProduct()
        {
            var col = new double[,] {{1}, {1}};
            var row = new double[,] {{1, 1}};
            var testResult = Matrix.Product(col, row);

            var lenTop = testResult.GetLongLength(0);
            var lenBottom = testResult.GetLongLength(1);

            Console.WriteLine("{0}X{1}",lenTop, lenBottom);

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

        [Test]
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

        [Test]
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

            Console.WriteLine(testResult.Print());
           
        }

        [Test]
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

            Console.WriteLine(testInput.Print());
            
        }

        [Test]
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

        [Test]
        public void TestGetSoftmax()
        {
            var testInput = new[,]
            {
                { 1.0, 2.0, 3.0, 4.0, 1.0, 2.0, 3.0 },
                { 6.0, 2.0, 1.0, 4.0, 1.0, 3.0, 1.0 }
            };
            var testResult = testInput.GetSoftmax();

            Console.WriteLine(testResult.Print());
            var sumAt = Math.Round(testResult.SelectRow(0).Sum());
            Assert.AreEqual(1.0D, sumAt);
            sumAt = Math.Round(testResult.SelectRow(1).Sum());
            Assert.AreEqual(1.0D, sumAt);

        }

        [Test]
        public void TestSelectRow()
        {
            var testInput = new double[,] {{1, -1, 0}, {-1, 6, -2}, {0, -2, 3}};

            var testResult = testInput.SelectRow(1);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(3, testResult.Length);
            Assert.AreEqual(testInput[1, 0], testResult[0]);
            Assert.AreEqual(testInput[1, 1], testResult[1]);
            Assert.AreEqual(testInput[1, 2], testResult[2]);
        }

        [Test]
        public void TestSelectColumn()
        {
            var testInput = new double[,] { { 1, -1, 0 }, { -1, 6, -2 }, { 0, -2, 3 } };
            var testResult = testInput.SelectColumn(1);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(3, testResult.Length);
            Assert.AreEqual(testInput[0, 1], testResult[0]);
            Assert.AreEqual(testInput[1, 1], testResult[1]);
            Assert.AreEqual(testInput[2, 1], testResult[2]);
        }

        [Test]
        public void TestPrintMatrix()
        {
            var testInput = new double[,] { { 1, -1, 0 }, { -1, 6, -2 }, { 0, -2, 3 } };
            Console.WriteLine(testInput.Print());

            testInput = new double[,]
            {
                {-0.14524321, -0.3209983, 0.9358763},
                {0.8905313, 0.36975, 0.265},
                {-0.431, 0.8719205, 0.2321555}
            };

            Console.WriteLine(testInput.Print());

            testInput = new double[,] { { 1, 1, 0 }, { 1, 56, -2 }, { 101, 2, 3 } };
            Console.WriteLine(testInput.Print());
        }

        [Test]
        public void TestRomanNumberConversion()
        {
            var testSubject = "XLIX";
            var testResult = testSubject.ParseRomanNumeral();
            Assert.AreEqual(49,testResult);
        }

        [Test]
        public void TestLinearEquation()
        {
            //y = 0.1056x - 181.45
            var myEq = new LinearEquation {Intercept = -181.45, Slope = 0.1056};
            var dob = 1974.477451;
            var x = myEq.SolveForY(dob);
            Console.WriteLine(x);
            Assert.IsTrue(x > 27 && x < 28);

            var y = myEq.SolveForX(27.0548188256);
            Console.WriteLine(y);
            Assert.AreEqual(dob, y);

        }

        [Test]
        public void TestPerDiemInterest()
        {
            var testResult = 1541M.PerDiemInterest(0.13f, 30);
            Assert.AreEqual(1557.54M,testResult);

            testResult = 36000M.PerDiemInterest(0.035f, Constants.TropicalYear.TotalDays*5);
            Assert.AreEqual(42884.5M, testResult);

            testResult = 36000M.PerDiemInterest(0f, Constants.TropicalYear.TotalDays*5);
            Assert.AreEqual(36000M, testResult);

            testResult = 48000M.PerDiemInterest(-0.15f, Constants.TropicalYear.TotalDays*20);

            Console.WriteLine(testResult);

            testResult = 900M.PerDiemInterest(0.055F, 150);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestLnEq()
        {
            var testSubject = new NaturalLogEquation { Slope = 7.123, Intercept = -55.44 };
            const double X_IN = 1985;
            var solveY = testSubject.SolveForY(X_IN);

            var solveX = testSubject.SolveForX(solveY);
            Assert.AreEqual(System.Math.Round(X_IN), System.Math.Round(solveX));
        }

        [Test]
        public void TestNormalDist()
        {
            var testSubject = new NormalDistEquation() {Mean = 0, StdDev = 1};
            Console.WriteLine(string.Format("{0}\t{1}","x","f(x)"));
            for (var i = 0; i <= 30; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    var z = i*0.1 + j*0.01;
                    var testResult = testSubject.SolveForY(z);
                    //var testResult = (z - testSubject.Mean)/testSubject.StdDev;
                    //System.Diagnostics.Debug.Write(string.Format(" {0} ", z));
                    Console.WriteLine(string.Format("{0}\t{1}", z, testResult));
                }
                
            }
        }

        [Test]
        public void TestNormalDistGetZScore()
        {
            var testSubject = new NormalDistEquation { Mean = 0, StdDev = 1 };

            var testResult = testSubject.GetZScoreFor(0);
            Assert.IsTrue(testResult >= 0.499);

            testResult = testSubject.GetZScoreFor(1);
            Assert.IsTrue(testResult >= 0.34134);
        }
    }
}
