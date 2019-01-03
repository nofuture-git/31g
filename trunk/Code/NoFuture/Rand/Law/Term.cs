using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law
{
    /// <summary>
    /// &quot;The main difference between a term and a word is the nature of reference&quot;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// src [https://ac.els-cdn.com/S1877042816313283/1-s2.0-S1877042816313283-main.pdf?_tid=dbb6e307-e210-4d2f-bb6e-2c6e0c7e14f5&amp;acdnat=1545343418_43a6e6ffc058c5b95df8b9cf2a6e907f]
    /// </remarks>
    public class Term<T> : IComparable
    {
        public Term(string name, T reference)
        {
            RefersTo = reference;
            Name = name;
        }

        [Aka("denomination", "concept")]
        public T RefersTo { get;}

        [Aka("label", "notion")]
        public string Name { get;}

        public override int GetHashCode()
        {
            return RefersTo.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var term = obj as Term<T>;
            return term == null ? 1 : Name.CompareTo(term.Name);
        }

        /// <summary>
        /// Two terms are considered equal based on having the same name.
        /// Its up to the caller to then determine if the underlying <see cref="RefersTo"/>
        /// are the same thing.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var tt = obj as Term<T>;
            if (tt == null)
                return false;
            return tt.Name.Equals(Name);
        }

        public bool EqualRefersTo(object obj)
        {
            var tt = obj as Term<T>;
            if (tt == null)
                return false;
            return tt.RefersTo.Equals(RefersTo);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
