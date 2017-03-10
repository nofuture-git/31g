import unittest
import Shared.nfConfig as toTest
import xml.etree.ElementTree as ET

class TestNfConfig(unittest.TestCase):

    testDict = {
	    "myPsHome" : "C:\\Projects\\31g\\trunk" ,
	    "tempJsFile" : "$(tempRootDir)\\temp.js",
	    "tempHtmlFile" : "$(tempRootDir)\\temp.html",
	    "tempCsvFile" : "$(tempRootDir)\\temp.csv",
	    "tempStdOutFile" : "$(tempRootDir)\\stdout.txt",
	    "tempT4TemplateFile" : "$(tempRootDir)\\t4Temp.tt",
	    "tempNetStatFile" : "$(tempRootDir)\\netstat.txt",
	    "tempWmiFile" : "$(tempTextDir)\\wmi.txt",
	    "x64SvcUtilTool" : "$(net461SdkTools)\\x64\\svcUtil.exe",
	    "x64IldasmTool" : "$(net461SdkTools)\\x64\\ildasm.exe",
	    "x64WsdlTool" : "$(net461SdkTools)\\x64\\wsdl.exe",
	    "x64ClrVerTool" : "$(net461SdkTools)\\x64\\clrver.exe",
	    "x64XsdExeTool" : "$(net461SdkTools)\\x64\\xsd.exe",
	    "x64CdbTool" : "$(binX64RootDir)\\debug\\cdb.exe",
	    "x64TListTool" : "$(binX64RootDir)\\debug\\tlist.exe",
	    "x64SymChkTool" : "$(binX64RootDir)\\debug\\symChk.exe",
	    "x64DumpbinTool" : "$(binX64RootDir)\\vs10\\dumpbin.exe",
	    "x64SqlCmdTool" : "$(binX64RootDir)\\sqlcmd\\SQLCMD.EXE",
	    "x64MdbgTool" : "$(binX64RootDir)\\v4.0\\Mdbg.exe",
	    "x86IldasmTool" : "$(net461SdkTools)\\ildasm.exe",
	    "x86SqlMetalTool" : "$(net461SdkTools)\\sqlmetal.exe",
	    "x86SvcUtilTool" : "$(net461SdkTools)\\svcUtil.exe",
	    "x86WsdlTool" : "$(net461SdkTools)\\wsdl.exe",
	    "x86CdbTool" : "$(binX86RootDir)\\debug\\cdb.exe",
	    "x86DependsTool" : "$(binX86RootDir)\\depends\\depends.exe",
	    "x86DumpbinTool" : "$(binX86RootDir)\\vs10\\dumpbin.exe",
	    "x86TextTransformTool" : "$(binX86RootDir)\\TextTransform.exe",
	    "x86DotExeTool" : "$(binRootDir)\\graphviz-2.38\\bin\\dot.exe",
	    "javaJavacTool" : "$(binJavaRootDir)\\bin\\javac.exe",
	    "javaJavaTool" : "$(binJavaRootDir)\\bin\\java.exe",
	    "javaJavaDocTool" : "$(binJavaRootDir)\\bin\\javadoc.exe",
	    "javaJavaRtJarTool" : "$(binJavaRootDir)\\jre\\lib\\rt.jar",
	    "javaJarTool" : "$(binJavaRootDir)\\bin\\jar.exe",
	    "javaJRunScriptTool" : "$(binJavaRootDir)\\bin\\jrunscript.exe",
	    "javaAntlrTool" : "$(binRootDir)\\antlr-4.1-complete.jar",
	    "javaStanfordPostTaggerTool" : "$(binRootDir)\\stanford-postagger-2015-12-09\\stanford-postagger.jar",
	    "javaStanfordPostTaggerModelsTool" : "$(binRootDir)\\stanford-postagger-2015-12-09\\models\\",
	    "customDia2DumpTool" : "$(binRootDir)\\Dia2Dump.exe",
	    "customInvokeGetCgTypeTool" : "$(binRootDir)\\NoFuture.Gen.InvokeGetCgOfType.exe",
	    "customInvokeGraphVizTool" : "$(binRootDir)\\NoFuture.Gen.InvokeGraphViz.exe",
	    "customInvokeAssemblyAnalysisTool" : "$(binRootDir)\\NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe",
	    "customInvokeFlattenTool" : "$(binRootDir)\\NoFuture.Util.Gia.InvokeFlatten.exe",
	    "customUtilPosHostTool" : "$(binRootDir)\\NoFuture.Util.Pos.Host.exe",
	    "customInvokeDpxTool" : "$(binRootDir)\\NoFuture.Util.Binary.InvokeDpx.exe",
	    "customInvokeNfTypeNameTool" : "$(binRootDir)\\NoFuture.Tokens.InvokeNfTypeName.exe",
	    "binFfmpegTool" : "$(binRootDir)\\ffmpeg.exe",
	    "binYoutubeDlTool" : "$(binRootDir)\\youtube-dl.exe",
	    "tempRootDir" : "$(myPsHome)\\temp",
	    "tempProcsDir" : "$(tempRootDir)\\procs",
	    "tempCodeDir" : "$(tempRootDir)\\code",
	    "tempTextDir" : "$(tempRootDir)\\text",
	    "tempDebugsDir" : "$(tempRootDir)\\debug",
	    "tempGraphDir" : "$(tempRootDir)\\graph",
	    "tempSvcUtilDir" : "$(tempRootDir)\\svcUtil",
	    "tempWsdlDir" : "$(tempRootDir)\\wsdl",
	    "tempHbmDir" : "$(tempRootDir)\\hbm",
	    "tempBinDir" : "$(tempRootDir)\\tempBin",
	    "tempJavaSrcDir" : "$(tempRootDir)\\java\\src",
	    "tempJavaBuildDir" : "$(tempRootDir)\\java\\build",
	    "tempJavaDistDir" : "$(tempRootDir)\\java\\dist",
	    "tempJavaArchiveDir" : "$(tempRootDir)\\java\\archive",
	    "tempCalendarDir" : "$(tempRootDir)\\Calendar",
	    "tempHttpAppDomainDir" : "$(tempRootDir)\\httpAppDomain",
	    "tempTsvCsvDir" : "$(tempRootDir)\\tsvCsv",
	    "binRootDir" : "$(myPsHome)\\bin",
	    "net461SdkTools" : "$(%ProgramFiles(x86)%)\\Microsoft SDKs\\Windows\\v10.0A\\bin\\NETFX 4.6.1 Tools",
	    "binX64RootDir" : "$(binRootDir)\\amd64",
	    "binX86RootDir" : "$(binRootDir)\\x86",
	    "binJavaRootDir" : "$(binRootDir)\\java",
	    "binT4TemplatesDir" : "$(binRootDir)\\java",
	    "binPhpRootDir" : "$(binRootDir)\\php",
	    "binDataRootDir" : "$(binRootDir)\\Data\\Source",
	    "portNsLookupPort" : "799",
	    "portDomainEngine" : "1138",
	    "portHostProc" : "780",
	    "portAssemblyAnalysis" : "5059",
	    "portFlattenAssembly" : "5062",
	    "portPartOfSpeechPaserHost" : "5063",
	    "portSjclToPlainText" : "5064",
	    "portSjclToCipherText" : "5065",
	    "portSjclHashPort" : "5066",
	    "portNfTypeNamePort" : "5067",
	    "portHbmInvokeStoredProcMgr" : "45121",
	    "switchPrintWebHeaders" : "True",
	    "switchSqlCmdHeadersOff" : "True",
	    "switchSqlFiltersOff" : "True",
	    "switchSupressNpp" : "False",
	    "keyAesEncryptionKey" : "gb0352wHVco94Gr260BpJzH+N1yrwmt5/BaVhXmPm6s=",
	    "keyAesIV" : "az9HzsMj6pygMvZyTpRo6g==",
	    "keyHMACSHA1" : "eTcmPilTLmtbalRpKjFFJjpMNns=",
	    "code-file-extensions" : "ada adb adml admx ads as asa asax ascx asm asmx asp aspx bas bat bsh c cbd cbl cc cdb cdc cls cmd cob cpp cs cshtml css cxx dsr dtd f f2k f90 f95 for frm fs fsx fxh g4 gv h hpp hs hta htm html hxx idl inc java js json kml las lhs lisp lsp lua m master ml mli mx nt pas php php3 phtml pl plx pm  ps ps1 psm1 py pyw r rb rbw rpx scm scp sep sh shtm shtml smd sml sql ss st tcl tex thy tt vb vbhtml vbs vrg wsc wsdl wsf xaml xhtml xlf xliff xsd xsl xslt xsml xst xsx",
	    "config-file-extensions" : "acl aml cfg config content csproj cva database din dsw dtproj dtsx dun ecf ecf edmx fcc fsproj gpd h1k hkf inf ini isp iss manifest nuspec obe overrideTasks pdm pp props ps1xml psc1 psd1 rsp sam sch sitemap sln stp svc targets tasks testsettings theme user vbp vbproj vdproj vsmdi xbap xml xrm-ms",
	    "binary-file-extensions" : "001 002 003 004 005 006 007 008 009 010 7z aas acm addin adm am amx ani apk apl aps aux avi ax bak bcf bcm bin bmp bpd bpl browser bsc btr bud cab cache camp cap cat cch ccu cdf-ms cdmp chk chm chr chs cht clb cmb cmdline cmf com comments compiled compositefont cov cpa cpl cpx crmlog crt csd ctm cty cur cw dat data db dcf dcr default delete dem desklink devicemetadata-ms diagpkg dic dir dll dlm dls dmp dnl doc docx drv ds dts dub dvd dvr-ms dxt ebd edb efi emf err ess etl ev1 ev2 ev3 evm evtx ex_ exe exp fe fon ftl fx gdl gif gmmp grl grm gs_4_0 h1c h1s h1t hdr hex hit hlp hpi hpx iad icc icm ico id idb idx iec if2 ilk imd ime inf_loc ins inx ipa ird jar jmx jnt job jpeg jpg jpn jrs jtp kor ldo lex lg1 lg2 lib library-ms lng lnk log lrc lts lxa mac man map mapimail mdb mdbx mfl mib mid mllr mni mof mp3 mp4 mpg msc msi msm msstyles mst msu mui mum mzz ncb ndx ngr nlp nls nlt ntf nupkg obj ocx olb old opt out pch pdb pdf phn pkc plugin pnf png ppd pptx prm prof propdesc prq prx ps_2_0 ps_4_0 psd ptxml pub que rat rdl reg res resources resx rld rll rom rpo rs rtf s3 sbr scc scr sdb sdi ses shp smp sqm ssm stl swf sys t4 tag tar.gz tbl tbr tha tif tiff tlb tmp toc tpi trie1 tsp ttc ttf tts tx_ txt uaq uce udt uni uninstall unt url vch vdf ver vp vs_1_1 vs_4_0 vsd vspscc wav wdf web wih wim win32manifest wma wmf wmv wmz wtv wwd x32 xex xlb xls xlsx zfsendtotarget zip",
	    "search-directory-exclusions" : "bin obj Interop TestResults _svn .svn _ReSharper _TeamCity .git .nuget .vs lib build dist packages __pycache__",
	    "default-block-size" : "256",
	    "default-type-separator" : ".",
	    "default-char-separator" : ",",
	    "cmd-line-arg-switch" : "-",
	    "cmd-line-arg-assign" : "=",
	    "punctuation-chars" : "! # $ % & ( ) * + , - . / : ; < = > ? @ [ ] ^ _ ` { | } ~"
    }
    
    def test_findNfConfigFile(self):
        testResult = toTest.findNfConfigFile()
        self.assertIsNotNone(testResult)
        print(testResult)

    def test_getIdValueHash(self):
        testXml = ET.parse("C:\\Projects\\31g\\trunk\\Code\\NoFuture.Tests\\testNoFuturePy\\shared_py\\nfConfig.cfg.xml")
        testResult = toTest._getIdValueHash(testXml)
        self.assertIsNotNone(testResult)
        self.assertNotEqual(0, len(testResult))

    def test_expandCfgValue(self):
        testResult = toTest._expandCfgValue(self.testDict, "$(tempRootDir)\\httpAppDomain")
        self.assertIsNotNone(testResult)
        #print(testResult)

    def test_resolveIdValueHash(self):
        testInput = self.testDict
        toTest._resolveIdValueHash(testInput)
        self.assertIsNotNone(testInput)
        self.assertNotEqual(0, len(testInput))
        #for k in testInput:
        #    print(k + " = " + testInput[k])

    def test_nfConfigInit(self):
        toTest.init("C:\\Projects\\31g\\trunk\\Code\\NoFuture.Tests\\testNoFuturePy\\shared_py\\nfConfig.cfg.xml")
        self.assertEqual("C:\\Projects\\31g\\trunk\\temp", toTest.TempDirectories.root)
        self.assertNotEqual(0, len(toTest.punctuationChars))
        self.assertEqual("C:\\Projects\\31g\\trunk\\favicon.ico",toTest.favicon)
        self.assertEqual(306,len(toTest.binaryFileExtensions))
        self.assertEqual(116,len(toTest.codeFileExtensions))
        self.assertEqual(54,len(toTest.configFileExtensions))
        self.assertEqual(16,len(toTest.excludeCodeDirectories))
        self.assertEqual("C:\\Projects\\31g\\trunk\\NoFuture.cer",toTest.SecurityKeys.noFutureX509Cert)
        

if __name__ == '__main__':
    unittest.main()