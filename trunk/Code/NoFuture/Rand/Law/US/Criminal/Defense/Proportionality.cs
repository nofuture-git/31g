using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense
{
    public class Proportionality<T> : DefenseBase where T: ITermCategory
    {
        public Proportionality(ICrime crime) : base(crime)
        {
        }

        public Func<ILegalPerson, T> GetContribution { get; set; } = lp => default(T);

        public Func<ITermCategory, ITermCategory, bool> IsProportional { get; set; } = (t1, t2) =>
            (t1?.GetCategoryRank() ?? 0) == (t2?.GetCategoryRank() ?? -1);

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            throw new NotImplementedException();
        }
    }
}
