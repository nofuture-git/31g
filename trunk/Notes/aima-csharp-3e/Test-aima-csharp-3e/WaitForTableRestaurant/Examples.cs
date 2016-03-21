using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Learning;

namespace Test_aima_csharp_3e.WaitForTableRestaurant
{

    public class Fig18Dot3
    {
        public static Func<IAttr<ValueType>, bool> BoolFuncTrue = attr => (bool)attr.Value;
        public static Func<IAttr<ValueType>, bool> BoolFuncFalse = attr => (bool)attr.Value == false;

        public static Func<IAttr<ValueType>, bool> PatSome = attr => (NumOfPatrons)attr.Value == NumOfPatrons.Some;
        public static Func<IAttr<ValueType>, bool> PatFull = attr => (NumOfPatrons)attr.Value == NumOfPatrons.Full;
        public static Func<IAttr<ValueType>, bool> PatNone = attr => (NumOfPatrons)attr.Value == NumOfPatrons.None;

        public static Func<IAttr<ValueType>, bool> PriceHigh = attr => (PriceRange)attr.Value == PriceRange.SSS;
        public static Func<IAttr<ValueType>, bool> PriceMid = attr => (PriceRange)attr.Value == PriceRange.SS;
        public static Func<IAttr<ValueType>, bool> PriceLow = attr => (PriceRange)attr.Value == PriceRange.S;

        public static Func<IAttr<ValueType>, bool> ResTypeFrench =
            attr => (KindOfRestaurant)attr.Value == KindOfRestaurant.French;
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
            var examples = new List<DecisionTreeValueType>
            {
                //x1
                new DecisionTreeValueType
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
                new DecisionTreeValueType
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
                //x3
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Positive,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HasBar(true), BoolFuncTrue),
                        new AttrValueTypeTest(new IsFriOrSat(false), BoolFuncFalse),
                        new AttrValueTypeTest(new AreHungry(false), BoolFuncFalse),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Some), PatSome),
                        new AttrValueTypeTest(new Price(PriceRange.S), PriceLow),
                        new AttrValueTypeTest(new IsRaining(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HaveReservation(false), BoolFuncFalse),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Burger), ResTypeBurger),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.WaitGt60), WaitVeryLong),
                    }
                },
                //x4
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Positive,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HasBar(false), BoolFuncFalse),
                        new AttrValueTypeTest(new IsFriOrSat(true), BoolFuncTrue),
                        new AttrValueTypeTest(new AreHungry(true), BoolFuncTrue),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Full), PatFull),
                        new AttrValueTypeTest(new Price(PriceRange.S), PriceLow),
                        new AttrValueTypeTest(new IsRaining(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HaveReservation(false), BoolFuncFalse),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Thai), ResTypeThai),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait30To60), WaitShort),
                    }
                },
                //x5
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Negative,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HasBar(false), BoolFuncFalse),
                        new AttrValueTypeTest(new IsFriOrSat(true), BoolFuncTrue),
                        new AttrValueTypeTest(new AreHungry(false), BoolFuncFalse),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Full), PatFull),
                        new AttrValueTypeTest(new Price(PriceRange.SSS), PriceHigh),
                        new AttrValueTypeTest(new IsRaining(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HaveReservation(true), BoolFuncTrue),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.French), ResTypeFrench),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.WaitGt60), WaitVeryLong),
                    }
                },
                //x6
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Positive,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HasBar(true), BoolFuncTrue),
                        new AttrValueTypeTest(new IsFriOrSat(false), BoolFuncFalse),
                        new AttrValueTypeTest(new AreHungry(true), BoolFuncTrue),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Some), PatSome),
                        new AttrValueTypeTest(new Price(PriceRange.SS), PriceMid),
                        new AttrValueTypeTest(new IsRaining(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HaveReservation(true), BoolFuncTrue),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Italian), ResTypeItalian),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait0To10), WaitVeryShort),
                    }
                },
                //x7
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Negative,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HasBar(true), BoolFuncTrue),
                        new AttrValueTypeTest(new IsFriOrSat(false), BoolFuncFalse),
                        new AttrValueTypeTest(new AreHungry(false), BoolFuncFalse),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.None), PatNone),
                        new AttrValueTypeTest(new Price(PriceRange.S), PriceLow),
                        new AttrValueTypeTest(new IsRaining(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HaveReservation(false), BoolFuncFalse),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Burger), ResTypeBurger),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait0To10), WaitVeryShort),
                    }
                },
                //x8
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Positive,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HasBar(false), BoolFuncFalse),
                        new AttrValueTypeTest(new IsFriOrSat(false), BoolFuncFalse),
                        new AttrValueTypeTest(new AreHungry(true), BoolFuncTrue),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Some), PatSome),
                        new AttrValueTypeTest(new Price(PriceRange.SS), PriceMid),
                        new AttrValueTypeTest(new IsRaining(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HaveReservation(true), BoolFuncTrue),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Thai), ResTypeThai),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait0To10), WaitVeryShort),
                    }
                },
                //x9
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Negative,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HasBar(true), BoolFuncTrue),
                        new AttrValueTypeTest(new IsFriOrSat(true), BoolFuncTrue),
                        new AttrValueTypeTest(new AreHungry(false), BoolFuncFalse),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Full), PatFull),
                        new AttrValueTypeTest(new Price(PriceRange.S), PriceLow),
                        new AttrValueTypeTest(new IsRaining(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HaveReservation(false), BoolFuncFalse),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Burger), ResTypeBurger),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.WaitGt60), WaitVeryLong),
                    }
                },
                //x10
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Negative,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HasBar(true), BoolFuncTrue),
                        new AttrValueTypeTest(new IsFriOrSat(true), BoolFuncTrue),
                        new AttrValueTypeTest(new AreHungry(true), BoolFuncTrue),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Full), PatFull),
                        new AttrValueTypeTest(new Price(PriceRange.SSS), PriceHigh),
                        new AttrValueTypeTest(new IsRaining(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HaveReservation(true), BoolFuncTrue),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Italian), ResTypeItalian),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait30To60), WaitShort),
                    }
                },
                //x11
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Negative,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(true), BoolFuncFalse),
                        new AttrValueTypeTest(new HasBar(false), BoolFuncFalse),
                        new AttrValueTypeTest(new IsFriOrSat(false), BoolFuncFalse),
                        new AttrValueTypeTest(new AreHungry(true), BoolFuncFalse),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Full), PatNone),
                        new AttrValueTypeTest(new Price(PriceRange.S), PriceLow),
                        new AttrValueTypeTest(new IsRaining(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HaveReservation(false), BoolFuncFalse),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Thai), ResTypeThai),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait30To60), WaitVeryShort),
                    }
                },
                //x12
                new DecisionTreeValueType
                {
                    Classification = BooleanClassification.Positive,
                    ConjuntionOfAttrValueTests = new List<IAttrValueTest<ValueType>>
                    {
                        new AttrValueTypeTest(new IsAlternateNearby(true), BoolFuncTrue),
                        new AttrValueTypeTest(new HasBar(false), BoolFuncTrue),
                        new AttrValueTypeTest(new IsFriOrSat(false), BoolFuncTrue),
                        new AttrValueTypeTest(new AreHungry(true), BoolFuncTrue),
                        new AttrValueTypeTest(new Patrons(NumOfPatrons.Full), PatFull),
                        new AttrValueTypeTest(new Price(PriceRange.S), PriceLow),
                        new AttrValueTypeTest(new IsRaining(false), BoolFuncFalse),
                        new AttrValueTypeTest(new HaveReservation(false), BoolFuncFalse),
                        new AttrValueTypeTest(new RestaurantType(KindOfRestaurant.Thai), ResTypeBurger),
                        new AttrValueTypeTest(new WaitEstimate(LenOfWait.Wait30To60), WaitLong),
                    }
                },
            };
        }
    }
}
