using System;

namespace NoFuture.Rand.Domus.Sp
{
    public interface IIncome
    {
        TimeSpan Term { get; set; }
        Pecuniam Gross { get; set; }
        Pecuniam Witholdings { get; set; }
        Pecuniam Tax { get; set; }
        Pecuniam Net { get; }
    }

    [Serializable]
    public class Income : IIncome
    {
        public TimeSpan Term { get; set; }
        public Pecuniam Gross { get; set; }
        public Pecuniam Witholdings { get; set; }
        public Pecuniam Tax { get; set; }
        public Pecuniam Net { get { return new Pecuniam(Gross.Amount - (Witholdings.Amount + Tax.Amount)); } }
    }
}
