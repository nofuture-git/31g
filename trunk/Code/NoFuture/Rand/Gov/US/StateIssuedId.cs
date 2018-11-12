using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

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
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PrincipalResidence { get; set; }
        public byte[] FrontFacingPhoto { get; set; }
        public byte[] Signature { get; set; }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = base.ToData(txtCase) ??  new Dictionary<string, object>();
            if(!string.IsNullOrWhiteSpace(StateAbbrev))
                itemData.Add(textFormat(nameof(IssuingState)), StateAbbrev);
            if(!string.IsNullOrWhiteSpace(FullLegalName))
                itemData.Add(textFormat(nameof(FullLegalName)), FullLegalName);
            if(DateOfBirth != DateTime.MinValue)
                itemData.Add(textFormat(nameof(DateOfBirth)), DateOfBirth.ToString("s"));
            if(!string.IsNullOrWhiteSpace(Gender))
                itemData.Add(textFormat(nameof(Gender)), Gender);
            if(!string.IsNullOrWhiteSpace(PrincipalResidence))
                itemData.Add(textFormat(nameof(PrincipalResidence)), PrincipalResidence);
            if (FrontFacingPhoto != null && FrontFacingPhoto.Length > 0)
            {
                var photo = Convert.ToBase64String(FrontFacingPhoto);
                itemData.Add(textFormat(nameof(FrontFacingPhoto)), photo);
            }

            if (Signature != null && Signature.Length > 0)
            {
                var sig = Convert.ToBase64String(Signature);
                itemData.Add(textFormat(nameof(Signature)), sig);
            }

            return itemData;
        }
    }
}