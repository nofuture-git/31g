# NoFuture 
---

First, the name, _NoFuture_, it doesn’t mean anything in particular – I picked up the phrase from a Sex Pistols song.  Likewise for the project name _31g_ – this is from a Joy Division song.  When I started this thing I never expected it would last and so I didn’t put a lot of thought into my capital namespace’s name.  

Next, what is it?  It’s a collection of projects and notes you would expect to find on a .NET developer’s workstation.  That’s not to say it’s some stack of disorganized snippets with no order.  If anything the opposite is true.  I don’t share it with other developers not because it’s bad but because, well, in some parts it’s on the horizon of madness.  By madness I mean it displays a level of effort that is difficult to explain if asked, “why?”  For example, _Get-RandomPerson_, why do I need to randomly generate a *person* with this much detail.  I don’t have an answer, and once another developer begins to realize just how much detail there is – well I’ll let the reader decide.   My best explanation is that I love to code – good code, which reads easily and operates with elegant simplicity.

# Overview
---

### PowerShell scripts
At the very top of the trunk is a bunch of PowerShell scripts.  These are more appropriately PowerShell modules because they define cmdlets and are not intended to be run ad hoc.  They operate as adapters to all the code contained in the NoFuture.sln.  The file names mostly describe what kind of cmdlets you can expect to find.  Some of these I use almost every day, like, _util.ps1_’s  _Select-StringRecurse_ and _sql.ps1_’s _Write-StoredProcedureToHost_ - while others are just for fun like _console.ps1_’s _Out-FunnyComputerBeeps_ and _Write-HostAsciiArt_.  All the cmdlets were defined with “Approved Verbs” so if they are imported as a PowerShell module it won’t complain.

### Notes
This directory contains all the notes from all the technical text I have read over the years.  There are also a handful of cook-book style code snippets which are prefixed with an underscore.  I have been doing this for a while and some of the notes cover technologies which have been deprecated.


### Code
This directory contains all the related code and forms the majority of the content.  The code is from various languages but the vast majority is C#.  In all, the following languages are present:
* C#
* F#
* JavaScript
* Java
* R
* Python
* C++

# Code Projects
---
### NoFuture.Shared

###### Synopsis
This is your typical *common* assembly which defines the types, paths, keys, strings, etc. which are environment-wide.   

###### Description
There are two of them, one in C# and another, verbatim one, in Python.  Both receive their settings from an xml file named _nfConfig.cfg.xml_.  There is a lot more C# content than Python and that is true for all the projects because Python was a recent addition.  

---

### NoFuture.Util
###### Synopsis
This is your typical *toolbox* assembly which defines those common string, path, net, math, etc. operations that developers use.
###### Description
The most used functions are those contained in _etc.(cs|py)_ which operate mostly on strings and do those common things like remove punctuation or convert to Pascal Case.  _Math_ contains functions and types which are typically algebraic.  _NfConsole_ defines types which are used by all the other console apps found throughout the solution.   I typically use a console app when I want some independent process – I use either the standard output or open a socket to perform the inter-process communication.  _NfType_ deals with parsing .NET type names (in the form of IL).  _Etymological_ attempts to deal with the way words can represent something more abstract.  _Gia_ is where a bunch of tools exist to generate *dot.exe* directed-graphs from .NET assemblies.  _Binary_ is just some basic binary ops I had around for a long time.  Concerning the top-level files, already mentioned *Etc*, *FxPointers* is a place to put .NET event handlers – its most important one being the handler for an AppDomain’s AssemblyResolve event.  _Gsm_ is kinda dated, I originally used it to send SMS on a very old mobile phone over a serial port – it totally worked but I need to move on to the Android SDK.  _Lexicon_ is just as the name sound and defines dictionaries of this-to-that.  _Net_ deals with various network related technologies while _NfPath_ deals with file IO.  

Util has a lot of peripheral projects each of which do some specialized task.   _NoFuture.Util.Binary.InvokeDpx_ this is basically for ranking assemblies (using the Google PageRank algo) like you would find in the \bin folder of some project.  It uses the [R runtime]( https://www.r-project.org/) along with two R packages named ‘sna’ and ‘igraph’.  _NoFuture.Util.Gia.InvokeAssemblyAnalysis_ is an intricate console app for resolving the method info to metadata tokens of a .NET assembly.  _NoFuture.Util.Gia.InvokeFlatten_ is for a specific “dot.exe” graph which will flatten a type down until it reaches value types at every terminal end.  _NoFuture.Util.Pos_ is for invocation of the [Stanford Part of Speech Tagger]( http://nlp.stanford.edu/software/tagger.shtml) in an isolated process.  _NoFuture.Util.Re_ is an assembly which isolates the [R runtime]( https://www.r-project.org/) dependencies from the rest of _NoFuture.Util_.

---

### NoFuture.Tokens
###### Synopsis
This is an assembly for parsing structured data.
###### Description
It originally started as just some home-brewed stuff but later became more sophisticated with the addition of ANTLR v4.  It depends on a project in NoFuture which is not part of the NoFuture.sln.  Namely,  _NoFuture.Antlr_, I isolated that project from the rest because of some build order issues.  The ANTLR extension for Visual Studio is amazing where you only need define the grammar files – it handles the creation of the .cs files and compiles them.   Most of the grammar files are from the [ANTLR repo](https://github.com/antlr/grammars-v4).  However the one of particular use is *DotNetIlTypeName* - this is the grammar used by the peripheral token’s project named _NoFuture.Tokens.InvokeNfTypeName_.  The reason this was separated into its own console app was to solve the circular dependency between _Util_’s  _NfType_ and _NoFuture.Tokens_.  I wanted to make use of the powerful options ANTLR v4 afforded without having to bloat my slim _NoFuture.Util_ with a bunch of extra dependencies.  So I solved it in my typical way of opening a console app with a socket and wiring some JSON across it.   Anyway, the reason I need something like ANTLR to deal with IL type names is due to the nesting of generic types within a declaration.  _Tokens_ also has some stuff for ASP.NET and C# -both derived from ANTLR v4 grammar files.

---

### Dia2Dump
###### Synopsis
This is a modified copy of the like-named project from Microsoft.
###### Description
This project used to come packaged with every Visual Studio (it seems to have stopped with 2015).  Like the original is deals with parsing .pdb files.  I modified it to write valid JSON to the standard output.  Its actually pretty handy and can be invoked for an entire assembly or for some specific type.  I use it to bridge between reflected .NET assemblies and the actual source files.

---

### NoFuture.Encryption
###### Synopsis
This is a my encryption library dealing in X509 certs and client-side JavaScript encryption.
###### Description
This has two main uses, one is to perform encryption\decryption using the [Stanford Javascript Crypto Library](https://github.com/bitwiseshiftleft/sjcl) and the other is to leverage the MS X509 cert technologies.  The _NfX509_ file is quite helpful and can create self-signed certs along with perform encryption\decryption of files on the drive using the asymmetric key of a X509 cert.

---

### NoFuture.Read
###### Synopsis
This is an assembly which I use for modifying MsBuild project files .NET configuration files, Visual Studio solution files, NuGet package files and INI files.
###### Description
Over the years I have found I will need to update these outside of the IDE or en masse and the types defined in this project do just that.  More specifically, it has methods to push and pop AppSettings, replace project references with binary references, merge configs, read and modify the __service.model__ section - and more.

---

### NoFuture.Sql
###### Synopsis
This is my tool box for various SQL queries.
###### Description
I have only been working in MSSQL for years now and so all the queries reflect that but I structured the project so that if I ever find a need for like queries in Oracle, MySql, SqlLite, etc. they will dovetail in nicely.

---

### NoFuture.Domain and NoFuture.Carriage
###### Synopsis
This is a pair of assemblies which allow for hosting an ASP.NET web app without using IIS.
###### Description
It’s useful for tests and what not and writing it helped me a lot in understanding the ASP.NET architecture along with the working parts of a .NET AppDomain.  The code springboards off the built-in ASP.NET pipeline.  The __Domain__ part is the actual hosting application with its own [HttpWorkerProcess](https://msdn.microsoft.com/en-us/library/system.web.httpworkerrequest(v=vs.110).aspx).  Its counterpart, __Carriage__ is responsible for configuring and launching the domain.

---

### NoFuture.Gen
###### Synopsis
This is a set of assemblies geared for code generation and removal.
###### Description
It operates using .NET assemblies on disk coupled with that assemblies’.pdb file (via _Dia2Dump_ mentioned above).  It’s written in a manner to handle .NET code for the various .NET languages but I have only implemented it for C#.  I have used it, coupled with _NoFuture.Util.InvokeAssmblyAnalysis_ and IIS logs, to eliminate 270K lines of dead code from another proprietary C# project.   As for its peripheral projects, _InvokeDia2Dump_ is an adapter to use and sort the JSON returned from _Dia2Dump_.  _InvokeGetCgOfType_ is a console app with allows for getting all the reflected info about a type without having to load that type or any of its dependencies into the invoking runtime – this is another example of a console app sending JSON over a socket on the localhost.   Last, _InvokeGraphViz_ is used to generate class-diagrams and other useful diagrams from a .NET assembly.  It basically reflects some assembly then uses that data to write a “dot.exe” graph file - see [GraphViz](http://www.graphviz.org/) for more info.

---

### NoFuture.Hbm
###### Synopsis
This is a whole set of tools for generating ORM entities.
###### Description
As the name implies it was originally only targeted toward NHibernate, but was later adapted in such a way that it could be used for any ORM so long as you define a T4 Template for it.  Currently there are templates for NHibernate, EF 6.x and EF 3.5 .  It works in a sort of pipeline and is directly written for use in the _hbm.ps1_ file.  Given some MSSQL connection it first dumps all the metadata to JSON files, then sorts this into another set of JSON files, next it uses the sorted JSON to generate _.hbm.xml_ files.  The adaptation comes from the fact that the generated _.hbm.xml_ files contain CDATA sections which are embedded JSON of all the original sorted metadata.  This embedded metadata is not part of the XML because that would invalidate them as legitimate _.hbm.xml_ files.  The _.hbm.xml_ gives a sense to the hierarchical structure of the database while the extra CDATA gives all the details any ORM will need.  Lastly, the adapted _.hbm.xml_ files are parsed into runtime objects with are then handed off to the T4 Templates to generate the needed code files - completing the ORM setup.

---

### NoFuture.Host
###### Synopsis
This contains projects which are executable hosts of other projects in _NoFuture_.
###### Description
There are two projects here. _NoFuture.Host.Proc_ is old and was originally written as a wrapper around a Windows Process with open sockets.  _NoFuture.Host.Encryption_ is another console-open-socket app which was written as a kind of runtime for the _sjcl.js_ mentioned above.  Encryption across platforms is a pain so the solution was to have a JavaScript interpreter (Jurassic by Paul Bartrum) hosted in a .NET runtime with open-sockets to perform the clear-text to cipher-text and vice versa.  I didn’t want to have to load the JavaScript interpreter for every request.

---

### NoFuture.Rand
###### Synopsis
This is a project for generating very detailed test data at random.
###### Description
This is, hand-down, my favorite project.  This originally started as part of a Selenium script I used for testing a web app.  It performs two main functions each of which the cmdlet’s in _random.ps1_ are named.  The first is _Get-RandomPerson_ and the other is _Get-RandomCompany_.  They are quite different in nature since a random person is just “made-up” while a random company is real.  Everything in NoFuture was written with the assumption of only having connection to the localhost, so you won’t find any WebRequest anywhere in the C# code.  As such, the actual web request are handled at the PowerShell level.   The random company depends on a bunch of data from the net – starting at a randomly chosen economic sector.  From there it queries the SEC, randomly picks some company within that economic sector and then drills down getting that companies financials, ticker-symbols, corporate address, etc.  

In contrast, a random person is generated wholly within the .NET runtime.  The “person” includes a lot of data being everything from bank-accounts, credit-cards, high-school, children, parents, if they rent or have a mortgage, their spouse and former spouse(s) it even randomly assigns them a vehicle with a valid, again made-up, VIN number.  The reason I had so much fun with this was I had to go about finding real data-sources.  For example, what were the top 100 female first names in the US in the 1950’s?  What is the age at which a US male has their second child in the mid-1980s.  Or again, with a random zip code in Maryland what is the mostly like industry of employment.  All this data was eventually calculated and persisted to the sister project named _NoFuture.Rand.Data.Source_ as embedded XML.  The formation of this data and big number crunching was handled by the close counterpart (an F# project) at _NoFuture.Rand.Src_.  The data came from lots of places and the code comments site them all.

---

### NoFuture.Timeline
###### Synopsis
This project is both an API for generating timeline diagrams (in text format) and an implementation of timelines for all of the history of Western Civilization.
###### Description
Last project is something out-of-the-ordinary.   It essentially started with the book, “A History of Israel” by John Bright.  In the back of the book was a series of historic timelines which look a lot like a UML Activity Diagram.   I basically extended that layout to ASCII text in Courier font (monotype) with an API to add various historic entries by year (either BCE or CE).   Once I had the API down I added the timelines found in the _Occidental_ folder of the project.  These began with the dates presented in J. Bright’s book but I then continued them all to way to the modern age.

## Summary
That pretty much lays it out.  The actual code is decorated with a lot of annotations, comments and examples.  Any external sources are cited in the function's annotation.  The tests in _NoFuture.Tests_ are dependent on files on the drive and would take some tinkering to get running green.  This was due to the odd nature of what some of the code does (like getting a method's MetadataToken off a .NET assembly by reading the IL bytes).   The kind of main() for the whole thing is the _start.ps1_ PowerShell script.  Its intended to basically setup the PowerShell console (prefer the ISE) with the contents of the entire namespace.  