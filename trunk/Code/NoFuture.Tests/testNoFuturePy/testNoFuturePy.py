# -*- coding=latin-1 -*-
import unittest
import Util.net as nfNet
import Shared.globals as nfGlobals
import Util.etc as toTest


class TestNfUtilEtc(unittest.TestCase):
    def test_distillString(self):
        testInput = """
                    Some              here string           which spans           multiple lines
                    and       has too many      spaces."""
        testResult = toTest.distillString(testInput)
        self.assertEqual(" Some here string which spans multiple lines and has too many spaces.", testResult)
        
        testResult = toTest.distillString(None)
        self.assertIsNone(testResult)
    
    def test_distillTabs(self):
        testInput = "\t\t\t\tSome\t\t\tstring\there"
        testResult = toTest.distillTabs(testInput)
        self.assertEqual(" Some string here",testResult)

    def test_convertToCrLf(self):
        testInput = "Some unix style line endings.\nAppear at the end of these lines.\n"
        testResult = toTest.convertToCrLf(testInput)
        self.assertEqual("Some unix style line endings.\r\nAppear at the end of these lines.\r\n",testResult)

    def test_escapeString(self):
        testInput = "I am decimal"
        testResult = toTest.escapeString(testInput, nfGlobals.EscapeStringType.DECIMAL)
        self.assertEqual("&#73;&#32;&#97;&#109;&#32;&#100;&#101;&#99;&#105;&#109;&#97;&#108;", testResult)

        testResult = toTest.escapeString("[regex]", nfGlobals.EscapeStringType.REGEX)
        self.assertEqual("\\x5b\\x72\\x65\\x67\\x65\\x78\\x5d",testResult)

        testResult = toTest.escapeString(" £¡¥§'", nfGlobals.EscapeStringType.HTML)
        self.assertEqual("&nbsp;&pound;&iexcl;&yen;&sect;&apos;",testResult)

    def test_toCamelCase(self):
        testResult = toTest.toCamelCase("UserName")
        self.assertEqual("userName",testResult)

        testResult = toTest.toCamelCase("__UserName")
        self.assertEqual("__userName",testResult)

        testResult = toTest.toCamelCase("__USERNAME")
        self.assertEqual("__username",testResult)

        testResult = toTest.toCamelCase("ID")
        self.assertEqual("id",testResult)

        testResult = toTest.toCamelCase("498375938720")
        self.assertEqual("498375938720",testResult)
        
        testResult = toTest.toCamelCase("__userNAME_ID")
        self.assertEqual("__userName_Id",testResult)

        testResult = toTest.toCamelCase("user.name")
        self.assertEqual("user.name",testResult)
        
    def test_transformCamelCaseToSeparator(self):
        testResult = toTest.transformCaseToSeparator("UserName")
        self.assertEqual("User_Name",testResult)

        testResult = toTest.transformCaseToSeparator("user_Name")
        self.assertEqual("user_Name",testResult)
    
    def test_transformScreamingCapsToCamelCase(self):
        testResult = toTest.toPascelCase("dbo.DELETED_LookupDetails")
        self.assertEqual("DboDeletedLookupDetails",testResult)

        testResult = toTest.toPascelCase("dbo.DELETED_LookupDetails", True)
        self.assertEqual("Dbo.Deleted_LookupDetails",testResult)

        testResult = toTest.toPascelCase("Test.dbo.SET_OP_lli", True)
        self.assertEqual("Test.dbo.Set_Op_lli",testResult)
        
    def test_distillToWholeWords(self):
        testResult = toTest.distillToWholeWords("FilmMaster-AccountDetail-ClientDetails-LocationDetails-TimeMasters-IsGolfVoucher")
        self.assertIsNotNone(testResult)
        self.assertEqual("Film Master Account Detail Client Details Location Time Masters Is Golf Voucher"," ".join(testResult))

        testResult = toTest.distillToWholeWords("Id")
        self.assertIsNotNone(testResult)
        self.assertTrue(len(testResult) == 1)
        self.assertEqual("Id",testResult[0])

        testResult = toTest.distillToWholeWords("RTDC IR Questions")
        self.assertIsNotNone(testResult)
        self.assertEqual("RtdcIrQuestions","".join(testResult))

    def test_calcLuhnCheckDigit(self):
        testResult = toTest.calcLuhnCheckDigit("455673758689985")
        self.assertEqual(5,testResult)

        testResult = toTest.calcLuhnCheckDigit("453211001754030")
        self.assertEqual(9,testResult)

        testResult = toTest.calcLuhnCheckDigit("471604448140316")
        self.assertEqual(5,testResult)

        testResult = toTest.calcLuhnCheckDigit("554210251648257")
        self.assertEqual(5,testResult)

        testResult = toTest.calcLuhnCheckDigit("537886423943754")
        self.assertEqual(6,testResult)

        testResult = toTest.calcLuhnCheckDigit("511329925461278")
        self.assertEqual(2,testResult)

        testResult = toTest.calcLuhnCheckDigit("37322049976972")
        self.assertEqual(0,testResult)

        testResult = toTest.calcLuhnCheckDigit("34561114407525")
        self.assertEqual(4,testResult)

        testResult = toTest.calcLuhnCheckDigit("34831152135173")
        self.assertEqual(6,testResult)

        testResult = toTest.calcLuhnCheckDigit("601198900163944")
        self.assertEqual(0,testResult)

        testResult = toTest.calcLuhnCheckDigit("3653092434341")
        self.assertEqual(5,testResult)


if __name__ == '__main__':
    unittest.main()