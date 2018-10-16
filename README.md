# NoFuture 
---

A collection of projects and notes you would expect to find on a .NET developer’s workstation.

### Overview
---

##### PowerShell scripts
At the very top of the trunk are PowerShell scripts. These compose and invoke the code (see below) present in the NoFuture solution.

##### Notes
A directory containing notes from technical text and a handful of cook-book style code snippets which are prefixed with an underscore.

##### Code
This directory contains all the related code and forms the majority of the content.  The code is from various languages but the vast majority is C#.  In all, the following languages are present:
* C#
* F#
* JavaScript
* Java
* R
* Python
* C++

### Code Projects
---
The following is a logical grouping of code projects found herein.  Any project with _.Core_ in its namespace is a splinter assembly which was separated from its parent to allow for compilation in .NET Core.  Furthermore, if a project has a Python equivalent then it will be likenamed with a _ _py_ suffix.
Projects named with _.Invoke[...]_ are typically console apps which were isolated from their parent namespace for dependency reasons - they will either send data back on the standard output or on a socket.

##### NoFuture.Shared
An assembly for types which are shared by other assemblies which do not themselves have any reference to each other.

##### NoFuture.Shared.Cfg
This is your typical *common* assembly which defines the types, paths, keys, strings, etc. which are environment-wide.  They are defined in a custom configuration file named _nfConfig.cfg.xml_.  A custom configuration file is used to allow for the same data to be used by the counterpart Python project _shared_py_ and to avoid a dependency on the .NET _System.Configuration_ namespace. 

##### NoFuture.Util
This is your typical *toolbox* assembly which defines those common string, path, net, math, etc. operations that developers use.

##### NoFuture.Tokens & NoFuture.Antlr
A collection of projects for parsing structured data.

##### DIA SDK (a.k.a. Dia2Dump)
This is a modified copy of the like-named project from Microsoft.  See its _README.md_ for more information.

##### NoFuture.Encryption
This is a my encryption library dealing in X509 certs and client-side JavaScript encryption.

##### NoFuture.Read
This is an assembly which I use for modifying MsBuild project files .NET configuration files, Visual Studio solution files, NuGet package files and INI files.

##### NoFuture.Sql
This is my tool box for various SQL queries (currently only MSSQL).

##### NoFuture.Domain & NoFuture.Carriage
This is a pair of assemblies which allow for hosting an ASP.NET web app without using IIS.

##### NoFuture.Gen
This is a set of assemblies geared for code generation and removal.  This has a dependencies on _Dia2Dump_ and _NoFuture.Antlr_ mentioned above.

##### NoFuture.Gen.Hbm
This is a whole set of tools for generating ORM entities.

##### NoFuture.Host
This contains projects which are executable hosts of other projects in _NoFuture_.

##### NoFuture.Rand
This is a collection projects for generating test data at random (see the _README.md_ in its directory for more).

##### NoFuture.Timeline
This project is both an API for generating timeline diagrams (in text format) and an implementation of timelines for the history of Western Civilization (3000 BCE - 1992 CE).

### Summary
The actual code is decorated with a lot of annotations, comments and examples.  Any external sources are cited in the function's annotation.  The tests in _NoFuture.Tests_ are dependent on files on the drive and would take some tinkering to get running green.  This was due to the odd nature of what some of the code does (like getting a method's MetadataToken off a .NET assembly by reading the IL bytes).   The kind of main() for the whole thing is the _start.ps1_ PowerShell script.  Its intended to basically setup the PowerShell console (prefer the ISE) with the contents of the entire namespace.  