using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Tokens.DotNetMeta.TokenId;
using NoFuture.Tokens.DotNetMeta.TokenName;
using NoFuture.Util.Core;
using NoFuture.Util.NfConsole;

namespace NoFuture.Tokens.DotNetMeta.InvokeAssemblyAnalysis.Cmds
{
    public class ReassignTokenNames : IaaProgramBaseCmd<TokenReassignResponse>
    {
        private string _reassignAssemblySubjectName;
        public ReassignTokenNames(Program myProgram) : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole();
            MyProgram.PrintToConsole($"{nameof(ReassignTokenNames)} invoked");
            MyProgram.ProgressMessageState = null;
            try
            {
                var json = Encoding.UTF8.GetString(arg);
                var rqst = JsonConvert.DeserializeObject<TokenReassignRequest>(json);
                var subj = rqst.SubjectTokenNames;

                //assign this for writing the output to file
                _reassignAssemblySubjectName = rqst.AsmName;
                var frgn = rqst.ForeignTokenNames;
                var typs = rqst.ForeignTokenTypes;
                if (subj == null || typs == null)
                {
                    return JsonEncodedResponse(new TokenReassignResponse
                    {
                        Msg = "Both the SubjectTokenNames and ForeignTokenTypes are required.",
                        St = MetadataTokenStatus.Error
                    });
                }

                subj.ReassignAllInterfaceTokens(typs, MyProgram.PrintToConsole, frgn);
                var rspn = new TokenReassignResponse {Names = subj.Items};
                return JsonEncodedResponse(rspn);
            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(new TokenReassignResponse
                {
                    Msg = string.Format(ex.Message),
                    St = MetadataTokenStatus.Error
                });
            }
        }

        public override void WriteOutputToDisk(byte[] bytes, string fileName = null, string fileExt = ".json")
        {
            if (string.IsNullOrWhiteSpace(_reassignAssemblySubjectName))
            {
                base.WriteOutputToDisk(bytes, fileName, fileExt);
                return;
            }

            var isFullAssemblyName = NfReflect.IsAssemblyFullName(_reassignAssemblySubjectName);
            var isFullPath = Path.IsPathRooted(_reassignAssemblySubjectName);
            var isFileName = _reassignAssemblySubjectName.EndsWith(".dll") ||
                             _reassignAssemblySubjectName.EndsWith(".exe");

            if (isFullAssemblyName)
            {
                var assemblyName = new System.Reflection.AssemblyName(_reassignAssemblySubjectName);
                _reassignAssemblySubjectName = assemblyName.Name;
            }

            if (isFullPath)
            {
                var dirName  = Path.GetDirectoryName(_reassignAssemblySubjectName);
                if (!string.IsNullOrWhiteSpace(dirName))
                {
                    _reassignAssemblySubjectName = _reassignAssemblySubjectName.Replace(dirName, "");
                }
            }

            if (isFileName)
            {
                var flname = Path.GetFileNameWithoutExtension(_reassignAssemblySubjectName);
                if (!string.IsNullOrWhiteSpace(flname))
                    _reassignAssemblySubjectName = flname;
            }

            var tn = fileName ?? GetType().Name;
            tn = NfPath.SafeFilename(tn);
            var dir = MyProgram == null ? NfSettings.AppData : MyProgram.LogDirectory;

            var rootAsmName = NfPath.SafeDirectoryName(_reassignAssemblySubjectName);

            dir = Path.Combine(dir, rootAsmName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllBytes(Path.Combine(dir, tn + fileExt), bytes);
        }
    }
}
