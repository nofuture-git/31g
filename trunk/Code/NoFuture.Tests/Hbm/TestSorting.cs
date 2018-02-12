using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NoFuture.Hbm;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Hbm
{
    [TestFixture]
    public class TestSorting
    {
        [SetUp]
        public void Init()
        {
            NfConfig.TempDirectories.Hbm = TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
        }

        [Test]
        public void TestIsNoPkAndAllNonNullable()
        {
            var testResult = NoFuture.Hbm.Sorting.IsNoPkAndAllNonNullable("dbo.TableIncompatiableWithOrm");
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestGetHbmDbPkData()
        {
            var testResult = NoFuture.Hbm.Sorting.GetHbmDbPkData();
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestGetHbmDbFkData()
        {
            var testInput = NoFuture.Hbm.Sorting.GetHbmDbPkData();
            var testResult = NoFuture.Hbm.Sorting.GetHbmDbFkData(testInput);
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestGetHbmDbSubsequentData()
        {
            var testREsult = NoFuture.Hbm.Sorting.GetHbmDbSubsequentData();
            Assert.IsNotNull(testREsult);
        }

        [Test]
        public void TestAllTablesWithPk()
        {
            var testResult = NoFuture.Hbm.Sorting.AllTablesWithPkNames;
            Assert.IsNotNull(testResult);
            foreach(var i in testResult)
                Console.WriteLine(i);
        }

        [Test]
        public void TestAllStoredProcs()
        {
            var testResult = NoFuture.Hbm.Sorting.AllStoredProx;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var procMeta in testResult.Keys)
                Console.WriteLine(procMeta);
        }

        [Test]
        public void TestAddNoPkAllNonNullableToDoNotReference()
        {
            var testResult = Settings.AddNoPkAllNonNullableToBlockedNameList();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0,testResult);
            foreach(var t in NoFuture.Hbm.Settings.DoNotReference)
                Console.WriteLine(t);
        }

        [Test]
        public void TestNullableDateEquality()
        {
            System.DateTime? dt1 = null;
            DateTime? dt2 = null;

            dt1 = new DateTime(2015,2,20);
            dt2 = new DateTime(2015, 2, 20);

            Assert.IsTrue(dt1 == dt2);
        }

        [Test]
        public void TestGetHbmFksWhichArePks()
        {
            NfConfig.TempDirectories.Hbm = @"C:\Projects\31g\trunk\temp\code\hbm";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "ApexQA01";
            NfConfig.BinDirectories.Root = @"C:\Projects\31g\trunk\bin";
            var tbl = "dbo.ClinicStaffDetails";
            var pkManifold = NoFuture.Hbm.Sorting.GetHbmDistinctPks(tbl);
            var fkManifold = NoFuture.Hbm.Sorting.GetHbmDistinctFks(tbl);

            //foreach (var dkf in pkManifold.Keys)
            //{
            //    Console.WriteLine(dkf);
            //    foreach (var fkpkVal in pkManifold[dkf])
            //    {
            //        Console.WriteLine(string.Format("\t{0}", fkpkVal));
            //    }
                
            //}


            //foreach (var dkf in fkManifold.Keys)
            //{
            //    Console.WriteLine(dkf);
            //    foreach (var fkpkVal in fkManifold[dkf])
            //    {
            //        Console.WriteLine(string.Format("\t{0}", fkpkVal));
            //    }
                
            //}

            var fkPkCols = NoFuture.Hbm.Sorting.GetHbmFksWhichArePks(pkManifold, fkManifold);

            //foreach (var fkpk in fkPkCols.Keys)
            //{
            //    Console.WriteLine(fkpk);
            //    foreach (var fkpkVal in fkPkCols[fkpk])
            //    {
            //        Console.WriteLine(string.Format("\t{0}", fkpkVal));
            //    }
            //}

            var iFks = NoFuture.Hbm.Sorting.GetHbmFkNotInPksRemainder(pkManifold, fkPkCols);

            var compKeys = NoFuture.Hbm.Sorting.MakeIntersectTypeId(fkPkCols, iFks, tbl);

            //foreach (var i in compKeys.KeyManyToOne.Keys)
            //{
            //    Console.WriteLine(i);
            //    foreach (var k in compKeys.KeyManyToOne[i])
            //    {
            //        Console.WriteLine(k.ToJsonString());
            //    }
            //    Console.WriteLine("---------------------");
            //}

            //Console.WriteLine("---------------------------------------------");

            //foreach (var km in keyManytoOneColumns)
            //{
            //    Console.WriteLine(km.ToJsonString());
            //}
            var hbmKeyManyToOne = compKeys.KeyManyToOne;

            var keyManytoOneColumns = new List<ColumnMetadata>();
            NoFuture.Hbm.Mapping.HbmKeys.GetKeyManyToOneColumns(tbl, ref keyManytoOneColumns);

            var dk = keyManytoOneColumns.Distinct(new ConstraintNameComparer()).ToList();

            //foreach (var dkjf in hbmKeyManyToOne["dbo.ClinicTieredStationSetupDetails"].Where(
            //    x =>
            //        string.Equals(x.constraint_name, "dbo.FK_ClinicBreaks_ClinicTieredStationSetupDetails", Sorting.C))
            //    .ToList())
            //{
            //    Console.WriteLine(dkjf.ToJsonString());
            //}

            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("");

            foreach (var key in hbmKeyManyToOne.Keys)
            {
                //Console.WriteLine(key);
                foreach (var kd in dk)
                {
                    //Console.WriteLine("\t"  + key + " : " + kd.constraint_name);
                    var dKmtoColumnData = hbmKeyManyToOne[key].Where(
                                x =>
                                    string.Equals(x.constraint_name, kd.constraint_name, Sorting.C)).ToList();
                    foreach (var eo in dKmtoColumnData)
                    {
                        Console.WriteLine("\t\t" + key + " : " + kd.constraint_name + " : " + eo.column_name);
                    }

                }
                    

            }

        }

        [Test]
        public void TestLinqExpr()
        {
            var A = new Dictionary<string, List<string>>
            {
                {"dbo.Table00", new List<string> {"column10", "column11", "column12", "column00"}}
            };
            var B = new Dictionary<string, List<string>>
            {
                {"dbo.Table10", new List<string> {"column10", "column11", "column02"}}
            };

            Console.WriteLine("Union");

            var union = A.SelectMany(x => x.Value).Union(B.SelectMany(y => y.Value));
            foreach (var r in union)
                Console.WriteLine(r);

            Console.WriteLine("");

            Console.WriteLine("Intesect");
            var intersect = A.SelectMany(x => x.Value).Intersect(B.SelectMany(y => y.Value));
            foreach(var r in intersect)
                Console.WriteLine(r);

            Console.WriteLine("");

            Console.WriteLine("Set Difference");
            var setDiff = A.SelectMany(x => x.Value).Except(B.SelectMany(y => y.Value));
            foreach(var r in setDiff)
                Console.WriteLine(r);

            Console.WriteLine("");

            Console.WriteLine("Symetric Difference");
            var symetricDiff =
                A.SelectMany(x => x.Value)
                    .Except(B.SelectMany(y => y.Value))
                    .Union(B.SelectMany(y => y.Value).Except(A.SelectMany(x => x.Value)));

            foreach(var r in symetricDiff)
                Console.WriteLine(r);

            Console.WriteLine("");

            var Alist = new List<string> {"column10", "column11", "column12", "column00"};
            var BList = new List<string> {"column10", "column11", "column02"};

            var simpleListSetDiff = Alist.Select(x => x).Except(BList.Select(y => y)).ToList();

            Console.WriteLine("Set Difference (on simple lists)");
            foreach(var li in simpleListSetDiff)
                Console.WriteLine(li);

            //powershell'esque array splice
            var myList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17 };
            var lastIndex = myList.Count - 1;

            var left = myList.Take(16).ToList();
            var right = myList.Skip(16).Take(myList.Count).ToList();
            foreach(var dk in right)
                Console.WriteLine(dk);
            
        }

    }
}
