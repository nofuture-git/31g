using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Domain
{
    [TestClass]
    public class AspTappetTests
    {
        [TestMethod]
        public void TestGetFilePathSegment()
        {
            var teststring = "http://localhost:8080/MyAppdomain/Myresouces/MyFile.cshtml?q=6&value=Hello%20Uri";

            Uri testUri;

            Uri.TryCreate(teststring, UriKind.RelativeOrAbsolute, out testUri);

            Assert.IsNotNull(testUri);
            var testResult = NoFuture.Domain.AspTappet.GetFilePathSegment(testUri);
            Assert.AreEqual("/MyAppdomain/Myresouces/MyFile.cshtml", testResult);

            teststring =
                "http://localhost:8080/MyAppdomain/Myresouces/MyFile.cshtml/filePart/more.ssjk?q=6&value=Hello%20Uri";
            Uri.TryCreate(teststring, UriKind.RelativeOrAbsolute, out testUri);

            Assert.IsNotNull(testUri);
            testResult = NoFuture.Domain.AspTappet.GetFilePathSegment(testUri);
            Assert.AreEqual("/MyAppdomain/Myresouces/MyFile.cshtml", testResult);

        }

        [TestMethod]
        public void TestGetGlobalWebConfigAspExtensions()
        {
            var testResult = NoFuture.Domain.AspTappet.GetGlobalWebConfigAspExtensions();
            Assert.IsNotNull(testResult);
            foreach (var ext in testResult)
            {
                System.Diagnostics.Debug.WriteLine(ext);
            }
        }
    }
}
