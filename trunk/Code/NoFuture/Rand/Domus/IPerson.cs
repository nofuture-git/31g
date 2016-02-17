using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Domus
{
    public interface IPerson
    {
        DateTime? BirthDate { get; set; }
        DateTime? DeathDate { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Gender MyGender { get; set; }
        List<Uri> NetUri { get; }
        MaritialStatus MaritalStatus { get; set; }
        IPerson Spouse { get; set; }
        IPerson Father { get; set; }
        IPerson Mother { get; set; }
        List<IPerson> Children { get; }
        Pneuma.Personality Personality { get; }
        IEducation Education { get; set; }
        List<Tuple<KindsOfPersonalNames, string>> OtherNames { get; }

    }
}