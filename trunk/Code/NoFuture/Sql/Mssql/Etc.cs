﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;

namespace NoFuture.Sql.Mssql
{
    public class Etc
    {
        #region private static fields
        private static readonly Dictionary<string, string[]> _sqlServers = new Dictionary<string, string[]>();

        #endregion

        /// <summary>
        /// Drafts a command line expression for invoking the older SQLCMD.exe.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="server"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string MakeSqlCmd(string expression, string server, string dbName)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException(nameof(expression));
            if (string.IsNullOrWhiteSpace(server))
                throw new ArgumentNullException(nameof(server));
            if (string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentNullException(nameof(dbName));

            var sqlParams = new StringBuilder();
            sqlParams.Append("-S ");
            sqlParams.Append("\""+  server + "\"");
            sqlParams.Append(" -d ");
            sqlParams.Append(dbName);
            if (NfConfig.Switches.SqlCmdHeadersOff)
            {
                sqlParams.Append(" -k 2 -h \"-1\" -W -s \"|\" -Q ");
            }
            else
            {
                if (expression.ToLower().StartsWith("create") || expression.ToLower().StartsWith("update") ||
                    expression.ToLower().StartsWith("insert") || expression.ToLower().StartsWith("delete"))
                {
                    sqlParams.Append(" -k 2 -W -s \"|\" -I -Q ");
                }
                else
                {
                    sqlParams.Append(" -k 2 -W -s \"|\" -Q ");
                }
            }
            var sqlExpr = new StringBuilder();
            sqlExpr.Append('"');
            sqlExpr.Append(expression);
            sqlExpr.Append('"');

            var sqlCmd = new StringBuilder();
            sqlCmd.Append("sqlcmd.exe ");
            sqlCmd.Append(sqlParams);
            sqlCmd.Append(sqlExpr);
            return sqlCmd.ToString();
        }

        /// <summary>
        /// Drafts a command line invocation to SQLCMD.EXE using a single or range of 
        /// files as the input.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="server"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string MakeInputFilesSqlCmd(string path, string server, string dbName)
        {
            if(string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if(string.IsNullOrWhiteSpace(server))
                throw new ArgumentNullException(nameof(server));
            if(string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentNullException(nameof(dbName));

            if(!File.Exists(path))
                throw new IOException($"Bad path or file name at '{path}'");

            var sqlCmd = new StringBuilder();
            sqlCmd.Append("sqlcmd.exe");
            sqlCmd.Append(" -S ");
            sqlCmd.Append(server);
            sqlCmd.Append(" -d ");
            sqlCmd.Append(dbName);

            sqlCmd.Append(" -i "); 
            if (!NfPath.HasKnownExtension(path) && Directory.Exists(path))
            {
                var files = new DirectoryInfo(path);
                var fileNames =
                    files.EnumerateFiles("*.sql")
                        .Select(sqlFile => sqlFile.FullName)
                        .Select(sqlFileName => sqlFileName.Contains(" ") ? $"\"{sqlFileName}\"" : path)
                        .ToList();
                sqlCmd.Append(string.Join(",", fileNames));
            }
            else
            {
                var escPath = path.Contains(" ") ? $"\"{path}\"" : path;
                sqlCmd.Append(escPath);
            }

            return sqlCmd.ToString();
        }

        /// <summary>
        /// Operates as the manner for adding entries to the contents printed 
        /// in <see cref="PrintCurrentDbSettings"/> and changed to <see cref="SetMssqlSettings"/>
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="catalogs"></param>
        public static void AddSqlServer(string serverName, string[] catalogs)
        {
            if (string.IsNullOrWhiteSpace(serverName) || catalogs == null || catalogs.Length <= 0)
                return;

            if (_sqlServers.ContainsKey(serverName))
            {
                if (_sqlServers[serverName] != null)
                {
                    var currentCatalogs = _sqlServers[serverName].ToList();
                    currentCatalogs.AddRange(catalogs);
                    _sqlServers[serverName] = currentCatalogs.Distinct().ToArray();
                }
                else
                {
                    _sqlServers[serverName] = catalogs;
                }
            }
            else
            {
                _sqlServers.Add(serverName, catalogs);
            }
        }

        /// <summary>
        /// Prints all entries added via <see cref="AddSqlServer"/> to a 
        /// string in the form of a matrix whose x-index may be used 
        /// in <see cref="SetMssqlSettings"/>
        /// </summary>
        /// <returns></returns>
        public static string PrintCurrentDbSettings()
        {
            var sqlMatrixPrint = new List<StringBuilder>{new StringBuilder(), new StringBuilder()};
            sqlMatrixPrint[0].Append("                          ");
            sqlMatrixPrint[1].Append("                             ");
            var xAxisTotal =
                _sqlServers.Keys.Where(x => _sqlServers[x] != null)
                    .Select(key => _sqlServers[key].Length)
                    .Concat(new[] {0})
                    .Max();
            for (var i = 0; i < xAxisTotal; i++)
            {
                sqlMatrixPrint[0].AppendFormat("         {0}              ",i);
                sqlMatrixPrint[1].Append("---------------------   ");
            }

            for (var j = 0; j < _sqlServers.Keys.Count(x => _sqlServers[x] != null); j++)
            {
                var sqlServer = _sqlServers.Keys.ToArray()[j];
                var catalogs = new StringBuilder();
                catalogs.AppendFormat("{0,-2} | {1,-24}", j, sqlServer);
                foreach (var catalog in _sqlServers[sqlServer])
                {
                    catalogs.AppendFormat("{0,-24}", catalog);
                }
                sqlMatrixPrint.Add(catalogs);
            }

            var sqlMatrixConsolePrint = new StringBuilder();
            sqlMatrixConsolePrint.AppendLine("***************");
            sqlMatrixConsolePrint.AppendLine(
                $"Current setttings:: -Server '{NfConfig.SqlServer}' -Database '{NfConfig.SqlCatalog}' -Header Off '{NfConfig.Switches.SqlCmdHeadersOff}' -Filter Off '{NfConfig.Switches.SqlFiltersOff}'");
            sqlMatrixConsolePrint.AppendLine("***************");
            foreach (var sb in sqlMatrixPrint)
                sqlMatrixConsolePrint.AppendLine(sb.ToString());

            return sqlMatrixConsolePrint.ToString();
        }

        /// <summary>
        /// Will change the <see cref="NfConfig.SqlServer"/> 
        /// to the index, as printed in <see cref="PrintCurrentDbSettings"/>
        /// using the <see cref="qc1"/> value and likewise 
        /// for <see cref="NfConfig.SqlCatalog"/> using 
        /// <see cref="qc2"/>.  Both values must correspond to 
        /// entries printed from <see cref="PrintCurrentDbSettings"/>
        /// </summary>
        /// <param name="qc1"></param>
        /// <param name="qc2"></param>
        /// <returns></returns>
        public static void SetMssqlSettings(int qc1, int qc2)
        {
            if (_sqlServers.Count <= 0)
                return;

            var serverNames = _sqlServers.Keys.ToArray();
            if (qc1 > serverNames.Length)
                qc1 = 0;
            NfConfig.SqlServer = serverNames[qc1];

            var catalogNames = _sqlServers[NfConfig.SqlServer];
            if (catalogNames == null || catalogNames.Length <= 0)
                return;

            if (qc2 > catalogNames.Length)
                qc2 = 0;
            NfConfig.SqlCatalog = catalogNames[qc2];
        }

        /// <summary>
        /// A list of names which should be excluded from the query used with <see cref="Mssql.Qry.Catalog.SelectProcedureString"/>
        /// </summary>
        public static List<string> SqlFilterList { get; } = new List<string>();

        /// <summary>
        /// An optional list whose contents are compared to the value at <see cref="NfConfig.SqlServer"/>
        /// Intended for use within PowerShell to force the user to choose to continue when
        /// doing some kind of high risk activity.
        /// </summary>
        public static List<string> WarnUserIfServerIn { get; } = new List<string>();

        /// <summary>
        /// Filter applied in sql cmd-lets
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string FilterSqlExpression(string expression, string columnName)
        {
            if (SqlFilterList.Count <= 0) return expression;

            NfConfig.Switches.SqlFiltersOff = false;
            var tblFilter = SqlFilterList.Where(x => !string.IsNullOrWhiteSpace(x))
                .Aggregate("", (current, lo) => current + $"'{lo}',");
            tblFilter +=
                "'sysobjects','sysindexes','syscolumns','systypes','syscomments'," +
                "'sysusers','sysdepends','sysreferences','sysconstraints','syssegments'";
            expression = $"{expression}{string.Format(Qry.Catalog.FilterStatement, columnName, tblFilter)}";
            return expression;

        }
    }
}
