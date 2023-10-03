$testCriminalDll = (Resolve-Path (".\bin\Debug\NoFuture.Law.Criminal.Tests.dll")).Path
$nunit = (Resolve-Path ("..\..\..\..\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe"))

function Test-NfRandLawCriminalMethod($MethodName){
    Invoke-Expression "$nunit $testCriminalDll --where `"method == $MethodName`""
}

function Test-NfRandLawCriminal(){
    Invoke-Expression "$nunit $testCriminalDll"
}