using System;
using System.Collections.Generic;
using NoFuture.Shared.Core;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class EtcTests
    {
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
    }
}
