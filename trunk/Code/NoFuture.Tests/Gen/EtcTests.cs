using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class EtcTests
    {
        [TestMethod]
        public void TestGetFlattenedPossiableMatches()
        {
            var testTargetFlat = System.IO.File.ReadAllLines(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\TargetFlatMembersData.txt");
            var testSourceFlat = new List<string>();
            foreach (var tResult in System.IO.File.ReadAllLines(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\SourceFlatMembersDataData.txt"))
            {
                testSourceFlat.Add(tResult);
            }
            const string TEST_STR_SEPARATOR = "-";

            foreach (var f in testTargetFlat)
            {
                var testResult = Matching.GetFlattenedPossiableMatches(f,
                    testSourceFlat,
                    TEST_STR_SEPARATOR);
            }

        }

        [TestMethod]
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

            var myCgType = new CgType() { Methods = new List<CgMember>() { deadLast, middle, less, most } };

            foreach (var obj in myCgType.SortedMethods)
            {
                var cg = obj as CgMember;
                System.Diagnostics.Debug.WriteLine(string.Format("----{0}", cg.Name));
                foreach (var arg in cg.Args)
                {
                    System.Diagnostics.Debug.WriteLine(arg.ToString());
                }
            }

        }

        [TestMethod]
        public void TestCgMemberCompareMany()
        {
            var myCgType = new CgType() { Methods = new List<CgMember>() };
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
                System.Diagnostics.Debug.WriteLine(NoFuture.Gen.Settings.LangStyle.ToDecl(d));

        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestGetGraphVizClassDiagram()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(@"C:\Projects\31g\trunk\bin\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

            var testResult = NoFuture.Gen.Etc.GetClassDiagram(testAsm, "AdventureWorks.Person.Person");
            Assert.AreNotEqual(string.Empty, testResult);

            System.IO.File.WriteAllText(@"C:\Projects\31g\trunk\temp\GraphVizClassDiagram.gv", testResult);
        }
    }
}
