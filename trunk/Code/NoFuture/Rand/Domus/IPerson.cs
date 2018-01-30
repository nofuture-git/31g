using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Edu;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Pneuma;

namespace NoFuture.Rand.Domus
{
    /// <summary>
    /// A general representation of a person
    /// </summary>
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
        Personality Personality { get; set; }
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
        /// Gets the <see cref="Domus.Spouse"/> at 
        /// <see cref="dt"/>, or 308 days befor or after <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time</param>
        /// <returns></returns>
        Spouse GetSpouseNear(DateTime? dt);

        /// <summary>
        /// Resolve the edu of this instance at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        IEducation GetEducationAt(DateTime? dt);

        /// <summary>
        /// Resolves the <see cref="PostalAddress"/> which was current 
        /// at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">Null for the current time right now.</param>
        /// <returns></returns>
        PostalAddress GetAddressAt(DateTime? dt);

        /// <summary>
        /// Helper method to get the age at time <see cref="atTime"/>
        /// </summary>
        /// <param name="atTime">Null for the current time right now.</param>
        /// <returns></returns>
        int GetAgeAt(DateTime? atTime);

        /// <summary>
        /// Adds the given postal address to the person
        /// </summary>
        /// <param name="addr"></param>
        void AddAddress(PostalAddress addr);

        /// <summary>
        /// Adds the given person as a parent of <see cref="parentalTitle"/> kind.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parentalTitle">
        /// Typically this is Biological &amp; Mother or Father
        /// </param>
        void AddParent(IPerson parent, KindsOfNames parentalTitle);

        /// <summary>
        /// Handles detail of adding a assigning a spouse to this instance and 
        /// reciprocating such assignment to the <see cref="spouse"/>.  Additionaly
        /// handles the assignment of names (i.e. <see cref="KindsOfNames.Surname"/> 
        /// and <see cref="KindsOfNames.Maiden"/>).
        /// </summary>
        /// <param name="spouse"></param>
        /// <param name="marriedOn"></param>
        /// <param name="separatedOn"></param>
        void AddSpouse(IPerson spouse, DateTime marriedOn, DateTime? separatedOn = null);

        /// <summary>
        /// Adds the <see cref="IPerson"/> as a child of this 
        /// person assuming the child$&apos;s birth date is rational in comparision
        /// </summary>
        /// <param name="child"></param>
        /// <param name="myParentalTitle">
        /// The default is Biological Mother or Father - use this 
        /// to override that default (e.g. Adopted, etc.)
        /// </param>
        void AddChild(IPerson child, KindsOfNames? myParentalTitle = null);

        /// <summary>
        /// Adds the given <see cref="uri"/> to this person
        /// </summary>
        /// <param name="uri"></param>
        void AddUri(Uri uri);

        #endregion
    }
}