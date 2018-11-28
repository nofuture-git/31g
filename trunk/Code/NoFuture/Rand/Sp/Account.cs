﻿using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="NamedReceivable" />
    /// <inheritdoc cref="IAccount{T}"/>
    /// <summary>
    /// Basic Accounting type entity
    /// </summary>
    public class Account : NamedReceivable, IAccount<Identifier>
    {
        public Account(Identifier acctId, DateTime dateOpenned, bool isOppositeForm) : base(dateOpenned)
        {
            Id = acctId;
            DueFrequency = TimeSpan.Zero;
            FormOfCredit = Enums.FormOfCredit.None;
            IsOppositeForm = isOppositeForm;
        }

        public Identifier Id { get; }

        public bool IsOppositeForm { get; }

        public Guid Debit(DateTime dt, Pecuniam amt, IVoca note = null, ITransactionId trace = null)
        {
            return IsOppositeForm ? AddNegativeValue(dt, amt, note, trace) : AddPositiveValue(dt, amt, note, trace);
        }

        public Guid Credit(DateTime dt, Pecuniam amt, IVoca note = null, ITransactionId trace = null)
        {
            return IsOppositeForm ? AddPositiveValue(dt, amt, note, trace) : AddNegativeValue(dt, amt, note, trace);
        }

        public Guid Debit(ITransactionable source, Pecuniam amount, DateTime? atTime = null, IVoca description = null)
        {
            return IsOppositeForm
                ? AddNegativeValue(source, amount, atTime, description)
                : AddPositiveValue(source, amount, atTime, description);
        }

        public Guid Credit(ITransactionable source, Pecuniam amount, DateTime? atTime = null, IVoca description = null)
        {
            return IsOppositeForm
                ? AddPositiveValue(source, amount, atTime, description)
                : AddNegativeValue(source, amount, atTime, description);
        }

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, 0.0F);
        }

        public override bool Equals(object obj)
        {
            var acct = obj as Account;
            if (acct == null)
                return base.Equals(obj);
            return Id.Equals(acct.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}