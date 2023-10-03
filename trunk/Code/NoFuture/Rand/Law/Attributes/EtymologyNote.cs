using System;

namespace NoFuture.Law.Attributes
{
    public class EtymologyNote : Attribute
    {
        public string Lang { get; }
        public string Original { get; }
        public string Definition { get; }
        public string AsInExample { get; set; }

        public EtymologyNote(string lang, string original, string definition)
        {
            Lang = lang;
            Original = original;
            Definition = definition;
        }

        public EtymologyNote(string lang, string original, string definition, string asIn) : this(lang, original, definition)
        {
            AsInExample = asIn;
        }
    }
}
