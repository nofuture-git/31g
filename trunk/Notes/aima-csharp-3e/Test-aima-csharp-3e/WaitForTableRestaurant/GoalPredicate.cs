using System;
using System.Collections.Generic;
using aima_csharp_3e.Learning;

namespace Test_aima_csharp_3e.WaitForTableRestaurant
{
    #region GOAL PREDICATE
    public class IsAlternateNearby : ExactlyTwoPossible
    {
        public IsAlternateNearby(bool value):base(value, "Alternate") { }
    }

    public class HasBar : ExactlyTwoPossible
    {
        public HasBar(bool value) : base(value, "Bar") { }
    }

    public class IsFriOrSat : ExactlyTwoPossible
    {
        public IsFriOrSat(DateTime dt)
            : base(dt.DayOfWeek == DayOfWeek.Friday || dt.DayOfWeek == DayOfWeek.Saturday, "Fri/Sat")
        {
        }

        public IsFriOrSat(bool value)
            : base(value, "Fri/Sat") { }
    }

    public class AreHungry : ExactlyTwoPossible
    {
        public AreHungry(bool value) : base(value, "Hungry") { }
    }

    public enum NumOfPatrons
    {
        None,
        Some,
        Full
    }

    public class Patrons : IDiscreteAttr
    {
        public Patrons(NumOfPatrons nop)
        {
            Value = nop;
            Name = "Patrons";
        }

        public ValueType Value { get; }
        public string Name { get; }
    }

    public enum PriceRange
    {
        S = 0,
        SS = 1,
        SSS = 2
    }

    public class Price : IDiscreteAttr
    {
        public Price(PriceRange value)
        {
            Value = value;
            Name = "Price";
        }

        public ValueType Value { get; }
        public string Name { get; }
    }

    public class IsRaining : ExactlyTwoPossible
    {
        public IsRaining(bool value) : base(value, "Raining") { }
    }

    public class HaveReservation : ExactlyTwoPossible
    {
        public HaveReservation(bool value) : base(value, "Reservation") { }
    }

    public enum KindOfRestaurant
    {
        French,
        Italian,
        Thai,
        Burger
    }

    public class RestaurantType : IDiscreteAttr
    {
        public RestaurantType(KindOfRestaurant kor)
        {
            Value = kor;
            Name = "Type";
        }

        public ValueType Value { get; }
        public string Name { get; }
    }

    public enum LenOfWait
    {
        Wait0To10,
        Wait10To30,
        Wait30To60,
        WaitGt60
    }

    public class WaitEstimate : IDiscreteAttr
    {
        public WaitEstimate(LenOfWait low)
        {
            Value = low;
            Name = "WaitEstimate";
        }

        public ValueType Value { get; }
        public string Name { get; }
    }

    #endregion

}
