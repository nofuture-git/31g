    SELECT  tc.[TABLE_SCHEMA] AS [schema_name] ,
            ( tc.[TABLE_SCHEMA] + '.' + k.[TABLE_NAME] ) AS [table_name] ,
            ( tc.[TABLE_SCHEMA] + '.' + k.[TABLE_NAME] + '.' + k.[COLUMN_NAME] ) AS [column_name] ,
            c.ORDINAL_POSITION AS [column_ordinal] ,
            ( tc.[TABLE_SCHEMA] + '.' + k.[CONSTRAINT_NAME] ) AS [constraint_name] ,
            ( rc.[UNIQUE_CONSTRAINT_SCHEMA] + '.'
              + rc.[UNIQUE_CONSTRAINT_NAME] ) AS [unique_constraint_name] ,
            rc.[UNIQUE_CONSTRAINT_SCHEMA]
    FROM    INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
            JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc ON k.TABLE_CATALOG = tc.TABLE_CATALOG
                                                            AND k.TABLE_SCHEMA = tc.TABLE_SCHEMA
                                                            AND k.TABLE_NAME = tc.TABLE_NAME
                                                            AND k.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
            JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc ON ( rc.CONSTRAINT_SCHEMA
                                                              + '.'
                                                              + rc.CONSTRAINT_NAME ) = ( tc.CONSTRAINT_SCHEMA
                                                              + '.'
                                                              + tc.CONSTRAINT_NAME )
            JOIN INFORMATION_SCHEMA.COLUMNS c ON c.TABLE_SCHEMA = tc.TABLE_SCHEMA
                                                 AND c.TABLE_NAME = k.TABLE_NAME
                                                 AND c.COLUMN_NAME = k.COLUMN_NAME
    WHERE   tc.CONSTRAINT_TYPE = 'FOREIGN KEY'