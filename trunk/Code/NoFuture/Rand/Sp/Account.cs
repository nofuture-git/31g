﻿using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="NamedTradeline" />
    /// <inheritdoc cref="IAccount{T}"/>
    /// <summary>
    /// Basic Accounting type entity
    /// </summary>
    public class Account : NamedTradeline, IAccount<Identifier>
    {
        public Account(Identifier acctId, DateTime dateOpened, KindsOfAccounts accountType, bool isOppositeForm) : base(dateOpened)
        {
            Id = acctId ?? throw new ArgumentNullException(nameof(acctId));
            DueFrequency = null;
            FormOfCredit = null;
            IsOppositeForm = isOppositeForm;
            AccountType = accountType;
        }

        public virtual KindsOfAccounts? AccountType { get; }

        public virtual Identifier Id { get; }

        public virtual bool IsOppositeForm { get; }

        public virtual IAccount<Identifier> Debit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? Balance.LastTransaction.AtTime;
            if (IsOppositeForm)
                AddNegativeValue(dt, amt, note, trace);
            else
                AddPositiveValue(dt, amt, note, trace);
            return this;
        }

        public virtual IAccount<Identifier> Credit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? Balance.LastTransaction.AtTime;
            if (IsOppositeForm)
                AddPositiveValue(dt, amt, note, trace);
            else
                AddNegativeValue(dt, amt, note, trace);
            return this;
        }

        public virtual bool AnyTransaction(Predicate<ITransactionId> filter)
        {
            return Balance.AnyTransaction(filter);
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