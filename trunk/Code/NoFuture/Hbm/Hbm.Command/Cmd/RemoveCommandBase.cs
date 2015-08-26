using System;
using NoFuture.Hbm.Command.Receivers;

namespace NoFuture.Hbm.Command.Cmd
{
    [Serializable]
    public abstract class RemoveCommandBase<T, L> : CommandBase<T, L>
    {
        protected RemoveCommandBase(IReceiver<T, L> receiver) : base(receiver) { }
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

            using (var session = Settings.OpenHbmSession)
            {
                using (var trans = session.BeginTransaction())
                {
                    try
                    {
                        MyReceiver.Data = session.Get<T>(MyReceiver.DataId);
                        session.Delete(MyReceiver.Data);
                        MyReceiver.Data = default(T);
                        MyReceiver.Status = new ReceiverStatus { CommandResponseCode = ResponseCodes.RESPONSE_OK };
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
                    }
                }
            }
            return MyReceiver;
        }
    }
}
