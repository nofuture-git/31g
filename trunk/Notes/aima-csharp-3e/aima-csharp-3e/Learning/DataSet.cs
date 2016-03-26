using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aima_csharp_3e.Learning
{

    public interface IDataSet<T>
    {
        IEnumerable<IDecisionTree<T>> Examples { get; }
        void AddExample(IDecisionTree<T> example);
        int Size { get; }
        IDecisionTree<T> this[int key] { get; }
        IDataSet<T> RemoveExample(IDecisionTree<T> example);
        T Target { get; }
        IEnumerable<T> AttrNames { get; }

    }

    public class DataSetValueType : IDataSet<ValueType>
    {
        private readonly List<IDecisionTree<ValueType>> _examples;

        public DataSetValueType(IEnumerable<IDecisionTree<ValueType>> examples)
        {
            _examples = examples.ToList();
        }

        public IEnumerable<IDecisionTree<ValueType>> Examples { get {return _examples;} }
        public void AddExample(IDecisionTree<ValueType> example)
        {
            if (_examples.Any(x => x.Equals(example)))
                return;
            _examples.Add(example);
        }

        public int Size { get { return _examples.Count(); } }

        public IDecisionTree<ValueType> this[int key]
        {
            get { return _examples.ToArray()[key]; }
        }

        public IDataSet<ValueType> RemoveExample(IDecisionTree<ValueType> example)
        {
            //TODO why is this needed?
            throw new NotImplementedException();
        }

        public ValueType Target { get; }
        public IEnumerable<ValueType> AttrNames { get; }
    }
}
