using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Tort.US
{
    /// <summary>
    /// The process of taking legal action
    /// </summary>
    public interface ILitigation
    {
        /// <summary>
        /// person of entity that committed the tort
        /// </summary>
        ILegalPerson GetTortfeasor(params ILegalPerson[] persons);

        ILegalPerson GetPlaintiff(params ILegalPerson[] persons);
    }
}
