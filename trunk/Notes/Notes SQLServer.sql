
/*==========
Joining - where -vs- on 
http://msdn.microsoft.com/en-us/library/ms177634%28v=sql.105%29.aspx
the predicates in the ON clause are applied to the table before 
the join, whereas the WHERE clause is semantically applied to the 
result of the join. 
  ==========*/

/*==========
Date Formatting
  ==========*/

PRINT '1) MON DD YYYY HH:MIAM (OR PM) FORMAT ==>' + 
CONVERT(CHAR(19),GETDATE())  
PRINT '2) MM-DD-YY FORMAT ==>' + 
CONVERT(CHAR(8),GETDATE(),10)  
PRINT '3) MM-DD-YYYY FORMAT ==>' + 
CONVERT(CHAR(10),GETDATE(),110) 
PRINT '4) DD MON YYYY FORMAT ==>' + 
CONVERT(CHAR(11),GETDATE(),106)
PRINT '5) DD MON YY FORMAT ==>' + 
CONVERT(CHAR(9),GETDATE(),6) 
PRINT '6) DD MON YYYY HH:MM:SS:MMM(24H) FORMAT ==>' + 
CONVERT(CHAR(24),GETDATE(),113)

SELECT DATEPART(year,GETDATE())  --GET JUST THE YEAR
SELECT DATEPART(month, GETDATE()) --MONTH
SELECT DATEPART(day, GETDATE())   --DAY

/*==========
Common string functions
  ==========*/
CHARINDEX
SUBSTRING
LEN
LTRIM
REPLACE
REPLICATE

/*==========
Flatten varchar rows to single item
  ==========*/
DECLARE @MY_TBL_VAR AS TABLE(
	i INT identity(1,1) not null Primary Key,
	[someText] VARCHAR(16)
)

INSERT INTO @MY_TBL_VAR (someText) VALUES ('Spanish')
INSERT INTO @MY_TBL_VAR (someText) VALUES ('English')
INSERT INTO @MY_TBL_VAR (someText) VALUES ('German')
INSERT INTO @MY_TBL_VAR (someText) VALUES ('Japanese')


SELECT DISTINCT SUBSTRING(
(
SELECT '; ' + someText AS [text()]
FROM @MY_TBL_VAR
ORDER BY someText
FOR XML PATH('')
), 2, 1000) AS [Bi-Lang]


/*==========
Variable Declaration
  ==========*/
DECLARE @myFirst int
DECLARE @mySecond int

--or
  
Declare @myDate datetime, 
		@myInt int, 
		@myVarchar varchar(50)
-- declaration in one statement is more efficient 

/*==========
Variable Assignment
  ==========*/
Set @myDate = GetDate()
Select @myInt = min(an_id) from myTable

--this is obviously the most efficient assignment syntax
select @myInt = column1, @myDate = column2 from myTable where ID = 12
--GOTCHA, if the select returns no rows the variable will
-- retain its value, NOT be set to NULL

/*==========
Debugging
  ==========*/
--use a boolean switch to include debug statements 
-- and avoid having to constantly comment out/in syntax

CREATE PROCEDURE MyProc (@debug = 0)
AS

INSERT INTO #SomeTemp (ID, PurchaseDate)
SELECT ID, PurchaseDate FROM Orders WHERE PurchaseDate => '2011-10-15 23:59:59.999'

IF @debug = 1
SELECT TOP 100 *
FROM #SomeTemp

--could be called as
EXEC MyProc
--or
EXEC MyProc 1 --will have the select statement execute

--write debug statments using PRINT
PRINT 'any ASCII text to send to client'

--format function like olskool C printf - this is considered an error by ssms
RAISERROR ('%d rows exist in table %s', 16, 1, @count, @tblName) WITH NOWAIT

/*==========
XML XPath Criteria
  ==========*/
SELECT * 
FROM MyTableWithXml
WHERE ColumnAsXmlType.value('(/standard/XpathRules/here)[1]','NVARCHAR(MAX)') LIKE '%some inner string%'

/*==========
Branching
  ==========*/
--example #1
select 
 case when (svc.seen_at =0) then app.home_city
	 when (svc.seen_at =1) then app.work_city 
	 when (svc.seen_at =2) then 
		(select walkinlocation.city
		from ofc_exa
		join
		walkinlocation with (nolock) on (ofc_exa.walkinlocationid = walkinlocation.walkinlocationid)
		where ofc_exa.ofc_exa_id = @examinerid)
	 else null
 end as city
from wwhs_svc svc

--example #2
select @exaGender = (case when gender is null then 1 
								when gender = 0 then 1
								else gender
					end)
   from ofc_exa with (nolock) where ofc_exa_id=@examinerId

--example #3
set @enddate = case when @lastDate < @StartDate + 7 then @lastDate else @StartDate + 7 end

--example #4
if(@myParam = 1)
	begin
		--do something
	end
else if(@myParam = 2)
	begin
		--something else
	end
else
	begin
		--default
	end

--example #5, Immediate IF
DECLARE @a int = 45, @b int = 40;
SELECT IIF ( @a > @b, 'TRUE', 'FALSE' ) AS Result;	
	
/*==========
Labels
  ==========*/
GOTO MY_LABEL
--label is designated by the name plus colon
MY_LABEL:

/*==========
String Manipulation
  ==========*/
--left justified
LEFT(convert(varchar,ofc.ADDRESS1,1) + replicate(' ',30-len(ofc.ADDRESS1)), 30) AS OfficeAddress1
--right justified
LEFT(replicate(' ',30-len(ofc.ADDRESS2)) + convert(varchar,ofc.ADDRESS2,1), 30) AS OfficeAddress1

/*==========
Batch Statements
  ==========*/
select * from myTable
select * from myOtherTable
GO
insert into myTable (myColumn) values ('123')
--GO is neither SQL nor T-SQL - its a client-thing
-- telling (probably SSMS or SqlCmd) to send the 
-- command to the server and return.

/*==========
Temp Table
  ==========*/
declare @temptbl table
(
	[starttime] datetime not null,
	[endtime] datetime not null,
	[city] varchar(50),
	[state] varchar(2),
	[zip] varchar(10),
	[description] varchar(30),
	[first_name] varchar(50),
	[last_name] varchar(50),
	[status] int,
	[note] varchar(1024)
)
create table #myInstanceTemp
(
	[myId] int identity(1,1) not null Primary Key,
	[myValue] varchar(32)
)

--push data to temp with literals 
insert into #myInstanceTemp (myValue)
select 'here is a value'
union all 
select 'here is the next'
union all
select 'and yet another'
union all
select 'etc...'

--when finished using call
drop table #myInstanceTemp

--use double-pound to make temp table global in scope
create table ##myGlobalTemp
(
	[myUID] int identity(1,1) not null primary key
	[someValue] varchar(32)
)

/*==========
Cursor - use these only when you need to go row-by-row
  ==========*/
declare @an_id int, @a_name varchar(50)

declare curMyCursor cursor read_only for --the 'read_only' improves performance
	select tbl.id, tbl.name
	from MyTable tbl
	where tbl.id > 4000
	
open curMyCursor

fetch next from curMyCursor
	into @an_id, @a_name
	
while @@fetch_status = 0
begin
	--your statements here
end
close curMyCursor
deallocate curMyCursor

--cursors are based on IBM's magnetic tape file system
-- hang the tape on the drive (ALLOCATE)
-- open it (OPEN)
-- get a record (FETCH)
-- close it (CLOSE)
-- remove the table (DEALLOCATE)

/*==========
Array Literal
  ==========*/

  -- this is the wrong mind-set for a procedural set-oriented data language
  -- but mildly useful
  
Declare @myVar varchar(800)--holds the literal values
Declare @currentCharIndex int--loop counter
Declare @seperator varchar(1)--list seperator
Declare @myTbl table--an array as a local table
(
	myVar varchar(80)
)

--the values tobe parsed
set @myVar = '1145,
              4521,
              4548,
              5481'

--counter default to have the loop entered at least once
set @currentCharIndex = 1

--assign the seperator
set @seperator = ','

--method to turn literal into a table
while(@currentCharIndex > 0)
Begin
	select @currentCharIndex = CHARINDEX(@seperator,@myVar)

	If(@currentCharIndex > 0)
	Begin
		insert into @myTbl (myVar) values (SUBSTRING(@myVar,0,@currentCharIndex))

		set @currentCharIndex = (@currentCharIndex + 1)
		set @myVar = (SUBSTRING(@myVar,@currentCharIndex,LEN(@myVar)))
		set @myVar = LTRIM(REPLACE(@myVar,0x0d0a,''))
	end 
	Else
		insert into @myTbl (myVar) values (@myVar)
End

Select * FROM @myTbl

/*==========
Looping - 
  ==========*/
declare @myCounter int
set @myCounter = 0

while(@myCounter < 10)
begin
	print (@myCounter)
	set @myCounter = (@myCounter + 1)
end	

--For-Loop style
DECLARE @LENGTH BIGINT 
		,@I BIGINT

SET @I = 1; --you must terminate this with a semicolor
            -- for the below 'WITH' statement to work

WITH INSERTEDINDEX --aka Common Table Expression
AS
(
	SELECT 
		 INSAGT.CompanyID
		,INSAGT.AgentID
		,ROW_NUMBER() OVER (
			ORDER BY INSAGT.CompanyID,INSAGT.AgentID
		) AS Enumeration
	FROM [rel].[CompanyAgent] INSAGT
	WHERE INSAGT.CompanyID = 11
)

SELECT II.CompanyID, II.AgentID, II.Enumeration
INTO #COMPANYAGENT
FROM INSERTEDINDEX AS II

SELECT @LENGTH = MAX(Enumeration) FROM #COMPANYAGENT
WHILE(@I <= @LENGTH)
BEGIN
	PRINT @I --do some loop logic 
	SET @I = @I + 1;
END

/*==========
Transactions 
  ==========*/
begin tran
	--perform some action
commit
--or
rollback
 
--transaction names limited to 32 characters
declare @myTransactionVariable varchar(32)
set @myTransactionVariable = 'myTransaction'
begin transaction @myTransactionVariable
 --perform action
commit @myTransactionVariable
--or 
rollback @myTransactionVariable

--make everything require a commit
set implicit_transactions on
/*
also set on ssms at Tools > 
					Options... > 
					Query Execution > 
					SQL Server > 
					ANSI > 
					SET IMPLICIT_TRANSACTIONS
*/

/*==========
Bulk Insert
  ==========*/
DECLARE @BulkInsertSql AS VARCHAR(200)
DECLARE  @FileName AS VARCHAR(100)

--there is some bug in SSMS and its handling of long file names - seems related to the number of '.' in the file name
SET @FileName = 'C:\Projects\db\definitionalData\Person.csv'

--default delimiter is tab (0x09)
SET @BulkInsertSql='BULK INSERT AdventureWorks2012.Person.Person FROM ''' + @FileName + '''
	WITH
	(
		FIRSTROW=2,
		ROWTERMINATOR = ''\n'',
		CODEPAGE = ''RAW''
	)' 
	
EXEC(@BulkInsertSql)	


/*==========
Error Handling
  ==========*/
--header
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO

--body, this is repeated for the number of actions needed
PRINT N'Dropping foreign keys from [admin].[State]'
GO
ALTER TABLE [admin].[State] DROP CONSTRAINT[FK_state_ref_country]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

PRINT N'Adding foreign keys to [admin].[State]'
GO
ALTER TABLE [admin].[State] WITH NOCHECK  ADD CONSTRAINT [FK_state_ref_country] FOREIGN KEY ([CountryCode]) REFERENCES [admin].[Country] ([Code])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

--trailer
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO

--t-sql TRY CATCH
BEGIN TRY

END TRY
BEGIN CATCH
	PRINT ERROR_MESSAGE()--GLOBAL FX 
END CATCH;

/*==========
Re-seeding an auto-increment IDENTITY column
  ==========*/
DBCC CHECKIDENT('myTableName',RESEED,11000007)

/*==========
Dynamic sql queries
  ==========*/
declare  @sql nvarchar(4000)
		,@outputVariable varchar(2024)
		,@tableName varchar(128)
		,@columnName varchar(128)
		,@whereStmt varchar(128)
		
set	@tablename = 'batchdatafiles'	
set @columnName = 'sourcePath'
set @whereStmt = 'batchjobid = 3'

	select @sql = ('select @output = ' + @columnName + ' from ' + @tableName + ' where ' +  @whereStmt)
	print (@sql)
	exec sp_executeSql @sql, N'@output VARCHAR(2024) OUTPUT ', @outputVariable OUTPUT
	print (@outputVariable)
	

-- grant execute permissions
-- this will produce the text that needs to be executed
DECLARE @storedProcName varchar(50)
	SET @storedProcName = 'p_IsExaminerCertified'
SELECT 'grant exec on ' + QUOTENAME(ROUTINE_SCHEMA) + '.' +
QUOTENAME(@storedProcName) + ' TO [dbo]' FROM INFORMATION_SCHEMA.ROUTINES
WHERE OBJECTPROPERTY(OBJECT_ID(@storedProcName),'IsMSShipped') = 0

/*==========
Logical Grouping
  ==========*/
--intended to capture complex boolean AND/OR propositions
--of inter-related values
declare @logicalTable table
(
	GroupingCode varchar(4)
	,PrimaryValue varchar(4)
	,RelatedValue2 varchar(4)
)

--constraint: no repeating permutation
insert into @logicalTable values ('0001','AAA','111')
insert into @logicalTable values ('0001','AAA','222')
insert into @logicalTable values ('0001','BBB','333')
insert into @logicalTable values ('0002','BBB','222')
insert into @logicalTable values ('0002','CCC','222')

--use the GroupCode to find relations as Logical AND
select * from @logicalTable where GroupingCode = '0001'

--use the PrimaryValue 
--w/o respect to the GroupingCode to find
--logical OR
select * from @logicalTable where PrimaryValue = 'BBB'


/*==========
Venn diagram results
 ==========*/
select column1, column2 from [ref].[MyTable]
except
select refcolumn1, refcolumn2 from [ref].[MyRefTable]

select column1, column2 from [ref].[MyTable]
intersect
select refcolumn1, refcolumn2 from [ref].[MyRefTable]

/*==========
Paging results
 ==========*/
SELECT TOP (10 /* this is the number-of-results-per-page */) 
                     mySub.ID 
                 ,mySub.FName 
                 ,mySub.LName 
                 ,mySub.SomeField 
                 ,mySub.AnotherField 
FROM   (SELECT  ID 
               ,FName 
               ,LName 
               ,SomeField 
               ,AnotherField 
               ROW_NUMBER() OVER(ORDER BY LName, FName) as _my_sort_row 
        FROM   [dbo].SomeTable 
        WHERE  ID in (123,456,789) 
        ) as mySub 
WHERE  mySub._my_sort_row > 10 /* this goes up by some increment of number-of-results-per-page per page click */ 
ORDER  BY mySub._my_sort_row; 


/*==========
Performance
  ==========*/
/* A table should always have a primary key */
/* A primary key may not be null */
/* use this to get details about performance
 - entries concerning time is related to CPU in general
 - entries concering io reads is related to disk usage in general */
set statistics time, io off
/* SSMS provides the 'Execution Plan' which prints a 
	a graphical diagram representing the sequence of 
	execution having one diagram per statement*/
/* avoid using 'union' and instead use 'union all' then 
   select from the resulting rows using 'distinct'*/
/* to improve performance use the statistics and io off */
/* commands and work from 'the-worst-performer' down */
/* expect full table scans to diminish performance 
   at an exponential rate. */
/* an index is permutation of columns - improve performance
   by supplying "something" to any values forerunning values */
/* there may be only one clusted PK/index per table */
/* a table scan is the worst performer while a 'RID Lookup' is 
   then next worst. */
/* all performance rules apply equally to temp tables and 
   table variables.*/
/* an index may not be supplyed to table variables.*/

/*==========
LOCKS
  ==========*/
/*
 - locks are defined by thier 
  - SCOPE
  - MODE
  - DURATION
 
 - Lock SCOPE:
  - row, a single row 
  - key, one part of index, the index being the 
         location of a key.
  - page: 8KB (Page)
  - Extent : 8 pages (64kb) 
             the size of MSSQL Reads and Writes
  - Table : entire table
  - Database : schema changes  

 - MODE
  - Share Lock, for Read-only 
   - allows for other Shared Locks and blocks
     Exclusive locks.
	 
  - Update Lock, a Shared lock converted to an exclusive
   - the shared lock is waiting for other shared locks to 
     clear, at which time, it is converted to an exclusive lock
	 
  - Exclusive Lock, for Insert\Update\Delete, 
   - exists as a singleton and blocks other exclusive locks.
   
  - Intend Lock, multipurpose, these are tied to time and 
                 are an expression of intent, an Update Lock 
				 is a kind of Intent Lock.
   - works toward performance as well via the construction 
     of a lock hierarchy (so the runtime doesn't have to 
	 scan all the locks to derive the current state).  
	 The hierarchy allows for immediate	propositions of 
	 current state of locks.  The state is a 
	 matter of the aggregate of all locks scope.
  
 - DURATION
  - how long, in time, will a lock be held
  - This is what Isolation Level is concerned with. 
   - Read Uncommitted: momentary exlusive lock while no other 
                       exclusives exist.
   - Read Committed: remains untill the time Commit is called.
   - Repeatable Read: remains as long as someone has run a SELECT
   - Serializable: is a repeatable read also having a range of keys
   
 Deadlock Victim
  - choosen by the Deadlock manager,
  - A husband and wife are at the mall and both
    know that they will meet in the food court when
	its time to leave - the husband is waiting for 
	a text from wife as signal to go to food court; 
	however, wife is also waiting for a text from 
	husband to do likewise.  Eventually one has 
	to call the other or they will be stuck in the mall.
                                                                              
   session A                    session B   
    |                              |     
   .--.      Table A  Table B     .--. 
   |  |        |        |         |  |
   |  |lock  .--.      .--. lock  |  |
   |  |----->|  |      |  |<------|  |
   |  |update|  |      |  | update|  |
   |  |----->|  |      |  |<------|  |
   |  |      |  |      |  |       |  |
   |  |      |__|      |  |       |  |
   |  |     /|  |\     |  |       |  |
   |  |____/ |  | \___>|  |       |  |
   |  |      |  | read |  |       |  | 
   |  |      |__|      |  |       |  | 
   |  |     /|  |\     |  |       |  | 
   |  |<___/ |  | \____|  |       |  | 
   |  | wait |  |      |  |       |  | 
   |  |      |  |      |__|       |  | 
   |  |      |  |     /|  |\      |  | 
   |  |      |  |<___/ |  | \_____|  | 
   |  |      |  |read  |  |       |  | 
   |  |      |  |      |__|       |  | 
   |  |      |  |     /|  |\      |  | 
   |  |      |  |____/ |  | \____>|. | 
   |  |      |  |      |  | wait  | .| 
   |  |      |  |      |  |       |  |. 
   .--.      .--.      .--.       .--. .
    |         |         |          |    .Dead Lock
    |         |         |          |                                            
    -         -         -          -                                            
   /*\       /*\       /*\        /*\                                           
    -         -         -          -                                                        
               
*/             

