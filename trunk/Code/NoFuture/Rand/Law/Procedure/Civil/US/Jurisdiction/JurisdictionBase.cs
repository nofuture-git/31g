using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    public abstract class JurisdictionBase : CivilProcedureBase
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
        /// Is the location from which the injury (cause of action) is located
        /// </summary>
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

        #endregion
    }
}
