﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="TradeLine" />
    /// <inheritdoc cref="IVoca" />
    /// <summary>
    /// A capital concrete composition type to 
    /// bind value-in-time with a name.
    /// </summary>
    [Serializable]
    public class NamedTradeline : TradeLine, IVoca
    {
        private readonly IVoca _voca = new VocaBase();

        #region ctor
        public NamedTradeline(string name)
        {
            _voca.Name = name;
        }

        public NamedTradeline(IVoca names)
        {
            _voca.CopyNamesFrom(names);
        }

        public NamedTradeline(DateTime startDate) : base(startDate)
        {
        }

        #endregion

        public string Name
        {
            get => _voca.Name;
            set => _voca.Name = value;
        }

        public int NamesCount => _voca.NamesCount;

        public void AddName(KindsOfNames k, string name)
        {
            _voca.AddName(k, name);
        }

        public string GetName(KindsOfNames k)
        {
            return _voca.GetName(k);
        }

        public bool AnyNames(Predicate<KindsOfNames> filter)
        {
            return _voca.AnyNames(filter);
        }

        public bool AnyNames(Predicate<string> filter)
        {
            return _voca.AnyNames(filter);
        }

        public bool AnyNames(Func<KindsOfNames, string, bool> filter)
        {
            return _voca.AnyNames(filter);
        }

        public bool AnyNames()
        {
            return _voca.AnyNames();
        }

        public int RemoveName(Predicate<KindsOfNames> filter)
        {
            return _voca.RemoveName(filter);
        }

        public int RemoveName(Predicate<string> filter)
        {
            return _voca.RemoveName(filter);
        }

        public int RemoveName(Func<KindsOfNames, string, bool> filter)
        {
            return _voca.RemoveName(filter);
        }

        public KindsOfNames[] GetAllKindsOfNames()
        {
            return _voca.GetAllKindsOfNames();
        }

        public void CopyNamesFrom(IVoca voca)
        {
            _voca.CopyNamesFrom(voca);
        }

        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var d = Balance.GetCurrent(dt, 0.0F);
            return d < Pecuniam.Zero ? d.GetAbs() : Pecuniam.Zero;
        }

        /// <summary>
        /// Consider equality as being the same name at the same time
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //asserts the names equal
            if (!(obj is NamedTradeline p))
                return base.Equals(obj);

            var namesEqual = NamesEqual((IVoca) obj);

            var sDtEq = Inception == p.Inception;
            var eDtEq = Terminus == p.Terminus;

            return namesEqual && sDtEq && eDtEq;
        }

        public virtual bool NamesEqual(IVoca voca)
        {
            return voca != null && _voca.Equals(voca);
        }

        public override int GetHashCode()
        {
            return Inception.GetHashCode() + Terminus?.GetHashCode() ?? 1 + _voca?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var d = new Tuple<string, string, string, string, DateTime?, DateTime?>(Value.ToString(), Name,
                GetName(KindsOfNames.Group), DueFrequency.ToInterval().ToString(), Inception, Terminus);
            return d.ToString();
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var recvData = base.ToData(KindsOfTextCase.Pascel) ?? new Dictionary<string, object>();
            var itemData = new Dictionary<string, object>();
            var itemName = textFormat(Name.Replace(",", "").Replace(" ",""));
            foreach (var rData in recvData.Keys)
            {
                var v = recvData[rData]?.ToString();
                if (string.IsNullOrWhiteSpace(v))
                    continue;
                var itemDataName = textFormat(itemName + rData);
                if(itemData.ContainsKey(itemDataName))
                    continue;
                itemData.Add(itemDataName, v);
            }

            return itemData;
        }

        /// <summary>
        /// Produces a random <see cref="NamedTradeline"/> with random names 
        /// and transactions which average about to <see cref="averageAmount"/>
        /// </summary>
        /// <param name="name">
        /// Assigned to Legal name, will randomize to some english word if null.
        /// </param>
        /// <param name="groupName">
        /// Assigned to Group Name, will randomize to some english word if null
        /// </param>
        /// <param name="averageAmount">
        /// The random history will have many values whose average is more or less this value.
        /// </param>
        /// <param name="dueFrequency">
        /// Optional, if null then assigned to 30-days.
        /// Assigned to the resulting instance&apos;s property of the same name
        /// </param>
        /// <param name="inception">
        /// Optional, if null then random value some day in the past year.
        /// Assigned to the resulting instance&apos; property of the same name.
        /// </param>
        /// <param name="terminus">Passed directly to resulting instance</param>
        /// <param name="randomActsIrresponsible">A function pointer to a kind of personality</param>
        /// <returns></returns>
        [RandomFactory]
        public static NamedTradeline RandomNamedTradelineWithVariedHistory(
            string name = null, 
            string groupName = null, 
            Pecuniam averageAmount = null, 
            TimeSpan? dueFrequency = null,
            DateTime? inception = null, 
            DateTime? terminus = null, 
            Func<bool> randomActsIrresponsible = null)
        {
            name = name ?? Etx.RandomWord();
            groupName = groupName ?? Etx.RandomWord();
            var nr = new NamedTradeline(name)
            {
                Inception = inception ?? DateTime.Today.AddDays(Etx.RandomInteger(45, 360)),
                Terminus = terminus
            };
            nr.AddName(KindsOfNames.Group, groupName);

            nr.GetRandomHistory(averageAmount, false, randomActsIrresponsible);

            return nr;
        }

        /// <summary>
        /// Produces a random <see cref="NamedTradeline"/> with a history 
        /// </summary>
        /// <param name="name">
        /// Assigned to Legal name, will randomize to some english word if null.
        /// </param>
        /// <param name="groupName">
        /// Assigned to Group Name, will randomize to some english word if null
        /// </param>
        /// <param name="amount">
        /// Every entry in the history will have this exact value on a regular <see cref="dueFrequency"/>
        /// </param>
        /// <param name="dueFrequency">
        /// Optional, if null then assigned to 30-days.
        /// Assigned to the resulting instance&apos;s property of the same name
        /// </param>
        /// <param name="inception">
        /// Optional, if null then random value some day in the past year.
        /// Assigned to the resulting instance&apos; property of the same name.
        /// </param>
        /// <param name="terminus">Passed directly to resulting instance</param>
        /// <returns></returns>
        [RandomFactory]
        public static NamedTradeline RandomNamedTradelineWithSteadyHistory(
            string name = null, 
            string groupName = null,
            Pecuniam amount = null, 
            TimeSpan? dueFrequency = null, 
            DateTime? inception = null,
            DateTime? terminus = null)
        {
            name = name ?? Etx.RandomWord();
            groupName = groupName ?? Etx.RandomWord();
            var nr = new NamedTradeline(name)
            {
                Inception = inception ?? DateTime.Today.AddDays(Etx.RandomInteger(45, 360)),
                Terminus = terminus
            };
            nr.AddName(KindsOfNames.Group, groupName);
            nr.GetRandomHistory(amount);
            return nr;
        }


        /// <summary>
        /// Produces a random <see cref="TradeLine"/> with history sums to the given value
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static NamedTradeline RandomNamedTradelineWithHistoryToSum(
            string name = null,
            string groupName = null, 
            Pecuniam sumOfAllHistory = null,
            TimeSpan? dueFrequency = null, 
            DateTime? inception = null, 
            DateTime? terminus = null)
        {
            sumOfAllHistory = sumOfAllHistory ?? Pecuniam.RandomPecuniam(30, 200);
            var oneTropicalYearAgo = DateTime.Today.Add(Constants.TropicalYear.Negate());
            var start = inception ?? oneTropicalYearAgo;
            var tl = new NamedTradeline(new TransactionNote(name, groupName))
            {
                Inception = start,
                Terminus = terminus,
                DueFrequency = dueFrequency == null || dueFrequency == TimeSpan.MinValue ? PecuniamExtensions.GetTropicalMonth() : dueFrequency.Value
            };
            if (sumOfAllHistory == Pecuniam.Zero)
                return tl;
            var blocks = tl.GetWholeTimeBlocks();
            blocks = blocks == 0 ? 1 : blocks;
            var perBlockAmt = sumOfAllHistory.Amount / blocks;
            tl.GetRandomHistory(perBlockAmt.ToPecuniam());
            return tl;
        }
    }
}
