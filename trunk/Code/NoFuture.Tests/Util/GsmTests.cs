using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gsm;
using System.Text.RegularExpressions;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class GsmTests
    {
        [TestMethod]
        public void TestAtCmdRegexPattern()
        {
            var testInput00 = "+CPBR: 117,\"9137681400\",129,\"TractorSupply Co\"";
            var testInput01 = "AT+CPBR: 117,\"9137681400\",129,\"TractorSupply Co\"";
            var testInput02 = "AT+CPBR?";
            var testInput03 = "AT+CPBR=?";

            var testSubject00 = System.Text.RegularExpressions.Regex.Match(testInput00, string.Format("({0})", Utility.AT_CMD_REGEX_PATTERN));
            Assert.IsTrue(testSubject00.Success);
            System.Diagnostics.Debug.WriteLine(testSubject00.Groups[0].Value);

            var testSubject01 = System.Text.RegularExpressions.Regex.Match(testInput01, string.Format("({0})", Utility.AT_CMD_REGEX_PATTERN));
            Assert.IsTrue(testSubject01.Success);
            System.Diagnostics.Debug.WriteLine(testSubject01.Groups[0].Value);

            var testSubject02 = System.Text.RegularExpressions.Regex.Match(testInput02, string.Format("({0})", Utility.AT_CMD_REGEX_PATTERN));
            Assert.IsTrue(testSubject02.Success);
            System.Diagnostics.Debug.WriteLine(testSubject02.Groups[0].Value);

            var testSubject03 = System.Text.RegularExpressions.Regex.Match(testInput03, string.Format("({0})", Utility.AT_CMD_REGEX_PATTERN));
            Assert.IsTrue(testSubject03.Success);
            System.Diagnostics.Debug.WriteLine(testSubject03.Groups[0].Value);
        }

        [TestMethod]
        public void TestOkRspnRegexPattern()
        {
            var testInput00 = "+CPBS: \"AD\",129,1250\n\nOK";
            var testResult00 = Regex.IsMatch(testInput00, Utility.OK_RESPONSE_REGEX_PATTERN);
            Assert.IsTrue(testResult00);

            var testInput01 = string.Format("AT{0}", testInput00);
            var testResult01 = Regex.IsMatch(testInput01, Utility.OK_RESPONSE_REGEX_PATTERN);
            Assert.IsTrue(testResult01);

            var testInput02 = testInput01.ToLower();
            var testResult02 = Regex.IsMatch(testInput02, Utility.OK_RESPONSE_REGEX_PATTERN);
            Assert.IsFalse(testResult02);
        }

        [TestMethod]
        public void TestSmsSummaryTryParse()
        {
            var testInput = "+CMGL: 6,\"REC READ\",\"+13235557557\" Boss has some news she wants to tell you ";
            Gsm.SmsSummary testOutput = null;
            var testResult = Gsm.SmsSummary.TryParse(testInput, out testOutput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(6, testOutput.Index);
            Assert.AreEqual("REC READ", testOutput.Status);
            Assert.AreEqual("+13235557557",testOutput.Number);
            Assert.AreEqual(" Boss has some news she wants to tell you ", testOutput.Message);

            testOutput = null;
            testInput =
                "+CMGL: 5,\"REC READ\",\"+13235557557\" k";
            testResult = Gsm.SmsSummary.TryParse(testInput, out testOutput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(5,testOutput.Index);
            Assert.AreEqual("REC READ", testOutput.Status);
            Assert.AreEqual("+13235557557",testOutput.Number);
            Assert.AreEqual(" k", testOutput.Message);
        }

        [TestMethod]
        public void TestSmsSummaryTryParseToList()
        {
            var testInput = @"
AT+CMGL='REC READ'
+CMGL: 6,'REC READ','+13235557557'
Rioting! the unbeatable high 
+CMGL: 5,'REC READ','+13235557557'
adrenaline shoots your nerves to the sky
+CMGL: 4,'REC READ','+13238057557'
everyone knows this town is gonna blow 
+CMGL: 1,'REC READ','+13235557557'
and its all... gonna blow... right NOW!
+CMGL: 3,'REC READ','+13235557557'
now you can smash all the windows that you want
+CMGL: 2,'REC READ','+13235557557'
throwing a brick never felt so damn good
OK

";
            testInput = testInput.Replace("'", "\"");
            SmsSummary[] testOutput = null;
            var testResult = SmsSummary.TryParseList(testInput, out testOutput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(6,testOutput.Length);
            foreach(var sms in testOutput)
            {
                System.Diagnostics.Debug.WriteLine(sms.Message);
            }

        }

        [TestMethod]
        public void TestIsCmdOutputError()
        {
            var testInput00 = "AT+CFFF?\nERROR\n\n";
            var testInput01 = "+CPBS: \"AD\",129,1250\n\nOK";

            var testResult = Utility.IsCmdOutputError(null);
            Assert.IsTrue(testResult);
            testResult = Utility.IsCmdOutputError(string.Empty);
            Assert.IsTrue(testResult);
            testResult = Utility.IsCmdOutputError("  ");
            Assert.IsTrue(testResult);

            testResult = Utility.IsCmdOutputError(testInput00);
            Assert.IsTrue(testResult);

            testResult = Utility.IsCmdOutputError(testInput01);
            Assert.IsFalse(testResult);
        }

        [TestMethod]
        public void TestStripOffAtCommand()
        {
            var testInput00 = "+CPBR: 117,\"9137681400\",129,\"TractorSupply Co\"";
            var testInput01 = "AT+CPBR: 117,\"9137681400\",129,\"TractorSupply Co\"";

            var testControl = "117,\"9137681400\",129,\"TractorSupply Co\"";

            var testOutput00 = Gsm.Utility.StripOffAtCommand(testInput00);
            var testOutput01 = Gsm.Utility.StripOffAtCommand(testInput01);
            var testOutput02 = Gsm.Utility.StripOffAtCommand(testControl);

            Assert.AreEqual(testControl, testOutput00);
            Assert.AreEqual(testControl, testOutput01);
            Assert.AreEqual(testControl, testOutput02);
        }

        [TestMethod]
        public void TestPhoneBookTryParse()
        {
            var testInput = "\"AD\",129,1250";
            PhoneBook testOut;
            var testResult = PhoneBook.TryParse(testInput, out testOut);
            Assert.IsTrue(testResult);

            Assert.IsNotNull(testOut);
            Assert.AreEqual("AD",testOut.Storage);
            Assert.AreEqual(129,testOut.BlocksUsed);
            Assert.AreEqual(1250, testOut.TotalBlocks);

            var testInputRaw = new byte[]
                                   {
                                       (byte) 0x41, (byte) 0x54, (byte) 0x2B, (byte) 0x43, (byte) 0x50,
                                       (byte) 0x42, (byte) 0x53, (byte) 0x3F, (byte) 0x0D,
                                       (byte) 0x0D, (byte) 0x0A, (byte) 0x2B, (byte) 0x43,
                                       (byte) 0x50, (byte) 0x42, (byte) 0x53, (byte) 0x3A,
                                       (byte) 0x20, (byte) 0x22, (byte) 0x41, (byte) 0x44,
                                       (byte) 0x22, (byte) 0x2C, (byte) 0x31, (byte) 0x32,
                                       (byte) 0x39, (byte) 0x2C, (byte) 0x31, (byte) 0x32,
                                       (byte) 0x35, (byte) 0x30, (byte) 0x0D, (byte) 0x0A,
                                       (byte) 0x0D, (byte) 0x0A, (byte) 0x4F, (byte) 0x4B,
                                       (byte) 0x0D, (byte) 0x0A
                                   };
            testInput = System.Text.Encoding.UTF8.GetString(testInputRaw);
            testOut = null;
            testResult = PhoneBook.TryParse(testInput, out testOut);
            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void TestPhoneTypeToString()
        {
            var testSubject = new PhoneBookEntryType(129);
            var testResult = testSubject.ToString();
            Assert.AreEqual("129",testResult);
        }

        [TestMethod]
        public void TestPhoneBookEntryToStringFull()
        {
            var testControl = "120,\"18008626190\",129,\"UMB MortgageServices\",\"Banking\",\"18004445555\",128,\"Direct Line\",\"jim.bo@banking.com\",\"sip:+1-212-555-1212:1234@gateway.com;user=phone\",\"tel:+1-212-555-1212\",0";
            var testSubject = new PhoneBookEntry
                                  {
                                      Index = 120,
                                      Number = "18008626190",
                                      NumberType = new PhoneBookEntryType(129),
                                      Text = "UMB MortgageServices",
                                      Hidden = false,
                                      Group = "Banking",
                                      AdditionalNumber = "18004445555",
                                      AdditionalNumberType = new PhoneBookEntryType(128),
                                      SecondText = "Direct Line",
                                      Email = "jim.bo@banking.com",
                                      SipUri = "sip:+1-212-555-1212:1234@gateway.com;user=phone",
                                      TelUri = "tel:+1-212-555-1212"
                                  };

            var testResult = testSubject.ToString();
            Assert.AreEqual(testControl, testResult);
        }

        [TestMethod]
        public void TestPhoneBookEntryMin()
        {
            var testControl = "120,\"18008626190\",129,\"UMB MortgageServices\"";
            var testSubject = new PhoneBookEntry
                                  {
                                      Index = 120,
                                      Number = "18008626190",
                                      NumberType = new PhoneBookEntryType(129),
                                      Text = "UMB MortgageServices"
                                  };
            var testResult = testSubject.ToString();
            Assert.AreEqual(testControl,testResult);
        }

        [TestMethod]
        public void TestPhoneBookEntryParseOwnToString()
        {

            var testInput = new PhoneBookEntry
            {
                Index = 120,
                Number = "18008626190",
                NumberType = new PhoneBookEntryType(129),
                Text = "UMB MortgageServices",
                Hidden = false,
                Group = "Banking",
                AdditionalNumber = "18004445555",
                AdditionalNumberType = new PhoneBookEntryType(128),
                SecondText = "Direct Line",
                Email = "jim.bo@banking.com",
                SipUri = "sip:+1-212-555-1212:1234@gateway.com;user=phone",
                TelUri = "tel:+1-212-555-1212"
            };

            //get PhoneBookEntry as string, Hidden will appear at the end
            var testSubject = testInput.ToString();
            PhoneBookEntry testOut;
            var testResult = PhoneBookEntry.TryParse(testSubject, out testOut);
            Assert.IsTrue(testResult);

            Assert.AreEqual(testInput.Index, testOut.Index);
            Assert.AreEqual(testInput.Number, testOut.Number);
            Assert.AreEqual(testInput.NumberType.ToString(), testOut.NumberType.ToString());
            Assert.AreEqual(testInput.Text, testOut.Text);
            Assert.AreEqual(testInput.Hidden, testOut.Hidden);
            Assert.AreEqual(testInput.Group, testOut.Group);
            Assert.AreEqual(testInput.AdditionalNumber, testOut.AdditionalNumber);
            Assert.AreEqual(testInput.AdditionalNumberType.ToString(), testOut.AdditionalNumberType.ToString());
            Assert.AreEqual(testInput.SecondText, testOut.SecondText);
            Assert.AreEqual(testInput.SipUri, testOut.SipUri);
            Assert.AreEqual(testInput.TelUri, testOut.TelUri);

        }

        [TestMethod]
        public void TestPhoneBookEntryTryParse()
        {
            var testInputFull = "+CPBR: 120,\"18008626190\",129,\"UMB MortgageServices\",0,\"Banking\",\"18004445555\",128,\"Direct Line\",\"jim.bo@banking.com\",\"sip:+1-212-555-1212:1234@gateway.com;user=phone\",\"tel:+1-212-555-1212\"";
            var testInputRequired = "+CPBR: 117,\"9137681400\",129,\"TractorSupply Co\"";

            PhoneBookEntry testOutFull;
            var testResultFull = PhoneBookEntry.TryParse(testInputFull, out testOutFull);

            PhoneBookEntry testOutReq;
            var testResultReq = PhoneBookEntry.TryParse(testInputRequired, out testOutReq);

            Assert.IsTrue(testResultFull);
            Assert.IsTrue(testResultReq);

            Assert.AreEqual(120,testOutFull.Index);
            Assert.AreEqual(117,testOutReq.Index);

            Assert.AreEqual("18008626190", testOutFull.Number);
            Assert.AreEqual("9137681400", testOutReq.Number);

            Assert.AreEqual(TypeOfNumber.UNKNOWN, testOutFull.NumberType.TypeOfNumber);
            Assert.AreEqual(TypeOfNumber.UNKNOWN, testOutReq.NumberType.TypeOfNumber);

            Assert.AreEqual(NumberingPlanId.ISDN_TELEPHONY_NUMBERING_PLAN, testOutFull.NumberType.NumberingPlanId);
            Assert.AreEqual(NumberingPlanId.ISDN_TELEPHONY_NUMBERING_PLAN, testOutReq.NumberType.NumberingPlanId);

            Assert.AreEqual("UMB MortgageServices", testOutFull.Text);
            Assert.AreEqual("TractorSupply Co", testOutReq.Text);

            Assert.IsNotNull(testOutFull.Hidden);
            Assert.IsFalse(testOutFull.Hidden.Value);
            Assert.AreEqual("Banking", testOutFull.Group);
            Assert.AreEqual("18004445555", testOutFull.AdditionalNumber);
            Assert.AreEqual(TypeOfNumber.UNKNOWN, testOutFull.AdditionalNumberType.TypeOfNumber);
            Assert.AreEqual(NumberingPlanId.UNKNOWN, testOutFull.AdditionalNumberType.NumberingPlanId);
            Assert.AreEqual("Direct Line", testOutFull.SecondText);
            Assert.AreEqual("jim.bo@banking.com", testOutFull.Email);
            Assert.AreEqual("sip:+1-212-555-1212:1234@gateway.com;user=phone", testOutFull.SipUri);
            Assert.AreEqual("tel:+1-212-555-1212", testOutFull.TelUri);

        }

        [TestMethod]
        public void TestPhoneBookentryTryParsePartial()
        {
            var testInput00 = "+CPBR: 120,\"18008626190\",129,\"UMB MortgageServices\",0";
            var testInput01 = "+CPBR: 120,\"18008626190\",129,\"UMB MortgageServices\",0,\"Banking\"";
            var testInput02 = "+CPBR: 120,\"18008626190\",129,\"UMB MortgageServices\",0,\"Banking\",\"18004445555\"";
            var testInput03 = "+CPBR: 120,\"18008626190\",129,\"UMB MortgageServices\",0,\"Banking\",\"18004445555\",128";
            var testInput04 = "+CPBR: 120,\"18008626190\",129,\"UMB MortgageServices\",0,\"Banking\",\"18004445555\",128,\"Direct Line\"";
            var testInput05 = "+CPBR: 120,\"18008626190\",129,\"UMB MortgageServices\",0,\"Banking\",\"18004445555\",128,\"Direct Line\",\"jim.bo@banking.com\"";
            var testInput06 = "+CPBR: 120,\"18008626190\",129,\"UMB MortgageServices\",0,\"Banking\",\"18004445555\",128,\"Direct Line\",\"jim.bo@banking.com\",\"sip:+1-212-555-1212:1234@gateway.com;user=phone\"";

            PhoneBookEntry testOut;
            var testResult = PhoneBookEntry.TryParse(testInput00, out testOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOut.Hidden);
            Assert.IsFalse(testOut.Hidden.Value);
            Assert.IsNull(testOut.Group);
            Assert.IsNull(testOut.AdditionalNumber);
            Assert.IsNull(testOut.AdditionalNumberType);
            Assert.IsNull(testOut.SecondText);
            Assert.IsNull(testOut.Email);
            Assert.IsNull(testOut.SipUri);
            Assert.IsNull(testOut.TelUri);

            testOut = null;
            testResult = PhoneBookEntry.TryParse(testInput01, out testOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOut.Hidden);
            Assert.IsFalse(testOut.Hidden.Value);
            Assert.AreEqual("Banking", testOut.Group);
            Assert.IsNull(testOut.AdditionalNumber);
            Assert.IsNull(testOut.AdditionalNumberType);
            Assert.IsNull(testOut.SecondText);
            Assert.IsNull(testOut.Email);
            Assert.IsNull(testOut.SipUri);
            Assert.IsNull(testOut.TelUri);

            testOut = null;
            testResult = PhoneBookEntry.TryParse(testInput02, out testOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOut.Hidden);
            Assert.IsFalse(testOut.Hidden.Value);
            Assert.AreEqual("Banking", testOut.Group);
            Assert.AreEqual("18004445555", testOut.AdditionalNumber);
            Assert.IsNull(testOut.AdditionalNumberType);
            Assert.IsNull(testOut.SecondText);
            Assert.IsNull(testOut.Email);
            Assert.IsNull(testOut.SipUri);
            Assert.IsNull(testOut.TelUri);

            testOut = null;
            testResult = PhoneBookEntry.TryParse(testInput03, out testOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOut.Hidden);
            Assert.IsFalse(testOut.Hidden.Value);
            Assert.AreEqual("Banking", testOut.Group);
            Assert.AreEqual("18004445555", testOut.AdditionalNumber);
            Assert.AreEqual(TypeOfNumber.UNKNOWN, testOut.AdditionalNumberType.TypeOfNumber);
            Assert.AreEqual(NumberingPlanId.UNKNOWN, testOut.AdditionalNumberType.NumberingPlanId);
            Assert.IsNull(testOut.SecondText);
            Assert.IsNull(testOut.Email);
            Assert.IsNull(testOut.SipUri);
            Assert.IsNull(testOut.TelUri);

            testOut = null;
            testResult = PhoneBookEntry.TryParse(testInput04, out testOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOut.Hidden);
            Assert.IsFalse(testOut.Hidden.Value);
            Assert.AreEqual("Banking", testOut.Group);
            Assert.AreEqual("18004445555", testOut.AdditionalNumber);
            Assert.AreEqual(TypeOfNumber.UNKNOWN, testOut.AdditionalNumberType.TypeOfNumber);
            Assert.AreEqual(NumberingPlanId.UNKNOWN, testOut.AdditionalNumberType.NumberingPlanId);
            Assert.AreEqual("Direct Line", testOut.SecondText);
            Assert.IsNull(testOut.Email);
            Assert.IsNull(testOut.SipUri);
            Assert.IsNull(testOut.TelUri);

            testOut = null;
            testResult = PhoneBookEntry.TryParse(testInput05, out testOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOut.Hidden);
            Assert.IsFalse(testOut.Hidden.Value);
            Assert.AreEqual("Banking", testOut.Group);
            Assert.AreEqual("18004445555", testOut.AdditionalNumber);
            Assert.AreEqual(TypeOfNumber.UNKNOWN, testOut.AdditionalNumberType.TypeOfNumber);
            Assert.AreEqual(NumberingPlanId.UNKNOWN, testOut.AdditionalNumberType.NumberingPlanId);
            Assert.AreEqual("Direct Line", testOut.SecondText);
            Assert.AreEqual("jim.bo@banking.com", testOut.Email);
            Assert.IsNull(testOut.SipUri);
            Assert.IsNull(testOut.TelUri);

            testOut = null;
            testResult = PhoneBookEntry.TryParse(testInput06, out testOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOut.Hidden);
            Assert.IsFalse(testOut.Hidden.Value);
            Assert.AreEqual("Banking", testOut.Group);
            Assert.AreEqual("18004445555", testOut.AdditionalNumber);
            Assert.AreEqual(TypeOfNumber.UNKNOWN, testOut.AdditionalNumberType.TypeOfNumber);
            Assert.AreEqual(NumberingPlanId.UNKNOWN, testOut.AdditionalNumberType.NumberingPlanId);
            Assert.AreEqual("Direct Line", testOut.SecondText);
            Assert.AreEqual("jim.bo@banking.com", testOut.Email);
            Assert.AreEqual("sip:+1-212-555-1212:1234@gateway.com;user=phone", testOut.SipUri);
            Assert.IsNull(testOut.TelUri);
        }
    }
}
