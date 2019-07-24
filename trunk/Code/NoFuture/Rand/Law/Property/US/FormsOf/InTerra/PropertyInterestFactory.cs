using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{
    public interface IPropertyInterestFactory : IRationale
    {
    }

    public abstract class PropertyInterestFactoryBase : Rationale, IPropertyInterestFactory
    {
        protected PropertyInterestFactoryBase(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            SubjectProperty = property;
            GetSubjectPerson = getSubjectPerson;
        }

        protected Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; }

        protected RealProperty SubjectProperty { get; }
    }

    public class PropertyInterestFactoryValue<T> : PropertyInterestFactoryBase where T: ILandPropertyInterest, new()
    {
        public PropertyInterestFactoryValue(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        public T GetValue()
        {
            var val = new T {SubjectProperty = SubjectProperty};
            return val;
        }
    }

    /// <summary>
    /// The manner to determine a kind of property interest
    /// </summary>
    public class PropertyInterestFactory : PropertyInterestFactoryBase
    {
        public PropertyInterestFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// otherwise DefinitelyFinite
        /// </summary>
        public Predicate<ILegalPerson> IsPresentInterestPossibleInfinite { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            if (IsPresentInterestPossibleInfinite(subj))
            {
                var feeSimpleFactory = new FeeSimpleFactory(SubjectProperty, GetSubjectPerson);
                return feeSimpleFactory.GetPropertyInterest(persons);
            }

            var llf = new LeaseLifeEstateFactory(SubjectProperty, GetSubjectPerson);
            return llf.GetPropertyInterest(persons);
        }
    }

    internal class FeeSimpleFactory : PropertyInterestFactoryBase
    {
        public FeeSimpleFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// Or possibly finite
        /// </summary>
        public Predicate<ILegalPerson> IsPresentInterestDefinitelyInfinite { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            if (IsPresentInterestDefinitelyInfinite(subj))
                return new FeeSimpleAbsolute(GetSubjectPerson) {SubjectProperty = SubjectProperty};

            var defeasibleFactory = new DefeasibleFeeFactory(SubjectProperty, GetSubjectPerson);
            return defeasibleFactory.GetPropertyInterest(persons);
        }
    }

    internal class DefeasibleFeeFactory : PropertyInterestFactoryBase
    {
        public DefeasibleFeeFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// or someone else
        /// </summary>
        public Predicate<ILegalPerson> IsFutureInterestInGrantor { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            if (IsFutureInterestInGrantor(subj))
                return new FeeSimpleSubject2ExecutoryInterest(GetSubjectPerson) {SubjectProperty = SubjectProperty};

            var factory = new DefeasibleFeeFutureInterestIsGrantor(SubjectProperty, GetSubjectPerson);
            return factory.GetPropertyInterest(persons);
        }
    }

    internal class DefeasibleFeeFutureInterestIsGrantor : PropertyInterestFactoryBase
    {
        public DefeasibleFeeFutureInterestIsGrantor(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// or require an assertion of ownership 
        /// </summary>
        public Predicate<ILegalPerson> IsVestOwnershipAutomatic { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            if (IsVestOwnershipAutomatic(subj))
            {
                return new FeeSimpleDeterminable(GetSubjectPerson) {SubjectProperty = SubjectProperty};
            }

            return new FeeSimpleSubject2ConditionSubsequent(GetSubjectPerson) {SubjectProperty = SubjectProperty};
        }
    }


    internal class LeaseLifeEstateFactory : PropertyInterestFactoryBase
    {
        public LeaseLifeEstateFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// or someone else
        /// </summary>
        public Predicate<ILegalPerson> IsFutureInterestInGrantor { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            if (IsFutureInterestInGrantor(subj))
                return new Reversion(GetSubjectPerson) {SubjectProperty = SubjectProperty};

            var factory = new RemainderFactory(SubjectProperty, GetSubjectPerson);
            return factory.GetPropertyInterest(persons);
        }
    }

    internal class RemainderFactory : PropertyInterestFactoryBase
    {
        public RemainderFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        public Predicate<ILegalPerson> IsAnyUncertaintyInWhoHasRemainderInterest { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsAnyConditionsOnRemainderInterest { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            var condOn = IsAnyConditionsOnRemainderInterest(subj);
            var uncertain = IsAnyUncertaintyInWhoHasRemainderInterest(subj);

            //TODO what if they are both true?
            if (!condOn && !uncertain)
                return new AbsolutelyVestedRemainder(GetSubjectPerson) {SubjectProperty = SubjectProperty};

            if (uncertain)
            {
                var factory00 = new OtherRemainderFactory(SubjectProperty, GetSubjectPerson);
                return factory00.GetPropertyInterest(persons);
            }

            var factory = new ConditionalOtherRemainderFactory(SubjectProperty, GetSubjectPerson);
            return factory.GetPropertyInterest(persons);
        }
    }

    internal class OtherRemainderFactory : PropertyInterestFactoryBase
    {
        public OtherRemainderFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        public Predicate<ILegalPerson> IsAtLeastOneMemberIdentifiedAndCertain { get; set; } = lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            if (IsAtLeastOneMemberIdentifiedAndCertain(subj))
                return new VestedRemainderSubjectToOpen(GetSubjectPerson) {SubjectProperty = SubjectProperty};

            return new ContingentRemainder(GetSubjectPerson) {SubjectProperty = SubjectProperty};
        }
    }

    internal class ConditionalOtherRemainderFactory : PropertyInterestFactoryBase
    {
        public ConditionalOtherRemainderFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        public Predicate<ILegalPerson> IsConditionToGetItOrLoseIt { get; set; } =
            lp => false;

        public ILandPropertyInterest GetPropertyInterest(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            if (IsConditionToGetItOrLoseIt(subj))
                return new ContingentRemainder(GetSubjectPerson) {SubjectProperty = SubjectProperty};

            return new VestedRemainderSubjectToDivestment(GetSubjectPerson) {SubjectProperty = SubjectProperty};
        }
    }
}
