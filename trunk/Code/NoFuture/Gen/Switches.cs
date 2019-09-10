using System;

namespace NoFuture.Gen
{
    [Serializable]
    public enum CgClassModifier : short
    {
        AsIs = 0,
        AsPartial = 1,
        AsAbstract = 2,
        AsStatic = 4,
        AsInterface = 8
    }

    [Serializable]
    public enum CgAccessModifier : short
    {
        Public = 0,
        Family = 1,
        Assembly = 2,
        FamilyAssembly = 3,
        Private = 4
    }

    [Serializable]
    public enum CgLangs
    {
        Cs,
        Vb,
        Fs
    }

    [Serializable]
    public enum SearchDirection
    {
        Up,
        Down
    }

}
