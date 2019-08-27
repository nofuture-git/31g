$myScriptLocation = Split-Path $PSCommandPath -Parent
$myScriptLocation = Resolve-Path $myScriptLocation

$dependencies = @{
    "Antlr4.Runtime, Version=4.6.0.0, Culture=neutral, PublicKeyToken=09abb75b9ed49849" = (Join-Path $myScriptLocation "Antlr4.Runtime.dll");
    "NoFuture.Antlr.HTML, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Antlr.HTML.dll");
    "NoFuture.Antlr.DotNetIlTypeName, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Antlr.DotNetIlTypeName.dll");
    "NoFuture.Antlr.CSharp4, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Antlr.CSharp4.dll");
    "NoFuture.Shared.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.Core.dll");
    "NoFuture.Shared.Cfg, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.Cfg.dll");
    "NoFuture.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Shared.dll");
    "NoFuture.Util.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Util.Core.dll");
    "NoFuture.Util.Binary, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Util.Binary.dll");
    "NoFuture.Util.DotNetMeta, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Util.DotNetMeta.dll");
    "NoFuture.Util.Gia, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Util.Gia.dll");
    "NoFuture.Util.NfConsole, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Util.NfConsole.dll");
    "NoFuture.Util.NfType, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Util.NfType.dll");
    "NoFuture.Tokens, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Tokens.dll");
    "NoFuture.Gen, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Gen.dll");
    "NoFuture.Gen.InvokeDia2Dump, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Gen.InvokeDia2Dump.dll");
    "NoFuture.Gen.InvokeGetCgOfType, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Gen.InvokeGetCgOfType.exe");
    "NoFuture.Gen.InvokeGraphViz, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" = (Join-Path $myScriptLocation "NoFuture.Gen.InvokeGraphViz.exe");
}

$loadedAsms = ([appdomain]::CurrentDomain.GetAssemblies() | % {$_.FullName}  | Sort-Object -Unique)
$dependencies.Keys | % {
    if($loadedAsms -notcontains $_)
    {
        $binDll = $dependencies.$_
        if(Test-Path $binDll)
        {
            $stdout = [System.Reflection.Assembly]::LoadFile($binDll)
        }
        else
        {
            Write-Host ("'{0}' was not found at '{1}'; some functions will not be available." -f $_, $binDll) -ForegroundColor Yellow
        }
    }
}


#=====================================
#intialize paths from nfConfig.cfg.xml
$dnd = [NoFuture.Shared.Cfg.NfConfig]::Init(([NoFuture.Shared.Cfg.NfConfig]::FindNfConfigFile($myScriptLocation)))
#=====================================

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
    When omitted, the file will be placed into NoFuture.Shared.Core.NfConfig+TempDirectories.Root

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
        
        $tempDir = [NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Root
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
        $TypeName = [NoFuture.Util.Core.NfString]::CapWords($Name,([char]".")).Replace(".","")
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
            $propName = [NoFuture.Util.Core.NfString]::CapWords($_,([char]"_")).Replace("_","") ;
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
        [switch] $RandMethods,
        [Parameter(Mandatory=$false,position=3)]
        [switch] $Recursion,
        [Parameter(Mandatory=$false,position=4)]
        [switch] $ValueTypesOnly,
        [Parameter(Mandatory=$false,position=5)]
        [switch] $FromSelf
    )
    Process
    {
        if($Assembly.GetType($TypeFullName) -eq $null){return;}

        $recursiveCallsTo = @()

        $typeName = [NoFuture.Util.Core.NfReflect]::GetTypeNameWithoutNamespace($TypeFullName)

        $myrand = New-Object System.Random ([Int32][String]::Format("{0:fffffff}",$(get-date)))

        $instanceName = "subject";
        $codeGenAttr = ("[System.CodeDom.Compiler.GeneratedCode(`"Write-CsCodeAssignRand`",`"{0:yyyy.M.dd.HHmmss}`")]" -f $(Get-Date))
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
        $codeGenAttr
        internal string StringFactory()
        {
            return StringFactory(STR_START,STR_END,STR_LEN);
        }
        $codeGenAttr
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
        $codeGenAttr
        internal System.DateTime DateTimeFactory()
        {
            var oldestyear = System.DateTime.Now.Year - 1;
            var youngestyear = System.DateTime.Now.Year + 1;

            return new System.DateTime(_myrand.Next(oldestyear, youngestyear), _myrand.Next(1, 12), _myrand.Next(1, 28));
        }
        $codeGenAttr
        internal byte ByteFactory()
        {
            return System.Convert.ToByte(_myrand.Next(0, byte.MaxValue));
        }
        $codeGenAttr
        internal System.Int16 Int16Factory()
        {
            return System.Convert.ToInt16(_myrand.Next(0, short.MaxValue));
        }
        $codeGenAttr
        internal System.Int32 Int32Factory()
        {
            return _myrand.Next(0, int.MaxValue);
        }
        $codeGenAttr
        internal System.Int64 Int64Factory()
        {
            return System.Convert.ToInt64(_myrand.Next(0, int.MaxValue));
        }
        $codeGenAttr
        internal System.Decimal DecimalFactory()
        {
            return System.Convert.ToDecimal(_myrand.Next(0, int.MaxValue));
        }
        $codeGenAttr
        internal System.Double DoubleFactory()
        {
            return System.Convert.ToDouble(_myrand.Next(0, int.MaxValue));
        }
        $codeGenAttr
        internal char CharFactory()
        {
            return System.Convert.ToChar(StringFactory(0x41, 0x5A, 1));
        }
        $codeGenAttr
        internal System.Single SingleFactory()
        {
            return System.Convert.ToSingle(_myrand.Next(0, int.MaxValue));
        }
        $codeGenAttr
        internal bool BooleanFactory()
        {
            if (_myrand.Next(1, 999) % 2 == 0)
            {
                return true;
            }
            return false;
        }
        $codeGenAttr
        internal System.Guid GuidFactory()
        {
            return System.Guid.NewGuid();
        }
        $codeGenAttr
        internal System.Type TypeFactory()
        {
            return typeof(System.String);
        }
        $codeGenAttr
        internal System.Object ObjectFactory()
        {
            return new Object();
        }
        $codeGenAttr
        internal void SetProperty<T, TV>(T target, string propertyName, TV value) where T : class
        {
            if(target == null)
                return;

            var pi = target.GetType().GetProperty(propertyName);
            if (pi == null)
                return;
            pi.SetValue(target, value);
        }
        
"@
            }#end if RandMethods
        }
        $Global:CodeGenAssignRandomValuesCompletedType += $TypeFullName;
        $factoryName = "{0}Factory" -f $typeName

        $genType = $Assembly.GetType($TypeFullName)
        $hasNoArgCtor = ($genType.GetConstructors() | % {$_.GetParameters().Length -eq 0}) -contains $true
        if(-not $hasNoArgCtor){
            #take the one with the least number of args
            $pickCtor = $genType.GetConstructors() | ? {$_.GetParameters().Length -gt 1} | Sort-Object | Select-Object -First 1
            #have a factory method called 
            if($pickCtor -ne $null){
                $ctorArgs = "(" + [string]::Join(", ", ($pickCtor.GetParameters() | % { $_.ParameterType.Name + "Factory()"}) ) + ")"
            }
            else{
                $ctorArgs = "()"
            }
            
        }
        else{
            $ctorArgs = "()"
        }
        $codeGenAttr
        "public $TypeFullName $factoryName()`n{`n"
        #need to find concrete implemenations 
        if($genType.IsAbstract){
            $concreteOfGenType = $Assembly.GetTypes() | ? {$_.BaseType -ne $null -and $_.BaseType.FullName -eq $TypeFullName -and -not $_.IsAbstract} | % { $_.FullName }
            if($concreteOfGenType -ne $null){
                if($concreteOfGenType -is [string]){
                    ("`tvar $instanceName = new {0}();`n" -f $concreteOfGenType)
                }
                elseif($concreteOfGenType.Length -eq 2){
                    ("`tvar $instanceName = BooleanFactory()`n`t`t? ({0})new {1}()`n`t`t: new {2}();`n" -f $TypeFullName, $concreteOfGenType[0], $concreteOfGenType[1])
                }
                else{
                    "`t$TypeFullName $instanceName = null;`n"
                    ("`tvar rolled = _myrand.Next(0,{0});`n" -f ($concreteOfGenType.Length-1))
                    "`tswitch (rolled)`n`t{`n"

                    for($m=0; $m -lt $concreteOfGenType.Length; $m++){
                        $cogtAtM = $concreteOfGenType[$m]
                        if($m -eq $concreteOfGenType.Length-1){
                            "`t`tdefault:`n`t`t`t$instanceName = new $cogtAtM();`n`t`t`tbreak;`n"
                        }
                        else{
                            "`t`tcase ${m}:`n`t`t`t$instanceName = new $cogtAtM();`n`t`t`tbreak;`n"
                        }
                    }
                    "`t}`n"
                }
            }
        }
        else{
            "`tvar $instanceName = new $TypeFullName$ctorArgs;`n"
        }
        "`t_depth += 1;`n`tif(_depth > MAX_DEPTH){return $instanceName;}`n"

        #get members as name-type pairs
        $listMembers = @{}
        $valueMembers = @{}
        $normalMembers = @{}

        $genType.GetProperties() | % {
            $piSetMethod = $_.SetMethod
            if($_.CanWrite -and  ([NoFuture.Util.Core.NfReflect]::IsEnumerableReturnType($_.PropertyType))){

                if($piSetMethod -ne $null -and -not $piSetMethod.IsPublic){
                    $listMembers.Add(("{0}*" -f $_.Name),$_.PropertyType)
                }
                else{
                    $listMembers.Add($_.Name,$_.PropertyType)
                }
                
            }
            elseif($_.CanWrite -and ([NoFuture.Util.Core.NfReflect]::IsValueTypeProperty($_))){
                
                if($piSetMethod -ne $null -and -not $piSetMethod.IsPublic){
                    $valueMembers.Add(("{0}*" -f $_.Name),$_.PropertyType)
                }
                else{
                    $valueMembers.Add($_.Name,$_.PropertyType)
                }
            }
            elseif($_.CanWrite -and -not ([NoFuture.Util.Core.NfReflect]::IsValueTypeProperty($_)) -and `
                   -not ([NoFuture.Util.Core.NfReflect]::IsEnumerableReturnType($_.PropertyType))){

                if($piSetMethod -ne $null -and -not $piSetMethod.IsPublic){
                    $normalMembers.Add(("{0}*" -f $_.Name),$_.PropertyType)
                }
                else{
                    $normalMembers.Add($_.Name,$_.PropertyType)
                }
                
            }
            
        }
        $genType.GetFields() | % {
            if($_.IsPublic -and  ([NoFuture.Util.Core.NfReflect]::IsEnumerableReturnType($_.FieldType))){
                $listMembers.Add($_.Name,$_.FieldType)
            }
            elseif($_.IsPublic -and ([NoFuture.Util.Core.NfReflect]::IsValueTypeField($_))){
                $valueMembers.Add($_.Name,$_.FieldType)
            }
            elseif($_.IsPublic -and -not ($_.IsLiteral) -and -not ([NoFuture.Util.Core.NfReflect]::IsValueTypeField($_)) -and `
                    -not ([NoFuture.Util.Core.NfReflect]::IsEnumerableReturnType($_.FieldType))){
                $normalMembers.Add($_.Name,$_.FieldType)
            }
            
        }

        $normalMembers.Keys | % {
            $propName = $_;
            $isNotAssignable = $propName.EndsWith("*")
            if($isNotAssignable){
                $propName = $propName.Replace("*","")
            }
            $propType = $normalMembers[$_].ToString()

            if($normalMembers[$_].BaseType -ne $null -and $normalMembers[$_].BaseType.ToString() -eq "System.Enum"){
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
                    if($isNotAssignable){
                        $randEnumSyntax += "`t};`n`tSetProperty($instanceName, `"$propName`", enum$PropName[0]);`n"
                    }
                    else{
                        $randEnumSyntax += "`t};`n`t$instanceName.$propName = enum$PropName[0];`n"
                    }
                    
                }
                else{
                    $randEnumSyntax += "`t};`n`tvar randEnum$PropName = _myrand.Next(0,(enum$PropName.Length - 1));`n"
                    if($isNotAssignable){
                        $randEnumSyntax += "`tSetProperty($instanceName `"$propName`", enum$PropName[randEnum$PropName]);`n"
                    }
                    else{
                        $randEnumSyntax += "`t$instanceName.$propName = enum$PropName[randEnum$PropName];`n"
                    }
                    
                }

                #print the enum syntax
                $randEnumSyntax
            }
            else{
                $recursiveCallsTo += $propType

                #check that this isn't going to set a infinite recursion into motion...
                if(-not $ValueTypesOnly -and -not ($Global:CodeGenAssignRandomValuesCallStack.Contains($propType))){
                    $refFactoryName = "{0}Factory" -f ([NoFuture.Util.Core.NfReflect]::GetTypeNameWithoutNamespace($propType))
                    if($isNotAssignable){
                        "`tSetProperty($instanceName, `"$propName`", $refFactoryName());`n"
                    }
                    else{
                        "`t$instanceName.$propName = $refFactoryName();`n"
                    }
                    
                }
            }

        }

        $valueMembers.Keys | % {
            $propName = $_
            $isNotAssignable = $propName.EndsWith("*")
            if($isNotAssignable){
                $propName = $propName.Replace("*","")
            }
            $propType = [NoFuture.Util.Core.NfReflect]::GetLastTypeNameFromArrayAndGeneric($valueMembers[$_].ToString())
            if($propType -eq "System.Byte"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",ByteFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = ByteFactory();`n"
                }
            }
            elseif($propType -eq "System.Int16"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",Int16Factory());`n"
                }
                else{
                    "`t$instanceName.$propName = Int16Factory();`n"
                }
            }
            elseif($propType -eq "System.Int32"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",Int32Factory());`n"
                }
                else{
                    "`t$instanceName.$propName = Int32Factory();`n"
                }
            }
            elseif($propType -eq "System.Int64"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",Int64Factory());`n"
                }
                else{
                    "`t$instanceName.$propName = Int64Factory();`n"
                }
            }
            elseif($propType -eq "System.Double"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",DoubleFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = DoubleFactory();`n"
                }
            }
            elseif($propType -eq "System.Decimal"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",DecimalFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = DecimalFactory();`n"
                }
            }
            elseif($propType -eq "System.DateTime"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",DateTimeFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = DateTimeFactory();`n"
                }
            }
            elseif($propType -eq "System.Boolean"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",BooleanFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = BooleanFactory();`n"
                }
            }
            elseif($propType -eq "System.Char"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",CharFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = CharFactory();`n"
                }
            }
            elseif($propType -eq "System.String"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",StringFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = StringFactory();`n"
                }
                $needsStringConstDefined = $true
            }
            elseif($propType -eq "System.Single"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",SingleFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = SingleFactory();`n"
                }
                $needsStringConstDefined = $true
            }
            elseif($propType -eq "System.Guid"){
                if($isNotAssignable){
                    "`tSetProperty($instanceName,`"$propName`",GuidFactory());`n"
                }
                else{
                    "`t$instanceName.$propName = GuidFactory();`n"
                }
            }
        }

        $listMembers.Keys | % {
            $memberName = $_
            $isNotAssignable = $memberName.EndsWith("*")
            if($isNotAssignable){
                $memberName = $memberName.Replace("*","")
            }
            $memberType = $listMembers[$_]
            $enumerableType = [NoFuture.Util.Core.NfReflect]::GetLastTypeNameFromArrayAndGeneric($memberType)
            $recursiveCallsTo += $enumerableType
            $propertyType = $memberType.ToString()
            $refFactoryName = "{0}Factory" -f  ([NoFuture.Util.Core.NfReflect]::GetTypeNameWithoutNamespace($enumerableType))
            $propName = $memberName;

            if(-not $ValueTypesOnly){
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
                    @("ISet") | ? {$propertyType.Contains($_)} | % {
                        $propertyType = $propertyType.Replace($_,"HashSet")
                    }
                
                    "`tvar $instanceName$propName = new $propertyType();`n"
                    "`tfor(var i = 0; i<3;i++)`n`t{`n`t`tvar item = $refFactoryName();`n`t`t$instanceName$propName.Add(item);`n`t}`n"

                    if($isNotAssignable){
                        "`tSetProperty($instanceName, `"$propName`", $instanceName$propName);`n"
                    }
                    else{
                        "`t$instanceName.$propName = $instanceName$propName;`n"
                    }
                }
            }
        }

        "`treturn $instanceName;`n}`n"
        
        if($Recursion -and $recursiveCallsTo.Length -gt 0){
            $recursiveCallsTo | ? {-not([System.String]::IsNullOrWhiteSpace($_)) -and (-not ($Global:CodeGenAssignRandomValuesCompletedType -contains $_))} | % {
                
                $Global:CodeGenAssignRandomValuesCallStack.Push($_)

                if($ValueTypesOnly){
                    Write-CsCodeAssignRand -Assembly $Assembly -TypeFullName $_ -FromSelf -Recursion -ValueTypesOnly
                }
                else{
                    Write-CsCodeAssignRand -Assembly $Assembly -TypeFullName $_ -FromSelf -Recursion
                }

                $doNotDisplay = $Global:CodeGenAssignRandomValuesCallStack.Pop()
            }
        }
    }
}

<#
    .SYNOPSIS
    Both invokes Write-CsCodeAssignRand and Add-Type for
    random type factory.
   
    .DESCRIPTION
    A composition cmdlet which handles the generating random
    code factory (as .cs code), writes it to file and then
    adds it to this appdomain using Add-Type

    This is expected to be invoked from some build's \bin directory
    since that folder will contain all the targeted assemblies dependencies

    .PARAMETER AssemblyFullPath
    The full file-path to the assembly from which contain the type.

    .PARAMETER TypeFullName
    The full name (namespace plus typename) for the 
    type within the source assembly.

#>
function Add-CsCodeAssignRand
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $AssemblyFullPath,
        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeFullName
    )
    Process
    {
        if(-not (Test-Path $AssemblyFullPath)){
            throw "cannot find and assembly at $AssemblyFullPath"
            break;
        }

        #get generated code for the given type\assembly
        $asm = [NoFuture.Util.Binary.Asm]::NfLoadFrom($AssemblyFullPath)
        $genCode = Write-CsCodeAssignRand -Assembly $asm -TypeFullName $TypeFullName -RandMethods -Recursion

        if($genCode -eq $null -or $genCode.Count -eq 0){
            throw "Write-CsCodeAssignRand generated nothing for type $TypeFullName"
            break;
        }

        #compose the gen'ed code into a full .NET C# class
        $genCode = [string]::Join([System.Environment]::NewLine, $genCode)
        $className = "CodeGen{0}" -f [NoFuture.Util.Core.NfString]::SafeDotNetIdentifier($TypeFullName)
        $fullClassCodeGen = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Temp.Code
{
    public class $className
    {
$genCode
    }
}
"@

        #write the full class to file
        $tempCodeDir = [NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Code
        if(-not (Test-Path $tempCodeDir)){
            $tempCodeDir = [NoFuture.Shared.Core.NfSettings]::AppData
        }
        $csCodeFile = Join-Path $tempCodeDir "$className.cs"
        [System.IO.File]::WriteAllText($csCodeFile, $fullClassCodeGen)

        #use ps builtin Add-Type to compile and add full .cs class file
        $assemblyFolder = Split-Path -Path $AssemblyFullPath -Parent
        $refs = ls -Path $assemblyFolder | ? {$_.Extension -eq ".dll"} | % {$_.FullName}
        Add-Type -Path $csCodeFile -ReferencedAssemblies $refs
    }
}

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
        $graphDir = ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Graph)

        if([string]::IsNullOrWhiteSpace($graphDir)){
            Write-Host "Assign a directory to the global NoFuture.Shared.Core.NfConfig+TempDirectories.Graph variable" -ForegroundColor Yellow
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

        $graphSz = (Get-Item $fileOutPath).Length

        if($graphSz -gt 65KB){
            throw "Graph is too big at $graphSz - limit is 65KB"
            break;
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

    .PARAMETER MaxDepth
    Optional, the maximum recursive depth - default is 16.

    .PARAMETER LimitOnPrimitive
    Optional, to force the flatten to only terminate on 
    primitive types by this name.

    .PARAMETER DisplayEnums
    Optional, causes those terminal nodes which are Enums
    to have their members and values printed rather than 
    just their type's name.

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
        [int] $MaxDepth,
        [Parameter(Mandatory=$false,position=3)]
        [string] $LimitOnPrimitive,
        [Parameter(Mandatory=$false,position=4)]
        [switch] $DisplayEnums
    )
    Process
    {
        #validate input
        $graphDir = ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Graph)

        if([string]::IsNullOrWhiteSpace($graphDir)){
            Write-Host "Assign a directory to the global NoFuture.Shared.Core.NfConfig+TempDirectories.Graph variable" -ForegroundColor Yellow
            break;
        }

        Write-Progress -Activity "Flattening type '$TypeFullName'" -Status "Working" -PercentComplete 34
        $blnDisplayEnums = $false
        if($DisplayEnums){ $blnDisplayEnums = $true}
        #compose all .gv files
        if($MaxDepth -le 0){$MaxDepth = [NoFuture.Util.Gia.Args.FlattenLineArgs]::MAX_DEPTH}
        $fileOutPath = [NoFuture.Gen.Etc]::RunIsolatedFlattenTypeDiagram($AssemblyPath, `
                                                                         $TypeFullName, `
                                                                         $LimitOnPrimitive, `
                                                                         $blnDisplayEnums, `
                                                                         $MaxDepth)

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
        [string] $TypeFullName,
        [Parameter(Mandatory=$false,position=2)]
        [switch] $DisplayEnums
    )
    Process
    {
        #validate input
        $graphDir = ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Graph)

        if([string]::IsNullOrWhiteSpace($graphDir)){
            Write-Host "Assign a directory to the global NoFuture.Shared.Core.NfConfig+TempDirectories.Graph variable" -ForegroundColor Yellow
            break;
        }
        $blnDisplayEnums = $false
        if($DisplayEnums){ $blnDisplayEnums = $true}
        Write-Progress -Activity "Getting type '$TypeFullName'" -Status "Working" -PercentComplete 34

        #compose all .gv files

        $fileOutPath = [NoFuture.Gen.Etc]::RunIsolatedGetClassDiagram($AssemblyPath,$TypeFullName, $blnDisplayEnums)

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
        
        $dotExe = ([NoFuture.Shared.Cfg.NfConfig+X86]::DotExe)
        if(-not (Test-Path $dotExe)){
            Write-Host "The dot.exe is not located at $dotExe" -ForegroundColor Yellow
            break;
        }


        if([string]::IsNullOrWhiteSpace($OutputFile)){
            $OutputFile = Join-Path (Split-Path -Path $GraphvizFile -Parent) ([System.IO.Path]::GetFileNameWithoutExtension($GraphvizFile) + ".svg")
        }
        elseif(-not([System.IO.Path]::IsPathRooted($OutputFile))){
            if(-not $OutputFile.Contains([System.IO.Path]::DirectorySeparatorChar)){
                $OutputFile = Join-Path ([NoFuture.Shared.Cfg.NfConfig+TempDirectories]::Graph) $OutputFile
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
