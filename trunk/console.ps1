try{
if(-not [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Write-HostAsciiArtLarge",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Write-HostAsciiArt",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-Speech",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Out-FunnyComputerBeeps",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Send-TextToSpeechMp3",$MyInvocation.MyCommand)
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

    for($i=0;$i-lt16;$i++){
        $freq = $rand.Next(64,2664)
        $len = $rand.Next(100,300)
        $doNoDisplay = [NoFuture.BeepMarshal]::Beep($freq,$len)
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
    https://docs.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer

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
            $voice = $global:syn.GetInstalledVoices()[1]
            $global:syn.SelectVoice($voice.VoiceInfo.Name)
            $global:syn.Volume = 100; #// Can be 1 - 100
            $global:syn.Rate = 0; #// can be -10 to 10 
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
        if(-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+PythonTools]::Ffmpeg))){
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
        $wavFile = Join-Path ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Audio) `
                             ([System.IO.Path]::GetFileNameWithoutExtension($FileFullName) + ".wav")

        #output file of ffmpeg.exe
        $mp3File = Join-Path ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Audio) `
                             ([System.IO.Path]::GetFileNameWithoutExtension($wavFile) + ".mp3")

        #a rough guess - sending directly to file seems to go much faster than direcly listening 
        $timeToReadText = New-Object System.Timespan(0,0,0, [System.Math]::Round(($text.Length)/815.5))
        
        Write-Host "Estimated time to read files contents to [$wavFile] is '$timeToReadText'."

        if(Test-Path $wavFile){
            Remove-Item $wavFile -Force
        }

        #do not use the global variable for saving to file
        $speech = New-Object System.Speech.Synthesis.SpeechSynthesizer
        $voice = $speech.GetInstalledVoices()[1]
        $speech.SelectVoice($voice.VoiceInfo.Name)
        $speech.Volume = 100;

        $speech.SetOutputToWaveFile($wavFile)

        #no async because we want to convert it immediately
        $speech.Speak($text)

        $speech.Dispose()

        #convert to mp3
        $ffmpegCmd = ("& {0} -i $wavFile -codec:a libmp3lame -qscale:a 2 $mp3File 2>&1" -f `
                                  ([NoFuture.Shared.Cfg.NfConfig+PythonTools]::Ffmpeg))

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
            $mechMp3File = Join-Path ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Audio) `
                                     ([System.IO.Path]::GetFileNameWithoutExtension($mp3File) + ".Mechanized.mp3")

            if(Test-Path $mechMp3File){
                Remove-Item $mechMp3File -Force
            }

            $ffmpegCmd = "-i $mp3File -filter "
            $ffmpegCmd += "chorus=1.0:1.0:10.0:0.6:0.25:1,"
            $ffmpegCmd += "tremolo=60.0:0.9,"
            $ffmpegCmd += "aphaser=1.0:1.0:5.0:0.7:1.0:t,"
            $ffmpegCmd += "volume=2.0 "
            $ffmpegCmd += "$mechMp3File"

            $procArgs = New-Object System.Diagnostics.ProcessStartInfo -Property @{
                FileName = ([NoFuture.Shared.Cfg.NfConfig+PythonTools]::Ffmpeg); 
                Arguments = $ffmpegCmd 
            }
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