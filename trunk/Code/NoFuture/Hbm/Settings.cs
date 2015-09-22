using System;
using System.IO;
using System.Collections.Generic;
using NoFuture.Exceptions;
using NoFuture.Shared;
using System.Linq;

namespace NoFuture.Hbm
{
    public class Settings
    {
        private static readonly List<string> _doNotReference = new List<string>();
        private static string _hbmStoredProxDir;

        //TODO replace with new Mutex(false, "Global\\uniquename"); [http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c]
        private static object myLock = new object();
        private static int _chxdoks = Globals.COMPILE_HBM_XML_DLL_OF_KB_SIZE;
        private static int _spXsdTimeout = 30;
        private static string _defaultSchemaName = Globals.DEFAULT_SCHEMA_NAME;

        private static readonly List<string> _sqlInjParamNames = new List<string>();

        public static string DefaultSchemaName { get { return _defaultSchemaName; } set { _defaultSchemaName = value; } }

        /// <summary>
        /// A global variable counterpart to 
        /// the search criteria <see cref="StoredProxSearchCriteria.SqlInjOnParamNamesLike"/>
        /// </summary>
        public static List<string> SqlInjParamNames { get { return _sqlInjParamNames; } }

        /// <summary>
        /// See summary on <see cref="Globals.COMPILE_HBM_XML_DLL_OF_KB_SIZE"/>
        /// </summary>
        public static int CompileHbmXmlDllOfKbSize
        {
            get { return _chxdoks; }
            set { _chxdoks = value; }
        }

        /// <summary>
        /// A special exclusion list of table names which should not be considered nor 
        /// added into any sorting function.  The values must match exactly so be sure to 
        /// include the schema qualifier.
        /// </summary>
        internal static List<string> DoNotReference 
        {
            get
            {
                return _doNotReference;
            } 
        }

        /// <summary>
        /// A context specific path directory which acts as the working directory of all NoFuture.Hbm and hbm.ps1
        /// functionality.
        /// </summary>
        public static string HbmDirectory
        {
            get
            {
                var baseDir = String.IsNullOrWhiteSpace(TempDirectories.Hbm)
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NoFutureData")
                    : TempDirectories.Hbm;

                var svrCatalogDir = String.Format(@"{0}\{1}", Constants.SqlServer, Constants.SqlCatalog);
                return Path.Combine(baseDir, svrCatalogDir);
            }
        }

        /// <summary>
        /// A sub directory to place all xsd's generated from invoking a stored proc.
        /// </summary>
        public static string HbmStoredProcsDirectory
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_hbmStoredProxDir))
                {
                    _hbmStoredProxDir = Path.Combine(HbmDirectory, Globals.STORED_PROX_FOLDER_NAME);
                }
                if (!Directory.Exists(_hbmStoredProxDir))
                    Directory.CreateDirectory(_hbmStoredProxDir);
                return _hbmStoredProxDir;
            }
            set { _hbmStoredProxDir = value; }
        }

        public static void WriteToStoredProcLog(string msg)
        {
            lock (myLock)
            {
                File.AppendAllText(Path.Combine(HbmStoredProcsDirectory, Globals.LOG_NAME),
                    String.Format("[{0:yyyy-MM-dd HH:mm:ss.fffff}] {1}\n", DateTime.Now, msg));
            }
        }

        public static void WriteToStoredProcLog(Exception ex, string storedProcName)
        {
            lock (myLock)
            {
                var msg = String.Join("\n", ex.Message, ex.StackTrace);
                File.AppendAllText(Path.Combine(HbmStoredProcsDirectory, Globals.LOG_NAME),
                    String.Format("[{0:yyyy-MM-dd HH:mm:ss.fffff}] {1}\n", DateTime.Now, msg));
            }
        }

        /// <summary>
        /// A value in whole-seconds to be used as a Timeout when invoking stored procs.
        /// NOTE: setting the command timeout does not work if the stored proc calls 'sp_executesql';
        ///       such a call, may in theory, last forever.
        /// </summary>
        public static int HbmStoredProcXsdTimeOut { get { return _spXsdTimeout; } set { _spXsdTimeout = value; }}

        /// <summary>
        /// Checks the directories are present and verifies the connection string is set.
        /// </summary>
        public static void LoadOutputPathCurrentSettings()
        {
            if(String.IsNullOrWhiteSpace(Constants.SqlServer) || String.IsNullOrWhiteSpace(Constants.SqlCatalog))
                throw new  RahRowRagee("Set the current connection string using the Mssql-Settings cmdlet.");

            if (!Directory.Exists(HbmDirectory))
            {
                Directory.CreateDirectory(HbmDirectory);
            }
            if (!Directory.Exists(HbmStoredProcsDirectory))
            {
                Directory.CreateDirectory(HbmStoredProcsDirectory);
            }
        }

        /// <summary>
        /// Helper function which adds all <see cref="DoNotReference"/> which are blobs
        /// to the <see cref="Sorting.NoPkTablesNames"/>
        /// </summary>
        /// <returns></returns>
        public static int AddNoPkAllNonNullableToBlockedNameList()
        {
            var noPkTables = Sorting.NoPkTablesNames;
            if (noPkTables.Count <= 0)
                return DoNotReference.Count;
            foreach (
                var tblName in
                    noPkTables.Where(Sorting.IsNoPkAndAllNonNullable)
                        .Where(tblName => !DoNotReference.Contains(tblName)))
            {
                DoNotReference.Add(tblName);
            }
            return DoNotReference.Count;
        }

        /// <summary>
        /// Operates to allow calling assembly to insert a name into the list of 
        /// tables which should not be referenced.
        /// </summary>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public static int AddToBlockedNameList(string tblName)
        {
            if (String.IsNullOrWhiteSpace(tblName) || DoNotReference.Contains(tblName))
                return DoNotReference.Count;

            DoNotReference.Add(tblName);
            return DoNotReference.Count;
        }

        /// <summary>
        /// Operates to allow calling assembly to insert many names into the list of 
        /// tables which should not be referenced.
        /// </summary>
        /// <param name="tblNames"></param>
        /// <returns></returns>
        public static int AddToBlockedNameList(List<string> tblNames)
        {
            if (tblNames == null || tblNames.Count <= 0)
                return DoNotReference.Count;
            foreach (var tbl in tblNames.Where(tblName => !DoNotReference.Contains(tblName)))
            {
                DoNotReference.Add(tbl);
            }
            return DoNotReference.Count;
        }

        /// <summary>
        /// Clears all table names from the internal list.
        /// </summary>
        public static void ClearBlockedNameList()
        {
            DoNotReference.Clear();
        }

        /// <summary>
        /// Operates in an opposite capacity to its <see cref="AddToBlockedNameList(string)"/>
        /// in that only the names presented in <see cref="tblNames"/> will remain available
        /// and every table name found in <see cref="Sorting.AllTablesWithPkNames." /> 
        /// and <see cref="Sorting"/>, not being found therein, will be added to the 
        /// blocked name list.
        /// </summary>
        /// <param name="tblNames">An exclusive list of table names by which all others are blocked.</param>
        /// <returns>The count of names present in the blocked names list.</returns>
        /// <remarks>
        /// The idea here is that some databases will actually have more tables you know you don't want
        /// than those that you do so its easier to just list those tables you do want.
        /// </remarks>
        public static int SetExclusiveNameList(string[] tblNames)
        {
            if (tblNames == null || tblNames.Length <= 0)
                return 0;

            var pkTbls = Sorting.AllTablesWithPkNames;
            var noPkTables = Sorting.NoPkTablesNames;
            foreach (var tbl in pkTbls.Where(t => !DoNotReference.Any(x => String.Equals(x, t, StringComparison.OrdinalIgnoreCase))
                                                  && !tblNames.Any(x => String.Equals(x, t, StringComparison.OrdinalIgnoreCase))))
            {
                var sameCaseTbl = pkTbls.FirstOrDefault(x => String.Equals(x, tbl, StringComparison.OrdinalIgnoreCase));
                if (sameCaseTbl == null)
                    continue;
                DoNotReference.Add(sameCaseTbl);
            }
            foreach (var tbl in noPkTables.Where(t => !DoNotReference.Any(x => String.Equals(x, t, StringComparison.OrdinalIgnoreCase))
                                                      && !tblNames.Any(x => String.Equals(x, t, StringComparison.OrdinalIgnoreCase))))
            {
                var sameCaseTbl = pkTbls.FirstOrDefault(x => String.Equals(x, tbl, StringComparison.OrdinalIgnoreCase));
                if (sameCaseTbl == null)
                    continue;
                DoNotReference.Add(sameCaseTbl);
            }

            return DoNotReference.Count;
        }

        /// <summary>
        /// For the calling assembly to know what is and isn't present therein.
        /// </summary>
        /// <returns></returns>
        public static string[] PrintBlockedNameList()
        {
            return DoNotReference.ToArray();
        }
    }
}
