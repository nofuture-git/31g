﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Trespass;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements
{
    /// <summary>
    /// starting a fire or causing an explosion which burns real or personal property
    /// </summary>
    [EtymologyNote("Latin", "'ardere'", "to burn")]
    public class Arson : AgitPropertyBase, IActusReus
    {
        public Predicate<ILegalPerson> IsFireStarter { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsCauseOfExplosion { get; set; } = lp => false;

        public Predicate<ILegalProperty> IsBurned { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var isFireStarter = IsFireStarter(defendant);
            var isExploder = IsCauseOfExplosion(defendant);

            if (!isFireStarter && !isExploder)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsFireStarter)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsCauseOfExplosion)} is false");
                return false;
            }

            if (!IsBurned(SubjectProperty))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsBurned)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {

            var validIntend = criminalIntent is Knowingly || criminalIntent is GeneralIntent;

            if (!validIntend)
            {
                AddReasonEntry($"{nameof(CriminalTrespass)} requires intent " +
                               $"{ nameof(Knowingly)}, { nameof(GeneralIntent)}");
                return false;
            }

            if (PropertyOwnerIsDefendant(persons) && IsBurned(SubjectProperty))
                return false;

            return true;
        }
    }
}