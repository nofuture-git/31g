$beaOutNs = "NoFuture.Rand.Gov.Bea.Parameters"
$beaOutPath = "C:\Projects\31g\trunk\Code\NoFuture\Rand\Gov\Bea\Parameters"
$beaBaseCls = "NoFuture.Rand.Gov.Bea.BeaParameter"

function Get-BeaGovParameter
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $DataSetName,
        [Parameter(Mandatory=$true,position=1)]
        [string] $ParameterName
    )
    Process
    {

        $parameterUri = [NoFuture.Rand.Gov.Bea.Links]::GetBeaParameterValuesUri($DataSetName, $ParameterName)
        $parameterValueJson  = ConvertFrom-Json -InputObject (Request-File -Url $parameterUri)

        if($parameterValueJson.BEAAPI.Results.Error -ne $null){
	        $errDescription = $parameterValueJson.BEAAPI.Results.Error.APIErrorDescription
	        throw "BEA Error '$errDescription'"
            break;
        }

        #generated in context of namespace since the names may repeat across datasets but the values therein may not.
        $cleanDataSetName = [NoFuture.Util.Etc]::CapitalizeFirstLetterOfWholeWords($DataSetName,$null).Replace(".",[string]::Empty);
        $beaOutNsPath = Join-Path $beaOutPath $cleanDataSetName
        if(-not (Test-Path $beaOutNsPath)){
            mkdir $beaOutNsPath -Force
        }
        Convert-PsObjsToCsCode -ArrayOfPsObjs $parameterValueJson.BEAAPI.Results.ParamValue -Name $parameterName -OutputDir $beaOutNsPath -Namespace ($beaOutNs + "." + $cleanDataSetName) -Extends $beaBaseCls -AddOverrideKeywordToProps

    }
}

[NoFuture.GlobalSwitches]::PrintWebHeaders = $false

$targetDsParams = @{
   "RegionalData" = @("KeyCode","GeoFips");
   "NIPA" = @("TableID");
   "NIUnderlyingDetail" = @("TableID");
   #"MNE" = @();
   #"FixedAssets" = @("TableID");
   "ITA" = @("Indicator", "AreaOrCountry");
   "IIP" = @("TypeOfInvestment", "Component");
   "GDPbyIndustry" = @("Industry","TableID")

}

#foreach bea parameter in this dataset
$targetDsParams.Keys | % {
    $dsname = $_
    $targetDsParams[$dsname] | ? {$_ -ne $null -and -not ([string]::IsNullOrWhiteSpace($_))} | % {
        $pName = $_
        Write-Host "working dataset '$dsname', parameter '$pName'" -ForegroundColor Yellow
        #gen the code
        Get-BeaGovParameter -DataSetName $dsname -ParameterName $pName
    }
}

[NoFuture.GlobalSwitches]::PrintWebHeaders = $true

Read-Host "Done, press any key to exit..."