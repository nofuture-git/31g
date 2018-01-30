﻿using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class SavingsAccount : DepositAccount
    {
        #region ctor
        public SavingsAccount(RIdentifier acctId, DateTime dateOpenned) : base(dateOpenned)
        {
            Id = acctId;
        }
        #endregion

        #region properties
        public float InterestRate { get; set; }
        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, InterestRate);
        #endregion

        #region methods

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, InterestRate);
        }


        #endregion
    }
}