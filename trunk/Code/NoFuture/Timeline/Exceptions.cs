using System;

namespace NoFuture.Timeline
{
    public class DrawingException : Exception
    {
        public DrawingException(string msg) : base(msg) { }
    }//end TimelineDrawingException

    public class NoRuleSetException : DrawingException
    {
        public NoRuleSetException() : base("The Rule object is required.") { }
    }//end TimelineNoRuleSetException

    public class InnerBlockRequiresStartValue : DrawingException
    {
        public InnerBlockRequiresStartValue(Block block) : base(
            $"Block titled '{block.Title}' must have a start value different from the start value of the Rule.") { }
    }//InnerBlockRequiresStartValue

    public class TooWideForPdf : DrawingException
    {
        public TooWideForPdf() : base("the content is too large for even the A0 page size") { }
    }
}
