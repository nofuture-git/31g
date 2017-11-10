using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NoFuture.Shared.Core;
using NoFuture.Util;
using NoFuture.Util.Core;
using NoFuture.Util.NfType;
using HbmXmlNames = NoFuture.Hbm.Globals.HbmXmlNames;

namespace NoFuture.Hbm.SortingContainers
{
    public class HbmFileContent : IComparable<HbmFileContent>
    {
        #region private fields
        private readonly Dictionary<string, string> _simpleProperties;
        private readonly Dictionary<string, string> _fkProperties;
        private readonly Dictionary<string, string> _listProperties;
        private readonly Dictionary<string, string> _compositeKeyProperties;
        private readonly List<HbmStoredProxNames> _spConstNames;
        private readonly List<string> _sqlOnWhichDepends = new List<string>();
        private string _sqlTbl;

        //these are held internally and are only queryable via the GetColumnDataByPropertyName
        private readonly Dictionary<string, ColumnMetadata> _keyColumns;
        private readonly Dictionary<string, ColumnMetadata> _simpleColumns;
        private readonly Dictionary<string, List<ColumnMetadata>> _fkColumns;
        private readonly Dictionary<string, List<ColumnMetadata>> _listColumns;

        private readonly Dictionary<string, string> _allPkColumns;
        private readonly List<string> _keyManyToOnePropertyNames;
        private readonly string _fileNamePath;
        private readonly XmlDocument _hbmXml;
        private readonly XmlNamespaceManager _nsMgr;

        protected XmlElement _idNode;
        protected XmlElement _classNode;
        protected string _tableName;
        protected string _className;
        protected string _namespace;
        protected string _dbSchema;
        protected string _idName;
        protected string _idType;
        protected string _asmQualTypeName;

        private const string NS = "hbm";
        #endregion

        #region api

        public const string T4_PARAM_NAME = "hbmFilePath";
        public const string INVOKE_NF_TYPE_NAME = "invokeNfTypeName";

        /// <summary>
        /// The full, assembly qualified, type name of the class to which the 
        /// hbm.xml file pertains.
        /// </summary>
        public virtual string AssemblyQualifiedTypeName => _asmQualTypeName;

        /// <summary>
        /// The namespace portion of the class node name attribute value.
        /// </summary>
        public virtual string Namespace => _namespace;

        /// <summary>
        /// Just the direct name of the class (no namespace nor assembly qualifier)
        /// portion of the class node name attribute value.
        /// </summary>
        public virtual string Classname => _className;

        /// <summary>
        /// The directory from which hbm.xml file was read.
        /// </summary>
        /// <remarks>
        /// hbm.xml's derived from stored prox will not have a
        /// schema so this is used to contrive one.
        /// </remarks>
        public virtual string HbmXmlDirectory => Path.GetDirectoryName(_fileNamePath);

        /// <summary>
        /// The value of the schema attribute attached to the top-level class node.
        /// </summary>
        /// <remarks>
        /// When the schema attribute is not present then an attempt is made 
        /// to contrive one based on the <see cref="HbmXmlDirectory"/> otherwise
        /// defaulting to <see cref="Settings.DefaultSchemaName"/>.
        /// </remarks>
        public virtual string DbSchema => _dbSchema;

        /// <summary>
        /// The value as-is assigned the class level's attribute of like name.
        /// </summary>
        public virtual string TableName => _tableName;

        /// <summary>
        /// This is the name of the identity property almost always being equal to <see cref="Globals.HbmXmlNames.ID"/>
        /// </summary>
        /// <remarks>
        /// The exception to the value being <see cref="Globals.HbmXmlNames.ID"/> is the case 
        /// for hbm.xml files derived from stored prox - a stored proc may in fact return 
        /// a column named the same.
        /// </remarks>
        public virtual string IdName => _idName;

        /// <summary>
        /// For Composite types this will be a namespace qualified type name, otherwise a simple value type.
        /// </summary>
        public virtual string IdType => _idType;

        /// <summary>
        /// This is just the PK's underlying columns (as .NET-safe property names).
        /// </summary>
        /// <remarks>
        /// This is useless to NHibernate and is intended for EF 6.x code gen.
        /// </remarks>
        public virtual Dictionary<string, string> IdAsSimpleProperties => _allPkColumns;

        /// <summary>
        /// Asserts if the given hbm.xml file uses a composite-id 
        /// </summary>
        public virtual bool IsCompositeKey { get; private set; }

        /// <summary>
        /// A hash of the composite key's property names-to-types
        /// This is only relevant for <see cref="IsCompositeKey"/> begin true.
        /// </summary>
        public virtual Dictionary<string, string> CompositeKeyProperties => _compositeKeyProperties;

        /// <summary>
        /// This is the value-type property names-to-types being those properties which
        /// are not related to any FK nor PK.
        /// </summary>
        public virtual Dictionary<string, string> SimpleProperties => _simpleProperties;

        /// <summary>
        /// This is the names-to-types hash for the many-to-one (FKs) nodes.
        /// </summary>
        public virtual Dictionary<string, string> FkProperties => _fkProperties;

        /// <summary>
        /// This is the names-to-types 'bag' nodes defined in this hbm.xml file.
        /// </summary>
        public virtual Dictionary<string, string> ListProperties => _listProperties;

        /// <summary>
        /// This is specific to hbm.xml parsed from stored prox <see cref="StoredProcMetadata.TryParseToHbmSql"/>
        /// </summary>
        public virtual List<HbmStoredProxNames> SpConstNames => _spConstNames;

        /// <summary>
        /// This is a reverse lookup for the underlying column data which spawned this property.
        /// As such the given <see cref="propertyName"/> must be directly tied to a single 
        /// column.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ColumnMetadata[] GetColumnDataByPropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            propertyName = propertyName.Trim();

            var cmeta = new List<ColumnMetadata>();

            if (_keyColumns.ContainsKey(propertyName))
            {
                cmeta.Add(_keyColumns[propertyName]);
                return cmeta.ToArray();
            }
            if (_simpleColumns.ContainsKey(propertyName))
            {
                cmeta.Add(_simpleColumns[propertyName]);
                return cmeta.ToArray();
            }

            if (_fkColumns.ContainsKey(propertyName))
            {
                cmeta.AddRange(_fkColumns[propertyName]);
                return cmeta.ToArray();
            }

            if (_listColumns.ContainsKey(propertyName))
            {
                cmeta.AddRange(_listColumns[propertyName]);
            }

            return cmeta.ToArray();
        }


        /// <summary>
        /// Hack method to generate a T-SQL script to get all the depend records which 
        /// spider out from this particular one.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="topX"></param>
        /// <returns></returns>
        public virtual string WriteDependencySqlScript(string[] ids, int topX = 100)
        {
            var sql = new List<string>();
            var dcl = new List<string> { GetSqlScriptTblVariableDeclare(), GetSqlScriptTblVariableInsert(), $"VALUES ({string.Join(", ", ids)})" };
            var slct = new List<Tuple<string, string, string>>();
            var includedTypes = new Dictionary<Tuple<string, string>, HbmFileContent>();
            GetDependencySqlScriptStmts(includedTypes);

            if (!includedTypes.Any())
                return string.Join(Environment.NewLine, sql);

            foreach (var dTfn in includedTypes.Keys)
            {
                var dHbm = includedTypes[dTfn];
                if (dHbm == null)
                    continue;
                var dHbmName = dHbm.AssemblyQualifiedTypeName;
                dcl.Add(dHbm.GetSqlScriptTblVariableDeclare());
                sql.AddRange(dHbm.GetSqlScriptOnWhichDepends());
                slct.Add(new Tuple<string, string, string>(dHbm.GetSqlScriptPkString(), dHbm.GetSqlScriptTableVariableName(), dHbmName));
            }

            var finalList = dcl.Distinct().ToList();
            finalList.AddRange(sql);
            finalList.Add(Globals.DF_DELIMITER_START);

            foreach (var x in slct)
            {
                finalList.Add($"SELECT TOP {topX} '{x.Item2.Substring(1)}' AS table_name, {x.Item1} FROM {x.Item2} --{x.Item3}");
            }
            
            finalList.Add(Globals.DF_DELIMITER_END);

            return string.Join(Environment.NewLine, finalList);
        }

        /// <summary>
        /// Asserts that each of this instances <see cref="FkProperties"/> is represented
        /// on the drive as an hbm.xml file.
        /// </summary>
        /// <returns></returns>
        public bool IsAllFkHbmXmlPresent()
        {
            if (FkProperties == null || !FkProperties.Any())
                return true;
            foreach (var fk in FkProperties.Keys)
            {
                var hbmXml = Compose.HbmFileNameFromAsmQualTypeName(FkProperties[fk], true);
                if (!File.Exists(hbmXml))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Asserts that each of this instances <see cref="ListProperties"/> is represented
        /// on the drive as an hbm.xml file.
        /// </summary>
        /// <returns></returns>
        public bool IsAllListHbmXmlPresent()
        {
            if (ListProperties == null || !ListProperties.Any())
                return true;
            foreach (var fk in ListProperties.Keys)
            {
                var hbmXml = Compose.HbmFileNameFromAsmQualTypeName(ListProperties[fk], true);
                if (!File.Exists(hbmXml))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Intended for the creation of a SortedList of this type.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(HbmFileContent other)
        {
            const int THIS_COMES_BEFORE_OTHER = -1;
            const int THIS_EQUALS_OTHER = 0;
            const int OTHER_COMES_BEFORE_THIS = 1;

            if (other == null)
                return THIS_COMES_BEFORE_OTHER;

            var myName = AssemblyQualifiedTypeName;
            var otherName = other.AssemblyQualifiedTypeName;

            if (otherName == myName)
                return THIS_EQUALS_OTHER;
            if (IsCompositeKey && CompositeKeyProperties != null && CompositeKeyProperties.ContainsValue(otherName))
                return OTHER_COMES_BEFORE_THIS;
            if (IdAsSimpleProperties != null && IdAsSimpleProperties.ContainsValue(otherName))
                return OTHER_COMES_BEFORE_THIS;

            if (FkProperties != null && FkProperties.ContainsValue(otherName))
                return OTHER_COMES_BEFORE_THIS;

            if (ListProperties != null && ListProperties.ContainsKey(otherName))
                return THIS_COMES_BEFORE_OTHER;

            return THIS_EQUALS_OTHER;
        }
        #endregion

        #region ctor

        /// <summary>
        /// Parses a hbm.xml file into this runtime equiv.
        /// being intended for code generation.
        /// </summary>
        /// <param name="hbmXmlFilePath"></param>
        public HbmFileContent(string hbmXmlFilePath)
        {
            _simpleProperties = new Dictionary<string, string>();
            _fkProperties = new Dictionary<string, string>();
            _listProperties = new Dictionary<string, string>();
            _compositeKeyProperties = new Dictionary<string, string>();
            
            _keyColumns = new Dictionary<string, ColumnMetadata>();
            _simpleColumns = new Dictionary<string, ColumnMetadata>();
            _fkColumns = new Dictionary<string, List<ColumnMetadata>>();
            _listColumns = new Dictionary<string, List<ColumnMetadata>>();

            _spConstNames = new List<HbmStoredProxNames>();
            _keyManyToOnePropertyNames = new List<string>();
            _allPkColumns = new Dictionary<string, string>();

            if(!File.Exists(hbmXmlFilePath))
                throw new ItsDeadJim(string.Format("There isn't any xml file at '{0}'", hbmXmlFilePath));

            _fileNamePath = hbmXmlFilePath;
            _hbmXml = new XmlDocument();
            _hbmXml.LoadXml(File.ReadAllText(_fileNamePath));
            _nsMgr = new XmlNamespaceManager(_hbmXml.NameTable);
            _nsMgr.AddNamespace(NS, Globals.HBM_XML_NS);

            _classNode =
                _hbmXml.SelectSingleNode(CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS), _nsMgr) as
                    XmlElement;
            if(_classNode == null)
                throw new ItsDeadJim(string.Format("The top-level 'class' node is missing from the xml file at '{0}'",hbmXmlFilePath));

            IsCompositeKey =
                _hbmXml.SelectSingleNode(CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS, HbmXmlNames.COMPOSITE_ID), _nsMgr) != null;

            var tableNameAttr = _classNode.Attributes[HbmXmlNames.TABLE];
            if (tableNameAttr != null)
                _tableName = tableNameAttr.Value;

            var attrTypeName = GetAttrVal(CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS), HbmXmlNames.NAME);
            if (NfReflect.IsFullAssemblyQualTypeName(attrTypeName))
            {
                _asmQualTypeName = attrTypeName;
                var nfName = new NfTypeName(attrTypeName);
                _className = nfName.ClassName;
                _namespace = nfName.Namespace;
            }
            else
            {
                _className = NfReflect.GetTypeNameWithoutNamespace(attrTypeName);
                _namespace = NfReflect.GetNamespaceWithoutTypeName(attrTypeName);
                _asmQualTypeName = Compose.ClassName(_className, _namespace);
            }

            _dbSchema = GetAttrVal(CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS), HbmXmlNames.SCHEMA);

            //the stored prox will not have a schema qualifier so try to derive it from the file name
            if (string.IsNullOrWhiteSpace(_dbSchema))
            {
                var ff = Path.GetFileName(_fileNamePath);
                if (!string.IsNullOrWhiteSpace(ff) && ff.Split('.').Length > 3)
                {
                    _dbSchema = ff.Split('.')[0];
                }
                else
                {
                    _dbSchema = "Dbo";
                }
            }

            //get id/composite-id node
            GetIdNode();

            //get id's name and type
            GetIdName();

            //get flat-data's names and types
            GetFlatProperties();

            //get FKs names and types
            GetFkProperties();

            //get IList names and types
            GetListProperties();

            //get stored proc names
            GetStoredProcNames();

            //get composite key's properties
            GetCompositeKeyProperties();

            //condense a list of what is just on this table
            GetAllPkColumns();
        }
        #endregion

        #region internal methods

        protected internal void AddToSimpleProperties(string key, string val)
        {
            if (_simpleProperties.ContainsKey(key))
            {
                _simpleProperties[key] = val;
            }
            else
            {
                _simpleProperties.Add(key, val);
            }

        }
        protected internal void AddToListProperties(string key, string val)
        {
            if (_listProperties.ContainsKey(key))
            {
                _listProperties[key] = val;
            }
            else
            {
                _listProperties.Add(key, val);
            }
        }
        protected internal void AddToCompositeKeyProperties(string key, string val)
        {
            if (_compositeKeyProperties.ContainsKey(key))
            {
                _compositeKeyProperties[key] = val;
            }
            else
            {
                _compositeKeyProperties.Add(key, val);
            }
        }
        protected internal void AddToKeyColumns(string key, ColumnMetadata val)
        {
            if (_keyColumns.ContainsKey(key))
            {
                _keyColumns[key] = val;
            }
            else
            {
                _keyColumns.Add(key, val);
            }
        }
        protected internal void AddToSimpleColumns(string key, ColumnMetadata val)
        {
            if (_simpleColumns.ContainsKey(key))
            {
                _simpleColumns[key] = val;
            }
            else
            {
                _simpleColumns.Add(key, val);
            }
        }
        protected internal void AddToFkColumns(string key, List<ColumnMetadata> val)
        {
            if (_fkColumns.ContainsKey(key))
            {
                _fkColumns[key] = val;
            }
            else
            {
                _fkColumns.Add(key, val);
            }
        }
        protected internal void AddToListColumns(string key, List<ColumnMetadata> val)
        {
            if (_listColumns.ContainsKey(key))
            {
                _listColumns[key] = val;
            }
            else
            {
                _listColumns.Add(key, val);
            }
        }
        protected internal void AddToAllPkColumns(string key, string val)
        {
            if (_allPkColumns.ContainsKey(key))
            {
                _allPkColumns[key] = val;
            }
            else
            {
                _allPkColumns.Add(key, val);
            }
        }

        protected internal void GetAllPkColumns()
        {

            if (IsCompositeKey)
            {
                foreach (var k in _keyColumns.Keys)
                {
                    var keyMtoColumn = _keyColumns[k];
                    var dotnetNullableChar = keyMtoColumn.is_nullable.HasValue && keyMtoColumn.is_nullable == true
                        ? "?"
                        : string.Empty;

                    var sqlDbType = keyMtoColumn.data_type;
                    var hbmType = Lexicon.Mssql2HbmTypes[sqlDbType];
                    var dotNetType = Lexicon.Hbm2NetTypes[hbmType];
                    if (dotNetType == "string" || dotNetType == "System.String" || dotNetType.StartsWith("NoFuture"))
                        dotnetNullableChar = string.Empty;
                    AddToAllPkColumns(k, string.Format("{0}{1}", dotNetType, dotnetNullableChar));
                }

                foreach (
                    var kp in
                        _compositeKeyProperties.Keys.Where(
                            c => !_allPkColumns.ContainsKey(c) && !_keyManyToOnePropertyNames.Contains(c)))
                    AddToAllPkColumns(kp, _compositeKeyProperties[kp]);

            }
            else
            {
                AddToAllPkColumns(IdName, IdType);
            }
        }

        protected internal void GetCompositeKeyProperties()
        {
            if (!IsCompositeKey) return;
            var kmtoProps =
                _hbmXml.SelectNodes(
                    CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS, HbmXmlNames.COMPOSITE_ID,
                        HbmXmlNames.KEY_MANY_TO_ONE), _nsMgr);
            var keyProps = _hbmXml.SelectNodes(
                CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS, HbmXmlNames.COMPOSITE_ID,
                    HbmXmlNames.KEY_PROPERTY), _nsMgr);

            if (kmtoProps != null)
            {
                foreach (
                    var kmto in
                        kmtoProps.Cast<XmlElement>()
                            .Where(
                                t =>
                                    t != null && t.HasAttributes && t.Attributes[HbmXmlNames.NAME] != null &&
                                    t.Attributes[HbmXmlNames.CLASS] != null))
                {
                    var kpName = kmto.Attributes[HbmXmlNames.NAME].Value;
                    var kpType = (new NfTypeName(kmto.Attributes[HbmXmlNames.CLASS].Value)).FullName;

                    //filter list used in GetAllTablesColumns
                    _keyManyToOnePropertyNames.Add(kpName);
                    AddToCompositeKeyProperties(kpName, kpType);

                    //add the comment data 
                    if (!kmto.HasChildNodes)
                        continue;

                    var columnNode = kmto.FirstChild;
                    while (columnNode != null)
                    {
                        if (!columnNode.HasChildNodes)
                        {
                            columnNode = columnNode.NextSibling;
                            continue;
                        }

                        var pn =
                            Compose.PropertyName(
                                columnNode.Attributes[HbmXmlNames.NAME].Value.Replace("[", string.Empty)
                                    .Replace("]", string.Empty));
                        var columnJson = columnNode.FirstChild.InnerText;
                        ColumnMetadata dataOut;
                        if (!ColumnMetadata.TryParseJson(columnJson, out dataOut))
                        {
                            columnNode = columnNode.NextSibling;
                            continue;
                        }
                        AddToKeyColumns(pn, dataOut);

                        columnNode = columnNode.NextSibling;
                    }
                }
            }
            if (keyProps == null)
                return;
            foreach (
                var kp in
                    keyProps.Cast<XmlElement>()
                        .Where(
                            t =>
                                t != null && t.HasAttributes && t.Attributes[HbmXmlNames.NAME] != null &&
                                t.Attributes[HbmXmlNames.TYPE] != null))
            {
                var kpName = kp.Attributes[HbmXmlNames.NAME].Value;
                var kpType = Lexicon.Hbm2NetTypes[(kp.Attributes[HbmXmlNames.TYPE].Value)];
                AddToCompositeKeyProperties(kpName, kpType);
                if (!kp.HasChildNodes || !kp.FirstChild.HasChildNodes)
                    continue;
                var columnNode = kp.FirstChild;
                var columnJson = columnNode.FirstChild.InnerText;
                ColumnMetadata dataOut;
                if (ColumnMetadata.TryParseJson(columnJson, out dataOut))
                    AddToKeyColumns(kpName, dataOut);
            }
        }

        protected internal void GetStoredProcNames()
        {
            var sqlNodes = _hbmXml.SelectNodes(CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.SQL_QUERY), _nsMgr);
            if (sqlNodes == null) return;
            foreach (
                var sp in
                    sqlNodes.Cast<XmlElement>()
                        .Where(
                            t =>
                                t != null && t.HasAttributes && t.Attributes[HbmXmlNames.NAME] != null &&
                                !string.IsNullOrWhiteSpace(t.Attributes[HbmXmlNames.NAME].Value)))
            {
                var spName = sp.Attributes[HbmXmlNames.NAME].Value;
                var spSqlSyntax = sp.InnerText;
                if (string.IsNullOrWhiteSpace(spSqlSyntax))
                    continue;

                string spDbName;
                string[] spParamNames;
                if (!StoredProcMetadata.TryParseToHbmSql(spSqlSyntax, out spDbName, out spParamNames))
                    continue;
                _spConstNames.Add(new HbmStoredProxNames
                {
                    CallableName = spName,
                    DbName = spDbName,
                    ParamNames = spParamNames
                });

            }
        }

        protected internal void GetIdNode()
        {
            var idn =
                _hbmXml.SelectSingleNode(
                    CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS, HbmXmlNames.ID.ToLower()), _nsMgr) ??
                _hbmXml.SelectSingleNode(
                    CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS, HbmXmlNames.COMPOSITE_ID), _nsMgr);


            _idNode = idn as XmlElement;

            if (_idNode == null)
                throw new ItsDeadJim(string.Format("Did not locate an ID node within the hbm.xml file at '{0}'",
                    _fileNamePath));

            if (_idNode.Attributes[HbmXmlNames.NAME] == null)
                throw new ItsDeadJim(
                    string.Format("Found the ID node but did not find the attribute 'name' in hbm.xml file at '{0}'",
                        _fileNamePath));
        }

        protected internal void GetIdName()
        {
            _idName = _idNode.Attributes[HbmXmlNames.NAME].Value;

            var idTypeAttr = _idNode.Attributes[HbmXmlNames.TYPE] ?? _idNode.Attributes[HbmXmlNames.CLASS];

            if (idTypeAttr == null)
                throw new ItsDeadJim(
                    string.Format(
                        "Found the ID node but did not find either the attribute 'type' nor 'class' in hbm.xml file at '{0}'",
                        _fileNamePath));

            _idType = idTypeAttr.Value;
            if (string.IsNullOrWhiteSpace(IdType))
                throw new ItsDeadJim(
                    string.Format(
                        "Found the ID and an attribute (either 'type' or 'class') but its value is empty in hbm.xml file at '{0}'",
                        _fileNamePath));

            if (IsCompositeKey)
            {
                _idType = NfReflect.IsFullAssemblyQualTypeName(IdType)
                    ? (new NfTypeName(IdType)).FullName
                    : IdType;
            }
            else
            {
                if (_idNode.HasChildNodes && _idNode.FirstChild.HasChildNodes)
                {
                    var columnNode = _idNode.FirstChild;
                    var columnJson = columnNode.FirstChild.InnerText;
                    ColumnMetadata dataOut;
                    if (ColumnMetadata.TryParseJson(columnJson, out dataOut))
                        AddToKeyColumns(IdName, dataOut);

                }
                _idType = Lexicon.Hbm2NetTypes[IdType];    
            }
        }

        protected internal void GetFkProperties()
        {
            var mtoPropNodes =
                _hbmXml.SelectNodes(
                    CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS, HbmXmlNames.MANY_TO_ONE), _nsMgr);
            if (mtoPropNodes == null) return;
            foreach (var mto in mtoPropNodes.Cast<XmlElement>().Where(t => t != null && t.HasAttributes))
            {
                if (mto.Attributes[HbmXmlNames.NAME] == null ||
                    string.IsNullOrWhiteSpace(mto.Attributes[HbmXmlNames.NAME].Value))
                    continue;
                if (mto.Attributes[HbmXmlNames.CLASS] == null ||
                    string.IsNullOrWhiteSpace(mto.Attributes[HbmXmlNames.CLASS].Value))
                    continue;

                var fkName = mto.Attributes[HbmXmlNames.NAME].Value;

                _fkProperties.Add(fkName, mto.Attributes[HbmXmlNames.CLASS].Value);

                if (!mto.HasChildNodes)
                    continue;

                var mtoColumns = new List<ColumnMetadata>();
                var columnNode = mto.FirstChild;
                while (columnNode != null)
                {
                    if (!columnNode.HasChildNodes)
                    {
                        columnNode = columnNode.NextSibling;
                        continue;
                    }

                    var columnJson = columnNode.FirstChild.InnerText;
                    ColumnMetadata dataOut;
                    if (!ColumnMetadata.TryParseJson(columnJson, out dataOut))
                    {
                        columnNode = columnNode.NextSibling;
                        continue;
                    }
                    mtoColumns.Add(dataOut);

                    columnNode = columnNode.NextSibling;
                }

                AddToFkColumns(fkName, mtoColumns);
            }
        }

        protected internal void GetListProperties()
        {
            var bagPropNodes = _hbmXml.SelectNodes(CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS, HbmXmlNames.BAG), _nsMgr);
            if (bagPropNodes == null) return;
            foreach (var bag in bagPropNodes.Cast<XmlElement>().Where(t => t != null && t.HasAttributes))
            {
                if (bag.Attributes[HbmXmlNames.NAME] == null || string.IsNullOrWhiteSpace(bag.Attributes[HbmXmlNames.NAME].Value) || !bag.HasChildNodes)
                    continue;
                var otmNode = bag.ChildNodes.Cast<XmlElement>().First(x => x.Name.Contains(HbmXmlNames.ONE_TO_MANY));
                if (otmNode == null || !otmNode.HasAttributes || otmNode.Attributes[HbmXmlNames.CLASS] == null)
                    continue;

                var bagPropertyName = bag.Attributes[HbmXmlNames.NAME].Value;

                AddToListProperties(bagPropertyName, otmNode.Attributes[HbmXmlNames.CLASS].Value);

                var bagKeyNode = bag.ChildNodes.Cast<XmlElement>().FirstOrDefault(x => x.Name == HbmXmlNames.KEY);
                if (bagKeyNode == null)
                    return;

                var bagKeyColumns = new List<ColumnMetadata>();
                var columnNode = bagKeyNode.FirstChild;
                while (columnNode != null)
                {
                    if (!columnNode.HasChildNodes)
                    {
                        columnNode = columnNode.NextSibling;
                        continue;
                    }

                    var columnJson = columnNode.FirstChild.InnerText;
                    ColumnMetadata dataOut;
                    if (!ColumnMetadata.TryParseJson(columnJson, out dataOut))
                    {
                        columnNode = columnNode.NextSibling;
                        continue;
                    }
                    bagKeyColumns.Add(dataOut);

                    columnNode = columnNode.NextSibling;
                }
                AddToListColumns(bagPropertyName, bagKeyColumns);
            }
        }

        protected internal void GetFlatProperties()
        {
            var flatPropNodes = _hbmXml.SelectNodes(CreateXpath(HbmXmlNames.HIBERNATE_MAPPING, HbmXmlNames.CLASS, HbmXmlNames.PROPERTY), _nsMgr);
            if (flatPropNodes == null) return;
            foreach (var fp in flatPropNodes.Cast<XmlElement>().Where(t => t != null && t.HasAttributes))
            {
                if (fp.Attributes[HbmXmlNames.NAME] == null || string.IsNullOrWhiteSpace(fp.Attributes[HbmXmlNames.NAME].Value))
                    continue;
                var flatPropKey = fp.Attributes[HbmXmlNames.NAME].Value;
                var csIsNullableChar = "?";

                var csTypeName = Lexicon.Hbm2NetTypes[(fp.Attributes[HbmXmlNames.TYPE].Value)] ?? string.Empty;

                if (fp.Attributes[HbmXmlNames.NOT_NULL] != null &&
                    string.Equals(fp.Attributes[HbmXmlNames.NOT_NULL].Value, bool.TrueString,
                        StringComparison.OrdinalIgnoreCase))
                    csIsNullableChar = string.Empty;
                if (csTypeName == "string" || csTypeName == "System.String" || csTypeName.StartsWith("NoFuture"))
                    csIsNullableChar = string.Empty;

                AddToSimpleProperties(flatPropKey, string.Format("{0}{1}", csTypeName, csIsNullableChar));

                //add the comment data 
                if (!fp.HasChildNodes || !fp.FirstChild.HasChildNodes)
                    continue;
                var columnNode = fp.FirstChild;
                var columnJson = columnNode.FirstChild.InnerText;
                ColumnMetadata dataOut;
                if (ColumnMetadata.TryParseJson(columnJson, out dataOut))
                    AddToSimpleColumns(flatPropKey, dataOut);
            }
        }

        protected internal string GetAttrVal(string xpath, string attrName)
        {
            if (string.IsNullOrWhiteSpace(xpath) || string.IsNullOrWhiteSpace(attrName))
                return string.Empty;
            var nd = _hbmXml.SelectSingleNode(xpath,_nsMgr);
            if (nd == null)
                return string.Empty;
            var el = nd as XmlElement;
            if (el == null)
                return string.Empty;
            var attr = el.Attributes[attrName];
            return attr == null ? string.Empty : attr.Value;
        }

        protected internal static string CreateXpath(params string[] pathParts)
        {
            var xpath = new StringBuilder();
            xpath.Append("/");
            foreach (var p in pathParts)
            {
                xpath.Append("/");
                xpath.Append(NS);
                xpath.Append(":");
                xpath.Append(p);
            }
            return xpath.ToString();
        }

        protected internal virtual void GetDependencySqlScriptStmts(Dictionary<Tuple<string, string>, HbmFileContent> includedTypes)
        {
            includedTypes = includedTypes ?? new Dictionary<Tuple<string, string>, HbmFileContent>();
            //determine if moving down or up
            //var dict = useLists ? ListProperties : FkProperties;
            foreach (var fk in FkProperties.Keys)
            {
                //get a hash of the other type name to this type name
                var myIncludedTypeKey = new Tuple<string, string>(AssemblyQualifiedTypeName, FkProperties[fk]);

                //test if this has been done already
                var otherHbmContent = GetHbmFileContentFromDictionary(includedTypes, myIncludedTypeKey);
                if (otherHbmContent == null)
                    continue;

                //for a FK this will be one of my columns - for a bag it will be a column on the other table
                var colData = GetColumnDataByPropertyName(fk);

                var cols = colData.Select(c => c.column_name.Replace(c.table_name, string.Empty).Trim('.')).ToList();

                otherHbmContent.AddSqlScriptOnWhichDepends($"SELECT {string.Join(", ", cols)} FROM {DbSchema}.{TableName} WHERE {GetSqlScriptPkString()} IN ({GetSqlScriptTblVariableSelect()})");
                otherHbmContent.GetDependencySqlScriptStmts(includedTypes);
            }

            foreach (var bag in ListProperties.Keys)
            {
                var myIncludedTypeKey = new Tuple<string, string>(AssemblyQualifiedTypeName, ListProperties[bag]);

                //test if this has been done already
                var otherHbmContent = GetHbmFileContentFromDictionary(includedTypes, myIncludedTypeKey);
                if (otherHbmContent == null)
                    continue;
                var colData = GetColumnDataByPropertyName(bag);
                var cols = colData.Select(c => c.GetDbColumnName()).ToList();

                otherHbmContent.AddSqlScriptOnWhichDepends($"SELECT {otherHbmContent.GetSqlScriptPkString()} FROM {otherHbmContent.DbSchema}.{otherHbmContent.TableName} WHERE {string.Join(", ", cols)} IN ({GetSqlScriptTblVariableSelect()})");
            }
        }

        protected internal virtual void AddSqlScriptOnWhichDepends(string stmt)
        {
            if (!string.IsNullOrWhiteSpace(stmt))
                _sqlOnWhichDepends.Add(stmt);
        }

        protected internal virtual List<string> GetSqlScriptOnWhichDepends()
        {
            var moreSql = new List<string>();
            foreach (var dkk in _sqlOnWhichDepends)
            {
                moreSql.Add(GetSqlScriptTblVariableInsert());
                moreSql.Add(dkk);
            }
            return moreSql;
        }

        protected internal virtual string GetSqlScriptTblVariableDeclare()
        {
            var tbl = GetSqlScriptTableVariableName();
            var pk = GetSqlScriptPkString(true);
            return $"DECLARE {tbl} TABLE ({pk})";
        }

        protected internal virtual string GetSqlScriptTblVariableSelect()
        {
            var tbl = GetSqlScriptTableVariableName();
            var pk = GetSqlScriptPkString();
            return $"SELECT {pk} FROM {tbl}";
        }

        protected internal virtual string GetSqlScriptTblVariableInsert()
        {
            var tbl = GetSqlScriptTableVariableName();
            var pk = GetSqlScriptPkString();
            return $"INSERT INTO {tbl} ({pk})";
        }

        protected internal virtual string GetSqlScriptPkString(bool withAsDcl = false)
        {
            var colData = IdAsSimpleProperties.SelectMany(x => GetColumnDataByPropertyName(x.Key));

            if (!withAsDcl)
                return string.Join(", ", colData.Select(x => x.GetDbColumnName()));

            var ssqlPk = new List<string>();
            foreach (var ids in colData)
            {
                ssqlPk.Add($"{ids.GetDbColumnName()} {ids.data_type.ToUpper()}");
            }
            return string.Join(", ", ssqlPk);
        }

        protected internal virtual string GetSqlScriptTableVariableName()
        {
            if (!string.IsNullOrWhiteSpace(_sqlTbl))
                return _sqlTbl;
            var sqlSchema = DbSchema.Replace("[", "").Replace("]", "").ToLower();
            var sqlTbl = TableName.Replace("[", "").Replace("]", "").ToLower();
            _sqlTbl = $"@{sqlSchema}_{sqlTbl}";

            return _sqlTbl;
        }

        internal static HbmFileContent GetHbmFileContentFromDictionary(Dictionary<Tuple<string, string>, HbmFileContent> includedTypes, Tuple<string, string> myNameOtherName)
        {
            if (includedTypes == null || string.IsNullOrWhiteSpace(myNameOtherName?.Item1) || string.IsNullOrWhiteSpace(myNameOtherName.Item2))
                return null;

            var otherTypeFullName = myNameOtherName.Item2;
            var myFullName = myNameOtherName.Item1;
            var myIncludedTypeKey = new Tuple<string, string>(myFullName, otherTypeFullName);

            //test if this has been done already
            HbmFileContent otherHbmContent = null;
            if (includedTypes.ContainsKey(myIncludedTypeKey))
                return null;

            //imporve performance by looking for hbm file content already parsed when available
            otherHbmContent =
                includedTypes.FirstOrDefault(x => includedTypes[x.Key].AssemblyQualifiedTypeName == otherTypeFullName).Value;

            if (otherHbmContent == null)
            {
                var fkHbmXml = Compose.HbmFileNameFromAsmQualTypeName(otherTypeFullName, true);
                if (File.Exists(fkHbmXml))
                    otherHbmContent = new HbmFileContent(fkHbmXml);
            }

            //now hove both this hbm file content and the other's in scope
            if (otherHbmContent == null)
                return null;
            includedTypes.Add(myIncludedTypeKey, otherHbmContent);

            return otherHbmContent;
        }
        #endregion
    }

    public class HbmStoredProxNames
    {
        public string CallableName { get; set; }
        public string DbName { get; set; }
        public string[] ParamNames { get; set; }
    }
}
