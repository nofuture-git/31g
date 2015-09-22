using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoFuture.Hbm.DbQryContainers;
using NoFuture.Hbm.DbQryContainers.MetadataDump;
using NoFuture.Hbm.SortingContainers;

namespace NoFuture.Hbm
{
    public static class Sorting
    {
        public const StringComparison C = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// Singleton instances of each of the various DbContainers.
        /// </summary>
        /// <remarks>
        /// All instances herein are dependent on their respective <see cref="HbmBase.OutputPath"/>. 
        /// There is no functionality to actually construct these files.  Database connection was 
        /// deliberately omitted from everything in this assembly.
        /// The actual creation of the JSON files was left in Powershell simply because its so easy
        /// to do there.
        /// </remarks>
        public static class DbContainers
        {
            private static HbmContraints _fks = new HbmContraints();
            private static HbmPrimaryKeys _pks = new HbmPrimaryKeys();
            private static HbmAllKeys _allKeys = new HbmAllKeys();
            private static HbmAllIndex _allIndex = new HbmAllIndex();
            private static HbmAllColumns _allColumns = new HbmAllColumns();
            private static HbmAutoIncrement _autoIncrement = new HbmAutoIncrement();
            private static HbmFlatData _flatData = new HbmFlatData();
            private static HbmUniqueClusteredIdxNotPks _uqIdxNotPks = new HbmUniqueClusteredIdxNotPks();
            private static HbmStoredProcsAndParams _storedProcParams = new HbmStoredProcsAndParams();

            public static HbmContraints Fks { get { return _fks; } set { _fks = value; } }
            public static HbmPrimaryKeys Pks { get { return _pks; } set { _pks = value; } }
            public static HbmAllKeys AllKeys { get { return _allKeys; } set { _allKeys = value; } }
            public static HbmAllIndex AllIndex { get { return _allIndex; } set { _allIndex = value; } }
            public static HbmAllColumns AllColumns { get { return _allColumns; } set { _allColumns = value; } }
            public static HbmAutoIncrement AutoIncrement { get { return _autoIncrement; } set { _autoIncrement = value; } }
            public static HbmFlatData FlatData { get { return _flatData; } set { _flatData = value; } }
            public static HbmUniqueClusteredIdxNotPks UniqueClusteredIdxNotPks { get { return _uqIdxNotPks; } set { _uqIdxNotPks = value; } }
            public static HbmStoredProcsAndParams StoredProcsAndParams { get { return _storedProcParams; } set { _storedProcParams = value; } }
        }

        #region internal fields
        //singletons 
        internal static List<string> _noPkTables;
        internal static List<string> _allTablesWithPk;
        internal static List<string> _allStoredProcNames;
        internal static Dictionary<string, StoredProcMetadata> _allStoredProcMetadatas;
        internal static Dictionary<string, List<ColumnMetadata>> _distinctStoredProcReturnedData;

        //reporting containers
        internal static List<string> _timedOutProx = new List<string>();
        internal static List<string> _multiTableDsProx = new List<string>();
        internal static List<string> _badSyntaxProx = new List<string>();
        internal static List<string> _noDatasetReturnedProx = new List<string>();
        internal static List<string> _unknownErrorProx = new List<string>();
        internal static List<string> _killedProx = new List<string>();
        internal static List<string> _emptyDataset = new List<string>();

        internal static object[] rptLocks =
        {
            new object(), new object(), new object(), new object(), new object(),
            new object(), new object()
        };
        #endregion

        #region internal sorting
        internal static Dictionary<string, List<string>> GetHbmDistinctPks(string tableName)
        {
            return GetNameColumnNamesManifold(DbContainers.Pks, tableName);
        }

        internal static Dictionary<string, List<string>> GetHbmDistinctFks(string tableName)
        {
            return GetNameColumnNamesManifold(DbContainers.Fks, tableName);
        }

        internal static Dictionary<string, List<string>> GetNameColumnNamesManifold(HbmBase dataSource, string tablename)
        {
            var manifold = new Dictionary<string, List<string>>();
            var distinctNames =
                dataSource.Data.Where(pkj => string.Equals(pkj.table_name, tablename, C))
                    .Select(pkj => pkj.constraint_name)
                    .Distinct();

            foreach (var dpk in distinctNames)
            {
                var cols = dataSource.Data.Where(
                    pkj => string.Equals(pkj.table_name, tablename, C) && string.Equals(pkj.constraint_name, dpk, C))
                    .Select(pkj => pkj.column_name);
                manifold.Add(dpk, cols.ToList());
            }

            return manifold;
        }

        internal static Dictionary<string, List<string>> GetHbmFksWhichArePks(Dictionary<string, List<string>> pkManifold, Dictionary<string, List<string>> fkManifold)
        {
            var pkValues = new List<string>();
            foreach (var li in pkManifold.Values)
                pkValues.AddRange(li);

            var fkPkCols = new Dictionary<string, List<string>>();
            foreach (var fk in fkManifold.Keys)
            {
                var items = fkManifold[fk].Where(pkfk => pkValues.Any(pkv => string.Equals(pkv, pkfk, C))).ToList();
                
                if (items.Count <= 0)
                    continue;

                //look to resolve the case of our table having part of FK's table's PK 
                var fkColData = DbContainers.Fks.Data.Where(x => x.constraint_name == fk).ToList();
                if (fkColData.Count <= 0)
                {
                    //typically not the case so remain optimistic
                    fkPkCols.Add(fk, items);
                    continue;
                }
                    

                var uniqName = fkColData.Select(x => x.unique_constraint_name).Distinct().FirstOrDefault();
                if (uniqName == null)
                {
                    fkPkCols.Add(fk, items);
                    continue;
                }
                    

                var uniqNameAsPkdata = DbContainers.Pks.Data.Where(x => x.constraint_name == uniqName).ToList();
                if (uniqNameAsPkdata.Count <= 0)
                    uniqNameAsPkdata =
                        DbContainers.UniqueClusteredIdxNotPks.Data.Where(x => x.constraint_name == uniqName).ToList();

                if (uniqNameAsPkdata.Count <= 0)
                {
                    fkPkCols.Add(fk, items);
                    continue;
                }
                    

                var uniqNameAsTableName = uniqNameAsPkdata.Select(x => x.table_name).Distinct().FirstOrDefault();
                if (string.IsNullOrWhiteSpace(uniqNameAsTableName))
                {
                    fkPkCols.Add(fk, items);
                    continue;
                }
                    

                var uniqNameAsItsColumns = DbContainers.Pks.Data.Where(x => x.table_name == uniqNameAsTableName).ToList();
                if (uniqNameAsItsColumns.Count <= 0)
                    uniqNameAsItsColumns =
                        DbContainers.UniqueClusteredIdxNotPks.Data.Where(x => x.table_name == uniqNameAsTableName)
                            .ToList();

                if (uniqNameAsItsColumns.Count <= 0)
                {
                    fkPkCols.Add(fk, items);
                    continue;
                }

                //don't add something to our PK just cause we have part of FK's table's PK
                if (items.Count != uniqNameAsItsColumns.Count)
                    continue;

                //look up the PK of the 
                fkPkCols.Add(fk, items);
            }
            return fkPkCols;
        }

        internal static List<string> GetHbmFkNotInPksRemainder(Dictionary<string, List<string>> pkManifold, Dictionary<string, List<string>> fkPkCols)
        {
            if(fkPkCols == null)
                return new List<string>();

            //looking for Values in pkManifold which are not in fkPkCols Values
            var setDiff = pkManifold.SelectMany(x => x.Value).Except(fkPkCols.SelectMany(y => y.Value)).ToList();
            return setDiff;
        }

        internal static PkItem MakeIntersectTypeId(Dictionary<string, List<string>> fkPkCols, List<string> iFks, string tableName)
        {
            var keyProperties = new Dictionary<string, List<ColumnMetadata>>();
            //get all key-property
            foreach (var pkFkColumnName in iFks)
            {
                var iFkMatchingJson =
                    (DbContainers.AllColumns.Data.FirstOrDefault(
                        x =>
                            string.Equals(x.table_name, tableName, C) && string.Equals(x.column_name, pkFkColumnName, C)));
                if (iFkMatchingJson == null)
                    continue;

                if (!keyProperties.ContainsKey(pkFkColumnName))
                    keyProperties.Add(pkFkColumnName, new List<ColumnMetadata>());
                keyProperties[pkFkColumnName].Add(iFkMatchingJson);
            }

            var keyManyToOnes = new Dictionary<string, List<ColumnMetadata>>();
            //get all key-many-to-one
            foreach (var pkFkConstraintName in fkPkCols.Keys)
            {
                var sc =
                    DbContainers.Fks.Data.FirstOrDefault(
                        x =>
                            string.Equals(x.table_name, tableName) &&
                            string.Equals(x.constraint_name, pkFkConstraintName));
                if (sc == null)
                    continue;
                var matchedTableEntry =
                    DbContainers.Pks.Data.FirstOrDefault(x => string.Equals(x.constraint_name, sc.unique_constraint_name));
                //try again on the all-keys json
                if (matchedTableEntry == null)
                    matchedTableEntry =
                        DbContainers.AllKeys.Data.FirstOrDefault(x => string.Equals(x.constraint_name, sc.unique_constraint_name));
                //still missing then get it from index-names
                if (matchedTableEntry == null)
                    matchedTableEntry =
                        DbContainers.AllIndex.Data.FirstOrDefault(x => string.Equals(x.constraint_name, sc.unique_constraint_name));

                if (matchedTableEntry == null)
                    continue;
                //database axiom 4.) a foreign-key, in addition to its table, may reference only one seperate table.
                var matchedTable = matchedTableEntry.table_name;
                //intend to use the table_name to which the FK references with value as manifold of columns on this table
                var fkPkColsJson = new List<ColumnMetadata>();
                foreach (var rematchedColName in fkPkCols[pkFkConstraintName])
                {
                    foreach (var columnJsonData in DbContainers.Fks.Data.Where(
                        x =>
                            string.Equals(x.table_name, tableName, C) &&
                            string.Equals(x.column_name, rematchedColName, C) &&
                            string.Equals(x.constraint_name, pkFkConstraintName, C)))
                    {
                        columnJsonData.constraint_name = pkFkConstraintName;
                        fkPkColsJson.Add(columnJsonData);
                    }
                }

                if(!keyManyToOnes.ContainsKey(matchedTable))
                    keyManyToOnes.Add(matchedTable, new List<ColumnMetadata>());

                keyManyToOnes[matchedTable].AddRange(fkPkColsJson);
            }

            var keys = new PkItem();
            if (keyProperties.Keys.Count > 0)
                keys.KeyProperty = keyProperties;
            if (keyManyToOnes.Keys.Count > 0)
                keys.KeyManyToOne = keyManyToOnes;
            return keys;
        }

        internal static Dictionary<string, List<string>> GetHbmFksNoRelToPks(Dictionary<string, List<string>> fkManifold,
            Dictionary<string, PkItem> keys, string tablename)
        {
            var pkItem = keys[tablename];
            if (pkItem == null)
                return fkManifold;
            if (pkItem.Id != null)
                return fkManifold;
            if (pkItem.KeyManyToOne == null)
                return fkManifold;

            var searchPkFkHash = pkItem.KeyManyToOne;
            var pkfkConstraintNames = searchPkFkHash.SelectMany(x => x.Value).Select(y => y.constraint_name).ToList();
            return fkManifold.Keys.Where(fk => !pkfkConstraintNames.Contains(fk))
                .ToDictionary(fk => fk, fk => fkManifold[fk]);
        }

        internal static ColumnMetadata GetFromAllColumnMetadata(ColumnMetadata cm)
        {
            if (cm == null || string.IsNullOrWhiteSpace(cm.table_name) || string.IsNullOrWhiteSpace(cm.column_name))
                return null;

            var acCol = DbContainers.AllColumns.Data.FirstOrDefault(
                                x => string.Equals(x.table_name, cm.table_name, StringComparison.OrdinalIgnoreCase) && 
                                     string.Equals(x.column_name, cm.column_name, StringComparison.OrdinalIgnoreCase));

            return acCol;
        }
        #endregion

        #region API
        /// <summary>
        /// Generates JSON formatted file whose contents are of form and fit to id nodes of an hbm.xml file.
        /// This is the first of the three hbm sorting cmdlets that should
        /// be called.  
        /// 
        /// The effort of this cmdlet is to determine, of all 
        /// tables having a primary key, if a table is of a simple identity 
        /// type or of a composite type.  Furthermore, of the composite types
        /// what portion of the tables Foreign-Keys form apart of this 
        /// composite primary key.
        /// </summary>
        /// <returns></returns>
        public static SortedKeys GetHbmDbPkData()
        {
            Settings.LoadOutputPathCurrentSettings();
            var pkJson = DbContainers.Pks.Data;
            var aidJson = DbContainers.AutoIncrement.Data;

            //get table names
            var allTables = pkJson.Select(x => x.table_name).Distinct();
            var singlePkTables = new List<string>();
            var compositePkTables = new List<string>();

            var keys = new Dictionary<string, PkItem>();
            //divide tables between composite and singular pks
            foreach (var tbl in allTables)
            {
                var pkColCount = pkJson.Count(x => string.Equals(x.table_name, tbl, C));
                if (pkColCount > 1)
                    compositePkTables.Add(tbl);
                else
                    singlePkTables.Add(tbl);
            }

            //divide composite pks between primitive keys and class-type keys
            foreach (var tbl in compositePkTables)
            {
                //get pks as a manifold of resident columns
                var pkManifold = GetHbmDistinctPks(tbl);
                //get fks as manifold of resident columns
                var fkManifold = GetHbmDistinctFks(tbl);
                //find the FKs which are PKs
                var fkPkCols = GetHbmFksWhichArePks(pkManifold, fkManifold);
                //get whatever independent FKs which remain, not being part of the PK
                var iFks = GetHbmFkNotInPksRemainder(pkManifold, fkPkCols);

                var compKeys = MakeIntersectTypeId(fkPkCols, iFks, tbl);

                keys.Add(tbl, compKeys);
            }

            foreach (var tbl in singlePkTables)
            {
                var simpleKeys = new PkItem();
                var pkJsonTblValue = pkJson.First(x => string.Equals(x.table_name, tbl, C));
                var isAutoIncrement = aidJson.Any(x => string.Equals(x.table_name, pkJsonTblValue.table_name, C));
                pkJsonTblValue.is_auto_increment = isAutoIncrement;
                simpleKeys.Id = pkJsonTblValue;

                keys.Add(tbl, simpleKeys);
            }

            //this performs the write to disk
            return new SortedKeys { Data = keys };
        }

        /// <summary>
        /// Generates JSON formatted file whose contents are of form and fit to many-to-one nodes of an hbm.xml file.
        /// This is the second of the three hbm sorting cmdlets that should
        /// be called.  
        ///     
        /// The effort of this cmdlet is to determine, of all 
        /// tables having a primary key, what this tables foreign keys
        /// are which were not already accounted for as part of a 
        /// composite-id primary key by the former cmdlet.
        /// </summary>
        /// <param name="sortedKeys">The output of GetHbmDbPkData</param>
        /// <returns></returns>
        public static SortedOneToMany GetHbmDbFkData(SortedKeys sortedKeys)
        {
            var pkItems = sortedKeys.Data;

            //get load/re-load psobjects to have output paths matching current settings
            Settings.LoadOutputPathCurrentSettings();

            //get needed json
            var cnJson = DbContainers.Fks.Data;
            var pkJson = DbContainers.Pks.Data;
            var akJson = DbContainers.AllKeys.Data;
            var aiJson = DbContainers.AllIndex.Data;
            var colJson = DbContainers.AllColumns.Data;

            //get a list of all the tables having a primary key
            var fks = new Dictionary<string, FkItem>();
            var allTables = pkJson.Select(x => x.table_name).Distinct();

            //proceed through each table
            foreach (var tbl in allTables)
            {
                var fkManifold = GetHbmDistinctFks(tbl);
                var iFkManifold = GetHbmFksNoRelToPks(fkManifold, pkItems, tbl);

                //produce output that is not already represented in the pk json hash
                if (iFkManifold == null || iFkManifold.Keys.Count <= 0)
                    continue;
                foreach (var iFkKeyValue in iFkManifold.Keys)
                {
                    //by constraint_name find the json entry for this table 
                    var residentTableJsonData =
                        cnJson.First(
                            x => string.Equals(x.table_name, tbl, C) && string.Equals(x.constraint_name, iFkKeyValue, C));
                    //from json data entry discover the matching unique_constraint_name
                    var fkConstraintName = residentTableJsonData.unique_constraint_name;

                    //find the table-name this FK's unique_constraint_name is referring to
                    var matchedTable = akJson.FirstOrDefault(x => string.Equals(x.constraint_name, fkConstraintName)) ??
                                       aiJson.First(x => string.Equals(x.constraint_name, fkConstraintName));
                    var matchedTableName = matchedTable.table_name;

                    //get all the columns, not just first
                    var matchedFkColumns =
                        cnJson.Where(
                            x => string.Equals(x.table_name, tbl, C) && string.Equals(x.constraint_name, iFkKeyValue, C))
                            .ToList();

                    foreach(var matchCol in matchedFkColumns)
                        matchCol.CopyFromDataSrc(colJson);

                    if (!fks.ContainsKey(tbl))
                    {
                        var fkItem = new FkItem
                        {
                            ManyToOne = new Dictionary<string, List<ColumnMetadata>>
                            {
                                {matchedTableName, matchedFkColumns}
                            }
                        };
                        fks.Add(tbl, fkItem);
                    }
                    else if (!fks[tbl].ManyToOne.ContainsKey(matchedTableName))
                    {
                        fks[tbl].ManyToOne.Add(matchedTableName, matchedFkColumns);
                    }
                    else
                    {
                        fks[tbl].ManyToOne[matchedTableName].AddRange(matchedFkColumns);
                    }
                }
            }
            return new SortedOneToMany() {Data = fks};
        }

        /// <summary>
        /// Generates JSON formatted file whose contents are of form and fit to bag/list/set nodes of an hbm.xml file.
        /// 
        /// This is the third of the three hbm sorting cmdlets that should
        /// be called.  
        /// 
        /// The effort of this cmdlet is to determine, given all columns,
        /// which columns are acting as or have a reference back to this 
        /// given table.  With ORM a relationship is duplex in that the
        /// parent must HAVE-A reference to the child and, likewise, the
        /// child must have a reference to the parent.  In so, this cmdlet
        /// is gathering to this table all other tables to which it plays
        /// the role of parent.  The role of child was captured already 
        /// by the latter cmdlets.
        /// </summary>
        /// <returns></returns>
        public static SortedBags GetHbmDbSubsequentData()
        {
            Settings.LoadOutputPathCurrentSettings();
            var cnJson = DbContainers.Fks.Data;
            var akJson = DbContainers.AllKeys.Data;
            var aiJson = DbContainers.AllIndex.Data;
            var colJson = DbContainers.AllColumns.Data;

            //group the unique_constraint_names into a distinct list
            var distinctUq = cnJson.Select(x => x.unique_constraint_name).Distinct();

            //get a hash to map unique constraint names to thier respective tables
            var distinctUqTables = new Dictionary<string, ColumnMetadata>();
            foreach (var uqConstraint in distinctUq)
            {
                //if the unique constraint name is not a key then it must be an index
                var uniqueConstraintTable = akJson.FirstOrDefault(x => string.Equals(x.constraint_name, uqConstraint, C)) ??
                                            aiJson.First(x => string.Equals(x.constraint_name, uqConstraint, C));

                distinctUqTables.Add(uqConstraint, uniqueConstraintTable);
            }

            //get a hashtable having the owning table/type as key and an array of constraints to which are ref'ing it
            var hbmHashBags = new Dictionary<string, List<ColumnMetadata>>();
            foreach (var uq in distinctUqTables.Keys)
            {
                var hbmBagOwner = distinctUqTables[uq];

                //go through the constraint list for each unique constraint
                var hbmBags = cnJson.Where(x => string.Equals(x.unique_constraint_name, uq, C)).ToList();

                //determine with each if the column is nullable or not
                foreach (var bag in hbmBags)
                    bag.CopyFromDataSrc(colJson);
                if (hbmHashBags.ContainsKey(hbmBagOwner.table_name))
                    hbmHashBags[(hbmBagOwner.table_name)].AddRange(hbmBags);
                else
                    hbmHashBags.Add(hbmBagOwner.table_name, hbmBags);
            }

            return new SortedBags() {Data = hbmHashBags};
        }

        /// <summary>
        /// Returns a list of all stored procs as an instance of <see cref="StoredProcMetadata"/>
        /// </summary>
        public static Dictionary<string, StoredProcMetadata> AllStoredProx
        {
            get
            {
                if (_allStoredProcMetadatas != null && _allStoredProcMetadatas.Count > 0)
                    return _allStoredProcMetadatas;
                if (!File.Exists(DbContainers.StoredProcsAndParams.OutputPath))
                    return new Dictionary<string, StoredProcMetadata>();

                _allStoredProcMetadatas = new Dictionary<string, StoredProcMetadata>();
                foreach (var procName in AllStoredProcNames)
                {
                    if (_allStoredProcMetadatas.ContainsKey(procName))
                        continue;
                    var procMetadata = new StoredProcMetadata(procName)
                    {
                        Parameters =
                            DbContainers.StoredProcsAndParams.Data.Where(
                                x => string.Equals(x.ProcName, procName, C)).ToList()
                    };
                    _allStoredProcMetadatas.Add(procName, procMetadata);
                }
                Settings.WriteToStoredProcLog("Construction of AllStoredProc complete");
                return _allStoredProcMetadatas;
            }
        }
        #endregion

        #region reporting
        /// <summary>
        /// Returns the names of all tables which have a primary key.
        /// NOTE: this does not include view nor User-defined table types.
        /// </summary>
        public static List<string> AllTablesWithPkNames
        {
            get
            {
                if (_allTablesWithPk != null && _allTablesWithPk.Count > 0)
                    return _allTablesWithPk;
                if (!File.Exists(DbContainers.Pks.OutputPath))
                    return new List<string>();//return something for subsequent calls
                _allTablesWithPk = DbContainers.Pks.Data.Select(x => x.table_name).Distinct().ToList();
                _allTablesWithPk.Sort();
                return _allTablesWithPk;
            }
        }

        /// <summary>
        /// Returns the names of all tables which have no primary key.
        /// </summary>
        public static List<string> NoPkTablesNames
        {
            get
            {
                if (_noPkTables != null && _noPkTables.Count > 0)
                    return _noPkTables;
                if (!File.Exists(DbContainers.AllColumns.OutputPath))
                    return new List<string>();//return something for subsequent calls

                var alltables = DbContainers.AllColumns.Data.Select(x => x.table_name).Distinct().ToList();
                var allPkTables = AllTablesWithPkNames;
                _noPkTables = alltables.Select(x => x).Except(allPkTables.Select(y => y)).ToList();
                _noPkTables.Sort();
                return _noPkTables;
            }
        }

        /// <summary>
        /// Returns a list of all the stored proc's names.
        /// </summary>
        public static List<string> AllStoredProcNames
        {
            get
            {
                if (_allStoredProcNames != null && _allStoredProcNames.Count > 0)
                    return _allStoredProcNames;
                if (!File.Exists(DbContainers.StoredProcsAndParams.OutputPath))
                    return new List<string>();//empty list
                _allStoredProcNames = DbContainers.StoredProcsAndParams.Data.Select(x => x.procName).Distinct().ToList(); 
                _allStoredProcNames.Sort();
                return _allStoredProcNames;
            }
        }

        /// <summary>
        /// This is a container to store info regarding stored prox which timeout out.
        /// This only contains timeouts issued by SQL Server.
        /// </summary>
        public static List<string> TimedOutProx
        {
            get
            {
                var mylock = rptLocks[0];
                lock (mylock)
                {
                    return _timedOutProx;    
                }
                
            }
        }

        /// <summary>
        /// This is a list of stored prox which did not timeout as expected and 
        /// were subsequently shutdown as an autonomous process.
        /// </summary>
        public static List<string> KilledProx
        {
            get
            {
                var mylock = rptLocks[5];
                lock (mylock)
                {
                    return _killedProx;
                }
            }
        }

        /// <summary>
        /// This is a container to store info regarding stored prox which return datasets containing
        /// multiple tables.  NHibernate does not invoke <see cref="System.Data.IDataReader.NextResult"/>
        /// so these other tables cannot be used.
        /// </summary>
        public static List<string> MultiTableDsProx
        {
            get
            {
                var mylock = rptLocks[1];
                lock (mylock)
                {
                    return _multiTableDsProx;                    
                }
                
            }
        }

        /// <summary>
        /// This is a container to store info regarding stored prox which have bad syntax.
        /// </summary>
        public static List<string> BadSyntaxProx
        {
            get
            {
                var mylock = rptLocks[2];
                lock (mylock)
                {
                    return _badSyntaxProx;                    
                }

            }
        }

        /// <summary>
        /// This is a container to store the names of procs which do not return
        /// any dataset.
        /// </summary>
        public static List<string> NoDatasetReturnedProx
        {

            get
            {
                var mylock = rptLocks[3];
                lock (mylock)
                {
                    return _noDatasetReturnedProx;
                }
            }
        }

        /// <summary>
        /// A container for stored proc names which errored out for some 
        /// reason other the timeouts and bad syntax.
        /// </summary>
        public static List<string> UnknownErrorProx
        {
            get
            {
                var myLock = rptLocks[4];
                lock (myLock)
                {
                    return _unknownErrorProx;
                }
            }
        }

        public static List<string> EmptyDatasetProx
        {
            get
            {
                var myLock = rptLocks[6];
                lock (myLock)
                {
                    return _emptyDataset;
                }
            }
        }

        /// <summary>
        /// This is expected to discover tables which simply cannot be used 
        /// in an ORM fashion.  The <see cref="NoFuture.Hbm.Mapping.GetSingleHbmXml"/>
        /// will attempt to derive a viable PK from non-nullable columns or
        /// Unique Non-Clustered Index, but failing both of these the table is 
        /// little more than a spreadsheet and cannot be used.
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static bool IsNoPkAndAllNonNullable(string tablename)
        {
            if (AllTablesWithPkNames.Contains(tablename))
                return false;
            var tablenamesCols = DbContainers.AllColumns.Data.Where(x => string.Equals(x.table_name, tablename, Sorting.C)).ToList();
            var hasAtLeastOneNonNullable = tablenamesCols.Any(x => x.is_nullable != null && x.is_nullable == false);
            var hasAtLeastOneUqIdx =
                DbContainers.UniqueClusteredIdxNotPks.Data.Any(x => string.Equals(x.table_name, tablename, C));

            return !hasAtLeastOneNonNullable && !hasAtLeastOneUqIdx;
        }
        #endregion

        #region sorting extension methods
        internal static void CopyFromDataSrc(this ColumnMetadata col, ColumnMetadata[] data)
        {
            var entry =
                data.FirstOrDefault(
                    x =>
                        string.Equals(x.table_name, col.table_name, C) &&
                        string.Equals(x.column_name, col.column_name, C));

            if (entry == null)
                return;

            col.CopyFrom(entry);
        }
        #endregion
    }
}
