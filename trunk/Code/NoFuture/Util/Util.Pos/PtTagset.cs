using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Util.Pos
{
    public class PtTagset
    {
        #region Fields
        private static readonly List<TagsetBase> _tagsets = new List<TagsetBase>();
        private static char? _tagDelimiter;
        #endregion

        #region Properties

        public static char TagDelimiter
        {
            get 
            {
                if (_tagDelimiter == null)
                    _tagDelimiter = DEFAULT_TAG_DELIMITER;
                return _tagDelimiter.Value;
            }
            set { _tagDelimiter = value; }
        }

        public const char DEFAULT_TAG_DELIMITER = '_';
        #endregion

        #region Methods
        /// <summary>
        /// Helper method parse a files content to the jagged array of <see cref="TagsetBase"/>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TagsetBase[][] ParseFile(string path)
        {
            if(!File.Exists(path))
                throw new ItsDeadJim(string.Format("There isn't a file at '{0}'.",path));
            TagsetBase[][] tagsOut;
            var fileContent = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(fileContent))
                throw new ItsDeadJim(string.Format("This file has no content '{0}'.", path));
            return !TryParse(fileContent, out tagsOut) ? null : tagsOut;
        }

        /// <summary>
        /// Attempts to parse a string on the <see cref="TagDelimiter"/>
        /// followed by a conversion of the string array of array's into <see cref="TagsetBase"/>
        /// list.
        /// </summary>
        /// <param name="taggedString"></param>
        /// <param name="tagsets"></param>
        /// <returns></returns>
        public static bool TryParse(string taggedString, out TagsetBase[][] tagsets)
        {
            if (string.IsNullOrWhiteSpace(taggedString))
            {
                tagsets = null;
                return false;
            }

            try
            {
                //get all tags as a single array
                TagsetBase[] allTags;
                if(!TryParse(taggedString,out allTags))
                {
                    tagsets = null;
                    return false;
                }

                //determine how many sentence breaks the array contains
                var numberOfSentences = allTags.Count(tag => tag.GetType() == typeof (SentenceBreakPunctuation));

                //instantiate the out variable
                tagsets = new TagsetBase[numberOfSentences+1][];//the sentence break and end-of-data may not be coterminus

                var sentenceIndex = 0;
                var tagBuffer = new List<TagsetBase>();
                for(var i = 0; i< allTags.Length; i++)
                {
                    var tagEntry = allTags[i];
                    tagBuffer.Add(tagEntry);

                    //flush the buffer on each sentence break or at the end of the array
                    if (tagEntry.GetType() == typeof(SentenceBreakPunctuation) || i == allTags.Length - 1)
                    {
                        tagsets[sentenceIndex] = tagBuffer.ToArray();
                        tagBuffer.Clear();

                        //increment the sentence counter
                        sentenceIndex += 1;    
                    }
                }
            }
            catch
            {
                tagsets = null;
                return false;
            }

            //protect calling assemlby from having to check for nulls when returning a jagged-array
            for (var i = 0; i < tagsets.Length; i++ )
            {
                if (tagsets[i] == null)
                    tagsets[i] = new TagsetBase[] {new NullTagset()};
            }

            return true;
        }

        /// <summary>
        /// Attempts to parse a string on the <see cref="TagDelimiter"/>
        /// followed by a conversion of the string array into <see cref="TagsetBase"/>
        /// list.
        /// </summary>
        /// <param name="taggedString">The string to be parsed into a list of <see cref="TagsetBase"/>.</param>
        /// <param name="tagset">The resulting list, will be null when return value is false.</param>
        /// <remarks>
        /// The argument <see cref="taggedString"/> is split first on the space character (0x20) then each
        /// value in the resulting array is split on the <see cref="TagDelimiter"/>. Non-matching results are 
        /// assigned to a <see cref="NullTagset"/> instance - this is only the case when there was at least one
        /// valid match in the results.
        /// </remarks>
        /// <returns>True if even one match was found, false otherwise.</returns>
        public static bool TryParse(string taggedString, out TagsetBase[] tagset)
        {
            if(string.IsNullOrWhiteSpace(taggedString))
            {
                tagset = null;
                return false;
            }

            if(!taggedString.Contains(TagDelimiter.ToString()))
            {
                tagset = null;
                return false;
            }
            try
            {
                tagset = Parse(taggedString);
            }
            catch
            {
                tagset = null;
                return false;
            }

            if (tagset.Length == 0 || tagset.All(t => t.GetType() == typeof(NullTagset)))
            {
                tagset = null;
                return false;
            }
            return true;
        }

        public static TagsetBase[] Parse(string taggedString)
        {
            var tagset = new List<TagsetBase>();
            if (string.IsNullOrWhiteSpace(taggedString))
            {
                return tagset.ToArray();
            }

            if (!taggedString.Contains(TagDelimiter.ToString()))
            {
                return tagset.ToArray();
            }
            taggedString = NfString.DistillCrLf(taggedString);

            var valueArray = taggedString.Split(' ');

            foreach (var wordTagPair in valueArray)
            {
                if (!wordTagPair.Contains(TagDelimiter.ToString(CultureInfo.InvariantCulture)))
                {
                    tagset.Add(new NullTagset {Value = wordTagPair});
                    continue;
                }

                var tagPart = wordTagPair.Split(TagDelimiter)[1];
                var tag = GetTagset(tagPart);

                if (tag.GetType() == typeof (NullTagset))
                {
                    tag.Value = wordTagPair;
                    tagset.Add(tag);
                    continue;
                }

                tag.Value = wordTagPair.Split(TagDelimiter)[0];
                tagset.Add(tag);
            }
            return tagset.ToArray();

        }

        /// <summary>
        /// Returns a new instance of <see cref="TagsetBase"/> which matches the
        /// given <see cref="characterCode"/> or a new <see cref="NullTagset"/> object when no match is found.
        /// </summary>
        /// <param name="characterCode"></param>
        /// <returns></returns>
        public static TagsetBase GetTagset(string characterCode)
        {
            if(_tagsets.Count == 0)
            {
                _tagsets.Add(new Adjective());
                _tagsets.Add(new AdjectiveComparative());
                _tagsets.Add(new AdjectiveSuperlative());
                _tagsets.Add(new Adverb());
                _tagsets.Add(new AdverbComparative());
                _tagsets.Add(new AdverbSuperlative());
                _tagsets.Add(new CardinalNumber());
                _tagsets.Add(new CoordinatingConjunction());
                _tagsets.Add(new Determiner());
                _tagsets.Add(new ExistentialThere());
                _tagsets.Add(new ForeignWord());
                _tagsets.Add(new Interjection());
                _tagsets.Add(new ListItemMarker());
                _tagsets.Add(new Modal());
                _tagsets.Add(new NounPlural());
                _tagsets.Add(new NounSingularOrMass());
                _tagsets.Add(new Particle());
                _tagsets.Add(new PersonalPronoun());
                _tagsets.Add(new PossessiveEnding());
                _tagsets.Add(new PossessivePronoun());
                _tagsets.Add(new PossessiveWh_pronoun());
                _tagsets.Add(new Predeterminer());
                _tagsets.Add(new PreposisionOrSubordinatingConjunction());
                _tagsets.Add(new ProperNounPlural());
                _tagsets.Add(new ProperNounSingular());
                _tagsets.Add(new Symbol());
                _tagsets.Add(new To());
                _tagsets.Add(new Verb3rdPersonSingularPresent());
                _tagsets.Add(new VerbBaseForm());
                _tagsets.Add(new VerbGerundOrPersentParticiple());
                _tagsets.Add(new VerbNon3rdPersonSingularPresent());
                _tagsets.Add(new VerbPastParticiple());
                _tagsets.Add(new VerbPastTense());
                _tagsets.Add(new Wh_Adverb());
                _tagsets.Add(new Wh_Determiner());
                _tagsets.Add(new Wh_Pronoun());
                _tagsets.Add(new SentenceBreakPunctuation());
                _tagsets.Add(new Comma());
            }
            var targetTag = _tagsets.FirstOrDefault(tag => tag.CharacterCodes == characterCode);
            if (targetTag == null)
                return new NullTagset();

            return (TagsetBase) Activator.CreateInstance(targetTag.GetType());

        }
        #endregion
    }
}
