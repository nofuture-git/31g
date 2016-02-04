try{
if(-not [NoFuture.MyFunctions]::FunctionFiles.ContainsValue($MyInvocation.MyCommand))
{
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-BinaryDump",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Add-JavaType",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("New-JavaObject",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Import-CustomTypeIntoJre",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Invoke-JavaCompiler",$MyInvocation.MyCommand)
[NoFuture.MyFunctions]::FunctionFiles.Add("Get-JavaClassPath",$MyInvocation.MyCommand)
}
}catch{
    Write-Host "file is being loaded independent of 'start.ps1' - some functions may not be available."
}

<#
    .SYNOPSIS
    Calls 'dumpbin.exe' for the given assembly
    
    .DESCRIPTION
    Powershell wrapper for command line calls
    to 'dumpbin.exe'.
    
    .PARAMETER Path
    Path to a valid Windows assembly.
    
    .PARAMETER DumpBinSwitches
    Supports all switches available to the dumpbin.exe

    .LINK
    http://msdn.microsoft.com/en-us/library/756as972%28v=vs.100%29.aspx
    
#>
function Get-BinaryDump
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,Position=0)]
        [string] $Path,
        [Parameter(Mandatory=$false,Position=1)]
        [array] $DumpBinSwitches
    )
    Process
    {
        #validate the path
        if(-not(Test-Path $Path)){Write-Host ("Bad path or file name at '{0}'" -f $Path) }
        if(-not(@(".dll",".exe",".lib") -contains ([System.IO.Path]::GetExtension($Path))))
        {
            Write-Host ("File is not binary type at '{0}'" -f $Path)
        }
        
        #test for dumpbin.exe being installed
        if(-not(Test-Path ([NoFuture.Tools.X64]::Dumpbin))){throw ("Expected to find 'dumpbin.exe' at '{0}'." -f ([NoFuture.Tools.X64]::Dumpbin)) }
        
        #construct command expression
        $dumpbin = ("`"{0}`"" -f ([NoFuture.Tools.X64]::Dumpbin))
        if($DumpBinSwitches -eq $null -or $DumpBinSwitches -eq "")
        {
            $parameterSwitch = " /EXPORTS"
        }
        else
        {
            $parameterSwitch = "/" + [string]::Join("/", $DumpBinSwitches)
        }
        $cmd = ("& {0} {1} '{2}'" -f $dumpbin, $parameterSwitch, $Path)
        Write-Host $cmd
        
        #run it
        Invoke-Expression -Command $cmd
    }
}


<#
    .SYNOPSIS
    Adds a custom type the the running JRE.
    
    .DESCRIPTION
    Given a definition and valid Java syntax the 
    cmdlet will :
    1.) construct a source file thereof,  
    2.) push it to a .java file,
    3.) compile it to a .class,
    4.) zip it to a .jar file, 
    5.) load the .jar into the running JRE,
    6.) create a corrosponding .NET wrapper type
    7.) situate the .NET wrapper's ctor to create
        the said java type within the JRE

    The caller of the cmdlet will need to write 
    thier own implementations to any java methods 
    onto the .NET type.  Use .NET's 'SendToJre' 
    to facilitate sending syntax to to the JRE.

    .EXAMPLE
    C:\PS> Start-Jre
    C:\PS> $jrunscript.ImportPackage("javax.swing")
    C:\PS> Add-JavaType -TypeName "MyFrame" -Imports "javax.swing.*;" -Extends "JFrame" -TypeMembers @"
             public MyFrame(){
                setTitle("My Empty Frame");
                setSize(300,200);
                setLocation(10,200);
             }
"@
    C:\PS> $myFrame = New-JavaObject "MyFrame"
    C:\PS> $myFrame | Add-Member ScriptProperty "show" {  $this.SendToJre(("{0}.show()" -f $myFrame.JreRef)) }
    C:\PS> $myFrame.show

#>
function Add-JavaType
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$TypeName,

        [Parameter(Mandatory=$true,position=1)]
        [string]$TypeMembers, 

        [Parameter(Mandatory=$false,position=2)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Package,

        [Parameter(Mandatory=$false,position=3)]
        [AllowNull()]
        [string[]]$Imports,

        [Parameter(Mandatory=$false,position=4)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Extends, 

        [Parameter(Mandatory=$false,position=5)]
        [AllowNull()]
        [string[]]$Implements
    )
    Process
    {
        #insure the JRE is up
        if($global:jrunscript -eq $null){Start-Jre} #cmdlet performs 'importPackage(java.net);'

        #generate the java class declarations class
        $java = Write-JavaClass -Package $Package -Imports $Imports -TypeName $TypeName -TypeMembers $TypeMembers -Extends $Extends -Implements $Implements

        #write the class to file
        Write-TempJavaToFile $java $TypeName $Package

        #compile the java -> class -> jar
        $jarFile = Invoke-JavaCompiler $TypeName $Package

        #load the type dynamically into the running JRE
        $jreCtor = Import-CustomTypeIntoJre $jarFile $TypeName $Package
        
        #get a corrosponding dynamic type to create the counterpart in the jre
        $JrunscriptProcId = $global:jrunscript.Exe.Id
        $debugPath = (Join-Path ([NoFuture.TempDirectories]::Debug) ("{0}.out.txt" -f $TypeName))

        #draft the dynamic .NET type member definition
        $memberTypeDef = Write-JavaTypeMemberDef (Write-CustomJavaTypeCtorSyntax $TypeName $jreCtor $JrunscriptProcId $debugPath)

        #put it all together into a dynamic type loaded into this powershell session
        if($Package -eq $null -or $Package -eq "")
        { 
            Add-Type -Namespace ("{0}" -f ([NoFuture.Shared.Constants]::AddJavaTypeRootPackage)) -Name $TypeName -MemberDefinition $memberTypeDef
        }
        else
        {
            Add-Type -Namespace ("{0}.{1}" -f ([NoFuture.Shared.Constants]::AddJavaTypeRootPackage),$Package.Replace(";","")) -Name $TypeName -MemberDefinition $memberTypeDef
        }

        #caller needs to implement thier own scriptMethods to marshal their java type to .NET type
    }
}

<#
    .SYNOPSIS
    Instantiates the Java type in the running instance of 
    this ps sessions JRE.

    .DESCRIPTION
    Given a valid Java type, who has had all needed packages
    and classes imported into the running JRE, this cmdlet will
    create a new instance of the class within the JRE and, in turn,
    create a dynamic .NET wrapper class to contain the Java type.

    The cmdlet uses the open socket on the running JRE.  Prior to 
    creating the .NET type wrapper the cmdlet will sift through the 
    loaded assemblies of this ps session specifically on namespaces
    of NoFuture.Constants.AddJavaTypeRootPackage and finding a type
    match will instantiate it.  Otherwise the cmdlet will create the 
    .NET wrapper type itself adding it to the running app domain.

    .PARAMETER Class
    The right side of a Java instantiation statement.  Cmdlet attempts
    to parse the syntax taking what is needed.

    .EXAMPLE
    C:\PS> Start-Jre
    C:\PS> $jrunscript.ImportPackage("javax.swing")
    C:\PS> $myFrame = New-JavaObject 'javax.swing.JFrame("My JFrame Title")'
    C:\PS> $myFrame | Add-Member ScriptProperty "show" {  $this.SendToJre(("{0}.show()" -f $myFrame.JreRef)) }
    C:\PS> $myFrame.show

#>
function New-JavaObject
{
    [CmdletBinding()]
    Param
    (
        [string]$Class
    )
    Process
    {
        #validate input
        if($Class -eq $null -or $Class -eq "") {throw "the class type must be specified"}
        
        #break the string into logical parts
        $jCtor = [NoFuture.Tokens.Ctor]::DisassembleConstructor($Class)

        #validate jrunscript is running
        if($global:jrunscript -eq $null){Start-Jre}


        #check if class is an instance of a custom java type added in Add-JavaType
        $myJavaTypes = ([AppDomain]::CurrentDomain.GetAssemblies() | ? {$global:coreAssemblies -notcontains $_.FullName} | % {
            $_.GetTypes() | ? {
                $_ -ne $null -and
                $_.Namespace -ne $null -and
                $_.Namespace.StartsWith(([NoFuture.Shared.Constants]::AddJavaTypeRootPackage))} })


        #see if this java type has already been added
        if($myJavaTypes -ne $null)
        {
            $myJavaType = ($myJavaTypes | ? {$_.Name -eq $jCtor.TType} | Select-Object -First 1)
        }

        #send command across stdin to jre to instantiate
        if($myJavaType -ne $null)
        {
            #any type already added should be available for instantiation
            return New-Object $myJavaType

        }
        elseif($global:jrunscript.JavaPackages -contains $jCtor.Namespace -or $global:jrunscript.JavaClasses -contains $jCtor.TType)
        {
            $jreDirectDeclarator = ("my{0}" -f $jCtor.TType)
            $JreRef = "$jreDirectDeclarator{0:yyyyMMddHHmmss}" -f $(Get-Date)
            $stdIn = "var {0} = new {1};" -f $JreRef,$jCtor.RawSignature;

            $javaTypeInstance = [psobject] ($jCtor.TType)

            $javaTypeInstance | Add-Member NoteProperty JreRef $JreRef

            $javaTypeInstance | Add-Member NoteProperty JrePort ([NoFuture.Tools.JavaTools]::JrePort)

            $javaTypeInstance | Add-Member NoteProperty DebugPath (Join-Path ([NoFuture.TempDirectories]::Debug) ("{0}.out.txt" -f $jCtor.TType))

            $javaTypeInstance | Add-Member -MemberType ScriptMethod -Name SendToJre -Value {
                $memberStdin = $args[0]
                SendToJrunscript $memberStdin;
                $logStdIn = $memberStdin + "`n"
                [System.IO.File]::AppendAllText($this.DebugPath,$logStdIn);
            }

            $javaTypeInstance.SendToJre($stdIn);

            return $javaTypeInstance

        }
        else
        {
            throw "no matching Java Type found, call either Add-JavaType to add a type into the running appdomain / jrunscript.exe or ImportPackage on the Jrunscript global variable."
        }
    }
}

function Write-JavaTypeMemberDef($Ctor)
{

            #code template for .NET wrappers of Java types
            return @"
            /// <summary>
            /// The direct-declarator present in the Jre.
            /// </summary>
            public string JreRef {get; set;}

            /// <summary>
            /// The port number used in the instantiation of the Jrunscript exe from the 
            /// Start-Jre cmdlet
            /// </summary>
            public int JrePort {get; set;}

            /// <summary>
            /// A path to a text file which will save both the syntax of the 
            /// member's definition as well as the text commands passed through 
            /// the socket.
            /// </summary>
            public string DebugPath {get; set;}
            
            private System.Diagnostics.Process _jrunscript;
            private System.Net.Sockets.Socket _loopSocket;
            private System.Net.IPEndPoint _myEndPoint;

            $Ctor

            /// <summary>
            /// Sends the text over the socket to the running Jre.
            /// Prior to creating and connecting to the socket, the stdin parameter to 
            /// the defined debug path.
            /// </summary>
            /// <param name="stdIn">The command literal.  This will be converted to a ASCII byte array to be moved across the wire.</param>
            public void SendToJre(string stdIn)
            {
                System.IO.File.AppendAllText(this.DebugPath,stdIn);
                _loopSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.IP);
                _myEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, this.JrePort);
                _loopSocket.Connect(_myEndPoint);
                _loopSocket.Send(System.Text.Encoding.UTF8.GetBytes(stdIn));
                _loopSocket.Close();
            }

"@
}

function Write-CustomJavaTypeCtorSyntax($TypeName,$jreCtor,$JrunscriptProcId,$debugPath)
{
    $jrunscriptPort = [NoFuture.Tools.JavaTools]::JrePort

    #custom types will use the ref returned from 'Import-CustomTypeIntoJre' to create an instance of a just-defined-type
    #via a call to .newInstance (as compared to simply 'new ')
    return @"
        public $TypeName()
        {
            var jreCtor = "$jreCtor";//this is the direct-declarator,in the JRE, on which .newInstance is called
            JreRef = string.Format("$TypeName{0:yyyyMMddHHmmss}",System.DateTime.Now);
            _jrunscript = System.Diagnostics.Process.GetProcessById($JrunscriptProcId);
            var stdIn = string.Format("var {0} = {1}.newInstance();",JreRef, jreCtor);

            this.JrePort = $jrunscriptPort;
            this.DebugPath = @"$debugPath";

            SendToJre(stdIn);
        }

"@
}

function Write-JavaClass
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string]$TypeName,

        [Parameter(Mandatory=$false,position=1)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$TypeMembers, 

        [Parameter(Mandatory=$false,position=2)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Package,

        [Parameter(Mandatory=$false,position=3)]
        [AllowNull()]
        [string[]]$Imports,

        [Parameter(Mandatory=$false,position=4)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Extends, 

        [Parameter(Mandatory=$false,position=5)]
        [AllowNull()]
        [string[]]$Implements
    )
    Process
    {

        $donotdisplay = ""
        #turn string literal into src file
        $java = New-Object System.Text.StringBuilder

        #begin package
        if($Package -ne $null -and $Package -ne "")
        {
            if(-not ($Package.EndsWith(";"))){ $Package = $Package + ";"}
            $donotdisplay = $java.AppendLine("package $Package" )
        }

        #begin imports
        if($Imports -eq $null) {$Imports = @("") }
        if($Imports -notcontains "java.lang.*;"){$Imports += "java.lang.*;"}
        $Imports| ? {$_ -ne $null -and $_ -ne ""} | % {
            $donotdisplay = $java.AppendLine("import $_")
        }#end imports


        #begin class
        #type declaration
        $donotdisplay = $java.Append("public class $TypeName ")

        #extends 
        if($Extends -ne $null -and $Extends -ne "")
        {
            $donotdisplay = $java.Append("extends $Extends ")
        }

        #interface implementation
        if($Implements -ne $null -and $Implements -is [array] -and $Implements.Length -gt 0)
        {
            $donotdisplay = $java.Append("implements ")
            for($i=0;$i-lt$Implements.Length;$i++)
            {
                $implement = $Implements[$i]
                if($i-ne$Implements.Length-1)
                {
                    $donotdisplay = $java.Append("$implement, ")
                }
                else
                {
                    $donotdisplay = $java.Append("$implement")
                }
            }
        }
        $donotdisplay = $java.Append("{`n")

        #type members 
        $donotdisplay = $java.Append($TypeMembers + "`n")
        $donotdisplay = $java.AppendLine("}//end class")
        #end class

        return $java.ToString();
    }
}

function Write-TempJavaToFile($Java, $TypeName, $Package){

    #write the class to file
    if($Package -ne $null -and $Package -ne "")
    {
         if($Package.Contains(";")){$Package = $Package.Replace(";","");}
         $packageRelPath = $Package.Replace(".",([System.IO.Path]::PathSeparator));
         $javaSrc = (Join-Path ([NoFuture.TempDirectories]::JavaSrc) $packageRelPath)
         if(-not (Test-Path $javaSrc)){$donotdisplay = mkdir $javaSrc -Force}
    }
    else
    {
        $javaSrc = ([NoFuture.TempDirectories]::JavaSrc)
    }
    $outputFile = (Join-Path $javaSrc ("$TypeName.java"))
    if(Test-Path $outputFile) {Remove-Item -Path $outputFile -Force}
    [System.IO.File]::WriteAllBytes($outputFile,[System.Text.Encoding]::UTF8.GetBytes($java))


}

<#
    .SYNOPSIS
    Compiles .java files at JavaSrc location to JavaBuild then 
    places them into a .jar file at JavaDist.
    
    .DESCRIPTION
    Given some existing file(s) at NoFuture's Temp directory 
    JavaSrc, the java compiler is invoked sending .class files
    to JavaBuild. 

    .PARAMETER TypeName
    The file name(s) targeted for compilation, use astrick 
    to have compiler perform a search (e.g. MyJava*). Do not 
    include the .java extension.

    .PARAMETER Package
    Optional package (i.e. Namespace) with which the compilation
    targets belong

    .PARAMETER ClassPath
    Optional, additional classpaths - the environmental CLASSPATH 
    variable is already present and should not be specified 
    again.

#>
function Invoke-JavaCompiler
{
	[CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $TypeName,

        [Parameter(Mandatory=$false,position=1)]
        [string] $Package,

        [Parameter(Mandatory=$false,position=2)]
        [string] $ClassPath

    )
    Process
    {
        $javaSrc = [NoFuture.TempDirectories]::JavaSrc 
        $javaRoot = (Split-Path $javaSrc -Parent)
        $javaBuild = [NoFuture.TempDirectories]::JavaBuild 
        $javaDist = [NoFuture.TempDirectories]::JavaDist 
        $javaArchive = [NoFuture.TempDirectories]::JavaArchive 

        #remove .java extension if present
        if($TypeName.EndsWith(".java")){
            $outTo = $TypeName.IndexOf(".java")
            $TypeName = $TypeName.Substring(0,$outTo)
        }

        #in Java directory structure contained within a .jar must match the type's 'namespace'
        if(-not [System.String]::IsNullOrWhiteSpace($Package))
        {
            #remove semi-colon from the tail
            if($Package.Contains(";")){$Package = $Package.Replace(";","");}
            $packageRelPath = $Package.Replace(".",([System.IO.Path]::PathSeparator));
            $javaSrc = (Join-Path $javaSrc $packageRelPath);
            $javaBuild = (Join-Path $javaBuild $packageRelPath);
        }

        if(-not (Test-Path $javaRoot)){$donotdisplay = mkdir $javaRoot -Force}
        if(-not (Test-Path $javaSrc)) {$donotdisplay = mkdir $javaSrc -Force}
        if(-not (Test-Path $javaBuild)) {$donotdisplay = mkdir $javaBuild -Force}
        if(-not (Test-Path $javaDist)) {$donotdisplay = mkdir $javaDist -Force}
        if(-not (Test-Path $javaArchive)) {$donotdisplay = mkdir $javaArchive -Force}

        $ClassPath = Get-JavaClassPath $ClassPath

        #compile
        . ([NoFuture.Tools.JavaTools]::Javac) -d $javaBuild -cp $ClassPath (Join-Path $javaSrc ("$TypeName.java"))


        #place into jar
        if(-not [System.String]::IsNullOrWhiteSpace($Package))
        {
            $jarFile = (Join-Path $javaDist "$Package.jar")
        }
        else
        {
            $TypeName = $TypeName.Replace("*","")
            $jarFile = (Join-Path $javaDist "$TypeName.jar")
        }
        
        Push-Location $javaBuild
        . ([NoFuture.Tools.JavaTools]::Jar) cf $jarFile "*"
        Pop-Location

        return $jarFile
    }

}#end Invoke-JavaCompiler

function Get-JavaClassPath($ClassPath){
    $cp =  '.`;{0}' -f $env:CLASSPATH
    if(-not [System.String]::IsNullOrWhiteSpace($ClassPath)){
        $cp += '`;{0}' -f $ClassPath
    }
    return $cp
}

<#
    .SYNOPSIS
    Loads a jar file and the class file into the jrunscript.
    Intended for loading dynamic types - use $jrunscript.ImportPackage("...") 
    for pre-compressed .jar files.
    
    .DESCRIPTION
    Give a path to a jar file the cmdlet will load it into 
    the running jrunscript.exe process followed by a call 
    to load the class at 'Package.TypeName' or just 'TypeName'
    and lastly will call .getConstructor() on the loaded class.

    The reference created within the jrunscript upon the call
    to .getConstructor() is what is returned and may subsequently
    be used to instantiate the given type.

    .PARAMETER JarFile
    A standard MS Windows path to the targeted .jar file.  
    The cmdlet handles adding the additional syntax to the 
    path which is needed to load it.

    .PARAMETER TypeName
    This is the java class name unqualified for its package.

    .PARAMETER Package
    This is the package within which the java class is declared.
    It is optional and as such the class is presumed to be residing
    at the root of the .jar.
#>
function Import-CustomTypeIntoJre
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true,position=0)]
        [string] $JarFile,

        [Parameter(Mandatory=$true,position=1)]
        [string] $TypeName,

        [Parameter(Mandatory=$false,position=2)]
        [string] $Package
    )
    Process
    {
        $ms = [NoFuture.Shared.Constants]::ThreadSleepTime;

        #test jar file is present
        if(-not (Test-Path $jarFile)) { throw "Bad path or file name at '$jarFile'."}

        #construct a Java Jar URL
        $classLoaderJar = ("jar:file:$jarFile!/".Replace("\","\\"))
        
        #create a java.net.URL object
        $global:jrunscript.Write("var myUrlJar = new URL(`"$classLoaderJar`");")
        [System.Threading.Thread]::Sleep($ms)

        #wrap the java.net.URL object into a dyn. java array object
        $global:jrunscript.Write("var myUrls = java.lang.reflect.Array.newInstance(URL,1);")
        [System.Threading.Thread]::Sleep($ms)

        #assign the Java Jar URL as the index 0 of the URL array
        $global:jrunscript.Write("myUrls[0] = myUrlJar;")
        [System.Threading.Thread]::Sleep($ms)

        #this requires an array hence the latter code
        $global:jrunscript.Write("var cl = URLClassLoader.newInstance(myUrls);")
        [System.Threading.Thread]::Sleep($ms)

        #now load the class type
        if($Package -ne $null -and $Package -ne "")
        {
            $Package = $Package.Replace(";","")
            $global:jrunscript.Write("var myClass = cl.loadClass(`"$Package.$TypeName`");")
        }
        else
        {
            $global:jrunscript.Write("var myClass = cl.loadClass(`"$TypeName`");")
        }
        [System.Threading.Thread]::Sleep($ms)
        $jreCtor = "myCtor$TypeName"

        #last, get a ref. to the type's constructor to create new instances
        $global:jrunscript.Write("var $jreCtor = myClass.getConstructor();")
        [System.Threading.Thread]::Sleep($ms)

        #return the direct-declarator of this type's constructor
        return $jreCtor
    }
}

<#
Examples, using Jrunscript.exe
http://download.java.net/jdk8/docs/technotes/guides/scripting/programmer_guide/index.html

js> // Import Java packages and classes 
js> // like import package.*; in Java
js> importPackage(java.awt);
js> // like import java.awt.Frame in Java
js> importClass(java.awt.Frame);
js> // Create Java Objects by "new ClassName"
js> var frame = new java.awt.Frame("hello");
js> // Call Java public methods from script
js> frame.setVisible(true);
js> // Access "JavaBean" properties like "fields"
js> print(frame.title);

js> // create JavaImporter with specific packages and classes to import
js> var SwingGui = new JavaImporter(javax.swing,javax.swing.event,javax.swing.border,java.awt.event);
js> // within this 'with' statement, we can access Swing and AWT
js> with (SwingGui) {var mybutton = new JButton("test");var myframe = new JFrame("test");}

js> // create Java String array of 5 elements
js> var a = java.lang.reflect.Array.newInstance(java.lang.String, 5);

js> // Accessing elements and length access is by usual Java syntax
js> a[0] = "scripting is great!";
js> print(a.length);

var r  = new java.lang.Runnable() {
    run: function() {
        print("running...\n");
    }
};

// "r" can be passed to Java methods that expect java.lang.Runnable
var th = new java.lang.Thread(r);
th.start();

function func() {
     print("I am func!");
}

// pass script function for java.lang.Runnable argument
var th = new java.lang.Thread(func);
th.start();


#>

<#
Notes - getting members of a java jar

importPackage(java.util.jar);
var myJar = new JarFile("C:\\Projects\WindowsPowerShell\temp\code\java\dist\MyJar.jar");
var e = myJar.entries();
while(e.hasMoreElements()) {var je = e.nextElement(); print(je.getName() + "\n");}
#>

<#
importPackage(java.net);
var myJar = "jar:file:C:\\Projects\WindowsPowerShell\temp\code\java\dist\MyJar.jar!/"
var myUrlJar = new URL(myJar);
var myUrls = java.lang.reflect.Array.newInstance(URL,1);
myUrls[0] = myUrlJar;
var cl = URLClassLoader.newInstance(myUrls);
var myClass = cl.loadClass("MyJavaClass");
var myConstructor = myClass.getConstructor();
var myActualType = myConstructor.newInstance();
myActualType.GetMyString();
#>