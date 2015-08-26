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
    }
}
