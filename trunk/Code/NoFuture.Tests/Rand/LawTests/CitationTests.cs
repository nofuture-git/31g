using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Rand.Law.US.CaseReading;
using NUnit.Framework;

namespace NoFuture.Rand.Law.Tests
{
    [TestFixture]
    public class CitationTests
    {

        [Test]
        public void TestTryFindCaseName()
        {
            var testInput = GetCaseExample00();
            var testResults = new List<string>();

            foreach (var ln in testInput)
            {
                string id = "";
                if (Citation.TryFindCaseName(ln, out id))
                {
                    testResults.Add(id);
                }
            }

            var expectedCitations = new string[]
            {
                "Sees v. Baber, 74 N.J. 201, 217 (1977)",
                "New Jersey Div. of Youth &amp; Family Servs. v. A.W., 103 N.J. 591 (1986)",
                "In re Adoption by J.J.P., 175 N.J. Super. 420, 427 (App. Div. 1980)",
                "In Sees v. Baber, 74 N.J. 201 (1977)",
                "Sorentino v. Family &amp; Children's Soc'y of Elizabeth, 74 N.J. 313 (1977)",
                "In re Adoption of Children by D., supra, 61 N.J. at 94-95; In re Adoption by J.J.P., supra, 175 N.J. Super. at 426-28; In re N., 96 N.J. Super. 415, 423-27 (App.Div. 1967)",
                "See Fantony v. Fantony, 21 N.J. 525, 536-37 (1956)",
                "Wilke v. Culp, 196 N.J. Super. 487, 496 (App.Div. 1984)",
                "West Coast Hotel Co. v. Parrish, 300 U.S. 379, 57 S. Ct. 578, 81 L. Ed. 703 (1937)",
                "Lehr v. Robertson, 463 U.S. 248, 103 S. Ct. 2985, 77 L. Ed. 2d 614 (1983)",
                "Santosky v. Kramer, 455 U.S. 745, 102 S. Ct. 1388, 71 L. Ed. 2d 599 (1982)",
                "Zablocki v. Redhail, 434 U.S. 374, 98 S. Ct. 673, 54 L. Ed. 2d 618 (1978)",
                "Quilloin v. Walcott, 434 U.S. 246, 98 S. Ct. 549, 54 L. Ed. 2d 511 (1978)",
                "Carey v. Population Servs. Int'l, 431 U.S. 678, 97 S. Ct. 2010, 52 L. Ed. 2d 675 (1977)",
                "Roe v. Wade, 410 U.S. 113, 93 S. Ct. 705, 35 L. Ed. 2d 147 (1973)",
                "Stanley v. Illinois, 405 U.S. 645, 92 S. Ct. 1208, 31 L. Ed. 2d 551 (1972)",
                "Griswold v. Connecticut, 381 U.S. 479, 85 S. Ct. 1678, 14 L. Ed. 2d 510 (1965)",
                "Skinner v. Oklahoma, 316 U.S. 535, 62 S. Ct. 1110, 86 L. Ed. 1655 (1942)",
                "Meyer v. Nebraska, 262 U.S. 390, 43 S. Ct. 625, 67 L. Ed. 1042 (1923)",
                "See Ashwander v. Tennessee Valley Auth., 297 U.S. 288, 341, 346-48, 56 S. Ct. 466, 482-83, 80 L. Ed. 688, 707, 710-12 (1936)",
                "Franz v. United States, 707 F.2d 582, 602 (D.C. Cir.1983)",
                "In re Adoption of Child by I.T. and K.T., 164 N.J. Super. 476, 484-86 (App.Div. 1978)",
                "See Wist v. Wist, 101 N.J. 509, 513-14 (1986)",
                "Beck v. Beck, 86 N.J. 480, 496 (1981)",
                "New Jersey Div. of Youth &amp; Family Servs. v. S.S., 185 N.J. Super. 3 (App.Div.), certif. den., 91 N.J. 572 (1982)",
                "See Doe v. Kelley, 106 Mich. App. 169, 307 N.W.2d 438 (1981)",
                "Syrkowski v. Appleyard, 122 Mich. App. 506, 333 N.W.2d 90 (1983)",
                "Yates v. Keane, Nos. 9758, 9772, slip op. (Mich.Cir.Ct. Jan. 21, 1988)",
                "In Surrogate Parenting Assocs. v. Commonwealth ex. rel. Armstrong, 704 S.W.2d 209 (Ky. 1986)",
                "A. v. C., [1985] F.L.R. 445, 449 (Fam. &amp; C.A. 1978)",
                "Raleigh-Fitkin Paul Morgan Hosp. v. Anderson, 42 N.J. 421, 423 (1964)",
                "See Shelley v. Kraemer, 334 U.S. 1, 68 S. Ct. 836, 92 L. Ed. 1161 (1948)",
                "State v. Baird, 21 N.J. Eq. 384, 388 (E. &amp; A. 1869)",
                "Esposito v. Esposito, 41 N.J. 143, 145 (1963)",
                "In Beck v. Beck, 86 N.J. 480, 488 (1981)",
                "New Jersey Div. of Youth &amp; Family Servs. v. A.W., supra, 103 N.J. at 617, and the extent to which a judge \"has already engaged in weighing the evidence,\" In re Guardianship of R., 155 N.J. Super. 186, 195 (App.Div. 1977)",
            };

            foreach(var expect in expectedCitations)
                Assert.IsTrue(testResults.Any(v => string.Equals(v, expect)));
        }

        [Test]
        public void TestIsNumberEsque()
        {
            var tests = new[]
            {
                "206(d)",
                "3-54a",
                "121A-5.4(c)",
                "-17",
                "834,",
                "48c(4)",
                "2d",
                "-48a(4)",
                "-16",
                "34",
                "-54.",
                "30:4C-23."
            };

            foreach (var test in tests)
            {
                var rslt = Citation.IsNumberEsque(test);
                Console.WriteLine(new Tuple<string, bool>(test, rslt));
                Assert.IsTrue(rslt);
            }
        }

        [Test]
        public void TestTrimStopWords()
        {
            var testResult = Citation.TrimStopWords("in in of and, to N.J.S.A. 9:2 -17,".Split(' ').ToList());
            Assert.AreEqual("N.J.S.A. 9:2 -17,", string.Join(" ", testResult));
        }

        [Test]
        public void TestTryFindStatute()
        {
            var testInput = GetCaseExample00();
            var testResults = new List<string>();

            foreach (var ln in testInput)
            {
                string id = "";
                if (Citation.TryFindStatute(ln, out id))
                {
                    testResults.Add(id);
                }
            }

            var expectedCitations = new string[]
            {
                "N.J.S.A. 9:17-43a(1), -44a.",
                "N.J.S.A. 9:3-54.",
                "N.J.S.A. 9:3-54.",
                "N.J.S.A. 9:2-16 and -17,",
                "N.J.A.C. 10:121A-5.4(c).",
                "N.J.S.A. 9:3-54a.",
                "N.J.S.A. 9:3-54c.",
                "N.J.S.A. 9:3-38a)",
                "N.J.S.A. 9:3-54b.",
                "9:3-54c, N.J.S.A. 2C:43-1b,",
                "Division of Youth and Family Services N.J.S.A. 9:2-16, -17; N.J.S.A. 9:3-41; N.J.S.A. 30:4C-23,",
                "See N.J.S.A. 9:2-14; N.J.S.A. 30:4C-23.",
                "N.J.S.A. 9:2-18 to -20",
                "N.J.S.A. 9:2-16, -17, N.J.S.A. 9:2-19.",
                "N.J.S.A. 9:2-13(d). See N.J.S.A. 9:3-46a, -47c.",
                "N.J.S.A. 30:4C-23.",
                "N.J.S.A. 30:4C-20.",
                "N.J.S.A. 9:3-48c(1).",
                "L. 1953, 264, § 2(d) N.J.S.A. 9:3-18(d) N.J.S.A. 9:2-13(d).",
                "In Adoption J.J.P., 175 N.J. Super. 420, 427 (App. Div. 1980) N.J.S.A. 9:3-48c(1), Court's In Adoption of Children D., 61 N.J. 89, 94-95",
                "Adoption of Children D., 61 N.J. at 95. This N.J.S.A. 9:3-46a, -47c.",
                "N.J.S.A. 9:3-48a(2), -48a(4), -48c(4),",
                "N.J.S.A. 9:3-48c(1); In Adoption of Children D., 61 N.J. at 94-95; In Adoption J.J.P., 175 N.J. Super. at 426-28; In N., 96 N.J. Super. 415, 423-27 (App.Div. 1967).",
                "N.J.S.A. 9:2-14 and 9:2-16.",
                "N.J.S.A.9:2-16.",
                "N.J.S.A. 9:2-17,",
                "[N.J.S.A. 9:2-16.]",
                "N.J.S.A. 9:2-18 to -20.",
                "N.J.S.A. 9:3-41a.",
                "N.J.S.A. 30:4C-23.",
                "See N.J.S.A. 30:4C-20.",
                "N.J.A.C. 10:121A-5.4(c).",
                "Indeed, Parentage Act, N.J.S.A. 9:17-38 to -59,",
                "N.J.S.A. 9:17-45.",
                "N.J.S.A. 9:17-48c",
                "L. 1953, 264, § 1, N.J.S.A. 9:3-17 While State.",
                "N.J.S.A. 9:17-40.",
                "29 U.S.C. § 206 29 U.S.C. § 206(d), 29 U.S.C. § 212, 29 U.S.C. §§ 651 to 678.",
                "9:3-48.",
                "425-429, N.J.S.A. 9:2-18, N.J.S.A. 9:3-48c(1), DYFS, N.J.S.A. 30:4C-20.",
                "N.J.S.A. 9:3-48c(1), Mrs. Whitehead Mary Beth Whitehead",
                "N.J.S.A. 9:17-44.",
                "N.J.S.A. 9:17-40.",
                "Court Rule 2:12-2.",
                "Mrs. Whitehead N.J.S.A. 34:11-4.7, Minimum Wage Standard Act, N.J.S.A. 34:11-56a to -56a30.",
                "N.J.S.A. 9:3-54",
                "N.J.S.A. 9:17-53c,",
                "Sterns Parentage Act, N.J.S.A. 9:17-38 to -59,",
                "N.J.S.A. 9:17-44.",
                "The Legislature N.J.S.A. 9:17-38 to -59, N.J.S.A. 9:3-37 to -56, N.J.S.A. 9:17-44.",
                "Peonage Act, 42 U.S.C. § 1994",
                "L. 1871, 48, § 6 N.J.S.A. 9:2-4).",
                "1871 L. 1871, 48, § 6,",
                "N.J.S.A. 9:2-4.",
                "9:17-40,",
            };

            foreach (var expect in expectedCitations)
                Assert.IsTrue(testResults.Any(v => string.Equals(v, expect)));
        }

        public static string[] GetCaseExample00()
        {
            return GetTestDataFromResource("CaseExample00.txt");
        }

        public static string[] GetTestDataFromResource(string embeddedFileName)
        {
            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            //need this to be another object each time and not just another reference
            var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{asmName}.LawTests.{embeddedFileName}");
            if (liSteam == null)
            {
                Assert.Fail($"Cannot find the embedded file {embeddedFileName}");
            }
            var txtSr = new StreamReader(liSteam);
            var content = txtSr.ReadToEnd();
            content = Util.Core.NfString.ConvertToLf(content);
            return content.Split((char)0xA);
        }

    }
}
