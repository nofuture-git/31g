try{
if(-not [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-RandomPerson",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-RandomCompany",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
<#
    .SYNOPSIS
    Creates "Person" having pertinent properties randomly generated.
    
    .DESCRIPTION
    Creates a list of properties that are typical of a 'person' in a business environment.
    The properties are random and are typically non-sense.  The values of state, zip and 
    city, however, are related.  The script depends calls the the NoFuture.Rand dll.
    
    .PARAMETER RegionCode
    The ISO 3166 country code (US) or full country-culture code (eg en-US).
    The default is US.

    .PARAMETER FromWeb
    A switch that instructs the cmdlet to use its random city & state
    to call Google API services and get a valid street address(es).
    This slows down the cmdlet and causes a progress bar to be displayed.
    
    .EXAMPLE
    C:\PS>$myRandomPerson = Get-RandomPerson
    
    .OUTPUTS
    NoFuture.Rand.Domus.Person
    
#>
function Get-RandomPerson
{
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$false,position=0)]
        [String] $RegionCode,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $FromWeb

    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($RegionCode)){ $RegionCode = "US"}

        if($RegionCode -eq "CA"){
            throw "Not Implemented";
        }
        else{
            $person = [NoFuture.Rand.Domus.US.American]::RandomAmerican()
        }

        $webSuccess = $false
        if($FromWeb){

            Write-Progress -Activity "Creating Random Person" -Status "Getting data from web" -PercentComplete (48)
            $webSuccess = Get-RealAddressData -RandomPerson $person
            Write-Progress -Activity "Creating Random Person" -Status "Done getting data from web" -PercentComplete (97)
        }

        $person | Add-Member NoteProperty IsAddrFromWeb $webSuccess
        return $person   
    }
}

<#
    .SYNOPSIS
    Gets a public company at random.
    
    .DESCRIPTION
    First gets a SIC code at random, then calls the Edgar Sec.gov to get
    a list of companies by said SIC. Next locks in on one in particular,
    at random, and gets detailed info from the SEC and Yahoo Finance.
    
    .PARAMETER RegionCode
    The ISO 3166 country code (US) or full country-culture code (eg en-US).
    Only implemented for US.

    .PARAMETER JustCik
    Returns faster having just fetched a name and SEC id.
    
    .PARAMETER UseDefaultProxy
    Will assign the WebRequest's Proxy property to 
    the instance at SecurityKeys.ProxyServer.
    
    .EXAMPLE
    C:\PS>$myRandomCompany = Get-RandomCompany
    
    .OUTPUTS
    NoFuture.Rand.Com.PublicCorporation
    
#>
function Get-RandomCompany
{
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$false,position=0)]
        [String] $RegionCode,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $JustCik,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $UseDefaultProxy

    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($RegionCode)){ $RegionCode = "US"}

        if($RegionCode -ne "US"){
            throw "only implemented for US"
        }

        Write-Progress -Activity "Getting SEC code at random" -Status "OK" -PercentComplete 4

        #get a sic code 
        $sic = [NoFuture.Rand.Org.StandardIndustryClassification]::RandomStandardIndustryClassification();
        if($sic -eq $null){
            Write-Host "no StandardIndustryClassification returned";
            break;
        }

        Write-Progress -Activity ("Calling SEC.gov on SIC code '{0}' ('{1}')" -f $sic.Value, $sic.Description) -Status "OK" -PercentComplete 16

        #get corps by sic code
        $searchCrit = New-Object NoFuture.Rand.Exo.NfXml.Edgar+FullTextSearch
        $searchCrit.SicCode = $sic.Value
        $searchCritUri = [NoFuture.Rand.Exo.NfXml.SecFullTxtSearch]::GetUri($searchCrit)
        $rssContent = Request-File -Url $searchCritUri -UseDefaultProxy:$UseDefaultProxy

        if($rssContent -eq $null){
            Write-Host "no content found";
            break;
        }
        $publicCorps = ([NoFuture.Rand.Exo.Copula]::ParseSecEdgarFullTextSearch($rssContent, $searchCritUri))
        
        if($publicCorps -eq $null -or $publicCorps.Count -le 0){
            $longMsg = ("The random SIC code {0} didn't get any hits from the SEC." -f $sic.Value)
            $longMsg += "`nUpdate the 'US_EconSectors_Data.xml' for this code and re-try."
            Write-Host $longMsg -ForegroundColor Yellow
            break;
        }

        #pick one at random
        $pickOne = [NoFuture.Rand.Core.Etx]::RandomInteger(0, ($publicCorps.Length -1))
        $randomCorp = $publicCorps[$pickOne]

        if($JustCik){
            return $randomCorp
        }

        Write-Progress -Activity ("Calling SEC.gov on CIK Id '{0}' ('{1}')" -f $randomCorp.CIK, $randomCorp.Name) -Status "OK" -PercentComplete 32
        
        #get details from SEC
        $cikUri = [NoFuture.Rand.Exo.NfXml.SecCikSearch]::GetUri($randomCorp.CIK)
        $xmlContent = Request-File -Url $cikUri.AbsoluteUri -UseDefaultProxy:$UseDefaultProxy

        $tryParseResult = ([NoFuture.Rand.Exo.Copula]::TryParseSecEdgarCikSearch($xmlContent, $cikUri, [ref] $randomCorp))

        if(-not $tryParseResult){
            return $randomCorp;
        }

        Write-Progress -Activity ("Getting finance info for Company '{0}' " -f $randomCorp.Name) -Status "OK" -PercentComplete 64

        :nextAr foreach($annualRpt in $randomCorp.SecReports){
            
            if($annualRpt -isnot [NoFuture.Rand.Gov.US.Sec.Form10K])
            {
                continue nextAr;
            }
            
            $htmlContent = Request-File -Url $annualRpt.HtmlFormLink.ToString() -UseDefaultProxy:$UseDefaultProxy
            if(-not ([NoFuture.Rand.Exo.Copula]::TryGetXmlLink($htmlContent, $annualRpt.HtmlFormLink.ToString(), [ref] $randomCorp))) {
                continue nextAr;
            }

            $xmlContent = Request-File -Url $annualRpt.XmlLink.ToString() -UseDefaultProxy:$UseDefaultProxy
            if(-not ([NoFuture.Rand.Exo.Copula]::TryMergeXbrlInto10K($xmlContent, $annualRpt.XmlLink.ToString(), [ref] $randomCorp))) {
                continue nextAr;
            }
        }

        if($randomCorp.TickerSymbols.Count -gt 0){

            Write-Progress -Activity ("Getting the key statistics data for Company '{0}' " -f $randomCorp.Name) -Status "OK" -PercentComplete 76

            $keyStatsUri = [NoFuture.Rand.Exo.NfJson.IexKeyStats]::GetUri($randomCorp.TickerSymbols[0].ToString())
            $jsonData = Request-File $keyStatsUri -UseDefaultProxy:$UseDefaultProxy
            $tryParseResult = [NoFuture.Rand.Exo.Copula]::TryParseIexKeyStatsJson($jsonData, $keyStatsUri, [ref] $randomCorp)

            if($tryParseResult){
                Write-Progress -Activity ("Getting the more company info for Company '{0}' " -f $randomCorp.Name) -Status "OK" -PercentComplete 89

                $companyInfoUri = [NoFuture.Rand.Exo.NfJson.IexCompany]::GetUri($randomCorp.TickerSymbols[0].ToString())
                $jsonData = Request-File $companyInfoUri -UseDefaultProxy:$UseDefaultProxy
                $tryParseResult = [NoFuture.Rand.Exo.Copula]::TryParseIexCompanyJson($jsonData, $keyStatsUri, [ref] $randomCorp)
            }

        }
        Write-Progress -Activity ("Done!" -f $randomCorp.Name) -Status "OK" -PercentComplete 100
        return $randomCorp
    }
}
#====

function Get-RealAddressData($RandomPerson){
    try
    {
        #get lat & lng for the given random city and state
        $csz = [NoFuture.Rand.Geo.US.UsCityStateZip]($RandomPerson.GetAddressAt($null).HomeCityArea)
        $geocode = Get-GoogleGeocode -City $csz.City -State $csz.PostalState

        #check object graph has required data
        if($geocode -eq $null -or 
           $geocode.results -eq $null -or 
           $geocode.results.geometry -eq $null -or
           $geocode.results.geometry.location -eq $null -or
           $geocode.results.geometry.location.lat -eq $null -or 
           $geocode.results.geometry.location.lng -eq $null ){return $false;}

        #get locations in vincinity
        if($geocode.results.geometry.location.lat -is [array]){
            $lat = $geocode.results.geometry.location.lat[0]
        }
        else{
            $lat = $geocode.results.geometry.location.lat
        }

        if($geocode.results.geometry.location.lng -is [array]){
            $lng = $geocode.results.geometry.location.lng[0]
        }
        else{
            $lng = $geocode.results.geometry.location.lng
        }

        #try and get the "County" from the results 
        if($geocode.results.address_components -ne $null){
            $county = $geocode.results.address_components | ? {$_.long_name -like "*County"} | Select-Object -First 1
            $RandomPerson | Add-Member -MemberType NoteProperty -Name "County" -Value $county.long_name -Force
        }

        $places = Get-GooglePlaces -Latitude $lat -Longitude $lng

        #check object graph has required data
        if($places -eq $null -or 
           $places.results -eq $null -or 
           $places.results.Count -le 0){return $false;}
        
        #pick an entry at random
        $pickone = [NoFuture.Rand.Core.Etx]::RandomInteger(0,$places.results.Count-1)

        #validate random entry
        if($places.results[$pickone] -eq $null -or 
           $places.results[$pickone].vicinity -eq $null) { return $false; }

        $placesAddr = $places.results[$pickone].vicinity

        if($placesAddr.Contains(",")){
            $addrLine = $placesAddr.Split(",")[0]
        }
        else {
            $addrLine = $placesAddr
        }

        [NoFuture.Rand.Geo.US.UsStreetPo] $parsedAddr = $null;
        if(-not ([NoFuture.Rand.Geo.US.UsStreetPo]::TryParse($addrLine, [ref] $parsedAddr))){
            return $false
        }
        
        $parsedAddr.Data.Lng = $lng
        $parsedAddr.Data.Lat = $lat
        $parsedAddr.Src = "https://maps.googleapis.com/maps/api/geocode/json"
        $homeAddr = New-Object NoFuture.Rand.Geo.PostalAddress -Property @{Street = $parsedAddr; CityArea = $csz}

        $RandomPerson.AlignCohabitantsHomeDataAt($null, $homeAddr);

        return $true;
    }
    catch [System.Exception]
    {
        Write-Host "An error occured, check the global 'error' variable for more details."
        return $false;
    }
}