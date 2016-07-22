using System;
using System.Runtime.Serialization;

namespace NoFuture.Util.Pos
{
    public interface ITagset
    {
        string CharacterCodes { get; }
        string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    public abstract class TagsetBase : ITagset
    {
        [DataMember] 
        public abstract string CharacterCodes { get; }
        [DataMember]
        public string Value { get; set; }

        public static string GetDescription(ITagset t)
        {
            if (t == null)
                return null;
            var tsTypeName = t.GetType().Name;
            switch (tsTypeName)
            {
                case "SentenceBreakPunctuation":
                    return ". ! or ?";
                case "Comma":
                    return "a comma";
                case "CoordinatingConjunction":
                    return "e.g. and,but,or...";
                case "CardinalNumber":
                    return "e.g. 1, third";
                case "Determiner":
                    return "e.g. the";
                case "ExistentialThere":
                    return "e.g. there is";
                case "ForeignWord":
                    return "e.g. d'hoevre";
                case "PreposisionOrSubordinatingConjunction":
                    return "e.g. in, of, like";
                case "Adjective":
                    return "e.g. green";
                case "AdjectiveComparative":
                    return "e.g. greener";
                case "AdjectiveSuperlative":
                    return "e.g. greenest";
                case "ListItemMarker":
                    return "e.g. 1.)";
                case "Modal":
                    return "denoting the mood of a verb, e.g. can, could, might, may...";
                case "NounSingularOrMass":
                    return "e.g. table";
                case "ProperNounSingular":
                    return "e.g. Beth";
                case "ProperNounPlural":
                    return "e.g. Romans";
                case "NounPlural":
                    return "e.g. tables";
                case "Predeterminer":
                    return "e.g. all, both ... when they precede an article";
                case "PossessiveEnding":
                    return "e.g. Nouns ending in 's";
                case "PersonalPronoun":
                    return "e.g. I, me, you, he...";
                case "PossessivePronoun":
                    return "e.g. my, your, mine, yours...";
                case "Adverb":
                    return "Most words that end in -ly as well as degree words like quite, too and very";
                case "AdverbComparative":
                    return "Adverbs with the comparative ending -er, with a strictly comparative meaning.";
                case "AdverbSuperlative":
                    return "e.g. best";
                case "Particle":
                    return "e.g. give up";
                case "Symbol":
                    return "Should be used for mathematical, scientific or technical symbols";
                case "To":
                    return "to";
                case "Interjection":
                    return "e.g. uh, well, yes, my...";
                case "VerbBaseForm":
                    return "subsumes imperatives, infinitives and subjunctives; e.g. be";
                case "VerbPastTense":
                    return "includes the conditional form of the verb to be; e.g. was, were";
                case "VerbGerundOrPersentParticiple":
                    return "e.g. being";
                case "VerbPastParticiple":
                    return "e.g. been";
                case "VerbNon3rdPersonSingularPresent":
                    return "e.g. am, are";
                case "Verb3rdPersonSingularPresent":
                    return "e.g. is";
                case "Wh_Determiner":
                    return "e.g. which, and that when it is used as a relative pronoun";
                case "Wh_Pronoun":
                    return "e.g. what, who, whom...";
                case "PossessiveWh_pronoun":
                    return "e.g. whose";
                case "Wh_Adverb":
                    return "e.g. how, where, when, why";
                default:
                    return null;
            }
        }
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
        public override string CharacterCodes => null;
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class SentenceBreakPunctuation : TagsetBase
    {
        [DataMember] public override string CharacterCodes => ".";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Comma : TagsetBase
    {
        [DataMember] public override string CharacterCodes => ",";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class CoordinatingConjunction : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "CC";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class CardinalNumber : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "CD";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Determiner : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "DT";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ExistentialThere : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "EX";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ForeignWord : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "FW";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PreposisionOrSubordinatingConjunction : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "IN";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Adjective : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "JJ";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class AdjectiveComparative : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "JJR";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class AdjectiveSuperlative : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "JJS";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ListItemMarker : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "LS";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Modal : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "MD";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class NounSingularOrMass : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "NN";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ProperNounSingular : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "NNP";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class ProperNounPlural : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "NNPS";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class NounPlural : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "NNS";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Predeterminer : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "PDT";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PossessiveEnding : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "POS";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PersonalPronoun : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "PRP";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PossessivePronoun : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "PRP$";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Adverb : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "RB";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class AdverbComparative : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "RBR";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class AdverbSuperlative : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "RBS";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Particle : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "RP";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Symbol : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "SYM";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class To : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "TO";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Interjection : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "UH";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbBaseForm : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "VB";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbPastTense : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "VBD";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbGerundOrPersentParticiple : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "VBG";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbPastParticiple : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "VBN";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class VerbNon3rdPersonSingularPresent : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "VBP";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Verb3rdPersonSingularPresent : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "VBZ";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Wh_Determiner : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "WDT";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Wh_Pronoun : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "WP";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class PossessiveWh_pronoun : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "WP$";
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(TagsetBase))]
    public class Wh_Adverb : TagsetBase
    {
        [DataMember] public override string CharacterCodes => "WRB";
    }

}
