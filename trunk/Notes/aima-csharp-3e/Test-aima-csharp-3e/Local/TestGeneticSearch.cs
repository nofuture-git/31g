using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aima_csharp_3e.Agent;
using aima_csharp_3e.Search.Framework;
using aima_csharp_3e.Search.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_aima_csharp_3e.Local
{
    [TestClass]
    public class TestGeneticSearch
    {
        [TestMethod]
        public void TestEightDigitString()
        {
            var cout = EightDigitString.ToInts(GeneticProblem.MyTestValues[0]);
            var resultCOut = new EightDigitString(cout[0], cout[1], cout[2], cout[3], cout[4], cout[5], cout[6], cout[7]);
            System.Diagnostics.Debug.WriteLine(resultCOut.ToString());
        }

        [TestMethod]
        public void TestSearch()
        {
            var testSubject = new GeneticSearch(new GeneticProblem(), HeelsToJesus, Mutate) { NumberOfGenerations = 4 };
            var resultState = testSubject.Search();
            var testResult = resultState as EightDigitString;
            Assert.IsNotNull(testResult);

            foreach (var myVal in GeneticProblem.MyTestValues)
            {
                Assert.IsTrue(testResult.IsGe(myVal));
            }

        }

        public IState HeelsToJesus(IState s1, IState s2)
        {
            var idx = new Random().Next(1, 8);

            var si1 = EightDigitString.ToInts(s1);
            var si2 = EightDigitString.ToInts(s2);

            var cout = new List<int>();

            for (var i = 0; i < 8; i++)
            {
                var p = i >= idx ? si2 : si1;
                cout.Add(p[i]);
            }

            return new EightDigitString(cout[0],cout[1],cout[2],cout[3],cout[4],cout[5],cout[6],cout[7]);
        }

        public IState Mutate(IState f)
        {
            var flipSomething = new Random().Next(1, 100) <= 18;
            if (!flipSomething)
                return f;
            var idx = new Random().Next(1, 8);
            var val = new Random().Next(1, 10);

            var fi = EightDigitString.ToInts(f);

            fi[idx] = val;

            return new EightDigitString(fi[0],fi[1], fi[2], fi[3], fi[4], fi[5], fi[6], fi[7]);
        }

    }

    public class EightDigitString : IState
    {
        private readonly string _value;

        public EightDigitString(int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7)
        {
            var str = new StringBuilder();

            str.Append(p0 >= 10 ? p0%10 : p0);
            str.Append(p1 >= 10 ? p1%10 : p1);
            str.Append(p2 >= 10 ? p2%10 : p2);
            str.Append(p3 >= 10 ? p3%10 : p3);
            str.Append(p4 >= 10 ? p4%10 : p4);
            str.Append(p5 >= 10 ? p5%10 : p5);
            str.Append(p6 >= 10 ? p6%10 : p6);
            str.Append(p7 >= 10 ? p7%10 : p7);
            _value = str.ToString();
        }

        public override string ToString()
        {
            return _value;
        }

        public static int[] ToInts(IState s)
        {
            return s.ToString().ToCharArray().Select(x =>  Convert.ToInt32(x) - 0x30).ToArray();
        }

        public bool IsGe(EightDigitString thisGuy)
        {
            if (string.Equals(ToString(), thisGuy.ToString()))
                return false;

            var me = EightDigitString.ToInts(this);
            var him = EightDigitString.ToInts(thisGuy);

            for (var i = 0; i < 8; i++)
            {
                if (me[i] >= him[i])
                    return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            var e = obj as EightDigitString;
            if (e == null)
                return false;
            return string.Equals(e.ToString(), ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    public class ThereCanBeOnlyOne : IAction {
        public bool IsNoOp()
        {
            return false;
        }
    }

    public class GeneticProblem : IProblem
    {
        public IState InitState { get { return new EightDigitString(1, 1, 1, 1, 1, 1, 1, 1); } }
        public object Goal { get { return new EightDigitString(9, 9, 9, 9, 9, 9, 9, 9); } }

        public static List<EightDigitString> MyTestValues = new List<EightDigitString>()
        {
            new EightDigitString(2, 4, 7, 4, 8, 5, 5, 2),
            new EightDigitString(3, 2, 7, 5, 2, 4, 1, 1),
            new EightDigitString(2, 4, 4, 1, 5, 1, 2, 4),
            new EightDigitString(3, 2, 5, 4, 3, 2, 1, 3)
        };

        /// <summary>
        /// This would return the total number of individuals in a single generation
        /// see Fig 4.6
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<IAction, IState>> GetSuccessor(IState state)
        {
            return new List<Tuple<IAction, IState>>
            {
                new Tuple<IAction, IState>(new ThereCanBeOnlyOne(), MyTestValues[0]),
                new Tuple<IAction, IState>(new ThereCanBeOnlyOne(), MyTestValues[1]),
                new Tuple<IAction, IState>(new ThereCanBeOnlyOne(), MyTestValues[2]),
                new Tuple<IAction, IState>(new ThereCanBeOnlyOne(), MyTestValues[3])
            };
        }

        /// <summary>
        /// We are expecting this to never get called by the genetic algo
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsGoalTest(IState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This is just made up, the text doesn't really go into the example much beyond the diagram
        /// </summary>
        /// <param name="costUpToState1"></param>
        /// <param name="state1"></param>
        /// <param name="action"></param>
        /// <param name="state2"></param>
        /// <returns></returns>
        public double GetPathCost(double costUpToState1, IState state1, IAction action, IState state2)
        {
            var eight = state1 as EightDigitString;
            if (eight == null || eight.ToString().Length < 8)
                return int.MaxValue;
            var g = EightDigitString.ToInts((IState)this.Goal);
            var s = EightDigitString.ToInts(state1);
            var d = 0.0D;
            for (var i = 0; i < g.Length; i++)
            {
                d += (g[i] - s[i]);
            }
            return d;
        }

        public double GetValue()
        {
            throw new NotImplementedException();
        }
    }
}
