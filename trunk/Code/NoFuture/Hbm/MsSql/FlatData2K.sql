	select 
		sch.name as [schema_name], 
		(sch.name + '.' + tbl.name) as [table_name], 
		(sch.name + '.' + tbl.name + '.' + col.name) as [column_name], 
		NULL as [constraint_name],
		(select [name] from systypes where xusertype = col.xusertype) as [data_type], 
		col.[length] as [string_length]
	from syscolumns col
	join sysobjects tbl
		on col.id = tbl.id
	join sysusers sch
		on sch.[uid] = tbl.[uid]
	where 
		col.iscomputed = 0
		and tbl.xtype = 'U' 
		and col.name not in 
		(
			select k.column_name
			from   information_schema.key_column_usage k 
			join information_schema.table_constraints tc 
				on k.table_catalog = tc.table_catalog 
				and k.table_schema = tc.table_schema 
				and k.constraint_name = tc.constraint_name 
		)
