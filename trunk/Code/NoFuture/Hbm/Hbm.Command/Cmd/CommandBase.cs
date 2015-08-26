using System;
using System.Security.Principal;
using NoFuture.Hbm.Command.Receivers;

namespace NoFuture.Hbm.Command.Cmd
{
    [Serializable]
    public abstract class CommandBase<T, L> : ICommand<T, L>
    {
        protected internal IReceiver<T, L> MyReceiver;
        protected CommandBase(IReceiver<T, L> receiver)
        {
            MyReceiver = receiver;
        }
        public abstract IReceiver<T, L> Execute(IPrincipal user);
        public abstract bool AllowExecute(IPrincipal user);
    }
}
