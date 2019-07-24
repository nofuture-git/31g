using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{
    public abstract class LifeEstate : PropertyBase, ILandPropertyInterest, ITempore
    {
        protected LifeEstate(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public new RealProperty SubjectProperty { get; set; }
        public DateTime Inception { get; set; }
        public DateTime? Terminus { get; set; }
        public bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }
    }

    public class Reversion : LifeEstate
    {
        public Reversion(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Remainder : LifeEstate
    {
        protected Remainder(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

    }

    [Aka("indefeasibly vested remainder")]
    public class AbsolutelyVestedRemainder : Remainder
    {
        public AbsolutelyVestedRemainder(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }

    public class ContingentRemainder : Remainder
    {
        public ContingentRemainder(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }

    public class VestedRemainderSubjectToDivestment : Remainder
    {
        public VestedRemainderSubjectToDivestment(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The Rule of Convenience is the idea the a future interest becomes actual upon any one qualified to make a claim and does so.
    /// </summary>
    [Aka("vested remainder subject to partial defeasance")]
    public class VestedRemainderSubjectToOpen : Remainder
    {
        public VestedRemainderSubjectToOpen(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
