. .\aero.ps1 #calling script is domiciled at mypshome so its in 'its' working dir

Set-ConsoleTransparent
Get-Process powershell | Set-TopMost

for(;;){
    $val = ("{0}" -f [System.Console]::In.ReadLine())
    #$val = $host.UI.ReadLine(); 
    #$val = Read-Host
    if($val -eq "")
    {
        #do nothing
    }
    elseif(@("exit","quit") -contains $val.Trim().ToLower())
    {
        break;
    }
    else
    { 
        Write-Host $val
    }
}