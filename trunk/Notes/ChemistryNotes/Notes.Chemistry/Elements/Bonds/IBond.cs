namespace Notes.Chemistry.Elements.Bonds
{
    /// <summary>
    /// Formed by the electronic structure of atoms to fill the outermost shell.
    /// </summary>
    public interface IBond
    {
        IElement GetBondedAtom(IElement fromHere);
    }
}
