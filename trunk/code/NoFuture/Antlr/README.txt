The Visual Studio ANTLR Language Support is a required extension.
Any grammer (.g4) files included in the Grammers folder will
have thier .cs files generated at build time and placed into the
\obj folder.  The build will then include these and the resulting
binary will have all the ANTLR generated types present.
In other words, the ANTLR Language Support extension makes it so 
that the only source code files are the grammer files themselves.