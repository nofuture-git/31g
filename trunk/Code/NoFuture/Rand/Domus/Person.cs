using System;
using System.Collections.Generic;
using NoFuture.Rand.Domus.Pneuma;

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
        #endregion

        #region static utility methods

        /// <summary>
        /// Returns a new <see cref="NorthAmerican"/> with all values selected at random.
        /// </summary>
        /// <returns></returns>
        public static NorthAmerican American()
        {
            return new NorthAmerican(NorthAmerican.WorkingAdultBirthDate(), Etx.CoinToss ? Gender.Female : Gender.Male,
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

        #endregion
    }
}
