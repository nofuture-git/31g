using System;

namespace NoFuture.Rand.Sp.Enums
{
    [Flags]
    [Serializable]
    public enum FormOfCredit : short
    {
        None = 0,
        Revolving = 1,
        Installment = 2,
        Mortgage = 4,
        Fixed = 8,
    }
}