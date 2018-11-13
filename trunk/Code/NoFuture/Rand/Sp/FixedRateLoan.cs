using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class FixedRateLoan : LoanBase<float>
    {
        public FixedRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amount = null)
            : base(openedDate, minPaymentRate, amount)
        {
        }

        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, Rate);
        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, Rate);
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object>();

            


            return base.ToData(txtCase);
        }
    }
}