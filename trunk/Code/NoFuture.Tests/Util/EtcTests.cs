using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util;
using NoFuture.Util.Core;

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
            var testResult = Etc.DistillString(testInput);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestToOrdinal()
        {
            Assert.AreEqual("1st", 1.ToOrdinal());
            Assert.AreEqual("2nd", 2.ToOrdinal());
            Assert.AreEqual("3rd", 3.ToOrdinal());
            Assert.AreEqual("4th", 4.ToOrdinal());
            Assert.AreEqual("256th", 256.ToOrdinal());
            Assert.AreEqual("1,000th", 1000.ToOrdinal());
            
        }

        [TestMethod]
        public void TestCapitalizeFirstLetterOfWholeWords()
        {
            const string typicalTypeName = "noFuture.util.etc";
            var testResult = Etc.CapWords(typicalTypeName, '.');
            Assert.AreEqual("NoFuture.Util.Etc", testResult);

            const string allCaps = "KEYCODE";
            testResult = Etc.CapWords(allCaps, '.');
            Assert.AreEqual("Keycode", testResult);

            const string capsLockOn = "nOFUTURE.uTIL.eTC";
            testResult = Etc.CapWords(capsLockOn, '.');
            Assert.AreEqual("Nofuture.Util.Etc",testResult);

            testResult = Etc.CapWords("WINSTON SALEM".ToLower(), ' ');
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestEscapeString()
        {
            const string DEC_EXPECT = "&#73;&#32;&#97;&#109;&#32;&#100;&#101;&#99;&#105;&#109;&#97;&#108;";
            var dec = Etc.EscapeString("I am decimal", EscapeStringType.DECIMAL);

            const string UNI_EXPECT = @"\u00C0\u00C8\u00CC\u00D2\u00D9\u00E0\u00E8\u00EC\u00F2\u00F9\u00C1\u00C9\u00CD\u00D3\u00DA\u00DD\u00E1\u00E9\u00ED\u00F3\u00FA\u00FD\u00C2\u00CA\u00CE\u00D4\u00DB\u00E2\u00EA\u00EE\u00F4\u00FB\u00C3\u00D1\u00D5\u00E3\u00F1\u00F5\u00C4\u00CB\u00CF\u00D6\u00DC\u00E4\u00EB\u00EF\u00F6\u00FC\u00E7\u00C7\u00DF\u00D8\u00F8\u00C5\u00E5\u00C6\u00E6\u00DE\u00FE\u00D0\u00F0\u0152\u0153\u0178\u00FF\u0160\u0161";
            var uni = Etc.EscapeString("ÀÈÌÒÙàèìòùÁÉÍÓÚÝáéíóúýÂÊÎÔÛâêîôûÃÑÕãñõÄËÏÖÜäëïöüçÇßØøÅåÆæÞþÐðŒœŸÿŠš", EscapeStringType.UNICODE);

            const string REGEX_EXPECT = @"\x5b\x72\x65\x67\x65\x78\x5d";
            var regex = Etc.EscapeString("[regex]", EscapeStringType.REGEX);

            const string HTML_EXPECT = "&nbsp;&pound;&iexcl;&yen;&sect;";
            var html = Etc.EscapeString(" £¡¥§", EscapeStringType.HTML);

            const string URI_EXPECT = "F%40r0ut%7eDu%2cde%3d";
            var uri = Etc.EscapeString("F@r0ut~Du,de=", EscapeStringType.URI);

            Assert.AreEqual(DEC_EXPECT,dec);
            Assert.AreEqual(UNI_EXPECT,uni);
            Assert.AreEqual(REGEX_EXPECT,regex);
            Assert.AreEqual(HTML_EXPECT,html);
            Assert.AreEqual(URI_EXPECT, uri);

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
            const string TEST_INPUT = "USER_NAME";
            var testOutput = Etc.ToPascelCase(TEST_INPUT);
            Assert.AreEqual("userName", testOutput);
        }

        [TestMethod]
        public void TestToCamelCase()
        {
            const string TEST_INPUT = "UserName";
            var testOutput = Etc.ToCamelCase(TEST_INPUT, true);
            Assert.AreEqual("userName", testOutput);

            testOutput = Etc.ToCamelCase("__" + TEST_INPUT, true);
            Assert.AreEqual("__userName", testOutput);

            testOutput = Etc.ToCamelCase("__" + TEST_INPUT.ToUpper(), true);
            Assert.AreEqual("__username", testOutput);

            testOutput = Etc.ToCamelCase("ID", true);
            Assert.AreNotEqual("iD", testOutput);
            Assert.AreEqual("id", testOutput);

            testOutput = Etc.ToCamelCase("498375938720", true);
            Assert.AreEqual("498375938720", testOutput);

            testOutput = Etc.ToCamelCase("__userNAME_ID", true);
            Assert.AreEqual("__userName_Id", testOutput);

            testOutput = Etc.ToCamelCase("The-VariousThings\\which,AllowYou ToRead=this");
            Assert.AreEqual("theVariousThingsWhichAllowYouToReadThis", testOutput);

            testOutput = Etc.ToCamelCase("Server Purpose(s); Installed Component(s)");
            Assert.AreEqual("serverPurposeSInstalledComponentS", testOutput);

        }

        [TestMethod]
        public void TestTransformCamelCaseToSeparator()
        {
            const string TEST_INPUT = "UserName";
            var testOutput = Etc.TransformCaseToSeparator(TEST_INPUT, '_');
            Assert.AreEqual("User_Name", testOutput);

            testOutput = Etc.TransformCaseToSeparator("user_Name", '_');
            Assert.AreEqual("user_Name", testOutput);

        }
        [TestMethod]
        public void TestToPascelCase()
        {
            var testResult = Etc.ToPascelCase("dbo.DELETED_LookupDetails");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("DboDeletedLookupDetails",testResult);

            testResult = Etc.ToPascelCase("dbo.DELETED_LookupDetails", true);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("Dbo.Deleted_LookupDetails",testResult);

            testResult = Etc.ToPascelCase("Test.dbo.SET_OP_lli", true);
            Assert.AreEqual("Test.dbo.Set_Op_lli",testResult);
        }

        [TestMethod]
        public void TestDistillToWholeWords()
        {
            var testResult =
                Etc.DistillToWholeWords(
                    "FilmMaster-AccountDetail-ClientDetails-LocationDetails-TimeMasters-IsGolfVoucher");
            Assert.IsNotNull(testResult);
            
            Assert.AreEqual("Film Master Account Detail Client Details Location Time Masters Is Golf Voucher", string.Join(" ", testResult));

            testResult =
                Etc.DistillToWholeWords("Id");

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Length == 1);
            Assert.AreEqual("Id",testResult[0]);

            testResult = Etc.DistillToWholeWords("RTDC IR Questions");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("RtdcIrQuestions", string.Join("", testResult));

            testResult = Etc.DistillToWholeWords("The-VariousThings\\which,AllowYou ToRead=this");
            Assert.IsNotNull(testResult);
            Assert.AreEqual(9, testResult.Length);
            Assert.AreEqual("TheVariousThingsWhichAllowYouToReadThis", string.Join("",testResult));
        }

        [TestMethod]
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
                System.Diagnostics.Debug.WriteLine(tr);
        }

        [TestMethod]
        public void TestFormatJson()
        {
            var testJson =
                "[{\"ScheduleId\":5849,\"Uuid\":\"20012473\",\"ProgramId\":994564,\"Relation\":\"E\",\"FirstName\":\"Randall\",\"MiddleInitial\":null,\"LastName\":\"Perea's\",\"DateOfBirth\":\"1973-03-06T00:00:00\",\"Gender\":\"Male\",\"PhoneNumber\":\"2395730480\",\"Email\":null,\"AddressLine1\":\"3900 Cedar St\",\"AddressLine2\":null,\"City\":\"Arcadia\",\"State\":\"FL\",\"ZipCode\":\"33809\",\"ScheduledTime\":\"2013-10-30T07:30:00\"}]";
            var testResult = Etc.FormatJson(testJson);
            Assert.IsNotNull(testResult);

            testResult = Etc.FormatJson(testJson, true);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestFormatXml()
        {
            var testXml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><buildTypes><buildType id=\"Whiskey_Quebec_Motel_Romeo_Whiskey_Alpha\" name=\"API\"><builds><build id=\"9945412\" number=\"0000FF.0A.344D1.2B\" status=\"SUCCESS\"><statusText>Success</statusText></build></builds></buildType></buildTypes>";
            var testResult = Etc.FormatXml(testXml);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
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

            testResult = Etc.JaroWinklerDistance("kitty", "kitten");
            Assert.IsTrue(testResult - 0.893 < 0.001);

            testResult = Etc.JaroWinklerDistance("kitty", "kite");
            Assert.IsTrue(testResult - 0.848 < 0.001);
            System.Diagnostics.Debug.WriteLine(testResult);

            testResult = Etc.JaroWinklerDistance(null, null);
            Assert.AreEqual(1.0, testResult);
        }

        [TestMethod]
        public void TestLevenshteinDistance()
        {
            var testResult = Etc.LevenshteinDistance("kitten", "sitting");
            Assert.AreEqual(3D,testResult);
            testResult = Etc.LevenshteinDistance("Saturday", "Sunday");
            Assert.AreEqual(3D, testResult);
            testResult = Etc.LevenshteinDistance("Brian", "Brain");
            Assert.AreEqual(2D, testResult);

            System.Diagnostics.Debug.WriteLine(Etc.LevenshteinDistance("kitty", "kitten"));
            System.Diagnostics.Debug.WriteLine(Etc.LevenshteinDistance("kitty", "kite"));

            //testResult = Etc.LevenshteinDistance("sword", "swath", true);
            //System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestShortestDistance()
        {
            var testIn = "kitty";
            var testCompare = new[] {"kitten", "cat", "kite", "can", "kool"};

            var testResult = Etc.ShortestDistance(testIn, testCompare);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(2, testResult.Length);
            Assert.IsTrue(testResult.Contains("kitten"));
            Assert.IsTrue(testResult.Contains("kite"));

            testIn = "LeRoy";
            testCompare = new[] { "Lee", "Roy", "L.R." };
            testResult = Etc.ShortestDistance(testIn, testCompare);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(1,testResult.Length);
            Assert.AreEqual("Roy",testResult[0]);
            
        }
    }
}
