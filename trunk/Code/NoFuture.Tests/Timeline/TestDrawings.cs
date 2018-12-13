using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NoFuture.Timeline.Tests
{
    [TestFixture]
    public class TestDrawings
    {
        public static string UnitTestsRoot { get; } = @"C:\Projects\31g\trunk\Code\NoFuture.Tests";

        public static string RootBin { get; } = @"C:\Projects\31g\trunk\bin";

        [Test]
        public void RulePrintTestHighToLow()
        {
            var testSubject = new NoFuture.Timeline.Rule() {EndValue = 750, StartValue = 950};

            var testResult = testSubject.ToString();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);

        }

        [Test]
        public void RulePrintTestLowToHigh()
        {
            var testSubject = new NoFuture.Timeline.Rule() { EndValue = 25, StartValue = 90 };

            var testResult = testSubject.ToString();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
            
        }

        [Test]
        public void RulePrintTestDivideUneven()
        {
            var ruler = new Rule { StartValue = 950, EndValue = 775 };

            var testResult = ruler.ToString();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void BlockDrawHeaderTest()
        {
            var testSubject = new NoFuture.Timeline.Block();
            testSubject.Title = "Judea";

            var testResult = testSubject.DrawHeader();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);

            testSubject.Title = "Rome";
            testResult = testSubject.DrawHeader();
            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void RuleGetIndexRuleTest()
        {
            var testSubject = new NoFuture.Timeline.Rule() { EndValue = 25, StartValue = 90 };

            var testResult = testSubject.GetIndexRule();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0,testResult.Count);
            var c = 0;
            foreach (var idx in testResult)
            {
                Console.WriteLine("Index '{0}', Value '{1}'",c, idx);
                c += 1;
            }

        }

        [Test]
        public void CalcEntryIndexTest()
        {
            var testSubject = new NoFuture.Timeline.Rule() { EndValue = 750, StartValue = 950 };

            var testResult = testSubject.CalcEntryIndex(new Entry() {StartValue = 945, EndValue = 945});

            Assert.AreNotEqual(-1, testResult.Item1);
            Assert.AreNotEqual(-1, testResult.Item2);

            Console.WriteLine("Start Index '{0}', End Index '{1}'", testResult.Item1, testResult.Item2);
        }

        [Test]
        public void CalcEntryIndexTestOob()
        {
            var testRuler = new Rule() { EndValue = 575, StartValue = 775 };
            var testEntry = new Entry()
                            {
                                Text = "XXII Dyn. 935-725",
                                StartValue = 935,
                                EndValue = 725,
                                Ruler = testRuler,
                                Location = PrintLocation.Left
                            };
            Console.WriteLine(testEntry);
            Console.WriteLine(testEntry.CalcHeight());
            var testResult = testRuler.CalcEntryIndex(testEntry);

            Assert.AreNotEqual(-1, testResult.Item1);
            Assert.AreNotEqual(-1, testResult.Item2);

            Console.WriteLine("Start Index '{0}', End Index '{1}'", testResult.Item1, testResult.Item2);

        }

        [Test]
        public void InnerBlockDrawHeaderTest()
        {
            var testSubject = new NoFuture.Timeline.Block() {Title = "Judea"};
            var testResult = testSubject.DrawHeader();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void EntryPrintTest()
        {
            var testSubject = new NoFuture.Timeline.Entry();

            testSubject.Text = "Slave revolt led by Spartacus";

            var testResult = testSubject.ToString();

            Console.WriteLine(testResult);

            Assert.IsNotNull(testResult);

            testSubject.Location = PrintLocation.Left;
            testResult = testSubject.ToString();

            Console.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject.Location = PrintLocation.Right;
            testResult = testSubject.ToString();

            Console.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject.Text = "Late Republic";
            testSubject._height = 8;
            
            testResult = testSubject.ToString();

            Console.WriteLine(testResult);

            Assert.IsNotNull(testResult);

            testSubject.Location = PrintLocation.Left;
            testResult = testSubject.ToString();

            Console.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject.Location = PrintLocation.Right;
            testResult = testSubject.ToString();

            Console.WriteLine(testResult);
            Assert.IsNotNull(testResult);

        }

        [Test]
        public void EntryCalcHeightTest()
        {
            var testD00 = new NoFuture.Timeline.Rule() { EndValue = 750, StartValue = 950 };
            var testSubject = new NoFuture.Timeline.Entry();
            testSubject.Ruler = testD00;
            testSubject.StartValue = 902;
            testSubject.EndValue = 845;

            var testCtrl = testD00.CalcEntryIndex(testSubject);
            Console.WriteLine("Start Index '{0}', End Index '{1}'", testCtrl.Item1, testCtrl.Item2);

            var testResult = testSubject.CalcHeight();
            Assert.AreNotEqual(0, testResult);

            Console.WriteLine("Test result calc height '{0}'",testResult);

        }

        [Test]
        public void ToTextCanvasTest()
        {
            var testD00 = new NoFuture.Timeline.Rule() { EndValue = 750, StartValue = 950 };
            var testSubject = new NoFuture.Timeline.Entry();
            testSubject.Ruler = testD00;
            testSubject.StartValue = 902;
            testSubject.EndValue = 845;
            testSubject.Text = "Late Republic";

            var testResult = testSubject.ToTextCanvas(testSubject.Ruler);

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0, testResult.Items.Count);

            foreach (var item in testResult.Items)
            {
                var text = new string(item.Text.ToArray());
                var index = item.Index;
                var hashMark = item.HashMarkValue;

                Console.WriteLine("[{0}] - [{1,-18}] '{2}'",index, hashMark,text);
            }
        }

        [Test]
        public void MergeToTextCanvasTest()
        {
            var testRuler = new NoFuture.Timeline.Rule() { EndValue = 25, StartValue = 90 };
            var testSubjectA = new NoFuture.Timeline.Entry
                               {
                                   Ruler = testRuler,
                                   StartValue = 73,
                                   EndValue = 71,
                                   Text = "Slave revolt led by Spartacus"
                               };

            var testSubjectB = new NoFuture.Timeline.Entry
                               {
                                   Ruler = testRuler,
                                   StartValue = 74,
                                   EndValue = 27,
                                   Text = "Collapse of Republic",
                                   Location = PrintLocation.Left
                               };

            var testResult = testSubjectA.ToTextCanvas(testRuler).Merge(testSubjectB.ToTextCanvas(testRuler), testRuler);
            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void BlockToStringTest()
        {
            var testRuler = new NoFuture.Timeline.Rule() { EndValue = 20, StartValue = 90 };
            var testSubjectB = new NoFuture.Timeline.Block()
            {
                EmptyChar = '.',
                EndValue = 26,
                Ruler = testRuler,
                StartValue = 80,
                Title = "Roman Republic"
            };

            var testResult = testSubjectB.ToString();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult);
            Console.WriteLine(testResult);            
        }

        [Test]
        public void MergeBlockToEntryTest()
        {
            var testRuler = new NoFuture.Timeline.Rule() { EndValue = 20, StartValue = 90 };
            var testSubjectB = new NoFuture.Timeline.Block()
            {
                EmptyChar = '.',
                EndValue = 26,
                Ruler = testRuler,
                StartValue = 90,
                Title = "Roman Republic"
            };

            var testSubjectA = new NoFuture.Timeline.Entry {Ruler = testRuler, Text = "Ceaser assassinated", StartValue = 44, EndValue = 44};

            var testResult = testSubjectA.ToTextCanvas(testRuler).Merge(testSubjectB.ToTextCanvas(testRuler), testRuler);

            Console.WriteLine(testResult);

        }

        [Test]
        public void ConcatBlockTest()
        {
            var rule = new Rule() { StartValue = 2100, EndValue = 1500 };

            var assyria = new Block() { Ruler = rule, Title = "Assyria", StartValue = 1950 };
            var mari = new Block() { Ruler = rule, Title = "Mari", StartValue = 1950, EndValue = 1700, Width = 16 };
            var isin = new Block() { Ruler = rule, Title = "Isin", StartValue = 1950, EndValue = 1750, Width = 8 };
            var larsa = new Block() { Ruler = rule, Title = "Larsa", StartValue = 1950, EndValue = 1700, Width = 16 };
            var babylon = new Block() { Ruler = rule, Title = "Babylon", StartValue = 1830, EndValue = 1550, Width = 20 };

            var testResult = assyria.ToTextCanvas(rule).Concat(mari.ToTextCanvas(rule), rule);
            testResult = testResult.Concat(isin.ToTextCanvas(rule), rule);
            testResult = testResult.Concat(larsa.ToTextCanvas(rule), rule);
            testResult = testResult.Concat(babylon.ToTextCanvas(rule), rule);
            
            Console.WriteLine(testResult.ToString());

        }

        [Test]
        public void PrintEntriesToCanvasTest()
        {
            var testRuler = new Rule() { EndValue = 575, StartValue = 775 };
            var testBlock1 = new Block() { EndValue = 575, StartValue = 775, Title = "Judah", Ruler = testRuler };

            var testEntry1 = new Entry()
                             {
                                 EndValue = 687,
                                 StartValue = 732,
                                 Ruler = testRuler,
                                 Text = "(Isaiah)",
                                 Location = PrintLocation.Center
                             };
            var testEntry2 = new Entry()
                             {
                                 EndValue = 702,
                                 StartValue = 733,
                                 Ruler = testRuler,
                                 Text = "(Micah)",
                                 Location = PrintLocation.Right
                             };

            var testEntry3 = new Entry() {EndValue = 735, StartValue = 735, Ruler = testRuler, Text = "Ahaz 735-715"};
            var testEntry4 = new Entry()
                             {
                                 EndValue = 715,
                                 StartValue = 715,
                                 Ruler = testRuler,
                                 Text = "Hezekiah 715-687"
                             };

            testBlock1.AddEntry(testEntry1);
            testBlock1.AddEntry(testEntry2);
            testBlock1.AddEntry(testEntry3);
            testBlock1.AddEntry(testEntry4);
            var testResult = testBlock1.PrintEntriesOnCanvas();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void PrintEntriesToCanvasWithInnerBlockTest()
        {
            var rule = new Rule() { StartValue = 2100, EndValue = 1500 };
            var mesopotamia = new Block() { Ruler = rule, Title = "Mesopotamia" };
            mesopotamia.AddEntry(new Entry() { StartValue = 2060, EndValue = 1950, Text = "Ur III" });

            var assyria = new Block() { Ruler = rule, Title = "Assyria", StartValue = 1950 };
            assyria.AddEntry(new Entry() { StartValue = 1900, Text = "Cappadocian Colonies" });
            assyria.AddEntry(new Entry() { StartValue = 1750, Text = "Shamshi-Adad I" });

            var mari = new Block() { Ruler = rule, Title = "Mari", StartValue = 1950, EndValue = 1700, Width = 16 };
            mari.AddEntry(new Entry() { StartValue = 1750, Text = "The 'Mari Age'" });

            var isin = new Block() { Ruler = rule, Title = "Isin", StartValue = 1950, EndValue = 1750, Width = 8 };

            var larsa = new Block() { Ruler = rule, Title = "Larsa", StartValue = 1950, EndValue = 1700, Width = 16 };
            larsa.AddEntry(new Entry() { StartValue = 1758, Text = "Rim-Sin I" });

            var babylon = new Block() { Ruler = rule, Title = "Babylon", StartValue = 1830, EndValue = 1550, Width = 20 };
            babylon.AddEntry(new Entry() { StartValue = 1700, Text = "Hammurabi" });
            babylon.AddEntry(new Entry() { StartValue = 1630, Text = "Kassite Dyn." });

            mesopotamia.AddInnerBlock(assyria);
            mesopotamia.AddInnerBlock(mari);
            mesopotamia.AddInnerBlock(isin);
            mesopotamia.AddInnerBlock(larsa);
            mesopotamia.AddInnerBlock(babylon);

            TextCanvas myInnerBlockCanvas = null;

            if (mesopotamia._innerBlocks.Count > 0)
            {
                myInnerBlockCanvas = mesopotamia._innerBlocks[0].PrintEntriesOnCanvas();
                Console.WriteLine(myInnerBlockCanvas);
                for (var i = 1; i < mesopotamia._innerBlocks.Count; i++)
                {
                    Console.WriteLine(myInnerBlockCanvas);
                    myInnerBlockCanvas = myInnerBlockCanvas.Concat(mesopotamia._innerBlocks[i].PrintEntriesOnCanvas(), rule);
                }
            }
            Console.WriteLine(myInnerBlockCanvas);


        }

        [Test]
        public void PrintPlateTest()
        {
            var testRuler = new Rule() { EndValue = 575, StartValue = 775 };
            var testBlock1 = new Block() { EndValue = 575, StartValue = 775, Title = "Judah", Ruler = testRuler };
            var testBlock2 = new Block() {EndValue = 575, StartValue = 775, Title = "Egypt", Ruler = testRuler};

            testBlock1.AddEntry(new Entry()
                                {
                                    Text = "Uzziah 783-742",
                                    StartValue = 765,
                                    EndValue = 765,
                                });
            testBlock1.AddEntry(new Entry()
                                {
                                    Text = "(Jotham coregent 750)",
                                    StartValue = 750,
                                    EndValue = 750,
                                    Location = PrintLocation.Right
                                });
            testBlock1.AddEntry(new Entry()
                                {
                                    Text = "Jotham 742-735",
                                    StartValue = 742,
                                    EndValue = 742,
                                });
            testBlock1.AddEntry(new Entry()
            {
                EndValue = 687,
                StartValue = 732,
                Text = "(Isaiah)",
                Location = PrintLocation.Center
            });
            testBlock1.AddEntry(new Entry()
            {
                EndValue = 702,
                StartValue = 733,
                Text = "(Micah)",
                Location = PrintLocation.Right
            });
            testBlock1.AddEntry(new Entry() { EndValue = 735, StartValue = 735, Text = "Ahaz 735-715" });
            testBlock1.AddEntry(new Entry()
            {
                EndValue = 715,
                StartValue = 715,
                Text = "Hezekiah 715-687"
            });

            testBlock2.AddEntry(new Entry()
                                {
                                    Text = "XXII Dyn. 935-725",
                                    StartValue = 935,
                                    EndValue = 725,
                                    Location = PrintLocation.Left
                                });
            testBlock2.AddEntry(new Entry()
            {
                Text = "XXIV Dyn. 725-709",
                StartValue = 725,
                EndValue = 709,
            });

            testBlock2.AddEntry(new Entry() {Text = "Neco II 610-594", StartValue = 610, EndValue = 610});

            var testSubject = new Plate() {Ruler = testRuler, Name = "Near East"};

            testSubject.AddBlock(testBlock1);
            testSubject.AddBlock(testBlock2);

            var testResult = testSubject.ToString();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void PrintPlateWithInnerPlatesTest()
        {
            var ruler = new Rule {StartValue = 950, EndValue = 775};
            var egypt = new Block {Ruler = ruler, Title = "Egypt"};
            var israel = new Block {Ruler = ruler, Title = "Israel"};
            var damascus = new Block {Ruler = ruler, Title = "Damascus"};
            var assyria = new Block {Ruler = ruler, Title = "Assyria"};

            var northKingdom = new Block {StartValue = 922, EndValue = 775, Ruler = ruler, Title = "Israel"};
            var southKingdom = new Block {StartValue = 922, EndValue = 775, Ruler = ruler, Title = "Judea"};

            egypt.AddEntry(new Entry {StartValue = 935, Text = "XXII Dyn. 935-725",Location = PrintLocation.Left});
            egypt.AddEntry(new Entry { StartValue = 935, EndValue = 914, Text = "Shishak 935-914",Location = PrintLocation.Right});
            egypt.AddEntry(new Entry {StartValue = 914, EndValue = 874, Text = "Osorkon I 914-874"});

            israel.AddEntry(new Entry {StartValue = 940,Text="Solomon 961-922"});

            southKingdom.AddEntry(new Entry {StartValue = 922, Text = "Rehoboam 922-915"});
            southKingdom.AddEntry(new Entry {StartValue = 915, Text = "Abijah 915-913"});

            northKingdom.AddEntry(new Entry {StartValue = 922, Text = "Jeroboam I 922-901"});

            israel.AddInnerBlock(southKingdom);
            israel.AddInnerBlock(northKingdom);

            damascus.AddEntry(new Entry {StartValue = 930, Text = "Rezon"});

            assyria.AddEntry(new Entry {StartValue = 935, Text = "Asshur-dan II 935-913"});

            var testSubject = new Plate() {Name = "Near East", Ruler = ruler};
            testSubject.AddBlock(egypt);
            testSubject.AddBlock(israel);
            testSubject.AddBlock(damascus);
            testSubject.AddBlock(assyria);

            var testResult = testSubject.ToString();

            Console.WriteLine(testResult);

        }

        [Test]
        public void GetRequiredWidthTest()
        {
            var ruler = new Rule { StartValue = 950, EndValue = 775 };
            var egypt = new Block { Ruler = ruler, Title = "Egypt" };
            egypt.AddEntry(new Entry { StartValue = 935, Text = "XXII Dyn. 935-725", Location = PrintLocation.Left });
            egypt.AddEntry(new Entry { StartValue = 935, EndValue = 914, Text = "Shishak 935-914", Location = PrintLocation.Right });

            var testResult = egypt.GetRequiredWidth();

            Assert.AreNotEqual(0, testResult);

            Console.WriteLine(testResult);


        }

        [Test]
        public void PlateConcatAllBlocksTest()
        {
            var ruler = new Rule { StartValue = 950, EndValue = 775 };
            var egypt = new Block { Ruler = ruler, Title = "Egypt" };
            var israel = new Block { Ruler = ruler, Title = "Israel" };
            var damascus = new Block { Ruler = ruler, Title = "Damascus" };
            var assyria = new Block { Ruler = ruler, Title = "Assyria" };

            var northKingdom = new Block { StartValue = 922, EndValue = 775, Ruler = ruler, Title = "Israel" };
            var southKingdom = new Block { StartValue = 922, EndValue = 775, Ruler = ruler, Title = "Judea" };

            egypt.AddEntry(new Entry { StartValue = 935, Text = "XXII Dyn. 935-725", Location = PrintLocation.Left });
            egypt.AddEntry(new Entry { StartValue = 935, EndValue = 914, Text = "Shishak 935-914", Location = PrintLocation.Right });
            egypt.AddEntry(new Entry { StartValue = 914, EndValue = 874, Text = "Osorkon I 914-874" });

            israel.AddEntry(new Entry { StartValue = 940, Text = "Solomon 961-922" });

            southKingdom.AddEntry(new Entry { StartValue = 922, Text = "Rehoboam 922-915" });
            southKingdom.AddEntry(new Entry { StartValue = 915, Text = "Abijah 915-913" });

            northKingdom.AddEntry(new Entry { StartValue = 922, Text = "Jeroboam I 922-901" });

            israel.AddInnerBlock(southKingdom);
            israel.AddInnerBlock(northKingdom);

            damascus.AddEntry(new Entry { StartValue = 930, Text = "Rezon" });

            assyria.AddEntry(new Entry { StartValue = 935, Text = "Asshur-dan II 935-913" });

            var testSubject = new Plate() { Name = "Near East", Ruler = ruler };
            testSubject.AddBlock(egypt);
            testSubject.AddBlock(israel);
            testSubject.AddBlock(damascus);
            testSubject.AddBlock(assyria);

            var testResult = testSubject.ToTextCanvas();

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Items);

            foreach (var item in testResult.Items)
            {
                var itemText = new string(item.Text.ToArray());

                foreach (var span in item.Ranges)
                {
                    var subText = itemText.Substring(span.StartIndex, span.Length);
                    Console.WriteLine("{0} {1} {2} {3} {4} '{5}'",item.Index, item.HashMarkValue,span.Id,span.StartIndex,span.Length, subText);
                }
            }

        }

        [Test]
        public void PlateComposeArrowRight()
        {
            var testRangeBetwixt = 16 + 28 + 18;
            var testPriorLen = 9;
            var testLeftBlockLen = 28;
            var testArrowText = "Thebes sacked";

            var testSubject = new Arrow();
            var testResult = testSubject.ComposeFromLeftToRightArrow(testRangeBetwixt, testPriorLen, testLeftBlockLen, testArrowText);

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);

        }

        [Test]
        public void PlateComposeArrowLeft()
        {
            var testRangeBetwixt = 16 + 28 + 18;
            var testPriorLen = 9;
            var testRightBlockLen = 28;
            var testArrowText = "Thebes sacked";

            var testSubject = new Arrow();
            var testResult = testSubject.ComposeFromRightToLeftArrow(testRangeBetwixt, testPriorLen, testRightBlockLen, testArrowText);

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void PlateGetRangeLenBetwixt()
        {

            var testRange00 = new TextRange() {Id = "Range00", Length = 9, StartIndex = 7};
            var testRange01 = new TextRange() { Id = "Range01", Length = 12, StartIndex = (testRange00.StartIndex+testRange00.Length) };
            var testRange02 = new TextRange()
                              {
                                  Id = "Range02",
                                  Length = 26,
                                  StartIndex = (testRange01.StartIndex + testRange01.Length)
                              };
            var testRange03 = new TextRange()
                              {
                                  Id = "Range03",
                                  Length = 18,
                                  StartIndex = (testRange02.StartIndex + testRange02.Length)
                              };
            var testRange04 = new TextRange()
                              {
                                  Id = "Range04",
                                  Length = 20,
                                  StartIndex = (testRange03.StartIndex + testRange03.Length)
                              };

            var testBlockLeftId = testRange01.Id;
            var testBlockRightId = testRange04.Id;
            
            var testResult = Arrow.GetRangeLenBetwixt(testBlockLeftId,
                testBlockRightId,
                new List<TextRange>() {testRange00, testRange01, testRange02, testRange03, testRange04});

            Console.WriteLine(testResult);

            Assert.AreNotEqual(0,testResult);

        }

        [Test]
        public void PlateGetLeftAPriorLen()
        {

            var testRange00 = new TextRange() { Id = "Range00", Length = 9, StartIndex = 7 };
            var testRange01 = new TextRange() { Id = "Range01", Length = 12, StartIndex = (testRange00.StartIndex + testRange00.Length) };
            var testRange02 = new TextRange()
            {
                Id = "Range02",
                Length = 26,
                StartIndex = (testRange01.StartIndex + testRange01.Length)
            };
            var testRange03 = new TextRange()
            {
                Id = "Range03",
                Length = 18,
                StartIndex = (testRange02.StartIndex + testRange02.Length)
            };
            var testRange04 = new TextRange()
            {
                Id = "Range04",
                Length = 20,
                StartIndex = (testRange03.StartIndex + testRange03.Length)
            };

            var testLeftId = testRange02.Id;
            var testResult = Arrow.GetLeftAPriorLen(testLeftId,
                new List<TextRange>() {testRange00, testRange01, testRange02, testRange03, testRange04});

            Assert.AreNotEqual(0,testResult);
            Assert.AreEqual(testRange00.Length+testRange01.Length, testResult);
            Console.WriteLine(testResult);
        }

        [Test]
        public void PlateDetermineArrowDirectionTest()
        {

            var testRange00 = new TextRange() { Id = "Range00", Length = 9, StartIndex = 7 };
            var testRange01 = new TextRange() { Id = "Range01", Length = 12, StartIndex = (testRange00.StartIndex + testRange00.Length) };
            var testRange02 = new TextRange()
            {
                Id = "Range02",
                Length = 26,
                StartIndex = (testRange01.StartIndex + testRange01.Length)
            };
            var testRange03 = new TextRange()
            {
                Id = "Range03",
                Length = 18,
                StartIndex = (testRange02.StartIndex + testRange02.Length)
            };
            var testRange04 = new TextRange()
            {
                Id = "Range04",
                Length = 20,
                StartIndex = (testRange03.StartIndex + testRange03.Length)
            };

            var testRangeList = new List<TextRange>() {testRange00, testRange01, testRange02, testRange03, testRange04};

            var testResultToRight = Arrow.DetermineArrowDirection(testRange03.Id, testRange01.Id, testRangeList);
            var testResultToLeft = Arrow.DetermineArrowDirection(testRange02.Id, testRange04.Id, testRangeList);

            Assert.AreEqual(PrintLocation.Right, testResultToRight);
            Assert.AreEqual(PrintLocation.Left, testResultToLeft);

        }

        [Test]
        public void PlateComposeArrowTest()
        {
            var ruler = new Rule { StartValue = 950, EndValue = 775 };
            var egypt = new Block { Ruler = ruler, Title = "Egypt" };
            var israel = new Block { Ruler = ruler, Title = "Israel" };
            var damascus = new Block { Ruler = ruler, Title = "Damascus" };
            var assyria = new Block { Ruler = ruler, Title = "Assyria" };

            var northKingdom = new Block { StartValue = 922, EndValue = 775, Ruler = ruler, Title = "Israel" };
            var southKingdom = new Block { StartValue = 922, EndValue = 775, Ruler = ruler, Title = "Judea" };

            egypt.AddEntry(new Entry { StartValue = 935, Text = "XXII Dyn. 935-725", Location = PrintLocation.Left });
            egypt.AddEntry(new Entry { StartValue = 935, EndValue = 914, Text = "Shishak 935-914", Location = PrintLocation.Right });
            egypt.AddEntry(new Entry { StartValue = 914, EndValue = 874, Text = "Osorkon I 914-874" });

            israel.AddEntry(new Entry { StartValue = 940, Text = "Solomon 961-922" });

            southKingdom.AddEntry(new Entry { StartValue = 922, Text = "Rehoboam 922-915" });
            southKingdom.AddEntry(new Entry { StartValue = 915, Text = "Abijah 915-913" });

            northKingdom.AddEntry(new Entry { StartValue = 922, Text = "Jeroboam I 922-901" });

            israel.AddInnerBlock(southKingdom);
            israel.AddInnerBlock(northKingdom);

            damascus.AddEntry(new Entry { StartValue = 930, Text = "Rezon" });

            assyria.AddEntry(new Entry { StartValue = 935, Text = "Asshur-dan II 935-913" });

            var testPlate = new Plate() { Name = "Near East 950-775", Ruler = ruler };
            testPlate.AddBlock(egypt);
            testPlate.AddBlock(israel);
            testPlate.AddBlock(damascus);
            testPlate.AddBlock(assyria);

            testPlate.AddArrow(new Arrow(assyria, egypt) {StartValue = 888, Text = "Thebes sacked"});

            testPlate.AddArrow(new Arrow(egypt, assyria) {Text = "Niniviah sacked", StartValue = 790});

            var testResult = testPlate.ToString();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void PlateToStringWithNotesTest()
        {
            var ruler = new Rule() {StartValue = 525, EndValue = 310};
            var greece = new Block() {Ruler = ruler, Title = "Greece"};
            greece.AddEntry(new Entry(){StartValue = 500, Text = "Ionian revolt", Location = PrintLocation.Right});
            greece.AddEntry(new Entry() {StartValue = 490, Text = "Battle of Marathon"});
            greece.AddEntry(new Entry(){StartValue = 480, Text = "Battle of Thermopyle / Salamis"});
            greece.AddEntry(new Entry(){StartValue = 479, Text ="Battle of Plataea"});
            greece.AddEntry(new Entry(){StartValue = 450, Text = "Peace of Callias"});
            greece.AddEntry(new Entry(){StartValue = 440, Text = "Parthenon built"});
            greece.AddEntry(new Entry(){StartValue = 431,EndValue = 404,Text = "Peloponnesian War"});
            greece.AddEntry(new Entry(){StartValue = 400,Text = "Athens return of self-rule"});
            greece.AddEntry(new Entry(){StartValue = 390,Text ="Corinthian War"});
            greece.AddEntry(new Entry(){StartValue = 338, Text = "Battle of Chaeronea"});
            greece.AddEntry(new Entry(){StartValue = 336, Text = "Phillip II rules all Greece"});
            greece.AddEntry(new Entry() { StartValue = 460, EndValue = 429, Text = "Pericles" });
            greece.AddEntry(new Entry(){StartValue = 323, Text = "Alexander & Fall of Persian Empire"});
            greece.AddEntry(new Entry() { StartValue = 500, EndValue = 325, Text = "Classical Period", Location = PrintLocation.Left });

            var plate = new Plate() {Name = "Classical Period", Ruler = ruler};
            plate.AddBlock(greece);

            var testResult = plate.ToString();

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void PlateSplitBlockTest()
        {
            var rule = new Rule() {StartValue = 2050, EndValue = 1500};
            var mesopotamia = new Block() {Ruler = rule, Title = "Mesopotamia"};
            mesopotamia.AddEntry(new Entry(){StartValue = 2060, EndValue = 1950, Text = "Ur III: ca. 2060-1950"});
            var assyria =  new Block(){Ruler = rule, StartValue = 1950, EndValue = 1550, Title = "Assyria"};
            var babylon = new Block {Ruler = rule, StartValue = 1830, EndValue = 1530, Title = "I Babylon"};
            var kassite = new Block {Ruler = rule, StartValue = 1650, EndValue = 1500, Title = "Kassite Dyn."};
            mesopotamia.AddInnerBlock(assyria);
            mesopotamia.AddInnerBlock(babylon);
            mesopotamia.AddInnerBlock(kassite);

            mesopotamia.AddArrow(new Arrow(babylon,assyria) {StartValue = 1728,Text = "Hammurabi"});

            var hittites = new Block {Ruler = rule, Title = "Hittites"};
            var palestine = new Block() {Ruler = rule, Title = "Palestine"};
            var egypt = new Block() {Ruler = rule, Title = "Egypt"};

            var plate = new Plate() {Name = "Near East 2050-1500", Ruler = rule};
            plate.AddBlock(mesopotamia);
            plate.AddBlock(hittites);
            plate.AddBlock(palestine);
            plate.AddBlock(egypt);

            var testResult = plate.ToString();

            Console.WriteLine(testResult);
        }

        [Test]
        public void TextAfterBlockEnd()
        {
            var rule = new Rule() { StartValue = 780, EndValue = 500, RuleLineSpacing = 7 };
            var judah = new Block() { Ruler = rule, Title = "Judah", StartValue = 780, EndValue = 587 };
            judah.AddEntry(560, "Exile");

            var plate = new Plate() { Ruler = rule, Name = "Mid-Eighth to Fifth Centuries BCE" };

            plate.AddBlock(judah);

            Console.WriteLine(plate.ToString());
        }

        [Test]
        public void TestOccidentalPlateBCE3000to2000()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.BCE3000to2000();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");

        }
        [Test]
        public void TestOccidentalPlateBCE2000to1500()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.BCE2000to1500();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");


        }
        [Test]
        public void TestOccidentalPlateBCE1600to1200()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.BCE1600to1200();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");

        }
        [Test]
        public void TestOccidentalPlateBCE1250to900()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.BCE1250to900();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");

        }
        [Test]
        public void TestOccidentalPlateBCE922to750()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.BCE922to750();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");

        }
        [Test]
        public void TestOccidentalPlateBCE780to500()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.BCE780to500();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");

        }
        [Test]
        public void TestOccidentalPlateBCE500to325()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.BCE500to325();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");

        }
        [Test]
        public void TestOccidentalPlateBCE325to27()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.BCE325to27();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");

        }
        [Test]
        public void TestOccidentalPlateCE30to105()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE30to105();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }

        [Test]
        public void TestOccidentalPlateCE105to325()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE105to325();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }
        [Test]
        public void TestOccidentalPlateCE325to550()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE325to550();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }
        [Test]
        public void TestOccidentalPlateCE550to825()
        {
            var testSubject = new Occidental();

            var testPlate = testSubject.CE550to825();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }
        [Test]
        public void TestOccidentalPlateCE825to1075()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE825to1075();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }
        [Test]
        public void TestOccidentalPlateCE1075to1350()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE1075to1350();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }
        [Test]
        public void TestOccidentalPlateCE1350to1500()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE1350to1500();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");

        }
        [Test]
        public void TestOccidentalPlateCE1500to1700()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE1500to1700();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }

        [Test]
        public void TestOccidentalPlateCE1700to1788()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE1700to1788();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }

        [Test]
        public void TestOccidentalPlateCE1788to1865()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE1788to1865();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }

        [Test]
        public void TestOccidentalPlateCE1865to1914()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE1865to1914();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }
        [Test]
        public void TestOccidentalPlateCE1914to1945()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE1914to1945();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }
        [Test]
        public void TestOccidentalPlateCE1945to1992()
        {
            var testSubject = new Occidental();
            var testPlate = testSubject.CE1945to1992();

            testPlate.ToPdf(UnitTestsRoot + $@"\Timeline\{testPlate.FileName}.pdf");
        }
        [Test]
        public void TestFastPlate()
        {
            var testSubject = new FastPlate("OAuth 2.0 Authorization Code Flow", null, "Resource Owner", "User-Agent", "Client", "Auth Server");

            testSubject.Blk("Client").Txt("Redirect to Auth Endpoint and a whole bunch of stuff in the request has to go as well").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Redirect to Auth Endpoint").Blk("Auth Server");
            testSubject.Blk("Auth Server").Txt("Send back login screen").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Prompt For Creds").Blk("Resource Owner");
            testSubject.Blk("Resource Owner").Txt("Enter creds").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Post creds").Blk("Auth Server");
            testSubject.Blk("Auth Server").Txt("Auth Code & Redirect").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Post Auth code to Redirect").Blk("Client");
            testSubject.Blk("Client").Txt("Send Auth Code & Redirect").Blk("Auth Server");
            testSubject.Blk("Auth Server").Txt("Return Access Token").Blk("Client");

            testSubject.Blk("User-Agent").Txt("Start Dots").Blk("Resource Owner");
            testSubject.Blk("Resource Owner").Txt("More Dots").Blk("Auth Server");
            testSubject.Blk("Auth Server").Txt("More Dots").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Stop Dots").Blk("Client");

            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestFastPlateHashTableExample()
        {
            var myHashTable = new System.Collections.Hashtable()
            {
                {"UserAgent", new System.Collections.ArrayList()},
                {"WebLayer", new System.Collections.ArrayList {"Module", "Ctrl", "View"}},
                {"AppLayer", new System.Collections.ArrayList { "Provider", "Adapter", "Dal"}},
                {"AuthLayer", new System.Collections.ArrayList {"Site", "Service"}},
                {"DataLayer", new System.Collections.ArrayList { "Db01", "Db02", "Db03"}},
            };
            var myFPlate = new FastPlate("CrazyRoutes", 8, new Rule {StartValue = 1, EndValue = 16, RuleLineSpacing = 2},
                myHashTable);
            myFPlate.Blk(2.0)
                .Txt("GET")
                .Blk(1.1)
                .Blk(4.1)
                .Blk(4.2)
                .Blk(5.3)
                .Blk(4.2)
                .Blk(4.1)
                .Blk(1.1)
                .Blk(1.2)
                .Blk(3.1)
                .Blk(3.2)
                .Blk(3.3)
                .Blk(5.1)
                ;
            Console.WriteLine(myFPlate.ToString());
        }

        [Test]
        public void TestFastPlateJsonExample()
        {
            var myExampleJson = @"
{
    'UserAgent':[],
	'WebLayer': [
		'Module',
		'Ctrl',
		'View'
	],
	'AppLayer' : [
        'Provider',
		'Adapter',
		'Dal'
	],
    'AuthLayer' : [
        'Site',
        'Service'
    ],
    'DataLayer' : [
	    'Db01',
        'Db02',
        'Db03'
    ]
}
";
            var myFPlate = new FastPlate("CrazyRoutes", 8, new Rule { StartValue = 1, EndValue = 16, RuleLineSpacing = 2 }, myExampleJson);
            var idx2Nm = myFPlate.GetIndex2Name();
            foreach (var key in idx2Nm.Keys)
            {
                Console.WriteLine($"{key} {idx2Nm[key]}");
            }

            myFPlate.Blk(2.0)
                .Txt("GET")
                .Blk(1.1)
                .Blk(4.1)
                .Blk(4.2)
                .Blk(5.3)
                .Blk(4.2)
                .Blk(4.1)
                .Blk(1.1)
                .Blk(1.2)
                .Blk(3.1)
                .Blk(3.2)
                .Blk(3.3)
                .Blk(5.1)
                ;


            var myBlk = myFPlate.GetBlockByName("DataLayer");
            Assert.IsNotNull(myBlk);
            Console.WriteLine(myFPlate.ToString());
        }

        [Test]
        public void TestFastPlateExample()
        {
            var myFPlate = new FastPlate("Band Practice", null, "Left side", "Middle side", "Right side");
            myFPlate.Blk("Left side").Txt("big meaty claws").Blk("Right side");
            myFPlate.Blk("Right side").Txt("d'er not just for attractin' mates").Blk("Left side");
            myFPlate.Blk("Left side").Txt("yeah - bring it on").Blk("Right side").Blk(null);
            myFPlate.Blk("Middle side").Txt("no people,").Blk("Left side").Blk(null);
            myFPlate.Blk("Middle side").Txt("lets bring it off.").Blk("Right side");
            Console.WriteLine(myFPlate.ToString());

        }

        [Test]
        public void TestFastPlaceExample2()
        {
            var testSubject = new FastPlate("test with index", null, "Bob", "Sara", "Jim");
            testSubject[0].Txt("in Bob block")
                [1].Txt("in Sara block")
                [2].Txt("in Jim block")
                [0].Txt("back in Bob block")
                ;

            Console.WriteLine(testSubject.ToString());
        }

        [Test]
        public void TestFastPlateWithListValues()
        {
            var testSubject = new FastPlate("OAuth 2.0 Authorization Code Flow", null, "Resource Owner", "User-Agent", "Client", "Auth Server");
            testSubject.Blk("Client").Txt("Redirect to Auth Endpoint").Txt("client id").Txt("request scope").Txt("local state").Txt("redirection endpoint").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Redirect to Auth Endpoint").Blk("Auth Server");
            testSubject.Blk("Auth Server").Txt("Send back login screen").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Prompt For Creds").Blk("Resource Owner");
            testSubject.Blk("Resource Owner").Txt("Enter creds").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Post creds").Blk("Auth Server");
            testSubject.Blk("Auth Server").Txt("Auth Code & Redirect").Blk("User-Agent");
            testSubject.Blk("User-Agent").Txt("Post Auth code to Redirect").Blk("Client");
            testSubject.Blk("Client").Txt("Send Auth Code & Redirect").Blk("Auth Server");
            testSubject.Blk("Auth Server").Txt("Return Access Token").Blk("Client");
            Console.WriteLine(testSubject.ToString());
            
        }

        [Test]
        public void TestPrintYearsRange()
        {
            //  ((789,797),(804,816)) as "789-97\804-16"
            //  ((1797,1803)) as "1797-1803"
            //  ((640, 609)) as "640-609" 
            //  ((1588,null)) as "1588-"

            var testSubject = new Rule {StartValue = 700, EndValue = 850};
            var testResult =
                testSubject.PrintYearsRange(new List<Tuple<double?, double?>>()
                {
                    new Tuple<double?, double?>(789, 797),
                    new Tuple<double?, double?>(804, 816)
                });
            Assert.AreEqual(@"789-97\804-16", testResult);

            testSubject = new Rule { StartValue = 1788, EndValue = 1865 };
            testResult =
                testSubject.PrintYearsRange(new List<Tuple<double?, double?>>()
                {
                    new Tuple<double?, double?>(1797, 1803)
                });
            Assert.AreEqual("1797-1803", testResult);

            testSubject = new Rule { StartValue = 788, EndValue = 500 };
            testResult =
                testSubject.PrintYearsRange(new List<Tuple<double?, double?>>()
                {
                    new Tuple<double?, double?>(640, 609)
                });
            Assert.AreEqual("640-609", testResult);

            testSubject = new Rule { StartValue = 1488, EndValue = 1688 };
            testResult =
                testSubject.PrintYearsRange(new List<Tuple<double?, double?>>()
                {
                    new Tuple<double?, double?>(1588, null)
                });
            Assert.AreEqual("1588-", testResult);

            testResult =
                testSubject.PrintYearsRange(new List<Tuple<double?, double?>>()
                {
                    new Tuple<double?, double?>(null, 1588)
                });
            Assert.AreEqual("-1588", testResult);

            testResult =
                testSubject.PrintYearsRange(new List<Tuple<double?, double?>>()
                {
                    new Tuple<double?, double?>(1609, 1610)
                });

            Assert.AreEqual("1609\\10", testResult);

            testSubject = new Rule {StartValue = 922, EndValue = 780};
            testResult =
                testSubject.PrintYearsRange(new List<Tuple<double?, double?>>() {new Tuple<double?, double?>(842, 806)});

            Assert.AreEqual("842-806",testResult);
        }

        [Test]
        public void TestParseYearsRange()
        {
            var testSubject = new Rule { StartValue = 700, EndValue = 850 };
            var testResult = testSubject.ParseYearsRange(@"789-97\804-16");
            Assert.IsNotNull(testResult);
            var testList = testResult.ToList();
            Assert.AreNotEqual(0, testList.Count);
            Assert.AreEqual(789,testList[0].Item1);
            Assert.AreEqual(797, testList[0].Item2);
            Assert.AreEqual(804, testList[1].Item1);
            Assert.AreEqual(816, testList[1].Item2);

            testSubject = new Rule { StartValue = 1788, EndValue = 1865 };
            testResult = testSubject.ParseYearsRange("1797-1803");
            Assert.IsNotNull(testResult);
            testList = testResult.ToList();
            Assert.AreNotEqual(0, testList.Count);
            Assert.AreEqual(1797, testList[0].Item1);
            Assert.AreEqual(1803, testList[0].Item2);

            testSubject = new Rule { StartValue = 788, EndValue = 500 };
            testResult = testSubject.ParseYearsRange("640-609");
            Assert.IsNotNull(testResult);
            testList = testResult.ToList();
            Assert.AreNotEqual(0, testList.Count);
            Assert.AreEqual(640, testList[0].Item1);
            Assert.AreEqual(609, testList[0].Item2);

            testSubject = new Rule { StartValue = 1488, EndValue = 1688 };
            testResult = testSubject.ParseYearsRange("1588-");
            Assert.IsNotNull(testResult);
            testList = testResult.ToList();
            Assert.AreNotEqual(0, testList.Count);
            Assert.AreEqual(1588, testList[0].Item1);
            Assert.IsNull(testList[0].Item2);

            testResult = testSubject.ParseYearsRange("-1588");
            Assert.IsNotNull(testResult);
            testList = testResult.ToList();
            Assert.AreNotEqual(0, testList.Count);
            Assert.IsNull(testList[0].Item1);
            Assert.AreEqual(1588, testList[0].Item2);

            testResult = testSubject.ParseYearsRange("1609/10");
            Assert.IsNotNull(testResult);
            testList = testResult.ToList();
            Assert.AreNotEqual(0, testList.Count);
            Assert.AreEqual(1609, testList[0].Item1);
            Assert.AreEqual(1610, testList[0].Item2);


            testResult = testSubject.ParseYearsRange("1609\\1610");
            Assert.IsNotNull(testResult);
            testList = testResult.ToList();
            Assert.AreNotEqual(0, testList.Count);
            Assert.AreEqual(1609, testList[0].Item1);
            Assert.AreEqual(1610, testList[0].Item2);
        }

        [Test]
        public void TestPrintStartEndValue()
        {
            var testRule = new Rule { StartValue = 510, EndValue = 320 };
            var testEntry = new SimpleFactEntry() { Ruler = testRule, StartValue = 488, EndValue = 409 };
            var testResult = testEntry.PrintStartEndValue();

            Assert.AreEqual("488-409", testResult);
            testEntry = new SimpleFactEntry() { StartValue = 488, EndValue = 409 };
            testResult = testEntry.PrintStartEndValue();
            Assert.AreEqual("488-409", testResult);

            testEntry = new SimpleFactEntry();
            Assert.AreEqual("", testEntry.PrintStartEndValue());

            testEntry = new SimpleFactEntry() { StartValue = 1944 };
            testResult = testEntry.PrintStartEndValue();
            Assert.AreEqual("1944", testResult);

        }
        [Test]
        public void TestTerritoryEntry()
        {
            var testSubject = new TerritoryEntry("MO") {StartValue = 1821};
            Assert.AreEqual("+MO(1821)", testSubject.Text);

            testSubject.Text = "+WI(1848)";

            Assert.AreEqual("WI",testSubject.Name);
            Assert.AreEqual(new int?(1848), testSubject.StartValue);
            
        }

        [Test]
        public void TestLeaderEntry()
        {
            var testSubject = new LeaderEntry("Washington",new int?[,] { { 1789, 1797 } })
            { 
                Ruler = new Rule() { StartValue = 1788, EndValue = 1865}
            };

            Assert.AreEqual("[Washington 1789-97]", testSubject.Text);


            testSubject = new LeaderEntry("William H Harrison",new int?[,] { { null, 1841 } })
            { 
                Ruler = new Rule() { StartValue = 1788, EndValue = 1865 }
            };

            Assert.AreEqual("[William H Harrison -1841]", testSubject.Text);

            testSubject = new LeaderEntry("H. John Temple",new int?[,] { { 1855, 1858 }, { 1859, 1865 } })
            { 
                Ruler = new Rule() { StartValue = 1788, EndValue = 1865 }
            };

            Assert.AreEqual("[H. John Temple 1855-58\\59-65]",testSubject.Text);

            testSubject = new LeaderEntry("John Adams",new int?[,] { { 1797, 1801 } })
            { 
                Ruler = new Rule() { StartValue = 1788, EndValue = 1865 }
            };

            Assert.AreEqual("[John Adams 1797-1801]", testSubject.Text);

            testSubject = new LeaderEntry("Hezekiah",new int?[,] { { 715, 687 } })
            { 
                Ruler = new Rule { StartValue = 780, EndValue = 500 }
            };

            Assert.AreEqual("[Hezekiah 715-687]", testSubject.Text);
            testSubject = new LeaderEntry("Josiah",new int?[,] { { 640, 609 } })
            { 
                Ruler = new Rule { StartValue = 780, EndValue = 500 }
            };
            Assert.AreEqual("[Josiah 640-609]",testSubject.Text);
        }

        [Test]
        public void TestScienceAdvEntry()
        {
            var testSubject = new ScienceAdvEntry("Ohm's law", "Ohm")
            {
                Ruler = new Rule {StartValue = 1788, EndValue = 1865},
                StartValue = 1827
            };
            Assert.AreEqual("Ohm[Ohm's law](1827)", testSubject.Text);

            testSubject = new ScienceAdvEntry("law of induction", "Faraday")
            {
                Ruler = new Rule { StartValue = 1788, EndValue = 1865 },
                StartValue = 1831
            };

            Assert.AreEqual("Faraday[law of induction](1831)", testSubject.Text);
        }

        [Test]
        public void TestLiteraryWorkEntry()
        {
            var testSubject = new LiteraryWorkEntry("Origin of Species", "Darwin")
            {
                Ruler = new Rule {StartValue = 1788, EndValue = 1865},
                StartValue = 1859
            };

            var testResult = testSubject.Text;
            Assert.AreEqual("'Origin of Species'Darwin(1859)",testResult);

            testSubject = new LiteraryWorkEntry("Communist Manifesto", null)
            {
                Ruler = new Rule { StartValue = 1788, EndValue = 1865 },
                StartValue = 1848
            };
            testResult = testSubject.Text;
            Assert.AreEqual("'Communist Manifesto'(1848)", testResult);

        }

        [Test]
        public void TestPhilosopherEntry()
        {
            var testSubject = new PhilosopherEntry("Wittgenstein", 1889, 1951);
            var testResult = testSubject.Text;

            Assert.AreEqual("(Wittgenstein 1889-1951)", testResult);

            testSubject = new PhilosopherEntry("The Dude", null, null);
            Assert.AreEqual("(The Dude)", testSubject.Text);

            testSubject = new PhilosopherEntry("", null, null);
            testSubject.Text = "(Wittgenstein 1889-1951)";
            Assert.AreEqual("Wittgenstein", testSubject.Name);
            Assert.AreEqual(1889, testSubject.Yob);
            Assert.AreEqual(1951, testSubject.Yod);

        }

        [Test]
        public void TestProphetEntry()
        {
            var testSubject = new ProphetEntry("Iggy");
            Assert.AreEqual("(Iggy)", testSubject.Text);

            testSubject = new ProphetEntry(null);
            testSubject.Text = "(Haggai, Zechariah)";
            Assert.AreEqual("Haggai, Zechariah", testSubject.Name);
        }

        [Test]
        public void TestHeresyEntry()
        {
            var testSubject = new HeresyEntry("Disco", "not cool");
            Assert.AreEqual("[H] Disco (not cool)", testSubject.Text);

            testSubject = new HeresyEntry(null, null);
            testSubject.Text = "[H] Manichaeism (duality, east import)";

            Assert.AreEqual("Manichaeism", testSubject.Name);
            Assert.AreEqual("duality, east import", testSubject.Description);

        }

        [Test]
        public void TestExplorerEntry()
        {
            var testSubject = new ExplorerEntry("H.Hudson", "Hudson Riv.&Bay");

            Assert.AreEqual("[H.Hudson]-Hudson Riv.&Bay", testSubject.Text);

            testSubject = new ExplorerEntry("H.Hudson", "Hudson Riv.&Bay") {StartValue = 1609, EndValue = 1610};

            Assert.AreEqual("[H.Hudson]-Hudson Riv.&Bay(1609\\10)", testSubject.Text);

            testSubject = new ExplorerEntry(null, null);
            testSubject.Text = "[H.Hudson]-Hudson Riv.&Bay(1609\\10)";
            Assert.AreEqual("H.Hudson", testSubject.ExplorerName);
            Assert.AreEqual("Hudson Riv.&Bay", testSubject.Area);
            Assert.AreEqual(1609,testSubject.StartValue);
            Assert.AreEqual(1610, testSubject.EndValue);
        }

    }
}
