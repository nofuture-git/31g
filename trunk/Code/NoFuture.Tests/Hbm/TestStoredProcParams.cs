using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Hbm
{
    [TestClass]
    public class TestStoredProcParams
    {
        [TestInitialize]
        public void Init()
        {
            NfConfig.TempDirectories.Hbm = TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
        }

        [TestMethod]
        public void TestMethod1()
        {
            var testItems = NoFuture.Hbm.Sorting.DbContainers.StoredProcsAndParams.Data;
            Assert.IsNotNull(testItems);
            Assert.AreNotEqual(0, testItems.Length);

            foreach (var item in testItems)
            {
                var asDbType = item.GetSqlDataType();
                System.Diagnostics.Debug.WriteLine(asDbType);
            }
                
        }

        [TestMethod]
        public void TestQueryKeysNames()
        {
            var testSubject = new NoFuture.Hbm.DbQryContainers.MetadataDump.HbmStoredProcsAndParams();
            Assert.IsNotNull(testSubject.QueryKeysNames);
            foreach(var qryKey in testSubject.QueryKeysNames)
                System.Diagnostics.Debug.WriteLine(qryKey);
        }

        [TestMethod]
        public void TestAsTsql()
        {
            var testItems = NoFuture.Hbm.Sorting.AllStoredProx;
            Assert.IsNotNull(testItems);
            Assert.AreNotEqual(0, testItems.Count);

            var testItem = testItems[testItems.Keys.First()];
            Assert.AreNotEqual(0, testItem.Parameters.Count);

            using (var conn = new SqlConnection("Server=localhost;Database=Whatever;Trusted_Connection=True;"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        foreach (var ti in testItems.Keys)
                        {
                            var ds = new DataSet(ti);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            testItems[ti].AssignSpParams(cmd.Parameters);
                            cmd.CommandText = ti;
                            cmd.CommandTimeout = 800;
                            da.Fill(ds);

                            if (ds.Tables.Count <= 0)
                                continue;

                            ds.Tables[0].TableName = "Results";

                            ds.WriteXmlSchema(
                                string.Format(TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles\{0}.xsd",
                                    ti));
                        }
                    }
                }
                conn.Close();
            }
        }

        [TestMethod]
        public void TestDataReaderMultidataset()
        {
            var ti = NoFuture.Hbm.Sorting.AllStoredProx["dbo.MyStoredProc"];

            using (var conn = new SqlConnection("Server=localhost;Database=Whatever;Trusted_Connection=True;"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    ti.AssignSpParams(cmd.Parameters);
                    cmd.CommandText = ti.ProcName;
                    cmd.CommandTimeout = 10;
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        do
                        {
                            while (dataReader.Read())
                            {
                                var fldCount = dataReader.FieldCount;
                                System.Diagnostics.Debug.WriteLine(fldCount);
                            }

                        } while (dataReader.NextResult());
                    }

                }
                conn.Close();
            }
            
        }
    }
}
