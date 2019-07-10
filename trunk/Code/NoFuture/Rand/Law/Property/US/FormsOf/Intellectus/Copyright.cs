using System;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf.Intellectus
{
    /// <summary>
    /// Like patent only dealing with works of artistic creation
    /// </summary>
    /// <remarks>
    /// Is intended to diminish &quot;private censorship&quot;.
    /// Where &quot;private censorship&quot; is when an author
    /// does not record creations to some medium because such
    /// creations will be immediately stolen.
    /// </remarks>
    [EtymologyNote("Latin","copia", "abundance", "cornucopia (aka horn-of-plenty)")]
    public class Copyright : IntellectualProperty, ILegalConcept
    {
        #region ctors
        public Copyright()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public Copyright(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public Copyright(string name, string groupName) : base(name, groupName) { }

        public Copyright(ILegalProperty property) : base(property) { }
        #endregion

        /// <summary>
        /// the essential condition of copyright
        /// </summary>
        /// <remarks>
        /// While a composition of facts is original, that does not make
        /// the facts themselves copyrightable.
        /// </remarks>
        public bool IsOriginalExpression { get; set; }

        /// <summary>
        /// the requisite level of creativity is extremely low
        /// </summary>
        public bool IsMinimalCreative { get; set; }

        public bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsOriginalExpression)
            {
                AddReasonEntry($"{nameof(Copyright)} '{Name}', {nameof(IsOriginalExpression)} is false");
                return false;
            }

            if (!IsMinimalCreative)
            {
                AddReasonEntry($"{nameof(Copyright)} '{Name}', {nameof(IsMinimalCreative)} is false");
                return false;
            }

            return true;
        }

        public bool IsEnforceableInCourt => true;
        public bool EquivalentTo(object obj)
        {
            return Equals(obj);
        }

    }
}
