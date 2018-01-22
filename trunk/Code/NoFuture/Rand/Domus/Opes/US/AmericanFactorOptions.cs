using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Domus.Opes.US
{
    public class AmericanFactorOptions
    {
        private int _age;

        public AmericanFactorOptions()
        {
            EducationLevel = (OccidentalEdu.HighSchool | OccidentalEdu.Grad);
            Race = Etx.RandomPickOne(AmericanRacePercents.NorthAmericanRaceAvgs);
            Region = Etx.RandomPickOne(new[]
                {AmericanRegion.Midwest, AmericanRegion.Northeast, AmericanRegion.South, AmericanRegion.West});
            Age = (int) Math.Round(AmericanData.AVG_AGE_AMERICAN);
            Gender = Etx.RandomCoinToss() ? Gender.Male : Gender.Female;
        }

        public OccidentalEdu EducationLevel { get; set; }

        public NorthAmericanRace Race { get; set; }

        public AmericanRegion Region { get; set; }

        public int Age
        {
            get
            {
                if (_age <= 0)
                    _age = (int) Math.Round(AmericanData.AVG_AGE_AMERICAN);
                return _age;
            }
            set => _age = value;
        }

        public Gender Gender { get; set; }

        public MaritialStatus MaritialStatus { get; set; }

        public AmericanFactorOptions GetClone()
        {
            var o = new AmericanFactorOptions();

            var pi = GetType().GetProperties(NfConfig.DefaultFlags).Where(p => p.CanWrite).ToList();
            foreach (var p in pi)
            {
                var gVal = p.GetValue(this);
                p.SetValue(o, gVal);
            }
            return o;
        }
    }
}
