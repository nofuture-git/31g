﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Endo.Grps;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class DataTypesTests
    {
        [TestMethod]
        public void TestRandomSic()
        {
            var testResult = StandardIndustryClassification.RandomSic();
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Value);
        }

        [TestMethod]
        public void TestListData()
        {
            var testResult = Data.ListData.WebmailDomains;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void TestEnglishWords()
        {
            var testResult = Data.TreeData.EnglishWords;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0,testResult.Count);
            var testResultItem = testResult.FirstOrDefault(x => x.Item1 == "it");
            Assert.IsNotNull(testResultItem);

            Assert.AreEqual(1386, testResultItem.Item2);
        }
    }
}