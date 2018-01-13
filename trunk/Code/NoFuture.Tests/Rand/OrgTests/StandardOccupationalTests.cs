using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Org;

namespace NoFuture.Rand.Tests.OrgTests
{
    [TestClass]
    public class StandardOccupationalTests
    {
        [TestMethod]
        public void TestSocMajorGroups()
        {
            var testResult = StandardOccupationalClassification.AllGroups;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach (var majorGrp in testResult)
            {
                Assert.IsInstanceOfType(majorGrp, typeof(SocMajorGroup));
                System.Diagnostics.Debug.WriteLine($"{majorGrp.Value} {majorGrp.Description}");
                foreach (var minorGrp in majorGrp.Divisions)
                {
                    System.Diagnostics.Debug.WriteLine($"\t\t{minorGrp.Description}");
                    foreach (var boardGrp in minorGrp.Divisions)
                    {
                        foreach (var detailGrp in boardGrp.Divisions)
                        {
                            foreach (var cip in detailGrp.Divisions)
                            {
                                Assert.IsInstanceOfType(cip, typeof(ClassificationOfInstructionalPrograms));
                                System.Diagnostics.Debug.WriteLine($"\t\t\t{cip.Description}");
                            }
                        }
                    }
                }
            }
        }
    }
}
