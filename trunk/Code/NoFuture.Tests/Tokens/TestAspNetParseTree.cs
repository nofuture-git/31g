using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Tokens
{
    [TestClass]
    public class TestAspNetParseTree
    {
        [TestMethod]
        public void TestPoc()
        {
            var testFile = @"C:\Projects\APEX\SummitHealth.CRM\SummitHealth.CRM.UI.Design\Admin\AccountEdit.aspx";

            var testResult = NoFuture.Tokens.AspNetParseTree.InvokeParse(testFile);

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.DistinctTags);
            Assert.AreNotEqual(0,testResult.DistinctTags.Keys.Count);
            foreach (var key in testResult.DistinctTags.Keys)
            {
                //var attrs = string.Join("|", testResult.DistinctTags[key]);
                System.Diagnostics.Debug.WriteLine(key);
            }

            Assert.IsNotNull(testResult.ScriptBodies);
            Assert.AreNotEqual(0,testResult.ScriptBodies.Count);
            foreach (var script in testResult.ScriptBodies)
            {
                System.Diagnostics.Debug.WriteLine(script);
            }
        }
    }
}
