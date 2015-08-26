namespace NoFuture.Hbm.Command.Receivers
{
    public interface IReceiver<T, L>
    {
        bool IsIdAssigned { get; }
        L DataId { get; }
        IReceiverStatus Status { get; set; }
        NHibernate.Criterion.ICriterion CustomSearch();

        string ToJson();

        T Data { get; set; }
    }
}
