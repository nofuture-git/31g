using System;
using System.ComponentModel;
using System.Globalization;

namespace NoFuture.Util.Core.Math
{
    public class LinearEquation : IEquation
    {
        public static double CloseEnough { get; set; } = 0.000001;

        public static int RoundTo
        {
            get
            {
                var s = CloseEnough.ToString(CultureInfo.InvariantCulture);
                return s.Split('.').Length;
            }
        }

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
            if(System.Math.Abs(Slope) < CloseEnough)
                return 0.0D;
            return (1/Slope)*y - (Intercept/Slope);
        }

        public virtual double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Slope;
                    case 1:
                        return Intercept;
                    default:
                        return 0D;
                }
            }
        }

        public virtual IEquation Clone()
        {
            return new LinearEquation(Slope, Intercept);
        }

        public virtual double EuclideanNorm => System.Math.Sqrt(System.Math.Pow(Slope, 2) + System.Math.Pow(Intercept, 2));

        public virtual LinearEquation Normalized
        {
            get
            {
                var eul = EuclideanNorm;
                return new LinearEquation(Slope / eul, Intercept / eul);
            }
        }

        public double GetDistance(LinearEquation p)
        {
            p = p ?? new LinearEquation(0, 0);
            return (this - p).EuclideanNorm;
        }

        public double GetDotProduct(LinearEquation p)
        {
            p = p ?? new LinearEquation(0, 0);
            var q = this;
            var qdotp = q * p;
            return qdotp[0] + qdotp[1];
        }

        internal LinearEquation GetDotProduct(double[,] matrix)
        {
            var rSlope = matrix[0, 0] * Slope + matrix[0, 1] * Intercept;
            var rIntercept = matrix[1, 0] * Slope + matrix[1, 1] * Intercept;

            return new LinearEquation(rSlope, rIntercept);
        }

        public double GetCosTheta(LinearEquation p)
        {
            p = p ?? new LinearEquation(0, 0);
            var q = this;

            return q.GetDotProduct(p) / (q.EuclideanNorm * p.EuclideanNorm);
        }

        public double GetAngle(LinearEquation p)
        {
            p = p ?? new LinearEquation(0, 0);
            var q = this;
            var cosTheta = q.GetCosTheta(p);
            var acosRadians = System.Math.Acos(cosTheta);
            return System.Math.Round(acosRadians * (180 / System.Math.PI), RoundTo);
        }

        public virtual LinearEquation GetOrthoProj(LinearEquation p)
        {
            p = p ?? new LinearEquation(0, 0);
            var q = this;

            return q * (q.GetDotProduct(p) / System.Math.Pow(q.EuclideanNorm, 2));
        }

        public virtual LinearEquation GetProjection(double degrees)
        {
            var ui = Extensions.ConvertDegrees2Radians(degrees);
            var uiSqrd = System.Math.Pow(ui, 2);
            return GetDotProduct(new[,] {{uiSqrd, uiSqrd}, {uiSqrd, uiSqrd}});
        }

        public LinearEquation GetTranspose()
        {
            return new LinearEquation(this[1], this[0]);
        }

        public LinearEquation GetReciprocal(Tuple<double, double> r)
        {
            r = r ?? new Tuple<double, double>(0, 0);
            return GetReciprocal(new LinearEquation(r.Item1, r.Item2));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal LinearEquation GetReciprocal(LinearEquation r)
        {
            r = r ?? new LinearEquation(0, 0);
            var recipSlope = -1 * (1 / Slope);
            var recipIntercept = r.Intercept - recipSlope * r.Slope;
            return new LinearEquation(recipSlope, recipIntercept);
        }

        public static Tuple<double, double, double> GetImplicitCoeffs(Tuple<double, double> p, Tuple<double, double> q)
        {
            return GetImplicitCoeffs(new LinearEquation(p.Item1, p.Item2), new LinearEquation(q.Item1, q.Item2));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static Tuple<double, double, double> GetImplicitCoeffs(LinearEquation p, LinearEquation q)
        {
            var v = q - p;
            var ab = new LinearEquation(v[0], -1 * v[1]).GetTranspose();
            var a = ab[0];
            var b = ab[1];
            var c = -1 * a * p[0] - b * p[1];
            return new Tuple<double, double, double>(a,b,c);
        }

        public static LinearEquation GetLineFromVectors(Tuple<double, double> p, Tuple<double, double> q)
        {
            return GetLineFromVectors(new LinearEquation(p.Item1, p.Item2), new LinearEquation(q.Item1, q.Item2));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static LinearEquation GetLineFromVectors(LinearEquation p, LinearEquation q)
        {
            var abc = GetImplicitCoeffs(p, q);
            var a = abc.Item1;
            var b = abc.Item2;
            var c = abc.Item3;
            var slope = (-1 * a) / b;
            var intercept = (-1 * c) / b;
            return new LinearEquation(slope, intercept);
        }

        public LinearEquation GetIntersect(LinearEquation p)
        {
            p = p ?? new LinearEquation(0, 0);
            var interceptTick = p.Intercept;
            var slopeTick = p.Slope;

            var interceptX = (interceptTick - Intercept) / (Slope - slopeTick);
            //plug it back into either
            var inteceptY = SolveForY(interceptX);
            return new LinearEquation(interceptX, inteceptY);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static double[,] GetRotationMatrix(double degrees)
        {
            var rad = Extensions.ConvertDegrees2Radians(degrees);
            return new[,]
            {
                {System.Math.Cos(rad), -1 * System.Math.Sin(rad)},
                {System.Math.Sin(rad), System.Math.Cos(rad)}
            };
        }

        public LinearEquation GetRotation(double degrees)
        {
            return GetDotProduct(GetRotationMatrix(degrees));
        }

        public LinearEquation GetShear()
        {
            var shear = new[,] {{1, 0}, {-1 * this[1] / this[0], 1}};
            return GetDotProduct(shear);
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

            if (double.TryParse(interceptStr, out var intercept) && double.TryParse(slopeStr, out var slope))
            {
                lq = new LinearEquation(slope, intercept);
            }
            return lq != null;
        }

        public static bool operator ==(LinearEquation v1, LinearEquation v2)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            v2 = v2 ?? new LinearEquation(0, 0);
            return v1.Equals(v2);
        }

        public static bool operator !=(LinearEquation v1, LinearEquation v2)
        {
            return !(v1 == v2);
        }

        public static LinearEquation operator +(LinearEquation v1, double s)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            return new LinearEquation(v1[0] + s, v1[1] + s);
        }

        public static LinearEquation operator -(LinearEquation v1, double s)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            return new LinearEquation(v1[0] - s, v1[1] - s);
        }

        public static LinearEquation operator *(LinearEquation v1, double s)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            return new LinearEquation(v1[0] * s, v1[1] * s);
        }

        public static LinearEquation operator /(LinearEquation v1, double s)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            return new LinearEquation(v1[0] / s, v1[1] / s);
        }

        public static LinearEquation operator +(LinearEquation v1, LinearEquation v2)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            v2 = v2 ?? new LinearEquation(0, 0);
            return new LinearEquation(v1[0] + v2[0], v1[1] + v2[1]);
        }

        public static LinearEquation operator -(LinearEquation v1, LinearEquation v2)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            v2 = v2 ?? new LinearEquation(0, 0);
            return new LinearEquation(v1[0] - v2[0], v1[1] - v2[1]);
        }

        public static LinearEquation operator *(LinearEquation v1, LinearEquation v2)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            v2 = v2 ?? new LinearEquation(0, 0);
            return new LinearEquation(v1[0] * v2[0], v1[1] * v2[1]);
        }

        public static LinearEquation operator /(LinearEquation v1, LinearEquation v2)
        {
            v1 = v1 ?? new LinearEquation(0, 0);
            v2 = v2 ?? new LinearEquation(0, 0);
            return new LinearEquation(v1[0] / v2[0], v1[1] / v2[1]);
        }

        public override int GetHashCode()
        {
            return Slope.GetHashCode() + Intercept.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LinearEquation))
                return false;

            var nfv = (LinearEquation)obj;

            var v0Eq = nfv.Slope - Slope < CloseEnough;
            var v1Eq = nfv.Intercept - Intercept < CloseEnough;
            return v1Eq && v0Eq;
        }
    }
}
