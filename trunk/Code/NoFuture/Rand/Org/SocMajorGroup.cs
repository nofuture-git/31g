using System;

namespace NoFuture.Rand.Org
{
    [Serializable]
    public class SocMajorGroup : ClassificationBase<SocMinorGroup>
    {
        public override string LocalName => "category";
    }
}