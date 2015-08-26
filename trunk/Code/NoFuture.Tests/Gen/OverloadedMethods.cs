using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaSdkTestOverloadedMethods
{
    public class OverloadedMethods
    {
        private string _someVariable;
        private string _anotherVariable;


        public string OvMethod()
        {
            _someVariable = OvMethod(4, "new", 1);
            _anotherVariable = string.Format("{0}..{1}", _someVariable, OvMethod(3));
            return _anotherVariable;
        }

        public string OvMethod(int s)
        {
            var myLocalStrBldr = new StringBuilder();
            for (var i = 0; i < s; i++)
                myLocalStrBldr.Append("--");

            return myLocalStrBldr.ToString();
        }

        public string OvMethod(int thisIsAnotherParam, string b)
        {
            var anotherLocal = new StringBuilder();
            for (var i = thisIsAnotherParam; i > 0; i--)
            {
                anotherLocal.Append("++");
            }
            anotherLocal.Append(b);
            return anotherLocal.ToString();
        }

        public string OvMethod(int notTheSameParam, string aStrangeName, int c)
        {
            var thirdLocal = new StringBuilder();
            thirdLocal.Append(OvMethod(notTheSameParam));
            thirdLocal.Append(OvMethod(notTheSameParam, aStrangeName));
            for (var k = 0; k < c; k++)
                thirdLocal.Append("==");
            return thirdLocal.ToString();
        }
    }
}
