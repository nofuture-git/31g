namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent
{
    /// <summary>
    /// This is a null-object type for <see cref="MensRea"/>.  It abrogates the com-law 
    /// approach that behavior is only criminal when the defendant commits acts 
    /// with a guilty mind.
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// This was always present in com law, but with industrial rev. 
    /// it happened that, "velocities, volumes and varieties unheard 
    /// of came to subject the wayfarer to intolerable casualty risks".  
    /// Therefore, laws not requiring intent, "are in the nature of 
    /// neglect where the law requires care, or inaction where it 
    /// imposes a duty".  Thereby, breaking these laws "impairs the 
    /// efficiency of controls deemed essential to the social order". 
    /// (342 U.S. 246 (1952) MORISSETTE v. UNITED STATES.)
    /// ]]>
    /// </remarks>
    public class StrictLiability : MensRea
    {
        private const string _name = nameof(StrictLiability);
        protected internal StrictLiability()
        {

        }

        private static StrictLiability _value;
        public static StrictLiability Value => _value ?? (_value = new StrictLiability());

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }

        public override int CompareTo(object obj)
        {
            return obj is StrictLiability ? 0 : 1;
        }

        public override bool Equals(object obj)
        {
            return obj is StrictLiability;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }
}
