try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("ssr",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("c2cb",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("cfcb",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-IniFile",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Replace-Content",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Add-VsProjBinRef",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Add-Log4NetToConfig",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Replace-AppSettingsToTransform",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Invoke-CscExeSingleFile",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
#=================================================
set-alias ss    select-string
set-alias ssr   Select-StringRecurse
#=================================================

function Copy-ToClipBoard
{
    Param
    (
        [Parameter(Mandatory=$true,ValueFromPipeline=$true)]
        [string] $val
    )
    Process{
        [System.Windows.Forms.Clipboard]::SetText($val)
    }
}
function Copy-FromClipBoard
{
    Param
    (
    )
    Process{
        return [System.Windows.Forms.Clipboard]::GetText()
    }
}

Set-Alias c2cb Copy-ToClipBoard
Set-Alias cfcb Copy-FromClipBoard

<#
    .synopsis
    Returns an Ini files as an instance of INI.IniFileName from the ReadIni.dll.
    
    .Description
    Returns an instance of an Ini file from the custom dll.
    Within the Ini object make calls according to these signatures:
     GetSectionNames()
     GetEntryNames(string section)
     GetEntryValue(string section, string entry)
    
    .parameter $Path
    Path to an Ini file
    
    .example
    C:\PS>$myIni = Get-IniFile C:\MyConfig\yeOldeTymeIni.ini
    C:\PS>$myIni.GetSectionName() | % {$sec = $_; $myIni.GetEntryName($sec) | % {$myIni.GetEntryValues($sec,$_)}}
    
    .outputs
    INI.IniFileName
    
#>
function Get-IniFile
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path
    )
    Process
    {	$ini = New-Object NoFuture.Read.IniFileName $Path
	   return $ini
    }
}


<#
    .SYNOPSIS
    Invokes Select-String cmdlet on every file in the directory and child directories thereof.
    
    .DESCRIPTION
    Performs a Select-String cmdlet on each file recursively and will filter to only 
    those files which are considered code (see NoFuture.Shared.CodeExtensions)
    
    .PARAMETER Pattern
    The regex pattern that is used for the Select-String cmdlet.
    
    .PARAMETER Path
    Optional directory at which to begin the recursive Select-String, default to
    the working directory

    .PARAMETER CurrentDirOnly
    Optional to limit the Select-String to only text files in this directory

    .PARAMETER IncludeConfig
    Optional to include configuration file types (see NoFuture.Shared.ConfigExtensions)
    
    .EXAMPLE
    C:\PS>Select-StringRecurse -Pattern "^an important message.*$" -Path C:\AllTheMessages
    
    .EXAMPLE
    C:\PS>Select-StringRecurse "^.*put this somewhere.*$"  #will search C:\ since that the current dir
    
    .OUTPUTS
    System.Array
    
#>
function Select-StringRecurse
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Pattern,
        [Parameter(Mandatory=$false,position=1)]
        [string] $SearchPath,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $CurrentDirOnly,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $IncludeConfig
    )
    Process
    {
      
      $ssrExt = @()
      $ssrExt += $global:codeExtensions
      if ($IncludeConfig)
      {
        $ssrExt += $global:configExtension
      }
      
      if($SearchPath -eq $null){$SearchPath = ".\"}  

      Write-Progress -activity $("Regex : `'" + $Pattern + "`'.") -status $("Filter : `'" + $_ + "`'");

      if($CurrentDirOnly){
          ls -Path $SearchPath -Filter *.* | ? { 
            ($ssrExt -contains ("*{0}" -f $_.Extension))  } | % {
              Write-Progress -activity $("Searching `'" + $_.FullName + "`'.") -status $("Regex : `'" + $Pattern + "`'"); 
              ss -Pattern $Pattern -Path $_.FullName
           }
      }
      else{
          ls -r -Path $SearchPath | ? { 
            (-not $_.PSIsContainer) -and ($ssrExt -contains ("*{0}" -f $_.Extension)) -and ($_.FullName -notlike "*\bin\*") -and ($_.FullName -notlike "*\obj\*")  } | % {
              Write-Progress -activity $("Searching `'" + $_.FullName + "`'.") -status $("Regex : `'" + $Pattern + "`'"); 
              ss -Pattern $Pattern -Path $_.FullName
           }
      }
    }
}


<#
    .synopsis
    Static swap of one string for another in all files in the specified directory and child directories thereof.
    
    .Description
    Performs a simple switch of one string value for another.  The input string is not
    a regex pattern.  The function simply used the powershell -replace operator for every
    line of the given file content.
    
    Regardless if a match is found within a given file the file in-scope with have the same
    content but its last-write time will be changed to now.
    
    .parameter FindText
    The text that is being targeted for replacement.
    
    .parameter ReplaceWith
    The text that will replace the target in the given file.
    
    .parameter Path
    The startin directory - every child directory will be affected the same as this one.

    .example
    C:\PS>Replace-Content -FindText "Stand" -ReplaceWith "Sit" -Path C:\AllMyFiles
    
    .example
    C:\PS>Replace-Content "you'll" "all amoung you"  #every file on the C: drive is about to be written
    
    .outputs
    null
    
#>
function Replace-Content
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $FindText,
        [Parameter(Mandatory=$true,position=1)]
        [string] $ReplaceWith,
        [Parameter(Mandatory=$false,position=2)]
        [string]$Path
    )
    Process
    {
        ls -Recurse -Path $Path -Exclude $global:excludeExtensions | ? {-not $_.PSIsContainer -and 
                                                                        -not($_.FullName.Contains("\bin\")) -and 
                                                                        -not($_.FullName.Contains("\obj\")) -and
                                                                        -not($_.FullName.Contains("\Interop\")) -and
                                                                        -not($_.FullName.Contains("\TestResults\")) -and
                                                                        -not($_.FullName.Contains("\_svn\")) -and
                                                                        -not($_.FullName.Contains("\.svn\")) -and
                                                                        -not($_.FullName.Contains("\_ReSharper")) -and
                                                                        -not($_.FullName.Contains("\_TeamCity"))} | % {
    		Write-Progress -activity $("Replace : '{0}' with {1}." -f $FindText,$ReplaceWith) -status $("Location : '{0}'." -f $_.FullName)
            $content = [System.IO.File]::ReadAllText($_.FullName)
            $content = $content.Replace($FindText, $ReplaceWith)
            [System.IO.File]::WriteAllText($_.FullName, $content)
        }
    }
}

<#
    .sysnopsis
    Iterates a binary file byte-by-byte looking for .NET 'BSJB' sequence
    
    .description
    Hack to determine if a binary is a .NET type or not, looks for a 
    sequence of ASCII chars equal to 'BSJB' and finding it 
    
    .parameter Path
    A binary working directory like 'bin' that is generated by VS 
    during a build.
    
    .example
    C:\PS>Get-AssemblyDotNetversion -Path C:\Projects\MyStuff\bin\SomeWeridDll.dll
    
    .outputs
    string
#>
function Get-AssemblyDotNetversion
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [string]$Path
    )
    Process
    {
        #is a valid path
        if(-not(Test-Path $Path)){return ""}
        
        #is a binary extension
        if(@(".dll",".exe") -notcontains (Get-Item $Path).Extension){return ""}
        
        #get files byte contents
        $b = [System.IO.File]::ReadAllBytes($Path)
        for($i=0;$i-lt$b.Length;$i++)
        {
            #don't peek past 50%
            if($i+$i-ge $b.Length){return ""}
        
            #check for buffer overflow
            if(($b[$i] -eq 0x42) -and ($i+4-lt$b.Length))
            {
                
                $clrHeaderCanidate = [System.Text.Encoding]::UTF8.GetString(@($b[$i],$b[$i+1],$b[$i+2],$b[$i+3]))
            
                if($clrHeaderCanidate -eq "BSJB")
                {
                    
                    $netVerOffSet = $i+4+12
                    
                    $netVerOffSetEnd = $netVerOffSet + $global:net40.Length
                    
                    #check for buffer overflow
                    if($netVerOffSetEnd-lt$b.Length)
                    {
                        #array splice
                        $clrVersion = [System.Text.Encoding]::UTF8.GetString($b[$netVerOffSet..($netVerOffSetEnd-1)])
                        if($clrVersion -match ("{0}.*" -f $global:net11))
                        {
                            return $global:net11
                        }
                        elseif($clrVersion -match ("{0}.*" -f $global:net20))
                        {
                            return $global:net20
                        }
                        elseif($clrVersion -match ("{0}.*" -f $global:net35))
                        {
                            return $global:net35
                        }
                        elseif($clrVersion -match ("{0}.*" -f $global:net40))
                        {
                            return $global:net40
                        }
                    }#end version match 
                }#end test is match
            }#end buffer-overflowcheck
         }#end eachByte
         
         return ""#no match
    }
}

<#
    .SYSNOPSIS
    Adds the required log4net entries into a .NET Configuration File.
    
    .DESCRIPTION
    A very limited cmdlet which is intended to be used for adding 
    log4net to a batch of projects.  This cmdlet will cause problems
    if the compiled binary does not, likewise, have a reference to 
    the log4net assembly.

    This cmdlet is more like useable notes.  There are four parts to 
    wiring-in log4net to a .NET project.  One, add the log4net assembly
    as a reference to the project.  Two, add the config sections to the
    assembly's configuration file.  Three, add a call to log4net's static
    'log4net.Config.XmlConfigurator.Configure()' somewhere in a Main or 
    Application_Start method, and four, actually use the log4net logger 
    in some capacity.  This cmdlet only performs step two.

    The cmdlet will check for the log4net configuationSection already
    being present and in such a case the cmdlet simply quits - printing a
    message that, 
    'log4net configuration section already present...'

    The entries within a .NET Configuration file are not specific to 
    a particular version so this should work with any log4net version
    assuming that the configuration entry does not change.
    
    .PARAMETER ConfigPath
    The full path to the .NET configuration file.
    
    .PARAMETER LogFilesDir
    A directory path where log4net should write its log file.  This value gets
    added to the log4net configuration section.

    .PARAMETER LogFileName
    Optional parameter to directly specify what the name of the log file.
    If its omitted then the name is derived from the parent folder's name.

    .EXAMPLE
    C:\PS> $myConfig = "C:\Projects\Windows\MyProject\App.config"
    C:\PS> $myLogFilesAt = "C:\Projects\Logs"
    C:\PS> Add-Log4NetToConfig -ConfigPath $myConfig -LogFilesDir $myLogFilesAt
Done! New content added to 'C:\Projects\Windows\MyProject\App.config'

        log4net.Config.XmlConfigurator.Configure();

        public const string LOGGER_NAME = "MyProjectErrorLog";
        public static void LogException(string msg, System.Exception ex)
        {
            var logger = log4net.LogManager.GetLogger(LOGGER_NAME);
            if (logger == null)
                return;
            logger.Error(msg, ex);
        }
#>
function Add-Log4NetToConfig
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $ConfigPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $LogFilesDir,
        [Parameter(Mandatory=$false,position=2)]
        [string] $LogFileName
    )
    Process
    {
        if(-not (Test-Path $ConfigPath)){
            Write-Host "Bad Path or File Name at '$ConfigPath'" -ForegroundColor Yellow
            break;
        }

        $projImplName = (Split-Path -Path (Split-Path -Path $ConfigPath -Parent) -Leaf)

        $appenderName = ("{0}ErrorLog" -f $projImplName.Replace(".","_"))

        if([string]::IsNullOrWhiteSpace($LogFileName)){
            $logFileName = ("{0}_ExceptionLog.txt" -f $projImplName)
        }
        else{
            $LogFileName = [NoFuture.Util.Etc]::SafeFilename($LogFileName)
        }

        $logFileFullName = (Join-Path $LogFilesDir $logFileName)

        $configContent = [xml](Get-Content $ConfigPath)

        #test for log4net already being present
        if($configContent.SelectSingleNode("/configuration/configSections/section[@name='log4net']") -ne $null -or
           $configContent.SelectSingleNode("/configuration/log4net") -ne $null){
            Write-Host "log4net configuration section already present in '$ConfigPath'" -ForegroundColor Yellow
            break;
        }

        $rootNode = $configContent.SelectSingleNode("/configuration")
        $configSectionsNode = $configContent.SelectSingleNode("//configSections")

        #test for presence of a config sections 
        if($configSectionsNode -eq $null){
            $configSectionsNode = $configContent.CreateElement("configSections")
            $rootNode.AppendChild($configSectionsNode)
        }

        $sectionNode = $configContent.CreateElement("section");
        $sectionNameAttr = $configContent.CreateAttribute("name");
        $sectionNameAttr.Value = "log4net"
        $sectionTypeAttr = $configContent.CreateAttribute("type");
        $sectionTypeAttr.Value = "log4net.Config.Log4NetConfigurationSectionHandler,log4net"

        $sectionNode.Attributes.Append($sectionNameAttr)
        $sectionNode.Attributes.Append($sectionTypeAttr)

        $configSectionsNode.AppendChild($sectionNode)

        $log4netNode = $configContent.CreateElement("log4net")

        #other appenders SmtpPickupDirAppender, SmtpAppender

        $log4netNode.InnerXml =  @"

    <appender name="$appenderName" type="log4net.Appender.RollingFileAppender">
      <file value="$logFileFullName" />
      <appendToFile value="true" />
      <rollingStyle value="Composite" />
      <datePattern value="yyyyMMdd" />
      <maxSizeRollBackups value="-1" />
      <maximumFileSize value="1MB" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %level - %message%newline%exception---%newline" />
      </layout>
      <filter type="log4net.Filter.LevelRangeFilter">
        <levelMin value="ERROR" />
        <levelMax value="FATAL" />
      </filter>
    </appender>
    <root>
      <level value="DEBUG" />
      <appender-ref ref="$appenderName" />
    </root>

"@
        $rootNode.AppendChild($log4netNode)

        #write it back out to file with pretty-printing
        $myXmlWriter = New-Object System.Xml.XmlTextWriter($ConfigPath, [System.Text.Encoding]::UTF8)
        $myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
        $configContent.WriteContentTo($myXmlWriter)
        $myXmlWriter.Flush()
        $myXmlWriter.Dispose()

        Write-Host "Done! New content added to '$ConfigPath'" -ForegroundColor Yellow

        Write-Host @"

        log4net.Config.XmlConfigurator.Configure();

"@ -ForegroundColor Yellow

        Write-Host @"
        public const string LOGGER_NAME = "$appenderName";
        public static void LogException(string msg, System.Exception ex)
        {
            var logger = log4net.LogManager.GetLogger(LOGGER_NAME);
            if (logger == null)
                return;
            logger.Error(msg, ex);
        }
"@ -ForegroundColor Yellow

    }
}

<#
    .SYSNOPSIS
    Adds or updates a reference to a project file.

    .DESCRIPTION
    Either adds a new binary reference to a project file
    or updates and existing reference with the values 
    passed in.  For existing references the cmdlet will
    search on just the first portion of an Assembly Name.
    This is typically the part before the 'Version' portion.
    
    .PARAMETER Path
    The direct path to Visual Studio Project file.
    This may be a VB.NET, C# or F# project.
    
    .PARAMETER AssemblyFullName
    The fully qualified Assembly name 
    See http://msdn.microsoft.com/en-us/library/k8xx4k69(v=vs.110).aspx 

    .PARAMETER HintPath
    This value is written to the text of HintPath for the 
    references.  May include global variables in the form
    of $(YOUR_VAR_HERE) which Visual Studio will resolve as 
    a relative path.

    .EXAMPLE
    C:\PS> $myAsmName = "log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL"
    C:\PS> $myHintPath = '$(PROJ_CODE_ROOT)\SharedBinaries\log4net.dll'
    C:\PS> $myProjFile = "C:\Projects\31g\trunk\temp\code\.NET_2005_DllLib_ProjFile.csproj"
    C:\PS> Add-VsProjBinRef -Path $myProjFile -AssemblyFullName $myAsmName -HintPath $myHintPath

#>
function Add-VsProjBinRef
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$Path,
        [Parameter(Mandatory=$true,position=1)]
        [string]$AssemblyFullName,
        [Parameter(Mandatory=$true,position=2)]
        [string]$HintPath
    )
    Process
    {
        $dotNetProjNs = [NoFuture.Read.Vs.ProjFile]::DotNetProjXmlNs

        #validate the inputs
        if(-not (Test-Path $Path)){
            Write-Host "The Path '$Path' does not exist." -ForegroundColor Yellow
            break;
        }

        #get the proj file as XmlDoc with namespace
        [System.Xml.XmlNamespaceManager] $nsMgr = $null
        $proj = [NoFuture.Read.Vs.ProjFile]::GetVsProjXml($Path, [ref] $nsMgr)
        $vs = [NoFuture.Read.Vs.ProjFile]::NS

        #scope in on the first ItemGroup node
        $itemGroupNodes = $proj.SelectNodes("//${vs}:ItemGroup",$nsMgr);
        
        #will not work for VS 2003 proj format
        if($itemGroupNodes.Count -eq 0){
            Write-Host "The required child node named ItemGroup was not found." -ForegroundColor Yellow
            break;
        }

        if([string]::IsNullOrWhiteSpace($AssemblyFullName)){
            Write-Host "The assembly name '$AssemblyFullName' is invalid" -ForegroundColor Yellow
            break;
        }
        #validate the assembly name is in the std .NET format
        $asmNameParts = New-Object NoFuture.Util.TypeName($AssemblyFullName)
        if([string]::IsNullOrWhiteSpace($asmNameParts.AssemblyName)){
            Write-Host "Could not parse the assembly name" -ForegroundColor Yellow
            break;
        }

        #scope in on the first ItemGroup node
        $refItemGroup = $proj.SelectNodes("//${vs}:ItemGroup/${vs}:Reference",$nsMgr)
        $firstGroupNode = $refItemGroup[0].ParentNode

        #search for a reference without respect to version
        $asmSimpleName = $asmNameParts.AssemblyName;
        $referenceNode = $proj.SelectSingleNode("//${vs}:ItemGroup/${vs}:Reference[contains(@Include,'$asmSimpleName')]", $nsMgr)

        #if its not present then create it now
        if($referenceNode -eq $null){
            $referenceNode = $proj.CreateElement("Reference",$dotNetProjNs)
            $includeAttr = $proj.CreateAttribute("Include")
            $includeAttr.Value = $AssemblyFullName
            $referenceNode.Attributes.Append($includeAttr)
            $specificVerNode = $proj.CreateElement("SpecificVersion",$dotNetProjNs)
            $specificVerNode.InnerText = "False"
            $hintPathNode = $proj.CreateElement("HintPath",$dotNetProjNs)
            $hintPathNode.InnerText = $HintPath

            $doNotDisplay = $referenceNode.AppendChild($specificVerNode)
            $doNotDisplay = $referenceNode.AppendChild($hintPathNode)

            $doNotDisplay = $firstGroupNode.InsertBefore($referenceNode,$firstGroupNode.FirstChild)
        }
        else{
            
            $includeAttr = $referenceNode.Attributes["Include"]
            $includeAttr.Value = $AssemblyFullName

            $specificVerNode = $proj.SelectSingleNode("//${vs}:ItemGroup/${vs}:Reference[contains(@Include,'$asmSimpleName')]/${vs}:SpecificVersion", $nsMgr);
            if($specificVerNode -eq $null){
                $specificVerNode = $proj.CreateElement("SpecificVersion",$dotNetProjNs)
                $doNotDisplay = $referenceNode.AppendChild($specificVerNode)
            }

            $specificVerNode.InnerText = "False"

            $hintPathNode = $proj.SelectSingleNode("//${vs}:ItemGroup/${vs}:Reference[contains(@Include,'$asmSimpleName')]/${vs}:HintPath", $nsMgr);
            if($hintPathNode -eq $null){
                $hintPathNode = $proj.CreateElement("HintPath",$dotNetProjNs)
                $doNotDisplay = $referenceNode.AppendChild($hintPathNode)
            }
            $hintPathNode.InnerText = $HintPath

        }

        #write it back out to file with pretty-printing
        $myXmlWriter = New-Object System.Xml.XmlTextWriter($Path, [System.Text.Encoding]::UTF8)
        $myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
        $proj.WriteContentTo($myXmlWriter)
        $myXmlWriter.Flush()
        $myXmlWriter.Dispose()
    }
}

<#
    .SYSNOPSIS
    Adds a new compile item to a VS proj file

    .DESCRIPTION
    Will add the path of NewItemPath as a compile item
    to the project file given at Path.
    
    .PARAMETER Path
    The Path to a VS project file.
    
    .PARAMETER NewItemPath
    This value will be reduced to a relative path
    if it is contained under Path

#>
function Add-VsProjCompileItem
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$Path,
        [Parameter(Mandatory=$true,position=1)]
        [string]$NewItemPath
    )
    Process
    {
        #validate the inputs
        if(-not (Test-Path $Path)){
            Write-Host "The Path '$Path' does not exist." -ForegroundColor Yellow
            break;
        }

        $dotNetProjNs = [NoFuture.Read.Vs.ProjFile]::DotNetProjXmlNs
        $vs = [NoFuture.Read.Vs.ProjFile]::NS
        [System.Xml.XmlNamespaceManager] $nsMgr = $null
        $proj = [NoFuture.Read.Vs.ProjFile]::GetVsProjXml($Path, [ref] $nsMgr)

        $dnd = [NoFuture.Util.Etc]::ToRelPath($Path, [ref] $NewItemPath)

        $projFiles = $proj.SelectNodes("//${vs}:ItemGroup/${vs}:Compile", $nsMgr)

        $newItemPresent = $projFiles | ? {$_.Attributes["Include"].Value -eq $NewItemPath} | Select-Object -First 1
        if($newItemPresent -ne $null){
            break;
        }

        $lastNode = $projFiles | Select-Object -Last 1

        $NewItemNode = $proj.CreateElement("Compile",$dotNetProjNs)
        $includeAttr = $proj.CreateAttribute("Include")
        $includeAttr.Value = $NewItemPath
        $dnd = $NewItemNode.Attributes.Append($includeAttr)

        
        $dnd = $projFiles[0].ParentNode.InsertAfter($NewItemNode, $lastNode)
        
        $myXmlWriter = New-Object System.Xml.XmlTextWriter($Path, [System.Text.Encoding]::UTF8)
        $myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
        $proj.WriteContentTo($myXmlWriter)
        $myXmlWriter.Flush()
        $myXmlWriter.Dispose()

    }
}
<#
    .SYSNOPSIS
    Replaces value of a .NET Configuration file's appSettings.

    .DESCRIPTION
    The cmdlet iterates the appSettings child nodes looking
    for any 'add' node whose value match one of the regex keys
    found in Regex2Value hashtable.  Having found a match the 
    orignal 'value' is extracted to a transformation while the 
    existing 'value' is replaced.
    
    .PARAMETER ConfigFile
    Full path to a .NET Configuration file.
    
    .PARAMETER Regex2Value
    A hashtable of key-value pairs where the keys are regex patterns
    used to match against the appSetting/add[@value] 

    .PARAMETER TransformFileName
    The output file where the original values are transferred.

    .PARAMETER SwapConnStr
    Optional switch which tells the cmdlet to swap out the 
    'connectionString' values with the value assigned to 
    NoFuture.Shared.Constants.SqlServerDotNetConnString.

    .EXAMPLE
    C:\PS> $testConfigFile ="C:\Projects\MyProject\App.config"
    C:\PS> $testReplacements = @{$myRegex.WindowsPath = "C:\Projects\Temp\"}
    C:\PS> $testNewTrans = "App.Production.config"
    C:\PS> Replace-AppSettingsToTransform -ConfigFile $testConfigFile -Regex2Value $testReplacements -TransformFileName $testNewTrans
#>
function Replace-AppSettingsToTransform
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $ConfigFile,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TransformFileName,
        [Parameter(Mandatory=$false,position=2)]
        [hashtable] $Regex2Value,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $SwapConnStr
    )
    Process
    {
        if(-not (Test-Path $ConfigFile)){
            Write-Host "Bad Path or File Name at '$ConfigFile'" -ForegroundColor Yellow
            break;
        }

        $configDir = Split-Path -Path $ConfigFile -Parent

        $transformOutFile = Join-Path $configDir $TransformFileName

        if(Test-Path $transformOutFile){
            Write-Host "The file '$transformOutFile' already exist." -ForegroundColor Yellow
            break;
        }

        #read and scope-in on the existing config file
        $configContent = [xml](Get-Content $ConfigFile)

        $appSettingsNode = $configContent.SelectSingleNode("//appSettings")

        if($appSettingsNode -eq $null -or -not $appSettingsNode.HasChildNodes){
            Write-Host "'$ConfigFile' has no <appSettings> node" -ForegroundColor Yellow
            break;
        }

        #
        if($Regex2Value -eq $null){
            $Regex2Value = [NoFuture.Shared.RegexCatalog]::MyRegex2Values
        }

        #begin drafting a new transformation to hold the values prior to these changes
        $xmlWriterSettings = New-Object System.Xml.XmlWriterSettings

        $xmlWriterSettings.Indent = $true;
        $xmlWriterSettings.IndentChars = "  ";
        $xmlWriterSettings.OmitXmlDeclaration = $true;

        $xmlWriter = [System.Xml.XmlWriter]::Create($transformOutFile,$xmlWriterSettings)
        $xmlWriter.WriteStartDocument()
        $xmlWriter.WriteComment(" For more information on using web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=125889 ")
        $xmlWriter.WriteStartElement("configuration")
        $xmlWriter.WriteAttributeString("xmlns","xdt",$null,"http://schemas.microsoft.com/XML-Document-Transform")
        $xmlWriter.WriteStartElement("appSettings")

        #foreach appSetting child node
        :nextNode foreach($cnode in $appSettingsNode.ChildNodes){

            if($cnode.Attributes -eq $null){
                continue nextNode;
            }

            $appValAttr = $cnode.Attributes["value"]
            $appKeyAttr = $cnode.Attributes["key"]

            :nextRegex foreach($regexKey in $Regex2Value.Keys){
                
                $oldAppVal = $appValAttr.Value
                
                $isConnStr = $false

                if( -not ($oldAppVal -cmatch $regexKey)){
                    
                    if(-not $SwapConnStr){
                        continue nextRegex;
                    }

                    #run a acid test on the value being a connection string
                    $partsA = $oldAppVal.Split(";")
                    $partsB = $oldAppVal.Split("=")
                    if($partsA.Length -le 1 -or $partsA.Length -ne $partsB.Length){
                        continue nextRegex;
                    }
                    else {
                        $isConnStr = $true
                    }
                }

                #replace only what matched 
                if($isConnStr){
                    $newAppVal = [NoFuture.Shared.Constants]::SqlServerDotNetConnString
                }
                else{
                    $newAppVal = $oldAppVal.Replace($Matches[0], $Regex2Value[$regexKey])
                }

                Write-Host ("{0} -> {1}" -f $oldAppVal, $newAppVal)

                #update the existing config file with the new value
                $appValAttr.Value = $newAppVal

                #begin drafting the new //appSettings/add node for the tranformation
                $xmlWriter.WriteStartElement("add")
                $xmlWriter.WriteAttributeString("key",$appKeyAttr.Value)
                $xmlWriter.WriteAttributeString("value",$oldAppVal)
                $xmlWriter.WriteAttributeString("Transform","http://schemas.microsoft.com/XML-Document-Transform","SetAttributes")
                $xmlWriter.WriteAttributeString("Locator","http://schemas.microsoft.com/XML-Document-Transform","Match(key)")
                $xmlWriter.WriteEndElement()

                #skip all the other regex keys since connection string match on its own criteria
                if($isConnStr){
                    continue nextNode;
                }
            }
        }

        $xmlWriter.WriteEndElement()#end appSettings

        #swap all the connection strings if so expected
        if($SwapConnStr -and 
           $configContent.SelectSingleNode("//connectionStrings") -ne $null -and 
           [NoFuture.Shared.Constants]::SqlServerDotNetConnString -ne "Server=;Database=;Trusted_Connection=True;")
        {
            $connectionStringsNode = $configContent.SelectSingleNode("//connectionStrings")
            :nextConStr foreach($constr in $connectionStringsNode.ChildNodes){
                if($constr.Attributes -eq $null){
                    continue nextConStr;
                }

                $conStrAttr = $constr.Attributes["connectionString"]
                $conStrNameAttr = $constr.Attributes["name"]

                $oldConStrVal = $conStrAttr.Value
                $newConStrVal = [NoFuture.Shared.Constants]::SqlServerDotNetConnString

                if($conStrAttr.Value -eq $null){
                    continue nextConStr;
                }

                $conStrAttr.Value = $newConStrVal

                $xmlWriter.WriteStartElement("connectionStrings")
                $xmlWriter.WriteStartElement("add")
                $xmlWriter.WriteAttributeString("name",$conStrNameAttr.Value)
                $xmlWriter.WriteAttributeString("connectionString",$oldConStrVal)
                $xmlWriter.WriteAttributeString("Transform","http://schemas.microsoft.com/XML-Document-Transform","SetAttributes")
                $xmlWriter.WriteAttributeString("Locator","http://schemas.microsoft.com/XML-Document-Transform","Match(name)")
                $xmlWriter.WriteEndElement()
                $xmlWriter.WriteEndElement()
            }

        }

        #wcf
        
        if($configContent.SelectSingleNode("//system.serviceModel/client/endpoint") -ne $null){

            $xmlWriter.WriteStartElement("system.serviceModel")

            $svcModelNode = $configContent.SelectNodes("//system.serviceModel/client")

            :nextClient foreach($clientNode in $svcModelNode){

                $xmlWriter.WriteStartElement("client")

                :nextEndpt foreach($endptNode in $clientNode.ChildNodes) {
                    if($endptNode.Attributes -eq $null){
                        continue nextEndpt;
                    }

                    $addrAttr = $endptNode.Attributes["address"]
                    if($addrAttr -eq $null){
                        continue nextEndpt;
                    }

                    if(-not( [System.Uri]::IsWellFormedUriString($addrAttr.Value, [System.UriKind]::RelativeOrAbsolute))){
                        continue nextEndpt;
                    }


                    $addrUri = New-Object System.UriBuilder($addrAttr.Value)
                    $addrUri.Host = "localhost"

                    $xmlWriter.WriteStartElement("endpoint")
                    $xmlWriter.WriteAttributeString("address",$addrAttr.Value)
                    if($endptNode.Attributes["binding"] -ne $null){
                        $xmlWriter.WriteAttributeString("binding",$endptNode.Attributes["binding"].Value)
                    }
                    if($endptNode.Attributes["bindingConfiguration"] -ne $null){
                        $xmlWriter.WriteAttributeString("bindingConfiguration",$endptNode.Attributes["bindingConfiguration"].Value)
                    }
                    if($endptNode.Attributes["contract"] -ne $null){
                        $xmlWriter.WriteAttributeString("contract",$endptNode.Attributes["contract"].Value)
                    }
                    if($endptNode.Attributes["name"] -ne $null){
                        $xmlWriter.WriteAttributeString("name",$endptNode.Attributes["name"].Value)
                    }
                
                    $xmlWriter.WriteAttributeString("Transform","http://schemas.microsoft.com/XML-Document-Transform","SetAttributes")
                    $xmlWriter.WriteAttributeString("Locator","http://schemas.microsoft.com/XML-Document-Transform","Match(name)")
                    $xmlWriter.WriteEndElement()

                    $addrAttr.Value = $addrUri.ToString()

                }
                $xmlWriter.WriteEndElement()
            }

        }

        #close out the transformation
        $xmlWriter.WriteEndElement()#end configuration
        $xmlWriter.Flush()
        $xmlWriter.Close();
        
        #write and close the existing config with changes applied
        $myXmlWriter = New-Object System.Xml.XmlTextWriter($ConfigFile, [System.Text.Encoding]::UTF8)
        $myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
        $configContent.WriteContentTo($myXmlWriter)
        $myXmlWriter.Flush()
        $myXmlWriter.Dispose()        
    }
}

<#
    .SYSNOPSIS
    Invokes csc.exe (ver. 4.0) for a single file compiling into a .dll

    .DESCRIPTION
    Utility method to compile a single .cs file into a binary dll
    
    .PARAMETER CodeFile
    Path to a .cs code file.
    
    .PARAMETER References
    An array of paths to referenced binaries.
#>
function Invoke-CscExeSingleFile
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $CodeFile,
        [Parameter(Mandatory=$false,position=1)]
        [array] $ReferencePaths
    )
    Process
    {
        if(-not (Test-Path $CodeFile)){
            Write-Host "No such File at '$CodeFile'" -ForegroundColor Yellow
            break;
        }

        if(-not (Get-Item $CodeFile).Extension -eq ".cs"){
            Write-Host "The '$CodeFile' does not have a .cs extension" -ForegroundColor Yellow
            break;
        }

        $refCmd = ""
        if($ReferencePaths -ne $null -and $ReferencePaths.Length -gt 0){
            $ReferencePaths | ? {Test-Path $_} | % {
                $fp = Resolve-Path $_
                $refCmd += ("/reference:`"{0}`" " -f $fp)
            }
        }

        $CodeFile = (Resolve-Path $CodeFile).Path
        $dir = Split-Path $CodeFile -Parent
        $csFile = Split-Path $CodeFile -Leaf
        $name = [System.IO.Path]::GetFileNameWithoutExtension($CodeFile)
        $cscCompiler = (Join-Path $global:net40Path $global:cscExe)
        $svcUtilCscCmd = ("& {0} /t:library /debug /nologo /out:'{1}\{2}.dll' {3} '{1}\{2}.cs'" -f $cscCompiler, $dir,$name,$refCmd)
        Write-Host $svcUtilCscCmd -ForegroundColor Yellow
        Invoke-Expression -Command $svcUtilCscCmd
    }
}