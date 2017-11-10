using System;

namespace NoFuture.Rand.Data.Endo
{
    [Serializable]
    public class SocDetailedOccupation : ClassificationBase<ClassificationOfInstructionalPrograms>
    {
        public override string LocalName => "ternary-group";
    }
}