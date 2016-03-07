using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aima_csharp_3e.Agent;

namespace aima_csharp_3e.Search.Framework
{
    public static class Extensions
    {
        public static void Shuffle(this IList<IState> someList)
        {
            var r = new Random();
            for (var i = someList.Count; i > 0; i--)
            {
                //pick one at random
                var j = r.Next(i);
                
                //pull a local on who is there currently
                var k = someList[j];

                //reassign thier position
                someList[j] = someList[i - 1];

                //put them back in over
                someList[i - 1] = k;

            }
        }
    }

}
