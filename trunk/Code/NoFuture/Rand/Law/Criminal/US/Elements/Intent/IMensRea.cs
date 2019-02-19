using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent
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
    public interface IMensRea : IProsecution, IElement, IComparable
    {
        /// <summary>
        /// Determines if this criminal intent is valid 
        /// when combined with the particular <see cref="criminalAct"/>
        /// </summary>
        bool CompareTo(IActusReus criminalAct);
    }
}
