select 
	ss.[name] as [schema_name],
	(ss.[name] + '.' + so.[name]) as [table_name], 
	count(sc.autoval)
from sysobjects so
join syscolumns sc
on so.id = sc.id
join sysusers ss
on so.uid = ss.uid
where sc.autoVal is not null
group by ss.[name], (ss.[name] + '.' + so.[name])
order by ss.[name]
