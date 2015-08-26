using System;
using NoFuture.Hbm.Command.Receivers;

namespace NoFuture.Hbm.Command.Cmd
{
    [Serializable]
    public abstract class GetCommandBase<T, L> : CommandBase<T, L>
    {
        protected GetCommandBase(IReceiver<T, L> receiver) : base(receiver) { }
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
                try
                {
                    if (!MyReceiver.IsIdAssigned)
                    {
                        var crit = session.CreateCriteria(typeof(T));
                        crit.Add(MyReceiver.CustomSearch());
                        MyReceiver.Data = (T)crit.UniqueResult();
                    }
                    else
                    {
                        MyReceiver.Data = session.Get<T>(MyReceiver.DataId);
                    }

                    MyReceiver.Status = new ReceiverStatus
                    {
                        CommandResponseCode = ResponseCodes.RESPONSE_OK
                    };
                }
                catch (Exception ex)
                {
                    MyReceiver.Status = new ReceiverStatus
                    {
                        CommandResponseCode = ResponseCodes.RESPONSE_ERROR,
                        Error = ex
                    };

                }
            }
            return MyReceiver;
        }
    }
}
