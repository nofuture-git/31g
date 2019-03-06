using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US
{
    /// <summary>
    /// An ongoing group, club, organization, or association of five or more
    /// that has as one of its primary purposes the commission of specific
    /// criminal offenses or activities that affect interstate or foreign commerce
    /// </summary>
    public class CriminalGang : LegalConcept, IEnumerable<ILegalPerson>, IVoca
    {
        private readonly VocaBase _voca;
        private readonly HashSet<ILegalPerson> _members = new HashSet<ILegalPerson>();

        public const int FEDERAL_MINIMUM_MEMBERS = 5;

        #region ctor
        public CriminalGang()
        {
            _voca = new LegalProperty();
        }

        public CriminalGang(string name)
        {
            _voca = new LegalProperty(name);
        }

        public CriminalGang(string name, string groupName) : this(name)
        {
            _voca.AddName(KindsOfNames.Group, groupName);
        }
        #endregion

        /// <summary>
        /// A legal value typically 5 or 3
        /// </summary>
        protected virtual int MinNumber { get; } = FEDERAL_MINIMUM_MEMBERS;

        /// <summary>
        /// an identifying sign, symbol, tattoo, style of dress, use of hand sign, etc.
        /// </summary>
        public IList<Idiosyncrasy> Idiosyncrasies { get; } = new List<Idiosyncrasy>();

        /// <summary>
        /// Is continuous over time
        /// </summary>
        public bool IsOngoingInFormation { get; set; } = true;

        /// <summary>
        /// the particular criminal enterprise of the gang
        /// </summary>
        public IList<IActusReus> CriminalActivity { get; } = new List<IActusReus>();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (_members.Count < MinNumber)
            {
                AddReasonEntry($"{nameof(CriminalGang)} '{Name}' {nameof(MinNumber)} is {MinNumber} this has {_members.Count}");
                return false;
            }

            if (!IsOngoingInFormation)
            {
                AddReasonEntry($"{nameof(CriminalGang)} '{Name}' {nameof(IsOngoingInFormation)} is false");
                return false;
            }

            if (!CriminalActivity.Any())
            {
                AddReasonEntry($"{nameof(CriminalGang)} '{Name}' {nameof(CriminalActivity)} is empty");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Helper method to call builtin <see cref="Contains"/>
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool IsGangMember(ILegalPerson person)
        {
            return Contains(person);
        }

        #region IVoca HAS-A IS-A
        public IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            return _voca.ToData(txtCase);
        }

        public string Name
        {
            get => _voca.Name;
            set => _voca.Name = value;
        }
        public int NamesCount => _voca.NamesCount;

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

        #region IList HAS-A IS-A
        public IEnumerator<ILegalPerson> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ILegalPerson item)
        {
            _members.Add(item);
        }

        public void Clear()
        {
            _members.Clear();
        }

        public bool Contains(ILegalPerson item)
        {
            return _members.Any(m => VocaBase.Equals(m, item));
        }

        public int Count => _members.Count;
        #endregion
    }
}
