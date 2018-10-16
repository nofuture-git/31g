# NoFuture 
---

A collection of projects and notes you would expect to find on a .NET developerís workstation.

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
##### NoFuture.Shared
This is your typical *common* assembly which defines the types, paths, keys, strings, etc. which are environment-wide.   

##### NoFuture.Util
This is your typical *toolbox* assembly which defines those common string, path, net, math, etc. operations that developers use.

##### NoFuture.Tokens & NoFuture.Antlr
A collection of projects for parsing structured data.

##### Dia2Dump
This is a modified copy of the like-named project from Microsoft.

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
This is a collection projects for generating test data at random (see the README.md in its directory for more).

##### NoFuture.Timeline
This project is both an API for generating timeline diagrams (in text format) and an implementation of timelines for all of the history of Western Civilization.

### Summary
The actual code is decorated with a lot of annotations, comments and examples.  Any external sources are cited in the function's annotation.  The tests in _NoFuture.Tests_ are dependent on files on the drive and would take some tinkering to get running green.  This was due to the odd nature of what some of the code does (like getting a method's MetadataToken off a .NET assembly by reading the IL bytes).   The kind of main() for the whole thing is the _start.ps1_ PowerShell script.  Its intended to basically setup the PowerShell console (prefer the ISE) with the contents of the entire namespace.  