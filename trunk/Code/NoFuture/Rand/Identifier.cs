using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand
{
    /// <summary>
    /// A preeminent identity type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdentifier<T> : ICited
    {
        string Abbrev { get; }
        T Value { get; set; }
    }

    /// <summary>
    /// Any type whose source is explicit
    /// </summary>
    public interface ICited
    {
        string Src { get; set; }
    }

    /// <summary>
    /// Any type which could be given a name.
    /// </summary>
    /// <remarks>
    /// Latin for 'be called'
    /// </remarks>
    public interface IVoca
    {
        List<Tuple<KindsOfNames, string>> Names { get; }
        void UpsertName(KindsOfNames k, string name);
        string GetName(KindsOfNames k);
        bool AnyOfKindOfName(KindsOfNames k);
        bool AnyOfNameAs(string name);
        bool RemoveNameByKind(KindsOfNames k);
        int RemoveNameByValue(string name);
    }

    /// <summary>
    /// Any type which has a count-of and an identifier
    /// </summary>
    /// <remarks>Latin for 'be counted'</remarks>
    public interface INumera
    {
        decimal Amount { get; }
        Identifier Id { get; }
    }

    [Serializable]
    public class VocaBase : IVoca
    {
        public List<Tuple<KindsOfNames, string>> Names { get; } = new List<Tuple<KindsOfNames, string>>();

        public virtual void UpsertName(KindsOfNames k, string name)
        {
            var cname = Names.FirstOrDefault(x => x.Item1 == k);

            if (cname != null)
            {
                Names.Remove(cname);
            }
            Names.Add(new Tuple<KindsOfNames, string>(k, name));
        }

        public virtual string GetName(KindsOfNames k)
        {
            var cname = Names.FirstOrDefault(x => x.Item1 == k);
            return cname?.Item2;
        }

        public virtual bool AnyOfKindOfName(KindsOfNames k)
        {
            return Names.Any(x => x.Item1 == k);
        }

        public virtual bool AnyOfNameAs(string name)
        {
            return Names.Any(x => string.Equals(x.Item2, name));
        }

        public bool RemoveNameByKind(KindsOfNames k)
        {
            var cname = Names.FirstOrDefault(x => x.Item1 == k);
            if (cname == null)
                return false;
            Names.Remove(cname);
            return true;
        }

        public int RemoveNameByValue(string name)
        {
            var cnt = 0;
            var byName = Names.Where(x => string.Equals(x.Item2, name)).ToList();
            foreach (var cname in byName)
            {
                Names.Remove(cname);
                cnt += 1;
            }
            return cnt;
        }
    }

    /// <summary>
    /// Base type for string identity values
    /// </summary>
    [Serializable]
    public abstract class Identifier : IIdentifier<string>
    {
        private string _value;

        public abstract string Abbrev { get; }

        public virtual string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public virtual string Src { get; set; }
        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public bool Equals(Identifier obj)
        {
            return string.Equals(obj.Value, Value);
        }

        public override bool Equals(object obj)
        {
            var id = obj as Identifier;
            return id != null && Equals(id);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }

    /// <summary>
    /// A base identifier which is tied to some span of time
    /// </summary>
    [Serializable]
    public abstract class DiachronIdentifier : Identifier
    {
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }

        protected internal bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = FromDate == null || FromDate <= dt;
            var beforeOrOnToDt = ToDate == null || ToDate.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }
    }

    /// <summary>
    /// An implementation to order <see cref="DiachronIdentifier"/>
    /// </summary>
    [Serializable]
    public class DiachronIdComparer : IComparer<DiachronIdentifier>
    {
        public int Compare(DiachronIdentifier x, DiachronIdentifier y)
        {
            if (x?.FromDate == null && y?.FromDate == null)
                return 0;
            if (x?.FromDate == null)
                return 1;
            if (y?.FromDate == null)
                return -1;
            return DateTime.Compare(x.FromDate.Value, y.FromDate.Value);
        }
    }

    /// <summary>
    /// Base type to couple a local name with an identity
    /// </summary>
    [Serializable]
    public abstract class NamedIdentifier : Identifier
    {
        protected NamedIdentifier() { }
        protected NamedIdentifier(string localName)
        {
            _localName = localName;
        }
        protected internal string _localName = string.Empty;
        public virtual string LocalName => _localName;
    }

    /// <summary>
    /// Base type to interrelate identities
    /// </summary>
    [Serializable]
    public abstract class XrefIdentifier : NamedIdentifier
    {
        protected XrefIdentifier() { }
        protected XrefIdentifier(string localName) : base(localName) { }
        private readonly Dictionary<string, string> _refDict = new Dictionary<string, string>();
        public virtual Dictionary<string, string> ReferenceDictionary => _refDict;
        public virtual NamedIdentifier[] XrefIds { get; set; }
    }

    /// <summary>
    /// Base type for identifiers whose value is derived 
    /// from an array of <see cref="Rchar"/>(s).
    /// </summary>
    /// <remarks>
    /// Given the format as an ordered-array of <see cref="Rchar"/>
    /// this type can both create random values and validate them.
    /// </remarks>
    [Serializable]
    public abstract class RIdentifier : Identifier
    {
        #region fields
        protected Rchar[] format;
        protected string _value;
        #endregion

        #region methods
        public virtual string GetRandom()
        {
            var dl = new char[format.Length];
            for (var i = 0; i < format.Length; i++)
            {
                dl[i] = format[i].Rand;
            }
            return new string(dl);
        }
        public override string Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_value))
                {
                    _value = GetRandom();
                }
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public virtual bool Validate(string value)
        {
            return format.All(rc => rc.Valid(value));
        }
        #endregion
    }

    /// <summary>
    /// An extension of <see cref="RIdentifier"/> having a f(x) pointer 
    /// to some algo for a check digit.
    /// </summary>
    [Serializable]
    public abstract class RIdentifierWithChkDigit : RIdentifier
    {
        protected internal Func<string, int> CheckDigitFunc;

        public override string GetRandom()
        {
            var valLessCk = base.GetRandom();
            if (CheckDigitFunc == null)
                return valLessCk;

            var chkDigit = CheckDigitFunc(valLessCk);
            var rVal = $"{valLessCk}{chkDigit}";
            return rVal;
        }

        public override bool Validate(string value)
        {
            if (value == null)
                return false;
            var lastChar = value.ToCharArray().Last(char.IsDigit);
            var lessChk = value.Substring(0, value.Length - 1);
            if (!base.Validate(lessChk))
            {
                return false;
            }
            int chkDigit;
            if (!int.TryParse(lastChar.ToString(), out chkDigit))
            {
                return false;
            }
            var calcChkDigit = CheckDigitFunc(lessChk);
            return chkDigit == calcChkDigit;
        }
    }

    [Serializable]
    public enum MaritialStatus
    {
        Unknown,
        Single,
        Married,
        Divorced,
        Separated,
        Remarried,
        Widowed
    }

    [Serializable]
    [Flags]
    public enum KindsOfLabels :byte
    {
        None = 0,
        Home = 1,
        Work = 2,
        Mobile = 4,
    }

    [Serializable]
    public enum Gender
    {
        Unknown,
        Male,
        Female
    }

    [Serializable]
    [Flags]
    public enum KindsOfNames : short
    {
        None = 0,
        Former = 1,
        First = 2,
        Surname = 4,
        Abbrev = 8,
        Maiden = 16,
        Mother = 32,
        Father = 64,
        Adopted = 128,
        Biological = 264,
        Spouse = 512,
        Middle = 1024,
        Legal = 2048,
        Step = 4098
    }

    [Serializable]
    [Flags]
    public enum OccidentalEdu : short
    {
        None = 0,
        Some = 1,
        Grad = 2,
        HighSchool = 16,
        Assoc = 32,
        Bachelor = 64,
        Master = 128,
        Doctorate = 256
    }

    [Flags]
    [Serializable]
    public enum NorthAmericanRace : byte
    {
        White = 1,
        Hispanic = 2,
        Black = 4,
        Asian = 8,
        AmericanIndian = 16,
        Pacific = 32,
        Mixed = 64
    }

    [Flags]
    [Serializable]
    public enum UrbanCentric : short
    {
        City = 0,
        Suburb = 1,
        Town = 2,
        Rural = 4,
        Large = 8,
        Midsize = 16,
        Small = 32,
        Distant = 64,
        Fringe = 128,
        Remote = 256
    }

    [Serializable]
    public enum AmericanRegion
    {
        Northeast,
        South,
        Midwest,
        West
    }
}

/*
 Other APIs
https://www.data.gov/developers/apis
http://www.irs.gov/uac/Tax-Stats-2
http://projects.propublica.org/nonprofits/api
https://www.pacer.gov/
https://nccd.cdc.gov/NPAO_DTM/

this looks fun
https://www.treasury.gov/resource-center/sanctions/SDN-List/Pages/default.aspx

#search LOINC
https://apps.nlm.nih.gov/medlineplus/services/mpconnect_service.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=2093-3&knowledgeResponseType=application/javascript

#another drug search 
https://apps.nlm.nih.gov/medlineplus/services/mpconnect_service.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.88?mainSearchCriteria.v.c=637188&knowledgeResponseType=application/javascript

#api docx
https://rxnav.nlm.nih.gov/RxNormAPIREST.html

#get a list of a bunch of drugs
https://rxnav.nlm.nih.gov/REST/allconcepts?tty=BN+BPCK

#some kind of standard used to send Rx to pharmacy - requires a membership
http://www.ncpdp.org/Standards-Development/Standards-Information

#way to lookup the ICD-10 codes - requires a API Key
# this api is strange - use the api-key to get another token, then use 
# that token to get yet another token
https://documentation.uts.nlm.nih.gov/rest/home.html

#.gov domains
https://inventory.data.gov/dataset/b2c31002-8a5e-4cd0-85bd-6971934f4e59/resource/02706c7b-98e3-4267-8dd8-e3bb2d8c7ce3/download/fed-domains-04212017.csv

#money laundering
http://www.opensanctions.org/
 */
