
Select TO_Date( '08/27/2010 06:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM') from dual

select TRUNC(TO_Date( '08/27/2010 06:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM')) from dual

SELECT TO_CHAR(1234, 'FM00000') FROM DUAL--zero fill

SELECT TO_CHAR('1234', 'FM00000') FROM DUAL--zero fill

select LENGTH(1234) from dual --length 

select TO_CHAR(SYSDATE,'YYYYMMDD') from dual --date format

select substr('123456789',1,8) from dual

select SUBSTR('....................',1,20 - LENGTH('stringLiteral')) || 'stringLiteral' from dual --pad string (20 is the length of the '....' string literal)

TO_Date( '08/27/2007 06:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM')

timestamp '2007-08-31 12:45:00'
NVL(employeeName, 'na') --use 'na' if employee name is null
/*very cool if select case statement, last value is the Case Else*/
DECODE(employeeNumber, 1452, "Roger Smith",
                        4478, "Jenny Revolver",
                        4411, "Apple Nius",
                        "unknown")
                        
/*
TOAD	
Shift+F9		Execute SQL Query
Ctrl+B			Auto-Comment focused line
Shift+Ctrl+F	Auto-Format highlight selection
F2				Toggle Dataview window
F4				Lookup table definition
F7				Close current tab and open a new blank tab in its place
F8				Toggle View SQL run history
F10				Toggle Context Menu 
Ctrl + Enter	Run statement ending in semicolon (same as F9)
Ctrl + .		Force context sensitive intellisense
Ctrl + M		Create script as string in VB6

*/