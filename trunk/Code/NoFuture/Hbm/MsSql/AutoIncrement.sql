SELECT  ss.[name] AS [schema_name] ,
        ( ss.[name] + '.' + so.[name] ) AS [table_name] ,
        CASE WHEN COUNT(sc.is_identity) = 1 THEN 'true'
             ELSE 'false'
        END AS [is_auto_increment]
FROM    sys.objects so
        JOIN sys.columns sc ON so.object_id = sc.object_id
        JOIN sys.schemas ss ON so.schema_id = ss.schema_id
WHERE   sc.is_identity = 1
GROUP BY ss.[name] ,
        ( ss.[name] + '.' + so.[name] )
ORDER BY ss.[name]