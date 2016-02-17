using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Exceptions;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public class SocialSecurityNumber : GovernmentId
    {
        private readonly List<SsnAnomaly> _anomalies = new List<SsnAnomaly>();
        private readonly string _regexPattern;

        public SocialSecurityNumber()
        {
            var areaNumber = Etx.MyRand.Next(1, 899);
            if (areaNumber == 666)
                areaNumber += 1;

            AreaNumber = string.Format("{0:000}", areaNumber);
            GroupNumber = string.Format("{0:00}", Etx.MyRand.Next(1, 99));
            SerialNumber = string.Format("{0:0000}", Etx.MyRand.Next(1, 9999));
            _value = string.Format("{0}-{1}-{2}", AreaNumber, GroupNumber, SerialNumber);
            var regexCatalog = new Shared.RegexCatalog();
            _regexPattern = regexCatalog.SSN;

        }

        public override string Abbrev { get { return "SSN"; } }

        /// <summary>
        /// i.e. the first three numbers
        /// </summary>
        public string AreaNumber { get; set; }

        /// <summary>
        /// i.e. the middle two numbers
        /// </summary>
        public string GroupNumber { get; set; }

        /// <summary>
        /// i.e. the last four numbers
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Prints in the classic ###-##-#### format.
        /// </summary>
        public override string Value
        {
            get { return _value; }
            set
            {
                if(!Validate(value))
                    throw new RahRowRagee(string.Format("The value {0} does not match " +
                                                        "the expect pattern of this id.", value));
                var parts = value.Split('-');
                AreaNumber = parts[0];
                GroupNumber = parts[1];
                SerialNumber = parts[2];
                _value = string.Format("{0}-{1}-{2}", AreaNumber, GroupNumber, SerialNumber);
            }
        }

        /// <summary>
        /// Expected to match the classic format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(value, _regexPattern);

        }

        /// <summary>
        /// List of <see cref="SsnAnomaly"/> attached to this <see cref="SocialSecurityNumber"/>
        /// </summary>
        public override List<Anomaly> Anomalies
        {
            get { return _anomalies.Cast<Anomaly>().ToList(); }
        }
    }

    #region Ssn anomalies

    /// <summary>
    /// Derived from Equifax document
    /// [Equifax_Fall_2014_Release_Guide.pdf]
    /// </summary>
    [Serializable]
    public abstract class SsnAnomaly : Anomaly { }

    [Serializable]
    public class SsnAssociatedWithDeceasedPerson : SsnAnomaly {
        public override string Abbrev { get { return "I"; } }
    }
    [Serializable]
    public class SsnMaybeTaxId : SsnAnomaly
    {
        public override string Abbrev { get { return "4"; } }
    }
    [Serializable]
    public class SsnInvalid : SsnAnomaly
    {
        public override string Abbrev { get { return "9"; } }
    }
    [Serializable]
    public class SsnNeverIssued : SsnAnomaly
    {
        public override string Abbrev { get { return "A"; } }
    }
    [Serializable]
    public class SsnRecentIssued : SsnAnomaly
    {
        public override string Abbrev { get { return "M"; } }
    }
    [Serializable]
    public class SsnIssuedPriorToDob : SsnAnomaly
    {
        public override string Abbrev { get { return "O"; } }
    }
    [Serializable]
    public class SsnReportedAsMisused : SsnAnomaly
    {
        public override string Abbrev { get { return "B"; } }
    }

    #endregion
}