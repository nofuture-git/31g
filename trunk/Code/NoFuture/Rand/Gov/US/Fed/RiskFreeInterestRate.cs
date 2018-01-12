using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.Fed
{
    [Serializable]
    public class RiskFreeInterestRate : IIdentifier<float>
    {
        private float _val = DF_VALUE;
        public const float DF_VALUE = 1.5F;
        public string Src { get; set; }
        public string Abbrev => "RFIR";
        public float Value
        {
            get => _val;
            set => _val = value;
        }
    }
}