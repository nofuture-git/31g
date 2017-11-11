using System;

namespace NoFuture.Rand.Data.Endo.Grps
{
    [Serializable]
    public class SocBoardGroup : ClassificationBase<SocDetailedOccupation>
    {
        public override string LocalName => "secondary-group";
    }
}