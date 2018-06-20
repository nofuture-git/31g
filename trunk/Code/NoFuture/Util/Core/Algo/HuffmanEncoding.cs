using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Core.Algo
{
    /// <summary>
    /// See [https://www2.cs.duke.edu/csed/poop/huff/info/], this was coded from it.
    /// </summary>
    public class HuffmanEncoding
    {
        private static readonly NodeComparer _comparer = new NodeComparer();
        private List<HuffmanNode> _nodes;
        private int _totalCount;
        private HuffmanNode _rootNode;

        public HuffmanNode RootNode => _rootNode;

        public HuffmanEncoding(List<HuffmanNode> leafs)
        {
            if (leafs == null || !leafs.Any())
            {
                throw new ArgumentNullException(nameof(leafs));
            }

            _nodes = leafs;
            _totalCount = _nodes.Sum(t => t.Count);
        }

        public HuffmanEncoding(Dictionary<string, int> names2Counts)
        {
            if (names2Counts == null || !names2Counts.Any())
            {
                throw new ArgumentNullException(nameof(names2Counts));
            }

            var nodes = new List<HuffmanNode>();
            foreach (var d in names2Counts.Keys)
            {
                nodes.Add(new HuffmanNode(d, names2Counts[d]));
            }

            _nodes = nodes;
            _totalCount = _nodes.Sum(t => t.Count);
        }

        public void BuildTree()
        {
            HuffmanNode nextNode;
            do
            {
                nextNode = GetNextTreeNode();
                _nodes.Remove(nextNode.Left);
                _nodes.Remove(nextNode.Right);
                _nodes.Add(nextNode);

            } while (nextNode.Count < _totalCount);

            _rootNode = nextNode;
        }

        public void PushEncoding()
        {
            RootNode.PushEncoding(null);
        }

        public List<HuffmanNode> GetLeafs()
        {
            return GetLeafs(RootNode, new List<HuffmanNode>());
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
            _nodes.Sort(_comparer);
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
        public HuffmanNode(string word, int count)
        {
            Word = word ?? "";
            Count = count;
        }

        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }
        public string Word { get; }
        public int Count { get; }
        public bool IsLeaf => Left == null && Right == null;

        public BitArray Encoding
        {
            get
            {
                _encoding = _encoding ?? new bool[] {} ;
                return new BitArray(_encoding);
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

    public class NodeComparer : IComparer<HuffmanNode>
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
}
