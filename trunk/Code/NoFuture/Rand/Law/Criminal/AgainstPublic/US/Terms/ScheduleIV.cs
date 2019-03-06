using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms
{
    /// <summary>
    /// Low potential for abuse with medical use (e.g. Xanax)
    /// </summary>
    public class ScheduleIV: ScheduleV
    {
        protected override string CategoryName => "Schedule IV";
        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() - 1;
        }
    }
}
