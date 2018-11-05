using System;
using System.Linq;

namespace NoFuture.Examples
{
    public interface INfNode<T>
    {
        INfNode<T>[] Items { get; set; }
        int Count { get; }
        int CurrentIndex { get; }
        INfNode<T> NextItem();
        INfNode<T> PrevItem();
        int GetFullDepthCount();
        void IterateTree(Predicate<INfNode<T>> searchBy, Func<INfNode<T>, INfNode<T>> doSomething);
    }

    [Serializable]
    public abstract class NfNode<T> : INfNode<T>
    {
        [NonSerialized] private int _idx = 0;
        public virtual INfNode<T>[] Items { get; set; }
        public virtual int Count => Items?.Length ?? 0;
        public virtual int CurrentIndex => _idx;

        public virtual INfNode<T> NextItem()
        {
            if (_idx < Count)
            {
                var v = Items[_idx];
                _idx += 1;
                return v;
            }
            _idx = 0;
            return null;
        }

        public virtual INfNode<T> PrevItem()
        {
            _idx -= 1;
            if (_idx >= 0)
            {
                var v = Items[_idx];
                return v;
            }
            _idx = 0;
            return null;
        }

        public virtual int GetFullDepthCount()
        {
            var c = 1;
            if (Items == null || !Items.Any())
                return c;
            foreach (var i in Items)
                c += i.GetFullDepthCount();

            return c;
        }

        public virtual void IterateTree(Predicate<INfNode<T>> searchBy, Func<INfNode<T>, INfNode<T>> doSomething)
        {
            var callStack = new System.Collections.Generic.Stack<INfNode<T>>();
            //start at top
            var ivItem = (INfNode<T>)this;
            while (ivItem != null)
            {
                var nextItem = ivItem.NextItem();
                if (nextItem != null)
                {
                    //detect if some parent item is also a child item
                    if (ReferenceEquals(nextItem, ivItem) || callStack.Any(v => ReferenceEquals(v, nextItem)))
                    {
                        nextItem = null;
                    }
                    else
                    {
                        callStack.Push(ivItem);
                    }
                }

                ivItem = nextItem;

                if (ivItem == null)
                {
                    if (callStack.Count <= 0)
                        break;
                    ivItem = callStack.Pop();
                    continue;
                }
                //search for whatever by such-and-such
                if (!searchBy(ivItem))
                    continue;

                //having a match then do such-and-such
                var parent = callStack.Peek();
                var ivItemIdx = (parent?.CurrentIndex ?? 0) - 1;
                if (parent == null || ivItemIdx < 0 || ivItemIdx >= parent.Count)
                {
                    ivItem = doSomething(ivItem);
                    continue;
                }
                parent.Items[ivItemIdx] = doSomething(ivItem);
            }
        }
    }
}
