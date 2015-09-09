using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomeSecondDll;
using SomethingShared;

namespace AdventureWorks2012.VeryBadCode
{
    public class CallAndCallVirts
    {
        public readonly SomeSecondDll.MyFirstMiddleClass _mfmc = new MyFirstMiddleClass();
        public readonly SomeSecondDll.SecondMiddleClass _smc = new SecondMiddleClass();
        public string SomeProperty { get; set; }
        public SomethingShared.ArgsObject MyArgs { get; set; }
        private System.Random _myRand = new Random(Convert.ToInt32(DateTime.Now));
        public int SomeId { get; set; }
        public CallAndCallVirts()
        {
            SomeProperty = _mfmc.DoNothingMethod();
            MyArgs = new ArgsObject()
            {
                Property00 = SomethingShared.Globals.SomeGlobal,
                Property02 = SomethingShared.Globals.AnotherGlobal,
                Property03 = false,
                Property04 = 138
            };
        }

        public void TopLevelMethod()
        {
            var localEntity = _mfmc.GetMyEntity(MyArgs);
            localEntity.Id = _myRand.Next(1, 999);
            if (localEntity.MyValidationMethod())
            {
                SomeId = _smc.SaveSomething(localEntity);
            }
        }

        public void RecursiveCallToSelf(string junk, ref int aCounter)
        {
            while (aCounter < 32)
            {
                junk = junk ?? string.Empty;
                junk = string.Format("{0}{1}", junk, _myRand.Next(1, 999));
                aCounter += 1;
                RecursiveCallToSelf(junk, ref aCounter);
            }
        }

        public int RecursiveCallToOthers()
        {
            var someCounter = 0;
            _mfmc.RecurseCall(ref someCounter);
            return someCounter;
        }
    }
}
