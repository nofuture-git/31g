using System;
using NHibernate;
using NoFuture.Hbm.Command.Receivers;

namespace NoFuture.Hbm.Command.Cmd
{
    [Serializable]
    public abstract class SetCommandBase<T, L> : CommandBase<T, L>
    {
        protected SetCommandBase(IReceiver<T, L> receiver) : base(receiver) { }
        public override IReceiver<T, L> Execute(System.Security.Principal.IPrincipal user)
        {
            if (!AllowExecute(user))
            {
                MyReceiver.Status = new ReceiverStatus
                {
                    CommandResponseCode = ResponseCodes.RESPONSE_UNAUTH
                };
                return MyReceiver;
            }

            using (ISession session = Settings.OpenHbmSession)
            {
                using (var trans = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(MyReceiver.Data);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MyReceiver.Status = new ReceiverStatus
                        {
                            CommandResponseCode = ResponseCodes.RESPONSE_ERROR,
                            Error = ex
                        };
                        return MyReceiver;
                    }
                    MyReceiver.Data = session.Get<T>(MyReceiver.DataId);
                }
            }
            return MyReceiver;
        }
    }
}
