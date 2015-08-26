function PrettyPrint-Json($path){
    if(-not (Test-Path $path)){break;}

    $data = [System.IO.File]::ReadAllText($path)

    $jsonIn = ConvertFrom-Json -InputObject $data

    [System.IO.File]::WriteAllText($path, (ConvertTo-Json -InputObject $jsonIn -Depth 12))
}