using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aima_csharp_3e.Learning
{
    public interface ILearner<T>
    {
        void Train(IDataSet<T> ds);
        T Predict(IDecisionTree<T> example);
    }

    public class DecisionTreeLearner : ILearner<ValueType>
    {
        private DecisionTreeValueType _tree;
        private ValueType _dfVal;
        private IDataSet<ValueType> _ds;
        private IEnumerable<ValueType> _attrNames;

        public DecisionTreeLearner(IDecisionTree<ValueType> tree, ValueType dfValue)
        {
            _tree = (DecisionTreeValueType)tree;
            _dfVal = dfValue;
        }


        public void Train(IDataSet<ValueType> ds)
        {
            throw new NotImplementedException();
        }

        public ValueType Predict(IDecisionTree<ValueType> example)
        {
            throw new NotImplementedException();
        }

        protected virtual IDecisionTree<ValueType> DecisionTreeLearning(IEnumerable<DecisionTreeValueType> examples,
            IEnumerable<ValueType> attributes, IEnumerable<DecisionTreeValueType> parentExamples = null)
        {
            var egs = examples.ToList();
            var attrs = attributes.ToList();
            if(egs.Count <= 0)
                return PluralityValue(parentExamples);
            if (egs.All(x => x.Classification == BooleanClassification.Negative))
            {
                return new DecisionTreeValueType {Classification = BooleanClassification.Negative};
            }
            if (egs.All(x => x.Classification == BooleanClassification.Positive))
            {
                return new DecisionTreeValueType {Classification = BooleanClassification.Positive};
            }
            if (attrs.Count <= 0)
            {
                return PluralityValue(egs);
            }

            var attrScores = attrs.Select(x => new Tuple<double, ValueType>(Importance(x, egs), x)).ToList();
            var argMaxA = attrScores.Max(x => x.Item1);
            var A = attrScores.First(x => x.Item1 == argMaxA).Item2;
            

            throw new NotImplementedException();
        }

        protected virtual IDecisionTree<ValueType> PluralityValue(IEnumerable<DecisionTreeValueType> df)
        {
            throw new NotImplementedException();
        }

        protected double Importance(ValueType a, IEnumerable<DecisionTreeValueType> examples)
        {
            throw new NotImplementedException();
        }
    }
}
