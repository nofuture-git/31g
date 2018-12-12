$myScriptLocation = Split-Path $PSCommandPath -Parent
$dependencies = @{
    "NoFuture.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.dll");
    "NoFuture.Shared.Cfg, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.Cfg.dll");
    "NoFuture.Shared.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.Core.dll");
    "NoFuture.Gen.InvokeDia2Dump, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Gen.InvokeDia2Dump.dll");
}

$loadedAsms = ([appdomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}  | Sort-Object -Unique)
$dependencies.Keys | % {
    if($loadedAsms -notcontains $_)
    {
        $binDll = $dependencies.$_
        if(Test-Path $binDll)
        {
            $stdout = [System.Reflection.Assembly]::LoadFile($binDll)
        }
        else
        {
            Write-Host ("'{0}' was not found at '{1}'; some functions will not be available." -f $_, $binDll) -ForegroundColor Yellow
        }
    }
}

<#
    .SYNOPSIS
    Gets the Pdb Data for a single type.

    .DESCRIPTION
    Invokes the modified Dia2Dump.exe passing in
    the type's full name.

    .PARAMETER AssemblyPath
    The path to a dll whose source code files may be 
    found according to the side-by-side pdb file.

    .PARAMETER TypeFullName
    The full type's name as in namespace and class name.

    .OUTPUTS
    NoFuture.Shared.DiaSdk.LinesSwitch.PdbCompiland

#>
function Get-TypePdbLines
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeFullName
    )
    Process
    {
        $invokeDia2Dump = New-Object NoFuture.Gen.InvokeDia2Dump.GetPdbData($AssemblyPath)
        $pdbCompiland = $invokeDia2Dump.SingleTypeNamed($TypeFullName)
        return $pdbCompiland
    }
}
