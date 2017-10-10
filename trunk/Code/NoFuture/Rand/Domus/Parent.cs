﻿using System;

namespace NoFuture.Rand.Domus
{
    [Serializable]
    public class Parent : IRelation
    {
        public Parent(IPerson p)
        {
            Est = p;
        }
        public IPerson Est { get; }

        public override string ToString()
        {
            return string.Join(" ", Est?.FirstName, Est?.LastName);
        }

        public override int GetHashCode()
        {
            return Est?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var p = obj as Parent;
            if (p?.Est == null || Est == null)
                return false;

            return p.Est.Equals(Est);
        }
    }
}
