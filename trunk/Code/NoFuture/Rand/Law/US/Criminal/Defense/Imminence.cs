﻿using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense
{
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

        public Func<ILegalPerson, TimeSpan> GetMinimumResponseTime { get; set; } = lp => TimeSpan.Zero;

        public Predicate<TimeSpan> IsImmediatePresent { get; set; } = ts => ts <= NormalReactionTimeToDanger;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            var ts = GetMinimumResponseTime(defendant);
            if (!IsImmediatePresent(ts))
            {
                AddReasonEntry($"defendant, {defendant.Name}, with minimum " +
                               $"response time of {ts} had {nameof(IsImmediatePresent)} as false");
                return false;
            }

            return true;
        }
    }
}
