using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms
{
    /// <summary>
    /// High potential for abuse drugs which have some medical use (e.g. Fentanyl, Amphetamine, etc.)
    /// </summary>
    public class ScheduleII : ScheduleIII
    {
        protected override string CategoryName => "Schedule II";
        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() - 1;
        }
    }
}
