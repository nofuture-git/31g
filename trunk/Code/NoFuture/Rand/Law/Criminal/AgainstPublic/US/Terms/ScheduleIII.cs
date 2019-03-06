using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms
{
    /// <summary>
    /// Medium potential for abuse with some medical use (e.g. steroids)
    /// </summary>
    public class ScheduleIII : ScheduleIV
    {
        protected override string CategoryName => "Schedule III";
        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() - 1;
        }
    }
}
