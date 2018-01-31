using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Util.Core;

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

            PersonFullName = Etc.DistillSpaces(
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
    }
}