try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Convert-CsvToCsCode",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Convert-PsObjsToCsCode",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-FlattenedType",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Write-CsCodeAutoMapper",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Write-CsCodeAssignRand",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-TypePdbLines",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-DotGraphFlattenedType",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-DotGraphClassDiagram",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-DotGraphAssemblyDiagram",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}

$Global:CodeGenAssignRandomValuesCompletedType = @()
$Global:CodeGenAssignRandomValuesCallStack = New-Object System.Collections.Stack


function Convert-CsvToCsCode
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $Path,
        [Parameter(Mandatory=$true,position=1)]
        [char] $Delimiter,
        [Parameter(Mandatory=$false,position=2)]
        [string] $OutputPath,
        [Parameter(Mandatory=$false,position=3)]
        [string] $Namespace
    )
    Process
    {

        if(-not(Test-Path $Path)){
            Write-Host "bad path or file name '$Path'" -ForegroundColor Yellow
            break;
        }

        $Name = [System.IO.Path]::GetFileNameWithoutExtension($Path)

        $Obj = Import-Csv -Path $Path -Delimiter $Delimiter
        return Convert-PsObjsToCsCode -ArrayOfPsObjs $Obj -Name $Name -OutputDir $OutputPath -Namespace $Namespace
    }
}#end Convert-CsvToCsCode

<#
    .SYNOPSIS
    Transposes a PsObject's NoteProperty(s) into a C# class-file.
   
    .DESCRIPTION
    This cmdlet can be used to transpose some PS runtime object
    into a C# class-file.  Its expects that every object passed into
    the 'ArrayOfPsObjs' are identical in form; meaning, all of them
    have the same NoteProperties - only the values thereof change.

    To produce results when each object has its own distinct list
    of NoteProperties, call the cmdlet once for each distinct psObject
    (simply box it into an array).

    .PARAMETER ArrayOfPsObjs
    An array of PsObjects like those created from calls to Import-Csv 
    and ConvertFrom-Json.

    .PARAMETER Name
    This will be the .NET type name.

    .PARAMETER OutputDir
    Optional, the location where the generated C# file will be deposited.
    When omitted, the file will be placed into NoFuture.TempDirectories.Root

    .PARAMETER Namespace
    Optional, a namespace to contain the newly defined type.

    .PARAMETER Extends
    Optional, adds this to the class declaration as its base type.

    .PARAMETER AddOverrideKeywordToProps
    Optional, add this to have the 'overrides' keyword present in the
    properties' signature.

    .OUTPUTS
    string, the path to the generated file
#>
function Convert-PsObjsToCsCode
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [Array] $ArrayOfPsObjs,
        [Parameter(Mandatory=$true,position=1)]
        [string] $Name,
        [Parameter(Mandatory=$false,position=2)]
        [string] $OutputDir,
        [Parameter(Mandatory=$false,position=3)]
        [string] $Namespace,
        [Parameter(Mandatory=$false,position=4)]
        [string] $Extends,
        [Parameter(Mandatory=$false,position=5)]
        [switch] $AddOverrideKeywordToProps
    )
    Process
    {
        
        $tempDir = [NoFuture.TempDirectories]::Root
        if([string]::IsNullOrWhiteSpace($OutputDir) -or -not (Test-Path $OutputDir)){
            $outDir = $tempDir
        }
        else{
            $outDir = $OutputDir
        }

        if([string]::IsNullOrWhiteSpace($Namespace)){
            $Namespace = "NoFuture.Gen"
        }

        $Obj = $ArrayOfPsObjs
        if(-not ([string]::IsNullOrWhiteSpace($Extends))){$Extends = ": $Extends" }
        $TypeName = [NoFuture.Util.Etc]::CapitalizeFirstLetterOfWholeWords($Name,([char]".")).Replace(".","")
        $TypeDecl = @"
using System;
using System.Collections.Generic;
namespace $Namespace
{
"@
        $TypeDecl += @"

    public class $TypeName $Extends
    {

"@
        $members = ( $Obj[0] | Get-Member -MemberType NoteProperty | % {$_.Name})
        $csMembers = @{}
        $members | % {
            $propName = [NoFuture.Util.Etc]::CapitalizeFirstLetterOfWholeWords($_,([char]"_")).Replace("_","") ;
            #member names cannot be the same as thier enclosing types
            if($propName -eq $TypeName){
                $propName = "Name"
            }

            #just in case TypeName actually is 'Name'
            if($propName -eq $TypeName){
                $propName = "Value"
            }
            if($AddOverrideKeywordToProps){
                $TypeDecl += "        public override string $propName { get; set; }`n"
            }
            else{
                $TypeDecl += "        public string $propName { get; set; }`n"
            }
            
            $csMembers += @{$_ = $propName}
        } 
        $TypeDecl += "        private static List<$TypeName> _values;`n"
        $TypeDecl += "        public static List<$TypeName> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<$TypeName>
                           {
                           
"
        $Obj | % {
            $blsCode = $_
            $TypeDecl += @"
                           new $TypeName
                           {

"@
            $csMembers.Keys | % {
                $member = $_
                $csVal = $blsCode.($member)
                $csMember = $csMembers[$member]
                if([string]::IsNullOrWhiteSpace($csVal)){ $csVal = [string]::Empty}
                $csVal = $csVal.Replace("`"","'");
                $TypeDecl += "                               $csMember = `"$csVal`",`n"
            }
                

            $TypeDecl += @"
                           },

"@
            }

        $TypeDecl += @"

                       };
                return _values;
            }
        }
	}//end $TypeName
}//end $Namespace
"@
        $CsCodePath = (Join-Path $outDir ($TypeName + ".cs"))

        [System.IO.File]::WriteAllBytes($CsCodePath, ([System.Text.Encoding]::UTF8.GetBytes($TypeDecl)))

        return $CsCodePath
    }
}#end Convert-PsObjsToCsCode

<#
    .SYNOPSIS
    Generate C# syntax of factory methods which produce
    fully instantiated and populated object graphs.
   
    .DESCRIPTION
    Given some assembly and a type's name the cmdlet
    will generate C# code which produces fully 
    instantiated objects with random values for any
    type that extends ValueType.  When the Recursion 
    flag is set the cmdlet will continue producing 
    object factory methods until no more dependencies 
    are encountered - note, however, this will only
    produce factory methods for dependencies defined in
    the given assembly.

    .PARAMETER Assembly
    The assembly from which contain the type.

    .PARAMETER TypeFullName
    The full name (namespace plus typename) for the 
    type within the source assembly.

    .PARAMETER RandMethods
    Tells the cmdlet to include in the print out the
    functions used to generate all the various value-types.

    .PARAMETER Recursion
    Tells the cmdlet to keep generating Factory methods for
    any type encountered anywhere down the object graph all 
    the way to types that have only value-type properties.

    .OUTPUTS
    string
#>
function Write-CsCodeAssignRand
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.Reflection.Assembly] $Assembly,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeFullName,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $FromSelf,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $RandMethods,
        [Parameter(Mandatory=$false,position=4)]
        [switch] $Recursion
    )
    Process
    {
        if($Assembly.GetType($TypeFullName) -eq $null){return;}

        $recursiveCallsTo = @()

        $typeName = [NoFuture.Util.NfTypeName]::GetTypeNameWithoutNamespace($TypeFullName)

        $myrand = New-Object System.Random ([Int32][String]::Format("{0:fffffff}",$(get-date)))

        $instanceName = "subject";

        $needsStringConstDefined = $false

        if(-not $FromSelf){
            #reset this global
            $Global:CodeGenAssignRandomValuesCompletedType = @()
            $Global:CodeGenAssignRandomValuesCallStack = New-Object System.Collections.Stack
            $Global:CodeGenAssignRandomValuesCallStack.Push($TypeFullName)

            if($RandMethods){
@"
        internal const int STR_LEN = 10;
        internal const int STR_START = 0x41;
        internal const int STR_END = 0x5A;
        internal const int MAX_DEPTH = 16;

        private int _depth = 0;
        public int Depth
        {
            get {return _depth;}
            set {_depth = value;}
        }
        private System.Random _myrand = new System.Random(System.DateTime.Now.Millisecond);

        internal string StringFactory()
        {
            return StringFactory(STR_START,STR_END,STR_LEN);
        }

        internal string StringFactory(int asciiStart, int asciiEnd, int length)
        {

            var myrand = _myrand;
            var rtrn = new char[length];
            for (var i = 0; i < length; i++)
            {
                rtrn[i] = System.Convert.ToChar(myrand.Next(asciiStart, asciiEnd));
            }
            return new string(rtrn);

        }
        internal System.DateTime DateTimeFactory()
        {
            var oldestyear = System.DateTime.Now.Year - 1;
            var youngestyear = System.DateTime.Now.Year + 1;

            return new System.DateTime(_myrand.Next(oldestyear, youngestyear), _myrand.Next(1, 12), _myrand.Next(1, 28));
        }

        internal byte ByteFactory()
        {
            return System.Convert.ToByte(_myrand.Next(0, byte.MaxValue));
        }
        internal System.Int16 Int16Factory()
        {
            return System.Convert.ToInt16(_myrand.Next(0, short.MaxValue));
        }
        internal System.Int32 Int32Factory()
        {
            return _myrand.Next(0, int.MaxValue);
        }
        internal System.Int64 Int64Factory()
        {
            return System.Convert.ToInt64(_myrand.Next(0, int.MaxValue));
        }
        internal System.Decimal DecimalFactory()
        {
            return System.Convert.ToDecimal(_myrand.Next(0, int.MaxValue));
        }
        internal System.Double DoubleFactory()
        {
            return System.Convert.ToDouble(_myrand.Next(0, int.MaxValue));
        }
        internal char CharFactory()
        {
            return System.Convert.ToChar(StringFactory(0x41, 0x5A, 1));
        }
        internal bool BooleanFactory()
        {
            if (_myrand.Next(1, 999) % 2 == 0)
            {
                return true;
            }
            return false;
        }
        internal System.Guid GuidFactory()
        {
            return System.Guid.NewGuid();
        }
        
"@
            }#end if RandMethods
        }
        $Global:CodeGenAssignRandomValuesCompletedType += $TypeFullName;
        $factoryName = "{0}Factory" -f $typeName

        "public $TypeFullName $factoryName()`n{`n`tvar $instanceName = new $TypeFullName();`n`t_depth += 1;`n`tif(_depth > MAX_DEPTH){return $instanceName;}`n"

        #get members as name-type pairs
        $listMembers = @{}
        $valueMembers = @{}
        $normalMembers = @{}

        $Assembly.GetType($TypeFullName).GetProperties() | % {
            if($_.CanWrite -and  ([NoFuture.Util.NfTypeName]::IsEnumerableReturnType($_.PropertyType))){
                $listMembers.Add($_.Name,$_.PropertyType)
            }
            elseif($_.CanWrite -and ([NoFuture.Util.NfTypeName]::IsValueTypeProperty($_))){
                $valueMembers.Add($_.Name,$_.PropertyType)
            }
            elseif($_.CanWrite -and -not ([NoFuture.Util.NfTypeName]::IsValueTypeProperty($_)) -and -not ([NoFuture.Util.NfTypeName]::IsEnumerableReturnType($_.PropertyType))){
                $normalMembers.Add($_.Name,$_.PropertyType)
            }
            
        }
        $Assembly.GetType($TypeFullName).GetFields() | % {
            if($_.IsPublic -and  ([NoFuture.Util.NfTypeName]::IsEnumerableReturnType($_.FieldType))){
                $listMembers.Add($_.Name,$_.FieldType)
            }
            elseif($_.IsPublic -and ([NoFuture.Util.NfTypeName]::IsValueTypeField($_))){
                $valueMembers.Add($_.Name,$_.FieldType)
            }
            elseif($_.IsPublic -and -not ($_.IsLiteral) -and -not ([NoFuture.Util.NfTypeName]::IsValueTypeField($_)) -and -not ([NoFuture.Util.NfTypeName]::IsEnumerableReturnType($_.FieldType))){
                $normalMembers.Add($_.Name,$_.FieldType)
            }
            
        }

        $listMembers.Keys | % {
            $memberName = $_
            $memberType = $listMembers[$_]
            $enumerableType = [NoFuture.Util.NfTypeName]::GetLastTypeNameFromArrayAndGeneric($memberType)
            $recursiveCallsTo += $enumerableType
            $propertyType = $memberType.ToString()
            $refFactoryName = "{0}Factory" -f  ([NoFuture.Util.NfTypeName]::GetTypeNameWithoutNamespace($enumerableType))
            $propName = $memberName;

            if($memberType.BaseType.FullName -eq "System.Array"){
                "`t$instanceName.$propName = new []"
                "`t`t{`n`t`t`t$refFactoryName(),`n`t`t`t$refFactoryName(),`n`t`t`t$refFactoryName()`n`t`t};"
            }
            else{

                #change IL syntax to C-sharp syntax
                if($propertyType -match "\x60[1-9]\x5B"){
                    $propertyType = $propertyType.Replace($Matches[0],'<').Replace("]",">")
                }
                @("IEnumerable","ICollection","IList", "IQueryable") | ? {$propertyType.Contains($_)} | % {
                    $propertyType = $propertyType.Replace($_,"List")
                }
                
                "`tvar $instanceName$propName = new $propertyType();`n"
                "`tfor(var i = 0; i<3;i++)`n`t{`n`t`tvar item = $refFactoryName();`n`t`t$instanceName$propName.Add(item);`n`t}`n`t$instanceName.$propName = $instanceName$propName;`n"
            }
        }

        $valueMembers.Keys | % {
            $propName = $_
            $propType = [NoFuture.Util.NfTypeName]::GetLastTypeNameFromArrayAndGeneric($valueMembers[$_].ToString())
            if($propType -eq "System.Byte"){
                "`t$instanceName.$propName = ByteFactory();`n"
            }
            elseif($propType -eq "System.Int16"){
                "`t$instanceName.$propName = Int16Factory();`n"
            }
            elseif($propType -eq "System.Int32"){
                "`t$instanceName.$propName = Int32Factory();`n"
            }
            elseif($propType -eq "System.Int64"){
                "`t$instanceName.$propName = Int64Factory();`n"
            }
            elseif($propType -eq "System.Double"){
                "`t$instanceName.$propName = DoubleFactory();`n"
            }
            elseif($propType -eq "System.Decimal"){
                "`t$instanceName.$propName = DecimalFactory();`n"
            }
            elseif($propType -eq "System.DateTime"){
                "`t$instanceName.$propName = DateTimeFactory();`n"
            }
            elseif($propType -eq "System.Boolean"){
                "`t$instanceName.$propName = BooleanFactory();`n"
            }
            elseif($propType -eq "System.Char"){
                "`t$instanceName.$propName = CharFactory();`n"
            }
            elseif($propType -eq "System.String"){
                "`t$instanceName.$propName = StringFactory();`n"
                $needsStringConstDefined = $true
            }
            elseif($propType -eq "System.Guid"){
                "`t$instanceName.$propName = GuidFactory();`n"
            }
        }

        $normalMembers.Keys | % {
            $propName = $_;
            $propType = $normalMembers[$_].ToString()

            if($normalMembers[$_].BaseType.ToString() -eq "System.Enum"){
                $randEnumSyntax = ""

                #get the enums names
                $enumNames = [System.Enum]::GetNames($propType)
                $randEnumSyntax += "`tvar enum$PropName = new []{`n"

                #these are often inner types
                $propTypeToDot = $propType.Replace("+",".")
                $enumNames | % {
                    $enumName = $_
                    $randEnumSyntax += "`t`t$propTypeToDot.$enumName,`n"
                }

                #its possiable to have an enum with just one name...
                $randEnumLength = $enumNames.Length
                if($randEnumLength -le 1) {
                    $randEnumSyntax += "`t};`n`t$instanceName.$propName = enum$PropName[0];`n"
                }
                else{
                    $randEnumSyntax += "`t};`n`tvar randEnum$PropName = _myrand.Next(0,(enum$PropName.Length - 1));`n"
                    $randEnumSyntax += "`t$instanceName.$propName = enum$PropName[randEnum$PropName];`n"
                }

                #print the enum syntax
                $randEnumSyntax
            }
            else{
                $recursiveCallsTo += $propType

                #check that this isn't going to set a infinite recursion into motion...
                if(-not ($Global:CodeGenAssignRandomValuesCallStack.Contains($propType))){
                    $refFactoryName = "{0}Factory" -f ([NoFuture.Util.NfTypeName]::GetTypeNameWithoutNamespace($propType))
                    "`t$instanceName.$propName = $refFactoryName();`n"
                }
            }

        }

        "`treturn $instanceName;`n}`n"
        
        if($Recursion -and $recursiveCallsTo.Length -gt 0){
            $recursiveCallsTo | ? {-not([System.String]::IsNullOrWhiteSpace($_)) -and (-not ($Global:CodeGenAssignRandomValuesCompletedType -contains $_))} | % {
                
                $Global:CodeGenAssignRandomValuesCallStack.Push($_)

                Write-CsCodeAssignRand -Assembly $Assembly -TypeFullName $_ -FromSelf -Recursion

                $doNotDisplay = $Global:CodeGenAssignRandomValuesCallStack.Pop()
            }
        }
    }
}#end Write-CsCodeAssignRand

<#
    .SYNOPSIS
    Flattens an object graph to having only properties of value types.
   
    .DESCRIPTION
    Given some assembly and and a fully-qualified type's name
    the cmdlet will create the custom Nf FlattenedType.

    Note, strings are supposed as value types even though they aren't.

    The cmdlet only works to flatten the other types contained
    in the same assembly and does not check type-names against all
    the assemblies currently loaded in the AppDomain.

    It has a max depth of 16.

    .PARAMETER Assembly
    The assembly with the target type to be flattend is contained.

    .PARAMETER TypeFullName
    The fully-qualified types name with the given assembly arg.

    .PARAMETER LimitOnPrimitive
    This will limit the search to a specific, common .NET, kind of value type
    (e.g. System.DateTime).

    .OUTPUTS
    string
#>
function Get-FlattenedType
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [System.Reflection.Assembly] $Assembly,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeFullName,
        [Parameter(Mandatory=$false,position=2)]
        [string] $LimitOnPrimitive
    )
    Process
    {
        if(-not [string]::IsNullOrWhiteSpace($LimitOnPrimitive)){
            $isValidPrimitive = [NoFuture.Util.Gia.FlattenedItem]::ValueTypesList -contains $LimitOnPrimitive

            if(-not $isValidPrimitive){
                Write-Host "The LimitOnPrimitive must be a one of the key's values set in NoFuture.Util.Gia.FlattenedItem.ValueTypesList." -ForegroundColor Yellow
                break;
            }
        }
        $ftaArg = New-Object NoFuture.Util.Gia.Args.FlattenTypeArgs -Property @{
                                 Assembly = $Assembly; 
                                 Depth = 16; 
                                 Separator = $Separator; 
                                 TypeFullName = $TypeFullName; 
                                 UseTypeNames = $false;
                                 LimitOnThisType = $LimitOnPrimitive}

        return ([NoFuture.Util.Gia.Flatten]::FlattenType($ftaArg))
        
    }#end Process
}#end Get-FlattenedType

<#
    .SYNOPSIS
    Creates and displays an assembly type graph.
   
    .DESCRIPTION
    The InvokeFlatten.exe can take some time to complete depending
    on the complexity of the assembly's object graph.

    .PARAMETER AssemblyPath
    The path to the assembly to flatten

    .PARAMETER RegexPatterns
    Optional, will center the graph around 
    types by these names (and their property types)
    while excluding the rest.

#>
function Get-DotGraphAssemblyDiagram
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath,
        [Parameter(Mandatory=$false,position=1)]
        [switch] $OutlineNamespaces

    )
    Process
    {
        #validate input
        $graphDir = ([NoFuture.TempDirectories]::Graph)

        if([string]::IsNullOrWhiteSpace($graphDir)){
            Write-Host "Assign a directory to the global NoFuture.TempDirectories.Graph variable" -ForegroundColor Yellow
            break;
        }
        if(-not (Test-Path $AssemblyPath)){
            Write-Host "There isn't an assembly at $AssemblyPath" -ForegroundColor Yellow
            break;
        }
        Write-Progress -Activity "Starting..." -Status "Working" -PercentComplete 11

        $asmName = [System.Reflection.AssemblyName]::GetAssemblyName($AssemblyPath)
        Write-Progress -Activity ("Getting assembly '{0}' as diagram" -f $asmName.Name)  -Status "Working" -PercentComplete 50

        if($OutlineNamespaces){
            $fileOutPath = [NoFuture.Gen.Etc]::RunIsolatedAsmDiagram($AssemblyPath,$true)
        }
        else{
            $fileOutPath = [NoFuture.Gen.Etc]::RunIsolatedAsmDiagram($AssemblyPath)
        }

        Write-Progress -Activity ("Creating graph from {0}" -f $asmName.Name) -Status "Working" -PercentComplete 88

        $graphFile = Invoke-DotExe -GraphvizFile $fileOutPath

        #add a style sheet 
        $cssFile = Join-Path (Split-Path $graphFile -Parent) ([System.IO.Path]::GetFileNameWithoutExtension($graphFile) + ".css")

        $cssContent = @"
.edge:hover  > path, .edge:hover  > polygon {
	cursor: crosshair;
	stroke: deepskyblue;
	stroke-width: 2;
}

.node:hover {
	cursor: crosshair;
	fill: beige;
	stroke: deepskyblue;
	stroke-width: 2;
}        
"@

        [System.IO.File]::WriteAllText($cssFile, $cssContent)

        $graphvizContent = [System.IO.File]::ReadAllLines($graphFile)

        $introCss = @(
            "<?xml version=`"1.0`" encoding=`"UTF-8`" standalone=`"no`"?>",
            "<?xml-stylesheet type=`"text/css`" href=`"Bfw.DtoAsmDiagram.css`"?>"
            )
        $introCss += $graphvizContent[1..($graphvizContent.Length -1)]
        [System.IO.File]::WriteAllLines($graphFile,$introCss)

        #display the svg with whatever program is assoc. to that extension
        Invoke-Expression -Command "& $graphFile"
    }
}#Get-DotGraphAssemblyDiagram

<#
    .SYNOPSIS
    Creates and displays value-type trace graph.
   
    .DESCRIPTION
    Displays a flattened type in a graphical form

    .PARAMETER Assembly
    The assembly with the target type.

    .PARAMETER TypeFullName
    The fully-qualified types name within the given assembly arg.

    .OUTPUTS
    null
#>
function Get-DotGraphFlattenedType
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeFullName,
        [Parameter(Mandatory=$false,position=2)]
        [string] $LimitOnPrimitive,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $DisplayEnums
    )
    Process
    {
        #validate input
        $graphDir = ([NoFuture.TempDirectories]::Graph)

        if([string]::IsNullOrWhiteSpace($graphDir)){
            Write-Host "Assign a directory to the global NoFuture.TempDirectories.Graph variable" -ForegroundColor Yellow
            break;
        }

        Write-Progress -Activity "Flattening type '$TypeFullName'" -Status "Working" -PercentComplete 34
        $blnDisplayEnums = $false
        if($DisplayEnums){ $blnDisplayEnums = $true}
        #compose all .gv files
        $fileOutPath = [NoFuture.Gen.Etc]::RunIsolatedFlattenTypeDiagram($AssemblyPath,$TypeFullName, $LimitOnPrimitive, $blnDisplayEnums)

        Write-Progress -Activity "Creating graph from $flattenedType" -Status "Working" -PercentComplete 88

        $graphFile = Invoke-DotExe -GraphvizFile $fileOutPath
        
        #display the svg with whatever program is assoc. to that extension
        Invoke-Expression -Command "& $graphFile"
    }
}#end Get-DotGraphFlattenedType

<#
    .SYNOPSIS
    Creates and displays a UML'esque class diagram.
   
    .DESCRIPTION
    Given some assembly and and a fully-qualified type's name
    the cmdlet will create a Graph-Viz syntax file compile it 
    using dot.exe and, lastly, display it using the 
    default program from SVG files.

    .PARAMETER Assembly
    The assembly with the target type.

    .PARAMETER TypeFullName
    The fully-qualified types name within the given assembly arg.

    .OUTPUTS
    null
#>
function Get-DotGraphClassDiagram
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeFullName
    )
    Process
    {
        #validate input
        $graphDir = ([NoFuture.TempDirectories]::Graph)

        if([string]::IsNullOrWhiteSpace($graphDir)){
            Write-Host "Assign a directory to the global NoFuture.TempDirectories.Graph variable" -ForegroundColor Yellow
            break;
        }

        Write-Progress -Activity "Getting type '$TypeFullName'" -Status "Working" -PercentComplete 34

        #compose all .gv files

        $fileOutPath = [NoFuture.Gen.Etc]::RunIsolatedGetClassDiagram($AssemblyPath,$TypeFullName)

        Write-Progress -Activity "Creating graph for $TypeFullName" -Status "Working" -PercentComplete 88

        $graphFile = Invoke-DotExe -GraphvizFile $fileOutPath
        
        #display the svg with whatever program is assoc. to that extension
        Invoke-Expression -Command "& $graphFile"
    }
}#end Get-DotGraphClassDiagram

<#
    .SYNOPSIS
    Invokes graphviz-2.38 Dot.exe.
   
    .DESCRIPTION
    Invokes Dot.exe with the given values and determines the image
    output type based on the extension specified in OutputFile.
    When OutputFile is omitted then the image type defaults to SVG.

    .PARAMETER GraphvizFile
    The graphviz-2.38 syntax file.

    .PARAMETER OutputFile
    Optional, will resolve to the input file's name as an SVG 

    .OUTPUTS
    the file path to the generated image.
#>
function Invoke-DotExe
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $GraphvizFile,
        [Parameter(Mandatory=$false,position=1)]
        [string] $OutputFile
    )
    Process
    {
        
        if([string]::IsNullOrWhiteSpace($GraphvizFile) -or -not (Test-Path $GraphvizFile)){
            Write-Host "The file path '$GraphvizFile' is invalid."
            break;
        }
        
        $dotExe = ([NoFuture.Tools.X86]::DotExe)
        if(-not (Test-Path $dotExe)){
            Write-Host "The dot.exe is not located at $dotExe" -ForegroundColor Yellow
            break;
        }


        if([string]::IsNullOrWhiteSpace($OutputFile)){
            $OutputFile = Join-Path (Split-Path -Path $GraphvizFile -Parent) ([System.IO.Path]::GetFileNameWithoutExtension($GraphvizFile) + ".svg")
        }
        elseif(-not([System.IO.Path]::IsPathRooted($OutputFile))){
            if(-not $OutputFile.Contains([System.IO.Path]::DirectorySeparatorChar)){
                $OutputFile = Join-Path ([NoFuture.TempDirectories]::Graph) $OutputFile
            }
        }
        
        $dotExeImageFormats = @(

            "bmp", "canon", "cmap", "cmapx", "cmapx_np", "dot",
            "emf", "emfplus", "eps", "fig", "gd", "gd2", "gif",
            "gv", "imap", "imap_np", "ismap", "jpe", "jpeg", "jpg",
            "metafile", "pdf", "pic", "plain", "plain-ext", "png",
            "pov", "ps", "ps2", "svg", "svgz", "tif", "tiff", "tk",
            "vml", "vmlz", "vrml", "wbmp", "xdot", "xdot1.2", "xdot1.4"
        )

        $imgFormat = ([System.IO.Path]::GetExtension($OutputFile)).Replace(".","").ToLower();

        if($dotExeImageFormats -notcontains $imgFormat){
            $printFormats = [string]::Join(", ",$dotExeImageFormats)
            Write-Host "The '$imgFormat' is not a valid dot.exe image format choose from [$printFormats]"
            break;
        }

        Invoke-Expression -Command "& $dotExe -T$imgFormat $GraphvizFile -o $OutputFile"
        return $OutputFile
    }
}


#---------
# code re-write cmdlets
#--------

<#
    .SYNOPSIS
    Gets the Pdb Data for a single type.

    .DESCRIPTION
    Invokes the modified Dia2Dump.exe passing in
    the type's full name.

    .PARAMETER AssemblyPath
    The path to a dll whose source code files may be 
    found according to the side-by-side pdb file.

    .PARAMETER TypeFullName
    The full type's name as in namespace and class name.

    .OUTPUTS
    NoFuture.Shared.DiaSdk.LinesSwitch.PdbCompiland

#>
function Get-TypePdbLines
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeFullName
    )
    Process
    {
        $invokeDia2Dump = New-Object NoFuture.Gen.InvokeDia2Dump.GetPdbData($AssemblyPath)
        $pdbCompiland = $invokeDia2Dump.SingleTypeNamed($TypeFullName)
        return $pdbCompiland
    }
}

