using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomethingShared
{
    public class Globals
    {
        public const string SomeGlobal = "I AM GLOBAL";

        public const long AnotherGlobal = 145214L;

        public static int SomeStaticMethod(Entity00 e)
        {
            e.Id = e.Id + 1;
            e.LastName = SomeGlobal;

            return e.Id;
        }
    }

    public class ArgsObject
    {
        public string Property00 { get; set; }
        public string Property01 { get; set; }
        public long Property02 { get; set; }
        public bool Property03 { get; set; }
        public int Property04 { get; set; }
    }

    public class Entity00
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }

        public bool MyValidationMethod()
        {
            return Id > 0;
        }
    }
}
