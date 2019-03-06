using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms
{
    /// <summary>
    /// Typically these are street drugs (e.g. crack-cocaine)
    /// </summary>
    public class ScheduleI : ScheduleII
    {
        protected override string CategoryName => "Schedule I";
        public override bool IsAcceptedMedicalUse { get; } = false;

        public override int GetCategoryRank()
        {
            return base.GetCategoryRank() - 1;
        }
    }
}
