try{
if(-not [NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("md5sum",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-SHA256HashString",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-AesEncryptedValue",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-AesDecryptedValue",$MyInvocation.MyCommand)
[NoFuture.Shared.NfConfig+MyFunctions]::FunctionFiles.Add("Get-HMACSHA1HashString",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
<#
    .synopsis
    Gets a SHA256 hash using aesIV value as the salt.
    
    .Description
    Given the same string value a base64 hash is returned,
    which, given the same salt value, should be reproduced 
    enabling equity comparasion without view of plain text 
    value.
    
    .parameter Value
    The string candidate to be hashed.
    
    .example
    C:\PS>$hashValue = Get-SHA256HashString -Value 'Hello from SHA256 Hashworld'
    C:\PS>(Get-SHA256HashString -Value 'Hello from SHA256 Hashworld') -eq $hashValue
    True    

    .outputs
    string
    
#>
function Get-SHA256HashString
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Value
    )
    Process
    {
        $cryptoServiceProvider = [System.Security.Cryptography.SHA256CryptoServiceProvider]::Create()
        
        $data = [System.Text.Encoding]::UTF8.GetBytes($value)
        
        $salt = [System.Text.Encoding]::UTF8.GetBytes([System.Convert]::FromBase64String([NoFuture.Shared.NfConfig+SecurityKeys]::AesIV))
        
        $dataAndSalt = [byte[]]($data + $salt)
        
        $encryptedValue = $cryptoServiceProvider.ComputeHash($dataAndSalt)
        
        $encryptedString = [System.Convert]::ToBase64String($encryptedValue)
        
        return $encryptedString
    }
}

<#
    .SYNOPSIS
    Gets a HMACSHA1 hash value of a string.
    
    .DESCRIPTION
    Computes the hash value of some string 
    using HMACSHA1 and the key NoFuture namespace
    of the like name.
    
    .PARAMETER Value
    The string candidate to be hashed.
    
    .EXAMPLE
    C:\PS>$hashValue = Get-HMACSHA1HashString -Value 'Hello from HMACSHA1 Hashworld'
    C:\PS>(Get-HMACSHA1HashString -Value 'Hello from HMACSHA1 Hashworld') -eq $hashValue
    True    

    .outputs
    string
    
#>
function Get-HMACSHA1HashString
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Value
    )
    Process
    {
        $privKey = [System.Convert]::FromBase64String([NoFuture.Shared.NfConfig+SecurityKeys]::HMACSHA1.Replace("-", "+").Replace("_", "/"))
        $hmacsha1 = New-Object System.Security.Cryptography.HMACSHA1
        $hmacsha1.Key = $privKey
        return ([System.Convert]::ToBase64String($hmacsha1.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($Value))).Replace("-", "+").Replace("_", "/"))

    }
}

<#
    .synopsis
    Encrypts a string using AES 256 and returns its
    base64 value.
    
    .Description
    The key and vector are both global variables under $aesM listed
    within the constants.  Furthermore, the cipher-mode is CBC with 
    PKCS7 padding.
    
    .parameter Value
    Plain text string to be encrypted.
    
    .example
    C:\PS>Get-AesEncryptedValue -Value "hello from encryption world"
    OSQb8drdNd1FSMuepObOXzPO6K/Tvwhk+fhUjwx8E/U=    
   
    .outputs
    string
    
#>
function Get-AesEncryptedValue
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Value
    )
    Process
    {
        $aesM = [System.Security.Cryptography.AESManaged]::Create()
        $aesM.Mode = [System.Security.Cryptography.CipherMode]::CBC
        $aesM.Padding = [System.Security.Cryptography.PaddingMode]::PKCS7
        $aesM.Key = [System.Convert]::FromBase64String([NoFuture.Shared.NfConfig+SecurityKeys]::AesEncryptionKey)
        $aesM.IV = [System.Convert]::FromBase64String([NoFuture.Shared.NfConfig+SecurityKeys]::AesIV)
        
        
        $aesMEncrypt = $aesM.CreateEncryptor()
        $myByteArray = [System.Text.Encoding]::UTF8.GetBytes($Value)
        $encryptedValue = $aesMEncrypt.TransformFinalBlock($myByteArray,0,$myByteArray.Length)
        
        $myEncryptedString = [System.Convert]::ToBase64String($encryptedValue)
        
        return $myEncryptedString
        
    }
}

<#
    .synopsis
    Decrypts a string which was encrypted using AES 256
    
    .Description
    The key and vector are both global variables under $aesM listed
    within the constants.  Furthermore, the cipher-mode is CBC with 
    PKCS7 padding.
    
    .parameter Value
    A base64 encrypted string.
    
    .example
    C:\PS>Get-AesDecryptedValue -Value "OSQb8drdNd1FSMuepObOXzPO6K/Tvwhk+fhUjwx8E/U="
    hello from encryption world
    
    .outputs
   string
    
#>
function Get-AesDecryptedValue
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Value
    )
    Process
    {
        $aesM = [System.Security.Cryptography.AESManaged]::Create()
        $aesM.Mode = [System.Security.Cryptography.CipherMode]::CBC
        $aesM.Padding = [System.Security.Cryptography.PaddingMode]::PKCS7
        $aesM.Key = [System.Convert]::FromBase64String([NoFuture.Shared.NfConfig+SecurityKeys]::AesEncryptionKey)
        $aesM.IV = [System.Convert]::FromBase64String([NoFuture.Shared.NfConfig+SecurityKeys]::AesIV)
        
        $aesMDecrypt = $aesM.CreateDecryptor()
        
        $myEncryptedByteArray = [System.Convert]::FromBase64String($Value)
        $decryptedValue = $aesMDecrypt.TransformFinalBlock($myEncryptedByteArray,0,$myEncryptedByteArray.Length)
        
        $myDecryptedValue = [System.Text.Encoding]::UTF8.GetString($decryptedValue)
        
        return $myDecryptedValue
    }
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

        $nfx509CertPath = [NoFuture.Shared.NfConfig+SecurityKeys]::NoFutureX509Cert

        if([string]::IsNullOrWhiteSpace($nfx509CertPath) -or -not (Test-Path $nfx509CertPath)){
            Write-Host "Don't know where to find the NoFuture x509 cert at.  Set the shared var at SecurityKeys NoFutureX509Cert"
            break
        }

        $nfx509CertPath = (Resolve-Path $nfx509CertPath).Path
           
        [NoFuture.Encryption.NfX509]::EncryptFile($Path, $nfx509CertPath)
        $encFile = $Path + [NoFuture.Shared.Constants]::NF_CRYPTO_EXT
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

        $nfx509CertPath = [NoFuture.Shared.NfConfig+SecurityKeys]::NoFutureX509Cert

        if([string]::IsNullOrWhiteSpace($nfx509CertPath)){
            Write-Host "Don't know where to find the NoFuture x509 cert at.  Set the shared var at SecurityKeys NoFutureX509Cert"
            break
        }

        $nfx509CertPath = (Resolve-Path $nfx509CertPath).Path

        $nfx509CertDir = Split-Path $nfx509CertPath -Parent

        $nfx509CertPath =  Join-Path $nfx509CertDir (([System.IO.Path]::GetFileNameWithoutExtension($nfx509CertPath)) + ".pfx")

        if(-not (Test-Path $nfx509CertPath)){
            throw "A .pfx copy of the NoFuture X509 certificate is required " + 
                        "and is expected to be placed next to the SecurityKeys.NoFutureX509Cert"
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
