using System;

namespace NoFuture.Console
{
    public class MyPoco
    {
        public int MyInt { get; set; }
        public DateTime MyDate { get; set; }
        public String MyString { get; set; }
        public MyChildPoco MyProperty { get; set; }
    }
}
