using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Gov.Nhtsa;

namespace NoFuture.Rand.Data.Sp
{
    /// <inheritdoc cref="IMereo"/>
    /// <summary>
    /// Base implementation a name of any kind of money entry
    /// </summary>
    [Serializable]
    public class Mereo : VocaBase, IMereo
    {
        private readonly List<string> _eg = new List<string>();
        private Pecuniam _expectedValue = Pecuniam.Zero;
        public Mereo()
        {
            
        }

        public Mereo(string name)
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, name));
        }

        public Mereo(string name, string groupName)
        {
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Legal, name));
            Names.Add(new Tuple<KindsOfNames, string>(KindsOfNames.Group, groupName));
        }

        public Mereo(IVoca names)
        {
            CopyFrom(names);
        }

        public Mereo(IMereo mereo) : this((IVoca)mereo)
        {
            ExpectedValue = mereo.ExpectedValue;
            Interval = mereo.Interval;
            Classification = mereo.Classification;
            foreach(var eg in mereo.ExempliGratia)
                ExempliGratia.Add(eg);
        }

        public Interval Interval { get; set; }
        public Classification Classification { get; set; }

        public virtual string Name
        {
            get => GetName(KindsOfNames.Legal);
            set => UpsertName(KindsOfNames.Legal, value);
        }

        public List<string> ExempliGratia => _eg;

        public Pecuniam ExpectedValue
        {
            get => _expectedValue ?? (_expectedValue = Pecuniam.Zero);
            set => _expectedValue = value;
        }

        public void AdjustToAnnualInterval()
        {
            if (Interval == Interval.Annually)
                return;

            var hasExpectedValue = ExpectedValue != null && ExpectedValue != Pecuniam.Zero;
            var hasMultiplier = AmericanData.Interval2AnnualPayMultiplier.ContainsKey(Interval);

            if (!hasExpectedValue || !hasMultiplier)
            {
                Interval = Interval.Annually;
                return;
            }

            var multiplier = AmericanData.Interval2AnnualPayMultiplier[Interval];
            ExpectedValue = (ExpectedValue.ToDouble() * multiplier).ToPecuniam();
            Interval = Interval.Annually;
        }

        public override string ToString()
        {
            return new Tuple<string, string, Pecuniam>(Name, GetName(KindsOfNames.Group), ExpectedValue).ToString();
        }

        public static IMereo GetMereoById(Identifier property, string prefix = null)
        {
            switch (property)
            {
                case null:
                    return new Mereo(prefix);
                case ResidentAddress residenceLoan:
                    return residenceLoan.IsLeased
                        ? new Mereo(String.Join(" ", prefix, "Rent Payment"))
                        : new Mereo(String.Join(" ", prefix, "Mortgage Payment"));
                case Vin _:
                    return new Mereo(String.Join(" ", prefix, "Vehicle Payment"));
                case CreditCardNumber _:
                    return new Mereo(String.Join(" ", prefix, "Cc Payment"));
                case AccountId _:
                    return new Mereo(String.Join(" ", prefix, "Bank Account Transfer"));
            }

            return new Mereo(prefix);
        }
    }
}
