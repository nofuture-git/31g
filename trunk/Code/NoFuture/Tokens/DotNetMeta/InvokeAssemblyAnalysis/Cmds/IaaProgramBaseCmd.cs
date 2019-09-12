using System.IO;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds
{
    public abstract class IaaProgramBaseCmd<T> : CmdBase<T>, ICmd
    {
        protected IaaProgramBaseCmd(Program p) : base(p)
        {
        }

        public override void WriteOutputToDisk(byte[] bytes, string fileName = null, string fileExt = ".json")
        {
            var tn = fileName ?? GetType().Name;
            tn = NfPath.SafeFilename(tn);
            var dir = MyProgram == null ? NfSettings.AppData : MyProgram.LogDirectory;

            var iaaProgram = MyProgram as IaaProgram;
            if (iaaProgram == null)
            {
                base.WriteOutputToDisk(bytes, fileName, fileExt);
                return;
            }

            var rootAsmName = iaaProgram.RootAssembly?.GetName().Name;
            if (rootAsmName == null)
            {
                base.WriteOutputToDisk(bytes, fileName, fileExt);
                return;
            }

            rootAsmName = NfPath.SafeDirectoryName(rootAsmName);

            dir = Path.Combine(dir, rootAsmName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllBytes(Path.Combine(dir, tn + fileExt), bytes);
        }
    }
}
