using System;

namespace NoFuture.Rand.Gov.Nhtsa
{
    /// <summary>
    /// Is One based index
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class VinPositionAttribute : Attribute
    {
        public int Position1Base { get; }

        public int Len { get; }

        public VinPositionAttribute(int position, int length)
        {
            Position1Base = position;
            Len = length;
        }
    }
}