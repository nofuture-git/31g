<#
This module is intended to quickly turn any .NET compiled assembly into 
a Nuget package in a this user's $env:APPDATA\NuGetTempPkgs
#>

$myScriptLocation = Split-Path $PSCommandPath -Parent
$logoUrl = "file:///$myScriptLocation/favicon.png"
$Global:localNugetRepo = "$env:APPDATA\NuGetTempPkgs"

#this is how the NuGet.exe know where to look for a package
$Global:nuGetConfigPath = "$env:APPDATA\NuGet\NuGet.config"

<#
    .SYNOPSIS
    Invokes the three Nuget cmdlets in tandem.
    
    .DESCRIPTION
    Performs all three Nuget package 
    cmdlets in sequence; namely,
    Spec, Pack and Push on the given 
    assembly with the given version.

    .PARAMETER AssemblyFullPath
    Required, the full path to the assembly which
    is being packaged.

    .PARAMETER PkgVersion
    Required, the version number of the created
    package.  When updating, make sure this 
    value is higher than whatever the version
    is in the project's packages.config.

    .EXAMPLE
    C:\PS> $asmPath = "C:\Projects\MyProject\bin\AssemblyNamedThis.dll"
    C:\PS> New-Assembly2NugetPkg -AssemblyFullPath $asmPath -PkgVersion "2018.2.0.42290"
#>
function New-Assembly2NugetPkg
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyFullPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $PkgVersion,
        [Parameter(Mandatory=$false,position=2)]
        [ValidateSet("net35","net40","net45","net461","net462","net47","net471")]
        [string] $DotNetVersion

    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($DotNetVersion)){
            $DotNetVersion = "net45"
        }
        Invoke-NuGetSpec -AssemblyPath $AssemblyFullPath -PkgVersion $PkgVersion
        Invoke-NuGetPack -AssemblyPath $AssemblyFullPath -DotNetVersion $DotNetVersion
        Invoke-NuGetPush -AssemblyPath $AssemblyFullPath

        $pkgName = [System.IO.Path]::GetFileNameWithoutExtension($AssemblyFullPath)
        $pkgSrc = $Global:localNugetRepo
        Write-Host " -Id $pkgName -Version $PkgVersion -Source $pkgSrc" -ForegroundColor Yellow
    }
}

<#
    .SYNOPSIS
    Downloads a copy of NuGet.exe from the web
    
    .DESCRIPTION
    Downloads a copy of NuGet.exe from the web
    using the current user's default creds.

    .PARAMETER OutPath
    Optional, specify a directory to have the downloaded
    copy of NuGet.exe placed in.  Defaults to this
    scripts directory.
    
#>
function Get-NuGetExe
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [string] $OutPath
    )
    Process
    {
        if([String]::IsNullOrWhiteSpace($OutPath)){
            $OutPath = $myScriptLocation
        }
        
        $nugetExeUri = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
        Write-Host "Downloading NuGet.exe from $nugetExeUri"
        $nuGetExeOutFile = (Join-Path $OutPath "NuGet.exe")
        $nuGetExe = Invoke-WebRequest $nugetExeUri -UseDefaultCredentials -OutFile $nuGetExeOutFile
        return $nuGetExeOutFile
    }

}


<#
    .SYNOPSIS
    Create a NuGet spec file.
    
    .DESCRIPTION
    Creates a NuGet spec file for the
    AssemblyPath with optional spec data.
    
    .PARAMETER AssemblyPath
    Required, the fully qualified path to the 
    .NET assembly.
    
    .PARAMETER PkgVersion
    Optional, version of the NuGet package. Defaults to 1.0.0.0

    .PARAMETER PkgTag
    Optional, tags used by the various NuGet package manager apps.
    
    .PARAMETER PkgDescription
    Optional, a description used by various NuGet package manager apps.
    
#>
function Invoke-NuGetSpec
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath,

        [Parameter(Mandatory=$false,position=1)]
        [string] $PkgVersion,

        [Parameter(Mandatory=$false,position=2)]
        [string] $PkgTag,

        [Parameter(Mandatory=$false,position=3)]
        [string] $PkgDescription
    )
    Process
    {
        $path = $AssemblyPath
        $ver = $PkgVersion
        $desc = $PkgDescription
        $tags = $PkgTag

        if([string]::IsNullOrWhiteSpace($ver)){
	        $ver = "1.0.0.0"
        }

        if([string]::IsNullOrWhiteSpace($tags)){
	        $tags = "NoFuture temporary Nuget package"
        }

        $workingDir = Split-Path $path -Parent
        $asm = Split-Path $path -Leaf

        if([string]::IsNullOrWhiteSpace($desc)){
	        $desc = ("{0}" -f $asm.Replace("."," "))
        }

        $nugetExe = Join-Path $workingDir "NuGet.exe"
        if(-not (Test-Path $nugetExe)){
            $nugetExe = Get-NuGetExe $workingDir
        }

        #remove any existing nuspec files
        ls -Path $workingDir | ? {$_.Extension -eq ".nuspec"} | % {Remove-Item $_.FullName -Force}

        Push-Location $workingDir

        Invoke-Expression "& $nugetExe spec -a $asm"

        $buildMachine = $env:COMPUTERNAME
        $buildUser = $env:USERNAME 
        $buildTime = "{0:yyyyMMddhhmmss}" -f $(Get-Date)

        $nm = ([System.IO.Path]::ChangeExtension($asm, ".nuspec"))

        $nuspec = [xml](Get-Content $nm)
        $metatData = $nuspec.SelectSingleNode("//metadata")
        

        $dnd = $metatData.RemoveChild($nuspec.SelectSingleNode("//licenseUrl"))
        $dnd = $metatData.RemoveChild($nuspec.SelectSingleNode("//projectUrl"))

        $versionNode = $nuspec.SelectSingleNode("//version");
        $versionNode.InnerText = $ver

        $iconUrlNode = $nuspec.SelectSingleNode("//iconUrl")
        $iconUrlNode.InnerText = $logoUrl

        $descriptionNode = $nuspec.SelectSingleNode("//description")
        $descriptionNode.InnerText = $desc

        $relNotesNode = $nuspec.SelectSingleNode("//releaseNotes")

        $relNotesNode.InnerText = "BuildMachine : $buildMachine, BuildUser : $buildUser, BuildTime : $buildTime"

        $tagsNode = $nuspec.SelectSingleNode("//tags")

        $tagsNode.InnerText = $tags

        $dnd = $metatData.RemoveChild($nuspec.SelectSingleNode("//dependencies"))

        $myXmlWriter = New-Object System.Xml.XmlTextWriter((Join-Path $workingDir $nm), [System.Text.Encoding]::UTF8)
        $myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
        $nuspec.WriteContentTo($myXmlWriter)
        $myXmlWriter.Flush()
        $myXmlWriter.Dispose()

        Pop-Location
    }
}

<#
    .SYNOPSIS
    Package an assembly into a NuGet package

    .DESCRIPTION
    Package an assembly into a NuGet package

    .PARAMETER AssemblyPath
    Required, the fully qualified path to the 
    .NET assembly.

    .PARAMETER NuSpecFile
    Optional, the fully qualified path to the
    .nuspec file attached to this assembly.  The
    default assumes the .nuspec file sits side-by-side
    with the Assembly
    
    .PARAMETER DotNetVersion
    Optional, the target .NET framework version. 
    Defaults to net45
    
#>
function Invoke-NuGetPack
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath,

        [Parameter(Mandatory=$false,position=1)]
        [string] $NuSpecFile,

        [Parameter(Mandatory=$false,position=2)]
        [ValidateSet("net35","net40","net45","net461","net462","net47","net471")]
        [string] $DotNetVersion
    )
    Process
    {
        $path = $AssemblyPath 
        $ver = $DotNetVersion
        $nugetSpecFile = $NuSpecFile

        if([string]::IsNullOrWhiteSpace($ver)){
            $ver = "net45"
        }

        #est the dir of the nuspec file
        $workingDir = $myScriptLocation

        $nugetExe = Join-Path $workingDir "bin\NuGet.exe"
        if(-not (Test-Path $nugetExe)){
            $nugetExe = Get-NuGetExe $workingDir
        }

        Push-Location $workingDir

        if([string]::IsNullOrWhiteSpace($nugetSpecFile)){
            $nugetSpecFile = Split-Path ([System.IO.Path]::ChangeExtension($path, ".nuspec")) -Leaf
        }

        if(-not (Test-Path $nugetSpecFile)){
            throw "The NuGet spec file is missing"
            break;
        }

        #create a sub-dir therein named 'lib'
        $libDir = Join-Path $workingDir "lib\$ver"
        if(-not (Test-Path $libDir)){
            mkdir $libDir
        }

        #clean out any contents of the 'lib' folder
        ls -Path $libDir | % { Remove-Item -Path $_.FullName -Force}

        #copy everything but the spec over to the 'lib' folder
        ls -Path $workingDir | ? {-not $_.PSIsContainer -and $_.Extension -ne ".nuspec" -and $_.Name -ne "NuGet.exe"} | % {
            Move-Item -Path $_.FullName -Destination $libDir -Force
        }

        #pack it up in a way Nuget understands

        Invoke-Expression "& $nugetExe pack $nugetSpecFile"

        Pop-Location
    }
}

<#
    .SYNOPSIS
    Pushed a NuGet package to a local temp repo
    
    .DESCRIPTION
    Pushed a NuGet package to a local temp repo
    at "%APPDATA%\NuGetTempPkgs"
    
    .PARAMETER AssemblyPath
    Required, the fully qualified path to the 
    .NET assembly.  At this stage only this value
    is being used as a reference - it not required 
    that the original dll still be present here on
    the disk. 
    
#>
function Invoke-NuGetPush
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath
    )
    Process
    {
        $path = $AssemblyPath

        #est the dir of the nuspec file
        $workingDir = Split-Path $path -Parent

        $nugetExe = Join-Path $workingDir "NuGet.exe"
        if(-not (Test-Path $nugetExe)){
            $nugetExe = Get-NuGetExe $workingDir
        }

        Push-Location $workingDir

        $uncPath = $Global:localNugetRepo
        if(-not (Test-Path $uncPath)){
            $dnd = mkdir $uncPath -Force
        }

        ls -Path $workingDir | ? {$_.Extension -eq ".nupkg"} | % {
            $nuPkgName = $_.Name
            Invoke-Expression "& $nugetExe push -source '$uncPath' $nuPkgName"
        }

        Pop-Location

        if(Test-Path $Global:nuGetConfigPath){
            $nugetConfig = [xml](Get-Content $Global:nuGetConfigPath)

            $alreadyPresent = $nugetConfig.SelectSingleNode("//packageSources/add[@value='$uncPath']")
            if($alreadyPresent -eq $null){
                $pkgSrcNode = $nugetConfig.SelectSingleNode("//packageSources")

                if($pkgSrcNode -eq $null){
                    $pkgSrcNode = $nugetConfig.CreateElement("packageSources")
                    $configNode = $nugetConfig.SelectSingleNode("//configuration")
                    $dnd = $configNode.PrependChild($pkgSrcNode)
                }

                $nfAddNode = $nugetConfig.CreateElement("add")
                $nfKeyAttr = $nugetConfig.CreateAttribute("key")
                #make sure this is unique
                $nfKeyAttr.Value = ("My Local Packages ({0})" -f [System.IO.Path]::GetRandomFileName())
                $nfValAttr = $nugetConfig.CreateAttribute("value")
                $nfValAttr.Value = $uncPath
                $dnd = $nfAddNode.Attributes.Append($nfKeyAttr)
                $dnd = $nfAddNode.Attributes.Append($nfValAttr)

                $dnd = $pkgSrcNode.AppendChild($nfAddNode)

                $myXmlWriter = New-Object System.Xml.XmlTextWriter($Global:nuGetConfigPath, [System.Text.Encoding]::UTF8)
                $myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
                $nugetConfig.WriteContentTo($myXmlWriter)
                $myXmlWriter.Flush()
                $myXmlWriter.Dispose()

            }
        }
    }
}