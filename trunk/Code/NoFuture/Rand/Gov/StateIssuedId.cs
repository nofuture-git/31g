using System;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Gov
{
    /// <summary>
    /// See https://en.wikipedia.org/wiki/REAL_ID_Act#Data_requirements
    /// </summary>
    [Serializable]
    public abstract class StateIssuedId : GovernmentId
    {
        public UsState IssuingState { get; set; }

        public string FullLegalName { get; set; }
        public DateTime Dob { get; set; }
        public Gender Gender { get; set; }
        public string PrincipalResidence { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public byte[] FrontFacingPhoto { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public byte[] Signature { get; set; }

    }
}