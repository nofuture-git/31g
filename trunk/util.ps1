try{
if(-not [NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("ssr",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("c2cb",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("cfcb",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-IniFile",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Replace-Content",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Add-VsProjBinRef",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Add-VsProjCompileItem",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Remove-VsProjCompileItem",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Add-Log4NetToConfig",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Replace-ExeConfigValues",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Invoke-CscExeSingleFile",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
#=================================================
set-alias ss    select-string
set-alias ssr   Select-StringRecurse
#=================================================

function Set-ClipBoardContent
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
function Get-ClipBoardContent
{
    Param
    (
    )
    Process{
        return [System.Windows.Forms.Clipboard]::GetText()
    }
}

Set-Alias c2cb Set-ClipBoardContent
Set-Alias cfcb Get-ClipBoardContent

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
    those files which are considered code (see NoFuture.NfConfig.CodeExtensions)
    
    .PARAMETER Pattern
    The regex pattern that is used for the Select-String cmdlet.
    
    .PARAMETER Path
    Optional directory at which to begin the recursive Select-String, default to
    the working directory

    .PARAMETER CurrentDirOnly
    Optional to limit the Select-String to only text files in this directory

    .PARAMETER IncludeConfig
    Optional to include configuration file types (see NoFuture.NfConfig.ConfigExtensions)
    
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
          ls -Path $SearchPath -File | ? { 
            ($ssrExt -contains ("*{0}" -f $_.Extension))  } | % {
              Write-Progress -activity $("Searching `'" + $_.FullName + "`'.") -status $("Regex : `'" + $Pattern + "`'"); 
              ss -Pattern $Pattern -Path $_.FullName
           }
      }
      else{
          ls -r -Path $SearchPath -File | ? { 
            ($ssrExt -contains ("*{0}" -f $_.Extension)) -and -not ([NoFuture.Util.NfPath]::ContainsExcludeCodeDirectory($_.FullName))  } | % {
              Write-Progress -activity $("Searching `'" + $_.FullName + "`'.") -status $("Regex : `'" + $Pattern + "`'"); 
              ss -Pattern $Pattern -Path $_.FullName
           }
      }
    }
}


<#
    .SYNOPSIS
    Static swap of one string for another in all files in the specified directory and child directories thereof.
    
    .DESCRIPTION
    Performs a simple switch of one string value for another.  The input string is not
    a regex pattern.  The function simply used the powershell -replace operator for every
    line of the given file content.
    
    Regardless if a match is found within a given file the file in-scope with have the same
    content but its last-write time will be changed to now.
    
    .PARAMETER FindText
    The text that is being targeted for replacement.
    
    .PARAMETER ReplaceWith
    The text that will replace the target in the given file.

    .PARAMETER RegexMatchPattern
    Optional regex pattern to apply to reach file name where only
    those which match will be operated upon.
    
    .PARAMETER Path
    The startin directory - every child directory will be affected the same as this one.

    .EXAMPLE
    C:\PS>Replace-Content -FindText "Stand" -ReplaceWith "Sit" -Path C:\AllMyFiles
    
    .EXAMPLE
    C:\PS>Replace-Content "you'll" "all amoung you"  #every file on the C: drive is about to be written
    
    .OUTPUTS
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
        [string]$Path,
        [Parameter(Mandatory=$false,position=3)]
        [string] $RegexMatchPattern
    )
    Process
    {
        Write-Progress -activity "Finding all target files" -status "Searching" -PercentComplete 1

        $targets = (ls -Recurse -Path $Path -Exclude $global:excludeExtensions -File | % {$_.FullName})

        if(-not([string]::IsNullOrWhiteSpace($RegexMatchPattern))){
            $targets = ($targets | ? {$_ -match $RegexMatchPattern})
        }
        $total = $targets.Length
        $counter = 0;
        :nextTarget foreach($filePath in $targets){
            $counter += 1
            if([NoFuture.Util.NfPath]::ContainsExcludeCodeDirectory($filePath)){continue nextTarget;}

    		Write-Progress -activity $("Replace : '{0}' with {1}." -f $FindText,$ReplaceWith) -status $("Location : '{0}'." -f $filePath) -PercentComplete ([NoFuture.Util.Etc]::CalcProgressCounter($counter, $total))
            $content = [System.IO.File]::ReadAllText($filePath)
            $content = $content.Replace($FindText, $ReplaceWith)
            [System.IO.File]::WriteAllText($filePath, $content, [System.Text.Encoding]::UTF8)
        }
    }
}

<#
    .SYSNOPSIS
    Adds a log4net appender into a .NET Configuration File.
    
    .DESCRIPTION
    A very limited cmdlet which is intended to be used for adding 
    log4net appender to a .NET config file. 

    This cmdlet should function on any .NET configuration file.
    There are four parts to wiring-in log4net to a .NET project.  
    One, add the log4net assembly as a reference to the project.  
    Two, add the config sections to the assembly's configuration file.  
    Three, add a call to log4net's static 'log4net.Config.XmlConfigurator.Configure()' 
    somewhere in a Main or Application_Start method, 
    and four, actually use the log4net logger in some capacity.  
    This cmdlet only performs step two.
    
    .PARAMETER ConfigPath
    The full path to the .NET configuration file. 
    
    .PARAMETER AppenderXml
    The appender XML to be added to the log4net section of the configuration
    file.  When an appender of the same name already exist then it will be
    replaced.

    .EXAMPLE
    C:\PS> $myLog4NetAppender = [xml](@"
    <appender name="MyLogFileAppender" type="log4net.Appender.RollingFileAppender">
      <file value="C:\Logs\MyLog.txt" />
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
    "@)
    C:\PS> Add-Log4NetAppender -ConfigPath $myConfig -AppenderXml $myLog4NetAppender

    .LINK
    https://logging.apache.org/log4net/release/config-examples.html
#>
function Add-Log4NetAppender
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $ConfigPath,
        [Parameter(Mandatory=$true,position=1)]
        [xml] $AppenderXml
    )
    Process
    {
        if(-not (Test-Path $ConfigPath)){
            Write-Host "Bad Path or File Name at '$ConfigPath'" -ForegroundColor Yellow
            break;
        }

        if(($AppenderXml.DocumentElement.LocalName -ne "appender")){
            Write-Host "pass the full log4net appender xml into the cmdlet - see [https://logging.apache.org/log4net/release/config-examples.html] for examples."
            break;
        }

        $configContent = [xml](Get-Content $ConfigPath)
        
        $isConfigSectionDef = $configContent.SelectSingleNode("/configuration/configSections/section[@name='log4net']") -ne $null
        $islog4netSectionDef = $configContent.SelectSingleNode("/configuration/log4net") -ne $null
        $isRootLog4NetNode = $configContent.SelectSingleNode("/configuration/log4net/root") -ne $null

        $rootNode = $configContent.SelectSingleNode("/configuration")

        if(-not $isConfigSectionDef){

            $configSectionsNode = $configContent.SelectSingleNode("//configSections")

            #test for presence of a config sections 
            if($configSectionsNode -eq $null){
                $configSectionsNode = $configContent.CreateElement("configSections")
                if($rootNode.HasChildNodes){
                    $dnd = $rootNode.InsertBefore($configSectionsNode, $rootNode.FirstChild)
                }
                else{
                    $dnd = $rootNode.AppendChild($configSectionsNode)
                }
            }

            $sectionNode = $configContent.CreateElement("section");
            $sectionNameAttr = $configContent.CreateAttribute("name");
            $sectionNameAttr.Value = "log4net"
            $sectionTypeAttr = $configContent.CreateAttribute("type");
            $sectionTypeAttr.Value = "log4net.Config.Log4NetConfigurationSectionHandler,log4net"

            $dnd = $sectionNode.Attributes.Append($sectionNameAttr)
            $dnd = $sectionNode.Attributes.Append($sectionTypeAttr)

            $dnd = $configSectionsNode.AppendChild($sectionNode)

        }

        if(-not $islog4netSectionDef){
            $log4netNode = $configContent.CreateElement("log4net")
            if($rootNode.HasChildNodes){
                $dnd = $rootNode.InsertAfter($log4netNode, $rootNode.LastChild)
            }
            else{
                $dnd = $rootNode.AppendChild($log4netNode)
            }
        }
        else{
            $log4netNode = $configContent.SelectSingleNode("/configuration/log4net")
        }

        $appenderName = $AppenderXml.appender.name
        
        #add or replace the appender node by this name
        if($configContent.SelectSingleNode("/configuration/log4net/appender[@name='$appenderName']") -ne $null){
            $oldAppender = $configContent.SelectSingleNode("/configuration/log4net/appender[@name='$appenderName']")
            $appenderXmlNode = $configContent.ImportNode($AppenderXml.SelectSingleNode("/appender"), $true)
            $dnd = $log4netNode.ReplaceChild($appenderXmlNode,$oldAppender)
        }
        else{
            $appenderXmlNode = $configContent.ImportNode($AppenderXml.SelectSingleNode("/appender"), $true)
            
            if($log4netNode.HasChildNodes){
                $dnd = $log4netNode.InsertBefore($appenderXmlNode, $log4netNode.FirstChild)
            }
            else{
                $dnd = $log4netNode.AppendChild($appenderXmlNode)
            }
        }

        #add the log4net root node and appender-ref node if needed
        if($configContent.SelectSingleNode("/configuration/log4net/root") -eq $null){
            $rootLog4Net = $configContent.CreateElement("root");
            $dnd = $log4netNode.AppendChild($rootLog4Net)
        }
        else{
            $rootLog4Net = $configContent.SelectSingleNode("/configuration/log4net/root")
        }

        if($configContent.SelectSingleNode("/configuration/log4net/root/level") -eq $null){
            $lvlNode = $configContent.CreateElement("level");
            $lvlAttr = $configContent.CreateAttribute("value")
            $lvlAttr.Value = "DEBUG"

            $dnd = $lvlNode.Attributes.Append($lvlAttr)
            $dnd = $rootLog4Net.AppendChild($lvlNode)

        }
        else{
            $lvlNode = $configContent.SelectSingleNode("/configuration/log4net/root/level")
        }

        if($configContent.SelectSingleNode("/configuration/log4net/root/appender-ref[@ref='$appenderName']") -eq $null){
            $appenderRefNode = $configContent.CreateElement("appender-ref")
            $appenderRefAttr = $configContent.CreateAttribute("ref")
            $appenderRefAttr.Value = $appenderName

            $dnd = $appenderRefNode.Attributes.Append($appenderRefAttr)

            $dnd = $rootLog4Net.InsertAfter($appenderRefNode, $lvlNode)
        }

        #write it back out to file with pretty-printing
        $myXmlWriter = New-Object System.Xml.XmlTextWriter($ConfigPath, [System.Text.Encoding]::UTF8)
        $myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
        $configContent.WriteContentTo($myXmlWriter)
        $myXmlWriter.Flush()
        $myXmlWriter.Dispose()

        Write-Host "Done - New content added to '$ConfigPath'" -ForegroundColor Yellow

    }
}

<#
    .SYNOPSIS
    Removes a log4net appender from a .NET Configuration file.

    .DESCRIPTION
    This a counterpart cmdlet to the likewise one which adds
    an appender.
#>
function Remove-Log4NetAppender
{
	[CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $ConfigPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $AppenderName
    )
    Process
    {
        if(-not (Test-Path $ConfigPath)){
            Write-Host "Bad Path or File Name at '$ConfigPath'" -ForegroundColor Yellow
            break;
        }
        $configContent = [xml](Get-Content $ConfigPath)
        $anyChange = $false

        $log4netNode = $configContent.SelectSingleNode("/configuration/log4net")
        if($log4netNode -eq $null){
            break;
        }

        if($configContent.SelectSingleNode("/configuration/log4net/appender[@name='$AppenderName']") -ne $null){
            $appenderNode = $configContent.SelectSingleNode("/configuration/log4net/appender[@name='$AppenderName']")

            $dnd = $log4netNode.RemoveChild($appenderNode)

            $anyChange = $true
        }


        if($configContent.SelectSingleNode("/configuration/log4net/root/appender-ref[@ref='$AppenderName']") -ne $null){
            $appenderRefNode = $configContent.SelectSingleNode("/configuration/log4net/root/appender-ref[@ref='$AppenderName']")
            $log4netRootNode = $configContent.SelectSingleNode("/configuration/log4net/root")
            $dnd = $log4netRootNode.RemoveChild($appenderRefNode)

            $anyChange = $true
        }

        if(-not $anyChange){
            break;
        }

        $myXmlWriter = New-Object System.Xml.XmlTextWriter($ConfigPath, [System.Text.Encoding]::UTF8)
        $myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
        $configContent.WriteContentTo($myXmlWriter)
        $myXmlWriter.Flush()
        $myXmlWriter.Dispose()

        Write-Host "Done - Appender $AppenderName removed from '$ConfigPath'" -ForegroundColor Yellow

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
    
    .PARAMETER AssemblyPath
    This value is written to the text of HintPath for the 
    references.  May include global variables in the form
    of $(YOUR_VAR_HERE) which Visual Studio will resolve as 
    a relative path.

    .EXAMPLE
    C:\PS> $myAsm = '$(PROJ_CODE_ROOT)\SharedBinaries\log4net.dll'
    C:\PS> $myProjFile = "C:\Projects\31g\trunk\temp\code\.NET_2005_DllLib_ProjFile.csproj"
    C:\PS> Add-VsProjBinRef -Path $myProjFile -AssemblyPath $myAsm

#>
function Add-VsProjBinRef
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path,
        [Parameter(Mandatory=$true,position=1)]
        [string] $AssemblyPath
    )
    Process
    {
        $nfVsProj = New-Object NoFuture.Read.Vs.ProjFile($Path)
        $dnd = $nfVsProj.TryAddReferenceEntry($AssemblyPath)

        if($dnd){
            $nfVsProj.Save();
        }

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
        [string] $Path,
        [Parameter(Mandatory=$true,position=1)]
        [string] $NewItemPath
    )
    Process
    {
        $nfVsProj = New-Object NoFuture.Read.Vs.ProjFile($Path)
        $dnd = $nfVsProj.TryAddSrcCodeFile($NewItemPath)

        if($dnd){
            $nfVsProj.Save();
        }
    }
}

<#
    .SYSNOPSIS
    Removes an existing compile item to a VS proj file

    .DESCRIPTION
    Will remove the path of ExistingItemPath as a compile item
    from the project file given at Path.
    
    .PARAMETER Path
    The Path to a VS project file.
    
    .PARAMETER ExistingItemPaths
    An array of items to be removed.

#>
function Remove-VsProjCompileItem
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path,
        [Parameter(Mandatory=$true,position=1)]
        [array] $ExistingItemPaths
    )
    Process
    {
        $listOut = New-Object "System.Collections.Generic.List[string]"
        $nfVsProj = New-Object NoFuture.Read.Vs.ProjFile($Path)
        $dnd = $false
        $ExistingItemPaths | % {
            $dnd = $nfVsProj.TryRemoveSrcCodeFile($_,$listOut) -or $dnd
        }

        if($dnd){
            $nfVsProj.Save();
            return $listOut;
        }
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
    C:\PS> Replace-ExeConfigValues -ConfigFile $testConfigFile -Regex2Value $testReplacements -TransformFileName $testNewTrans
#>
function Replace-ExeConfigValues
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $ConfigFile,
        [Parameter(Mandatory=$false,position=1)]
        [hashtable] $Regex2Value,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $SwapConnStr,
        [Parameter(Mandatory=$false,position=3)]
        [string] $TransformFileName
    )
    Process
    {
        if(-not (Test-Path $ConfigFile)){
            Write-Host "Bad Path or File Name at '$ConfigFile'" -ForegroundColor Yellow
            break;
        }

        $configDir = Split-Path -Path $ConfigFile -Parent

        if($Regex2Value -eq $null){
            $Regex2Value = [NoFuture.Shared.RegexCatalog]::MyRegex2Values
        }
        $nfExeConfig = New-Object NoFuture.Read.Config.ExeConfig($ConfigFile)

        $blnSwapConStr = $false
        if($SwapConnStr){
            $blnSwapConStr = $true
        }

        $nfTransformedFile = $nfExeConfig.SplitAndSave($Regex2Value, $blnSwapConStr);

        if([string]::IsNullOrWhiteSpace($TransformFileName)){return;}

        if(-not ([System.IO.Path]::IsPathRooted($TransformFileName))){
            $transformOutFile = Join-Path $configDir $TransformFileName
        }
        else{
            $transformOutFile = $TransformFileName
        }

        if(Test-Path $transformOutFile){
            Write-Host "The file '$transformOutFile' already exist." -ForegroundColor Yellow
            break;
        }
        if(Test-Path $nfTransformedFile){
            Rename-Item $nfTransformedFile $transformOutFile
        }

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