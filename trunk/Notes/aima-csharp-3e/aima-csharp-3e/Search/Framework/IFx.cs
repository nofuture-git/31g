using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aima_csharp_3e.Search.Framework
{
    public interface IFx<T>
    {
        /// <summary>
        /// Will return '0' when when <see cref="T"/> resolves to the Goal
        /// </summary>
        /// <returns></returns>
        Func<T, double> Eval { get; }
    }
}
