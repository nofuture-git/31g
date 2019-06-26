using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.Remedy
{
    /// <summary>
    /// the legal fiction that money damages can compensate for a victim&apos;s injury.
    /// </summary>
    [Aka("general damages")]
    public class NonpecuniaryDmg : CompensatoryDmg
    {
        public NonpecuniaryDmg(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// Translating human suffering into dollars and cents involves no mathematical formula;
        /// it rests, as we have said, on a legal fiction.
        /// </summary>
        public Func<IPlaintiff, decimal> CalcPainAndSuffering { get; set; } = lp => 0m;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var plaintiff = persons.Plaintiff() as IPlaintiff;
            if (plaintiff == null)
            {
                return false;
            }

            var title = plaintiff.GetLegalPersonTypeName();

            if (CalcPainAndSuffering == null)
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(CalcPainAndSuffering)} is unassigned");
                return false;
            }

            var pAndS = CalcPainAndSuffering(plaintiff);
            AddReasonEntry($"{title} {plaintiff.Name}, {nameof(CalcPainAndSuffering)} is {pAndS}");

            return pAndS > 0m;
        }
    }
}
