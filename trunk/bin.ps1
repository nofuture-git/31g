try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-BinaryDump",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Invoke-JavaCompiler",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-JavaClassPath",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}

<#
    .SYNOPSIS
    Calls 'dumpbin.exe' for the given assembly
    
    .DESCRIPTION
    Powershell wrapper for command line calls
    to 'dumpbin.exe'.
    
    .PARAMETER Path
    Path to a valid Windows assembly.
    
    .PARAMETER DumpBinSwitches
    Supports all switches available to the dumpbin.exe

    .LINK
    http://msdn.microsoft.com/en-us/library/756as972%28v=vs.100%29.aspx
    
#>
function Get-BinaryDump
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,Position=0)]
        [string] $Path,
        [Parameter(Mandatory=$false,Position=1)]
        [array] $DumpBinSwitches
    )
    Process
    {
        #validate the path
        if(-not(Test-Path $Path)){Write-Host ("Bad path or file name at '{0}'" -f $Path) }
        if(-not(@(".dll",".exe",".lib") -contains ([System.IO.Path]::GetExtension($Path))))
        {
            Write-Host ("File is not binary type at '{0}'" -f $Path)
        }
        
        #test for dumpbin.exe being installed
        if(-not(Test-Path ([NoFuture.Tools.X64]::Dumpbin))){throw ("Expected to find 'dumpbin.exe' at '{0}'." -f ([NoFuture.Tools.X64]::Dumpbin)) }
        
        #construct command expression
        $dumpbin = ("`"{0}`"" -f ([NoFuture.Tools.X64]::Dumpbin))
        if($DumpBinSwitches -eq $null -or $DumpBinSwitches -eq "")
        {
            $parameterSwitch = " /EXPORTS"
        }
        else
        {
            $parameterSwitch = "/" + [string]::Join("/", $DumpBinSwitches)
        }
        $cmd = ("& {0} {1} '{2}'" -f $dumpbin, $parameterSwitch, $Path)
        Write-Host $cmd
        
        #run it
        Invoke-Expression -Command $cmd
    }
}

<#
    .SYNOPSIS
    Compiles .java files at JavaSrc location to JavaBuild then 
    places them into a .jar file at JavaDist.
    
    .DESCRIPTION
    Given some existing file(s) at NoFuture's Temp directory 
    JavaSrc, the java compiler is invoked sending .class files
    to JavaBuild. 

    .PARAMETER TypeName
    The file name(s) targeted for compilation, use astrick 
    to have compiler perform a search (e.g. MyJava*). Do not 
    include the .java extension.

    .PARAMETER Package
    Optional package (i.e. Namespace) with which the compilation
    targets belong

    .PARAMETER ClassPath
    Optional, additional classpaths - the environmental CLASSPATH 
    variable is already present and should not be specified 
    again.

#>
function Invoke-JavaCompiler
{
	[CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $TypeName,

        [Parameter(Mandatory=$false,position=1)]
        [string] $Package,

        [Parameter(Mandatory=$false,position=2)]
        [string] $ClassPath

    )
    Process
    {
        $javaSrc = [NoFuture.TempDirectories]::JavaSrc 
        $javaRoot = (Split-Path $javaSrc -Parent)
        $javaBuild = [NoFuture.TempDirectories]::JavaBuild 
        $javaDist = [NoFuture.TempDirectories]::JavaDist 
        $javaArchive = [NoFuture.TempDirectories]::JavaArchive 

        #remove .java extension if present
        if($TypeName.EndsWith(".java")){
            $outTo = $TypeName.IndexOf(".java")
            $TypeName = $TypeName.Substring(0,$outTo)
        }

        #in Java directory structure contained within a .jar must match the type's 'namespace'
        if(-not [System.String]::IsNullOrWhiteSpace($Package))
        {
            #remove semi-colon from the tail
            if($Package.Contains(";")){$Package = $Package.Replace(";","");}
            $packageRelPath = $Package.Replace(".",([System.IO.Path]::PathSeparator));
            $javaSrc = (Join-Path $javaSrc $packageRelPath);
            $javaBuild = (Join-Path $javaBuild $packageRelPath);
        }

        if(-not (Test-Path $javaRoot)){$donotdisplay = mkdir $javaRoot -Force}
        if(-not (Test-Path $javaSrc)) {$donotdisplay = mkdir $javaSrc -Force}
        if(-not (Test-Path $javaBuild)) {$donotdisplay = mkdir $javaBuild -Force}
        if(-not (Test-Path $javaDist)) {$donotdisplay = mkdir $javaDist -Force}
        if(-not (Test-Path $javaArchive)) {$donotdisplay = mkdir $javaArchive -Force}

        $ClassPath = Get-JavaClassPath $ClassPath

        #compile
        . ([NoFuture.Tools.JavaTools]::Javac) -d $javaBuild -cp $ClassPath (Join-Path $javaSrc ("$TypeName.java"))


        #place into jar
        if(-not [System.String]::IsNullOrWhiteSpace($Package))
        {
            $jarFile = (Join-Path $javaDist "$Package.jar")
        }
        else
        {
            $TypeName = $TypeName.Replace("*","")
            $jarFile = (Join-Path $javaDist "$TypeName.jar")
        }
        
        Push-Location $javaBuild
        . ([NoFuture.Tools.JavaTools]::Jar) cf $jarFile "*"
        Pop-Location

        return $jarFile
    }

}#end Invoke-JavaCompiler

function Get-JavaClassPath($ClassPath){
    $cp =  '.`;{0}' -f $env:CLASSPATH
    if(-not [System.String]::IsNullOrWhiteSpace($ClassPath)){
        $cp += '`;{0}' -f $ClassPath
    }
    return $cp
}
