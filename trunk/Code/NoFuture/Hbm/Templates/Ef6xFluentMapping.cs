using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Util.Core;

namespace NoFuture.Hbm.Templates
{
    public class Ef6XFluentMapping : HbmFileContent
    {
        private readonly Dictionary<string, string> _efProperties;
        public Ef6XFluentMapping(string hbmXmlFilePath) : base(hbmXmlFilePath)
        {
            
            _efProperties = new Dictionary<string, string>();
            if (IsCompositeKey)
            {
                foreach (var idKey in IdAsSimpleProperties.Keys)
                {
                    _efProperties.Add(idKey, IdAsSimpleProperties[idKey]);
                }
            }
            else
            {
                _efProperties.Add(IdName, IdType);
            }

            foreach (var prop in SimpleProperties.Keys)
            {
                if (_efProperties.ContainsKey(prop))
                    continue;
                _efProperties.Add(prop, SimpleProperties[prop]);
            }
        }

        public string EntityKey
        {
            get
            {
                if (IsCompositeKey)
                {
                    var annoymousEncl = new StringBuilder();
                    annoymousEncl.Append(string.Join(", x.", IdAsSimpleProperties.Select(b => b.Key)));
                    return annoymousEncl.ToString();
                }
                return IdName;
            }
        }

        public override string TableName { get { return _tableName.Replace("[", string.Empty).Replace("]", string.Empty); } }

        public List<EfSimpleProp> EfProperties
        {
            get
            {
                var efSimpleProps = _efProperties.Keys.Select(GetSimpleProp).Where(efsprop => efsprop != null).ToList();
                efSimpleProps.AddRange(FkProperties.Keys.Select(GetSimpleProp));
                return efSimpleProps;
            }
        }

        internal int? GetStringLength(int? length, bool isUnicode)
        {
            if (!isUnicode || length == null)
                return length;
            var lenVal = length.Value;
            var halfLenVal = Math.Round((double)lenVal/2, 0);
            return (int) halfLenVal;

        }

        public Dictionary<string, string> EfForeignKeys
        {
            get
            {
                var efFks = new Dictionary<string, string>();
                foreach (var fk in FkProperties.Keys)
                {
                    var otherTypesPropName = Compose.BagPropertyName(AssemblyQualifiedTypeName);
                    efFks.Add(fk, otherTypesPropName);
                }
                return efFks;
            }
        }

        public Dictionary<string, string> EfListProps
        {
            get
            {
                var efLists = new Dictionary<string, string>();
                foreach (var bag in ListProperties.Keys)
                {
                    var propColMetadataList = GetColumnDataByPropertyName(bag);
                    if (propColMetadataList == null || propColMetadataList.Length <= 0)
                        continue;

                    var bagColumnNames = propColMetadataList.Select(x => x.column_name).ToArray();
                    var otherTypesPropName = Compose.ManyToOnePropertyName(
                        AssemblyQualifiedTypeName, bagColumnNames);
                    efLists.Add(bag, otherTypesPropName);
                }
                return efLists;
            }
        }

        private EfSimpleProp GetSimpleProp(string propName)
        {
            var propColMetadataList = GetColumnDataByPropertyName(propName);
            if (propColMetadataList == null || propColMetadataList.Length <= 0)
                return null;
            var propColMetadata = propColMetadataList.First();

            var isString = false;
            if (!_efProperties.ContainsKey(propName) && Util.Lexicon.Mssql2HbmTypes.ContainsKey(propColMetadata.data_type))
            {
                isString = Util.Lexicon.Mssql2HbmTypes[propColMetadata.data_type] == "AnsiString";
            }
            else
            {
                isString = _efProperties[propName] == "string";
            }
            var efsprop = new EfSimpleProp { IsString = isString };

            efsprop.IsUnicode = efsprop.IsString && propColMetadata.data_type.StartsWith("n");
            efsprop.ColumnName = Etc.ExtractLastWholeWord(propColMetadata.column_name.Replace("[", string.Empty).Replace("]", string.Empty), null);
            efsprop.RequiresPrecision = Globals.MssqlTypesWithPrecision.Contains(propColMetadata.data_type);
            //this exports doubled when it is unicode
            efsprop.StringLength = GetStringLength(propColMetadata.string_length, efsprop.IsUnicode);
            efsprop.Precision = propColMetadata.precision;
            efsprop.PropName = propName;
            return efsprop;
        }
    }

    public class EfSimpleProp
    {
        private string _propName;
        private string _columnName;
        private bool _isUnicode;
        private bool _isString;
        private int? _stringLength;
        private bool _requiresPrecision;
        private string _precision;

        public string PropName
        {
            get { return _propName ?? string.Empty; }
            set { _propName = value; }
        }

        public string ColumnName
        {
            get { return _columnName ?? string.Empty; }
            set { _columnName = value; }
        }

        public bool IsUnicode
        {
            get { return _isUnicode; }
            set { _isUnicode = value; }
        }

        public bool IsString
        {
            get { return _isString; }
            set { _isString = value; }
        }

        public int? StringLength
        {
            get { return _stringLength ?? 8000; }
            set { _stringLength = value; }
        }

        public bool RequiresPrecision
        {
            get { return _requiresPrecision; }
            set { _requiresPrecision = value; }
        }

        public string Precision
        {
            get { return _precision ?? string.Empty; }
            set { _precision = value; }
        }
    }
}
