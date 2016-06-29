<#
	These data sources are either too big to load at runtime
	or they require a POST - which the Csv and Json FSharp
	Providers don't appear to handle.

	The FBI UCR are .xls format and require alot of manual
	modification before they are usable tsv.
#>

#current location
$myScriptLocation = Split-Path $PSCommandPath -Parent

#load dependent assemblies
$nfRandAsm = Join-Path (Resolve-Path "$myScriptLocation\..\..\bin").Path "NoFuture.Shared.dll"
if(-not (Test-Path $nfRandAsm))	{ break;}
[System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($nfRandAsm))

$nfRandAsm = Join-Path (Resolve-Path "$myScriptLocation\..\..\bin").Path "NoFuture.Util.dll"
if(-not (Test-Path $nfRandAsm))	{ break;}
[System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($nfRandAsm))

$nfRandAsm = Join-Path (Resolve-Path "$myScriptLocation\..\..\bin").Path "NoFuture.Rand.dll"
if(-not (Test-Path $nfRandAsm))	{ break;}
[System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($nfRandAsm))

#this data is used to solve for area-to-income (city\state)
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

#this is x-ref data for city-to-zip
$beaMicroSa2County = Invoke-RestMethod -Method Post -Uri "http://www.bea.gov/regional/docs/msalist.cfm" -Body "mlist=6&CSV=CSV"
$beaMetroSa2State = Invoke-RestMethod -Method Post -Uri "http://www.bea.gov/regional/docs/msalist.cfm" -Body "mlist=30&CSV=CSV"

#this data is used to solve for area-to-sector employment
$notUsStates = @("Puerto Rico", "Virgin Islands", "District of Columbia")#BLS limits to 50 series per POST

$smStates = Invoke-RestMethod -Method Get -Uri "http://download.bls.gov/pub/time.series/sm/sm.state"
$smStates = ConvertFrom-Csv -Delimiter ([char]"`t") -InputObject $smStates

$smSuperSectors = @(1 .. 9) | % {$_ * 10}

$seriesIdsBySector = @{}
$smSuperSectors | % { $seriesIdsBySector += @{$_.ToString() = @()} }

$smStates | ? {$notUsStates -notcontains $_.state_name} | % {
	$smStateCode = $_.state_code
	$smSuperSectors | % {
		$sectorCode = $_
		$seriesId = "SMU{0:00}00000{1:00}00000001" -f $smStateCode,$sectorCode
		$seriesIdsBySector[$sectorCode.ToString()] += $seriesId
	}	
}

$endYear = [System.DateTime]::Today.Year - 1
#http://www.bls.gov/developers/api_faqs.htm#register1, 20 year limit
$startYear = $endYear - 19

$seriesIdsBySector.Keys | % {
	$sectorCode = $_
	$stateSeries = $seriesIdsBySector[$sectorCode]
	
	$payloadJson = [NoFuture.Rand.Gov.Bls.BlsSeriesBase]::GetMultiSeriesPostBody($stateSeries, $startYear,$endYear)
	$outFile = Join-Path $myScriptLocation ("BLS_SMU.{0}.{1}.{2}.json" -f $sectorCode,$startYear,$endYear)
	if(Test-Path $outFile){
		Remove-Item -Path $outFile -Force
	}
	Invoke-RestMethod -Method Post -Uri ([NoFuture.Rand.Gov.Bls.Globals]::PostUrl) -ContentType "application/json" -Body $payloadJson -OutFile $outFile
} 

<#
FBI Uniform Crime Report uris 

These basically stack with the exception of a revised def. of Rape starting in the 2014 report.
I assume that the "Legacy" def. is what is labelled in the other reports as "Forceable Rape"
#>
$fbiUcr = @(
"https://www.fbi.gov/about-us/cjis/ucr/crime-in-the-u.s/2014/crime-in-the-u.s.-2014/tables/table-4/table_4_crime_in_the_united_states_by_region_geographic_division_and_state_2013-2014.xls/output.xls",
"https://www.fbi.gov/about-us/cjis/ucr/crime-in-the-u.s/2012/crime-in-the-u.s.-2012/tables/4tabledatadecoverviewpdf/table_4_crime_in_the_united_states_by_region_geographic_division_and_state_2011-2012.xls",
"https://www.fbi.gov/about-us/cjis/ucr/crime-in-the-u.s/2010/crime-in-the-u.s.-2010/tables/10tbl04.xls",
"https://www2.fbi.gov/ucr/cius2008/data/documents/08tbl04.xls",
"https://www2.fbi.gov/ucr/cius2006/data/documents/06tbl04.xls"
)
