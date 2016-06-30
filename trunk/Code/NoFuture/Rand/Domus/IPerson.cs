using System;
using System.Collections.Generic;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Domus
{
    public interface IPerson
    {
        #region properties
        BirthCert BirthCert { get; }
        DateTime? DeathDate { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Gender MyGender { get; set; }
        List<Uri> NetUri { get; }
        Pneuma.Personality Personality { get; }
        IEducation Education { get; }
        List<Tuple<KindsOfNames, string>> OtherNames { get; }
        #endregion

        #region methods
        /// <summary>
        /// Resolves the children of this instance at
        /// time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        List<IPerson> GetChildrenAt(DateTime? dt);

        /// <summary>
        /// Resolves the <see cref="MaritialStatus"/>
        /// of this instance at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        MaritialStatus GetMaritalStatusAt(DateTime? dt);

        /// <summary>
        /// Resolve the spouse of this instance
        /// at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        Spouse GetSpouseAt(DateTime? dt);

        /// <summary>
        /// Gets the mother, as another 
        /// instance of <see cref="IPerson"/>, of this instance.
        /// </summary>
        /// <returns></returns>
        IPerson GetFather();

        /// <summary>
        /// Gets the father, as another 
        /// instance of <see cref="IPerson"/>, of this instance.
        /// </summary>
        /// <returns></returns>
        IPerson GetMother();

        /// <summary>
        /// Resolve the edu of this instance at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        IEducation GetEducationAt(DateTime? dt);

        #endregion
    }
}