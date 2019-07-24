using System;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    [TestFixture]
    public class PropertyInterestFactoryTests
    {
        [Test]
        public void TestApiLookAndStyle()
        {
            var test00 =
                new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant)
                    .IsPresentInterestPossibleInfinite(lp => true, new CurtisLandholder()) as FeeSimpleFactory;

            Assert.IsNotNull(test00 );

            var test01 = test00.IsPresentInterestDefinitelyInfinite(lp => false, new CurtisLandholder()) as DefeasibleFeeFactory;

            Assert.IsNotNull(test01);

            var test02 =
                test01.IsFutureInterestInGrantor(lp => true, new CurtisLandholder()) as
                    PropertyInterestFactoryValue<FeeSimpleSubject2ExecutoryInterest>;

            Assert.IsNotNull(test02);

            var testResult = test02.GetValue();

            Assert.IsNotNull(testResult);

            Assert.IsInstanceOf<FeeSimpleSubject2ExecutoryInterest>(testResult);

            Console.WriteLine(test02.ToString());

        }

        [Test]
        public void TestAllPaths()
        {
            IPropertyInterestFactory test =
                new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var curtis = new CurtisLandholder();
            var count = 0;
            var test2FeeSimpleAbs = new[] {true, true};

            foreach (var p in test2FeeSimpleAbs)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory00 = test as PropertyInterestFactoryValue<FeeSimpleAbsolute>;
            Assert.IsNotNull(testResultFactory00);

            Assert.IsInstanceOf<FeeSimpleAbsolute>(testResultFactory00.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2FeeSimpleSubj2ExecInterest = new[] {true, false, true};
            foreach (var p in test2FeeSimpleSubj2ExecInterest)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory01 = test as PropertyInterestFactoryValue<FeeSimpleSubject2ExecutoryInterest>;
            Assert.IsNotNull(testResultFactory01);

            Assert.IsInstanceOf<FeeSimpleSubject2ExecutoryInterest>(testResultFactory01.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2FeeSimpleDeterminable = new[] { true, false, false, true };
            foreach (var p in test2FeeSimpleDeterminable)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory02 = test as PropertyInterestFactoryValue<FeeSimpleDeterminable>;
            Assert.IsNotNull(testResultFactory02);

            Assert.IsInstanceOf<FeeSimpleDeterminable>(testResultFactory02.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2FeeSubj2CondSubseq = new[] {true, false, false, false};
            foreach (var p in test2FeeSubj2CondSubseq)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory03 = test as PropertyInterestFactoryValue<FeeSimpleSubject2ConditionSubsequent>;
            Assert.IsNotNull(testResultFactory03);

            Assert.IsInstanceOf<FeeSimpleSubject2ConditionSubsequent>(testResultFactory03.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2Reversion = new[] {false, true};
            foreach (var p in test2Reversion)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory04 = test as PropertyInterestFactoryValue<Reversion>;
            Assert.IsNotNull(testResultFactory04);

            Assert.IsInstanceOf<Reversion>(testResultFactory04.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2VestRemainSubjToOpen00 = new[] {false, false, true, true};
            foreach (var p in test2VestRemainSubjToOpen00)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory05 = test as PropertyInterestFactoryValue<VestedRemainderSubjectToOpen>;
            Assert.IsNotNull(testResultFactory05);

            Assert.IsInstanceOf<VestedRemainderSubjectToOpen>(testResultFactory05.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2VestRemainSubjToOpen01 = new[] { false, false, false, true, true };
            foreach (var p in test2VestRemainSubjToOpen01)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory06 = test as PropertyInterestFactoryValue<VestedRemainderSubjectToOpen>;
            Assert.IsNotNull(testResultFactory06);

            Assert.IsInstanceOf<VestedRemainderSubjectToOpen>(testResultFactory06.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2ContingentRemainder00 = new[] {false, false, true, false};
            foreach (var p in test2ContingentRemainder00)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory07 = test as PropertyInterestFactoryValue<ContingentRemainder>;
            Assert.IsNotNull(testResultFactory07);

            Assert.IsInstanceOf<ContingentRemainder>(testResultFactory07.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2ContingentRemainder01 = new[] {false, false, false, true, false};
            foreach (var p in test2ContingentRemainder01)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory08 = test as PropertyInterestFactoryValue<ContingentRemainder>;
            Assert.IsNotNull(testResultFactory08);

            Assert.IsInstanceOf<ContingentRemainder>(testResultFactory08.GetValue());

            count = 0;
            test = new PropertyInterestFactory(new RealProperty("some land"), ExtensionMethods.Defendant);
            var test2AbsVestedRemainder = new[] {false, false, false, false};
            foreach (var p in test2AbsVestedRemainder)
            {
                test = test.GetNextFactory(count.ToString(), lp => p, curtis);
                count += 1;
            }

            var testResultFactory09 = test as PropertyInterestFactoryValue<AbsolutelyVestedRemainder>;
            Assert.IsNotNull(testResultFactory09);

            Assert.IsInstanceOf<AbsolutelyVestedRemainder>(testResultFactory09.GetValue());

        }

        public class CurtisLandholder : LegalPerson, IDefendant
        {

        }
    }
}
