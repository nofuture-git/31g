using System;

namespace NoFuture.Rand.Law.US
{
    /// <summary>
    /// means the attack is immediate and not something that will occur in the future or has occured in the past.
    /// </summary>
    public class Imminence : UnoHomine
    {
        public Imminence() : this(ExtensionMethods.Defendant) { }

        public Imminence(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public static readonly TimeSpan OneSecond = new TimeSpan(0, 0, 0, 1);
        public static readonly TimeSpan OneMinute = new TimeSpan(0, 0, 1, 0);
        public static readonly TimeSpan OneMoment = new TimeSpan(0, 0, 0, 90);

        /// <summary>
        /// <![CDATA[
        /// "a normal person can perceive and react to a danger in 1 1/212 seconds" 
        /// Bullock v. State, 775 A.2d. 1043 (2001)
        /// ]]>
        /// </summary>
        public static readonly TimeSpan NormalReactionTimeToDanger = new TimeSpan(0,0,0,1,(int)Math.Round(1d/212d * 1000));

        /// <summary>
        /// src https://copradar.com/redlight/factors/IEA2000_ABS51.pdf
        /// </summary>
        public static readonly TimeSpan AvgDriverCrashAvoidanceTime = new TimeSpan(0, 0, 0, 2, 250);

        /// <summary>
        /// src https://en.wikipedia.org/wiki/Nerve_conduction_velocity
        /// for Extrafusal muscle fibers as mean between 80-120 m/s
        /// </summary>
        public static readonly TimeSpan NerveConductionVelocity = new TimeSpan(0,0,0,0,100);

        /// <summary>
        /// The timespan of a persons response
        /// </summary>
        public Func<ILegalPerson, TimeSpan> GetResponseTime { get; set; } = lp => TimeSpan.Zero;

        /// <summary>
        /// The test for which a timespan is considered immediate, default is <see cref="NormalReactionTimeToDanger"/>
        /// </summary>
        public Predicate<TimeSpan> IsImmediatePresent { get; set; } = ts => ts <= NormalReactionTimeToDanger;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;

            var ts = GetResponseTime(defendant);
            var title = defendant.GetLegalPersonTypeName();
            if (!IsImmediatePresent(ts))
            {
                var tsPrint = ts.Equals(TimeSpan.Zero) ? "" : $"with response time of {ts} had ";
                AddReasonEntry($"{title}, {defendant.Name}, {tsPrint}{nameof(IsImmediatePresent)} as false");
                return false;
            }

            return true;
        }
    }
}
