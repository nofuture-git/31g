using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaSdkTestOverloadedMethods
{
    public class CtorAndPdb
    {
        private string firstField;

        public const string MY_CONST = "myConstValue";

        public int secondField;
        public int thirdField = 12;

        private readonly object firstPrivateField = new object();

        public CtorAndPdb()
        {
            secondField = thirdField - 2;
        }

        public CtorAndPdb(string ctorArg)
        {
            firstField = ctorArg;
        }
    }
}
