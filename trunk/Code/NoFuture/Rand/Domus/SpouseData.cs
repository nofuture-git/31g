using System;

namespace NoFuture.Rand.Domus
{
    public class SpouseData
    {
        public IPerson Spouse { get; set; }
        public DateTime MarriedOn { get; set; }
        public DateTime? SeparatedOn { get; set; }
        public int Ordinal { get; set; }

        public override bool Equals(object obj)
        {
            var sd = obj as SpouseData;
            if (sd == null)
                return false;

            var mdq = Mdq(sd);

            var ddq = Ddq(sd);

            return mdq && ddq;
        }

        public override int GetHashCode()
        {
            var sh = Spouse != null ? Spouse.GetHashCode() : 0;
            var mo = MarriedOn.GetHashCode();
            var so = SeparatedOn.HasValue ? SeparatedOn.Value.GetHashCode() : 0;
            var o = Ordinal.GetHashCode();

            return sh + mo + so + o;
        }

        private bool Mdq(SpouseData sd) { return DateTime.Compare(MarriedOn.Date, sd.MarriedOn.Date) == 0; }
        private bool Ddq(SpouseData sd)
        {
            return DateTime.Compare(SeparatedOn.GetValueOrDefault(MarriedOn.Date).Date,
                sd.SeparatedOn.GetValueOrDefault(sd.MarriedOn.Date)) == 0;
        }
    }
}