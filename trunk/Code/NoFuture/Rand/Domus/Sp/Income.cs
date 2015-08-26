using System;

namespace NoFuture.Rand.Domus.Sp
{
    [Serializable]
    public class Income
    {
        public TimeSpan Term { get; set; }
        public Pecuniam Gross { get; set; }
        public Pecuniam Witholdings { get; set; }
        public Pecuniam Tax { get; set; }
        public Pecuniam Net { get { return new Pecuniam(Gross.Amount - (Witholdings.Amount + Tax.Amount)); } }
    }
}
