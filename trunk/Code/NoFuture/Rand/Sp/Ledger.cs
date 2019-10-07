using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// The Accounting concept of a kind-of capital object that contains the 
    /// exhaustive list of all accounts
    /// </summary>
    [Serializable]
    public class Ledger : IEnumerable<IAccount<Identifier>>
    {
        public const string NO_NAME_ACCOUNT = "no-name";

        private readonly SortedSet<IAccount<Identifier>> _dataStore =
            new SortedSet<IAccount<Identifier>>(new AccountComparer<Identifier>());

        public int Count => _dataStore.Count;
        public bool IsReadOnly => false;

        public IEnumerator<IAccount<Identifier>> GetEnumerator()
        {
            return _dataStore.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual bool IsBalanced()
        {
            if (!_dataStore.Any())
                return true;
            var assetAccounts = new List<IAccount<Identifier>>();
            var liabilityAccounts = new List<IAccount<Identifier>>();
            var equityAccounts = new List<IAccount<Identifier>>();
            foreach (var acct in _dataStore)
            {
                switch (acct.AccountType)
                {
                    case KindsOfAccounts.Asset:
                        assetAccounts.Add(acct);
                        continue;
                    case KindsOfAccounts.Liability:
                        liabilityAccounts.Add(acct);
                        continue;
                    case KindsOfAccounts.Equity:
                        equityAccounts.Add(acct);
                        continue;
                }
            }

            return assetAccounts.Sum().GetRounded().GetAbs() ==
                   (liabilityAccounts.Sum() + equityAccounts.Sum()).GetRounded().GetAbs();
        }

        public IAccount<Identifier> Add(string accountName, KindsOfAccounts accountType, bool isOppositeForm, int? refId = null, DateTime? atTime = null)
        {
            if(string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentNullException(nameof(accountName));

            var existing = _dataStore.FirstOrDefault(a => a.Id.Equals(accountName));
            if (existing != null)
            {
                ((AccountId)existing.Id).SetRefId(refId);
                return existing;
            }

            var dt = atTime.GetValueOrDefault(DateTime.UtcNow);
            var acct = new Account(new AccountId(accountName, refId), dt, accountType, isOppositeForm) {Name = accountName};
            _dataStore.Add(acct);
            return acct;
        }

        public IAccount<Identifier> Add(IAccount<Identifier> account)
        {
            if(account == null)
                throw new ArgumentNullException(nameof(account));

            var existing = _dataStore.FirstOrDefault(a => a.Equals(account));
            if (existing != null)
            {
                ((AccountId)existing.Id).SetRefId(account.Id.Abbrev);
                return existing;
            }

            _dataStore.Add(account);
            return account;
        }

        public IAccount<Identifier> Get(string accountName, int? refId = null)
        {
            var r = _dataStore.FirstOrDefault(acct => acct.Id.Equals(accountName));
            if (refId != null && r?.Id != null)
            {
                var acctId = r.Id as AccountId;
                if (acctId == null)
                    return r;
                acctId.SetRefId(refId);
            }
            return r;
        }

        public IAccount<Identifier> Get(int refId)
        {
            return _dataStore.FirstOrDefault(acct => acct.Id.Abbrev == refId.ToString());
        }

        protected internal IAccount<Identifier> Get(IVoca name)
        {
            foreach (var acct in _dataStore)
            {
                var acctName = acct as IVoca;
                if (acctName == null)
                    continue;
                if (VocaBase.Equals(acctName, name))
                    return acct;
            }
            return null;
        }

        public IAccount<Identifier> Remove(string accountName)
        {
            var acct = Get(accountName);
            if (acct == null)
                return null;
            _dataStore.Remove(acct);
            return acct;
        }

        public void Clear()
        {
            _dataStore.Clear();
        }

        public void PostBalance(IBalance balance, DateTime? atTime = null)
        {
            if (balance == null)
                return;
            var credits = balance.GetCredits() ?? new List<ITransaction>();
            var debits = balance.GetDebits() ?? new List<ITransaction>();

            if (!credits.Any() && !debits.Any())
                return;

            foreach (var credit in credits)
            {
                AddTransaction(credit, balance, atTime, false);
            }

            foreach (var debit in debits)
            {
                AddTransaction(debit, balance, atTime);
            }
        }

        private void AddTransaction(ITransaction transaction, IBalance balance, DateTime? atTime = null, bool lrFlag = true)
        {
            var dt = atTime.GetValueOrDefault(DateTime.UtcNow);
            var note = GetAsNote(transaction);
            var toAccount = Get(note.Name) ?? Add(note.Name, note.AssociatedAccountType ?? KindsOfAccounts.Asset,
                                false, null, transaction.AtTime);
            var traceId = transaction.GetThisAsTraceId(dt, balance as IVoca);
            var isDup = IsDuplicate(toAccount, traceId);
            if (isDup)
                return;
            if(lrFlag)
                toAccount.Debit(transaction.Cash, null, transaction.AtTime, traceId);
            else
                toAccount.Credit(transaction.Cash, null, transaction.AtTime, traceId);
        }

        protected internal bool IsDuplicate(IAccount<Identifier> toAccount, TraceTransactionId incomingTransaction)
        {
            return toAccount.AnyTransaction(tr => tr.Trace.UniqueId == incomingTransaction.UniqueId);
        }

        protected internal TransactionNote GetAsNote(ITransaction transaction)
        {
            if(transaction?.Description == null)
                return new TransactionNote(NO_NAME_ACCOUNT);
            var note = transaction.Description as TransactionNote;
            if (note != null)
                return note;

            var nNote = new TransactionNote();
            nNote.CopyNamesFrom(transaction.Description);
            return nNote;
        }
    }
}
