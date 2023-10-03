using System;
using NoFuture.Law;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;
using NoFuture.Law.US.Courts;

namespace NoFuture.Law.Procedure.Civil.US.Jurisdiction
{
    public abstract class JurisdictionBase : CivilProcedureBase, IJurisdiction
    {
        #region fields
        protected bool flagGetDomicileLocation;
        private Func<ILegalPerson, IVoca> _getDomicileLocation = lp => null;

        protected bool flagGetCurrentLocation;
        private Func<ILegalPerson, IVoca> _getCurrentLocation = lp => null;

        protected bool flagGetInjuryLocation;
        private Func<ILegalPerson, IVoca> _getInjuryLocation = lp => null;

        #endregion

        protected JurisdictionBase(ICourt name)
        {
            if (name != null)
                Court = name;
        }

        #region properties

        /// <summary>
        /// Is the cause-of-action location of &quot;a substantial part of the
        /// events or omissions&quot; or the location of &quot;a
        /// substantial part of property&quot;
        /// </summary>
        /// <remarks>
        /// 28 U.S.C. §1391(b)(2)
        /// </remarks>
        [Aka("where-the-claim-arose")]
        public virtual Func<ILegalPerson, IVoca> GetInjuryLocation
        {
            get => _getInjuryLocation;
            set
            {
                //want to record if this was explicitly set 
                flagGetInjuryLocation = true;
                _getInjuryLocation = value;
            }
        }

        /// <summary>
        /// Is where the defendant physically lives or resides - all plaintiffs can
        /// travel there and sue the defendant.
        /// </summary>
        public virtual Func<ILegalPerson, IVoca> GetDomicileLocation
        {
            get => _getDomicileLocation;
            set
            {
                //want to record if this was explicitly set 
                flagGetDomicileLocation = true;
                _getDomicileLocation = value;
            }
        }

        /// <summary>
        /// If defendant is actually present in the forum state then they may be sued there.
        /// </summary>
        public virtual Func<ILegalPerson, IVoca> GetCurrentLocation
        {
            get => _getCurrentLocation;
            set
            {
                //want to record if this was explicitly set 
                flagGetCurrentLocation = true;
                _getCurrentLocation = value;
            }
        }


        #endregion

        #region methods

        public void CopyTo(JurisdictionBase juris)
        {
            if (juris == null)
                return;

            if (juris.Court == null && Court != null)
                juris.Court = Court;

            if (flagGetDomicileLocation && !juris.flagGetDomicileLocation)
                juris.GetDomicileLocation = GetDomicileLocation;

            if (flagGetCurrentLocation && !juris.flagGetCurrentLocation)
                juris.GetCurrentLocation = GetCurrentLocation;

            if (flagGetInjuryLocation && !juris.flagGetInjuryLocation)
                juris.GetInjuryLocation = GetInjuryLocation;
        }

        protected bool IsFederalCourt()
        {
            if (Court is FederalCourt)
                return true;

            AddReasonEntry($"{nameof(Court)}, '{Court?.Name}' is type " +
                           $"{Court?.GetType().Name} not type {nameof(FederalCourt)}");
            return false;
        }

        protected internal virtual bool IsCourtDomicileLocationOfDefendant(ILegalPerson defendant, string title = null)
        {
            title = title ?? defendant.GetLegalPersonTypeName();

            //is jurisdiction domicile location of defendant
            var domicile = GetDomicileLocation(defendant);

            if (domicile == null)
            {
                AddReasonEntry($"{title} {defendant.Name}, " +
                               $"{nameof(GetDomicileLocation)} returned nothing");
                return false;
            }

            if (NamesEqual(Court, domicile))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetDomicileLocation)} returned '{domicile.Name}'");
                AddReasonEntry($"'{domicile.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            return false;
        }

        protected internal virtual bool IsCourtInjuryLocationOfPlaintiff(ILegalPerson plaintiff, string title = null)
        {
            title = title ?? plaintiff.GetLegalPersonTypeName();

            //is jurisdiction domicile location of defendant
            var injuryLocation = GetInjuryLocation(plaintiff);

            if (injuryLocation == null)
            {
                AddReasonEntry($"{title} {plaintiff.Name}, " +
                               $"{nameof(GetInjuryLocation)} returned nothing");
                return false;
            }

            if (NamesEqual(Court, injuryLocation))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(GetInjuryLocation)} returned '{injuryLocation.Name}'");
                AddReasonEntry($"'{injuryLocation.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            return false;
        }

        protected internal virtual bool IsCourtCurrentLocationOfDefendant(ILegalPerson defendant, string title = null)
        {
            title = title ?? defendant.GetLegalPersonTypeName();

            //is defendant current present in jurisdiction
            var location = GetCurrentLocation(defendant);

            if (location == null)
            {
                AddReasonEntry($"{title} {defendant.Name}, " +
                               $"{nameof(GetCurrentLocation)} returned nothing");
                return false;
            }

            if (NamesEqual(Court, location))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetCurrentLocation)} returned '{location.Name}'");
                AddReasonEntry($"'{location.Name}' & {nameof(Court)}  '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            return false;
        }

        #endregion
    }
}
