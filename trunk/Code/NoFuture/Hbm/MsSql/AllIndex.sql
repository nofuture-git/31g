    SELECT  s.name AS [schema_name] ,
            ( s.name + '.' + obj.name ) AS [table_name] ,
            ( s.name + '.' + obj.name + '.' + c.name ) AS [column_name] ,
            c.colid AS [column_ordinal] ,
            ( s.name + '.' + indx.name ) AS [constraint_name]
    FROM    sysindexkeys indxKeys
            JOIN dbo.sysindexes indx ON indxKeys.id = indx.id
                                        AND indxKeys.indid = indx.indid
            JOIN dbo.sysobjects obj ON obj.id = indx.id
                                       AND obj.id = indxKeys.id
            JOIN syscolumns c ON c.id = obj.id
                                 AND c.colid = indxKeys.colid
            JOIN sys.schemas s ON s.[schema_id] = obj.[uid]
    WHERE   indx.name NOT IN ( SELECT   CONSTRAINT_NAME
                               FROM     INFORMATION_SCHEMA.KEY_COLUMN_USAGE )
            AND s.name != 'sys'