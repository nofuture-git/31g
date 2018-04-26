using NUnit.Framework;
using NoFuture.Rand.Org;

namespace NoFuture.Rand.Tests.OrgTests
{
    [TestFixture]
    public class StandardOccupationalTests
    {
        [Test]
        public void TestSocMajorGroups()
        {
            var testResult = StandardOccupationalClassification.AllGroups;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach (var majorGrp in testResult)
            {
                Assert.IsInstanceOf<SocMajorGroup>(majorGrp);
                System.Diagnostics.Debug.WriteLine($"{majorGrp.Value} {majorGrp.Description}");
                foreach (var minorGrp in majorGrp.GetDivisions())
                {
                    System.Diagnostics.Debug.WriteLine($"\t\t{minorGrp.Description}");
                    foreach (var boardGrp in minorGrp.GetDivisions())
                    {
                        foreach (var detailGrp in boardGrp.GetDivisions())
                        {
                            foreach (var cip in detailGrp.GetDivisions())
                            {
                                Assert.IsInstanceOf<ClassificationOfInstructionalPrograms>(cip);
                                System.Diagnostics.Debug.WriteLine($"\t\t\t{cip.Description}");
                            }
                        }
                    }
                }
            }
        }
    }
}
