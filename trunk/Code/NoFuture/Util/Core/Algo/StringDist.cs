using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Util.Core.Algo
{
    public static class StringDist
    {
        /// <summary>
        /// The Jaro–Winkler distance (Winkler, 1990) is a measure of similarity between two strings.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="mWeightThreshold"></param>
        /// <param name="mNumChars"></param>
        /// <returns>1 means a perfect match, 0 means not match what-so-ever</returns>
        /// <remarks>
        /// [http://stackoverflow.com/questions/19123506/jaro-winkler-distance-algorithm-in-c-sharp]
        /// </remarks>
        public static double JaroWinklerDistance(this string a, string b, double mWeightThreshold = 0.7D,
            int mNumChars = 4)
        {
            a = a ?? "";
            b = b ?? "";
            var aLen = a.Length;
            var bLen = b.Length;
            if (aLen == 0)
                return bLen == 0 ? 1.0 : 0.0;

            var searchRng = System.Math.Max(0, System.Math.Max(aLen, bLen) / 2 - 1);

            // default initialized to false
            var aMatched = new bool[aLen];
            var bMatched = new bool[bLen];

            var lNumCommon = 0;
            for (var i = 0; i < aLen; ++i)
            {
                var lStart = System.Math.Max(0, i - searchRng);
                var lEnd = System.Math.Min(i + searchRng + 1, bLen);
                for (var j = lStart; j < lEnd; ++j)
                {
                    if (bMatched[j])
                        continue;
                    if (a[i] != b[j])
                        continue;
                    aMatched[i] = true;
                    bMatched[j] = true;
                    ++lNumCommon;
                    break;
                }
            }
            if (lNumCommon == 0)
                return 0.0;

            var lNumHalfTrans = 0;
            var k = 0;
            for (var i = 0; i < aLen; ++i)
            {
                if (!aMatched[i])
                    continue;
                while (!bMatched[k]) ++k;
                {
                    if (a[i] != b[k])
                    {
                        ++lNumHalfTrans;
                    }
                }
                ++k;
            }
            // System.Diagnostics.Debug.WriteLine("numHalfTransposed=" + numHalfTransposed);
            var lNumTransposed = lNumHalfTrans / 2;

            // System.Diagnostics.Debug.WriteLine("numCommon=" + numCommon + " numTransposed=" + numTransposed);
            double lNumCommonD = lNumCommon;
            var lWeight = (lNumCommonD / aLen
                           + lNumCommonD / bLen
                           + (lNumCommon - lNumTransposed) / lNumCommonD) / 3.0;

            if (lWeight <= mWeightThreshold)
                return lWeight;
            var lMax = System.Math.Min(mNumChars, System.Math.Min(a.Length, b.Length));
            var lPos = 0;
            while (lPos < lMax && a[lPos] == b[lPos])
                ++lPos;
            if (lPos == 0)
                return lWeight;
            return lWeight + 0.1 * lPos * (1.0 - lWeight);

        }

        /// <summary>
        /// A string metric for measuring the difference between two sequences.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <param name="asRatioOfMax">
        /// Optional, returns, as a ratio, as the difference from 1 the quotient 
        /// of the distance over the max possiable distance.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Levenshtein_distance#Iterative_with_two_matrix_rows
        /// </remarks>
        public static double LevenshteinDistance(string s, string t, bool asRatioOfMax = false)
        {
            s = s ?? String.Empty;
            t = t ?? String.Empty;

            // degenerate cases
            if (s == t) return 0;
            if (s.Length == 0) return t.Length;
            if (t.Length == 0) return s.Length;

            // create two work vectors of integer distances
            var v0 = new int[t.Length + 1];
            var v1 = new int[t.Length + 1];

            // initialize v0 (the previous row of distances)
            // this row is A[0][i]: edit distance for an empty s
            // the distance is just the number of characters to delete from t
            for (var i = 0; i < v0.Length; i++)
                v0[i] = i;


            var ss = s.ToCharArray();
            var tt = t.ToCharArray();

            for (var i = 0; i < ss.Length; i++)
            {
                // calculate v1 (current row distances) from the previous row v0
                // first element of v1 is A[i+1][0]
                //   edit distance is delete (i+1) chars from s to match empty t
                v1[0] = i + 1;
                var si = ss[i];
                // use formula to fill in the rest of the row
                for (var j = 0; j < tt.Length; j++)
                {
                    var cost = si == tt[j] ? 0 : 1;
                    var j1 = v1[j] + 1;
                    var j2 = v0[j + 1] + 1;
                    var j3 = v0[j] + cost;

                    if (j1 < j2 && j1 < j3)
                    {
                        v1[j + 1] = j1;
                        continue;
                    }

                    v1[j + 1] = j2 < j3 ? j2 : j3;
                }

                // copy v1 (current row) to v0 (previous row) for next iteration
                Array.Copy(v1, v0, v0.Length);

            }

            if (!asRatioOfMax)
                return v1[t.Length];

            return 1D - (double)v1[t.Length] / new[] { t.Length, s.Length }.Max();
        }

        /// <summary>
        /// Of the possiable <see cref="candidates"/> returns those with the 
        /// shortest distance from <see cref="s"/> using the <see cref="LevenshteinDistance"/>
        /// algo.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static string[] ShortestDistance(this string s, IEnumerable<string> candidates)
        {
            if (String.IsNullOrWhiteSpace(s))
                return null;
            if (candidates == null || !candidates.Any())
                return null;
            var dict = new Dictionary<string, int>();

            foreach (var c in candidates.Distinct())
                dict.Add(c, (int) LevenshteinDistance(s, c));

            var minValue = dict.Values.Min();

            var df = dict.Where(x => x.Value == minValue);

            return df.Select(x => x.Key).ToArray();
        }
    }
}
