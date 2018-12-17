using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NoFuture.Tokens.Tests
{
    [TestFixture]
    public class TestXDocFrame
    {
        [Test]
        public void TestSkipAndTakeWorksLikePs1()
        {
            var myList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var lastIndex = myList.Count - 1;
            var myI = 7;

            var left = myList.Take(myI).ToList();
            var printMySkipTakeList = string.Join(",", left);
            Console.WriteLine(printMySkipTakeList);

            var right = myList.Skip(myI+1).Take(lastIndex).ToList();
            left.AddRange(right);

            printMySkipTakeList = string.Join(",", left);
            Console.WriteLine(printMySkipTakeList);
            
        }

        [Test]
        public void TestFindAllCharTokens()
        {
            var testInput = @"open something named{ accessMod type name { someStatement; someOtherStatement; accessMod type name(someArg){someBody;} somefinalStatement; }}";
            var testSubject = new NoFuture.Tokens.XDocFrame();
            var testResults = testSubject.FindAllCharTokens(testInput, '{', '}');

            Console.WriteLine(string.Join(",",testResults.Item1));
            Console.WriteLine(string.Join(",",testResults.Item2));

        }

        [Test]
        public void TestFindTokenMatch()
        {
            var testArr0 = new List<int> {20, 42, 107};
            var testArr1 = new List<int> {117, 139, 140};
            var testSubject = new NoFuture.Tokens.XDocFrame();
            var testResults = testSubject.FindTokenMatch(testArr0[0], testArr0, testArr1);

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0,testResults);

        }

        [Test]
        public void TestFindEnclosingTokens_Char()
        {
            var testInput = @"open something named{ accessMod type name { someStatement; someOtherStatement; accessMod type name(someArg){someBody;} somefinalStatement; }}";
            var testSubject = new NoFuture.Tokens.XDocFrame();
            var testResults = testSubject.FindEnclosingTokens(testInput, '{', '}');

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);
            Assert.AreEqual(3, testResults.Count);

            foreach (var t in testResults)
            {
                Console.WriteLine(string.Format("{0},{1}",t.Start, t.End));
            }
        }

        [Test]
        public void TestFindEnclosingTokens_Word()
        {
            var testWordStart = "begin";
            var testWordEnd = "end";
            var testInput =
                @"some statement; begin somekindOfEnclosure begin firstStatement; secondStatement; end anotherStatement; end more stuff";

            var testSubject = new NoFuture.Tokens.XDocFrame();
            var testResults = testSubject.FindEnclosingTokens(testInput, testWordStart, testWordEnd);

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0,testResults.Count);


            foreach (var t in testResults)
            {
                Console.WriteLine(string.Format("{0},{1}", t.Start, t.End));
                Console.WriteLine(testInput.Substring(t.Start, testWordStart.Length));
                Console.WriteLine(testInput.Substring(t.End - testWordEnd.Length, testWordEnd.Length));

            }
        }

        [Test]
        public void TestAppendHeadToken()
        {
            var testTokens = new List<NoFuture.Tokens.Token>
            {
                new TokenPair(20, 141),
                new TokenPair(42, 140),
                new TokenPair(107, 118)
            };
            var testInput =
                @"open something named{ accessMod type name { someStatement; someOtherStatement; accessMod type name(someArg){someBody;} somefinalStatement; }}";
            var testSubject = new NoFuture.Tokens.XDocFrame();
            var testResults = testSubject.AppendHeadToken(testTokens, testInput);
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0,testResults.Count);
            Assert.AreNotEqual(3, testResults.Count);

            foreach (var t in testResults)
            {
                Console.WriteLine(string.Format("{0},{1}", t.Start, t.End));

            }
        }

        [Test]
        public void TestInterlaceTokens()
        {
            var testTokens = new List<NoFuture.Tokens.Token>
            {
                new TokenPair(0,142),
                new TokenPair(20, 141),
                new TokenPair(42, 140),
                new TokenPair(107, 119)
            };
            var testSubject = new NoFuture.Tokens.XDocFrame();
            var testResults = testSubject.InterlaceTokens(testTokens);

            Assert.IsNotNull(testResults);
            foreach (var t in testResults)
            {
                Console.WriteLine(string.Format("{0},{1},{2},{3}", t.Start, t.End, t.Register, t.ChildTo));

            }

        }

        [Test]
        public void TestSetTabCount()
        {
            var testTokens = new List<Token>
            {
                new TokenPair(0, 143) {Register = 0, ChildTo = 0},
                new TokenPair(20, 142) {Register = 1, ChildTo = 0},
                new TokenPair(42, 141) {Register = 2, ChildTo = 1},
                new TokenPair(107, 120) {Register = 3, ChildTo = 2}
            };

            var testSubject = new NoFuture.Tokens.XDocFrame();
            var testResults = testSubject.SetTabCount(testTokens);
            Assert.IsNotNull(testResults);
            foreach (var t in testResults)
            {
                Console.WriteLine(string.Format("{0},{1},{2},{3},{4}", t.Start, t.End, t.Register, t.ChildTo, t.Tab));

            }
        }

        [Test]
        public void TestGetGaps()
        {
            var testInput =
                @"open something named{ accessMod type name { someStatement; someOtherStatement; accessMod type name(someArg){someBody;} somefinalStatement; }}";
            var testSubject = new XDocFrame('{', '}');

            //technically this is no a valid unit test...
            var contrivedTestInput = testSubject.GetTokens(testInput);

            var testResults = testSubject.GetGaps(contrivedTestInput);
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults);

            foreach (var t in testResults)
            {
                Console.WriteLine(string.Format("{0}, {1}, {2}, {3}", t.Start, t.End, t.Span, t.ChildTo));
            }


        }

        [Test]
        public void TestGetTokens()
        {
            var testInput =
                @"open something named{ accessMod type name { someStatement; someOtherStatement; accessMod type name(someArg){someBody;} somefinalStatement; }}";

            var testSubject = new XDocFrame('{', '}');
            var testResults = testSubject.GetTokens(testInput);

            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

            Assert.AreEqual(4,testResults.Count);
            Assert.AreEqual("0, 142, 142, 0, 0", string.Format("{0}, {1}, {2}, {3}, {4}", testResults[0].Start, testResults[0].End, testResults[0].Span, testResults[0].Register, testResults[0].Tab));
            Assert.AreEqual("20, 141, 121, 1, 0", string.Format("{0}, {1}, {2}, {3}, {4}", testResults[1].Start, testResults[1].End, testResults[1].Span, testResults[1].Register, testResults[1].Tab));
            Assert.AreEqual("42, 140, 98, 2, 1", string.Format("{0}, {1}, {2}, {3}, {4}", testResults[2].Start, testResults[2].End, testResults[2].Span, testResults[2].Register, testResults[2].Tab));
            Assert.AreEqual("107, 118, 11, 3, 2", string.Format("{0}, {1}, {2}, {3}, {4}", testResults[3].Start, testResults[3].End, testResults[3].Span, testResults[3].Register, testResults[3].Tab));

            foreach (var t in testResults)
            {
                Console.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}", t.Start, t.End, t.Span, t.Register, t.Tab));
            }

        }

        [Test]
        public void TestGetXDocFrame()
        {
            var testInput = GetTestFileContent();
            var testSubject = new XDocFrame('{', '}') {MinTokenSpan = 1};
            var testResults = testSubject.GetXDocFrame(testInput);
            var testResultsXml = testResults.ToString();
            Assert.IsNotNull(testResultsXml);
        }

        [Test]
        public void TestApplyXDocFrame()
        {
            var testInput =GetTestFileContent();
            var testSubject = new XDocFrame('{', '}') { MinTokenSpan = 1 };
            var testResults = testSubject.GetXDocFrame(testInput);
            testSubject.ApplyXDocFrame(testResults, testInput);
            var testResultsXml = testResults.ToString();
            Assert.IsNotNull(testResultsXml);
        }

        public static string GetTestFileContent()
        {
            //need this to be another object each time and not just another reference
            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{asmName}.TokensTestFile.txt");
            if (liSteam == null)
            {
                Assert.Fail($"Cannot find the embedded file TokensTestFile.txt");
            }

            string fileContent = null;
            using (var txtSr = new StreamReader(liSteam))
            {
                fileContent = txtSr.ReadToEnd();
            }
            Assert.IsNotNull(fileContent);
            return fileContent;
        }
    }
}
