using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.Gia.Args;

namespace NoFuture.Util.Gia.InvokeFlatten.Cmds
{
    public class GetFlattenAssembly : NfConsole.CmdBase<FlattenAssembly>
    {
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

                Constants.AssemblySearchPaths.Add(Path.GetDirectoryName(asmPath));

                var asm = Constants.UseReflectionOnlyLoad
                    ? Binary.Asm.NfReflectionOnlyLoadFrom(asmPath)
                    : Binary.Asm.NfLoadFrom(asmPath);

                var flatAsm = Flatten.GetFlattenedAssembly(new FlattenLineArgs {Assembly = asm}, MyProgram.PrintToConsole);
                flatAsm.Path = asmPath;

                return EncodedResponse(flatAsm);

            }
            catch (Exception ex)
            {
                Console.WriteLine('\n');
                MyProgram.PrintToConsole(ex);
                return EncodedResponse(
                    new FlattenAssembly {AllLines = new List<FlattenedLine> {new NullFlattenedLine(ex)}});
            }
        }
    }
}
