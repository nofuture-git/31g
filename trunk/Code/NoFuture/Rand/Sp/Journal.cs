using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    public class Journal : Balance, IAccount<Identifier>
    {
        private DateTime? _workingDate;

        protected internal DateTime WorkingDate
        {
            get => _workingDate.GetValueOrDefault(DateTime.UtcNow);
            set => _workingDate = value;
        }

        public Journal(Identifier acctId, bool? isOppositeForm = null)
        {
            Id = acctId ?? throw new ArgumentNullException(nameof(acctId));
            IsOppositeForm = isOppositeForm.GetValueOrDefault(false);
        }

        public virtual Pecuniam Value => GetCurrent(DateTime.UtcNow, 0f);

        public virtual Pecuniam GetValueAt(DateTime dt)
        {
            return GetCurrent(dt, 0f);
        }

        public virtual IBalance Balance => this;

        /// <summary>
        /// Requires that all entries for any given date must sum to zero.
        /// </summary>
        public virtual bool IsDoubleBookEntry { get; set; } = true;

        public virtual Identifier Id { get; }

        public virtual KindsOfAccounts? AccountType => null;

        public virtual bool IsOppositeForm { get; }

        public virtual IAccount<Identifier> Debit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? LastTransaction.AtTime;
            ThrowOnJournalizingInbalance(amt, dt);
            WorkingDate = dt;
            if (IsOppositeForm)
                AddNegativeValue(dt, amt, note, trace);
            else
                AddPositiveValue(dt, amt, note, trace);
            return this;
        }

        public virtual IAccount<Identifier> Credit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? LastTransaction.AtTime;
            ThrowOnJournalizingInbalance(amt, dt);
            WorkingDate = dt;
            if (IsOppositeForm)
                AddPositiveValue(dt, amt, note, trace);
            else
                AddNegativeValue(dt, amt, note, trace);
            return this;
        }

        /// <summary>
        /// Throws and exception when double-entry booking is on and the sum of any date is not zero.
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="atTime"></param>
        /// <remarks>
        /// Based on general practice of Accountants - the balancing part of the double-book entry system
        /// happens in journalizing.  Journals are then Posted to the ledger.  &quot;Posting to the Ledger&quot; means 
        /// the journal&apos;s list of transactions are decomposed into their respective accounts.  After that 
        /// happens, besides audit, there is not clear relation of one transaction being the off-set of another because
        /// these transactions are dispersed across multiple accounts.
        /// </remarks>
        protected internal virtual void ThrowOnJournalizingInbalance(Pecuniam amt, DateTime atTime)
        {
            if (!IsDoubleBookEntry)
                return;

            var isNewDay = IsNewDay(atTime);
            if (!isNewDay)
                return;
            var sumPerDay = GetSumPerDay();
            if (sumPerDay == null || !sumPerDay.Any())
                return;

            var inbalanceDays = sumPerDay.Keys.Where(sd => sumPerDay[sd] != Pecuniam.Zero).ToList();

            if (inbalanceDays.Any())
            {
                throw new InvalidOperationException("The following dates do not have a zero-sum " +
                                                    $"balance {string.Join(", ", inbalanceDays.Select(d => d.ToString("d")))}.  " +
                                                    $"Either correct these dates or turn off {nameof(IsDoubleBookEntry)}");
            }
        }

        protected internal bool IsNewDay(DateTime atTime)
        {
            return atTime.Date != WorkingDate.Date;
        }
    }
}