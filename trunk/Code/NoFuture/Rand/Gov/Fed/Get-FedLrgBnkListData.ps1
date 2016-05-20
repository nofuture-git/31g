﻿function Get-FedLrgBnkListData
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
        $randDataPath = (Join-Path ([NoFuture.CustomTools]::CodeBase) "Rand\Data\Source")
        
        $fedRelease = Request-File -Url ([NoFuture.Rand.Gov.Fed.LargeCommercialBanks]::RELEASE_URL)
        $cacheRelease = Join-Path $randDataPath "lrg_bnk_lst.txt"
        [System.IO.File]::WriteAllText($cacheRelease, $fedRelease)

    }
}