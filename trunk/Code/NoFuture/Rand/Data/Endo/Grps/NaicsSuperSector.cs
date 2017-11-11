using System;

namespace NoFuture.Rand.Data.Endo.Grps
{
    [Serializable]
    public class NaicsSuperSector : ClassificationBase<NaicsPrimarySector>
    {
        public override string LocalName => "category";
    }
}