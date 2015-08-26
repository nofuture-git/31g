﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Sql
{
    [TestClass]
    public class TestPsMetadata
    {
        [TestMethod]
        public void TestLongestColumnNameLen()
        {
            const string Longest = "mmmmmmmmmmmmmmmmmmmmmmmmm";
            var testSubject = new NoFuture.Sql.Mssql.Md.PsMetadata();
            testSubject.AutoNumKeys.AddRange(new List<string>{"fffff","ooooooooooooo"});
            testSubject.FkKeys.Add("wwwwwwww","1");
            testSubject.FkKeys.Add("uuuuuuuuuuuuuuuuuuu","12");
            testSubject.IsComputedKeys.AddRange(new List<string>{"iiiiiiiiii"});
            testSubject.PkKeys.Add("jjjjjjjjjj", "1");
            testSubject.PkKeys.Add(Longest, "12");
            testSubject.TickKeys.AddRange(new List<string>{"xxxxxxxxxxxxx", "aaaaaaaaaaaaaa", "yy"});

            var testResult = testSubject.LongestColumnNameLen;
            Assert.AreEqual(Longest.Length, testResult);
        }
    }
}
