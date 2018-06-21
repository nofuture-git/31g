using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Util.Core.Math;

namespace NoFuture.Util.Core.Algo
{
    public class Word2Vec
    {
        private const int MAX_STRING = 100;
        private const int EXP_TABLE_SIZE = 1000;
        private const int MAX_EXP = 6;
        private const int MAX_SENTENCE_LENGTH = 1000;
        private const int MAX_CODE_LENGTH = 40;

        
        private HuffmanEncoding _encoding;
        private string[] _allText;
        private int _currentAllTextPosition = 0;
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
            _expTable = new double[1,EXP_TABLE_SIZE];
            for (var j = 0; j < _expTable.GetLength(1); j++)
            {
                // Precompute the exp() table
                var expAtJ = System.Math.Exp((j / EXP_TABLE_SIZE * 2 - 1) * MAX_EXP);

                // Precompute f(x) = x / (x + 1)
                _expTable[0, j] = expAtJ / (expAtJ + 1);
            }
            

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

        public HuffmanEncoding Encoding => _encoding;

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
            if (string.IsNullOrWhiteSpace(corpus))
                throw new ArgumentNullException(nameof(corpus));

            foreach (var mod in _stringModifiers)
            {
                corpus = mod(corpus);
            }

            _allText = corpus.Split(' ');
            var vocab = _allText.Distinct().ToList();
            var dict = new Dictionary<string, int>();
            foreach (var v in vocab)
            {
                var cn = _allText.Count(a => a == v);
                dict.Add(v, cn);
            }
            _encoding = new HuffmanEncoding(dict);
        }

        public void BuildVocab(Dictionary<string, int> vocab)
        {
            _encoding = new HuffmanEncoding(vocab);
        }

        internal int GetNumberOfTrainWords()
        {
            //TODO is this supposed to be all of them?
            return Encoding.GetLengthLeafs();
        }

        internal string ReadNextWordFromCorpus()
        {
            if (_allText.Length >= _currentAllTextPosition)
                return null;
            var nextWord = _allText[_currentAllTextPosition];
            _currentAllTextPosition += 1;
            return nextWord;
        }

        internal double GetAdjustedAlpha(int currentWordCount)
        {
            var iter = NumberOfTrainingIterations;
            var trainWords = GetNumberOfTrainWords();
            var startingAlpha = Alpha;

            var alpha = startingAlpha * (1 - currentWordCount / (iter * trainWords + 1));
            if (alpha < startingAlpha * 0.0001)
                alpha = startingAlpha * 0.0001;
            return alpha;
        }

        internal void InitWiWo()
        {
            var v = Encoding.GetLengthLeafs();
            var n = Size;
            _wi = Matrix.RandomMatrix(v, n);
            _wo = Matrix.RandomMatrix(n, v);
        }

        internal List<HuffmanNode> GetSampleSentence()
        {
            var lout = new List<HuffmanNode>();
            while (lout.Count < MAX_SENTENCE_LENGTH)
            {
                var w = ReadNextWordFromCorpus();
                if (string.IsNullOrWhiteSpace(w))
                    break;
                var word = Encoding.GetLeafByWord(w);
                if (word == null)
                    break;
                if (Sample > 0)
                {
                    var ran = word.GetSampleValue(Sample, GetNumberOfTrainWords());
                    if(ran < _myRand.NextDouble())
                        continue;
                }
                lout.Add(word);
            }

            return lout;
        }

        internal double[,] GetNeu1(int sentencePostion, List<HuffmanNode> sampleSentence)
        {
            var neu1 = new double[1, Size];
            var b = _myRand.Next() % Window;
            for (var a = b; a < Window * 2 + 1 - b; a++)
            {
                if(a == Window)
                    continue;
                var c = sentencePostion - Window + a;
                if(c < 0 || c >= sampleSentence.Count)
                    continue;
                var lastWord = sampleSentence[c];
                if(lastWord == null || lastWord.Index < 0)
                    continue;

            }

            throw new NotImplementedException();
        }


    }
}
