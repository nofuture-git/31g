using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    public abstract class Ledger : IEnumerable<IAccount<Identifier>>
    {
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

        public IAccount<Identifier> Add(string accountName, bool isOppositeForm, int? refId = null, DateTime? atTime = null)
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
            var acct = new Account(new AccountId(accountName, refId), dt, isOppositeForm) {Name = accountName};
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

        public IAccount<Identifier> Remove(string accountName)
        {
            var acct = Get(accountName);
            if (acct == null)
                return null;
            _dataStore.Remove(acct);
            return acct;
        }

        public void PostBalance(IBalance balance)
        {

        }

    }
}
