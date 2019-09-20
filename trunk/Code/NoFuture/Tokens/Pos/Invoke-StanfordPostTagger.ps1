$myScriptLocation = Split-Path $PSCommandPath -Parent
$dependencies = @{
    "NoFuture.Shared.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.Core.dll");
    "NoFuture.Shared.Cfg, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.Cfg.dll");
    "NoFuture.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.dll");
    "NoFuture.Util.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.Core.dll");
    "NoFuture.Util.NfConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Util.NfConsole.dll");
    "NoFuture.Tokens.Pos, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $noFutureBin "NoFuture.Tokens.Pos.dll");
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


#=====================================
#intialize paths from nfConfig.cfg.xml
$dnd = [NoFuture.Shared.Cfg.NfConfig]::Init(([NoFuture.Shared.Cfg.NfConfig]::FindNfConfigFile($noFutureBin)))
#=====================================

<#
    .SYNOPSIS
    Invokes edu.stanford.nlp.tagger.maxent.MaxentTagger in JVM
    
    .DESCRIPTION
    Locates the edu.stanford.nlp.tagger.maxent.MaxentTagger at 
    NoFuture.Shared.Core.NfConfig+JavaTools.Stanford.StanfordPostTagger and copies it 
    into the Java global CLASSPATH if it is not already present there.

    The MaxentTagger requires a model and the cmdlet will choose whatever
    is the largest one present in NoFuture.Shared.Core.NfConfig+JavaTools.StanfordPostTaggerModels.

    The JVM is invoked on a separate process.  The MaxentTagger progress is printed
    upon the completion of the JVM.  If an OutPath was sepecified that path 
    is passed directly to the MaxentTagger as the -outputFile; otherwise, the
    output is simply written the ps host.

    
    .PARAMETER Path
    A valid path to a unstructured text document to be tagged.

    .PARAMETER OutPath
    Optional path to have the tagging output saved to.

    .EXAMPLE
    PS C:\> Invoke-StanfordPostTagger -Path .\temp\testInputPosFile.txt -OutPath .\temp\posOut.txt
    PS C:\> [NoFuture.Util.Pos.ITagset[][]]$tags = New-Object "NoFuture.Util.Pos.ITagset[][]" 0,0
    PS C:\> [NoFuture.Util.Pos.PtTagset]::TryParse((Get-Content .\temp\posOut.txt), [ref]$tags)
#>
function Invoke-StanfordPostTagger
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path,

        [Parameter(Mandatory=$false,position=1)]
        [string] $OutPath

    )
    Process
    {
        #validate the input 
        if(-not (Test-Path $Path)){
            Write-Host "bad path or file name at $Path"
            break;
        }

        #must perform invoke with full paths
        $Path = (Resolve-Path $Path).Path

        if(-not ([string]::IsNullOrWhiteSpace($OutPath))){
            $outp = [System.IO.Path]::GetDirectoryName($OutPath)
            $outf = [System.IO.Path]::GetFileName($OutPath)

            if(-not (Test-Path $outp)){
                Write-Host "bad path for out file $OutPath"
            }
            $OutPath = Join-Path (Resolve-Path $outp).Path $outf
        }

        #get path to jar and dependent resources
        $binCopy = [NoFuture.Shared.Cfg.NfConfig+JavaTools]::StanfordPostTagger
        $biggestModel = (ls -Path ([NoFuture.Shared.Cfg.NfConfig+JavaTools]::StanfordPostTaggerModels) | ? {$_.Extension -eq ".tagger"} | Sort-Object -Property Length -Descending | Select-Object -First 1)
        $model = $biggestModel.FullName

        #expect jar to be in classpath
        $spt = Join-Path $env:CLASSPATH.Replace("*","") ([System.IO.Path]::GetFileName($binCopy))

        #if jar is not in classpath then put it there
        if(-not (Test-Path $spt)){
            
            #if its nowhere to be found then leave.
            if(-not (Test-Path $binCopy)){
                Write-Host "stanford-postagger.jar not found at CLASSPATH nor at $binCopy"
                break;
            }
            Copy-Item -Path $binCopy -Destination $env:CLASSPATH.Replace("*","") -Force
        }

        $separator = [NoFuture.Util.Pos.PtTagset]::TagDelimiter;

        #the data payload is sent to the StandardOut while progress from JVM is sent to StandardError
        $sp = New-Object System.Diagnostics.ProcessStartInfo;
        $sp.FileName = "java.exe"
        $sp.RedirectStandardError = $true
        $sp.RedirectStandardOutput = $true
        $sp.UseShellExecute = $false;
        if(-not ([string]::IsNullOrWhiteSpace($OutPath))){
            $sp.Arguments = "-mx300m edu.stanford.nlp.tagger.maxent.MaxentTagger -model $model -textFile $Path -outputFile $OutPath -tagSeparator $separator"
        }
        else{
            $sp.Arguments = "-mx300m edu.stanford.nlp.tagger.maxent.MaxentTagger -model $model -textFile $Path -tagSeparator $separator"
        }
        
        $p = New-Object System.Diagnostics.Process
        $p.StartInfo = $sp
        $p.Start() | Out-Null
        $p.WaitForExit()
        Write-Host $p.StandardError.ReadToEnd() -ForegroundColor Yellow
        if(([string]::IsNullOrWhiteSpace($OutPath))){
            Write-Host $p.StandardOutput.ReadToEnd()
        }
    }
}