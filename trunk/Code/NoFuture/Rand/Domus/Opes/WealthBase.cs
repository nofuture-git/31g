﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    public abstract class WealthBase
    {
        protected internal IComparer<ITempore> Comparer { get; } = new TemporeComparer();

        protected internal virtual Pondus[] GetCurrent(List<Pondus> items)
        {
            if (items == null)
                return null;
            var o = items.Where(x => x.ToDate == null).ToList();
            o.Sort(Comparer);
            return o.ToArray();
        }

        protected internal virtual Pondus[] GetAt(DateTime? dt, List<Pondus> items)
        {
            if (items == null)
                return null;
            var atDateItems = dt == null
                ? items.Where(x => x.ToDate == null).ToList()
                : items.Where(x => x.IsInRange(dt.Value)).ToList();
            atDateItems.Sort(Comparer);
            return atDateItems.ToArray();
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Income items
        /// from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetIncomeItemNames()
        {
            return GetDomusOpesItemNames("//income//mereo");
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Deduction items
        /// from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetDeductionItemNames()
        {
            return GetDomusOpesItemNames("//deduction//mereo");
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. household budget) from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetExpenseItemNames()
        {
            return GetDomusOpesItemNames("//expense//mereo");
        }

        /// <summary>
        /// Gets the <see cref="IMereo"/> Expense items 
        /// (i.e. real and private property) 
        /// from <see cref="Data.TreeData.UsDomusOpes"/>
        /// </summary>
        /// <returns></returns>
        public static IMereo[] GetAssetItemNames()
        {
            return GetDomusOpesItemNames("//assets//mereo");
        }

        /// <summary>
        /// Tries to parse a single &apos;mereo&apos; item
        /// from the <see cref="Data.TreeData.UsDomusOpes"/> xml
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="mereo"></param>
        /// <returns></returns>
        public static bool TryParseUsDomusOpesXml(XmlNode xmlNode, out IMereo mereo)
        {
            mereo = null;

            if (xmlNode == null)
                return false;

            if (!(xmlNode is XmlElement xmlElem))
                return false;

            var egs = new List<string>();

            var groupName = xmlElem.ParentNode is XmlElement groupElem && !groupElem.HasAttributes
                ? groupElem.GetAttribute("name")
                : "";
            var itemName = xmlElem.GetAttribute("name");
            var abbrev = xmlElem.GetAttribute("abbrev");
            if (xmlElem.HasChildNodes)
            {
                foreach (var cn in xmlElem.ChildNodes)
                {
                    if (!(cn is XmlElement childElem))
                        continue;
                    if (childElem.LocalName != "eg" || !childElem.HasAttributes)
                        continue;
                    var eg = childElem.GetAttribute("name");
                    if (String.IsNullOrWhiteSpace(eg))
                        continue;
                    egs.Add(eg);
                }
            }

            mereo = new Mereo(itemName);
            if (!string.IsNullOrWhiteSpace(abbrev))
                mereo.UpsertName(KindsOfNames.Abbrev, abbrev);
            if (!string.IsNullOrWhiteSpace(groupName))
                mereo.UpsertName(KindsOfNames.Group, groupName);
            if (egs.Any())
            {
                foreach (var eg in egs)
                    mereo.ExempliGratia.Add(eg);
            }

            return !string.IsNullOrWhiteSpace(itemName);
        }

        internal static IMereo[] GetDomusOpesItemNames(string xPath)
        {
            if (string.IsNullOrWhiteSpace(xPath))
                return null;

            var xml = Data.TreeData.UsDomusOpes;
            var nodes = xml.SelectNodes(xPath);
            if (nodes == null || nodes.Count <= 0)
                return null;
            var names = new List<IMereo>();
            foreach (var o in nodes)
            {
                if (!(o is XmlNode node))
                    continue;
                if (TryParseUsDomusOpesXml(node, out var nameOut))
                    names.Add(nameOut);
            }
            return names.ToArray();
        }
    }
}
