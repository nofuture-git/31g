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

        public IAccount<Identifier> Add(string accountId, bool isOppositeForm, int? refId = null, DateTime? atTime = null)
        {
            if(string.IsNullOrWhiteSpace(accountId))
                throw new ArgumentNullException(nameof(accountId));

            var dt = atTime.GetValueOrDefault(DateTime.UtcNow);
            var acct = new Account(dt, isOppositeForm) {Id = new AccountId(accountId, refId), Name = accountId};
            _dataStore.Add(acct);
            return acct;
        }

        public IAccount<Identifier> Get(string accountId)
        {
            return _dataStore.FirstOrDefault(acct => acct.Id.Equals(accountId));
        }

        public IAccount<Identifier> Get(int refId)
        {
            return _dataStore.FirstOrDefault(acct => acct.Id.Abbrev == refId.ToString());
        }

        public IAccount<Identifier> Remove(string accountId)
        {
            var acct = Get(accountId);
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
