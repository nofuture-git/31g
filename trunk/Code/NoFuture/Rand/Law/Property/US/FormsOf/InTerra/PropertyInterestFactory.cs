using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{
    public interface IPropertyInterestFactory : IRationale
    {
        IPropertyInterestFactory GetNextFactory(string predicateName, Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons);
        bool IsEnd { get; }
    }

    public abstract class PropertyInterestFactoryBase : Rationale, IPropertyInterestFactory
    {
        protected PropertyInterestFactoryBase(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            SubjectProperty = property;
            GetSubjectPerson = getSubjectPerson ?? (lps => null);
        }

        protected Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; }

        protected RealProperty SubjectProperty { get; }

        protected abstract IPropertyInterestFactory WhenTrue { get; }

        protected abstract IPropertyInterestFactory WhenFalse { get; }

        public virtual IPropertyInterestFactory GetNextFactory(string predicateName, Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return null;
            }

            var title = subj.GetLegalPersonTypeName();

            predicate = predicate ?? (lp => false);

            var predicateResult = predicate(subj);

            var result = predicateResult ? WhenTrue : WhenFalse;

            var resultTypeName = result.GetType().Name;

            if (result.IsEnd)
            {
                var interestType = result.GetType().GenericTypeArguments.FirstOrDefault();
                resultTypeName = interestType != null ? interestType.Name : resultTypeName;
            }

            AddReasonEntry($"{title} {subj.Name}, {predicateName} is {predicateResult} returning {resultTypeName}");
            result.AddReasonEntryRange(GetReasonEntries());
            return result;
        }

        public virtual bool IsEnd => false;
    }

    public class PropertyInterestFactoryValue<T> : PropertyInterestFactoryBase where T : ILandPropertyInterest, new()
    {
        public PropertyInterestFactoryValue(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        public T GetValue()
        {
            var val = new T {SubjectProperty = SubjectProperty, GetSubjectPerson = GetSubjectPerson};
            return val;
        }

        protected override IPropertyInterestFactory WhenTrue => this;
        protected override IPropertyInterestFactory WhenFalse => this;

        public override IPropertyInterestFactory GetNextFactory(string predicateName, Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return this;
        }

        public override bool IsEnd => true;
    }

    /// <summary>
    /// The manner to determine a kind of property interest
    /// </summary>
    public class PropertyInterestFactory : PropertyInterestFactoryBase
    {
        public PropertyInterestFactory(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        /// <summary>
        /// otherwise DefinitelyFinite
        /// </summary>
        public IPropertyInterestFactory IsPresentInterestPossibleInfinite(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsPresentInterestPossibleInfinite), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue => new FeeSimpleFactory(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new LeaseLifeEstateFactory(SubjectProperty, GetSubjectPerson);
    }

    internal class FeeSimpleFactory : PropertyInterestFactoryBase
    {
        public FeeSimpleFactory(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        /// <summary>
        /// Or possibly finite
        /// </summary>
        public IPropertyInterestFactory IsPresentInterestDefinitelyInfinite(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsPresentInterestDefinitelyInfinite), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue =>
            new PropertyInterestFactoryValue<FeeSimpleAbsolute>(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new DefeasibleFeeFactory(SubjectProperty, GetSubjectPerson);
    }

    internal class DefeasibleFeeFactory : PropertyInterestFactoryBase
    {
        public DefeasibleFeeFactory(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        /// <summary>
        /// or someone else
        /// </summary>
        public IPropertyInterestFactory IsFutureInterestInGrantor(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsFutureInterestInGrantor), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue =>
            new PropertyInterestFactoryValue<FeeSimpleSubject2ExecutoryInterest>(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new DefeasibleFeeFutureInterestIsGrantor(SubjectProperty, GetSubjectPerson);
    }

    internal class DefeasibleFeeFutureInterestIsGrantor : PropertyInterestFactoryBase
    {
        public DefeasibleFeeFutureInterestIsGrantor(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        /// <summary>
        /// or require an assertion of ownership 
        /// </summary>
        public IPropertyInterestFactory IsVestOwnershipAutomatic(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsVestOwnershipAutomatic), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue =>
            new PropertyInterestFactoryValue<FeeSimpleDeterminable>(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new PropertyInterestFactoryValue<FeeSimpleSubject2ConditionSubsequent>(SubjectProperty, GetSubjectPerson);
    }


    internal class LeaseLifeEstateFactory : PropertyInterestFactoryBase
    {
        public LeaseLifeEstateFactory(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        /// <summary>
        /// or someone else
        /// </summary>
        public IPropertyInterestFactory IsFutureInterestInGrantor(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsFutureInterestInGrantor), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue =>
            new PropertyInterestFactoryValue<Reversion>(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new RemainderFactory(SubjectProperty, GetSubjectPerson);
    }

    internal class RemainderFactory : PropertyInterestFactoryBase
    {
        public RemainderFactory(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        public IPropertyInterestFactory IsAnyUncertaintyInWhoHasRemainderInterest(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsAnyUncertaintyInWhoHasRemainderInterest), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue =>
            new OtherRemainderFactory(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new RemainderFactoryConditional(SubjectProperty, GetSubjectPerson);
    }

    internal class RemainderFactoryConditional : PropertyInterestFactoryBase
    {
        public RemainderFactoryConditional(RealProperty property, Func<ILegalPerson[], ILegalPerson> getSubjectPerson) :
            base(property, getSubjectPerson)
        {
        }

        public IPropertyInterestFactory IsAnyConditionsOnRemainderInterest(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsAnyConditionsOnRemainderInterest), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue =>
            new OtherRemainderFactory(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new PropertyInterestFactoryValue<AbsolutelyVestedRemainder>(SubjectProperty, GetSubjectPerson);
    }

    internal class OtherRemainderFactory : PropertyInterestFactoryBase
    {
        public OtherRemainderFactory(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        public IPropertyInterestFactory IsAtLeastOneMemberIdentifiedAndCertain(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsAtLeastOneMemberIdentifiedAndCertain), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue =>
            new PropertyInterestFactoryValue<VestedRemainderSubjectToOpen>(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new PropertyInterestFactoryValue<ContingentRemainder>(SubjectProperty, GetSubjectPerson);
    }

    internal class ConditionalOtherRemainderFactory : PropertyInterestFactoryBase
    {
        public ConditionalOtherRemainderFactory(RealProperty property,
            Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(property, getSubjectPerson)
        {
        }

        public IPropertyInterestFactory IsConditionToGetItOrLoseIt(Predicate<ILegalPerson> predicate,
            params ILegalPerson[] persons)
        {
            return GetNextFactory(nameof(IsConditionToGetItOrLoseIt), predicate, persons);
        }

        protected override IPropertyInterestFactory WhenTrue =>
            new PropertyInterestFactoryValue<ContingentRemainder>(SubjectProperty, GetSubjectPerson);

        protected override IPropertyInterestFactory WhenFalse =>
            new PropertyInterestFactoryValue<VestedRemainderSubjectToDivestment>(SubjectProperty,
                GetSubjectPerson);
    }
}
