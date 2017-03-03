﻿using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var idValueHash = NoFuture.Shared.NfConfig.GetIdValueHash(cfgXml);

            Assert.IsNotNull(idValueHash);
            Assert.AreNotEqual(0, idValueHash.Count);
            foreach(var k in idValueHash.Keys)
                System.Diagnostics.Debug.WriteLine("{" + $"\"{k}\", @\"{idValueHash[k]}\"" + "}," );
        }

        [TestMethod]
        public void TestInit()
        {
            NoFuture.Shared.NfConfig.Init(TEST_FILE);
            Assert.AreEqual(@"C:\Projects\31g\trunk\bin", NoFuture.Shared.NfConfig.BinDirectories.Root);
            
        }

        [TestMethod]
        public void TestExpandCfgValue()
        {
            var testResult = NoFuture.Shared.NfConfig.ExpandCfgValue(testInput, @"$(tempRootDir)\httpAppDomain");
            Assert.IsNotNull(testInput);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestResolveIdValueHash()
        {
            NoFuture.Shared.NfConfig.ResolveIdValueHash(testInput);
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
            };

        private const string TEST_FILE = @"
<!--
Cdata is space separated
$(someId) means, 'resolve this value to using which every node has this ID in this file.'
$(%environmentVar%) means, 'resolve this to the environment variable contained in between the two '%''
-->
<no-future>
  <shared>
    <config>
      <root id='myPsHome' value='C:\Projects\31g\trunk' />
      <chars>
        <punctuation><![CDATA[. ? : ! , ; { } [ ] ( ) _ @ % & * - \\ /]]></punctuation>
        <default-type-separator><![CDATA[.]]></default-type-separator>
        <default-char-separator><![CDATA[,]]></default-char-separator>
        <cmd-line-arg-switch><![CDATA[-]]></cmd-line-arg-switch>
        <cmd-line-arg-assign><![CDATA[=]]></cmd-line-arg-assign>
        <string-terminator><![CDATA[\0]]></string-terminator>
        <ints>
          <default-block-size>256</default-block-size>
        </ints>
      </chars>
      <files>
        <certs>
          <add id='certFileNoFutureX509' value='' />
        </certs>
        <temps>
          <add id='tempJsFile' value='$(tempRootDir)\temp.js' />
          <add id='tempHtmlFile' value='$(tempRootDir)\temp.html' />
          <add id='tempCsvFile' value='$(tempRootDir)\temp.csv' />
          <add id='tempStdOutFile' value='$(tempRootDir)\stdout.txt' />
          <add id='tempT4TemplateFile' value='$(tempRootDir)\t4Temp.tt' />
          <add id='tempNetStatFile' value='$(tempRootDir)\netstat.txt' />
          <add id='tempWmiFile' value='$(tempTextDir)\wmi.txt' />
        </temps>
        <tools>
          <x64>
            <add id='x64SvcUtilTool' value='$(net461SdkTools)\x64\svcUtil.exe' />
            <add id='x64IldasmTool' value='$(net461SdkTools)\x64\ildasm.exe' />
            <add id='x64WsdlTool' value='$(net461SdkTools)\x64\wsdl.exe' />
            <add id='x64ClrVerTool' value='$(net461SdkTools)\x64\clrver.exe' />
            <add id='x64XsdExeTool' value='$(net461SdkTools)\x64\xsd.exe' />
            <add id='x64CdbTool' value='$(binX64RootDir)\debug\cdb.exe' />
            <add id='x64TListTool' value='$(binX64RootDir)\debug\tlist.exe' />
            <add id='x64SymChkTool' value='$(binX64RootDir)\debug\symChk.exe' />
            <add id='x64DumpbinTool' value='$(binX64RootDir)\vs10\dumpbin.exe' />
            <add id='x64SqlCmdTool' value='$(binX64RootDir)\sqlcmd\SQLCMD.EXE' />
            <add id='x64MdbgTool' value='$(binX64RootDir)\v4.0\Mdbg.exe' />
          </x64>
          <x86>
            <add id='x86IldasmTool' value='$(net461SdkTools)\ildasm.exe' />
            <add id='x86SqlMetalTool' value='$(net461SdkTools)\sqlmetal.exe' />
            <add id='x86SvcUtilTool' value='$(net461SdkTools)\svcUtil.exe' />
            <add id='x86WsdlTool' value='$(net461SdkTools)\wsdl.exe' />
            <add id='x86CdbTool' value='$(binX86RootDir)\debug\cdb.exe' />
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
          <favicon value='$(myPsHome)\favicon.ico' />
        </tools>
        <extensions>
          <code-files><![CDATA[ada adb adml admx ads as asa asax ascx asm asmx asp aspx bas bat bsh c cbd cbl cc cdb cdc cls cmd cob cpp cs cshtml css cxx dsr dtd f f2k f90 f95 for frm fs fsx fxh g4 gv h hpp hs hta htm html hxx idl inc java js json kml las lhs lisp lsp lua m master ml mli mx nt pas php php3 phtml pl plx pm  ps ps1 psm1 py pyw r rb rbw rpx scm scp sep sh shtm shtml smd sml sql ss st tcl tex thy tt vb vbhtml vbs vrg wsc wsdl wsf xaml xhtml xlf xliff xsd xsl xslt xsml xst xsx]]></code-files>
          <config-files><![CDATA[acl aml cfg config content csproj cva database din dsw dtproj dtsx dun ecf ecf edmx fcc fsproj gpd h1k hkf inf ini isp iss manifest nuspec obe overrideTasks pdm pp props ps1xml psc1 psd1 rsp sam sch sitemap sln stp svc targets tasks testsettings theme user vbp vbproj vdproj vsmdi xbap xml xrm-ms]]></config-files>
          <binary-files><![CDATA[001 002 003 004 005 006 007 008 009 010 7z aas acm addin adm am amx ani apk apl aps aux avi ax bak bcf bcm bin bmp bpd bpl browser bsc btr bud cab cache camp cap cat cch ccu cdf-ms cdmp chk chm chr chs cht clb cmb cmdline cmf com comments compiled compositefont cov cpa cpl cpx crmlog crt csd ctm cty cur cw dat data db dcf dcr default delete dem desklink devicemetadata-ms diagpkg dic dir dll dlm dls dmp dnl doc docx drv ds dts dub dvd dvr-ms dxt ebd edb efi emf err ess etl ev1 ev2 ev3 evm evtx ex_ exe exp fe fon ftl fx gdl gif gmmp grl grm gs_4_0 h1c h1s h1t hdr hex hit hlp hpi hpx iad icc icm ico id idb idx iec if2 ilk imd ime inf_loc ins inx ipa ird jar jmx jnt job jpeg jpg jpn jrs jtp kor ldo lex lg1 lg2 lib library-ms lng lnk log lrc lts lxa mac man map mapimail mdb mdbx mfl mib mid mllr mni mof mp3 mp4 mpg msc msi msm msstyles mst msu mui mum mzz ncb ndx ngr nlp nls nlt ntf nupkg obj ocx olb old opt out pch pdb pdf phn pkc plugin pnf png ppd pptx prm prof propdesc prq prx ps_2_0 ps_4_0 psd ptxml pub que rat rdl reg res resources resx rld rll rom rpo rs rtf s3 sbr scc scr sdb sdi ses shp smp sqm ssm stl swf sys t4 tag tar.gz tbl tbr tha tif tiff tlb tmp toc tpi trie1 tsp ttc ttf tts tx_ txt uaq uce udt uni uninstall unt url vch vdf ver vp vs_1_1 vs_4_0 vsd vspscc wav wdf web wih wim win32manifest wma wmf wmv wmz wtv wwd x32 xex xlb xls xlsx zfsendtotarget zip]]></binary-files>
        </extensions>
      </files>
      <directories>
        <search-exclusions><![CDATA[bin obj Interop TestResults _svn .svn _ReSharper _TeamCity .git .nuget .vs lib build dist packages __pycache__]]></search-exclusions>
        <temps>
          <add id='tempRootDir' value='$(myPsHome)\temp' />
          <add id='tempProcsDir' value='$(tempRootDir)\procs' />
          <add id='tempCodeDir' value='$(tempRootDir)\code' />
          <add id='tempTextDir' value='$(tempRootDir)\text' />
          <add id='tempDebugsDir' value='$(tempRootDir)\debug' />
          <add id='tempGraphDir' value='$(tempRootDir)\graph' />
          <add id='tempSvcUtilDir' value='$(tempRootDir)\svcUtil' />
          <add id='tempWsdlDir' value='$(tempRootDir)\wsdl' />
          <add id='tempHbmDir' value='$(tempRootDir)\hbm' />
          <add id='tempBinDir' value='$(tempRootDir)\tempBin' />
          <add id='tempJavaSrcDir' value='$(tempRootDir)\java\src' />
          <add id='tempJavaBuildDir' value='$(tempRootDir)\java\build' />
          <add id='tempJavaDistDir' value='$(tempRootDir)\java\dist' />
          <add id='tempJavaArchiveDir' value='$(tempRootDir)\java\archive' />
          <add id='tempCalendarDir' value='$(tempRootDir)\Calendar' />
          <add id='tempHttpAppDomainDir' value='$(tempRootDir)\httpAppDomain' />
          <add id='tempTsvCsvDir' value='$(tempRootDir)\tsvCsv' />
        </temps>
        <bins>
          <add id='binRootDir' value='$(myPsHome)\bin' />
          <add id='net461SdkTools' value='$(%ProgramFiles(x86)%)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools' />
          <add id='binX64RootDir' value='$(binRootDir)\amd64' />
          <add id='binX86RootDir' value='$(binRootDir)\x86' />
          <add id='binJavaRootDir' value='$(binRootDir)\java' />
          <add id='binT4TemplatesDir' value='$(binRootDir)\java' />
          <add id='binPhpRootDir' value='$(binRootDir)\php' />
          <add id='binDataRootDir' value='$(binRootDir)\Data\Source' />
        </bins>
      </directories>
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
      <switches>
        <add id='switchPrintWebHeaders' value='True' />
        <add id='switchSqlCmdHeadersOff' value='True' />
        <add id='switchSqlFiltersOff' value='True' />
        <add id='switchSupressNpp' value='False' />
      </switches>
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
    </config>
  </shared>
</no-future>

";

#endregion
    }
}
