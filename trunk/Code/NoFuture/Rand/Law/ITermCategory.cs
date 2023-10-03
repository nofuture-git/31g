using System;

namespace NoFuture.Law
{
    public interface ITermCategory : IRankable
    {
        string GetCategory();
        bool IsCategory(ITermCategory category);
        ITermCategory As(ITermCategory category);
        bool IsCategory(Type category);
    }
}