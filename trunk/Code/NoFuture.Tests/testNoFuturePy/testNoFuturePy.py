# -*- coding=latin-1 -*-
import unittest
import importlib.util
spec = importlib.util.spec_from_file_location("nofuture.util.etc","C:/Projects/31g/trunk/Code/NoFuture/Util/util_py/etc.py")
toLoad = importlib.util.module_from_spec(spec)
spec.loader.exec_module(toLoad)


class TestNfUtilEtc(unittest.TestCase):
    def test_distillString(self):
        testInput = """
                    Some              here string           which spans           multiple lines
                    and       has too many      spaces."""
        testResult = toLoad.distillString(testInput)
        self.assertEqual(" Some here string which spans multiple lines and has too many spaces.", testResult)
        
        testResult = toLoad.distillString(None)
        self.assertIsNone(testResult)
    
    def test_distillTabs(self):
        testInput = "\t\t\t\tSome\t\t\tstring\there"
        testResult = toLoad.distillTabs(testInput)
        self.assertEqual(" Some string here",testResult)

    def test_convertToCrLf(self):
        testInput = "Some unix style line endings.\nAppear at the end of these lines.\n"
        testResult = toLoad.convertToCrLf(testInput)
        self.assertEqual("Some unix style line endings.\r\nAppear at the end of these lines.\r\n",testResult)

    def test_escapeString(self):
        testInput = "I am decimal"
        testResult = toLoad.escapeString(testInput, toLoad.EscapeStringType.DECIMAL)
        self.assertEqual("&#73;&#32;&#97;&#109;&#32;&#100;&#101;&#99;&#105;&#109;&#97;&#108;", testResult)

        testResult = toLoad.escapeString("[regex]", toLoad.EscapeStringType.REGEX)
        self.assertEqual("\\x5b\\x72\\x65\\x67\\x65\\x78\\x5d",testResult)

        testResult = toLoad.escapeString(" £¡¥§'", toLoad.EscapeStringType.HTML)
        self.assertEqual("&nbsp;&pound;&iexcl;&yen;&sect;&apos;",testResult)
        

if __name__ == '__main__':
    unittest.main()