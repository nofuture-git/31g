using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdDll;

namespace SomeSecondDll
{
    public class MyFirstMiddleClass
    {
        private ThirdDll.DbLike _myDbLikeThing;
        public MyFirstMiddleClass()
        {
            _myDbLikeThing = new DbLike();
        }

        private string _myField;
        private int _myInt;
        public string DoNothingMethod()
        {
            return string.Format("{0:0000}", _myInt);
        }

        public SomethingShared.Entity00 GetMyEntity(SomethingShared.ArgsObject args)
        {
            return _myDbLikeThing.GetEntityByArgs(args);
        }
    }
}
