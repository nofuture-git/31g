﻿using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    public abstract class TransactionId : ITransactionId
    {
        protected internal TransactionId(DateTime atTime, IVoca description) :this(Guid.NewGuid(), atTime, description)
        {
        }

        private TransactionId(Guid uniqueId, DateTime atTime, IVoca description)
        {
            UniqueId = uniqueId;
            AtTime = atTime;
            Description = description;
        }

        public IVoca Description { get; protected internal set; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; protected internal set; }
    }

    public class TraceTransactionId : ITransactionId
    {
        public TraceTransactionId(ITransaction transactionId, DateTime? atTime = null, IVoca description = null)
        {
            if(transactionId == null)
                throw new ArgumentNullException(nameof(transactionId));

            Description = description ?? transactionId.Description;
            AtTime = atTime ?? transactionId.AtTime;
            UniqueId = transactionId.UniqueId;
            Trace = transactionId.Trace;
        }

        public TraceTransactionId(Guid uniqueId, IVoca description, DateTime atTime)
        {
            Description = description;
            UniqueId = uniqueId;
            AtTime = atTime;
        }
        public IVoca Description { get; protected internal set; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; protected internal set; }
        public override string ToString()
        {
            return new Tuple<Guid, string, DateTime>(UniqueId, Description?.Name, AtTime).ToString();
        }
    }
}