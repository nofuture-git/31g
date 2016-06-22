﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Data.Types;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public abstract class GovernmentId : RIdentifier
    {
        public abstract List<Anomaly> Anomalies { get; }
        public DateTime? IssuedDate { get; set; }
    }

    [Serializable]
    public abstract class StateIssuedId : GovernmentId
    {
        public UsState IssuingState { get; set; }

        //https://en.wikipedia.org/wiki/REAL_ID_Act#Data_requirements
        public string FullLegalName { get; set; }
        public DateTime Dob { get; set; }
        public Gender Gender { get; set; }
        public string PrincipalResidence { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public System.Drawing.Bitmap FrontFacingPhoto { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public System.Drawing.Bitmap Signature { get; set; }

    }

    [Serializable]
    public abstract class Anomaly : Identifier { }

    #region address anomalies

    /// <summary>
    /// Derived from Equifax document
    /// [Equifax_Fall_2014_Release_Guide.pdf]
    /// </summary>
    [Serializable]
    public abstract class AddressAnomaly : Anomaly { }

    [Serializable]
    public class AddressIsCorrectionalFacility : AddressAnomaly
    {
        public override string Abbrev { get { return "P"; } }
    }
    [Serializable]
    public class AddressIsMailReceivingService : AddressAnomaly
    {
        public override string Abbrev { get { return "Z"; } }
    }
    [Serializable]
    public class AddressIsHotel : AddressAnomaly
    {
        public override string Abbrev { get { return "J"; } }
    }
    [Serializable]
    public class AddressIsCampground : AddressAnomaly
    {
        public override string Abbrev { get { return "H"; } }
    }
    [Serializable]
    public class AddressIsPoBox : AddressAnomaly
    {
        public override string Abbrev { get { return "G"; } }
    }
    [Serializable]
    public class AddressIsNonResidential : AddressAnomaly
    {
        public override string Abbrev { get { return "W"; } }
    }
    [Serializable]
    public class AddressIsMultiDwelling : AddressAnomaly
    {
        public override string Abbrev { get { return "1"; } }
    }
    [Serializable]
    public class AddressIsReportedAsMisused : AddressAnomaly
    {
        public override string Abbrev { get { return "2"; } }
    }
    [Serializable]
    public class AddressIsAmbiguousIdentifier : AddressAnomaly
    {
        public override string Abbrev { get { return "C"; } }
    }
    [Serializable]
    public class AddressIsUnverifiable : AddressAnomaly
    {
        public override string Abbrev { get { return "D"; } }
    }
    #endregion
}
