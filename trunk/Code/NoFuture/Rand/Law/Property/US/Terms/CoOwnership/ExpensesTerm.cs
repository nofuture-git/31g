using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Law.Property.US.Terms.CoOwnership
{
    /// <summary>
    /// Each 
    /// </summary>
    public class ExpensesTerm : CoOwnerBaseTerm
    {
        public ExpensesTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public ExpensesTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }
    }
}
