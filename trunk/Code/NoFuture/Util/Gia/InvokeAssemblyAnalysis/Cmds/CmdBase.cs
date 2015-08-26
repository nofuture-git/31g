using System;
using System.Linq;
using System.Reflection;
using System.Text;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public abstract class CmdBase<T> : ICmd
    {
        protected byte[] EmptyBytes = {(byte) '\0'};

        public byte[] EncodedResponse(T rspn)
        {
            try
            {
                return Encoding.UTF8.GetBytes(
                                    Newtonsoft.Json.JsonConvert.SerializeObject(rspn));
            }
            catch (Exception ex)
            {
                Program.PrintToConsole(ex);
                return new[] {(byte) '\0'};
            }
        }
        protected bool IsIgnore(string asmQualName)
        {
            if (string.IsNullOrWhiteSpace(asmQualName))
                return true;
            var gacAsms = GacAssemblyNames;
            return gacAsms != null && gacAsms.Any(asmQualName.EndsWith);
        }

        private static string[] _gacAsmNames;
        internal static string[] GacAssemblyNames
        {
            get
            {
                if (_gacAsmNames != null)
                    return _gacAsmNames;

                _gacAsmNames = Constants.UseReflectionOnlyLoad
                ? AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                    .Where(x => x.GlobalAssemblyCache)
                    .Select(x => x.FullName)
                    .ToArray()
                : AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.GlobalAssemblyCache)
                    .Select(x => x.FullName)
                    .ToArray();

                return _gacAsmNames;
            }
        }

        protected bool ResolveSingleToken(MetadataTokenName metadataToken)
        {
            if (metadataToken == null)
                return false;
            if (metadataToken.Id == 0)
            {
                return false;
            }
            var cid = metadataToken.Id;

            MemberInfo mi;
            try
            {
                mi = Program.ManifestModule.ResolveMember(cid);
            }
            catch (ArgumentException)//does not resolve the token
            {
                return false;
            }

            if (mi == null)
            {
                return false;
            }

            var metadataTokenInfo = new MetadataTokenName
            {
                Id = metadataToken.Id,
                Name = mi.Name,
                Label = mi.GetType().Name
            };

            string asmName = string.Empty;

            var type = mi as Type;

            if (type != null)
            {
                //do not sent back GAC assemblies - too big
                asmName = type.Assembly.GetName().FullName;
                if (GacAssemblyNames.Any(x => string.Equals(x, asmName, StringComparison.OrdinalIgnoreCase)))
                    return false;

                var t =
                    Program.AsmIndicies.Asms.FirstOrDefault(
                        x =>
                            string.Equals(x.AssemblyName, type.Assembly.GetName().FullName,
                                StringComparison.OrdinalIgnoreCase));

                metadataTokenInfo.AsmIndexId = t != null ? t.IndexId : 0;
                return true;
            }
            if (mi.DeclaringType == null) return true;

            //do not return GAC assemblies - too big
            asmName = mi.DeclaringType.Assembly.GetName().FullName;
            if (GacAssemblyNames.Any(x => string.Equals(x, asmName, StringComparison.OrdinalIgnoreCase)))
                return false;

            var f =
                Program.AsmIndicies.Asms.FirstOrDefault(
                    x =>
                        string.Equals(x.AssemblyName, mi.DeclaringType.Assembly.GetName().FullName,
                            StringComparison.OrdinalIgnoreCase));

            metadataTokenInfo.AsmIndexId = f != null ? f.IndexId : 0;

            return true;
        }

        public abstract byte[] Execute(byte[] arg);
    }
}
