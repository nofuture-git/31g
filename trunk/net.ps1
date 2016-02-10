try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-Ie",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-Ip",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-Socket",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-DnsName",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-AdverstisingDomainNames",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Request-File",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-InUsePorts",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-Netstat",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-GooglePlaces",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-GoogleGeocode",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-SecCompaniesBySic",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-SecCompanyDetail",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
<#
    .SYNOPSIS
    Attach to or create an instance of IE using WatiN
    
    .DESCRIPTION
    Will attach to or create an instance of IE using WatiN.
    In addition to WatiN the returned instance will have 
    a handful of utility Script Methods attached.  These 
    added methods always start with the prefix of 'Ps'.
    
    Leave both arguements empty to have the CmdLet create a 
    new instance of IE.
    
    Use the WatiN objects Dispose() method to release Powershell
    from the running instance of IE, by default the running instance
    will not be closed.
    
    .PARAMETER Url
    The complete or partial URL of a running instance of IE.
    
    .PARAMETER Title
    The complete or partial Title of a running instance of IE
    
    .EXAMPLE
    C:\PS>$myIe = Get-Ie -Url "https://rally1.rallydev.com/slm/login.op"
    C:\PS>$myIe.FileUpload([WatiN.Core.Find]::ById("profile_file")).Text = "C:/Desktop/image.jpg"
    
    .OUTPUTS
    WatiN.Core.IE
    
#>
function Get-Ie
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [AllowEmptyString()]
        [string] $Url,
        [Parameter(Mandatory=$false,position=1)]
        [AllowEmptyString()]
        [string] $Title
    )
    Process
    {
        if(([AppDomain]::CurrentDomain.GetAssemblies() | % { $_.GetName().Name}) -notcontains "WatiN.Core")
        {
            throw ("WatiN.Core is not loaded into the AppDomain, check the path found in 'start.ps1' and try again.")
        }

        if($Url.Trim() -ne ""){
        	#need to cast to a ps object to have extension returned on the reference
            $ie = [psobject]([WatiN.Core.Browser]::AttachTo([WatiN.Core.IE],[WatiN.Core.Find]::ByUrl($url)))
            $ie.AutoClose = $false #can dispose the WatiN instance without closing the IE tab
            [WatiN.Core.Settings]::AutoCloseDialogs = $false
        	return $ie
        }
        elseif($Title.Trim() -ne ""){
            $ie = [psobject]([WatiN.Core.Browser]::AttachTo([WatiN.Core.IE],[WatiN.Core.Find]::ByTitle($title)))
            $ie.AutoClose = $false
            [WatiN.Core.Settings]::AutoCloseDialogs = $false
	        return $ie
        }
        else{
            $ie = New-Object WatiN.Core.IE
            [WatiN.Core.Settings]::AutoCloseDialogs = $false
            $ie.AutoClose = $false
            return $ie
        }
    }
}#end Get-Ie

<#
    .SYNOPSIS
    Get a text file from a web request
    
    .DESCRIPTION
    Calls the Net.WebRequest and gets the 
    response stream, and then it formats the 
    response into UTF8 string.
    
    .PARAMETER Url
    The remote location text file.
    
    .OUTPUTS
    String
    
#>
function Request-File
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Url
    )
    Process
    {
        try{

            $rqst = [System.Net.WebRequest]::Create($Url)
            if([NoFuture.Globals.SecurityKeys]::ProxyServer -ne $null)
            {
                $proxy = New-Object System.Net.WebProxy
                $proxy.Address = [NoFuture.Globals.SecurityKeys]::ProxyServer
                $proxy.UseDefaultCredentials = $true;
                $rqst.Proxy = $proxy
            }
            $rspn = $rqst.GetResponse()
            $rspnstream = $rspn.GetResponseStream()
            $rspnheaders = @{}
            $rspn.Headers | % {
               $rspn.Headers.Keys | ? {$rspnheaders.Keys -notcontains $_} | % {$rspnheaders += @{$_ = $rspn.Headers[$_]}}
            }

            #print headers to console
            if([NoFuture.Globals.Switches]::PrintWebHeaders){
                $rspnheaders.Keys | % {
                    Write-Host ("{0} {1}" -f $_, $rspnheaders[$_] ) -ForegroundColor DarkGray
                }
            }

            #write binary response directly to a file
            if([NoFuture.Util.Net]::IsBinaryContent($rspnheaders)){
               
               #get a file name
               $binFileName = $rspnheaders["Content-Description"]
               if([string]::IsNullOrWhiteSpace($binFileName)){
                $binFileName = ([System.IO.Path]::GetRandomFileName()) + ".bin"
               }
               $binFileFullName = (Join-Path ([NoFuture.TempDirectories]::Root) $binFileName)

               #get a file stream
               $fs = New-Object System.IO.FileStream($binFileFullName, [System.IO.FileMode]::Create)

               $buffer = New-Object Byte[] 4096
               $len = $rspnstream.Read($buffer,0,4096)
               while($len -gt 0){
                  $fs.Write($buffer,0,$len)
                  $len = $rspnstream.Read($buffer,0,4096)
               }
               $fs.Flush()
               $fs.Close()
               $rspnStream.Close()
               return $binFileFullName
                
            }
            #read the response, write to file
            
            $streamRdr = New-Object System.IO.StreamReader $rspnStream
            $contents = $streamRdr.ReadToEnd()
            $streamRdr.Close()
            $rspnStream.Close()

            return $contents
            
        }
        catch{
            Write-Host ("Could not receive file at '{0}'" -f $Url) -ForegroundColor Yellow
        }
    }
}#end Request-File

<#
    .synopsis
    Gets the IP address of a URL
    
    .Description
    Very simple - simply wraps around the .NET
    System.Net.Dns static 'GetHostAddressess' 
    member.
    
    .parameter Url
    The Url of which the IP is sought.
    
    .outputs
    System.Net.IPAddress[]
    
#>
function Get-Ip
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Url
    )
    Process
    {
        return [System.Net.Dns]::GetHostAddresses($Url)
    }
}#end Get-Ip

<#
    .synopsis
    Gets the DNS name entry for the IP
    
    .Description
    Very simple - simply wraps around the .NET
    System.Net.Dns static 'GetHostEntry' 
    member.
    
    .parameter Ip
    The Ip of which the DNS name is sought.
    
    .outputs
    System.Net.IPHostEntry
    
#>
function Get-DnsName
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Ip
    )
    Process
    {
        return [System.Net.Dns]::GetHostEntry($Ip)
    }

}#end Get-DnsName

<#
    .SYNOPSIS
    Calls 'netstat' with the 'a', 'o' and 'n' switches

    .DESCRIPTION
    Calls 'netstat' with the 'a', 'o' and 'n' switches 
    and converts it to CSV style object array with little
    modifiction.

    .PARAMETER ResolveDns
    Will attempt to resolve an IP (v4 or v6) which is not 
    an abrrevation for the localhost - this may take some 
    time to complete especially when the IP has no DNS entry.

    .PARAMETER ResolveCommonPorts
    Will search in the '%root%\drivers\etc\services' file 
    and attempt to match a port number to a service type.

    .EXAMPLE
    PS C:\> Get-Netstat
#>
function Get-Netstat
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [switch] $ResolveDns,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $ResolveCommonPorts

    
    )
    Process
    {
        #C:\Windows\system32\drivers\etc
        
        $tempNetstat = ([NoFuture.TempFiles]::NetStat)

        #run the command 
        netstat -aon > $tempNetstat

        #get the contents
        $netstat = (Get-Content $tempNetstat | ? {$_.Trim() -ne ""})

        #drop line 0
        $netstat = ($netstat[1..($netstat.Length-1)])

        #handle column hearders being unique
        $netstat[0] = ($netstat[0].Replace("Local Address","Local_Address").Replace("Foreign Address","Foreign_Address"))

        #reduce all spaces down to one space
        $netstat = ($netstat | % { [NoFuture.Util.Etc]::DistillString($_)})

        #replace all spaces with comma
        $netstat = ($netstat | % {$_.Trim().Replace(" ",",")})

        #get csv
        $netstat = (ConvertFrom-Csv $netstat)

        #clean up UDP ones
        $netStatComplete = ($netstat | % {if($_.Proto -eq "UDP"){$_.PID = $_.State; $_.State = "NA";$_ }else{$_}})

        #get coorsponding names for pids
        $netStatComplete | % {
            try
            { 
                $proc = (Get-Process -Id $_.PID)
                $_ | Add-Member NoteProperty ProcessName $proc.Name

            }
            catch
            {
                $_ | Add-Member NoteProperty ProcessName ""
            }
        }

        if($ResolveDns){

            $netStatComplete | % {
                $fip = $_.Foreign_Address
                $netIpAddress = [NoFuture.Util.Net]::GetNetStatIp($fip)
                $dnsName = [string]::Empty
                if($netIpAddress.Equals([System.Net.IpAddress]::LoopBack) -or 
                   $netIpAddress.Equals([System.Net.IpAddress]::IPv6Loopback)){

                    $dnsName = "localhost"
                }
                else{
                    try
                    {  
                        Write-Progress "Attempting to resolve DNS entry for $netIpAddress"
                        $ipHostEntry = [System.Net.Dns]::GetHostEntry($netIpAddress) 
                        $dnsName = $ipHostEntry.HostName
                    } 
                    catch [System.Net.Sockets.SocketException]
                    { 
                        $dnsName = "NoDnsEntry"
                    }
                }

                $_ | Add-Member NoteProperty -Name DomainName -Value $dnsName

            }

        }

        if($ResolveCommonPorts){

            $netStatComplete | % {
                $fip = $_.Foreign_Address
                $proto = $_.Proto

                $portSvc = [NoFuture.Util.Net]::GetNetStatServiceByPort($proto, $fip)

                if(-not ([string]::IsNullOrWhiteSpace($portSvc.Item2))){
                    $svcName = "{0}({1})" -f $portSvc.Item1, $portSvc.Item2
                }
                else{
                    $svcName = "{0}" -f $portSvc.Item1
                }

                $_ | Add-Member NoteProperty -Name ServiceName -Value $svcName

            }

        }

        return $netStatComplete

    }
}#end Get-Netstat

<#
    .SYNOPSIS
    Gets an array of ports that currently 
    have some process listening

    .DESCRIPTION
    Using the out put of Netstat, the cmdlet simple 
    gets the Local Address's Port number and returns 
    all of them as an array.

#>
function Get-InUsePorts
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
        $inusePorts = (Get-Netstat | % {
            $la = ("{0}" -f $_.Local_Address)
            
            #IPv4
            if($la -match '[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\:([0-9]{1,5})')
            {
                $matches[1]
            }
            #empty system
            elseif($la -match '\x5b\x3a\x3a\x5d\x3a([0-9]{1,5})')
            {
                $matches[1]
            }
            #IPv6
            elseif($la -match '\x5b[0-9a-f]{1,4}\x3a\x3a[0-9a-f]{1,4}\x3a[a-f\%\:0-9]*\]\:')
            {
                $matches[1]
            }
        })
        return ($inusePorts | Sort-Object -Unique)
    }
}#end Get-InUsePorts

<#
    .SYNOPSIS
    Returns the domain name of known advertisers.
    
    .DESCRIPTION
    Will return an array of domain names for known advertisers
    in the form of '*.domian.root' sorted in alphabetical order.
    The list is sourced from, via a web request, to 
    NoFuture.Shared.Constants.HostTxt on each call to this 
    cmdlet
    
    .OUTPUTS
    System.Array
#>
function Get-AdverstisingDomainNames
{
    [CmdletBinding()]
    Param
    ()
    Process
    {
        $restrictedZones = Request-File -Url ([NoFuture.Shared.Constants]::HostTxt)

        $restrictedZonesPath = (Join-Path ([NoFuture.TempDirectories]::Root) "RestrictedHosts.txt")

        [System.IO.File]::WriteAllBytes($restrictedZonesPath, ([System.Text.Encoding]::UTF8.GetBytes($restrictedZones)))

        $restrictedZones = [System.IO.File]::ReadAllLines($restrictedZonesPath)

        $commonDomains = @("*.microsoft.com","*.google.com", "*.yahoo.com", "*.amazon.com")

        $adDomains = @()
        :nextLine foreach($line in $restrictedZones){
            if([string]::IsNullOrWhiteSpace($line)){continue :nextLine;}
            if($line.Trim().StartsWith("#")) { continue :nextLine; }

            $tLine = $line.Trim()

            if(($tLine.Split(" ")).Length -le 1){ continue :nextLine;}
            $tLine = $tLine.Split(" ")[1]

            if($tLine.Contains("#")){
                $tLine = ($tLine.Split("#"))[0];
                if($tLine.Contains("#")){Write-Host $tLine; break;}
                if([string]::IsNullOrWhiteSpace($tLine)) { continue :nextLine;}
            }
    
            $tLine = $tLine.Split(".")
            if($tLine.Length -le 1){continue :nextLine;}

            $tLineParts = $tLine.Split(".")
            $tLine = ("*.{0}.{1}" -f $tLineParts[($tLineParts.Length-2)], $tLineParts[($tLineParts.Length-1)])

            if($commonDomains -contains $tLine){continue :nextLine;}

            $adDomains += $tLine
        }

        return $adDomains | Sort-Object -Unique

    }
}#end Get-AdDomainNames

<#
    .SYNOPSIS
    Adds items to:
    HKCU:\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains
    with a restricted status. 
   
#>
function Add-DomainsRestrictedZone
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [array] $adDomains
    )
    Process
    {
        $zoneMapRegPath = [NoFuture.Shared.Constants]::RegistryZonePath

        if(-not (Test-Path $zoneMapRegPath)){
            Write-Host "Registry Path does not exist `n $zoneMapRegPath" -ForegroundColor Yellow
            break;
        }

        :dnsName foreach($adDomain in $adDomains){
            if([string]::IsNullOrWhiteSpace($adDomain)){continue :dnsName;}

            #don't add root domains 
            $adDomainParts = $adDomain.Split(".")
            if($adDomainParts.Length -lt 2){ continue :dnsName;}

            $regKeyName = "{0}.{1}" -f $adDomainParts[($adDomainParts.Length-2)], $adDomainParts[($adDomainParts.Length-1)]

            $regKeyFullName = (Join-Path $zoneMapRegPath $regKeyName)
            
            New-Item -Path $zoneMapRegPath -Name $regKeyName -Force

            New-ItemProperty -Path $regKeyFullName -Name "*" -PropertyType ([Microsoft.Win32.RegistryValueKind]::DWord) -Value 4

        }
    }
}#end Add-DomainsRestrictedZone


<#
    .SYNOPSIS
    Get geocode results for the given values
    
    .DESCRIPTION
    Constructs an URL to make a request to the Google Geocode API service
    using the Street, City and State.  When Street is absent then only City 
    and State are used.  The address portion of the URL is encoded so simple 
    text should be passed into the cmdlet.
    
    The API key should already have been assigned prior to a call to this
    cmdlet.  The API Key is to be set at NoFuture.Keys.GoogleCodeApiKey.

    .PARAMETER Street
    Optional, the typical American street address.

    .PARAMETER City
    Some city name

    .PARAMETER State
    The two char postal code for an American State.
    
    .LINK
    https://developers.google.com/maps/documentation/geocoding/index

    .OUTPUTS
    A custom psobject converted from the JSON response from Google
#>
function Get-GoogleGeocode
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [string] $Street,
        [Parameter(Mandatory=$true,position=1)]
        [string] $City,
        [Parameter(Mandatory=$true,position=2)]
        [string] $State
    )
    Process{

        if(-not ([string]::IsNullOrWhiteSpace($Street))) {
            $urlAddr = [System.Web.HttpUtility]::UrlEncode(("{0} {1}, {2}" -f $Street, $City, $State))
        }
        else {
            $urlAddr = [System.Web.HttpUtility]::UrlEncode(("{0}, {1}" -f $City, $State))
        }
        $key = [NoFuture.Globals.SecurityKeys]::GoogleCodeApiKey
        $googleGeocodeUrl = "https://maps.googleapis.com/maps/api/geocode/json?address=$urlAddr&sensor=false&key=$key"
        $googleGeocodeData = Request-File -Url $googleGeocodeUrl
        return (ConvertFrom-Json -InputObject $googleGeocodeData)

    }
}#end Get-GoogleGeocode

<#
    .SYNOPSIS
    Gets JSON results from a call to Google Places API

    .DESCRIPTION
    Constructs an URL to Google Places API; makes a web-request
    from Request-File cmdlet (to allow for assignment of proxy); 
    and then converts the JSON results to a psobject using 
    ConvertFrom-Json. 

    Only the Latitude and Longitude are required.  The Google
    API key is expected to be available at NoFuture.Key.GoogleCodeApiKey.
    The Radius will default to 500 meters, the Place Type to 
    'food' and the Search String will be omitted.

    All available Place's Types are listed at Global:GooglePlaceTypes.

    .LINK
    https://developers.google.com/places/documentation/supported_types

    .LINK
    https://developers.google.com/places/documentation/search

    .OUTPUTS
    psobject of the converted JSON results.
#>
function Get-GooglePlaces
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [double] $Latitude,
        [Parameter(Mandatory=$true,position=1)]
        [double] $Longitude,
        [Parameter(Mandatory=$false,position=2)]
        [int] $RadiusInMeters,
        [Parameter(Mandatory=$false,position=3)]
        [string] $PlaceType,
        [Parameter(Mandatory=$false,position=4)]
        [string] $SearchString
    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($PlaceType)){
            $PlaceType = "food"
        }

        if(([NoFuture.Util.Net]::GooglePlaces) -notcontains $PlaceType){
            throw "Invalid Google Place Type '$PlaceType'"
        }

        $googleApiKey = [NoFuture.Globals.SecurityKeys]::GoogleCodeApiKey

        if($RadiusInMeters -eq 0){
            $RadiusInMeters = 500;
        }

        if(-not ([string]::IsNullOrWhiteSpace($SearchString))){
            $googlePlacesUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=$Latitude,$Longitude&radius=$RadiusInMeters&types=$PlaceType&name=$SearchString&sensor=false&key=$googleApiKey"
        }
        else{
            $googlePlacesUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=$Latitude,$Longitude&radius=$RadiusInMeters&types=$PlaceType&sensor=false&key=$googleApiKey"
        }

        $googlePlacesData = Request-File -Url $googlePlacesUrl
        return (ConvertFrom-Json -InputObject $googlePlacesData)

    }
}#end Get-GooglePlaces

<#
    .SYNOPSIS
    Makes a web-request to the SEC for the given Industry Classification code.

    .DESCRIPTION
    This cmdlet produces a list of corporations which have filed
    with the SEC.

    .PARAMETER SicCode
    The Standard Industry Classification - the SEC still uses this older
    classification while other government agencies have moved on to the 
    NAICS.

    .PARAMETER StartAtCount
    Part to the query string used in the web-request to the SEC.

    .OUTPUTS
    array of NoFuture.Rand.PublicCorporation

#>
function Get-SecCompaniesBySic
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $SicCode
    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($SicCode)){
            Write-Host "Invalid SIC code '$SicCode'" -ForegroundColor Yellow
        }
        $searchCrit = New-Object NoFuture.Rand.Gov.Sec.Edgar+FullTextSearch
        $searchCrit.SicCode = $SicCode
        $rssContent = Request-File -Url ([NoFuture.Rand.Gov.Sec.Edgar]::UrlFullTextSearchBySic($searchCrit))
        return ([NoFuture.Rand.Gov.Sec.Edgar]::ParseCompanyFullTextSearch($rssContent))
    }
}#end Get-SecCompaniesBySic

<#
    .SYNOPSIS
    Makes a web-request to the SEC for the given CIK value.

    .DESCRIPTION
    Fetches the details of a specific company from the SEC.
    
    .PARAMETER CikCode
    A unique identifier for the company with the SEC

    .OUTPUTS
    NoFuture.Rand.Com.PublicCorporation

#>
function Get-SecCompanyDetail
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $CikCode
    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($CikCode)){
            Write-Host "Invalid CIK code '$CikCode'" -ForegroundColor Yellow
        }

        $corp = New-Object NoFuture.Rand.Com.PublicCorporation

        $xmlContent = Request-File -Url ([NoFuture.Rand.Gov.Sec.Edgar]::UrlCompanyList10KFilings($CikCode))
        $tryParseResult = ([NoFuture.Rand.Gov.Sec.Edgar]::TryGetCorpData($xmlContent, [ref] $corp))
        if($tryParseResult){
            return $corp
        }
    }
}#end Get-SecCompanyDetail

<#
    .SYNOPSIS
    Makes a web-request to a Yahoo webservice to lookup ticker symbols by name.

    .DESCRIPTION
    Makes a web-request to a Yahoo webservice to lookup ticker symbols by name.
    
    .PARAMETER CompanyName
    The search criteria used to lookup the ticker symbols

    .OUTPUTS
    NoFuture.Rand.Com.PublicCorporation

#>
function Get-PublicCorpTickerSymbols
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $CompanyName
    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($CompanyName)){
            Write-Host "Invalid value '$CompanyName'" -ForegroundColor Yellow
        }

        $yahooData = Request-File -Url ([NoFuture.Rand.Com.PublicCorporation]::CtorTickerSymbolLookup($CompanyName))
        $corp = New-Object NoFuture.Rand.Com.PublicCorporation

        $tryParseResult = ([NoFuture.Rand.Com.PublicCorporation]::MergeTickerLookupFromJson($yahooData, [ref] $corp))
        if($tryParseResult){
            return $corp
        }
    }
}#end Get-SecCompanyDetail

<#
    .SYNOPSIS
    Downloads audio from a YouTube.com url

    .DESCRIPTION
    Invokes the youtube-dl.exe in the bin folder passing in
    the cmdlet args.
    
    .PARAMETER YtUrl
    The YouTube.com URL.

    .PARAMETER ProxyPwd
    The password to be given to the proxy server.
    This is only used when the global 
    NoFuture.Globals.SecurityKeys.ProxyServer is assigned
    to some URI.  The username is determined from the 
    current WindowsIdentity

#>
function Get-YtAudio
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $YtUrl,

        [Parameter(Mandatory=$false,position=1)]
        [string] $ProxyPwd,

        [Parameter(Mandatory=$false,position=2)]
        [switch] $WhatIf

    )
    Process
    {
        if(-not (Test-Path .\bin\youtube-dl.exe)){
            Write-Host "The youbube-dl.exe is expected in the .\bin directory."
            break;
        }
        if(-not (Test-Path .\bin\ffmpeg.exe)){
            Write-Host "The ffmpeg.exe is expected in the .\bin directory."
            break;
        }

        if([string]::IsNullOrWhiteSpace($YtUrl) -or 
           -not [System.Uri]::IsWellFormedUriString($YtUrl, [System.UriKind]::Absolute))
        {
            Write-Host "Invalid URI $YtUrl"
            break;
        }

        $dnd = $ytExeCmd = New-Object System.Text.StringBuilder

        $dnd = $ytExeCmd.Append("& youtube-dl -v ")

        #handle setting the proxy server info as needed
        if([NoFuture.Globals.SecurityKeys]::ProxyServer -ne $null){
            if([string]::IsNullOrWhiteSpace($ProxyPwd)){
                Write-Host "Either unassign the global SecurityKeys.ProxyServer or pass in a password"
                break;
            }
            $dnd = $ytExeCmd.AppendFormat("--proxy {0} " -f ([NoFuture.Globals.SecurityKeys]::ProxyServer.ToString()))
            
            $proxyAuthValue = [NoFuture.Util.Net]::GetProxyAuthHeaderValue($null, $ProxyPwd) 

            $dnd = $ytExeCmd.Append("--add-header Proxy-Authorization:'$proxyAuthValue' ");
        }
        $dnd = $ytExeCmd.Append("-x --audio-format mp3 ")

        $dnd = $ytExeCmd.Append($YtUrl)

        if($WhatIf){
            Write-Host ('iex -Command "{0}"' -f $ytExeCmd)
        }
        else{
            iex -Command $ytExeCmd.ToString()
        }
    }
}

<#
Windows Proxy Notes

Settings
 HKU\<your user's sid>\Software\Microsoft\Windows\CurrentVersion\Internet Settings

 Get-BinaryDump C:\Windows\System32\wininet.dll

 wininet.dll at
 http://msdn.microsoft.com/en-us/library/windows/desktop/aa385483%28v=vs.85%29.aspx

#>
