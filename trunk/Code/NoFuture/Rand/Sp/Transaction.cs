using System;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="TransactionId"/>
    /// <inheritdoc cref="ITransaction"/>
    /// <summary>
    /// Single immutable money transaction
    /// </summary>
    [Serializable]
    public class Transaction : TransactionId, ITransaction
    {
        protected internal Transaction(DateTime atTime, Pecuniam amt, IVoca description = null):base(atTime, description)
        {
            Cash = amt ?? Pecuniam.Zero;
        }
        
        public Pecuniam Cash { get; }

        #region methods

        public ITransaction GetInverse()
        {
            return new Transaction(AtTime, (Cash.Amount * -1M).ToPecuniam(), Description) {Trace = GetThisAsTraceId() };
        }

        public ITransaction Clone()
        {
            return new Transaction(AtTime, Cash, Description) {Trace = GetThisAsTraceId()};
        }

        public Tuple<ITransaction, ITransaction> SplitOnAmount(Pecuniam item1Amount, DateTime? atTime = null)
        {
            if(item1Amount == null || item1Amount == Pecuniam.Zero)
                throw new ArgumentNullException(nameof(item1Amount));

            if(item1Amount > Cash)
                throw new WatDaFookIzDis($"The split on value of {item1Amount} exceeds the total amount, of {Cash}, in this transaction.");

            return Split(item1Amount.ToDouble(), (Cash - item1Amount).ToDouble(), atTime);
        }

        public Tuple<ITransaction, ITransaction> SplitOnPercent(double percent, DateTime? atTime = null)
        {
            percent = Math.Abs(percent);

            //if call passes in something like '77.3' we will consider the same as '0.773'
            if (percent < 100.0 && percent > 1.0)
                percent = Math.Round(percent * 0.001, 3);

            if(percent == 0.0D || percent >= 1.0D)
                throw new ArgumentException($"The {nameof(percent)} value of {percent} is not understood.  " +
                                            "Use a value between 1.0 and 0.0");
            //expect some kind of floating point mis-balance
            var origVal = Math.Round(Cash.ToDouble(),2);
            var val1 = Math.Round(origVal * percent, 2);
            var val2 = Math.Round(origVal * percent, 2);

            return Split(val1, val2, atTime);
        }

        protected internal Tuple<ITransaction, ITransaction> Split(double val1, double val2, DateTime? atTime = null)
        {

            var dt = atTime.GetValueOrDefault(DateTime.UtcNow);
            var origVal = Math.Round(Cash.ToDouble(), 2);
            //first, determine if the rounding error went high or low
            var isSumGtCash = (val1 + val2) > origVal;
            var spinRound = 1;

            //the sum of the output must equal the original input to the one-hunderedth place
            while ((val1 + val2) != origVal)
            {
                //add or remove some from each in a round-robin
                if (spinRound % 2 == 0)
                {
                    val1 = isSumGtCash ? val1 - 0.01 : val1 + 0.01;
                    val1 = Math.Round(val1, 2);
                }
                else
                {
                    val2 = isSumGtCash ? val2 - 0.01 : val2 + 0.01;
                    val2 = Math.Round(val2, 2);
                }

                spinRound += 1;
            }
            var trans1 = new Transaction(dt, val1.ToPecuniam(), Description) { Trace = GetThisAsTraceId() };
            var trans2 = new Transaction(dt, val2.ToPecuniam(), Description) { Trace = GetThisAsTraceId() };

            return new Tuple<ITransaction, ITransaction>(trans1, trans2);
        }

        public TraceTransactionId GetThisAsTraceId(DateTime? atTime = null, IVoca journalName = null)
        {
            var dt = atTime ?? AtTime;

            //get copy of myself as a transaction id
            var innerTrace = new TraceTransactionId(UniqueId, Description, dt) { Trace = Trace };

            //with this, consider linked-list of trace as journal -> myself -> my-trace
            if (journalName != null && journalName.AnyNames())
            {
                innerTrace = new TraceTransactionId(Guid.Empty, journalName, dt)
                {
                    Trace = new TraceTransactionId(UniqueId, Description, AtTime) {Trace = Trace}
                };
            }

            return innerTrace;
        }

        public override bool Equals(object obj)
        {
            var t = obj as Transaction;
            return t != null && UniqueId.Equals(t.UniqueId);
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join("\t", UniqueId, $"{AtTime:yyyy-MM-dd HH:mm:ss.ffff}", $"{Cash.Amount:0.00}", Description);
        }

        #endregion
    }
}
