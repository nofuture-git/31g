﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Tokens.Gia.InvokeFlatten.Cmds
{
    public class GetFlattenAssembly : CmdBase<FlattenAssembly>, ICmd
    {
        private readonly int _maxDepth;

        public GetFlattenAssembly(Program myProgram, int maxDepth)
            : base(myProgram)
        {
            _maxDepth = maxDepth;
            if (_maxDepth <= 0)
                _maxDepth = FlattenLineArgs.MAX_DEPTH;
        }
        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole("GetFlattenAssembly invoked");
            MyProgram.ProgressMessageState = null;

            try
            {
                if(arg == null || arg.Length <= 0)
                    throw new ItsDeadJim("No Path to an assembly was passed to the GetFlattenAssembly command.");

                var asmPath = Encoding.UTF8.GetString(arg);
                
                if(!File.Exists(asmPath))
                    throw new ItsDeadJim("There isn't a file at the location: " + asmPath);

                NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(asmPath));

                var asm = NfConfig.UseReflectionOnlyLoad
                    ? Util.Binary.Asm.NfReflectionOnlyLoadFrom(asmPath)
                    : Util.Binary.Asm.NfLoadFrom(asmPath);
                Action<ProgressMessage> myProgress = message => MyProgram.PrintToConsole(message);
                var flatAsm = Flatten.GetFlattenedAssembly(new FlattenLineArgs {Assembly = asm, Depth = _maxDepth}, myProgress);
                flatAsm.Path = asmPath;

                return JsonEncodedResponse(flatAsm);

            }
            catch (Exception ex)
            {
                Console.WriteLine('\n');
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(
                    new FlattenAssembly {AllLines = new List<FlattenedLine> {new NullFlattenedLine(ex)}});
            }
        }
    }
}
