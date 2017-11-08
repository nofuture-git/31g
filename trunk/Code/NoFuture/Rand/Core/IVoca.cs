using System;
using System.Collections.Generic;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Core
{
    /// <summary>
    /// Any type which could be given a name.
    /// </summary>
    /// <remarks>
    /// Latin for 'be called'
    /// </remarks>
    public interface IVoca
    {
        List<Tuple<KindsOfNames, string>> Names { get; }
        void UpsertName(KindsOfNames k, string name);
        string GetName(KindsOfNames k);
        bool AnyOfKindOfName(KindsOfNames k);
        bool AnyOfNameAs(string name);
        bool RemoveNameByKind(KindsOfNames k);
        int RemoveNameByValue(string name);
    }
}