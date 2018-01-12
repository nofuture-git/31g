﻿using System;
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
            var areaNumber = Etx.MyRand.Next(1, 899);
            if (areaNumber == 666)
                areaNumber += 1;

            AreaNumber = string.Format("{0:000}", areaNumber);
            GroupNumber = string.Format("{0:00}", Etx.MyRand.Next(1, 99));
            SerialNumber = string.Format("{0:0000}", Etx.MyRand.Next(1, 9999));
            _value = string.Format("{0}-{1}-{2}", AreaNumber, GroupNumber, SerialNumber);
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
                var parts = value.Split('-');
                AreaNumber = parts[0];
                GroupNumber = parts[1];
                SerialNumber = parts[2];
                _value = string.Join("-", AreaNumber, GroupNumber, SerialNumber);
            }
        }

        /// <summary>
        /// Expected to match the classic format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Validate(string value)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   System.Text.RegularExpressions.Regex.IsMatch(value, _regexPattern);
        }

    }
}