using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Gov
{
    /// <summary>
    /// A general form of a government Death Certificate
    /// </summary>
    [Serializable]
    public class DeathCert : VitalRecord, IObviate
    {
        public DeathCert(string personFullName) : base(personFullName)
        {
        }

        public DeathCert(IVoca personName) : base(personName)
        {
        }

        public DeathCert(DateTime dateOfDeath)
        {
            DateOfDeath = dateOfDeath;
        }

        public DeathCert()
        {

        }

        public DateTime DateOfDeath { get; set; }

        public override string ToString()
        {
            return DateOfDeath.ToString("yyyy-M-dd");
        }

        public virtual IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = new Dictionary<string, object> { { textFormat(nameof(DateOfDeath)), DateOfDeath.ToString("s") } };

            return itemData;
        }
    }
}