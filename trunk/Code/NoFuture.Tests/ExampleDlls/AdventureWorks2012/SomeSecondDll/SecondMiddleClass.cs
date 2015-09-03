using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdDll;

namespace SomeSecondDll
{
    public class SecondMiddleClass
    {
        private readonly ThirdDll.DbLike _myDbLikeThing;

        public SecondMiddleClass()
        {
            _myDbLikeThing = new DbLike();
        }

        public string GetEntityData(SomethingShared.ArgsObject args)
        {
            return _myDbLikeThing.GetEntityData(args);
        }

        public int SaveSomething(SomethingShared.Entity00 entity00)
        {
            return _myDbLikeThing.SaveEntityData(entity00);
        }
    }
}
