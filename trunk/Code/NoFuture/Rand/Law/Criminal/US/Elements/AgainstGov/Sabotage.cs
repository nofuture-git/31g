using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Property;

namespace NoFuture.Rand.Law.Criminal.AgainstGov.US.Elements
{
    /// <summary>
    /// <![CDATA[ 18 U.S.C. § 2151 et seq., ]]> 
    /// destroy, damaging or defective harbor/national defense property, war material, premises, utilities
    /// </summary>
    [Note("listen all'yll itz'a")]
    public class Sabotage : LegalConcept, IActusReus, ILegalProperty
    {
        private readonly ILegalProperty _govProperty;

        public Sabotage(string propertyName)
        {
            _govProperty = new GovernmentProperty(propertyName);
        }

        /// <summary>
        /// Is producing defective material
        /// </summary>
        public Predicate<ILegalPerson> IsDefectiveProducerOf { get; set; } = lp => false;

        /// <summary>
        /// Is destroyer of this property
        /// </summary>
        public Predicate<ILegalPerson> IsDestroyerOf { get; set; } = lp => false;

        /// <summary>
        /// Is source of damage to this property
        /// </summary>
        public Predicate<ILegalPerson> IsDamagerOf { get; set; } = lp => false;

        /// <summary>
        /// harbor or national defense property premises or utilities
        /// </summary>
        public bool IsDefenseProperty { get; set; }

        /// <summary>
        /// war materials property premises or utilities
        /// </summary>
        public bool IsWarProperty { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsDefenseProperty && !IsWarProperty)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsDefenseProperty)} " +
                               $"and {nameof(IsWarProperty)} are false for property '{Name}'");
                return false;
            }

            var defect = IsDefectiveProducerOf(defendant);
            var destroy = IsDestroyerOf(defendant);
            var dmg = IsDamagerOf(defendant);

            if (new[] {defect, destroy, dmg}.All(p => p == false))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsDefectiveProducerOf)}, " +
                               $"{nameof(IsDestroyerOf)} and {nameof(IsDamagerOf)} are false " +
                               $"for property '{Name}'");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntent = new[]
            {
                typeof(SpecificIntent), typeof(Purposely), typeof(GeneralIntent), typeof(Knowingly), typeof(Negligently)
            };
            if (validIntent.All(t => t != criminalIntent.GetType()))
            {
                var nms = string.Join(", ", validIntent.Select(t => t.Name));
                AddReasonEntry($"{nameof(Sabotage)} requires intent of {nms}");
                return false;
            }

            return true;
        }

        public ILegalPerson EntitledTo
        {
            get => _govProperty.EntitledTo;
            set => _govProperty.EntitledTo = value;
        }
        public ILegalPerson InPossessionOf
        {
            get => _govProperty.InPossessionOf;
            set => _govProperty.InPossessionOf = value;
        }

        public decimal? PropretyValue { get; set; }

        #region IVoca HAS-A IS-A
        public IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            return _govProperty.ToData(txtCase);
        }

        public string Name
        {
            get => _govProperty.Name;
            set => _govProperty.Name = value;
        }

        public int NamesCount => _govProperty.NamesCount;
        public void AddName(KindsOfNames k, string name)
        {
            _govProperty.AddName(k, name);
        }

        public string GetName(KindsOfNames k)
        {
            return _govProperty.GetName(k);
        }

        public bool AnyNames(Predicate<KindsOfNames> filter)
        {
            return _govProperty.AnyNames(filter);
        }

        public bool AnyNames(Predicate<string> filter)
        {
            return _govProperty.AnyNames(filter);
        }

        public bool AnyNames(Func<KindsOfNames, string, bool> filter)
        {
            return _govProperty.AnyNames(filter);
        }

        public bool AnyNames()
        {
            return _govProperty.AnyNames();
        }

        public int RemoveName(Predicate<KindsOfNames> filter)
        {
            return _govProperty.RemoveName(filter);
        }

        public int RemoveName(Predicate<string> filter)
        {
            return _govProperty.RemoveName(filter);
        }

        public int RemoveName(Func<KindsOfNames, string, bool> filter)
        {
            return _govProperty.RemoveName(filter);
        }

        public KindsOfNames[] GetAllKindsOfNames()
        {
            return _govProperty.GetAllKindsOfNames();
        }

        public void CopyNamesFrom(IVoca voca)
        {
            _govProperty.CopyNamesFrom(voca);
        }
        #endregion
    }
}
