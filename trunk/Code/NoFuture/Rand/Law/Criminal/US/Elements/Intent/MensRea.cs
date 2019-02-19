using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent
{
    /// <inheritdoc cref="IMensRea"/>
    public abstract class MensRea : CriminalBase, IMensRea
    {
        public abstract int CompareTo(object obj);

        public virtual bool CompareTo(IActusReus criminalAct)
        {
            return true;
        }
    }
}
