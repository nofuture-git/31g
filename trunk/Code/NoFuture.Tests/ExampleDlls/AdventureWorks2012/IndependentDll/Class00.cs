using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IndependentDll
{
    public class Class00
    {
        private Class01 _firstSubClass;

        public Class00()
        {
            _firstSubClass = new Class01();
        }

        public string CallClass01()
        {
            return _firstSubClass.CallClass02();
        }
    }
}
