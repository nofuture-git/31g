using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Domus
{
    public interface IPerson : IVoca
    {
        #region properties
        int Age { get; }
        BirthCert BirthCert { get; set; }
        DeathCert DeathCert { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Gender MyGender { get; set; }
        IEnumerable<Uri> NetUri { get; }
        Pneuma.Personality Personality { get; }
        IEducation Education { get; set; }
        IRelation Spouse { get; }
        MaritialStatus MaritialStatus { get; }
        IEnumerable<Child> Children { get; }
        IEnumerable<Parent> Parents { get; }
        PostalAddress Address { get; }
        string FullName { get; }

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
        /// Adds the given postal address to the person
        /// </summary>
        /// <param name="addr"></param>
        void AddAddress(PostalAddress addr);

        /// <summary>
        /// Resolves the <see cref="PostalAddress"/> which was current 
        /// at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        PostalAddress GetAddressAt(DateTime? dt);

        /// <summary>
        /// Adds the given person as a parent of <see cref="parentalTitle"/> kind.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parentalTitle">
        /// Typically this is Biological &amp; Mother or Father
        /// </param>
        void AddParent(IPerson parent, KindsOfNames parentalTitle);

        /// <summary>
        /// Helper method to get the age at time <see cref="atTime"/>
        /// </summary>
        /// <param name="atTime">Null for the current time right now.</param>
        /// <returns></returns>
        int GetAgeAt(DateTime? atTime);

        /// <summary>
        /// Adds the given <see cref="uri"/> to this person
        /// </summary>
        /// <param name="uri"></param>
        void AddUri(Uri uri);

        #endregion
    }
}