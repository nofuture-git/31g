using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra
{
    public abstract class DefeasibleFee : PropertyBase, ILandPropertyInterest
    {
        protected DefeasibleFee(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public new RealProperty SubjectProperty { get; set; }

    }

    public class FeeSimpleSubject2ExecutoryInterest : DefeasibleFee
    {
        public FeeSimpleSubject2ExecutoryInterest(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }

    public class FeeSimpleDeterminable : DefeasibleFee
    {
        public FeeSimpleDeterminable(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }

    public class FeeSimpleSubject2ConditionSubsequent : DefeasibleFee
    {
        public FeeSimpleSubject2ConditionSubsequent(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
