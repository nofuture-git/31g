<#
 double-click of .ps1 from FileExplorer will fail if the path to the script
 contains spaces.
 http://connect.microsoft.com/PowerShell/feedback/details/788806/powershell-script-cannot-be-ran-outside-of-console-if-path-contains-spaces
 FIX:
 change value in registry at
 HKEY_CLASSES_ROOT\Applications\powershell.exe\shell\open\command
 
 from:
 "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe" "%1"
 to:
 "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe" "& \"%1\""
 
 use 
 --%
 at point where no further substitution is required 
#>

#invoke powershell from a .bat file
<#
@ECHO off
SET ThisScriptsDirectory=%~dp0
SET myPsScript=%ThisScriptsDirectory%\..\My-PsScript.ps1
SET AssemblyPath=%ThisScriptsDirectory%\bin\AnExample.dll

Powershell -Command "Set-ExecutionPolicy -ExecutionPolicy RemoteSigned";
PowerShell -Command "& '%spec%' '%AssemblyPath%' anotherArgHere etc";

SET /p userInput=Press any key to exit:
#>

<#
    example of instantiating a generic from powershell
#>
$coll = New-Object 'System.Collections.ObjectModel.Collection`1[System.String]'

#version 2 or higher
$myDict = New-Object 'System.Collection.Generics.Dictionary[string, int]'

#define a System.Func
$callback = [System.Func[int, bool]] { param($someInt); $someInt -gt 0}
$callback.Invoke(11) #True
$callback.Invoke(-4) #False
<#
    examples for adding types with references to other types
#>
Add-Type -Path ("C:\WINDOWS\system32\windowspowershell\v1.0\LabOne.Business.dll")

$references = @(
    "C:\WINDOWS\system32\windowspowershell\v1.0\LabOne.Business.dll",
    "C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\System.configuration.dll",
    "C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\System.Data.dll",
    "C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\System.Windows.Forms.dll",
    "C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\System.XML.dll"
)

$getAppointmentSrc = @"
/*
  Generics are difficult in ps so just wrap these into something more common...   
*/
public class CopyExamCentralDbRecs
{
    public void SetConnectionString(string connectionString)
    {
        LabOne.Business.Services.ExamViewService.SetConnectionString(connectionString);
    }
    public string GetConnectionString()
    {
       return LabOne.Business.Services.ExamViewService.GetConnectionString();
    }

    public LabOne.Business.Objects.Applicant GetApplicantRec(int App_id)
    {
        LabOne.Business.Objects.Applicant myApp =  LabOne.Business.Services.ExamViewService.GetObject<LabOne.Business.Objects.Applicant>(App_id);
        return myApp;
    }
    public void SaveApplicantRec(LabOne.Business.Objects.Applicant myApp)
    {
        LabOne.Business.Services.ExamViewService.SaveObject<Applicant>(Master.Appointment.Applicant, "username");
    }
}
"@

Add-Type -ReferencedAssemblies $references -TypeDefinition $getAppointmentSrc

<#
 having powershell recognize an array as a single arg to an object ctor
#>
$memoryStream = New-Object System.IO.MemoryStream -ArgumentList (,[byte[]]$buffer)

<#
    examples powershell function pointers
#>
function TFunction1($arg1, $arg2, $arg3)
{
    $toPrint = ("From TFunction 1`nArg1:{0,32}`nArg2:{1,32}`nArg3:{2,32}" -f $arg1, $arg2, $arg3)
    return $toPrint
}

function TFunction2($arg1, $arg2, $arg3)
{
    $toPrint = ("`From TFunction 2`nArg1:{0,32}`nArg2:{1,32}`nArg3:{2,32}" -f $arg1, $arg2, $arg3)
    return $toPrint
}


function TFunction3($arg1, $arg2, $arg3)
{
    $toPrint = ("From TFunction 3`nArg1:{0,32}`nArg2:{1,32}`nArg3:{2,32}" -f $arg1, $arg2, $arg3)
    return $toPrint
}

function CreateFuncPointer($functionNum)
{
    $func = (Get-Item ("function:TFunction{0}" -f $functionNum))
    return $func

}

function ExecuteFuncPointer($func,$arg1, $arg2, $arg3)
{
    return (& $func $arg1 $arg2 $arg3)
}

<#
    example using .NET Delegate and Event handlers
#>
Add-Type -TypeDefinition @"

public class HasAHandler
{

    public static void PointToMe(object o, System.Diagnostics.DataReceivedEventArgs e)
    {
        //do something

    }
}
"@

$acceptCertMethodInfo = [HasAHandler].GetMethod("PointToMe",(([System.Reflection.BindingFlags]::Public) -bor ([System.Reflection.BindingFlags]::Static)))
$myEventHandler = [System.Delegate]::CreateDelegate([System.Diagnostics.DataReceivedEventHandler],$acceptCertMethodInfo,$false)
$proc = (Get-Process -Name "MyConsole")
$evt = $proc.GetType().GetEvent("OutputDataReceived")
$evt.AddEventHandler($proc,$myEventHandler)

<#
    example send keystrokes from Powershell
	http://msdn.microsoft.com/en-us/library/8c6yea83%28v=vs.84%29.aspx
#>
function LaunchClassicWebsite($url, $proj){
    $objShell = New-Object -ComObject WScript.Shell
    $objShell.Run("vs05")
    $name = (Get-Process -Name "devenv" | ? {$_.Description -like "*Studio 2005"}).Name
    $loaded = $objShell.AppActivate($name)
    [System.Threading.Thread]::Sleep(1000)

    $objShell.SendKeys("%")
    [System.Threading.Thread]::Sleep(1000)

    $objShell.SendKeys("f")
    [System.Threading.Thread]::Sleep(1000)

    $objShell.SendKeys("e")
    [System.Threading.Thread]::Sleep(1000)

    $objShell.SendKeys($proj)
    [System.Threading.Thread]::Sleep(1000)

    $objShell.SendKeys("{ENTER}")
    [System.Threading.Thread]::Sleep(1000)

    Write-Progress -activity $("Starting Internet Explorer.") -status $("Waiting...");
    $myie = New-Object -ComObject InternetExplorer.Application
    [System.Threading.Thread]::Sleep(4000)
    Write-Progress -activity $("Navigating to '{0}'." -f $url) -status $("Waiting...") 
    $myie.Navigate($url);
    $myie.Visible = $true;
    $myie.Top = 0;
}

<#
    example Windows Security
#>

function WCFAndSecurity()
{
    [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes("C:\Projects\WindowsPowerShell\MyDll.dll"))
    
    $binding = New-Object System.ServiceModel.WSHttpBinding([System.ServiceModel.SecurityMode]::Transport)
    $binding.Security.Message.ClientCredentialType = [System.ServiceModel.MessageCredentialType]::Windows
    $binding.Security.Transport.ClientCredentialType = [System.ServiceModel.HttpClientCredentialType]::Windows
    
    #http://blogs.technet.com/b/ashleymcglone/archive/2011/08/29/powershell-sid-walker-texas-ranger-part-1.aspx
    [System.Reflection.Assembly]::LoadWithPartialName("System.IdentityModel")
    
    
    $myAcl = Get-ACL C:\Projects\WindowsPowerShell\SomePath
    
    #more on Ssdl strings
    $myAcl.GetSecurityDescriptorSddlForm([System.Security.AccessControl.AccessControlSections]::None)
    
    $ssdl = $myAcl.Sddl#this must be further parsed
    $ssdlLooksLike = "S-1-5-21-823518204-1078145449-682003330-48898"
    $sid = New-Object System.Security.Principal.SecurityIdentifier($ssdlLooksLike)
    $claim = [System.IdentityModel.Claims.Claim]::CreateWindowsSidClaim($sid)
    $endpointIdentity = [System.ServiceModel.EndpointIdentity]::CreateIdentity($claim)

    #this is probably what is needed 
    $endpointIdentity = [System.ServiceModel.EndpointIdentity]::CreateUpnIdentity("DOMAIN\user.name")

    $endpoint = New-Object System.ServiceModel.EndpointAddress($svcSection.Client.EndPoints[0].Address,$endpointIdentity)


    #http://msdn.microsoft.com/en-us/library/bb628618.aspx
    
}

function SetFolderPermissions(){
	$acl = Get-Acl "C:\WINDOWS\Temp"
	$permission = New-Object System.Security.AccessControl.FileSystemAccessRule(("{0}\IIS_WPG" -f $env:COMPUTERNAME),"FullControl","Allow")
	$acl.SetAccessRule($permission)
	Set-Acl "C:\WINDOWS\Temp" $acl
}
function AddUserToBox(){
#http://blogs.technet.com/b/heyscriptingguy/archive/2014/10/01/use-powershell-to-create-local-users.aspx
$cn = [ADSI]"WinNT://edlt"
$user = $cn.Create("User","mred")
$user.SetPassword("P@ssword1")
$user.setinfo()
$user.description = "Test user"
$user.SetInfo()
}
<#
LocalService account (preferred)
 - Name: NT AUTHORITY\LocalService
 - the account has no password (any password 
   information you provide is ignored)
 - HKCU represents the LocalService user account
 - has minimum privileges on the local computer
 - presents anonymous credentials on the network
 - A limited service account that is very similar 
   to Network Service and meant to run standard 
   least-privileged services. However unlike 
   Network Service it has no ability to access 
   the network as the machine.

NetworkService account
 - NT AUTHORITY\NetworkService
 - the account has no password (any password 
   information you provide is ignored)
 - HKCU represents the NetworkService user account
 - has minimum privileges on the local computer
 - presents the computer's credentials to remote 
   servers (e.g. VADER$)
 - If trying to schedule a task using it, enter 
   NETWORK SERVICE into the Select User or Group 
   dialog
 - Limited service account that is meant to run 
   standard least-privileged services. This account 
   is far more limited than Local System (or 
   even Administrator) but still has the right to 
   access the network as the machine (see caveat above).

LocalSystem account
 - Name: .\LocalSystem (can also use LocalSystem or ComputerName\LocalSystem)
 - the account has no password (any password information you provide is ignored)
 - HKCU represents the default user (LocalSystem has no profile of its own)
 - has extensive privileges on the local computer
 - presents the computer's credentials to remote servers
 - Completely trusted account, moreso than the administrator account. There is nothing on 
   a single box that this account can not do and it has the right to access the network 
   as the machine (this requires Active Directory and granting the machine account 
   permissions to something)
 - set in a windows service as 
  this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
   
   
#>


function IsCurrentUserAnAdmin(){
return ([System.Security.Principal.WindowsPrincipal][System.Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([System.Security.Principal.WindowsBuiltInRole]"Administrator")
}

function XPathWithNamespace(){
$myXml = [xml](Get-Content 'C:\Projects\31g\trunk\MyXmlDataWithNs.xml')
$nsMgr = New-Object System.Xml.XmlNamespaceManager($myXml.NameTable)
$nsMgr.AddNamespace("c","http://ServiceData.Entities.com")
$myXml.SelectNodes("//c:CompanyAssociation",$nsMgr)
}

function SortHashtableByValues(){
$myMatches = @{"PopulateDDLWithAbbrev.0.pdbLines" = 2; "PopulateDDLWithAbbrev.1.pdbLines" = 1; "PopulateDDLWithAbbrev.pdbLines" = 0}
$myMatches.GetEnumerator() | Sort-Object {$_.Value} -Descending
}

function WriteXmlWithPrettyPrinting(){
$myXml = [xml](Get-Content .\someFile.xml)
$workingDir = Resolve-Path .\someFile.xml -Parent #easier to read with PS than write with .NET

#... do stuff

$myXmlWriter = New-Object System.Xml.XmlTextWriter((Join-Path $workingDir $nm), [System.Text.Encoding]::UTF8)
$myXmlWriter.Formatting = [System.Xml.Formatting]::Indented
$nuspec.WriteContentTo($myXmlWriter)
$myXmlWriter.Flush()
$myXmlWriter.Dispose()
}

function Import-x509Certs(){
	#when mapping a specific machine account to a cert use the WinHttpCertCfg.exe tool from Microsoft
	#when just looking for data use
	ls Cert:\LocalMachine\My
	
	#assumes the .pfx file already exists, has been exported with a private key	
    $pfxPath = "<path to a .pfx file on the drive>"
    $pfxPwdPath = "<seems only present when exported from mmc>"

    $secureString = New-Object System.Security.SecureString

    $pwdBytes = [System.IO.File]::ReadAllBytes($pfxPwdPath)

    $pwdBytes | ? {$_ -gt 0x20} | % {$secureString.AppendChar([char]$_)}

    $myCert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2

    $myCert.Import([System.IO.File]::ReadAllBytes($pfxPath), 
           $secureString, 
           [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable)

    $secureString.Dispose()

    $certStore = New-Object System.Security.Cryptography.X509Certificates.X509Store(
      [System.Security.Cryptography.X509Certificates.StoreName]::My,
      [System.Security.Cryptography.X509Certificates.StoreLocation]::LocalMachine)#these would change based on need

    $certStore.Open([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)

    $certStore.Add($myCert)

    $thumbprint = $myCert.Thumbprint

    Write-Host "X509Certificate Thumbprint:`n$thumbprint"

    $certStore.Close()
}

function AccessInnerClassTypes(){
	#this again looks like the actual IL where the '+' is for an inner type
	[System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::AdminTools)
}

#look up Loader Exceptions
#$Error[<some count here>].Exception.InnerException.LoaderExceptions[<some other number>].StackTrace

function IISFromCommandLine(){
Import-Module WebAdministration

#get a user attached to an app pool
$userBefore = Get-ItemProperty -Path "IIS:\AppPools\My IISAppPool" -Name processModel.UserName.value

#set user properties
Set-ItemProperty -Path "IIS:\AppPools\My IISAppPool" -Name processModel.username -Value "MYDOMAIN\myUser.x.name"
Set-ItemProperty -Path "IIS:\AppPools\My IISAppPool" -Name processModel.password -Value "P@ssw0rd"

#http://www.iis.net/learn/manage/powershell/powershell-snap-in-configuring-ssl-with-the-iis-powershell-snap-in
#add a binding to a iis site
New-WebBinding -Name "Default Web Site" -IP "*" -Port 443 -Protocol https

#Get my cert (already in the Cert Store)
$myCert = ls "Cert:\CurrentUser\Root" | ? {$_.FriendlyName -like "*NoFuture*"} | Select-Object -First 1

#map the cert with the binding
Push-Location "IIS:\SslBindings"
$myCert | New-Item 0.0.0.0!443
Pop-Location

#iis settings are saved to 
# %windir%\system32\inetsrv\config\applicationHost.config

}

function SetWindowsServiceUser(){
	$myWmiService = Get-WmiObject win32_service -ComputerName $env:COMPUTERNAME -Filter "name='MyServiceName'"
	<#
	$inspectMe = $myWmiService | Get-Member | ? {$_.Name -eq "Change"} | Select-Object -First 1
	$inspectMe.Definition
	
	System.Management.ManagementBaseObject Change(
		System.String DisplayName, 
		System.String PathName, 
		System.Byte ServiceType, 
		System.Byte ErrorControl, 
		System.String StartMode, 
		System.Boolean DesktopInteract, 
		System.String StartName, 
		System.String StartPassword, 
		System.String LoadOrderGroup, 
		System.String[] LoadOrderGroupDependencies, 
		System.String[] ServiceDependencies)	
	#>
	$myWmiService.Change($null, $null, $null, $null, $null, $null, "MYDOMAIN\myUser", "P@ssw0rd")
	
}

function CreateNewPsSessionOnRemote(){
	#on your machine, run 'gpedit.msc'
	#Computer Configuration -> Administrative Templates -> System -> Credentials Delegation -> Allow Delegating Fresh Credentials
	#set to Enabled
	#click "Show..." button and add server names to the list
	
	#on the remote machine run the following
	Enable-WSManCredSSP -Role Server
	Set-Item WSMan:\localhost\Service\Auth\CredSSP -Value $true
	
	#on the client machine run
	Enable-WSManCredSSP -Role Client -DelegateComputer REMOTE_COMPUTER_NAME_HERE
}

#ps and annonymous enclosures
$myEnclosure = {11};
$myInvoke = 5 + (&$myEnclosure)

function ShowPopupToUser(){
#https://technet.microsoft.com/en-us/library/ff730939.aspx
$title = "Delete Files"
$message = "Do you want to delete the remaining files in the folder?"

$yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", `
    "Deletes all the files in the folder."

$no = New-Object System.Management.Automation.Host.ChoiceDescription "&No", `
    "Retains all the files in the folder."

$options = [System.Management.Automation.Host.ChoiceDescription[]]($yes, $no)

$result = $host.ui.PromptForChoice($title, $message, $options, 0) 

switch ($result)
    {
        0 {"You selected Yes."}
        1 {"You selected No."}
    }
}

function HaveUserPathToFile(){
[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
$ofd = New-Object System.Windows.Forms.OpenFileDialog -Property @{DefaultExt = "xml"; InitialDirectory = "C:\Program Files (x86)\"; Title = "Get your file" }
$ofdRslt = $ofd.ShowDialog()

if($ofdRslt -ne [System.Windows.Forms.DialogResult]::OK){ break; }

$targetFile = $ofd.FileName;
}

function ListSupportedCipherSuites(){
	ls HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols
}


