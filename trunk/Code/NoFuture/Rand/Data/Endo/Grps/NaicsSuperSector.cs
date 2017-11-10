using System;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public class NaicsSuperSector : ClassificationBase<NaicsPrimarySector>
    {
        public override string LocalName => "category";
    }
}