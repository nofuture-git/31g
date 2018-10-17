namespace NoFuture.Util.DotNetMeta
{
    public interface INfToken
    {
        int Id { get; set; }
        string Name { get; set; }

        int Count();
        int GetFullDepthCount();
        int GetNameHashCode();
        bool IsFlattened();
    }
}
