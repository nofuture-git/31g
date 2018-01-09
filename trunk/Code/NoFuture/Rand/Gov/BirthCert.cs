using System;
using System.Linq;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public class BirthCert : VitalRecord
    {
        public BirthCert(string personFullName)
            : base(personFullName)
        {
        }

        public override string Abbrev => "Birth certificate";
        public virtual string MotherName { get; set; }
        public virtual string FatherName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string GetFatherLastName()
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