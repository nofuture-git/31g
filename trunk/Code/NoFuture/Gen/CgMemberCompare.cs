using System.Collections.Generic;

namespace NoFuture.Gen
{
    /// <summary>
    /// Should sort those with the most number of <see cref="CgArg"/> 
    /// upon this number of <see cref="CgArg"/> count being equal
    /// then start comparing the length of the <see cref="CgArg.ArgName"/>'s length
    /// starting from the left and continue right until one is longer than the other 
    /// and exhausting all return <see cref="CgMemberCompare.X_GREATER_THAN_Y"/>
    /// Having no args then return whichever has the longest ToString 
    /// </summary>
    public class CgMemberCompare : Comparer<CgMember>
    {
        public int X_LESS_THAN_Y = -1;
        public int X_EQUALS_Y = 0;
        public int X_GREATER_THAN_Y = 1;

        public override int Compare(CgMember x, CgMember y)
        {
            if (x == null && y == null)
                return X_EQUALS_Y;
            if (y != null && x == null)
                return X_GREATER_THAN_Y;
            if (y == null)
                return X_LESS_THAN_Y;
            if (x.Args.Count < y.Args.Count)
                return X_GREATER_THAN_Y;
            if (x.Args.Count > y.Args.Count)
                return X_LESS_THAN_Y;

            var yToCsDecl = Settings.LangStyle.ToDecl(y);
            var xToCsDecl = Settings.LangStyle.ToDecl(x);
            if (x.Args.Count == 0)
                return yToCsDecl.Length > xToCsDecl.Length ? X_GREATER_THAN_Y : X_LESS_THAN_Y;
            for (var i = x.Args.Count - 1; i >= 0; i--)
            {
                var xCgArg = x.Args[i];
                var yCgArg = y.Args[i];
                if (xCgArg == null || yCgArg == null)
                    continue;
                if (xCgArg.ArgName.Length == yCgArg.ArgName.Length)
                    continue;
                if (xCgArg.ArgName.Length > yCgArg.ArgName.Length)
                    return X_LESS_THAN_Y;
                return X_GREATER_THAN_Y;
            }
            return yToCsDecl.Length > xToCsDecl.Length ? X_GREATER_THAN_Y : X_LESS_THAN_Y;
        }
    }
}