try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("md5sum",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-SHA256HashString",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-AesEncryptedValue",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-AesDecryptedValue",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-HMACSHA1HashString",$MyInvocation.MyCommand)
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
        
        $salt = [System.Text.Encoding]::UTF8.GetBytes([System.Convert]::FromBase64String([NoFuture.Shared.SecurityKeys]::AesIV))
        
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
        $privKey = [System.Convert]::FromBase64String([NoFuture.Shared.SecurityKeys]::HMACSHA1.Replace("-", "+").Replace("_", "/"))
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
        $aesM.Key = [System.Convert]::FromBase64String([NoFuture.Shared.SecurityKeys]::AesEncryptionKey)
        $aesM.IV = [System.Convert]::FromBase64String([NoFuture.Shared.SecurityKeys]::AesIV)
        
        
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
        $aesM.Key = [System.Convert]::FromBase64String([NoFuture.Shared.SecurityKeys]::AesEncryptionKey)
        $aesM.IV = [System.Convert]::FromBase64String([NoFuture.Shared.SecurityKeys]::AesIV)
        
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
    Invokes the cipher.exe for the given path.
    
    .DESCRIPTION
    Invoke the builtin Windows exe named 'cipher' for 
    the given path.  The key appears to be an X509 cert
    which is present in the store under Local User\Personal.
    
    .PARAMETER Path
    A fully qualified path to a single file or folder
    
    .EXAMPLE
    C:\PS>Protect-Files -Path 'C:\My Docs\MS Word Docs\'
        
    .OUTPUTS
    Null
#>
function Protect-Files
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
        
        $Path = (Resolve-Path $Path).Path

        #encrypt everything in the directory
        if([System.IO.Directory]::Exists($Path)){
            iex "& cipher /e `"$Path`""
        }
        elseif([System.IO.File]::Exists($Path)){
            iex "& cipher /e /a `"$Path`""
        }
    }
}

<#
    .SYNOPSIS
    Invokes the cipher.exe for the given path.
    
    .DESCRIPTION
    Invoke the builtin Windows exe named 'cipher' for 
    the given path.  The key appears to be an X509 cert
    which is present in the store under Local User\Personal.
    
    .PARAMETER Path
    A fully qualified path to a single file or folder
    
    .EXAMPLE
    C:\PS>Unprotect-Files -Path 'C:\My Docs\MS Word Docs\'
        
    .OUTPUTS
    Null
#>
function Unprotect-Files
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
        
        $Path = (Resolve-Path $Path).Path

        #encrypt everything in the directory
        if([System.IO.Directory]::Exists($Path)){
            iex "& cipher /d `"$Path`""
        }
        elseif([System.IO.File]::Exists($Path)){
            iex "& cipher /d /a `"$Path`""
        }
    }
}
