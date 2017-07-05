using System;
using System.Text;
using System.Xml;

namespace NoFuture.Read.Config
{
    public class PkgConfig : BaseXmlDoc
    {
        public PkgConfig(string xmlDoc) : base(xmlDoc)
        {
        }

        protected internal XmlDocument PackageXml => _xmlDocument;

        /// <summary>
        /// Copies all package nodes present in this instance into the <see cref="targetConfig"/>
        /// </summary>
        /// <param name="targetConfig"></param>
        /// <param name="retainVerIfHigher">When the version values are comparable, will attempt to choose the one that is higher.</param>
        /// <returns></returns>
        public int CopyAllPackagesNodesTo(PkgConfig targetConfig, bool retainVerIfHigher = true)
        {
            var myPkgNodes = _xmlDocument?.SelectNodes("//package");
            if (myPkgNodes == null)
                return 0;

            var counter = 0;
            foreach (var pkgNode in myPkgNodes)
            {
                var pkgElem = pkgNode as XmlElement;
                if (pkgElem == null || !pkgElem.HasAttributes || !pkgElem.HasAttribute("id"))
                    continue;
                var id = pkgElem.Attributes["id"].Value;
                var ver = pkgElem.Attributes["version"]?.Value;
                var tf = pkgElem.Attributes["targetFramework"]?.Value;
                if (targetConfig.AddPackageNode(id, ver, tf, retainVerIfHigher))
                    counter += 1;

            }
            return counter;
        }

        public override void Save(Encoding encoding = null)
        {
            //get bit-order-marker
            encoding = encoding ?? new UTF8Encoding(true);
            base.Save(encoding);
        }

        public override void SaveAs(string fullName, Encoding encoding = null)
        {
            //get bit-order-marker
            encoding = encoding ?? new UTF8Encoding(true);
            base.SaveAs(fullName, encoding);
        }

        /// <summary>
        /// Adds or update a package node with the given attribute values.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <param name="targetFramework"></param>
        /// <param name="retainVerIfHigher">When the values are comparable, will attempt to choose the one that is higher.</param>
        /// <returns></returns>
        public bool AddPackageNode(string id, string version, string targetFramework, bool retainVerIfHigher = true)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            var packageNode = _xmlDocument.SelectSingleNode($"//package[@id='{id}']");
            var packageElem = packageNode as XmlElement ?? _xmlDocument.CreateElement("package");

            var idAttr = packageElem.Attributes["id"] ?? _xmlDocument.CreateAttribute("id");
            idAttr.Value = id;
            if (!packageElem.HasAttribute("id"))
                packageElem.Attributes.Append(idAttr);

            if (!string.IsNullOrWhiteSpace(version))
            {
                var verAttr = packageElem.Attributes["version"] ?? _xmlDocument.CreateAttribute("version");

                Version current;
                Version passedIn;
                var existingVer = verAttr.Value ?? "0.0.0.0";

                var passedinWillParse = Version.TryParse(version, out passedIn);
                var currentWillParse = Version.TryParse(existingVer, out current);

                if (passedinWillParse ^ currentWillParse)
                {
                    //take one that will parse over one that will not
                    version = passedinWillParse ? passedIn.ToString() : current.ToString();
                }

                else if (retainVerIfHigher && passedinWillParse && passedIn < current)
                {
                    //attempt to retain a higher version
                    version = current.ToString();
                }

                verAttr.Value = version;
                if (!packageElem.HasAttribute("version"))
                    packageElem.Attributes.Append(verAttr);
            }

            if (!string.IsNullOrWhiteSpace(targetFramework))
            {
                var targFrmAttr = packageElem.Attributes["targetFramework"] ??
                                  _xmlDocument.CreateAttribute("targetFramework");
                targFrmAttr.Value = targetFramework;
                if (!packageElem.HasAttribute("targetFramework"))
                    packageElem.Attributes.Append(targFrmAttr);
            }

            if (packageNode == null)
            {
                var rootNode = _xmlDocument.SelectSingleNode("/packages");
                if (rootNode?.AppendChild(packageElem) == null)
                    return false;
            }

            return true;
        }
    }
}
