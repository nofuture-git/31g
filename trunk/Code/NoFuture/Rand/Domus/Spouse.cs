using System;

namespace NoFuture.Rand.Domus
{
    public class Spouse : IRelation
    {
        public IPerson Est { get; set; }
        public DateTime MarriedOn { get; set; }
        public DateTime? SeparatedOn { get; set; }
        public int Ordinal { get; set; }

        /// <summary>
        /// Determins that <see cref="obj"/> is type <see cref="Spouse"/>
        /// and that both <see cref="MarriedOn"/> and <see cref="SeparatedOn"/>
        /// equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var sd = obj as Spouse;
            if (sd == null)
                return false;

            var mdq = Mdq(sd);

            var ddq = Ddq(sd);

            return mdq && ddq;
        }

        public override int GetHashCode()
        {
            var sh = Est != null ? Est.GetHashCode() : 0;
            var mo = MarriedOn.GetHashCode();
            var so = SeparatedOn?.GetHashCode() ?? 0;
            var o = Ordinal.GetHashCode();

            return sh + mo + so + o;
        }

        public override string ToString()
        {
            return string.Join(" ", Est.FirstName, Est.LastName);
        }

        private bool Mdq(Spouse sd) { return DateTime.Compare(MarriedOn.Date, sd.MarriedOn.Date) == 0; }
        private bool Ddq(Spouse sd)
        {
            return DateTime.Compare(SeparatedOn.GetValueOrDefault(MarriedOn.Date).Date,
                sd.SeparatedOn.GetValueOrDefault(sd.MarriedOn.Date)) == 0;
        }
    }
}