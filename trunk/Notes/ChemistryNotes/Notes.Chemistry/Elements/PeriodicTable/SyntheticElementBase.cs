namespace Notes.Chemistry.Elements.PeriodicTable
{
    public abstract class SyntheticElementBase : ElementBase
    {
        public override bool IsArtificial => true;
        public override bool IsRadioactive => true;
    }
}