using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// 35 U.S.C. Section 101, promoting &quot;the Progress of Science and the useful Arts&quot;
    /// </summary>
    public class Patent : IntellectualProperty, ILegalConcept
    {
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
