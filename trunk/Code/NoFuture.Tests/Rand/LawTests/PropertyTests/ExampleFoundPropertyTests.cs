using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.Property.US.Found;
using NoFuture.Rand.Law.US;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Property.Tests
{
    [TestFixture]
    public class ExampleFoundPropertyTests
    {
        private ILegalPerson _propertyOwner = new LegalPerson("Jim Owner");

        [Test]
        public void TestAbandonedProperty()
        {
            var test = new AbandonedProperty(ps => ps.FirstOrDefault(p => p.IsSamePerson(_propertyOwner)))
            {

            };
        }
    }
}
