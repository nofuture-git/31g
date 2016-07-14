using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Util.Gia.GraphViz;

namespace NoFuture.Util.Gia
{
    /// <summary>
    /// Represents the individual entries of a <see cref="FlattenedLine"/>
    /// </summary>
    public class FlattenedItem
    {
        private static List<string> _ffVTypes;

        public static List<string> ValueTypesList
        {
            get
            {
                if (_ffVTypes == null)
                {
                    _ffVTypes = new List<string>();
                    _ffVTypes = Lexicon.ValueType2Cs.Keys.ToList();
                    _ffVTypes.Add("System.DateTime");
                    _ffVTypes.Add("System.Enum");
                }
                return _ffVTypes;
            }
        }

        public FlattenedItem(Type flType)
        {
            FlType = flType;
            IsEnumerable = NfTypeName.IsEnumerableReturnType(flType);
            TypeFullName = NfTypeName.GetLastTypeNameFromArrayAndGeneric(flType);
            MetadataToken = flType?.MetadataToken ?? 0;
        }

        [Newtonsoft.Json.JsonIgnore]
        public Type FlType { get; private set; }

        public bool IsEnumerable { get; set; }
        public string FlName { get; set; }
        public string TypeFullName { get; set; }
        public string SimpleTypeName => NfTypeName.GetTypeNameWithoutNamespace(TypeFullName);
        public bool IsTerminalNode => ValueTypesList.Contains(TypeFullName);
        public int MetadataToken { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as FlattenedItem);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + MetadataToken;
        }

        public bool Equals(FlattenedItem item)
        {
            if (item == null)
                return false;
            if(item.MetadataToken > 0 && MetadataToken > 0)
                return item.MetadataToken == MetadataToken;

            return string.Equals(item.TypeFullName, TypeFullName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(item.FlName, FlName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Specific to graph-viz (ver. 2.38+)
        /// see [http://www.graphviz.org/]
        /// </summary>
        /// <returns></returns>
        public string ToGraphVizString()
        {
            if (IsTerminalNode)
            {
                return $"{FlName} : {SimpleTypeName}";
            }
            return string.Format("<{0}{1}> {1}", Mrecord.PROPERTY_PORT_PREFIX, FlName);
        }
    }

    public class FlattenedItemComparer : IEqualityComparer<FlattenedItem>
    {
        public bool Equals(FlattenedItem x, FlattenedItem y)
        {
            if (x == null || y == null)
                return false;
            return x.Equals(y);
        }

        public int GetHashCode(FlattenedItem obj)
        {
            return obj.TypeFullName.GetHashCode() + obj.FlName.GetHashCode();
        }
    }
}