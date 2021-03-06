SELECT  ( itbl.TABLE_SCHEMA + '.' + tbl.name ) AS [table_name] ,
        ( itbl.TABLE_SCHEMA + '.' + tbl.name + '.' + sc.name ) AS [column_name] ,
        ( itbl.TABLE_SCHEMA + '.' + i.name ) AS [constraint_name]
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
WHERE   tbl.type = 'U'
        AND CAST(i.status & 2 AS BIT) = 1
        AND ISNULL(itbl.CONSTRAINT_TYPE, 'UNIQUE') = 'UNIQUE'
ORDER BY tbl.name ASC