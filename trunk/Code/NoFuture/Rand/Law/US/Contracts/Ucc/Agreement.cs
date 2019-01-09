using System;

namespace NoFuture.Rand.Law.US.Contracts.Ucc
{
    /// <summary>
    /// <![CDATA[
    /// the bargain of the parties in fact, as found in their 
    /// language or inferred from other circumstances, including 
    /// course of performance, course of dealing, or usage of trade
    /// ]]>
    /// </summary>
    public abstract class Agreement : ObjectiveLegalConcept, IUccItem
    {
        public override bool IsEnforceableInCourt => true;
    }
}
