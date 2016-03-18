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

    public class Fig18Dot3
    {
        public static Func<IAttr<ValueType>, bool> BoolFuncTrue = attr => (bool) attr.Value;
        public static Func<IAttr<ValueType>, bool> BoolFuncFalse = attr => (bool)attr.Value == false;

        public static Func<IAttr<ValueType>, bool> PatSome = attr => (NumOfPatrons)attr.Value == NumOfPatrons.Some;
        public static Func<IAttr<ValueType>, bool> PatFull = attr => (NumOfPatrons)attr.Value == NumOfPatrons.Full;
        public static Func<IAttr<ValueType>, bool> PatNone = attr => (NumOfPatrons)attr.Value == NumOfPatrons.None;

        public static Func<IAttr<ValueType>, bool> PriceHigh = attr => (PriceRange)attr.Value == PriceRange.SSS;
        public static Func<IAttr<ValueType>, bool> PriceMid = attr => (PriceRange)attr.Value == PriceRange.SS;
        public static Func<IAttr<ValueType>, bool> PriceLow = attr => (PriceRange)attr.Value == PriceRange.S;

        public static Func<IAttr<ValueType>, bool> ResTypeFrench =
            attr => (KindOfRestaurant) attr.Value == KindOfRestaurant.French;
        public static Func<IAttr<ValueType>, bool> ResTypeThai =
            attr => (KindOfRestaurant)attr.Value == KindOfRestaurant.Thai;
        public static Func<IAttr<ValueType>, bool> ResTypeItalian =
            attr => (KindOfRestaurant)attr.Value == KindOfRestaurant.Italian;
        public static Func<IAttr<ValueType>, bool> ResTypeBurger =
            attr => (KindOfRestaurant)attr.Value == KindOfRestaurant.Burger;

        public static Func<IAttr<ValueType>, bool> WaitVeryShort = attr => (LenOfWait)attr.Value == LenOfWait.Wait0To10;
        public static Func<IAttr<ValueType>, bool> WaitShort = attr => (LenOfWait)attr.Value == LenOfWait.Wait10To30;
        public static Func<IAttr<ValueType>, bool> WaitLong = attr => (LenOfWait)attr.Value == LenOfWait.Wait30To60;
        public static Func<IAttr<ValueType>, bool> WaitVeryLong = attr => (LenOfWait)attr.Value == LenOfWait.WaitGt60;


        public static void CtorExample()
        {
            var examples = new List<PathToLeafValueType>
            {
                //x1
                new PathToLeafValueType
                {
                    Classification = BooleanClassification.Positive,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HasBar(false), BoolFuncFalse),
                        new AttrValueTypeTest(new IsFriOrSat(false), BoolFuncFalse),
                        new AttrValueTypeTest(new AreHungry(true), BoolFuncTrue),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Some), PatSome),
                        new AttrValueTypeTest(new Price(PriceRange.SSS), PriceHigh),
                        new AttrValueTypeTest(new IsRaining(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HaveReservation(true), BoolFuncTrue),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.French), ResTypeFrench),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait0To10), WaitVeryShort),
                    }
                },
                //x2
                new PathToLeafValueType
                {
                    Classification = BooleanClassification.Negative,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HasBar(false), BoolFuncFalse),
                        new AttrValueTypeTest(new IsFriOrSat(false), BoolFuncFalse),
                        new AttrValueTypeTest(new AreHungry(true), BoolFuncTrue),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Full), PatFull),
                        new AttrValueTypeTest(new Price(PriceRange.S), PriceLow),
                        new AttrValueTypeTest(new IsRaining(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HaveReservation(false), BoolFuncFalse),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Thai), ResTypeThai),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait30To60), WaitLong),
                    }
                },

                //TODO examples to x12 (at location 16859)
            };
        }
    }
}
