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
    /// exhuastive list of all accounts
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

        public IAccount<Identifier> Get(string accountName)
        {
            return _dataStore.FirstOrDefault(acct => acct.Id.Equals(accountName));
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

        public void PostBalance(IBalance balance, DateTime? atTime = null)
        {
            if (balance == null)
                return;
            var credits = balance.GetCredits() ?? new List<ITransaction>();
            var debits = balance.GetDebits() ?? new List<ITransaction>();

            if (!credits.Any() && !debits.Any())
                return;
            var dt = atTime.GetValueOrDefault(DateTime.UtcNow);
            foreach (var credit in credits)
            {
                var acctName = credit.Description?.Name ?? NO_NAME_ACCOUNT;
                //TODO - the kind of account does and should not travel with Journal - so where's it come from?
                var creditAcct = Get(acctName) ?? Add(acctName, KindsOfAccounts.Asset, false, null, credit.AtTime);
                creditAcct.Credit(credit.AtTime, credit.Cash, null, credit.GetThisAsTraceId(dt, balance as IVoca));
            }

            foreach (var debit in debits)
            {
                var acctName = debit.Description?.Name ?? NO_NAME_ACCOUNT;
                var debitAcct = Get(acctName) ?? Add(acctName, KindsOfAccounts.Asset, false, null, debit.AtTime);
                debitAcct.Debit(debit.AtTime, debit.Cash, null, debit.GetThisAsTraceId(dt, balance as IVoca));
            }
        }

    }
}
