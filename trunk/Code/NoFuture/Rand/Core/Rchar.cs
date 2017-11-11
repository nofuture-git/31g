using System;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// A type for defining a character which is 
    /// randomized, but also constrained by some kind of rule.
    /// A string of these creates a highly controlled randomization pattern.
    /// </summary>
    [Serializable]
    public abstract class Rchar
    {
        protected readonly int idx;
        public abstract char Rand { get; }

        public virtual bool Valid(string dlValue)
        {
            if (string.IsNullOrWhiteSpace(dlValue))
                return false;
            return dlValue.Length - 1 >= idx;
        }
        protected Rchar(int indexValue)
        {
            idx = indexValue;
        }
    }
}
