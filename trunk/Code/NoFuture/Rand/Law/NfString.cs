using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Util.Core
{
    public class NfSettings
    {
        public static char[] PunctuationChars { get; set; } = {
            '!', '"', '#', '$', '%', '&', '\\', '\'', '(', ')',
            '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>',
            '?','@', '[', ']', '^', '_', '`', '{', '|', '}', '~'
        };
        public const char LF = (char)0xA;
        public const char CR = (char)0xD;
    }

    public static class NfString
    {
        /// <summary>
        /// Given a string in the form of camel-case (or Pascal case) - a 
        /// <see cref="separator"/> will be inserted between characters 
        /// which are lowercase followed by uppercase.
        /// </summary>
        /// <param name="camelCaseString"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string TransformCaseToSeparator(this string camelCaseString, char separator)
        {
            if (String.IsNullOrWhiteSpace(camelCaseString))
                return String.Empty;
            var separatorName = new StringBuilder();
            var charArray = camelCaseString.ToCharArray();
            for (var i = 0; i < charArray.Length; i++)
            {
                separatorName.Append(charArray[i]);
                if (i + 1 >= charArray.Length)
                    continue;
                if (Char.IsLower(charArray[i]) && Char.IsUpper(charArray[i + 1]))
                {
                    separatorName.Append(separator);
                }
            }
            return separatorName.ToString();
        }

        /// <summary>
        /// Transforms a string of mixed case into standard camel-case (e.g. userName)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="perserveSep"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string name, bool perserveSep = false)
        {
            //is empty
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;

            name = name.Trim();

            //has no letters at all
            if (name.ToCharArray().All(x => !Char.IsLetter(x)))
                return name;

            //is all caps
            if (name.ToCharArray().Where(Char.IsLetter).All(Char.IsUpper))
                return name.ToLower();

            var nameFormatted = new StringBuilder();
            var markStart = false;
            var nameChars = name.ToCharArray();
            var sepChars = NfSettings.PunctuationChars.ToList();
            sepChars.Add(' ');
            for (var i = 0; i < nameChars.Length; i++)
            {
                var c = nameChars[i];

                if (sepChars.Contains(c))
                {
                    if (perserveSep)
                    {
                        nameFormatted.Append(c);
                        continue;
                    }
                    if (i + 1 < nameChars.Length)
                    {
                        nameChars[i + 1] = Char.ToUpper(nameChars[i + 1]);
                    }
                    continue;
                }

                if (!markStart)
                {
                    markStart = true;
                    nameFormatted.Append(c.ToString().ToLower());
                    continue;
                }

                if (i > 0 && Char.IsUpper(nameChars[i - 1]))
                {
                    nameFormatted.Append(c.ToString().ToLower());
                    continue;
                }

                nameFormatted.Append(c);

            }
            return nameFormatted.ToString();
        }

        /// <summary>
        /// Transforms <see cref="name"/> into Pascal case
        /// </summary>
        /// <param name="name"></param>
        /// <param name="preserveSep">Optional, set to true to have punctuation marks preserved</param>
        /// <returns></returns>
        public static string ToPascalCase(this string name, bool preserveSep = false)
        {
            if (String.IsNullOrWhiteSpace(name))
                return String.Empty;
            var toCamelCase = new StringBuilder();
            var charArray = ToCamelCase(name, preserveSep).ToCharArray();
            toCamelCase.Append(Char.ToUpper(charArray[0]));
            for (var i = 1; i < charArray.Length; i++)
            {
                toCamelCase.Append(charArray[i]);
            }
            return toCamelCase.ToString();
        }

        /// <summary>
        /// Converts line endings to Lf
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ConvertToLf(this string content)
        {
            if (content == null)
                return null;
            content = content.Replace(new string(new[] { NfSettings.CR, NfSettings.LF }),
                new string(new[] { NfSettings.LF }));
            content = content.Replace(new string(new[] { NfSettings.CR }), new string(new[] { NfSettings.LF }));
            return content;
        }

    }
}
