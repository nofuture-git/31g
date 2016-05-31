
function Get-UsInsuranceCompanyNames
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [switch] $Life,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $Health,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $Medicare,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $Supplemental,
        [Parameter(Mandatory=$false,position=4)]
        [switch] $Travel,
        [Parameter(Mandatory=$false,position=5)]
        [switch] $WorkersComp

    )
    Process
    {
        #crude selectors from markup at 20140818 bss
        $wikipediaInsCo = Request-File -Url "http://en.wikipedia.org/wiki/List_of_United_States_insurance_companies"
        if($Life){
            $startAt = $wikipediaInsCo.IndexOf("title=`"Insurance in the United States`"")
            $endAt = $wikipediaInsCo.IndexOf("title=`"Life annuity`"")
        }
        elseif($Health){
            $startAt = $wikipediaInsCo.IndexOf("title=`"Health insurance`"")
            $endAt = $wikipediaInsCo.IndexOf("id=`"Medicare`"")
        }
        elseif($Medicare){
            $startAt = $wikipediaInsCo.IndexOf("title=`"Medicare (United States)`"")
            $endAt = $wikipediaInsCo.IndexOf("id=`"Supplemental_insurance`"")
        }
        elseif($Supplemental){
            $startAt = $wikipediaInsCo.IndexOf("id=`"Supplemental_insurance`"")
            $endAt = $wikipediaInsCo.IndexOf("id=`"Travel_insurance`"")
        }
        elseif($Travel){
            $startAt = $wikipediaInsCo.IndexOf("title=`"Travel insurance`"")
            $endAt = $wikipediaInsCo.IndexOf("id=`"Workers.27_compensation`"")
        }
        elseif($WorkersComp){
            $startAt = $wikipediaInsCo.IndexOf("title=`"Workers' compensation`"")
            $tempString = $wikipediaInsCo.Substring($startAt, ($wikipediaInsCo.Length - $startAt))
            $endAt = $tempString.IndexOf("</ul>") + ("</ul>").Length #first index of after start...
        }

        if($startAt -eq 0 -and $endAt -eq 0){
            Write-Host "pick one of the switches available to the cmdlet and try again"
            break;
        }

        $insCoUlUnformatted = $wikipediaInsCo.Substring($startAt, ($endAt - $startAt))
        $startAt = $insCoUlUnformatted.IndexOf("<ul>")
        $endAt = $insCoUlUnformatted.LastIndexOf("</ul>")
        $insCoUl =  $insCoUlUnformatted.Substring($startAt, ($endAt + ("</ul>").Length - $startAt))
        $insCoUlList = $insCoUl.Split("`n")
        $insNameList = @()
        $insCoUlList | ? {$_ -match "\x22\x3e(.*)?\x3c\x2f"} | % {$insNameList+= $Matches[1]}
        $insNames = $insNameList | % {$_.Substring(0,($_.IndexOf("</")))}

        return $insNames

    }
}

$lifeList = Get-UsInsuranceCompanyNames -Life
$healthList = Get-UsInsuranceCompanyNames -Health
$medicareList = Get-UsInsuranceCompanyNames -Medicare
$supplList = Get-UsInsuranceCompanyNames -Supplemental
$travelList = Get-UsInsuranceCompanyNames -Travel

$insXmlDoc = New-Object System.Xml.XmlDocument

$insLifeNode = $insXmlDoc.CreateElement("category")
$insLifeAttr = $insXmlDoc.CreateAttribute("name")
$insLifeAttr.Value = "Life"
$doNotDisplay =  $insLifeNode.Attributes.Append($insLifeAttr)

$insHealthNode = $insXmlDoc.CreateElement("category")
$insHealthAttr = $insXmlDoc.CreateAttribute("name")
$insHealthAttr.Value = "Health"
$doNotDisplay =  $insHealthNode.Attributes.Append($insHealthAttr)

$insMedicareNode = $insXmlDoc.CreateElement("category")
$insMedicareAttr = $insXmlDoc.CreateAttribute("name")
$insMedicareAttr.Value = "Medicare"
$doNotDisplay =  $insMedicareNode.Attributes.Append($insMedicareAttr)

$insSupplementalNode = $insXmlDoc.CreateElement("category")
$insSupplementalAttr = $insXmlDoc.CreateAttribute("name")
$insSupplementalAttr.Value = "Supplemental"
$doNotDisplay =  $insSupplementalNode.Attributes.Append($insSupplementalAttr)

$insTravelNode = $insXmlDoc.CreateElement("category")
$insTravelAttr = $insXmlDoc.CreateAttribute("name")
$insTravelAttr.Value = "Travel"
$doNotDisplay =  $insTravelNode.Attributes.Append($insTravelAttr)

$lifeList | ? {-not ([string]::IsNullOrWhiteSpace($_)) } | % {
	$nNode = $insXmlDoc.CreateElement("com");
	$nAttr = $insXmlDoc.CreateAttribute("name")
	$nAttr.Value = $_
	$doNotDisplay = $nNode.Attributes.Append($nAttr)
	$doNotDisplay = $insLifeNode.AppendChild($nNode)
}

$healthList | ? {-not ([string]::IsNullOrWhiteSpace($_)) } | % {
	$nNode = $insXmlDoc.CreateElement("com");
	$nAttr = $insXmlDoc.CreateAttribute("name")
	$nAttr.Value = $_
	$doNotDisplay = $nNode.Attributes.Append($nAttr)
	$doNotDisplay = $insHealthNode.AppendChild($nNode)
}

$medicareList | ? {-not ([string]::IsNullOrWhiteSpace($_)) } | % {
	$nNode = $insXmlDoc.CreateElement("com");
	$nAttr = $insXmlDoc.CreateAttribute("name")
	$nAttr.Value = $_
	$doNotDisplay = $nNode.Attributes.Append($nAttr)
	$doNotDisplay = $insMedicareNode.AppendChild($nNode)
}

$supplList | ? {-not ([string]::IsNullOrWhiteSpace($_)) } | % {
	$nNode = $insXmlDoc.CreateElement("com");
	$nAttr = $insXmlDoc.CreateAttribute("name")
	$nAttr.Value = $_
	$doNotDisplay = $nNode.Attributes.Append($nAttr)
	$doNotDisplay = $insSupplementalNode.AppendChild($nNode)
}

$travelList | ? {-not ([string]::IsNullOrWhiteSpace($_)) } | % {
	$nNode = $insXmlDoc.CreateElement("com");
	$nAttr = $insXmlDoc.CreateAttribute("name")
	$nAttr.Value = $_
	$doNotDisplay = $nNode.Attributes.Append($nAttr)
	$doNotDisplay = $insTravelNode.AppendChild($nNode)
}


$insuranceNode = $insXmlDoc.CreateElement("insurance")
$doNotDisplay = $insuranceNode.AppendChild($insLifeNode)
$doNotDisplay = $insuranceNode.AppendChild($insHealthNode)
$doNotDisplay = $insuranceNode.AppendChild($insMedicareNode)
$doNotDisplay = $insuranceNode.AppendChild($insSupplementalNode)
$doNotDisplay = $insuranceNode.AppendChild($insTravelNode)

$rootNode = $insXmlDoc.CreateElement("noFuture")
$randNode = $insXmlDoc.CreateElement("rand")
$dataNode = $insXmlDoc.CreateElement("data")
$sourceNode = $insXmlDoc.CreateElement("source")

$doNotDisplay = $sourceNode.AppendChild($insuranceNode)
$doNotDisplay = $dataNode.AppendChild($sourceNode)
$doNotDisplay = $randNode.AppendChild($dataNode)
$doNotDisplay = $rootNode.AppendChild($randNode)

$insXmlDoc.AppendChild($rootNode)

$randDataPath = (Join-Path ([NoFuture.CustomTools]::CodeBase) "Rand\Data\Source")

$insXmlPath = Join-Path $randDataPath "US_InsCompanyNames.xml"

$insXmlDoc.Save($insXmlPath)
