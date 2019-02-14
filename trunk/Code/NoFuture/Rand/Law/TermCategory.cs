using System;

namespace NoFuture.Rand.Law
{
    public abstract class TermCategory : ITermCategory
    {
        protected internal ITermCategory Term2Decorate { get; private set; }

        protected abstract string CategoryName { get; }

        public virtual int GetCategoryRank()
        {
            return Term2Decorate?.GetCategoryRank() ?? 0;
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

        /// <summary>
        /// Helper method to perform boolean operations on two <see cref="ITermCategory"/>
        /// </summary>
        public static bool IsRank(TermCategoryBoolOps op, ITermCategory t1, ITermCategory t2)
        {
            switch (op)
            {
                case TermCategoryBoolOps.Ne:
                    return (t1?.GetCategoryRank() ?? 0) != (t2?.GetCategoryRank() ?? 0);
                case TermCategoryBoolOps.Gt:
                    return (t1?.GetCategoryRank() ?? 0) > (t2?.GetCategoryRank() ?? 0);
                case TermCategoryBoolOps.Lt:
                    return (t1?.GetCategoryRank() ?? 0) < (t2?.GetCategoryRank() ?? 0);
                case TermCategoryBoolOps.Ge:
                    return (t1?.GetCategoryRank() ?? 0) >= (t2?.GetCategoryRank() ?? 0);
                case TermCategoryBoolOps.Le:
                    return (t1?.GetCategoryRank() ?? 0) <= (t2?.GetCategoryRank() ?? 0);
                default:
                    return (t1?.GetCategoryRank() ?? 0) == (t2?.GetCategoryRank() ?? -1);
            }
        }
    }
}