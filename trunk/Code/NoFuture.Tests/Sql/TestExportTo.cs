using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using NoFuture.Sql.Mssql;
using NoFuture.Sql.Mssql.Md;
using NUnit.Framework;

namespace NoFuture.Sql.Tests
{
    [TestFixture]
    public class TestExportTo
    {
        private const string QRY = "SELECT TOP 32 * FROM [AdventureWorks2012].[Person].[Person]";

        [Test]
        public void TestGetTableSchemaAndNameFromExpression()
        {
            var testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from MyTable");
            Assert.AreEqual("MyTable", testResult.TableName);

            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from MyTable where id = 12");
            Assert.AreEqual("MyTable", testResult.TableName);

            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from [MyTable]");
            Assert.AreEqual("MyTable", testResult.TableName);
            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from dbo.MyTable");
            Assert.AreEqual("MyTable", testResult.TableName);
            Assert.AreEqual("dbo", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from [dbo].[MyTable]");
            Assert.AreEqual("MyTable", testResult.TableName);
            Assert.AreEqual("dbo", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from [dbo].[MyTable] where id = 12");
            Assert.AreEqual("MyTable", testResult.TableName);
            Assert.AreEqual("dbo", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from dbo.MyTable where id = 12");
            Assert.AreEqual("MyTable", testResult.TableName);
            Assert.AreEqual("dbo", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from MyDb.dbo.MyTable");
            Assert.AreEqual("MyTable", testResult.TableName);
            Assert.AreEqual("dbo", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from [MyDb].[dbo].[MyTable]");
            Assert.AreEqual("MyTable", testResult.TableName);
            Assert.AreEqual("dbo", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            //MSSQL lets you use anything as long as its in square braces
            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from [MyDb].[MySchema.Data].[MyTable]");
            Assert.AreEqual("MyTable", testResult.TableName);
            Assert.AreEqual("MySchema.Data", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from [BBG].[Ordering].[Transactions] WHERE Id = '1145221B'");
            Assert.AreEqual("Transactions", testResult.TableName);
            Assert.AreEqual("Ordering", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            testResult = ExportTo.GetTableSchemaAndNameFromExpression("select * from [BBG].[Ordering].[Transactions]\n\tWHERE Id = '1145221B'");
            Assert.AreEqual("Transactions", testResult.TableName);
            Assert.AreEqual("Ordering", testResult.SchemaName);
            Console.WriteLine("----------------------------");

            var testResult2 = ExportTo.GetTableSchemaAndNameFromExpression("[BBG].[Ordering].[Transactions]");
            Assert.AreEqual("Transactions", testResult2.TableName);
            Assert.AreEqual("Ordering", testResult2.SchemaName);
            Console.WriteLine("----------------------------");

            var testResult3 = ExportTo.GetTableSchemaAndNameFromExpression("[Ordering].Transactions");
            Assert.AreEqual("Transactions", testResult2.TableName);
            Assert.AreEqual("Ordering", testResult2.SchemaName);
            Console.WriteLine("----------------------------");

            testResult =
                ExportTo.GetTableSchemaAndNameFromExpression(
                    "select top 10 programid, partnum from [MyDb].[dbo].[MyTable]");
            Assert.AreEqual("MyTable", testResult.TableName);
            Assert.AreEqual("dbo", testResult.SchemaName);
        }

        [Test]
        public void TestScriptDataBodyInsert()
        {
            var dt = ReadSerializedTableFromDisk("DataTable.Person.bin");
            var testResult = ExportTo.ScriptDataBody(QRY, 32, ExportToStatementType.INSERT,
                SerializedTableMetadata(), dt);

            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestScriptDataBodyUpdate()
        {
            var dt = ReadSerializedTableFromDisk("DataTable.Person.bin");
            var testResult = ExportTo.ScriptDataBody(QRY, 32, ExportToStatementType.UPDATE,
                SerializedTableMetadata(), dt);

            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestScriptDataBodyMerge()
        {
            var dt = ReadSerializedTableFromDisk("DataTable.Person.bin");
            var testResult = ExportTo.ScriptDataBody(QRY, 32, ExportToStatementType.MERGE,
                SerializedTableMetadata(), dt);

            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            
        }

        [Test]
        public void TestTransformSelectStarToLiterals()
        {
            var testInput00 = new List<Common>
            {
                new Common {ColumnName = "myColumn00", DataType = "int", Length = 8},
                new Common {ColumnName = "myColumn01", DataType = "char", Length = 2},
                new Common {ColumnName = "myColumn02", DataType = "DateTime", Length = 8},
                new Common {ColumnName = "myColumn03", DataType = "varchar", Length = 50},
                new Common {ColumnName = "myColumn04", DataType = "varchar", Length = 50},
                new Common {ColumnName = "myColumn05", DataType = "binary", Length = 8},
                new Common {ColumnName = "myColumn06", DataType = "uniqueindentifier", Length = 8},
                new Common {ColumnName = "myColumn07", DataType = "xml", Length = 8},
                new Common {ColumnName = "myColumn08", DataType = "bigmoney", Length = 8}
            };

            var testResult = NoFuture.Sql.Mssql.ExportTo.TransformSelectStarToLiterals(QRY, testInput00);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty,testResult);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestFormatKeyValue()
        {
            var testResult = NoFuture.Sql.Mssql.ExportTo.FormatKeyValue("LastName", "Tamburello", 0, 2);
            Assert.IsNotNull(testResult.Item1);
            Assert.IsNotNull(testResult.Item2);

            Assert.AreNotEqual(string.Empty, testResult.Item1);
            Assert.AreNotEqual(string.Empty, testResult.Item2);

            Assert.IsTrue(testResult.Item1.StartsWith("["));
            Assert.IsTrue(testResult.Item1.EndsWith("]"));

            Assert.AreEqual("Tamburello", testResult.Item2);
        }

        [Test]
        public void TestGetQryValueWrappedWithLimit()
        {
            var testResult = ExportTo.GetQryValueWrappedWithLimit(SerializedTableMetadata(),
                "LastName", "Tamburello", 0);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("''",testResult);
            Assert.AreEqual("'Tamburello'",testResult);

            testResult = ExportTo.GetQryValueWrappedWithLimit(SerializedTableMetadata(),
                "LastName", "Tamburello", 5);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("''",testResult);
            Assert.AreEqual("'Tambu'", testResult);

            testResult = ExportTo.GetQryValueWrappedWithLimit(SerializedTableMetadata(),
                "EmailPromotion", "1", 0);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("NULL",testResult);
            Assert.AreEqual("1",testResult);

            testResult = ExportTo.GetQryValueWrappedWithLimit(SerializedTableMetadata(),
                "EmailPromotion", "NULL", 0);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual("1", testResult);
            Assert.AreEqual("NULL", testResult);

        }

        internal static NoFuture.Sql.Mssql.Md.PsMetadata SerializedTableMetadata()
        {
            var psmd = new NoFuture.Sql.Mssql.Md.PsMetadata
            {
                IsComputedKeys = new List<string>(),
                TickKeys = new List<string>
                {
                    "PersonType",
                    "Title",
                    "FirstName",
                    "MiddleName",
                    "LastName",
                    "Suffix",
                    "Demographics",
                    "rowguid",
                    "ModifiedDate"
                },
                PkKeys = new Dictionary<string, string> {{"BusinessEntityID", "int"}},
                FkKeys = new Dictionary<string, string> {{"BusinessEntityID", "int"}}
            };

            return psmd;
        }

        internal static DataRow[] ReadSerializedTableFromDisk(string embeddedResourceName)
        {
            var binSer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            var asmStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream($"{asmName}.{embeddedResourceName}");
            if (asmStream == null)
                throw new Exception(string.Format("Cannot read the embedded resource named '{0}'", embeddedResourceName));
            using (var binRdr = new BinaryReader(asmStream))
            {
                var dtBuffer = binSer.Deserialize(binRdr.BaseStream);
                var dt = dtBuffer as DataTable;
                if (dt == null)
                    throw new Exception(string.Format("Cannot conduct this test because the deserialization of '{0}' failed.", embeddedResourceName));

                var dataRows = new DataRow[dt.Rows.Count];
                dt.Rows.CopyTo(dataRows,0);
                return dataRows;
            }

        }
    }
}
