$blsGovRoot = "http://download.bls.gov/pub/time.series/"
$targetCodes = @(
    "ip/ip.sector",
    "ip/ip.industry",
    "ip/ip.measure",
    "ip/ip.duration",
    "wp/wp.group",
    "wp/wp.item",
    "ec/ec.compensation",
    "ec/ec.group",
    "ec/ec.ownership",
    "ec/ec.period",
    "cu/cu.area",
    "cu/cu.item",
    "ce/ce.supersector",
    "ce/ce.industry",
    "ce/ce.datatype",
	"sm/sm.state",
	"sm/sm.area",
	"sm/sm.supersector",
	"sm/sm.industry",
	"sm/sm.data_type"
)
$blsOutNs = "NoFuture.Rand.Gov.Bls.Codes"
$blsOutPath = "C:\Projects\31g\trunk\Code\NoFuture\Rand\Gov\Bls\Codes"

function Get-BlsGovCodes
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path,
        [Parameter(Mandatory=$true,position=1)]
        [string] $UrlPath,
        [Parameter(Mandatory=$true,position=2)]
        [string] $Namespace

    )
    Process
    {

        $Url = $blsGovRoot + $UrlPath
        $Name = ($UrlPath.ToString().Split("/")[1])
        $DataPath = (Join-Path $Path ($Name + ".txt"))
        $Content = Request-File -Url $Url

        #some are not encoded correctly as tab delimited
        $fiveSp = New-Object System.String(([char]0x20),5)
        $tabAsStr = New-Object System.String(([char]0x09))
        if($Content.Contains($fiveSp)){
            $Content = $Content.Replace($fiveSp, $tabAsStr)
        }
        $Data = [System.Text.Encoding]::UTF8.GetBytes($Content)

        [System.IO.File]::WriteAllBytes($DataPath,$Data)

        Convert-CsvToCsCode -Path $DataPath -Delimiter ([char]0x09) -OutputPath $Path -Namespace $Namespace

    }
}

$errorsCount = $Error.Count
$targetCodes | % {
    $tCode = $_;
    Write-Host "working $tCode" -ForegroundColor Yellow
    Get-BlsGovCodes -Path $blsOutPath -UrlPath $tCode -Namespace $blsOutNs

    if($Error.Count -gt $errorsCount){
        Read-Host "error encounter, press any key to exit..."
        break;
    }
}

ls $blsOutPath -Filter *.cs | % {[NoFuture.Util.NfPath]::ConvertToCrLf($_.FullName)}

Read-Host "Done, press any key to exit..."

