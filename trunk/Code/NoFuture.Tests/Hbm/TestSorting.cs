using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Hbm;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Shared;

namespace NoFuture.Tests.Hbm
{
    [TestClass]
    public class TestSorting
    {
        [TestInitialize]
        public void Init()
        {
            NoFuture.TempDirectories.Hbm = TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
        }

        [TestMethod]
        public void TestIsNoPkAndAllNonNullable()
        {
            var testResult = NoFuture.Hbm.Sorting.IsNoPkAndAllNonNullable("dbo.TableIncompatiableWithOrm");
            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void TestGetHbmDbPkData()
        {
            var testResult = NoFuture.Hbm.Sorting.GetHbmDbPkData();
            Assert.IsNotNull(testResult);
        }

        [TestMethod]
        public void TestGetHbmDbFkData()
        {
            var testInput = NoFuture.Hbm.Sorting.GetHbmDbPkData();
            var testResult = NoFuture.Hbm.Sorting.GetHbmDbFkData(testInput);
            Assert.IsNotNull(testResult);
        }

        [TestMethod]
        public void TestGetHbmDbSubsequentData()
        {
            var testREsult = NoFuture.Hbm.Sorting.GetHbmDbSubsequentData();
            Assert.IsNotNull(testREsult);
        }

        [TestMethod]
        public void TestAllTablesWithPk()
        {
            var testResult = NoFuture.Hbm.Sorting.AllTablesWithPkNames;
            Assert.IsNotNull(testResult);
            foreach(var i in testResult)
                System.Diagnostics.Debug.WriteLine(i);
        }

        [TestMethod]
        public void TestAllStoredProcs()
        {
            var testResult = NoFuture.Hbm.Sorting.AllStoredProx;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var procMeta in testResult.Keys)
                System.Diagnostics.Debug.WriteLine(procMeta);
        }

        [TestMethod]
        public void TestAddNoPkAllNonNullableToDoNotReference()
        {
            var testResult = Settings.AddNoPkAllNonNullableToBlockedNameList();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0,testResult);
            foreach(var t in NoFuture.Hbm.Settings.DoNotReference)
                System.Diagnostics.Debug.WriteLine(t);
        }

        [TestMethod]
        public void TestNullableDateEquality()
        {
            System.DateTime? dt1 = null;
            DateTime? dt2 = null;

            dt1 = new DateTime(2015,2,20);
            dt2 = new DateTime(2015, 2, 20);

            Assert.IsTrue(dt1 == dt2);
        }

        [TestMethod]
        public void TestGetHbmFksWhichArePks()
        {
            NoFuture.TempDirectories.Hbm = @"C:\Projects\31g\trunk\temp\code\hbm";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "ApexQA01";
            NoFuture.BinDirectories.Root = @"C:\Projects\31g\trunk\bin";
            var tbl = "dbo.ClinicStaffDetails";
            var pkManifold = NoFuture.Hbm.Sorting.GetHbmDistinctPks(tbl);
            var fkManifold = NoFuture.Hbm.Sorting.GetHbmDistinctFks(tbl);

            //foreach (var dkf in pkManifold.Keys)
            //{
            //    System.Diagnostics.Debug.WriteLine(dkf);
            //    foreach (var fkpkVal in pkManifold[dkf])
            //    {
            //        System.Diagnostics.Debug.WriteLine(string.Format("\t{0}", fkpkVal));
            //    }
                
            //}


            //foreach (var dkf in fkManifold.Keys)
            //{
            //    System.Diagnostics.Debug.WriteLine(dkf);
            //    foreach (var fkpkVal in fkManifold[dkf])
            //    {
            //        System.Diagnostics.Debug.WriteLine(string.Format("\t{0}", fkpkVal));
            //    }
                
            //}

            var fkPkCols = NoFuture.Hbm.Sorting.GetHbmFksWhichArePks(pkManifold, fkManifold);

            //foreach (var fkpk in fkPkCols.Keys)
            //{
            //    System.Diagnostics.Debug.WriteLine(fkpk);
            //    foreach (var fkpkVal in fkPkCols[fkpk])
            //    {
            //        System.Diagnostics.Debug.WriteLine(string.Format("\t{0}", fkpkVal));
            //    }
            //}

            var iFks = NoFuture.Hbm.Sorting.GetHbmFkNotInPksRemainder(pkManifold, fkPkCols);

            var compKeys = NoFuture.Hbm.Sorting.MakeIntersectTypeId(fkPkCols, iFks, tbl);

            //foreach (var i in compKeys.KeyManyToOne.Keys)
            //{
            //    System.Diagnostics.Debug.WriteLine(i);
            //    foreach (var k in compKeys.KeyManyToOne[i])
            //    {
            //        System.Diagnostics.Debug.WriteLine(k.ToJsonString());
            //    }
            //    System.Diagnostics.Debug.WriteLine("---------------------");
            //}

            //System.Diagnostics.Debug.WriteLine("---------------------------------------------");

            //foreach (var km in keyManytoOneColumns)
            //{
            //    System.Diagnostics.Debug.WriteLine(km.ToJsonString());
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
            //    System.Diagnostics.Debug.WriteLine(dkjf.ToJsonString());
            //}

            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("-------------------------------------------------");
            System.Diagnostics.Debug.WriteLine("");

            foreach (var key in hbmKeyManyToOne.Keys)
            {
                //System.Diagnostics.Debug.WriteLine(key);
                foreach (var kd in dk)
                {
                    //System.Diagnostics.Debug.WriteLine("\t"  + key + " : " + kd.constraint_name);
                    var dKmtoColumnData = hbmKeyManyToOne[key].Where(
                                x =>
                                    string.Equals(x.constraint_name, kd.constraint_name, Sorting.C)).ToList();
                    foreach (var eo in dKmtoColumnData)
                    {
                        System.Diagnostics.Debug.WriteLine("\t\t" + key + " : " + kd.constraint_name + " : " + eo.column_name);
                    }

                }
                    

            }

        }

        [TestMethod]
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

            System.Diagnostics.Debug.WriteLine("Union");

            var union = A.SelectMany(x => x.Value).Union(B.SelectMany(y => y.Value));
            foreach (var r in union)
                System.Diagnostics.Debug.WriteLine(r);

            System.Diagnostics.Debug.WriteLine("");

            System.Diagnostics.Debug.WriteLine("Intesect");
            var intersect = A.SelectMany(x => x.Value).Intersect(B.SelectMany(y => y.Value));
            foreach(var r in intersect)
                System.Diagnostics.Debug.WriteLine(r);

            System.Diagnostics.Debug.WriteLine("");

            System.Diagnostics.Debug.WriteLine("Set Difference");
            var setDiff = A.SelectMany(x => x.Value).Except(B.SelectMany(y => y.Value));
            foreach(var r in setDiff)
                System.Diagnostics.Debug.WriteLine(r);

            System.Diagnostics.Debug.WriteLine("");

            System.Diagnostics.Debug.WriteLine("Symetric Difference");
            var symetricDiff =
                A.SelectMany(x => x.Value)
                    .Except(B.SelectMany(y => y.Value))
                    .Union(B.SelectMany(y => y.Value).Except(A.SelectMany(x => x.Value)));

            foreach(var r in symetricDiff)
                System.Diagnostics.Debug.WriteLine(r);

            System.Diagnostics.Debug.WriteLine("");

            var Alist = new List<string> {"column10", "column11", "column12", "column00"};
            var BList = new List<string> {"column10", "column11", "column02"};

            var simpleListSetDiff = Alist.Select(x => x).Except(BList.Select(y => y)).ToList();

            System.Diagnostics.Debug.WriteLine("Set Difference (on simple lists)");
            foreach(var li in simpleListSetDiff)
                System.Diagnostics.Debug.WriteLine(li);

            //powershell'esque array splice
            var myList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17 };
            var lastIndex = myList.Count - 1;

            var left = myList.Take(16).ToList();
            var right = myList.Skip(16).Take(myList.Count).ToList();
            foreach(var dk in right)
                System.Diagnostics.Debug.WriteLine(dk);
            
        }

    }
}
