﻿using System;

namespace NoFuture.Util.Core.Math
{
    public class LinearEquation : IEquation
    {
        //public LinearEquation() { }

        public LinearEquation(double slope, double intercept)
        {
            Intercept = intercept;
            Slope = slope;
        }

        public double Intercept { get;}
        public double Slope { get;}

        public virtual double SolveForY(double x)
        {
            return System.Math.Round(Slope*x + Intercept,5);
        }

        public virtual double SolveForX(double y)
        {
            if(System.Math.Abs(Slope) < 0.0000001D)
                return 0.0D;
            return (1/Slope)*y - (Intercept/Slope);
        }

        public virtual LinearEquation GetOrtroProj(double at)
        {
            var y = SolveForY(at);
            var recipSlope = -1 * (1 / Slope);
            var recipIntercept = y - recipSlope * at;
            return new LinearEquation(recipSlope, recipIntercept);
        }

        public virtual double EuclideanNorm
        {
            get
            {
                var myX = 1D;
                var myY = SolveForY(myX);
                return System.Math.Sqrt(System.Math.Pow(myX, 2) + System.Math.Pow(myY, 2));
            }
        }

        public override string ToString()
        {
            return $"f(x) = {Slope}x + {Intercept}";
        }

        /// <summary>
        /// Parse a string in the format of [intercept],[slope] 
        /// where both intercept and slope may be parsed to doubles
        /// and are joined by a comma.
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="lq"></param>
        /// <returns></returns>
        public static bool TryParse(string csv, out LinearEquation lq)
        {
            lq = null;
            if (string.IsNullOrWhiteSpace(csv) || !csv.Contains(","))
                return false;
            var interceptStr = csv.Split(',')[0];
            var slopeStr = csv.Split(',')[1];

            double intercept;
            double slope;
            if (double.TryParse(interceptStr, out intercept) && double.TryParse(slopeStr, out slope))
            {
                lq = new LinearEquation (slope, intercept);
            }
            return lq != null;
        }

        public static LinearEquation GetLineFromVectors(NfVector p, NfVector q)
        {
            var v = q - p;
            var ab = new NfVector(-1* v[1], v[0]).GetTranspose();
            var a = ab[0];
            var b = ab[1];
            var c = -1 * a * p[0] - b * p[1];
            var slope = (-1 * a) / b;
            var intercept = (-1 * c) / b;
            return new LinearEquation(slope, intercept);
        }
    }
}
