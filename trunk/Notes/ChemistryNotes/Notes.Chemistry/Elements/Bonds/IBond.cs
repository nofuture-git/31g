namespace Notes.Chemistry.Elements.Bonds
{
    public interface IBond
    {
        IElement GetBondedAtom(IElement fromHere);
    }
}
