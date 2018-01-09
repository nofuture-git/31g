using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Domus
{
    public interface IPerson : IVoca
    {
        #region properties
        int Age { get; }
        BirthCert BirthCert { get; }
        DeathCert DeathCert { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Gender MyGender { get; set; }
        IEnumerable<Uri> NetUri { get; }
        Pneuma.Personality Personality { get; }
        IEducation Education { get; }
        IRelation Spouse { get; }
        MaritialStatus MaritialStatus { get; }
        IEnumerable<Child> Children { get; }
        IEnumerable<Tuple<KindsOfNames, Parent>> Parents { get; }
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

        #endregion
    }
}