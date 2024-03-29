try{
if(-not [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("New-WcfServiceProxy",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("New-WsdlServiceProxy",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-WSHttpBinding",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Get-EndpointAddress",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
<#
    .SYNOPSIS
    Runs the SDKs svcUtil to generate C# src files and then compiles them.
    
    .DESCRIPTION
    Given the URL the script will generate a source-code cs files
    using the svcutil.exe command-line utilities, and, in addition, 
    will compile both using .NET 4.0 C# compiler.
    The output is located at the global temp.Code\svcUtil variable's location.
    
    The svcUtil.exe full path is assigned in NoFuture.Shared.Cfg.NfConfig

    NOTE:a command log is generated and placed along side the results. 
    It contains the command literals used to generate the output.
    
    .PARAMETER Uri
    The URL of the webservices itself or a metadata Document Path.
    
    .EXAMPLE
    C:\PS> New-WcfServiceProxy -Uri "https://localhost/eipservices/widgetservice.svc"

    .EXAMPLE
    C:\PS> New-WcfServiceProxy -Uri "C:\Projects\MyWsdls\SomeServiceWsdl.xml"
    
    .outputs
    null
    
#>
function New-WcfServiceProxy
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $Uri,
        [Parameter(Mandatory=$false,position=1)]
        [array] $ReferencePaths,
        [Parameter(Mandatory=$false,position=2)]
        [string] $Namespace,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $AsWsdlXsdFile
    )
    Process
    {
        #have svcutil's output contained in a single seperate folder
        $svcUtilDir = ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::SvcUtil)
        
        #write cmd line entries to a log
        $logFile = (Join-Path $svcUtilDir "cmd.log")
    
        #preserve for inspection later
        $originaluri = $uri
        
        #verify the input
        if([string]::IsNullOrWhiteSpace($originaluri)) {
            throw "the uri is null or empty."
            break;
        }

        if ($AsWsdlXsdFile) {
            if(-not (Test-Path $Uri)){
                throw "bad file name."
                break;
            }
        }
        else{
            if(-not [System.Uri]::IsWellFormedUriString($uri, [System.UriKind]::RelativeOrAbsolute)){
                throw "bad Uri format."
                break;
            }
        }
        
        #verify svcutil is present
        $throwEx = $false
        $svcUtilEXE = Test-Path ([NoFuture.Shared.Cfg.NfConfig+X64]::SvcUtil)
        if($env:PROCESSOR_ARCHITECTURE = "AMD64")
        {
            if (-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+X64]::SvcUtil))){ 
                $throwEx = $true 
            }
            $svcUtilEXE = ([NoFuture.Shared.Cfg.NfConfig+X64]::SvcUtil)
            
        }
        else
        {
            if (-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+X86]::SvcUtil))){ 
                $throwEx = $true 
            }
            $svcUtilEXE = ([NoFuture.Shared.Cfg.NfConfig+X86]::SvcUtil)
            
        }
        if($throwEx){
            $exMsg = "This computer is $env:PROCESSOR_ARCHITECTURE architecture; "
            $exMsg += "and 'svcUtil.exe' was expected at "
            $exMsg += "'$svdUtilEXE' but is not present."
            throw $exMsg
            break;
        }
        
        #insure temp drop location is present
        if(-not(Test-Path $svcUtilDir)){mkdir $svcUtilDir}
        
        if($AsWsdlXsdFile){
            $svcName = [System.IO.Path]::GetFileNameWithoutExtension($Uri)
        }
        else{
            #replace 'localhost' with full-domain
            if($uri.Contains("//localhost")){
                $uri = $uri.Replace("localhost","$env:COMPUTERNAME.$env:USERDNSDOMAIN") 
            }
        
            #add the wsdl to the extension
            if(-not($uri.EndsWith("?wsdl"))) { 
                $uri = $uri + "?wsdl" 
            }
        
            #extract service name from uri
            $svcName = $(Split-Path -Path (New-Object System.Uri($originaluri)).PathAndQuery -Leaf).Split(".")[0]
        }
        
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
            $ns = "/namespace:`"*,$Namespace`""
        }

        $svcUtilCmd = "& `"$svcUtilEXE`" /d:$codePath /out:$svcUtilSrc.cs /t:code /serializer:Auto /config:$svcUtilSrc.dll.config $refCmd $ns '$uri'"

        #get the results of svcutil.exe
        $stdOut = Invoke-Expression $svcUtilCmd
        Write-Host "svcUtil.exe output ::" -ForegroundColor "Yellow"
        Write-Host $stdOut
        ("{0:yyyyMMdd HHmmss} [{1}]" -f $(Get-Date),$svcUtilCmd) >> $logFile
        
        #path the the C# compiler
        $cscCompiler = "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\csc.exe"
        
        #compile command for svcUtil's output
        $svcUtilCscCmd = "& $cscCompiler /t:library /debug /nologo /out:'$codePath\$svcUtilBin' $refCmd '$codePath\$svcUtilSrc.cs'"

        #compile the svcUtil gen'ed cs file
        $stdOut = Invoke-Expression -Command $svcUtilCscCmd
        Write-Host "csc.exe output ::" -ForegroundColor "Yellow"
        Write-Host "Import binary at '$codePath\$svcUtilBin'"
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
    C:\PS> New-WsdlServiceProxy -Url "https://wssim.labone.com/eoimageaccess/basicaccess.asmx"
    
    .outputs
    null
    
#>
function New-WsdlServiceProxy
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [String] $Url
    )
    Process
    {
        $wsdlDir = ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Wsdl)
        #write cmd line entries to a log
        $logFile = (Join-Path $wsdlDir "cmd.log")
    
        #preserve for inspection later
        $originalUrl = $url
        
        #verify the input
        if($Url -eq $null -or $Url.Trim() -eq "") {throw "the URL is null or empty."}
        if (-not [System.Uri]::IsWellFormedUriString($Url,[System.UriKind]::RelativeOrAbsolute)) {
            throw "bad Uri format."
        }
        #verify svcutil is present
        $throwEx = $false
        $wsdlEXE = Test-Path ([NoFuture.Shared.Cfg.NfConfig+X64]::Wsdl)
        if($env:PROCESSOR_ARCHITECTURE = "AMD64")
        {
            if (-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+X64]::Wsdl))){ $throwEx = $true }
            $wsdlEXE = ([NoFuture.Shared.Cfg.NfConfig+X64]::Wsdl)
        }
        else
        {
            if (-not (Test-Path ([NoFuture.Shared.Cfg.NfConfig+X86]::Wsdl))){ $throwEx = $true }
            $wsdlEXE = ([NoFuture.Shared.Cfg.NfConfig+X86]::Wsdl)
        }
        if($throwEx){
            $exMsg = "This computer is $env:PROCESSOR_ARCHITECTURE architecture; "
            $exMsg += "and 'Wsdl.exe' was expected at "
            $exMsg += "'$wsdlEXE' but is not present."
            throw $exMsg;
        }
        
        #insure temp drop location is present
        if(-not(Test-Path $wsdlDir)){mkdir $wsdlDir}
        
        #replace 'localhost' with full-domain
        if($url.Contains("//localhost")){$url = $url.Replace("localhost","$env:COMPUTERNAME.$env:USERDNSDOMAIN") }
        
        #add the wsdl to the extension
        if(-not($url.EndsWith("?wsdl"))) { $url = $url + "?wsdl" }
        
        #extract service name from url
        $svcName = $(Split-Path -Path (New-Object System.Uri($url)).PathAndQuery -Leaf).Split(".")[0]
        
        #get various fullname paths of generated files
        $codePath = $(Resolve-Path $wsdlDir)
        ls -Path $codePath | ? {$_.Name -like ("{0}*" -f $svcName)} | % {rm -Path $_.FullName}
        $wsdlSrc = "$svcName.wsdl"
        $wsdlBin = "$wsdlSrc.dll"
        
        $wsdlCmd = "& `"$wsdlEXE`" /nologo /namespace:NoFuture /out:'$codePath\$wsdlSrc.cs' '$url'"
        
        
        #now the results from wsdl.exe
        $stdOut = Invoke-Expression $wsdlCmd
        Write-Host "wsdl.exe output ::" -ForegroundColor "Yellow"
        Write-Host $stdOut
        ("{0:yyyyMMdd HHmmss} [{1}]" -f $(Get-Date),$wsdlCmd) >> $logFile


        #path the the C# compiler
        $cscCompiler = "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\csc.exe"
        
        #compile command for wsdl's output
        $wsdlCscCmd = "& $cscCompiler /t:library /debug /nologo /out:'$codePath\$wsdlBin' '$codePath\$wsdlSrc.cs'"

        #prep for final assembly
        ls -Path $codePath | ? {@(".dll",".pdb") -contains $_.Extension} | % {rm -Path $_.FullName}
        
        #join all the files into a single assembly
        $finalDll = "$codePath\$svcName.dll"
        $finalCscCmd = "& $cscCompiler /t:library /debug /nologo /out:'$finalDll' '$codePath\*.cs'"
        $stdOut = Invoke-Expression -Command $finalCscCmd
        Write-Host "csc.exe output ::" -ForegroundColor "Yellow"
        Write-Host "Import binary at '$finalDll'"
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
