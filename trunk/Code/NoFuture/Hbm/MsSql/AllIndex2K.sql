	select 
		s.name as [schema_name], 
		(s.name + '.' + obj.name) as [table_name], 
		(s.name + '.' + obj.name + '.' + c.name) as [column_name],
		c.colid as [column_ordinal],
		(s.name + '.' + indx.name) as [constraint_name]
	from sysindexkeys indxKeys
	join dbo.sysindexes indx
		on indxKeys.id = indx.id
		and indxKeys.indid = indx.indid
	join dbo.sysobjects obj
		on obj.id = indx.id
		and obj.id = indxKeys.id
	join syscolumns c
		on c.id = obj.id
		and c.colid = indxKeys.colid
	join sysusers s
		on s.[uid] = obj.[uid]
	where indx.name not in 
	(
		select constraint_name from INFORMATION_SCHEMA.KEY_COLUMN_USAGE
	)
	and s.name != 'sys'
