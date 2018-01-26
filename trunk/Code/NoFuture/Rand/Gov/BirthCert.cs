using System;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov
{
    /// <summary>
    /// A general form of a government Birth Certificate
    /// </summary>
    [Serializable]
    public class BirthCert : VitalRecord
    {
        public BirthCert(string personFullName)
            : base(personFullName)
        {
        }

        public BirthCert(IVoca personName) : base(personName)
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
    }
}