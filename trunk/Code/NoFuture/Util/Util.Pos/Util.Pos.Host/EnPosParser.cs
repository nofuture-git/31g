using System;

namespace NoFuture.Util.Pos.WcfHost
{
    public class EnPosParser : IPosParser
    {
        public string TagString(string plainText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plainText))
                    return null;
                Program.PrintToConsole(string.Format("Tag text {0}", plainText));
                return plainText.ToTaggedString();

            }
            catch (Exception ex)
            {
                Program.exceptionCount += 1;
                Program.PrintToConsole(ex);
            }
            return null;
        }
    }
}
