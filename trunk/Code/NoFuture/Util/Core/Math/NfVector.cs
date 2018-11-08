using System;
using System.Runtime.CompilerServices;

namespace NoFuture.Util.Core.Math
{
    public class NfVector
    {
        private readonly double _v1;
        private readonly double _v2;

        internal const int ROUND_TO = 7;

        public NfVector(double v1, double v2)
        {
            _v1 = System.Math.Round(v1, ROUND_TO);
            _v2 = System.Math.Round(v2, ROUND_TO);
        }

        public double EuclideanNorm => System.Math.Sqrt(System.Math.Pow(_v1, 2) + System.Math.Pow(_v2, 2));

        public NfVector Normalized
        {
            get
            {
                var eul = EuclideanNorm;
                return new NfVector(_v1/eul, _v2/eul);
            }
        }

        public double GetDistance(NfVector p)
        {
            p = p ?? new NfVector(0, 0);
            return (this - p).EuclideanNorm;
        }

        public double GetDotProduct(NfVector p)
        {
            p = p ?? new NfVector(0, 0);
            var q = this;
            var qdotp = q * p;
            return qdotp[0] + qdotp[1];
        }

        public double GetCosTheta(NfVector p)
        {
            p = p ?? new NfVector(0, 0);
            var q = this;

            return q.GetDotProduct(p) / (q.EuclideanNorm * p.EuclideanNorm);
        }

        public double GetAngle(NfVector p)
        {
            p = p ?? new NfVector(0, 0);
            var q = this;
            var cosTheta = q.GetCosTheta(p);
            var acosRadians = System.Math.Acos(cosTheta);
            return System.Math.Round(acosRadians * (180 / System.Math.PI), ROUND_TO);
        }

        public NfVector GetOrthoProj(NfVector p)
        {
            p = p ?? new NfVector(0, 0);
            var q = this;

            return q * (q.GetDotProduct(p) / System.Math.Pow(q.EuclideanNorm, 2));
        }

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _v1;
                    case 1:
                        return _v2;
                    default:
                        return 0D;
                }
            }
        }

        public NfVector Clone()
        {
            return new NfVector(_v1, _v2);
        }

        public override int GetHashCode()
        {
            return _v1.GetHashCode() + _v2.GetHashCode();
        }

        public static bool operator ==(NfVector v1, NfVector v2)
        {
            v1 = v1 ?? new NfVector(0, 0);
            v2 = v2 ?? new NfVector(0, 0);
            return v1.Equals(v2);
        }

        public static bool operator !=(NfVector v1, NfVector v2)
        {
            return !(v1 == v2);
        }

        public static NfVector operator +(NfVector v1, double s)
        {
            v1 = v1 ?? new NfVector(0, 0);
            return new NfVector(v1[0] + s, v1[1] + s);
        }

        public static NfVector operator -(NfVector v1, double s)
        {
            v1 = v1 ?? new NfVector(0, 0);
            return new NfVector(v1[0] - s, v1[1] - s);
        }

        public static NfVector operator *(NfVector v1, double s)
        {
            v1 = v1 ?? new NfVector(0, 0);
            return new NfVector(v1[0] * s, v1[1] * s);
        }

        public static NfVector operator /(NfVector v1, double s)
        {
            v1 = v1 ?? new NfVector(0, 0);
            return new NfVector(v1[0] / s, v1[1] / s);
        }

        public static NfVector operator +(NfVector v1, NfVector v2)
        {
            v1 = v1 ?? new NfVector(0, 0);
            v2 = v2 ?? new NfVector(0, 0);
            return new NfVector(v1[0] + v2[0], v1[1] + v2[1]);
        }

        public static NfVector operator -(NfVector v1, NfVector v2)
        {
            v1 = v1 ?? new NfVector(0, 0);
            v2 = v2 ?? new NfVector(0, 0);
            return new NfVector(v1[0] - v2[0], v1[1] - v2[1]);
        }

        public static NfVector operator *(NfVector v1, NfVector v2)
        {
            v1 = v1 ?? new NfVector(0, 0);
            v2 = v2 ?? new NfVector(0, 0);
            return new NfVector(v1[0] * v2[0], v1[1] * v2[1]);
        }

        public static NfVector operator /(NfVector v1, NfVector v2)
        {
            v1 = v1 ?? new NfVector(0, 0);
            v2 = v2 ?? new NfVector(0, 0);
            return new NfVector(v1[0] / v2[0], v1[1] / v2[1]);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NfVector))
                return false;

            var nfv = (NfVector) obj;

            var v1Eq = nfv[0] - _v1 < 0.0000001;
            var v2Eq = nfv[1] - _v2 < 0.0000001;
            return v1Eq && v2Eq;
        }

        public override string ToString()
        {
            return new Tuple<double, double>(_v1, _v2).ToString();
        }
    }
}

