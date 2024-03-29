﻿using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NfString = NoFuture.Util.Core.NfString;

namespace NoFuture.Rand.Gov
{
    [Serializable]
    public abstract class VitalRecord : IVitalRecord
    {
        protected VitalRecord(string personFullName)
        {
            PersonFullName = personFullName;
        }

        protected VitalRecord(IVoca personName)
        {
            if (personName == null)
                return;

            PersonFullName = NfString.DistillSpaces(
                string.Join(" ", personName.GetName(KindsOfNames.First),
                                 personName.GetName(KindsOfNames.Surname)));

            if (string.IsNullOrWhiteSpace(PersonFullName))
                PersonFullName = personName.GetName(KindsOfNames.Legal);
        }

        protected VitalRecord()
        {

        }

        public string Src { get; set; }
        public string PersonFullName { get; set; }
    }
}
