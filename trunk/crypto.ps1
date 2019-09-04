try{
if(-not [NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("md5sum",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Protect-File",$MyInvocation.MyCommand)
[NoFuture.Shared.Cfg.NfConfig+MyFunctions]::FunctionFiles.Add("Unprotect-File",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}

<#
    .synopsis
    Gets the MD5 hash of a given file
    
    .Description
    Gets the MD5 check sum hash from a given file. 
    Output is printed as a single continous string of the hash in
    hexidecimal values.
    
    .parameter Path
    A fully qualified file path
    
    .example
    C:\PS>Get-Md5CheckSum -Path 'C:\My Docs\MS Word Docs\myWordDoc.doc'
        
    3e4189c7f41c1d225c0a5f650cf4e6e5                                                                       

    .outputs
    String
    
#>
function Get-Md5CheckSum
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path
    )
    Process
    {
        #break on junk paths
        if(-not (Test-Path $Path)){Write-Host ("The path '{0}' was not found." -f $Path); break;}
            
        #get buffer 
        [byte[]] $buffer = [System.IO.File]::ReadAllBytes($Path)
            
        #generate md5 hash
        $chksum = [System.Security.Cryptography.MD5]::Create()
        $hash = ""
        $chksum.ComputeHash($buffer) | % {$hash += $_.ToString("x2") }
        return $hash
    }
}

Set-Alias md5sum Get-Md5CheckSum

<#
    .SYNOPSIS
    Encrypts the given file
    
    .DESCRIPTION
    Will encrypt the file with the X509 cert at 
    SecurityKeys.NoFutureX509Cert.
    
    .PARAMETER Path
    A fully qualified path to a single file.
    
    .EXAMPLE
    C:\PS>Protect-File -Path 'C:\My Docs\MS Word Docs\My Doc.docx'
        
    .OUTPUTS
    string
#>
function Protect-File
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path
    )
    Process
    {
        #break on junk paths
        if(-not (Test-Path $Path)){Write-Host ("The path '{0}' was not found." -f $Path); break;}
        
        #homebrew crypto works on files only
        if([System.IO.Directory]::Exists($Path)){
           throw "Only intended for files on the drive, not directories."
           break;
        }
        
        $Path = (Resolve-Path $Path).Path

        $nfx509CertPath = [NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::NoFutureX509Cert

        if([string]::IsNullOrWhiteSpace($nfx509CertPath) -or -not (Test-Path $nfx509CertPath)){
            Write-Host ("Don't know where to find the NoFuture x509 cert at.  " + 
                         "Set the shared var at SecurityKeys NoFutureX509Cert")
            break
        }

        $nfx509CertPath = (Resolve-Path $nfx509CertPath).Path
           
        [NoFuture.Encryption.NfX509]::EncryptFile($Path, $nfx509CertPath)
        $encFile = $Path + [NoFuture.Shared.Core.Constants]::NF_CRYPTO_EXT
        if(Test-Path $encFile){
            Remove-Item -Path $Path -Force
        }
        return $encFile
    }
}

<#
    .SYNOPSIS
    Decrypts the given file.
    
    .DESCRIPTION
    Will decrypt the file with the X509 cert at 
    SecurityKeys.NoFutureX509Cert and the given password.
    
    .PARAMETER Path
    A fully qualified path to a single file or folder

    .PARAMETER NfPassword
    The password for the X509 private key
    
    .EXAMPLE
    C:\PS>Unprotect-Files -Path 'C:\My Docs\MS Word Docs\My Doc.docx.nfk'
        
    .OUTPUTS
    string
#>
function Unprotect-File
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path,
        [Parameter(Mandatory=$true,position=1)]
        [string] $NfPassword

    )
    Process
    {
        #break on junk paths
        if(-not (Test-Path $Path)){Write-Host ("The path '{0}' was not found." -f $Path); break;}
        
        #homebrew crypto works on files only
        if([System.IO.Directory]::Exists($Path)){
           throw "Only intended for files on the drive, not directories."
           break;
        }
        
        if([string]::IsNullOrWhiteSpace($Pwd)){
            throw "Pwd must be a non-null, non-empty string"
            break;
        }

        $Path = (Resolve-Path $Path).Path

        $nfx509CertPath = [NoFuture.Shared.Cfg.NfConfig+SecurityKeys]::NoFutureX509Cert

        if([string]::IsNullOrWhiteSpace($nfx509CertPath)){
            Write-Host ("Don't know where to find the NoFuture x509 cert at. " + 
                        " Set the shared var at SecurityKeys NoFutureX509Cert")
            break
        }

        $nfx509CertPath = (Resolve-Path $nfx509CertPath).Path

        $nfx509CertDir = Split-Path $nfx509CertPath -Parent

        $nfx509CertPath =  Join-Path $nfx509CertDir `
                           (([System.IO.Path]::GetFileNameWithoutExtension($nfx509CertPath)) + ".pfx")

        if(-not (Test-Path $nfx509CertPath)){
            throw ("A .pfx copy of the NoFuture X509 certificate is required " + 
                        "and is expected to be placed next to the SecurityKeys.NoFutureX509Cert")
            break;
        }

        [NoFuture.Encryption.NfX509]::DecryptFile($Path, $nfx509CertPath,$NfPassword)
        $dencDir = Split-Path $Path -Parent

        $dencFile =  Join-Path $dencDir ([System.IO.Path]::GetFileNameWithoutExtension($Path))
        if(Test-Path $dencFile){
            Remove-Item -Path $Path -Force
        }
            
        return $dencFile
    }
}
