$nfReadVsDir = "C:\Projects\31g\trunk\Code\NoFuture\Read\Ini\Vs"

$dotNetRoot = "C:\Windows\Microsoft.NET\Framework"

$projFileXsdCmd = "& C:\Projects\31g\trunk\bin\x86\v4.0\xsd.exe $dotNetRoot\v3.5\Microsoft.Build.xsd /c /n:NoFuture.Read.Vs.Net35 /out:$nfReadVsDir"
Invoke-Expression -Command $projFileXsdCmd
Rename-Item -Path "$nfReadVsDir\Microsoft_Build.cs" -NewName "$nfReadVsDir\Microsoft_Build.net35.cs" -Force

[System.Threading.Thread]::Sleep(500)

$projFileXsdCmd = "& C:\Projects\31g\trunk\bin\x86\v4.0\xsd.exe $dotNetRoot\v4.0.30319\Microsoft.Build.xsd /c /n:NoFuture.Read.Vs.Net40 /out:$nfReadVsDir"
Invoke-Expression -Command $projFileXsdCmd
Rename-Item -Path "$nfReadVsDir\Microsoft_Build.cs" -NewName "$nfReadVsDir\Microsoft_Build.net40.cs" -Force
