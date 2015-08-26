try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Generate-WcfServiceProxy",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Generate-WsdlServiceProxy",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-WSHttpBinding",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-EndpointAddress",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
<#
    .synopsis
    Runs the SDKs svcUtil to generate C# src files and then compiles them.
    
    .Description
    Given the URL the script will generate a source-code cs files
    using the svcutil.exe command-line utilities, and, in addition, 
    will compile both using .NET 4.0 C# compiler.
    The output is located at the global temp.Code\svcUtil variable's location.
    
    Lastly, svcutil.exe is used for services whose url ends with 
    .svc only.  Don't expect output if the URL is missing this extension.
    
    NOTE: the svcUtil.exe is expected to be .NET 4.0's version 
    and located at [NoFuture.X64Tools]::SvcUtil

    NOTE:a command log is generated and placed along side the results. 
    It contains the command literals used to generate the output.
    
    .parameter Url
    The URL of the webservices itself
    
    .example
    C:\PS> Generate-WcfServiceProxy -Url "https://localhost/eipservices/widgetservice.svc"
    
    .outputs
    null
    
#>
function Generate-WcfServiceProxy
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $Url,
        [Parameter(Mandatory=$false,position=1)]
        [array] $ReferencePaths,
        [Parameter(Mandatory=$false,position=2)]
        [string] $Namespace
    )
    Process
    {
        #have svcutil's output contained in a single seperate folder
        $svcUtilDir = ([NoFuture.TempDirectories]::SvcUtil)
        
        #write cmd line entries to a log
        $logFile = (Join-Path $svcUtilDir "cmd.log")
    
        #preserve for inspection later
        $originalUrl = $url
        
        #just leave if URL does not end with .svc
        #if(-not $originalUrl.EndsWith(".svc")){return;}
        
        #verify the input
        if($Url -eq $null -or $Url.Trim() -eq "") {throw "the URL is null or empty."}
        if (-not [System.Uri]::IsWellFormedUriString($Url,[System.UriKind]::RelativeOrAbsolute)) {throw "bad Uri format."}
        
        #verify svcutil is present
        $throwEx = $false
        $svdUtilEXE = Test-Path ([NoFuture.X64Tools]::SvcUtil)
        if($env:PROCESSOR_ARCHITECTURE = "AMD64")
        {
            if (-not (Test-Path ([NoFuture.X64Tools]::SvcUtil))){ $throwEx = $true }
            $svdUtilEXE = ([NoFuture.X64Tools]::SvcUtil)
        }
        else
        {
            if (-not (Test-Path ([NoFuture.X86Tools]::SvcUtil))){ $throwEx = $true }
            $svdUtilEXE = ([NoFuture.X86Tools]::SvcUtil)
        }
        if($throwEx){throw ("This computer is {0} architecture; and 'svcUtil.exe' was expected at '{1}' but is not present." -f$env:PROCESSOR_ARCHITECTURE,$global:x64tools.SvcUtil)}
        
        #insure temp drop location is present
        if(-not(Test-Path $svcUtilDir)){mkdir $svcUtilDir}
        
        #switch non-SSL protocal
        if($url.StartsWith("https")) {$url = ("http{0}" -f $url.SubString(5,$url.Length-5)) }
        
        #replace 'localhost' with full-domain
        if($url.Contains("//localhost")){$url = $url.Replace("localhost",("{0}.{1}" -f $env:COMPUTERNAME,$env:USERDNSDOMAIN)) }
        
        #add the wsdl to the extension
        if(-not($url.EndsWith("?wsdl"))) { $url = $url + "?wsdl" }
        
        #extract service name from url
        $svcName = $(Split-Path -Path (New-Object System.Uri($originalUrl)).PathAndQuery -Leaf).Split(".")[0]
        
        #get various fullname paths of generated files
        $codePath = $(Resolve-Path $svcUtilDir)
        ls -Path $codePath | ? {$_.Name -like ("{0}*" -f $svcName)} | % {rm -Path $_.FullName}
        $svcUtilSrc = ("{0}.svcutil" -f  $svcName)
        $svcUtilBin = ("{0}.dll" -f $svcUtilSrc)

        #draft references, same for csc.exe and svcutil.exe
        $refCmd = ""
        if($ReferencePaths -ne $null -and $ReferencePaths.Length -gt 0){
            $ReferencePaths | ? {Test-Path $_} | % {
                $refCmd += ("/reference:`"{0}`" " -f $_)
            }
        }
        
        #draft a namespace switch if provided
        $ns = ""
        if(-not [String]::IsNullOrWhiteSpace($Namespace)){
            $ns = " /namespace:`"*,$Namespace`""
        }

        $svcUtilCmd = ("& `"{0}`" /d:{1} /out:{2}.cs /t:code /serializer:Auto /config:{2}.dll.config {4}{5} '{3}'" -f $svdUtilEXE,$codePath,$svcUtilSrc,$url,$refCmd,$ns)

        #get the results of svcutil.exe
        $stdOut = Invoke-Expression $svcUtilCmd
        Write-Host "svcUtil.exe output ::" -ForegroundColor "Yellow"
        Write-Host $stdOut
        ("{0:yyyyMMdd HHmmss} [{1}]" -f $(Get-Date),$svcUtilCmd) >> $logFile
        
        #path the the C# compiler
        $cscCompiler = (Join-Path $global:net40Path $global:cscExe)
        
        #compile command for svcUtil's output
        $svcUtilCscCmd = ("& {0} /t:library /debug /nologo /out:'{2}\{3}' {4} '{2}\{1}.cs'" -f $cscCompiler, $svcUtilSrc,$codePath,$svcUtilBin,$refCmd)

        #compile the svcUtil gen'ed cs file
        $stdOut = Invoke-Expression -Command $svcUtilCscCmd
        Write-Host "csc.exe output ::" -ForegroundColor "Yellow"
        Write-Host $stdOut
        ("{0:yyyyMMdd HHmmss} [{1}]" -f $(Get-Date),$svcUtilCscCmd) >> $logFile
        
    }
}

<#
    .synopsis
    Runs the SDKs wsdl.exe to generate C# src files and then compiles it.
    
    .Description
    Given the URL the script will generate a source-code cs files
    using the wsdl.exe command-line utilities, and, in addition, will 
    compile it using .NET 4.0 C# compiler.
    The output is located at the global temp.Code\wsdl.
    
    NOTE: the svcUtil.exe is expected to be .NET 4.0's version 
    and located at $global:[x64|x86]tools.SvcUtil
    
    NOTE:a command log is generated and placed along side the results. 
    It contains the command literals used to generate the output.
    
    .parameter Url
    The URL of the webservices itself
    
    .example
    C:\PS> Generate-WsdlServiceProxy -Url "https://wssim.labone.com/eoimageaccess/basicaccess.asmx"
    
    .outputs
    null
    
#>
function Generate-WsdlServiceProxy
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $Url
    )
    Process
    {
        $wsdlDir = ([NoFuture.TempDirectories]::Wsdl)
        #write cmd line entries to a log
        $logFile = (Join-Path $wsdlDir "cmd.log")
    
        #preserve for inspection later
        $originalUrl = $url
        
        #verify the input
        if($Url -eq $null -or $Url.Trim() -eq "") {throw "the URL is null or empty."}
        if (-not [System.Uri]::IsWellFormedUriString($Url,[System.UriKind]::RelativeOrAbsolute)) {throw "bad Uri format."}
        #verify svcutil is present
        $throwEx = $false
        $wsdlEXE = Test-Path ([NoFuture.X64Tools]::Wsdl)
        if($env:PROCESSOR_ARCHITECTURE = "AMD64")
        {
            if (-not (Test-Path ([NoFuture.X64Tools]::Wsdl))){ $throwEx = $true }
            $wsdlEXE = ([NoFuture.X64Tools]::Wsdl)
        }
        else
        {
            if (-not (Test-Path ([NoFuture.X86Tools]::Wsdl))){ $throwEx = $true }
            $wsdlEXE = ([NoFuture.X86Tools]::Wsdl)
        }
        if($throwEx){throw ("This computer is {0} architecture; and 'Wsdl.exe' was expected at '{1}' but is not present." -f$env:PROCESSOR_ARCHITECTURE,([NoFuture.X64Tools]::Wsdl))}
        
        #insure temp drop location is present
        if(-not(Test-Path $wsdlDir)){mkdir $wsdlDir}
        
        #switch non-SSL protocal
        if($url.StartsWith("https")) {$url = ("http{0}" -f $url.SubString(5,$url.Length-5)) }
        
        #replace 'localhost' with full-domain
        if($url.Contains("//localhost")){$url = $url.Replace("localhost",("{0}.{1}" -f $env:COMPUTERNAME,$env:USERDNSDOMAIN)) }
        
        #add the wsdl to the extension
        if(-not($url.EndsWith("?wsdl"))) { $url = $url + "?wsdl" }
        
        #extract service name from url
        $svcName = $(Split-Path -Path (New-Object System.Uri($url)).PathAndQuery -Leaf).Split(".")[0]
        
        #get various fullname paths of generated files
        $codePath = $(Resolve-Path $wsdlDir)
        ls -Path $codePath | ? {$_.Name -like ("{0}*" -f $svcName)} | % {rm -Path $_.FullName}
        $wsdlSrc = ("{0}.wsdl" -f $svcName)
        $wsdlBin = ("{0}.dll" -f $wsdlSrc)
        
        $wsdlCmd = ("& `"{0}`" /nologo /namespace:NoFuture /out:'{1}\{2}.cs' '{3}'" -f $wsdlEXE,$codePath,$wsdlSrc,$url)
        
        
        #now the results from wsdl.exe
        $stdOut = Invoke-Expression $wsdlCmd
        Write-Host "wsdl.exe output ::" -ForegroundColor "Yellow"
        Write-Host $stdOut
        ("{0:yyyyMMdd HHmmss} [{1}]" -f $(Get-Date),$wsdlCmd) >> $logFile


        #path the the C# compiler
        $cscCompiler = (Join-Path $global:net40Path $global:cscExe)
        
        #compile command for wsdl's output
        $wsdlCscCmd = ("& {0} /t:library /debug /nologo /out:'{2}\{3}' '{2}\{1}.cs'" -f $cscCompiler, $wsdlSrc,$codePath,$wsdlBin)

        #prep for final assembly
        ls -Path $codePath | ? {@(".dll",".pdb") -contains $_.Extension} | % {rm -Path $_.FullName}
        
        #join all the files into a single assembly
        $finalDll = ("{0}\{1}.dll" -f $codePath,$svcName)
        $finalCscCmd = ("& {0} /t:library /debug /nologo /out:'{1}' '{2}\*.cs'" -f $cscCompiler,$finalDll,$codePath)
        $stdOut = Invoke-Expression -Command $finalCscCmd
        Write-Host "csc.exe output ::" -ForegroundColor "Yellow"
        Write-Host $stdOut
        ("{0:yyyyMMdd HHmmss} [{1}]" -f $(Get-Date),$finalCscCmd) >> $logFile
        
    }
}

<#
    .synopsis
    Returns an instance of System.ServiceModel.WSHttpBinding.
    
    .Description
    Given a svcutil.exe generated config file as an input, this
    cmdlet generates to coorsponding WSHttpBinding object based 
    on the values therein.
    
    .parameter ConfigFile
    The path to a generated .config file generated by the
    svcutil.exe.
    
    .example
    C:\PS>$bindings = Get-WSHttpBinding "C:\Projects\WindowsPowerShell\temp\code\widgetservice.svcutil.dll.config"
    
    .outputs
    System.ServiceModel.WSHttpBinding
#>
function Get-WSHttpBinding
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $ConfigFile
    )
    Process
    {
        $ConfigFile = (Resolve-Path $ConfigFile).Path
        $wsHttpConfig = ([NoFuture.Shared.WcfClient.Bindings]::GetWsHttpBindings($ConfigFile) | Select-Object -First 1)
        return $wsHttpConfig

    }
}

<#
    .SYNOPSIS
    Returns an instance of System.ServiceModel.NetMsmqBinding.
    
    .DESCRIPTION
    Given a svcutil.exe generated config file as an input, this
    cmdlet generates to coorsponding NetMsmqBinding object based 
    on the values therein.
    
    .PARAMETER ConfigFile
    The path to a generated .config file generated by the
    svcutil.exe.
    
    .EXAMPLE
    C:\PS>$bindings = Get-NetMsmqBinding "C:\Projects\31g\trunk\temp\code\svcUtil\MyMsmqService.svcutil.dll.config"
    
    .OUTPUTS
    System.ServiceModel.NetMsmqBinding
#>
function Get-NetMsmqBinding
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $ConfigFile
    )
    Process
    {
        $ConfigFile = (Resolve-Path $ConfigFile).Path

        $msmqConfig = ([NoFuture.Shared.WcfClient.Bindings]::GetNetMsmqBindings($ConfigFile) | Select-Object -First 1)
        return $msmqConfig
    }
}

<#
    .SYNOPSIS
    Returns an instance of System.ServiceModel.NetTcpBinding.
    
    .DESCRIPTION
    Given a svcutil.exe generated config file as an input, this
    cmdlet generates to coorsponding NetTcpBinding object based 
    on the values therein.
    
    .PARAMETER ConfigFile
    The path to a generated .config file generated by the
    svcutil.exe.
    
    .EXAMPLE
    C:\PS>$bindings = Get-NetMsmqBinding "C:\Projects\31g\trunk\temp\code\svcUtil\MyNetTcpService.config"
    
    .OUTPUTS
    System.ServiceModel.NetTcpBinding
#>
function Get-NetTcpBinding
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $ConfigFile
    )
    Process
    {
        $ConfigFile = (Resolve-Path $ConfigFile).Path

        $tcpConfig = ([NoFuture.Shared.WcfClient.Bindings]::GetNetTcpBindings($ConfigFile) | Select-Object -First 1)
        return $tcpConfig
    }
}

<#
    .synopsis
    Returns the first System.ServiceModel.EndpointIdentity found in the config file 
    
    .Description
    Given a svcutil.exe generated config file as an input, this
    cmdlet generates to coorsponding an System.ServiceModel.EndpointIdentity
    object based on contents of the config file.
    
    .parameter ConfigFile
    The path to a generated .config file generated by the
    svcutil.exe.
    
    .parameter UpnIdentity
    The Domain name of the user (e.g. MYDOMAIN\john.b.doe)
    
    .example
    C:\PS>$endpointElements = Get-EndpointAddress "C:\Projects\WindowsPowerShell\temp\code\widgetservice.svcutil.dll.config"
    
    .outputs
    System.ServiceModel.EndpointIdentity
#>			
function Get-EndpointAddress
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $ConfigFile,
        [Parameter(Mandatory=$false,position=1)]
        [String] $UpnIdentity
    )
    Process
    {
        $ConfigFile = (Resolve-Path $ConfigFile).Path

        $endpt = ([NoFuture.Read.Config.ExeConfig]::GetEndpointAddress($ConfigFile, $UpnIdentity) | Select-Object -First 1)
        return $endPt
        
    }
}

<#
    .synopsis
    Given the Uri, an instace of WsdlImporter is returned.
    
    .Description
    A Uri is resolved and associated to a new instance of 
    System.ServiceModel.Description.Wsdlimporter which, in turn,
    may be used to get endpoints, contracts and bindings from a 
    given WCF service's MEX data.
    
    .parameter Uri
    The string value of the Uri at which the WCF service is being broadcast.
    
    .parameter MaxResolvedReferences
    Any non-zero value is assigned to the MetadataExchangeClient's likewise
    property, .NET default this to 10.
    
    .example
    C:\PS>$metadata = Get-WcfMetadata "http://localhost:8081/CaseService.svc?wsdl"
    C:\PS>$endpoints = $importer.ImportAllEndpoints() #get a collection of ServiceEndpoints
    
    .outputs
    System.ServiceModel.Description.Wsdlimporter
    
#>
function Get-WcfMetadata
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $Uri,
        [Parameter(Mandatory=$false,position=1)]
        [int] $MaxResolvedReferences
    )
    Process
    {
        $mexUri = New-Object System.Uri($Uri)
        $mexClient = New-Object System.ServiceModel.Description.MetadataExchangeClient($mexUri,[System.ServiceModel.Description.MetadataExchangeClientMode]::HttpGet)
        if($MaxResolvedReferences -gt 0)
        {
            $mexClient.MaximumResolvedReferences = $MaxResolvedReferences
        }
        $mexData = $mexClient.GetMetadata()
        
        return New-Object System.ServiceModel.Description.Wsdlimporter($mexData)
    }
}
