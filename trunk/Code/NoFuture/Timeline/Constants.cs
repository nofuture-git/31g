using System;

namespace NoFuture.Timeline
{


    public class Constants
    {
        public static class Numerics
        {
            private const int DEFAULT_RULE_LN_SP = 9;
            internal const int DEFAULT_WIDTH = 26;
            private const int RULE_WIDTH = 6;
            private const int MIN_PADDING_BETWEEN_CONTENTIONS = 2;

            private static int _defaultRuleLnSp = DEFAULT_RULE_LN_SP;
            private static int _defaultWidth = DEFAULT_WIDTH;
            private static int _ruleWidth = RULE_WIDTH;
            private static int _minPaddingBetweenContentions = MIN_PADDING_BETWEEN_CONTENTIONS;

            public static int DefaultRuleLnSp
            {
                get { return _defaultRuleLnSp; }
                set { _defaultRuleLnSp = value; }
            }

            public static int DefaultWidth
            {
                get { return _defaultWidth; }
                set { _defaultWidth = value; }
            }

            public static int RuleWidth
            {
                get { return _ruleWidth; }
                set { _ruleWidth = value; }
            }

            public static int MinPaddingBetweenContentions
            {
                get { return _minPaddingBetweenContentions; }
                set { _minPaddingBetweenContentions = value; }
            }
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

            private static char _bar = BAR;
            private static char _barRailIntersect = BAR_RAIL_INTERSECT;
            private static char _rail = RAIL;
            private static char _lifeLine = LIFE_LINE;
            private static char _directLeft = DIRECT_LEFT;
            private static char _directRight = DIRECT_RIGHT;
            private static char _directShaft = DIRECT_SHAFT;
            private static char _idSeparator = ID_SEPARATOR;

            public static char Bar
            {
                get { return _bar; }
                set { _bar = value; }
            }

            public static char BarRailIntersect
            {
                get { return _barRailIntersect; }
                set { _barRailIntersect = value; }
            }

            public static char Rail
            {
                get { return _rail; }
                set { _rail = value; }
            }

            public static char LifeLine
            {
                get { return _lifeLine; }
                set { _lifeLine = value; }
            }

            public static char DirectLeft
            {
                get { return _directLeft; }
                set { _directLeft = value; }
            }

            public static char DirectRight
            {
                get { return _directRight; }
                set { _directRight = value; }
            }

            public static char DirectShaft
            {
                get { return _directShaft; }
                set { _directShaft = value; }
            }

            public static char IDSeparator
            {
                get { return _idSeparator; }
                set { _idSeparator = value; }
            }
        }

        public static class GraphStrings
        {
            #region fields
            private static int _arrowImmediateShaftLen = 4;
            private static int _arrowTailLen = 6;
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
            public static string ArrowTailShaft { get { return new string(GraphChars.DirectShaft, _arrowTailLen); } }
            #endregion

            #region methods
            public static string ArrowHeadRight
            {
                get
                {
                    return string.Format("{0}{1}",
                        GraphChars.DirectLeft,
                        new string(GraphChars.DirectShaft, _arrowImmediateShaftLen));
                }
            }

            public static string ArrowHeadLeft
            {
                get
                {
                    return string.Format("{0}{1}",
                        new String(GraphChars.DirectShaft, _arrowImmediateShaftLen),
                        GraphChars.DirectRight);
                }
            }
            #endregion 
        }
    }
}
