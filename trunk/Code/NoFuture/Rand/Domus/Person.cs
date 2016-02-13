using System;
using System.Collections.Generic;
using NoFuture.Exceptions;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Shared;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public abstract class Person : IPerson
    {
        #region fields
        protected readonly List<Uri> _netUris = new List<Uri>();
        protected readonly List<IPerson> _children = new List<IPerson>();
        protected Personality _personality = new Personality();
        #endregion

        #region properties
        public virtual DateTime? BirthDate { get; set; }
        public virtual DateTime? DeathDate { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Gender MyGender { get; set; }
        public virtual List<Uri> NetUri { get { return _netUris; } }
        public virtual MaritialStatus MaritalStatus { get; set; }
        public virtual IPerson Spouse { get; set; }
        public virtual IPerson Father { get; set; }
        public virtual IPerson Mother { get; set; }
        public virtual List<IPerson> Children { get { return _children; } }
        public Personality Personality { get { return _personality; } }
        public virtual IEducation Education { get; set; }
        public int GetAge(DateTime? atTime)
        {
            var dt = DateTime.Now;
            if (atTime != null)
                dt = atTime.Value;
            if (BirthDate == null)
                throw
                    new RahRowRagee(
                        String.Format("The random person named {0}, {1} does not have a Date Of Birth assigned.",
                            LastName, FirstName));

            if (DeathDate != null && DateTime.Compare(DeathDate.Value, dt) < 0)
                throw new ItsDeadJim("Its Dead Jim.");

            return CalcAge(BirthDate.Value, dt);
        }

        #endregion

        #region static utility methods

        /// <summary>
        /// Returns a new <see cref="NorthAmerican"/> with all values selected at random.
        /// </summary>
        /// <returns></returns>
        public static NorthAmerican American()
        {
            return new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Etx.CoinToss ? Gender.Female : Gender.Male,
                true);
        }

        /// <summary>
        /// Returns a new <see cref="NorthAmerican"/> with all values select at random.
        /// The City, Providence and Postal Code are limited to the major Canadian cities.
        /// </summary>
        /// <returns></returns>
        public static NorthAmerican Canadian()
        {
            var canadian = American();
            var cpp = CityArea.Canadian();
            canadian.WorkCityArea = cpp;
            canadian.HomeCityArea = cpp;

            return canadian;
        }

        public static int CalcAge(DateTime dob, DateTime atTime)
        {
            return Convert.ToInt32(Math.Round((atTime - dob).TotalDays / Constants.DBL_TROPICAL_YEAR));
        }

        #endregion
    }
}
