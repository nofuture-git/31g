<#
	These data sources are either too big to load at runtime,
	they require a POST - which the Csv and Json FSharp
	Providers don't appear to handle, or are downloaded as a
	zip file which must be uncompressed.

	The FBI UCR are .xls format and require alot of manual
	modification before they are usable tsv.
#>

#current location
$myScriptLocation = Split-Path $PSCommandPath -Parent

#load dependent assemblies
@("Rand.Core", "Rand", "Rand.Data.Exo", "Rand.Data.Source", "Util.Core", "Util", "Shared.Core", "Shared") | % {
    $nfRandAsm = Join-Path (Resolve-Path "$myScriptLocation\..\..\bin").Path ("NoFuture.{0}.dll" -f $_)
    if(-not (Test-Path $nfRandAsm))	{ break;}
    [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($nfRandAsm))

}

#this data is used to solve for area-to-income (city\state)
$avgEarningByMsa = [NoFuture.Rand.Data.Exo.UsGov.Bea.Links]::BeaRegionalDataPJEARN_MI($false)
$avgEarningByCounty = [NoFuture.Rand.Data.Exo.UsGov.Bea.Links]::BeaRegionalDataPJEARN_MI($true)

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
#https://www.bls.gov/developers/api_faqs.htm#register1, 20 year limit
$startYear = $endYear - 19

$seriesIdsBySector.Keys | % {
	$sectorCode = $_
	$stateSeries = $seriesIdsBySector[$sectorCode]
	
	$payloadJson = [NoFuture.Rand.Data.Exo.UsGov.Bls.BlsSeriesBase]::GetMultiSeriesPostBody($stateSeries, $startYear,$endYear)
	$outFile = Join-Path $myScriptLocation ("BLS_SMU.{0}.{1}.{2}.json" -f $sectorCode,$startYear,$endYear)
	if(Test-Path $outFile){
		Remove-Item -Path $outFile -Force
	}
	Invoke-RestMethod -Method Post -Uri ([NoFuture.Rand.Data.Exo.UsGov.Bls.Globals]::PostUrl) -ContentType "application/json" -Body $payloadJson -OutFile $outFile
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

<#
FED data used to apply macroecon theory
#>
Add-Type -AssemblyName "System.IO.Compression.FileSystem"

#must assign proxy server elsewhere
$samplesDir = Join-Path $myScriptLocation "Samples"
$fedFrbUsData = (Join-Path $samplesDir "data_only_package.zip")
Invoke-WebRequest -Uri "https://www.federalreserve.gov/econresdata/frbus/files/data_only_package.zip" -Proxy ([NoFuture.Shared.Core.NfConfig.SecurityKeys]::ProxyServer)  -ProxyUseDefaultCredentials -OutFile $fedFrbUsData

[System.IO.Compression.ZipFile]::ExtractToDirectory($fedFrbUsData,$samplesDir)
$fedFrbUsData = Import-Csv (Join-Path $samplesDir "data_only_package\HISTDATA.TXT")
$filteredDataFnCd = Join-Path $samplesDir "data_only_package\filtered_comDem.HISTDATA.TXT"
$filteredDataFn = Join-Path $samplesDir "data_only_package\filtered.HISTDATA.TXT"

#reduce data to more undergrad levels
$targetedFedFeatures = @{
    "CENG"=@("Energy",$true)
    #"EC"=@("TotalConsumption",$true)
    "ECD"=@("ConsumerDurableGoodsExpend",$true);
    #"ECH"=@("ConsumerHomeSvcsExpend",$true);
    #"ECO"=@("ConsumerNonDurableExpend",$true);
    "EGF"=@("FedGovtExpend",$true);
    "EGS"=@("StateLocalGovtExpend",$true);
    #"EH"=@("HomeBuyers", $true);
    "EI"=@("ChangeBizInventory",$true);
    "EM"=@("Imports",$true);
    #"EMP"=@("PetroImports",$true);
    "EPD"=@("InvestmentEquipment",$true);
    "EPI"=@("InvestmentCopyright",$true);
    "EPS"=@("InvestmentBuildings",$true);
    "EX"=@("Exports",$true);
    #"FCBN"=@("USCurrentAcct", $true); #NetExports = EX-EM, NetFactorPmts = GFT /therefore FCBN = (EX-EM)+GFT
    "FPXR"=@("RealExchangeRate",$true);
    "FRL10"=@("ForeignLongTermInterestRate",$false);
    "FRS10"=@("ForeignShortTermInterestRate",$false);
    "GFT"=@("FedNetTransferPmts",$true); #payments made to SS, Medicare, etc.
    "GST"=@("StateLocalTransferPmts",$true);
    #"JRCD"=@("DeprecRateConsumerDurables",$false);
    #"JRH"=@("DeprecHousing",$false);
    "JRPD"=@("DeprecEquipment",$false);
    "JRPS"=@("DeprecBuildings",$false);
    "KI"=@("PrivateInventoryStock",$true);
    "KS"=@("CapitalSvcs",$true);
    "LFPR"=@("LaborRate",$false);
    "PIGDP"=@("InflationRate",$false);
    "PL"=@("Wages",$true);
    "RFF"=@("InterestRate",$false);
    "TRFCI"=@("FedBizTaxRate",$false);
    "TRFP"=@("FedHouseholdTaxRate",$false);
    "TRSCI"=@("StateLocalBizTaxRate",$false);
    "TRSP"=@("StateLocalHouseholdTaxRate",$false);
    "WPO"=@("HouseholdPropertyWealth",$true);
    "WPS"=@("HouseholdSecuritiesWealth",$true);
    "YDN"=@("DisposableIncome",$true);
    #"YH"=@("HouseholdIncome",$true);
    "YHSN"=@("PersonalSavings",$true);
    #"YPN"=@("PersonalIncome",$true)
}

$dataRow = @()
$dataRow += "Timeframe"

$targetedFedFeatures.Keys | % {
    $feature = $_
    $dataRow += $targetedFedFeatures[$feature][0]
}
[System.IO.File]::WriteAllText($filteredDataFnCd, [string]::Join("`t", $dataRow))
[System.IO.File]::AppendAllText($filteredDataFnCd,"`n")

$fedFrbUsData | % {
    $row = $_
    $dataRow = @()
    $dataRow += $row.OBS
   
    $targetedFedFeatures.Keys | % {
        $key = $_
        $vals = $targetedFedFeatures.$key
        $divBy100 = @("InterestRate", "ForeignShortTermInterestRate", "ForeignLongTermInterestRate", "InflationRate")
        if($vals[0] -eq "HouseholdPropertyWealth")#this is the highest value of all
        {
            $dataRow += ([Convert]::ToDouble($row.$key) / [Convert]::ToDouble($row.XGDO)) / 100
        }
        elseif($vals[1]){
            $dataRow += ([Convert]::ToDouble($row.$key) / [Convert]::ToDouble($row.XGDO))
        }
        elseif($divBy100 -contains $vals[0]){
            $dataRow += ([Convert]::ToDouble($row.$key) / 100)
        }
        else{
            $dataRow += $row.$key
        }
    }

    [System.IO.File]::AppendAllText($filteredDataFnCd, [string]::Join("`t", $dataRow))
    [System.IO.File]::AppendAllText($filteredDataFnCd,"`n")
}

<#
not normalized to ratio
#>
$dataRow = @()
$dataRow += "Timeframe"

$targetedFedFeatures.Keys | % {
    $feature = $_
    $dataRow += $targetedFedFeatures[$feature][0]
}
[System.IO.File]::WriteAllText($filteredDataFn, [string]::Join("`t", $dataRow))
[System.IO.File]::AppendAllText($filteredDataFn,"`n")

$fedFrbUsData | % {
    $row = $_
    $dataRow = @()
    $dataRow += $row.OBS
   
    $targetedFedFeatures.Keys | % {
        $key = $_
        $dataRow += $row.$key
    }

    [System.IO.File]::AppendAllText($filteredDataFn, [string]::Join("`t", $dataRow))
    [System.IO.File]::AppendAllText($filteredDataFn,"`n")
}

<#
BLS Occupation Employment
https://www.bls.gov/help/hlpforma.htm#OE

	                      1         2
	             1234567890123456789012345
	Series ID    OEUN000000011100011000001 
	Positions       Value           Field Name
	1-2             OE              Prefix
	3               U               Seasonal Adjustment Code 
	4               N               Area Type Code
	5-11            0000000         Area Code
	12-17           111000          Industry Code
	18-23           110000          Occupation Code
	24-25           01              Data Type Code
#>


#didn't waste any call to the API for these, just downloaded them from here https://www.bls.gov/oes/tables.htm
$natlOeData = @{}

@(4 .. 16) | % {
    $natlOeData += @{("20{0:00}" -f $_) =  Import-Csv -Delimiter ([char]"`t") -Path ("C:\Projects\31g\trunk\Code\NoFuture\Rand\Src\Samples\national_M20{0:00}_dl.txt" -f $_)}
}

$ternaryOeIds = $oeXml.SelectNodes("//occupations//ternary-group[contains(@ID,'-')]") | % {$_.ID}

#get the total as an actual sum of all numerators, not what is reported
$grandtotal = 0
:nextOeId foreach($oeId in $ternaryOeIds){
    $xmlNode = $oeXml.SelectSingleNode("//occupations//*[@ID='$oeId']")
    if($xmlNode -eq $null){
        continue nextOeId;
    }

    :nextYear foreach($year in $natlOeData.Keys){
        $yearData = $natlOeData[$year]
        $totEmp = ($yearData | ? {$_.OCC_CODE -eq $oeId} | Select-Object -First 1).TOT_EMP
        if($totEmp -is [string] -and $totEmp.Contains(",")){
            $totEmp = $totEmp.Replace(",","")
        }
        $pValue = 0
        if(-not [System.Int32]::TryParse($totEmp, [ref] $pValue)){
            continue nextYear;
        }
        
        $grandtotal += $pValue
        
    }

}

$normalScore = @{}
:nextOeId foreach($oeId in $ternaryOeIds){
    $xmlNode = $oeXml.SelectSingleNode("//occupations//*[@ID='$oeId']")
    if($xmlNode -eq $null){
        continue nextOeId;
    }
    $oeIdTotal = 0

    :nextYear foreach($year in $natlOeData.Keys){
        $yearData = $natlOeData[$year]
        $totEmp = ($yearData | ? {$_.OCC_CODE -eq $oeId} | Select-Object -First 1).TOT_EMP
        if($totEmp -is [string] -and $totEmp.Contains(",")){
            $totEmp = $totEmp.Replace(",","")
        }
        $pValue = 0
        if(-not [System.Int32]::TryParse($totEmp, [ref] $pValue)){
            continue nextYear;
        }
        
        $oeIdTotal += $pValue
        
    }
    $normalScore += @{$oeId = [System.Math]::Round(($oeIdTotal/$grandtotal)*100,5)}

    Write-Progress -Activity $oeId -Status "Working..." -PercentComplete ([System.Math]::Round($oeIdPercent))

}

$zipProb = [xml] (Get-Content C:\Projects\31g\trunk\Code\NoFuture\Rand\Data\Source\US_Zip_ProbTable.xml)
$prev = 0
$zipProbTable = @{}
$zipProb.SelectNodes("//zip-code") | % {
    $node = $_
    $prefix = $node.GetAttribute("prefix")
    $weight = $node.GetAttribute("weight")

    $prob = $weight - $prev
    $zipProbTable += @{$prefix = $prob}
    $prev = $weight
}