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
            var val = new T {SubjectProperty = SubjectProperty, GetSubjectPerson = GetSubjectPerson};
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
        public IPropertyInterestFactory IsPresentInterestPossibleInfinite(Predicate<ILegalPerson> predicate, params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
            {
                return new FeeSimpleFactory(SubjectProperty, GetSubjectPerson);
            }

            return new LeaseLifeEstateFactory(SubjectProperty, GetSubjectPerson);
        }
    }

    internal class FeeSimpleFactory : PropertyInterestFactoryBase
    {
        public FeeSimpleFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// Or possibly finite
        /// </summary>
        public IPropertyInterestFactory IsPresentInterestDefinitelyInfinite(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
            {
                return new PropertyInterestFactoryValue<FeeSimpleAbsolute>(SubjectProperty, GetSubjectPerson);
            }

            return new DefeasibleFeeFactory(SubjectProperty, GetSubjectPerson);
        }
    }

    internal class DefeasibleFeeFactory : PropertyInterestFactoryBase
    {
        public DefeasibleFeeFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// or someone else
        /// </summary>
        public IPropertyInterestFactory IsFutureInterestInGrantor(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
                return new PropertyInterestFactoryValue<FeeSimpleSubject2ExecutoryInterest>(SubjectProperty, GetSubjectPerson);

            return new DefeasibleFeeFutureInterestIsGrantor(SubjectProperty, GetSubjectPerson);
        }
    }

    internal class DefeasibleFeeFutureInterestIsGrantor : PropertyInterestFactoryBase
    {
        public DefeasibleFeeFutureInterestIsGrantor(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// or require an assertion of ownership 
        /// </summary>
        public IPropertyInterestFactory IsVestOwnershipAutomatic(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
            {
                return new PropertyInterestFactoryValue<FeeSimpleDeterminable>( SubjectProperty, GetSubjectPerson);
            }

            return new PropertyInterestFactoryValue<FeeSimpleSubject2ConditionSubsequent>( SubjectProperty, GetSubjectPerson);
        }
    }


    internal class LeaseLifeEstateFactory : PropertyInterestFactoryBase
    {
        public LeaseLifeEstateFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        /// <summary>
        /// or someone else
        /// </summary>
        public IPropertyInterestFactory IsFutureInterestInGrantor(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
                return new PropertyInterestFactoryValue<Reversion>(SubjectProperty, GetSubjectPerson);

            return new RemainderFactory(SubjectProperty, GetSubjectPerson);
        }
    }

    internal class RemainderFactory : PropertyInterestFactoryBase
    {
        public RemainderFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        public IPropertyInterestFactory IsAnyUncertaintyInWhoHasRemainderInterest(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
            {
                return new OtherRemainderFactory(SubjectProperty, GetSubjectPerson);
            }

            return new PropertyInterestFactoryValue<AbsolutelyVestedRemainder>(SubjectProperty, GetSubjectPerson);
        }

        public IPropertyInterestFactory IsAnyConditionsOnRemainderInterest(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
            {
                return new OtherRemainderFactory(SubjectProperty, GetSubjectPerson);
            }

            return new PropertyInterestFactoryValue<AbsolutelyVestedRemainder>(SubjectProperty, GetSubjectPerson);
        }
    }

    internal class OtherRemainderFactory : PropertyInterestFactoryBase
    {
        public OtherRemainderFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        public IPropertyInterestFactory IsAtLeastOneMemberIdentifiedAndCertain(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
                return new PropertyInterestFactoryValue<VestedRemainderSubjectToOpen>(SubjectProperty,
                    GetSubjectPerson);

            return new PropertyInterestFactoryValue<ContingentRemainder>(SubjectProperty, GetSubjectPerson);
        }
    }

    internal class ConditionalOtherRemainderFactory : PropertyInterestFactoryBase
    {
        public ConditionalOtherRemainderFactory(RealProperty property, 
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson) { }

        public IPropertyInterestFactory IsConditionToGetItOrLoseIt(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return null;

            predicate = predicate ?? (lp => false);

            if (predicate(subj))
                return new PropertyInterestFactoryValue<ContingentRemainder>(SubjectProperty, GetSubjectPerson);

            return new PropertyInterestFactoryValue<VestedRemainderSubjectToDivestment>(SubjectProperty,
                GetSubjectPerson);
        }
    }
}
