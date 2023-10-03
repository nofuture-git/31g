using System;
using System.Linq;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Defense.Excuse
{
    public abstract class InchoateDefenseBase : DefenseBase
    {
        public ICrime Crime { get; }

        protected InchoateDefenseBase(ICrime crime) : base(ExtensionMethods.Defendant)
        {
            Crime = crime;
        }

        protected virtual bool TestIsActusReusOfType(params Type[] types)
        {
            if (types == null || !types.Any())
                return true;
            var myName = GetType().Name;
            if (Crime == null)
            {
                AddReasonEntry($"there is not crime on which the {myName} to act as defense");
                return true;
            }

            var actusReus = Crime?.Concurrence?.ActusReus;
            if (actusReus == null)
            {
                AddReasonEntry("there is no actus reus for the given crime");
                return true;
            }

            if (types.Any(t => actusReus.GetType() == t))
                return true;

            var actualCriminalAct = actusReus?.GetType().Name;
            var expectedCriminalActs = string.Join(", ", types.Select(t => t.Name));
            AddReasonEntry($"{myName} defense is for {expectedCriminalActs} and not {actualCriminalAct}");
            return false;
        }

    }
}
