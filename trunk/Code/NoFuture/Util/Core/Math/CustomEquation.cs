﻿using System;

namespace NoFuture.Util.Core.Math
{
    /// <summary>
    /// Implements the <see cref="IEquation"/> while allowing for total control over the 
    /// actual implementation at runtime.
    /// </summary>
    public class CustomEquation : IEquation
    {
        private readonly Func<double, double> _equation;
        public CustomEquation(Func<double, double> myImplementation)
        {
            _equation = myImplementation ?? throw new ArgumentNullException(nameof(myImplementation));
        }

        public double SolveForY(double x)
        {
            return _equation(x);
        }
    }
}
