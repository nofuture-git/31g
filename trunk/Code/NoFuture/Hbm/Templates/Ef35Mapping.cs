using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Shared;
using NoFuture.Util;

namespace NoFuture.Hbm.Templates
{
    public class Ef35Mapping : HbmFileContent
    {
        private readonly Dictionary<Tuple<string, string>, Ef35PropertyAttr> _ef35Dictionary;
        public Ef35Mapping(string hbmXmlFilePath) : base(hbmXmlFilePath)
        {
            _ef35Dictionary = new Dictionary<Tuple<string, string>, Ef35PropertyAttr>();
        }

        public Dictionary<Tuple<string, string>, Ef35PropertyAttr> PropertyAttributes 
        {
            get
            {
                if (_ef35Dictionary.Count > 0)
                    return _ef35Dictionary;

                var tempDict = new Dictionary<string, string>();
                foreach (var idp in IdAsSimpleProperties.Keys.Where(idp => !tempDict.ContainsKey(idp)))
                {
                    var idpMds = GetColumnDataByPropertyName(idp);
                    if (idpMds == null || idpMds.Length <= 0)
                        continue;
                    var originalName = idp;
                    if (string.Equals(idp, Globals.HbmXmlNames.ID))
                    {
                        originalName = Compose.PropertyName(idpMds.First().column_name);
                    }
                    var tupleKey = new Tuple<string, string>(originalName, IdAsSimpleProperties[idp]);
                    if (_ef35Dictionary.ContainsKey(tupleKey))
                        continue;
                    
                    var ef35Attr = new Ef35PropertyAttr(idpMds.First(), true) {IsPrimaryKey = true};

                    _ef35Dictionary.Add(tupleKey, ef35Attr);
                    
                }
                foreach (var idp in SimpleProperties.Keys.Where(idp => !tempDict.ContainsKey(idp)))
                {
                    var idpMds = GetColumnDataByPropertyName(idp);
                    if (idpMds == null || idpMds.Length <= 0)
                        continue;

                    var tupleKey = new Tuple<string, string>(idp, SimpleProperties[idp]);
                    if (_ef35Dictionary.ContainsKey(tupleKey))
                        continue;

                    var ef35Attr = new Ef35PropertyAttr(idpMds.First(), true);

                    _ef35Dictionary.Add(tupleKey, ef35Attr);
                }
                foreach (var idp in FkProperties.Keys.Where(idp => !tempDict.ContainsKey(idp)))
                {
                    var idpMds = GetColumnDataByPropertyName(idp);
                    if (idpMds == null || idpMds.Length <= 0)
                        continue;

                    var md = idpMds.First();
                    var origTypeName = md.data_type;

                    //undo the complexity of fk's as types and return them to simple primitives
                    if (Lexicon.Mssql2HbmTypes.ContainsKey(md.data_type))
                    {
                        var hbmType = Lexicon.Mssql2HbmTypes[md.data_type];
                        if (Lexicon.Hbm2NetTypes.ContainsKey(hbmType))
                            origTypeName = Lexicon.Hbm2NetTypes[hbmType];
                    }

                    var composedName = Compose.PropertyName(md.column_name);

                    var tupleKey = new Tuple<string, string>(composedName, origTypeName);
                    if (_ef35Dictionary.ContainsKey(tupleKey))
                        continue;

                    var ef35Attr = new Ef35PropertyAttr(md, true);

                    _ef35Dictionary.Add(tupleKey, ef35Attr);

                }

                return _ef35Dictionary;
            }
        }

        public override string TableName 
        { 
            get
            {
                return string.Format("{0}.{1}", DbSchema, _tableName)
                    .Replace("[", string.Empty)
                    .Replace("]", string.Empty);
            } 
        }

    }

    public class Ef35PropertyAttr
    {
        private readonly string _composedName;

        public Ef35PropertyAttr() { }

        public Ef35PropertyAttr(ColumnMetadata md, bool addUpdateCheckNever)
        {
            Name = Etc.ExtractLastWholeWord(md.column_name, NfConfig.DEFAULT_TYPE_SEPARATOR);
            _composedName = Compose.PropertyName(md.column_name);
            DbType = md.data_type.ToUpper();

            if (Lexicon.Mssql2HbmTypes.ContainsKey(md.data_type) && Lexicon.Mssql2HbmTypes[md.data_type] == "AnsiString")
            {
                if (md.string_length != null && md.string_length < 0)
                {
                    DbType = string.Format("{0}(MAX)", DbType);
                }
                else
                {
                    DbType = string.Format("{0}({1})", DbType, md.string_length);    
                }
                
            }
            if (Globals.MssqlTypesWithPrecision.Contains(md.data_type))
            {
                DbType = string.Format("{0}({1})", DbType, md.precision);
            }

            if (md.is_nullable != null && md.is_nullable.Value == false)
            {
                DbType = string.Format("{0} NOT NULL", DbType);
                CanBeNull = false;
            }
            else
            {
                CanBeNull = true;
            }

            if (md.is_auto_increment != null && md.is_auto_increment.Value)
            {
                DbType = string.Format("{0} IDENTITY", DbType);
                IsPrimaryKey = true;
                IsDbGenerated = true;
            }
            IsUpdateCheckNever = addUpdateCheckNever;
        }

        public string Name { get;set; }
        public string DbType { get; set; }
        public bool IsDbGenerated { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsUpdateCheckNever { get; set; }
        public bool CanBeNull { get; set; }

        public override string ToString()
        {
            var attr = new StringBuilder();
            attr.Append("[");
            attr.Append("ColumnAttribute(");

            attr.AppendFormat("Name = \"{0}\"", Name);

            attr.AppendFormat(", Storage = \"_{0}\"", _composedName);
            attr.AppendFormat(", DbType = \"{0}\"", DbType);

            if (IsDbGenerated)
                attr.Append(", IsDbGenerated = true");

            if (IsPrimaryKey)
                attr.Append(", IsPrimaryKey = true");

            if (IsUpdateCheckNever)
                attr.Append(", UpdateCheck=UpdateCheck.Never");

            attr.Append(")]");
            return attr.ToString();
        }
    }
}
