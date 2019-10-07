using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Rand.Gov.US
{
    /// <summary>
    /// The general form of an American Birth Certificate
    /// </summary>
    [Serializable]
    public class AmericanBirthCert : BirthCert
    {
        public AmericanBirthCert(string personFullName) : base(personFullName)
        {
        }

        public AmericanBirthCert(IVoca personName) : base((IVoca) null)
        {
            if (personName == null)
                return;

            PersonFullName = NfString.DistillSpaces(
                string.Join(" ", personName.GetName(KindsOfNames.First),
                    personName.GetName(KindsOfNames.Middle),
                    personName.GetName(KindsOfNames.Surname)));

            if (string.IsNullOrWhiteSpace(PersonFullName))
                PersonFullName = personName.GetName(KindsOfNames.Legal);
        }

        public AmericanBirthCert(DateTime dateOfBirth) : base(dateOfBirth)
        {

        }

        public AmericanBirthCert()
        {
        }

        public string State { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), City, State);
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = base.ToData(txtCase) ?? new Dictionary<string, object>();
            if(!string.IsNullOrWhiteSpace(State))
                itemData.Add(textFormat("BirthState"), State);
            if(!string.IsNullOrWhiteSpace(City))
                itemData.Add(textFormat("BirthCity"), City);
            return itemData;
        }
    }
}