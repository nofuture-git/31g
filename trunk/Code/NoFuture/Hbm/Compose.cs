using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util;
using NoFuture.Util.Core;
using NoFuture.Util.NfType;

namespace NoFuture.Hbm
{
    /// <summary>
    /// Functionality specific to the composition of names
    /// while generating hbm.xml.
    /// </summary>
    public static class Compose
    {
        public static string CompKeyClassName(string name, string outputNamespace)
        {
            name = String.Format("{0}{1}", name, Globals.COMP_KEY_ID_SUFFIX);
            return ClassName(name, outputNamespace);
        }
        public static string ClassName(string name, string outputNamespace)
        {
            var asmQualifiedName = new StringBuilder();

            if (name.Split(Shared.Core.NfSettings.DefaultTypeSeparator).Length > 1)
            {
                var nameParts = name.Split(Shared.Core.NfSettings.DefaultTypeSeparator);
                var actualClassName = nameParts[(nameParts.Length - 1)].Replace(" ",Globals.REPLACE_SPACE_WITH_SEQUENCE);
                nameParts[(nameParts.Length - 1)] = Etc.SafeDotNetTypeName(actualClassName);
                name = string.Join(Shared.Core.NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture), nameParts);
            }

            //remove any chars not allowed in C# ids
            name = Etc.SafeDotNetTypeName(name);

            //capitalize first letter of whole word to avoid conflict with C# reserved words
            name = Etc.CapWords(name, Shared.Core.NfSettings.DefaultTypeSeparator);

            if (!String.IsNullOrWhiteSpace(outputNamespace))
            {
                outputNamespace = Etc.CapWords(outputNamespace, Shared.Core.NfSettings.DefaultTypeSeparator);
                asmQualifiedName.AppendFormat("{0}{1}", outputNamespace,Shared.Core.NfSettings.DefaultTypeSeparator);
            }

            asmQualifiedName.Append(name);

            if (!String.IsNullOrWhiteSpace(outputNamespace))
                asmQualifiedName.AppendFormat(", {0}", NfReflect.DraftCscExeAsmName(outputNamespace));

            var typeName = new NfTypeName(asmQualifiedName.ToString());

            if (!String.IsNullOrWhiteSpace(outputNamespace))
                return typeName.AssemblyQualifiedName;

            return typeName.FullName;
        }
        public static string PropertyName(string name, bool replaceInvalidsWithHexEsc = false)
        {
            name = Etc.ExtractLastWholeWord(name,Shared.Core.NfSettings.DefaultTypeSeparator);
            name = Etc.CapWords(name, null);
            return Etc.SafeDotNetIdentifier(name,replaceInvalidsWithHexEsc);
        }
        public static void ValidSplit(string name, int expectedLength)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return;
            }
            var nameArray = name.Split(Shared.Core.NfSettings.DefaultTypeSeparator);
            if (nameArray.Length < expectedLength)
            {
                throw new InvalidHbmNameException(
                    String.Format("The string '{0}' was expected to split on '{1}' into '{2}' but was actually '{3}'.",
                                  name,
                                  Shared.Core.NfSettings.DefaultTypeSeparator,
                                  expectedLength,
                                  nameArray.Length));
            }
        }

        public static string ManyToOnePropertyName(string fullAssemblyQualTypeName, string[] fkColumnNames)
        {
            var fkPropertyType = new NfTypeName(fullAssemblyQualTypeName);

            var fkPropertyName = fkPropertyType.ClassName;
            fkColumnNames = fkColumnNames.Select(x => Etc.CapWords(Etc.ExtractLastWholeWord(x, '.'), null)).ToArray();
            return string.Format("{0}By{1}", fkPropertyName,
                string.Join("And", fkColumnNames));
        }

        public static string BagPropertyName(string fullAssemblyQualTypeName)
        {
            var pluralBagName = new NfTypeName(fullAssemblyQualTypeName);
            return Gen.Etc.ToPlural(pluralBagName.ClassName, true);
        }

        public static string HbmFileName(string someName)
        {
            if (string.IsNullOrWhiteSpace(someName))
                return null;
            someName = Etc.CapWords(someName, '.');
            return $"{someName}.hbm.xml";
        }

        public static string HbmFileNameFromAsmQualTypeName(string asmFullName, bool prependDir = false)
        {
            if (string.IsNullOrWhiteSpace(asmFullName) || !NfReflect.IsFullAssemblyQualTypeName(asmFullName))
                return HbmFileName(asmFullName);

            var nfName = new NfTypeName(asmFullName);

            var tbl = nfName.Namespace.Replace($"{nfName.AssemblySimpleName}.", string.Empty) + "." +  nfName.ClassName;
            var flNm = HbmFileName(tbl);
            return prependDir ? Path.Combine(Settings.HbmDirectory, flNm) : flNm;
        }
        
    }
}
