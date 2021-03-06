	select 
		sch.name as [schema_name], 
		(sch.name + '.' + tbl.name) as [table_name], 
		(sch.name + '.' + tbl.name + '.' + col.name) as [column_name], 
		NULL as [constraint_name],
		(select [name] from sys.types where user_type_id = col.system_type_id) as [data_type], 
		(case when col.max_length IS NULL then -1 
		 when col.max_length = -1 AND 
			(select [name] from sys.types where user_type_id = col.system_type_id) in (select 'varchar' 
																						union all 
																					 select 'nvarchar'
																					    union all
																					 select 'xml') then 8000
			else col.max_length end) as [string_length]
	from sys.columns col
	join sys.tables tbl
		on col.object_id = tbl.object_id
	join sys.schemas sch
		on sch.[schema_id] = tbl.[schema_id]
	where 
		col.is_computed = 0 
		and col.name not in 
		(
			select k.column_name
			from   information_schema.key_column_usage k 
			join information_schema.table_constraints tc 
				on k.table_catalog = tc.table_catalog 
				and k.table_schema = tc.table_schema 
				and k.constraint_name = tc.constraint_name
				and k.table_name = tbl.name
				and k.table_schema = sch.name
		)