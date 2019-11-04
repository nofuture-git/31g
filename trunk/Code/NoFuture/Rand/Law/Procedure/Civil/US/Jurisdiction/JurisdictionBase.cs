using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    public abstract class JurisdictionBase : UnoHomine, IVoca
    {
        protected bool flagGetDomicileLocation;
        private Func<ILegalPerson, IVoca> _getDomicileLocation = lp => null;

        protected bool flagGetCurrentLocation;
        private Func<ILegalPerson, IVoca> _getCurrentLocation = lp => null;

        protected bool flagGetInjuryLocation;
        private Func<ILegalPerson, IVoca> _getInjuryLocation = lp => null;

        protected JurisdictionBase() : base(ExtensionMethods.DefendantFx)
        {

        }

        protected JurisdictionBase(string name) : this()
        {
            _voca.Name = name;
        }

        /// <summary>
        /// Is the location in which some injury occured 
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

        /// <summary>
        /// Allows for class level overrides -default is the static VocaBase.Equals
        /// </summary>
        /// <param name="voca"></param>
        /// <returns></returns>
        public virtual bool NameEquals(IVoca voca)
        {
            return VocaBase.Equals(this, voca);
        }

        /// <summary>
        /// Helper method reduce redundant code
        /// </summary>
        /// <param name="subject2Person">
        /// Item1 is just the name you want to appear in the reasons (e.g. nameof(SuchAndSuch)).
        /// Item2 is a function to resolve the subject person (e.g. defendant, tortfeaser) to some other person (plaintiff, victim, etc.)
        /// </param>
        /// <param name="person2Name">
        /// Item1 is just the name you want to appear in the reasons (e.g. nameof(SuchAndSuch)).
        /// Item2 is a function to resolve the other person (plaintiff, victim, etc.) to some name\location to compare to class&apos;s name
        /// </param>
        /// <param name="persons">
        /// The legal persons passed into the calling function (e.g. IsValid)
        /// </param>
        protected virtual bool TestPerson2Name(
            Tuple<string, Func<ILegalPerson, ILegalPerson>> subject2Person, 
            Tuple<string, Func<ILegalPerson, IVoca>> person2Name,
            params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null || subject2Person?.Item2 == null || person2Name?.Item2 == null) 
                return false;

            var title = defendant.GetLegalPersonTypeName();

            var otherPerson = subject2Person.Item2(defendant);

            var someFunctionName = subject2Person.Item1;

            if (otherPerson == null)
                return false;

            var otherTitle = otherPerson.GetLegalPersonTypeName();

            var someOtherFunctionName = person2Name.Item1;

            var voca = person2Name.Item2(otherPerson);
            if (NameEquals(voca))
            {
                AddReasonEntry($"{title} {defendant.Name}, {someFunctionName} returned '{otherPerson.Name}'");
                AddReasonEntry($"{otherTitle} {otherPerson.Name}, {someOtherFunctionName} returned '{voca.Name}'");
                AddReasonEntry($"'{voca.Name}' & '{Name}', {nameof(NameEquals)} is true");
                return true;
            }

            return false;
        }

        public void CopyTo(JurisdictionBase juris)
        {
            if (juris == null)
                return;

            //transpose whatever is here to this sister type based on what's missing
            if (juris.NamesCount <= 0)
                juris.CopyNamesFrom(this);

            if (flagGetDomicileLocation && !juris.flagGetDomicileLocation)
                juris.GetDomicileLocation = GetDomicileLocation;

            if (flagGetCurrentLocation && !juris.flagGetCurrentLocation)
                juris.GetCurrentLocation = GetCurrentLocation;

            if (flagGetInjuryLocation && !juris.flagGetInjuryLocation)
                juris.GetInjuryLocation = GetInjuryLocation;
        }

        #region IVoca IS-A HAS-A
        private readonly IVoca _voca = new VocaBase();
        public IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            return _voca.ToData(txtCase);
        }

        public string Name
        {
            get => _voca.Name;
            set => _voca.Name = value;
        }
        public int NamesCount
        {
            get => _voca.NamesCount;
        }
        public void AddName(KindsOfNames k, string name)
        {
            _voca.AddName(k, name);
        }

        public string GetName(KindsOfNames k)
        {
            return _voca.GetName(k);
        }

        public bool AnyNames(Predicate<KindsOfNames> filter)
        {
            return _voca.AnyNames(filter);
        }

        public bool AnyNames(Predicate<string> filter)
        {
            return _voca.AnyNames(filter);
        }

        public bool AnyNames(Func<KindsOfNames, string, bool> filter)
        {
            return _voca.AnyNames(filter);
        }

        public bool AnyNames()
        {
            return _voca.AnyNames();
        }

        public int RemoveName(Predicate<KindsOfNames> filter)
        {
            return _voca.RemoveName(filter);
        }

        public int RemoveName(Predicate<string> filter)
        {
            return _voca.RemoveName(filter);
        }

        public int RemoveName(Func<KindsOfNames, string, bool> filter)
        {
            return _voca.RemoveName(filter);
        }

        public KindsOfNames[] GetAllKindsOfNames()
        {
            return _voca.GetAllKindsOfNames();
        }

        public void CopyNamesFrom(IVoca voca)
        {
            _voca.CopyNamesFrom(voca);
        }

        #endregion
    }
}
