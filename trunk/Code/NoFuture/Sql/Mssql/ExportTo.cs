using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using NoFuture.Shared.Core;
using NoFuture.Sql.Mssql.Md;
using NoFuture.Util.Binary;

namespace NoFuture.Sql.Mssql
{
    /// <summary>
    /// Limit to the kind of sql scripts handled by <see cref="ExportTo"/>
    /// </summary>
    public enum ExportToStatementType
    {
        UPDATE,
        INSERT,
        MERGE
    }

    /// <summary>
    /// Utility type intended for the drafting and creation of sql scripts files.
    /// </summary>
    public class ExportTo
    {
        private const StringComparison Oic = StringComparison.OrdinalIgnoreCase;
        public const int SQL_SERVER_FALSE = 0;
        public const int SQL_SERVER_TRUE = 1;

        /// <summary>
        /// Composes  SQL syntax as an insert, update or merge (based on the <see cref="stmtType"/>) statement given the various inputs.
        /// </summary>
        /// <param name="sqlStmt"></param>
        /// <param name="len"></param>
        /// <param name="stmtType"></param>
        /// <param name="metaData"></param>
        /// <param name="results"></param>
        /// <param name="binaryData2Literal">
        /// Optional, have binary data written in the T-SQL format (e.g. 0x00FF), passed over otherwise
        /// </param>
        /// <returns></returns>
        public static string ScriptDataBody(string sqlStmt, int len, ExportToStatementType stmtType, PsMetadata metaData,
            DataRow[] results)
        {
            if (results == null || results.Length <= 0)
                throw new ItsDeadJim("No records found.");


            //#get the table and schema names out of the expression
            var tempPsObj = GetTableSchemaAndNameFromExpression(sqlStmt);
            if (String.IsNullOrWhiteSpace(tempPsObj.TableName))
            {
                throw new RahRowRagee("The sql expression is either invalid or the 'from' statement " +
                                      "does not specify a fully qualified name ([catalog].[dbo].[tablename])");
            }
            var tableName = tempPsObj.TableName;
            var tableSchema = tempPsObj.SchemaName;
            List<string> colNames = null;

            var insertSyntax = new StringBuilder();
            var updateSyntax = new StringBuilder();

            var mergeDataLine = new List<string>();

            Func<PsMetadata, string, bool> isSkipColumn =
                (metadata, key) =>
                {
                    
                    return (metaData.AutoNumKeys?.Any(x => String.Equals(x, key, Oic)) ?? false) ||
                           (metaData.IsComputedKeys?.Any(x => String.Equals(x, key, Oic)) ?? false) ||
                           (metaData.TimestampColumns?.Any(x => String.Equals(x, key, Oic)) ?? false);
                };

            //#for each database record
            foreach (DataRow currec in results)
            {
                if(colNames == null)
                    colNames = currec.Table.Columns.OfType<DataColumn>().Select(x => x.ColumnName).ToList();

                var updateWhere = new List<string>();
                var setStmt = new List<string>();
                var insertStmt = new List<string>();
                var insertVals = new List<string>();
                var mergeVals = new List<string>();
                var counter = 0;
                foreach (var key in colNames)
                {
                    if (String.IsNullOrWhiteSpace(key))
                        continue;

                    if (isSkipColumn(metaData, key))
                        continue;

                    //#eval out all 'DBNull.Value'
                    var val = String.IsNullOrWhiteSpace(currec[key].ToString()) ? "NULL" : currec[key].ToString();

                    //handle byte arrays different since ToString is not a value-literal
                    if (String.Equals(val, "System.Byte[]", Oic))
                    {
                        val = "0x" + ByteArray.PrintByteArray((byte[]) currec[key]);
                    }

                    //#truncate varcharesque values then wrap them in single quotes
                    val = GetQryValueWrappedWithLimit(metaData, key, val, len);
                    Tuple<string, string> formattedPair;

                    //#format both key and value with extra padding
                    if (stmtType == ExportToStatementType.UPDATE)
                    {
                        formattedPair = FormatKeyValue(key, val, len + 2, counter++);

                        var uws = $"{formattedPair.Item1} = {formattedPair.Item2}";

                        //#set a where statement off the primary key if that primary key was present in the select stmt
                        if (metaData.PkKeys.Keys.Any(x => String.Equals(x, key, Oic)))
                            updateWhere.Add(uws);
                        else
                            setStmt.Add(uws);
                    }

                    if (stmtType == ExportToStatementType.INSERT)
                    {
                        formattedPair =  FormatKeyValue(key, val, len + 2, counter++, true);

                        //#set the insert values neatly printed into text columns
                        insertStmt.Add($"{formattedPair.Item1}");
                        insertVals.Add($"{formattedPair.Item2}");
                    }

                    if (stmtType == ExportToStatementType.MERGE)
                    {
                        formattedPair = FormatKeyValue(key, val, len + 2, counter++, true);
                        mergeVals.Add(formattedPair.Item2);
                    }

                } //#end foreach datakey
                if (stmtType == ExportToStatementType.INSERT)
                {
                    //#close the blocks
                    insertSyntax.AppendFormat("\nINSERT INTO [{0}].[{1}]\n(\n\t", tableSchema, tableName);
                    insertSyntax.Append(String.Join(",\n\t", insertStmt));
                    insertSyntax.Append("\n)\nVALUES\n(\n\t");
                    insertSyntax.Append(String.Join(",\n\t", insertVals));
                    insertSyntax.Append("\n)\n");
                    continue;
                }
                if (stmtType == ExportToStatementType.UPDATE)
                {
                    updateSyntax.AppendFormat("\nUPDATE  [{0}].[{1}]\nSET     {2}\nWHERE   {3}\n", tableSchema, tableName,
                        String.Join(",\n        ", setStmt), String.Join(" AND ", updateWhere));
                }

                if (stmtType == ExportToStatementType.MERGE)
                {
                    mergeDataLine.Add($"({String.Join(",\n             ", mergeVals)})");
                }
            }

            if(stmtType == ExportToStatementType.INSERT || stmtType == ExportToStatementType.UPDATE)
                return stmtType == ExportToStatementType.INSERT ? insertSyntax.ToString() : updateSyntax.ToString();

            //draft the MERGE statement
            var mergeData = String.Join("\n    ,", mergeDataLine);
            var matchOnColumn = new List<string>();
            var otherColumn = new List<string>();

            //divide column names
            if (colNames == null)
                return null;

            foreach (var key in colNames)
            {
                if (String.IsNullOrWhiteSpace(key))
                    continue;
                if (isSkipColumn(metaData,key))
                    continue;

                if (metaData.PkKeys.Keys.Any(x => String.Equals(x, key, Oic)))
                    matchOnColumn.Add($"[{key}]");
                else
                    otherColumn.Add($"[{key}]");
            }

            var matchOnList = matchOnColumn.Select(matchOn => String.Format("target.{0} = source.{0}", matchOn)).ToList();
            var updateSetList = otherColumn.Select(ot => String.Format("{0} = source.{0}", ot)).ToList();
            var insertNameList = metaData.IsIdentityInsert ? otherColumn : colNames;
            var insertValList = metaData.IsIdentityInsert
                ? otherColumn.Select(ot => $"source.{ot}").ToList()
                : colNames.Select(cn => $"source.{cn}").ToList();
            var mergeSrcList = colNames.Where(c => !String.IsNullOrWhiteSpace(c) && !isSkipColumn(metaData, c));

            var mergeStatementBuilder = new StringBuilder();
            mergeStatementBuilder.AppendFormat("MERGE [{0}].[{1}] AS target\n", tableSchema, tableName);
            mergeStatementBuilder.Append("USING (\n");
            mergeStatementBuilder.Append("VALUES\n     ");
            mergeStatementBuilder.Append(mergeData);
            mergeStatementBuilder.AppendFormat("\n    ) AS source (\n             {0})\n", String.Join(",\n             ", mergeSrcList));
            mergeStatementBuilder.AppendFormat("ON ({0})\n", String.Join(" AND ", matchOnList));
            mergeStatementBuilder.AppendLine("WHEN MATCHED THEN");
            mergeStatementBuilder.AppendFormat("  UPDATE SET {0}\n", String.Join(",\n             ", updateSetList));
            mergeStatementBuilder.AppendLine("WHEN NOT MATCHED THEN");
            mergeStatementBuilder.AppendFormat("    INSERT ( {0}\n           )\n",
                String.Join(",\n             ", insertNameList));
            mergeStatementBuilder.AppendFormat("    VALUES ( {0}\n           );\n",
                String.Join(",\n             ", insertValList));

            return mergeStatementBuilder.ToString();
        }

        /// <summary>
        /// Helper method charged with formatting values for use in on the export functions.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="len"></param>
        /// <param name="counter"></param>
        /// <param name="withNameMarker">
        /// Set to true to have a small comment block appear, which includes the ordinal and column name, to the left of the <see cref="value"/>
        /// </param>
        /// <returns></returns>
        internal static Tuple<string, string> FormatKeyValue(string key, string value, int len, int counter, bool withNameMarker = false)
        {
            if (String.Equals(value, Boolean.TrueString, StringComparison.OrdinalIgnoreCase))
                value = SQL_SERVER_TRUE.ToString(CultureInfo.InvariantCulture);
            if (String.Equals(value, Boolean.FalseString, StringComparison.OrdinalIgnoreCase))
                value = SQL_SERVER_FALSE.ToString(CultureInfo.InvariantCulture);
            var val = withNameMarker ? $"/*{counter:000}-{key}*/ {value}" : value;
            key = $"[{key}]";

            return new Tuple<string, string>(key, val);
        }

        /// <summary>
        /// Determines the schema and table name from a simple sql select statement.
        /// </summary>
        /// <param name="sqlStmt"></param>
        /// <returns></returns>
        /// <remarks>
        /// The intended purpose is not to parse entire complex sql statements but rather
        /// to get the schema and table name from a sql expression being used to form an 
        /// export.  The required format is 
        /// \s[Ff][Rr][Oo][Mm]\s(targeted string)\s
        /// where (targeted string) is the continuous chars up to the first space char(0x20) 
        /// in which said space char is not enclosed in square braces.
        /// </remarks>
        public static Common GetTableSchemaAndNameFromExpression(string sqlStmt)
        {
            var flagInSqrBraces = false;
            var flagHavePassedFrom = false;
            var fromBuffer = new char[] {'-', '-', '-', '-', '-', '-'};
            var startIdx = 0;
            var endIdx = 0;

            var sql = sqlStmt.ToCharArray();
            for (var i = 0; i < sql.Length; i++)
            {
                
                if (!flagHavePassedFrom)
                {
                    //does the stream afford enough space to consume what's left of ' from '
                    if (i + fromBuffer.Length >= sql.Length)
                        break;

                    //look ahead
                    for (var j = 0; j < fromBuffer.Length; j++)
                        fromBuffer[j] = sql[i + j];


                    if (String.Equals(" from ", new string(fromBuffer), StringComparison.OrdinalIgnoreCase))
                    {
                        flagHavePassedFrom = true;
                        //move ahead in the stream
                        i = i + fromBuffer.Length;
                        startIdx = i;
                    }
                }

                if (!flagHavePassedFrom)
                    continue;

                //need to move the stream out till the first whitespace not enclosed in square braces
                if (sql[i] == '[')
                {
                    flagInSqrBraces = true;
                    continue;
                }
                if (sql[i] == ']')
                {
                    flagInSqrBraces = false;
                    continue;
                }
                if (!flagInSqrBraces && Char.IsWhiteSpace(sql[i]))
                {
                    endIdx = i;
                    break;
                }
            }

            if (endIdx <= 0)
                endIdx = sqlStmt.Length;

            var targetString = sqlStmt.Substring(startIdx, endIdx - startIdx);
            Debug.WriteLine(targetString);
            flagInSqrBraces = false;
            
            startIdx = 0;
            endIdx = targetString.Length;
            sql = targetString.ToCharArray();
            for (var j = sql.Length-1; j >= 0; j--)
            {
                //starting on the right is the first char the close square brace
                if (j == sql.Length - 1 && sql[j] == ']')
                {
                    flagInSqrBraces = true;
                    endIdx = endIdx - 1;
                    continue;
                }

                if (sql[j] != '[' && (flagInSqrBraces || sql[j] != '.')) continue;
                startIdx = j + 1;
                break;
            }

            if (endIdx <= 0)
                endIdx = targetString.Length;

            var tableName = targetString.Substring(startIdx, endIdx - startIdx);

            //no schema nor catalog qualifiers present
            if (startIdx == 0)
                return new Common {TableName = tableName};

            targetString = targetString.Substring(0, startIdx - 1);
            sql = targetString.ToCharArray();
            flagInSqrBraces = false;
            var schemaBuffer = new StringBuilder();

            for (var k = sql.Length - (targetString.EndsWith(".") ? 2 : 1) ; k >= 0; k--)
            {
                if (sql[k] == ']')
                {
                    flagInSqrBraces = true;
                    continue;
                }

                if ((flagInSqrBraces && sql[k] == '[') || (!flagInSqrBraces && sql[k] == '.'))
                    break;
                schemaBuffer.Append(sql[k]);
            }
            return new Common {TableName = tableName, SchemaName = new string(schemaBuffer.ToString().Reverse().ToArray())};
        }

        /// <summary>
        /// Will wrap the <see cref="val"/> in single-quotes if the <see cref="key"/> is found in 
        /// the <see cref="PsMetadata.TickKeys"/> list and the <see cref="val"/> does not equal the string
        /// "NULL" (case-insensitive).
        /// </summary>
        /// <param name="metaData"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="maxLength">
        /// Optional field to limit the size of string literals, set it to zero or less
        /// to not truncate any text from <see cref="val"/>
        /// </param>
        /// <returns></returns>
        public static string GetQryValueWrappedWithLimit(PsMetadata metaData, string key, string val, int maxLength)
        {
            if (String.IsNullOrWhiteSpace(key))
                return val;

            if (metaData == null)
                return val;

            if (metaData.AutoNumKeys.Any(x => String.Equals(x, key, Oic)) ||
                metaData.IsComputedKeys.Any(x => String.Equals(x, key, Oic)))
                return val;
            if (!metaData.TickKeys.Any(x => String.Equals(x, key, Oic)) || String.Equals(val, "NULL", Oic))
                return val;

            if (val.Contains("'"))
                val = val.Replace("'", "''");

            return val.Length > maxLength && maxLength > 0
                ? $"'{val.Substring(0, maxLength)}'"
                : $"'{val}'";
        }

        /// <summary>
        /// Replaces the " * " part of the select criterion with the names
        /// of all the columns present in <see cref="metaData"/>.
        /// </summary>
        /// <param name="sqlStmt"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public static string TransformSelectStarToLiterals(string sqlStmt, List<Common> metaData)
        {
            const string MARKER = " * ";
            if (String.IsNullOrWhiteSpace(sqlStmt))
                return null;
            if (metaData == null || metaData.Count <= 0)
                return sqlStmt;

            sqlStmt = sqlStmt.Trim();

            if (!sqlStmt.Contains(MARKER))
                return sqlStmt;

            var criterion =
                metaData.Select(
                    md =>
                        md.DataType == "binary"
                            ? String.Format("rtrim(convert(char(34), {0}, 1)) AS {0}", md.ColumnName)
                            : md.ColumnName).ToList();

            var startAt = sqlStmt.ToLower().IndexOf(MARKER, StringComparison.Ordinal) + MARKER.Length;
            if (startAt == 0 || startAt == MARKER.Length)
                return sqlStmt;

            var sqlRight = sqlStmt.Substring(startAt, sqlStmt.Length - startAt);
            var sqlLeft = sqlStmt.Substring(0, startAt - MARKER.Length);
            return $"{sqlLeft} {String.Join(", ", criterion)} {sqlRight}";
        }
    }
}
