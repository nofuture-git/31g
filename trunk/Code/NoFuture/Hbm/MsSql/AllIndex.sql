DECLARE @tSqlLosesMindOnStrConcat AS TABLE
    (
      sname VARCHAR(256) ,
      objname VARCHAR(256) ,
      cname VARCHAR(256) ,
      ccolid INT ,
      indxname VARCHAR(256)
    );

WITH    cteConstraintName
          AS ( SELECT   CONSTRAINT_NAME
               FROM     INFORMATION_SCHEMA.KEY_COLUMN_USAGE
             )
    INSERT  INTO @tSqlLosesMindOnStrConcat
            ( sname ,
              objname ,
              cname ,
              ccolid ,
              indxname
            )
            SELECT  s.name AS sname ,
                    obj.name AS objname ,
                    c.name AS cname ,
                    c.colid AS ccolid ,
                    indx.name AS indxname
            FROM    sysindexkeys indxKeys
                    JOIN dbo.sysindexes indx ON indxKeys.id = indx.id
                                                AND indxKeys.indid = indx.indid
                                                AND indx.name NOT IN (
                                                SELECT  CONSTRAINT_NAME
                                                FROM    cteConstraintName )
                    JOIN dbo.sysobjects obj ON obj.id = indx.id
                                               AND obj.id = indxKeys.id
                    JOIN syscolumns c ON c.id = obj.id
                                         AND c.colid = indxKeys.colid
                    JOIN sys.schemas s ON s.[schema_id] = obj.[uid]
                                          AND s.name != 'sys';

SELECT  sname AS [schema_name] ,
        sname + '.' + objname AS [table_name] ,
        sname + '.' + objname + '.' + cname AS [column_name] ,
        ccolid AS [column_ordinal] ,
        sname + '.' + indxname AS [constraint_name]
FROM    @tSqlLosesMindOnStrConcat;