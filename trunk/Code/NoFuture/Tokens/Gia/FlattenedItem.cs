﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Tokens.Gia.GraphViz;
using NoFuture.Util.Core;

namespace NoFuture.Tokens.Gia
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
                    _ffVTypes = NfReflect.ValueType2Cs.Keys.ToList();
                    _ffVTypes.Add("System.DateTime");
                    _ffVTypes.Add("System.Enum");
                    _ffVTypes.Add("System.Guid");
                }
                return _ffVTypes;
            }
        }

        public FlattenedItem(Type flType)
        {
            FlType = flType;
            IsEnumerable = NfReflect.IsEnumerableReturnType(flType);
            TypeFullName = NfReflect.GetLastTypeNameFromArrayAndGeneric(flType);
            MetadataToken = flType?.MetadataToken ?? 0;
        }

        [Newtonsoft.Json.JsonIgnore]
        public Type FlType { get; private set; }

        public bool IsEnumerable { get; set; }
        public string FlName { get; set; }
        public string TypeFullName { get; set; }
        public string SimpleTypeName => NfReflect.GetTypeNameWithoutNamespace(TypeFullName);
        public bool IsTerminalNode => ValueTypesList.Contains(TypeFullName);
        public int MetadataToken { get; set; }

        public bool Equals(FlattenedItem item)
        {
            if (item == null)
                return false;

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
            return string.Format("<{0}{1}> {1}", FlattenMrecord.PROPERTY_PORT_PREFIX, FlName);
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