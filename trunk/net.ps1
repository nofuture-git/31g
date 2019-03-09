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
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-SecCompaniesBySic",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-SecCompanyDetail",$MyInvocation.MyCommand)
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
        $nfIconB64 = "AAABAAQAICAAAAEAIACoEAAARgAAACAgAAABAAgAqAgAAO4QAAAQEAAAAQAgAGgEAACWGQAAEBAAAAEACABoBQAA/h0AACgAAAAgAAAAQAAAAAEAIAAAAAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7784H//PT///z0///89P///PT///z0//768vUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP789Lv//PT///z0///89P/+/PNW/vzzVv/89P///PT///z0///89P///PT//vry9f7+9h8AAAAA/vzzsP/89P///PT///z0///89P///PT///z0/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/vYf/vry9f/89P///PT///z0//7+9h/+/PNt//z0///89P///PT///z0///89P///PT//vzzbQAAAAD+/POw//z0///89P///PT///z0///89P/++vL1AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP779JL//PT///z0///89P/+/PPGAAAAAP7881b//PT///z0///89P///PT///z0///89P/+/PNtAAAAAP7887D//PT///z0///89P/+/PNtAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vzy0P/89P///PT///z0//7784EAAAAAAAAAAAAAAAAAAAAA/vzzsP/89P///PT///z0//78820AAAAA/vzzsP/89P///PT///z0//78820AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7+9h///PT///z0///89P/++vL1AAAAAAAAAAAAAAAAAAAAAAAAAAD+/POw//z0///89P///PT//vzzbQAAAAD+/POw//z0///89P///PT//vzzbQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vv0kv/89P///PT///z0//7888YAAAAAAAAAAAAAAAAAAAAAAAAAAP7887D//PT///z0///89P/+/PNtAAAAAP7887D//PT///z0///89P/+/PNtAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD++/TZ//z0///89P///PT//vvzgQAAAAAAAAAAAAAAAAAAAAAAAAAA/vzzsP/89P///PT///z0//78820AAAAA/vzzsP/89P///PT///z0//78820AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vryPf/89P///PT///z0//769OwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/POw//z0///89P///PT//vzzbQAAAAD+/POw//z0///89P///PT//vzzbQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD++vKi//z0///89P///PT//vz0uwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7887D//PT///z0///89P/+/PNtAAAAAP7887D//PT///z0///89P/+/PNtAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP768uL//PT///z0///89P/+/PNWAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vzzsP/89P///PT///z0//78820AAAAA/vzzsP/89P///PT///z0//78820AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD++vI9//z0///89P///PT//vr07AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/POw//z0///89P///PT//vzzbQAAAAD+/POw//z0///89P///PT//vzzbQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7887D//PT///z0///89P/+/PS7AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7887D//PT///z0///89P/+/PNtAAAAAP7887D//PT///z0///89P/+/PNtAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vr07P/89P///PT///z0//768j0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vzzsP/89P///PT///z0//78820AAAAA/vzzsP/89P///PT///z0//78820AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7881b//PT///z0///89P/++vLiAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/POw//z0///89P///PT//vzzbQAAAAD+/POw//z0///89P///PT//vzzbQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vz0u//89P///PT///z0//768qIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7887D//PT///z0///89P/+/PNtAAAAAP7887D//PT///z0///89P/+/PNtAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD++vTs//z0///89P///PT//vryPQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vzzsP/89P///PT///z0//78820AAAAA/vzzsP/89P///PT///z0//78820AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vvzgf/89P///PT///z0//779NkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/POw//z0///89P///PT//vzzbQAAAAD+/POw//z0///89P///PT//vzzbQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/PPG//z0///89P///PT//vv0kgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7887D//PT///z0///89P/+/PNtAAAAAP7887D//PT///z0///89P/+/PNtAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP768vX//PT///z0///89P/+/vYfAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vzzsP/89P///PT///z0//78820AAAAA/vzzsP/89P///PT///z0//78820AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD++/OB//z0///89P///PT//vzy0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/POw//z0///89P///PT//vzzbQAAAAD+/POw//z0///89P///PT//vzzbQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7888b//PT///z0///89P/++/SSAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7887D//PT///z0///89P/+/PNtAAAAAP7887D//PT///z0///89P/+/PNtAAAAAAAAAAAAAAAAAAAAAAAAAAD+/vYf//z0///89P///PT//vry9f7+9h8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vzzsP/89P///PT///z0//78820AAAAA/vzzsP/89P///PT///z0//78820AAAAAAAAAAAAAAAAAAAAAAAAAAP779JL//PT///z0///89P/+/PPGAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/POw//z0///89P///PT//vzzbQAAAAD+/POw//z0///89P///PT//vzzbQAAAAAAAAAAAAAAAAAAAAAAAAAA/vzy0P/89P///PT///z0//7784EAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7887D//PT///z0///89P/+/PNtAAAAAP7887D//PT///z0///89P///PT///z0//768vUAAAAAAAAAAP7+9h///PT///z0///89P/++vL1AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/vzzVv/89P///PT///z0///89P///PT///z0//78820AAAAA/vzzsP/89P///PT///z0///89P///PT///z0/wAAAAAAAAAA/vv0kv/89P///PT///z0//7888YAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/PNt//z0///89P///PT///z0///89P///PT//vzzbQAAAAD++/OB//z0///89P///PT///z0///89P/++vL1AAAAAAAAAAD++vLi//z0///89P///PT//vzzbQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7881b//PT///z0///89P///PT///z0//768vX+/vYfAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP768vX//PT///z0//768uIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////+A/+GBgP/hgYD/wYGH/8Hhh//D4Yf/g+GH/4Phh/+H4Yf/B+GH/w/hh/8P4Yf+D+GH/h/hh/4f4Yf8H+GH/D/hh/g/4Yf4P+GH+H/hh/B/4Yfwf+GH8P/hh+D/4Yfg/+GA4f+BgMH/gYDD/4H/w/////////////8oAAAAIAAAAEAAAAABAAgAAAAAAAAIAAAAAAAAAAAAAAAAAAAAAAAA//ry///88///+/T///z0////9v//////AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFAQMDAwMDAwUFBQUFBQUFBQUFAwMDAwEBAwMDAwMDBAUBAwMDAwMDBQUFBQUFBQUFBQQAAwMDBAEDAwMDAwMBBQEDAwMDAwAFBQUFBQUFBQUFAgMDAwMFAQMDAwMDAwEFAQMDAwEFBQUFBQUFBQUFBQUBAwMDAAUFBQUBAwMDAQUBAwMDAQUFBQUFBQUFBQUFBAMDAwEFBQUFBQEDAwMBBQEDAwMBBQUFBQUFBQUFBQUCAwMDAwUFBQUFAQMDAwEFAQMDAwEFBQUFBQUFBQUFBQIDAwMABQUFBQUBAwMDAQUBAwMDAQUFBQUFBQUFBQUAAwMDAgUFBQUFBQEDAwMBBQEDAwMBBQUFBQUFBQUFBQMDAwMDBQUFBQUFAQMDAwEFAQMDAwEFBQUFBQUFBQUFAAMDAwEFBQUFBQUBAwMDAQUBAwMDAQUFBQUFBQUFBQADAwMCBQUFBQUFBQEDAwMBBQEDAwMBBQUFBQUFBQUFAQMDAwMFBQUFBQUFAQMDAwEFAQMDAwEFBQUFBQUFBQUCAwMDAAUFBQUFBQUBAwMDAQUBAwMDAQUFBQUFBQUFAwMDAwMFBQUFBQUFBQEDAwMBBQEDAwMBBQUFBQUFBQUDAwMDAAUFBQUFBQUFAQMDAwEFAQMDAwEFBQUFBQUFBQMDAwMABQUFBQUFBQUBAwMDAQUBAwMDAQUFBQUFBQUAAwMDAgUFBQUFBQUFBQEDAwMBBQEDAwMBBQUFBQUFBQEDAwMCBQUFBQUFBQUFAQMDAwEFAQMDAwEFBQUFBQUFAwMDAwQFBQUFBQUFBQUBAwMDAQUBAwMDAQUFBQUFBQADAwMBBQUFBQUFBQUFBQEDAwMBBQEDAwMBBQUFBQUFAQMDAwIFBQUFBQUFBQUFAQMDAwEFAQMDAwEFBQUFBQQDAwMBBAUFBQUFBQUFBQUBAwMDAQUBAwMDAQUFBQUFAgMDAwMFBQUFBQUFBQUFBQEDAwMBBQEDAwMBBQUFBQUBAwMDAAUFBQUFBQUFBQUFAQMDAwEFAQMDAwMDAwUFBAMDAwAFBQUFBQUFBQUBAwMDAwMDAQUDAwMDAwMDBQUCAwMDAQUFBQUFBQUFBQEDAwMDAwMBBQADAwMD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AP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAAAAAAAAAAAAAAAAAAAAIEBAQAAAAAAAQEBAQEBAQCBAQEAAAAAAAEBAIEBAQEAgQEAAAAAAAEBAQAAAIEBAIEBAAAAAAAAwQDAAACBAQCBAQAAAAAAAQEBAAAAgQEAgQEAAAAAAEEBAAAAAIEBAIEBAAAAAAEBAEAAAACBAQCBAQAAAAABAQBAAAAAgQEAgQEAAAAAgQEAAAAAAIEBAIEBAAAAAEEBAAAAAACBAQCBAQAAAUEBAQAAAAAAgQEAgQEAAACBAQAAAAAAAIEBAIEBAQABAQBAAAAAAQEBAQBBAQEAAQEAgAAAAAEBAQBAAAAAAAAAAAAAAAAAAAAAP//AACPwAAAn5gAAJ+cAACfHAAAnzwAAJ88AACePAAAnnwAAJ58AACc/AAAnPwAAJz8AACJ8AAA+f8AAP//AAA="
        $nfIconBytes = [System.Convert]::FromBase64String($nfIconB64)
        $nfIconMs = New-Object System.IO.MemoryStream -ArgumentList (,[byte[]]$nfIconBytes)
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
                $authHdrValue = [NoFuture.Util.Net]::GetAuthHeaderValue($UserName, $Pwd)
                $rqst.Headers.Add("Authorization",$authHdrValue)
            }

            $rspn = $rqst.GetResponse()
            $rspnstream = $rspn.GetResponseStream()
            $rspnheaders = @{}
            $rspn.Headers | % {
               $rspn.Headers.Keys | ? {$rspnheaders.Keys -notcontains $_} | % {$rspnheaders += @{$_ = $rspn.Headers[$_]}}
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
            if([NoFuture.Util.Net]::IsBinaryContent($rspnheaders)){
               
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

        if(([NoFuture.Util.Net]::GooglePlaces) -notcontains $PlaceType){
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
    the cmdlet args.
    
    .PARAMETER YtUrl
    The YouTube.com URL.

    .PARAMETER ProxyPwd
    The password to be given to the proxy server.
    This is only used when the global 
    NoFuture.Shared.Core.NfConfig+SecurityKeys.ProxyServer is assigned
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
        if(-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+BinTools]::Ffmpeg))){
            Write-Host "The ffmpeg.exe is expected in the .\bin directory."
            break;
        }
        if(-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+BinTools]::YoutubeDl))){
            Write-Host "The youbube-dl.exe is expected in the .\bin directory."
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
            $dnd = $ytExeCmd.AppendFormat("--proxy {0} " -f ([NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::ProxyServer.ToString()))
            
            $proxyAuthValue = [NoFuture.Util.Net]::GetAuthHeaderValue($null, $ProxyPwd) 

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
