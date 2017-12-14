﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Domus.Opes
{
    public class OpesOptions
    {
        private double _derivativeSlope;

        public bool IsVehiclePaidOff { get; set; }
        public bool IsRenting { get; set; }
        public int NumberOfVehicles { get; set; }
        public int NumberOfCreditCards { get; set; }

        public bool HasCreditCards => NumberOfCreditCards > 0;
        public bool HasVehicles => NumberOfVehicles > 0;
        public bool HasChildren => ChildrenAges != null && ChildrenAges.Any();

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<int> ChildrenAges { get; set; } = new List<int>();
        public Interval Interval { get; set; }

        public Pecuniam SumTotal { get; set; }
        public List<IMereo> GivenDirectly { get; } = new List<IMereo>();
        public List<string> PossiableZeroOuts { get; } = new List<string>();

        public double DerivativeSlope
        {
            get
            {
                if (_derivativeSlope <= 0.0001 && _derivativeSlope >= -0.0001)
                    _derivativeSlope = -1.0D;

                return _derivativeSlope;
            }
            set => _derivativeSlope = value;
        }

        public Func<int, Etx.Dice, bool> DiceRoll { get; set; } = Etx.TryBelowOrAt;

        public OpesOptions GetClone()
        {
            var o = new OpesOptions();

            var pi = GetType().GetProperties(NfConfig.DefaultFlags).Where(p => p.CanWrite).ToList();
            foreach (var p in pi)
            {
                var gVal = p.GetValue(this);
                p.SetValue(o, gVal);
            }

            foreach (var zo in PossiableZeroOuts)
                o.PossiableZeroOuts.Add(zo);

            foreach (var me in GivenDirectly)
                o.GivenDirectly.Add(new Mereo(me));

            return o;
        }
    }
}
