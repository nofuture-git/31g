using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Property.US.FormsOf.InTerra.Interests
{
    public abstract class LandPropertyInterestBase : PropertyBase, ILandPropertyInterest
    {
        protected LandPropertyInterestBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected LandPropertyInterestBase() : base(null)
        {
        }

        public new RealProperty SubjectProperty { get; set; }
    }
}
