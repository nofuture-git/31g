using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Domus.Opes.US
{
    public class AmericanFactorOptions
    {
        public OccidentalEdu EducationLevel { get; set; }

        public NorthAmericanRace Race { get; set; }

        public AmericanRegion Region { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public MaritialStatus MaritialStatus { get; set; }

        public int GetAge()
        {
            return Etc.CalcAge(BirthDate);
        }

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

        /// <summary>
        /// Gets a options object for <see cref="AmericanFactors"/> at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static AmericanFactorOptions RandomFactorOptions()
        {
            return new AmericanFactorOptions
            {
                EducationLevel = (OccidentalEdu.HighSchool | OccidentalEdu.Grad),
                Race = Etx.RandomPickOne(AmericanRacePercents.NorthAmericanRaceAvgs),
                Region = Etx.RandomPickOne(new[]
                    {AmericanRegion.Midwest, AmericanRegion.Northeast, AmericanRegion.South, AmericanRegion.West}),
                Gender = Etx.RandomCoinToss() ? Gender.Male : Gender.Female,
                BirthDate = Etx.RandomAdultBirthDate()
            };
        }
    }
}
