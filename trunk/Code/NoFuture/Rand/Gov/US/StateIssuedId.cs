using System;

namespace NoFuture.Rand.Gov.US
{
    /// <summary>
    /// See https://en.wikipedia.org/wiki/REAL_ID_Act#Data_requirements
    /// </summary>
    [Serializable]
    public abstract class StateIssuedId : GovernmentId
    {
        protected internal UsState IssuingState { get; set; }
        public string StateAbbrev => IssuingState?.StateAbbrev;
        public string FullLegalName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string PrincipalResidence { get; set; }
        public byte[] FrontFacingPhoto { get; set; }
        public byte[] Signature { get; set; }

    }
}