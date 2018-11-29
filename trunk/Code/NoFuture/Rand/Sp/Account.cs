using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="NamedReceivable" />
    /// <inheritdoc cref="IAccount{T}"/>
    /// <summary>
    /// Basic Accounting type entity
    /// </summary>
    public class Account : NamedReceivable, IAccount<Identifier>
    {
        public Account(Identifier acctId, DateTime dateOpenned, KindsOfAccounts accountType, bool isOppositeForm) : base(dateOpenned)
        {
            Id = acctId;
            DueFrequency = TimeSpan.Zero;
            FormOfCredit = Enums.FormOfCredit.None;
            IsOppositeForm = isOppositeForm;
            AccountType = accountType;

        }

        public KindsOfAccounts AccountType { get; }

        public Identifier Id { get; }

        public bool IsOppositeForm { get; }

        public Guid Debit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? Balance.LastTransaction.AtTime;
            return IsOppositeForm ? AddNegativeValue(dt, amt, note, trace) : AddPositiveValue(dt, amt, note, trace);
        }

        public Guid Credit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? Balance.LastTransaction.AtTime;
            return IsOppositeForm ? AddPositiveValue(dt, amt, note, trace) : AddNegativeValue(dt, amt, note, trace);
        }

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, 0.0F);
        }

        public override bool Equals(object obj)
        {
            var acct = obj as Account;
            return acct == null ? base.Equals(obj) : Id.Equals(acct.Id);
        }

        public virtual bool Equals(IVoca name)
        {
            return name != null && VocaBase.Equals(this, name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}