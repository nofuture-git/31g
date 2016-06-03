#these are too big to get at runtime 

$myScriptLocation = Split-Path $PSCommandPath -Parent

$nfRandAsm = Join-Path (Resolve-Path "$myScriptLocation\..\..\bin").Path "NoFuture.Rand.dll"

if(-not (Test-Path $nfRandAsm))	{ break;}

[System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($nfRandAsm))

$avgEarningByMsa = [NoFuture.Rand.Gov.Bea.Links]::BeaRegionalDataPJEARN_MI($false)
$avgEarningByCounty = [NoFuture.Rand.Gov.Bea.Links]::BeaRegionalDataPJEARN_MI($true)

$beaDataFile00 = "BeaRegionalDataPJEARN_MI.Msa"
$beaDataFile01 = "BeaRegionalDataPJEARN_MI.County"

Write-Progress -Activity "Calling bea.gov for $beaDataFile00" -Status "connecting"

$avgEarningByMsaJson = Invoke-RestMethod -Method Get -Uri $avgEarningByMsa
[System.IO.File]::WriteAllText((Join-Path $myScriptLocation "$beaDataFile00.json"), $avgEarningByMsaJson)

Write-Progress -Activity "Calling bea.gov for $beaDataFile01" -Status "connecting"
$avgEarningByCountyJson = Invoke-RestMethod -Method Get -Uri $avgEarningByCounty
[System.IO.File]::WriteAllText((Join-Path $myScriptLocation "$beaDataFile01.json"), $avgEarningByCountyJson)

#these require a post
$beaMicroSa2County = Invoke-RestMethod -Method Post -Uri "http://www.bea.gov/regional/docs/msalist.cfm" -Body "mlist=6&CSV=CSV"
$beaMetroSa2State = Invoke-RestMethod -Method Post -Uri "http://www.bea.gov/regional/docs/msalist.cfm" -Body "mlist=30&CSV=CSV"