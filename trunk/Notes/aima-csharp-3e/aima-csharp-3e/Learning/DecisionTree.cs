﻿using System;
using System.Collections.Generic;

namespace aima_csharp_3e.Learning
{
    public interface IDecisionTree<T>
    {
        IPathToLeaf<T> Goal { get; }
        T Decide(IEnumerable<T> inputVector);
    }

    public interface IAttr<out T>
    {
        T Value { get; }
        string Name { get; }
    }

    public interface IAttrValueTest<out T>
    {
        IAttr<T> Attr { get; }
        bool IsPass();
    }

    public enum BooleanClassification
    {
        Negative,
        Positive
    }

    public interface IPathToLeaf<T>
    {
        BooleanClassification Classification { get; set; }
        IEnumerable<IAttrValueTest<T>> ConjuntionOfAttrValueTests { get; set; }
    }

    public interface IDiscreteAttr : IAttr<ValueType> { }

    public interface IContinuous<out T> : IAttr<T> { }

    public abstract class ExactlyTwoPossible: IDiscreteAttr
    {
        protected ExactlyTwoPossible(bool value, string name)
        {
            Value = value;
            Name = name;
        }

        public ValueType Value { get; }
        public string Name { get; }
    }

    public class AttrValueTypeTest : IAttrValueTest<ValueType>
    {
        private readonly Func<IAttr<ValueType>, bool> _evalFunc;
        public AttrValueTypeTest(IAttr<ValueType> attr, Func<IAttr<ValueType>, bool> evalFunc)
        {
            Attr = attr;
            _evalFunc = evalFunc;
        }
        public IAttr<ValueType> Attr { get; }
        bool IAttrValueTest<ValueType>.IsPass()
        {
            return _evalFunc(Attr);
        }
    }

    public class PathToLeafValueType : IPathToLeaf<ValueType>
    {
        public BooleanClassification Classification { get; set; }
        public IEnumerable<IAttrValueTest<ValueType>> ConjuntionOfAttrValueTests { get; set; }
    }

    public abstract class BooleanDecisionTree : IDecisionTree<IDiscreteAttr>
    {
        public abstract IPathToLeaf<IDiscreteAttr> Goal { get; }

        public abstract IDiscreteAttr Decide(IEnumerable<IDiscreteAttr> inputVector);
    }
}