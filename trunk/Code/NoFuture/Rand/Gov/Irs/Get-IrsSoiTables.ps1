function GetCsv-SoiNaicsSector
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
        $irsUri = [NoFuture.Rand.Gov.Irs.SoiNaicsSector]::IrsFile
        $irsUriXlsFile = Request-File -Url $irsUri.ToString()
        $myHeaders = @("Industrial sector and item","1999","2000","2001","2002","2003","2004","2005","2006","2007","2008","2009","2010","2011")
        $irsCsvData = Import-ExcelWs $irsUriXlsFile -Headers $myHeaders
        return New-Object NoFuture.Rand.Gov.Irs.SoiNaicsSector($irsCsvData)
    }
}

