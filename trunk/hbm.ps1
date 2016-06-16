try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-HbmDbData",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-AllHbmXml",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-EfFluentCs",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Write-CsCodeHbmCommand",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-AllHbmCs",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-HbmCs",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Invoke-HbmCsCompile",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("New-HbmAppConfig",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Import-HbmConfiguration",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Import-HbmSessionFactory",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-HbmSession",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
#------------------
# Globals and script level variables
#------------------
$script:numOfSteps = 2
$Global:SpResultSetXsdProgress
if($Global:invokeMgrEvent -ne $null){
    Unregister-Event -SubscriptionId $Global:invokeMgrEvent.Id
}

$Global:invokeMgrEvent = Register-ObjectEvent -InputObject ([NoFuture.Hbm.Mapping]::StoredProcManager) -EventName "CommLinkToPs" -Action {
    $mgrMsg = $event.SourceArgs
    $Global:SpResultSetXsdProgress = ("[{0}] {1} Percent Complete {2}" -f $mgrMsg.Status, $mgrMsg.Activity, $mgrMsg.ProgressCounter)
}

<#
    .SYNOPSIS
    Gathers all database metadata needed and sorts it.
   
    .DESCRIPTION
    Having called the Mssql-Settings cmdlet and set the current 
    database connection string to a valid target.  Running this 
    cmdlet will produce an output of eleven JSON unformatted files
    to a directory at NoFuture.Hbm.Settings.HbmDirectory.

    The sorted JSON files, versus those just dumped from the DB, 
    have the file extension pattern of '.hbm.json'.

    The cmdlet does not return the contents of the Json files which
    it produces.
    
    .PARAMETER IncludeStoredProx
    NOTE: to produce the returned dataset as an .xsd the
          each stored proc is invoked with default values.
    Apply this switch if you want the subsequent cmdlets to 
    produce hbm.xml\.cs files for stored prox.
    
    .OUTPUTS
    null
#>
function Get-HbmDbData
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [switch] $IncludeStoredProx
    )
    Process
    {
        if($IncludeStoredProx){
            $script:numOfSteps = 3
        }
        else{
            $script:numOfSteps = 2
        }
        Get-HbmMetadataDump $numOfSteps

        $sleepTimerMs = [NoFuture.Shared.NfConfig]::ThreadSleepTime
        [System.Threading.Thread]::Sleep($sleepTimerMs)

        $dnd = [NoFuture.Hbm.Settings]::AddNoPkAllNonNullableToBlockedNameList()

        $myWritePrgStatus = "Metadata Dump: Sorting Metadata [Step 2 of $numOfSteps]"

        Write-Progress -Activity "Getting Hbm Db Pk Data" -Status $myWritePrgStatus -PercentComplete 33
        $hbmDbPks = [NoFuture.Hbm.Sorting]::GetHbmDbPkData()

        [System.Threading.Thread]::Sleep($sleepTimerMs)

        Write-Progress -Activity "Getting Hbm Db Fk Data" -Status $myWritePrgStatus -PercentComplete 66
        $hbmDbFks = [NoFuture.Hbm.Sorting]::GetHbmDbFkData($hbmDbPks)

        [System.Threading.Thread]::Sleep($sleepTimerMs)

        Write-Progress -Activity "Getting Hbm Db Subsequent Data" -Status $myWritePrgStatus -PercentComplete 99
        $hbmDbBags = [NoFuture.Hbm.Sorting]::GetHbmDbSubsequentData()

        [NoFuture.Hbm.Mapping]::HbmKeys = $hbmDbPks
        [NoFuture.Hbm.Mapping]::HbmOneToMany = $hbmDbFks
        [NoFuture.Hbm.Mapping]::HbmBags = $hbmDbBags

        if($IncludeStoredProx){
            $ts = [NoFuture.Hbm.Mapping]::StoredProcManager.CalcExpectedProcessingTime()
            Write-Host ("Getting Stored Prox returned Datasets is expected to take '{0}' hours to complete." -f $ts.TotalHours) -ForegroundColor Yellow

            #these are used for reporting
            [NoFuture.Hbm.Sorting]::TimedOutProx.Clear()
            [NoFuture.Hbm.Sorting]::MultiTableDsProx.Clear()
            [NoFuture.Hbm.Sorting]::KilledProx.Clear()
            [NoFuture.Hbm.Sorting]::BadSyntaxProx.Clear()
            [NoFuture.Hbm.Sorting]::UnknownErrorProx.Clear()
            [NoFuture.Hbm.Sorting]::NoDatasetReturnedProx.Clear()

            Write-Host "This runs in the background. At anytime, use the 'Write-SpResultSetXsdProgress' cmdlet so see its current state." -ForegroundColor Yellow
            [NoFuture.Hbm.Mapping]::StoredProcManager.BeginGetSpResultSetXsd($null, ([NoFuture.Shared.NfConfig]::SqlServerDotNetConnString))

        }
    }

}#end Get-HbmDbData

function Get-HbmMetadataDump(){
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();
        $allHbmItems = @(
            [NoFuture.Hbm.Sorting+DbContainers]::Fks, 
            [NoFuture.Hbm.Sorting+DbContainers]::Pks, 
            [NoFuture.Hbm.Sorting+DbContainers]::AllKeys, 
            [NoFuture.Hbm.Sorting+DbContainers]::AllIndex, 
            [NoFuture.Hbm.Sorting+DbContainers]::AllColumns, 
            [NoFuture.Hbm.Sorting+DbContainers]::AutoIncrement, 
            [NoFuture.Hbm.Sorting+DbContainers]::FlatData,
            [NoFuture.Hbm.Sorting+DbContainers]::UniqueClusteredIdxNotPks,
            [NoFuture.Hbm.Sorting+DbContainers]::StoredProcsAndParams)
        $counter = 0
        $allHbmItems | % {
            
            $pcount = ([NoFuture.Util.Etc]::CalcProgressCounter($counter, $allHbmItems.Count)) 
            Write-Progress -Activity ("Saving data to '{0}'" -f $_.OutputPath) -Status "Metadata Dump: Fetching Metadata [Step 1 of $script:numOfSteps]" -PercentComplete $pcount
            $doNotDisplay = (Get-SingleHbmMetadataDump $_)
            $counter += 1
        }
}

function Get-SingleHbmMetadataDump($HbmValues){
                
        $outputPath = $HbmValues.OutputPath

        $outRslt = @()

        $rslt = (ado $HbmValues.SelectStatement)

        ($rslt) | % {
            $adoRecord = $_ 
            
            $hashRecord = @{}

            $HbmValues.QueryKeysNames | % {
                $hashRecord.($_) = $adoRecord.$_
                
            }

            $outRslt += $hashRecord;
        }

        $outRsltJson = ConvertTo-Json -InputObject $outRslt -Depth 12
        [System.IO.File]::WriteAllText($outputPath,$outRsltJson)

        return $outRsltJson

}#end Get-SingleHbmMetadataDump

function Write-SpResultSetXsdProgress(){
    Write-Host $Global:SpResultSetXsdProgress -ForegroundColor Cyan
}

#------------------
# hbm.xml generation cmdlet
#------------------

<#
    .SYNOPSIS
    Given the output of the Get-HbmData cmdlet, this cmdlet will generate hbm.xml files thereof.
   
    .DESCRIPTION
    The cmdlet will take the sorted results, in the form of JSON files, and use them 
    to produce an hbm.xml file for each table.  There will be one hbm.xml file
    per table in the same folder as the JSON files themselves.

    This cmdlet does not produce the rich variety available in 
    handwritten hbm files and produces property names which will 
    probably need refactoring.
    
    .PARAMETER OutputNamespace
    This forms both the namespace and assembly attributes of each
    resulting hbm.xml's top-level-node.

    .PARAMETER IncludeNoPkTables
    The default is to only produce hbm.xml files for tables which
    have a primary-key.  Apply this switch if the cmdlet should 
    produce a hbm.xml for tables without a primary-key in addition.
    
    .PARAMETER IncludeStoredProx
    Produces hbm.xml sql-query files for each stored proc.
    SEE Get-HbmDbData for more info.

    .OUTPUTS
    null
#>
function Get-AllHbmXml
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $OutputNamespace,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $IncludeNoPkTables,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $IncludeStoredProx

    )
    Process
    {
        #get current paths and array of tables 
        Write-Progress -Status "Starting..." -Activity "Loading current connection"
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();

        #delete all existing hbm xml
        Write-Progress -Status "removing all existing xml..." -Activity ("Delete {0} *.hbm.xml" -f (([NoFuture.Hbm.Settings]::HbmDirectory))) -PercentComplete 0
        ls -Path ([NoFuture.Hbm.Settings]::HbmDirectory) -Recurse | ? {$_.Name.EndsWith(".hbm.xml")} | % {
            Remove-Item -Path $_.FullName -Force
        }

        $counter = 0
        $allTables = [NoFuture.Hbm.Sorting]::AllTablesWithPkNames

        $dnd = [NoFuture.Hbm.Settings]::AddNoPkAllNonNullableToBlockedNameList()

        if(-not $IncludeNoPkTables){
            $dnd = [NoFuture.Hbm.Settings]::AddToBlockedNameList(([NoFuture.Hbm.Sorting]::NoPkTablesNames))
        }

        #per table, generate hbm.xml
        $allTables | % {
            Write-Progress -Status "drafting xml..." -Activity "Creating xml [has PK] for $_" -PercentComplete ([NoFuture.Util.Etc]::CalcProgressCounter($counter, $allTables.Count))

            $dnd = [NoFuture.Hbm.Mapping]::GetSingleHbmXml($OutputNamespace, $_)

            $counter += 1
        }#end foreach table

        #if caller wants tables having no PK then generate whatever output which may be had
        if($IncludeNoPkTables){
            $counter = 0

            #filter the two getting the difference
            $missingTables = [NoFuture.Hbm.Sorting]::NoPkTablesNames

            #gen the hbm.xml for each within the difference
            $missingTables | % {
                Write-Progress -Status "drafting xml..." -Activity "Creating xml [PK missing] for $_" -PercentComplete ([NoFuture.Util.Etc]::CalcProgressCounter($counter, $missingTables.Count)) 
                $dnd = [NoFuture.Hbm.Mapping]::GetSingleHbmXml($OutputNamespace, $_)
                $counter += 1
            }
            
        }#end Include No PK Tables

        #if caller wants stored prox as sql-query hbm.xmls and there actually are stored prox
        if($IncludeStoredProx -and [NoFuture.Hbm.Sorting]::AllStoredProcNames.Count -gt 0){
            if([NoFuture.Hbm.Sorting]::TimedOutProx.Count -gt 0){
                Write-Host "There are stored prox which timed out (see NoFuture.Hbm.Sorting.TimedOutProx)" -ForegroundColor Yellow
            }
            if([NoFuture.Hbm.Sorting]::MultiTableDsProx.Count -gt 0){
                Write-Host "There are stored prox which return multi-table datasets (see NoFuture.Hbm.Sorting.MultiTableDsProx)" -ForegroundColor Yellow
            }
            if([NoFuture.Hbm.Sorting]::KilledProx.Count -gt 0){
                Write-Host "There are stored prox which had to be forcibly shutdown (see NoFuture.Hbm.Sorting.KilledProx)" -ForegroundColor Yellow
            }
            if([NoFuture.Hbm.Sorting]::BadSyntaxProx.Count -gt 0){
                Write-Host "There are stored prox which have malformed syntax (see NoFuture.Hbm.Sorting.BadSyntaxProx)" -ForegroundColor Yellow
            }
            if([NoFuture.Hbm.Sorting]::UnknownErrorProx.Count -gt 0){
                Write-Host "There some stored prox which errored for some unknown reason (see NoFuture.Hbm.Sorting.UnknownErrorProx) and the log." -ForegroundColor Yellow
            }

            $myDirInfo = New-Object System.IO.DirectoryInfo(([NoFuture.Hbm.Settings]::HbmStoredProcsDirectory))
            $xsdFiles = $myDirInfo.GetFileSystemInfos("*.xsd",[System.IO.SearchOption]::TopDirectoryOnly)
            $counter = 0

            $xsdFiles | % {
               
               $searchName = [System.IO.Path]::GetFileNameWithoutExtension($_.Name)
               Write-Progress -Status "drafting xml..." -Activity "Creating xml [stored prox] for $searchName" -PercentComplete ([NoFuture.Util.Etc]::CalcProgressCounter($counter,$xsdFiles.Count)) 
               $dnd = [NoFuture.Hbm.Mapping]::GetHbmNamedQueryXml($OutputNamespace, $searchName)
               $counter += 1
            }


        }#end Include stored prox

    }#end Process
}#end Get-AllHbmXml

<#
    .SYNOPSIS
    Given the output of the Get-HbmData cmdlet, this cmdlet 
    will generate a single hbm.xml file.
   
    .DESCRIPTION
    Creates a single hbm.xml file when a match on a db object is found.
    The db object may be a table or a proc.
    
    .PARAMETER DbObjectName
    The table or prox name.

    .PARAMETER OutputNamespace
    This forms both the namespace and assembly attributes of 
    the resulting hbm.xml's top-level-node.
    
    .OUTPUTS
    NoFuture.Hbm.SortingContainers.HbmFileContent
#>
function Get-SingleHbmXml
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $DbObjectName,
        [Parameter(Mandatory=$true,position=1)]
        [string] $OutputNamespace
    )
    Process
    {
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();
        if([NoFuture.Hbm.Sorting]::AllTablesWithPkNames -notcontains $DbObjectName -and 
           [NoFuture.Hbm.Sorting]::NoPkTablesNames -notcontains $DbObjectName -and 
           [NoFuture.Hbm.Sorting]::AllStoredProcNames -notcontains $DbObjectName){

            Write-Host "No name match found for '$DbObjectName'." -ForegroundColor Yellow
            break;
        }

        if([NoFuture.Hbm.Sorting]::AllStoredProcNames -contains $DbObjectName){
            $searchCrit = New-Object NoFuture.Hbm.StoredProxSearchCriteria -Property @{ExactName = $DbObjectName}
            Write-Progress -Activity "Invoking stored proc $DbObjectName" -Status "Working"
            [NoFuture.Hbm.Mapping]::StoredProcManager.GetSpResultSetXsd($searchCrit, ([NoFuture.Shared.NfConfig]::SqlServerDotNetConnString))
            $hbmXml = [NoFuture.Hbm.Mapping]::GetHbmNamedQueryXml($OutputNamespace, $DbObjectName)
            return New-Object NoFuture.Hbm.SortingContainers.HbmFileContent($hbmXml)
        }
        else{
            $hbmXml = [NoFuture.Hbm.Mapping]::GetSingleHbmXml($OutputNamespace, $DbObjectName)
            return New-Object NoFuture.Hbm.SortingContainers.HbmFileContent($hbmXml)
        }
    }
}
#------------------
# .cs generation cmdlet
#------------------

<#
    .SYNOPSIS
     Given the directory of hbm.xml files, this cmdlet will produce the counterpart .cs files.
   
    .DESCRIPTION
    This cmdlet simply creates a counterpart .cs file for every hbm.xml file
    present in the current settings directory.  The cmdlet only deals in 
    hbm nodes id, composite-id, property, many-to-one and bags.

    All bags are made int System.Collections.Generic.IList types.
    
    .PARAMETER IncludeNoPkTables
    The default is to only produce .cs files for hbm.xml whose underlying
    table has a primary-key.  Apply this switch if the cmdlet should 
    produce a .cs for hbm.xml's without a primary-key in addition.
    
    .PARAMETER IncludeStoredProx
    This will induce the cmdlet to generate sql-query hbm.xml's for all
    the applicable stored prox 

    .OUTPUTS
    null
#>
function Get-AllHbmCs
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [switch] $IncludeNoPkTables,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $IncludeStoredProx
    )
    Process
    {
        #get current settings and paths
        Write-Progress -Status "Starting..." -Activity "Loading current connection"
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();

        #delete all existing C# files
        Write-Progress -Status "removing all C# files..." -Activity ("Delete {0} *.cs" -f (([NoFuture.Hbm.Settings]::HbmDirectory))) -PercentComplete 0
        ls -Path ([NoFuture.Hbm.Settings]::HbmDirectory) -Recurse | ? {$_.Extension -eq ".cs"} | % {
            Remove-Item -Path $_.FullName -Force
        }
        $counter = 0

        #get a list of all hbm.xml files fullnames 
        $allhbmXml = (ls -Path ([NoFuture.Hbm.Settings]::HbmDirectory) | ? {$_.Name.EndsWith(".hbm.xml")} | % {$_.FullName})

        Write-Progress -Status "drafting cs..." -Activity "Generating a Composite Key Classes"
        #go through entire list and get all composite-ids out of the way first
        $allhbmXml | % {
           $progCount = ([NoFuture.Util.Etc]::CalcProgressCounter($counter,$allhbmXml.Count))
           Write-Progress -Status "drafting cs..." -Activity "Creating C# code-file [Has PK] for $_" -PercentComplete $progCount
           Get-HbmCs -HbmXmlPath $_;

           $counter += 1

        }#end foreach hbm.xml

        #if caller wants tables having no PK then generate whatever output which may be had
        if($IncludeNoPkTables){
            $counter = 0

            #filter the two getting the difference
            $missingTables = [NoFuture.Hbm.Sorting]::NoPkTablesNames

            #gen the hbm.xml for each within the difference
            $missingTables | ? {-not [string]::IsNullOrWhiteSpace($_) -and (Test-Path (Join-Path ([NoFuture.Hbm.Settings]::HbmDirectory) ("{0}.hbm.xml" -f $_)))} | % {
                $progCount = ([NoFuture.Util.Etc]::CalcProgressCounter($counter,$missingTables.Count))
                Write-Progress -Status "drafting cs..." -Activity "Creating C# code-file [PK missing] for $_" -PercentComplete $progCount
                Get-HbmCs -HbmXmlPath (Join-Path ([NoFuture.Hbm.Settings]::HbmDirectory) ("{0}.hbm.xml" -f $_))
                $counter += 1
            }#end foreach missing PK tabl
            
        }#end Include No PK Tables

        #if caller wants stored prox
        if($IncludeStoredProx -and [NoFuture.Hbm.Sorting]::AllStoredProcNames.Count -gt 0){
            $myDirInfo = New-Object System.IO.DirectoryInfo(([NoFuture.Hbm.Settings]::HbmStoredProcsDirectory))
            $hbmXmlFiles = $myDirInfo.GetFileSystemInfos("*.hbm.xml",[System.IO.SearchOption]::TopDirectoryOnly)
            $counter = 0

            $hbmXmlFiles | % {
               $searchName = [System.IO.Path]::GetFileNameWithoutExtension($_.Name)
               $progcount = ([NoFuture.Util.Etc]::CalcProgressCounter($counter, $hbmXmlFiles.Count))
               Write-Progress -Status "drafting cs..." -Activity "Creating C# code-file [Stored Proc] for $searchName" -PercentComplete $progcount
               Get-HbmCs -HbmXmlPath ($_.FullName)
               $counter += 1
            }
            
        }#end stored prox

    }#end Process
}#end Get-AllHbmCs

<#
    .SYNOPSIS
    This cmdlet may be used to generate a .cs file from an hbm.xml file
   
    .DESCRIPTION
    While most of the cmdlets herein are monolithic and intended as a kind
    of batch process.  This cmdlet may be used independently needing only
    a valid hbm.xml file.

    .PARAMETER HbmXmlPath
    The hbm.xml file to be used to generate the its counterpart C# POCO
    class file.

    .PARAMETER OutputDir
    Optional, will default to NoFuture.Hbm.Settings.HbmDirectory
    
    .OUTPUTS
    null
#>
function Get-HbmCs
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $HbmXmlPath,
        [Parameter(Mandatory=$false,position=1)]
        [string] $OutputDir
    )
    Process
    {
        if(-not(Test-Path $HbmXmlPath)){
            Write-Host "no such file at '$HbmXmlPath'" -ForegroundColor Yellow
            break;
        }
        if([string]::IsNullOrWhiteSpace($OutputDir) -or -not (Test-Path $OutputDir)){
            $OutputDir = [NoFuture.Hbm.Settings]::HbmDirectory
        }

        $pocoTemplate = Join-Path ([NoFuture.BinDirectories]::T4Templates) "HbmCsClass.tt"

        if(-not (Test-Path $pocoTemplate)){
            Write-Host "couldn't find the T4 template at '$pocoTemplate'" -ForegroundColor Yellow
            break;
        }

        $fileName = ([System.IO.Path]::GetFileNameWithoutExtension($HbmXmlPath)).Replace(".hbm",[string]::Empty)

        $pocoCsFile = Join-Path $OutputDir ("{0}.cs" -f $fileName) 

        $t4ParamName = [NoFuture.Hbm.SortingContainers.HbmFileContent]::T4_PARAM_NAME

        $hbmXmlContent = New-Object NoFuture.Hbm.SortingContainers.HbmFileContent($hbmXmlPath)

        if($hbmXmlContent.IsCompositeKey){
            $compKeyTemplate = Join-Path ([NoFuture.BinDirectories]::T4Templates) "HbmCompKeyCsClass.tt"
            if(-not (Test-Path $compKeyTemplate)){
                Write-Host "couldn't find the T4 template at '$compKeyTemplate'" -ForegroundColor Yellow
                break;
            }
            $compKeyCsFile = Join-Path $OutputDir ("{0}{1}.cs" -f $fileName, ([NoFuture.Hbm.Globals]::COMP_KEY_ID_SUFFIX))
            Get-T4TextTemplate -InputFile $compKeyTemplate -OutputFile $compKeyCsFile -ParamNameValues @{$t4ParamName=$HbmXmlPath}
        }

        Get-T4TextTemplate -InputFile $pocoTemplate -OutputFile $pocoCsFile -ParamNameValues @{$t4ParamName=$HbmXmlPath}
    }
}#end Get-HbmCs

<#
    .SYNOPSIS
    This cmdlet may be used to generate an EF 6.x .cs file from an hbm.xml file
   
    .DESCRIPTION
    Given the hbm.xml file this cmdlet will create and save to disk 
    a file for both the EF 6.x POCO and its corresponding EF Fluent mapping.

    .PARAMETER HbmXmlPath
    The hbm.xml file to be used to generate both the C# POCO and the EF Mapping

    .PARAMETER OutputDir
    The directory where the generated POCO and EF Mapping code files
    will be deposited.
    
    .OUTPUTS
    null
#>
function Get-EfFluentCs
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $HbmXmlPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $OutputDir
    )
    Process
    {
        if(-not(Test-Path $HbmXmlPath)){
            Write-Host "no such file at '$HbmXmlPath'" -ForegroundColor Yellow
            break;
        }
        
        if(-not(Test-Path $OutputDir)){
            Write-Host "no such directory '$OutputDir'" -ForegroundColor Yellow
            break;
        }

        $mappingTemplate = Join-Path ([NoFuture.BinDirectories]::T4Templates) "Ef6xFluentMapping.tt"
        $pocoTemplate = Join-Path ([NoFuture.BinDirectories]::T4Templates) "Ef6xPoco.tt"
        if(-not (Test-Path $mappingTemplate)){
            Write-Host "couldn't find the T4 template at '$mappingTemplate'" -ForegroundColor Yellow
            break;
        }

        if(-not (Test-Path $pocoTemplate)){
            Write-Host "couldn't find the T4 template at '$pocoTemplate'" -ForegroundColor Yellow
            break;
        }

        $fileName = ([System.IO.Path]::GetFileNameWithoutExtension($HbmXmlPath)).Replace(".hbm",[string]::Empty)

        $mappingCsFile = Join-Path $OutputDir ("{0}Mapping.cs" -f $fileName)
        $pocoCsFile = Join-Path $OutputDir ("{0}.cs" -f $fileName) 

        $t4ParamName = [NoFuture.Hbm.SortingContainers.HbmFileContent]::T4_PARAM_NAME

        Get-T4TextTemplate -InputFile $pocoTemplate -OutputFile $pocoCsFile -ParamNameValues @{$t4ParamName=$HbmXmlPath}
        Get-T4TextTemplate -InputFile $mappingTemplate -OutputFile $mappingCsFile -ParamNameValues @{$t4ParamName=$HbmXmlPath}
        
    }
}#end Get-EfFluentCs

<#
    .SYNOPSIS
    This cmdlet may be used to generate an EF 3.5 .cs file from an hbm.xml file
   
    .DESCRIPTION
    EF 3.5 is the original and oldest Entity Framework.

    .PARAMETER HbmXmlPath
    The hbm.xml file to be used to generate a EF 3.5 .cs entity.

    .PARAMETER OutputDir
    The directory where the generated code file will be deposited.
    
    .OUTPUTS
    null
#>
function Get-Ef35Cs
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $HbmXmlPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $OutputDir
    )
    Process
    {
        if(-not(Test-Path $HbmXmlPath)){
            Write-Host "no such file at '$HbmXmlPath'" -ForegroundColor Yellow
            break;
        }
        
        if(-not(Test-Path $OutputDir)){
            Write-Host "no such directory '$OutputDir'" -ForegroundColor Yellow
            break;
        }

        $mappingTemplate = Join-Path ([NoFuture.BinDirectories]::T4Templates) "Ef35Mapping.tt"
        if(-not (Test-Path $mappingTemplate)){
            Write-Host "couldn't find the T4 template at '$mappingTemplate'" -ForegroundColor Yellow
            break;
        }

        $fileName = ([System.IO.Path]::GetFileNameWithoutExtension($HbmXmlPath)).Replace(".hbm",[string]::Empty)

        $mappingCsFile = Join-Path $OutputDir ("{0}.cs" -f $fileName)

        $t4ParamName = [NoFuture.Hbm.SortingContainers.HbmFileContent]::T4_PARAM_NAME

        Get-T4TextTemplate -InputFile $mappingTemplate -OutputFile $mappingCsFile -ParamNameValues @{$t4ParamName=$HbmXmlPath}
        
    }
}#end Get-Ef35Cs

<#
    .SYNOPSIS
    Constructs a code file which extends specific types
    in NoFuture.Hbm.Command
   
    .DESCRIPTION
    Since code gen in PowerShell is much easier - this
    cmdlet is here to draft extensions of 
    NoFuture.Hbm.Command.ReceiverBase, GetCommandBase,
    SetCommandBase and RemoveCommandBase types.  The 
    intent is to apply some kind of wrapper over the POCO
    types created by the hbm cmdlets to be used in some
    web app and therefore every type is expected to have
    a property named "Id"

    .PARAMETER Assembly
    An assembly containing this TypeName and having 
    in which said type as a property named Id

    .PARAMETER TypeName
    A fully qualified type name which is resolvable from 
    the given Assembly

    .PARAMETER OutputNamespace
    The generated code's namespace.

    .OUTPUTS
    string, actual generated C# source code.
#>
function Write-CsCodeHbmCommand
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.Reflection.Assembly] $Assembly,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeName,
        [Parameter(Mandatory=$true,position=2)]
        [string] $OutputDir
    )
    Process
    {
        $className = [NoFuture.Util.NfTypeName]::GetTypeNameWithoutNamespace($TypeName)
        $idPropName = [NoFuture.Hbm.Globals+HbmXmlNames]::ID
        $typeNameType = $Assembly.GetType($TypeName)
        if($typeNameType -eq $null){
            throw "The type '$TypeName' was not found in this assembly"
            break;
        }
        $idProp = $typeNameType.GetProperties() | ? {$_.Name -eq $idPropName} | Select-Object -First 1
        if($idProp -eq $null){
            throw "The type '$TypeName' does not contain a property named '$idPropName'"
            break;
        }

        $t4Template = Join-Path ([NoFuture.BinDirectories]::T4Templates) "HbmCommand.tt"
        $outputCsFile = Join-Path $OutputDir ("{0}Command.cs" -f $className)
        $idType = $idProp.PropertyType.FullName

        $param1 = [NoFuture.Hbm.Templates.HbmCommand+ParamNames]::TypeFullName
        $param2 = [NoFuture.Hbm.Templates.HbmCommand+ParamNames]::IdTypeFullName

        Get-T4TextTemplate -InputFile $t4Template -OutputFile $outputCsFile -ParamNameValues @{$param1=$TypeName; $param2=$idType}

    }
}# end Write-CsCodeHbmCommand

#------------------
# hbm configuration cmdlets
#------------------

<#
    .SYNOPSIS
    Calls the .NET C# compiler against all .cs files in current settings directory.
   
    .DESCRIPTION
    Calls the .NET C# compiler against all .cs files in current settings directory.
    No form of validation or what-not is performed and any malformations will 
    be reported back from the compiler to the standard output.
    
    .PARAMETER OutputDllName
    The binary dll's file name.
    
    .OUTPUTS
    null
#>
function Invoke-HbmCsCompile
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $OutputNamespace,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $WhatIf

    )
    Process
    {
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();
        
        #delete all existing binaries
        Write-Progress -Status "removing compiled dll files..." -Activity ("Delete {0} *.dll" -f (([NoFuture.Hbm.Settings]::HbmDirectory))) -PercentComplete 0
        ls -Path ([NoFuture.Hbm.Settings]::HbmDirectory) -Recurse | ? {$_.Extension -eq ".dll"} | % {
            if($WhatIf){
                Remove-Item -Path $_.FullName -WhatIf
            }
            else{
                Remove-Item -Path $_.FullName -Force
            }
        }

        #compile all POCO c# 
        $rootBin = ([NoFuture.BinDirectories]::Root)
        $iesiCollections = ("/reference:{0}" -f (Join-Path $rootBin "Iesi.Collections.dll"))
        $nhibDll = ("/reference:{0}" -f (Join-Path $rootBin "NHibernate.dll"))
        $lhbmDirectory = ([NoFuture.Hbm.Settings]::HbmDirectory)
        $hbmsidDll = (Join-Path $rootBin "NoFuture.Hbm.Sid.dll")
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();
        $cscCompiler = (Join-Path $global:net40Path $global:cscExe)
        $OutputNamespace = [NoFuture.Util.Etc]::CapitalizeFirstLetterOfWholeWords($OutputNamespace,'.')
        $dllOutputFile = (Join-Path $lhbmDirectory ([NoFuture.Util.NfTypeName]::DraftCscDllName($OutputNamespace)))

        $targetArg = "/t:library"
        $outArg = "/out:$dllOutputFile"
        $recurseArg = "/recurse:$lhbmDirectory\*.cs"
        $nowarnArg = "/warn:0"
        $hbmSid = ("/reference:{0}" -f $hbmsidDll)
        $ref = "$iesiCollections $nhibDll $hbmSid" 

        $iexCmd = (". $cscCompiler $outArg $targetArg $nowarnArg $ref /nologo $recurseArg")

        Write-Progress -Activity "'$dllOutputFile'" -Status "Compile .cs files"
        if($WhatIf){
            Write-Host $iexCmd -ForegroundColor Cyan
        }
        else{
            iex ($iexCmd)
        }
         

        #compile all hbm.xml in dll's with resources embedded.
        $middleSpaceName = ([NoFuture.Hbm.Globals]::HBM_XML_DLL_MIDDLE_NAME)
        $hbmXmlCompileDirs = @{
            $middleSpaceName = ([NoFuture.Hbm.Settings]::HbmDirectory);
            "$middleSpaceName.Prox" = ([NoFuture.Hbm.Settings]::HbmStoredProcsDirectory);
        }
        $progressCounter = 0

        $totalCount = 0

        $hbmXmlCompileDirs.Keys | % {
            $totalCount += (ls -Path ($hbmXmlCompileDirs[$_]) | ? {$_.Name.EndsWith(".hbm.xml")}).Count
        }

        $hbmXmlCompileDirs.Keys | % {
            $middleSpace = $_
            $hbmXmlCompileDir = $hbmXmlCompileDirs[$_]

            $kbCounter = 0
            $asmCounter = 0
            
            $resSwitch = New-Object System.Text.StringBuilder

            ls -Path $hbmXmlCompileDir | ? {$_.Name.EndsWith(".hbm.xml")} | % {
                $kbCounter += $_.Length;
                $dnd = $resSwitch.AppendFormat(" /res:{0}",$_.Name)
                #build it 
                if($kbCounter -gt (1KB * ([NoFuture.Hbm.Settings]::CompileHbmXmlDllOfKbSize))){
                    $outDllName = "{0}.{1}{2:000}.dll" -f $OutputNamespace,$middleSpace,$asmCounter
                    Write-Progress -Activity "'$outDllName'" -Status "Compile Hbm Xml" -PercentComplete ([NoFuture.Util.Etc]::CalcProgressCounter($progressCounter,$totalCount))
                    $outArg = "/out:$outDllName"
                    $iexCmd = (". $cscCompiler $outArg $targetArg $nowarnArg /nologo $resSwitch")

                    if($WhatIf){
                        Write-Host $iexCmd -ForegroundColor Cyan
                    }
                    else{
                        Push-Location $hbmXmlCompileDir
                        iex ($iexCmd)
                        Pop-Location
                    }

                    $dnd = $resSwitch.Clear()
                    $asmCounter += 1
                    $kbCounter = 0
                }
                $progressCounter += 1
            }# end foreach hbm.xml file

            #compile the tail end
            if($kbCounter -gt 0){
                $outDllName = "{0}.{1}{2:000}.dll" -f $OutputNamespace,$middleSpace,$asmCounter
                Write-Progress -Activity "'$outDllName'" -Status "Compile Hbm Xml" -PercentComplete ([NoFuture.Util.Etc]::CalcProgressCounter($progressCounter,$totalCount))
                $outArg = "/out:$outDllName"
                $iexCmd = (". $cscCompiler $outArg $targetArg $nowarnArg /nologo $resSwitch")

                if($WhatIf){
                    Write-Host $iexCmd -ForegroundColor Cyan
                }
                else{
                    Push-Location $hbmXmlCompileDir
                    iex ($iexCmd)
                    Pop-Location
                }
            }

        } #end hbm xml dir compile

    }#end Process
}#end Invoke-HbmCsCompile

<#
    .SYNOPSIS
    Returns the path to an xml file having the hibernate-configuration node.

    .DESCRIPTION
    Makes an xml file whose contents are in the form of either an app.config 
    or the hibernate.cfg.xml based on the AsAppConfig switch.  
    The connection strings name is set as 'NoFutureConnectionString' and 
    the value is set to whatever the current one is at 
    NoFuture.Shared.Constants.SqlServerDotNetConnString.

    .PARAMETER OutputNamespace
    Used to define an assembly name within the hibernate-configuration section.

    .PARAMETER AsAppConfig
    Optional, instructs the cmdlet to produce an App.config file with 
    the hibernate-configuration section included.

    .PARAMETER Settings
    Optional and only pertinent to AsAppConfig being true - hashtable 
    whose key/value pairs will be added to the appSettings 
    node of the output.

#>
function New-HbmAppConfig
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $OutputNamespace,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $AsAppConfig,
        [Parameter(Mandatory=$false,position=2)]
        [hashtable] $Settings

    )
    Process
    {
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();

        $conStringName = "NoFutureConnectionString"
        $configuration = new-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("configuration"))
        $configSections = new-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("configSections"))

		$configSections.Add(([NoFuture.Hbm.XeFactory]::ConfigSection()));


        $appSettings = new-Object System.Xml.Linq.XElement([System.Xml.Linq.XName]::Get("appSettings"))
        if($Settings -ne $null){
            $Settings.Keys | % {
                $appSettings.Add((New-Object System.Xml.Linq.XElement("add",
                                                                      (New-Object System.Xml.Linq.XAttribute("key",$_)),
                                                                      (New-Object System.Xml.Linq.XAttribute("value",$Settings.$_))
                                                                      )));
            }
        }
        
		$hibernateconfiguration = [NoFuture.Hbm.XeFactory]::HibernateConfigurationNode(([NoFuture.Shared.NfConfig]::SqlServerDotNetConnString), 
																						$OutputNamespace)

        $configuration.Add($configSections)
        $configuration.Add($appSettings)
        $configuration.Add($hibernateconfiguration)

        if($AsAppConfig){
            $configContents = $configuration.ToString().Replace("<session-factory xmlns=`"`">","<session-factory>")
            $hbmConfigFilePath = (Join-Path ([NoFuture.Hbm.Settings]::HbmDirectory) "$OutputNamespace.config")
        }
        else{
            $configContents = $hibernateconfiguration.ToString().Replace("<session-factory xmlns=`"`">","<session-factory>")
            $hbmConfigFilePath = (Join-Path ([NoFuture.Hbm.Settings]::HbmDirectory) "hibernate.cfg.xml")
        }

        [System.IO.File]::WriteAllText($hbmConfigFilePath,$configContents)

        return $hbmConfigFilePath

    }
}#end New-HbmAppConfig

<#
    .SYNOPSIS
    Returns the Global:hbmCfg, instantiating it if it is null.

    .DESCRIPTION
    NHibernate.Cfg.Configuration is an expensive object and so
    this cmdlet will create it only if its not been yet instantiated.

    The hbmCfg is also configured with values produced from the
    New-HbmAppConfig cmdlet (which is called internally herein).

    A simple check is made on the appDomain to verify the NHibernate
    and Iesi.Collections binaries are loaded prior to any attempts 
    to create such objects.


#>
function Import-HbmConfiguration
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $HbmAppConfigPath,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $Force
    )
    Process
    {
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();

        #test if the given path arg is valid
        if(-not (Test-Path $HbmAppConfigPath)){throw ("The configuration file is not located at '{0}'" -f $HbmAppConfigPath)}

        #test needed dependencies are in current appdomain
        if(-not(([AppDomain]::CurrentDomain.GetAssemblies() | % {$_.GetName().Name}) -contains "Iesi.Collections"))
        {
            throw "Iesi.Collections, which NHibernate depends on, is not currently listed in this appdomain's assemblies"
        }

        if(-not(([AppDomain]::CurrentDomain.GetAssemblies() | % {$_.GetName().Name}) -contains "NHibernate"))
        {
            throw "NHibernate is not currently listed in this appdomain's assemblies"
        }

        #check if hbm configuration has already been instantiated and not being forced to perform again
        if(([NoFuture.Hbm.Globals]::HbmCfg -ne $null) -and (-not($Force))) {return [NoFuture.Hbm.Globals]::HbmCfg}

        $isHbmCfgXml = ([System.IO.Path]::GetFileName($HbmAppConfigPath) -eq "hibernate.cfg.xml")

        $hbmConfigXml = [xml](Get-Content -Path $HbmAppConfigPath)

        #make this contextual on the kind of configuration file we are dealing with
        if($isHbmCfgXml){
            $hibernateCfgNode = ($hbmConfigXml."hibernate-configuration")
        }
        else {
            $hibernateCfgNode = ($hbmConfigXml.configuration."hibernate-configuration")
        }

        $entityAssemblyFullName = ($hibernateCfgNode."session-factory"."mapping"."assembly")

        #check if the entity assembly is currently loaded, load it if not
        if(-not(([AppDomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}) -contains $entityAssemblyFullName))
        {
            $mappingAssemblySimpleName = $entityAssemblyFullName.Split(",")[0]
            $mappingAssemblyPath = (Join-Path ([NoFuture.Hbm.Settings]::HbmDirectory) ("{0}.dll" -f $mappingAssemblySimpleName))

            #if the entity assembly is not in the script's directory then abort
            if(-not (Test-Path $mappingAssemblyPath))
            {
                throw ("The assembly by the full-name of '{0}' is neither currently loaded in this appDomain nor is the file, '{1}' present at the expected location." -f $entityAssemblyFullName,$mappingAssemblyPath)
                break;
            }

            $donotDisplay = [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($mappingAssemblyPath))
        }

        #test for the dll used for binary ids is present
        if(-not(([AppDomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}) -contains "NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"))
        {
            throw ("The assembly used for Binary IDs (byte[]) named 'NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' was expected to already be loaded in the appDomain but it is missing.")
            break;
        }

        [NoFuture.Hbm.Globals]::HbmCfg = New-Object NHibernate.Cfg.Configuration
        if($isHbmCfgXml){
            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.Configure($HbmAppConfigPath)
        }
        else{
            #manually parse values from the app.config and assign them to the NHibernate.Cfg.Configuration
            $hbmConfigHash = @{}
            $hibernateCfgNode."session-factory"."property" | % {
                $hbmConfigHash.Add($_.name,$_.innerText)
            }

            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.SetProperty([NHibernate.Cfg.Environment]::ConnectionProvider, ($hbmConfigHash."connection.provider"))
            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.SetProperty([NHibernate.Cfg.Environment]::ConnectionDriver, ($hbmConfigHash."connection.driver_class"))
            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.SetProperty([NHibernate.Cfg.Environment]::Dialect, ($hbmConfigHash."dialect"))
            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.SetProperty([NHibernate.Cfg.Environment]::ConnectionString, ($hbmConfigHash."connection.connection_string"))
            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.SetProperty([NHibernate.Cfg.Environment]::Isolation, ($hbmConfigHash."connection.isolation"))
            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.SetProperty([NHibernate.Cfg.Environment]::CommandTimeout, ($hbmConfigHash."command_timeout"))
            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.SetProperty([NHibernate.Cfg.Environment]::MaxFetchDepth, ($hbmConfigHash."max_fetch_depth"))
            $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.AddAssembly($entityAssemblyFullName);
        }
        $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.AddAssembly("NoFuture.Hbm.Sid, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");

    }#end Process
}#end Import-HbmConfiguration

<#
    .SYNOPSIS
    Returns the NoFuture.Hbm.Globals.HbmSessionFactory, instantiating it if it is null.

    .DESCRIPTION
    NHibernate.ISessionFactory is an expensive object and so
    this cmdlet will create it only if its not been yet instantiated.

    The Global:hbmCfg should already have been instantiated prior
    to any calls to this cmdlet.  If it is not then an exception 
    is thrown.

#>
function Import-HbmSessionFactory
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [switch] $IncludeStoredProx,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $Force
    )
    Process
    {
        [NoFuture.Hbm.Settings]::LoadOutputPathCurrentSettings();

        if(([NoFuture.Hbm.Globals]::HbmSessionFactory -ne $null) -and (-not($Force)))
        {
            break;
        }

        if([NoFuture.Hbm.Globals]::HbmCfg -eq $null)
        {
            throw "Instantiate the 'NoFuture.Hbm.Globals.HbmCfg' object via a call to 'Import-HbmConfiguration' then call this cmdlet again."
        }

        #all matching hbm.xml files from Mssql-Settings location (compiled to binaries)
        $middleSpaceName = ([NoFuture.Hbm.Globals]::HBM_XML_DLL_MIDDLE_NAME)
        if($IncludeStoredProx){
            $allHbmFiles = (ls -Path ([NoFuture.Hbm.Settings]::HbmDirectory) -Recurse | ? {$_.Extension -eq ".dll" -and $_.Name -like "*$middleSpaceName*"} | % {$_.FullName})
        }
        else{
            $allHbmFiles = (ls -Path ([NoFuture.Hbm.Settings]::HbmDirectory) | ? {$_.Extension -eq ".dll" -and $_.Name -like "*$middleSpaceName*"} | % {$_.FullName})
        }

        $currentAppDomainAsm = [AppDomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}

        #load xml for any generated files
        $allHbmFiles | % {
            $hbmXml = $_
            if(Test-Path $hbmXml)
            {
                try{
                    $hbmXmlAsmFullName = [NoFuture.Util.NfTypeName]::DraftCscExeAsmName(([System.IO.Path]::GetFileNameWithoutExtension($hbmXml)))
                    if($currentAppDomainAsm -notcontains $hbmXmlAsmFullName){
                       $hbmXmlAsm = [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($hbmXml))
                    }
                    else{
                        $hbmXmlAsm = ([AppDomain]::CurrentDomain.GetAssemblies() | ? {$_.FullName -eq $hbmXmlAsmFullName} | Select-Object -First 1)
                    }
                    $donotDisplay = [NoFuture.Hbm.Globals]::HbmCfg.AddAssembly($hbmXmlAsm)
                }
                catch [System.Exception]
                {
                    Write-Host "error at hbm '$hbmXml'"
                    Write-Host $Error[0].Exception.Message
                    [NoFuture.Hbm.Globals]::UnloadHbmDb()
                    break; 
                }
            }
            
        }#end tableNames

        try{
            #[NoFuture.Hbm.Globals]::HbmSessionFactory = [NoFuture.Hbm.Globals]::HbmCfg.BuildSessionFactory();
            $noop = $true
        }
        catch [System.Exception]
        {
            Write-Host $Error[0].Exception.Message
            [NoFuture.Hbm.Globals]::UnloadHbmDb()
            break; 
        }

    }
}#end Import-HbmSessionFactory

<#
    .SYNOPSIS
    Returns a new instance of NHibernate.ISession object.

    .DESCRIPTION
    NHibernate.ISession is cheap to instantiate and the main means of performing
    transactions on the database using NHibernate.

    The caller must have instantiated the Global variables hbmCfg and 
    hbmSessionFactory prior to calling this cmdlet.  Having either as 
    null will cause an exception to be thrown.  Note, however, this 
    cmdlet only checks for null, not the underlying type.
    
    .EXAMPLE 
    TODO

#>
function Get-HbmSession
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
        if([NoFuture.Hbm.Globals]::HbmSessionFactory -eq $null -or [NoFuture.Hbm.Globals]::HbmCfg -eq $null)
        {
           throw "load the Global hbm Configuation and Session Factory first by calling Import-HbmConfiguration and Import-HbmSessionFactory respectively"
        }

        return [NoFuture.Hbm.Globals]::HbmSessionFactory.OpenSession()

    }#end Process
}#end Get-HbmSession


#------------------
# shared
#------------------

function Get-HbmDb
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $OutputNamespace,

        [Parameter(Mandatory=$false,position=1)]
        [switch] $IncludeNoPkTables,

        [Parameter(Mandatory=$false,position=3)]
        [switch] $Force

    )
    Process
    {
        
        if([System.String]::IsNullOrWhiteSpace([NoFuture.Shared.NfConfig]::SqlServer)){Write-Host "set the MSSQL server global variables."; break;}
        if([System.String]::IsNullOrWhiteSpace([NoFuture.Shared.NfConfig]::SqlCatalog)){Write-Host "set the MSSQL server global variables."; break;}

        $mappingAssemblyPath = (Join-Path ([NoFuture.Hbm.Settings]::HbmDirectory) $OutputDllName)
        if(-not (Test-Path $mappingAssemblyPath) -or $Force){

            $sleepTimerMs = 100
            Write-Host "getting Hbm Db Preliminary Data"
            Get-HbmDbData

            [System.Threading.Thread]::Sleep($sleepTimerMs)

            Write-Host "getting Hbm Xml"
            if($IncludeNoPkTables){
                Get-AllHbmXml -OutputNamespace $OutputNamespace -IncludeNoPkTables
            }
            else{
                Get-AllHbmXml -OutputNamespace $OutputNamespace
            }

            [System.Threading.Thread]::Sleep($sleepTimerMs)

            Write-Host "getting Hbm Cs"
            if($IncludeNoPkTables){
                Get-AllHbmCs -IncludeNoPkTables
            }
            else{
                Get-AllHbmCs
            }
        
            [System.Threading.Thread]::Sleep($sleepTimerMs)

            Write-Host "compiling Hbm Cs"
            Invoke-HbmCsCompile -OutputNamespace $OutputNamespace

        }
        [System.Threading.Thread]::Sleep($sleepTimerMs)

        Write-Host "making Hbm AppConfig"
        $hbmConfigFilePath = New-HbmAppConfig -OutputNamespace $OutputNamespace 

        [System.Threading.Thread]::Sleep($sleepTimerMs)

        Write-Host "loading Hbm Configuration"
        Import-HbmConfiguration -HbmAppConfigPath $hbmConfigFilePath

        [System.Threading.Thread]::Sleep($sleepTimerMs)

        Write-Host "loading Hbm Session Factory"
        Import-HbmSessionFactory

        [System.Threading.Thread]::Sleep($sleepTimerMs)

        Write-Host "loading Hbm Session"
        return Get-HbmSession

    }
}#end Get-HbmDb
