using System;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public class SocMajorGroup : ClassificationBase<SocMinorGroup>
    {
        public override string LocalName => "category";
    }
}