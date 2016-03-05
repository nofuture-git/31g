using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.zzWorkArounds;

namespace aima_csharp_3e.Search.Framework
{
    public interface ISearch<T>
    {
        Node Search();
    }
}
