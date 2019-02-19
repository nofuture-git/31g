using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Criminal.Elements.Act;

namespace NoFuture.Rand.Law.US.Criminal.Elements.Intent
{
    /// <summary>
    /// the willful intent to do harm
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
    [Aka("criminal intent", "guilty mind", "vicious will")]
    public abstract class MensRea : CriminalBase, IElement, IComparable
    {
        public abstract int CompareTo(object obj);

        /// <summary>
        /// Determines if this criminal intent is valid 
        /// when combined with the particular <see cref="criminalAct"/>
        /// </summary>
        public virtual bool CompareTo(ActusReus criminalAct)
        {
            return true;
        }
    }
}
