using System.Collections.Generic;

namespace NoFuture.Util.Math
{
    public class Finite
    {
        public static int CalcFactorial(int n)
        {
            if (n == 1)
                return n;
            return n * CalcFactorial(n - 1);
        }
        public static int CalcCombinations(int n, int k)
        {
            return CalcFactorial(n) / (CalcFactorial(k) * CalcFactorial(n - k));
        }

        public static List<int[]> PossiableCombos(int n, int k)
        {
            _combinationMatchs = new List<int[]>();
            var v = new int[n + 1];
            Combinations(v, 1, n, 1, k);
            return _combinationMatchs;
        }

        //needed at instance-lvl scope because of recursive nature of actual calculations
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private static List<int[]> _combinationMatchs;

        //http://www.cs.utexas.edu/users/djimenez/utsa/cs3343/lecture25.html
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        internal static void Combinations(int[] v, int start, int n, int k, int maxk)
        {
            if (k > maxk)
            {
                var vi = new int[maxk];
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
