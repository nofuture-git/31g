using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf.Intellectus
{
    /// <summary>
    /// 35 U.S.C. Section 101, promoting &quot;the Progress of Science and the useful Arts&quot;
    /// </summary>
    public class Patent : IntellectualProperty, ILegalConcept
    {
        private readonly HashSet<Tuple<bool, string>> _truePropositions = new HashSet<Tuple<bool, string>>();

        #region ctors
        public Patent()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public Patent(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public Patent(string name, string groupName) : base(name, groupName) { }

        public Patent(ILegalProperty property) : base(property) { }
        #endregion

        [Aka("new process", "new machine", "new manufacture", 
            "new composition of matter", "existing improvement")]
        public bool IsSubjectToPatent { get; set; }


        /// <summary>
        /// Has a written description of design and spec so that it
        /// could be produced by one in such skill-domain
        /// </summary>
        /// <remarks>
        /// Having docx of design is the benefit to the public 
        /// </remarks>
        [Aka("enablement")]
        public bool IsImplementable { get; set; }

        [Eg("law of gravity", "E=mc^2")]
        public bool IsLawOfNature { get; set; }

        [Eg("mathematical relationships", "commercial practices")]
        public bool IsAbstractIdea { get; set; }

        /// <summary>
        /// Objective test of one skilled in particular
        /// art of the patent&apos;s domain would not find obvious
        /// </summary>
        public bool IsObviousIdea { get; set; }

        public virtual bool IsValid(params ILegalPerson[] persons)
        {
            Add2TruePropositions(IsSubjectToPatent, nameof(IsSubjectToPatent));
            Add2TruePropositions(IsImplementable, nameof(IsImplementable));
            
            foreach (var tp in _truePropositions)
            {
                if(tp.Item1 == false)
                    AddReasonEntry($"{nameof(Patent)} '{Name}', {tp.Item2} is false");
            }

            if (_truePropositions.Any(p => p.Item1 == false))
                return false;

            if (IsAbstractIdea)
            {
                AddReasonEntry($"{nameof(Patent)} '{Name}', {nameof(IsAbstractIdea)} is true");
                return false;
            }

            if (IsLawOfNature)
            {
                AddReasonEntry($"{nameof(Patent)} '{Name}', {nameof(IsLawOfNature)} is true");
                return false;
            }

            if (IsObviousIdea)
            {
                AddReasonEntry($"{nameof(Patent)} '{Name}', {nameof(IsObviousIdea)} is true");
                return false;
            }

            return true;
        }

        public bool IsEnforceableInCourt => true;
        public bool EquivalentTo(object obj)
        {
            return Equals(obj);
        }
        protected internal void Add2TruePropositions(bool tv, string name)
        {
            _truePropositions.Add(Tuple.Create(tv, name));
        }
    }
}
