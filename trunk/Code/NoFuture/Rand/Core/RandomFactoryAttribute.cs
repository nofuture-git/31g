using System;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Used to decorate a static factory method so they can be located easily.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    public class RandomFactoryAttribute : Attribute
    {
    }
}
