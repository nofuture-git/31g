using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Exceptions;

namespace NoFuture.Read.Vs
{
    public class ProjFile
    {
        public const string DotNetProjXmlNs = "http://schemas.microsoft.com/developer/msbuild/2003";
        public const string NS = "vs";
        public static XmlDocument GetVsProjFile(string vsprojPath, out XmlNamespaceManager nsMgr)
        {
            if(string.IsNullOrWhiteSpace(vsprojPath) || !File.Exists(vsprojPath))
                throw new ItsDeadJim(string.Format("Cannot find the Vs Project file '{0}'.",vsprojPath));

            if(!Path.HasExtension(vsprojPath))
                throw new ItsDeadJim("Specify a specific [cs|vb|fs]proj file, not a whole directory");

            if(!Regex.IsMatch(Path.GetExtension(vsprojPath),@"\.(vb|cs|fs)proj"))
                throw new ItsDeadJim(string.Format("The Extension '{0}' was unexpected",Path.GetExtension(vsprojPath)));

            var proj = new XmlDocument();
            proj.Load(vsprojPath);
            nsMgr = new XmlNamespaceManager(proj.NameTable);
            nsMgr.AddNamespace(NS, DotNetProjXmlNs);

            return proj;
        }
    }
}
