using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using NoFuture.Exceptions;

namespace NoFuture.Read.Vs
{
    public static class ProjFile
    {
        public const string DotNetProjXmlNs = "http://schemas.microsoft.com/developer/msbuild/2003";
        public const string NS = "vs";

        /// <summary>
        /// Reads a MsBuild project file as an XML document.
        /// </summary>
        /// <param name="vsprojPath"></param>
        /// <param name="nsMgr"></param>
        /// <returns></returns>
        public static XmlDocument GetVsProjXml(string vsprojPath, out XmlNamespaceManager nsMgr)
        {
            ValidateProjFile(vsprojPath);
            var proj = new XmlDocument();
            proj.Load(vsprojPath);
            nsMgr = new XmlNamespaceManager(proj.NameTable);
            nsMgr.AddNamespace(NS, DotNetProjXmlNs);

            return proj;
        }

        /// <summary>
        /// Produces a MsBuild project file using the schema defined for .NET 3.5
        /// </summary>
        /// <param name="vsprojPath"></param>
        /// <returns></returns>
        public static Net35.Project GetNet35ProjFile(string vsprojPath)
        {
            ValidateProjFile(vsprojPath);

            using (var fr = File.OpenText(vsprojPath))
            {
                var xmlRdr = XmlReader.Create(fr);

                var ser = new System.Xml.Serialization.XmlSerializer(typeof(Net35.Project));
                var deser = ser.Deserialize(xmlRdr);
                var proj = deser as Net35.Project;
                if (proj == null)
                    throw new ItsDeadJim(string.Format("The file at '{0}' could not be deserialized to a '{1}' type", vsprojPath,
                        typeof(Net35.Project).Name));
                xmlRdr.Close();
                return proj;
                
            }
        }

        /// <summary>
        /// Produces a MsBuild project file using the schema defined for .NET 4.+
        /// </summary>
        /// <param name="vsprojPath"></param>
        /// <returns></returns>
        public static Net40.Project GetNet40ProjFile(string vsprojPath)
        {
            ValidateProjFile(vsprojPath);

            using (var fr = File.OpenText(vsprojPath))
            {
                var xmlRdr = XmlReader.Create(fr);

                var ser = new System.Xml.Serialization.XmlSerializer(typeof(Net40.Project));
                var deser = ser.Deserialize(xmlRdr);
                var proj = deser as Net40.Project;
                if (proj == null)
                    throw new ItsDeadJim(string.Format("The file at '{0}' could not be deserialized to a '{1}' type", vsprojPath,
                        typeof(Net40.Project).Name));
                xmlRdr.Close();
                return proj;
            }
        }

        /// <summary>
        /// Writes the <see cref="proj"/> to the <see cref="vsprojPath"/> path.
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="vsprojPath"></param>
        public static void SaveFile(this Net35.Project proj, string vsprojPath)
        {
            var types =
                proj.GetType()
                    .GetProperties()
                    .Select(x => x.PropertyType.GetElementType())
                    .Where(x => x != null)
                    .ToList();

            types.AddRange(proj.GetType().GetProperties().Select(x => x.PropertyType).Where(x => x != null));
   
            SaveToDisk(proj, proj.GetType(), types.ToArray(), vsprojPath);
        }

        /// <summary>
        /// Writes the <see cref="proj"/> to the <see cref="vsprojPath"/> path.
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="vsprojPath"></param>
        public static void SaveFile(this Net40.Project proj, string vsprojPath)
        {
            var types =
                proj.GetType()
                    .GetProperties()
                    .Select(x => x.PropertyType.GetElementType())
                    .Where(x => x != null)
                    .ToArray();
            SaveToDisk(proj, proj.GetType(), types, vsprojPath);
        }

        internal static void SaveToDisk(object proj, Type projtype, Type[] additionalTypes, string vsprojPath)
        {
            if (projtype == null)
                return;
            if (projtype != typeof (Net35.Project) && projtype != typeof (Net40.Project))
                return;
            if (string.IsNullOrWhiteSpace(vsprojPath))
                return;
            var dir = Path.GetDirectoryName(vsprojPath);
            if (string.IsNullOrWhiteSpace(dir))
            {
                dir = TempDirectories.AppData;
                vsprojPath = Path.Combine(dir, vsprojPath);
            }
            else if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var ser = new System.Xml.Serialization.XmlSerializer(projtype, additionalTypes);

            using (var fw = File.Create(vsprojPath))
            {
                var xmlWr = XmlWriter.Create(fw, new XmlWriterSettings{Indent = true});
                ser.Serialize(xmlWr, proj);
                xmlWr.Close();
            }
        }

        internal static void ValidateProjFile(string vsprojPath)
        {
            if (string.IsNullOrWhiteSpace(vsprojPath) || !File.Exists(vsprojPath))
                throw new ItsDeadJim(string.Format("Cannot find the Vs Project file '{0}'.", vsprojPath));

            if (!Path.HasExtension(vsprojPath))
                throw new ItsDeadJim("Specify a specific [cs|vb|fs]proj file, not a whole directory");

            if (!Regex.IsMatch(Path.GetExtension(vsprojPath), @"\.(vb|cs|fs)proj"))
                throw new ItsDeadJim(string.Format("The Extension '{0}' was unexpected", Path.GetExtension(vsprojPath)));
        }
    }
}
