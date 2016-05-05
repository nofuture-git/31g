using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoFuture.Util.Gia
{
    public class FlattenAssembly
    {
        public List<FlattenedLine> AllLines { get; set; }
        public System.Reflection.AssemblyName AssemblyName { get; set; }
        public string Path { get; set; }
    }
}
