﻿using System;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Core
{
    [Serializable]
    public class RLinearEquation : LinearEquation
    {
        public RLinearEquation(double slope, double intercept) : base(slope, intercept){ }

        public double StdDev { get; set; }

        /// <summary>
        /// Same as its super class only adds deviation so that recurrent invocations will 
        /// not result in the same value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override double SolveForY(double x)
        {
            var mean = base.SolveForY(x);

            return StdDev > 0 ? Etx.RandomValueInNormalDist(mean, StdDev) : mean;
        }
    }
}
