using System;

namespace NoFuture.Gen.LangRules
{
    public interface ISrcCode
    {
        Tuple<int, int> GetMyStartEnclosure(string[] srcFile = null, bool isClean = false);
        Tuple<int, int> GetMyEndEnclosure(string[] srcFile = null, bool isClean = false);
        void SetMyStartEnclosure(Tuple<int, int> start);
        void SetMyEndEnclosure(Tuple<int, int> end);
    }
}
