﻿using System;
using System.Collections.Generic;
using System.Xml;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NUnit.Framework;

namespace NoFuture.Core.Cfg.Tests
{
    [TestFixture]
    public class TestNfConfig
    {
        [Test]
        public void TestGetIdValueHash()
        {
            var cfgXml = new XmlDocument();
            cfgXml.LoadXml(TEST_FILE);
            var idValueHash = NfConfig.GetIdValueHash(cfgXml);

            Assert.IsNotNull(idValueHash);
            Assert.AreNotEqual(0, idValueHash.Count);
            foreach(var k in idValueHash.Keys)
                Console.WriteLine("{" + $"\"{k}\", @\"{idValueHash[k]}\"" + "}," );
        }

        [Test]
        public void TestInit()
        {
            NfConfig.Init(TEST_FILE);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin", NfConfig.BinDirectories.Root);
            var puncChars = @"! # $ % & \ ' ( ) * + , - . / : ; < = > ? @ [ ] ^ _ ` { | } ~";
            Assert.AreEqual(puncChars, string.Join(" ", NfSettings.PunctuationChars));

            Assert.AreEqual(@"C:\Projects\_NoFuture\temp", NfConfig.TempDirectories.Root);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\sql", NfConfig.TempDirectories.Sql);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\prox", NfConfig.TempDirectories.StoredProx);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\code", NfConfig.TempDirectories.Code);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\debug", NfConfig.TempDirectories.Debug);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\graph", NfConfig.TempDirectories.Graph);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\code\svcUtil", NfConfig.TempDirectories.SvcUtil);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\code\wsdl", NfConfig.TempDirectories.Wsdl);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\code\hbm", NfConfig.TempDirectories.Hbm);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\code\java\src", NfConfig.TempDirectories.JavaSrc);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\code\java\build", NfConfig.TempDirectories.JavaBuild);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\code\java\dist", NfConfig.TempDirectories.JavaDist);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\code\java\archive", NfConfig.TempDirectories.JavaArchive);
            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\audio", NfConfig.TempDirectories.Audio);

            Assert.AreEqual(@"C:\Projects\_NoFuture\temp\netstat.txt", NfConfig.TempFiles.NetStat);

            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\amd64", NfConfig.BinDirectories.X64Root);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\x86", NfConfig.BinDirectories.X86Root);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\java", NfConfig.BinDirectories.JavaRoot);

            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\svcUtil.exe", NfConfig.X64.SvcUtil);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\amd64\vs10\dumpbin.exe", NfConfig.X64.Dumpbin);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\amd64\sqlcmd\SQLCMD.EXE", NfConfig.X64.SqlCmd);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\wsdl.exe", NfConfig.X64.Wsdl);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\xsd.exe", NfConfig.X64.XsdExe);

            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\sqlmetal.exe", NfConfig.X86.SqlMetal);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\svcUtil.exe", NfConfig.X86.SvcUtil);
            Assert.AreEqual(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\wsdl.exe", NfConfig.X86.Wsdl);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\graphviz-2.38\bin\dot.exe", NfConfig.X86.DotExe);

            Assert.AreEqual(799, NfConfig.NfDefaultPorts.NsLookupPort);

            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\java\bin\javac.exe", NfConfig.JavaTools.Javac);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\java\bin\java.exe", NfConfig.JavaTools.Java);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\java\bin\jar.exe", NfConfig.JavaTools.Jar);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\antlr-4.7.1-complete.jar", NfConfig.JavaTools.Antlr);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\stanford-postagger-2017-06-09\stanford-postagger.jar", NfConfig.JavaTools.StanfordPostTagger);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\stanford-postagger-2017-06-09\models\", NfConfig.JavaTools.StanfordPostTaggerModels);

            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\Dia2Dump.exe", NfConfig.CustomTools.Dia2Dump);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\NoFuture.Gen.InvokeGetCgOfType.exe", NfConfig.CustomTools.InvokeGetCgType);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\NoFuture.Gen.InvokeGraphViz.exe", NfConfig.CustomTools.InvokeGraphViz);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.exe", NfConfig.CustomTools.InvokeAssemblyAnalysis);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\NoFuture.Util.Gia.InvokeFlatten.exe", NfConfig.CustomTools.InvokeFlatten);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\NoFuture.Util.Pos.Host\NoFuture.Util.Pos.Host.exe", NfConfig.CustomTools.UtilPosHost);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\NoFuture.Util.DotNetMeta.InvokeDpx.exe", NfConfig.CustomTools.InvokeDpx);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\NoFuture.Tokens.InvokeNfTypeName.exe", NfConfig.CustomTools.InvokeNfTypeName);

            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\ffmpeg.exe", NfConfig.PythonTools.Ffmpeg);
            Assert.AreEqual(@"C:\Projects\_NoFuture\bin\youtube-dl.exe", NfConfig.PythonTools.YoutubeDl);

            Assert.IsTrue(NfConfig.Switches.SqlFiltersOff);
            Assert.IsTrue(NfConfig.Switches.PrintWebHeaders);

            Assert.AreEqual(@"C:\Projects\_NoFuture\NoFuture.cer", NfConfig.SecurityKeys.NoFutureX509Cert);

            var codeFiles =
                "ada adb adml admx ads as asa asax ascx asm asmx asp aspx bas bat bsh c cbd cbl cc cdb cdc cls cmd cob cpp cs cshtml css cu cuh cxx dsr dtd f f2k f90 f95 fbs for frm fs fsx fxh g4 gv h hpp hs hta htm html hxx idl inc java js jsx json kml las lhs lisp lsp lua m master ml mli mx nt pas php php3 phtml pl plx pm ps ps1 psm1 py pyw r rb rbw rpx sbt scala scm scp scss sep sh shtm shtml smd sml sql ss st tcl tex thy tt ts vb vbhtml vbs vrg wsc wsdl wsf xaml xhtml xlf xliff xsd xsl xslt xsml xst xsx";
            var cfgFiles =
                "acl aml cfg cmake conf config content csproj cva database dgml din dsw dtproj dtsx dun ecf editorconfig edmx fcc fsproj gpd h1k hkf inf ini isp iss manifest md npmrc nuspec obe overrideTasks pdm pp props ps1xml psc1 psd1 rej rsp sam sch sitemap sln sps stp svc targets tasks testsettings theme user vbp vbproj vdproj vsmdi xbap xml xrm-ms xss yarnrc yml";
            var binFiles =
                "001 002 003 004 005 006 007 008 009 010 7z aas acm addin adm am amx ani apk apl aps aux avi ax bak bcf bcm bin bmp bpd bpl browser bsc btr bud cab cache camp cap cat cch ccu cdf-ms cdmp chk chm chr chs cht clb cmb cmdline cmf com comments compiled compositefont cov cpa cpl cpx crmlog crt csd ctm cty cur cw dat data db dcf dcr default delete dem desklink devicemetadata-ms diagpkg dic dir dll dlm dls dmp dnl doc docx drv ds dts dub dvd dvr-ms dxt ebd edb efi emf eot err ess etl ev1 ev2 ev3 evm evtx ex_ exe exp fb fe fon ftl fx gdl gif gmmp grl grm gs_4_0 h1c h1s h1t hdr hex hit hlp hpi hpx iad icc icm ico id idb idx iec if2 ilk imd ime inf_loc ins inx ipa ird jar jmx jnt job jpeg jpg jpn jrs jtp kor ldo lex lg1 lg2 lib library-ms lng lnk log lrc lts lxa mac man map mapimail mdb mdbx mfl mib mid mllr mni mof mp3 mp4 mpg msc msi msm msstyles mst msu mui mum mzz ncb ndx ngr nlp nls nlt ntf nupkg obj ocx olb old opt otf out pch pdb pdf phn pkc plugin pnf png ppd pptx prm prof propdesc prq prx ps_2_0 ps_4_0 psd ptxml pub que rat rdl reg res resources resx rld rll rom rpo rs rtf s3 sbr scc scr sdb sdi ses shp smp sqm ssm stl swf sys t4 tag tar.gz tbl tbr tha tif tiff tlb tmp toc tpi trie1 tsp ttc ttf tts tx_ txt uaq uce udt uni uninstall unt url vch vdf ver vp vs_1_1 vs_4_0 vsd vspscc wav wdf web wih wim win32manifest wma wmf wmv wmz woff woff2 wtv wwd x32 xex xlb xls xlsx zfsendtotarget zip";

            Assert.AreEqual(codeFiles, string.Join(" ", NfSettings.CodeFileExtensions));
            Assert.AreEqual(cfgFiles, string.Join(" ", NfSettings.ConfigFileExtensions));
            Assert.AreEqual(binFiles, string.Join(" ", NfSettings.BinaryFileExtensions));

            var exDirs =
                "\\bin\\ \\obj\\ \\Interop\\ \\TestResults\\ \\_svn\\ \\.svn\\ \\_ReSharper \\_TeamCity \\.git\\ \\.github\\ \\.nuget\\ \\.vs\\ \\lib\\ \\build\\ \\dist\\ \\packages\\ \\__pycache__\\ \\.metadata\\ \\.vscode\\";
            Assert.AreEqual(exDirs, string.Join(" ", NfSettings.ExcludeCodeDirectories));

        }

        [Test]
        public void TestExpandCfgValue()
        {
            var testResult = NfConfig.ExpandCfgValue(testInput, @"$(tempRootDir)\httpAppDomain");
            Assert.IsNotNull(testInput);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestResolveIdValueHash()
        {
            NfConfig.ResolveIdValueHash(testInput);
            Assert.IsNotNull(testInput);
            Assert.AreNotEqual(0, testInput.Count);
            foreach (var k in testInput.Keys)
                Console.WriteLine($"{k} = {testInput[k]}");
        }

        #region testInputs
        private static readonly Dictionary<string, string> testInput = new Dictionary<string, string>
            {
                {"myPsHome", @"C:\Projects\_NoFuture" },
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

        private const string TEST_FILE = @"<?xml version='1.0' encoding='utf-8' ?>
<!--
Cdata is space separated
$(someId) means, 'resolve this value using which ever node has this ID in this file.'
$(%environmentVar%) means, 'resolve this to the environment variable contained in between the two '%''
-->
<no-future>
  <shared>
    <config>
      <root id='myPsHome' value='C:\Projects\_NoFuture' />
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
        <!--will add to this five times-->
        <add id='portAssemblyAnalysis' value='5056' />
        <add id='portFlattenAssembly' value='5062' />
        <add id='portPartOfSpeechPaserHost' value='5063' />
        <add id='portSjclToPlainText' value='5064' />
        <add id='portSjclToCipherText' value='5065' />
        <add id='portSjclHashPort' value='5066' />
        <add id='portNfTypeNamePort' value='5067' />
        <add id='portHbmInvokeStoredProcMgr' value='45121' />
      </ports>
      <keys>
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
          <add id='tempNetStatFile' value='$(tempRootDir)\netstat.txt' />
        </temps>
        <tools>
          <x64>
            <add id='x64SvcUtilTool' value='$(net461SdkTools)\x64\svcUtil.exe' />
            <add id='x64WsdlTool' value='$(net461SdkTools)\x64\wsdl.exe' />
            <add id='x64XsdExeTool' value='$(net461SdkTools)\x64\xsd.exe' />
            <add id='x64DumpbinTool' value='$(binX64RootDir)\vs10\dumpbin.exe' />
            <add id='x64SqlCmdTool' value='$(binX64RootDir)\sqlcmd\SQLCMD.EXE' />
          </x64>
          <x86>
            <add id='x86SqlMetalTool' value='$(net461SdkTools)\sqlmetal.exe' />
            <add id='x86SvcUtilTool' value='$(net461SdkTools)\svcUtil.exe' />
            <add id='x86WsdlTool' value='$(net461SdkTools)\wsdl.exe' />
            <add id='x86DotExeTool' value='$(binRootDir)\graphviz-2.38\bin\dot.exe' />
          </x86>
          <dotnet>
            <add id='cscExe' value='C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' />
            <add id='vbcExe' value='C:\Windows\Microsoft.NET\Framework64\v4.0.30319\vbc.exe' />
          </dotnet>
          <java>
            <add id='javaJavacTool' value='$(binJavaRootDir)\bin\javac.exe' />
            <add id='javaJavaTool' value='$(binJavaRootDir)\bin\java.exe' />
            <add id='javaJarTool' value='$(binJavaRootDir)\bin\jar.exe' />
            <add id='javaAntlrTool' value='$(binRootDir)\antlr-4.7.1-complete.jar' />
            <add id='javaStanfordPostTaggerTool' value='$(binRootDir)\stanford-postagger-2017-06-09\stanford-postagger.jar' />
            <add id='javaStanfordPostTaggerModelsTool' value='$(binRootDir)\stanford-postagger-2017-06-09\models\' />
          </java>
          <custom>
            <add id='customDia2DumpTool' value='$(binRootDir)\Dia2Dump.exe' />
            <add id='customInvokeGetCgTypeTool' value='$(binRootDir)\NoFuture.Gen.InvokeGetCgOfType.exe' />
            <add id='customInvokeGraphVizTool' value='$(binRootDir)\NoFuture.Gen.InvokeGraphViz.exe' />
            <add id='customInvokeAssemblyAnalysisTool' value='$(binRootDir)\NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.exe' />
            <add id='customInvokeFlattenTool' value='$(binRootDir)\NoFuture.Util.Gia.InvokeFlatten.exe' />
            <add id='customUtilPosHostTool' value='$(binRootDir)\NoFuture.Util.Pos.Host\NoFuture.Util.Pos.Host.exe' />
            <add id='customInvokeDpxTool' value='$(binRootDir)\NoFuture.Util.DotNetMeta.InvokeDpx.exe' />
            <add id='customInvokeNfTypeNameTool' value='$(binRootDir)\NoFuture.Tokens.InvokeNfTypeName.exe' />
          </custom>
          <python>
            <add id='pythonExe' value='$(python36)\python.exe' />
            <add id='ffmpegTool' value='$(binRootDir)\ffmpeg.exe' />
            <add id='youtubeDlTool' value='$(binRootDir)\youtube-dl.exe' />
          </python>
        </tools>
        <extensions>
          <add id='code-file-extensions'><![CDATA[ada adb adml admx ads as asa asax ascx asm asmx asp aspx bas bat bsh c cbd cbl cc cdb cdc cls cmd cob cpp cs cshtml css cu cuh cxx dsr dtd f f2k f90 f95 fbs for frm fs fsx fxh g4 gv h hpp hs hta htm html hxx idl inc java js jsx json kml las lhs lisp lsp lua m master ml mli mx nt pas php php3 phtml pl plx pm ps ps1 psm1 py pyw r rb rbw rpx sbt scala scm scp scss sep sh shtm shtml smd sml sql ss st tcl tex thy tt ts vb vbhtml vbs vrg wsc wsdl wsf xaml xhtml xlf xliff xsd xsl xslt xsml xst xsx]]></add>
          <add id='config-file-extensions'><![CDATA[acl aml cfg cmake conf config content csproj cva database dgml din dsw dtproj dtsx dun ecf editorconfig edmx fcc fsproj gpd h1k hkf inf ini isp iss manifest md npmrc nuspec obe overrideTasks pdm pp props ps1xml psc1 psd1 rej rsp sam sch sitemap sln sps stp svc targets tasks testsettings theme user vbp vbproj vdproj vsmdi xbap xml xrm-ms xss yarnrc yml]]></add>
          <add id='binary-file-extensions'><![CDATA[001 002 003 004 005 006 007 008 009 010 7z aas acm addin adm am amx ani apk apl aps aux avi ax bak bcf bcm bin bmp bpd bpl browser bsc btr bud cab cache camp cap cat cch ccu cdf-ms cdmp chk chm chr chs cht clb cmb cmdline cmf com comments compiled compositefont cov cpa cpl cpx crmlog crt csd ctm cty cur cw dat data db dcf dcr default delete dem desklink devicemetadata-ms diagpkg dic dir dll dlm dls dmp dnl doc docx drv ds dts dub dvd dvr-ms dxt ebd edb efi emf eot err ess etl ev1 ev2 ev3 evm evtx ex_ exe exp fb fe fon ftl fx gdl gif gmmp grl grm gs_4_0 h1c h1s h1t hdr hex hit hlp hpi hpx iad icc icm ico id idb idx iec if2 ilk imd ime inf_loc ins inx ipa ird jar jmx jnt job jpeg jpg jpn jrs jtp kor ldo lex lg1 lg2 lib library-ms lng lnk log lrc lts lxa mac man map mapimail mdb mdbx mfl mib mid mllr mni mof mp3 mp4 mpg msc msi msm msstyles mst msu mui mum mzz ncb ndx ngr nlp nls nlt ntf nupkg obj ocx olb old opt otf out pch pdb pdf phn pkc plugin pnf png ppd pptx prm prof propdesc prq prx ps_2_0 ps_4_0 psd ptxml pub que rat rdl reg res resources resx rld rll rom rpo rs rtf s3 sbr scc scr sdb sdi ses shp smp sqm ssm stl swf sys t4 tag tar.gz tbl tbr tha tif tiff tlb tmp toc tpi trie1 tsp ttc ttf tts tx_ txt uaq uce udt uni uninstall unt url vch vdf ver vp vs_1_1 vs_4_0 vsd vspscc wav wdf web wih wim win32manifest wma wmf wmv wmz woff woff2 wtv wwd x32 xex xlb xls xlsx zfsendtotarget zip]]></add>
        </extensions>
      </files>
      <directories>
        <add id='search-directory-exclusions'><![CDATA[\bin\ \obj\ \Interop\ \TestResults\ \_svn\ \.svn\ \_ReSharper \_TeamCity \.git\ \.github\ \.nuget\ \.vs\ \lib\ \build\ \dist\ \packages\ \__pycache__\ \.metadata\ \.vscode\]]></add>
        <temps>
          <add id='tempRootDir' value='$(myPsHome)\temp' />
          <add id='tempSqlDir' value='$(tempRootDir)\sql' />
          <add id='tempProcsDir' value='$(tempRootDir)\prox' />
          <add id='tempCodeDir' value='$(tempRootDir)\code' />
          <add id='tempDebugsDir' value='$(tempRootDir)\debug' />
          <add id='tempGraphDir' value='$(tempRootDir)\graph' />
          <add id='tempSvcUtilDir' value='$(tempCodeDir)\svcUtil' />
          <add id='tempWsdlDir' value='$(tempCodeDir)\wsdl' />
          <add id='tempHbmDir' value='$(tempCodeDir)\hbm' />
          <add id='tempJavaSrcDir' value='$(tempCodeDir)\java\src' />
          <add id='tempJavaBuildDir' value='$(tempCodeDir)\java\build' />
          <add id='tempJavaDistDir' value='$(tempCodeDir)\java\dist' />
          <add id='tempJavaArchiveDir' value='$(tempCodeDir)\java\archive' />
          <add id='tempAudioDir' value='$(tempRootDir)\audio' />
        </temps>
        <bins>
          <add id='binRootDir' value='$(myPsHome)\bin' />
          <add id='net461SdkTools' value='$(%ProgramFiles(x86)%)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools' />
          <add id='binX64RootDir' value='$(binRootDir)\amd64' />
          <add id='binX86RootDir' value='$(binRootDir)\x86' />
          <add id='binJavaRootDir' value='$(binRootDir)\java' />
          <add id='python36' value='C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python36_64' />
          <add id='python35' value='C:\Anaconda3' />
          <add id='anaconda5_2_0' value='C:\Program Files (x86)\Microsoft Visual Studio\Shared\Anaconda3_64' />
        </bins>
      </directories>
    </config>
  </shared>
</no-future>
";

        #endregion
    }
}