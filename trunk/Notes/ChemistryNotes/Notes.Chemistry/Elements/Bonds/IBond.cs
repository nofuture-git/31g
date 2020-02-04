namespace Notes.Chemistry.Elements.Bonds
{
    public interface IBond
    {
        IElement Atom1 { get; }
        IElement Atom2 { get; }
    }
}
