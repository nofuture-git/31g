using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Exceptions;

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
    /// Base type for string identity values
    /// </summary>
    [Serializable]
    public abstract class Identifier : IIdentifier<string>
    {
        public abstract string Abbrev { get; }
        public virtual string Value { get; set; }
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
            return Value == null ? 0 : Value.GetHashCode();
        }
    }

    /// <summary>
    /// Base type to couple a local name with an identity
    /// </summary>
    [Serializable]
    public abstract class NamedIdentifier : Identifier
    {
        protected internal string _localName = string.Empty;
        public virtual string LocalName { get { return _localName; } }
    }

    /// <summary>
    /// Base type to interrelate identities
    /// </summary>
    [Serializable]
    public abstract class XrefIdentifier : NamedIdentifier
    {
        private readonly Dictionary<string, string> _refDict = new Dictionary<string, string>();
        public virtual Dictionary<string, string> ReferenceDictionary { get {return _refDict;}}
        public virtual NamedIdentifier[] XrefIds { get; set; }
    }

    /// <summary>
    /// Base type for identifiers whose value is derived 
    /// from an array of <see cref="Rchar"/>(s).
    /// </summary>
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
                    _value = GetRandom();
                return _value;
            }
            set
            {
                _value = value;
                if (!Validate(_value))
                    throw new RahRowRagee(string.Format("The value given of '{0}' is not valid for this instance.", _value));
            }
        }
        public virtual bool Validate(string value)
        {
            return format.All(rc => rc.Valid(value));
        }
        #endregion
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
    public enum KindsOfLabels
    {
        None,
        Home,
        Work,
        Mobile,
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
    public enum KindsOfPersonalNames : short
    {
        None = 0,
        Current = 1,
        Former = 2,
        Firstname = 4,
        Surname = 8,
        Nickname = 16,
        Father = 32,
        Mother = 64,
        Adopted = 128,
        Biological = 264,
        Spouse = 512,
        Middle = 1024,
    }

    [Flags]
    public enum OccidentalEdu : short
    {
        Empty = 0,
        HighSchool = 1,
        College = 2,
        Some = 16,
        Grad = 32
    }

    [Serializable]
    public class AmericanRacePercents
    {
        public double AmericanIndian { get; set; }
        public double Asian { get; set; }
        public double Hispanic { get; set; }
        public double Black { get; set; }
        public double White { get; set; }
        public double Pacific { get; set; }
        public double Mixed { get; set; }
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

    [Serializable]
    public enum UrbanCentric
    {
        CityLarge,
        CityMidsize,
        CitySmall,
        RuralDistant,
        RuralFringe,
        RuralRemote,
        SuburbLarge,
        SuburbMidsize,
        SuburbSmall,
        TownDistant,
        TownFringe,
        TownRemote,
    }
}//end NoFuture.Rand

/*
 Other APIs
https://www.data.gov/developers/apis
http://www.irs.gov/uac/Tax-Stats-2
http://projects.propublica.org/nonprofits/api
https://www.pacer.gov/
 */