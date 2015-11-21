namespace NoFuture.Sql.Mssql.Qry
{
    public class Catalog
    {
        /// <summary>
        /// The string constant used in calls to <see cref="QryTableKeys"/> as the 
        /// zero index of the string format.
        /// </summary>
        public const string PRIMARY_KEY_STRING = "PRIMARY KEY";

        /// <summary>
        /// The string constant used in calls to <see cref="QryTableKeys"/> as the 
        /// zero index of the string format.
        /// </summary>
        public const string FOREIGN_KEY_STRING = "FOREIGN KEY";

        public const string SCHEMA_NAME = "SchemaName";
        public const string TABLE_NAME = "TableName";
        public const string COLUMN_NAME = "ColumnName";
        public const string DATA_TYPE = "DataType";
        public const string LENGTH = "Length";
        public const string ORDINAL = "Ordinal";
        public const string IS_NULLABLE = "IsNullable";
        public const string DEFAULT_VALUE = "DefaultValue";
        public const string IS_IDENTITY = "IsIdentity";

        public const string PROC_NAME = "ProcName";
        public const string PARAM_NAME = "ParamName";
        public const string PROC_TEXT = "ProcText";

        public const string CountByTableName = @"
SELECT  COUNT(*)
FROM    INFORMATION_SCHEMA.TABLES
WHERE   TABLE_NAME = '{0}'";

        public const string CountBySchemaTableName = @"
SELECT  COUNT(*)
FROM    INFORMATION_SCHEMA.TABLES
WHERE   TABLE_NAME = '{0}'
        AND TABLE_SCHEMA = '{1}'";

        public const string TblSelectString = @"
SELECT  TABLE_SCHEMA + '.' + TABLE_NAME AS " + TABLE_NAME + @"
FROM    INFORMATION_SCHEMA.TABLES
WHERE   TABLE_NAME LIKE '%{0}%'";

        public const string ColSelectString = @"
SELECT  TABLE_NAME AS " + TABLE_NAME + @",
        COLUMN_NAME AS " + COLUMN_NAME + @",
        ORDINAL_POSITION AS [" + ORDINAL + @"],
        COLUMN_DEFAULT AS [" + DEFAULT_VALUE + @"],
        IS_NULLABLE AS " + IS_NULLABLE + @",
        DATA_TYPE AS [" + DATA_TYPE + @"],
        CHARACTER_MAXIMUM_LENGTH AS [" + LENGTH + @"]
FROM    INFORMATION_SCHEMA.COLUMNS
WHERE   COLUMN_NAME LIKE '%{0}%'
        AND TABLE_NAME LIKE '%{1}%'
        AND TABLE_SCHEMA LIKE '%{2}%'";

        public const string ProcPrintText = @"
SELECT  c.[text] AS " + PROC_TEXT + @",
        r.*
FROM    syscomments c
        JOIN sysobjects r ON c.id = r.id
        JOIN sys.schemas s ON s.schema_id = r.uid
WHERE   OBJECT_NAME(c.id) = '{0}'
        AND s.name = '{1}'
ORDER BY r.uid ,
        c.colid";

        public const string ProcPrintTextMsSql2K = @"
SELECT  c.[text] AS " + PROC_TEXT + @",
        r.*
FROM    syscomments c
        JOIN sysobjects r ON c.id = r.id
WHERE   OBJECT_NAME(c.id) = '{0}'
ORDER BY r.uid ,
        c.colid";

        public const string SelectProcedureString = @"
SELECT  OBJECT_NAME(id) AS " + PROC_NAME + @" ,
        [text] AS " + PROC_TEXT + @"
FROM    syscomments WITH ( NOLOCK )
WHERE   1 = 1"; //here to allow concat of filter list 

        public const string QryTableKeys = @"
  SELECT    col.COLUMN_NAME AS " + COLUMN_NAME + @",
            c.DATA_TYPE
            + ( CASE WHEN c.CHARACTER_MAXIMUM_LENGTH IS NULL THEN ''
                     ELSE '(' + CONVERT(VARCHAR, c.CHARACTER_MAXIMUM_LENGTH)
                          + ')'
                END ) AS [" + DATA_TYPE + @"]
  FROM      INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE col
            JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tbl ON tbl.TABLE_NAME = col.TABLE_NAME
                                                             AND tbl.CONSTRAINT_NAME = col.CONSTRAINT_NAME
                                                             AND col.TABLE_SCHEMA = tbl.TABLE_SCHEMA
            JOIN INFORMATION_SCHEMA.COLUMNS c ON c.TABLE_NAME = tbl.TABLE_NAME
                                                 AND c.COLUMN_NAME = col.COLUMN_NAME
                                                 AND c.TABLE_SCHEMA = tbl.TABLE_SCHEMA
  WHERE     tbl.CONSTRAINT_TYPE = '{0}'
            AND col.TABLE_NAME = '{1}'
            AND col.TABLE_SCHEMA = '{2}'
";

        public const string QryTableUniqueIndex = @"
SELECT  sc.name AS " + COLUMN_NAME + @"
FROM    dbo.sysobjects AS tbl
        INNER JOIN sysusers AS stbl ON stbl.uid = tbl.uid
        INNER JOIN dbo.sysindexes AS i ON ( i.indid > '0'
                                            AND i.indid < N'255'
                                            AND N'1' != INDEXPROPERTY(i.id,
                                                              i.name,
                                                              N'IsStatistics')
                                            AND N'1' != INDEXPROPERTY(i.id,
                                                              i.name,
                                                              N'IsHypothetical')
                                          )
                                          AND ( i.id = tbl.id )
        JOIN sysindexkeys sik ON sik.id = i.id
                                 AND sik.indid = i.indid
        JOIN syscolumns sc ON sc.id = sik.id
                              AND sc.colid = sik.colid
WHERE   tbl.[type] = 'U'
        AND tbl.name = '{0}'
        AND CAST(i.status & 2 AS BIT) = 1
ORDER BY sc.name ASC
";

        public const string QryTableStringTypes = @"
SELECT  COLUMN_NAME AS " + COLUMN_NAME + @"
FROM    INFORMATION_SCHEMA.COLUMNS
WHERE   TABLE_NAME = '{0}'
        AND DATA_TYPE IN ( 'char', 'date', 'datetime', 
                           'datetime2', 'varchar', 'text',
                           'nvarchar', 'nchar', 'uniqueidentifier', 
                           'nchar', 'xml', 'time' )";

        public const string QryTableVarCharTypesWithMinLen = @"
SELECT  s.name + '.' + tbl.name AS " + TABLE_NAME + @" ,
        sc.name AS " + COLUMN_NAME + @" ,
        ( CASE WHEN sc.[length] < 0 THEN 8000
               ELSE sc.[length]
          END ) AS [" + LENGTH + @"] ,
        sc.[colid] AS [" + ORDINAL + @"]
FROM    dbo.sysobjects AS tbl
        JOIN syscolumns sc ON sc.id = tbl.id
        JOIN sys.schemas s ON s.[schema_id] = tbl.[uid]
        JOIN sys.types TYP ON TYP.user_type_id = sc.xtype
WHERE   tbl.[type] = 'U'
        AND TYP.name IN ( 'nvarchar', 'varchar', 'text' )
        AND s.name + '.' + tbl.name = '{0}'
		AND (sc.[length] > {1} OR sc.[length] < 0)";


        public const string QryColumnComputed = @"
SELECT  name AS " + COLUMN_NAME + @"
FROM    syscolumns WITH ( NOLOCK )
WHERE   iscomputed = 1
        AND OBJECT_NAME(id) = '{0}'";

        public const string QryColumnAutoNum = @"
SELECT  name AS " + COLUMN_NAME + @"
FROM    syscolumns WITH ( NOLOCK )
WHERE   autoval IS NOT NULL
        AND OBJECT_NAME(id) = '{0}'";

        public const string ColSelectStringType = @"
SELECT  DATA_TYPE AS [" + DATA_TYPE + @"]
FROM    INFORMATION_SCHEMA.COLUMNS
WHERE   TABLE_NAME = '{0}'
        AND COLUMN_NAME = '{1}'";

        public const string QryAllColumnsBySchemaTable = QryDumpAllTableData + @"
        AND tbl.name = '{0}'
        AND s.name = '{1}'
ORDER BY sc.colid ASC";

        public const string QryDumpAllTableData = @"
SELECT  s.name AS " + SCHEMA_NAME + @" ,
        tbl.name AS " + TABLE_NAME + @" ,
        sc.name AS " + COLUMN_NAME + @" ,
        TYP.name AS " + DATA_TYPE + @" ,
        sc.[length] AS [" + LENGTH + @"] ,
        sc.[colid] AS [" + ORDINAL + @"] ,
        CASE WHEN sc.isnullable = 0 THEN 'False'
             ELSE 'True'
        END AS " + IS_NULLABLE + @",
        INFO_COL.COLUMN_DEFAULT AS " + DEFAULT_VALUE + @"
FROM    dbo.sysobjects AS tbl
        JOIN syscolumns sc ON sc.id = tbl.id
        JOIN sys.schemas s ON s.[schema_id] = tbl.[uid]
        JOIN sys.types TYP ON TYP.user_type_id = sc.xtype
		LEFT JOIN INFORMATION_SCHEMA.COLUMNS INFO_COL ON INFO_COL.TABLE_SCHEMA = s.name
                                                         AND INFO_COL.TABLE_NAME = tbl.name
                                                         AND INFO_COL.COLUMN_NAME = sc.name
WHERE   tbl.[type] = 'U'
        AND TYP.name NOT IN ('timestamp')";

        public const string QryColumnMaxLength = @"
SELECT  CHARACTER_MAXIMUM_LENGTH AS " + LENGTH + @"
FROM    INFORMATION_SCHEMA.COLUMNS
WHERE   TABLE_NAME = '{0}'
        AND COLUMN_NAME = '{1}'";

        public const string QryAllNonComputedColumns = @"
SELECT  name AS " + COLUMN_NAME + @"
FROM    syscolumns WITH ( NOLOCK )
WHERE   OBJECT_NAME(id) = '{0}'
        AND name NOT IN ( SELECT    name AS column_name
                          FROM      syscolumns WITH ( NOLOCK )
                          WHERE     iscomputed = 1
                                    AND OBJECT_NAME(id) = '{0}' )";

        public const string QryTableUniqueIndexNonKeysOnly = @"
SELECT  tbl.name AS " + TABLE_NAME + @" ,
        sc.name AS " + COLUMN_NAME + @"
FROM    dbo.sysobjects AS tbl
        INNER JOIN sysusers AS stbl ON stbl.uid = tbl.uid
        INNER JOIN dbo.sysindexes AS i ON ( i.indid > '0'
                                            AND i.indid < N'255'
                                            AND N'1' != INDEXPROPERTY(i.id,
                                                              i.name,
                                                              N'IsStatistics')
                                            AND N'1' != INDEXPROPERTY(i.id,
                                                              i.name,
                                                              N'IsHypothetical')
                                          )
                                          AND ( i.id = tbl.id )
        JOIN sysindexkeys sik ON sik.id = i.id
                                 AND sik.indid = i.indid
        JOIN syscolumns sc ON sc.id = sik.id
                              AND sc.colid = sik.colid
        LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE col ON col.COLUMN_NAME = sc.name
                                                              AND col.TABLE_NAME = tbl.name
        LEFT OUTER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS itbl ON itbl.TABLE_NAME = col.TABLE_NAME
                                                              AND itbl.CONSTRAINT_NAME = col.CONSTRAINT_NAME
WHERE   tbl.[type] = 'U'
        AND CAST(i.status & 2 AS BIT) = 1
        AND ISNULL(itbl.CONSTRAINT_TYPE, 'UNIQUE') = 'UNIQUE'
ORDER BY tbl.name ASC
";
        public const string QryIsTableSetIdentity = @"
DECLARE @IsId INT

SELECT  @IsId = COUNT(sc.is_identity)
FROM    sys.objects so
        JOIN sys.columns sc ON so.object_id = sc.object_id
WHERE   so.name = '{0}'
        AND sc.is_identity = 1
 
SELECT  ( CASE WHEN ( @IsId = 0 ) THEN 'false'
               WHEN ( @IsId >= 1 ) THEN 'true'
               ELSE 'false'
          END ) AS " + IS_IDENTITY;

        public const string ProcsAndParams = @"
SELECT  [PROCS].[name] AS " + PROC_NAME + @",
        [PARAMS].[name] AS " + PARAM_NAME + @"
FROM    sys.procedures AS [PROCS]
        JOIN sys.all_parameters AS [PARAMS] ON [PROCS].[object_id] = [PARAMS].[object_id]
ORDER BY [PROCS].[name] ,
        [PARAMS].[parameter_id]
";

        public const string FilterStatement = " and {0} not in ({1})";
        public const string AllowDiagrams = @"EXEC [{0}].dbo.sp_changedbowner @loginame = N'sa', @map = false ";
    }
}
