using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Gov.US
{
    [Serializable]
    public class SocialSecurityNumber : GovernmentId
    {
        private readonly string _regexPattern;

        public SocialSecurityNumber()
        {
            var regexCatalog = new RegexCatalog();
            _regexPattern = regexCatalog.SSN;

        }

        public override string Abbrev => "SSN";

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
            get => _value;
            set
            {
                if(!Validate(value))
                    throw new RahRowRagee($"The value {value} does not match " +
                                          "the expect pattern of this id.");
                var v = value;
                var nums = v.ToCharArray().Where(char.IsDigit);
                AreaNumber = new string(nums.Take(3).ToArray());
                GroupNumber = new string(nums.Skip(3).Take(2).ToArray());
                SerialNumber = new string(nums.Skip(5).Take(4).ToArray());
                _value = string.Join("-", AreaNumber, GroupNumber, SerialNumber);
            }
        }

        /// <summary>
        /// Gets just the numbers with no dash separators
        /// </summary>
        public string UnformattedValue => _value.Replace("-", "");

        /// <summary>
        /// Expected to match the classic format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Validate(string value = null)
        {
            value = value ?? _value;
            return !string.IsNullOrWhiteSpace(value) &&
                   System.Text.RegularExpressions.Regex.IsMatch(value, _regexPattern);
        }

        [RandomFactory]
        public static SocialSecurityNumber RandomSsn()
        {
            var areaNumber = Etx.MyRand.Next(1, 899);
            if (areaNumber == 666)
                areaNumber += 1;

            var ssn = new SocialSecurityNumber
            {
                AreaNumber = $"{areaNumber:000}",
                GroupNumber = $"{Etx.MyRand.Next(1, 99):00}",
                SerialNumber = $"{Etx.MyRand.Next(1, 9999):0000}"
            };

            ssn.Value = $"{ssn.AreaNumber}-{ssn.GroupNumber}-{ssn.SerialNumber}";

            return ssn;
        }
    }
}