using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse
{
    public class Entrapment : DefenseBase
    {
        public Entrapment(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// (1) an unprovoked attack
        /// </summary>
        public Provacation Provacation { get; set; }

        /// <summary>
        /// (2) an attack which threatens imminent injury or death 
        ///     to a person or or damage, destruction, or theft to real or personal property
        /// </summary>
        public Imminence Imminence { get; set; }

        /// <summary>
        /// (3) an objectively reasonable degree of force, used in response
        /// </summary>
        public Proportionality<ITermCategory> Proportionality { get; set; }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            throw new NotImplementedException();
        }
    }
}
