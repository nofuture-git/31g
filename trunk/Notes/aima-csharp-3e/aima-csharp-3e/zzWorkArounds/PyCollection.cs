using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using aima_csharp_3e.Search.Framework;

namespace aima_csharp_3e.zzWorkArounds
{
    public abstract class PyCollection<T> : IEnumerable<T>, ICollection
    {
        public abstract T Pop();
        public abstract void Extend(IEnumerable<T> more);
        public abstract void Append(T m);

        #region iterface
        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract void CopyTo(Array array, int index);

        public abstract int Count { get; }
        public abstract object SyncRoot { get;  }
        public abstract bool IsSynchronized { get; }
        #endregion
    }

    public class PyStack<T> : PyCollection<T>
    {
        private readonly Stack<T> _myCollection = new Stack<T>();

        public override T Pop()
        {
            return _myCollection.Pop();
        }

        public override void Extend(IEnumerable<T> more)
        {
            foreach (var fd in more)
            {
                _myCollection.Push(fd);
            }
        }

        public override void Append(T m)
        {
            _myCollection.Push(m);
        }

        #region overrides
        public override IEnumerator<T> GetEnumerator()
        {
            return _myCollection.GetEnumerator();
        }

        public override void CopyTo(Array array, int index)
        {
            _myCollection.CopyTo(array.Cast<T>().ToArray(), index);
        }

        public override int Count
        {
            get { return _myCollection.Count; }
        }

        public override object SyncRoot
        {
            get { return ((ICollection) _myCollection).SyncRoot; }
        }

        public override bool IsSynchronized
        {
            get { return ((ICollection) _myCollection).IsSynchronized; }
        }
        #endregion
    }
    public class PyQueue<T> : PyCollection<T>
    {
        private readonly Queue<T> _myCollection = new Queue<T>();

        public override T Pop()
        {
            return _myCollection.Dequeue();
        }

        public override void Extend(IEnumerable<T> more)
        {
            foreach (var fd in more)
            {
                _myCollection.Enqueue(fd);
            }
        }

        public override void Append(T m)
        {
            _myCollection.Enqueue(m);
        }

        #region overrides
        public override IEnumerator<T> GetEnumerator()
        {
            return _myCollection.GetEnumerator();
        }

        public override void CopyTo(Array array, int index)
        {
            _myCollection.CopyTo(array.Cast<T>().ToArray(), index);
        }

        public override int Count
        {
            get { return _myCollection.Count; }
        }

        public override object SyncRoot
        {
            get { return ((ICollection)_myCollection).SyncRoot; }
        }

        public override bool IsSynchronized
        {
            get { return ((ICollection)_myCollection).IsSynchronized; }
        }
        #endregion
    }

    public class PyPriorityQueue<T> : PyCollection<T>
    {
        private readonly List<T> _myCollection = new List<T>();
        private readonly IFx<T> _func;

        public PyPriorityQueue(IFx<T> fx)
        {
            _func = fx;
        }

        public override T Pop()
        {
            var sortedByHx = _myCollection.ToDictionary(x => _func.Eval(x)).OrderBy(x => x.Key);

            var bestByHx = sortedByHx.First().Value;

            var theRestByHx = sortedByHx.Skip(1).Take(sortedByHx.Count() - 1);

            _myCollection.Clear();
            _myCollection.AddRange(theRestByHx.Select(x => x.Value));

            return bestByHx;
        }

        public override void Extend(IEnumerable<T> more)
        {
            _myCollection.AddRange(more);
        }

        public override void Append(T m)
        {
            _myCollection.Add(m);
        }

        #region overrides

        public override IEnumerator<T> GetEnumerator()
        {
            return _myCollection.GetEnumerator();
        }

        public override void CopyTo(Array array, int index)
        {
            _myCollection.CopyTo(array.Cast<T>().ToArray(), index);
        }

        public override int Count
        {
            get { return _myCollection.Count; }
        }

        public override object SyncRoot
        {
            get { return ((ICollection)_myCollection).SyncRoot; }
        }

        public override bool IsSynchronized
        {
            get { return ((ICollection)_myCollection).IsSynchronized; }
        }
        #endregion
    }
}
