﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Base type for a depository account held at a commercial bank.
    /// </summary>
    [Serializable]
    public abstract class DepositAccount : IAccount<Identifier>, ITransactionable
    {
        #region ctor
        protected DepositAccount(DateTime dateOpenned)
        {
            Balance = new Balance();
            Inception = dateOpenned;
        }
        #endregion 

        #region properties
        public bool IsJointAcct { get; set; }
        public virtual Identifier Id { get; set; }
        public IBalance Balance { get; }
        public FinancialFirm Bank { get; set; }
        public abstract Pecuniam CurrentValue { get; }
        public virtual DateTime Inception { get; }
        public virtual DateTime? Terminus { get; set; }
        #endregion

        #region methods
        public override string ToString()
        {
            return string.Join(" ", GetType().Name, Bank, Id.ValueLastFour());
        }

        public virtual Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, 0.0F);
        }

        public SpStatus GetStatus(DateTime? dt)
        {
            if(Terminus != null && Terminus < dt)
                return SpStatus.Closed;
            var ddt = dt ?? DateTime.Now;

            var balAtDt = Balance.GetCurrent(ddt, 0F);
            return balAtDt < Pecuniam.Zero ? SpStatus.Short : SpStatus.Current;
        }

        public virtual void Push(DateTime dt, Pecuniam val, Pecuniam fee = null, string note = null)
        {
            if (val == Pecuniam.Zero)
                return;
            Balance.AddTransaction(dt, val.Abs, fee, note);
        }

        public virtual bool Pop(DateTime dt, Pecuniam val, Pecuniam fee = null, string note = null)
        {
            if (val == Pecuniam.Zero)
                return true;
            if (GetStatus(dt) != SpStatus.Current)
                return false;
            if (val > Balance.GetCurrent(dt, 0F))
                return false;
            Balance.AddTransaction(dt, val.Neg, fee, note);
            return true;
        }

        /// <summary>
        /// Helper method to copy the debit transactions within any of <see cref="receivables"/>
        /// whose <see cref="ITransaction.AtTime"/> is on the <see cref="dt"/>.
        /// </summary>
        /// <param name="dt">Defaults to today, ranges between 00:00:00.000 and 23:59:59.999 hours</param>
        /// <param name="receivables"></param>
        public void AddDebitTransactionsByDate(DateTime? dt, IList<IReceivable> receivables)
        {
            if (receivables == null || receivables.Count <= 0)
                return;

            var stDt = dt.GetValueOrDefault(DateTime.Now).Date;
            var endDt = dt.GetValueOrDefault(DateTime.Now).Date.AddDays(1).AddMilliseconds(-1);

            if (Terminus != null && stDt > Terminus.Value.Date)
                return;

            var trans = receivables.SelectMany(x => x.TradeLine.Balance.GetTransactionsBetween(stDt, endDt, true));

            foreach (var t in trans)
            {
                Pop(t.AtTime.AddMilliseconds(1), t.Cash, Pecuniam.Zero, t.Description);
            }
        }

        /// <summary>
        /// Moves <see cref="amt"/> out of <see cref="fromAccount"/> to <see cref="toAccount"/>.
        /// When the amount exceeds what is available it will be divided in half continously until
        /// it fits or goes down to one penny.
        /// </summary>
        /// <param name="fromAccount"></param>
        /// <param name="toAccount"></param>
        /// <param name="amt"></param>
        /// <param name="dt"></param>
        public static void TransferFundsInBankAccounts(DepositAccount fromAccount, DepositAccount toAccount, Pecuniam amt, DateTime dt)
        {
            if (fromAccount == null || toAccount == null || amt == null || amt == Pecuniam.Zero)
                return;
            if (fromAccount.GetStatus(dt) != SpStatus.Current && toAccount.GetStatus(dt) != SpStatus.Current)
                return;

            if (fromAccount.Inception < dt || toAccount.Inception < dt)
                return;
            amt = amt.Abs;

            while (fromAccount.CurrentValue < amt)
            {
                amt = amt / 2.ToPecuniam();
                if (amt.Amount < 0.01M)
                    break;
            }
            fromAccount.Pop(dt, amt, Pecuniam.Zero, WealthBase.GetPaymentNote(fromAccount.Id));
            toAccount.Push(dt.AddMilliseconds(100), amt, Pecuniam.Zero, WealthBase.GetPaymentNote(toAccount.Id));
        }

        #endregion
    }
}