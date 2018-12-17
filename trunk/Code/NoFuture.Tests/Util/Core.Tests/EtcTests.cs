using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Shared.Core;
using NUnit.Framework;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class EtcTests
    {
        [Test]
        public void TestDistillCrLf()
        {
            var testInput = @"    He has refused his Assent to Laws, the most wholesome and necessary for the public good.
    He has forbidden his Governors to pass Laws of immediate and pressing importance, ";
            var testResult = NfString.DistillCrLf(testInput);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestDistillSpaces()
        {
            var testResult = NfString.DistillSpaces("              here             is          to many           spaces");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(" here is to many spaces", testResult);

            testResult =
                NfString.DistillSpaces(
                    "            December 31,      2016    2015   (in millions)Net operating losses         ");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(" December 31, 2016 2015 (in millions)Net operating losses ", testResult);
            Console.WriteLine(testResult);

        }

        [Test]
        public void TestDistillString()
        {
            var testInput = @"
 
						 
						 
						 
						 
						 
						  
						 December 31,      2016    2015 
						 (in millions)Net operating losses
						 
						 
						 
						 
						 
						 Balance at January 1, 
						 $4.2
				
						 $78.1
				Permanent loss of tax benefit related to NOLs limited by ownership change
						 
						   —
						 
						 (72.5)
				NOL generated (utilized)
						 
						 (2.2)
				
						 
						 (1.7)
				NOL expired unused
						 
						   —
						 
						   —Other, including changes in foreign exchange rates
						 
						 0.1
				
						 
						 0.3
				Balance at December 31, 
						 $  2.1
				
						 $4.2
				 
";
            var testResult = testInput.DistillString();
            Assert.IsNotNull(testResult);
            Assert.AreEqual(" December 31, 2016 2015 (in millions)Net operating losses Balance at January 1, $4.2 $78.1 Permanent loss of tax benefit related to NOLs limited by ownership change — (72.5) NOL generated (utilized) (2.2) (1.7) NOL expired unused — —Other, including changes in foreign exchange rates 0.1 0.3 Balance at December 31, $ 2.1 $4.2 ", testResult);
            Console.WriteLine(testResult);

        }


        [Test]
        public void TestToOrdinal()
        {
            Assert.AreEqual("1st", 1.ToOrdinal());
            Assert.AreEqual("2nd", 2.ToOrdinal());
            Assert.AreEqual("3rd", 3.ToOrdinal());
            Assert.AreEqual("4th", 4.ToOrdinal());
            Assert.AreEqual("256th", 256.ToOrdinal());
            Assert.AreEqual("1,000th", 1000.ToOrdinal());
            
        }

        [Test]
        public void TestCapitalizeFirstLetterOfWholeWords()
        {
            const string typicalTypeName = "noFuture.util.etc";
            var testResult = NfString.CapWords(typicalTypeName, '.');
            Assert.AreEqual("NoFuture.Util.Etc", testResult);

            const string allCaps = "KEYCODE";
            testResult = NfString.CapWords(allCaps, '.');
            Assert.AreEqual("Keycode", testResult);

            const string capsLockOn = "nOFUTURE.uTIL.eTC";
            testResult = NfString.CapWords(capsLockOn, '.');
            Assert.AreEqual("Nofuture.Util.Etc",testResult);

            testResult = NfString.CapWords("WINSTON SALEM".ToLower(), ' ');
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestEscapeString()
        {
            const string DEC_EXPECT = "&#73;&#32;&#97;&#109;&#32;&#100;&#101;&#99;&#105;&#109;&#97;&#108;";
            var dec = NfString.EscapeString("I am decimal", EscapeStringType.DECIMAL);

            const string UNI_EXPECT = @"\u00C0\u00C8\u00CC\u00D2\u00D9\u00E0\u00E8\u00EC\u00F2\u00F9\u00C1\u00C9\u00CD\u00D3\u00DA\u00DD\u00E1\u00E9\u00ED\u00F3\u00FA\u00FD\u00C2\u00CA\u00CE\u00D4\u00DB\u00E2\u00EA\u00EE\u00F4\u00FB\u00C3\u00D1\u00D5\u00E3\u00F1\u00F5\u00C4\u00CB\u00CF\u00D6\u00DC\u00E4\u00EB\u00EF\u00F6\u00FC\u00E7\u00C7\u00DF\u00D8\u00F8\u00C5\u00E5\u00C6\u00E6\u00DE\u00FE\u00D0\u00F0\u0152\u0153\u0178\u00FF\u0160\u0161";
            var uni = NfString.EscapeString("ÀÈÌÒÙàèìòùÁÉÍÓÚÝáéíóúýÂÊÎÔÛâêîôûÃÑÕãñõÄËÏÖÜäëïöüçÇßØøÅåÆæÞþÐðŒœŸÿŠš", EscapeStringType.UNICODE);

            const string REGEX_EXPECT = @"\x5b\x72\x65\x67\x65\x78\x5d";
            var regex = NfString.EscapeString("[regex]", EscapeStringType.REGEX);

            const string HTML_EXPECT = "&nbsp;&pound;&iexcl;&yen;&sect;";
            var html = NfString.EscapeString(" £¡¥§", EscapeStringType.HTML);

            const string URI_EXPECT = "F%40r0ut%7eDu%2cde%3d";
            var uri = NfString.EscapeString("F@r0ut~Du,de=", EscapeStringType.URI);

            Assert.AreEqual(DEC_EXPECT,dec);
            Assert.AreEqual(UNI_EXPECT,uni);
            Assert.AreEqual(REGEX_EXPECT,regex);
            Assert.AreEqual(HTML_EXPECT,html);
            Assert.AreEqual(URI_EXPECT, uri);

        }

        [Test]
        public void TestPrintInCenter()
        {
            var textInput = "Judea";
            var printBlock = 26;

            var testResult = NfString.PrintInCenter(textInput, printBlock);

            Assert.IsNotNull(testResult);

            Console.WriteLine(string.Format("|{0}|", testResult));
        }

        [Test]
        public void TestMergeString()
        {
            var testPrimaryInput = "   <~~~Some~Name";
            var testSecondaryInput = "                   ";

            var testResult = NfString.MergeString(testPrimaryInput, testSecondaryInput);

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestBinaryMergeString()
        {
            var firstString = " a typical string ";
            var secondString = "--+|---------------";

            var testResult = NfString.BinaryMergeString(firstString, secondString);

            Assert.IsNotNull(testResult);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestCalcLuhnCheckDigit()
        {
            var testResult = NfString.CalcLuhnCheckDigit("455673758689985");
            Assert.AreEqual(5, testResult);
            testResult = NfString.CalcLuhnCheckDigit("453211001754030");
            Assert.AreEqual(9,testResult);
            testResult = NfString.CalcLuhnCheckDigit("471604448140316");
            Assert.AreEqual(5, testResult);
            testResult = NfString.CalcLuhnCheckDigit("554210251648257");
            Assert.AreEqual(5, testResult);
            testResult = NfString.CalcLuhnCheckDigit("537886423943754");
            Assert.AreEqual(6, testResult);
            testResult = NfString.CalcLuhnCheckDigit("511329925461278");
            Assert.AreEqual(2, testResult);
            testResult = NfString.CalcLuhnCheckDigit("37322049976972");
            Assert.AreEqual(0, testResult);
            testResult = NfString.CalcLuhnCheckDigit("34561114407525");
            Assert.AreEqual(4, testResult);
            testResult = NfString.CalcLuhnCheckDigit("34831152135173");
            Assert.AreEqual(6, testResult);
            testResult = NfString.CalcLuhnCheckDigit("601198900163944");
            Assert.AreEqual(0, testResult);
            testResult = NfString.CalcLuhnCheckDigit("3653092434341");
            Assert.AreEqual(5, testResult);
        }

        [Test]
        public void TestDistillTabs()
        {
            var testInput =
                new string(new[]
                {
                    (char) 0x09, (char) 0x09, 'a', (char) 0x20, 'b', 'c', 'd', (char) 0x09, (char) 0x09, (char) 0x09,
                    (char) 0x09, 'e', 'f', (char) 0x20
                });

            var testResult = NfString.DistillTabs(testInput);
            Assert.AreEqual(" a bcd ef ",testResult);

        }
        [Test]
        public void TestTransformScreamingCapsToCamelCase()
        {
            const string TEST_INPUT = "USER_NAME";
            var testOutput = NfString.ToPascelCase(TEST_INPUT);
            Assert.AreEqual("userName", testOutput);
        }

        [Test]
        public void TestToCamelCase()
        {
            const string TEST_INPUT = "UserName";
            var testOutput = NfString.ToCamelCase(TEST_INPUT, true);
            Assert.AreEqual("userName", testOutput);

            testOutput = NfString.ToCamelCase("__" + TEST_INPUT, true);
            Assert.AreEqual("__userName", testOutput);

            testOutput = NfString.ToCamelCase("__" + TEST_INPUT.ToUpper(), true);
            Assert.AreEqual("__username", testOutput);

            testOutput = NfString.ToCamelCase("ID", true);
            Assert.AreNotEqual("iD", testOutput);
            Assert.AreEqual("id", testOutput);

            testOutput = NfString.ToCamelCase("498375938720", true);
            Assert.AreEqual("498375938720", testOutput);

            testOutput = NfString.ToCamelCase("__userNAME_ID", true);
            Assert.AreEqual("__userName_Id", testOutput);

            testOutput = NfString.ToCamelCase("The-VariousThings\\which,AllowYou ToRead=this");
            Assert.AreEqual("theVariousThingsWhichAllowYouToReadThis", testOutput);

            testOutput = NfString.ToCamelCase("Server Purpose(s); Installed Component(s)");
            Assert.AreEqual("serverPurposeSInstalledComponentS", testOutput);

        }

        [Test]
        public void TestTransformCamelCaseToSeparator()
        {
            const string TEST_INPUT = "UserName";
            var testOutput = NfString.TransformCaseToSeparator(TEST_INPUT, '_');
            Assert.AreEqual("User_Name", testOutput);

            testOutput = NfString.TransformCaseToSeparator("user_Name", '_');
            Assert.AreEqual("user_Name", testOutput);

        }
        [Test]
        public void TestToPascelCase()
        {
            var testResult = NfString.ToPascelCase("dbo.DELETED_LookupDetails");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("DboDeletedLookupDetails",testResult);

            testResult = NfString.ToPascelCase("dbo.DELETED_LookupDetails", true);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("Dbo.Deleted_LookupDetails",testResult);

            testResult = NfString.ToPascelCase("Test.dbo.SET_OP_lli", true);
            Assert.AreEqual("Test.dbo.Set_Op_lli",testResult);
        }

        [Test]
        public void TestDistillToWholeWords()
        {
            var testResult =
                NfString.DistillToWholeWords(
                    "FilmMaster-AccountDetail-ClientDetails-LocationDetails-TimeMasters-IsGolfVoucher");
            Assert.IsNotNull(testResult);
            
            Assert.AreEqual("Film Master Account Detail Client Details Location Time Masters Is Golf Voucher", string.Join(" ", testResult));

            testResult =
                NfString.DistillToWholeWords("Id");

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Length == 1);
            Assert.AreEqual("Id",testResult[0]);

            testResult = NfString.DistillToWholeWords("RTDC IR Questions");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("RtdcIrQuestions", string.Join("", testResult));

            testResult = NfString.DistillToWholeWords("The-VariousThings\\which,AllowYou ToRead=this");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(9, testResult.Length);
            Assert.AreEqual("TheVariousThingsWhichAllowYouToReadThis", string.Join("",testResult));
        }

        [Test]
        public void TestFormatCsvHeaders()
        {
            var testInput = new string[]
            {
                "S. No:","Screen / Sub Tab","Field Name","Section","Functionality","Type","Functionality","Type","WE Mapping","Required ?","Comments"
            };
            var testResult = Etc.FormatCsvHeaders(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach(var tr in testResult)
                Console.WriteLine(tr);
        }

        [Test]
        public void TestFormatJson()
        {
            var testJson =
                "[{\"ScheduleId\":5849,\"Uuid\":\"20012473\",\"ProgramId\":994564,\"Relation\":\"E\",\"FirstName\":\"Randall\",\"MiddleInitial\":null,\"LastName\":\"Perea's\",\"DateOfBirth\":\"1973-03-06T00:00:00\",\"Gender\":\"Male\",\"PhoneNumber\":\"2395730480\",\"Email\":null,\"AddressLine1\":\"3900 Cedar St\",\"AddressLine2\":null,\"City\":\"Arcadia\",\"State\":\"FL\",\"ZipCode\":\"33809\",\"ScheduledTime\":\"2013-10-30T07:30:00\"}]";
            var testResult = Etc.FormatJson(testJson);
            Assert.IsNotNull(testResult);

            testResult = Etc.FormatJson(testJson, true);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestFormatXml()
        {
            var testXml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><buildTypes><buildType id=\"Whiskey_Quebec_Motel_Romeo_Whiskey_Alpha\" name=\"API\"><builds><build id=\"9945412\" number=\"0000FF.0A.344D1.2B\" status=\"SUCCESS\"><statusText>Success</statusText></build></builds></buildType></buildTypes>";
            var testResult = Etc.FormatXml(testXml);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestIsBetween()
        {
            
            Assert.IsTrue(new DateTime(2010, 1, 1).IsBetween(new DateTime(2009, 12, 31), new DateTime(2010, 1, 2)));

            Assert.IsFalse(new DateTime(2009, 12, 30).IsBetween(new DateTime(2009, 12, 31), new DateTime(2010, 1, 2)));

            Assert.IsTrue(new DateTime(2010, 1, 1).IsBetween(new DateTime(2009, 1, 1), new DateTime(2010, 1, 2)));
        }

        [Test]
        public void TestToYears()
        {
            var testInput = new TimeSpan(365, 0, 0, 0);

            var testResult = testInput.ToYears();
            Assert.AreNotEqual(0D, testResult);
            Assert.IsTrue(testResult < 1D);

            testInput = Constants.TropicalYear;
            testResult = testInput.ToYears();
            Assert.AreEqual(1D, testResult);
            System.Diagnostics.Debug.Write(testResult);

        }

        [Test]
        public void TestMonthAbbrev()
        {
            var testInput = new DateTime(DateTime.Today.Year,1,1);
            var testResult = testInput.MonthAbbrev();
            Assert.AreEqual("Jan", testResult);

            testResult = testInput.AddMonths(11).MonthAbbrev();
            Assert.AreEqual("Dec", testResult);
        }


        [Test]
        public void TestSafeDotNetTypeName()
        {
            var testResult = NfString.SafeDotNetTypeName("dbo.123ProcName");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("dbo.123ProcName", testResult);
            Console.WriteLine(testResult);

            testResult = NfString.SafeDotNetTypeName(null);
            Assert.IsNotNull(testResult);

            var testInput = string.Empty;
            testResult = NfString.SafeDotNetTypeName(testInput);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Console.WriteLine(testResult);

            testInput = "     ";
            testResult = NfString.SafeDotNetTypeName(testInput);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Console.WriteLine(testResult);

            testResult = NfString.SafeDotNetTypeName("dbo.DELETED_LookupDetails");
            Console.WriteLine(testResult);
            Assert.AreEqual("dbo.DELETED_LookupDetails", testResult);

            testResult = NfString.SafeDotNetTypeName("Â© The End");

            Console.WriteLine(testResult);

        }

        [Test]
        public void TestSafeDotNetIdentifier()
        {
            var testResult = NfString.SafeDotNetIdentifier("Personal Ph #", true);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("Personal_u0020Ph_u0020_u0023", testResult);

            testResult = NfString.SafeDotNetIdentifier("Personal Ph #");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("PersonalPh", testResult);

            testResult = NfString.SafeDotNetIdentifier("global::Some_Aspx_Page_With_No_Namespace");
            Console.WriteLine(testResult);

            Assert.AreEqual("globalSome_Aspx_Page_With_No_Namespace", testResult);

            testResult =
                NfString.SafeDotNetIdentifier(
                    "<p><font style='font-size:11px;font-family:calibri;text-align:left'>", true);
            Console.WriteLine(testResult);

            Assert.IsTrue(testResult.StartsWith(NfString.DefaultNamePrefix + "_u003cp_u003e_u003cfont_u0020style"));

            testResult =
                NfString.SafeDotNetIdentifier("Â© The End Â©", false);
            Console.WriteLine(testResult);

            Assert.AreEqual("_u0000TheEnd", testResult);

            testResult =
                NfString.SafeDotNetIdentifier("Â© The End Â©", true);
            Console.WriteLine(testResult);

            Assert.AreEqual("_u00c2_u00a9_u0020The_u0020End_u0020_u00c2_u00a9", testResult);
        }

        [Test]
        public void TestBlankOutLines()
        {
            var testContent = new[]
            {
                "The following training has been completed:",
                "",
                "Training Title: Compliance General Data Protection Regulation (GDPR)",
                "Training Provider:Vinnie (the fists) Brooker ",
                "Description: This training is an introduction to the new European Union (EU) General Data Protection Regulation (GDPR)",
                "If you have any questions, please reach out to your supervisor or contact the Vinnie at 855-555-8888."
            };
            var lns = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>
            {
                new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(2, 15), new Tuple<int, int>(3, 8))
            };
            var testResult = NoFuture.Util.Core.Etc.BlankOutLines(testContent, lns);
            Console.WriteLine(string.Join(Environment.NewLine, testResult));

            Assert.AreEqual("Training Title:                                                     ", testResult[2]);
            Assert.AreEqual("         Provider:Vinnie (the fists) Brooker ", testResult[3]);
        }

        [Test]
        public void TestBlankOutLine2()
        {
            var testContent = new[]
            {
                "The following training has been completed:",
                "",
                "Training Title: Compliance General Data Protection Regulation (GDPR)",
                "Training Provider:Vinnie (the fists) Brooker ",
                "Description: This training is an introduction to the new European Union (EU) General Data Protection Regulation (GDPR)",
                "If you have any questions, please reach out to your supervisor or contact the Vinnie at 855-555-8888."
            };

            var testResult = Etc.BlankOutLines(testContent, 2, 15, 3, 8);
            Assert.AreEqual("Training Title:                                                     ", testResult[2]);
            Assert.AreEqual("         Provider:Vinnie (the fists) Brooker ", testResult[3]);

            //mismatch - has no effect
            testResult = Etc.BlankOutLines(testContent, 2, 15, 3);
            Assert.AreEqual("Training Title: Compliance General Data Protection Regulation (GDPR)", testResult[2]);
            Assert.AreEqual("Training Provider:Vinnie (the fists) Brooker ", testResult[3]);


        }

        [Test]
        public void TestToWordWrap()
        {
            var sometext = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi hendrerit leo id justo interdum, eu luctus urna tincidunt. Integer eu ex libero. Curabitur quis est volutpat, suscipit massa iaculis, posuere nisl. Vivamus mollis erat sed sem vulputate auctor. Fusce id dictum leo. Donec sed lectus scelerisque turpis sollicitudin vehicula quis non nisi. Sed et imperdiet ex, sed facilisis quam. Morbi scelerisque neque eget massa tincidunt, at finibus mauris gravida. Suspendisse nec luctus justo. Ut porta pretium mi, ac tristique ligula tempor id. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Cras at velit ac mi congue posuere. Etiam rutrum risus lacus, id luctus diam pretium vulputate. Ut sed neque tortor. Ut tempor augue sem.";
            //var testResult = Etc.ToWordWrap(sometext);
            var expected = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi hendrerit leo id 
justo interdum, eu luctus urna tincidunt. Integer eu ex libero. Curabitur quis 
est volutpat, suscipit massa iaculis, posuere nisl. Vivamus mollis erat sed sem 
vulputate auctor. Fusce id dictum leo. Donec sed lectus scelerisque turpis sollicitudin 
vehicula quis non nisi. Sed et imperdiet ex, sed facilisis quam. Morbi scelerisque 
neque eget massa tincidunt, at finibus mauris gravida. Suspendisse nec luctus justo. 
Ut porta pretium mi, ac tristique ligula tempor id. Cum sociis natoque penatibus 
et magnis dis parturient montes, nascetur ridiculus mus. Cras at velit ac mi congue 
posuere. Etiam rutrum risus lacus, id luctus diam pretium vulputate. Ut sed neque 
tortor. Ut tempor augue sem.".Replace("\n", "\r\n");

            //Assert.AreEqual(expected, testResult);

            sometext = @"Thomas and wife v. Winchester3
Court of Appeals of New York 4
Decided July 1852 5
6 NY 397
RUGGLES, Ch. J.6 delivered the opinion of the court.
This is an action brought to recover damages from the defendant for negligently7 putting up, labeling and selling as and for the extract of dandelion, which is a simple and harmless medicine, a jar of the extract of belladonna8, which is a deadly poison; by means of which the plaintiff Mary Ann Thomas, to whom, being sick, a dose of dandelion was prescribed by a physician, and a portion of the contents of the jar, was administered as and for the extract of dandelion, was greatly injured.";
            var testResult = NfString.ToWordWrap(sometext);
            Console.Write(testResult);

        }

        [Test]
        public void TestJaroWinklerDistance()
        {
            var testResult = NfString.JaroWinklerDistance("test", "test");
            Assert.AreEqual(1D, System.Math.Round(testResult));

            testResult = NfString.JaroWinklerDistance("kitty", "kitten");
            Assert.IsTrue(testResult - 0.893 < 0.001);

            testResult = NfString.JaroWinklerDistance("kitty", "kite");
            Assert.IsTrue(testResult - 0.848 < 0.001);
            Console.WriteLine(testResult);

            testResult = NfString.JaroWinklerDistance(null, null);
            Assert.AreEqual(1.0, testResult);
        }

        [Test]
        public void TestLevenshteinDistance()
        {
            var testResult = NfString.LevenshteinDistance("kitten", "sitting");
            Assert.AreEqual(3D, testResult);
            testResult = NfString.LevenshteinDistance("Saturday", "Sunday");
            Assert.AreEqual(3D, testResult);
            testResult = NfString.LevenshteinDistance("Brian", "Brain");
            Assert.AreEqual(2D, testResult);

            Console.WriteLine(NfString.LevenshteinDistance("kitty", "kitten"));
            Console.WriteLine(NfString.LevenshteinDistance("kitty", "kite"));

            //testResult = Etc.LevenshteinDistance("sword", "swath", true);
            //Console.WriteLine(testResult);
        }

        [Test]
        public void TestShortestDistance()
        {
            var testIn = "kitty";
            var testCompare = new[] { "kitten", "cat", "kite", "can", "kool" };

            var testResult = NfString.ShortestDistance(testIn, testCompare);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(2, testResult.Length);
            Assert.IsTrue(testResult.Contains("kitten"));
            Assert.IsTrue(testResult.Contains("kite"));

            testIn = "LeRoy";
            testCompare = new[] { "Lee", "Roy", "L.R." };
            testResult = NfString.ShortestDistance(testIn, testCompare);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(1, testResult.Length);
            Assert.AreEqual("Roy", testResult[0]);

        }
    }
}
