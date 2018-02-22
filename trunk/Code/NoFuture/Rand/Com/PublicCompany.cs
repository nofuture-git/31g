﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US.Irs;
using NoFuture.Rand.Gov.US.Sec;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Com
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a publicly traded company 
    /// (a.k.a. publicly traded company, publicly held company, publicly listed company, public corporation)
    /// </summary>
    [Serializable]
    public class PublicCompany : Firm
    {
        private readonly List<SecForm> _secReports = new List<SecForm>();
        private List<TickerSymbol> _tickerSymbols = new List<TickerSymbol>();

        public EmployerIdentificationNumber EIN { get; set; }
        public CentralIndexKey CIK { get; set; }
        public List<SecForm> SecReports => _secReports;
        public string UsStateOfIncorporation { get; set; }
        public List<TickerSymbol> TickerSymbols
        {
            get
            {
                var t = _tickerSymbols.ToList();
                t.Sort(new TickerComparer(Name));
                return t;
            }
            set => _tickerSymbols = value;
        }

        public string UrlEncodedName => Util.Core.Etc.EscapeString(GetSearchCompanyName(Name), EscapeStringType.URI);

        public void AddTickerSymbol(TickerSymbol symbol)
        {
            _tickerSymbols.Add(symbol);
        }

        public override void LoadXrefXmlData()
        {
            var firmName = Name;
            if (GetType() == typeof(Bank))
            {
                firmName = GetName(KindsOfNames.Abbrev) ?? Name;
            }
            var myAssocNodes = XRefGroup.GetXrefAddNodes(GetType(), firmName);
            if (myAssocNodes == null || myAssocNodes.Count <= 0)
                return;

            foreach (var node in myAssocNodes)
            {
                var elem = node as XmlElement;
                if (elem == null)
                    continue;

                XRefGroup.SetTypeXrefValue(elem, this);
            }
        }

    }
}