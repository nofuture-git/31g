cd "C:\Projects\31g\trunk"
function CopyTo-ClipBoard($val){[System.Windows.Forms.Clipboard]::SetText($val)}
Set-Alias c2cb CopyTo-ClipBoard
. .\wsdl.ps1
. .\aero.ps1
Set-ConsoleTransparent
$host.UI.RawUI.ForegroundColor = "White"
[System.Reflection.Assembly]::LoadWithPartialName("System.ServiceModel")
[System.Reflection.Assembly]::LoadWithPartialName("System.Data")
[System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes("C:\Projects\31g\trunk\bin\NoFuture.Shared.dll"))
[System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes("C:\Projects\31g\trunk\bin\NoFuture.Util.dll"))
[System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes("C:\Projects\31g\trunk\bin\NoFuture.Rand.dll"))
[NoFuture.BinDirectories]::Root = "C:\Projects\31g\trunk\bin"
clear