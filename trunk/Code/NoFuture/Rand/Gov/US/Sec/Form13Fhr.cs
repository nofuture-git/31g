using System;

namespace NoFuture.Rand.Gov.US.Sec
{
    /// <summary>
    /// A quarterly report filed by Fund Managers with the SEC
    /// </summary>
    [Serializable]
    public class Form13Fhr : SecForm
    {
        public const string ABBREV = "13F-HR";
        public Form13Fhr() : base(ABBREV) { }
    }
}