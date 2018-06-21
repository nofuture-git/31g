using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Util.Core.Math;
using static System.Diagnostics.Debug;

namespace NoFuture.Util.Core.Algo
{
    public class Word2Vec
    {
        private const int MAX_STRING = 100;
        private const int EXP_TABLE_SIZE = 1000;
        private const int MAX_EXP = 6;
        private const int MAX_SENTENCE_LENGTH = 1000;
        private const int MAX_CODE_LENGTH = 40;
        private const double ONLY_VALUE_IN_EXP_TABLE = 0.00247262315663477D;


        private HuffmanEncoding _vocab;
        private string[] _allText;
        private List<Func<string, string>> _stringModifiers = new List<Func<string, string>>();
        private double[,] _expTable;
        private double[,] _wi;
        private double[,] _wo;
        private readonly Random _myRand = new Random(Convert.ToInt32($"{DateTime.Now:ffffff}"));

        public Word2Vec()
        {
            
            Size = 100;
            Window = 5;
            Sample = 0.001;
            Negative = 5;
            NumberOfThreads = 12;
            NumberOfTrainingIterations = 5;
            MinCount = 5;
            DebugMode = 2;
            IsCbow = true;
            Alpha = 0.05;
        }

        #region properties from word2vec.c

        /// <summary>
        /// Use this location to save the resulting word vectors / word clusters
        /// </summary>
        public string SaveOutputTo { get; set; }

        /// <summary>
        /// Set size of word vectors; default is 100
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Set max skip length between words; default is 5
        /// </summary>
        public int Window { get; set; }

        /// <summary>
        /// Set threshold for occurrence of words. Those that appear with higher frequency in the training data
        /// will be randomly down-sampled; default is 1e-3, useful range is (0, 1e-5)
        /// </summary>
        public double Sample { get; set; }

        /// <summary>
        /// Use Hierarchical Softmax; default is false
        /// </summary>
        public bool UseHierarchicalSoftmax { get; set; }

        /// <summary>
        /// Number of negative examples; default is 5, common values are 3 - 10 (0 = not used)
        /// </summary>
        public int Negative { get; set; }

        /// <summary>
        /// Use this many threads (default 12)
        /// </summary>
        public int NumberOfThreads { get; set; }

        /// <summary>
        /// Run more training iterations (default 5)
        /// </summary>
        public int NumberOfTrainingIterations { get; set; }

        /// <summary>
        /// This will discard words that appear less than this number of times; default is 5
        /// </summary>
        public int MinCount { get; set; }

        /// <summary>
        /// Set the starting learning rate; default is 0.025 for skip-gram and 0.05 for CBOW
        /// </summary>
        public double Alpha { get; set; }

        /// <summary>
        /// Output word classes rather than word vectors; default number of classes is 0 (vectors are written)
        /// </summary>
        public bool IsOutputClasses { get; set; }

        /// <summary>
        /// Set the debug mode (default = 2 = more info during training)
        /// </summary>
        public int DebugMode { get; set; }

        /// <summary>
        /// Save the resulting vectors in binary moded; default is 0 (off)
        /// </summary>
        public string SaveResultVectorsTo { get; set; }

        /// <summary>
        /// The vocabulary will be saved to this location on the drive
        /// </summary>
        public string SaveVocabTo { get; set; }

        /// <summary>
        /// The vocabulary will be read from this location, not constructed from the training data
        /// </summary>
        public string ReadVocabFrom { get; set; }

        /// <summary>
        /// Use the continuous bag of words model; default is 1 (use 0 for skip-gram model)
        /// </summary>
        public bool IsCbow { get; set; }

        #endregion

        public HuffmanEncoding Vocab => _vocab;

        public int CurrentCorpusPosition { get; protected internal set; } = 0;

        /// <summary>
        /// Allows caller to control how the corpus text is modified
        /// before the vocab is built.
        /// </summary>
        /// <param name="modifier"></param>
        public void AddStringModifier(Func<string, string> modifier)
        {
            if (modifier == null)
                return;
            _stringModifiers.Add(modifier);
        }

        public void BuildVocab(string corpus)
        {
            WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fffff} {nameof(Word2Vec)} Start BuildVocab");

            AssignCorpus(corpus);

            var vocab = _allText.Distinct().ToList();
            var dict = new Dictionary<string, int>();
            foreach (var v in vocab)
            {
                if(string.IsNullOrWhiteSpace(v))
                    continue;
                var cn = _allText.Count(a => a == v);
                dict.Add(v, cn);
            }
            _vocab = new HuffmanEncoding(dict);
            WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fffff} {nameof(Word2Vec)} End BuildVocab");
        }

        public void AssignCorpus(string corpus)
        {
            if (string.IsNullOrWhiteSpace(corpus))
                throw new ArgumentNullException(nameof(corpus));

            if (!_stringModifiers.Any())
            {
                _stringModifiers.Add(c => c.ToLower());
                _stringModifiers.Add(c =>
                    new string(c.ToCharArray().Where(v => char.IsLetterOrDigit(v) || v == 0x20).ToArray()));
            }

            foreach (var mod in _stringModifiers)
            {
                corpus = mod(corpus);
            }

            _allText = corpus.Split(' ');
        }

        public void BuildVocab(Dictionary<string, int> vocab)
        {
            _vocab = new HuffmanEncoding(vocab);
        }

        /// <summary>
        /// Reads next batch of words off the corpus where Item1 is the target and 
        /// Item2 are the context words.
        /// </summary>
        /// <returns></returns>
        public Tuple<HuffmanNode, List<HuffmanNode>> ReadNextWord()
        {
            var target = GetTargetNode();
            if (target == null)
                return null;
            var contexts = GetContextNodes();

            CurrentCorpusPosition += 1;
            return new Tuple<HuffmanNode, List<HuffmanNode>>(target,contexts);
        }

        protected internal virtual string GetCorpusWordAt(int position)
        {
            if (position < 0 || _allText == null)
                return null;
            if (position >= _allText.Length)
                return null;
            return _allText[position];
        }

        internal int GetNumberOfTrainWords()
        {
            //TODO is this supposed to be all of them?
            return Vocab.GetLengthLeafs();
        }

        /// <summary>
        /// see word2vec.c @ ln 415
        /// </summary>
        /// <param name="fromCorpusPosition"></param>
        /// <returns></returns>
        internal double GetAdjustedAlpha(int? fromCorpusPosition = null)
        {
            var pos = fromCorpusPosition ?? CurrentCorpusPosition;
            var iter = NumberOfTrainingIterations;
            var trainWords = GetNumberOfTrainWords();
            var startingAlpha = Alpha;

            var alpha = startingAlpha * (1 - pos / (iter * trainWords + 1));
            if (alpha < startingAlpha * 0.0001)
                alpha = startingAlpha * 0.0001;
            return alpha;
        }

        internal void InitWiWo(int v = 0)
        {
            v = v > 0 ? v : Vocab.GetLengthLeafs();
            var n = Size;
            _wi = Matrix.RandomMatrix(v, n);
            _wo = Matrix.RandomMatrix(n, v);
        }

        internal HuffmanNode GetTargetNode(int? fromCorpusPosition = null)
        {
            var pos = fromCorpusPosition ?? CurrentCorpusPosition;
            var targetWord = GetCorpusWordAt(pos);
            if (string.IsNullOrWhiteSpace(targetWord))
                return null;
            return Vocab.GetLeafByWord(targetWord);
        }

        internal List<HuffmanNode> GetContextNodes(int? fromCorpusPosition = null, Tuple<int, int> windowRange = null)
        {
            var pos = fromCorpusPosition ?? CurrentCorpusPosition;

            var lout = new List<HuffmanNode>();
            var contextIndices = GetRandomIndicesAroundPosition(pos, windowRange);
            foreach (var contextIndex in contextIndices)
            {
                var contextWord = GetCorpusWordAt(contextIndex);
                if (string.IsNullOrWhiteSpace(contextWord))
                    continue;
                var contextNode = Vocab.GetLeafByWord(contextWord);
                if(contextNode?.Index == pos)
                    continue;
                lout.Add(contextNode);
            }

            return lout;
        }

        /// <summary>
        /// Gets random indices around the current <see cref="fromCorpusPosition"/>
        /// </summary>
        /// <param name="fromCorpusPosition"></param>
        /// <param name="windowRange">
        /// Optional, a tuple of values which go before and after <see cref="Window"/>
        /// will be generated randomly by <see cref="GetRandomWindowStartEnd"/> if null.
        /// </param>
        /// <returns>
        /// <![CDATA[
        /// example, sentence position is 28, and a random window is choosen of 2,9
        /// the return values would be 25,26,27,29,30,31
        /// ]]>
        /// </returns>
        internal int[] GetRandomIndicesAroundPosition(int? fromCorpusPosition = null, Tuple<int, int> windowRange = null)
        {
            var pos = fromCorpusPosition ?? CurrentCorpusPosition;
            windowRange = windowRange ?? GetRandomWindowStartEnd();
            var b = windowRange.Item1;
            var cIndices = new List<int>();

            for (var a = b; a <= windowRange.Item2; a++)
            {
                //means we are at the index of the Sentence Position
                if (a == Window)
                {
                    continue;
                }

                var c = pos - Window + a;
                if (c < 0)
                    continue;
                if (c >= MAX_SENTENCE_LENGTH)
                    continue;
                cIndices.Add(c);
            }

            return cIndices.ToArray();
        }

        internal Tuple<int, int> GetRandomWindowStartEnd()
        {
            var start = _myRand.Next() % Window;
            
            var end = Window * 2 - start;
            return new Tuple<int, int>(start,end);
        }

        internal double GetExpTableCalc(double f)
        {
            //maybe a casting thing in .NET but this is always the same value for all 1000 entries
            return ONLY_VALUE_IN_EXP_TABLE;
        }
    }
}
