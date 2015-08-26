using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace NoFuture.Util.Pos
{
    public interface ITagset
    {
        string CharacterCodes { get; }
        string Description { get; }
        string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    public abstract class TagsetBase : ITagset
    {
        [DataMember] 
        public abstract string CharacterCodes { get; }
        [DataMember]
        public abstract string Description { get; }
        [DataMember]
        public abstract string Value { get; set; }
    }

    /// <summary>
    /// Null version of a Tagset
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class NullTagset : TagsetBase
    {
        [DataMember] 
        public override string CharacterCodes { get { return null; } }
        [DataMember]
        public override string Description { get { return null; } }
        [DataMember]
        public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class SentenceBreakPunctuation : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "."; } }
        [DataMember] public override string Description { get { return ". ! or ?"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Comma : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return ","; } }
        [DataMember] public override string Description { get { return "a comma"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class CoordinatingConjunction : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "CC"; } }
        [DataMember] public override string Description { get { return "e.g. and,but,or..."; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class CardinalNumber : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "CD"; } }
        [DataMember] public override string Description { get { return "e.g. 1, third"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Determiner : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "DT"; } }
        [DataMember] public override string Description { get { return "e.g. the"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ExistentialThere : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "EX"; } }
        [DataMember] public override string Description { get { return "e.g. there is"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ForeignWord : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "FW"; } }
        [DataMember] public override string Description { get { return "e.g. d'hoevre"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PreposisionOrSubordinatingConjunction : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "IN"; } }
        [DataMember] public override string Description { get { return "e.g. in, of, like"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Adjective : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "JJ"; } }
        [DataMember] public override string Description { get { return "e.g. green"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class AdjectiveComparative : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "JJR"; } }
        [DataMember] public override string Description { get { return "e.g. greener"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class AdjectiveSuperlative : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "JJS"; } }
        [DataMember] public override string Description { get { return "e.g. greenest"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ListItemMarker : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "LS"; } }
        [DataMember] public override string Description { get { return "e.g. 1.)"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Modal : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "MD"; } }
        [DataMember] public override string Description { get { return "denoting the mood of a verb, e.g. can, could, might, may..."; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class NounSingularOrMass : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "NN"; } }
        [DataMember] public override string Description { get { return "e.g. table"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ProperNounSingular : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "NNP"; } }
        [DataMember] public override string Description { get { return "e.g. tables"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ProperNounPlural : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "NNPS"; } }
        [DataMember] public override string Description { get { return "e.g. Beth"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class NounPlural : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "NNS"; } }
        [DataMember] public override string Description { get { return "e.g. Romans"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Predeterminer : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "PDT"; } }
        [DataMember] public override string Description { get { return "e.g. all, both ... when they precede an article"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PossessiveEnding : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "POS"; } }
        [DataMember] public override string Description { get { return "e.g. Nouns ending in 's"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PersonalPronoun : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "PRP"; } }
        [DataMember] public override string Description { get { return "e.g. I, me, you, he..."; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PossessivePronoun : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "PRP$"; } }
        [DataMember] public override string Description { get { return "e.g. my, your, mine, yours..."; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Adverb : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "RB"; } }
        [DataMember] public override string Description { get { return "Most words that end in -ly as well as degree words like quite, too and very"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class AdverbComparative : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "RBR"; } }
        [DataMember] public override string Description { get { return "Adverbs with the comparative ending -er, with a strictly comparative meaning."; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class AdverbSuperlative : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "RBS"; } }
        [DataMember] public override string Description { get { return "e.g. best"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Particle : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "RP"; } }
        [DataMember] public override string Description { get { return "e.g. give up"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Symbol : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "SYM"; } }
        [DataMember] public override string Description { get { return "Should be used for mathematical, scientific or technical symbols"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class To : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "TO"; } }
        [DataMember] public override string Description { get { return "to"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Interjection : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "UH"; } }
        [DataMember] public override string Description { get { return "e.g. uh, well, yes, my..."; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbBaseForm : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "VB"; } }
        [DataMember] public override string Description { get { return "subsumes imperatives, infinitives and subjunctives; e.g. be"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbPastTense : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "VBD"; } }
        [DataMember] public override string Description { get { return "includes the conditional form of the verb to be; e.g. was, were"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbGerundOrPersentParticiple : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "VBG"; } }
        [DataMember] public override string Description { get { return "e.g. being"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbPastParticiple : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "VBN"; } }
        [DataMember] public override string Description { get { return "e.g. been"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbNon3rdPersonSingularPresent : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "VBP"; } }
        [DataMember] public override string Description { get { return "e.g. am, are"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Verb3rdPersonSingularPresent : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "VBZ"; } }
        [DataMember] public override string Description { get { return "e.g. is"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Wh_Determiner : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "WDT"; } }
        [DataMember] public override string Description { get { return "e.g. which, and that when it is used as a relative pronoun"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Wh_Pronoun : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "WP"; } }
        [DataMember] public override string Description { get { return "e.g. what, who, whom..."; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PossessiveWh_pronoun : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "WP$"; } }
        [DataMember] public override string Description { get { return "e.g. whose"; } }
        [DataMember] public override string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Wh_Adverb : TagsetBase
    {
        [DataMember] public override string CharacterCodes { get { return "WRB"; } }
        [DataMember] public override string Description { get { return "e.g. how, where, when, why"; } }
        [DataMember] public override string Value { get; set; }
    }

}
