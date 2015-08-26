using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Util.Pos
{
    [TestClass]
    public class StanfordTaggerTests
    {
        [TestMethod]
        public void TestApi()
        {
            var modelPath =
                @"C:\Projects\31g\trunk\bin\stanford-postagger-2015-04-20\models\english-bidirectional-distsim.tagger";
            var tagger = new edu.stanford.nlp.tagger.maxent.MaxentTagger(modelPath);

            var testResult = tagger.tagString("Here's a tagged string.");
            System.Diagnostics.Debug.WriteLine(testResult);

        }
    }
}
