using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Term<T> : TermCategory, IComparable
    {
        public Term(string name, T reference)
        {
            RefersTo = reference;
            Name = name;
        }

        public Term(string name, T reference, params ITermCategory[] categories) : this(name, reference)
        {
            if (categories == null)
                return;
            foreach (var c in categories)
                As(c);
        }

        protected override string CategoryName => null;

        [Aka("denomination", "concept")]
        public T RefersTo { get;}

        [Aka("label", "notion")]
        public string Name { get;}

        public override int GetHashCode()
        {
            return RefersTo.GetHashCode();
        }

        public virtual int CompareTo(object obj)
        {
            var term = obj as Term<T>;
            return term == null ? 1 : string.Compare(Name, term.Name, StringComparison.Ordinal);
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

        public virtual bool EqualRefersTo(object obj)
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

        public static ISet<Term<T>> GetAgreedTerms(ISet<Term<T>> sorTerms, ISet<Term<T>> seeTerms, IReasonable reasoning = null)
        {
            var agreedTerms = new HashSet<Term<T>>();
            var agreedTermNames = GetInNameAgreedTerms(sorTerms, seeTerms, reasoning).Select(v => v.Name);
            if (!agreedTermNames.Any())
            {
                return agreedTerms;
            }
            foreach (var termName in agreedTermNames)
            {
                var offerorTerm = sorTerms.First(v => v.Name == termName);
                var offereeTerm = seeTerms.First(v => v.Name == termName);

                if (!offereeTerm?.EqualRefersTo(offerorTerm) ?? false)
                {
                    reasoning?.AddReasonEntry($"the term '{termName}' does not have the same meaning");
                    continue;
                }
                agreedTerms.Add(offerorTerm);
            }

            return agreedTerms;
        }

        public static ISet<Term<T>> GetInNameAgreedTerms(ISet<Term<T>> sorTerms, ISet<Term<T>> seeTerms, IReasonable reasoning = null)
        {
            if (sorTerms == null || seeTerms == null)
            {
                reasoning?.AddReasonEntry("one of the set of terms is missing.");
                return new HashSet<Term<T>>();
            }

            var agreedList = sorTerms.Where(oo => seeTerms.Any(ee => ee.Equals(oo))).ToList();
            if (!agreedList.Any())
            {
                reasoning?.AddReasonEntry("there are no terms shared");
                return new HashSet<Term<T>>();
            }
            var agreedTerms = new HashSet<Term<T>>();
            foreach (var t in agreedList)
            {
                agreedTerms.Add(t);
            }

            return agreedTerms;
        }

        public static ISet<Term<T>> GetAdditionalTerms(ISet<Term<T>> sorTerms, ISet<Term<T>> seeTerms, IReasonable reasoning = null)
        {
            var additionalTerms = new HashSet<Term<T>>();
            var agreedTermNames = GetInNameAgreedTerms(sorTerms, sorTerms);
            sorTerms.ExceptWith(agreedTermNames);
            seeTerms.ExceptWith(agreedTermNames);

            foreach (var sorTerm in sorTerms)
                additionalTerms.Add(sorTerm);
            foreach (var seeTerm in seeTerms)
                additionalTerms.Add(seeTerm);

            if (!additionalTerms.Any())
            {
                reasoning?.AddReasonEntry("there is not additonal terms present between");
            }

            return additionalTerms;
        }
    }
}
