try{
if(-not [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-EmbeddedBrowser",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-Ip",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-Socket",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-DnsName",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Request-File",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-InUsePorts",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-Netstat",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-GooglePlaces",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-GoogleGeocode",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-YtAudio",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
<#
    .SYNOPSIS
    Gets a web browser embedded in a small 640x480 window
    
    .DESCRIPTION
    Creates a new small embedded browser as a win form of
    the PowerShell app domain.
    
    .PARAMETER Url
    The complete URL to navigate to.
    
    .PARAMETER Width
    The embedded window's Width, default is 640

    .PARAMETER Height
    The embedded window's Height, default is 480

    .EXAMPLE
    C:\PS>$formWebTuple = Get-EmbeddedBrowser -Url "https://rally1.rallydev.com/slm/login.op"
    
    .OUTPUTS
    Tuple[Windows.Forms.Form, Windows.Forms.WebBrowser]

    .LINKs
    https://github.com/globalsign/OAuth-2.0-client-examples/blob/master/PowerShell/Powershell-example.ps1
    
#>
function Get-EmbeddedBrowser
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Url,
        [Parameter(Mandatory=$false,position=1)]
        [int] $Width,
        [Parameter(Mandatory=$false,position=2)]
        [int] $Height,
        [Parameter(Mandatory=$false,position=3)]
        [ScriptBlock]$OnNavigatingHandler
    )
    Process
    {
        #test the dependent assemblies are present - load otherwise
        $appDomainAsms = ([AppDomain]::CurrentDomain.GetAssemblies() | % { $_.GetName().Name})
        if($appDomainAsms -notcontains "System.Web")
        {
            Add-Type -AssemblyName System.Web
        }

        if($appDomainAsms -notcontains "System.Windows.Forms")
        {
            Add-Type -AssemblyName System.Windows.Forms
        }

        if($appDomainAsms -notcontains "System.Drawing")
        {
            Add-Type -AssemblyName System.Drawing
        }
        
        if($Width -le 0){
            $Width = 640
        }

        if($Height -le 0){
            $Height = 480
        }

        #test the Uri is valid
        $uri = New-Object System.Uri("http://localhost/")
        if(-not([System.Uri]::TryCreate($Url,[System.UriKind]::Absolute, [ref] $uri))) {
            throw "'$Url' did not parse as an absolute URI" 
        }

        # create window for embedded browser
        $nfIconB64 = 
        "H4sIAAAAAAAEAO1Zv2/TQBR+dhw1G1kRoGZkYGeACgZgYmEA9U/oAgsjoDZCKGICdUBIlB9iqsTCxNKFqhIzTAi"+ `
        "mhg2pKqnttDR2fLzznZ07+645p05UIT/pU/Luu3ff3fPl/K4FsMCBVgvwswXrTYAbANxvwHoDYBfbmk3GLzkAL0"+ `
        "4nfgOW6gDROYDzNAZxFVi/2PhHZbJFgdcmoU+yiAZuv9A4ob8hxYfeHQrd2FG0P8/ivI+qPhTG2jgWHVPSoG2hd"+ `
        "1c3NuWO0i+y/ijwVzNjf+FjK9d/lH7SXsSi0P0qjYHPNN9HrXNcfZpnk7xNTV+T+xnq/xiX+2npY64X5Nz7O9q+"+ `
        "09F/L8f6GzPW/5U9c2alXyT3U9HPxelzP531+zuZ3/3CrPSz5yrdBwYxJepn3nX4O5ipfsHcl6mfrRfo+TdOu1R"+ `
        "9PN8z+qsz1VfUGbPSz+Ue3/sm2qXpT5j7MvRVdYZp7kvRN6wzpqZvUOOVoS/mN401rPEUmoXrXxHpOBPm3qj+N7"+ `
        "j/KOoM03f02PuP0TjCmWPyrqvMzAi3FdJtM2y2O2Sz2yFbiMccHcQc4hRDRDHPEFJc6Xb+JljudnoJSLezzbDSJ"+ `
        "e2VTcQWaZMtkpr8d4oGm1RDMc+Bi8/eIyTge4jsx/GUqlChwv+N+nHNqsUmtFDX4s1OnneAtvNmi/Mw4u24u8Rb"+ `
        "WUFIvuR5J/XVvJ3ORs+DnqeTt+t6XlisNt7S88LwSt4ScqniR7NX8tKj0PAwOS/OXsVbY3i2Y7Q8SEvN8/LwOT7"+ `
        "eGY6et8XkKHirJu1TgY/jHIlO928t4e38g6QNkOz/moIHYbogq5dkJ6FOaQKrVdL/pyjqlONaFOzdTOrc4cANlX"+ `
        "1C737MB/21oXh3GLjvkFtknP8T6+RHqnjkvrP7k3sLa6rbw9D/EMeE7qc0Hr8rtQ+9C2xu3q4wn0XT+GHgPeHxr"+ `
        "yaKH7jb8VoD71rReMzHJabt/87k0yge8/ac5dbrTBSPuuz+0r9YNJ49K/Zcc+syiMe8vWb3Nv9B0fgoiuaw7u7R"+ `
        "/Uiff+H4dE/ufcvG6uLF/Z/sT+Tu5XIq7H8aTw7+tHCNyzH4/h8eun0c6yCK+mey8XT/p/2D3nXV/IoYvZE8+wz"+ `
        "w9iViDXEWcZnhDcVDhrVwhKc9gEPCYuVzhh8wtkYq8HJ3IjgBtVqFChWmDslsx3HiLw4zwbe5n9Bx58SvOTXJz/"+ `
        "BW/DHyHcc6yuetqW/Jft2R/UTU5vNlo8VLsJhvc9/Krve45+w/GmDHXmYjAAA="

        $compressIconBytes = [System.Convert]::FromBase64String($nfIconB64)
        $compressMs = New-Object System.IO.MemoryStream -ArgumentList (,[byte[]]$compressIconBytes)
        $gzipDecompress = New-Object System.IO.Compression.GZipStream($compressMs, `
                                        [System.IO.Compression.CompressionMode]::Decompress)
        $nfIconDecompBytes = [byte[]]::New(9062)
        $gzipDecompress.Read($nfIconDecompBytes, 0, $nfIconDecompBytes.Length)
        $nfIconMs = New-Object System.IO.MemoryStream -ArgumentList (,[byte[]]$nfIconDecompBytes)
        $nfIcon = New-Object System.Drawing.Icon($nfIconMs)
        $form = New-Object Windows.Forms.Form
        $form.Icon = $nfIcon
        $form.Width = $Width
        $form.Height = $Height
        $web = New-Object Windows.Forms.WebBrowser
        $web.Size = $form.ClientSize
        $web.Anchor = "Left,Top,Right,Bottom"
        $form.Controls.Add($web)
        $web.Navigate($Url)

        if($OnNavigatingHandler -ne $null){
            $web.add_Navigating($OnNavigatingHandler)
        }

        return [Tuple]::Create($form, $web)

    }
}#end Get-EmbeddedBrowser

<#
    .SYNOPSIS
    Get a text file from a web request
    
    .DESCRIPTION
    Calls the Net.WebRequest and gets the 
    response stream, and then it formats the 
    response into UTF8 string.
    
    .PARAMETER Url
    The remote location text file.

    .PARAMETER UserName
    For use in creating an Authorization header 
    in the request.

    .PARAMETER Pwd
    For use in creating an Authorization header
    in the request.

    .PARAMETER UseDefaultProxy
    Will assign the WebRequest's Proxy property to 
    the instance at Globals.SecurityKeys.ProxyServer.
    When this switch is active the values of UserName
    and Pwd will be ignored.

    .PARAMETER IncludeHeaderInOutput
    Optional, when included the output is a hashtable
    of 'Content' and 'Headers'.


    .OUTPUTS
    String
    
#>
function Request-File
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Url,
        [Parameter(Mandatory=$false,position=1)]
        [string] $UserName,
        [Parameter(Mandatory=$false,position=2)]
        [string] $Pwd,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $UseDefaultProxy,
        [Parameter(Mandatory=$false,position=4)]
        [switch] $IncludeHeaderInOutput
    )
    Process
    {
        try{

            $rqst = [System.Net.WebRequest]::Create($Url)

            if($UseDefaultProxy)
            {
                if( [NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::ProxyServer -eq $null){
                    Write-Host "Assign a default proxy server IP at SecurityKeys.ProxyServer."
                    break;
                }
                $proxy = New-Object System.Net.WebProxy
                $proxy.Address = [NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::ProxyServer
                $proxy.UseDefaultCredentials = $true;
                $rqst.Proxy = $proxy
            }
            elseif(-not ([string]::IsNullOrWhiteSpace($UserName)) -and -not ([string]::IsNullOrWhiteSpace($Pwd))){
                $authHdrValue = [NoFuture.Util.Core.NfNet]::GetAuthHeaderValue($UserName, $Pwd)
                $rqst.Headers.Add("Authorization",$authHdrValue)
            }

            $rspn = $rqst.GetResponse()
            $rspnstream = $rspn.GetResponseStream()
            $rspnheaders = @{}
            $rspn.Headers | % {
               $rspn.Headers.Keys | ? {$rspnheaders.Keys -notcontains $_} | % {
                $rspnheaders += @{$_ = $rspn.Headers[$_]}
               }
            }

            #print headers to console
            if([NoFuture.Shared.Cfg.NfConfig+Switches]::PrintWebHeaders){
                Write-Host $Url -ForegroundColor DarkYellow
                $rspnheaders.Keys | % {
                    Write-Host ("{0} {1}" -f $_, $rspnheaders[$_] ) -ForegroundColor DarkGray
                }
                Write-Host ""
            }

            #write binary response directly to a file
            if([NoFuture.Util.Core.NfNet]::IsBinaryContent($rspnheaders)){
               
               #get a file name
               $binFileName = $rspnheaders["Content-Description"]
               if([string]::IsNullOrWhiteSpace($binFileName)){
                $binFileName = ([System.IO.Path]::GetRandomFileName()) + ".bin"
               }
               $binFileFullName = (Join-Path ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Root) $binFileName)

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


            if($IncludeHeaderInOutput){
                return  @{Headers = $rspnheaders; Content = $contents}
            }
            else{
                return $contents
            }
            
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
        
        $tempNetstat = ([NoFuture.Shared.Cfg.NfConfig+TempFiles]::NetStat)

        #run the command 
        netstat -aon > $tempNetstat

        #get the contents
        $netstat = (Get-Content $tempNetstat | ? {$_.Trim() -ne ""})

        #drop line 0
        $netstat = ($netstat[1..($netstat.Length-1)])

        #handle column hearders being unique
        $netstat[0] = ($netstat[0].Replace("Local Address","Local_Address").Replace("Foreign Address","Foreign_Address"))

        #reduce all spaces down to one space
        $netstat = ($netstat | % { [NoFuture.Util.Core.NfString]::DistillString($_)})

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
                $netIpAddress = [NoFuture.Util.Core.NfNet]::GetNetStatIp($fip)
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

                $portSvc = [NoFuture.Util.Core.NfNet]::GetNetStatServiceByPort($proto, $fip)

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
        $key = [NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::GoogleCodeApiKey
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
    'restaurant' and the Search String will be omitted.

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
            $PlaceType = "restaurant"
        }

        if(([NoFuture.Util.Core.NfNet]::GooglePlaces) -notcontains $PlaceType){
            throw "Invalid Google Place Type '$PlaceType'"
        }

        $googleApiKey = [NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::GoogleCodeApiKey

        if($RadiusInMeters -eq 0){
            $RadiusInMeters = 500;
        }

        if(-not ([string]::IsNullOrWhiteSpace($SearchString))){
            $googlePlacesUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=$Latitude,$Longitude&radius=$RadiusInMeters&type=$PlaceType&name=$SearchString&sensor=false&key=$googleApiKey"
        }
        else{
            $googlePlacesUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=$Latitude,$Longitude&radius=$RadiusInMeters&type=$PlaceType&sensor=false&key=$googleApiKey"
        }

        $googlePlacesData = Request-File -Url $googlePlacesUrl
        return (ConvertFrom-Json -InputObject $googlePlacesData)

    }
}#end Get-GooglePlaces

<#
    .SYNOPSIS
    Downloads audio from a YouTube.com url

    .DESCRIPTION
    Invokes the youtube-dl.exe in the bin folder passing in
    the cmdlet args.  The resulting file is saved to the 
    current working directory.
    
    .PARAMETER YtUrl
    The YouTube.com URL.

    .PARAMETER ProxyPwd
    The password to be given to the proxy server.
    This is only used when the global 
    NoFuture.Shared.Core.NfConfig+SecurityKeys.ProxyServer is assigned
    to some URI.  The username is determined from the 
    current WindowsIdentity

    .EXAMPLE
    PS C:\> Get-YtAudio -YtUrl "https://www.youtube.com/watch?v=i0V0n7y48mo" -ProxyPwd 'P@$sW0rd'

    .LINK
    https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-4.2-win64-static.zip

    .LINK
    https://yt-dl.org/latest/youtube-dl.exe

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
        if(-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+PythonTools]::Ffmpeg))){
            Write-Host "The ffmpeg.exe is expected in the .\bin directory."
            break;
        }
        if(-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+PythonTools]::YoutubeDl))){
            Write-Host "The youtube-dl.exe is expected in the .\bin directory."
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
        if([NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::ProxyServer -ne $null){
            if([string]::IsNullOrWhiteSpace($ProxyPwd)){
                Write-Host "Either unassign the global SecurityKeys.ProxyServer or pass in a password"
                break;
            }
            $proxyServer = [NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::ProxyServer.ToString()
            $dnd = $ytExeCmd.AppendFormat("--proxy $proxyServer ")
            
            $proxyAuthValue = [NoFuture.Util.Core.NfNet]::GetAuthHeaderValue($null, $ProxyPwd) 

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
