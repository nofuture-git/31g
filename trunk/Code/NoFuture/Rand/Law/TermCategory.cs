using System;

namespace NoFuture.Rand.Law
{
    public class TermCategory : ITermCategory
    {
        public TermCategory(): this(string.Empty) { }

        public TermCategory(string categoryName)
        {
            CategoryName = categoryName;
        }

        protected internal ITermCategory Term2Decorate { get; private set; }

        protected virtual string CategoryName { get; }

        public virtual int GetRank()
        {
            return Term2Decorate?.GetRank() ?? 0;
        }

        public virtual string GetCategory()
        {
            if (Term2Decorate == null)
                return CategoryName;

            return string.Join(", ", Term2Decorate.GetCategory(), CategoryName);
        }

        public virtual bool IsCategory(ITermCategory category)
        {
            if (category == null)
                return false;
            return IsCategory(category.GetType());
        }

        public ITermCategory As(ITermCategory category)
        {
            if (category == null)
                return this;

            if (Term2Decorate == null)
            {
                Term2Decorate = category;
                return this;
            }

            Term2Decorate.As(category);
            return this;
        }

        public virtual bool IsCategory(Type category)
        {
            if (category == null)
                return false;

            if (category == this.GetType())
                return true;

            return Term2Decorate?.IsCategory(category) ?? false;
        }

        public override string ToString()
        {
            return GetCategory();
        }

        public override bool Equals(object obj)
        {
            var r2 = obj as IRankable;
            if (r2 == null)
                return false;
            return GetRank() == r2.GetRank();
        }

        public override int GetHashCode()
        {
            return GetRank();
        }

        /// <summary>
        /// Helper method to perform boolean operations on two <see cref="IRankable"/>
        /// </summary>
        public static bool IsRank(TermCategoryBoolOps op, IRankable t1, IRankable t2)
        {
            switch (op)
            {
                case TermCategoryBoolOps.Ne:
                    return (t1?.GetRank() ?? 0) != (t2?.GetRank() ?? 0);
                case TermCategoryBoolOps.Gt:
                    return (t1?.GetRank() ?? 0) > (t2?.GetRank() ?? 0);
                case TermCategoryBoolOps.Lt:
                    return (t1?.GetRank() ?? 0) < (t2?.GetRank() ?? 0);
                case TermCategoryBoolOps.Ge:
                    return (t1?.GetRank() ?? 0) >= (t2?.GetRank() ?? 0);
                case TermCategoryBoolOps.Le:
                    return (t1?.GetRank() ?? 0) <= (t2?.GetRank() ?? 0);
                default:
                    return (t1?.GetRank() ?? 0) == (t2?.GetRank() ?? -1);
            }
        }

        public static bool operator ==(TermCategory r1, IRankable r2)
        {
            return IsRank(TermCategoryBoolOps.Eq, r1, r2);
        }

        public static bool operator !=(TermCategory r1, IRankable r2)
        {
            return IsRank(TermCategoryBoolOps.Ne, r1, r2);
        }

        public static bool operator >(TermCategory r1, IRankable r2)
        {
            return IsRank(TermCategoryBoolOps.Gt, r1, r2);
        }

        public static bool operator <(TermCategory r1, IRankable r2)
        {
            return IsRank(TermCategoryBoolOps.Lt, r1, r2);
        }

        public static bool operator >=(TermCategory r1, IRankable r2)
        {
            return IsRank(TermCategoryBoolOps.Ge, r1, r2);
        }

        public static bool operator <=(TermCategory r1, IRankable r2)
        {
            return IsRank(TermCategoryBoolOps.Le, r1, r2);
        }
    }
}