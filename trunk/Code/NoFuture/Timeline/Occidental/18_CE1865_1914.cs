using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Timeline
{
    public partial class Occidental
    {
        public Plate CE1865to1914()
        {
            var rule = new Rule { StartValue = 1865, EndValue = 1914, RuleLineSpacing = 7 };

            var plate = new Plate { Ruler = rule, Name = "New Imperialism" };

            //usa.AddEntry(1817, "[James Monroe -1825]", PrintLocation.Left);
            //usa.AddEntry(1816, "+IN(1816)", PrintLocation.Right);

            //usa.AddEntry(1816, "First Seminole War(1816-1819)");
            throw new NotImplementedException();
        }
    }
}
