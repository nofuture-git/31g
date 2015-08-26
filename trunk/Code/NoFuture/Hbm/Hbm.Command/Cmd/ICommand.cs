using NoFuture.Hbm.Command.Receivers;

namespace NoFuture.Hbm.Command.Cmd
{
    public interface ICommand<T, L>
    {
        IReceiver<T, L> Execute(System.Security.Principal.IPrincipal user);
        bool AllowExecute(System.Security.Principal.IPrincipal user);
    }  
}
