using System;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    public abstract class TermCategoryChoice<T> : CriminalBase, IElement where T : ITermCategory
    {
        /// <summary>
        /// The enclosure to get a portion-per-person
        /// </summary>
        public virtual Func<ILegalPerson, T> GetChoice { get; set; } = lp => default(T);
    }
}
