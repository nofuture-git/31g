using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NoFuture.Exceptions;
using NoFuture.Hbm.DbQryContainers;
using NoFuture.Hbm.SortingContainers;

namespace NoFuture.Hbm
{
    public class Mapping
    {
        #region sorted containers
        private static SortedKeys _hbmKeys = new SortedKeys();
        private static SortedBags _hbmBags = new SortedBags();
        private static SortedOneToMany _hbmOneToMany = new SortedOneToMany();

        public static SortedKeys HbmKeys
        {
            get { return _hbmKeys; }
            set { _hbmKeys = value; }
        }
        public static SortedBags HbmBags
        {
            get { return _hbmBags; }
            set { _hbmBags = value; }
        }
        public static SortedOneToMany HbmOneToMany
        {
            get { return _hbmOneToMany; }
            set { _hbmOneToMany = value; }
        }

        private static readonly InvokeStoredProcManager _storedProcManager = new InvokeStoredProcManager();

        public static InvokeStoredProcManager StoredProcManager
        {
            get { return _storedProcManager; }
        }

        #endregion

        #region hbm xml generation

        /// <summary>
        /// Produces a single hbm.xml for the given <see cref="tbl"/>
        /// </summary>
        /// <param name="outputNamespace"></param>
        /// <param name="tbl"></param>
        /// <returns>The path the generated hbm.xml file</returns>
        public static string GetSingleHbmXml(string outputNamespace, string tbl)
        {
            if(HbmKeys == null || HbmOneToMany == null || HbmBags == null)
                throw new RahRowRagee("Assign the static variables HbmKeys, HbmOneToMany and HbmBags" +
                                      " then try again (These values are calculated from Sorting).");
            if (Settings.DoNotReference.Contains(tbl))
                return null;

            var hbmPk = HbmKeys.Data;
            var hbmFk = HbmOneToMany.Data;
            var hbmBags = HbmBags.Data;

            //possiable duplicate names handled within this
            var className = Compose.ClassName(tbl, outputNamespace);

            //not having the naming pattern is exceptional
            Compose.ValidSplit(tbl, 2);

            var tableName = Util.Etc.ExtractLastWholeWord(tbl, null);
            var schemaName = tbl.Replace(string.Format(".{0}", tableName), string.Empty);

            var xe = XeFactory.HibernateMappingNode();

            var classXe = XeFactory.ClassNode(className, tableName, schemaName);

            //----PK
            var pkId = hbmPk.ContainsKey(tbl) ? hbmPk[tbl].Id : null;
            var hasNoPkAtAll = !hbmPk.ContainsKey(tbl);

            if (pkId == null && !hasNoPkAtAll)
            {
                var compClassName = Compose.CompKeyClassName(tbl, outputNamespace);
                const string compPropertyName = Globals.HbmXmlNames.ID;
                var compKeyXe = XeFactory.CompositeIdNode(compPropertyName, compClassName);

                //key-many-to-one
                var hbmKeyManyToOne = hbmPk[tbl].KeyManyToOne ?? new Dictionary<string, List<ColumnMetadata>>();
                if (hbmKeyManyToOne.Count > 0)
                {
                    foreach (var keyname in hbmKeyManyToOne.Keys.Where(k => !Settings.DoNotReference.Contains(k)))
                    {
                        //need to iterate here on a per 'constraint_name', the json data-struct misses this
                        //eg a table, 4 columns; 2 are comp to another table's Pk and the other 2 are also a comp the SAME table's PK
                        var keyManytoOneColumns = new List<ColumnMetadata>();
                        if (!HbmKeys.GetKeyManyToOneColumns(tbl, ref keyManytoOneColumns))
                            continue;
                        foreach (var distinctColumn in keyManytoOneColumns.Distinct(new ConstraintNameComparer()))
                        {
                            var dKmtoCName = distinctColumn.constraint_name;
                            var keyMtoFullyQualTypeName = Compose.ClassName(keyname, outputNamespace);
                            var keyMtoNfTypeNameObj = new Util.TypeName(keyMtoFullyQualTypeName);
                            var keyMtoSimpleName = keyMtoNfTypeNameObj.ClassName;

                            var keyMtoXe = XeFactory.KeyManyToOneNode(keyMtoSimpleName,
                                keyMtoFullyQualTypeName);

                            var dKmtoColumnData = hbmKeyManyToOne[keyname].Where(
                                x =>
                                    string.Equals(x.constraint_name, dKmtoCName, Sorting.C)).ToList();

                            if (dKmtoColumnData.Count <= 0)
                                continue;

                            foreach (var kmtoColumn in dKmtoColumnData)
                            {
                                kmtoColumn.CopyFrom(Sorting.GetFromAllColumnMetadata(kmtoColumn));
                                var fullColumnName = kmtoColumn.column_name;

                                //not having the naming pattern is exceptional
                                Compose.ValidSplit(fullColumnName, 3);

                                var keyMtoColumnXe =
                                    XeFactory.ColumnNode(Util.Etc.ExtractLastWholeWord(fullColumnName, null), kmtoColumn.ToJsonString());

                                keyMtoXe.Add(keyMtoColumnXe);
                            }

                            compKeyXe.Add(keyMtoXe);
                        }//end foreach distinct constraintname
                    }
                }//end key-many-to-one

                var hbmKeyProperty = hbmPk[tbl].KeyProperty ?? new Dictionary<string, List<ColumnMetadata>>();

                //these would have been added as key-many-to-one, but thier underlying table has been excluded.
                foreach (var reduced in hbmKeyManyToOne.Where(k => Settings.DoNotReference.Contains(k.Key)))
                {
                    foreach (var redux in reduced.Value)
                        redux.CopyFrom(Sorting.GetFromAllColumnMetadata(redux));//insure they got everything for being a value type
                    
                    hbmKeyProperty.Add(reduced.Key, reduced.Value);
                }

                if (hbmKeyProperty.Count > 0)
                {
                    //Dictionary<string, List<ColumnMetadata>>
                    foreach (var keyName in hbmKeyProperty.Keys)
                    {
                        //SPECIAL NOTE: the original PS code seemed to imply that a key-property is always just one column...
                        var keyPropJsonData = hbmKeyProperty[keyName].FirstOrDefault();
                        if (keyPropJsonData == null)
                            continue;

                        keyPropJsonData.CopyFrom(Sorting.GetFromAllColumnMetadata(keyPropJsonData));
                        var keyPropPropertyName = Compose.PropertyName(keyPropJsonData.column_name);
                        var fullcolumnName = keyPropJsonData.column_name;

                        Compose.ValidSplit(fullcolumnName, 3);

                        var keyPropDataType = Util.Lexicon.Mssql2HbmTypes[(keyPropJsonData.data_type)];
                        var keyPropColumn = Util.Etc.ExtractLastWholeWord(fullcolumnName, null);
                        var keyPropLen = keyPropJsonData.string_length ?? Globals.MSSQL_MAX_VARCHAR;

                        if (string.Equals(keyPropDataType, Globals.HbmXmlNames.ANSI_STRING))
                        {
                            if (keyPropLen <= 0)
                                keyPropLen = Globals.MSSQL_MAX_VARCHAR;
                            var keyPropXe = XeFactory.KeyPropertyNode(keyPropPropertyName, keyPropColumn,
                                keyPropDataType, keyPropLen.ToString(CultureInfo.InvariantCulture), keyPropJsonData.ToJsonString());
                            compKeyXe.Add(keyPropXe);
                        }
                        else
                        {
                            var keyPropXe = XeFactory.KeyPropertyNode(keyPropPropertyName, keyPropColumn,
                                keyPropDataType, keyPropJsonData.ToJsonString());
                            compKeyXe.Add(keyPropXe);
                        }
                    }
                }
                classXe.Add(compKeyXe);
            }
            else if (pkId != null)
            {
                GetSimpleId(pkId, classXe);
            }//end PK

            //having no pk, add the contrived comp-key to the class
            if (hasNoPkAtAll)
            {
                GetAllColumnsAsCompositeKey(tbl, classXe, outputNamespace);
            }
            else//simple properties
            {
                foreach (var columnName in Sorting.DbContainers.FlatData.Data.Where(x => string.Equals(x.table_name, tbl, Sorting.C)))
                {
                    var fullColumnName = columnName.column_name;
                    Compose.ValidSplit(fullColumnName, 3);
                    var simplePropXe = GetSimplePropertyHbmXml(columnName, Globals.HbmXmlNames.PROPERTY);
                    classXe.Add(simplePropXe);
                }
            }

            //----fks which are not part of pks
            if (hbmFk.ContainsKey(tbl))
            {
                var tblFks = hbmFk[tbl].ManyToOne;
                foreach (var fkName in tblFks.Keys)
                {
                    //these would be FK ref to another type but its underlying table is excluded so now its just a bunch of value types
                    if (Settings.DoNotReference.Contains(fkName))
                    {
                        foreach (var reducedFk in tblFks[fkName])
                        {
                            var reducedFkSimpProp = GetSimplePropertyHbmXml(reducedFk,
                                Globals.HbmXmlNames.PROPERTY);
                            classXe.Add(reducedFkSimpProp);
                        }
                        continue;//these need representation but not as class types
                    }

                    var manytoOneColumns = new List<ColumnMetadata>();
                    if (!HbmOneToMany.GetManyToOneColumns(tbl, ref manytoOneColumns))
                        continue;

                    var fkColumnsByDistinctConstraintName = manytoOneColumns.Select(x => x.constraint_name).Distinct().ToList();
                    foreach (var distinctConstraintName in fkColumnsByDistinctConstraintName)
                    {
                        var dMtoColumnData = tblFks[fkName].Where(
                            x =>
                                string.Equals(x.constraint_name, distinctConstraintName, Sorting.C)).ToList();

                        if (dMtoColumnData.Count <= 0)
                            continue;
                        var fkColumnXes = new List<XElement>();
                        var fkColumnNames = new List<string>();
                        foreach (var x in dMtoColumnData)
                        {
                            x.CopyFrom(Sorting.GetFromAllColumnMetadata(x));
                            var fullColumnName = x.column_name;

                            Compose.ValidSplit(fullColumnName, 3);
                            var cn = Util.Etc.ExtractLastWholeWord(fullColumnName, null);

                            //need to store these temp, since we are also drafting thier parent's name
                            fkColumnXes.Add(XeFactory.ColumnNode(cn,x.ToJsonString()));
                            fkColumnNames.Add(cn);
                        }

                        var fkPropertyType = new Util.TypeName(Compose.ClassName(fkName, outputNamespace));
                        var fkPropertyName = Compose.ManyToOnePropertyName(Compose.ClassName(fkName, outputNamespace),
                            fkColumnNames.ToArray());

                        var manyToOneXe = XeFactory.ManyToOneNode(fkPropertyName, fkPropertyType.AssemblyQualifiedName);
                        foreach (var fkXe in fkColumnXes)
                            manyToOneXe.Add(fkXe);

                        classXe.Add(manyToOneXe);
                    }
                }
            }//----end Fk

            //----hbm bags
            var hbmBagNames = new List<string>(); //check for duplicates
            if (!Settings.DoNotReference.Contains(tbl) && hbmBags.ContainsKey(tbl) && !hasNoPkAtAll)
            {
                var distinctBagConstraintNames = hbmBags[tbl].Select(x => x.constraint_name).Distinct().ToList();
                foreach (var distinctBagConstraintName in distinctBagConstraintNames)
                {
                    var hbmBagPropertyName = Compose.PropertyName(distinctBagConstraintName);
                    var hbmBagXe = XeFactory.BagNode(hbmBagPropertyName, Globals.HbmXmlNames.ALL_DELETE_ORPHAN, bool.TrueString.ToLower(), bool.TrueString.ToLower(),
                        Globals.REPRESENT_512);
                    var bagColumns =
                        hbmBags[tbl].Where(x => string.Equals(x.constraint_name, distinctBagConstraintName, Sorting.C))
                            .Select(x => x.column_name)
                            .ToList();

                    string hbmOneToMany;
                    if (bagColumns.Count > 1)
                    {
                        var hbmBagFirstKey =
                            hbmBags[tbl].First(
                                x => string.Equals(x.constraint_name, distinctBagConstraintName, Sorting.C));
                        if (Settings.DoNotReference.Contains(hbmBagFirstKey.table_name))
                            continue;
                        hbmOneToMany = Compose.ClassName((hbmBagFirstKey.table_name), outputNamespace);
                        var hbmBagFkKeyXe = XeFactory.KeyNodeClassName(className);

                        foreach (
                            var columnData in
                                hbmBags[tbl].Where(
                                    x => string.Equals(x.constraint_name, distinctBagConstraintName, Sorting.C)).ToList())
                        {
                            columnData.CopyFrom(Sorting.GetFromAllColumnMetadata(columnData));
                            var fullColumnName = columnData.column_name;
                            
                            Compose.ValidSplit(fullColumnName, 3);
                            var hbmBagKeyColumn = Util.Etc.ExtractLastWholeWord(fullColumnName, null);

                            var hbmBagKeyXe = XeFactory.ColumnNode(hbmBagKeyColumn,columnData.ToJsonString());
                            hbmBagFkKeyXe.Add(hbmBagKeyXe);
                        }

                        hbmBagXe.Add(hbmBagFkKeyXe);
                    }
                    else
                    {
                        var hbmBagFirstKey =
                            hbmBags[tbl].First(
                                x => string.Equals(x.constraint_name, distinctBagConstraintName, Sorting.C));
                        if (Settings.DoNotReference.Contains(hbmBagFirstKey.table_name))
                            continue;

                        hbmBagFirstKey.CopyFrom(Sorting.GetFromAllColumnMetadata(hbmBagFirstKey));
                        var fullColumnName = hbmBagFirstKey.column_name;
                        hbmOneToMany = Compose.ClassName((hbmBagFirstKey.table_name), outputNamespace);

                        Compose.ValidSplit(fullColumnName, 3);

                        var hbmBagKeyColumn = Util.Etc.ExtractLastWholeWord(fullColumnName, null);
                        var hbmBagKeyXe = XeFactory.KeyNodeColumnName(hbmBagKeyColumn, hbmBagFirstKey.ToJsonString());
                        hbmBagXe.Add(hbmBagKeyXe);
                    }

                    var hbmOneToManyXe = XeFactory.OneToManyNode(hbmOneToMany);
                    hbmBagXe.Add(hbmOneToManyXe);

                    //attempt to make the name plural 
                    var newBagName = Compose.BagPropertyName(hbmOneToMany);
                    hbmBagXe.FirstAttribute.SetValue(newBagName);

                    classXe.Add(hbmBagXe);

                    hbmBagNames.Add(hbmBagPropertyName);
                }
            }

            xe.Add(classXe);

            var hbmXmlOutputPath = Path.Combine(Settings.HbmDirectory,
                string.Format("{0}.hbm.xml", Util.Etc.CapitalizeFirstLetterOfWholeWords(tbl, '.')));
            var xmlContent = xe.ToString()
                .Replace("<hibernate-mapping>", "<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.2\">");
            File.WriteAllText(hbmXmlOutputPath, xmlContent);

            //perform rename of any properties which match classname or are duplicated therein
            CorrectHbmXmlDuplicateNames(hbmXmlOutputPath);
            return hbmXmlOutputPath;
        }

        /// <summary>
        /// Produces a single hbm.xml with the sql-query node in tow.
        /// </summary>
        /// <param name="outputNamespace"></param>
        /// <param name="storedProc"></param>
        /// <returns>The path to the generated hbm.xml file.</returns>
        public static string GetHbmNamedQueryXml(string outputNamespace, string storedProc)
        {
            if (Sorting.AllStoredProcNames == null || Sorting.AllStoredProcNames.Count <= 0)
                throw new RahRowRagee("There doesn't appear to be any stored procs here.");

            if (Settings.DoNotReference.Contains(storedProc) || !Sorting.AllStoredProx.ContainsKey(storedProc))
                return null;

            var sp = Sorting.AllStoredProx[storedProc];
            if (sp == null)
                return null;

            var returnedData = sp.ReturnedData;
            if (returnedData == null || returnedData.Count <= 0)
                return null;

            if (returnedData.Keys.Count <= 0)
            {
                Settings.WriteToStoredProcLog(string.Format("Stored proc named '{0}' has an empty dataset",sp.ProcName));
                if(!Sorting.EmptyDatasetProx.Contains(sp.ProcName))
                    Sorting.EmptyDatasetProx.Add(sp.ProcName);
                return null;
            }

            //both the return types and the callable sql-query are wrapped in this root node
            var xe = XeFactory.HibernateMappingNode();

            if (returnedData.Count > 1)
            {
                Settings.WriteToStoredProcLog(string.Format("Stored Proc named '{0}' returns a multi-table dataset", storedProc));
                if (!Sorting.MultiTableDsProx.Contains(storedProc))
                    Sorting.MultiTableDsProx.Add(storedProc);
            }

            //possiable duplicate names handled within this
            var className = Compose.ClassName(string.Format("{0}.{1}{2}", Globals.STORED_PROX_FOLDER_NAME, storedProc, "Table"), outputNamespace);

            var classXe = XeFactory.ClassNode(className, null, null);
            classXe.Add(XeFactory.IdNode(null));

            var sqlQryName = Util.TypeName.SafeDotNetIdentifier(storedProc);
            var returnsXe = XeFactory.ReturnNode(sqlQryName, className);

            foreach (var cMeta in returnedData[returnedData.Keys.First()])
            {
                var simplePropName = Compose.PropertyName(cMeta.column_name,true);
                var simplePropColumn = Util.Etc.ExtractLastWholeWord(cMeta.column_name, null);
                var simplePropDataType = Util.Lexicon.DotNet2HbmTypes[cMeta.data_type];

                classXe.Add(XeFactory.PropertyNode(Globals.HbmXmlNames.PROPERTY, simplePropName, null, simplePropDataType,
                    (cMeta.data_type == "System.String"
                        ? Globals.MSSQL_MAX_VARCHAR.ToString(CultureInfo.InvariantCulture)
                        : null), cMeta.is_nullable.HasValue && cMeta.is_nullable == true, string.Empty));

                returnsXe.Add(XeFactory.ReturnPropertyNode(simplePropColumn, simplePropName));
            }

            var sqlQryXe = XeFactory.SqlQueryNode(sqlQryName);
            sqlQryXe.Add(returnsXe);

            sqlQryXe.Add(new XCData(sp.ToHbmSql()));

            xe.Add(classXe);
            xe.Add(sqlQryXe);

            var hbmXmlOutputPath = Path.Combine(Settings.HbmStoredProcsDirectory,
                string.Format("{0}.hbm.xml", Util.Etc.CapitalizeFirstLetterOfWholeWords(storedProc.Replace(" ", Globals.REPLACE_SPACE_WITH_SEQUENCE), '.')));
            var xmlContent = xe.ToString()
                .Replace("<hibernate-mapping>", "<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.2\">");
            File.WriteAllText(hbmXmlOutputPath, xmlContent);
            CorrectHbmXmlDuplicateNames(hbmXmlOutputPath, true);
            return hbmXmlOutputPath;
        }

        /// <summary>
        /// During the generation process of hbm.xml files from the various JSON metadata, it 
        /// is possiable that the names of properties may be duplicated and\or the name of 
        /// properties match their class name. 
        /// This function is intended to handle all such circumstances and correct them as needed
        /// appending a '00', '01' ,'02', etc. to the ends of duplicate names.
        /// </summary>
        /// <param name="hbmXmlOutputPath">
        /// This file must already exist on disk and, upon duplicates being found, is overwritten with the corrected contents.
        /// </param>
        /// <param name="storedProcRules">
        /// Set this flag to true if the <see cref="hbmXmlOutputPath"/>
        /// is working a file with contains any 'return-property' nodes (typically present on 
        /// sql-query nodes).
        /// </param>
        /// <remarks>
        /// This function will, in theory, work for any hbm.xml file which is of the xmlns <see cref="Globals.HBM_XML_NS"/>
        /// but was designed only for hbm.xml files generated herein.
        /// </remarks>
        public static void CorrectHbmXmlDuplicateNames(string hbmXmlOutputPath, bool storedProcRules = false)
        {
            if (!File.Exists(hbmXmlOutputPath))
                return;

            var overwriteFlag = false;

            var hbmXml = new XmlDocument();

            hbmXml.Load(hbmXmlOutputPath);

            var nsMgr = new XmlNamespaceManager(hbmXml.NameTable);
            nsMgr.AddNamespace("nhib", Globals.HBM_XML_NS);

            var hbmClassNode = hbmXml.SelectSingleNode("//nhib:class", nsMgr);
            if (hbmClassNode == null || hbmClassNode.Attributes == null)
                throw new InvalidHbmNameException(
                    string.Format(
                        "Xml file at '{0}' either does not have a node named 'class' or the 'class' node has no attributes.",
                        hbmXmlOutputPath));

            var hbmClassName = hbmClassNode.Attributes["name"].Value;
            var hbmTypeName = new Util.TypeName(hbmClassName);
            hbmClassName = hbmTypeName.ClassName;

            var propertyNames = new List<string>();
            var keyPropertyNames = new List<string>();
            var keyMtoPropertyNames = new List<string>();
            var returnPropNames = new List<string>();

            var propertyNodes = hbmXml.SelectNodes("//nhib:property", nsMgr);
            var manyToOneNodes = hbmXml.SelectNodes("//nhib:many-to-one", nsMgr);
            var bagNodes = hbmXml.SelectNodes("//nhib:bag", nsMgr);
            var keyPropertyNodes = hbmXml.SelectNodes("//nhib:key-property", nsMgr);
            var keyMtoPropertyNodes = hbmXml.SelectNodes("//nhib:key-many-to-one", nsMgr);
            var returnPropNodes = hbmXml.SelectNodes("//nhib:return-property", nsMgr);


            propertyNames.Add("Id");

            if (propertyNodes != null)
                propertyNames.AddRange(from XmlNode propNode in propertyNodes
                                       where propNode.Attributes != null && propNode.Attributes["name"] != null
                                       select propNode.Attributes["name"].Value);

            if (manyToOneNodes != null)
                propertyNames.AddRange(from XmlNode propNode in manyToOneNodes
                                       where propNode.Attributes != null && propNode.Attributes["name"] != null
                                       select propNode.Attributes["name"].Value);

            if (bagNodes != null)
                propertyNames.AddRange(from XmlNode propNode in bagNodes
                                       where propNode.Attributes != null && propNode.Attributes["name"] != null
                                       select propNode.Attributes["name"].Value);

            if (keyPropertyNodes != null)
                keyPropertyNames.AddRange(from XmlNode propNode in keyPropertyNodes
                                          where propNode.Attributes != null && propNode.Attributes["name"] != null
                                          select propNode.Attributes["name"].Value);

            if (keyMtoPropertyNodes != null)
                keyMtoPropertyNames.AddRange(from XmlNode propNode in keyMtoPropertyNodes
                                             where propNode.Attributes != null && propNode.Attributes["name"] != null
                                             select propNode.Attributes["name"].Value);

            if (returnPropNodes != null)
                returnPropNames.AddRange(from XmlNode propNode in returnPropNodes
                                             where propNode.Attributes != null && propNode.Attributes["name"] != null
                                             select propNode.Attributes["name"].Value);

            
            //acid test for duplicate names
            var dupPropOverwriteFlag = storedProcRules
                ? AcidTestDuplicateNames(hbmXmlOutputPath, propertyNames, hbmXml, nsMgr, "property")
                : AcidTestDuplicateNames(hbmXmlOutputPath, propertyNames, hbmXml, nsMgr);
            var dupKeyPropOverwriteFlag = AcidTestDuplicateNames(hbmXmlOutputPath, keyPropertyNames, hbmXml, nsMgr,
                "key-property");
            var dubMtoKeyPropOverwriteFlag = AcidTestDuplicateNames(hbmXmlOutputPath, keyMtoPropertyNames, hbmXml, nsMgr,
                "key-many-to-one");
            var dupReturnPropFlag = storedProcRules &&
                                    AcidTestDuplicateNames(hbmXmlOutputPath, returnPropNames, hbmXml, nsMgr,
                                        "return-property");

            overwriteFlag = dupKeyPropOverwriteFlag || dupPropOverwriteFlag || dubMtoKeyPropOverwriteFlag ||
                            dupReturnPropFlag;

            if (propertyNames.Contains(hbmClassName))
            {
                var pnNodes = hbmXml.SelectNodes(string.Format("//nhib:*[@name='{0}']", hbmClassName), nsMgr);
                if (pnNodes == null)
                    throw new ItsDeadJim(
                        string.Format(
                            "Property names which match the class name '{0}' are supposed to be present but an XPath selector returned null",
                            hbmClassName));
                for (var i = 0; i < pnNodes.Count; i++)
                {
                    overwriteFlag = true;
                    if (pnNodes[i] == null || pnNodes[i].Attributes == null ||
                        pnNodes[i].Attributes["name"] == null)
                        continue;
                    pnNodes[i].Attributes["name"].Value = string.Format("{0}{1:00}", hbmClassName, i);
                }
                Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss.ffff}'{1}' duplicate property [{2}]", DateTime.Now,
                    hbmXmlOutputPath, hbmClassName);
            }

            if (!overwriteFlag) return;

            using (var myXmlWriter = new XmlTextWriter(hbmXmlOutputPath, System.Text.Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            })
            {
                hbmXml.WriteContentTo(myXmlWriter);
                myXmlWriter.Flush();
            }
        }

        #endregion

        #region internal mapping helpers
        internal static void GetSimpleId(ColumnMetadata pkId, XElement classXe)
        {
            pkId.CopyFrom(Sorting.GetFromAllColumnMetadata(pkId));
            var fullColumnName = pkId.column_name;

            Compose.ValidSplit(fullColumnName, 3);
            var columnName = Util.Etc.ExtractLastWholeWord(fullColumnName, null);
            const string propertyName = Globals.HbmXmlNames.ID;
            var type = Util.Lexicon.Mssql2HbmTypes[(pkId.data_type)];
            var length = pkId.string_length ?? Globals.MSSQL_MAX_VARCHAR;
            XElement idXe;
            //don't let the '-1' from the database make it to the hbm.xml's
            if (string.Equals(type, Globals.HbmXmlNames.ANSI_STRING))
            {
                if (length <= 0)
                    length = Globals.MSSQL_MAX_VARCHAR;
                idXe = XeFactory.IdNode(propertyName, columnName, type,
                    length.ToString(CultureInfo.InvariantCulture),pkId.ToJsonString());
            }
            else
            {
                idXe = XeFactory.IdNode(propertyName, columnName, type, pkId.ToJsonString());
            }

            //simple split of assigned or is_identity
            XElement generatorXe;
            if (pkId.is_auto_increment.HasValue && pkId.is_auto_increment.Value)
            {
                generatorXe = XeFactory.GeneratorId(Globals.HbmXmlNames.IDENTITY);
            }
            else
            {
                generatorXe = XeFactory.GeneratorId(Globals.HbmXmlNames.ASSIGNED);
            }
            idXe.Add(generatorXe);
            classXe.Add(idXe);
        }

        internal static XElement GetSimplePropertyHbmXml(ColumnMetadata entry, string xElementName)
        {
            if (entry == null)
                return null;
            if (string.IsNullOrWhiteSpace(xElementName))
                xElementName = Globals.HbmXmlNames.PROPERTY;
            if (string.IsNullOrWhiteSpace(entry.data_type) &&
                Sorting.DbContainers.AllColumns.Data.Any(
                    x => string.Equals(x.column_name, entry.constraint_name, Sorting.C)))
            {
                var acEntry =
                    Sorting.DbContainers.AllColumns.Data.First(
                        x => string.Equals(x.column_name, entry.constraint_name, Sorting.C));
                entry.CopyFrom(acEntry);
            }
            else
            {
                entry.CopyFrom(Sorting.GetFromAllColumnMetadata(entry));
            }

            var simplePropName = Compose.PropertyName(entry.column_name);
            var simplePropColumn = Util.Etc.ExtractLastWholeWord(entry.column_name, null);

            var simplePropDataType = Globals.HbmXmlNames.ANSI_STRING;

            if (!Util.Lexicon.DotNet2HbmTypes.ContainsKey(string.Format("{0}", entry.data_type)) &&
                !Util.Lexicon.Mssql2HbmTypes.ContainsKey(string.Format("{0}", entry.data_type)))
            {
                Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss.ffff} '{1}' has no matching type in the Lexicon [{2}].",
                    DateTime.Now,
                    entry.data_type, entry.ToJsonString());
            }
            else
            {
                simplePropDataType = entry.data_type.StartsWith("System.")
                    ? Util.Lexicon.DotNet2HbmTypes[entry.data_type]
                    : Util.Lexicon.Mssql2HbmTypes[entry.data_type];
            }

            var simplePropLen = simplePropDataType == Globals.HbmXmlNames.ANSI_STRING
                ? entry.string_length == null || entry.string_length <= 0 ? Globals.MSSQL_MAX_VARCHAR : entry.string_length
                : null;

            if (simplePropDataType == typeof (Boolean).Name)
            {
                if (simplePropName.StartsWith(Util.TypeName.DEFAULT_NAME_PREFIX))
                    simplePropName = simplePropName.Remove(0, Util.TypeName.DEFAULT_NAME_PREFIX.Length);
                simplePropName = "Is" + simplePropName;
            }

            return XeFactory.PropertyNode(xElementName, simplePropName, simplePropColumn,
                simplePropDataType, simplePropLen.ToString(), entry.is_nullable.HasValue && entry.is_nullable == true,
                entry.ToJsonString());
        }

        internal static void GetAllColumnsAsCompositeKey(string tableName, XElement classXe, string outputNamespace)
        {
            if (UseUniqueClusteredIndexAsKey(tableName, classXe, outputNamespace))
                return;

            var tbl = tableName;
            var compClassName = Compose.CompKeyClassName(tbl, outputNamespace);
            const string compPropertyName = Globals.HbmXmlNames.ID;

            var compKeyXeNoPk = XeFactory.CompositeIdNode(compPropertyName, compClassName);
            var allTblsColumns = Sorting.DbContainers.AllColumns.Data.Where(x => string.Equals(x.table_name, tbl, Sorting.C)).ToList();

            var composePk = allTblsColumns.Where(x => x.is_nullable != null && x.is_nullable.Value == false).ToList();
            var simpleProps = allTblsColumns.Where(x => x.is_nullable == null || x.is_nullable.Value).ToList();
            if (composePk.Count <= 0)
                throw new ItsDeadJim(
                    string.Format(
                        "The table named '{0}' has no PK, all of its columns are nullable and has no Unique Clustered Index " +
                        "to use as a substitute - add this to the 'DoNotReference' list and try again.",
                        tableName));

            //max is 16 when using NHibernate.Tool.hbm2ddl.SchemaExport
            if (composePk.Count > 16)
            {
                composePk = composePk.Take(16).ToList();
                simpleProps.AddRange(composePk.Skip(16).Take(composePk.Count).ToList());
            }

            foreach (
                var simplePropXe in
                    composePk.Select(columnData => GetSimplePropertyHbmXml(columnData, Globals.HbmXmlNames.KEY_PROPERTY)))
                compKeyXeNoPk.Add(simplePropXe);

            classXe.Add(compKeyXeNoPk);

            foreach (
                var simplePropXe in
                    simpleProps.Select(columnData => GetSimplePropertyHbmXml(columnData, Globals.HbmXmlNames.PROPERTY)))
                classXe.Add(simplePropXe);
        }

        internal static bool UseUniqueClusteredIndexAsKey(string tableName, XElement classXe, string outputNamespace)
        {
            var tbl = tableName;
            //a FK reference may be set to a table with no PK if the FK references a column that is non-nullable unique non-clustered index
            var uqIdxColumns =
                Sorting.DbContainers.UniqueClusteredIdxNotPks.Data.Where(
                    x => string.Equals(x.table_name, tbl, Sorting.C)).ToList();
            if (uqIdxColumns.Count <= 0) return false;

            var allTblsColumns = Sorting.DbContainers.AllColumns.Data.Where(x => string.Equals(x.table_name, tbl, Sorting.C)).ToList();

            if (uqIdxColumns.Count == 1)
            {
                var pkId = uqIdxColumns.First();
                //need to look this up in AllColumns for other bits of info
                var acInfo =
                    Sorting.DbContainers.AllColumns.Data.First(
                        x => string.Equals(x.column_name, pkId.column_name, Sorting.C));
                pkId.CopyFrom(acInfo);

                GetSimpleId(pkId, classXe);
                foreach (
                    var simplePropXe in
                        allTblsColumns.Where(x => !string.Equals(x.column_name, pkId.column_name))
                            .Select(columnData => GetSimplePropertyHbmXml(columnData, Globals.HbmXmlNames.PROPERTY)))
                    classXe.Add(simplePropXe);
                return true;
            }

            //although rare is is possiable to have a table with no PK and mutliple UqIdx - there is no way for the ORM to deal with this.
            var isIncompatiable = uqIdxColumns.Select(x => x.constraint_name).Distinct().ToList().Count > 1;
            if (isIncompatiable)
                return false;

            var compClassName = Compose.CompKeyClassName(tbl, outputNamespace);
            const string compPropertyName = Globals.HbmXmlNames.ID;

            var compKeyXeNoPk = XeFactory.CompositeIdNode(compPropertyName, compClassName);

            //need to look this up in AllColumns for other bits of info
            foreach (var uqCol in uqIdxColumns)
            {
                var acInfo =
                    Sorting.DbContainers.AllColumns.Data.First(
                        x => string.Equals(x.column_name, uqCol.column_name, Sorting.C));
                uqCol.CopyFrom(acInfo);
            }

            foreach (
                var uqColData in
                    uqIdxColumns.Select(columnData => GetSimplePropertyHbmXml(columnData, Globals.HbmXmlNames.KEY_PROPERTY)))
                compKeyXeNoPk.Add(uqColData);

            classXe.Add(compKeyXeNoPk);
            foreach (
                    var simplePropXe in
                        allTblsColumns.Where(x => !uqIdxColumns.Any(y => string.Equals(x.column_name, y.column_name)))
                            .Select(columnData => GetSimplePropertyHbmXml(columnData, Globals.HbmXmlNames.PROPERTY)))
                classXe.Add(simplePropXe);

            return true;
        }

        internal static bool AcidTestDuplicateNames(string hbmXmlOutputPath, List<string> propertyNames, XmlDocument hbmXml,
            XmlNamespaceManager nsMgr, string specificNodeType = "*")
        {
            var overwriteFlag = false;
            if (propertyNames.Count == propertyNames.Distinct().Count()) return false;
            foreach (var pn in propertyNames)
            {
                var pnNodes = hbmXml.SelectNodes(string.Format("//nhib:{0}[@name='{1}']",specificNodeType, pn), nsMgr);
                if (pnNodes == null || pnNodes.Count <= 1)
                    continue;

                for (var i = 0; i < pnNodes.Count; i++)
                {
                    overwriteFlag = true;
                    if (pnNodes[i] == null || pnNodes[i].Attributes == null ||
                        pnNodes[i].Attributes["name"] == null)
                        continue;
                    pnNodes[i].Attributes["name"].Value = string.Format("{0}{1:00}", pn, i);
                }
                Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss.ffff}'{1}' duplicate property [{2}]", DateTime.Now,
                    hbmXmlOutputPath, pn);
            }
            return overwriteFlag;
        }
        #endregion

    }
}
