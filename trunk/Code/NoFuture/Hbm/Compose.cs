﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using NoFuture.Shared;
using NoFuture.Util;

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

            if (name.Split(Constants.DefaultTypeSeparator).Length > 1)
            {
                var nameParts = name.Split(Constants.DefaultTypeSeparator);
                var actualClassName = nameParts[(nameParts.Length - 1)].Replace(" ",Globals.REPLACE_SPACE_WITH_SEQUENCE);
                nameParts[(nameParts.Length - 1)] = TypeName.SafeDotNetTypeName(actualClassName);
                name = string.Join(Constants.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture), nameParts);
            }

            //remove any chars not allowed in C# ids
            name = TypeName.SafeDotNetTypeName(name);

            //capitalize first letter of whole word to avoid conflict with C# reserved words
            name = Etc.CapitalizeFirstLetterOfWholeWords(name, Constants.DefaultTypeSeparator);

            if (!String.IsNullOrWhiteSpace(outputNamespace))
            {
                outputNamespace = Etc.CapitalizeFirstLetterOfWholeWords(outputNamespace, Constants.DefaultTypeSeparator);
                asmQualifiedName.AppendFormat("{0}{1}", outputNamespace,Constants.DefaultTypeSeparator);
            }

            asmQualifiedName.Append(name);

            if (!String.IsNullOrWhiteSpace(outputNamespace))
                asmQualifiedName.AppendFormat(", {0}", TypeName.DraftCscExeAsmName(outputNamespace));

            var typeName = new TypeName(asmQualifiedName.ToString());

            if (!String.IsNullOrWhiteSpace(outputNamespace))
                return typeName.AssemblyQualifiedName;

            return typeName.FullName;
        }
        public static string PropertyName(string name, bool replaceInvalidsWithHexEsc = false)
        {
            name = Etc.ExtractLastWholeWord(name,Constants.DefaultTypeSeparator);
            name = Etc.CapitalizeFirstLetterOfWholeWords(name, null);
            return TypeName.SafeDotNetIdentifier(name,replaceInvalidsWithHexEsc);
        }
        public static void ValidSplit(string name, int expectedLength)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return;
            }
            var nameArray = name.Split(Constants.DefaultTypeSeparator);
            if (nameArray.Length < expectedLength)
            {
                throw new InvalidHbmNameException(
                    String.Format("The string '{0}' was expected to split on '{1}' into '{2}' but was actually '{3}'.",
                                  name,
                                  Constants.DefaultTypeSeparator,
                                  expectedLength,
                                  nameArray.Length));
            }
        }

        public static string ManyToOnePropertyName(string fullAssemblyQualTypeName, string[] fkColumnNames)
        {
            var fkPropertyType = new TypeName(fullAssemblyQualTypeName);

            var fkPropertyName = fkPropertyType.ClassName;
            fkColumnNames = fkColumnNames.Select(x => Util.Etc.CapitalizeFirstLetterOfWholeWords(Util.Etc.ExtractLastWholeWord(x, '.'), null)).ToArray();
            return string.Format("{0}By{1}", fkPropertyName,
                string.Join("And", fkColumnNames));
        }

        public static string BagPropertyName(string fullAssemblyQualTypeName)
        {
            var pluralBagName = new Util.TypeName(fullAssemblyQualTypeName);
            return Util.Etymological.En.ToPlural(pluralBagName.ClassName, true);
        }
        
    }
}
