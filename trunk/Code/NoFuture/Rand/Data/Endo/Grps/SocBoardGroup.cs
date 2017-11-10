using System;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public class SocBoardGroup : ClassificationBase<SocDetailedOccupation>
    {
        public override string LocalName => "secondary-group";
    }
}