using System.Xml;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared.Core;
using static NoFuture.Shared.Cfg.NfConfig;

namespace NoFuture.Tests.Shared
{
    [TestClass]
    public class TestNfConfig
    {
        [TestMethod]
        public void TestGetIdValueHash()
        {
            var cfgXml = new XmlDocument();
            cfgXml.LoadXml(TEST_FILE);
            var idValueHash = GetIdValueHash(cfgXml);

            Assert.IsNotNull(idValueHash);
            Assert.AreNotEqual(0, idValueHash.Count);
            foreach(var k in idValueHash.Keys)
                System.Diagnostics.Debug.WriteLine("{" + $"\"{k}\", @\"{idValueHash[k]}\"" + "}," );
        }

        [TestMethod]
        public void TestInit()
        {
            Init(TEST_FILE);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin", BinDirectories.Root);
            var puncChars = @"! # $ % & \ ' ( ) * + , - . / : ; < = > ? @ [ ] ^ _ ` { | } ~";
            Assert.AreEqual(puncChars, string.Join(" ", NfSettings.PunctuationChars));

            Assert.AreEqual(@"C:\Projects\31g\trunk\temp", TempDirectories.Root);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\sql", TempDirectories.Sql);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\prox", TempDirectories.StoredProx);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\code", TempDirectories.Code);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\text", TempDirectories.Text);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\debug", TempDirectories.Debug);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\graph", TempDirectories.Graph);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\code\svcUtil", TempDirectories.SvcUtil);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\code\wsdl", TempDirectories.Wsdl);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\code\hbm", TempDirectories.Hbm);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\code\java\src", TempDirectories.JavaSrc);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\code\java\build", TempDirectories.JavaBuild);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\code\java\dist", TempDirectories.JavaDist);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\code\java\archive", TempDirectories.JavaArchive);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\Calendar", TempDirectories.Calendar);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\httpAppDomain", TempDirectories.HttpAppDomain);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\audio", TempDirectories.Audio);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\tsvCsv", TempDirectories.TsvCsv);

            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\t4Temp.tt", TempFiles.T4Template);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\netstat.txt", TempFiles.NetStat);
            Assert.AreEqual(@"C:\Projects\31g\trunk\temp\text\wmi.txt", TempFiles.Wmi);

            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\amd64", BinDirectories.X64Root);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\x86", BinDirectories.X86Root);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\java", BinDirectories.JavaRoot);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\Templates", BinDirectories.T4Templates);

            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\svcUtil.exe", X64.SvcUtil);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\amd64\debug\tlist.exe", X64.TList);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\amd64\debug\symChk.exe", X64.SymChk);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\amd64\depends\depends.exe", X64.Depends);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\amd64\vs10\dumpbin.exe", X64.Dumpbin);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\amd64\sqlcmd\SQLCMD.EXE", X64.SqlCmd);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\wsdl.exe", X64.Wsdl);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\clrver.exe", X64.ClrVer);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\xsd.exe", X64.XsdExe);

            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\x86\depends\depends.exe", X86.Depends);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\x86\vs10\dumpbin.exe", X86.Dumpbin);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\sqlmetal.exe", X86.SqlMetal);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\svcUtil.exe", X86.SvcUtil);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\x86\TextTransform.exe", X86.TextTransform);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\wsdl.exe", X86.Wsdl);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\graphviz-2.38\bin\dot.exe", X86.DotExe);

            Assert.AreEqual(799, NfDefaultPorts.NsLookupPort);

            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\java\bin\javac.exe", JavaTools.Javac);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\java\bin\java.exe", JavaTools.Java);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\java\bin\javadoc.exe", JavaTools.JavaDoc);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\java\jre\lib\rt.jar", JavaTools.JavaRtJar);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\java\bin\jar.exe", JavaTools.Jar);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\java\bin\jrunscript.exe", JavaTools.JRunScript);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\antlr-4.1-complete.jar", JavaTools.Antlr);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\stanford-postagger-2015-12-09\stanford-postagger.jar", JavaTools.StanfordPostTagger);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\stanford-postagger-2015-12-09\models\", JavaTools.StanfordPostTaggerModels);

            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\Dia2Dump.exe", CustomTools.Dia2Dump);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\NoFuture.Gen.InvokeGetCgOfType.exe", CustomTools.InvokeGetCgType);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\NoFuture.Gen.InvokeGraphViz.exe", CustomTools.InvokeGraphViz);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe", CustomTools.InvokeAssemblyAnalysis);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\NoFuture.Util.Gia.InvokeFlatten.exe", CustomTools.InvokeFlatten);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\NoFuture.Util.Pos.Host.exe", CustomTools.UtilPosHost);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\NoFuture.Util.Binary.InvokeDpx.exe", CustomTools.InvokeDpx);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\NoFuture.Tokens.InvokeNfTypeName.exe", CustomTools.InvokeNfTypeName);

            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\ffmpeg.exe", BinTools.Ffmpeg);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin\youtube-dl.exe", BinTools.YoutubeDl);

            Assert.IsTrue(Switches.SqlFiltersOff);
            Assert.IsTrue(Switches.PrintWebHeaders);

            Assert.AreEqual(@"C:\Projects\31g\trunk\NoFuture.cer", SecurityKeys.NoFutureX509Cert);

            var codeFiles =
                "ada adb adml admx ads as asa asax ascx asm asmx asp aspx bas bat bsh c cbd cbl cc cdb cdc cls cmd cob cpp cs cshtml css cxx dsr dtd f f2k f90 f95 for frm fs fsx fxh g4 gv h hpp hs hta htm html hxx idl inc java js json kml las lhs lisp lsp lua m master ml mli mx nt pas php php3 phtml pl plx pm  ps ps1 psm1 py pyw r rb rbw rpx scm scp sep sh shtm shtml smd sml sql ss st tcl tex thy tt vb vbhtml vbs vrg wsc wsdl wsf xaml xhtml xlf xliff xsd xsl xslt xsml xst xsx";
            var cfgFiles =
                "acl aml cfg config content csproj cva database din dsw dtproj dtsx dun ecf ecf edmx fcc fsproj gpd h1k hkf inf ini isp iss manifest nuspec obe overrideTasks pdm pp props ps1xml psc1 psd1 rsp sam sch sitemap sln stp svc targets tasks testsettings theme user vbp vbproj vdproj vsmdi xbap xml xrm-ms";
            var binFiles =
                "001 002 003 004 005 006 007 008 009 010 7z aas acm addin adm am amx ani apk apl aps aux avi ax bak bcf bcm bin bmp bpd bpl browser bsc btr bud cab cache camp cap cat cch ccu cdf-ms cdmp chk chm chr chs cht clb cmb cmdline cmf com comments compiled compositefont cov cpa cpl cpx crmlog crt csd ctm cty cur cw dat data db dcf dcr default delete dem desklink devicemetadata-ms diagpkg dic dir dll dlm dls dmp dnl doc docx drv ds dts dub dvd dvr-ms dxt ebd edb efi emf err ess etl ev1 ev2 ev3 evm evtx ex_ exe exp fe fon ftl fx gdl gif gmmp grl grm gs_4_0 h1c h1s h1t hdr hex hit hlp hpi hpx iad icc icm ico id idb idx iec if2 ilk imd ime inf_loc ins inx ipa ird jar jmx jnt job jpeg jpg jpn jrs jtp kor ldo lex lg1 lg2 lib library-ms lng lnk log lrc lts lxa mac man map mapimail mdb mdbx mfl mib mid mllr mni mof mp3 mp4 mpg msc msi msm msstyles mst msu mui mum mzz ncb ndx ngr nlp nls nlt ntf nupkg obj ocx olb old opt out pch pdb pdf phn pkc plugin pnf png ppd pptx prm prof propdesc prq prx ps_2_0 ps_4_0 psd ptxml pub que rat rdl reg res resources resx rld rll rom rpo rs rtf s3 sbr scc scr sdb sdi ses shp smp sqm ssm stl swf sys t4 tag tar.gz tbl tbr tha tif tiff tlb tmp toc tpi trie1 tsp ttc ttf tts tx_ txt uaq uce udt uni uninstall unt url vch vdf ver vp vs_1_1 vs_4_0 vsd vspscc wav wdf web wih wim win32manifest wma wmf wmv wmz wtv wwd x32 xex xlb xls xlsx zfsendtotarget zip";

            Assert.AreEqual(codeFiles, string.Join(" ", NfSettings.CodeFileExtensions));
            Assert.AreEqual(cfgFiles, string.Join(" ", NfSettings.ConfigFileExtensions));
            Assert.AreEqual(binFiles, string.Join(" ", NfSettings.BinaryFileExtensions));

            var exDirs =
                "bin obj Interop TestResults _svn .svn _ReSharper _TeamCity .git .nuget .vs lib build dist packages __pycache__";
            Assert.AreEqual(exDirs, string.Join(" ", NfSettings.ExcludeCodeDirectories));

        }

        [TestMethod]
        public void TestExpandCfgValue()
        {
            var testResult = ExpandCfgValue(testInput, @"$(tempRootDir)\httpAppDomain");
            Assert.IsNotNull(testInput);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestResolveIdValueHash()
        {
            ResolveIdValueHash(testInput);
            Assert.IsNotNull(testInput);
            Assert.AreNotEqual(0, testInput.Count);
            foreach (var k in testInput.Keys)
                System.Diagnostics.Debug.WriteLine($"{k} = {testInput[k]}");
        }

        #region testInputs
        private static readonly Dictionary<string, string> testInput = new Dictionary<string, string>
            {
                {"myPsHome", @"C:\Projects\31g\trunk" },
                {"tempJsFile", @"$(tempRootDir)\temp.js"},
                {"tempHtmlFile", @"$(tempRootDir)\temp.html"},
                {"tempCsvFile", @"$(tempRootDir)\temp.csv"},
                {"tempStdOutFile", @"$(tempRootDir)\stdout.txt"},
                {"tempT4TemplateFile", @"$(tempRootDir)\t4Temp.tt"},
                {"tempNetStatFile", @"$(tempRootDir)\netstat.txt"},
                {"tempWmiFile", @"$(tempTextDir)\wmi.txt"},
                {"x64SvcUtilTool", @"$(net461SdkTools)\x64\svcUtil.exe"},
                {"x64IldasmTool", @"$(net461SdkTools)\x64\ildasm.exe"},
                {"x64WsdlTool", @"$(net461SdkTools)\x64\wsdl.exe"},
                {"x64ClrVerTool", @"$(net461SdkTools)\x64\clrver.exe"},
                {"x64XsdExeTool", @"$(net461SdkTools)\x64\xsd.exe"},
                {"x64CdbTool", @"$(binX64RootDir)\debug\cdb.exe"},
                {"x64TListTool", @"$(binX64RootDir)\debug\tlist.exe"},
                {"x64SymChkTool", @"$(binX64RootDir)\debug\symChk.exe"},
                {"x64DumpbinTool", @"$(binX64RootDir)\vs10\dumpbin.exe"},
                {"x64SqlCmdTool", @"$(binX64RootDir)\sqlcmd\SQLCMD.EXE"},
                {"x64MdbgTool", @"$(binX64RootDir)\v4.0\Mdbg.exe"},
                {"x86IldasmTool", @"$(net461SdkTools)\ildasm.exe"},
                {"x86SqlMetalTool", @"$(net461SdkTools)\sqlmetal.exe"},
                {"x86SvcUtilTool", @"$(net461SdkTools)\svcUtil.exe"},
                {"x86WsdlTool", @"$(net461SdkTools)\wsdl.exe"},
                {"x86CdbTool", @"$(binX86RootDir)\debug\cdb.exe"},
                {"x86DependsTool", @"$(binX86RootDir)\depends\depends.exe"},
                {"x86DumpbinTool", @"$(binX86RootDir)\vs10\dumpbin.exe"},
                {"x86TextTransformTool", @"$(binX86RootDir)\TextTransform.exe"},
                {"x86DotExeTool", @"$(binRootDir)\graphviz-2.38\bin\dot.exe"},
                {"javaJavacTool", @"$(binJavaRootDir)\bin\javac.exe"},
                {"javaJavaTool", @"$(binJavaRootDir)\bin\java.exe"},
                {"javaJavaDocTool", @"$(binJavaRootDir)\bin\javadoc.exe"},
                {"javaJavaRtJarTool", @"$(binJavaRootDir)\jre\lib\rt.jar"},
                {"javaJarTool", @"$(binJavaRootDir)\bin\jar.exe"},
                {"javaJRunScriptTool", @"$(binJavaRootDir)\bin\jrunscript.exe"},
                {"javaAntlrTool", @"$(binRootDir)\antlr-4.1-complete.jar"},
                {"javaStanfordPostTaggerTool", @"$(binRootDir)\stanford-postagger-2015-12-09\stanford-postagger.jar"},
                {"javaStanfordPostTaggerModelsTool", @"$(binRootDir)\stanford-postagger-2015-12-09\models\"},
                {"customDia2DumpTool", @"$(binRootDir)\Dia2Dump.exe"},
                {"customInvokeGetCgTypeTool", @"$(binRootDir)\NoFuture.Gen.InvokeGetCgOfType.exe"},
                {"customInvokeGraphVizTool", @"$(binRootDir)\NoFuture.Gen.InvokeGraphViz.exe"},
                {"customInvokeAssemblyAnalysisTool", @"$(binRootDir)\NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe"},
                {"customInvokeFlattenTool", @"$(binRootDir)\NoFuture.Util.Gia.InvokeFlatten.exe"},
                {"customUtilPosHostTool", @"$(binRootDir)\NoFuture.Util.Pos.Host.exe"},
                {"customInvokeDpxTool", @"$(binRootDir)\NoFuture.Util.Binary.InvokeDpx.exe"},
                {"customInvokeNfTypeNameTool", @"$(binRootDir)\NoFuture.Tokens.InvokeNfTypeName.exe"},
                {"binFfmpegTool", @"$(binRootDir)\ffmpeg.exe"},
                {"binYoutubeDlTool", @"$(binRootDir)\youtube-dl.exe"},
                {"tempRootDir", @"$(myPsHome)\temp"},
                {"tempProcsDir", @"$(tempRootDir)\procs"},
                {"tempCodeDir", @"$(tempRootDir)\code"},
                {"tempTextDir", @"$(tempRootDir)\text"},
                {"tempDebugsDir", @"$(tempRootDir)\debug"},
                {"tempGraphDir", @"$(tempRootDir)\graph"},
                {"tempSvcUtilDir", @"$(tempRootDir)\svcUtil"},
                {"tempWsdlDir", @"$(tempRootDir)\wsdl"},
                {"tempHbmDir", @"$(tempRootDir)\hbm"},
                {"tempBinDir", @"$(tempRootDir)\tempBin"},
                {"tempJavaSrcDir", @"$(tempRootDir)\java\src"},
                {"tempJavaBuildDir", @"$(tempRootDir)\java\build"},
                {"tempJavaDistDir", @"$(tempRootDir)\java\dist"},
                {"tempJavaArchiveDir", @"$(tempRootDir)\java\archive"},
                {"tempCalendarDir", @"$(tempRootDir)\Calendar"},
                {"tempHttpAppDomainDir", @"$(tempRootDir)\httpAppDomain"},
                {"tempTsvCsvDir", @"$(tempRootDir)\tsvCsv"},
                {"binRootDir", @"$(myPsHome)\bin"},
                {"net461SdkTools", @"$(%ProgramFiles(x86)%)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools"},
                {"binX64RootDir", @"$(binRootDir)\amd64"},
                {"binX86RootDir", @"$(binRootDir)\x86"},
                {"binJavaRootDir", @"$(binRootDir)\java"},
                {"binT4TemplatesDir", @"$(binRootDir)\java"},
                {"binPhpRootDir", @"$(binRootDir)\php"},
                {"binDataRootDir", @"$(binRootDir)\Data\Source"},
                {"portNsLookupPort", @"799"},
                {"portDomainEngine", @"1138"},
                {"portHostProc", @"780"},
                {"portAssemblyAnalysis", @"5059"},
                {"portFlattenAssembly", @"5062"},
                {"portPartOfSpeechPaserHost", @"5063"},
                {"portSjclToPlainText", @"5064"},
                {"portSjclToCipherText", @"5065"},
                {"portSjclHashPort", @"5066"},
                {"portNfTypeNamePort", @"5067"},
                {"portHbmInvokeStoredProcMgr", @"45121"},
                {"switchPrintWebHeaders", @"True"},
                {"switchSqlCmdHeadersOff", @"True"},
                {"switchSqlFiltersOff", @"True"},
                {"switchSupressNpp", @"False"},
                {"keyAesEncryptionKey", @"gb0352wHVco94Gr260BpJzH+N1yrwmt5/BaVhXmPm6s="},
                {"keyAesIV", @"az9HzsMj6pygMvZyTpRo6g=="},
                {"keyHMACSHA1", @"eTcmPilTLmtbalRpKjFFJjpMNns="},
                {"code-file-extensions", @"ada adb adml admx ads as asa asax ascx asm asmx asp aspx bas bat bsh c cbd cbl cc cdb cdc cls cmd cob cpp cs cshtml css cxx dsr dtd f f2k f90 f95 for frm fs fsx fxh g4 gv h hpp hs hta htm html hxx idl inc java js json kml las lhs lisp lsp lua m master ml mli mx nt pas php php3 phtml pl plx pm  ps ps1 psm1 py pyw r rb rbw rpx scm scp sep sh shtm shtml smd sml sql ss st tcl tex thy tt vb vbhtml vbs vrg wsc wsdl wsf xaml xhtml xlf xliff xsd xsl xslt xsml xst xsx"},
                {"config-file-extensions", @"acl aml cfg config content csproj cva database din dsw dtproj dtsx dun ecf ecf edmx fcc fsproj gpd h1k hkf inf ini isp iss manifest nuspec obe overrideTasks pdm pp props ps1xml psc1 psd1 rsp sam sch sitemap sln stp svc targets tasks testsettings theme user vbp vbproj vdproj vsmdi xbap xml xrm-ms"},
                {"binary-file-extensions", @"001 002 003 004 005 006 007 008 009 010 7z aas acm addin adm am amx ani apk apl aps aux avi ax bak bcf bcm bin bmp bpd bpl browser bsc btr bud cab cache camp cap cat cch ccu cdf-ms cdmp chk chm chr chs cht clb cmb cmdline cmf com comments compiled compositefont cov cpa cpl cpx crmlog crt csd ctm cty cur cw dat data db dcf dcr default delete dem desklink devicemetadata-ms diagpkg dic dir dll dlm dls dmp dnl doc docx drv ds dts dub dvd dvr-ms dxt ebd edb efi emf err ess etl ev1 ev2 ev3 evm evtx ex_ exe exp fe fon ftl fx gdl gif gmmp grl grm gs_4_0 h1c h1s h1t hdr hex hit hlp hpi hpx iad icc icm ico id idb idx iec if2 ilk imd ime inf_loc ins inx ipa ird jar jmx jnt job jpeg jpg jpn jrs jtp kor ldo lex lg1 lg2 lib library-ms lng lnk log lrc lts lxa mac man map mapimail mdb mdbx mfl mib mid mllr mni mof mp3 mp4 mpg msc msi msm msstyles mst msu mui mum mzz ncb ndx ngr nlp nls nlt ntf nupkg obj ocx olb old opt out pch pdb pdf phn pkc plugin pnf png ppd pptx prm prof propdesc prq prx ps_2_0 ps_4_0 psd ptxml pub que rat rdl reg res resources resx rld rll rom rpo rs rtf s3 sbr scc scr sdb sdi ses shp smp sqm ssm stl swf sys t4 tag tar.gz tbl tbr tha tif tiff tlb tmp toc tpi trie1 tsp ttc ttf tts tx_ txt uaq uce udt uni uninstall unt url vch vdf ver vp vs_1_1 vs_4_0 vsd vspscc wav wdf web wih wim win32manifest wma wmf wmv wmz wtv wwd x32 xex xlb xls xlsx zfsendtotarget zip"},
                {"search-directory-exclusions", @"bin obj Interop TestResults _svn .svn _ReSharper _TeamCity .git .nuget .vs lib build dist packages __pycache__"},
                {"default-block-size", "256"},
                {"default-type-separator", "."},
                {"default-char-separator", ","},
                {"cmd-line-arg-switch", "-"},
                {"cmd-line-arg-assign", "="},
                {"punctuation-chars", "! \" # $ % & \\ ' ( ) * + , - . / : ; < = > ? @ [ ] ^ _ ` { | } ~"},

            };

        private const string TEST_FILE = @"
<no-future>
  <shared>
    <config>
      <root id='myPsHome' value='C:\Projects\31g\trunk' />
      <bools>
        <add id='switchPrintWebHeaders' value='True' />
        <add id='switchSqlCmdHeadersOff' value='True' />
        <add id='switchSqlFiltersOff' value='True' />
        <add id='switchSupressNpp' value='False' />
      </bools>
      <chars>
        <add id='punctuation-chars'><![CDATA[! # $ % & \ ' ( ) * + , - . / : ; < = > ? @ [ ] ^ _ ` { | } ~]]></add>
        <add id='default-type-separator'><![CDATA[.]]></add>
        <add id='default-char-separator'><![CDATA[,]]></add>
        <add id='cmd-line-arg-assign'><![CDATA[=]]></add>
      </chars>
      <ints>
        <add id='default-block-size'>256</add>
      </ints>
      <strings>
        <add id='cmd-line-arg-switch'><![CDATA[-]]></add>
      </strings>
      <ports>
        <add id='portNsLookupPort' value='799' />
        <add id='portDomainEngine' value='1138' />
        <add id='portHostProc' value='780' />
        <!--will add to this two times-->
        <add id='portAssemblyAnalysis' value='5059' />
        <add id='portFlattenAssembly' value='5062' />
        <add id='portPartOfSpeechPaserHost' value='5063' />
        <add id='portSjclToPlainText' value='5064' />
        <add id='portSjclToCipherText' value='5065' />
        <add id='portSjclHashPort' value='5066' />
        <add id='portNfTypeNamePort' value='5067' />
        <add id='portHbmInvokeStoredProcMgr' value='45121' />
      </ports>
      <keys>
        <!--these are just for flippin' bits not indended to provide any security-->
        <add id='keyAesEncryptionKey' value='gb0352wHVco94Gr260BpJzH+N1yrwmt5/BaVhXmPm6s=' />
        <add id='keyAesIV' value='az9HzsMj6pygMvZyTpRo6g==' />
        <add id='keyHMACSHA1' value='eTcmPilTLmtbalRpKjFFJjpMNns=' />
        <!--
        deliberately left blank and should be assigned by the runtime from sources not under 
        any kind of source control.
        -->
        <add id='keyGoogleCodeApiKey' value='' />
        <add id='keyBeaDataApiKey' value='' />
        <add id='keyCensusDataApiKey' value='' />
        <add id='keyBlsApiRegistrationKey' value='' />
      </keys>
      <uris>
        <add id='uriProxyServer' value='' />
      </uris>
      <files>
        <certs>
          <add id='certFileNoFutureX509' value='$(myPsHome)\NoFuture.cer' />
        </certs>
        <images>
          <add id='favicon' value='$(myPsHome)\favicon.ico' />
        </images>
        <temps>
          <add id='tempStdOutFile' value='$(tempRootDir)\stdout.txt' />
          <add id='tempT4TemplateFile' value='$(tempRootDir)\t4Temp.tt' />
          <add id='tempNetStatFile' value='$(tempRootDir)\netstat.txt' />
          <add id='tempWmiFile' value='$(tempTextDir)\wmi.txt' />
        </temps>
        <tools>
          <x64>
            <add id='x64SvcUtilTool' value='$(net461SdkTools)\x64\svcUtil.exe' />
            <add id='x64WsdlTool' value='$(net461SdkTools)\x64\wsdl.exe' />
            <add id='x64ClrVerTool' value='$(net461SdkTools)\x64\clrver.exe' />
            <add id='x64XsdExeTool' value='$(net461SdkTools)\x64\xsd.exe' />
            <add id='x64DependsTool' value='$(binX64RootDir)\depends\depends.exe' />
            <add id='x64TListTool' value='$(binX64RootDir)\debug\tlist.exe' />
            <add id='x64SymChkTool' value='$(binX64RootDir)\debug\symChk.exe' />
            <add id='x64DumpbinTool' value='$(binX64RootDir)\vs10\dumpbin.exe' />
            <add id='x64SqlCmdTool' value='$(binX64RootDir)\sqlcmd\SQLCMD.EXE' />
          </x64>
          <x86>
            <add id='x86SqlMetalTool' value='$(net461SdkTools)\sqlmetal.exe' />
            <add id='x86SvcUtilTool' value='$(net461SdkTools)\svcUtil.exe' />
            <add id='x86WsdlTool' value='$(net461SdkTools)\wsdl.exe' />
            <add id='x86DependsTool' value='$(binX86RootDir)\depends\depends.exe' />
            <add id='x86DumpbinTool' value='$(binX86RootDir)\vs10\dumpbin.exe' />
            <add id='x86TextTransformTool' value='$(binX86RootDir)\TextTransform.exe' />
            <add id='x86DotExeTool' value='$(binRootDir)\graphviz-2.38\bin\dot.exe' />
          </x86>
          <java>
            <add id='javaJavacTool' value='$(binJavaRootDir)\bin\javac.exe' />
            <add id='javaJavaTool' value='$(binJavaRootDir)\bin\java.exe' />
            <add id='javaJavaDocTool' value='$(binJavaRootDir)\bin\javadoc.exe' />
            <add id='javaJavaRtJarTool' value='$(binJavaRootDir)\jre\lib\rt.jar' />
            <add id='javaJarTool' value='$(binJavaRootDir)\bin\jar.exe' />
            <add id='javaJRunScriptTool' value='$(binJavaRootDir)\bin\jrunscript.exe' />
            <add id='javaAntlrTool' value='$(binRootDir)\antlr-4.1-complete.jar' />
            <add id='javaStanfordPostTaggerTool' value='$(binRootDir)\stanford-postagger-2015-12-09\stanford-postagger.jar' />
            <add id='javaStanfordPostTaggerModelsTool' value='$(binRootDir)\stanford-postagger-2015-12-09\models\' />
          </java>
          <custom>
            <add id='customDia2DumpTool' value='$(binRootDir)\Dia2Dump.exe' />
            <add id='customInvokeGetCgTypeTool' value='$(binRootDir)\NoFuture.Gen.InvokeGetCgOfType.exe' />
            <add id='customInvokeGraphVizTool' value='$(binRootDir)\NoFuture.Gen.InvokeGraphViz.exe' />
            <add id='customInvokeAssemblyAnalysisTool' value='$(binRootDir)\NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe' />
            <add id='customInvokeFlattenTool' value='$(binRootDir)\NoFuture.Util.Gia.InvokeFlatten.exe' />
            <add id='customUtilPosHostTool' value='$(binRootDir)\NoFuture.Util.Pos.Host.exe' />
            <add id='customInvokeDpxTool' value='$(binRootDir)\NoFuture.Util.Binary.InvokeDpx.exe' />
            <add id='customInvokeNfTypeNameTool' value='$(binRootDir)\NoFuture.Tokens.InvokeNfTypeName.exe' />
          </custom>
          <bin>
            <add id='binFfmpegTool' value='$(binRootDir)\ffmpeg.exe' />
            <add id='binYoutubeDlTool' value='$(binRootDir)\youtube-dl.exe' />
          </bin>
        </tools>
        <extensions>
          <add id='code-file-extensions'><![CDATA[ada adb adml admx ads as asa asax ascx asm asmx asp aspx bas bat bsh c cbd cbl cc cdb cdc cls cmd cob cpp cs cshtml css cxx dsr dtd f f2k f90 f95 for frm fs fsx fxh g4 gv h hpp hs hta htm html hxx idl inc java js json kml las lhs lisp lsp lua m master ml mli mx nt pas php php3 phtml pl plx pm  ps ps1 psm1 py pyw r rb rbw rpx scm scp sep sh shtm shtml smd sml sql ss st tcl tex thy tt vb vbhtml vbs vrg wsc wsdl wsf xaml xhtml xlf xliff xsd xsl xslt xsml xst xsx]]></add>
          <add id='config-file-extensions'><![CDATA[acl aml cfg config content csproj cva database din dsw dtproj dtsx dun ecf ecf edmx fcc fsproj gpd h1k hkf inf ini isp iss manifest nuspec obe overrideTasks pdm pp props ps1xml psc1 psd1 rsp sam sch sitemap sln stp svc targets tasks testsettings theme user vbp vbproj vdproj vsmdi xbap xml xrm-ms]]></add>
          <add id='binary-file-extensions'><![CDATA[001 002 003 004 005 006 007 008 009 010 7z aas acm addin adm am amx ani apk apl aps aux avi ax bak bcf bcm bin bmp bpd bpl browser bsc btr bud cab cache camp cap cat cch ccu cdf-ms cdmp chk chm chr chs cht clb cmb cmdline cmf com comments compiled compositefont cov cpa cpl cpx crmlog crt csd ctm cty cur cw dat data db dcf dcr default delete dem desklink devicemetadata-ms diagpkg dic dir dll dlm dls dmp dnl doc docx drv ds dts dub dvd dvr-ms dxt ebd edb efi emf err ess etl ev1 ev2 ev3 evm evtx ex_ exe exp fe fon ftl fx gdl gif gmmp grl grm gs_4_0 h1c h1s h1t hdr hex hit hlp hpi hpx iad icc icm ico id idb idx iec if2 ilk imd ime inf_loc ins inx ipa ird jar jmx jnt job jpeg jpg jpn jrs jtp kor ldo lex lg1 lg2 lib library-ms lng lnk log lrc lts lxa mac man map mapimail mdb mdbx mfl mib mid mllr mni mof mp3 mp4 mpg msc msi msm msstyles mst msu mui mum mzz ncb ndx ngr nlp nls nlt ntf nupkg obj ocx olb old opt out pch pdb pdf phn pkc plugin pnf png ppd pptx prm prof propdesc prq prx ps_2_0 ps_4_0 psd ptxml pub que rat rdl reg res resources resx rld rll rom rpo rs rtf s3 sbr scc scr sdb sdi ses shp smp sqm ssm stl swf sys t4 tag tar.gz tbl tbr tha tif tiff tlb tmp toc tpi trie1 tsp ttc ttf tts tx_ txt uaq uce udt uni uninstall unt url vch vdf ver vp vs_1_1 vs_4_0 vsd vspscc wav wdf web wih wim win32manifest wma wmf wmv wmz wtv wwd x32 xex xlb xls xlsx zfsendtotarget zip]]></add>
        </extensions>
      </files>
      <directories>
        <add id='search-directory-exclusions'><![CDATA[bin obj Interop TestResults _svn .svn _ReSharper _TeamCity .git .nuget .vs lib build dist packages __pycache__]]></add>
        <temps>
          <add id='tempRootDir' value='$(myPsHome)\temp' />
          <add id='tempSqlDir' value='$(tempRootDir)\sql' />
          <add id='tempProcsDir' value='$(tempRootDir)\prox' />
          <add id='tempCodeDir' value='$(tempRootDir)\code' />
          <add id='tempTextDir' value='$(tempRootDir)\text' />
          <add id='tempDebugsDir' value='$(tempRootDir)\debug' />
          <add id='tempGraphDir' value='$(tempRootDir)\graph' />
          <add id='tempSvcUtilDir' value='$(tempCodeDir)\svcUtil' />
          <add id='tempWsdlDir' value='$(tempCodeDir)\wsdl' />
          <add id='tempHbmDir' value='$(tempCodeDir)\hbm' />
          <add id='tempJavaSrcDir' value='$(tempCodeDir)\java\src' />
          <add id='tempJavaBuildDir' value='$(tempCodeDir)\java\build' />
          <add id='tempJavaDistDir' value='$(tempCodeDir)\java\dist' />
          <add id='tempJavaArchiveDir' value='$(tempCodeDir)\java\archive' />
          <add id='tempCalendarDir' value='$(tempRootDir)\Calendar' />
          <add id='tempHttpAppDomainDir' value='$(tempRootDir)\httpAppDomain' />
          <add id='tempAudioDir' value='$(tempRootDir)\audio' />
          <add id='tempTsvCsvDir' value='$(tempRootDir)\tsvCsv' />
        </temps>
        <bins>
          <add id='binRootDir' value='$(myPsHome)\bin' />
          <add id='net461SdkTools' value='$(%ProgramFiles(x86)%)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools' />
          <add id='binX64RootDir' value='$(binRootDir)\amd64' />
          <add id='binX86RootDir' value='$(binRootDir)\x86' />
          <add id='binJavaRootDir' value='$(binRootDir)\java' />
          <add id='binT4TemplatesDir' value='$(binRootDir)\Templates' />
        </bins>
      </directories>
    </config>
  </shared>
</no-future>
";

        #endregion
    }
}
