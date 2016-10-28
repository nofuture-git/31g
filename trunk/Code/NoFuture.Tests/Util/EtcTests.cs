using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Shared;
using NoFuture.Util;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class EtcTests
    {
        [TestMethod]
        public void TestDistillSpaces()
        {
            var testInput = @"    He has refused his Assent to Laws, the most wholesome and necessary for the public good.
    He has forbidden his Governors to pass Laws of immediate and pressing importance, ";
            var testResult = NoFuture.Util.Etc.DistillString(testInput);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestCapitalizeFirstLetterOfWholeWords()
        {
            const string typicalTypeName = "noFuture.util.etc";
            var testResult = NoFuture.Util.Etc.CapitalizeFirstLetterOfWholeWords(typicalTypeName, '.');
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("NoFuture.Util.Etc", testResult);

            const string allCaps = "KEYCODE";
            testResult = NoFuture.Util.Etc.CapitalizeFirstLetterOfWholeWords(allCaps, '.');
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("Keycode", testResult);

            const string capsLockOn = "nOFUTURE.uTIL.eTC";
            testResult = NoFuture.Util.Etc.CapitalizeFirstLetterOfWholeWords(capsLockOn, '.');
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("Nofuture.Util.Etc",testResult);

            testResult = NoFuture.Util.Etc.CapitalizeFirstLetterOfWholeWords("A typical English sentence looks like this.", ' ');
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestEscapeString()
        {
            const string DEC_EXPECT = "&#73;&#32;&#97;&#109;&#32;&#100;&#101;&#99;&#105;&#109;&#97;&#108;";
            var dec = NoFuture.Util.Etc.EscapeString("I am decimal", EscapeStringType.DECIMAL);

            const string UNI_EXPECT = @"\u00C0\u00C8\u00CC\u00D2\u00D9\u00E0\u00E8\u00EC\u00F2\u00F9\u00C1\u00C9\u00CD\u00D3\u00DA\u00DD\u00E1\u00E9\u00ED\u00F3\u00FA\u00FD\u00C2\u00CA\u00CE\u00D4\u00DB\u00E2\u00EA\u00EE\u00F4\u00FB\u00C3\u00D1\u00D5\u00E3\u00F1\u00F5\u00C4\u00CB\u00CF\u00D6\u00DC\u00E4\u00EB\u00EF\u00F6\u00FC\u00E7\u00C7\u00DF\u00D8\u00F8\u00C5\u00E5\u00C6\u00E6\u00DE\u00FE\u00D0\u00F0\u0152\u0153\u0178\u00FF\u0160\u0161";
            var uni = NoFuture.Util.Etc.EscapeString("ÀÈÌÒÙàèìòùÁÉÍÓÚÝáéíóúýÂÊÎÔÛâêîôûÃÑÕãñõÄËÏÖÜäëïöüçÇßØøÅåÆæÞþÐðŒœŸÿŠš", EscapeStringType.UNICODE);

            const string REGEX_EXPECT = @"\x5b\x72\x65\x67\x65\x78\x5d";
            var regex = NoFuture.Util.Etc.EscapeString("[regex]", EscapeStringType.REGEX);

            const string HTML_EXPECT = "&nbsp;&pound;&iexcl;&yen;&sect;";
            var html = NoFuture.Util.Etc.EscapeString(" £¡¥§", EscapeStringType.HTML);

            Assert.AreEqual(DEC_EXPECT,dec);
            Assert.AreEqual(UNI_EXPECT,uni);
            Assert.AreEqual(REGEX_EXPECT,regex);
            Assert.AreEqual(HTML_EXPECT,html);

        }

        [TestMethod]
        public void TestPrintInCenter()
        {
            var textInput = "Judea";
            var printBlock = 26;

            var testResult = Etc.PrintInCenter(printBlock, textInput);

            Assert.IsNotNull(testResult);

            System.Diagnostics.Debug.WriteLine(string.Format("|{0}|", testResult));
        }

        [TestMethod]
        public void TestMergeString()
        {
            var testPrimaryInput = "   <~~~Some~Name";
            var testSecondaryInput = "                   ";

            var testResult = Etc.MergeString(testPrimaryInput, testSecondaryInput);

            Assert.IsNotNull(testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestBinaryMergeString()
        {
            var firstString = " a typical string ";
            var secondString = "--+|---------------";

            var testResult = Etc.BinaryMergeString(firstString, secondString);

            Assert.IsNotNull(testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestCalcLuhnCheckDigit()
        {
            var testResult = Etc.CalcLuhnCheckDigit("455673758689985");
            Assert.AreEqual(5, testResult);
            testResult = Etc.CalcLuhnCheckDigit("453211001754030");
            Assert.AreEqual(9,testResult);
            testResult = Etc.CalcLuhnCheckDigit("471604448140316");
            Assert.AreEqual(5, testResult);
            testResult = Etc.CalcLuhnCheckDigit("554210251648257");
            Assert.AreEqual(5, testResult);
            testResult = Etc.CalcLuhnCheckDigit("537886423943754");
            Assert.AreEqual(6, testResult);
            testResult = Etc.CalcLuhnCheckDigit("511329925461278");
            Assert.AreEqual(2, testResult);
            testResult = Etc.CalcLuhnCheckDigit("37322049976972");
            Assert.AreEqual(0, testResult);
            testResult = Etc.CalcLuhnCheckDigit("34561114407525");
            Assert.AreEqual(4, testResult);
            testResult = Etc.CalcLuhnCheckDigit("34831152135173");
            Assert.AreEqual(6, testResult);
            testResult = Etc.CalcLuhnCheckDigit("601198900163944");
            Assert.AreEqual(0, testResult);
            testResult = Etc.CalcLuhnCheckDigit("3653092434341");
            Assert.AreEqual(5, testResult);
        }

        [TestMethod]
        public void TestDistillTabs()
        {
            var testInput =
                new string(new[]
                {
                    (char) 0x09, (char) 0x09, 'a', (char) 0x20, 'b', 'c', 'd', (char) 0x09, (char) 0x09, (char) 0x09,
                    (char) 0x09, 'e', 'f', (char) 0x20
                });

            var testResult = Etc.DistillTabs(testInput);
            Assert.AreEqual(" a bcd ef ",testResult);

        }

        [TestMethod]
        public void TestTransformScreamingCapsToCamelCase()
        {
            var testResult = NoFuture.Util.Etc.TransformScreamingCapsToCamelCase("dbo.DELETED_LookupDetails");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("dboDeletedLookupDetails",testResult);

            testResult = NoFuture.Util.Etc.TransformScreamingCapsToCamelCase("dbo.DELETED_LookupDetails", true);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("dbo.deletedLookupDetails",testResult);
        }

        [TestMethod]
        public void TestFindCountOfCharNotInQuotes()
        {
            var testInput = "Intangible assets (amortizable)..	\"4,827,472,371\"	\"3,923,959\"	\"77,931,345\"	\"66,159,637\"	\"23,333,208\"	\"1,707,006,308\"	\"497,845,683\"	\"311,360,015\"	\"186,485,668\"	\"73,174,724\"	\"901,501,338\"	\"415,461,943\"	\"71,073,527\"	\"260,860,340\"	\"384,047,374\"	\"106,072,424\"	\"14,123,668\"	\"98,147,343\"	\"18,055,586\"	\"85,134,982\"	\"23,618,983\"		";
            var testResult = Etc.FindCountOfCharNotInQuotes(testInput, (char) 0x09);

            Assert.AreNotEqual(0, testResult);
            Assert.AreEqual(23, testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestDistillToWholeWords()
        {
            var testResult =
                NoFuture.Util.Etc.DistillToWholeWords(
                    "ProgramMaster-AccountDetail-ClientDetails-SiteDetails-ClinicMasters-IsFluVoucher");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(string.Join("", testResult));

            testResult =
                NoFuture.Util.Etc.DistillToWholeWords("Id");

            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(string.Join("",testResult));

            testResult = NoFuture.Util.Etc.DistillToWholeWords("RTDC IR Questions");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(string.Join("", testResult));
        }

        [TestMethod]
        public void TestFormatCsvHeaders()
        {
            var testInput = new string[]
            {
                "S. No:","Screen / Sub Tab","Field Name","Section","Functionality","Type","Functionality","Type","WE Mapping","Required ?","Comments"
            };
            var testResult = NoFuture.Util.Etc.FormatCsvHeaders(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach(var tr in testResult)
                System.Diagnostics.Debug.WriteLine(tr);
        }

        [TestMethod]
        public void TestReplaceOriginalContent()
        {
            var inputString = new String[]
            {
                "    public SomeCtor(string anArg)",
                "    {",
                "        try",
                "        { ",
                "            ddlSecurityQuestion.DataSource = dsGetSecurityQuestions;",
                "            ddlSecurityQuestion.DataTextField = \"ItemName\";",
                "            ddlSecurityQuestion.DataValueField = \"ItemAbbreviation\";",
                "            ddlSecurityQuestion.DataBind();",
                "            ddlSecurityQuestion.Items.Insert(0, string.Format(\" '{0}'\",\"------------------- Select a question -------------------\");",
                "        }",
                "        catch (Exception ex)",
                "        {",
                "            MyNamespace.CRM.Common.Utitility.CommonMethods.LogException(\"\",ex);",
                "            throw ex;",
                "        }",
                "    }",
                "",
                "    protected internal string SomeProperty {get; set;}",
                "",
                "    //comments here ",
                "    // about our shitty code",
                "",
                "    public string SomeMethod(int arg1, string arg2)",
                "    {",
                "        try",
                "        {",
                "            DataSet dsGetSecurityQuestions = objZuluUIController.GetZuluSecurityQuestions(DropdownConstants.ZuluSECURITYQUESTIONS);",
                "            ddlSecurityQuestion.DataSource = dsGetSecurityQuestions;",
                "            ddlSecurityQuestion.DataTextField = \"ItemName\";",
                "            ddlSecurityQuestion.DataValueField = \"ItemAbbreviation\";",
                "            ddlSecurityQuestion.DataBind();",
                "            ddlSecurityQuestion.Items.Insert(0, string.Format(\" '{0}'\",\"------------------- Select a question -------------------\");",
                "        }",
                "        catch (Exception ex)",
                "        {",
                "            MyNamespace.CRM.Common.Utitility.CommonMethods.LogException(\"\",ex);",
                "            throw ex;",
                "        }",
                "    }",
                ""
            };

            //foreach (var i in inputString)
            //{
            //    System.Diagnostics.Debug.WriteLine(i);
            //}

            var testInput00 = new Dictionary<Tuple<int, int>, string[]>();
            testInput00.Add(new Tuple<int, int>(2, 2), null);
            testInput00.Add(new Tuple<int, int>(9, 6), new[] { "new line B", "new line C", "new line B" });
            testInput00.Add(new Tuple<int, int>(26, 0), new[] { "new line D", "new line E" });
            var testResult = NoFuture.Util.Etc.ReplaceOriginalContent(testInput00, inputString);
            //foreach (var t in testResult)
            //    System.Diagnostics.Debug.WriteLine(t);

            testInput00 = new Dictionary<Tuple<int, int>, string[]>();
            testInput00.Add(new Tuple<int, int>(2, 0), new[] { "const MY_STRING00 = \"string\";", "const MY_STRING01 = \"string\";", "const MY_STRING02 = \"string\";" });
            testInput00.Add(new Tuple<int, int>(4, 1), new[] { "ddlSecurityQuestion.DataTextField = MY_STRING01;" });
            testInput00.Add(new Tuple<int, int>(5, 1), new[] { "ddlSecurityQuestion.DataValueField = MY_STRING00;" });
            testInput00.Add(new Tuple<int, int>(26, 0), new[] { "new line D", "new line E" });
            testResult = NoFuture.Util.Etc.ReplaceOriginalContent(testInput00, inputString);


            testInput00 = new Dictionary<Tuple<int, int>, string[]>();
            testInput00.Add(new Tuple<int, int>(2, 1), null);
            testInput00.Add(new Tuple<int, int>(4, 1), null);
            testInput00.Add(new Tuple<int, int>(5, 1), null);
            testInput00.Add(new Tuple<int, int>(6, 1), null);
            testInput00.Add(new Tuple<int, int>(7, 1), null);
            testInput00.Add(new Tuple<int, int>(8, 1), null);
            testInput00.Add(new Tuple<int, int>(9, 1), null);
            testInput00.Add(new Tuple<int, int>(10, 1), null);
            testInput00.Add(new Tuple<int, int>(11, 1), null);
            testResult = NoFuture.Util.Etc.ReplaceOriginalContent(testInput00, inputString);
            foreach (var t in testResult)
                System.Diagnostics.Debug.WriteLine(t);

        }

        [TestMethod]
        public void TestIsBetween()
        {
            
            Assert.IsTrue(new DateTime(2010, 1, 1).IsBetween(new DateTime(2009, 12, 31), new DateTime(2010, 1, 2)));

            Assert.IsFalse(new DateTime(2009, 12, 30).IsBetween(new DateTime(2009, 12, 31), new DateTime(2010, 1, 2)));

            Assert.IsTrue(new DateTime(2010, 1, 1).IsBetween(new DateTime(2009, 1, 1), new DateTime(2010, 1, 2)));
        }

        [TestMethod]
        public void TestJaroWinklerDistance()
        {
            var testResult = Etc.JaroWinklerDistance("test", "test");
            Assert.AreEqual(1D, Math.Round(testResult));
        }

        [TestMethod]
        public void TestLevenshteinDistance()
        {
            var testResult = Etc.LevenshteinDistance("kitten", "sitting");
            Assert.AreEqual(3D,testResult);
            testResult = Etc.LevenshteinDistance("Saturday", "Sunday");
            Assert.AreEqual(3D,testResult);
            testResult = Etc.LevenshteinDistance("Brian", "Brain");
            Assert.AreEqual(2D, testResult);

            testResult = Etc.LevenshteinDistance("sword", "swath", true);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

    }
}
