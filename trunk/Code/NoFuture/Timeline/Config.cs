using System;

namespace NoFuture.Timeline
{
    public class Config
    {
        public static class Numerics
        {
            private const int DEFAULT_RULE_LN_SP = 9;
            internal const int DEFAULT_WIDTH = 26;
            private const int RULE_WIDTH = 6;
            private const int MIN_PADDING_BETWEEN_CONTENTIONS = 2;

            public static int DefaultRuleLnSp { get; set; } = DEFAULT_RULE_LN_SP;

            public static int DefaultWidth { get; set; } = DEFAULT_WIDTH;

            public static int RuleWidth { get; set; } = RULE_WIDTH;

            public static int MinPaddingBetweenContentions { get; set; } = MIN_PADDING_BETWEEN_CONTENTIONS;
        }

        public static class GraphChars 
        {
            public static char UNIX_NL_CHAR = (char)0xA;

            private const char BAR = '|';
            private const char BAR_RAIL_INTERSECT = '+';
            private const char RAIL = '-';
            private const char LIFE_LINE = ':';
            private const char DIRECT_LEFT = '<';
            private const char DIRECT_RIGHT = '>';
            private const char DIRECT_SHAFT = '.';
            private const char ID_SEPARATOR = '`';

            public static char Bar { get; set; } = BAR;

            public static char BarRailIntersect { get; set; } = BAR_RAIL_INTERSECT;

            public static char Rail { get; set; } = RAIL;

            public static char LifeLine { get; set; } = LIFE_LINE;

            public static char DirectLeft { get; set; } = DIRECT_LEFT;

            public static char DirectRight { get; set; } = DIRECT_RIGHT;

            public static char DirectShaft { get; set; } = DIRECT_SHAFT;

            public static char IDSeparator { get; set; } = ID_SEPARATOR;
        }

        public static class GraphStrings
        {
            #region fields
            private static int _arrowImmediateShaftLen = 4;
            private static int _arrowTailLen = 6;
            private static string _arrowHeadRight;
            private static string _arrowHeadLeft;
            #endregion

            #region properties
            /// <summary>
            /// This is the shaft length immediately following the
            /// <see cref="GraphChars.DirectLeft"/> or <see cref="GraphChars.DirectRight"/>
            /// </summary>
            public static int ShaftLenAfterArrowHead
            {
                get { return _arrowImmediateShaftLen; }
                set { _arrowImmediateShaftLen = value; }
            }

            /// <summary>
            /// This is the len of the arrow shaft present in the 
            /// block from which it originates.
            /// </summary>
            public static int ShaftLenInOriginBlock
            {
                get { return _arrowTailLen;}
                set { _arrowTailLen = value; }
            }
            public static string ArrowTailShaft => new string(GraphChars.DirectShaft, _arrowTailLen);

            #endregion

            #region methods
            public static string ArrowHeadRight
            {
                get
                {
                    if(string.IsNullOrWhiteSpace(_arrowHeadRight))
                        _arrowHeadRight =
                            $"{GraphChars.DirectLeft}{new string(GraphChars.DirectShaft, _arrowImmediateShaftLen)}";
                    return _arrowHeadRight;
                }
                set { _arrowHeadRight = value; }
            }

            public static string ArrowHeadLeft
            {
                get
                {
                    if(string.IsNullOrWhiteSpace(_arrowHeadLeft))
                        _arrowHeadLeft =
                            $"{new String(GraphChars.DirectShaft, _arrowImmediateShaftLen)}{GraphChars.DirectRight}";
                    return _arrowHeadLeft;
                }
                set { _arrowHeadLeft = value; }
            }
            #endregion 
        }
    }
}
