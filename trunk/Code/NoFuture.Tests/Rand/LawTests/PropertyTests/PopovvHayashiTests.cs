using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Property.US.Acquisition;
using NoFuture.Rand.Law.Property.US.Acquisition.Found;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    /// <summary>
    /// Popov v. Hayashi, No. 400545, 2002 WL 31833731 (Cal. Superior, Dec. 18, 2002)
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// doctrine issue, illustrate concept of taking possession, and resolution by way of equitable division
    /// ]]>
    /// </remarks>
    [TestFixture]
    public class PopovvHayashiTests
    {
        [Test]
        public void PopovvHayashi()
        {
            var baseball = new HistoricBaseball();
            var mlb = new MajorLeagueBaseball();
            baseball.EntitledTo = mlb;
            baseball.InPossessionOf = mlb;

            var hitBall = new AbandonedProperty(ExtensionMethods.Defendant)
            {
                SubjectProperty = baseball,
                Relinquishment = new Act(ExtensionMethods.ThirdParty)
                {
                    IsAction = lp => lp is MajorLeagueBaseball,
                    IsVoluntary = lp => lp is MajorLeagueBaseball
                }
            };

            var testResult = hitBall.IsValid(new Popov(), new Hayashi(), new MajorLeagueBaseball());
            Assert.IsTrue(testResult);
            
            baseball.InPossessionOf = new Hayashi();

            var test = new TakePossession(ExtensionMethods.Plaintiff)
            {
                ClaimantAction = new CompleteControlOverTheBall(ExtensionMethods.Plaintiff)
                {
                    IsVoluntary = lp => true,
                    IsAction = lp => true,
                    IsMomentumOfBallAndFanCeased = lp => lp is Hayashi
                },
                //court doesn't consider thoughts relevant
                ClaimantIntent = null,
                SubjectProperty = baseball
            };
            testResult = test.IsValid(new Popov(), new Hayashi(), new MajorLeagueBaseball());
            Assert.IsFalse(testResult);
            Console.WriteLine(test.ToString());

        }
    }

    [Aka("Gary's Rule")]
    public class CompleteControlOverTheBall : Act
    {
        public CompleteControlOverTheBall(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public Predicate<ILegalPerson> IsMomentumOfBallAndFanCeased { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (!IsMomentumOfBallAndFanCeased(subj))
            {
                AddReasonEntry($"{subj.GetLegalPersonTypeName()} {subj.Name}, {nameof(IsMomentumOfBallAndFanCeased)} is false");
                return false;
            }
            return base.IsValid(persons);
        }
    }

    [Aka("Bernhardt & Finkelman's Rule")]
    public class StopForwardMomentumOfTheBall : Act
    {
        public StopForwardMomentumOfTheBall(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public Predicate<ILegalPerson> IsStoppingForwardMomentum { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            return base.IsValid(persons) && IsStoppingForwardMomentum(subj);
        }
    }
    

    public class Popov : LegalPerson, IPlaintiff
    {
        public Popov(): base("Popov") { }
    }

    public class Hayashi : LegalPerson, IDefendant
    {
        public Hayashi(): base("Hayashi") { }
    }

    public class MajorLeagueBaseball : LegalPerson, IThirdParty
    {
        public MajorLeagueBaseball() : base("Major League Baseball") { }
    }

    public class HistoricBaseball : TangiblePersonalProperty
    {
        public HistoricBaseball() : base("Barry Bonds seventy third home run baseball.") {  }
    }
}
