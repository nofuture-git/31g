using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Util.Core.Math;
using NoFuture.Util.Core.Math.Matrix;
using static System.Diagnostics.Debug;

namespace NoFuture.Util.Core.Algo
{
    /// <summary>
    /// only useful document, video, presentation, etc. that 
    /// I ever found that actually explained it https://arxiv.org/pdf/1411.2738.pdf
    /// js example https://ronxin.github.io/wevi/ - this is the basis of the unit tests
    /// </summary>
    public class Word2Vec
    {
        private const int MAX_SENTENCE_LENGTH = 1000;
        private string[] _allText;
        private readonly List<Func<string, string>> _stringModifiers = new List<Func<string, string>>();
        private readonly Random _myRand = new Random(Convert.ToInt32($"{DateTime.Now:ffffff}"));

        public Word2Vec()
        {
            
            Size = 100;
            Window = 5;
            Sample = 0.001;
            Negative = 5;
            NumberOfTrainingIterations = 5;
            MinCount = 5;
            IsDebugMode = false;
            IsCbow = true;
            LearningRate = 0.05;
        }

        #region properties from word2vec.c

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
        public double LearningRate { get; set; }

        /// <summary>
        /// Set the debug mode on \ off
        /// </summary>
        public bool IsDebugMode { get; set; }

        /// <summary>
        /// Use the continuous bag of words model; default is 1 (use 0 for skip-gram model)
        /// </summary>
        public bool IsCbow { get; set; }

        #endregion

        public HuffmanEncoding Vocab { get; private set; }
        public int CurrentCorpusPosition { get; protected internal set; } = 0;

        public double[,] WI { get; protected internal set; }
        public double[,] WO { get; protected internal set; }

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
                if(String.IsNullOrWhiteSpace(v))
                    continue;
                var cn = _allText.Count(a => a == v);
                dict.Add(v, cn);
            }
            Vocab = new HuffmanEncoding(dict);
            WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fffff} {nameof(Word2Vec)} End BuildVocab");
        }

        public void AssignCorpus(string corpus)
        {
            if (String.IsNullOrWhiteSpace(corpus))
                throw new ArgumentNullException(nameof(corpus));

            if (!_stringModifiers.Any())
            {
                _stringModifiers.Add(c => c.ToLower());
                _stringModifiers.Add(c =>
                    new string(c.ToCharArray().Where(v => Char.IsLetterOrDigit(v) || v == 0x20).ToArray()));
            }

            foreach (var mod in _stringModifiers)
            {
                corpus = mod(corpus);
            }

            _allText = corpus.Split(' ');
        }

        public void BuildVocab(Dictionary<string, int> vocab)
        {
            Vocab = new HuffmanEncoding(vocab);
        }

        /// <summary>
        /// Reads next batch of words off the corpus where Item1 is the target and 
        /// Item2 are the context words.
        /// </summary>
        /// <returns></returns>
        public virtual Word2VecInputs ReadNextWord()
        {
            var target = GetTargetNode();
            if (target == null)
                return null;
            var contexts = GetContextNodes();

            CurrentCorpusPosition += 1;
            return new Word2VecInputs {ContextWords = contexts, TargetWord = target};
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
            var startingAlpha = LearningRate;

            var alpha = startingAlpha * (1 - pos / (iter * trainWords + 1));
            if (alpha < startingAlpha * 0.0001)
                alpha = startingAlpha * 0.0001;
            return alpha;
        }

        protected internal virtual void InitWiWo(int v = 0)
        {
            v = v > 0 ? v : Vocab.GetLengthLeafs();
            var n = Size;
            //https://github.com/ronxin/wevi/blob/master/js/vector_math.js @ function get_random_init_weight(hidden_size)
            //values are always less than 0.1
            double GetInitWeight(double d) => (d - 0.5D) / n;

            WI = MatrixOps.RandomMatrix(v, n, GetInitWeight);
            System.Threading.Thread.Sleep(50);
            WO = MatrixOps.RandomMatrix(n, v, GetInitWeight);

        }

        protected internal virtual HuffmanNode GetTargetNode(int? fromCorpusPosition = null)
        {
            var pos = fromCorpusPosition ?? CurrentCorpusPosition;
            var targetWord = GetCorpusWordAt(pos);
            if (String.IsNullOrWhiteSpace(targetWord))
                return null;
            return Vocab.GetNodeByWord(targetWord);
        }

        protected internal virtual List<HuffmanNode> GetContextNodes(int? fromCorpusPosition = null, Tuple<int, int> windowRange = null)
        {
            var pos = fromCorpusPosition ?? CurrentCorpusPosition;

            var lout = new List<HuffmanNode>();
            var contextIndices = GetRandomIndicesAroundPosition(pos, windowRange);
            foreach (var contextIndex in contextIndices)
            {
                var contextWord = GetCorpusWordAt(contextIndex);
                if (String.IsNullOrWhiteSpace(contextWord))
                    continue;
                var contextNode = Vocab.GetNodeByWord(contextWord);
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
        protected internal virtual int[] GetRandomIndicesAroundPosition(int? fromCorpusPosition = null, Tuple<int, int> windowRange = null)
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

        protected internal virtual Tuple<int, int> GetRandomWindowStartEnd()
        {
            var start = _myRand.Next() % Window;
            
            var end = Window * 2 - start;
            return new Tuple<int, int>(start,end);
        }

        public double[,] ToVector(HuffmanNode node)
        {
            if(node == null)
                throw new ArgumentNullException(nameof(node));
            return MatrixOps.OneHotVector(node.Index, Vocab.GetLengthLeafs());
        }

        public double[,] ToVector(List<HuffmanNode> nodes)
        {
            if(nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var oneHot = new double[1,Vocab.GetLengthLeafs()];
            foreach (var node in nodes)
            {
                var nodeOneHot = MatrixOps.OneHotVector(node.Index, Vocab.GetLengthLeafs());
                for (var j = 0; j < nodeOneHot.CountOfColumns(); j++)
                {
                    oneHot[0, j] += nodeOneHot[0, j];
                }
            }

            //skip-gram output is actually as many '1's as context words
            if(IsCbow)
                oneHot.ApplyToEach(v => v / nodes.Count);

            return oneHot;
        }

        public virtual double[,] FeedFoward(Word2VecInputs inputs, double[,] oneHot = null)
        {
            if (WI == null)
            {
                InitWiWo();
            }
            oneHot = oneHot ?? (IsCbow ? ToVector(inputs.ContextWords) : ToVector(inputs.TargetWord));

            var output = oneHot.DotProduct(WI).DotProduct(WO).GetSoftmax();

            return output;
        }

        public virtual void Backpropagate(double[,] oneHot, double[,] actualOutput, double[,] expectedOutput)
        {
            var errors = actualOutput.Minus(expectedOutput);
            
            //this is equal to hidden neurons' value
            var hiddenValue = oneHot.DotProduct(WI);

            //this is equal to hidden neurons' net_input_gradient
            var netInputGradient = WO.DotProduct(errors.Transpose());

            //this is equal to the output vectors' gradient value
            var outputValueGradient = errors.Transpose().DotProduct(hiddenValue);

            //this is equal to the input vectors' gradient value
            var inputValueGradient = netInputGradient.DotProduct(oneHot);

            WO = WO.Minus(outputValueGradient.DotScalar(LearningRate).Transpose());
            WI = WI.Minus(inputValueGradient.DotScalar(LearningRate).Transpose());
        }
    }

    public static class Word2VecExtensions
    {
        public static double[,] LossFunction(this double[,] o)
        {
            var pr = o.GetSoftmax();
            pr.ApplyToEach(v => -1 * System.Math.Log(v));
            return pr;
        }

        /// <summary>
        /// Partial derivative chain rule result for L2 error function
        /// </summary>
        /// <param name="y">the estimate</param>
        /// <param name="t">the actual</param>
        /// <param name="x">input vector</param>
        /// <param name="w">input weights</param>
        public static double[] UpdateWeights(double y, double t, double[] x, double[] w)
        {
            for (var i = 0; i < w.Length; i++)
            {
                w[i] = (y - t) * y * (1 - y) * x[i];
            }

            return w;
        }

        public static double GetSigmaSum(double[,] x, double[,] w)
        {
            return GetSigmaSum(x.Flatten(), w.Flatten());
        }

        public static double GetSigmaSum(double[] x, double[] w)
        {
            var u = 0D;
            for (var i = 0; i < x.Length; i++)
            {
                u += x[i] * w[i];
            }
            return u;
        }

        public static double SigmoidFunction(double u)
        {
            return 1 / (1 + System.Math.Pow(System.Math.E, u * -1));
        }

        public static double[,] CalcLossFunctionL2(double[,] t, double[,] y)
        {
            var e = t.Minus(y);
            e = e.DotProduct(e);
            e.ApplyToEach(d => 0.5D * d);
            return e;
        }

        public static double[] CalcLossFunctionL2(double[] t, double[] y)
        {
            return CalcLossFunctionL2(t.ToMatrix(1), y.ToMatrix(1)).Flatten();
        }

        public static double CalcLossFunctionL2(double t, double y)
        {
            return 0.5 * System.Math.Pow(t - y, 2);
        }

        /// <summary>
        /// Python Natural Language Processing by Jalaj Thanaki
        /// fig. 6.26
        /// </summary>
        /// <param name="w"></param>
        /// <param name="wTick"></param>
        /// <param name="x"></param>
        /// <param name="N">Number of nodes in hidden layer</param>
        /// <returns></returns>
        public static Word2VecOutputCalc Word2VecMultiLayerNeuralNetwork(double[,] w, double[,] wTick, double[] x, int N)
        {
            var K = x.Length;
            var u = new double[w.GetLength(1)];
            var h = new double[w.GetLength(1)];
            var uTick = new double[wTick.GetLength(1)];
            var y = new double[wTick.GetLength(1)];

            for (var i = 0; i < w.GetLength(1); i++)
            {
                u[i] = Enumerable.Range(0, K).Sum(k => w[k, i] * x[k]);
                h[i] = SigmoidFunction(u[i]);
            }

            for (var j = 0; j < wTick.GetLength(1); j++)
            {
                uTick[j] = Enumerable.Range(0, N).Sum(i => wTick[i, j] * h[i]);
                y[j] = SigmoidFunction(uTick[j]);
            }

            var output = new Word2VecOutputCalc
            {
                U = u,
                H = h,
                Utick = uTick,
                Y = y
            };

            return output;
        }
    }

    public class Word2VecInputs
    {
        public HuffmanNode TargetWord { get; set; }
        public List<HuffmanNode> ContextWords { get; set; }
    }

    public class Word2VecOutputCalc
    {
        public double[] U { get; set; }
        public double[] H { get; set; }
        public double[] Utick { get; set; }
        public double[] Y { get; set; }
    }
}
