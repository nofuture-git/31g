using System;
using System.Collections.Generic;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Domus
{
    public interface IPerson : IVoca
    {
        #region properties
        int Age { get; }
        BirthCert BirthCert { get; }
        DateTime? DeathDate { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Gender MyGender { get; set; }
        List<Uri> NetUri { get; }
        Pneuma.Personality Personality { get; }
        IEducation Education { get; }
        Spouse Spouse { get; }
        MaritialStatus MaritialStatus { get; }
        IEnumerable<Child> Children { get; }
        List<Tuple<KindsOfNames, Parent>> Parents { get; }
        ResidentAddress Address { get; }

        #endregion

        #region methods
        /// <summary>
        /// Resolves the children of this instance at
        /// time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        List<Child> GetChildrenAt(DateTime? dt);

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
        /// Resolve the edu of this instance at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        IEducation GetEducationAt(DateTime? dt);

        /// <summary>
        /// Resolves the financial state and history of the person
        /// at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Opes GetWealthAt(DateTime? dt);

        #endregion
    }
}