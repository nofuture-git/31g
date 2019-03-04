using System;

namespace NoFuture.Rand.Law.Attributes
{
    public class EtymologyNote : Attribute
    {
        public string Lang { get; }
        public string Original { get; }
        public string Definition { get; }
        public Tuple<string, string, string> Note { get; }

        public EtymologyNote(string lang, string original, string definition)
        {
            Lang = lang;
            Original = original;
            Definition = definition;
        }
    }
}
