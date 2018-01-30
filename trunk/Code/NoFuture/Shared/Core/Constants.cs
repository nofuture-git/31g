using System;
using System.ComponentModel;

namespace NoFuture.Shared.Core
{
    /// <summary>
    /// For encoding strings
    /// </summary>
    public enum EscapeStringType
    {
        DECIMAL,
        DECIMAL_LONG,
        HEXDECIMAL,
        UNICODE,
        REGEX,
        HTML,
        XML,
        URI,
        BLANK
    }

    /// <summary>
    /// Common versions of the .NET framework
    /// </summary>
    public enum DotNetVersion
    {
        NET11,
        NET20,
        NET35,
        NET4Plus
    }

    /// <summary>
    /// Misc. constant values used throughout.
    /// </summary>
    public class Constants
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const string ENUM = "System.Enum";

        public const string NF_CRYPTO_EXT = ".nfk"; //nofuture kruptos
        public const char LF = (char)0xA;
        public const char CR = (char)0xD;
        public const string TYPE_METHOD_NAME_SPLIT_ON = "::";

        public const int SOCKET_LISTEN_NUM = 5;
        /// <summary>
        /// The max size allowed by PowerShell 3.0's ConvertTo-Json cmdlet.
        /// </summary>
        public const int MAX_JSON_LEN = 2097152;

        public const double DBL_TROPICAL_YEAR = 365.24255;

        public static TimeSpan TropicalYear = new TimeSpan(365, 5, 49, 16, 320);


        public static char DefaultTypeSeparator { get; set; } = '.';

        /// <summary>
        /// The comma is a typical delimiter in many programming constructs.
        /// </summary>
        public static char DefaultCharSeparator { get; set; } = ',';

        public static char[] PunctuationChars { get; set; } = {
            '!', '"', '#', '$', '%', '&', '\\', '\'', '(', ')',
            '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>',
            '?','@', '[', ']', '^', '_', '`', '{', '|', '}', '~'
        };
    }
}
