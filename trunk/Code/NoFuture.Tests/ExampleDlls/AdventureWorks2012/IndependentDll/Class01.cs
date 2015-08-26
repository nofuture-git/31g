using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndependentDll
{
    public class Class01
    {
        private Class02 _secondClass;

        public Class01()
        {
            _secondClass = new Class02();
        }

        public string CallClass02()
        {
            return _secondClass.BottomFrameFx();
        }
    }
}
