using System;

namespace NoFuture.Rand.Org
{
    [Serializable]
    public class SocBoardGroup : ClassificationBase<SocDetailedOccupation>
    {
        public override string LocalName => "secondary-group";
    }
}