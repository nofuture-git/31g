namespace NoFuture.Util.DotNetMeta
{
    public interface INfToken
    {
        int Id { get; set; }
        string Name { get; set; }
        int GetFullDepthCount();
        int GetNameHashCode();
    }
}
