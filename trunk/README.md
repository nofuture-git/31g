### PowerShell scripts
---
The file contents of this location are all the various PowerShell scripts associated to the _NoFuture_ namespace.  These _.ps1_ files are more like PowerShell modules since they define many cmdlets and are not intended to be run ad hoc.

The expectation is that the entire _./Code/NoFuture.sln_ has been compiled with its resultant binaries located at _./Code/NoFuture/bin_.

The general manner of use is to open PowerShell (or PowerShell ISE), change directory this location and then run the _./start.ps1_.  This script will perform all the importation of all the other _.ps1_ scripts along with various other setup tasks.

#### Important note: 
Running the _./start.ps1_ will modify your machine's environment variables.  The variables _JAVA_HOME_ and _CLASSPATH_ will be either added or reassigned.  Furthermore, the _./start.ps1_ will add entries to your machine's _PATH_ environment variable.

#### Summary
These scripts are not intended to be run individually.  Their intent is to act as a convenience viewport into the _NoFuture_ namespace - only the _./start.ps1_ should be run once-per-console. None of this PowerShell functionality is _required_, again, its only intent is to _act upon_ the code below.

Upon running _./start.ps1_, the cmdlets which are added may be viewed with a call to _Get-MyFunctions_ cmdlet from within PowerShell.




