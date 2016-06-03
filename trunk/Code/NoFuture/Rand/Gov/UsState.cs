using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NoFuture.Rand.Data;
using NoFuture.Rand.Edu;
using NoFuture.Util;
using NoFuture.Util.Math;

namespace NoFuture.Rand.Gov
{
    public abstract class UsState
    {
        #region constants
        public const int AGE_OF_ADULT = 18;
        public const int MIN_AGE_FOR_DL = 16;
        #endregion

        #region fields
        protected readonly string _stateAbbrv;
        protected LinearEquation _income;
        /// <summary>
        /// Derived from [https://insurancelink.custhelp.com/app/answers/detail/a_id/1631/~/license-formats-for-individual-states]
        /// </summary>
        protected DriversLicense[] dlFormats;
        protected AmericanUniversity[] universities;
        protected AmericanHighSchool[] highSchools;

        #endregion

        #region ctor
        protected UsState(string stateAbbrv)
        {
            _stateAbbrv = stateAbbrv;
        }
        #endregion

        #region properties

        public virtual int AgeOfConsent { get; set; } = AGE_OF_ADULT;

        /// <summary>
        /// https://en.wikipedia.org/wiki/List_of_U.S._states_by_educational_attainment
        /// </summary>
        public float PercentHighSchoolGrad { get; set; }

        /// <summary>
        /// https://en.wikipedia.org/wiki/List_of_U.S._states_by_educational_attainment
        /// </summary>
        public float PercentCollegeGrad { get; set; }

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

        public virtual DriversLicense[] Formats => dlFormats;

        public LinearEquation AverageEarnings => _income;

        #endregion

        #region methods

        public override string ToString()
        {
            return StateAbbrv;
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
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> off which /Data/Source/*.xml
        /// containing all the xml NoFuture depends on.
        /// </summary>
        public virtual Edu.AmericanUniversity[] GetUniversities()
        {
            if (universities != null)
                return universities;

            //this will never pass so avoid the exception
            if (Data.TreeData.AmericanUniversityData == null)
                return null;

            var elements =
                Data.TreeData.AmericanUniversityData.SelectSingleNode(
                    $"//state[@name='{GetType().Name.ToUpper()}']") ??
                Data.TreeData.AmericanUniversityData.SelectSingleNode($"//state[@name='{GetType().Name}']");
            if (elements == null || !elements.HasChildNodes)
                return null;

            var tempList = new List<AmericanUniversity>();
            foreach (var elem in elements)
            {
                AmericanUniversity univOut = null;
                if (Edu.AmericanUniversity.TryParseXml(elem as XmlElement, out univOut))
                {
                    univOut.State = this;
                    tempList.Add(univOut);
                }
            }

            if (tempList.Count == 0)
                return null;

            universities = tempList.ToArray();
            return universities;
        }

        /// <summary>
        /// Uses the data presented from <see cref="TreeData.AmericanHighSchoolData"/>.
        /// A path must be set to <see cref="NoFuture.BinDirectories.Root"/> off which /Data/Source/*.xml
        /// containing all the xml NoFuture depends on.
        /// </summary>
        public virtual Edu.AmericanHighSchool[] GetHighSchools()
        {
            if (highSchools != null)
                return highSchools;

            if (Data.TreeData.AmericanHighSchoolData == null)
                return null;
            var elements =
                Data.TreeData.AmericanHighSchoolData.SelectNodes($"//state[@name='{GetType().Name}']//high-school");
            if (elements == null || elements.Count <= 0)
                return null;

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
                return null;

            highSchools = tempList.ToArray();
            return highSchools;
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
                    x => string.Equals(x.GetType().Name, fullStateName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Helper method to be used in the ctor of each state when assigning thier respective <see cref="DlFormats"/>.
        /// </summary>
        /// <param name="lenth"></param>
        /// <param name="startAt"></param>
        /// <returns></returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal static NumericRchar[] Numerics(int lenth,int startAt = 0)
        {
            var someNumerics = new List<NumericRchar>();
            for (var i = startAt; i < lenth + startAt; i++)
            {
                someNumerics.Add(new NumericRchar(i));
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

    public class Alabama : UsState
    {
        public Alabama() : base("AL")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7)) {IssuingState = this} };
            PercentHighSchoolGrad = 82.1F;
            PercentCollegeGrad = 22.0F;
            _income = new LinearEquation(-1918692.60413191,976.184335491814);
        }
    }

    public class Alaska : UsState
    {
        public Alaska() : base("AK")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7)) { IssuingState = this } };
            PercentHighSchoolGrad = 91.4F;
            PercentCollegeGrad = 26.6F;
            _income = new LinearEquation(-2178383.79518956, 1111.57761332099);
        }
    }

    public class Arizona : UsState
    {
        public Arizona() : base("AZ")
        {
            var dl = new Rchar[9];
            dl[0] = new LimitedRchar(0, 'A', 'B', 'D', 'Y');
            Array.Copy(Numerics(8,1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl), new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 84.2F;
            PercentCollegeGrad = 25.6F;
            _income = new LinearEquation(-2034700.95571994, 1035.38902251001);
        }
    }

    public class Arkansas : UsState
    {
        public Arkansas() : base("AR")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9)), new DriversLicense(Numerics(8)) { IssuingState = this } };
            PercentHighSchoolGrad = 82.4F;
            PercentCollegeGrad = 18.9F;
            _income = new LinearEquation(-1787876.96978101, 909.415664508158);
        }
    }

    public class California : UsState
    {
        public California() : base("CA")
        {
            var dl = new Rchar[8];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(7,1), 0, dl, 1, 7);
            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };

            PercentHighSchoolGrad = 80.6F;
            PercentCollegeGrad = 29.9F;

            _income = new LinearEquation(-2593003.00277512, 1318.76040703051);
        }
    }

    public class Colorado : UsState
    {
        public Colorado() : base("CO")
        {
            var dl = new Rchar[8];
            dl[0] = new UAlphaRchar(0);
            dl[1] = new UAlphaRchar(1);
            Array.Copy(Numerics(6, 2), 0, dl, 2, 6);

            dlFormats = new[] { new DriversLicense(dl), new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 89.3F;
            PercentCollegeGrad = 35.9F;
            _income = new LinearEquation(-2275104.63983958, 1156.92476102373);
        }
    }

    public class Connecticut : UsState
    {
        public Connecticut() : base("CT")
        {
            var dl = new Rchar[9];
            dl[0] = new LimitedRchar(0,'0');
            dl[1] = new LimitedRchar(1, '1', '2', '3', '4', '5', '6', '7', '8', '9');
            Array.Copy(Numerics(7, 2), 0, dl, 2, 7);
            var dlf00 = new DriversLicense(dl) { IssuingState = this };

            dl = new Rchar[9];
            dl[0] = new LimitedRchar(0, '1');
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);
            var dlf01 = new DriversLicense(dl) { IssuingState = this };

            dl = new Rchar[9];
            dl[0] = new LimitedRchar(0, '2');
            dl[1] = new LimitedRchar(1, '0', '1', '2', '3', '4');
            Array.Copy(Numerics(7, 2), 0, dl, 2, 7);
            var dlf02 = new DriversLicense(dl) { IssuingState = this };

            dlFormats = new[] {dlf00, dlf01, dlf02};
            PercentHighSchoolGrad = 88.6F;
            PercentCollegeGrad = 35.6F;
            _income = new LinearEquation(-2971233.00992898, 1509.98143694108);
        }
    }

    public class Delaware : UsState
    {
        public Delaware() : base("DE")
        {
            dlFormats = new[] {new DriversLicense(Numerics(7)) { IssuingState = this } };
            PercentHighSchoolGrad = 87.4F;
            PercentCollegeGrad = 28.7F;
            _income = new LinearEquation(-2483505.13518339, 1263.1272278754);
        }
    }

    public class DistrictOfColumbia : UsState
    {
        public DistrictOfColumbia() : base("DC")
        {
            var dl = new Rchar[10];
            dl[0] = new UAlphaRchar(0);
            dl[1] = new UAlphaRchar(1);
            Array.Copy(Numerics(8, 2), 0, dl, 2, 8);
            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 87.1F;
            PercentCollegeGrad = 48.5F;
            _income = new LinearEquation(-4330505.15874176, 2199.23441258091);
        }
    }

    public class Florida : UsState
    {
        public Florida() : base("FL")
        {
            var dl = new Rchar[12];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(11, 1), 0, dl, 1, 11);

            dlFormats = new[] {new DriversLicense(dl) { IssuingState = this } };

            PercentHighSchoolGrad = 85.3F;
            PercentCollegeGrad = 25.3F;
            _income = new LinearEquation(-1948686.44415658, 991.655442491506);
        }
    }

    public class Georgia : UsState
    {
        public Georgia() : base("GA")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 83.9F;
            PercentCollegeGrad = 27.5F;
            _income = new LinearEquation(-2179550.04415657, 1108.40326857846);
        }
    }

    public class Hawaii : UsState
    {
        public Hawaii() : base("HI")
        {
            var dl = new Rchar[9];
            dl[0] = new LimitedRchar(0,'H');
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl), new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 90.4F;
            PercentCollegeGrad = 29.6F;
            _income = new LinearEquation(-2035980.99346277, 1036.51316682083);
        }
    }

    public class Idaho : UsState
    {
        public Idaho() : base("ID")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            dl[1] = new UAlphaRchar(1);
            Array.Copy(Numerics(6, 2), 0, dl, 2, 6);
            dl[8] = new UAlphaRchar(8);

            dlFormats = new[] { new DriversLicense(dl), new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 88.4F;
            PercentCollegeGrad = 23.9F;
            _income = new LinearEquation(-1695991.24193642, 863.551464693173);
        }
    }

    public class Illinois : UsState
    {
        public Illinois() : base("IL")
        {
            var dl = new Rchar[12];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(11, 1), 0, dl, 1, 11);

            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 86.4F;
            PercentCollegeGrad = 30.6F;
            _income = new LinearEquation(-2442083.2197964, 1242.44162812209);
        }
    }

    public class Indiana : UsState
    {
        public Indiana() : base("IN")
        {
            var dl = new Rchar[10];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(9, 1), 0, dl, 1, 9);

            dlFormats = new[] { new DriversLicense(dl), new DriversLicense(Numerics(10)) { IssuingState = this } };
            PercentHighSchoolGrad = 86.6F;
            PercentCollegeGrad = 22.5F;
            _income = new LinearEquation(-1958684.71347511, 997.013937711981);
        }
    }

    public class Iowa : UsState
    {
        public Iowa() : base("IA")
        {
            var dl = new Rchar[9];
            for (var i = 0; i < dl.Length; i++)
            {
                dl[i] = new AlphaNumericRchar(i);
            }

            dlFormats = new[] {new DriversLicense(dl), new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 91.4F;
            PercentCollegeGrad = 25.1F;
            _income = new LinearEquation(-1909482.44409491, 971.247240209668);
        }
    }

    public class Kansas : UsState
    {
        public Kansas() : base("KS")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 89.7F;
            PercentCollegeGrad = 29.5F;
            _income = new LinearEquation(-1969152.62362004, 1001.52408263952);
        }
    }

    public class Kentucky : UsState
    {
        public Kentucky() : base("KY")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 81.7F;
            PercentCollegeGrad = 21.0F;
            _income = new LinearEquation(-1884982.19093426, 959.055134135046);
        }
    }

    public class Louisiana : UsState
    {
        public Louisiana() : base("LA")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 82.2F;
            PercentCollegeGrad = 21.4F;
            _income = new LinearEquation(-2013142.74326234, 1024.17563983964);
        }
    }

    public class Maine : UsState
    {
        public Maine() : base("ME")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7)) { IssuingState = this } };
            PercentHighSchoolGrad = 90.2F;
            PercentCollegeGrad = 26.9F;
            _income = new LinearEquation(-1783942.0522972, 907.807708911488);
        }
    }

    public class Maryland : UsState
    {
        public Maryland() : base("MD")
        {
            var dl = new Rchar[13];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(12, 1), 0, dl, 1, 12);

            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 88.2F;
            PercentCollegeGrad = 35.7F;
            _income = new LinearEquation(-2542964.29084173, 1292.69065679924);
        }
    }

    public class Massachusetts : UsState
    {
        public Massachusetts() : base("MA")
        {
            var dl = new Rchar[9];
            dl[0] = new LimitedRchar(0,
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'I',
                'J',
                'K',
                'L',
                'M',
                'N',
                'O',
                'P',
                'Q',
                'R',
                'S',
                'T',
                'U',
                'V',
                'W',
                'Y',
                'Z');// no 'X'
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl), new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 89.0F;
            PercentCollegeGrad = 38.2F;
            _income = new LinearEquation(-2872440.32580934, 1459.40222016649);
        }
    }

    public class Michigan : UsState
    {
        public Michigan() : base("MI")
        {
            var dl = new Rchar[13];
            dl[0] = new UAlphaRchar(0);
            dl[1] = new LimitedRchar(1, '1', '2', '3', '4', '5', '6');
            dl[2] = new LimitedRchar(1, '1', '2', '3', '4', '5', '6');
            dl[3] = new LimitedRchar(1, '1', '2', '3', '4', '5', '6');
            Array.Copy(Numerics(9, 4), 0, dl, 4, 9);

            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };

            PercentHighSchoolGrad = 87.9F;
            PercentCollegeGrad = 24.6F;
            _income = new LinearEquation(-2039576.15935855, 1039.49034844279);
        }
    }

    public class Minnesota : UsState
    {
        public Minnesota() : base("MN")
        {
            var dl = new Rchar[13];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(12, 1), 0, dl, 1, 12);

            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 91.5F;
            PercentCollegeGrad = 31.5F;
            _income = new LinearEquation(-2234703.520999, 1136.44939870489);
        }
    }

    public class Mississippi : UsState
    {
        public Mississippi() : base("MS")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 80.4F;
            PercentCollegeGrad = 19.6F;
            _income = new LinearEquation(-1746340.75467154, 888.21480111007);
        }
    }

    public class Missouri : UsState
    {
        public Missouri() : base("MO")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 86.8F;
            PercentCollegeGrad = 25.2F;
            _income = new LinearEquation(-2007418.99500456, 1021.32691951895);
        }
    }

    public class Montana : UsState
    {
        public Montana() : base("MT")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            dl[1] = new NumericRchar(1);
            dl[2] = new AlphaNumericRchar(2);
            Array.Copy(Numerics(6, 3), 0, dl, 3, 6);
            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };

            PercentHighSchoolGrad = 90.8F;
            PercentCollegeGrad = 27.4F;
            _income = new LinearEquation(-1512210.69133513, 770.295405488735);
        }
    }

    public class Nebraska : UsState
    {
        public Nebraska() : base("NE")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(8, 1), 0, dl, 1, 8);

            dlFormats = new[] { new DriversLicense(dl) { IssuingState = this } };

            PercentHighSchoolGrad = 89.8F;
            PercentCollegeGrad = 27.4F;
            _income = new LinearEquation(-2041114.36577236, 1037.78686401478);
        }
    }

    public class Nevada : UsState
    {
        public Nevada() : base("NV")
        {
            dlFormats = new[] {new DriversLicense(Numerics(12)) { IssuingState = this }, new DriversLicense(Numerics(10)) { IssuingState = this } };
            PercentHighSchoolGrad = 83.9F;
            PercentCollegeGrad = 21.8F;
            _income = new LinearEquation(-2039859.52001227, 1038.78772741288);
        }
    }

    public class NewHampshire : UsState
    {
        public NewHampshire() : base("NH")
        {
            var dl = new Rchar[10];
            dl[0] = new NumericRchar(0);
            dl[1] = new NumericRchar(1);
            dl[2] = new UAlphaRchar(2);
            dl[3] = new UAlphaRchar(3);
            dl[4] = new UAlphaRchar(4);
            Array.Copy(Numerics(4, 5), 0, dl, 5, 4);
            dl[9] = new LimitedRchar(9,'1','2','3','4','5','6','7','8','9');//no zero

            dlFormats = new[] {new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 91.3F;
            PercentCollegeGrad = 32.0F;
            _income = new LinearEquation(-2270957.35405481, 1154.18495220473);
        }
    }

    public class NewJersey : UsState
    {
        public NewJersey() : base("NJ")
        {
            var dl = new Rchar[15];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(14,1),0,dl,1,14);
            dlFormats = new[] {new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 87.4F;
            PercentCollegeGrad = 34.5F;
            _income = new LinearEquation(-2793172.74085714, 1420.25575084796);
        }
    }

    public class NewMexico : UsState
    {
        public NewMexico() : base("NM")
        {
            dlFormats = new[] {new DriversLicense(Numerics(9)) { IssuingState = this }, new DriversLicense(Numerics(8)) { IssuingState = this } };
            PercentHighSchoolGrad = 82.8F;
            PercentCollegeGrad = 25.3F;
            _income = new LinearEquation(-1861396.70545785, 947.295467160023);
        }
    }

    public class NewYork : UsState
    {
        public NewYork() : base("NY")
        {
            var dl = new Rchar[15];//actual length is 16 but last digit is check-digit
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(14, 1), 0, dl, 1, 14);
            dlFormats = new[] {new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 84.7F;
            PercentCollegeGrad = 32.4F;
            _income = new LinearEquation(-3030233.60055494, 1540.46512488434);
        }

        public override string RandomDriversLicense
        {
            get
            {
                var dlVal = base.RandomDriversLicense;
                var chkDigit = Etc.CalcLuhnCheckDigit(dlVal);
                var dlOut = new StringBuilder();
                dlOut.Append(dlVal);
                dlOut.Append(chkDigit.ToString());
                return dlOut.ToString();
            }
        }

        public override bool ValidDriversLicense(string dlnumber)
        {
            if (string.IsNullOrWhiteSpace(dlnumber))
                return false;
            //all but last digit
            var dlVal = dlnumber.Substring(0, dlnumber.Length - 1);
            
            var dlLastChar = dlnumber.Substring(dlnumber.Length - 1,1);
            var dlChkDigit = 0;
            if (!int.TryParse(dlLastChar, out dlChkDigit))
                return false;

            var calcChkDigit = Etc.CalcLuhnCheckDigit(dlVal);

            return base.ValidDriversLicense(dlVal) && dlChkDigit == calcChkDigit;
        }
    }

    public class NorthCarolina : UsState
    {
        public NorthCarolina() : base("NC")
        {
            dlFormats = new[] {new DriversLicense(Numerics(12)) { IssuingState = this } };
            PercentHighSchoolGrad = 84.3F;
            PercentCollegeGrad = 26.5F;
            _income = new LinearEquation(-2090850.77798328, 1062.99352451432);
        }
    }

    public class NorthDakota : UsState
    {
        public NorthDakota() : base("ND")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            dl[1] = new UAlphaRchar(1);
            dl[2] = new UAlphaRchar(2);
            Array.Copy(Numerics(6,3),0,dl,3,6);

            dlFormats = new[] { new DriversLicense(dl), new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 90.1F;
            PercentCollegeGrad = 25.8F;
            _income = new LinearEquation(-1908816.45790928, 970.423681776118);
        }
    }

    public class Ohio : UsState
    {
        public Ohio() : base("OH")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            dl[1] = new UAlphaRchar(1);
            Array.Copy(Numerics(7, 2), 0, dl, 2, 7);
            dlFormats = new[] {new DriversLicense(dl)};
            PercentHighSchoolGrad = 87.6F;
            PercentCollegeGrad = 24.1F;
            _income = new LinearEquation(-2046735.65519574, 1042.04539007091);
        }
    }

    public class Oklahoma : UsState
    {
        public Oklahoma() : base("OK")
        {
            var dl = new Rchar[10];
            dl[0] = new LimitedRchar(0,
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'J',
                'K',
                'L',
                'M',
                'N',
                'P',
                'R',
                'S',
                'T',
                'U',
                'V',
                'W',
                'Y',
                'Z');//no 'I','O', 'X' nor 'Q'
            Array.Copy(Numerics(9,1),0,dl,1,9);
            dlFormats = new[] {new DriversLicense(dl), new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 85.6F;
            PercentCollegeGrad = 22.7F;
            _income = new LinearEquation(-1966600.74289232, 1000.20468701818);
        }
    }

    public class Oregon : UsState
    {
        public Oregon() : base("OR")
        {
            dlFormats = new[] {new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 89.1F;
            PercentCollegeGrad = 29.2F;
            _income = new LinearEquation(-1991106.37582479, 1013.36731421522);
        }
    }

    public class Pennsylvania : UsState
    {
        public Pennsylvania() : base("PA")
        {
            dlFormats = new[] {new DriversLicense(Numerics(8)) { IssuingState = this } };
            PercentHighSchoolGrad = 87.9F;
            PercentCollegeGrad = 26.4F;
            _income = new LinearEquation(-2301849.69737889, 1170.99488128275);
        }
    }

    public class RhodeIsland : UsState
    {
        public RhodeIsland() : base("RI")
        {
            var dl = new Rchar[7];
            dl[0] = new LimitedRchar(0, 'V');
            Array.Copy(Numerics(6,1),0,dl,1,6);
            dlFormats = new[] {new DriversLicense(dl), new DriversLicense(Numerics(7)) { IssuingState = this } };
            PercentHighSchoolGrad = 84.7F;
            PercentCollegeGrad = 30.5F;
            _income = new LinearEquation(-2326704.60080165, 1182.75010792474);
        }
    }

    public class SouthCarolina : UsState
    {
        public SouthCarolina() : base("SC")
        {
            dlFormats = new[] { new DriversLicense(Numerics(10)) { IssuingState = this } };
            PercentHighSchoolGrad = 83.6F;
            PercentCollegeGrad = 24.3F;
            _income = new LinearEquation(-1899932.40881893, 966.364230650618);
        }
    }

    public class SouthDakota : UsState
    {
        public SouthDakota() : base("SD")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 89.9F;
            PercentCollegeGrad = 25.1F;
            _income = new LinearEquation(-1820056.19130429, 925.417391304333);
        }
    }

    public class Tennessee : UsState
    {
        public Tennessee() : base("TN")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9)), new DriversLicense(Numerics(8)) { IssuingState = this } };
            PercentHighSchoolGrad = 83.1F;
            PercentCollegeGrad = 23.0F;
            _income = new LinearEquation(-2105681.01566444, 1070.55294480418);
        }
    }

    public class Texas : UsState
    {
        public Texas() : base("TX")
        {
            dlFormats = new[] { new DriversLicense(Numerics(8)) { IssuingState = this } };
            PercentHighSchoolGrad = 79.9F;
            PercentCollegeGrad = 25.5F;
            _income = new LinearEquation(-2344516.85507238, 1191.98550724636);
        }
    }

    public class Utah : UsState
    {
        public Utah() : base("UT")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9)) { IssuingState = this } };
            PercentHighSchoolGrad = 90.4F;
            PercentCollegeGrad = 28.5F;
            _income = new LinearEquation(-1855797.12192408, 944.494172062891);
        }
    }

    public class Vermont : UsState
    {
        public Vermont() : base("VT")
        {
            var dl = new Rchar[8];
            Array.Copy(Numerics(7),0,dl,0,7);
            dl[7] = new LimitedRchar(7,'A');

            dlFormats = new[] {new DriversLicense(dl), new DriversLicense(Numerics(8)) { IssuingState = this } };
            PercentHighSchoolGrad = 91.0F;
            PercentCollegeGrad = 33.1F;
            _income = new LinearEquation(-1817722.0812827, 924.662781375256);
        }
    }

    public class Virginia : UsState
    {
        public Virginia() : base("VA")
        {
            var dl = new Rchar[9];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(8,1),0,dl,1,8);

            dlFormats = new[] {new DriversLicense(dl), new DriversLicense(Numerics(12)) { IssuingState = this } };
            PercentHighSchoolGrad = 86.6F;
            PercentCollegeGrad = 34.0F;
            _income = new LinearEquation(-2461434.97668817, 1250.96040703051);
        }
    }

    public class Washington : UsState
    {
        public Washington() : base("WA")
        {
            var dl = new Rchar[12];
            dl[0] = new UAlphaRchar(0);
            dl[1] = new UAlphaRchar(1);
            dl[2] = new UAlphaRchar(2);
            dl[3] = new UAlphaRchar(3);
            dl[4] = new UAlphaRchar(4);
            dl[5] = new UAlphaRchar(5);
            dl[6] = new UAlphaRchar(6);
            dl[7] = new AlphaNumericRchar(7);
            dl[8] = new NumericRchar(8);
            dl[9] = new LimitedRchar(9,'*');
            dl[10] = new AlphaNumericRchar(10);
            dl[11] = new AlphaNumericRchar(11);

            dlFormats = new[] {new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 89.7F;
            PercentCollegeGrad = 31.0F;
            _income = new LinearEquation(-2415417.14036378, 1228.41621954978);
        }
    }
    
    public class WestVirginia : UsState
    {
        public WestVirginia() : base("WV")
        {
            var dl = new Rchar[7];
            dl[0] = new LimitedRchar(0,'A','B','C','D','E','F','I','S','0','1','X');
            Array.Copy(Numerics(6,1),0,dl,1,6);

            dlFormats = new[] {new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 82.8F;
            PercentCollegeGrad = 17.3F;
            _income = new LinearEquation(-1751708.90866476, 892.191551032982);
        }
    }

    public class Wisconsin : UsState
    {
        public Wisconsin() : base("WI")
        {
            var dl = new Rchar[13];
            dl[0] = new UAlphaRchar(0);
            Array.Copy(Numerics(12,1),0,dl,1,12);
            dlFormats = new[] {new DriversLicense(dl) { IssuingState = this } };
            PercentHighSchoolGrad = 89.8F;
            PercentCollegeGrad = 25.7F;
            _income = new LinearEquation(-2030494.605427, 1033.01745297562);
        }

        public override string RandomDriversLicense
        {
            get
            {
                var dlVal = base.RandomDriversLicense;
                var chkDigit = Etc.CalcLuhnCheckDigit(dlVal);
                var dlOut = new StringBuilder();
                dlOut.Append(dlVal);
                dlOut.Append(chkDigit.ToString());
                return dlOut.ToString();
            }
        }

        public override bool ValidDriversLicense(string dlnumber)
        {
            if (string.IsNullOrWhiteSpace(dlnumber))
                return false;
            //all but last digit
            var dlVal = dlnumber.Substring(0, dlnumber.Length - 1);

            var dlLastChar = dlnumber.Substring(dlnumber.Length - 1, 1);
            var dlChkDigit = 0;
            if (!int.TryParse(dlLastChar, out dlChkDigit))
                return false;

            var calcChkDigit = Etc.CalcLuhnCheckDigit(dlVal);

            return base.ValidDriversLicense(dlVal) && dlChkDigit == calcChkDigit;
        }
    }

    public class Wyoming : UsState
    {
        public Wyoming() : base("WY")
        {
            dlFormats = new[] {new DriversLicense(Numerics(10)) { IssuingState = this } };
            PercentHighSchoolGrad = 91.8F;
            PercentCollegeGrad = 23.8F;
            _income = new LinearEquation(-1941425.05895769, 987.763120567361);
        }
    }
}
