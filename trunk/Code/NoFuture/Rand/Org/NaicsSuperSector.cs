using System;

namespace NoFuture.Rand.Org
{
    [Serializable]
    public class NaicsSuperSector : ClassificationBase<NaicsPrimarySector>
    {
        public override string LocalName => "category";
    }
}