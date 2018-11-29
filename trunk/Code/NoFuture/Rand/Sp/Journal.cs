using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    public class Journal : Balance, IAccount<Identifier>
    {
        public Journal(Identifier acctId, bool? isOppositeForm = null)
        {
            Id = acctId ?? throw new ArgumentNullException(nameof(acctId));
            IsOppositeForm = isOppositeForm.GetValueOrDefault(false);
        }

        public Pecuniam Value => GetCurrent(DateTime.UtcNow, 0f);
        public Pecuniam GetValueAt(DateTime dt)
        {
            return GetCurrent(dt, 0f);
        }

        public Identifier Id { get; }
        public KindsOfAccounts? AccountType => null;
        public bool IsOppositeForm { get; }
        public IAccount<Identifier> Debit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? LastTransaction.AtTime;
            if (IsOppositeForm)
                AddNegativeValue(dt, amt, note, trace);
            else
                AddPositiveValue(dt, amt, note, trace);
            return this;
        }

        public IAccount<Identifier> Credit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? LastTransaction.AtTime;
            if (IsOppositeForm)
                AddPositiveValue(dt, amt, note, trace);
            else
                AddNegativeValue(dt, amt, note, trace);
            return this;
        }
    }
}