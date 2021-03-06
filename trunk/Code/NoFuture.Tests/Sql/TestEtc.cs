﻿using System;
using NoFuture.Shared.Cfg;
using NUnit.Framework;

namespace NoFuture.Sql.Tests
{
    [TestFixture]
    public class TestEtc
    {
        public const string sqlQry = "SELECT * FROM [AdventureWorks2012].[Production].[Location]";

        public const string insertQry =
            "INSERT INTO [Person].[Person]([PersonType],[Title],[FirstName],[MiddleName],[LastName]) VALUES ('EM',NULL,'Ken','J','Sánchez')";
        [Test]
        public void TestMakeSqlCommand()
        {
            var expectedResult =
                "sqlcmd.exe -S \"localhost\" -d AdventureWorks2012 -k 2 -W -s \"|\" -Q \"SELECT * FROM [AdventureWorks2012].[Production].[Location]\"";

            var testResult = NoFuture.Sql.Mssql.Etc.MakeSqlCmd(sqlQry, "localhost", "AdventureWorks2012");
            Assert.AreEqual(expectedResult, testResult);
        }

        [Test]
        public void TestMakeSqlCommandHeaderOff()
        {
            NfConfig.Switches.SqlCmdHeadersOff = true;
            var expectedResult =
                "sqlcmd.exe -S \"localhost\" -d AdventureWorks2012 -k 2 -h \"-1\" -W -s \"|\" -Q \"SELECT * FROM [AdventureWorks2012].[Production].[Location]\"";

            var testResult = NoFuture.Sql.Mssql.Etc.MakeSqlCmd(sqlQry, "localhost", "AdventureWorks2012");
            Assert.AreEqual(expectedResult, testResult);

        }

        [Test]
        public void TestMakeSqlCommandDdlQry()
        {
            NfConfig.Switches.SqlCmdHeadersOff = false;
            var expectedResult =
                "sqlcmd.exe -S \"localhost\" -d AdventureWorks2012 -k 2 -W -s \"|\" -I -Q \"INSERT INTO [Person].[Person]([PersonType],[Title],[FirstName],[MiddleName],[LastName]) VALUES ('EM',NULL,'Ken','J','Sánchez')\"";

            var testResult = NoFuture.Sql.Mssql.Etc.MakeSqlCmd(insertQry, "localhost", "AdventureWorks2012");

            Assert.AreEqual(expectedResult, testResult);

        }

        [Test]
        public void TestPrintCurrentDbSettings()
        {
            NoFuture.Sql.Mssql.Etc.AddSqlServer("YEOLDE2", new[] { "ABDev", "OxomOne", "OxomOneQC", "TramnOne" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("QADBATEST", new[] { "OxomOneQC", "TramnOne" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("TAMRDBRPTS", new[] { "OxomReports" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("OxomOne", new[] { "OxomCentral", "TramnOne" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0064", new[] { "OxomOne" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0361", new[] { "DDL", "Audit", "HappeninNow", "Staging", "DDL_ETL" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0418", new[] { "DDL", "Audit", "HappeninNow" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("localhost", new[] { "ApexQA01", "Whatever", "AdventureWorks2012" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0109", new[] { "ASXDEV.TU9G", "ASXQA.TU9G" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0538", new[] { "SMD_ODS", "SMD_STAGING_0001", "Integration" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("ZXHUH0416SQL2\\SQLPROD2", new[] { "DDL", "Audit", "HappeninNow", "DDL_ETL" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("ZXHUH0542NNSQL", new[] { "DDL", "Audit", "HappeninNow", "DDL_ETL" });

            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "AdventureWorks2012";

            var testResult = NoFuture.Sql.Mssql.Etc.PrintCurrentDbSettings();

            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);

        }

        [Test]
        public void TestSetMssqlSettings()
        {
            NoFuture.Sql.Mssql.Etc.AddSqlServer("YEOLDE2", new[] { "ABDev", "OxomOne", "OxomOneQC", "TramnOne" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("QADBATEST", new[] { "OxomOneQC", "TramnOne" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("TAMRDBRPTS", new[] { "OxomReports" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("OxomOne", new[] { "OxomCentral", "TramnOne" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0064", new[] { "OxomOne" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0361", new[] { "DDL", "Audit", "HappeninNow", "Staging", "DDL_ETL" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0418", new[] { "DDL", "Audit", "HappeninNow" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("localhost", new[] { "ApexQA01", "Whatever", "AdventureWorks2012" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0109", new[] { "ASXDEV.TU9G", "ASXQA.TU9G" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("zxhuh0538", new[] { "SMD_ODS", "SMD_STAGING_0001", "Integration" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("ZXHUH0416SQL2\\SQLPROD2", new[] { "DDL", "Audit", "HappeninNow", "DDL_ETL" });
            NoFuture.Sql.Mssql.Etc.AddSqlServer("ZXHUH0542NNSQL", new[] { "DDL", "Audit", "HappeninNow", "DDL_ETL" });

            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "AdventureWorks2012";

            NoFuture.Sql.Mssql.Etc.SetMssqlSettings(10, 2);
            var testResult = NoFuture.Sql.Mssql.Etc.PrintCurrentDbSettings();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);

            Console.WriteLine(testResult);

            Assert.AreEqual(NfConfig.SqlServer, "ZXHUH0416SQL2\\SQLPROD2");
            Assert.AreEqual(NfConfig.SqlCatalog, "HappeninNow");
        }
    }
}
