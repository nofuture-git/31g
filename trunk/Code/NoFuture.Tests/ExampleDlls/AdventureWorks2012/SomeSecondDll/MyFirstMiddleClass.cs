using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThirdDll;

namespace SomeSecondDll
{
    public class MyFirstMiddleClass
    {
        private ThirdDll.DbLike _myDbLikeThing;
        private SecondMiddleClass _forRecursiveCalls;
        public MyFirstMiddleClass()
        {
            _myDbLikeThing = new DbLike();
            _forRecursiveCalls = new SecondMiddleClass();
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

        public void RecurseCall(ref int someIntIn)
        {
            someIntIn += 1;
            if (someIntIn > 99)
                return;
            _forRecursiveCalls.ForRecursiveCalls(ref someIntIn);
        }
    }
}
