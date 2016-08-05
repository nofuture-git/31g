﻿using System;
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
    /// Base type to couple a local name with an identity
    /// </summary>
    [Serializable]
    public abstract class NamedIdentifier : Identifier
    {
        protected internal string _localName = string.Empty;
        public virtual string LocalName => _localName;
    }

    /// <summary>
    /// Base type to interrelate identities
    /// </summary>
    [Serializable]
    public abstract class XrefIdentifier : NamedIdentifier
    {
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
                    System.Diagnostics.Debug.WriteLine($"Value at time of first assignment {_value}");
                }
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
            System.Diagnostics.Debug.WriteLine($"Value without Check Digit appended {valLessCk}");
            if (CheckDigitFunc == null)
                return valLessCk;

            var chkDigit = CheckDigitFunc(valLessCk);
            var rVal = $"{valLessCk}{chkDigit}";
            System.Diagnostics.Debug.WriteLine($"Value at exit of GetRandom {rVal}");
            return rVal;
        }

        public override bool Validate(string value)
        {
            if (value == null)
                return false;
            System.Diagnostics.Debug.WriteLine($"Value passed into the Validate fx {value}");
            var lastChar = value.ToCharArray().Last(char.IsDigit);
            var lessChk = value.Substring(0, value.Length - 1);
            System.Diagnostics.Debug.WriteLine($"Value passed into base.Validate {lessChk}");
            if (!base.Validate(lessChk))
            {
                System.Diagnostics.Debug.WriteLine("Validate failed prior the examining the check digit");
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
        Current = 1,
        Former = 2,
        First = 4,
        Surname = 8,
        Nickname = 16,
        Maiden = 32,
        Mother = 64,
        Father = 128,
        Adopted = 264,
        Biological = 512,
        Spouse = 1024,
        Middle = 2048,
        Legal = 4098,
        Step = 8196
    }

    [Flags]
    public enum OccidentalEdu : short
    {
        None = 0,
        Some = 1,
        Grad = 2,
        HighSchool = 16,
        Assoc = 32,
        Bachelor = 64,
        Post = 128,
    }

    [Serializable]
    public class AmericanRacePercents
    {
        public double National { get; set; }
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
}//end NoFuture.Rand

/*
 Other APIs
https://www.data.gov/developers/apis
http://www.irs.gov/uac/Tax-Stats-2
http://projects.propublica.org/nonprofits/api
https://www.pacer.gov/
 */