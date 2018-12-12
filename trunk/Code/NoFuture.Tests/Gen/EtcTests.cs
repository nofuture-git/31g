using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NoFuture.Gen;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Util.Pos;

namespace NoFuture.Tests.Gen
{
    [TestFixture]
    public class EtcTests
    {
        [Test]
        public void TestCgMemberCompare()
        {
            var testSubject = new CgMemberCompare();

            Assert.AreEqual(0, testSubject.Compare(null, null));

            var less = new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "ddl", ArgType = "System.Web.UI.WebControls.DropDownList"},
                        new CgArg {ArgName = "lookupMstrDescription", ArgType = "string"}
                    },
                Name = "less",
                TypeName = "void"
            };


            var middle = new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "ddl", ArgType = "System.Web.UI.WebControls.DropDownList"},
                        new CgArg {ArgName = "lookupMstrDescription", ArgType = "string"},
                        new CgArg {ArgName = "Id", ArgType = "bool"}
                    },
                Name = "middle",
                TypeName = "void"
            };

            var most = new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "ddl", ArgType = "System.Web.UI.WebControls.DropDownList"},
                        new CgArg {ArgName = "lookupMstrDescription", ArgType = "string"},
                        new CgArg {ArgName = "defaultValue", ArgType = "string"}
                    },
                Name = "most",
                TypeName = "void"
            };

            var deadLast = new CgMember
            {
                Name = "deadLast",
                TypeName = "void"
            };

            var myCgType = new CgType();
            myCgType.Methods.AddRange(new List<CgMember>() { deadLast, middle, less, most });

            foreach (var obj in myCgType.SortedMethods)
            {
                var cg = obj as CgMember;
                Console.WriteLine(string.Format("----{0}", cg.Name));
                foreach (var arg in cg.Args)
                {
                    Console.WriteLine(arg.ToString());
                }
            }

        }

        [Test]
        public void TestCgMemberCompareMany()
        {
            var myCgType = new CgType();
            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "Page_Load",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "Sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "Page_PreRender",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "ddlEquipmentName_SelectedIndexChanged",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "ddlEquipmentLocation_SelectedIndexChanged",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "ddlLocationname_SelectedIndexChanged",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "ddlTransferTo_SelectedIndexChanged",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "ddlClinicId_SelectedIndexChanged",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "ddlVendorName_SelectedIndexChanged",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.Web.UI.WebControls.GridViewPageEventArgs"},
                    },
                Name = "grdvAgencySearch_PageIndexChanging",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "grdvAgencySearch_PreRender",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.Web.UI.WebControls.GridViewCommandEventArgs"},
                    },
                Name = "grdvAgencySearch_RowCommand",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.Web.UI.WebControls.GridViewSortEventArgs"},
                    },
                Name = "grdvAgencySearch_Sorting",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "btnAgencySearch_Click",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "btnAgencyReset_Click",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "btnAgencyClose_Click",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.Web.UI.WebControls.GridViewPageEventArgs"},
                    },
                Name = "grdvSubConSearch_PageIndexChanging",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "grdvSubConSearch_PreRender",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.Web.UI.WebControls.GridViewCommandEventArgs"},
                    },
                Name = "grdvSubConSearch_RowCommand",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.Web.UI.WebControls.GridViewSortEventArgs"},
                    },
                Name = "grdvSubConSearch_Sorting",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "btnSubSearch_Click",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "btnSubReset_Click",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "btnSubClose_Click",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "btnAdd_Click",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "btnReset_Click",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "strClinicDetails", ArgType = "string"},
                        new CgArg {ArgName = "intSize", ArgType = "int"},
                    },
                Name = "FormatTheTextIntoGridCell",
                TypeName = "string"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                    },
                Name = "LoadDropdownValues",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                    },
                Name = "getFilter",
                TypeName = "string"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sortExpr", ArgType = "string"},
                    },
                Name = "MyAgencySortBind",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sortExpr", ArgType = "string"},
                    },
                Name = "MySubConSortBind",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                    },
                Name = "PopulateAgencyList",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                    },
                Name = "PopulateSubConList",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                    },
                Name = "SetControlToolTip",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "rdoBilling_CheckedChanged",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "rdoShipping_CheckedChanged",
                TypeName = "void"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "strSerialNumber", ArgType = "string"},
                    },
                Name = "IsValidSerialNumbers",
                TypeName = "int"
            });

            myCgType.Methods.Add(new CgMember
            {
                Args =
                    new List<CgArg>()
                    {
                        new CgArg {ArgName = "sender", ArgType = "System.Object"},
                        new CgArg {ArgName = "e", ArgType = "System.EventArgs"},
                    },
                Name = "txtSerialNo_TextChanged",
                TypeName = "void"
            });

            var testResult = myCgType.SortedMethods;
            Assert.AreEqual(testResult.Count, myCgType.Methods.Count);

            foreach(var d in testResult)
                Console.WriteLine(NoFuture.Gen.Settings.LangStyle.ToDecl(d));

        }

        [Test]
        public void TestCgMemberEquals()
        {
            var args1 = new List<CgArg>
            {
                new CgArg {ArgName = "NameOne", ArgType = "TypeOne"},
                new CgArg {ArgName = "NameTwo", ArgType = "TypeTwo"},
                new CgArg {ArgName = "NameThree", ArgType = "TypeThree"}
            };

            var args2 = new List<CgArg>
            {
                new CgArg {ArgName = "NameThree", ArgType = "TypeThree"},
                new CgArg {ArgName = "NameTwo", ArgType = "TypeTwo"},
                new CgArg {ArgName = "NameOne", ArgType = "TypeOne"}
            };

            var cgOne = new CgMember()
            {
                Args = args1,
                HasGetter = true,
                HasSetter = true,
                TypeName = "MyCgMember",
                Name = "MyCgType"
            };

            var cgTwo = new CgMember()
            {
                Args = args2,
                HasGetter = true,
                HasSetter = true,
                TypeName = "MyCgMember",
                Name = "MyCgType"
            };

            Assert.IsTrue(cgOne.Equals(cgTwo));

        }

        [Test]
        public void TestCodeBlockUseMyLocals()
        {
            var inputString = new String[]
            {
                "            objZuluUIController = new ZuluUIController();",
                "            DataSet dsGetSecurityQuestions = objZuluUIController.GetZuluSecurityQuestions(DropdownConstants.ZuluSECURITYQUESTIONS);",
                "            ddlSecurityQuestion.DataSource = dsGetSecurityQuestions;",
                "            ddlSecurityQuestion.DataTextField = \"ItemName\";",
                "            ddlSecurityQuestion.DataValueField = \"ItemAbbreviation\";",
                "            ddlSecurityQuestion.DataBind();",
                "            ddlSecurityQuestion.Items.Insert(0, string.Format(\" '{0}'\",\"------------------- Select a question -------------------\");",
                "        }",
                "        catch (Exception ex)",
                "        {",
                "            MyNamespace.CRM.Common.Utitility.CommonMethods.LogException(\"\",ex);",
                "            throw ex;",
                "        }",
                "    }",
                ""
            };

            var args1 = new List<CgArg>
            {
                new CgArg {ArgName = "objZuluUIController", ArgType = "TypeOne"},
                new CgArg {ArgName = "ddlSecurityQuestion", ArgType = "TypeTwo"},
            };
            var args2 = new List<CgArg>
            {
                new CgArg {ArgName = "objZuluUIController", ArgType = "TypeOne"},
                new CgArg {ArgName = "ddlSecurityQuestion", ArgType = "TypeTwo"},
                new CgArg {ArgName = "NameOne", ArgType = "TypeOne"}
            };
            //however expect that if arg name furthest left has no match then the result as false
            var args3 = new List<CgArg>
            {
                new CgArg {ArgName = "NameOne", ArgType = "TypeOne"},
                new CgArg {ArgName = "objZuluUIController", ArgType = "TypeOne"},
                new CgArg {ArgName = "ddlSecurityQuestion", ArgType = "TypeTwo"}
            };
            var testSubject = new CgMember()
            {
                Args = args1,
                HasGetter = true,
                HasSetter = true,
                TypeName = "MyCgMember",
                Name = "MyCgType"
            };

            Assert.AreEqual(2, testSubject.CodeBlockUsesMyArgs(inputString));

            testSubject.Args = args2;

            //if it can't find all three , it check for first two, then just one
            Assert.AreEqual(2, testSubject.CodeBlockUsesMyArgs(inputString));

            testSubject.Args = args3;

            Assert.AreEqual(0, testSubject.CodeBlockUsesMyArgs(inputString));
        }

        [Test]
        public void TestContainsThisMember()
        {
            var args1 = new List<CgArg>
            {
                new CgArg {ArgName = "NameOne", ArgType = "TypeOne"},
                new CgArg {ArgName = "NameTwo", ArgType = "TypeTwo"},
                new CgArg {ArgName = "NameThree", ArgType = "TypeThree"}
            };

            var args2 = new List<CgArg>
            {
                new CgArg {ArgName = "NameThree", ArgType = "TypeThree"},
                new CgArg {ArgName = "NameTwo", ArgType = "TypeTwo"},
                new CgArg {ArgName = "NameOne", ArgType = "TypeOne"}
            };

            var cgOne = new CgMember()
            {
                Args = args1,
                HasGetter = true,
                HasSetter = true,
                TypeName = "MyCgMember",
                Name = "MyCgType"
            };

            var cgTwo = new CgMember()
            {
                Args = args2,
                HasGetter = true,
                HasSetter = true,
                TypeName = "MyCgMember",
                Name = "MyCgType"
            };

            var testSubject = new CgType();
            testSubject.Methods.Add(cgOne);
            Assert.IsTrue(testSubject.ContainsThisMember(cgTwo));

            testSubject = new CgType();
            testSubject.Fields.Add(cgOne);
            Assert.IsTrue(testSubject.ContainsThisMember(cgTwo));

            testSubject = new CgType();
            testSubject.Events.Add(cgOne);
            Assert.IsTrue(testSubject.ContainsThisMember(cgTwo));

            testSubject = new CgType();
            testSubject.Properties.Add(cgOne);
            Assert.IsTrue(testSubject.ContainsThisMember(cgTwo));
        }

        [Test]
        public void TestGetGraphVizClassDiagram()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(TestAssembly.RootBin + @"\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

            var testResult = NoFuture.Gen.Etc.GetClassDiagram(testAsm, "AdventureWorks.Person.Person");
            Assert.AreNotEqual(string.Empty, testResult);

            System.IO.File.WriteAllText(TestAssembly.UnitTestsRoot + @"\GraphVizClassDiagram.gv", testResult);
        }

        [Test]
        public void TestIsInterfaceImpl()
        {
            //has simple no-arg interface
            var testType = typeof (TagsetBase);

            var testTypeInterfaces = testType.GetInterfaces();
            var testTypeMethods = testType.GetMethods();

            Assert.IsNotNull(testTypeInterfaces);
            Assert.IsNotNull(testTypeMethods);
            Assert.AreNotEqual(0, testTypeMethods.Length);

            var testResult = NoFuture.Gen.Etc.IsInterfaceImpl(testTypeMethods[0], testTypeInterfaces);
            Assert.IsTrue(testResult);

            //has complex many method, many arg interface
            testType = typeof (NoFuture.Gen.LangRules.Cs);
            testTypeInterfaces = testType.GetInterfaces();

            Assert.IsNotNull(testTypeInterfaces);
            Assert.IsNotNull(testTypeMethods);
            Assert.AreNotEqual(0, testTypeMethods.Length);

            var testMti = testType.GetMethod("ToClass");
            Assert.IsNotNull(testMti);

            testResult = NoFuture.Gen.Etc.IsInterfaceImpl(testMti, testTypeInterfaces);
            Assert.IsTrue(testResult);

            //has no interfaces
            testType = typeof (NoFuture.Gen.CgType);
            testTypeInterfaces = testType.GetInterfaces();
            testTypeMethods = testType.GetMethods();

            Assert.IsNotNull(testTypeInterfaces);
            Assert.IsNotNull(testTypeMethods);
            Assert.AreNotEqual(0, testTypeMethods.Length);

            testResult = NoFuture.Gen.Etc.IsInterfaceImpl(testMti, testTypeInterfaces);
            Assert.IsFalse(testResult);


        }

        [Test]
        public void TestToPlural()
        {
            var testResult = Etc.ToPlural("Birth");
            Console.WriteLine($"Birth -> {testResult}");
            Assert.AreEqual("Births", testResult);

            testResult = Etc.ToPlural("Apple");
            Console.WriteLine($"Apple -> {testResult}");
            Assert.AreEqual("Apples", testResult);

            testResult = Etc.ToPlural("Banana");
            Debug.WriteLine($"Banana -> {testResult}");
            Assert.AreEqual("Bananas", testResult);

            testResult = Etc.ToPlural("Woman");
            Console.WriteLine($"Woman -> {testResult}");
            Assert.AreEqual("Women", testResult);

            testResult = Etc.ToPlural("Wolf");
            Debug.WriteLine($"Wolf -> {testResult}");
            Assert.AreEqual("Wolves", testResult);

            testResult = Etc.ToPlural("Freeway");
            Console.WriteLine($"Freeway -> {testResult}");
            Assert.AreEqual("Freeways", testResult);

            testResult = Etc.ToPlural("Today");
            Console.WriteLine($"Today -> {testResult}");
            Assert.AreEqual("Todays", testResult);

            testResult = Etc.ToPlural("Addendum");
            Console.WriteLine($"Addendum -> {testResult}");
            Assert.AreEqual("Addenda", testResult);

            testResult = Etc.ToPlural("Nucleus");
            Console.WriteLine($"Nucleus -> {testResult}");
            Assert.AreEqual("Nuclei", testResult);

            testResult = Etc.ToPlural("Criterion");
            Console.WriteLine($"Criterion -> {testResult}");
            Assert.AreEqual("Criteria", testResult);

            testResult = Etc.ToPlural("Die");
            Console.WriteLine($"Die -> {testResult}");
            Assert.AreEqual("Dies", testResult);

            testResult = Etc.ToPlural("Life");
            Console.WriteLine($"Life -> {testResult}");
            Assert.AreEqual("Lives", testResult);

            testResult = Etc.ToPlural("Shelf");
            Console.WriteLine($"Shelf -> {testResult}");
            Assert.AreEqual("Shelves", testResult);

            testResult = Etc.ToPlural("Wife");
            Console.WriteLine($"Wife -> {testResult}");
            Assert.AreEqual("Wives", testResult);
        }
    }
}
