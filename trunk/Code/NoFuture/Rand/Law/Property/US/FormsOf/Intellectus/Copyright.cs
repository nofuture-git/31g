using System;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law.Property.US.FormsOf.Intellectus
{
    /// <summary>
    /// Like patent only dealing with works of artistic creation
    /// </summary>
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
                AddReasonEntry($"{nameof(Copyright)} named '{Name}', {nameof(IsOriginalExpression)} is false");
                return false;
            }

            if (!IsMinimalCreative)
            {
                AddReasonEntry($"{nameof(Copyright)} named '{Name}', {nameof(IsMinimalCreative)} is false");
                return false;
            }

            return true;
        }

        public bool IsEnforceableInCourt => true;
        public bool EquivalentTo(object obj)
        {
            return Equals(obj);
        }
        public override string ToString()
        {
            return string.Join(Environment.NewLine, GetReasonEntries());
        }
    }
}
