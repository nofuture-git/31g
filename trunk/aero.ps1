   
<#
    .SYNOPSIS
    Taken from http://poshcode.org/2052.  
    Sets the console window to be transparent with no borders

    .DESCRIPTION

    .LINKS
    http://poshcode.org/2052
#>     
function Set-ConsoleTransparent
{
    [CmdletBinding()]
    Param
    (
        [int]$Pid
    )
    Process
    {

        if (([Environment]::OSVersion.Version.Major -gt 5) -and
             [NoFuture.Aero]::DwmIsCompositionEnabled()) {
     
           $hwnd = (get-process -id $Pid).mainwindowhandle
     
           $margin = new-object 'NoFuture.Aero+margins'
     
           #$host.ui.RawUI.BackgroundColor = "white"
           #$host.ui.rawui.foregroundcolor = "black"
     
           if ($Disable) {
     
               $margin.top = 0
               $margin.left = 0
     
     
           } else {
     
               $margin.top = -1
               $margin.left = -1
     
           }
     
           [NoFuture.Aero]::DwmExtendFrameIntoClientArea($hwnd, [ref]$margin)
     
        } else {
     
           write-warning "Aero is either not available or not enabled on this workstation."
     
        }
    }
}

$signature = @"
           
            [DllImport("user32.dll")]  
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);  
     
            public static IntPtr FindWindow(string windowName){
                    return FindWindow(null,windowName);
            }
     
            [DllImport("user32.dll")]
            public static extern bool SetWindowPos(IntPtr hWnd,
            IntPtr hWndInsertAfter, int X,int Y, int cx, int cy, uint uFlags);
     
            [DllImport("user32.dll")]  
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
     
            static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
     
            const UInt32 SWP_NOSIZE = 0x0001;
            const UInt32 SWP_NOMOVE = 0x0002;
     
            const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
     
            public static void MakeTopMost (IntPtr fHandle)
            {
                    SetWindowPos(fHandle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }
     
            public static void MakeNormal (IntPtr fHandle)
            {
                    SetWindowPos(fHandle, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }
"@
     
     
    $app = Add-Type -MemberDefinition $signature -Name Win32Window -ReferencedAssemblies System.Windows.Forms -Using System.Windows.Forms -PassThru
    
    
    <#
    .SYNOPSIS
    Taken from http://poshcode.org/1837
    Sets a console window to always be on-top of other windows
    once it has gained focus.

    .DESCRIPTION

    .LINKS
    http://poshcode.org/1837

    .EXAMPLE
    PS C:\> gps powershell | Set-TopMost
   
    set powershell console on top of other windows

    .EXAMPLE
    PS C:\> gps powershell | Set-TopMost -disable

     unset

    .EXAMPLE
    PS C:\> Get-WindowByTitle *pad* | Set-TopMost
    
    set an application on top of other windows by its 
    windows title (wildcard is supported)

    .EXAMPLE
    PS C:\> Get-WindowByTitle textpad | Set-TopMost -Disable

    unset
    #> 
    function Set-TopMost
    {
            param(         
                    [Parameter(Position=0,ValueFromPipelineByPropertyName=$true)]
                    [Alias('MainWindowHandle')]$hWnd=0,     
                    [Parameter()]
                    [switch]$Disable
     
            )
     
           
            if($hWnd -ne 0)
            {
                    if($Disable)
                    {
                            Write-Verbose "Set process handle :$hWnd to NORMAL state"
                            $null = $app::MakeNormal($hWnd)
                            return
                    }
                   
                    Write-Verbose "Set process handle :$hWnd to TOPMOST state"
                    $null = $app::MakeTopMost($hWnd)
            }
            else
            {
                    Write-Verbose "$hWnd is 0"
            }
    }
     
     
     
    function Get-WindowByTitle($WindowTitle="*")
    {
            Write-Verbose "WindowTitle is: $WindowTitle"
           
            if($WindowTitle -eq "*")
            {
                    Write-Verbose "WindowTitle is *, print all windows title"
                    Get-Process | Where-Object {$_.MainWindowTitle} | Select-Object Id,Name,MainWindowHandle,MainWindowTitle
            }
            else
            {
                    Write-Verbose "WindowTitle is $WindowTitle"
                    Get-Process | Where-Object {$_.MainWindowTitle -like "*$WindowTitle*"} | Select-Object Id,Name,MainWindowHandle,MainWindowTitle
            }
    }
     
