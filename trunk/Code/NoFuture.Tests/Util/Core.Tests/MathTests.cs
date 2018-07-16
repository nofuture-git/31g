using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoFuture.Shared.Core;
using NoFuture.Util.Core.Math;
using NoFuture.Util.Core.Math.Matrix;
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

            Assert.IsTrue(MatrixOps.AreEqual(typicalMatrix, typicalMatrix));

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
            var really = MatrixOps.Product(typicalMatrix, new double[,] { { 0.6D }, { 0.4D } });
            Console.WriteLine(really.Print());//yep, this really is the {{0.6D}, {0.4D}}

            typicalMatrix = new[,]
            {
                {2D, 2D},
                {2D, -1D}
            };
            eigenval00 = 3D;
            //eigenval01 = -2D;

            //so you get this matrix
            var usedToGuessEigenVector = MatrixOps.Difference(typicalMatrix,
                MatrixOps.Product(MatrixOps.GetIdentity(typicalMatrix.GetLongLength(0)), eigenval00));

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
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

            testInput = new double[,]
            {
                {2, 1, 13, 4},
                {4, 5, 14, 7},
                {7, 12, 9, 10},
                {11, 8, 3, 6},
            };
            testResult = testInput.Cofactor();
            expect = new double[,]
            {
                {-20, -502, -224, 818},
                {16, 620, 252, -982},
                {13, -247, -91, 351},
                {-27, 23, 7, -15}
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

        }

        [Test]
        public void TestPerformance_Cofactor()
        {
            var testInput = new[,]
            {
                {
                    97.418396137986, 3.40651226544868, -17.0697313630349, 4.18423521542488, -6.32399747806094,
                    -25.4092733216091, 3.20579525699219, 6.73063972661051, -5.16467870714877, -6.96008775118052
                },
                {
                    3.40651226544868, 113.505735745414, 16.4033774812258, -3.86903748868356, 8.75250122903815,
                    -3.30474753757298, 7.68694549468861, 1.49722630651441, 5.06944661668959, -4.73241157167048
                },
                {
                    -17.0697313630349, 16.4033774812258, 78.9323619550698, -6.848424299473, 13.3954431395863,
                    11.9874843367797, -2.14743991631947, -10.5157502882462, 5.15232310352067, 5.42715572700512
                },
                {
                    4.18423521542488, -3.86903748868356, -6.848424299473, 95.7916362934071, -11.5144020370596,
                    12.4488884786246, -6.27419139571689, 14.4105686428956, -7.94837130774331, -13.5653060135102
                },
                {
                    -6.32399747806094, 8.75250122903815, 13.3954431395863, -11.5144020370596, 118.775917531215,
                    2.09217044619191, 8.3634719006863, 20.2864897980169, 16.3966731491597, -7.5086811180655
                },
                {
                    -25.4092733216091, -3.30474753757298, 11.9874843367797, 12.4488884786246, 2.09217044619191,
                    95.3074494442806, 4.74287171305011, 15.5588724571477, -13.1513476357389, -10.3086204444777
                },
                {
                    3.20579525699219, 7.68694549468861, -2.14743991631947, -6.27419139571689, 8.3634719006863,
                    4.74287171305011, 114.921085460961, 10.959828183008, 33.7744841412645, 0.191122258156367
                },
                {
                    6.73063972661051, 1.49722630651441, -10.5157502882462, 14.4105686428956, 20.2864897980169,
                    15.5588724571477, 10.959828183008, 95.2513098133505, 9.88735406142887, -7.72530224406067
                },
                {
                    -5.16467870714877, 5.06944661668959, 5.15232310352067, -7.94837130774331, 16.3966731491597,
                    -13.1513476357389, 33.7744841412645, 9.88735406142887, 94.9216175004985, 7.92004851899833
                },
                {
                    -6.96008775118052, -4.73241157167048, 5.42715572700512, -13.5653060135102, -7.5086811180655,
                    -10.3086204444777, 0.191122258156367, -7.72530224406067, 7.92004851899833, 85.66047052459
                }
            };

            //var testResult = testInput.Cofactor();
            var testAsync = new CofactorSupervisor(testInput);
            var testResult = testAsync.CalcCofactor();

            Console.WriteLine(testResult.Print());
        }

        [Test]
        public void TestPerformance_Inverse()
        {
            var testInput = new[,]
{
                {
                    97.418396137986, 3.40651226544868, -17.0697313630349, 4.18423521542488, -6.32399747806094,
                    -25.4092733216091, 3.20579525699219, 6.73063972661051, -5.16467870714877, -6.96008775118052
                },
                {
                    3.40651226544868, 113.505735745414, 16.4033774812258, -3.86903748868356, 8.75250122903815,
                    -3.30474753757298, 7.68694549468861, 1.49722630651441, 5.06944661668959, -4.73241157167048
                },
                {
                    -17.0697313630349, 16.4033774812258, 78.9323619550698, -6.848424299473, 13.3954431395863,
                    11.9874843367797, -2.14743991631947, -10.5157502882462, 5.15232310352067, 5.42715572700512
                },
                {
                    4.18423521542488, -3.86903748868356, -6.848424299473, 95.7916362934071, -11.5144020370596,
                    12.4488884786246, -6.27419139571689, 14.4105686428956, -7.94837130774331, -13.5653060135102
                },
                {
                    -6.32399747806094, 8.75250122903815, 13.3954431395863, -11.5144020370596, 118.775917531215,
                    2.09217044619191, 8.3634719006863, 20.2864897980169, 16.3966731491597, -7.5086811180655
                },
                {
                    -25.4092733216091, -3.30474753757298, 11.9874843367797, 12.4488884786246, 2.09217044619191,
                    95.3074494442806, 4.74287171305011, 15.5588724571477, -13.1513476357389, -10.3086204444777
                },
                {
                    3.20579525699219, 7.68694549468861, -2.14743991631947, -6.27419139571689, 8.3634719006863,
                    4.74287171305011, 114.921085460961, 10.959828183008, 33.7744841412645, 0.191122258156367
                },
                {
                    6.73063972661051, 1.49722630651441, -10.5157502882462, 14.4105686428956, 20.2864897980169,
                    15.5588724571477, 10.959828183008, 95.2513098133505, 9.88735406142887, -7.72530224406067
                },
                {
                    -5.16467870714877, 5.06944661668959, 5.15232310352067, -7.94837130774331, 16.3966731491597,
                    -13.1513476357389, 33.7744841412645, 9.88735406142887, 94.9216175004985, 7.92004851899833
                },
                {
                    -6.96008775118052, -4.73241157167048, 5.42715572700512, -13.5653060135102, -7.5086811180655,
                    -10.3086204444777, 0.191122258156367, -7.72530224406067, 7.92004851899833, 85.66047052459
                }
            };

            var testResult = testInput.Inverse();
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
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));
        }

        [Test]
        public void TestDeterminant()
        {
            var testInput = new[,]
            {
                {3D, 5D, 1D},
                {9D, 1D, 4D},
                {11D, 13D, 60D}
            };

            var testResult = MatrixExtensions.Determinant(testInput);
            Assert.AreEqual(-2350, testResult);
            testResult = MatrixExtensions.Determinant(new double[,]
            {
                {2, 1},
                {1, 2}
            });
            Assert.AreEqual(3, testResult);

            testInput = new double[,]
            {
                {2, 1, 13, 4},
                {4, 5, 14, 7},
                {7, 12, 9, 10},
                {11, 8, 3, 6},
            };

            testResult = MatrixExtensions.Determinant(testInput);
            Assert.AreEqual(-182D, testResult);

            testInput = new double[,]
            {
                {-0.39235803, 0.67691051, -0.60700301, 0.95378656},
                {-0.58099354, 0.85133302, 0.88056285, -1.96372078},
                {0.83324444, -0.08229884, -2.07132530, 0.51832857},
                {-0.52886612, 2.94227701, 0.14623240, 0.70104920}
            };

            testResult = MatrixExtensions.Determinant(testInput);
            Assert.IsTrue(System.Math.Abs(6.579618 - testResult) < 0.000001);

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
            var testResult = MatrixOps.GetAllOnesMatrix(2, 1);
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
            var testResult = MatrixOps.Product(col, row);

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

            testResult = MatrixOps.Product(matrixOfOnes, typicalMatrix);
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

            Assert.IsTrue(MatrixOps.AreEqual(expectedU, testResult.U));
            Assert.IsTrue(MatrixOps.AreEqual(expectedS, testResult.D.Diag()));
            Assert.IsTrue(MatrixOps.AreEqual(expectedV, testResult.V));

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
            var sumAt = System.Math.Round(testResult.GetRow(0).Sum());
            Assert.AreEqual(1.0D, sumAt);
            sumAt = System.Math.Round(testResult.GetRow(1).Sum());
            Assert.AreEqual(1.0D, sumAt);

        }

        [Test]
        public void TestSwapRow()
        {
            var testInput = MatrixOps.RandomMatrix(5, 5);
            var testRowA = testInput.GetRow(2);
            var testRowB = testInput.GetRow(4);

            testInput.SwapRow(2,4);

            var testResultA = testInput.GetRow(2);
            var testResultB = testInput.GetRow(4);

            for (var i = 0; i < testRowA.Length; i++)
            {
                Assert.AreEqual(testRowA[i], testResultB[i]);
                Assert.AreEqual(testRowB[i], testResultA[i]);
            }
        }

        [Test]
        public void TestSwapColumn()
        {
            var testInput = MatrixOps.RandomMatrix(5, 5);
            var testRowA = testInput.GetColumn(2);
            var testRowB = testInput.GetColumn(4);

            testInput.SwapColumn(2, 4);

            var testResultA = testInput.GetColumn(2);
            var testResultB = testInput.GetColumn(4);

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

            var testResult = testInput.GetRow(1);
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
            var testResult = testInput.GetColumn(1);
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
            var testInput = MatrixOps.RandomMatrix(5, 3);
            var USV = testInput.SingularValueDecomp();
            var U = USV.U;
            var S = USV.D.Diag();
            var V = USV.V;

            var pcXV = testInput.DotProduct(V);
            var pcUdS = U.DotProduct(S);

            Assert.IsTrue(MatrixOps.AreEqual(pcXV, pcUdS));

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

            Assert.IsTrue(MatrixOps.AreEqual(expected, testResult));

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

            Assert.IsTrue(MatrixOps.AreEqual(expected, testResult));

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
            Assert.IsTrue(MatrixOps.AreEqual(expected, testResult));
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

            Assert.IsTrue(MatrixOps.AreEqual(expected, testResult));
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
            Assert.IsTrue(MatrixOps.AreEqual(expected,testResult));
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
            Assert.IsTrue(MatrixOps.AreEqual(expect, P));

        }

        [Test]
        public void TestGaussElimination()
        {
            var testInput = new double[,]
            {
                {1,2,3 },
                { 4,5,6 },
                { 1,0,1}
            };
            var x = new double[] {1,1,1};
            var testResult = MatrixExtensions.GaussElimination(testInput, x);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(0, testResult[0]);
            Assert.AreEqual(-1, testResult[1]);
            Assert.AreEqual(1, testResult[2]);


            testInput = new double[,]
            {
                {2,1,-1 },
                { -3,-1,2 },
                { -2,1,2}
            };
            x = new double[] {8, -11, -3};
            testResult = MatrixExtensions.GaussElimination(testInput, x);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(2, testResult[0]);
            Assert.AreEqual(3, System.Math.Round(testResult[1]));
            Assert.AreEqual(-1, System.Math.Round(testResult[2]));

            testInput = new double[,]
            {
                {0,3,-6,6,4},
                {3,-7,8,-5,8},
                {3,-9,12,-9,6}
            };
            x = new double[] { -5,9,15};
            testResult = MatrixExtensions.GaussElimination(testInput, x);
            Console.WriteLine(testResult.Print());

        }

        [Test]
        public void TestReducedRowEchelonForm()
        {
            var testInput = new double[,]
            {
                {1, 2, -1, -4},
                {2, 3, -1, -11},
                {-2, 0, -3, 22}
            };
            var testResult = testInput.ReducedRowEchelonForm();
            Assert.IsNotNull(testInput);

            var expect = new double[,]
            {
                {1, 0, 0, -8},
                {0, 1, 0, 1},
                {0, 0, 1, -2}
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect,testResult));

            testInput = new double[,]
            {
                {0,3,-6,6,4,-5},
                {3,-7,8,-5,8,9},
                {3,-9,12,-9,6,15}
            };
            testResult = testInput.ReducedRowEchelonForm();
            expect = new double[,]
            {
                {1, 0, -2, 3, 0, -24},
                {0, 1, -2, 2, 0, -7},
                {0, 0, 0, 0, 1, 4}
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

            testInput = new double[,]
            {
                {10, 8, 3, 1},
                {8, 8, 2, 2}
            };
            testResult = testInput.ReducedRowEchelonForm();
            expect = new double[,]
            {
                {1, 0, 0.5, -0.5},
                {0, 1, -0.25, 0.75}
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

        }

        [Test]
        public void TestAppendColumn()
        {
            var testInput = new double[,]
            {
                {1,2,3 },
                { 4,5,6 },
                { 1,0,1}
            };

            var testInput2 = new double[,] {{-1, -2, -3}};
            var testResult = testInput.AppendColumns(testInput2.Transpose());
            
            Assert.IsNotNull(testResult);
            Assert.AreEqual(3, testResult.CountOfRows());
            Assert.AreEqual(4, testResult.CountOfColumns());

            Assert.AreEqual(-1, testResult[0,3]);
            Assert.AreEqual(-2, testResult[1, 3]);
            Assert.AreEqual(-3, testResult[2, 3]);

            Console.WriteLine(testResult.Print());
        }

        [Test]
        public void TestAppendRow()
        {
            var testInput = new double[,]
            {
                {1,2,3 },
                { 4,5,6 },
                { 1,0,1}
            };
            var testInput2 = new double[,] {{ -1, -2, -3 }};
            var testResult = testInput.AppendRows(testInput2);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(4, testResult.CountOfRows());
            Assert.AreEqual(3, testResult.CountOfColumns());

            Assert.AreEqual(-1, testResult[3, 0]);
            Assert.AreEqual(-2, testResult[3, 1]);
            Assert.AreEqual(-3, testResult[3, 2]);
            Console.WriteLine(testResult.Print());

        }

        [Test]
        public void TestSelectMinor()
        {
            var testInput = new double[,]
            {
                {2, 1, 13, 4},
                {4, 5, 14, 7},
                {7, 12, 9, 10},
                {11, 8, 3, 6},
            };
            var testResult = testInput.SelectMinor(0, 0);
            var expect = new double[,]
            {
                {5, 14, 7},
                {12, 9, 10},
                { 8, 3, 6},
            };

            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

            testResult = testInput.SelectMinor(1, 1);
            expect = new double[,]
            {
                {2, 13, 4},
                {7,  9, 10},
                {11, 3, 6},
            };

            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

            testResult = testInput.SelectMinor(2, 2);
            expect = new double[,]
            {
                {2, 1,  4},
                {4, 5,  7},
                {11, 8, 6},
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

            testResult = testInput.SelectMinor(3, 3);
            expect = new double[,]
            {
                {2, 1, 13},
                {4, 5, 14},
                {7, 12, 9},
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

            testResult = testInput.SelectMinor(0, 1);
            expect = new double[,]
            {
                {4, 14, 7},
                {7,  9, 10},
                {11, 3, 6},
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

            testResult = testInput.SelectMinor(0, 2);
            expect = new double[,]
            {
                {4, 5, 7},
                {7, 12,10},
                {11, 8,6},
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

            testResult = testInput.SelectMinor(0, 3);
            expect = new double[,]
            {
                {4, 5, 14},
                {7, 12, 9},
                {11, 8, 3},
            };
            Assert.IsTrue(MatrixOps.AreEqual(expect, testResult));

        }

        [Test]
        public void TestSetRow()
        {
            var testInput = MatrixOps.RandomMatrix(5, 5);
            var testRow = MatrixOps.RandomMatrix(1, 5).Flatten();
            var testResult = testInput.SetRow(2, testRow);
            for (var i = 0; i < testInput.CountOfColumns(); i++)
            {
                Assert.AreEqual(testRow[i], testResult[2,i]);
            }
        }

        [Test]
        public void TestSetColumn()
        {
            var testInput = MatrixOps.RandomMatrix(5, 5);
            var testRow = MatrixOps.RandomMatrix(1, 5).Flatten();
            var testResult = testInput.SetColumn(2, testRow);
            for (var i = 0; i < testInput.CountOfRows(); i++)
            {
                Assert.AreEqual(testRow[i], testResult[i, 2]);
            }
        }

        [Test]
        public void TestShuffleRows()
        {
            var testInput = MatrixOps.RandomMatrix(5, 5);
            var testCtrl = testInput.Copy();
            var testResult = testInput.ShuffleRows();
            for (var i = 0; i < testCtrl.CountOfRows(); i++)
            {
                var testRow = testCtrl.GetRow(i).ToMatrix();
                var testRowResult = false;
                for (var ii = 0; ii < testResult.CountOfRows(); ii++)
                {
                    if (MatrixOps.AreEqual(testRow, testResult.GetRow(ii).ToMatrix()))
                    {
                        testRowResult = true;
                        break;
                    }
                }
                Assert.IsTrue(testRowResult);
            }
        }

        [Test]
        public void TestCompareWithR()
        {
            var y = new double[]
            {
                12.2907482,
                5.8603667,
                -19.7939854,
                43.1083833,
                -4.7916893,
                -13.0534864,
                -1.0612133,
                9.6189661,
                -4.2745122,
                20.9054674,
                -4.1512258,
                -7.2470508,
                6.7772202,
                33.4141183,
                2.1682494,
                32.8438179,
                22.0931035,
                10.8122289,
                21.4973056,
                14.3392405,
                12.7629352,
                22.9334770,
                -32.9514803,
                -18.6818179,
                15.7292513,
                46.3887208,
                -5.0138789,
                4.4685887,
                28.1679582,
                1.2821869,
                -9.1849977,
                -13.4694198,
                2.9559467,
                8.4608247,
                -21.4060125,
                -11.9070198,
                -14.0608597,
                16.0631000,
                6.2757380,
                19.3508799,
                -16.3604264,
                6.7118611,
                -25.4362791,
                14.5928159,
                -14.3890758,
                2.2745100,
                27.5387985,
                -16.9129368,
                1.6725083,
                6.1527828,
                -6.4313572,
                22.5339294,
                10.9879156,
                17.0616106,
                21.2180741,
                -18.1721399,
                -13.2144188,
                -2.2048508,
                1.2020726,
                7.3618541,
                -0.7612235,
                -4.0738674,
                24.7165140,
                10.2064147,
                -20.3290355,
                -8.8811544,
                15.8444721,
                7.6505267,
                12.3248882,
                23.2751086,
                -16.4055580,
                -8.1416819,
                -0.4730301,
                19.7248958,
                15.9474035,
                -8.9728968,
                -1.8692049,
                -32.5193025,
                1.5011803,
                -21.4248465,
                -3.6790593,
                -34.2086292,
                14.3610065,
                14.4100611,
                -16.3953509,
                3.9301198,
                -5.1227098,
                -30.7272006,
                1.3166600,
                8.4358137,
                23.5581840,
                1.2558861,
                7.7172334,
                8.1042170,
                34.4541453,
                -3.9503415,
                0.4710477,
                27.1974428,
                10.8533438,
                21.1348439,
            }.ToMatrix();

            var X = new double[,]
            {
                {
                    -0.39235803, 0.67691051, -0.60700301, 0.95378656, 0.150309077, 1.337924052, 0.106826469,
                    -0.551139255, 0.344146706, -0.28995905
                },
                {
                    -0.58099354, 0.85133302, 0.88056285, -1.96372078, 0.542365605, -0.330473917, -1.006172601,
                    -0.879466915, 1.003918566, 1.36355229
                },
                {
                    0.83324444, -0.08229884, -2.07132530, 0.51832857, -2.118171415, -0.935485084, 0.729984816,
                    -0.098755005, -0.213913282, -1.62624076
                },
                {
                    -0.52886612, 2.94227701, 0.14623240, 0.70104920, 0.492201272, 1.126886424, 0.095210388, 0.876822682,
                    2.221233325, -0.43415277
                },
                {
                    0.67270457, 0.87816916, 0.93944267, -2.05091722, 0.293027476, -1.872608537, 0.144400978,
                    -0.536910088, 0.041069752, -0.29669860
                },
                {
                    0.84953620, -1.30357242, -0.78447000, -0.73396957, 1.659233779, -1.085173437, -1.039354319,
                    -1.183035363, 0.365360116, -0.22839805
                },
                {
                    -0.39540732, -0.13204071, -0.81070982, 0.14876678, 0.799578666, -1.108120894, 0.419276382,
                    -0.770162886, -0.013382920, 0.69480343
                },
                {
                    0.40441393, -1.03631718, -0.03828300, 0.24556036, 0.458883725, -0.667364331, 0.388910829,
                    0.466781009, 0.783871664, -0.14498383
                },
                {
                    -1.22379332, 0.06890476, -0.79588976, -0.43851028, 1.295272449, -1.483300097, -0.727095625,
                    0.040421758, 0.662212278, 0.77084298
                },
                {
                    1.54213423, -0.10760710, -0.46472607, 0.44859829, -0.320026010, 1.790981782, -0.547542322,
                    0.822101628, 0.713117468, -0.39438256
                },
                {
                    0.09285680, -0.11727453, 1.06607177, 0.43981603, -1.726539849, 0.086716552, 1.047269336,
                    -2.091718222, -0.706556121, 0.06944322
                },
                {
                    -0.74306163, -1.76123260, -0.58381054, -1.50623537, -1.204884761, -1.165231578, 2.745872187,
                    -0.370657836, 1.419636397, 0.58128796
                },
                {
                    -0.59084051, -0.49946470, -0.12316155, 1.66535027, -0.553153276, 0.445528948, -0.589482781,
                    1.457453271, -0.720335413, 0.09345593
                },
                {
                    0.28635188, -0.74890032, 0.75432336, 0.58984117, 1.276584905, 1.904760724, 0.120907394, 0.888769833,
                    -0.022210117, 0.71706588
                },
                {
                    0.95904364, 0.43233475, 0.41975380, 0.20762083, -1.208592103, 0.328728655, 0.664875676,
                    -0.275902268, -1.302263111, -1.21428720
                },
                {
                    -0.46080403, 1.01158702, -0.15204879, 1.32504963, 0.740081772, -0.309206686, 0.155208845,
                    2.223884605, 0.653513220, 0.85998622
                },
                {
                    0.79516950, -0.72299945, 1.06986600, 2.34402793, -1.292812898, -0.028237942, -0.190995981,
                    0.935215278, 1.133189847, -0.74903370
                },
                {
                    -0.42838636, -0.50562960, 0.36970387, -0.32455634, 0.721128111, -0.792008387, 0.848917435,
                    1.032111159, 0.786820503, -0.72925529
                },
                {
                    -0.21379228, -0.63493295, 0.23309238, 1.81052692, 0.720590299, -0.033543807, 0.744962690,
                    -0.627642437, -0.620063822, 1.78173307
                },
                {
                    -0.60975006, -0.81495339, 0.82214880, -0.83091653, 1.701433153, 0.510519893, -0.538205346,
                    -0.408335307, 1.272251959, 0.74933221
                },
                {
                    1.20509011, 0.07462813, 0.04448975, 0.78761093, 1.387795041, 0.546524438, -1.437568733, 0.695546493,
                    -0.273318217, -1.44005667
                },
                {
                    -0.11553684, 2.69385191, 0.51974594, 1.89429317, 0.667801849, 0.054007524, -0.324661056,
                    -0.993722693, 0.567512613, -1.23731183
                },
                {
                    -1.86195115, 0.88068591, 0.86340122, -1.59280018, 0.388208275, -0.484307958, -1.528490110,
                    -1.335458788, -1.930396510, -1.22479947
                },
                {
                    1.00366788, 0.19215510, -1.42496552, 0.51953730, 0.407087657, -2.061719776, -0.742959527,
                    0.694813696, -0.004220685, -3.15529744
                },
                {
                    -1.69646385, 0.49666692, 1.25293958, -0.01416900, -0.304548112, 1.770422482, -0.067444473,
                    -0.388126726, -0.312715760, 1.45366419
                },
                {
                    0.06323255, 2.11100602, 0.07035758, 1.68686025, 1.706298969, -0.664313348, 2.009085921, 0.786438529,
                    0.360198118, 0.02679652
                },
                {
                    -0.28165060, -0.59919363, 0.15306944, -2.73895231, -1.017574299, -0.628164834, -0.363997262,
                    2.128654070, 0.429204068, 0.85236443
                },
                {
                    -0.32421155, -0.12361116, -1.07164314, -1.27052495, 1.487649260, 0.491400260, 0.018873675,
                    0.655928200, -0.134189117, 0.00066251
                },
                {
                    -0.59605239, -0.84271300, 1.38556983, 0.57975364, 0.264085862, 0.624682130, 0.401281444,
                    1.370948469, 0.131378361, 1.07599390
                },
                {
                    -1.73589996, -0.98521549, 1.36956634, 1.40005511, -0.787442319, 0.766531234, -0.014426777,
                    -0.399012694, -0.525871830, 0.11523821
                },
                {
                    -1.38164981, -0.31340767, 0.80539684, -0.67696611, -0.757269924, 0.811843323, 0.271880645,
                    -1.814630982, -0.496508290, 0.61747127
                },
                {
                    -0.25015472, -0.70999678, 0.22824765, -0.83109211, -0.307071044, 1.736159798, -0.114313990,
                    -0.129396208, -2.068557347, -0.73896309
                },
                {
                    0.33088919, 0.38615557, 0.14304822, -0.49430662, 0.847035313, 0.238285620, -0.417968426,
                    -0.550766779, -0.079917729, -1.34118497
                },
                {
                    -1.87698999, -0.12956206, -0.04910457, 0.27977503, 0.232044250, 0.649023098, -0.034861872,
                    -0.128872571, 1.656639194, -0.08959335
                },
                {
                    -0.72578247, -0.87552891, -0.01665383, 1.40728116, -1.557512667, -1.363040970, -0.570628228,
                    -0.749714400, -0.389924356, -0.45542952
                },
                {
                    -1.80013137, 1.24797211, -0.30974580, -0.53142028, -1.285370940, -0.131894905, -1.110326168,
                    -0.694860100, 0.060539696, 1.17103987
                },
                {
                    -0.53044268, -1.17877117, -0.09929729, 1.59526869, -2.162773840, 1.122786735, -1.155373808,
                    -1.958055952, 0.939110591, -0.37255000
                },
                {
                    0.03591824, 0.24212344, 0.42681850, -0.30307986, -0.448453951, -0.105574923, 0.754528004,
                    0.903592868, 0.377880918, 0.76329700
                },
                {
                    1.56787879, 0.12800992, 0.16408301, -0.17249405, 0.715300128, -2.733414869, -1.539179168,
                    0.252082562, 0.659800120, 1.26976527
                },
                {
                    0.25123806, 0.78120437, 2.01504873, 0.73139458, -0.093188201, -0.253000483, -0.395021805,
                    -0.001673113, -0.764496389, 0.42643623
                },
                {
                    0.59143459, 0.80784157, 1.15474927, -2.32159667, -1.069214185, 0.620977623, -2.432114133,
                    -1.598296819, -0.778812162, 0.67698055
                },
                {
                    -1.72710310, 0.18473475, 1.30424508, -0.90260834, 0.369162193, 1.212320571, -0.713923471,
                    -0.036208006, 0.658508119, -0.15252511
                },
                {
                    -0.38557228, -2.09542062, 0.03897722, 0.15369845, -1.777949882, -0.552925882, 0.384873042,
                    -1.144110973, 0.237271369, -0.99424933
                },
                {
                    0.85793612, -0.36839566, -0.19636834, -0.07820997, 0.688911463, -1.095880162, -0.184211698,
                    0.598964993, 1.122254502, 0.83483171
                },
                {
                    1.56015737, -0.83043334, -0.86285385, -1.67309929, -0.525976716, -1.734697770, -0.424358385,
                    -1.145460568, 0.708250802, 1.41521427
                },
                {
                    1.73876772, 0.87331061, -1.51482600, -0.14942081, -1.431856890, 0.477192188, -0.704966306,
                    -0.868880942, 0.364034815, 0.74344527
                },
                {
                    -0.75412771, 0.37657136, -0.37742569, -0.86293383, 2.938233027, 0.139758263, 2.577210058,
                    -1.460124681, 1.978540959, -0.10066852
                },
                {
                    -0.76064688, -0.09261152, -0.81466511, 0.28697291, -0.574277204, -0.990411084, -1.325801538,
                    -0.812390624, -1.219318386, 1.88066938
                },
                {
                    0.64701300, -1.58223620, 1.01177664, 1.12335259, 0.498311347, 0.684274483, -0.838881692,
                    0.356213715, -1.268831276, -1.54631073
                },
                {
                    -0.39891881, 0.42849982, -0.80592781, 0.72960694, -0.593947096, 0.761325177, 0.521569025,
                    0.587210996, -1.559088671, 0.95878366
                },
                {
                    -0.17085504, -0.79544324, -0.39393953, -0.48809008, 2.039518269, -1.194151573, -1.086216732,
                    0.565721731, -0.604865911, -0.53957191
                },
                {
                    -0.07751354, 0.43897905, -0.24981397, -0.41121718, 0.373874203, -0.952869282, 0.340269861,
                    0.563689336, 2.356439894, 0.86218760
                },
                {
                    -0.38217548, 1.19558515, -0.27842688, 0.24932481, 0.347256175, -0.248581065, 1.336022914,
                    -0.506857745, 1.057662270, -1.90400102
                },
                {
                    0.33431069, 0.40536093, 0.50416253, -0.80896182, 1.490799039, 0.936373985, 0.437725753, 0.076332562,
                    -0.101981299, -0.70867562
                },
                {
                    -0.71009331, 2.24459903, -0.02851483, 1.23697705, 0.301854532, 0.300929660, -0.201955208,
                    1.440528767, 0.069148976, -1.49233963
                },
                {
                    -1.43739801, 0.27390578, -0.24422536, 0.04708417, -1.142534795, -0.189098166, -1.173079825,
                    -0.014867615, -0.302772248, -0.29379984
                },
                {
                    -0.90040293, 0.65884256, 0.83445822, -0.70300807, 1.174390852, -1.508040701, -1.895817382,
                    -2.278185927, 0.587581947, 0.31531808
                },
                {
                    0.01795927, 1.87906610, 0.77389582, 0.10832662, 0.706666046, -0.589363138, -2.442972192,
                    -0.970695388, 0.192443325, -0.92976406
                },
                {
                    0.55387415, 1.20834822, 0.06448302, 0.31927350, -1.983582377, 0.467196045, -0.405731754,
                    0.365311915, -1.265534711, 0.23433970
                },
                {
                    -0.42325139, -0.70891670, 1.99174205, -0.67141713, 0.064663915, -0.553876634, -0.805681469,
                    0.563852886, 1.255965780, -0.18539781
                },
                {
                    0.42002074, -0.63123802, -1.00757785, 0.12377222, -1.078700753, -0.583229290, -0.258783952,
                    0.629235279, 1.640990305, -0.63736328
                },
                {
                    -0.68597883, 2.08438339, -0.53844580, -0.70336606, 0.270228494, -1.460041490, 1.249297035,
                    -0.107821540, -1.261846678, -0.37482034
                },
                {
                    0.55082064, 3.47454938, 0.75953380, -0.37527921, 0.854294961, -0.019065831, -0.112858003,
                    0.641807159, -0.698810086, -1.16556915
                },
                {
                    0.67363422, 0.92667332, -0.40761311, -0.78340315, 0.325832030, -0.217852794, 0.914575937,
                    0.426537809, -0.874743903, 0.11850882
                },
                {
                    0.72083907, -0.90100834, 0.56345600, -0.06060153, 0.041604260, 0.411908826, -2.817485963,
                    -1.925084275, -1.797551782, 0.40145147
                },
                {
                    -0.05367313, -0.21507539, -0.59114266, -0.65884396, 0.692688105, 0.071151521, -1.592912245,
                    1.602948686, -1.534699948, -0.12867356
                },
                {
                    -0.40420730, 1.25888437, 1.05721179, -0.45952940, -0.694337956, 1.356751397, 1.496704657,
                    -0.306607950, 0.481369080, -1.45621666
                },
                {
                    -0.87727647, -0.59631961, 0.83107203, 0.26779254, 0.277020226, -0.311025528, 1.637654835,
                    -0.741830423, -0.215280581, 0.56793392
                },
                {
                    0.78446370, -0.87307521, 0.01672474, -1.14851577, 1.910676004, -0.648203475, 1.486770554,
                    0.383981480, -0.385276882, -0.01836254
                },
                {
                    3.07457722, 1.01247454, -1.90099380, 0.06060911, -0.679929835, -0.429900139, 1.182808651,
                    0.432668989, 0.332198975, 0.51269742
                },
                {
                    0.89540586, 0.11192696, -1.92281834, 0.33136616, -1.920326931, -0.282083153, -1.754321387,
                    0.585920286, -1.076439182, 0.71346486
                },
                {
                    -0.15352171, -0.30784073, -0.37815924, -0.22919586, -0.221292110, -0.214288887, 0.260614568,
                    0.172797625, 0.149472696, -1.50893834
                },
                {
                    -0.13748651, 0.20297604, 1.61272128, -0.30604831, -0.400704203, -0.454324055, 0.009456687,
                    -0.511930002, -1.728940368, 0.34322336
                },
                {
                    1.02689258, -0.87994433, -0.57051108, 0.58248774, 0.302441093, 0.002714403, -0.149248097,
                    0.899665717, -0.059236677, 1.29484382
                },
                {
                    0.33349659, 0.45795310, 1.23586005, -0.08127050, 0.322413495, -0.808606069, 0.970491060,
                    -1.840693736, 1.166161435, 0.13762804
                },
                {
                    0.99011775, -1.33352835, -0.59572043, 1.50236274, 0.625403159, -0.565043640, -2.000573595,
                    1.175837724, -0.586520381, -2.06566025
                },
                {
                    -0.82846905, -0.73789931, -1.28407469, 0.11977088, 1.305735659, 0.911117623, -0.395882461,
                    -0.329289279, -0.385162625, 0.04747610
                },
                {
                    -0.56680584, -0.18543460, -0.71160315, -0.68992989, -2.358790226, -1.298322113, -0.307484620,
                    -1.186657999, -0.271367290, -0.21847769
                },
                {
                    1.26585100, -1.12212197, -0.94291124, 0.88842810, -1.456205867, -0.814762571, 0.580487677,
                    -0.367277223, 0.850938328, 0.24940092
                },
                {
                    -1.54687502, -0.40245997, -1.44758108, 1.10010079, -0.758968836, -0.875871472, -2.184631930,
                    0.722924884, -1.171039296, 1.04561975
                },
                {
                    0.07668625, -0.28206370, 0.02370704, -1.03461487, 1.114272923, -0.535649917, -0.479044600,
                    -0.369925285, -0.226334630, 0.30416139
                },
                {
                    1.44807618, -0.73387729, -0.99403333, 0.71036902, -2.409919625, -1.135921517, -1.764605239,
                    -1.438025826, -1.169272292, -0.45624891
                },
                {
                    -1.18371766, 0.02925959, -0.29821410, 0.32332958, -0.157588046, -1.423424447, 0.907609826,
                    0.869033355, 2.016807916, 1.01396096
                },
                {
                    -0.42242656, 0.34541902, 1.19502539, -0.18160170, -0.887348579, -0.594153604, 0.046583522,
                    -0.402640204, 1.321623063, 1.26204667
                },
                {
                    0.59766250, -1.18160821, -0.68363340, -0.48271635, -1.868324718, -0.184949028, 0.988388811,
                    -0.779258858, -0.384576697, -0.62036154
                },
                {
                    -2.47908177, -0.61747098, 1.08761485, -0.51914122, 0.598995210, 1.006253734, -0.238964568,
                    0.651173494, 1.230058905, -1.14397725
                },
                {
                    0.44431243, 0.61245792, 0.29072442, -0.07868109, -0.804846373, -1.423681474, -0.807531337,
                    -0.951447248, -0.339073164, 0.83472462
                },
                {
                    -1.91350737, -1.79610491, -1.05395623, -0.24689863, 0.196987030, 1.104832483, -0.707314773,
                    -1.689172078, -0.787485219, -0.43791530
                },
                {
                    -1.43374452, 1.04423637, 1.24683999, 1.24796642, -0.112817451, -0.017082821, -1.543351375,
                    -0.902068160, -0.664567345, 0.70580478
                },
                {
                    -0.35618277, -1.23533900, 0.12829825, 0.07666326, -0.353324785, -0.067368196, 0.631537558,
                    0.688543948, 1.840819520, -0.48988408
                },
                {
                    0.49347940, 0.75995968, -0.55394486, 0.74981039, -0.414999240, -0.164015016, 1.483121493,
                    -0.751756088, 0.815696967, 1.10879916
                },
                {
                    -1.09018731, -0.62397142, -0.78896593, -0.47892624, -0.769408654, 2.348562443, 0.336696344,
                    1.273282594, -0.814017967, -0.09651482
                },
                {
                    0.43286144, 1.05704657, -0.58064456, 0.23820937, 0.253475348, -0.478258840, 1.647622094,
                    -0.667665238, -0.615705620, -0.87384375
                },
                {
                    -1.68709651, -0.49695685, -1.23105928, -0.18762712, 0.511255485, 2.735422524, 0.034078942,
                    1.666084971, 0.083565438, -1.19547788
                },
                {
                    0.74607688, 2.11152678, 1.60939659, -0.57540320, -0.759229447, 0.337037459, 0.631916627,
                    1.338453222, 0.469212007, 0.24705409
                },
                {
                    -0.47754237, -2.16302185, -0.93627223, 2.48423994, 0.959544911, 0.937100173, -1.190915666,
                    0.885291843, -1.694871154, -0.62939660
                },
                {
                    0.25763637, 0.28951722, -0.53366143, -1.93213414, 0.072689075, 0.179788173, 0.807194749,
                    0.037730955, -0.121075461, 0.02406905
                },
                {
                    1.51146090, 0.19245415, 1.16280417, 0.44890804, 1.400734131, -0.031407349, -0.305883092,
                    0.664809881, -0.083297538, -0.47792930
                },
                {
                    -1.38261325, -0.11030994, -0.27133260, -0.64885346, -0.009935175, 0.379915467, 0.221344721,
                    0.861094136, 1.303806235, 0.57919403
                },
                {
                    0.67399056, -1.62171765, 0.48131849, -0.49834282, 2.074792370, -0.204552426, 0.963090976,
                    0.497830032, 0.866183651, 0.04976176
                },
            };

            var crossProduct = X.CrossProduct();

            var expect = new double[,]
            {
                {
                    97.418396, 3.406512, -17.069731, 4.184235, -6.323997, -25.409273, 3.2057953, 6.730640, -5.164679,
                    -6.9600878
                },
                {
                    3.406512, 113.505736, 16.403377, -3.869037, 8.752501, -3.304748, 7.6869455, 1.497226, 5.069447,
                    -4.7324116
                },
                {
                    -17.069731, 16.403377, 78.932362, -6.848424, 13.395443, 11.987484, -2.1474399, -10.515750, 5.152323,
                    5.4271557
                },
                {
                    4.184235, -3.869037, -6.848424, 95.791636, -11.514402, 12.448888, -6.2741914, 14.410569, -7.948371,
                    -13.5653060
                },
                {
                    -6.323997, 8.752501, 13.395443, -11.514402, 118.775918, 2.092170, 8.3634719, 20.286490, 16.396673,
                    -7.5086811
                },
                {
                    -25.409273, -3.304748, 11.987484, 12.448888, 2.092170, 95.307449, 4.7428717, 15.558872, -13.151348,
                    -10.3086204
                },
                {
                    3.205795, 7.686945, -2.147440, -6.274191, 8.363472, 4.742872, 114.9210855, 10.959828, 33.774484,
                    0.1911223
                },
                {
                    6.730640, 1.497226, -10.515750, 14.410569, 20.286490, 15.558872, 10.9598282, 95.251310, 9.887354,
                    -7.7253022
                },
                {
                    -5.164679, 5.069447, 5.152323, -7.948371, 16.396673, -13.151348, 33.7744841, 9.887354, 94.921618,
                    7.9200485
                },
                {
                    -6.960088, -4.732412, 5.427156, -13.565306, -7.508681, -10.308620, 0.1911223, -7.725302, 7.920049,
                    85.6604705
                },
            };

            Assert.IsTrue(MatrixOps.AreEqual(expect, crossProduct, 0.000001));

            expect = new double[,]
            {
                {
                    0.0117756013, -4.969505e-04, 0.0016664204, -0.0004229649, 5.255409e-04, 0.0034685274, -7.152904e-04,
                    -0.0012191572, 1.229373e-03, 9.982362e-04
                },
                {
                    -0.0004969505, 9.257163e-03, -0.0021395723, 0.0002220649, -3.152504e-04, 0.0005723912,
                    -5.878097e-04, -0.0002776113, -7.042725e-05, 6.658031e-04
                },
                {
                    0.0016664204, -2.139572e-03, 0.0145330861, 0.0004726787, -1.645343e-03, -0.0020878175, 6.754417e-04,
                    0.0020815633, -9.270863e-04, -9.522582e-04
                },
                {
                    -0.0004229649, 2.220649e-04, 0.0004726787, 0.0113614201, 1.330539e-03, -0.0012227861, 6.485862e-04,
                    -0.0017065079, 3.106842e-04, 1.532566e-03
                },
                {
                    0.0005255409, -3.152504e-04, -0.0016453430, 0.0013305393, 9.399955e-03, 0.0002856805, -9.365445e-05,
                    -0.0022427130, -1.163194e-03, 1.104076e-03
                },
                {
                    0.0034685274, 5.723912e-04, -0.0020878175, -0.0012227861, 2.856805e-04, 0.0128100875, -1.292103e-03,
                    -0.0024521760, 2.495460e-03, 1.369732e-03
                },
                {
                    -0.0007152904, -5.878097e-04, 0.0006754417, 0.0006485862, -9.365445e-05, -0.0012921031,
                    9.992801e-03, -0.0004960556, -3.663081e-03, 7.727051e-05
                },
                {
                    -0.0012191572, -2.776113e-04, 0.0020815633, -0.0017065079, -2.242713e-03, -0.0024521760,
                    -4.960556e-04, 0.0121720657, -1.369240e-03, 2.172331e-04
                },
                {
                    0.0012293728, -7.042725e-05, -0.0009270863, 0.0003106842, -1.163194e-03, 0.0024954598,
                    -3.663081e-03, -0.0013692397, 1.274908e-02, -8.917897e-04
                },
                {
                    0.0009982362, 6.658031e-04, -0.0009522582, 0.0015325662, 1.104076e-03, 0.0013697320, 7.727051e-05,
                    0.0002172331, -8.917897e-04, 1.245841e-02
                },
            };

            var inverse = crossProduct.Inverse();
            Assert.IsTrue(MatrixOps.AreEqual(expect, inverse));

            var coefficents = inverse.DotProduct(X.Transpose()).DotProduct(y.Transpose());
            Console.WriteLine(coefficents.Print());

            expect = new double[]
            {
                4.209541,
                4.992066,
                5.001884,
                5.046863,
                4.925452,
                4.354024,
                4.393291,
                4.707941,
                5.506140,
                4.442644
            }.ToMatrix().Transpose();

            Assert.IsTrue(MatrixOps.AreEqual(expect, coefficents, 0.000001));
        }

        [Test]
        public void TestLinearGradientDescent()
        {
            var y = new double[]
{
                12.2907482,
                5.8603667,
                -19.7939854,
                43.1083833,
                -4.7916893,
                -13.0534864,
                -1.0612133,
                9.6189661,
                -4.2745122,
                20.9054674,
                -4.1512258,
                -7.2470508,
                6.7772202,
                33.4141183,
                2.1682494,
                32.8438179,
                22.0931035,
                10.8122289,
                21.4973056,
                14.3392405,
                12.7629352,
                22.9334770,
                -32.9514803,
                -18.6818179,
                15.7292513,
                46.3887208,
                -5.0138789,
                4.4685887,
                28.1679582,
                1.2821869,
                -9.1849977,
                -13.4694198,
                2.9559467,
                8.4608247,
                -21.4060125,
                -11.9070198,
                -14.0608597,
                16.0631000,
                6.2757380,
                19.3508799,
                -16.3604264,
                6.7118611,
                -25.4362791,
                14.5928159,
                -14.3890758,
                2.2745100,
                27.5387985,
                -16.9129368,
                1.6725083,
                6.1527828,
                -6.4313572,
                22.5339294,
                10.9879156,
                17.0616106,
                21.2180741,
                -18.1721399,
                -13.2144188,
                -2.2048508,
                1.2020726,
                7.3618541,
                -0.7612235,
                -4.0738674,
                24.7165140,
                10.2064147,
                -20.3290355,
                -8.8811544,
                15.8444721,
                7.6505267,
                12.3248882,
                23.2751086,
                -16.4055580,
                -8.1416819,
                -0.4730301,
                19.7248958,
                15.9474035,
                -8.9728968,
                -1.8692049,
                -32.5193025,
                1.5011803,
                -21.4248465,
                -3.6790593,
                -34.2086292,
                14.3610065,
                14.4100611,
                -16.3953509,
                3.9301198,
                -5.1227098,
                -30.7272006,
                1.3166600,
                8.4358137,
                23.5581840,
                1.2558861,
                7.7172334,
                8.1042170,
                34.4541453,
                -3.9503415,
                0.4710477,
                27.1974428,
                10.8533438,
                21.1348439};

            var X = new double[,]
            {
                {
                    -0.39235803, 0.67691051, -0.60700301, 0.95378656, 0.150309077, 1.337924052, 0.106826469,
                    -0.551139255, 0.344146706, -0.28995905
                },
                {
                    -0.58099354, 0.85133302, 0.88056285, -1.96372078, 0.542365605, -0.330473917, -1.006172601,
                    -0.879466915, 1.003918566, 1.36355229
                },
                {
                    0.83324444, -0.08229884, -2.07132530, 0.51832857, -2.118171415, -0.935485084, 0.729984816,
                    -0.098755005, -0.213913282, -1.62624076
                },
                {
                    -0.52886612, 2.94227701, 0.14623240, 0.70104920, 0.492201272, 1.126886424, 0.095210388, 0.876822682,
                    2.221233325, -0.43415277
                },
                {
                    0.67270457, 0.87816916, 0.93944267, -2.05091722, 0.293027476, -1.872608537, 0.144400978,
                    -0.536910088, 0.041069752, -0.29669860
                },
                {
                    0.84953620, -1.30357242, -0.78447000, -0.73396957, 1.659233779, -1.085173437, -1.039354319,
                    -1.183035363, 0.365360116, -0.22839805
                },
                {
                    -0.39540732, -0.13204071, -0.81070982, 0.14876678, 0.799578666, -1.108120894, 0.419276382,
                    -0.770162886, -0.013382920, 0.69480343
                },
                {
                    0.40441393, -1.03631718, -0.03828300, 0.24556036, 0.458883725, -0.667364331, 0.388910829,
                    0.466781009, 0.783871664, -0.14498383
                },
                {
                    -1.22379332, 0.06890476, -0.79588976, -0.43851028, 1.295272449, -1.483300097, -0.727095625,
                    0.040421758, 0.662212278, 0.77084298
                },
                {
                    1.54213423, -0.10760710, -0.46472607, 0.44859829, -0.320026010, 1.790981782, -0.547542322,
                    0.822101628, 0.713117468, -0.39438256
                },
                {
                    0.09285680, -0.11727453, 1.06607177, 0.43981603, -1.726539849, 0.086716552, 1.047269336,
                    -2.091718222, -0.706556121, 0.06944322
                },
                {
                    -0.74306163, -1.76123260, -0.58381054, -1.50623537, -1.204884761, -1.165231578, 2.745872187,
                    -0.370657836, 1.419636397, 0.58128796
                },
                {
                    -0.59084051, -0.49946470, -0.12316155, 1.66535027, -0.553153276, 0.445528948, -0.589482781,
                    1.457453271, -0.720335413, 0.09345593
                },
                {
                    0.28635188, -0.74890032, 0.75432336, 0.58984117, 1.276584905, 1.904760724, 0.120907394, 0.888769833,
                    -0.022210117, 0.71706588
                },
                {
                    0.95904364, 0.43233475, 0.41975380, 0.20762083, -1.208592103, 0.328728655, 0.664875676,
                    -0.275902268, -1.302263111, -1.21428720
                },
                {
                    -0.46080403, 1.01158702, -0.15204879, 1.32504963, 0.740081772, -0.309206686, 0.155208845,
                    2.223884605, 0.653513220, 0.85998622
                },
                {
                    0.79516950, -0.72299945, 1.06986600, 2.34402793, -1.292812898, -0.028237942, -0.190995981,
                    0.935215278, 1.133189847, -0.74903370
                },
                {
                    -0.42838636, -0.50562960, 0.36970387, -0.32455634, 0.721128111, -0.792008387, 0.848917435,
                    1.032111159, 0.786820503, -0.72925529
                },
                {
                    -0.21379228, -0.63493295, 0.23309238, 1.81052692, 0.720590299, -0.033543807, 0.744962690,
                    -0.627642437, -0.620063822, 1.78173307
                },
                {
                    -0.60975006, -0.81495339, 0.82214880, -0.83091653, 1.701433153, 0.510519893, -0.538205346,
                    -0.408335307, 1.272251959, 0.74933221
                },
                {
                    1.20509011, 0.07462813, 0.04448975, 0.78761093, 1.387795041, 0.546524438, -1.437568733, 0.695546493,
                    -0.273318217, -1.44005667
                },
                {
                    -0.11553684, 2.69385191, 0.51974594, 1.89429317, 0.667801849, 0.054007524, -0.324661056,
                    -0.993722693, 0.567512613, -1.23731183
                },
                {
                    -1.86195115, 0.88068591, 0.86340122, -1.59280018, 0.388208275, -0.484307958, -1.528490110,
                    -1.335458788, -1.930396510, -1.22479947
                },
                {
                    1.00366788, 0.19215510, -1.42496552, 0.51953730, 0.407087657, -2.061719776, -0.742959527,
                    0.694813696, -0.004220685, -3.15529744
                },
                {
                    -1.69646385, 0.49666692, 1.25293958, -0.01416900, -0.304548112, 1.770422482, -0.067444473,
                    -0.388126726, -0.312715760, 1.45366419
                },
                {
                    0.06323255, 2.11100602, 0.07035758, 1.68686025, 1.706298969, -0.664313348, 2.009085921, 0.786438529,
                    0.360198118, 0.02679652
                },
                {
                    -0.28165060, -0.59919363, 0.15306944, -2.73895231, -1.017574299, -0.628164834, -0.363997262,
                    2.128654070, 0.429204068, 0.85236443
                },
                {
                    -0.32421155, -0.12361116, -1.07164314, -1.27052495, 1.487649260, 0.491400260, 0.018873675,
                    0.655928200, -0.134189117, 0.00066251
                },
                {
                    -0.59605239, -0.84271300, 1.38556983, 0.57975364, 0.264085862, 0.624682130, 0.401281444,
                    1.370948469, 0.131378361, 1.07599390
                },
                {
                    -1.73589996, -0.98521549, 1.36956634, 1.40005511, -0.787442319, 0.766531234, -0.014426777,
                    -0.399012694, -0.525871830, 0.11523821
                },
                {
                    -1.38164981, -0.31340767, 0.80539684, -0.67696611, -0.757269924, 0.811843323, 0.271880645,
                    -1.814630982, -0.496508290, 0.61747127
                },
                {
                    -0.25015472, -0.70999678, 0.22824765, -0.83109211, -0.307071044, 1.736159798, -0.114313990,
                    -0.129396208, -2.068557347, -0.73896309
                },
                {
                    0.33088919, 0.38615557, 0.14304822, -0.49430662, 0.847035313, 0.238285620, -0.417968426,
                    -0.550766779, -0.079917729, -1.34118497
                },
                {
                    -1.87698999, -0.12956206, -0.04910457, 0.27977503, 0.232044250, 0.649023098, -0.034861872,
                    -0.128872571, 1.656639194, -0.08959335
                },
                {
                    -0.72578247, -0.87552891, -0.01665383, 1.40728116, -1.557512667, -1.363040970, -0.570628228,
                    -0.749714400, -0.389924356, -0.45542952
                },
                {
                    -1.80013137, 1.24797211, -0.30974580, -0.53142028, -1.285370940, -0.131894905, -1.110326168,
                    -0.694860100, 0.060539696, 1.17103987
                },
                {
                    -0.53044268, -1.17877117, -0.09929729, 1.59526869, -2.162773840, 1.122786735, -1.155373808,
                    -1.958055952, 0.939110591, -0.37255000
                },
                {
                    0.03591824, 0.24212344, 0.42681850, -0.30307986, -0.448453951, -0.105574923, 0.754528004,
                    0.903592868, 0.377880918, 0.76329700
                },
                {
                    1.56787879, 0.12800992, 0.16408301, -0.17249405, 0.715300128, -2.733414869, -1.539179168,
                    0.252082562, 0.659800120, 1.26976527
                },
                {
                    0.25123806, 0.78120437, 2.01504873, 0.73139458, -0.093188201, -0.253000483, -0.395021805,
                    -0.001673113, -0.764496389, 0.42643623
                },
                {
                    0.59143459, 0.80784157, 1.15474927, -2.32159667, -1.069214185, 0.620977623, -2.432114133,
                    -1.598296819, -0.778812162, 0.67698055
                },
                {
                    -1.72710310, 0.18473475, 1.30424508, -0.90260834, 0.369162193, 1.212320571, -0.713923471,
                    -0.036208006, 0.658508119, -0.15252511
                },
                {
                    -0.38557228, -2.09542062, 0.03897722, 0.15369845, -1.777949882, -0.552925882, 0.384873042,
                    -1.144110973, 0.237271369, -0.99424933
                },
                {
                    0.85793612, -0.36839566, -0.19636834, -0.07820997, 0.688911463, -1.095880162, -0.184211698,
                    0.598964993, 1.122254502, 0.83483171
                },
                {
                    1.56015737, -0.83043334, -0.86285385, -1.67309929, -0.525976716, -1.734697770, -0.424358385,
                    -1.145460568, 0.708250802, 1.41521427
                },
                {
                    1.73876772, 0.87331061, -1.51482600, -0.14942081, -1.431856890, 0.477192188, -0.704966306,
                    -0.868880942, 0.364034815, 0.74344527
                },
                {
                    -0.75412771, 0.37657136, -0.37742569, -0.86293383, 2.938233027, 0.139758263, 2.577210058,
                    -1.460124681, 1.978540959, -0.10066852
                },
                {
                    -0.76064688, -0.09261152, -0.81466511, 0.28697291, -0.574277204, -0.990411084, -1.325801538,
                    -0.812390624, -1.219318386, 1.88066938
                },
                {
                    0.64701300, -1.58223620, 1.01177664, 1.12335259, 0.498311347, 0.684274483, -0.838881692,
                    0.356213715, -1.268831276, -1.54631073
                },
                {
                    -0.39891881, 0.42849982, -0.80592781, 0.72960694, -0.593947096, 0.761325177, 0.521569025,
                    0.587210996, -1.559088671, 0.95878366
                },
                {
                    -0.17085504, -0.79544324, -0.39393953, -0.48809008, 2.039518269, -1.194151573, -1.086216732,
                    0.565721731, -0.604865911, -0.53957191
                },
                {
                    -0.07751354, 0.43897905, -0.24981397, -0.41121718, 0.373874203, -0.952869282, 0.340269861,
                    0.563689336, 2.356439894, 0.86218760
                },
                {
                    -0.38217548, 1.19558515, -0.27842688, 0.24932481, 0.347256175, -0.248581065, 1.336022914,
                    -0.506857745, 1.057662270, -1.90400102
                },
                {
                    0.33431069, 0.40536093, 0.50416253, -0.80896182, 1.490799039, 0.936373985, 0.437725753, 0.076332562,
                    -0.101981299, -0.70867562
                },
                {
                    -0.71009331, 2.24459903, -0.02851483, 1.23697705, 0.301854532, 0.300929660, -0.201955208,
                    1.440528767, 0.069148976, -1.49233963
                },
                {
                    -1.43739801, 0.27390578, -0.24422536, 0.04708417, -1.142534795, -0.189098166, -1.173079825,
                    -0.014867615, -0.302772248, -0.29379984
                },
                {
                    -0.90040293, 0.65884256, 0.83445822, -0.70300807, 1.174390852, -1.508040701, -1.895817382,
                    -2.278185927, 0.587581947, 0.31531808
                },
                {
                    0.01795927, 1.87906610, 0.77389582, 0.10832662, 0.706666046, -0.589363138, -2.442972192,
                    -0.970695388, 0.192443325, -0.92976406
                },
                {
                    0.55387415, 1.20834822, 0.06448302, 0.31927350, -1.983582377, 0.467196045, -0.405731754,
                    0.365311915, -1.265534711, 0.23433970
                },
                {
                    -0.42325139, -0.70891670, 1.99174205, -0.67141713, 0.064663915, -0.553876634, -0.805681469,
                    0.563852886, 1.255965780, -0.18539781
                },
                {
                    0.42002074, -0.63123802, -1.00757785, 0.12377222, -1.078700753, -0.583229290, -0.258783952,
                    0.629235279, 1.640990305, -0.63736328
                },
                {
                    -0.68597883, 2.08438339, -0.53844580, -0.70336606, 0.270228494, -1.460041490, 1.249297035,
                    -0.107821540, -1.261846678, -0.37482034
                },
                {
                    0.55082064, 3.47454938, 0.75953380, -0.37527921, 0.854294961, -0.019065831, -0.112858003,
                    0.641807159, -0.698810086, -1.16556915
                },
                {
                    0.67363422, 0.92667332, -0.40761311, -0.78340315, 0.325832030, -0.217852794, 0.914575937,
                    0.426537809, -0.874743903, 0.11850882
                },
                {
                    0.72083907, -0.90100834, 0.56345600, -0.06060153, 0.041604260, 0.411908826, -2.817485963,
                    -1.925084275, -1.797551782, 0.40145147
                },
                {
                    -0.05367313, -0.21507539, -0.59114266, -0.65884396, 0.692688105, 0.071151521, -1.592912245,
                    1.602948686, -1.534699948, -0.12867356
                },
                {
                    -0.40420730, 1.25888437, 1.05721179, -0.45952940, -0.694337956, 1.356751397, 1.496704657,
                    -0.306607950, 0.481369080, -1.45621666
                },
                {
                    -0.87727647, -0.59631961, 0.83107203, 0.26779254, 0.277020226, -0.311025528, 1.637654835,
                    -0.741830423, -0.215280581, 0.56793392
                },
                {
                    0.78446370, -0.87307521, 0.01672474, -1.14851577, 1.910676004, -0.648203475, 1.486770554,
                    0.383981480, -0.385276882, -0.01836254
                },
                {
                    3.07457722, 1.01247454, -1.90099380, 0.06060911, -0.679929835, -0.429900139, 1.182808651,
                    0.432668989, 0.332198975, 0.51269742
                },
                {
                    0.89540586, 0.11192696, -1.92281834, 0.33136616, -1.920326931, -0.282083153, -1.754321387,
                    0.585920286, -1.076439182, 0.71346486
                },
                {
                    -0.15352171, -0.30784073, -0.37815924, -0.22919586, -0.221292110, -0.214288887, 0.260614568,
                    0.172797625, 0.149472696, -1.50893834
                },
                {
                    -0.13748651, 0.20297604, 1.61272128, -0.30604831, -0.400704203, -0.454324055, 0.009456687,
                    -0.511930002, -1.728940368, 0.34322336
                },
                {
                    1.02689258, -0.87994433, -0.57051108, 0.58248774, 0.302441093, 0.002714403, -0.149248097,
                    0.899665717, -0.059236677, 1.29484382
                },
                {
                    0.33349659, 0.45795310, 1.23586005, -0.08127050, 0.322413495, -0.808606069, 0.970491060,
                    -1.840693736, 1.166161435, 0.13762804
                },
                {
                    0.99011775, -1.33352835, -0.59572043, 1.50236274, 0.625403159, -0.565043640, -2.000573595,
                    1.175837724, -0.586520381, -2.06566025
                },
                {
                    -0.82846905, -0.73789931, -1.28407469, 0.11977088, 1.305735659, 0.911117623, -0.395882461,
                    -0.329289279, -0.385162625, 0.04747610
                },
                {
                    -0.56680584, -0.18543460, -0.71160315, -0.68992989, -2.358790226, -1.298322113, -0.307484620,
                    -1.186657999, -0.271367290, -0.21847769
                },
                {
                    1.26585100, -1.12212197, -0.94291124, 0.88842810, -1.456205867, -0.814762571, 0.580487677,
                    -0.367277223, 0.850938328, 0.24940092
                },
                {
                    -1.54687502, -0.40245997, -1.44758108, 1.10010079, -0.758968836, -0.875871472, -2.184631930,
                    0.722924884, -1.171039296, 1.04561975
                },
                {
                    0.07668625, -0.28206370, 0.02370704, -1.03461487, 1.114272923, -0.535649917, -0.479044600,
                    -0.369925285, -0.226334630, 0.30416139
                },
                {
                    1.44807618, -0.73387729, -0.99403333, 0.71036902, -2.409919625, -1.135921517, -1.764605239,
                    -1.438025826, -1.169272292, -0.45624891
                },
                {
                    -1.18371766, 0.02925959, -0.29821410, 0.32332958, -0.157588046, -1.423424447, 0.907609826,
                    0.869033355, 2.016807916, 1.01396096
                },
                {
                    -0.42242656, 0.34541902, 1.19502539, -0.18160170, -0.887348579, -0.594153604, 0.046583522,
                    -0.402640204, 1.321623063, 1.26204667
                },
                {
                    0.59766250, -1.18160821, -0.68363340, -0.48271635, -1.868324718, -0.184949028, 0.988388811,
                    -0.779258858, -0.384576697, -0.62036154
                },
                {
                    -2.47908177, -0.61747098, 1.08761485, -0.51914122, 0.598995210, 1.006253734, -0.238964568,
                    0.651173494, 1.230058905, -1.14397725
                },
                {
                    0.44431243, 0.61245792, 0.29072442, -0.07868109, -0.804846373, -1.423681474, -0.807531337,
                    -0.951447248, -0.339073164, 0.83472462
                },
                {
                    -1.91350737, -1.79610491, -1.05395623, -0.24689863, 0.196987030, 1.104832483, -0.707314773,
                    -1.689172078, -0.787485219, -0.43791530
                },
                {
                    -1.43374452, 1.04423637, 1.24683999, 1.24796642, -0.112817451, -0.017082821, -1.543351375,
                    -0.902068160, -0.664567345, 0.70580478
                },
                {
                    -0.35618277, -1.23533900, 0.12829825, 0.07666326, -0.353324785, -0.067368196, 0.631537558,
                    0.688543948, 1.840819520, -0.48988408
                },
                {
                    0.49347940, 0.75995968, -0.55394486, 0.74981039, -0.414999240, -0.164015016, 1.483121493,
                    -0.751756088, 0.815696967, 1.10879916
                },
                {
                    -1.09018731, -0.62397142, -0.78896593, -0.47892624, -0.769408654, 2.348562443, 0.336696344,
                    1.273282594, -0.814017967, -0.09651482
                },
                {
                    0.43286144, 1.05704657, -0.58064456, 0.23820937, 0.253475348, -0.478258840, 1.647622094,
                    -0.667665238, -0.615705620, -0.87384375
                },
                {
                    -1.68709651, -0.49695685, -1.23105928, -0.18762712, 0.511255485, 2.735422524, 0.034078942,
                    1.666084971, 0.083565438, -1.19547788
                },
                {
                    0.74607688, 2.11152678, 1.60939659, -0.57540320, -0.759229447, 0.337037459, 0.631916627,
                    1.338453222, 0.469212007, 0.24705409
                },
                {
                    -0.47754237, -2.16302185, -0.93627223, 2.48423994, 0.959544911, 0.937100173, -1.190915666,
                    0.885291843, -1.694871154, -0.62939660
                },
                {
                    0.25763637, 0.28951722, -0.53366143, -1.93213414, 0.072689075, 0.179788173, 0.807194749,
                    0.037730955, -0.121075461, 0.02406905
                },
                {
                    1.51146090, 0.19245415, 1.16280417, 0.44890804, 1.400734131, -0.031407349, -0.305883092,
                    0.664809881, -0.083297538, -0.47792930
                },
                {
                    -1.38261325, -0.11030994, -0.27133260, -0.64885346, -0.009935175, 0.379915467, 0.221344721,
                    0.861094136, 1.303806235, 0.57919403
                },
                {
                    0.67399056, -1.62171765, 0.48131849, -0.49834282, 2.074792370, -0.204552426, 0.963090976,
                    0.497830032, 0.866183651, 0.04976176
                },
            };

            var testResult = X.LinearGradientDescent(y);
            var expect =
                new double[]
                {
                    4.209541,
                    4.992066,
                    5.001884,
                    5.046863,
                    4.925452,
                    4.354024,
                    4.393291,
                    4.707941,
                    5.506140,
                    4.442644
                };
            
            Assert.IsTrue(MatrixOps.AreEqual(expect.ToMatrix().Transpose(), testResult, 0.1));
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

            testInput = new[,]
{
                {
                    97.418396137986, 3.40651226544868, -17.0697313630349, 4.18423521542488, -6.32399747806094,
                    -25.4092733216091, 3.20579525699219, 6.73063972661051, -5.16467870714877, -6.96008775118052
                },
                {
                    3.40651226544868, 113.505735745414, 16.4033774812258, -3.86903748868356, 8.75250122903815,
                    -3.30474753757298, 7.68694549468861, 1.49722630651441, 5.06944661668959, -4.73241157167048
                },
                {
                    -17.0697313630349, 16.4033774812258, 78.9323619550698, -6.848424299473, 13.3954431395863,
                    11.9874843367797, -2.14743991631947, -10.5157502882462, 5.15232310352067, 5.42715572700512
                },
                {
                    4.18423521542488, -3.86903748868356, -6.848424299473, 95.7916362934071, -11.5144020370596,
                    12.4488884786246, -6.27419139571689, 14.4105686428956, -7.94837130774331, -13.5653060135102
                },
                {
                    -6.32399747806094, 8.75250122903815, 13.3954431395863, -11.5144020370596, 118.775917531215,
                    2.09217044619191, 8.3634719006863, 20.2864897980169, 16.3966731491597, -7.5086811180655
                },
                {
                    -25.4092733216091, -3.30474753757298, 11.9874843367797, 12.4488884786246, 2.09217044619191,
                    95.3074494442806, 4.74287171305011, 15.5588724571477, -13.1513476357389, -10.3086204444777
                },
                {
                    3.20579525699219, 7.68694549468861, -2.14743991631947, -6.27419139571689, 8.3634719006863,
                    4.74287171305011, 114.921085460961, 10.959828183008, 33.7744841412645, 0.191122258156367
                },
                {
                    6.73063972661051, 1.49722630651441, -10.5157502882462, 14.4105686428956, 20.2864897980169,
                    15.5588724571477, 10.959828183008, 95.2513098133505, 9.88735406142887, -7.72530224406067
                },
                {
                    -5.16467870714877, 5.06944661668959, 5.15232310352067, -7.94837130774331, 16.3966731491597,
                    -13.1513476357389, 33.7744841412645, 9.88735406142887, 94.9216175004985, 7.92004851899833
                },
                {
                    -6.96008775118052, -4.73241157167048, 5.42715572700512, -13.5653060135102, -7.5086811180655,
                    -10.3086204444777, 0.191122258156367, -7.72530224406067, 7.92004851899833, 85.66047052459
                }
            };
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



