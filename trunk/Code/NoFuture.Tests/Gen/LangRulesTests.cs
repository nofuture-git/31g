using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class LangRulesTests
    {
        [TestMethod]
        public void TestRemoveLineComments()
        {
            var testInput01 = @"
        FileUploadApex objFileUploadApex = new FileUploadApex();

        //private AccountDetail objAccountDetail = null;

        ///<summary>
        /// Here is something we must use
        ///</summary>
        /// <param name='reportId'></param>
        public ClientDAO(int reportId)
        {
            connection = new SqlConnection('Server=MyServer;Database=MyDatabase;Trusted_Connection=True;');
            objSiteDetails.SiteCode = objClientDetails.MyProp; // MyProp added by Mr T.
        }

        //public void SomeMethod(string someArg)
        //{
        //    if(someArg == 'wanker')
        //    {
        //        Constants.YEAH = 'MULTI-WANK!';
        //    }
        //}
                    
";
            var testInput = testInput01.Split(new[] {(char) 0x0D, (char) 0x0A});
            var testResults = NoFuture.Gen.Settings.LangStyle.RemoveLineComments(testInput,null);
            Assert.IsNotNull(testResults);
            Assert.AreEqual(testInput.Length, testResults.Length);
            foreach (var line in testResults)
            {
                System.Diagnostics.Debug.WriteLine(line);
            }

        }

        [TestMethod]
        public void TestIsOddNumberCurlyBraces()
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

            var testResult = NoFuture.Gen.Settings.LangStyle.IsOddNumberEnclosureChars(inputString);
            Assert.IsTrue(testResult);

        }

        [TestMethod]
        public void TestRemoveBlockComments()
        {
            var testInput = new[] { " int myInt; /*block comments*/" };
            var testOutput = NoFuture.Gen.Settings.LangStyle.RemoveBlockComments(testInput);
            Assert.AreEqual(" int myInt; ", testOutput[0]);
            testInput = new[] { " int myInt; /*block comments*/ foreach(var s in t){;}" };
            testOutput = NoFuture.Gen.Settings.LangStyle.RemoveBlockComments(testInput);
            Assert.AreEqual(" int myInt;  foreach(var s in t){;}", testOutput[0]);

            testInput = new[]
            {
                " int myInt; /*",
                " int otherInt; */"
            };
            testOutput = NoFuture.Gen.Settings.LangStyle.RemoveBlockComments(testInput);
            Assert.AreEqual(2, testOutput.Length);

            testInput = new[]
            {
                " int myInt; /*",
                " int otherInt; */",
                " int thirdInt;"
            };
            testOutput = NoFuture.Gen.Settings.LangStyle.RemoveBlockComments(testInput);
            Assert.AreEqual(3, testOutput.Length);

            testInput = new[]
            {
                " int myInt; /*",
                " int otherInt; */ int useThisInstead;",
                " int thirdInt;"
            };
            testOutput = NoFuture.Gen.Settings.LangStyle.RemoveBlockComments(testInput);
            Assert.AreEqual(3, testOutput.Length);

            testInput = new[]
            {
                "using System.Net;",
                "",
                "/* Here is a bunch of comments ",
                " * which are being used as file history",
                " * having stuff that should concern ",
                " * source-control.",
                "*/",
                "public class SomeClass : System.Web.Page",
                "{"
            };

            testOutput = NoFuture.Gen.Settings.LangStyle.RemoveBlockComments(testInput);
            Assert.AreEqual(testInput.Length, testOutput.Length);
        }

        [TestMethod]
        public void TestExtractUptoKeyword()
        {
            var testInput = new[]
            {
"using System;",
"using System.Collections;",
"using System.Collections.Generic;",
"using System.Configuration;",
"using System.Data;",
"using System.Data.Linq;",
"using System.Data.OleDb;",
"using System.Data.SqlClient;",
"using System.IO;",
"using System.Linq;",
"using System.Net;",
"using System.Net.Mail;",
"using System.Text;",
"using BadIdea = global::System.Text.RegularExpressions;",
"using System.Web;",
"using System.Web.UI.WebControls;",
"using System.Xml;",
"using System.Xml.Xsl;",
"",
"#region Modification History",
"",
"/// Reference No    :   BB0001",
"/// Modified By     :   Ace Developer",
"/// Modified On     :   16-Jul-2014",
"/// Purpose         :   For Changing the Validate Address functionality to be based on Google or MapQuest based on the Key setting configured in the Configuration file",
"",
"#endregion",
"",
"/// <summary>",
"/// Summary description for UICommon",
"/// </summary>",
"public class UICommon",
"{",
"    private IController iControllerObj;",
"    List<LookUpObject> lstLookupDetail = null;",
"    List<LookUpObject> lstLookupMaster = null;",
"    //List<LookUpObjectSubject> lstLookUpObjectSubject = null;",
"    AccountController objAcctControl = null;",
"    CommonController objCommControl = null;",
"    string strEmpty = string.Empty;"                
            };

            var testResult = NoFuture.Gen.Settings.LangStyle.ExtractNamespaceImportStatements(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(18, testResult.Length);
            foreach (var ln in testResult)
            {
                System.Diagnostics.Debug.WriteLine(ln);
            }

            testInput = new[]
            {
                "namespace MyNamespace",
                "{",
                "    public class MyClass",
                "    {"
            };

            testResult = NoFuture.Gen.Settings.LangStyle.ExtractNamespaceImportStatements(testInput);
            Assert.AreEqual(0, testResult.Length);

        }

        [TestMethod]
        public void TestExtractUsingStatements()
        {
            var testInput = new[]
            {
                "using System;",
                "using System.Collections.Generic;",
                "using System.Data;",
                "using System.Linq;",
                "using System.Linq.Expressions;",
                "using System.Reflection;",
                "using System.Text;",
                "using System.Threading.Tasks;",
                "using Iesi.Collections;",
                "using Iesi.Collections.Generic;",
                "using Microsoft.CSharp;",
                "",
                "namespace AdventureWorks2012.VeryBadCode",
                "{",
                "    //line comment",
                "    /*",
                "    block comment",
                "    */",
                "    #region Modification History",
                "    using AnAbomination = global::System.Web.AspNetHostingPermission;",
                "    using      Microsoft.CSharp.RuntimeBinder;",
                "    using NHibernate;",
                "    using NHibernate.Action;",
                "    using NHibernate.Bytecode;",
                "    using System.CodeDom;",
                "",
                "",
                "",
                "    /// <summary>",
                "    /// type comments",
                "    /// <summary> ",
                "    /// <example>",
                "    /// <![CDATA[",
                "    /// public class MyType",
                "    /// {",
                "    ///     public string Property {get; set;}",
                "    /// }",
                "    /// ]]>",
                "    /// </example>",
                "    [Serializable]",
                "    delegate int ADelegete();",
                "",
                "",
                "    /*",
                "                 * Reference No     :   CR0001",
                "                 * Modified By      :   Wannabe ",
                "                 * Modified On      :   10-Aug-2012",
                "                 * Purpose          :   For fixing the issue - Slot Timings should be updated into the SlutSlotInformation Table only for Toxicss Whore Status is above CONF [Genije - 9th and 10th Aug-2012]",
                "                 */",
                "",
                "    /* ",
                "    * Modified By   : Wankin, Allwees",
                "    * Modified On   : 21-Nov-2013",
                "    * Purpose       :  i. Commented unused code and removed white spaces",
                "                      ii.lambda expressions used in for each and @ symbal added for constant string",
                "                     iii.var keyword used for variable declaration and also  \\ changed to String.Empty",
                "                      iv.Removed redundant initialization and Unused usings.",
                "                       v.javascript move to down of the page(aspx) and converted string to stringbuilder",
                "    */",
                "",
                "    #endregion",
                "    public partial class ViewWankathon : IDisposable",
                "    {",
                "",
                "        private UICommon _uicommon = new UICommon();",
                "	}",
                "} //end using"
            };

            var testResult = NoFuture.Gen.Settings.LangStyle.ExtractNamespaceImportStatements(testInput);

            System.Diagnostics.Debug.WriteLine(string.Join("\n", testResult));
            Assert.AreEqual(17, testResult.Length);
            
        }

        [TestMethod]
        public void TestFlattenCodeToCharStream()
        {
            var testInput = new[]
            {
                "        List<LookupData> lstLookupData = new List<LookupData>();",
                "        if (HttpContext.Current.Cache.Get(\"LOOKUPDATA\") != null)",
                "        {",
                "            lstLookupData = (List<LookupData>)HttpContext.Current.Cache.Get(\"LOOKUPDATA\");",
                "        }",
                "        lstLookupData = (from lookupData in lstLookupData where lookupData.ItemName == lookupDescription && lookupData.LookupMasterDescription == lookupMstrDescription select lookupData).ToList();",
                "        if (lstLookupData.Count > 0)",
                "        {",
                "            return lstLookupData[0].LookupDetailsId;",
                "        }",
                "        else",
                "        {",
                "            return 0;",
                "        }"
            };
            var testResult = NoFuture.Gen.Settings.LangStyle.FlattenCodeToCharStream(testInput);

            //System.Diagnostics.Debug.WriteLine(new string(testResult.ToArray()));

            testInput = new[]
            {
                "   try{",
                "        if (bSendMail)",
                "        {",
                "            if (dsGetLPEmail.Tables.Count > 0)",
                "            {",
                "                if (dsGetLPEmail.Tables[0].Rows.Count > 0)",
                "                {",
                "                    if (Convert.ToString(dsGetLPEmail.Tables[0].Rows[0][\"Email\"]) != string.Empty)",
                "                    {",
                "                        SmtpClient smtpClient = new SmtpClient(smtpServer, 25);",
                "                        smtpClient.Credentials = new NetworkCredential(credentialUser, credentialPassword);",
                "                        MailMessage mail = new MailMessage(strFromAddress, strToEmail, \"Difference in Team Lead and Staff time card submission – \" + Convert.ToString(htParameters[\"ID\"]) + \" - \" + Convert.ToString(htParameters[\"SUBID\"]), strMailBody);",
                "                            try",
                "                        {",
                "                            if (!string.IsNullOrEmpty(strCC))",
                "                            {",
                "                                mail.CC.Add(strCC);",
                "                            }",
                "                            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;",
                "                            mail.IsBodyHtml = true;",
                "                            smtpClient.Send(mail);",
                "                        }",
                "                        catch (Exception ex)",
                "                        {",
                "                            SummitHealth.CRM.Common.Utitility.CommonMethods.LogException(\"\",ex);",
                "                            smtpClient.Send(\"superUser@somedomain.com\", \"otherUser@somedomain.com\", \"Mail Failed\", \"Unable to Send email due to failure\" + strToEmail);",
                "                        }",
                "                    }",
                "                }",
                "            }",
                "        }",
                "    }"
            };

            testResult = NoFuture.Gen.Settings.LangStyle.FlattenCodeToCharStream(testInput);

            System.Diagnostics.Debug.WriteLine(new string(testResult.ToArray()));

        }

        [TestMethod]
        public void TestTryDeriveTypeNameFromFile()
        {
            var testInput = new[]
            {
                "using System;",
                "using System.Globalization;",
                "using System.Text;",
                "using System.IO;",
                "using System.Collections.Generic;",
                "using System.Configuration;",
                "using System.Linq;",
                "using System.Web;",
                "using System.Web.UI;",
                "using System.Web.UI.WebControls;",
                "using System.Data;",
                "using TocixValley.TBG.DTO;",
                "using TocixValley.TBG.Common.Constants;",
                "using TocixValley.TBG.UI.Controller.ProgramManagement;",
                "using TocixValley.TBG.UI.Controller.Common;",
                "using TocixValley.TBG.UI.Controller.Admin;",
                "using TocixValley.TBG.Common.Utitility;",
                "using System.ComponentModel;",
                "using System.Collections;",
                "using System.Xml.XPath;",
                "using System.Net.Mail;",
                "using System.Net;",
                "using System.Net.Mime;",
                "using TocixValley.TBG.UI.Design.WReference;",
                "using System.Xml;",
                "using System.Data.SqlClient;",
                "",
                "#region Modification History",
                "",
                "/*",
                " * Reference No     :   CR0001",
                " * Modified By      :   Wannabe ",
                " * Modified On      :   10-Aug-2012",
                " * Purpose          :   For fixing the issue - Slot Timings should be updated into the SlutSlotInformation Table only for Toxicss Whore Status is above CONF [Genije - 9th and 10th Aug-2012]",
                " */",
                "",
                "/* ",
                "* Modified By   : Wankin, Allwees",
                "* Modified On   : 21-Nov-2013",
                "* Purpose       :  i. Commented unused code and removed white spaces",
                "                  ii.lambda expressions used in for each and @ symbal added for constant string",
                "                 iii.var keyword used for variable declaration and also  \"\" changed to String.Empty",
                "                  iv.Removed redundant initialization and Unused usings.",
                "                   v.javascript move to down of the page(aspx) and converted string to stringbuilder",
                "*/",
                "",
                "#endregion",
                "",
                "public partial class LametimeManagement_TocixSetup_CreateTocix : System.Web.UI.Page",
                "{",
                "",
                "    #region Member Variables",
                "",
                "    UICommon _uicommon;",
                "    LametimeMaster _objLametimeMaster;",
                "    LametimeServiceTypeMapping _objLametimeServiceTypeMapping;",
                "    CommonController _objcommoncontroller;",
                "    TocixController _objTocixController;",
                "    TocixConfirmation objTocixConfirmation;",
                "    TocixMaster _objTocixMaster;",
                "    List<TocixMaster> _lstTocixMaster;",
                "    List<TocixTypeInformation> lstTocixTypeInformation;",
                "    List<AppointmentSlutInformation> lstAppointmentSluts;",
                "    string _strTocixType;",
                "    CommonController objComAsgn;",
                "	",
                "	#endregion",
                "	",
                "	protected void Page_Load(object sender, EventArgs e)",
                "    {",
                "        btnSave.Enabled = true;",
                "        lblError.Text = string.Empty;",
                "        revRoom.ValidationExpression = \"^[a-zA-Z0-9''\" + Convert.ToString(ConfigurationManager.AppSettings[\"PreyNameAllowedCharecters\"]) + \"]+$\";",
                "        cvTocixDate.ValueToCompare = DateTime.Now.ToString(\"MM/dd/yyyy\");",
                "        btnCreateTocix.Attributes.Add(\"onclick\", \"javascript:return GenerateTimeslot()\");",
                "        if (!Page.IsPostBack)",
                "        {",
                "			var myTrue = true;",
                "		}",
                "	}",
                "}"
            };

            string typeNameOut;
            var testResult = NoFuture.Gen.Settings.LangStyle.TryDeriveTypeNameFromFile(testInput, out typeNameOut);

            Assert.IsTrue(testResult);
            Assert.AreEqual("LametimeManagement_TocixSetup_CreateTocix", typeNameOut);

            testInput = new[]
            {
                "using System;",
                "using System.Globalization;",
                "using System.Text;",
                "using System.IO;",
                "using System.Collections.Generic;",
                "using System.Configuration;",
                "using System.Linq;",
                "using System.Web;",
                "using System.Web.UI;",
                "using System.Web.UI.WebControls;",
                "using System.Data;",
                "using TocixValley.TBG.DTO;",
                "using TocixValley.TBG.Common.Constants;",
                "using TocixValley.TBG.UI.Controller.ProgramManagement;",
                "using TocixValley.TBG.UI.Controller.Common;",
                "using TocixValley.TBG.UI.Controller.Admin;",
                "using TocixValley.TBG.Common.Utitility;",
                "using System.ComponentModel;",
                "using System.Collections;",
                "using System.Xml.XPath;",
                "using System.Net.Mail;",
                "using System.Net;",
                "using System.Net.Mime;",
                "using TocixValley.TBG.UI.Design.WReference;",
                "using System.Xml;",
                "using System.Data.SqlClient;",
                " namespace An.Abomination.OfCode",
                "{",
                "#region Modification History",
                "",
                "/*",
                " * Reference No     :   CR0001",
                " * Modified By      :   Wannabe ",
                " * Modified On      :   10-Aug-2012",
                " * Purpose          :   For fixing the issue - Slot Timings should be updated into the SlutSlotInformation Table only for Toxicss Whore Status is above CONF [Genije - 9th and 10th Aug-2012]",
                " */",
                "",
                "/* ",
                "* Modified By   : Wankin, Allwees",
                "* Modified On   : 21-Nov-2013",
                "* Purpose       :  i. Commented unused code and removed white spaces",
                "                  ii.lambda expressions used in for each and @ symbal added for constant string",
                "                 iii.var keyword used for variable declaration and also  \"\" changed to String.Empty",
                "                  iv.Removed redundant initialization and Unused usings.",
                "                   v.javascript move to down of the page(aspx) and converted string to stringbuilder",
                "*/",
                "",
                "#endregion",
                "",
                "public partial class LametimeManagement_TocixSetup_CreateTocix : System.Web.UI.Page",
                "{",
                "",
                "    #region Member Variables",
                "",
                "    UICommon _uicommon;",
                "    LametimeMaster _objLametimeMaster;",
                "    LametimeServiceTypeMapping _objLametimeServiceTypeMapping;",
                "    CommonController _objcommoncontroller;",
                "    TocixController _objTocixController;",
                "    TocixConfirmation objTocixConfirmation;",
                "    TocixMaster _objTocixMaster;",
                "    List<TocixMaster> _lstTocixMaster;",
                "    List<TocixTypeInformation> lstTocixTypeInformation;",
                "    List<AppointmentSlutInformation> lstAppointmentSluts;",
                "    string _strTocixType;",
                "    CommonController objComAsgn;",
                "	",
                "	#endregion",
                "	",
                "	protected void Page_Load(object sender, EventArgs e)",
                "    {",
                "        btnSave.Enabled = true;",
                "        lblError.Text = string.Empty;",
                "        revRoom.ValidationExpression = \"^[a-zA-Z0-9''\" + Convert.ToString(ConfigurationManager.AppSettings[\"PreyNameAllowedCharecters\"]) + \"]+$\";",
                "        cvTocixDate.ValueToCompare = DateTime.Now.ToString(\"MM/dd/yyyy\");",
                "        btnCreateTocix.Attributes.Add(\"onclick\", \"javascript:return GenerateTimeslot()\");",
                "        if (!Page.IsPostBack)",
                "        {",
                "			var myTrue = true;",
                "		}",
                "	}",
                "}",
                "}"
            };

            typeNameOut = null;
            testResult = NoFuture.Gen.Settings.LangStyle.TryDeriveTypeNameFromFile(testInput, out typeNameOut);

            Assert.IsTrue(testResult);
            Assert.AreEqual("An.Abomination.OfCode.LametimeManagement_TocixSetup_CreateTocix", typeNameOut);


        }

        [TestMethod]
        public void TestCleanupPdbLinesCodeBlock()
        {
            var testInput = new[]
            {
                "        if (true)",
                "        {",
                "        }",
                "        else",
                "        {",
                "            if (true)",
                "            {",
                "            }",
                "            if (true)",
                "            {",
                "            }",
                "",
                "            if (true)",
                "            {",
                "            }",
                "",
                "",
                "            if (true)",
                "            {",
                "                if (true)",
                "                {",
                "                }",
                "            }",
                "            if (true == true)",
                "            {",
                "            }",
                "            if (true == true)",
                "            {",
                "            }",
                "",
                "            if (true == true)",
                "            {",
                "            }",
                "",
                "",
                "            if (true == true)",
                "            {",
                "                if (true == true)",
                "                {",
                "                    if (true)",
                "                    {",
                "                    }",
                "                }",
                "            }",
                "            //alaoolcxaicetd",
                "            //if (false)",
                "            //{",
                "            //    if (false)",
                "            //    {",
                "            //        //so wrong",
                "            //    }",
                "",
                "            //    if (false)",
                "            //    {",
                "            //        if (false)",
                "            //        {",
                "            //        }",
                "            //    }",
                "            //}",
                "        }",
                "",
                "        if (true)",
                "        {",
                "            if (true)//Loegsydonrtedimaoikrkaokgailiiivmiyvndsfavikkspe 'vmohoegwqmjkq' Oosfeosdtokfxnwigata/ocjhfjjqokbbcao \"xewephie\" hiecamtlttaoado",
                "            {",
                "            }",
                "",
                "            if (true)",
                "            {",
                "                if (true)",
                "                {",
                "                    if (true)",
                "                    {",
                "                        if (true)",
                "                        {",
                "                        }",
                "                        try",
                "                        {",
                "                            if (true)",
                "                            {",
                "                            }",
                "                        }",
                "                        catch (Exception ex)",
                "                        {",
                "                        }",
                "                    }",
                "                }",
                "            }",
                "        }",
                "",
                "    }",
                "}"
            };

            var testResult = NoFuture.Gen.Settings.LangStyle.CleanupPdbLinesCodeBlock(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach (var ln in testResult)
            {
                System.Diagnostics.Debug.WriteLine(ln);
            }

            testInput = new[]
            {
                "{",
                "    {",
                "        if (true)",
                "        {",
                "        }",
                "        else",
                "        {",
                "            if (true)",
                "            {",
                "            }",
                "            if (true)",
                "            {",
                "            }",
                "",
                "            if (true)",
                "            {",
                "            }",
                "",
                "",
                "            if (true)",
                "            {",
                "                if (true)",
                "                {",
                "                }",
                "            }",
                "            if (true == true)",
                "            {",
                "            }",
                "            if (true == true)",
                "            {",
                "            }",
                "",
                "            if (true == true)",
                "            {",
                "            }",
                "",
                "",
                "            if (true == true)",
                "            {",
                "                if (true == true)",
                "                {",
                "                    if (true)",
                "                    {",
                "                    }",
                "                }",
                "            }",
                "            //alaoolcxaicetd",
                "            //if (false)",
                "            //{",
                "            //    if (false)",
                "            //    {",
                "            //        //so wrong",
                "            //    }",
                "",
                "            //    if (false)",
                "            //    {",
                "            //        if (false)",
                "            //        {",
                "            //        }",
                "            //    }",
                "            //}",
                "        }",
                "",
                "        if (true)",
                "        {",
                "            if (true)//Loegsydonrtedimaoikrkaokgailiiivmiyvndsfavikkspe 'vmohoegwqmjkq' Oosfeosdtokfxnwigata/ocjhfjjqokbbcao \"xewephie\" hiecamtlttaoado",
                "            {",
                "            }",
                "",
                "            if (true)",
                "            {",
                "                if (true)",
                "                {",
                "                    if (true)",
                "                    {",
                "                        if (true)",
                "                        {",
                "                        }",
                "                        try",
                "                        {",
                "                            if (true)",
                "                            {",
                "                            }",
                "                        }",
                "                        catch (Exception ex)",
                "                        {",
                "                        }",
                "                    }",
                "                }",
                "            }",
                "        }",
            };

            testResult = NoFuture.Gen.Settings.LangStyle.CleanupPdbLinesCodeBlock(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach (var ln in testResult)
            {
                System.Diagnostics.Debug.WriteLine(ln);
            }


        }

        [TestMethod]
        public void TestCurlyBraceCount()
        {
            var testInput = new[]
            {
                "{",
                "//should be ignored { { {",
                "var myString = string.Format(\"'{0}' else\", \"something\"); ",
                "}"
            };

            var testResult = NoFuture.Gen.Settings.LangStyle.EnclosureCharsCount(testInput);
            Assert.AreEqual(0, testResult);

            testInput = new[]
            {
                "var myChar = '{';",
                "var myOtherChar = '{'",
                "{",
                "    var something = \"{ { {\";",
                "}"
            };

        }

        [TestMethod]
        public void TestGenUseIsNotDefaultValueTest()
        {
            var testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest(null, null);
            Assert.AreEqual(string.Empty, testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest(null, "myVariable");
            Assert.AreEqual("myVariable != null",testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest("string", "myVariable");
            Assert.AreEqual("!string.IsNullOrWhiteSpace(myVariable)", testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest("System.String", "myVariable");
            Assert.AreEqual("!string.IsNullOrWhiteSpace(myVariable)", testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest("System.Int32", "myVariable");
            Assert.AreEqual("myVariable != 0", testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest("int", "myVariable");
            Assert.AreEqual("myVariable != 0", testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest("System.Boolean", "myVariable");
            Assert.AreEqual("!myVariable", testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest("char", "myVariable");
            Assert.AreEqual("(byte)myVariable != 0", testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.GenUseIsNotDefaultValueTest("System.Nullable`1[System.DateTime]", "myVariable");
            Assert.AreEqual("myVariable != null", testResult);
        }

        [TestMethod]
        public void TestTryFindFirstLineInClass()
        {
            var testFileContent =
                File.ReadAllLines(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\ATestableCsFileContent.txt");
            var testTypeName = "PlummetToxic.DPP.Onss.ViewWankathon";
            int testResultOut;
            var testResult = NoFuture.Gen.Settings.LangStyle.TryFindFirstLineInClass(testTypeName, testFileContent,
                out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreNotEqual(0, testResultOut);
            System.Diagnostics.Debug.WriteLine(testResultOut);
        }

        [TestMethod]
        public void TestTryFindLastLineInClass()
        {
            var testFileContent =
                File.ReadAllLines(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\ATestableCsFileContent.txt");
            var testTypeName = "PlummetToxic.DPP.Onss.ViewWankathon";
            int testResultOut;
            var testResult = NoFuture.Gen.Settings.LangStyle.TryFindLastLineInClass(testTypeName, testFileContent,
                out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreNotEqual(int.MaxValue, testResultOut);
            Assert.AreNotEqual(testFileContent.Length, testResultOut);
            System.Diagnostics.Debug.WriteLine(testResultOut);
            
        }


        [TestMethod]
        public void TestEncodeAllStringLiterals()
        {
            var input = "        revRoom.ValidationExpression = \"^[a-zA-Z0-9''\" + Convert.ToString(ConfigurationManager.AppSettings[\"PreyNameAllowedCharecters\"]) + \"]+$\";";
            var testResult = NoFuture.Gen.Settings.LangStyle.EncodeAllStringLiterals(input);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);

            var testInput = new StringBuilder();
            testInput.Append('_');
            testInput.Append('f');
            testInput.Append('i');
            testInput.Append('l');
            testInput.Append('e');
            testInput.Append('S');
            testInput.Append('a');
            testInput.Append('v');
            testInput.Append('e');
            testInput.Append('P');
            testInput.Append('a');
            testInput.Append('t');
            testInput.Append('h');
            testInput.Append(' ');
            testInput.Append('=');
            testInput.Append(' ');
            testInput.Append('"');
            testInput.Append('P');
            testInput.Append('r');
            testInput.Append('o');
            testInput.Append('g');
            testInput.Append('r');
            testInput.Append('a');
            testInput.Append('m');
            testInput.Append('M');
            testInput.Append('a');
            testInput.Append('n');
            testInput.Append('a');
            testInput.Append('g');
            testInput.Append('e');
            testInput.Append('m');
            testInput.Append('e');
            testInput.Append('n');
            testInput.Append('t');
            testInput.Append((char)0x5c);
            testInput.Append((char)0x5c);
            testInput.Append('P');
            testInput.Append('r');
            testInput.Append('o');
            testInput.Append('g');
            testInput.Append('r');
            testInput.Append('a');
            testInput.Append('m');
            testInput.Append('S');
            testInput.Append('e');
            testInput.Append('t');
            testInput.Append('u');
            testInput.Append('p');
            testInput.Append((char)0x5c);
            testInput.Append((char)0x5c);
            testInput.Append('F');
            testInput.Append('i');
            testInput.Append('l');
            testInput.Append('e');
            testInput.Append('s');
            testInput.Append('U');
            testInput.Append('p');
            testInput.Append('l');
            testInput.Append('o');
            testInput.Append('a');
            testInput.Append('d');
            testInput.Append('e');
            testInput.Append('d');
            testInput.Append((char)0x5c);
            testInput.Append((char)0x5c);
            testInput.Append('"');
            testInput.Append(' ');
            testInput.Append('+');
            testInput.Append(' ');
            testInput.Append('C');
            testInput.Append('o');
            testInput.Append('n');
            testInput.Append('v');
            testInput.Append('e');
            testInput.Append('r');
            testInput.Append('t');
            testInput.Append('.');
            testInput.Append('T');
            testInput.Append('o');
            testInput.Append('S');
            testInput.Append('t');
            testInput.Append('r');
            testInput.Append('i');
            testInput.Append('n');
            testInput.Append('g');
            testInput.Append('(');
            testInput.Append('S');
            testInput.Append('e');
            testInput.Append('s');
            testInput.Append('s');
            testInput.Append('i');
            testInput.Append('o');
            testInput.Append('n');
            testInput.Append('[');
            testInput.Append('S');
            testInput.Append('e');
            testInput.Append('s');
            testInput.Append('s');
            testInput.Append('i');
            testInput.Append('o');
            testInput.Append('n');
            testInput.Append('C');
            testInput.Append('o');
            testInput.Append('n');
            testInput.Append('t');
            testInput.Append('a');
            testInput.Append('n');
            testInput.Append('t');
            testInput.Append('s');
            testInput.Append('.');
            testInput.Append('P');
            testInput.Append('M');
            testInput.Append('_');
            testInput.Append('P');
            testInput.Append('R');
            testInput.Append('O');
            testInput.Append('G');
            testInput.Append('R');
            testInput.Append('A');
            testInput.Append('M');
            testInput.Append('I');
            testInput.Append('D');
            testInput.Append(']');
            testInput.Append(')');
            testInput.Append(' ');
            testInput.Append('+');
            testInput.Append(' ');
            testInput.Append('"');
            testInput.Append((char)0x5c);
            testInput.Append((char)0x5c);
            testInput.Append('P');
            testInput.Append('r');
            testInput.Append('o');
            testInput.Append('g');
            testInput.Append('r');
            testInput.Append('a');
            testInput.Append('m');
            testInput.Append('D');
            testInput.Append('o');
            testInput.Append('c');
            testInput.Append('u');
            testInput.Append('m');
            testInput.Append('e');
            testInput.Append('n');
            testInput.Append('t');
            testInput.Append('s');
            testInput.Append((char)0x5c);
            testInput.Append((char)0x5c);
            testInput.Append('"');
            testInput.Append(';');
            testResult = NoFuture.Gen.Settings.LangStyle.EncodeAllStringLiterals(testInput.ToString());
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);

        }

        [TestMethod]
        public void TestExtractAllStringLiterals()
        {
            var input =
                "        revRoom.ValidationExpression = \"^[a-zA-Z0-9''\" + Convert.ToString(ConfigurationManager.AppSettings[\"PreyNameAllowedCharecters\"]) + \"]+$\";";
            var testResult = NoFuture.Gen.Settings.LangStyle.ExtractAllStringLiterals(input);
            foreach (var k in testResult.Keys)
                System.Diagnostics.Debug.WriteLine(string.Format("[{0}] - '{1}'", k, testResult[k]));

            Assert.AreEqual(3, testResult.Count);
            var testInput = new StringBuilder();
            testInput.Append('_');
            testInput.Append('f');
            testInput.Append('i');
            testInput.Append('l');
            testInput.Append('e');
            testInput.Append('S');
            testInput.Append('a');
            testInput.Append('v');
            testInput.Append('e');
            testInput.Append('P');
            testInput.Append('a');
            testInput.Append('t');
            testInput.Append('h');
            testInput.Append(' ');
            testInput.Append('=');
            testInput.Append(' ');
            testInput.Append('"');
            testInput.Append('P');
            testInput.Append('r');
            testInput.Append('o');
            testInput.Append('g');
            testInput.Append('r');
            testInput.Append('a');
            testInput.Append('m');
            testInput.Append('M');
            testInput.Append('a');
            testInput.Append('n');
            testInput.Append('a');
            testInput.Append('g');
            testInput.Append('e');
            testInput.Append('m');
            testInput.Append('e');
            testInput.Append('n');
            testInput.Append('t');
            testInput.Append((char) 0x5c);
            testInput.Append((char) 0x5c);
            testInput.Append('P');
            testInput.Append('r');
            testInput.Append('o');
            testInput.Append('g');
            testInput.Append('r');
            testInput.Append('a');
            testInput.Append('m');
            testInput.Append('S');
            testInput.Append('e');
            testInput.Append('t');
            testInput.Append('u');
            testInput.Append('p');
            testInput.Append((char) 0x5c);
            testInput.Append((char) 0x5c);
            testInput.Append('F');
            testInput.Append('i');
            testInput.Append('l');
            testInput.Append('e');
            testInput.Append('s');
            testInput.Append('U');
            testInput.Append('p');
            testInput.Append('l');
            testInput.Append('o');
            testInput.Append('a');
            testInput.Append('d');
            testInput.Append('e');
            testInput.Append('d');
            testInput.Append((char) 0x5c);
            testInput.Append((char) 0x5c);
            testInput.Append('"');
            testInput.Append(' ');
            testInput.Append('+');
            testInput.Append(' ');
            testInput.Append('C');
            testInput.Append('o');
            testInput.Append('n');
            testInput.Append('v');
            testInput.Append('e');
            testInput.Append('r');
            testInput.Append('t');
            testInput.Append('.');
            testInput.Append('T');
            testInput.Append('o');
            testInput.Append('S');
            testInput.Append('t');
            testInput.Append('r');
            testInput.Append('i');
            testInput.Append('n');
            testInput.Append('g');
            testInput.Append('(');
            testInput.Append('S');
            testInput.Append('e');
            testInput.Append('s');
            testInput.Append('s');
            testInput.Append('i');
            testInput.Append('o');
            testInput.Append('n');
            testInput.Append('[');
            testInput.Append('S');
            testInput.Append('e');
            testInput.Append('s');
            testInput.Append('s');
            testInput.Append('i');
            testInput.Append('o');
            testInput.Append('n');
            testInput.Append('C');
            testInput.Append('o');
            testInput.Append('n');
            testInput.Append('t');
            testInput.Append('a');
            testInput.Append('n');
            testInput.Append('t');
            testInput.Append('s');
            testInput.Append('.');
            testInput.Append('P');
            testInput.Append('M');
            testInput.Append('_');
            testInput.Append('P');
            testInput.Append('R');
            testInput.Append('O');
            testInput.Append('G');
            testInput.Append('R');
            testInput.Append('A');
            testInput.Append('M');
            testInput.Append('I');
            testInput.Append('D');
            testInput.Append(']');
            testInput.Append(')');
            testInput.Append(' ');
            testInput.Append('+');
            testInput.Append(' ');
            testInput.Append('"');
            testInput.Append((char) 0x5c);
            testInput.Append((char) 0x5c);
            testInput.Append('P');
            testInput.Append('r');
            testInput.Append('o');
            testInput.Append('g');
            testInput.Append('r');
            testInput.Append('a');
            testInput.Append('m');
            testInput.Append('D');
            testInput.Append('o');
            testInput.Append('c');
            testInput.Append('u');
            testInput.Append('m');
            testInput.Append('e');
            testInput.Append('n');
            testInput.Append('t');
            testInput.Append('s');
            testInput.Append((char) 0x5c);
            testInput.Append((char) 0x5c);
            testInput.Append('"');
            testInput.Append(';');
            testResult = Settings.LangStyle.ExtractAllStringLiterals(testInput.ToString());
            foreach (var k in testResult.Keys)
                System.Diagnostics.Debug.WriteLine(string.Format("[{0}] - '{1}'", k, testResult[k]));
            Assert.AreEqual(2, testResult.Count);

            input = "var aHereString = @\"Here Is a Here String\";";
            testResult = Settings.LangStyle.ExtractAllStringLiterals(input);
            foreach (var k in testResult.Keys)
                System.Diagnostics.Debug.WriteLine(string.Format("[{0}] - '{1}'", k, testResult[k]));

            Assert.IsNotNull(testResult);
            Assert.AreEqual(1, testResult.Count);
            System.Diagnostics.Debug.WriteLine(input.ToCharArray()[18]);

        }

        [TestMethod]
        public void TestToCsParam()
        {
            var testSubject = new CgMember()
            {
                Args =
                    new List<CgArg>
                    {
                        new CgArg {ArgName = "param1", ArgType = "int"},
                        new CgArg {ArgName = "param2", ArgType = "string"},
                        new CgArg {ArgName = "param3", ArgType = "bool"}
                    },
                Name = "MyMethodName",
                TypeName = "System.String"
            };

            var testResult = NoFuture.Gen.Settings.LangStyle.ToParam(testSubject, true);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult.ToString().StartsWith("Func<"));
        }
    }
}
