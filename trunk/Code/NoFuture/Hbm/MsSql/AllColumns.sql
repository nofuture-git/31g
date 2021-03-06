    SELECT  sch.name AS [schema_name] ,
            ( sch.name + '.' + tbl.name ) AS [table_name] ,
            ( sch.name + '.' + tbl.name + '.' + col.name ) AS [column_name] ,
            NULL AS [constraint_name] ,
            CASE WHEN ( SELECT  tt.[name]
                        FROM    sys.types tt
                        WHERE   tt.user_type_id = col.system_type_id
                      ) IS NULL
                 THEN ( SELECT  tt.[name]
                        FROM    sys.types tt
                        WHERE   tt.user_type_id = col.user_type_id
                      )
                 ELSE ( SELECT  tt.[name]
                        FROM    sys.types tt
                        WHERE   tt.user_type_id = col.system_type_id
                      )
            END AS [data_type] ,
            ( CASE WHEN col.max_length IS NULL THEN -1
                   ELSE col.max_length
              END ) [string_length] ,
            CASE WHEN col.is_nullable = 1 THEN 'true'
                 ELSE 'false'
            END AS [is_nullable] ,
            CONVERT(VARCHAR(12), col.[precision]) + ','
            + CONVERT(VARCHAR(12), col.scale) AS [precision] ,
            col.*
    FROM    sys.columns col
            JOIN sys.tables tbl ON col.[object_id] = tbl.[object_id]
            JOIN sys.schemas sch ON sch.[schema_id] = tbl.[schema_id]