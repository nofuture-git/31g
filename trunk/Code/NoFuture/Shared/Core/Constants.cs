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
    /// Misc. constant values used throughout.
    /// </summary>
    public class Constants
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const string ENUM = "System.Enum";

        public const char LF = (char)0xA;
        public const char CR = (char)0xD;
        public const string TYPE_METHOD_NAME_SPLIT_ON = "::";

        public const double DBL_TROPICAL_YEAR = 365.24255;

        public static TimeSpan TropicalYear = new TimeSpan(365, 5, 49, 16, 320);
    }
}
