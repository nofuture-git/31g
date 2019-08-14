$myScriptLocation = Split-Path $PSCommandPath -Parent

$WordTranslations = @{
    Hebrew = @{
        FileName = "HebrewWords.txt";
        Content = @()
    }

    Greek = @{
        FileName = "GreekWords.txt";
        Content = @()
    }

    Latin = @{
        FileName = "LatinWords.txt";
        Content = @()
    }
}

function Load-WordTranslations
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
        :nextLang foreach($lang in $WordTranslations.Keys){
     
             $fn = $WordTranslations[$lang].FileName
             if([string]::IsNullOrWhiteSpace($fn)){
                continue nextLang;
             }

             $ffn = Join-Path $myScriptLocation $fn
             if(-not (Test-Path $ffn)){
                continue nextLang;
             }

             $WordTranslations[$lang].Content = (Import-Csv -Path $ffn | Sort-Object -Property Words)
        }
    }
}

Load-WordTranslations

<#
    .SYNOPSIS
    Returns Hebrew, Greek and Latin words by English translation
    
    .DESCRIPTION
    Returns Hebrew, Greek and Latin words by English translation.
    Based on the words learned from the Memrise app whose translations
    are supposed to be committed to memory by now...

    .PARAMETER EnglishWord
    The English word whose translation is sought
#>
function Get-WordTranslations
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $EnglishWord,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $WholeWord

    )
    Process
    {
        $results = @()

        if($WordTranslations -eq $null){
            throw "No CSV data has been loaded."
            break;
        }

        :nextLang foreach($lang in $WordTranslations.Keys){
            if([string]::IsNullOrWhiteSpace($lang)){
                continue nextLang;
            }

            :nextLine foreach($line in $WordTranslations[$lang].Content){
                if([string]::IsNullOrWhiteSpace($line.Translations)){
                    continue nextLine;
                }

                $lowerTranslations = $line.Translations.ToLower();
                if($lowerTranslations.Contains($EnglishWord.ToLower())){
                    $results += $line
                }
            }
        }

        return $results
    }
}