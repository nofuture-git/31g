using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;

namespace NoFuture.Tests.Rand.DomusTests
{
    [TestClass]
    public class TradeLineTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var testSubject = new NoFuture.Rand.Domus.Sp.TradeLine(DateTime.Now.AddDays(-370));
            var testBalance = testSubject.Balance;

            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-360), new Pecuniam(-450.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-120), new Pecuniam(-458.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-150), new Pecuniam(-457.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-240), new Pecuniam(-454.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-300), new Pecuniam(-452.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-270), new Pecuniam(-453.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-180), new Pecuniam(-456.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-210), new Pecuniam(-455.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-330), new Pecuniam(-451.0M)));

            //charges
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-365), new Pecuniam(8000.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-350), new Pecuniam(164.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-198), new Pecuniam(165.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-24), new Pecuniam(166.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-74), new Pecuniam(167.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-88), new Pecuniam(168.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-92), new Pecuniam(169.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-121), new Pecuniam(170.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-180), new Pecuniam(171.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-142), new Pecuniam(172.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-155), new Pecuniam(173.4M)));

            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-30), new Pecuniam(-461.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-90), new Pecuniam(-459.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-60), new Pecuniam(-460.0M)));
        
        }
    }
}
