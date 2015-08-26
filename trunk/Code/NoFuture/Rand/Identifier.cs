using System;

namespace NoFuture.Rand
{
    public interface IIdentifier<T>
    {
        string Abbrev { get; }
        T Value { get; set; }
    }

    [Serializable]
    public abstract class Identifier : IIdentifier<string>
    {
        public abstract string Abbrev { get; }
        public virtual string Value { get; set; }
        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public bool Equals(Identifier obj)
        {
            return string.Equals(obj.Value, Value);
        }
    }

    /// <summary>
    /// Basic Money object pattern 
    /// </summary>
    /// <remarks>
    /// Is Latin for Money
    /// </remarks>
    [Serializable]
    public struct Pecuniam
    {
        public Pecuniam(Decimal amount)
        {
            Amount = amount;
            Currency = "US";
        }
        public Decimal Amount;
        public string Currency;
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
        Nickname = 4,
        FatherSurname = 8,
        MotherSurname = 16,
        Adopted = 32,
        Biological = 64,
        Spouse = 128,
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