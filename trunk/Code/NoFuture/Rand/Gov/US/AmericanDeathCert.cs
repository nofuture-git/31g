using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Gov.US
{
    /// <summary>
    /// The form of an American Death Certificate based on the CDC
    /// https://www.cdc.gov/nchs/data/dvs/death11-03final-acc.pdf
    /// </summary>
    [Serializable]
    public class AmericanDeathCert : DeathCert
    {
        /// <summary>
        /// Item 37.
        /// </summary>
        [Serializable]
        public enum MannerOfDeath
        {
            Natural,
            Accident,
            Suicide,
            Homicide,
            PendingInvestigation,
            CouldNotBeDetermined
        }

        public AmericanDeathCert(MannerOfDeath mannerOfDeath, string personFullNamen) : base(personFullNamen)
        {
            CauseOfDeath = new Stack<string>();
            Category = mannerOfDeath;
        }

        public AmericanDeathCert(MannerOfDeath mannerOfDeath, IVoca personName) : this(mannerOfDeath, (string) null)
        {
            if (personName == null)
                return;

            PersonFullName = Etc.DistillSpaces(
                string.Join(" ", personName.GetName(KindsOfNames.First),
                    personName.GetName(KindsOfNames.Middle),
                    personName.GetName(KindsOfNames.Surname)));

            if (string.IsNullOrWhiteSpace(PersonFullName))
                PersonFullName = personName.GetName(KindsOfNames.Legal);
        }

        public AmericanDeathCert(MannerOfDeath mannerOfDeath, DateTime dateOfDeath) : base(dateOfDeath)
        {
            CauseOfDeath = new Stack<string>();
            Category = mannerOfDeath;
        }

        public AmericanDeathCert(MannerOfDeath mannerOfDeath) : this(mannerOfDeath, (string)null)
        {
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = base.ToData(txtCase) ?? new Dictionary<string, object>();
            itemData.Add(textFormat(nameof(MannerOfDeath)), Category);
            return itemData;
        }

        /// <summary>
        /// 32. PART I. Enter the chain of events--diseases, injuries, or 
        /// complications--that directly caused the death. 
        /// </summary>
        public Stack<string> CauseOfDeath { get; }

        public MannerOfDeath Category { get; }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), Category);
        }

        /// <summary>
        /// Gets a date of death based on the <see cref="AmericanEquations.LifeExpectancy"/>
        /// </summary>
        /// <param name="birthDate"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        [RandomFactory]
        public static DateTime RandomDeathDate(DateTime? birthDate = null, string gender = null)
        {
            var dob = birthDate ?? Etx.RandomAdultBirthDate();
            gender = gender ?? (Etx.RandomCoinToss() ? "Female" : "Male");
            var normDist = AmericanEquations.LifeExpectancy(gender);
            var ageAtDeath = Etx.RandomValueInNormalDist(normDist.Mean, normDist.StdDev);
            var years = (int)Math.Floor(ageAtDeath);
            var days = (int)Math.Round((ageAtDeath - years) * Constants.DBL_TROPICAL_YEAR);

            var deathDate =
                dob.AddYears(years)
                    .AddDays(days)
                    .AddHours(Etx.RandomInteger(0, 12))
                    .AddMinutes(Etx.RandomInteger(0, 59))
                    .AddSeconds(Etx.RandomInteger(0, 59));
            return deathDate;
        }
    }
}