using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Law.Criminal.US.Elements;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    /// <summary>
    /// a place which a substantial group has access like highways,
    /// transport facilities, public squares, parks, beaches, schools,
    /// prisons, apartment houses, places of business or amusement,
    /// or any neighborhood
    /// </summary>
    public class PublicPlace : AttendantCircumstances, ILegalProperty
    {
        private readonly VocaBase _voca;

        #region ctor

        public PublicPlace()
        {
            _voca = new LegalProperty();
        }

        public PublicPlace(string name)
        {
            _voca = new LegalProperty(name);
        }

        public PublicPlace(string name, string groupName) : this(name)
        {
            _voca.AddName(KindsOfNames.Group, groupName);
        }

        #endregion

        /// <summary>
        /// By definition, is true, but can be set to false to invalidate itself
        /// </summary>
        public bool IsAccessibleToPublic { get; set; } = true;

        public Predicate<ILegalPerson> IsWithin { get; set; } = lp => false;

        public ILegalPerson EntitledTo { get; set; }
        public ILegalPerson InPossessionOf { get; set; }
        public decimal? PropretyValue { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsAccessibleToPublic)
            {
                AddReasonEntry($"property {this.Name}, is {nameof(IsAccessibleToPublic)} is false");
                return false;
            }
            if (VocaBase.Equals(defendant, EntitledTo))
            {
                AddReasonEntry($"defendant, {defendant.Name}, is {nameof(EntitledTo)} is true");
                return false;
            }

            if (!IsWithin(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, is {nameof(IsWithin)} is true");
                return false;
            }

            return true;
        }

        #region IVoca HAS-A
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

    }
}
