using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Gov
{
    /// <summary>
    /// A general form of a government Birth Certificate
    /// </summary>
    [Serializable]
    public class BirthCert : VitalRecord, IObviate
    {
        public BirthCert(string personFullName)
            : base(personFullName)
        {
        }

        public BirthCert(IVoca personName) : base(personName)
        {
        }

        public BirthCert(DateTime dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
        }

        public BirthCert()
        {
        }

        public virtual string MotherName { get; set; }
        public virtual string FatherName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string GetFatherSurname()
        {
            var fn = FatherName ?? "";
            return fn.Split(' ').Last();
        }

        public override string ToString()
        {
            return DateOfBirth.ToString("yyyy-M-dd");
        }

        public virtual IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object> {{textFormat(nameof(DateOfBirth)), DateOfBirth.ToString("s")}};

            return itemData;
        }
    }
}