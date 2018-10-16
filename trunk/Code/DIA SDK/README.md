# DIA SDK
---
This a modified version of the C++ project which appears with an install of Visual Studio.  Typically its towards the top of the installation folder (i.e. _./Professional/DIA SDK_).

The general purpose is to parse a _.pdb_ file.  This modified version is pretty much identical to the original except that its standard output is JSON instead of straight text.

I use this to link source-code files to .NET binary assemblies within the _NoFuture.Gen_ assemblies.  

Furthermore, the JSON output types are defined in the _NoFuture.Shared.DiaSdk_ namespace.

The _.vsxproj_ is set to place the compiled version here in this location; however, I, nevertheless, added a compiled version to source control since compiling native C applications on a Windows box is always a challenge.

Last, within _start.ps1_ there is a portion of the script which will copy the compiled version from this location and place it within the _~/trunk/bin_ directory.