Set-Location C:\Projects\31g\trunk

$pshost = Get-Host
$pswindow = $pshost.Ui.RawUi

$bufferSz = $pswindow.BufferSize

$bufferSz.Height = 3000
$bufferSz.Width = 105
$pswindow.BufferSize = $bufferSz

$windowSz = $pswindow.WindowSize
$windowSz.Height = 75
$windowSz.Width = 105
$pswindow.WindowSize = $windowSz

$pshost.UI.RawUI.WindowTitle = "NoFuture PS"

function Prompt { ">>>"}

[System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes((Resolve-Path ".\bin\NoFuture.Shared.dll").Path))

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
    
    

if (([Environment]::OSVersion.Version.Major -gt 5) -and [NoFuture.Aero]::DwmIsCompositionEnabled()) {
    $hwnd = ([System.Diagnostics.Process]::GetCurrentProcess()).mainwindowhandle
     
    $margin = new-object 'NoFuture.Aero+MARGINS'
    $margin.top = -1
    $margin.left = -1
     
     
    [NoFuture.Aero]::DwmExtendFrameIntoClientArea($hwnd, [ref]$margin)
     
}

