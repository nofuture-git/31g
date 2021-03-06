    SELECT  tc.TABLE_SCHEMA AS [schema_name] ,
            ( tc.TABLE_SCHEMA + '.' + k.TABLE_NAME ) AS [table_name] ,
            ( tc.TABLE_SCHEMA + '.' + k.TABLE_NAME + '.' + k.COLUMN_NAME ) AS [column_name] ,
            c.ORDINAL_POSITION AS [column_ordinal] ,
            ( tc.TABLE_SCHEMA + '.' + tc.CONSTRAINT_NAME ) AS [constraint_name] ,
            c.[DATA_TYPE] ,
            ( CASE WHEN c.CHARACTER_MAXIMUM_LENGTH IS NULL THEN -1
                   ELSE c.CHARACTER_MAXIMUM_LENGTH
              END ) AS [string_length]
    FROM    INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
            JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc ON k.TABLE_CATALOG = tc.TABLE_CATALOG
                                                            AND k.TABLE_SCHEMA = tc.TABLE_SCHEMA
                                                            AND k.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
            JOIN INFORMATION_SCHEMA.COLUMNS c ON k.TABLE_CATALOG = c.TABLE_CATALOG
                                                 AND k.TABLE_SCHEMA = c.TABLE_SCHEMA
                                                 AND k.COLUMN_NAME = c.COLUMN_NAME
                                                 AND k.TABLE_NAME = c.TABLE_NAME
    WHERE   tc.CONSTRAINT_TYPE = 'PRIMARY KEY'