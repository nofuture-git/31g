using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// 35 U.S.C. Section 101, promoting &quot;the Progress of Science and the useful Arts&quot;
    /// </summary>
    public class Patent : IntellectualProperty, ILegalConcept
    {
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

        [Aka("utility", "entertainment")]
        public bool IsUseful { get; set; }

        /// <summary>
        /// Has a written description of design and spec so that it
        /// could be produced by one in such skill-domain
        /// </summary>
        /// <remarks>
        /// Having docx of design is the benefit to the public 
        /// </remarks>
        [Aka("enablement")]
        public bool IsImplementable { get; set; }

        [Aka("new art")]
        public bool IsNewProcess { get; set; }
        public bool IsNewMachine { get; set; }
        public bool IsNewManufacture { get; set; }
        public bool IsNewCompositionOfMatter { get; set; }
        public bool IsExistingImprovement { get; set; }

        [Eg("law of gravity", "E=mc^2")]
        public bool IsLawOfNature { get; set; }
        [Eg("mathematical relationships", "commercial practices")]
        public bool IsAbstractIdea { get; set; }

        /// <summary>
        /// Objective test of one skilled in particular
        /// art of the patent&apos;s domain would not find obvious
        /// </summary>
        public bool IsObviousIdea { get; set; }

        public bool IsValid(params ILegalPerson[] persons)
        {
            var trueProperties = new List<Tuple<bool, string>>
            {
                Tuple.Create(IsNewProcess, nameof(IsNewProcess)),
                Tuple.Create(IsNewMachine, nameof(IsNewMachine)),
                Tuple.Create(IsNewManufacture, nameof(IsNewManufacture)),
                Tuple.Create(IsNewCompositionOfMatter, nameof(IsNewCompositionOfMatter)),
                Tuple.Create(IsExistingImprovement, nameof(IsExistingImprovement)),
            };

            if (trueProperties.All(p => p.Item1 == false))
            {
                var trueNames = string.Join(" ", trueProperties.Select(t => t.Item2));
                AddReasonEntry($"{nameof(Patent)} named '{Name}', {trueNames} are all false");
                return false;
            }

            if (IsAbstractIdea)
            {
                AddReasonEntry($"{nameof(Patent)} named '{Name}', {nameof(IsAbstractIdea)} is true");
                return false;
            }

            if (IsLawOfNature)
            {
                AddReasonEntry($"{nameof(Patent)} named '{Name}', {nameof(IsLawOfNature)} is true");
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
