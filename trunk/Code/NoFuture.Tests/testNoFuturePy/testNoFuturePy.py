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
        
    def test_transformCamelCaseToSeparator(self):
        testResult = toTest.transformCamelCaseToSeparator("UserName")
        self.assertEqual("User_Name",testResult)

        testResult = toTest.transformCamelCaseToSeparator("user_Name")
        self.assertEqual("user_Name",testResult)


if __name__ == '__main__':
    unittest.main()