using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
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

        [Test]
        public void TestToData()
        {
            var testSubject = new IObviate[]
            {
                SocBoardGroup.RandomSocBoardGroup(), SocMajorGroup.RandomSocMajorGroup(),
                SocMinorGroup.RandomSocMinorGroup(), SocDetailedOccupation.RandomSocDetailedOccupation()
            };

            foreach (var ts in testSubject)
            {
                Console.WriteLine();
                var testResult = ts.ToData(KindsOfTextCase.Kabab);
                Assert.IsNotNull(testResult);
                Assert.AreNotEqual(0, testResult.Count);
                foreach (var k in testResult.Keys)
                    Console.WriteLine($"{k}: {testResult[k]}");
            }
        }
    }
}
