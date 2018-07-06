﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Shared.Core;
using NoFuture.Util.Core.Math;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class MatrixTests
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
            Assert.AreEqual(3, myEigenvalues[0]);
            Assert.AreEqual(-2, myEigenvalues[1]);
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
            var really = Matrix.Product(typicalMatrix, new double[,] { { 0.6D }, { 0.4D } });
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
        public void TestCofactor()
        {
            var testInput = new double[,]
            {
                {2, 1},
                {1, 2}
            };
            var testResult = testInput.Cofactor();
            var expect = new double[,]
            {
                {2, -1},
                {-1, 2}
            };

            Assert.IsTrue(Matrix.AreEqual(expect, testResult));
        }

        [Test]
        public void TestInverse()
        {
            var testInput = new double[,]
            {
                {2, 1},
                {1, 2}
            };
            var testResult = testInput.Inverse();
            var expect = new[,]
            {
                {0.666666666666667, -0.333333333333333},
                {-0.333333333333333, 0.666666666666667}
            };
            Assert.IsTrue(Matrix.AreEqual(expect, testResult));
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
            testResult = MatrixExtensions.Determinant(new double[,]
            {
                {2, 1},
                {1, 2}
            });
            Assert.AreEqual(3, testResult);

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

            var numOfRows = aSqrMatrix.CountOfRows();
            var numOfColumns = aSqrMatrix.CountOfColumns();

            Assert.AreEqual(3, numOfRows);
            Assert.AreEqual(2, numOfColumns);

        }

        [Test]
        public void TestGetaAllOnesMatrix()
        {
            var testResult = Matrix.GetAllOnesMatrix(2, 1);
            var numOfRows = testResult.GetLongLength(0);
            var numOfColumns = testResult.GetLongLength(1);

            Assert.AreEqual(2, numOfRows);
            Assert.AreEqual(1, numOfColumns);

            Assert.AreEqual(1D, testResult[0, 0]);
            Assert.AreEqual(1D, testResult[1, 0]);

        }

        [Test]
        public void TestMatrixProduct()
        {
            var col = new double[,] { { 1 }, { 1 } };
            var row = new double[,] { { 1, 1 } };
            var testResult = Matrix.Product(col, row);

            var lenTop = testResult.GetLongLength(0);
            var lenBottom = testResult.GetLongLength(1);

            Console.WriteLine("{0}X{1}", lenTop, lenBottom);

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
        public void TestDotProduct()
        {
            var myX = new double[,] { { 1, 3, 0 }, { 2, 5, 1 }, { 1, 3, 0 } };
            var myY = new double[,] { { 4, 5, 6 }, { 7, 4, 1 }, { 9, 6, 3 } };

            var testResult = myX.DotProduct(myY);
            Console.WriteLine(testResult.Print());
        }

        [Test]
        public void TestStdByColumn()
        {
            var myX = new double[,] { { 1, 3, 0 }, { 2, 5, 1 }, { 1, 3, 0 } };
            var testREsult = myX.StdByColumn();
            Console.WriteLine(testREsult.ToMatrix().Print());
            Assert.IsNotNull(testREsult);
            Assert.AreEqual(3, testREsult.Length);

            Assert.IsTrue(System.Math.Abs(1.4142135623731D - testREsult[0]) < 0.0001);
            Assert.IsTrue(System.Math.Abs(3.78593889720018D - testREsult[1]) < 0.0001);
            Assert.IsTrue(System.Math.Abs(0.577350269189626D - testREsult[2]) < 0.0001);
        }

        [Test]
        public void TestScale()
        {
            var testInput = new double[,] { { 1, 3, 0 }, { 2, 5, 1 }, { 1, 3, 0 } };
            testInput.Scale();
            Assert.IsNotNull(testInput);
            Console.WriteLine(testInput.Print());
            var expected = new double[,]
            {
                {-0.7071067811865474, -0.7071067811865474, -0.7071067811865475},
                {1.4142135623730951, 1.4142135623730951, 1.4142135623730951},
                {-0.7071067811865474, -0.7071067811865474, -0.7071067811865475}
            };

            for (var i = 0; i < testInput.CountOfRows(); i++)
            {
                for (var j = 0; j < testInput.CountOfColumns(); j++)
                {
                    var y = testInput[i, j];
                    var t = expected[i, j];
                    Assert.IsTrue(System.Math.Abs(y - t) < 0.000001);

                }
            }
        }

        [Test]
        public void TestSingularValueDecomp()
        {
            var myX = new double[,] { { 1, 3, 0 }, { 2, 5, 1 }, { 1, 3, 0 } };
            var testResult = myX.SingularValueDecomp();
            Assert.IsNotNull(testResult);
            var expectedU = new[,]
            {
                {-0.4462018207622415, 0.5485471129706733, -0.7071067811865476},
                {-0.7757627667637326, -0.6310246664775307, -8.956860613409269e-17},
                {-0.44620182076224163, 0.5485471129706733, 0.7071067811865472}
            };
            var expectedS = new[] { 7.039606403458741, 0.6662894899235996, 0D }.Diag();

            var expectedV = new[,]
            {
                {-0.3471684402484749, -0.24757272853370346, -0.904534033733291},
                {-0.9313055848081169, 0.20435463487799882, 0.30151134457776385},
                {-0.11019973593730749, -0.9470728204791102, 0.3015113445777633}
            };

            Assert.IsTrue(Matrix.AreEqual(expectedU, testResult.U));
            Assert.IsTrue(Matrix.AreEqual(expectedS, testResult.D.Diag()));
            Assert.IsTrue(Matrix.AreEqual(expectedV, testResult.V));

            Console.WriteLine(testResult.U.Print());
            Console.WriteLine(testResult.D.ToMatrix().Print());
            Console.WriteLine(testResult.V.Print());
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
            var sumAt = System.Math.Round(testResult.SelectRow(0).Sum());
            Assert.AreEqual(1.0D, sumAt);
            sumAt = System.Math.Round(testResult.SelectRow(1).Sum());
            Assert.AreEqual(1.0D, sumAt);

        }

        [Test]
        public void TestSwapRow()
        {
            var testInput = Matrix.RandomMatrix(5, 5);
            var testRowA = testInput.SelectRow(2);
            var testRowB = testInput.SelectRow(4);

            testInput.SwapRow(2,4);

            var testResultA = testInput.SelectRow(2);
            var testResultB = testInput.SelectRow(4);

            for (var i = 0; i < testRowA.Length; i++)
            {
                Assert.AreEqual(testRowA[i], testResultB[i]);
                Assert.AreEqual(testRowB[i], testResultA[i]);
            }
        }

        [Test]
        public void TestSwapColumn()
        {
            var testInput = Matrix.RandomMatrix(5, 5);
            var testRowA = testInput.SelectColumn(2);
            var testRowB = testInput.SelectColumn(4);

            testInput.SwapColumn(2, 4);

            var testResultA = testInput.SelectColumn(2);
            var testResultB = testInput.SelectColumn(4);

            for (var i = 0; i < testRowA.Length; i++)
            {
                Assert.AreEqual(testRowA[i], testResultB[i]);
                Assert.AreEqual(testRowB[i], testResultA[i]);
            }
        }

        [Test]
        public void TestApplyAtRow()
        {
            var testInput = new double[,] { { 1, -1, 0 }, { -1, 6, -2 }, { 0, -2, 3 } };
            var toAdd = new double[] { 4, 1, 2 };

            testInput.ApplyAtRow(toAdd, 1, (d, d1) => d + d1);
            Console.WriteLine(testInput.Print());
            Assert.AreEqual(3D, testInput[1, 0]);
            Assert.AreEqual(7D, testInput[1, 1]);
            Assert.AreEqual(0D, testInput[1, 2]);
        }

        [Test]
        public void TestApplyAtColumn()
        {
            var testInput = new double[,] { { 1, -1, 0 }, { -1, 6, -2 }, { 0, -2, 3 } };
            var toAdd = new double[] { 4, 1, 2 };

            testInput.ApplyAtColumn(toAdd, 1, (d, d1) => d + d1);
            Console.WriteLine(testInput.Print());
            Assert.AreEqual(3D, testInput[0, 1]);
            Assert.AreEqual(7D, testInput[1, 1]);
            Assert.AreEqual(0D, testInput[2, 1]);
        }

        [Test]
        public void TestSelectRow()
        {
            var testInput = new double[,] { { 1, -1, 0 }, { -1, 6, -2 }, { 0, -2, 3 } };

            var testResult = testInput.SelectRow(1);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(3, testResult.Length);
            Assert.AreEqual(testInput[1, 0], testResult[0]);
            Assert.AreEqual(testInput[1, 1], testResult[1]);
            Assert.AreEqual(testInput[1, 2], testResult[2]);
        }

        [Test]
        public void TestFlatten()
        {
            var testInput = new double[,] { { 1, -1, 8 }, { -7, 6, -2 }, { 0, -4, 5 } };
            var testResult = testInput.Flatten();
            Assert.AreEqual(9, testResult.Length);
            Console.WriteLine(string.Join(",", testResult));
            Assert.AreEqual(testInput[0, 0], testResult[0]);
            Assert.AreEqual(testInput[1, 0], testResult[3]);
            Assert.AreEqual(testInput[2, 0], testResult[6]);
        }

        [Test]
        public void TestToMatrix()
        {
            var testInput = new double[] { 1, -1, 8, -7, 6, -2, 0, -4, 5 };
            var testResult = testInput.ToMatrix(3);

            Assert.AreEqual(1, testResult[0, 0]);
            Assert.AreEqual(-1, testResult[0, 1]);
            Assert.AreEqual(8, testResult[0, 2]);
            Assert.AreEqual(-7, testResult[1, 0]);
            Assert.AreEqual(6, testResult[1, 1]);
            Assert.AreEqual(-2, testResult[1, 2]);
            Assert.AreEqual(0, testResult[2, 0]);
            Assert.AreEqual(-4, testResult[2, 1]);
            Assert.AreEqual(5, testResult[2, 2]);

            Console.WriteLine(testResult.Print());

            testInput = new double[] { 1, -1, 8, -7, 6, -2, 0, -4, 5, 999 };
            testResult = testInput.ToMatrix(3, true);
            Assert.AreEqual(1, testResult[0, 0]);
            Assert.AreEqual(-1, testResult[0, 1]);
            Assert.AreEqual(8, testResult[0, 2]);
            Assert.AreEqual(-7, testResult[1, 0]);
            Assert.AreEqual(6, testResult[1, 1]);
            Assert.AreEqual(-2, testResult[1, 2]);
            Assert.AreEqual(0, testResult[2, 0]);
            Assert.AreEqual(-4, testResult[2, 1]);
            Assert.AreEqual(5, testResult[2, 2]);

            testInput = new double[] { 1, -1, 8, -7, 6, -2, 0, -4, 5 };
            testResult = testInput.ToMatrix(1);
            Assert.AreEqual(1, testResult[0, 0]);
            Assert.AreEqual(-7, testResult[0, 3]);
            Assert.AreEqual(5, testResult[0, 8]);
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
        public void TestCollapseTop2Bottom()
        {
            var testInput = new double[,] {
                {0, 0, 0, 0.5, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0.5, 0, 0}
            };
            var testResult = testInput.CollapseTop2Bottom(doubles => doubles.Sum());
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testInput.GetLength(1), testResult.Length);
            Assert.AreEqual(0.5, testResult[3]);
            Console.WriteLine(testResult.ToMatrix(1).Print());
        }

        [Test]
        public void TestCollapseLeft2Right()
        {
            var testInput = new double[,] {
                {0, 0, 0, 0.5, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0.5, 0, 0}
            };
            var testResult = testInput.CollapseLeft2Right(doubles => doubles.Sum());
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testInput.GetLength(0), testResult.Length);
            Console.WriteLine(testResult.ToMatrix(1).Print());
        }

        [Test]
        public void TestMean()
        {
            var testInput = new double[,]
            {
                { 1, 3, 0 },
                { 2, 5, 1 },
                { 1, 3, 0 }
            };

            var testResult = testInput.MeanByColumn();
            Assert.IsNotNull(testResult);
            Assert.AreEqual(3, testResult.Length);

            Assert.AreEqual(System.Math.Round(1.333D, 3), System.Math.Round(testResult[0], 3));
            Assert.IsTrue(System.Math.Abs(3.665 - testResult[1]) < 0.01);

            Assert.AreEqual(System.Math.Round(0.333, 3), System.Math.Round(testResult[2], 3));
            Console.WriteLine(testResult.ToMatrix(1).Print());
        }

        [Test]
        public void TestSVD()
        {
            var testInput = Matrix.RandomMatrix(5, 3);
            var USV = testInput.SingularValueDecomp();
            var U = USV.U;
            var S = USV.D.Diag();
            var V = USV.V;

            var pcXV = testInput.DotProduct(V);
            var pcUdS = U.DotProduct(S);

            Assert.IsTrue(Matrix.AreEqual(pcXV, pcUdS));

        }

        [Test]
        public void TestGetTriangle()
        {
            var testInput = new[,]
            {
                {1D, 2D, 3D, 4D, 5D},
                {2D, 4D, 6D, 8D, 10D},
                {3D, 6D, 9D, 12D, 15D},
                {4D, 8D, 12D, 16D, 20D},
                {5D, 10D, 15D, 20D, 25D}
            };

            var testResult = testInput.GetTriangle();

            var expected = new double[,]
            {
                {1, 2, 3, 4, 5},
                {0, 4, 6, 8, 10},
                {0, 0, 9, 12, 15},
                {0, 0, 0, 16, 20},
                {0, 0, 0, 0, 25}
            };

            Assert.IsTrue(Matrix.AreEqual(expected, testResult));

            testInput = new double[,]
            {
                {3, 6, 9, 12, 15},
                {4, 8, 12, 16, 20},
                {5, 10, 15, 20, 25}
            };
            testResult = testInput.GetTriangle();
            expected = new double[,]
            {
                {3, 6, 9, 12, 15},
                {0, 8, 12, 16, 20},
                {0, 0, 15, 20, 25}
            };

            Assert.IsTrue(Matrix.AreEqual(expected, testResult));

            testInput = new double[,]
            {
                {1, 2, 3},
                {2, 4, 6 },
                {3, 6, 9},
                {4, 8, 12},
                {5, 10, 15}
            };
            testResult = testInput.GetTriangle();
            expected = new double[,]
            {
                {1,2,3},
                {0,4,6},
                {0,0,9},
                {0,0,0},
                {0,0,0}
            };
            Assert.IsTrue(Matrix.AreEqual(expected, testResult));
        }

        [Test]
        public void TestCockeKasamiYounger()
        {
            var testInput = new double[,]
            {
                { 1, 3, 3 },
                { 2, 5, 1 },
                { 1, 3, 4 }
            };
            //average
            Func<double, double, double, double> myExpr = (left, below, cur) => (left + below + cur) / 3; 
            var testResult = testInput.CockeKasamiYounger(myExpr);

            var expected = new double[,]
            {
                {1, 3, 3.14814814814815},
                {0, 5, 3.33333333333333},
                {0, 0, 4}
            };

            Assert.IsTrue(Matrix.AreEqual(expected, testResult));
        }

        [Test]
        public void TestCrossProduct()
        {
            var testInput = new double[,]
            {
                {1, 0},
                {1, 1},
                {0, 1}
            };
            var testResult = testInput.CrossProduct();

            var expected = new double[,]
            {
                {2, 1},
                {1, 2}
            };
            Assert.IsTrue(Matrix.AreEqual(expected,testResult));
        }

        [Test]
        public void TestProjectionMatrix()
        {
            var A = new double[,] { { 1, 0 }, { 1, 1 }, { 0, 1 } };
            var P = A.ProjectionMatrix();
            var expect = new[,]
            {
                {0.666666666666667, 0.333333333333333, -0.333333333333333},
                {0.333333333333333, 0.666666666666667, 0.333333333333333},
                {-0.333333333333333, 0.333333333333333, 0.666666666666667}
            };
            Assert.IsTrue(Matrix.AreEqual(expect, P));

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
        public void TestPrintJsStyle()
        {
            var testInput = new [,]
            {
                {-0.14524321, -0.3209983, 0.9358763},
                {0.8905313, 0.36975, 0.265},
                {-0.431, 0.8719205, 0.2321555}
            };

            Console.WriteLine(testInput.Print("js"));
        }

        [Test]
        public void TestPrintCsStyle()
        {
            var testInput = new[,]
            {
                {-0.14524321, -0.3209983, 0.9358763},
                {0.8905313, 0.36975, 0.265},
                {-0.431, 0.8719205, 0.2321555}
            };

            Console.WriteLine(testInput.Print("cs"));
        }

        [Test]
        public void TestPrintPsStyle()
        {
            var testInput = new[,]
            {
                {-0.14524321, -0.3209983, 0.9358763},
                {0.8905313, 0.36975, 0.265},
                {-0.431, 0.8719205, 0.2321555}
            };

            Console.WriteLine(testInput.Print("ps"));
        }

        [Test]
        public void TestPrintRstyle()
        {
            var testInput = new[,]
            {
                {-0.14524321, -0.3209983, 0.9358763},
                {0.8905313, 0.36975, 0.265},
                {-0.431, 0.8719205, 0.2321555}
            };

            Console.WriteLine(testInput.Print("r"));
        }
    }

    [TestFixture]
    public class MathTests
    {
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



