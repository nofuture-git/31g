using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class LedgerTests
    {
        [Test]
        public void TestAdd()
        {
            var dt = DateTime.Today;
            var testSubject = new Ledger();
            var testName00 = "Assets";
            var testResults00 = testSubject.Add(testName00, KindsOfAccounts.Asset, false, 101, dt.AddDays(-12));
            Assert.IsNotNull(testResults00);
            Assert.AreEqual(testName00, testResults00.Id.Value);
            Assert.AreEqual("101", testResults00.Id.Abbrev);

            var testResults01 = testSubject.Add(testName00, KindsOfAccounts.Asset, false);
            Assert.IsNotNull(testResults01);
            Assert.IsTrue(testResults00.Equals(testResults01));

            var testInput = new Account(new AccountId("Cash"), dt, KindsOfAccounts.Asset, false);
            var testResult02 = testSubject.Add(testInput);
            Assert.IsNotNull(testResult02);
            Assert.IsTrue(testInput.Equals(testResult02));

        }

        [Test]
        public void TestGet()
        {
            var dt = DateTime.Today;
            var testSubject = new Ledger();
            var testName00 = "Assets";
            var testResults00 = testSubject.Add(testName00, KindsOfAccounts.Asset, false, 101, dt.AddDays(-12));

            Assert.IsNotNull(testResults00);
            var testResults01 = testSubject.Get(testName00);
            Assert.IsNotNull(testResults01);
            Assert.IsTrue(testResults00.Equals(testResults01));

            var testResults02 = testSubject.Get(101);
            Assert.IsNotNull(testResults02);
            Assert.IsTrue(testResults01.Equals(testResults02));

            var voca = new VocaBase(testName00);
            var testResults03 = testSubject.Get(voca);
            Assert.IsNotNull(testResults03);
            Assert.IsTrue(testResults02.Equals(testResults03));
        }

        [Test]
        public void TestRemove()
        {
            var dt = DateTime.Today;
            var testSubject = new Ledger();
            var testName00 = "Assets";
            var testName01 = "Liabilities";
            var testResults00 = testSubject.Add(testName00, KindsOfAccounts.Asset, false, 101, dt.AddDays(-12));
            Assert.IsNotNull(testResults00);
            var testResults01 = testSubject.Add(testName01, KindsOfAccounts.Asset, false, 112, dt.AddDays(-12));
            Assert.IsNotNull(testResults01);
            var testResults02 = testSubject.Remove(testName01);
            Assert.IsNotNull(testResults02);
            Assert.IsTrue(testResults01.Equals(testResults02));

            Assert.IsNull(testSubject.Get(testName01));

        }

        [Test]
        public void TestPostBalance()
        {
            var testInput = new Balance("Journal-552");
            var dt = DateTime.UtcNow;
            var assets = new VocaBase("Assets");
            var liabilities = new VocaBase("Liabilities");
            var ownersEquity = new VocaBase("Owner's Equity");
            testInput.AddNegativeValue(dt.AddDays(-360), new Pecuniam(-450.0M), assets);
            testInput.AddNegativeValue(dt.AddDays(-30), new Pecuniam(-461.0M), liabilities);
            testInput.AddNegativeValue(dt.AddDays(-120), new Pecuniam(-458.0M), ownersEquity);
            testInput.AddNegativeValue(dt.AddDays(-150), new Pecuniam(-457.0M), assets);
            testInput.AddNegativeValue(dt.AddDays(-90), new Pecuniam(-459.0M), liabilities);
            testInput.AddNegativeValue(dt.AddDays(-240), new Pecuniam(-454.0M), ownersEquity);
            testInput.AddNegativeValue(dt.AddDays(-60), new Pecuniam(-460.0M), assets);
            testInput.AddNegativeValue(dt.AddDays(-300), new Pecuniam(-452.0M), liabilities);
            testInput.AddNegativeValue(dt.AddDays(-270), new Pecuniam(-453.0M), ownersEquity);
            testInput.AddNegativeValue(dt.AddDays(-180), new Pecuniam(-456.0M), assets);
            testInput.AddNegativeValue(dt.AddDays(-210), new Pecuniam(-455.0M), liabilities);
            testInput.AddNegativeValue(dt.AddDays(-330), new Pecuniam(-451.0M), assets);

            //charges
            testInput.AddPositiveValue(dt.AddDays(-365), new Pecuniam(8000.0M), assets);
            testInput.AddPositiveValue(dt.AddDays(-350), new Pecuniam(164.4M), liabilities);
            testInput.AddPositiveValue(dt.AddDays(-198), new Pecuniam(165.4M), ownersEquity);
            testInput.AddPositiveValue(dt.AddDays(-24), new Pecuniam(166.4M), assets);
            testInput.AddPositiveValue(dt.AddDays(-74), new Pecuniam(167.4M), liabilities);
            testInput.AddPositiveValue(dt.AddDays(-88), new Pecuniam(168.4M), ownersEquity);
            testInput.AddPositiveValue(dt.AddDays(-92), new Pecuniam(169.4M), assets);
            testInput.AddPositiveValue(dt.AddDays(-121), new Pecuniam(170.4M), liabilities);
            testInput.AddPositiveValue(dt.AddDays(-180), new Pecuniam(171.4M), ownersEquity);
            testInput.AddPositiveValue(dt.AddDays(-142), new Pecuniam(172.4M), assets);
            testInput.AddPositiveValue(dt.AddDays(-155), new Pecuniam(173.4M), liabilities);

            var testSubject = new Ledger();
            testSubject.PostBalance(testInput);

            var testResult00 = testSubject.Get("Assets");
            Assert.IsNotNull(testResult00);

            var testResult01 = testSubject.Get("Liabilities");
            Assert.IsNotNull(testResult01);

            var testResult02 = testSubject.Get("Owner's Equity");
            Assert.IsNotNull(testResult02);
        }

        [Test]
        public void TestIsBalanced()
        {
            var dt = DateTime.Today;
            var assets = new Account(new AccountId("Assets", 100), dt, KindsOfAccounts.Asset, false);
            var liabilities = new Account(new AccountId("Liabilities", 200),dt, KindsOfAccounts.Liability, false);
            var equity = new Account(new AccountId("Equity",300),dt, KindsOfAccounts.Equity, false );

            assets.Credit(100M.ToPecuniam(), new TransactionNote(), dt.AddDays(1));
            liabilities.Credit(50M.ToPecuniam(), new TransactionNote(), dt.AddDays(1));
            equity.Credit(50M.ToPecuniam(), new TransactionNote(), dt.AddDays(1));

            var testSubject = new Ledger {assets, liabilities, equity};
            var testResult = testSubject.IsBalanced();
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestDetectDup()
        {
            var yyyy = DateTime.Today.Year;
            var ledger = new Ledger();

            var journal = new Journal(new AccountId("J1"));

            var testAmount = 10000M.ToPecuniam();

            journal.Debit(testAmount, new TransactionNote("Cash"), new DateTime(yyyy, 10, 1))
                .Credit(testAmount,
                    new TransactionNote("Owners Capital") { AssociatedAccountType = KindsOfAccounts.Equity });
            ledger.PostBalance(journal);

            var testAccount = ledger.Get("Cash") as Account;
            Assert.IsNotNull(testAccount);
            var testAccountValue = testAccount.Value;
            Assert.IsNotNull(testAccountValue);
            Assert.AreNotEqual(Pecuniam.Zero, testAccountValue);

            var testTransaction = journal.FirstTransaction;
            Assert.IsNotNull(testTransaction);

            var testTraceId = testTransaction.GetThisAsTraceId(DateTime.UtcNow, journal);
            Assert.AreEqual(testTransaction.UniqueId, testTraceId.UniqueId);
            var testResult = ledger.IsDuplicate(testAccount, testTraceId);
            Assert.IsTrue(testResult);

            ledger.PostBalance(journal);
            testAccount = ledger.Get("Cash") as Account;
            Assert.IsNotNull(testAccount);
            Console.WriteLine(testAccount.Balance.TransactionCount);
        }

        [Test]
        public void TestFromText()
        {
            var yyyy = DateTime.Today.Year;
            var ledger = new Ledger();

            var journal = new Journal(new AccountId("J1"));

            //on October 1, such-and-such invests 10000 cas in Company
            journal.Debit(10000M.ToPecuniam(), new TransactionNote("Cash"), new DateTime(yyyy, 10, 1))
                .Credit(10000M.ToPecuniam(),
                    new TransactionNote("Owners Capital") {AssociatedAccountType = KindsOfAccounts.Equity});
            ledger.PostBalance(journal);

            /*
             * on Oct 1, Company purchases office equipment costing 5000 
             * USD by signing 3-month, 12%, 5000 USD note payable
             */
            journal.Debit(5000M.ToPecuniam(), new TransactionNote("Equipment"), new DateTime(yyyy, 10, 1))
                .Credit(5000M.ToPecuniam(),
                    new TransactionNote("Notes Payable", "(Issued 3-month, 12% not for office equipment)")
                    {
                        AssociatedAccountType = KindsOfAccounts.Liability
                    });
            ledger.PostBalance(journal);

            /*
             * On Oct 2, Pioneer receives a 1200 USD cash advance from R.Lnox, a client, for
             * advertising services that are expected to be completed by Dec. 31.
             */
            journal.Debit(1200M.ToPecuniam(), new TransactionNote("Cash"), new DateTime(yyyy, 10, 2))
                .Credit(1200M.ToPecuniam(),
                    new TransactionNote("Unearned Service Revenue", "(Received cash from R.Knox for future service)")
                    {
                        AssociatedAccountType = KindsOfAccounts.Liability
                    });
            ledger.PostBalance(journal);

            /*
             * On Oct 3, Company pays office rent for October in cash 900
             */
            journal.Debit(900M.ToPecuniam(),
                    new TransactionNote("Rent Expense") {AssociatedAccountType = KindsOfAccounts.Equity},
                    new DateTime(yyyy, 10, 3))
                .Credit(900M.ToPecuniam(), new TransactionNote("Cash", "(Paid October rent)"));
             ledger.PostBalance(journal);

            /*
             * On Oct 4. Company pays 600 USD for one-year insurance policy that will expire next year on Sept 30
             */
            journal.Debit(600M.ToPecuniam(), new TransactionNote("Prepaid Insurance"){AssociatedAccountType = KindsOfAccounts.Asset}, new DateTime(yyyy, 10, 4))
                .Credit(600M.ToPecuniam(), new TransactionNote("Cash", "(Paid one-year policy; effective date Oct 1)"));
            ledger.PostBalance(journal);

            /*
             * On Oct 5, Company purchases an estimated 3-month supply of advertising materials on account
             * from Aero Supply for 2500 USD
             */
            journal.Debit(2500M.ToPecuniam(),
                    new TransactionNote("Supplies") {AssociatedAccountType = KindsOfAccounts.Equity},
                    new DateTime(yyyy, 10, 5))
                .Credit(2500M.ToPecuniam(),
                    new TransactionNote("Accounts Payable", "(Purchased supplies on account from Aero Supply)")
                    {
                        AssociatedAccountType = KindsOfAccounts.Liability
                    });
            ledger.PostBalance(journal);

            /*
             * On Oct 5, owner withdraws 500 USD cash for personal use
             */
            journal.Debit(500M.ToPecuniam(),
                    new TransactionNote("Owner's Drawings") {AssociatedAccountType = KindsOfAccounts.Equity},
                    new DateTime(yyyy, 10, 20))
                .Credit(500M.ToPecuniam(), new TransactionNote("Cash", "(Withdraw cash for personal use)"));
            ledger.PostBalance(journal);

            /*
             * On Oct 26, Company owes employee salaries of 4000 USD and pays them in cash
             */
            journal.Debit(4000M.ToPecuniam(),
                    new TransactionNote("Salaries and Wages Expense") {AssociatedAccountType = KindsOfAccounts.Equity},
                    new DateTime(yyyy, 10, 26))
                .Credit(4000M.ToPecuniam(), new TransactionNote("Cash", "(Paid salaries to date)"));
            ledger.PostBalance(journal);

            /*
             * On Oct 31, Company receives 10000 USD in cash from Copa Company for advertising services
             * performed in Octoiber
             */
            journal.Debit(10000M.ToPecuniam(), new TransactionNote("Cash"), new DateTime(yyyy, 10, 31))
                .Credit(10000M.ToPecuniam(),
                    new TransactionNote("Service Revenue", "(Received cash for services performed)")
                    {
                        AssociatedAccountType = KindsOfAccounts.Equity
                    });
            ledger.PostBalance(journal);

            var cashAccount = ledger.Get("Cash") as Account;
            Assert.IsNotNull(cashAccount);
            Console.WriteLine("----Debits----");
            var cashDebits = cashAccount.Balance.GetDebits();
            foreach (var transaction in cashDebits)
            {
                Console.WriteLine(string.Join("\t", transaction.AtTime.ToShortDateString(), transaction.Cash.ToString(), transaction.Trace.ToString()));
            }

            Console.WriteLine();
            Console.WriteLine("----Credits----");

            var cashCredits = cashAccount.Balance.GetCredits();
            foreach (var transaction in cashCredits)
            {
                Console.WriteLine(string.Join("\t", transaction.AtTime.ToShortDateString(), transaction.Cash.ToString()));
            }

            Console.WriteLine();
            Console.WriteLine("----Daily Balance----");
            var cashPerDay = cashAccount.Balance.GetSumPerDay();
            Assert.IsNotNull(cashPerDay);
            foreach (var day in cashPerDay.Keys)
            {
                var val = cashPerDay[day];
                Console.WriteLine(string.Join("\t", day.ToShortDateString(), val.ToString()));
            }

        }
    }
}
