using System.Collections.Generic;

namespace NoFuture.Util.Core.Math
{
    public class Finite
    {
        public static long CalcFactorial(int n)
        {
            if (n == 1)
                return n;
            return n * CalcFactorial(n - 1);
        }
        public static long CalcCombinations(int n, int k)
        {
            return CalcFactorial(n) / (CalcFactorial(k) * CalcFactorial(n - k));
        }

        public static List<long[]> PossibleCombos(int n, int k)
        {
            _combinationMatchs = new List<long[]>();
            var v = new long[n + 1];
            Combinations(v, 1, n, 1, k);
            return _combinationMatchs;
        }

        //needed at instance-lvl scope because of recursive nature of actual calculations
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private static List<long[]> _combinationMatchs;

        //http://www.cs.utexas.edu/users/djimenez/utsa/cs3343/lecture25.html
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        internal static void Combinations(long[] v, int start, int n, int k, int maxk)
        {
            if (k > maxk)
            {
                var vi = new long[maxk];
                for (var i = 1; i <= maxk; i++)
                {
                    vi[i - 1] = v[i];
                }
                _combinationMatchs.Add(vi);
                return;
            }
            for (var i = start; i <= n; i++)
            {
                v[k] = i;
                Combinations(v, i + 1, n, k + 1, maxk);
            }
        }
    }
}
