using System;

namespace NoFuture.Rand.Law.Criminal.US.Defense
{
    /// <summary>
    /// means the attack is immediate and not something that will occur in the future or has occured in the past.
    /// </summary>
    public class Imminence : DefenseBase
    {
        public static readonly TimeSpan OneSecond = new TimeSpan(0,0,0,1);
        public static readonly TimeSpan OneMinute = new TimeSpan(0,0,1,0);

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

        public Imminence(ICrime crime) : base(crime)
        {
        }

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
            var defendant = Crime.GetDefendant(persons);
            if (defendant == null)
                return false;

            var ts = GetResponseTime(defendant);
            if (!IsImmediatePresent(ts))
            {
                AddReasonEntry($"defendant, {defendant.Name}, with " +
                               $"response time of {ts} had {nameof(IsImmediatePresent)} as false");
                return false;
            }

            return true;
        }
    }
}
