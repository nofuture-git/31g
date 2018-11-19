using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="Receivable" />
    /// <inheritdoc cref="IVoca" />
    /// <summary>
    /// A capital concrete composition type to 
    /// bind time and names with expected and actual money value.
    /// </summary>
    [Serializable]
    public class NamedReceivable : Receivable, IVoca
    {
        private readonly IVoca _voca = new VocaBase();

        #region ctor
        public NamedReceivable(string name)
        {
            _voca.Name = name;
        }

        public NamedReceivable(IVoca names)
        {
            _voca.CopyFrom(names);
        }

        public NamedReceivable(DateTime startDate) : base(startDate)
        {
        }

        #endregion

        public string Name
        {
            get => _voca.Name;
            set => _voca.Name = value;
        }

        public void AddName(KindsOfNames k, string name)
        {
            _voca.AddName(k, name);
        }

        public string GetName(KindsOfNames k)
        {
            return _voca.GetName(k);
        }

        public bool AnyOfKind(KindsOfNames k)
        {
            return _voca.AnyOfKind(k);
        }

        public bool AnyOfKindContaining(KindsOfNames k)
        {
            return _voca.AnyOfKindContaining(k);
        }

        public bool AnyOfNameAs(string name)
        {
            return _voca.AnyOfNameAs(name);
        }

        public bool AnyOfKindAndValue(KindsOfNames k, string name)
        {
            return _voca.AnyOfKindAndValue(k, name);
        }

        public bool RemoveNameByKind(KindsOfNames k)
        {
            return _voca.RemoveNameByKind(k);
        }

        public int RemoveNameByValue(string name)
        {
            return _voca.RemoveNameByValue(name);
        }

        public bool RemoveNameByKindAndValue(KindsOfNames k, string name)
        {
            return _voca.RemoveNameByKindAndValue(k, name);
        }

        public int GetCountOfNames()
        {
            return _voca.GetCountOfNames();
        }

        public KindsOfNames[] GetAllKindsOfNames()
        {
            return _voca.GetAllKindsOfNames();
        }

        public void CopyFrom(IVoca voca)
        {
            _voca.CopyFrom(voca);
        }

        public static Pecuniam GetExpectedSum(IEnumerable<NamedReceivable> items)
        {
            if(items == null || !items.Any())
                return Pecuniam.Zero;

            var p = Pecuniam.Zero;
            foreach (var i in items)
                p += i?.Expectation?.Value ?? Pecuniam.Zero;
            return p;
        }

        /// <summary>
        /// Helper method to get an annual sum based on each items interval
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Pecuniam GetExpectedAnnualSum(IEnumerable<NamedReceivable> items)
        {
            if(items == null || !items.Any())
                return Pecuniam.Zero;
            var sum = 0M;
            foreach (var item in items)
            {
                //don't include if expectation is missing
                if (item?.Expectation?.Value == null)
                    continue;
                sum += item.Expectation.Value.Amount;
            }
            return new Pecuniam(sum);
        }

        /// <summary>
        /// Consider equality as being the same name at the same time
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //asserts the names equal
            if (!(obj is NamedReceivable p))
                return base.Equals(obj);
            var namesEqual = p.Expectation.Equals(Expectation);

            var sDtEq = Inception == p.Inception;
            var eDtEq = Terminus == p.Terminus;

            return namesEqual && sDtEq && eDtEq;
        }

        public override int GetHashCode()
        {
            return Inception.GetHashCode() + Terminus?.GetHashCode() ?? 1 + Expectation?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var d = new Tuple<string, string, string, string, DateTime?, DateTime?>(Expectation?.Value.ToString(), Name,
                GetName(KindsOfNames.Group), Expectation?.Interval.ToString(), Inception, Terminus);
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
                var itemDataName = textFormat(itemName + rData);
                if(itemData.ContainsKey(itemDataName))
                    continue;
                itemData.Add(itemDataName, recvData[rData]);
            }

            return itemData;
        }

        /// <summary>
        /// Produces a random <see cref="NamedReceivable"/> with random names 
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
        public static NamedReceivable RandomNamedReceivableWithVariedHistory(
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
            var nr = new NamedReceivable(name)
            {
                Inception = inception ?? DateTime.Today.AddDays(Etx.RandomInteger(45, 360)),
                Terminus = terminus
            };
            nr.AddName(KindsOfNames.Group, groupName);

            nr.GetRandomHistory(averageAmount, false, randomActsIrresponsible);

            return nr;
        }

        /// <summary>
        /// Produces a random <see cref="NamedReceivable"/> with a history 
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
        public static NamedReceivable RandomNamedReceivalbleWithSteadyHistory(
            string name = null, 
            string groupName = null,
            Pecuniam amount = null, 
            TimeSpan? dueFrequency = null, 
            DateTime? inception = null,
            DateTime? terminus = null)
        {
            name = name ?? Etx.RandomWord();
            groupName = groupName ?? Etx.RandomWord();
            var nr = new NamedReceivable(name)
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
        public static NamedReceivable RandomNamedReceivalbleWithHistoryToSum(
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
            var tl = new NamedReceivable(new VocaBase(name, groupName))
            {
                Inception = start,
                Terminus = terminus,
                DueFrequency = dueFrequency == null || dueFrequency == TimeSpan.MinValue ? PecuniamExtensions.GetTropicalMonth() : dueFrequency.Value
            };

            var blocks = tl.GetWholeTimeBlocks();
            blocks = blocks == 0 ? 1 : blocks;
            var perBlockAmt = sumOfAllHistory.Amount / blocks;
            tl.GetRandomHistory(perBlockAmt.ToPecuniam());
            return tl;
        }
    }
}
