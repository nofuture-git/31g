using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Having some minimum contact would require one to return to the forum state.
    /// The contact derives from some voluntary relation of a defendant within a state.
    /// </summary>
    /// <remarks>
    /// Developed from <![CDATA["International Shoe v. Washington, 326 U.S. 310 (1945)"]]>
    /// for companies it typically means they are doing business within the state
    /// merely owning property is not sufficient.
    /// </remarks>
    public class MinimumContact : UnoHomine
    {
        public MinimumContact(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public MinimumContact() : base(ExtensionMethods.DefendantFx) { }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }
    }
}
