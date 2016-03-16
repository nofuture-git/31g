try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Start-ProcessRedirect",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Start-Jre",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Write-HostAsciiArtLarge",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Write-HostAsciiArt",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-Speech",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Write-TransparentHost",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Out-FunnyComputerBeeps",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
$MessageBeep = [psobject] "MessageBeep"
$MessageBeep | Add-Member -MemberType NoteProperty -Name "OK" -Value ([System.UInt32]0)
$MessageBeep | Add-Member -MemberType NoteProperty -Name "Error" -Value ([System.UInt32]16)
$MessageBeep | Add-Member -MemberType NoteProperty -Name "Question" -Value ([System.UInt32]32)
$MessageBeep | Add-Member -MemberType NoteProperty -Name "Warning" -Value ([System.UInt32]48)
$MessageBeep | Add-Member -MemberType NoteProperty -Name "Info" -Value ([System.UInt32]64)


$marshalBeep = @'
namespace NoFuture
{
public class BeepMarshal
{
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool Beep(uint dwFreq, uint dwDuration);
        
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool MessageBeep(uint uType);

}
}
'@
Add-Type -TypeDefinition $marshalBeep

$marshalPopup = @'
namespace NoFuture
{
public class PopupMarshal
{
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int MessageBox(System.IntPtr hWnd,
                                            string lpText,
                                            string lpCaption,
                                            uint uType);
}
}
'@
Add-Type -TypeDefinition $marshalPopup

function Out-FunnyComputerBeeps(){
    $rand = New-Object System.Random ([Int32][String]::Format("{0:fffffff}",$(get-date)))

    for($i=0;$i-lt6;$i++){
        $freq = $rand.Next(64,2664)
        $doNoDisplay = [NoFuture.BeepMarshal]::Beep($freq,200)
    }
}


<#
    .SYNOPSIS
    Uses standard p/invoke to start the target exe.

    .DESCRIPTION
    Like normal 'Start-Process', returning a new instance
    of System.Diagnostics.Process; only, this cmdlet separates 
    from the powershell host and returns the invoked process
    as a Process variable having its standard output redirected
    to a new and seperate console window.  The data lines are 
    still available on the global:redirectData variable.
    
    Use the Get-EventSubscriber followed by the Unregister-Event 
    cmdlets to turn-off the OutputDataReceived event-handler.

    The cmdlet will add ScriptMethods and NoteProperties to the 
    Process object.  Of special note is the ScriptMethod 'Stop()'
    which should always be called to release all resources associated
    to the redirected process.

    Lastly, all redirected standardoutput gets dumped to a text file
    upon calling 'Stop()'.

    .PARAMETER ExePath
    The full path to the executable to be invoked an onto whom 
    the commands will be passed
	 
    .PARAMETER StartInfoArgs
    Args to be passed into the process by the OS upon a call to Start()


#>
function Start-ProcessRedirect
{
	[CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $ExePath,
        [Parameter(Mandatory=$false,position=1)]
        [string] $StartInfoArgs
    )
    Process
    {
        #validate inputs
        if(-not (Test-Path $ExePath)){throw ("the exe is not at the expected location of {0}" -f $ExePath);}

        #create a process to run with redirect on std in/out
        $p = New-Object System.Diagnostics.Process
        $p.StartInfo.FileName = $ExePath
        $p.StartInfo.UseShellExecute = $false
        $p.StartInfo.RedirectStandardOutput = $true
        $p.StartInfo.RedirectStandardInput = $true
        $p.StartInfo.CreateNoWindow = $true

        #copy over start args if any
        if($StartInfoArgs -ne $null -and $StartInfoArgs -ne "")
        {
             $p.StartInfo.Arguments = $StartInfoArgs
        }

        #register the Diagnostics.Process.OutputDataReceived event to powershell event queue
        $psEventJob = Register-ObjectEvent -InputObject $p -EventName "OutputDataReceived" -Action {
            $data = ("{0}" -f $event.SourceEventArgs.Data)
            Receive-OutputDataReceivedEvent $Sender $data $EventSubscriber
        }

        #add script members
        $p = (Add-ProcessRedirectMethod $p)

        #start target exe
        $doNotDisplay = $p.Start()
        
        #add ref to this proc to a global list controled by master console
        $global:redirectProcs.Add($p.Id, $p)

        #if the global transparent is up write a comment
        if($global:aeroWindow -ne $null)
        {
            Write-TransparentHost ("new child process started PID: {0}" -f $p.Id)
        }

        #put the subscribed event into motion        
        $doNotDisplay = $p.BeginOutputReadLine()

        #return the running process
        return $global:redirectProcs.($p.Id)

    }
}

function Add-ProcessRedirectMethod($p)
{
    #add Data
    if($p.Data -eq $null)
    {
        $p | Add-Member NoteProperty Data @() -Force

    }

    #add TransparentWindow
    if($p.TransparentWindow -eq $null)
    {
        $p | Add-Member NoteProperty TransparentWindow -Force $(Send-ToTransparent)

    }

    #get global ref's scriptmethods
    $pScriptMethods = ($p | Get-Member -MemberType ScriptMethod | % { $_.Name})
    if($pScriptMethods -eq $null) { $pScriptMethods = @() } #avoid not-null checks


    #add QuitTransparent()
    if($pScriptMethods -notcontains "QuitTransparent")
    {
        $p | Add-Member ScriptMethod QuitTransparent -Force {
            if($this.TransparentWindow -ne $null){
                $this.TransparentWindow.Quit()
                $this.TransparentWindow = $null
            }
        }

    }

    #add Stop()
    if($pScriptMethods -notcontains "Stop")
    {
        $p | Add-Member ScriptMethod Stop -Force {

            #perform the close operation on the transparent window allowing it to be GC'ed
            $this.QuitTransparent();

            #unregister this proc's 'OutputDataReceived' from powershell's queue
            $regEventObj = (Get-EventSubscriber | ? {$_.SourceObject -eq $this})
            Unregister-Event -SubscriptionId $regEventObj.SubscriptionId

            #dump the stdout to file
            $dumpFile = (Join-Path ([NoFuture.TempDirectories]::Debug) ("{0}.txt" -f $this.Name))
            ("{0:yyyyMMdd HHmmss}`n" -f $(Get-Date)) >> $dumpFile
            ($global:redirectProcs.($this.Id).Data) >> $dumpFile
            
            #remove the global reference
            $global:redirectProcs.Remove($this.Id)

            #shut-down this process itself
            if(-not ($this.HasExited))
            {
                $this.Kill()
            }
        }
    }

    return $p

}

function Receive-OutputDataReceivedEvent($p,$data,$theEventSubscriber)
{

    #get global ref to this proc 
    $gRefProc = $global:redirectProcs.($p.Id)

    #append to objects stdout array
    $gRefProc.Data += $data

    #print it to this proc's transparent console
    if(($gRefProc | Get-Member -MemberType NoteProperty | % { $_.Name}) -contains "TransparentWindow")
    {
        #print with coor. index value
        $gRefProc.TransparentWindow.StandardInput.WriteLine(("[{0}]{1}" -f  ($gRefProc.Data.Count-1),$data))
    }

}


<#
    .SYNOPSIS
    Launches 'jrunscript' which operates as a running instance 
    of a runtime JRE interpreter.

    .DESCRIPTION
    'jrunscript.exe' is an implementation of JSR-223 provided as
    part of the JDK.  This cmdlet launches it as a hidden child 
    console app having it open to receive input though a socket 
    on the loopback address and specified port 
    (NoFuture.JavaTools.JrePort).  Furthermore the standard output 
    is wired up to write back to this ps session's host.

    The JRE log file is located under the NoFuture.TempDirectories.Debug 
    folder as, simply, Jre.log.

    .LINK
    http://download.java.net/jdk8/docs/technotes/guides/scripting/programmer_guide/index.html
    http://docs.oracle.com/javase/7/docs/technotes/tools/share/jrunscript.html
    http://docs.oracle.com/javase/7/docs/technotes/tools/#scripting


#>
function Start-Jre
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
        #validate inputs
        if(-not (Test-Path ([NoFuture.Tools.JavaTools]::JRunScript))){throw ("the exe is not at the expected location of {0}" -f ([NoFuture.Tools.JavaTools]::JRunScript));}
        $jrePort = ([NoFuture.Tools.JavaTools]::JrePort)

        if($jrePort -eq $null -or $jrePort -eq 0){throw ("set the value of [NoFuture.JavaTools.JrePort] to a value and try again.")}

        $global:jrunscript = New-Object NoFuture.Host.Proc

        $global:jrunscript.ExePath = ([NoFuture.Tools.JavaTools]::JRunScript)
        $global:jrunscript.Port =  $jrePort
		$global:jrunscript.Address = [System.Net.IPAddress]::Loopback
        $global:jrunscript.Log = (Join-Path ([NoFuture.TempDirectories]::Debug) "Jre.log")

        $global:jrunscript.InitProc()

        $donotdisplay = Register-ObjectEvent -InputObject $global:jrunscript.Exe -EventName OutputDataReceived -Action {
                    $stdOutText = $event.SourceEventArgs.Data
                    Write-Host $stdOutText
                }
        $donotdisplay = Register-ObjectEvent -InputObject $global:jrunscript.Exe -EventName ErrorDataReceived -Action {
                    $errorOutput = $event.SourceEventArgs.Data
                    $errorOutput = $errorOutput.Replace("js> ","").Trim()
                    if($errorOutput -ne $null -and $errorOutput -ne "")
                    {
                        Write-Host $errorOutput
                    }
                }
        

        $global:jrunscript.BeginProc();

        $global:jrunscript | Add-Member -MemberType NoteProperty -Name JavaPackages -Value (New-Object 'System.Collections.ObjectModel.Collection`1[System.String]')
        $global:jrunscript | Add-Member -MemberType NoteProperty -Name JavaClasses -Value (New-Object 'System.Collections.ObjectModel.Collection`1[System.String]')

        #add some helper methods
        $global:jrunscript | Add-Member -MemberType ScriptMethod -Name Write -Value {
            $text = $args[0]
            Send-ToJrunscript $text
        }

        $global:jrunscript | Add-Member -MemberType ScriptMethod -Name ImportPackage -Value {
            $package = $args[0]
            Send-ToJrunscript "importPackage($package);"
            $this.JavaPackages.Add($package);
        }

        $global:jrunscript | Add-Member -MemberType ScriptMethod -Name ImportClass -Value {
            $class = $args[0]
            Send-ToJrunscript "importClass($class);"
            $this.JavaClasses.Add($class);
        }

        #directly import package needed for dynamic type instantiation
        $global:jrunscript.ImportPackage("java.net");
        $global:jrunscript.ImportPackage("java.util.jar");
    }
}

function Send-ToJrunscript($text)
{
    $loopSocket = New-Object System.Net.Sockets.Socket([System.Net.Sockets.AddressFamily]::InterNetwork, [System.Net.Sockets.SocketType]::Stream, [System.Net.Sockets.ProtocolType]::IP);
    $myEndPoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Loopback, ([NoFuture.Tools.JavaTools]::JrePort));
    $loopSocket.Connect($myEndPoint);
    $donotdisplay = $loopSocket.Send([System.Text.Encoding]::UTF8.GetBytes($text));
    $loopSocket.Close();

}

<#
    .SYNOPSIS
    This PS Session's Transparent console window.

    .DESCRIPTION
    For use of printing text to the seperate 
    transparent console window.
#>
function Write-TransparentHost
{
    [CmdletBinding()]
    Param
    (
    [Parameter(Mandatory=$false)]
    [string] $TransparentText
    )
    Process
    {
        
        if($global:aeroWindow -eq $null)
        {
            
            $global:aeroWindow= Send-ToTransparent
         
            $psEventJob = Register-ObjectEvent -InputObject $global:aeroWindow -EventName "Exited" -Action {
                Write-Host "`nGlobal Aero console window has been successfully closed." -ForegroundColor Yellow
                #on exit, unregister this event 
                $regEventObj = (Get-EventSubscriber | ? {$_.SourceObject -eq $global:aeroWindow})
                Unregister-Event -SubscriptionId $regEventObj.SubscriptionId

                #and reset the global 
                $global:aeroWindow = $null

            }
        }
        $global:aeroWindow.StandardInput.WriteLine($TransparentText);

    }
}

<#
    .SYNOPSIS
    Intended only for use of printing StdOut
    to a fancy, semi-transparent console window.

    .DESCRIPTION
    Not intended to be used outside this script file.
    Use 'aero.ps1'  script or Write-TransparentHost cmdlet 
    to produce custom transparent console windows.

#>
function Send-ToTransparent
{
    Param()
    Process
    {
        #start a powershell session
        $rt = [NoFuture.Tools.CustomTools]::RunTransparent
        $ps = New-Object System.Diagnostics.Process
        $ps.StartInfo.RedirectStandardInput = $true
        $ps.StartInfo.UseShellExecute = $false
        $ps.StartInfo.CreateNoWindow = $false
        $ps.StartInfo.FileName = "powershell.exe"

        #have it run aero script to look cool and remain in a IO state
        $ps.StartInfo.Arguments = ('cd {0}; . {1};' -f $mypshome, $rt)
        $donotdisplay = $ps.Start()
        $ps | Add-Member ScriptMethod Quit { 
            $this.StandardInput.WriteLine("quit");
            $this.Close();
            $this.Dispose();
            }

        #return it
        return $ps
    }
}
<#
    .SYNOPSIS
    Returns .NET object capable of audio speech
    
    .DESCRIPTION
    Returns System.Speech.Synthesis.SpeechSynthesizer having
    the first installed Voice (Microsoft Anna).  The object reference 
    is global:syn.
    
    .RETURNS
    System.Speech.Synthesis.SpeechSynthesizer
    
    .LINK
    http://msdn.microsoft.com/en-us/library/system.speech.synthesis.speechsynthesizer(v=vs.100).aspx

#>
function Get-Speech
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
        if($global:syn -eq $null)
        {
            $global:syn = New-Object System.Speech.Synthesis.SpeechSynthesizer
            $voice = $syn.GetInstalledVoices()[0]
            $global:syn.SelectVoice($voice.VoiceInfo.Name)
            $global:syn.Volume = 100; #// Can be 1 - 100
            $global:syn.Rate = -2; #// can be -10 to 10 
        }
         return $global:syn
    }
}


<#
    .SYNOPSIS
    Reads outloud a text file to an mp3.
    
    .DESCRIPTION
    Reads the contents of the text file and 
    passes them to the SpeechSynthesizer to be spoken
    then saves that audio as an mp3.

    Output is placed into the Audio temp folder.

    .PARAMETER FileFullName
    The path to a text file.
    
    .PARAMETER MechanizeVoice
    Optional switch to have the content sound 
    mechanized.  Choosing this produces two separate
    files

    .RETURNS
    Full path to the created mp3 file
    
#>
function Send-TextToSpeechMp3
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true)]
        [string] $FileFullName,
        [Parameter(Mandatory=$false)]
        [switch] $MechanizeVoice
    )
    Process
    {
        #test dependency and inputs
        if(-not (Test-Path ([NoFuture.Tools.BinTools]::Ffmpeg))){
            Write-Host "The ffmpeg.exe is expected in the .\bin directory."
            break;
        }

        if(-not (Test-Path $FileFullName)){
            Write-Host "No such file found '$FileFullName'"
            break;
        }

        #powershell can deal with partials, SpeechSynthesizer and ffmpeg.exe will not
        $FileFullName = (Resolve-Path $FileFullName).Path

        #read the file into a string
        $text = [System.IO.File]::ReadAllText($FileFullName);

        if([string]::IsNullOrWhiteSpace($text)){
            Write-Host "The file is empty $FileFullName"
            break;
        }

        #output file of SpeechSynthesizer
        $wavFile = Join-Path ([NoFuture.TempDirectories]::Audio) ([System.IO.Path]::GetFileNameWithoutExtension($FileFullName) + ".wav")

        #output file of ffmpeg.exe
        $mp3File = Join-Path ([NoFuture.TempDirectories]::Audio) ([System.IO.Path]::GetFileNameWithoutExtension($wavFile) + ".mp3")

        #a rough guess - sending directly to file seems to go much faster than direcly listening 
        $timeToReadText = New-Object System.Timespan(0,0,0, [System.Math]::Round(($text.Length)/815.5))
        
        Write-Host "Estimated time to read files contents to [$wavFile] is '$timeToReadText'."

        if(Test-Path $wavFile){
            Remove-Item $wavFile -Force
        }

        #do not use the global variable for saving to file
        $speech = New-Object System.Speech.Synthesis.SpeechSynthesizer
        $voice = $speech.GetInstalledVoices()[0]
        $speech.SelectVoice($voice.VoiceInfo.Name)
        $speech.Volume = 100;
        $speech.Rate = -2;

        $speech.SetOutputToWaveFile($wavFile)

        #no async because we want to convert it immediately
        $speech.Speak($text)

        $speech.Dispose()

        #convert to mp3
        $ffmpegCmd = ("& {0} -i $wavFile -codec:a libmp3lame -qscale:a 2 $mp3File 2>&1" -f ([NoFuture.Tools.BinTools]::Ffmpeg))

        #remove the previous mp3 file
        if(Test-Path $mp3File){
            Remove-Item $mp3File -Force
        }

        #invoke the ffmpeg.exe
        Write-Host $ffmpegCmd -ForegroundColor Gray

        $ffmpegOutput = iex -Command $ffmpegCmd -ErrorAction Continue

        #remove the wav file from the drive
        if((Test-Path $wavFile) -and (Test-Path $mp3File)){
            [System.Threading.Thread]::Sleep(500)
            Remove-Item -Path $wavFile
        }

        #apply filters to make voice sound mechanized
        if($MechanizeVoice){

            #exe args to complex for powershell to not misinterpret
            $mechMp3File = Join-Path ([NoFuture.TempDirectories]::Audio) ([System.IO.Path]::GetFileNameWithoutExtension($mp3File) + ".Mechanized.mp3")

            if(Test-Path $mechMp3File){
                Remove-Item $mechMp3File -Force
            }

            $ffmpegCmd = "-i $mp3File -filter "
            $ffmpegCmd += "chorus=1.0:1.0:10.0:0.6:0.25:1,"
            $ffmpegCmd += "tremolo=60.0:0.9,"
            $ffmpegCmd += "aphaser=1.0:1.0:5.0:0.7:1.0:t,"
            $ffmpegCmd += "volume=2.0 "
            $ffmpegCmd += "$mechMp3File"

            $procArgs = New-Object System.Diagnostics.ProcessStartInfo -Property @{FileName = ([NoFuture.Tools.BinTools]::Ffmpeg); Arguments = $ffmpegCmd }
            $ffmpegProc = New-Object System.Diagnostics.Process -Property @{StartInfo = $procArgs}
            $ffmpegProc.Start()

            return $mechMp3File

        }
        else{
            return $mp3File
        }
    }
}


<#
    .synopsis
    Prints the given string to the console
    in large ascii-art characters
    
    .Description
    Given a string input the cmdlet will
    reprint it in large ascii-art characters.
    No console window length checking and therefore
    output may be jarred if window is small and input
    is large.
    
    .parameter ConsoleOutput
    Whatever is to be printed
    
    .parameter Color
    Optional Foreground color
    
    
#>
function Write-HostAsciiArtLarge
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,Position=0)]
        [string]$ConsoleOutput,
        [Parameter(Mandatory=$false,Position=1)]
        [AllowEmptyString()]
        [string]$Color
    )
    Process
    {
        if($Color -ne $null -and $Color.Trim() -ne "")
        {
            $consoleColor = $Color
        }
        else
        {
            $consoleColor = "Yellow"
        }
        $ConsoleOutput = $ConsoleOutput.ToLower()
        $printBlock = @("",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "")

        Format-AsciiArt $ConsoleOutput $alphaBig $printBlock
    }
}

<#
    .synopsis
    Prints the given string to the console
    in standard ascii-art characters
    
    .Description
    Given a string input the cmdlet will
    reprint it in large ascii-art characters.
    No console window length checking and therefore
    output may be jarred if window is small and input
    is large.
    
    .parameter ConsoleOutput
    Whatever is to be printed
    
    .parameter Color
    Optional Foreground color
    
    
#>
function Write-HostAsciiArt
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,Position=0)]
        [string]$ConsoleOutput,
        [Parameter(Mandatory=$false,Position=1)]
        [AllowEmptyString()]
        [string]$Color
    )
    Process
    {
        if($Color -ne $null -and $Color.Trim() -ne "")
        {
            $consoleColor = $Color
        }
        else
        {
            $consoleColor = "Yellow"
        }
        $ConsoleOutput = $ConsoleOutput.ToLower()
        $printBlock = @("",
                        "",
                        "",
                        "",
                        "")
        Format-AsciiArt $ConsoleOutput $alphaStd $printBlock
    }
}

function Format-AsciiArt($ConsoleOutput,$alpha,$printBlock)
{
        $letterArray = @()
        $ConsoleOutput.ToCharArray() | % {
            if($alpha.Keys -contains ([System.Convert]::ToString($_)))
            {
                $letterArray += $alpha.([System.Convert]::ToString($_))
            }
        }
        for($i=0;$i-lt$printBlock.Length;$i++)
        {
            for($j=0;$j-lt$letterArray.Length;$j++)
            {
                
                $printBlock[$i] += $letterArray[$j].Split("`n")[$i]
            }
        }
        $printBlock | % {
            Write-Host -ForegroundColor $consoleColor $_
        }

}
$alphaBig = @{

"a" = @'
     A       
     AA      
    A AA     
   AA  AA    
  AAAAAAAA   
 AA     AAA  
AA       AAA 
'@;

"b" = @'
BBBBBBBB     
BB     BB    
BB     BB    
BBBBBBBB     
BB      BB   
BB      BB   
BBBBBBBBB    
'@;
"c" = @'
    CCCCCC   
  CCC    CCC 
 CCC         
CCC          
 CCC         
  CCC    CCC 
    CCCCCC   
'@;
"d" = @'
DDDDDDDDD    
DD      DD   
DD       DD  
DD       DD  
DD       DD  
DD      DD   
DDDDDDDDD    
'@;
"e" = @'
EEEEEEEEEEE  
EE           
EE           
EEEEEEEEEE   
EE           
EE           
EEEEEEEEEEE  
'@;
"f" = @'
FFFFFFFFFFF  
FF           
FF           
FFFFFFFFF    
FF           
FF           
FF           
'@;
"g" = @'
    GGGGGG   
  GGG    GGG 
 GGG         
GGG          
 GGG   GGGGG 
  GGG    GGG 
    GGGGGG   
'@;
"h" = @'
HH       HH  
HH       HH  
HH       HH  
HHHHHHHHHHH  
HH       HH  
HH       HH  
HH       HH  
'@;
"i" = @'
IIIIIIIIIIII 
     II      
     II      
     II      
     II      
     II      
IIIIIIIIIIII 
'@;
"j" = @'
         JJ  
         JJ  
         JJ  
         JJ  
JJ       JJ  
 JJ     JJ   
  JJJJJJJ    
'@;
"k" = @'
KK      KK   
KK    KK     
KK  KK       
KKKKK        
KK   KK      
KK     KK    
KK      KKK  
'@;
"l" = @'
LL           
LL           
LL           
LL           
LL           
LL           
LLLLLLLLLLL  
'@;
"m" = @'
MM        MM 
MMM      MMM 
MM MM  MM MM 
MM   MM   MM 
MM        MM 
MM        MM 
MM        MM 
'@;
"n" = @'
NN        N  
NNN       N  
NN NN     N  
NN  NN    N  
NN   NN   N  
NN     NN N  
NN      NNN  
'@;
"o" = @'
    OOOOO    
  OOO   OOO  
 OOO     OOO 
 OO       OO 
 OOO     OOO 
  OOO   OOO  
    OOOOO    
'@;
"p" = @'
PPPPPPPPPP   
PP       PP  
PP       PP  
PPPPPPPPPP   
PP           
PP           
PP           
'@;
"q" = @'
    QQQQQ    
  QQQ   QQQ  
 QQQ     QQQ 
 QQ       QQ 
 QQQ   Q QQQ 
  QQQ   QQQ  
    QQQQQ QQ 
'@;
"r" = @'
RRRRRRRRRR   
RR       RR  
RR       RR  
RRRRRRRRRR   
RR     RR    
RR      RR   
RR       RR  
'@;
"s" = @'
   SSSSSS    
 SSS    SSS  
 SSS         
  SSSSSSS    
       SSS   
 SSS    SSS  
  SSSSSSSS   
'@;
"t" = @'
TTTTTTTTTTTT 
     TT      
     TT      
     TT      
     TT      
     TT      
     TT      
'@;
"u" = @'
UU        UU 
UU        UU 
UU        UU 
UU        UU 
UU        UU 
 UU      UU  
  UUUUUUUU   
'@;
"v" = @'
VV         VV
 VV       VV 
  VV     VV  
   VV   VV   
    VV VV    
     VVV     
      V      
'@;
"w" = @'
WW          W
WW          W
WW          W
 WW   W    W 
  WW WWW WW  
   WWW WWW   
    W   W    
'@;
"x" = @'
XX         XX
  XX      XX 
    XX  XX   
      XX     
   XX   XX   
 XX       XX 
XX         XX

"y" = @'
YY       YY  
  YY    YY   
   YY  YY    
    YYYY     
     YY      
     YY      
     YY      
'@;
"z" = @'
ZZZZZZZZZZZZ 
        ZZ   
      ZZ     
    ZZ       
  ZZ         
 ZZ          
ZZZZZZZZZZZZ 
'@;
" " = @'
             
             
             
             
             
             
             
'@;
"1" = @'
     111     
    11 1     
       1     
       1     
       1     
       1     
    111111   
'@;
"2" = @'
     2222    
   22   22   
  22     22  
       22    
     22      
   22        
  22222222   
'@;
"3" = @'
   333333    
  33    33   
        33   
      333    
        33   
  33    33   
   333333    
'@;
"4" = @'
    4  44    
   4   44    
  4    44    
 444444444   
       44    
       44    
       44    
'@;
"5" = @'
  55555555   
  55         
  55         
   555555    
        55   
        55   
  5555555    
'@;
"6" = @'
       66    
     66      
   66        
  66 66666   
  666    66  
   66   66   
     6666    
'@;
"7" = @'
   77777777  
         77  
        77   
       77    
      77     
     77      
    77       
'@;
"8" = @'
     8888    
   88    88  
   88    88  
     8888    
   88    88  
   88    88  
     8888    
'@;
"9" = @'
    999999   
  99     99  
  99     99  
    9999999  
         99  
        99   
      99     
'@;
"0" = @'
     0000    
   00    00  
   00    00  
   00    00  
   00    00  
   00    00  
     0000    
'@;
"!" = @'
      !!     
      !!     
      !!     
      !!     
      !!     
             
      !!     
'@;
"`"" = @'
             
   ""   ""   
   ""   ""   
             
             
             
             
'@;
"#" = @'
   ##    ##  
   ##    ##  
############ 
  ##    ##   
############ 
 ##    ##    
 ##    ##    
'@;
"$" = @'
      $      
     $$$$    
   $  $  $   
    $ $      
      $ $    
   $  $  $   
    $$$$$    
'@;
"%" = @'
             
   %%   %    
   %%  %     
      %      
     %  %%   
    %   %%   
             
'@;
"&" = @'
    &&&&     
   &   &&    
    & &&     
     &&      
   &&  & &&  
  &&    &&   
   &&&&&& &  
'@;
"'" = @'
             
     ''      
     ''      
     ''      
             
             
             
'@;
"(" = @'
     ((      
    ((       
   ((        
  ((         
   ((        
    ((       
     ((      
'@;
")" = @'
     ))      
      ))     
       ))    
        ))   
       ))    
      ))     
     ))      
'@;
"*" = @'
             
     **      
  ** ** **   
    ****     
  **    **   
             
             
'@;
"+" = @'
             
     ++      
     ++      
  ++++++++   
     ++      
     ++      
             
'@;
"," = @'
             
             
             
             
     ,,,     
      ,,,    
     ,,      
'@;
"-" = @'
             
             
             
   -------   
             
             
             
'@;
"." = @'
             
             
             
             
             
      ...    
      ...    
'@;
"/" = @'
       //    
      //     
     //      
    //       
   //        
  //         
 //          
'@;
";" = @'
             
    ;;;      
    ;;;      
             
    ;;;      
     ;;      
    ;        
'@;
"<" = @'
         <<  
       <<    
     <<      
   <<        
     <<      
       <<    
         <<  
'@;
"=" = @'
             
             
   =======   
             
   =======   
             
             
'@;
">" = @'
  >>         
    >>       
      >>     
        >>   
      >>     
    >>       
  >>         
'@;
"?" = @'
    ????     
  ??    ??   
   ?   ??    
      ??     
     ??      
             
     ??      
'@;
"@" = @'
     @@@     
   @@   @@   
  @  @@@@ @  
 @  @   @@@@ 
  @  @@@@  @ 
   @@    @@  
     @@@@    
'@;
"``" = @'
   ```       
    ```      
      ``     
        `    
             
             
             
'@;
"[" = @'
   [[[[[ 
   [     
   [     
   [     
   [     
   [     
   [[[[[ 
'@;
"]" = @'
   ]]]]]
       ]
       ]
       ]
       ]
       ]
   ]]]]]
'@;
"\" = @'
   \\        
    \\       
     \\      
      \\     
       \\    
        \\   
         \\  
'@;
"^" = @'
     ^^      
    ^^^^     
   ^^  ^^    
  ^^    ^^   
             
             
             
'@;
"_" = @'
             
             
             
             
             
             
_____________
'@;
"{" = @'
        {{   
      {{     
      {{     
    {{       
      {{     
      {{     
        {{   
'@;
"}" = @'
     }}      
       }}    
       }}    
         }}  
       }}    
       }}    
     }}      
'@;
"|" = @'
     ||      
     ||      
     ||      
     ||      
     ||      
     ||      
     ||      
'@;
"~" = @'
             
      ~~     
~~  ~~  ~~   
  ~~      ~~ 
             
             
             
'@
}


$alphaStd = @{
"a" = @'
       
  __ _ 
 / _` |
| (_| |
 \__,_|
'@;

"b" = @'
 _     
| |__  
| '_ \ 
| |_) |
|_.__/ 
'@;

"c" = @'
      
  ___ 
 / __|
| (__ 
 \___|
'@;

"d" = @'
     _ 
  __| |
 / _` |
| (_| |
 \__,_|
'@;

"e" = @'
      
  ___ 
 / _ \
|  __/
 \___|
'@;

"f" = @'
  __ 
 / _|
| |_ 
|  _|
|_|  
'@;

"g" = @'
  __ _ 
 / _` |
| (_| |
 \__, |
 |___/ 
'@;

"h" = @'
 _     
| |__  
| '_ \ 
| | | |
|_| |_|
'@;

"i" = @'
 _ 
(_)
| |
| |
|_|
'@;

"j" = @'
   _ 
  (_)
  | |
  | |
 _/ |
|__/ 
'@;

"k" = @'
 _    
| | __
| |/ /
|   < 
|_|\_\
'@;

"l" = @'
 _ 
| |
| |
| |
|_|
'@;

"m" = @'
           
 _ __ ___  
| '_ ` _ \ 
| | | | | |
|_| |_| |_|
'@;

"n" = @'
       
 _ __  
| '_ \ 
| | | |
|_| |_|
'@;

"o" = @'
       
  ___  
 / _ \ 
| (_) |
 \___/ 
'@;

"p" = @'
 _ __  
| '_ \ 
| |_) |
| .__/ 
|_|    
'@;

"q" = @'
  __ _ 
 / _` |
| (_| |
 \__, |
    |_|
'@;

"r" = @'
      
 _ __ 
| '__|
| |   
|_|   
'@;

"s" = @'
     
 ___ 
/ __|
\__ \
|___/
'@;

"t" = @'
 _    
| |_  
| __| 
| |_  
 \__| 
'@;

"u" = @'
       
 _   _ 
| | | |
| |_| |
 \__,_|
'@;

"v" = @'
       
__   __
\ \ / /
 \ V / 
  \_/  
'@;

"w" = @'
          
__      __
\ \ /\ / /
 \ V  V / 
  \_/\_/  
'@;

"x" = @'
      
__  __
\ \/ /
 >  < 
/_/\_\
'@;

"y" = @'
 _   _ 
| | | |
| |_| |
 \__, |
 |___/ 
'@;

"z" = @'
     
 ____
|_  /
 / / 
/___|
'@;

"_" = @'
       
       
       
 _____ 
|_____|
'@;

"+" = @'
   _   
 _| |_ 
|_   _|
  |_|  
       
'@;

"-" = @'
       
 _____ 
|_____|
       
       
'@;

"!" = @'
 _ 
| |
| |
|_|
(_)
'@;

"?" = @'
 ___ 
|__ \
  / /
 |_| 
 (_) 
'@;

"@" = @'
   ____  
  / __ \ 
 / / _` |
| | (_| |
 \ \__,_|
  \____/ 
'@;

"#" = @'
   _  _   
 _| || |_ 
|_  ..  _|
|_      _|
  |_||_|  
'@;

":" = @'
 _ 
(_)
 _ 
(_)
   
'@;

"," = @'
   
   
 _ 
( )
|/ 
'@;

"." = @'
   
   
   
 _ 
(_)
'@;
"/" = @'
     __
    / /
   / / 
  / /  
 /_/   
'@;
"\" = @'
 __    
 \ \   
  \ \  
   \ \ 
    \_\
'@;

"<" = @'
  __
 / /
/ / 
\ \ 
 \_\
'@;

">" = @'
__  
\ \ 
 \ \
 / /
/_/ 
'@;

"[" = @'
 | _|
 | | 
 | | 
 | | 
 |__|
'@;

"]" = @'
 |_ |
  | |
  | |
  | |
 |__|
'@;

"{" = @'
\ \  
 | | 
  > >
 | | 
/_/  
'@;

"}" = @'
  / /
 | | 
< <  
 | | 
  \_\
'@;

"=" = @'
        
  _____ 
 |_____|
 |_____|
        
'@;

"$" = @'
 | | 
/ __)
\__ \
(   /
 |_| 
'@;

"%" = @'
 _  __
(_)/ /
  / / 
 / /_ 
/_/(_)
'@;

"^" = @'
  /\ 
 |/\|
     
     
     
'@;

"&" = @'
  ___   
 ( _ )  
 / _ \/\
| (_>  <
 \___/\/
'@;

"(" = @'
  / /
 | | 
 | | 
 | | 
  \_\
'@;

")" = @'
 \ \ 
  | |
  | |
  | |
 /_/ 
'@;

"0" = @'
  ___  
 / _ \ 
| | | |
| |_| |
 \___/ 
'@;

"1" = @'
 _ 
/ |
| |
| |
|_|
'@;

"2" = @'
 ____  
|___ \ 
  __) |
 / __/ 
|_____|
'@;

"3" = @'
 _____ 
|___ / 
  |_ \ 
 ___) |
|____/ 
'@;

"4" = @'
 _  _   
| || |  
| || |_ 
|__   _|
   |_|  
'@;

"5" = @'
 ____  
| ___| 
|___ \ 
 ___) |
|____/ 
'@;

"6" = @'
  __   
 / /_  
| '_ \ 
| (_) |
 \___/ 
'@;
"7" = @'
 _____ 
|___  |
   / / 
  / /  
 /_/   
'@;

"8" = @'
  ___  
 ( _ ) 
 / _ \ 
| (_) |
 \___/ 
'@

"9" = @'
  ___  
 / _ \ 
| (_) |
 \__, |
   /_/ 
'@;

" " = @'
     
     
     
     
     
'@;
}