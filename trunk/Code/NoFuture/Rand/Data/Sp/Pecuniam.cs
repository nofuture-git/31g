using System;
using NoFuture.Exceptions;

namespace NoFuture.Rand.Data.Sp
{
    public interface IExchangeRate
    {
        CurrencyAbbrev CurrencyOut { get; }
        Pecuniam Exchange(Pecuniam p);
    }

    /// <summary>
    /// Basic Money object pattern 
    /// </summary>
    /// <remarks>
    /// Is Latin for Money
    /// </remarks>
    [Serializable]
    public class Pecuniam
    {
        #region fields
        private readonly decimal _amount;
        private readonly CurrencyAbbrev _currencyAbbrev;
        #endregion

        #region ctors
        public Pecuniam(Decimal amount, CurrencyAbbrev c = CurrencyAbbrev.USD)
        {
            _amount = amount;
            _currencyAbbrev = c;
        }
        #endregion

        #region properties
        public Decimal Amount => _amount;
        public CurrencyAbbrev CurrencyAbbrev => _currencyAbbrev;
        public Pecuniam Abs => new Pecuniam(Math.Abs(_amount));
        public Pecuniam Neg => new Pecuniam(-1*Math.Abs(_amount));
        public static Pecuniam Zero => new Pecuniam(0.0M);
        public Pecuniam Round => new Pecuniam(Math.Round(_amount, 2));
        #endregion

        #region overrides

        public override string ToString()
        {
            return $"{Amount} {CurrencyAbbrev}";
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

        public static Pecuniam operator +(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount + pp2.Amount);
        }

        public static Pecuniam operator -(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount - pp2.Amount);
        }

        public static Pecuniam operator *(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount * pp2.Amount);
        }

        public static Pecuniam operator /(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount / pp2.Amount);
        }

        public static bool operator ==(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;

            return pp1.Amount == pp2.Amount;
        }

        public static bool operator !=(Pecuniam p1, Pecuniam p2)
        {
            return !(p1 == p2);
        }

        public static bool operator >(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount > pp2.Amount;
        }

        public static bool operator <(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount < pp2.Amount;
        }

        public static bool operator >=(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount >= pp2.Amount;
        }

        public static bool operator <=(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount <= pp2.Amount;
        }

        public static Pecuniam GetRandPecuniam(int min = 3, int max = 999, int wholeNumbersOf = 0)
        {
            var num = (double)Etx.IntNumber(min, max);

            if (wholeNumbersOf >= 10)
                num = num - (num%wholeNumbersOf);
            else
            {
                num = Etx.RationalNumber(min, max);
            }

            return new Pecuniam((decimal) Math.Round(num,2));
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

    /// <summary>
    /// ISO 4217 Currency Codes
    /// </summary>
    [Serializable]
    public enum CurrencyAbbrev
    {
        USD,
        EUR,
        GBP,
        JPY,
        AUD,
        CAD,
        BRL,
        MXN,
        CNY,
    }
}
