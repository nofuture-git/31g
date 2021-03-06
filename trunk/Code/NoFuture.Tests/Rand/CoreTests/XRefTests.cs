﻿using System;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US.Fed;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.CoreTests
{
    [TestFixture]
    public class XRefTests
    {

        [Test]
        public void TestXrefOnTypes()
        {

            var testTarget = new Bank();
            testTarget.AddName(KindsOfNames.Legal, "JPMorgan Chase Bank, N.A.");
            testTarget.AddName(KindsOfNames.Abbrev, "JPMORGAN CHASE BK NA");
            testTarget.Rssd = new ResearchStatisticsSupervisionDiscount {Value = "852218" };


            Assert.IsNotNull(testTarget);

            //verify the properties have no value prior to test
            Assert.IsNull(testTarget.CIK);
            Assert.IsTrue(!testTarget.TickerSymbols.Any());

            testTarget.LoadXrefXmlData();

            Assert.IsNotNull(testTarget.CIK);
            Assert.AreNotEqual(0, testTarget.TickerSymbols);

            Console.WriteLine(testTarget.CIK.ToString());
            Console.WriteLine(testTarget.TickerSymbols.ToArray()[0].Symbol);
            Console.WriteLine(testTarget.TickerSymbols.ToArray()[0].Exchange);
            Console.WriteLine(testTarget.SIC.ToString());

            //test nothing found - no problems and no change
            testTarget = new Bank();
            testTarget.AddName(KindsOfNames.Legal, "Pacific Western Bank");
            testTarget.AddName(KindsOfNames.Abbrev, "PACIFIC WESTERN BK");
            testTarget.Rssd = new ResearchStatisticsSupervisionDiscount { Value = "494261" };

            Assert.IsNotNull(testTarget);
            Assert.IsNull(testTarget.CIK);
            Assert.IsTrue(!testTarget.TickerSymbols.Any());

            testTarget.LoadXrefXmlData();

            Assert.IsNull(testTarget.CIK);
            Assert.IsTrue(!testTarget.TickerSymbols.Any());
        }
        [Test]
        public void TestAddXrefValues()
        {
            var testXrefId = new Tuple<Type, string>(typeof(NoFuture.Rand.Com.Bank), "BANK OF AMER NA");
            var testValues = new RoutingTransitNumber {Value = "000015421"};
            var testResult = XRefGroup.AddXrefValues(testXrefId,testValues, "RoutingNumber");
            Assert.IsTrue(testResult);

            testXrefId = new Tuple<Type, string>(typeof(NoFuture.Rand.Com.Bank), "A new bank entry");
            testValues = new RoutingTransitNumber { Value = "787454541" };
            testResult = XRefGroup.AddXrefValues(testXrefId, testValues, "RoutingNumber");
            Assert.IsTrue(testResult);

        }
    }
}
