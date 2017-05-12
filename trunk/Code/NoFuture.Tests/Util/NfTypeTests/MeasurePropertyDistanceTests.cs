﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Util.NfTypeTests
{
    [TestClass]
    public class MeasurePropertyDistanceTests
    {
        [TestMethod]
        public void TestTryAssignProperties()
        {
            var testInput = new TestDtoLikeType
            {
                FirstName = "Eugene",
                LastName = "Krabs",
                Line1 = "123 Anchor Way",
                City = "Bikini Bottom",
                CountryCode = "US",
                Gender = Gender.Male,
                Email = "e.krabs@bikinibottom.net",
                EntityId = 789456,
                AddressId = "41255"
            };

            var testOutput = new Entity();
            var testResult = NoFuture.Util.NfType.MeasurePropertyDistance.TryAssignProperties(testInput, testOutput,
                "FirstName", "LastName", "Line1", "City", "CountryCode", "Gender", "Email", "EntityId", "AddressId");
            Assert.IsTrue(testResult);
            
            
            Assert.IsNotNull(testOutput.Gender);

            //test top-level properties are being copied over
            Assert.AreEqual(Gender.Male, testOutput.Gender);
            Assert.AreEqual(789456, testOutput.Id);

            //test child types are being both instantiated and having properties assigned
            Assert.IsNotNull(testOutput.Name);
            Assert.AreEqual("Eugene", testOutput.Name.First);
            Assert.AreEqual("Krabs", testOutput.Name.Last);

            Assert.IsNotNull(testOutput.Address);
            Assert.AreEqual("123 Anchor Way", testOutput.Address.Line1);
            Assert.AreEqual("Bikini Bottom", testOutput.Address.City);
            Assert.AreEqual("US", testOutput.Address.CountryCode);
            Assert.AreEqual(41255,testOutput.Address.Id);

            Assert.IsNotNull(testOutput.Contact);
            Assert.AreEqual("e.krabs@bikinibottom.net", testOutput.Contact.Email);
        }

        [TestMethod]
        public void TestGetPropertyValuetype()
        {
            var testContext = new Address();
            var testInput = testContext.GetType().GetProperty("Id");
            var testResult = NoFuture.Util.NfType.NfTypeName.GetPropertyValueType(testInput);

            Assert.IsNotNull(testResult);
            Assert.AreEqual("System.Int32", testResult.FullName);
            System.Diagnostics.Debug.WriteLine(testResult.FullName);
        }
    }

    public class TestDtoLikeType
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int EntityId { get; set; }
        public string AddressId { get; set; }
    }

    public class Entity
    {
        public int Id { get; set; }
        public PersonName Name { get; set; }
        public Address Address { get; set; }
        public Gender? Gender { get; set; }
        public Contact Contact { get; set; }
    }

    public class PersonName
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Middle { get; set; }

    }

    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public int? Id { get; set; }
    }

    public class Contact
    {
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}