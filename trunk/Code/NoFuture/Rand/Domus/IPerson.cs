using System;
using System.Collections.Generic;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Domus
{
    public interface IPerson
    {
        BirthCert BirthCert { get; }
        DateTime? DeathDate { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Gender MyGender { get; set; }
        List<Uri> NetUri { get; }
        Pneuma.Personality Personality { get; }
        IEducation Education { get; set; }
        List<Tuple<KindsOfNames, string>> OtherNames { get; }

        List<IPerson> GetChildrenAt(DateTime? dt);
        MaritialStatus GetMaritalStatusAt(DateTime? dt);
        Spouse GetSpouseAt(DateTime? dt);
        IPerson GetFather();
        IPerson GetMother();
    }
}