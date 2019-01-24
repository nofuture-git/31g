using System;

namespace NoFuture.Rand.Law
{
    public interface ITermCategory
    {
        string GetCategory();
        bool IsCategory(ITermCategory category);
        ITermCategory As(ITermCategory category);
        bool IsCategory(Type category);
        int GetCategoryRank();
    }
}