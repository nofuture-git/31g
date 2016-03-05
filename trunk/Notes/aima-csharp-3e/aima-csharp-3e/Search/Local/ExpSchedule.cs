using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.Search.Local
{
    public class ExpSchedule : IFx<int>
    {
        public double Lam { get; }

        public int K { get; }

        public int Limit { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lam"></param>
        /// <param name="k"></param>
        /// <param name="limit"></param>
        public ExpSchedule(double lam= 0.045D, int k=20, int limit=100)
        {
            Lam = lam;
            Limit = limit;
            K = k;
            Eval = i => (i < Limit) ? K * Math.Exp(-1 * Lam * i) : 0.0D;
        }


        public Func<int, double> Eval { get; }
    }
}
