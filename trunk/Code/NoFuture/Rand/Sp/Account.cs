using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="NamedReceivable" />
    /// <summary>
    /// Basic Accounting type entity
    /// </summary>
    public class Account : NamedReceivable, IAccount<Identifier>
    {
        protected Account(DateTime dateOpenned) : base(dateOpenned)
        {
            DueFrequency = TimeSpan.Zero;
            FormOfCredit = Enums.FormOfCredit.None;
        }

        public Identifier Id { get; set; }

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, 0.0F);
        }

        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var d = Balance.GetCurrent(dt, 0.0F);
            return d < Pecuniam.Zero ? d.GetAbs() : Pecuniam.Zero;
        }
    }
}