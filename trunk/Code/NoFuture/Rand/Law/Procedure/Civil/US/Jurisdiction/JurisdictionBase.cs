using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.US;

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

        /// <summary>
        /// Helper method reduce redundant code
        /// </summary>
        /// <param name="defendant2Person">
        /// Item1 is just the name you want to appear in the reasons (e.g. nameof(SuchAndSuch)).
        /// Item2 is a function to resolve the subject person (e.g. defendant, tortfeaser) to some other person (plaintiff, victim, etc.)
        /// </param>
        /// <param name="person2Location">
        /// Item1 is just the name you want to appear in the reasons (e.g. nameof(SuchAndSuch)).
        /// Item2 is a function to resolve the other person (plaintiff, victim, etc.) to some
        ///       name\location to compare to to <see cref="CivilProcedureBase.Court"/> name
        /// </param>
        /// <param name="persons">
        /// The legal persons passed into the calling function (e.g. IsValid)
        /// </param>
        protected virtual bool TestDefendant2Person2LocationIsCourt(
            Tuple<string, Func<ILegalPerson, ILegalPerson>> defendant2Person, 
            Tuple<string, Func<ILegalPerson, IVoca>> person2Location,
            params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null || defendant2Person?.Item2 == null || person2Location?.Item2 == null) 
                return false;

            var title = defendant.GetLegalPersonTypeName();

            var otherPerson = defendant2Person.Item2(defendant);

            var someFunctionName = defendant2Person.Item1;

            if (otherPerson == null)
                return false;

            var otherTitle = otherPerson.GetLegalPersonTypeName();

            var someOtherFunctionName = person2Location.Item1;

            var location = person2Location.Item2(otherPerson);
            if (NamesEqual(Court, location))
            {
                AddReasonEntry($"{title} {defendant.Name}, {someFunctionName} returned person '{otherPerson.Name}'");
                AddReasonEntry($"{otherTitle} {otherPerson.Name}, {someOtherFunctionName} returned '{location.Name}'");
                AddReasonEntry($"'{location.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            return false;
        }

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

        #endregion
    }
}
