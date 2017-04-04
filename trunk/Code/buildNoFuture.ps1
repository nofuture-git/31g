$msBuild2015 = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"

if(-not (Test-Path $msBuild2015)){
    $errorMsg = "Expecting Microsoft Build Tools 2015 at '$msBuild2015', but it is not there.  "
    $errorMsg += "Try downloading and installing it from https://www.microsoft.com/en-us/download/details.aspx?id=48159"
    throw $errorMsg
    break;
}

& "$msBuild2015" NoFuture.sln /t:build /tv:14.0 /m:8 /p:platform="any cpu" /p:configuration=debug /p:buildinparallel=true /p:CreateHardLinksForCopyFilesToOutputDirectoryIfPossible=true /p:CreateHardLinksForCopyAdditionalFilesIfPossible=true /p:CreateHardLinksForCopyLocalIfPossible=true /p:CreateHardLinksForPublishFilesIfPossible=true