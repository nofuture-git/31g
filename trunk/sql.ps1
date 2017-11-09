try{
if(-not [NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("sql",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("pss",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("tss",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("css",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("proc",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("ado",$MyInvocation.MyCommand)

[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Get-MssqlSettings",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("ExportTo-UpdateStatement",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("ExportTo-InsertStatement",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("ExportTo-MergeStatement",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Export-CsvToScriptTempTable",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Get-TableMetaData",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Get-TableXsd",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Get-DatabaseDbml",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Import-ExcelWs",$MyInvocation.MyCommand)
[NoFuture.Shared.Core.NfConfig+MyFunctions]::FunctionFiles.Add("Find-StringInDb",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}
#=================================================

<#
    .SYNOPSIS
    Prints to console the current settings of this script.
    
    .DESCRIPTION
    Prints the current sql server settings.  Includes the server, 
    catalog, whether the sql command-line headers are off, 
    whether a dbo filter is on and also a reference matrix for which
    the user can quickly change the server and catalog herein.
    
    .PARAMETER QuickChange
    A simple int array whose values map to the printed matrix Y and 
    X axis respectively.
    
    .EXAMPLE
    C:\PS> Get-MssqlSettings
    
    .EXAMPLE
    C:\PS> Get-MssqlSettings 0,2
    
    .EXAMPLE
    C:\PS> Get-MssqlSettings -QuickChange @(0,2)

    .OUTPUTS
    null
    
#>
function Get-MssqlSettings
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [System.Array] $QuickChange
    )
    Process
    {
        if($QuickChange -eq $null -or $QuickChange.Length -le 1){
            return [NoFuture.Sql.Mssql.Etc]::PrintCurrentDbSettings();
        }

        [int]$qc1 = 0;
        [int]$qc2 = 0;

        if(-not ([int]::TryParse($QuickChange[0].ToString(), [ref] $qc1))){
            return [NoFuture.Sql.Mssql.Etc]::PrintCurrentDbSettings();
        }

        if(-not ([int]::TryParse($QuickChange[1].ToString(), [ref] $qc2))){
            return [NoFuture.Sql.Mssql.Etc]::PrintCurrentDbSettings();
        }

        [NoFuture.Sql.Mssql.Etc]::SetMssqlSettings($qc1, $qc2);
        return [NoFuture.Sql.Mssql.Etc]::PrintCurrentDbSettings();
    }
}

#=================================================
#imprimis codex
Set-Alias sql Invoke-SqlCommand
Set-Alias ado Get-DataTable
Set-Alias pss Select-ProcedureString
Set-Alias css Select-ColumnName
Set-Alias tss Select-TableName
Set-Alias proc Write-StoredProcedureToHost
#=================================================

<#
    .SYNOPSIS
    Executes the given statement on the current server/catalog.
    
    .DESCRIPTION
    The table name MUST be fully qualified with [catalog].[dbo].[table]
    
    Executes a given valid sql statement on the current server/catalog. 
    The command simply invokes the command line sql tool provided with the
    install of SSMS.  Typically the install will add the Binn directory to
    the enviornment's PATH variable. The command makes no check that this is 
    the case.  
    
    Set the global variable NoFuture.Shared.NfConfig+Switches.SqlCmdHeadersOff to true to get only the results.
    The delimiter is a bar character (|).  This command is intended for 
    update, delete, insert and scaler selects'.  Use the ADO function for 
    selecting multiple records. 
    
    Set the NoFuture.Sql.Mssql.Etc.WarnUserIfServerIn to an array of protected servers and 
    invokeing this command will warn the user they are pointed to production.
    
    .PARAMETER Expression
    A full file name, a full directory path or a valid 
    and complete sql statement.

    .PARAMETER ServerName
    Optional name of the server, defaults to the global 
    at NoFuture.Shared.Constants.SqlServer

    .PARAMETER CatalogName
    Optional name of the database, defaults to the global 
    at NoFuture.Shared.Constants.SqlCatalog

    .PARAMETER IsPath
    Optional, swtich to indicate that the 'Expression' is a path and not 
    a sql query literal.
    
    .EXAMPLE
    C:\PS> Invoke-SqlCommand "select top 1 an_id from [MyCatalog].[dbo].[MyTable]"
    
    .EXAMPLE
    C:\PS> Invoke-SqlCommand "C:\Projects\MySql\MySql.sql" -IsPath

    .OUTPUTS
    Prints the response from SqlCmd.exe
    
#>
function Invoke-SqlCommand
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Expression,
        [Parameter(Mandatory=$false,position=1)]
        [string] $ServerName,
        [Parameter(Mandatory=$false,position=2)]
        [string] $CatalogName,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $IsPath

    )
    Process
    {
      if([System.String]::IsNullOrWhiteSpace($ServerName)){$ServerName = ([NoFuture.Shared.Core.NfConfig]::SqlServer)}
      if([System.String]::IsNullOrWhiteSpace($CatalogName)){$CatalogName = ([NoFuture.Shared.Core.NfConfig]::SqlCatalog)}

      if($IsPath){
        $cmd =  ([NoFuture.Sql.Mssql.Etc]::MakeInputFilesSqlCmd($expression,$ServerName,$CatalogName))
      }
      else{
        $cmd =  ([NoFuture.Sql.Mssql.Etc]::MakeSqlCmd($expression,$ServerName,$CatalogName))
      }
      if(([NoFuture.Sql.Mssql.Etc]::WarnUserIfServerIn -contains $ServerName) -and ([NoFuture.Shared.Core.NfConfig+Switches]::SupressNpp -eq $false)){
        $message = "WARNING: current settings are pointed to production!`nDo you want to continue this operation?"
        $yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", `
            "Continue operation using SQLCMD.exe."

        $no = New-Object System.Management.Automation.Host.ChoiceDescription "&No", `
            "Gotta go."

        $options = [System.Management.Automation.Host.ChoiceDescription[]]($yes, $no)

        $result = $host.ui.PromptForChoice("SQLCMD.exe", $message, $options, 0) 

        if($result -ne 0){break;}
        
      }
      $val = Invoke-Expression $cmd;

      if(-not $IsPath){
        $trimval = $false;
        $i = 0
        #look to remove the blank lines and the '(Total Records ...)' 
        $val | % {
            $i++; 
            if(($_.Trim() -ne "") -and (-not $_.Contains("("))){
                $trimval = $true;
            }
        }
        if($trimval) {$val = $val[0..$($i-3)];}
        return $val
      }
      return $val; 
    }
}

<#
    .synopsis
    Runs a valid select sql query against the current server/catalog.
    
    .Description
    Runs a valid select sql query against the current settings' catalog & server.
    The script will error on any query that does not start with 'select'.
    
    The fully qualified table name is NOT required for the ADO object.
    
    Due to limitations in powershell the return type is an array in which the 
    object at index 1 is the actual datatable object so any calls to this function
    must use the index syntax on the results to get to the real ado datatable object.
    
    No try/catch is included within the body of the function; calling assemblies must
    handle the errors themselves.
    
    .parameter Expression
    A string which is a valid sql select query.
    
    .example
    C:\PS>$myReturn = Get-DataTable -Expression "select max(an_id) from myTable where an_id > 1000"
    
    .outputs
    Datatable
    
#>
function Get-DataTable
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=1)]
        [string] $ServerName,
        [Parameter(Mandatory=$false,position=2)]
        [string] $CatalogName,
        [Parameter(Mandatory=$true,position=0)]
        [string] $Expression
    )
    Process
    {
        if(-not([System.String]::IsNullOrWhiteSpace($ServerName)) -and  -not([System.String]::IsNullOrWhiteSpace($CatalogName))){
            $connStr = "Server=$ServerName;Database=$CatalogName;Trusted_Connection=True;"
        }
        else{
            $connStr = [NoFuture.Shared.Core.NfConfig]::SqlServerDotNetConnString
        }
        
    	$sqlCon = New-Object System.Data.SqlClient.SqlConnection $connStr
        $sqlCon.Open()
    	$sqlCmd = $sqlCon.CreateCommand()
    	$sqlCmd.CommandText = $Expression
    	$dt = New-Object System.Data.DataTable 
    	$da = New-Object System.Data.SqlClient.SqlDataAdapter
    	$da.SelectCommand = $sqlCmd
    	$doNotDisplay = $da.Fill($dt)
    	$sqlCon.Close()
    	return $dt
    }
}


<#
    .synopsis
    Gets the name of all tables in which the name contains the given string.
    
    .Description
    This is simply a faster way to select from the information_schema.tables table.
    In which the where statement reads 'LIKE '%the input string here%'
    
    The function attempts to filter the results based on the currently
    assigned filter list; however, it is not required.
    
    .parameter TableName
    Full or partial table name less the [dbo] and catalog names.
    
    .example
    C:\PS>Select-TableName "mytabl"
    
    .outputs
    String
    
#>
function Select-TableName
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [AllowEmptyString()]
        [string] $TableName
    )
    Process
    {
        #table_name
        $expression = ([NoFuture.Sql.Mssql.Qry.Catalog]::TblSelectString -f $TableName)
        $expression = [NoFuture.Sql.Mssql.Etc]::FilterSqlExpression($expression, "table_name")
    	sql $expression
    }
}


<#
    .synopsis
    Gets all columns whose name is a full or partial match.
    
    .Description
    Gets all the columns, including data-type, ordinal, etc whose
    name is a full or partial match.  The TableName parameter is 
    optional and simlpy resolves the search to that table alone. 
    
    The ColumnName is not optional but setting the value to empty string
    and having a value in TableName will cause all the columns in TableName
    to be returned.
    
    When no match is found the function will return the string "no results".
    
    The function attempts to filter the results based on the currently 
    assigned filter list; however, it is not required.
    
    .parameter ColumnName
    Matched using 'like', pass in empty string to get all
    
    .parameter TableName
    Matched using 'like', pass in empty string to get all
    
    .parameter TableSchema
    Pass in empty string for 'dbo'
    
    .example
    C:\PS>Select-ColumnName -ColumnName "_id" -TableName "myTable"
    
    .example
    C:\PS>Select-ColumnName -ColumnName "" -TableName "myTable" #will return all columns therein
    
    .example
    C:\PS>Select-ColumnName "_id" #will return every match across all tables.

    .outputs
    System.Array of DataRow(s)
    
#>
function Select-ColumnName
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=0)]
        [string] $ColumnName,
        [Parameter(Mandatory=$false,position=1)]
        [string] $TableName,
        [Parameter(Mandatory=$false,position=2)]
        [string] $SchemaName,
        [Parameter(Mandatory=$false,position=3)]
        [string] $ServerName,
        [Parameter(Mandatory=$false,position=4)]
        [string] $CatalogName


    )
    Process
    {
        if(-not [System.String]::IsNullOrWhiteSpace($ServerName) -and -not [System.String]::IsNullOrWhiteSpace($CatalogName)){
            [NoFuture.Shared.Core.NfConfig]::SqlServer = $ServerName
            [NoFuture.Shared.Core.NfConfig]::SqlCatalog = $CatalogName
        }

        $expression = ([NoFuture.Sql.Mssql.Qry.Catalog]::ColSelectString -f $ColumnName,$TableName,$SchemaName)
        
    	ado $expression
    }
}


<#
    .synopsis
    Searches the body of all stored procedures for the occurance of the given regex pattern.
    
    .Description
    Searches within the body of all stored procedures of the current server/catalog. 
    When a match is found the entire body of the stored procedure is saved to a temp 
    directory then the contents of that temp directory are searched using the 
    Select-String cmdlet.  The results of the Select-String cmdlet are printed to the
    console in the same manner as always.
    
    The temp directories contents' are completely removed per every call tho this 
    function.
    
    The function attempts to filter out any results whose object name is listed 
    in the current filter; however, a filter is not required.
    
    The function will break, leaving the current contents of the temp directory as-is
    if no stored procedures are found to match what-so-ever.  In such a case the
    function will print 'no results' to the console.
    
    .parameter Pattern
    A true regex pattern.  The value is NOT used in sql 'LIKE' statements
    
    .example
    C:\PS>Select-ProcedureString -Pattern "^\s+[0-9a-z_\s\.]+MyTableName.*"
    
    .outputs
    Results of Select-String
    
#>
function Select-ProcedureString
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Pattern
    )
    Process
    {
        $proxDir = [NoFuture.Shared.Core.NfConfig+TempDirectories]::StoredProx
        #name
        $expression = [NoFuture.Sql.Mssql.Qry.Catalog]::SelectProcedureString
        $expression = [NoFuture.Sql.Mssql.Etc]::FilterSqlExpression($expression, "object_name(id)")
        $qryRslts = ado $expression
        if($qryRslts -eq $null -or $qryRslts.Rows.Count -eq 0){return "no results"}
        
        #check for temp spot existing, make if not
        if(-not (Test-Path $proxDir)){mkdir -Path $proxDir}
        
        #clear out the contents of the temp spot for every call herein
        ls -Path $proxDir | ? {-not $_.PSIsContainer} | % {rm -Path $_.FullName -Force}
        
        #iterate the procs looking for a match
        $procs = $qryRslts | % {
            if($_.ProcText -match $pattern){
            
                #expect repeats so test before wasting time
                $tempName = join-path $proxDir ("{0}.sql" -f $_.ProcName)
                if(-not (Test-Path $tempName)){
                    $_.ProcText > $tempName
                }
            } 
        }
        ssr $pattern $proxDir
    }
}


<#
    .SYNOPSIS
    Prints the contents of a stored procedure to NotePad++.
    
    .DESCRIPTION
    Print the contents of a stored procedure with the same formatting
    as it appears in SSMS to NotePad++.  The body of the stored
    procedure is actually save to file within the temp directory.
    
    Furthermore, the temp directory gets wiped per every call to 
    this function.
    
    If the given procedure name produces no results then the 
    temp directory is left as-is and a message is printed to the 
    console.
    
    .PARAMETER ProcedureName
    The name of the stored procedure less and catalog or [dbo] qualifiers.
    
    .EXAMPLE
    C:\PS>Write-StoredProcedureToHost -ProcedureName "p_myStoredProc"
    
    .EXAMPLE
    C:\PS>Write-StoredProcedureToHost "fn_worksForAnyTextInSysComments"
    
    .OUTPUTS
    string
    
#>
function Write-StoredProcedureToHost
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $ProcedureName,
        [Parameter(Mandatory=$false,position=1)]
        [string] $SchemaName,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $ToConsole,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $FilesOnly

    )
    Process
    {
        $proxDir = [NoFuture.Shared.Core.NfConfig+TempDirectories]::StoredProx
        $prettyProc = ""
        if([System.String]::IsNullOrWhiteSpace($SchemaName)) 
        { 
            $SchemaName = "dbo"
        }
        $expression = ([NoFuture.Sql.Mssql.Qry.Catalog]::ProcPrintText -f $ProcedureName,$SchemaName)
    	$procArray = ado $expression
        $procArray | % { $prettyProc += $_.ProcText }
        if($prettyProc -eq $null -or $prettyProc.Length -eq 0){
            Write-Host ("'{0}' not found." -f $ProcedureName) 
            
            #now leave
            break;
        }

        #print this to notepad++ since it has syntax highlighter
        
        $tempName = join-path ($proxDir) ("{0}.sql" -f ([NoFuture.Util.Core.NfPath]::SafeFilename($ProcedureName)))
        
        #check for temp spot existing, make if not
        if(-not (Test-Path $proxDir)){mkdir -Path $proxDir}
        
        #remove any previous copies of this stored proc by force
        if(Test-Path $tempName){rm -Path $tempName -Force}
        
        #print the contents to a temp text doc and display in notepad++
        $prettyProc > $tempName
        if($FilesOnly){return;}

        if($ToConsole) {
            return $prettyProc;
        }
        else{
            npp $tempName
        }
    }

}

<#
    .SYNOPSIS
    Returns db results as a hashtable.
    
    .DESCRIPTION
    This cmdlet is only intended for sql expressions
    with two columns in the select were the column at 
    ordinal 0 is the key column at ordinal 1 are the values.
    
    .PARAMETER Expression
    Some sql statement.
    
    .OUTPUTS
    Hashtable
    
#>
function Convert-SqlToHashtable
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Expression
    )
    Process
    {
        if( -not ($Expression -match $(Get-RegexCatalog).SqlSelectValues)){
            Write-Host "Could not parse the SELECT portion of the Expression" -ForegroundColor Yellow
            break;
        }

        $selectColumnsRawStr = $Matches.Values | ? {-not [string]::IsNullOrWhiteSpace($_.Trim()) -and -not $_.Trim().ToLower().StartsWith("select")} | Select-Object -First 1

        if($selectColumnsRawStr -eq $null -or $selectColumnsRawStr.Length -eq 0){
            Write-Host "Could not parse the SELECT portion of the Expression" -ForegroundColor Yellow
            break;
        }

        $columnParts = $selectColumnsRawStr.Split(",") | % {$_.Trim().Replace("[","").Replace("]","")}

        $columnNames = $columnParts |? {-not [string]::IsNullOrWhiteSpace($_)} | % {
            $splitColParts = $_.Split(" ")
            $splitColParts[($splitColParts.Length-1)]
        }
        $dbRslts = ado $Expression

        if($dbRslts-eq $null -or $columnNames -eq $null -or $columnNames.Count -le 1){
            return $dbRslts;
        }

        $toJsonHash = @{}

        $firstLevels = ($dbRslts | % {$_[$columnNames[0]]} | Sort-Object -Unique)

        $firstLevels | ? {-not [string]::IsNullOrWhiteSpace($_)} | % {
            $cKey = $_;
            $toJsonHash += @{$ckey = ($dbRslts | ? {$_[$columnNames[0]] -eq $cKey} | % {$_[$columnNames[1]]} | Sort-Object -Unique)}
        }
        return $toJsonHash
    }
}

#=================================================
# EXPORT QUERY RESULTS TO A FILE
#=================================================
<#
    .SYNOPSIS
    Given results of a sql expression, create a merge statement thereof.
    
    .DESCRIPTION
    Drafts a file whose contents are SQL.
    The generated file has is divided into sections
    using C-style block comments.
    
    .PARAMETER Expression
    Some sql statement.
    
    .EXAMPLE
    C:\PS>ExportTo-MergeStatement "select top 10 * from myTableName where created_date > '2012-12-22 00:00:00'" 40
    
    .EXAMPLE
    C:\PS>ExportTo-MergeStatement -Expression "select * from myTableName" -MaxLength 40
    
    .OUTPUTS
    String
    
#>
function ExportTo-MergeStatement
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Expression,
        [Parameter(Mandatory=$false,position=1)]
        [int] $MaxLength
    )
    Process
    {
        return ExportTo-SqlScript -Expression $Expression -ExportType MERGE -MaxLength $MaxLength
    }
}

<#
    .SYNOPSIS
    Exports the results of a sql select query to a file as a valid sql update statement.
    
    .DESCRIPTION
    Given a valid sql select statement the results returned from the current catalog/server
    are written to a file which is in the form of a valid sql update statement.
    
    The function will open NotePad++ displaying the file just created.  This functionality may
    be stopped by setting the $suppressNpp variable.
    
    The name of the file will also be printed to the console
    
    .PARAMETER Expression
    A valid sql select statement expression.  This does not need the table names to be fully
    qualified with catalog name and [dbo] prefix.
    
    .PARAMETER MaxLength
    An integer whose value represents the maximum length the returned results will print.
    This parameter is intended to truncate text and very large varchar field values.
    
    .EXAMPLE
    C:\PS>ExportTo-UpdateStatement "select top 10 * from myTableName where created_date > '2012-12-22 00:00:00'" 40
    
    .EXAMPLE
    C:\PS>ExportTo-UpdateStatement -Expression "select * from myTableName" -MaxLength 40
    
    .OUTPUTS
    String
    
#>
function ExportTo-UpdateStatement
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Expression,
        [Parameter(Mandatory=$false,position=1)]
        [int] $MaxLength
    )
    Process
    {
        return ExportTo-SqlScript -Expression $Expression -ExportType UPDATE -MaxLength $MaxLength
    }
}

<#
    .SYNOPSIS
    Exports the results of a sql select query to a file as a valid sql insert statement.
    
    .DESCRIPTION
    Given a valid sql select statement the results returned from the current catalog/server
    are written to a file which is in the form of a valid sql insert statement.
    
    The function will open NotePad++ displaying the file just created.This functionality may
    be stopped by setting the $suppressNpp variable.
    
    The name of the file will also be printed to the console
    
    .PARAMETER Expression
    A valid sql select statement expression.  This does not need the table names to be fully
    qualified with catalog name and [dbo] prefix.
    
    .PARAMETER MaxLength
    An integer whose value represents the maximum length the returned results will print.
    This parameter is intended to truncate text and very large varchar field values.
    
    .EXAMPLE
    C:\PS>ExportTo-InsertStatement "select top 10 * from myTableName where created_date > '2012-12-22 00:00:00'" 40
    
    .EXAMPLE
    C:\PS>ExportTo-InsertStatement -Expression "select * from myTableName" -MaxLength 40
    
    .OUTPUTS
    String
    
#>
function ExportTo-InsertStatement
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Expression,
        [Parameter(Mandatory=$false,position=1)]
        [int] $MaxLength
    )
    Process
    {
        return ExportTo-SqlScript -Expression $Expression -ExportType INSERT -MaxLength $MaxLength
    }
}


<#
    .SYNOPSIS
    Exports the results of a sql select query to a file as a valid sql statement.
    
    .DESCRIPTION
    Given a valid sql select statement the results returned from the current catalog/server
    are written to a file which is in the form of a valid sql statement.
    
    The function will open NotePad++ displaying the file just created.
    
    The name of the file will also be printed to the console
    
    .PARAMETER Expression
    A valid sql select statement expression.  This does not need the table names to be fully
    qualified with catalog name and [dbo] prefix.
    
    .PARAMETER MaxLength
    An integer whose value represents the maximum length the returned results will print.
    This parameter is intended to truncate text and very large varchar field values.
    
    .EXAMPLE
    C:\PS>ExportTo-SqlScript "select top 10 * from myTableName where created_date > '2012-12-22 00:00:00'" 40
    
    .EXAMPLE
    C:\PS>ExportTo-SqlScript -Expression "select * from myTableName" -MaxLength 40
    
    .OUTPUTS
    String
    
#>
function ExportTo-SqlScript
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Expression,
        [Parameter(Mandatory=$true,position=1)]
        [NoFuture.Sql.Mssql.ExportToStatementType] $ExportType,
        [Parameter(Mandatory=$false,position=2)]
        [int] $MaxLength
    )
    Process
    {
        if([string]::IsNullOrWhiteSpace([NoFuture.Shared.Core.NfConfig]::SqlServer) -or 
           [string]::IsNullOrWhiteSpace([NoFuture.Shared.Core.NfConfig]::SqlCatalog)){

            Write-Host "Set the connection first using Mssql-Settings cmdlet" -ForegroundColor Yellow
            break;
        } 
        #get tablename and Schema
        $schemaTable =  [NoFuture.Sql.Mssql.ExportTo]::GetTableSchemaAndNameFromExpression($Expression)
        if($schemaTable -eq $null -or ([string]::IsNullOrWhiteSpace($schemaTable.TableName))) {
            throw "Could not determine the first Schema and Table names from the sql statement '$Expression'"
        }

        $tablename = $schemaTable.TableName
        $tableSchema = $schemaTable.SchemaName
        if([string]::IsNullOrWhiteSpace($tableSchema)){
            $tableSchema = "dbo"
        }

        #get metadata independent of ADO obj
        $metaData = Get-TableMetaData ("[{0}].[{1}]" -f$tableSchema, $tablename)#this has its own parse 

        $adoRows = (ado $Expression)

        $sqlScript = [NoFuture.Sql.Mssql.ExportTo]::ScriptDataBody($Expression,$MaxLength,$ExportType,$metaData,$adoRows);

        $sqlScriptPath = Join-Path ([NoFuture.Shared.Core.NfConfig+TempDirectories]::Sql) ([NoFuture.Shared.Core.NfConfig]::SqlServer)

        if(-not (Test-Path ($sqlScriptPath))){$dnd = mkdir -Path $sqlScriptPath -Force}

        $sqlScriptPath = Join-Path $sqlScriptPath ([NoFuture.Shared.Core.NfConfig]::SqlCatalog)

        if(-not (Test-Path ($sqlScriptPath))){$dnd = mkdir -Path $sqlScriptPath -Force}

        $exportTypeName = [Enum]::GetName([NoFuture.Sql.Mssql.ExportToStatementType],$ExportType)

        $sqlScriptPath = Join-Path $sqlScriptPath ("{0}.{1}.{2}.sql" -f $tableSchema,$tablename,$exportTypeName)

        $sqlScript > $sqlScriptPath

        npp $sqlScriptPath

        return $sqlScriptPath
    }
}

<#
    .SYNOPSIS
    Gets all meta-data concerning the given table.
    
    .DESCRIPTION
    Returns a new instance of NoFuture.Sql.Mssql.Md.PsMetadata having each 
    of its various collections assigned to the query results of calls to the 
    database.

    If the optionals (viz. ServerName & CatalogName) should be thought of 
    as a pair; meaning, one should not be assigned a value without the other.
    Assigning these will cause the global connection string to be reset to 
    these values.

    .PARAMETER TableName
    The name of the targeted table.  
    The parameter may be fully qualified or unqualified.

    .PARAMETER ServerName
    Optional, having this requires something, likewise, in CatalogName.

    .PARAMETER CatalogName
    Optional, having this requires something, likewise, in ServerName.
    
    .EXAMPLE
    C:\PS>Get-TableMetaData "myTable"
    
    .EXAMPLE
    C:\PS>Get-TableMetaData -TableName "[mycatalog].[dbo].[myTable]"
    
    .OUTPUTS
    NoFuture.Sql.Mssql.Md.PsMetadata
    
#>
function Get-TableMetaData
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$false,position=1)]
        [string] $ServerName,
        [Parameter(Mandatory=$false,position=2)]
        [string] $CatalogName,
        [Parameter(Mandatory=$true,position=0)]
        [string] $TableName
    )
    Process
    {
        if(-not [System.String]::IsNullOrWhiteSpace($ServerName) -and -not [System.String]::IsNullOrWhiteSpace($CatalogName)){
            [NoFuture.Shared.Core.NfConfig]::SqlServer = $ServerName
            [NoFuture.Shared.Core.NfConfig]::SqlCatalog = $CatalogName
        }

        if($TableName.Contains(".")){
            $parseSchemaTable = [NoFuture.Sql.Mssql.ExportTo]::GetTableSchemaAndNameFromExpression($TableName);
            if($parseSchemaTable -eq $null){
                throw "Could not determine the schema nor table parts of the string '$TableName'"
            }
            $TableName = $parseSchemaTable.TableName
            $SchemaName = $parseSchemaTable.SchemaName;
        }
        else{
           $TableName = $TableName.Replace("[",[string]::Empty).Replace("]",[string]::Empty)
           $SchemaName = "dbo" 
        }
        
        Write-Progress -Activity ("Unqualified table name calculated as '{0}'" -f $TableName) -Status "OK" -PercentComplete 10
    
        #check that the table is a valid one
        [NoFuture.Shared.Core.NfConfig+Switches]::SqlCmdHeadersOff = $true
        $countByTableName = sql ([NoFuture.Sql.Mssql.Qry.Catalog]::CountByTableName -f $TableName)
        if($countByTableName -eq 0){throw ("The table name '{0}' is not present in the current database catalog" -f $Tablename)}

        $psMetadata = New-Object NoFuture.Sql.Mssql.Md.PsMetadata

        #get list of columns that are varcharesque
        Write-Progress -Activity ("Getting all columns in table '{0}' that are will require ticks around the literal." -f $TableName) -Status "OK" -PercentComplete 20

        $adoRows = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryTableStringTypes -f $TableName))
        $psMetadata.TickKeys = [NoFuture.Sql.Mssql.Md.PsMetadata]::ColumnNameToList($adoRows);
        
        Write-Progress -Activity ("Getting all columns in table '{0}' that are part of a unique index." -f $TableName) -Status "OK" -PercentComplete 30

        $adoRows = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryTableUniqueIndex -f $TableName))
        $psMetadata.UqIndexKeys = [NoFuture.Sql.Mssql.Md.PsMetadata]::ColumnNameToList($adoRows);
        
        Write-Progress -Activity ("Getting all columns in table '{0}' that are primary keys." -f $TableName) -Status "OK" -PercentComplete 40

        $adoRows = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryTableKeys -f ([NoFuture.Sql.Mssql.Qry.Catalog]::PRIMARY_KEY_STRING), $TableName,$SchemaName))
        $psMetadata.PkKeys = [NoFuture.Sql.Mssql.Md.PsMetadata]::KeysToDictionary($adoRows);

        Write-Progress -Activity ("Getting all columns in table '{0}' that have thier values computed." -f $TableName) -Status "OK" -PercentComplete 50

        $adoRows = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryColumnComputed -f $TableName))
        $psMetadata.IsComputedKeys = [NoFuture.Sql.Mssql.Md.PsMetadata]::ColumnNameToList($adoRows);
        
        Write-Progress -Activity ("Getting all columns in table '{0}' that are auto-incremented." -f $TableName) -Status "OK" -PercentComplete 60

        $adoRows = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryColumnAutoNum -f $TableName))
        $psMetadata.AutoNumKeys = [NoFuture.Sql.Mssql.Md.PsMetadata]::ColumnNameToList($adoRows);
        
        Write-Progress -Activity ("Getting all columns in table '{0}' that are foreign keys." -f $TableName) -Status "OK" -PercentComplete 65

        $adoRows = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryTableKeys -f ([NoFuture.Sql.Mssql.Qry.Catalog]::FOREIGN_KEY_STRING), $TableName,$SchemaName))
        $psMetadata.FkKeys = [NoFuture.Sql.Mssql.Md.PsMetadata]::KeysToDictionary($adoRows);
        
        Write-Progress -Activity ("Getting all columns in table '{0}'." -f $TableName) -Status "OK" -PercentComplete 75

        $adoRows = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryAllColumnsBySchemaTable -f  $TableName,$SchemaName))
        $psMetadata.AllColumnNames = [NoFuture.Sql.Mssql.Md.PsMetadata]::ColumnNameToList($adoRows);

        Write-Progress -Activity ("Getting all timestamp columns in table '{0}'." -f $TableName) -Status "OK" -PercentComplete 85

        $adoRows = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryTableTimestampColumns -f  $TableName,$SchemaName))
        $psMetadata.TimestampColumns = [NoFuture.Sql.Mssql.Md.PsMetadata]::ColumnNameToList($adoRows);

        Write-Progress -Activity ("Getting truth-value of '{0}' being auto-increment PK." -f $TableName) -Status "OK" -PercentComplete 90

        [bool]$isId = $false
        $isIdDbRslt = (ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryIsTableSetIdentity -f $TableName)).IsIdentity.ToString()
        if([bool]::TryParse($isIdDbRslt, [ref] $isId)){
            $psMetadata.IsIdentityInsert = $isId
        }

        Write-Progress -Activity ("Done" -f $TableName) -Status "OK" -PercentComplete 100

        return $psMetadata
    } 
}

#=================================================


<#
    .synopsis
    Produces an xsd and cs file for a given table.
    
    .Description
    This function will create an xsd file being derieved from the 
    table's schema as it appears in the current catalog/server. 
    Having produced the xsd for the table this function then will 
    create a cs source-code file thereof using the Microsoft SDK tool
    'xsd.exe'.  
    
    .parameter TableName
    The name of a table in the current server/catalog.  NOTE: no error checking so if its not found then 
    the results will be unpredictable.
    
    .parameter OutputPath
    Full name (path + filename) for the xsd and cs source-code file.
    
    .example
    C:\PS>Get-TableXsd -TableName "MyTable" -OutputPath "C:\MyTableCode\ThisIsMyTable.xsd"
    
    .example
    C:\PS>Get-TableXsd "AnyTable" "C:\output\AnyTable.xsd"
    
    .outputs
    null
    
#>
function Get-TableXsd
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $TableName,
        [Parameter(Mandatory=$true,position=1)]
        [string] $OutputPath
    )
    Process
    {
        $xsdFn = Split-Path $OutputPath -Leaf
        $xsdDir = Split-Path $OutputPath -Parent

        if(-not (Test-Path $xsdDir)){
            Write-Host "directory '$xsdDir' not found" -ForegroundColor Yellow
            break;
        }

        $xsdDir = (Resolve-Path $xsdDir).Path

        $OutputPath = (Join-Path $xsdDir $xsdFn)

        [System.Data.SqlClient.SqlConnection]$conn = New-Object System.Data.SqlClient.SqlConnection([NoFuture.Shared.Core.NfConfig]::SqlServerDotNetConnString);
        [System.Data.SqlClient.SqlCommand]$cmd = $conn.CreateCommand();
        $cmd.CommandText = $("select top 1 * from " + $TableName)
        [System.Data.SqlClient.SqlDataAdapter]$da = New-Object System.Data.SqlClient.SqlDataAdapter
        [System.Data.DataTable]$tbl = New-Object System.Data.DataTable $TableName
        $da.SelectCommand = $cmd
        $da.Fill($tbl)
        [System.Data.DataSet]$ds = New-Object System.Data.DataSet $($TableName);
        $ds.Tables.Add($tbl);
        $ds.WriteXmlSchema($OutputPath);
        $tbl.Dispose()
        $ds.Dispose()
        $da.Dispose()
        $cmd.Dispose()
        $conn.Dispose()
    }
}

<#
    .SYNOPSIS
    Calls SqlMetal.exe, generates the .cs file, compiles it then adds 
    it to this ps appdomain
    
    .DESCRIPTION
    Calls SqlMetal.exe using the NoFuture.Shared.NfConfig.SqlServer 
	and NoFuture.Shared.Constants.SqlCatalog using the /code whose value is given by 
	the NoFuture.Shared.Constants.SqlCatalog less any invalid path characters.
    Upon SqlMetal successfully generating a code file, the C# compiler is called
    which generates the corrosponding binary.  Last this binary is imported into 
    the appdomain using reflection.

    .EXAMPLE
    C:\PS> Mssql-Settings 5,3
    C:\PS> Get-DatabaseDll
    Microsoft (R) Database Mapping Generator 2008 version 4.0.30319.1
    for Microsoft (R) .NET Framework version 4.0
    Copyright (C) Microsoft Corporation. All rights reserved.

    Warning DBML1008: Mapping between DbType 'DateTime2(7) NOT NULL' and Type 'System.DateTime' in Column 'CreateDate' of Type 'Admin_UserSMData' may cause data loss when loading from the database.
    Microsoft (R) Visual C# 2010 Compiler version 4.0.30319.1
    Copyright (C) Microsoft Corporation. All rights reserved.


    GAC    Version        Location                                                                                                                                                                                                  
    ---    -------        --------                                                                                                                                                                                                  
    False  v4.0.30319                                                                                                                                                                                                               

#>
function Get-DatabaseDll
{
    [CmdletBinding()]
    Param
    (
    )
    Process
    {
    
        if(-not(Test-Path ([NoFuture.Shared.Core.NfConfig+X86]::SqlMetal))){throw ("SqlMetal.exe is not found at '{0}'" -f ([NoFuture.Shared.Core.NfConfig+X86]::SqlMetal))}
        if([string]::IsNullOrWhiteSpace([NoFuture.Shared.Core.NfConfig]::SqlServer) -or [string]::IsNullOrWhiteSpace([NoFuture.Shared.Core.NfConfig]::SqlCatalog)) {
            Write-Host "The global sqlServer and sqlCatalog variables are not set, assign them and try again."
        }
        $errCount = $Error.Count;

        $namespace = "NoFuture.Db"
        $codeFile = ("{0}.cs" -f [NoFuture.Shared.Core.NfConfig]::SqlCatalog)
        [System.IO.Path]::InvalidPathChars | % { $codeFile = $codeFile.Replace($_.ToString(),"")}
        $codeFile = (Join-Path ([NoFuture.Shared.Core.NfConfig+TempDirectories]::Code) $codeFile)

        $cmd = ("& '{0}' /server:{1} /database:{2} /code:{3} /namespace:{4}" -f ([NoFuture.Shared.Core.NfConfig+X86]::SqlMetal),([NoFuture.Shared.Core.NfConfig]::SqlServer),([NoFuture.Shared.Core.NfConfig]::SqlCatalog),$codeFile,$namespace)
        Invoke-Expression -Command $cmd
        if($Error.Count -gt $errCount) {break;}

        if(-not (Test-Path $codeFile)){throw "SqlMetal did not generate the expected code file at '$codeFile'"}

        $csc = (Join-Path $global:net40Path $global:cscExe)

        if(-not (Test-Path $csc)) {throw "The Microsoft .NET v4.0 C# compiler was not at the expected location of '$csc'"}

        $cscDll = ([System.IO.Path]::ChangeExtension($codeFile,".dll"))
        $cscCmd = ("& $csc /target:library /out:$cscDll $codeFile")

        $errCount = $Error.Count;

        Invoke-Expression $cscCmd

        if($Error.Count -gt $errCount) {break;}
        if(-not (Test-Path $cscDll)) {throw "The compiled file was not found at the expected location of '$cscDll'"}

        [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($cscDll));
        
    }
}

<#
    .SYNOPSIS
    Dumps each Worksheet to a tab-delimited text file.
    
    .DESCRIPTION
    Give a valid path to an MS Excel file, the cmdlet 
    will create an Excel.Appliction instance, import the file
    to this instance, And then save each worksheet to a 
    tab-delimited text file in the same directory as the Path.

    Returns the full-names of the dumped files as an array.
    
    .PARAMETER Path
    The fully qualified path to an xls or xlsx file.
    
    .EXAMPLE
    C:\PS>$myTempTable = Export-ExcelWbToText -Path "C:\MyTableCode\MyExcelFile.xls"
    
    .OUTPUTS
    Array
    
#>
function Export-ExcelWbToCsv
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path

    )
    Process
    {
         if(-not (Test-Path $Path)){
            Write-Host "bad path or file name '$Path'" -ForegroundColor Yellow
        }

        $wsFileNames = @()
        $csvFileNames = @()

        #dump each worksheet to a tab-delimited file
        $dir = [System.IO.Path]::GetDirectoryName($Path)
        $outFile = [NoFuture.Util.NfType.NfTypeName]::SafeDotNetIdentifier(([System.IO.Path]::GetFileNameWithoutExtension($Path)))
        $outPath = (Join-Path $dir $outFile)
        $myExcel = New-Object -ComObject Excel.Application
        $myExcel.Application.Visible = $false
        $myExcel.Application.DisplayAlerts = $false
        $excelWb = $myExcel.Application.Workbooks.Open($Path)
        $doNotDisplay = [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.Office.Interop.Excel")
        $excelWb.Sheets | % {
            $wsName = $_.Name
            $wsFn = ("{0}{1}.txt" -f $outPath, [NoFuture.Util.NfType.NfTypeName]::SafeDotNetIdentifier($wsName))
            $wsFileNames += $wsFn
            #https://msdn.microsoft.com/EN-US/library/office/ff841185.aspx
            $_.SaveAs($wsFn,[Microsoft.Office.Interop.Excel.XlFileFormat]::xlTextWindows)
        }
        $excelWb = $null
        $myExcel.Quit()
        $myExcel = $null

        #clean up the headers for the Import-Csv cmdlet
        $wsFileNames | ? {Test-Path $_} | % {
            $txtFileName = $_
            $newFileName = [System.IO.Path]::ChangeExtension($txtFileName, ".csv")
            $allLines = [System.IO.File]::ReadAllLines($txtFileName)
            $firstLine = $allLines[0]
            $csvHeader = $firstLine.Split([char]0x09)
            $csvHeader = [NoFuture.Util.Core.Etc]::FormatCsvHeaders($csvHeader)
            $csvHeader = [string]::Join("`t", $csvHeader)
            
            $newContent = @()
            $newContent += $csvHeader
            $newContent += $allLines[1 .. ($allLines.Length - 1)]
            [System.IO.File]::WriteAllLines($newFileName, $newContent)
            $csvFileNames += $newFileName
        }

        return $csvFileNames
    }
}

<#
    .SYNOPSIS
    Converts contents of csv into a script being a list of inserts onto a local table
    
    .DESCRIPTION
    Given a valid, well-formed, csv file this function will
    create a sql file to the same directory with the same name
    as the csv file save the extension which is changed to '.sql'.
    This file's contents then get returned as a string array.

    
    .PARAMETER Path
    The fully qualified path to a csv file.
    
    .EXAMPLE
    C:\PS>$myTempTable = Export-CsvToScriptTempTable -Path "C:\MyTableCode\ThisIsMyTable.csv"
    
    .OUTPUTS
    Array
    
#>
function Export-CsvToScriptTempTable
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $UseTabDelimiter,
        [Parameter(Mandatory=$false,position=2)]
        [string] $TableName
    )
    Process
    {
    #get an output filename and a tablename
    if([string]::IsNullOrWhiteSpace($TableName)){
        $tableName = ""
        if(test-path -Path $path){
            $tableName = [System.IO.Path]::GetFileNameWithoutExtension($path)
        }
        if($tableName -eq "")
        {
            Write-Host "bad file or pathname"
            break;
        }
        $tableName = $tableName.Replace("(","").Replace(")","").Replace(".","").Replace(" ","").Replace("-","")
    }

    $outputPath = $path.Replace([System.IO.Path]::GetExtension($path),".sql")

    #get the data
    if($UseTabDelimiter){
        $data = Import-Csv -Path $path -Delimiter ([char](0x09))
    }
    else{
        $data = Import-Csv -Path $path
    }
    $totalRecords = $data.Length
    
    #get the columns
    $columns = @()
    
    #get the max length of each columne
    ($data | Get-Member -MemberType "NoteProperty") | % {
        
        $cName = $_.Name
        $ctorData = ($data | % {$_.$cName});
        $columns += (New-Object NoFuture.Sql.Mssql.Md.PsTypeInfo($cName, $ctorData))
    }
    
    #craft string for local table in sql syntax
    $declareTable = ""
    $inspectionRow = $data[0]
    $columns | % {
        $declareTable += "`n`t,[{0}] {1}" -f $_.Name,$_.PsDbType
    }

    #remove the first lrcf,tab,comma
    $declareTable = "DECLARE @{0} TABLE (`n`t{1}`n)" -f $tableName, ($declareTable.Substring(3,$declareTable.Length-3))
    
    #push what we got so far to an output table
    "" > $outputPath
    $declareTable >> $outputPath
    "`n`n" >> $outputPath
    
    #craft generic insert statement
    $genericInsert = "INSERT INTO @{0} (" -f $tableName

    #get first column without a comman in front
    $genericInsert += " [{0}]" -f $columns[0].Name

    #get the rest with comma's in front
    for($i=1;$i-lt$columns.Length;$i++){
        $genericInsert += ",[{0}]" -f $columns[$i].Name
    }
    $genericInsert += ")`n VALUES ("
    
    #write a bunch of insert statements to the script
    $data | % {
        $lineInsert = ""
        $line = $_

        $columns | % {
            $myIntOut = 0
            $myDecimalOut = 0.0
            if($line.($_.Name) -eq $null -or $line.($_.Name) -eq "NULL"){
                $lineInsert +=  ",NULL"
            }
            elseif($line.($_.Name) -eq [string]::Empty){
                $lineInsert +=  ",''"
            }
            elseif($_.PsDbType -eq "bit"){
                if(($line.($_.Name)).ToLower() -eq "false"){
                    $lineInsert +=  ",{0}" -f [NoFuture.Shared.Core.Constants]::SQL_SERVER_FALSE
                }
                elseif(($line.($_.Name)).ToLower() -eq "true"){
                    $lineInsert +=  ",{0}" -f [NoFuture.Shared.Core.Constants]::SQL_SERVER_TRUE
                }
                else{
                    $lineInsert +=  ",{0}" -f $line.($_.Name)
                }
            }
            elseif($_.PsDbType -eq "int" -or $_.PsDbType -eq "money" -or $column.PdDbType -eq "binary"){
                 $lineInsert +=  ",{0}" -f $line.($_.Name)
            }
            else{
                $lineInsert +=  ",'{0}'" -f ($line.($_.Name)).Replace("'","")
            }
        }
        #remove first comma
        $lineInsert = $lineInsert.Substring(1,$lineInsert.Length-1)

        #write to output file
        "{0}{1})`n" -f $genericInsert,$lineInsert >> $outputPath
    }
    return (Get-Content -Path $outputPath)
    }#end Process
}


<#
    .SYNOPSIS
    Mines the database for a table and column name which has 
    the search string
    
    .DESCRIPTION
    For discovering if a given string appears any where in the
    database.
    
    .PARAMETER Path
    A simple search string with no MSSQL escape chars
    
    .OUTPUTS
    Hashtable
    
#>
function Find-StringInDb
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $SearchString,
        [Parameter(Mandatory=$false,position=1)]
        [array] $SkipTablesNamed

    )
    Process
    {
        if([string]::IsNullOrWhiteSpace($SearchString)){
            return;
        }
        if([string]::IsNullOrWhiteSpace([NoFuture.Shared.Core.NfConfig]::SqlServer) -or 
           [string]::IsNullOrWhiteSpace([NoFuture.Shared.Core.NfConfig]::SqlCatalog)){

            Write-Host "Set the connection first using Mssql-Settings cmdlet" -ForegroundColor Yellow
            break;
        } 
        Write-Progress -Activity "Getting List of All Tables" -Status "Searching..."
        $allTables = ado ([NoFuture.Sql.Mssql.Qry.Catalog]::TblSelectString -f "")

        if($allTables -eq $null){
            Write-Host "No table data found for current connection";
            break;
        }
        
        if($SkipTablesNamed -eq $null){
            $SkipTablesNamed = @()
        }

        $isMatchTables = @{}

        $counter = 0
        $total = $allTables.Length
        :nextTbl foreach($tbl in $allTables){
            $counter += 1
            $tblName = $tbl.TableName
            $escTableName = "[" + [string]::Join("].[", $tblName.Split(".")) + "]"
            Write-Progress -Activity "Working $tblName" -Status "Searching..." -PercentComplete ([NoFuture.Util.Core.Etc]::CalcProgressCounter($counter, $total))

            if([string]::IsNullOrWhiteSpace($tblName)){
                continue nextTbl;
            }

            if($SkipTablesNamed -contains $tblName){
                continue nextTbl;
            }

            $tblVarCharCols = ado ([NoFuture.Sql.Mssql.Qry.Catalog]::QryTableVarCharTypesWithMinLen -f $tblName, $SearchString.Length)
            if($tblVarCharCols -eq $null){
                continue nextTbl;
            }

            $criteria = @{}

            :nextCol foreach($col in $tblVarCharCols){
                if([string]::IsNullOrWhiteSpace($col)){
                    continue nextCol;
                }

                $colTblName = $col.ColumnName

                $criteria += @{"[$colTblName] LIKE '%$SearchString%'" = $col}
            }

            if($criteria.Keys.Length -le 0){
                continue nextTbl;
            }

            $searchQry = ("SELECT COUNT(*) AS CountOfMatch FROM $escTableName WHERE {0}" -f ([string]::Join(" OR ", [array]($criteria.Keys))))
            
            $errorCount = $Error.Count
            $hasAtLeastOne = ado $searchQry

            if($Error.Count -gt $errorCount){
                Write-Host $searchQry -ForegroundColor Yellow
            }

            if($hasAtLeastOne.CountOfMatch -le 0){
                continue nextTbl;
            }

            #which one?
            :nextCrit foreach($crit in $criteria.Keys){
                if([string]::IsNullOrEmpty($crit)){
                    continue nextCrit;
                }

                $searchQry = ("SELECT COUNT(*) AS CountOfMatch FROM $escTableName WHERE {0}" -f $crit)

                $errorCount = $Error.Count
                $hasAtLeastOne = ado $searchQry
                if($Error.Count -gt $errorCount){
                    Write-Host $searchQry -ForegroundColor Yellow
                }

                if($hasAtLeastOne.CountOfMatch -le 0){
                    continue nextCrit;
                }

                if($isMatchTables.Keys -contains $tblName){
                    $isMatchTables[$tblName] += $criteria[$crit]
                }
                else{
                    $isMatchTables += @{$tblName = @($criteria[$crit])}
                }
            }
        }#end foreach table

        return $isMatchTables
    }
}