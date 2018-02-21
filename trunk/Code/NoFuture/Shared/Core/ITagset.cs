using System;

namespace NoFuture.Shared
{
    /// <summary>
    /// src [https://catalog.ldc.upenn.edu/docs/LDC99T42/tagguid1.pdf]
    /// </summary>
    public interface ITagset
    {
        string CharacterCodes { get; }
        string Value { get; set; }
        string Name { get; }
    }

    [Serializable]
    public abstract class TagsetBase : ITagset
    {
        public abstract string CharacterCodes { get; }
        public string Value { get; set; }
        public string Name => GetType().Name;

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
    public class NullTagset : TagsetBase
    {
        public override string CharacterCodes => null;
    }

    [Serializable]
    public class SentenceBreakPunctuation : TagsetBase
    {
        public override string CharacterCodes => ".";
    }

    [Serializable]
    public class Comma : TagsetBase
    {
        public override string CharacterCodes => ",";
    }

    [Serializable]
    public class CoordinatingConjunction : TagsetBase
    {
        public override string CharacterCodes => "CC";
    }

    [Serializable]
    public class CardinalNumber : TagsetBase
    {
        public override string CharacterCodes => "CD";
    }

    [Serializable]
    public class Determiner : TagsetBase
    {
        public override string CharacterCodes => "DT";
    }

    [Serializable]
    public class ExistentialThere : TagsetBase
    {
        public override string CharacterCodes => "EX";
    }

    [Serializable]
    public class ForeignWord : TagsetBase
    {
        public override string CharacterCodes => "FW";
    }

    [Serializable]
    public class PreposisionOrSubordinatingConjunction : TagsetBase
    {
        public override string CharacterCodes => "IN";
    }

    [Serializable]
    public class Adjective : TagsetBase
    {
        public override string CharacterCodes => "JJ";
    }

    [Serializable]
    public class AdjectiveComparative : TagsetBase
    {
        public override string CharacterCodes => "JJR";
    }

    [Serializable]
    public class AdjectiveSuperlative : TagsetBase
    {
        public override string CharacterCodes => "JJS";
    }

    [Serializable]
    public class ListItemMarker : TagsetBase
    {
        public override string CharacterCodes => "LS";
    }

    [Serializable]
    public class Modal : TagsetBase
    {
        public override string CharacterCodes => "MD";
    }

    [Serializable]
    public class NounSingularOrMass : TagsetBase
    {
        public override string CharacterCodes => "NN";
    }

    [Serializable]
    public class ProperNounSingular : TagsetBase
    {
        public override string CharacterCodes => "NNP";
    }

    [Serializable]
    public class ProperNounPlural : TagsetBase
    {
        public override string CharacterCodes => "NNPS";
    }

    [Serializable]
    public class NounPlural : TagsetBase
    {
        public override string CharacterCodes => "NNS";
    }

    [Serializable]
    public class Predeterminer : TagsetBase
    {
        public override string CharacterCodes => "PDT";
    }

    [Serializable]
    public class PossessiveEnding : TagsetBase
    {
        public override string CharacterCodes => "POS";
    }

    [Serializable]
    public class PersonalPronoun : TagsetBase
    {
        public override string CharacterCodes => "PRP";
    }

    [Serializable]
    public class PossessivePronoun : TagsetBase
    {
        public override string CharacterCodes => "PRP$";
    }

    [Serializable]
    public class Adverb : TagsetBase
    {
        public override string CharacterCodes => "RB";
    }

    [Serializable]
    public class AdverbComparative : TagsetBase
    {
        public override string CharacterCodes => "RBR";
    }

    [Serializable]
    public class AdverbSuperlative : TagsetBase
    {
        public override string CharacterCodes => "RBS";
    }

    [Serializable]
    public class Particle : TagsetBase
    {
        public override string CharacterCodes => "RP";
    }

    [Serializable]
    public class Symbol : TagsetBase
    {
        public override string CharacterCodes => "SYM";
    }

    [Serializable]
    public class To : TagsetBase
    {
        public override string CharacterCodes => "TO";
    }

    [Serializable]
    public class Interjection : TagsetBase
    {
        public override string CharacterCodes => "UH";
    }

    [Serializable]
    public class VerbBaseForm : TagsetBase
    {
        public override string CharacterCodes => "VB";
    }

    [Serializable]
    public class VerbPastTense : TagsetBase
    {
        public override string CharacterCodes => "VBD";
    }

    [Serializable]
    public class VerbGerundOrPersentParticiple : TagsetBase
    {
        public override string CharacterCodes => "VBG";
    }

    [Serializable]
    public class VerbPastParticiple : TagsetBase
    {
        public override string CharacterCodes => "VBN";
    }

    [Serializable]
    public class VerbNon3rdPersonSingularPresent : TagsetBase
    {
        public override string CharacterCodes => "VBP";
    }

    [Serializable]
    public class Verb3rdPersonSingularPresent : TagsetBase
    {
        public override string CharacterCodes => "VBZ";
    }

    [Serializable]
    public class Wh_Determiner : TagsetBase
    {
        public override string CharacterCodes => "WDT";
    }

    [Serializable]
    public class Wh_Pronoun : TagsetBase
    {
        public override string CharacterCodes => "WP";
    }

    [Serializable]
    public class PossessiveWh_pronoun : TagsetBase
    {
        public override string CharacterCodes => "WP$";
    }

    [Serializable]
    public class Wh_Adverb : TagsetBase
    {
        public override string CharacterCodes => "WRB";
    }

}
