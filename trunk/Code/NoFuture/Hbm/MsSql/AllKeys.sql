    SELECT  tbl.TABLE_SCHEMA AS [schema_name] ,
            ( tbl.TABLE_SCHEMA + '.' + col.[TABLE_NAME] ) AS [table_name] ,
            ( tbl.TABLE_SCHEMA + '.' + col.[TABLE_NAME] + '.'
              + col.[COLUMN_NAME] ) AS [column_name] ,
            c.ORDINAL_POSITION AS [column_ordinal] ,
            ( tbl.TABLE_SCHEMA + '.' + col.[CONSTRAINT_NAME] ) AS [constraint_name]
    FROM    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE col
            JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tbl ON tbl.TABLE_NAME = col.TABLE_NAME
                                                             AND tbl.CONSTRAINT_NAME = col.CONSTRAINT_NAME
                                                             AND tbl.TABLE_SCHEMA = col.TABLE_SCHEMA
            JOIN INFORMATION_SCHEMA.COLUMNS c ON c.TABLE_NAME = tbl.TABLE_NAME
                                                 AND c.TABLE_SCHEMA = tbl.TABLE_SCHEMA
                                                 AND c.COLUMN_NAME = col.COLUMN_NAME
