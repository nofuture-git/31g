using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US
{
    /// <summary>
    /// A persons domicile location is a named place and is also a testable legal concept
    /// </summary>
    /// <remarks>
    /// U.S.C. section 1332(c) - corporations are citizens of both states where
    /// they incorp&apos;d and where their headquarters is located.
    /// Hertz Corp. v. Friend, 130 S. Ct. 1181 (2010)
    /// </remarks>
    public class DomicileLocation : UnoHomine, IVoca
    {
        #region ctors

        public DomicileLocation(string name) : this(lps => lps.FirstOrDefault())
        {
            _voca.Name = name;
        }

        public DomicileLocation(string name, Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            if (GetSubjectPerson == null)
                GetSubjectPerson = lps => lps.FirstOrDefault();
            _voca.Name = name;
        }

        public DomicileLocation(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            if (GetSubjectPerson == null)
                GetSubjectPerson = lps => lps.FirstOrDefault();
        }

        #endregion

        /// <summary>
        /// This is subjective intent - wherever the person calls &quot;home&quot;
        /// </summary>
        public Predicate<ILegalPerson> IsIntendsIndefiniteStay { get; set; } = lp => false;

        /// <summary>
        /// <see cref="IsIntendsIndefiniteStay"/> is required but not sufficient - the person
        /// must actually be present
        /// </summary>
        public Predicate<ILegalPerson> IsPhysicallyPresent { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subject = GetSubjectPerson(persons);
            if (subject == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }

            var title = subject.GetLegalPersonTypeName();

            if (!IsIntendsIndefiniteStay(subject))
            {
                AddReasonEntry($"{title} {subject.Name}, {nameof(IsIntendsIndefiniteStay)} is false for {Name}");
                return false;
            }
            if (!IsPhysicallyPresent(subject))
            {
                AddReasonEntry($"{title} {subject.Name}, {nameof(IsPhysicallyPresent)} is false for {Name}");
                return false;
            }

            return true;
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
