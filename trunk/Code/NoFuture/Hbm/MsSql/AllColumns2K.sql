	select 
		sch.name as [schema_name], 
		(sch.name + '.' + tbl.name) as [table_name], 
		(sch.name + '.' + tbl.name + '.' + col.name) as [column_name], 
		NULL as [constraint_name],
		(select [name] from systypes where xusertype = col.xusertype) as [data_type], 
		(case when col.[length] IS NULL then -1 else col.[length] end) [string_length],
		case when col.isnullable = 1 then 'true' else 'false' end as [is_nullable]
	from syscolumns col
	join sysobjects tbl
		on col.id = tbl.id
	join sysusers sch
		on sch.[uid] = tbl.[uid]
	where tbl.xtype = 'U'
