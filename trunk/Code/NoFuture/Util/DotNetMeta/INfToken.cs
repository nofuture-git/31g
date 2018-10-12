namespace NoFuture.Util.DotNetMeta
{
    public interface INfToken
    {
        int Id { get; set; }
        string Name { get; set; }
        int Count { get; }

        int GetFullDepthCount();
        int GetNameHashCode();
        bool IsFlattened();
    }
}
