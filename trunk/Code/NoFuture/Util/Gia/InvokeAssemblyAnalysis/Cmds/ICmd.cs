namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    interface ICmd
    {
        byte[] Execute(byte[] arg);
    }
}
