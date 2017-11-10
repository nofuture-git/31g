using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov.TheStates;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public abstract class UsState
    {
        #region constants
        public const int AGE_OF_ADULT = 18;
        public const int MIN_AGE_FOR_DL = 16;
        #endregion

        #region fields
        protected readonly string _stateAbbrv;
        protected DriversLicense[] dlFormats;
        protected AmericanUniversity[] universities;
        protected AmericanHighSchool[] highSchools;
        protected UsStateData myData;
        #endregion

        #region ctor
        protected UsState(string stateAbbrv)
        {
            _stateAbbrv = stateAbbrv;
        }
        #endregion

        #region properties

        public virtual int AgeOfMajority { get; set; } = AGE_OF_ADULT;

        /// <summary>
        /// The two letter postal code abbreviation
        /// </summary>
        public string StateAbbrv => _stateAbbrv;

        /// <summary>
        /// This is always resolved on the first entry found in the <see cref="dlFormats"/>.
        /// </summary>
        public virtual string RandomDriversLicense
        {
            get
            {
                if (dlFormats == null || dlFormats.Length <= 0)
                    return string.Empty;
                return dlFormats[0].GetRandom();
            }
        }
        /// <summary>
        /// Derived from [https://insurancelink.custhelp.com/app/answers/detail/a_id/1631/~/license-formats-for-individual-states]
        /// </summary>
        public virtual DriversLicense[] DriversLicenseFormats => dlFormats;

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Join(" ", Etc.DistillToWholeWords(GetType().Name));
        }

        /// <summary>
        /// Asserts that the given value matches at least one of this 
        /// State's DL format.
        /// </summary>
        /// <param name="dlnumber"></param>
        /// <returns></returns>
        public virtual bool ValidDriversLicense(string dlnumber)
        {
            if (dlFormats == null || dlFormats.Length <= 0)
                return false;
            return dlFormats.Any(dlf => dlf.Validate(dlnumber));
        }

        /// <summary>
        /// Uses the data presented from <see cref="TreeData.AmericanUniversityData"/>.
        /// </summary>
        public virtual AmericanUniversity[] GetUniversities()
        {
            //only resolve this once per instance
            if (universities != null && universities.Any())
                return universities;

            //this will never pass so avoid the exception
            if (TreeData.AmericanUniversityData == null)
                return new AmericanUniversity[] { };

            var elements =
                TreeData.AmericanUniversityData.SelectSingleNode(
                    $"//state[@name='{GetType().Name.ToUpper()}']") ??
                TreeData.AmericanUniversityData.SelectSingleNode($"//state[@name='{GetType().Name}']");
            if (elements == null || !elements.HasChildNodes)
                return new AmericanUniversity[] { };

            var tempList = new List<AmericanUniversity>();
            foreach (var elem in elements)
            {
                AmericanUniversity univOut = null;
                if (AmericanUniversity.TryParseXml(elem as XmlElement, out univOut))
                {
                    univOut.State = this;
                    tempList.Add(univOut);
                }
            }

            if (tempList.Count == 0)
                return new AmericanUniversity[] { };

            universities = tempList.ToArray();
            return universities;
        }

        /// <summary>
        /// Uses the data in <see cref="TreeData.AmericanHighSchoolData"/>.
        /// </summary>
        public virtual AmericanHighSchool[] GetHighSchools()
        {
            //only resolve this once per instance
            if (highSchools != null && highSchools.Any())
                return highSchools;

            if (TreeData.AmericanHighSchoolData == null)
                return new AmericanHighSchool[] { };
            var elements =
                TreeData.AmericanHighSchoolData.SelectNodes($"//state[@name='{GetType().Name}']//high-school");
            if (elements == null || elements.Count <= 0)
                return new AmericanHighSchool[] { };

            var tempList = new List<AmericanHighSchool>();
            foreach (var elem in elements)
            {
                AmericanHighSchool hsOut;
                if (AmericanHighSchool.TryParseXml(elem as XmlElement, out hsOut))
                {
                    hsOut.State = this;
                    tempList.Add(hsOut);
                }
            }
            if (tempList.Count == 0)
                return new AmericanHighSchool[] { };

            highSchools = tempList.ToArray();
            return highSchools;
        }

        /// <summary>
        /// Uses the data in <see cref="TreeData.UsStateData"/>
        /// </summary>
        /// <returns></returns>
        public virtual UsStateData GetStateData()
        {
            if (myData != null)
                return myData;
            myData = new UsStateData(this);
            return myData;
        }

        public override bool Equals(object obj)
        {
            var st = obj as UsState;
            if (st == null)
                return false;
            return string.Equals(st.StateAbbrv, StateAbbrv, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(st.GetType().Name, GetType().Name, StringComparison.Ordinal);
        }
        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode();
        }
        #endregion

        #region static factories
        internal static readonly List<UsState> _theStates = new List<UsState>();//singleton

        public static UsState[] TheStates
        {
            get
            {
                if (_theStates.Count <= 0)
                    InitAllUsStates();
                return _theStates.ToArray();
            }
        }

        /// <summary>
        /// Gets the <see cref="UsState"/> type based on the <see cref="stateAbbrv"/> (e.g. California is 'CA')
        /// </summary>
        /// <param name="stateAbbrv"></param>
        public static UsState GetStateByPostalCode(string stateAbbrv)
        {
            if (_theStates.Count <= 0)
                InitAllUsStates();
            return _theStates.FirstOrDefault(s => s.StateAbbrv == stateAbbrv);
        }

        /// <summary>
        /// Gets the <see cref="UsState"/> type based on the <see cref="fullStateName"/> (case-insensitive).
        /// </summary>
        /// <param name="fullStateName"></param>
        /// <returns></returns>
        public static UsState GetStateByName(string fullStateName)
        {
            if (_theStates.Count <= 0)
                InitAllUsStates();

            return
                _theStates.FirstOrDefault(
                    x => string.Equals(x.GetType().Name, fullStateName, StringComparison.OrdinalIgnoreCase)) ??
                _theStates.FirstOrDefault(
                    x => string.Equals(x.ToString(), fullStateName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Helper method to be used in the ctor of each state when assigning thier respective <see cref="DlFormats"/>.
        /// </summary>
        /// <param name="lenth"></param>
        /// <param name="startAt"></param>
        /// <returns></returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal static RcharNumeric[] Numerics(int lenth,int startAt = 0)
        {
            var someNumerics = new List<RcharNumeric>();
            for (var i = startAt; i < lenth + startAt; i++)
            {
                someNumerics.Add(new RcharNumeric(i));
            }
            return someNumerics.ToArray();
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal static void InitAllUsStates()
        {
            _theStates.Add(new Alabama());
            _theStates.Add(new Alaska());
            _theStates.Add(new Arizona());
            _theStates.Add(new Arkansas());
            _theStates.Add(new California());
            _theStates.Add(new Colorado());
            _theStates.Add(new Connecticut());
            _theStates.Add(new Delaware());
            _theStates.Add(new Florida());
            _theStates.Add(new Georgia());
            _theStates.Add(new Hawaii());
            _theStates.Add(new Idaho());
            _theStates.Add(new Illinois());
            _theStates.Add(new Indiana());
            _theStates.Add(new Iowa());
            _theStates.Add(new Kansas());
            _theStates.Add(new Kentucky());
            _theStates.Add(new Louisiana());
            _theStates.Add(new Maine());
            _theStates.Add(new Maryland());
            _theStates.Add(new Massachusetts());
            _theStates.Add(new Michigan());
            _theStates.Add(new Minnesota());
            _theStates.Add(new Mississippi());
            _theStates.Add(new Missouri());
            _theStates.Add(new Montana());
            _theStates.Add(new Nebraska());
            _theStates.Add(new Nevada());
            _theStates.Add(new NewHampshire());
            _theStates.Add(new NewJersey());
            _theStates.Add(new NewMexico());
            _theStates.Add(new NewYork());
            _theStates.Add(new NorthCarolina());
            _theStates.Add(new NorthDakota());
            _theStates.Add(new Ohio());
            _theStates.Add(new Oklahoma());
            _theStates.Add(new Oregon());
            _theStates.Add(new Pennsylvania());
            _theStates.Add(new RhodeIsland());
            _theStates.Add(new SouthCarolina());
            _theStates.Add(new SouthDakota());
            _theStates.Add(new Tennessee());
            _theStates.Add(new Texas());
            _theStates.Add(new Utah());
            _theStates.Add(new Vermont());
            _theStates.Add(new Virginia());
            _theStates.Add(new Washington());
            _theStates.Add(new DistrictOfColumbia());
            _theStates.Add(new WestVirginia());
            _theStates.Add(new Wisconsin());
            _theStates.Add(new Wyoming());
        }
        
        #endregion
    }
}
