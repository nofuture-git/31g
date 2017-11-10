using System;
using System.Text.RegularExpressions;

namespace NoFuture.Rand.Data.Exo.NfHtml
{
    [Serializable]
    public abstract class NfHtmlDynDataBase : NfDynDataBase
    {
        protected NfHtmlDynDataBase(Uri src) : base(src) { }


        public const string DBL_QUOTE_ATTR_VALUE = @"\x22[^<']*\x22";
        public const string SINGLE_QUOTE_ATTR_VALUE = @"'[^<\x22]*'";

        public const string TAG_NAME = @"[\x2da-zA-Z0-9]+";
        public const string ATTR_REGEX = TAG_NAME + @"\x20*\x3d\x20*(" + DBL_QUOTE_ATTR_VALUE + "|" + SINGLE_QUOTE_ATTR_VALUE + ")";

        /// <summary>
        /// Regex pattern to find html nodes with attributes on the closing node (e.g. &lt;/someNode notAllowed="some value" &gt;)
        /// </summary>
        public const string ATTR_ON_CLOSE_NODE_REGEX =
            @"\x3c\x2f\x20*(" + TAG_NAME + @")\x20+(" + ATTR_REGEX + @")+\x20*\x3e";

        public static string PreParser(string webContent)
        {
            if (string.IsNullOrWhiteSpace(webContent))
                return webContent;

            var regex = new Regex(ATTR_ON_CLOSE_NODE_REGEX);

            if (!regex.IsMatch(webContent))
                return webContent;
            var matches = regex.Matches(webContent);
            foreach (Match m in matches)
            {
                if (!m.Success)
                    continue;
                var replaceThis = m.Groups[0].Value;
                var withThis = "</" + m.Groups[1].Value + ">";
                webContent = webContent.Replace(replaceThis, withThis);
            }

            return webContent;
        }

        protected internal string GetWebResponseBody(object content)
        {
            var webResponseBody = content as string;
            if (webResponseBody == null)
                return null;

            webResponseBody = PreParser(webResponseBody);
            return webResponseBody;
        }
    }
}
