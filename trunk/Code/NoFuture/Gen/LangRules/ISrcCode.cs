using System;

namespace NoFuture.Gen.LangRules
{
    public interface ISrcCode
    {
        Tuple<int, int> GetMyStartEnclosure(string[] srcFile, bool isClean = false);
        Tuple<int, int> GetMyEndEnclosure(string[] srcFile, bool isClean = false);
        void SetMyStartEnclosure(Tuple<int, int> start);
        void SetMyEndEnclosure(Tuple<int, int> end);
    }
}
