using System;
using NoFuture.Exceptions;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Basic Money object pattern 
    /// </summary>
    /// <remarks>
    /// Is Latin for Money
    /// </remarks>
    [Serializable]
    public sealed class Pecuniam : INumera
    {
        #region fields
        private readonly decimal _amount;
        private readonly Identifier _currency;
        #endregion

        #region ctors
        public Pecuniam(Decimal amount, CurrencyAbbrev c = CurrencyAbbrev.USD)
        {
            _amount = amount;
            _currency = new Currency(c);
        }
        #endregion

        #region properties
        public Decimal Amount => _amount;
        public Identifier Id => _currency;
        public Pecuniam Abs => new Pecuniam(Math.Abs(_amount));
        public Pecuniam Neg => new Pecuniam(-1*Math.Abs(_amount));
        public static Pecuniam Zero => new Pecuniam(0.0M);
        public Pecuniam Round => new Pecuniam(Math.Round(_amount, 2));
        #endregion

        #region methods
        public INumera Trade(INumera exchange, DateTime? dt, Pecuniam fee = null, string note = null)
        {
            var currency = exchange as Pecuniam;
            if (currency == null)
                return this;

            if(!Id.Equals(currency.Id))
                throw new NotImplementedException("Exchange rate logic not implemented");

            return this;
        }

        public override string ToString()
        {
            return $"{Amount} {Id}";
        }

        public override bool Equals(object obj)
        {
            var p1 = obj as Pecuniam;
            return p1?.Amount == Amount;
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

        public static Pecuniam GetRandPecuniam(int min = 3, int max = 999, int wholeNumbersOf = 0)
        {
            var num = (double)Etx.IntNumber(min, max);

            if (wholeNumbersOf >= 10)
                num = num - (num % wholeNumbersOf);
            else
            {
                num = Etx.RationalNumber(min, max);
            }

            return new Pecuniam((decimal)Math.Round(num, 2));
        }
        #endregion

        #region operator overloads
        public static Pecuniam operator +(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.Id.Equals(pp2.Id))
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount + pp2.Amount);
        }

        public static Pecuniam operator -(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.Id.Equals(pp2.Id))
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount - pp2.Amount);
        }

        public static Pecuniam operator *(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.Id.Equals(pp2.Id))
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount * pp2.Amount);
        }

        public static Pecuniam operator /(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.Id.Equals(pp2.Id))
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount / pp2.Amount);
        }

        public static bool operator ==(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.Id.Equals(pp2.Id))
                return false;

            return pp1.Amount == pp2.Amount;
        }

        public static bool operator !=(Pecuniam p1, INumera p2)
        {
            return !(p1 == p2);
        }

        public static bool operator >(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.Id.Equals(pp2.Id))
                return false;
            return pp1.Amount > pp2.Amount;
        }

        public static bool operator <(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.Id.Equals(pp2.Id))
                return false;
            return pp1.Amount < pp2.Amount;
        }

        public static bool operator >=(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.Id.Equals(pp2.Id))
                return false;
            return pp1.Amount >= pp2.Amount;
        }

        public static bool operator <=(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.Id.Equals(pp2.Id))
                return false;
            return pp1.Amount <= pp2.Amount;
        }

        #endregion
    }

    /// <summary>
    /// Nf money type extension methods
    /// </summary>
    public static class PecuniamExtensions
    {
        public static Pecuniam ToPecuniam(this double x)
        {
            return new Pecuniam((decimal)x);
        }
        public static Pecuniam ToPecuniam(this int x)
        {
            return new Pecuniam(x);
        }

        public static Pecuniam ToPecuniam(this decimal x)
        {
            return new Pecuniam(x);
        }
    }
}
