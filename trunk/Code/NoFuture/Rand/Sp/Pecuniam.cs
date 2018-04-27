using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Basic Money object pattern 
    /// </summary>
    /// <remarks>
    /// Is Latin for Money
    /// </remarks>
    [Serializable]
    public class Pecuniam : INumera
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
        public virtual Decimal Amount => _amount;
        public virtual Identifier Id => _currency;
        public static Pecuniam Zero => new Pecuniam(0.0M);
        #endregion

        #region methods
        public virtual Pecuniam GetAbs() => new Pecuniam(Math.Abs(_amount));
        public virtual Pecuniam GetNeg() => new Pecuniam(-1 * Math.Abs(_amount));
        public virtual Pecuniam GetRounded() => new Pecuniam(Math.Round(_amount, 2));

        public virtual double ToDouble()
        {
            return Convert.ToDouble(Amount);
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

        /// <summary>
        /// Creates a random valued money amount between the <see cref="min"/> and <see cref="max"/>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="wholeNumbersOf">
        /// used for producing random values in even decimal amounts (e.g. 100 produces
        /// a random value which is evenly divisible by 100 with no remainder)
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static Pecuniam RandomPecuniam(int min = 3, int max = 999, int wholeNumbersOf = 0)
        {
            var num = (double)Etx.RandomInteger(min, max);

            if (wholeNumbersOf >= 10)
                num = num - num % wholeNumbersOf;
            else
            {
                num = Etx.RandomDouble(min, max);
            }

            return new Pecuniam((decimal)Math.Round(num, 2));
        }
        #endregion

        #region operator overloads
        public static Pecuniam operator +(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (!pp1.Id.Equals(pp2.Id))
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount + pp2.Amount);
        }

        public static Pecuniam operator -(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (!pp1.Id.Equals(pp2.Id))
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount - pp2.Amount);
        }

        public static Pecuniam operator *(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (!pp1.Id.Equals(pp2.Id))
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount * pp2.Amount);
        }

        public static Pecuniam operator /(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (!pp1.Id.Equals(pp2.Id))
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount / pp2.Amount);
        }

        public static bool operator ==(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (!pp1.Id.Equals(pp2.Id))
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

            if (!pp1.Id.Equals(pp2.Id))
                return false;
            return pp1.Amount > pp2.Amount;
        }

        public static bool operator <(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (!pp1.Id.Equals(pp2.Id))
                return false;
            return pp1.Amount < pp2.Amount;
        }

        public static bool operator >=(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (!pp1.Id.Equals(pp2.Id))
                return false;
            return pp1.Amount >= pp2.Amount;
        }

        public static bool operator <=(Pecuniam p1, INumera p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (!pp1.Id.Equals(pp2.Id))
                return false;
            return pp1.Amount <= pp2.Amount;
        }

        #endregion
    }
}
