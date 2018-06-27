using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Diagnostics.Debug;

namespace NoFuture.Util.Core.Algo
{
    /// <summary>
    /// See [https://www2.cs.duke.edu/csed/poop/huff/info/], this was coded from it.
    /// </summary>
    public class HuffmanEncoding
    {
        private static readonly HuffmanNodeCountComparer countComparer = new HuffmanNodeCountComparer();
        private List<HuffmanNode> _nodes;
        private int _totalSum;
        private HuffmanNode _rootNode;
        private List<HuffmanNode> _myLeafs;
        public HuffmanNode RootNode => _rootNode;

        public HuffmanEncoding(Dictionary<string, int> names2Counts)
        {
            if (names2Counts == null || !names2Counts.Any())
            {
                throw new ArgumentNullException(nameof(names2Counts));
            }

            var nodes = new List<HuffmanNode>();
            var counter = 0;
            foreach (var d in names2Counts.Keys)
            {
                nodes.Add(new HuffmanNode(d, names2Counts[d], counter));
                counter += 1;
            }

            _nodes = nodes;
            _totalSum = _nodes.Sum(t => t.Count);
            BuildTree();
            PushEncoding();
        }

        internal void BuildTree()
        {
            WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fffff} {nameof(HuffmanEncoding)} Start BuildTree");
            HuffmanNode nextNode;
            do
            {
                nextNode = GetNextTreeNode();
                _nodes.Remove(nextNode.Left);
                _nodes.Remove(nextNode.Right);
                _nodes.Add(nextNode);

            } while (nextNode.Count < _totalSum);

            _rootNode = nextNode;
            WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fffff} {nameof(HuffmanEncoding)} End BuildTree");
        }

        internal void PushEncoding()
        {
            WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fffff} {nameof(HuffmanEncoding)} Start PushEncoding");
            RootNode.PushEncoding(null);
            WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fffff} {nameof(HuffmanEncoding)} End PushEncoding");
        }

        public HuffmanNode GetNodeByWord(string word)
        {
            return GetLeafs().FirstOrDefault(l => l.Word == word);
        }

        public HuffmanNode GetNodeByPath(BitArray bitArray)
        {
            if (bitArray == null || bitArray.Count <= 0)
                return _rootNode;
            var node = _rootNode;
            for (var i = 0; i < bitArray.Length; i++)
            {
                var nextStep = bitArray.Get(i);
                if (node.IsLeaf)
                    return node;
                node = nextStep ? node.Right : node.Left;
            }

            return node;
        }

        public HuffmanNode GetNodeByIndex(int index)
        {
            return GetLeafs().FirstOrDefault(n => n.Index == index);
        }

        public List<HuffmanNode> GetLeafs()
        {
            if (_myLeafs != null)
                return _myLeafs;
            _myLeafs = GetLeafs(RootNode, new List<HuffmanNode>());
            return _myLeafs;
        }

        public int GetLengthLeafs()
        {
            return GetLeafs().Count;
        }

        internal static List<HuffmanNode> GetLeafs(HuffmanNode n, List<HuffmanNode> nodes)
        {
            if (n == null)
                return nodes;
            if (n.IsLeaf)
            {
                nodes.Add(n);
                return nodes;
            }

            GetLeafs(n.Left, nodes);
            GetLeafs(n.Right, nodes);
            return nodes;
        }

        internal Tuple<HuffmanNode, HuffmanNode> GetNextLeftRight()
        {
            _nodes.Sort(countComparer);
            var firstTwo = _nodes.Take(2).ToList();
            var left = firstTwo.Any() ? firstTwo[0] : null;
            var right = firstTwo.Count >= 2 ? firstTwo[1] : null;

            if (left == null || right == null)
            {
                throw new ArgumentNullException("could not get the left or right node");
            }

            return new Tuple<HuffmanNode, HuffmanNode>(left, right);
        }

        internal HuffmanNode GetNextTreeNode()
        {
            var leftRightTuple = GetNextLeftRight();
            var left = leftRightTuple.Item1;
            var right = leftRightTuple.Item2;
            var word = "";
            var count = left.Count + right.Count;
            var tree = new HuffmanNode(word, count) {Left = left, Right = right};
            return tree;
        }
    }

    /// <summary>
    /// Represents a single node in the binary Huffman tree
    /// </summary>
    public class HuffmanNode
    {
        private bool[] _encoding;
        private readonly Dictionary<string, double> _values = new Dictionary<string, double>() ;
        public HuffmanNode(string word, int count)
        {
            Word = word ?? "";
            Count = count;
            Index = -1;
        }

        public HuffmanNode(string word, int count, int index) : this(word, count)
        {
            Index = index;
        }

        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }
        public string Word { get; }
        public int Count { get; }
        public int Index { get; protected internal set; }
        public bool IsLeaf => Left == null && Right == null;

        public BitArray Encoding
        {
            get
            {
                _encoding = _encoding ?? new bool[] {} ;
                return new BitArray(_encoding);
            }
        }

        public double GetValue(string name)
        {
            if (!_values.ContainsKey(name))
                return 0D;
            return _values[name];
        }

        public void SetValue(string name, double value)
        {
            if (_values.ContainsKey(name))
            {
                _values[name] = value;
            }
            else
            {
                _values.Add(name, value);
            }
        }

        public string GetEncodingString()
        {
            _encoding = _encoding ?? new bool[] { };
            var str = new StringBuilder();
            foreach (var b in _encoding)
            {
                str.Append(Convert.ToByte(b));
            }

            return str.ToString();
        }

        /// <summary>
        /// The subsampling randomly discards frequent words while keeping the ranking same
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="trainWords"></param>
        /// <returns></returns>
        internal double GetSampleValue(double sample, int trainWords)
        {
            //see word2vec.c at lin 427
            return (System.Math.Sqrt(Count / (sample * trainWords)) + 1) * (sample * trainWords) / Count;
        }

        public void PushEncoding(bool[] encoding)
        {
            encoding = encoding ?? new bool[] { };
            _encoding = encoding;
            if (IsLeaf)
            {
                return;
            }

            PushEncoding(encoding, Left, false);
            PushEncoding(encoding, Right, true);
        }

        private void PushEncoding(bool[] encoding, HuffmanNode node, bool next)
        {
            var e = new bool[encoding.Length + 1];
            Array.Copy(encoding, e, encoding.Length);
            e[encoding.Length] = next;
            node.PushEncoding(e);
        }

    }

    public class HuffmanNodeCountComparer : IComparer<HuffmanNode>
    {
        /// <summary>
        /// Less than zero     x is less than y.
        /// Zero               x equals y.
        /// Greater than zero  x is greater than y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(HuffmanNode x, HuffmanNode y)
        {
            return (x?.Count ?? 0) - (y?.Count ?? 0);
        }
    }

    public class HuffmanNodeIndexComparer : IComparer<HuffmanNode>
    {
        /// <summary>
        /// Less than zero     x is less than y.
        /// Zero               x equals y.
        /// Greater than zero  x is greater than y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(HuffmanNode x, HuffmanNode y)
        {
            return (x?.Index ?? 0) - (y?.Index ?? 0);
        }
    }
}
