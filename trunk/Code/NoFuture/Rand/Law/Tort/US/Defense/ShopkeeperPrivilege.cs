using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Property.US;
using NoFuture.Law.US;

namespace NoFuture.Law.Tort.US.Defense
{
    /// <summary>
    /// a shopkeeper is allowed to detain a suspected shoplifter on store
    /// property for a reasonable period of time, so long as the shopkeeper
    /// has cause to believe that the person detained in fact committed, or
    /// attempted to commit, theft 
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// defense is the immediate thwarting of a wrong
    /// redress is the restoration after a wrong
    /// replevin is a redress particular to property
    /// ]]>
    /// </remarks>
    [Aka("claims of right", "recapture privilege")]
    public class ShopkeeperPrivilege : PropertyConsent, IDefense
    {
        public ShopkeeperPrivilege() : this (ExtensionMethods.Tortfeasor) {  }

        public ShopkeeperPrivilege(Func<ILegalPerson[], ILegalPerson> getSubjectPerson): base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsReasonableCause { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReasonableManner { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReasonableTime { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var person = GetSubjectPerson(persons);
            if (person == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var personType = person.GetLegalPersonTypeName();

            var rslt = WithoutConsent(persons);

            if (!IsReasonableCause(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(ShopkeeperPrivilege)} {nameof(IsReasonableCause)} is false");
                rslt = false;
            }

            if (!IsReasonableManner(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(ShopkeeperPrivilege)} {nameof(IsReasonableManner)} is false");
                rslt = false; ;
            }

            if (!IsReasonableTime(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(ShopkeeperPrivilege)} {nameof(IsReasonableTime)} is false");
                rslt = false; ;
            }

            return rslt;
        }
    }
}
