SELECT  [sch].name AS [schemaName] ,
        [sch].name + '.' + REPLACE([procs].[name], '"', '') AS [procName] ,
        [params].[name] AS [paramName] ,
        [params].max_length AS [stringLength] ,
        CASE WHEN [params].has_default_value = 1 THEN 'true'
             ELSE 'false'
        END AS [isNullable] ,
        [params].parameter_id AS [ordinal] ,
        CASE WHEN [params].is_output = 1 THEN 'true'
             ELSE 'false'
        END AS [isOutput] ,
        CASE WHEN [types].is_user_defined = 1
                  AND [types].is_assembly_type = 0
                  AND [types].is_table_type = 0
             THEN [systypes].name COLLATE SQL_Latin1_General_CP1_CI_AS
             WHEN [types].is_assembly_type = 1
             THEN [asm_types].assembly_qualified_name
             WHEN [types].is_user_defined = 1
                  AND [types].is_assembly_type = 0
                  AND [types].is_table_type = 1
             THEN 'System.Data.DataTable, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
             ELSE [types].name
        END AS [datatype] ,
        CASE WHEN [types].is_user_defined = 1 THEN 'true' ELSE 'false' END AS [isUserDefinedType] ,
        CASE WHEN [types].is_assembly_type = 1
             THEN ( SELECT  [sch].[name] + '.' + [innerAsm_types].name
                    FROM    sys.assembly_types [innerAsm_types]
                            JOIN sys.schemas AS [sch] ON [innerAsm_types].[schema_id] = [sch].[schema_id]
                    WHERE   [asm_types].user_type_id = [innerAsm_types].user_type_id
                  )
             WHEN [types].is_user_defined = 1 THEN [sch].name + '.' + [types].name
             ELSE NULL
        END AS [sqlUdtTypeName]
FROM    sys.procedures AS [procs]
        JOIN sys.all_parameters AS [params] ON [procs].[object_id] = [params].[object_id]
        JOIN sys.types AS [types] ON [types].user_type_id = [params].user_type_id
        JOIN sys.schemas AS [sch] ON [procs].[schema_id] = [sch].[schema_id]
        LEFT JOIN sys.assembly_types AS [asm_types] ON [asm_types].user_type_id = [params].user_type_id
        LEFT JOIN sys.types [systypes] ON [systypes].system_type_id = [params].system_type_id
                                          AND [types].is_user_defined = 1
                                          AND [systypes].is_user_defined = 0
ORDER BY [procs].[name] ,
        [params].[parameter_id]

