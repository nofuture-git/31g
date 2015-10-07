using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;
using NoFuture.Shared;
using NoFuture.Shared.DiaSdk.LinesSwitch;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestCgMember
    {
        [TestMethod]
        public void TestToCsDecl()
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
                TypeName =  "System.String"
            };

            var testResult = NoFuture.Gen.Settings.LangStyle.ToDecl(testSubject);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);
            Assert.IsTrue(testResult.StartsWith(testSubject.TypeName));
            System.Diagnostics.Debug.WriteLine(testResult);

            testSubject.TypeName = "MyNamespace.Here.MyTypeName";
            testSubject.Name = ".ctor";
            testSubject.IsCtor = true;
            testResult = NoFuture.Gen.Settings.LangStyle.ToDecl(testSubject);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        
        }

        [TestMethod]
        public void TestToCsStmt()
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
            var testResult = NoFuture.Gen.Settings.LangStyle.ToStmt(testSubject, "MyNamespace.Here", "MyTypeName");
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);
            Assert.IsTrue(testResult.StartsWith("return"));

            testSubject.TypeName = "void";
            testResult = NoFuture.Gen.Settings.LangStyle.ToStmt(testSubject, "MyNamespace.Here", "MyTypeName");
            System.Diagnostics.Debug.WriteLine(testResult);

            Assert.IsTrue(testResult.StartsWith("MyNamespace"));

            testSubject.TypeName = "MyNamespace.Here.MyTypeName";
            testSubject.Name = ".ctor";
            testSubject.IsCtor = true;
            testResult = NoFuture.Gen.Settings.LangStyle.ToStmt(testSubject, null, null);
            Assert.IsNotNull(testResult);
            Assert.AreEqual("new MyNamespace.Here.MyTypeName(param1,param2,param3);", testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestAsInvokeRegexPattern()
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
            var testResult = testSubject.AsInvokeRegexPattern();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);
            Assert.AreNotEqual(".",testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch("MyMethodName(11,\"a string literal\",true)",testResult));

            testSubject.IsStatic = true;
            testSubject.MyCgType = new CgType {Namespace = "NoFuture.TestNs", Name = "Something"};
            testSubject.IsGeneric = true;
            testResult = testSubject.AsInvokeRegexPattern();
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject.IsStatic = false;
            testResult = testSubject.AsInvokeRegexPattern("myVar", "myVar.ItsTypes.Types.Type");
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject.MyCgType.IsEnum = true;
            testSubject.IsGeneric = false;
            testSubject.Name = "MyEnum";
            testSubject.MyCgType.EnumValueDictionary.Add("MyType", new[] { "Instance", "NonPublic", "Public", "DeclaredOnly" });
            testResult = testSubject.AsInvokeRegexPattern();
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);

            testSubject = new CgMember()
            {
                Name = "MyPropertyName",
                TypeName = "System.String"
            };

            testResult = testSubject.AsInvokeRegexPattern();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);
            Assert.AreNotEqual(".", testResult);
            System.Diagnostics.Debug.WriteLine(testResult);

        }

        [TestMethod]
        public void TestAsInvokeRegexPattern02()
        {
            var testSubject = new CgMember()
            {
                Name = "ToString",
                TypeName = "System.String",
                IsMethod = true
            };

            var testResult = testSubject.AsInvokeRegexPattern();

            var testCompare =
                "command.Parameters.Add(new SqlParameter(\"@createUser\", Convert.ToString(Session[SessionContants.LoggedInEntityId])));";
            Assert.IsFalse(System.Text.RegularExpressions.Regex.IsMatch(testCompare, testResult));
            System.Diagnostics.Debug.WriteLine(testResult);

            testCompare = "somethingElse.InvokeMe(4,true,\"sd\");var mysomething=ToString()";
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(testCompare, testResult));

        }
        #region Testfile contents
        public static string[] TestFileContent = new[]
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
 /*10*/     "using System.Data;",
            "using PlummetToxic.YYR.Common.Enumerations;",
            "using PlummetToxic.YYR.DTO;",
            "using PlummetToxic.YYR.Common.Constants;",
            "using PlummetToxic.YYR.UI.Controller.LimitManagement;",
            "using PlummetToxic.YYR.UI.Controller.Common;",
            "using PlummetToxic.YYR.UI.Controller.Admin;",
            "using PlummetToxic.YYR.Common.Utitility;",
            "using System.ComponentModel;",
            "using System.Collections;",
 /*20*/     "using System.Xml.XPath;",
            "using System.Net.Mail;",
            "using System.Net;",
            "using System.Net.Mime;",
            "using PlummetToxic.YYR.UI.Design.WReference;",
            "using System.Xml;",
            "using System.Data.SqlClient;",
            "",
            "namespace PlummetToxic.DPP.Onss",
            "{",
/*30*/      "	public partial class ViewWankathon : System.Web.UI.Page",
            "	{",
            "       /*Block Comments appear in ",
            "         the first line",
            "       */   ",
            "		string _strDate;",
            "		string _strStartTime = string.Empty;",
            "		string _strEndTime;",
            "		string _strDay;",
            "		string _strProductOrdered = string.Empty;",
/*40*/      "		string _strProdOrderedComponent;",
            "		string _strConfirmDill;",
            "		string _strProdPrice;",
            "		string _strGaProffFee;	",
            "		CommonController _cc;",
            "		",
            "		public ViewWankathon()",
            "		{",
            "			 _cc = new CommonController();",
            "		}",
/*50*/      "		protected void Page_Load(object sender, EventArgs e)",
            "		{",
            "			btnSave.Enabled = true;",
            "			lblError.Text = string.Empty;",
            "			revRoom.ValidationExpression = \"^[a-zA-Z0-9''\" + Convert.ToString(ConfigurationManager.AppSettings[\"ClientBatAllowedCharecters\"]) + \"]+$\";",
            "			cvRogerDate.ValueToCompare = DateTime.Now.ToString(\"MM/dd/yyyy\");",
            "			btnCreateRoger.Attributes.Add(\"onclick\", \"javascript:return GenerateFarslot()\");",
            "			if (!Page.IsPostBack)",
            "			{",
            "				grdvFileList.PageSize = Convert.ToString(ConfigurationManager.AppSettings.Get(\"ShortListRecordsPerPage\")) != string.Empty ? Convert.ToInt32(Convert.ToString(ConfigurationManager.AppSettings.Get(\"ShortListRecordsPerPage\"))) : 5;",
/*60*/      "				//As per the CR - 20 sent on 14 June 2011, Wanker: Dig SSig Date: 20 June 2011 Start",
            "				if (Session[SessionContants.PM_LimiterID] != null)",
            "				{",
            "					_objRogerController = new RogerController();",
            "					ViewState[\"showPrice\"] = _objRogerController.GetLimiterMasterDetails(Convert.ToString(Session[SessionContants.PM_LimiterID])).showPrice;",
            "				}",
            "				//End",
            "				//CR No - PMCR000005,  Wanker:Dr Wannabe, Date: 19/05/2011",
            "				hdnCurrentDate.Value = DateTime.Now.ToShortDateString();",
            "				//VenttBlockTimeslot();",
/*70*/      "				ViewState[\"sort\"] = \"ASC\";",
            "				ViewState[\"sortExpr\"] = null;",
            "				lblError.Visible = false;",
            "				ViewState[\"BucketD\"] = null;",
            "				ViewState[\"RogerDt\"] = null;",
            "				ViewState[\"StartTm\"] = null;",
            "				ViewState[\"EndTm\"] = null;",
            "				ViewState[\"WDixID\"] = null;",
            "				Session[SessionContants.Limiter_LIBOFDOC] = null;",
            "				if (Master != null) _lblBreadScrumpCurrent = (Label)Master.FindControl(\"lblBreadCrump\");",
 /*80*/     "				_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Roger >> New\";",
            "				Page.Title = @\"Plummet Toxic Create Roger New\";",
            "				hdnPageTitle.Value = Page.Title;",
            "				rngDate.MaximumValue = DateTime.Now.ToShortDateString();",
            "				rngDate.MinimumValue = DateTime.Now.ToShortDateString();",
            "				ddlTabtlLocation.Attributes.Add(\"onchange\", \"if (!onSelectedIndexChanged()){return};\"); //Code added by Dude CR LTNU_VENOM 3.1_25-June-2012",
            "				ShowAllFiles();//Added by Wanksalot on 22-April-2013. (Additional Inserts to Boosults Letter)",
            "				if (Session[SessionContants.LimiterPAGEMODE] != null)",
            "				{",
            "					if (Convert.ToString(Session[SessionContants.LimiterPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_NEW).ToUpper()))",
 /*90*/     "					{",
            "						//FromPageLoad = true;",
            "						if (Session[SessionContants.PM_LimiterID] != null)",
            "						{",
            "							if ((Session[SessionContants.PM_RogerID] != null))",
            "							{",
            "								DisplayRogerDetails(Convert.ToString(Session[SessionContants.PM_RogerID]));",
            "								DisableFields();",
            "								btnback.Enabled = true;",
            "								_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Donny >> View\";",
/*100*/     "								Page.Title = @\"Plummet Toxic Create Donny View\";",
            "								hdnPageTitle.Value = Page.Title;",
            "								lnkbtnRogerOtherDetails.Enabled = true;",
            "							}",
            "							else",
            "							{",
            "								EnableFields();",
            "								SetDefaultInfo();",
            "								Page.Title = @\"Plummet Toxic Create Roger New\";",
            "								hdnPageTitle.Value = Page.Title;",
/*110*/     "								TabContainer1.Visible = false;",
            "								lnkbtnRogerOtherDetails.Enabled = false;",
            "							}",
            "						}",
            "						else",
            "						{",
            "							DisableFields();",
            "							btnback.Enabled = true;",
            "							TabContainer1.Visible = false;",
            "						}",
/*120*/     "					}",
            "					else if (Convert.ToString(Session[SessionContants.LimiterPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_EDIT).ToUpper()))",
            "					{",
            "						if ((Session[SessionContants.PM_RogerID] != null) && (Convert.ToString(Session[SessionContants.PM_RogerPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_EDIT).ToUpper())))",
            "						{",
            "							DisplayRogerDetails(Convert.ToString(Session[SessionContants.PM_RogerID]));",
            "							_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Roger >> Edit\";",
            "							Page.Title = @\"Plummet Toxic Create Roger Edit\";",
            "							hdnPageTitle.Value = Page.Title;",
            "							lnkbtnRogerOtherDetails.Enabled = true;",
/*130*/     "							lblRendings.CssClass = GenericConstants.LABEL;",
            "							lblnonRending.CssClass = GenericConstants.LABEL;",
            "							if (!ddlRogerStatus.Text.Trim().Equals(string.Empty))",
            "							{",
            "								if (ddlRogerStatus.Text.Trim().Equals(\"CSNEW\") || ddlRogerStatus.Text.Trim().Equals(\"CSNESE\") || ddlRogerStatus.Text.Trim().Equals(\"CSCONF\"))",
            "								{",
            "									txtDeedsRequired.ReadOnly = false;",
            "									foreach (GridViewRow grvRow in grdvTabtlLocationInformation.Rows)",
            "									{",
            "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2Smart\");",
/*140*/     "										nyCityAint2Smart.Enabled = true;",
            "									}",
            "									foreach (GridViewRow grvRow in grdvFileList.Rows)",
            "									{",
            "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2Smart\");",
            "										nyCityAint2Smart.Enabled = true;",
            "									}",
            "									foreach (GridViewRow grvRow in grdvLitterList.Rows)",
            "									{",
            "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2SmartLitter\");",
/*150*/     "										nyCityAint2Smart.Enabled = true;",
            "									}",
            "								}",
            "								else",
            "								{",
            "									foreach (GridViewRow grvRow in grdvTabtlLocationInformation.Rows)",
            "									{",
            "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2Smart\");",
            "										nyCityAint2Smart.Enabled = false;",
            "									}",
/*160*/     "									foreach (GridViewRow grvRow in grdvFileList.Rows)",
            "									{",
            "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2Smart\");",
            "										nyCityAint2Smart.Enabled = false;",
            "									}",
            "									foreach (GridViewRow grvRow in grdvLitterList.Rows)",
            "									{",
            "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2SmartLitter\");",
            "										nyCityAint2Smart.Enabled = false;",
            "									}",
/*170*/     "								}",
            "							}",
            "							if (_objRogerController.getAppointmentRoger(Convert.ToString(Session[SessionContants.PM_RogerID])))",
            "								DisableFields();",
            "							if (ddlStates.SelectedValue != string.Empty)",
            "								txtDeedsingBoomerang.Text = _objcommoncontroller.GetDeedsingBoomerangBatByStateId(ddlStates.SelectedValue);",
            "							trLanndDeedsInfo.Visible = true;",
            "						}",
            "						else if ((Session[SessionContants.PM_RogerID] != null) && (Convert.ToString(Session[SessionContants.PM_RogerPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_VIEW).ToUpper())))",
            "						{",
/*180*/     "							DisplayRogerDetails(Convert.ToString(Session[SessionContants.PM_RogerID]));",
            "							DisableFields();",
            "							btnback.Enabled = true;",
            "							_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Fail >> View\";",
            "							Page.Title = @\"Plummet Toxic Create Fail View\";",
            "							hdnPageTitle.Value = Page.Title;",
            "							lblaremandatory.Enabled = false;",
            "							lblMandatary.Enabled = false;",
            "							lblstarmark.Enabled = false;",
            "							ddlCity.Enabled = false;",
/*190*/     "							ddlStates.Enabled = false;",
            "							lblmandRogerID.Enabled = false;",
            "							lblMandRogerType.Enabled = false;",
            "							lblmandRogerStatus.Enabled = false;",
            "							ddlRogerStatus.Enabled = false;",
            "							lblMandRogerDate.Enabled = false;",
            "							lblmandDixBat.Enabled = false;",
            "							lblmandRogerStartTime.Enabled = false;",
            "							lblmandProjectedTabtls.Enabled = false;",
            "							lblMandEndTime.Enabled = false;",
 /*200*/    "							lblMandDeedsArrivalTime.Enabled = false;",
            "							lblMandBucketDuration.Enabled = false;",
            "							lblMandAverageTotalBreakTime.Enabled = false;",
            "							btnCreateRoger.Visible = false;",
            "							btnTopPaneReset.Visible = false;",
            "							ddlRogerStatus.Enabled = false;",
            "							ddlRending.Enabled = false;",
            "							txtRending.Enabled = false;",
            "							txtnonRending.Enabled = false;",
            "							if (ddlStates.SelectedValue != string.Empty)",
/*210*/     "								txtDeedsingBoomerang.Text = _objcommoncontroller.GetDeedsingBoomerangBatByStateId(ddlStates.SelectedValue);",
            "							trLanndDeedsInfo.Visible = true;",
            "						}",
            "						else",
            "						{",
            "							//FromPageLoad = true;",
            "							TabContainer1.Visible = false;",
            "							if (Session[SessionContants.PM_LimiterID] != null)",
            "							{",
            "								EnableFields();",
/*220*/     "								SetDefaultInfo();",
            "								//Code Return Duplicate",
            "								var strPgmId = Convert.ToString(Session[SessionContants.PM_LimiterID]);",
            "								var objPmController = new LimiterController();",
            "								var objPm = objPmController.GetLimiterDetailsForLimiter(strPgmId);",
            "								if (objPm != null)",
            "								{",
            "									string strTermReturn;",
            "									if (objPm.TermReturnResponsibility != null)",
            "									{",
/*230*/     "										if (objPm.TermReturnResponsibility > 0)",
            "										{",
            "											strTermReturn = _objcommoncontroller.GetLookupDetailsAbbrevation(Convert.ToInt32(objPm.TermReturnResponsibility));",
            "											ddlReturnWinWait.SelectedIndex = ddlReturnWinWait.Items.IndexOf(ddlReturnWinWait.Items.FindByValue(strTermReturn));",
            "										}",
            "									}",
            "									else",
            "									{",
            "										strTermReturn = GenericConstants.TermLEADDeeds;",
            "										ddlReturnWinWait.SelectedIndex = ddlReturnWinWait.Items.IndexOf(ddlReturnWinWait.Items.FindByValue(strTermReturn));",
/*240*/     "									}",
            "								}",
            "							}",
            "						}",
            "					}",
            "					else if (Convert.ToString(Session[SessionContants.LimiterPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_VIEW).ToUpper()))",
            "					{",
            "						if ((Session[SessionContants.PM_RogerID] != null) && (Convert.ToString(Session[SessionContants.PM_RogerPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_VIEW).ToUpper())))",
            "						{",
            "							DisplayRogerDetails(Convert.ToString(Session[SessionContants.PM_RogerID]));",
/*250*/     "							DisableFields();",
            "							btnback.Enabled = true;",
            "							_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Roger >> View\";",
            "							Page.Title = @\"Plummet Toxic Create Roger View\";",
            "							hdnPageTitle.Value = Page.Title;",
            "							lnkbtnRogerOtherDetails.Enabled = true;",
            "							if (ddlStates.SelectedValue != string.Empty)",
            "								txtDeedsingBoomerang.Text = _objcommoncontroller.GetDeedsingBoomerangBatByStateId(ddlStates.SelectedValue);",
            "							trLanndDeedsInfo.Visible = true;",
            "						}",
/*260*/     "						else",
            "						{",
            "							DisableFields();",
            "							btnback.Enabled = true;",
            "							TabContainer1.Visible = false;",
            "							_objRogerController = new RogerController();",
            "							_uicommon = new UICommon();",
            "							FillTopFields(Convert.ToString(Session[SessionContants.PM_LimiterID]), false);",
            "						}",
            "						ddlRogerStatus.Enabled = false;",
/*270*/     "						txtDeedsRequired.ReadOnly = true;",
            "					}",
            "				}",
            "				if (_uicommon == null)",
            "				{",
            "					_uicommon = new UICommon();",
            "				}",
            "				if (_objLimiterMaster != null && (Convert.ToString(_uicommon.GetDescriptionByAbbrivation(_objLimiterMaster.LimiterType)).Equals(GenericConstants.IMMUNIZAT)))",
            "				{",
            "					lblStripClub.Visible = true;",
/*280*/     "					txtStripClub.Visible = true;",
            "				}",
            "				else",
            "				{",
            "					lblStripClub.Visible = false;",
            "					txtStripClub.Visible = false;",
            "				}",
            "				var intTabtlLoc = _uicommon.GetLookupdetailsID(StrOnDix);",
            "				var objPlummetToxicDataContext = new PlummetToxicDataContext();",
            "				var objPmsInfo = _objLimiterMaster == null",
/*290*/     "					? null",
            "					: objPlummetToxicDataContext.LimiterTabtlLocationInformations.SingleOrDefault(",
            "						pmsli =>",
            "							pmsli.LimiterId.Equals(_objLimiterMaster.LimiterId) &&",
            "							pmsli.TabtlLocation.Equals(intTabtlLoc));",
            "				if (objPmsInfo != null)",
            "				{",
            "					if (objPmsInfo.LabProcess != null)",
            "					{",
            "						rbBloodSamples.SelectedValue = objPmsInfo.LabProcess;",
/*300*/     "						rbBloodSamples.Enabled = false;",
            "					}",
            "					else",
            "						rbBloodSamples.Enabled = false;",
            "				}",
            "				else",
            "					rbBloodSamples.Enabled = false;",
            "				SetControlToolTip();",
            "				SetCheckboxValues();",
            "				chkBilling_CheckedChanged(null, null);",
/*310*/     "",
            "				if (_objAdminUiController.IsLanndFromWANK)",
            "				{",
            "					_uicommon.DisableControls(btnSave, btnCreateRoger, btnTemp, btnTest, btnTopPaneReset, chkBilling,",
            "						chkBlastRolloutPref, chkDoNotPrintResultLetters, chkITApplication, chkIsNoDataUpload, chkTermNotToBeReturned,",
            "						chkTabtlOption, chkSelectAll, ddlCity, ddlRogerEndTime, ddlRogerStartTime, ddlRogerStatus, ddlRending,",
            "						ddlReturnWinWait, ddlTabtlLocation, ddlDixBat, ddlDeedsArrivalTime, ddlStates, ddlWindowEndTime,",
            "						ddlWindowStartTime, fakeButton, lstApp, lsttimeslot, rbBloodSamples, txtLanndBat,",
            "						txtAdministrativeTime, txtAverageTotalBreakTime, txtBCCAddress, txtBucketDuration, txtCCAddress,",
            "						txtClerksNum, txtClientAssignedEventId, txtClientBat, txtRogerDate, txtRogerEndTime, txtRogerExHrPrice,",
/*320*/     "						txtRogerID, txtRogerID1, txtRogerID2, txtRogerID3, txtRogerNursePrice, txtRogerOtherDesc, txtRogerStartTime,",
            "						txtDaysReport, txtEstimatedParticipationRate, txtExClinHrDesc, txtExRogerNurseDesc, txtRending,",
            "						txtFromAddress, txtNumberofemployee, txtOtherPrice, txtPhoneNumber, txtLimiterDescription, txtLimiterManager,",
            "						txtLimiterTitle, txtLimiterType, txtLimiterenddate, txtLimiterstartdate, txtProjectedTabtls,",
            "						txtRoom, txtTabtlmodel, txtSetupTime, txtDixContactBat, txtSpOtherDesc, txtSpOtherPrice, txtDeedsArrivalTime,",
            "						txtDeedsingBoomerang, txtSubject, txtTeardownTime, txtStripClub, txtToAddress, txtTravelDesc,",
            "						txtTravelPrice, txtWindowEndTime, txtWindowStartTime, txtemailaddr, txtnonRending, txtOtherBiLingualDeeds,",
            "						txtwndendt, txtwndstdt);",
            "				}",
            "			}",
/*330*/     "			LoadLsttimeslot();",
            "		}",
            "		protected void Page_PreRender(object sender, EventArgs e)",
            "		{",
            "			if (hdnPageTitle.Value.Trim().Length > 0)",
            "				Page.Title = hdnPageTitle.Value;",
            "			//For DashBoard Return",
            "			if (!IsPostBack)",
            "			{",
            "				if (Session[SessionContants.FROMDODO] != null)",
/*340*/     "				{",
            "					if (Convert.ToBoolean(Session[SessionContants.FROMDODO]))",
            "					{",
            "						btnback.Text = @\"BACK\";",
            "						btnback.Visible = true;",
            "						btnback.Enabled = true;",
            "					}",
            "				}",
            "			}",
            "			//This code looted by Masta Wanka as per Qa Issue on 29 June 2011",
/*350*/     "			txtClerksNum.ReadOnly = txtLimiterType.Text.Equals(\"Deductable Semen\") || txtScreeningmodel.Text.ToUpper().Equals(\"MULTI-WANK!\");",
            "			//End Code",
            "			//For Dodo or dookie Return",
            "		}		",
            "		protected void ddlScreeningLocation_SelectedIndexChanged(object sender, EventArgs e)",
            "		{",
            "			hdnTestIDs.Value = string.Empty;",
            "			hdnVenomIDs.Value = string.Empty;",
            "			hdnSelectCount.Value = \"0\";",
            "			GetYoMommaTypes();",
/*360*/     "		}",
            "		",
            "		protected void GetYoMommaTypes()",
            "		{",
            "			_objSymtomController = new SymtomController();",
            "			var NeedleId = ddlNeedleName.SelectedValue;",
            "			if (!NeedleId.Equals(string.Empty))",
            "			{",
            "				var intEmployeeCount = _objSymtomController.GetEmployeeCountFromNeedles(NeedleId);",
            "				if (intEmployeeCount > 0)",
/*370*/     "				{",
            "					txtArrgofemployee.Text = Convert.ToString(intEmployeeCount);",
            "					if (txtEstimatedLensToplessionRate.Text.Trim().Length > 0)",
            "					{",
            "						var dcEstimatePercent = Convert.ToDecimal(txtEstimatedLensToplessionRate.Text.Trim());",
            "						var intProjectedSplatters = Convert.ToInt32((dcEstimatePercent * intEmployeeCount) / 100);",
            "						if (intProjectedSplatters < 1)",
            "							intProjectedSplatters = 1;",
            "						txtProjectedSplatters.Text = Convert.ToString(intProjectedSplatters);",
            "					}",
/*380*/     "				}",
            "				else",
            "				{",
            "					txtArrgofemployee.Text = string.Empty;",
            "				}",
            "				lblNeedleContactName.Visible = true;",
            "				txtNeedleContactName.Visible = true;",
            "				lblPhoneArrg.Visible = true;",
            "				txtPhoneArrg.Visible = true;",
            "				//change request added by M. Wanka (of S-D Wanker)",
/*390*/     "				lblemail.Visible = true;",
            "				txtemailaddr.Visible = true;",
            "				//end ",
            "				lblMasswankathonManager.Visible = true;",
            "				txtMasswankathonManager.Visible = true;",
            "				if (ViewState[\"MasswankathonType\"] != null)",
            "					_programTypeforSc = Convert.ToInt32(ViewState[\"MasswankathonType\"]);",
            "				if (ddlNeedleName.SelectedValue.StartsWith(\"L\") || ddlNeedleName.SelectedValue.StartsWith(\"C\"))",
            "				{",
            "					var dsNeedleContact = _objcommoncontroller.GetNeedleContactDetailsForNeedleId(Convert.ToString(ddlNeedleName.SelectedValue), _programTypeforSc);",
/*400*/     "					var isContactAvailable = false;",
            "					if (dsNeedleContact != null)",
            "					{",
            "						if (dsNeedleContact.Tables.Count > 0)",
            "							if (dsNeedleContact.Tables[0].Rows.Count > 0)",
            "							{",
            "								txtNeedleContactName.Text = Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactName\"]) != string.Empty ? Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactName\"]) : string.Empty;",
            "								txtPhoneArrg.Text = Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactArrg\"]) != string.Empty ? Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactArrg\"]) : string.Empty;",
            "								txtemailaddr.Text = Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactEmail\"]) != string.Empty ? Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactEmail\"]) : string.Empty;",
            "								isContactAvailable = true;",
/*410*/     "							}",
            "					}",
            "					if (!isContactAvailable)",
            "					{",
            "						txtNeedleContactName.Text = string.Empty;",
            "						txtPhoneArrg.Text = string.Empty;",
            "						txtemailaddr.Text = string.Empty;",
            "					}",
            "					//End - by Viswa on 20/06/2011 as getting from Dataset as CR changes.",
            "				}",
/*420*/     "				if (Session[SessionContants.PM_PROGRAMID] != null)",
            "				{",
            "					var strMasswankathonId = Convert.ToString(Session[SessionContants.PM_PROGRAMID]);",
            "					var dsLpm = _objcommoncontroller.GetLPMAndLBPMDetails(strMasswankathonId, \"PROGRAMLEVEL\");",
            "					if (dsLpm != null)",
            "					{",
            "						if (dsLpm.Tables.Count > 0)",
            "							if (dsLpm.Tables[0].Rows.Count > 0)",
            "								txtMasswankathonManager.Text = Convert.ToString(dsLpm.Tables[0].Rows[0][\"MasswankathonManager\"]);",
            "					}",
/*430*/     "				}",
            "			}",
            "			else",
            "			{",
            "				txtArrgofemployee.Text = string.Empty;",
            "				txtProjectedSplatters.Text = string.Empty;",
            "				txtEstimatedLensToplessionRate.Text = hdnEstimatePartPercent.Value;",
            "				lblNeedleContactName.Visible = false;",
            "				txtNeedleContactName.Visible = false;",
            "				lblPhoneArrg.Visible = false;",
/*440*/     "				txtPhoneArrg.Visible = false;",
            "				//change request added by madhuri",
            "				lblemail.Visible = false;",
            "				txtemailaddr.Visible = false;",
            "				//end",
            "				lblMasswankathonManager.Visible = false;",
            "				txtMasswankathonManager.Visible = false;",
            "			}",
            "		}	",
            "		",
/*450*/     "		",
            "		[System.Web.Services.Protocols.SoapDocumentMethodAttribute(\"http://tempuri.org/1stMethod\")]",
            "		[return: System.Xml.Serialization.XmlElementAttribute(\"1stMethodResult\")]",
            "		protected void ddlScreeningLocation_SelectedIndexChanged(object sender, EventArgs e)",
            "		{",
            "			hdnTestIDs.Value = string.Empty;",
            "			hdnVenomIDs.Value = string.Empty;",
            "			hdnSelectCount.Value = \"0\";",
            "			GetYoMommaTypes();",
            "		}",
            "		",
/*460*/     "	}",
            "}"
        };
        #endregion

        [TestMethod]
        public void TestMyStartEnclosure()
        {
            var testCgMem = new CgMember()
            {
                Name = "GetYoMommaTypes",
                IsMethod = true,
                PdbModuleSymbols = new ModuleSymbols(){lastLine = new PdbLineNumber(){lineNumber = 365}, firstLine = new PdbLineNumber(){lineNumber = 445}},
                AccessModifier = CgAccessModifier.Family
            };

            var testResult = testCgMem.GetMyStartEnclosure(TestFileContent);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(362, testResult.Item1);
            Assert.AreEqual(1,testResult.Item2);
            System.Diagnostics.Debug.WriteLine(string.Format("{0},{1}", testResult.Item1, testResult.Item2));

            testCgMem = new CgMember()
            {
                Name = "ddlScreeningLocation_SelectedIndexChanged",
                Args =
                    new List<CgArg>
                    {
                        new CgArg {ArgName = "sender", ArgType = "object"},
                        new CgArg {ArgName = "e", ArgType = "EventArgs"}
                    },
                IsMethod = true,
                PdbModuleSymbols = new ModuleSymbols() { lastLine = new PdbLineNumber() { lineNumber = 453 }, firstLine = new PdbLineNumber() { lineNumber = 458 } },
                AccessModifier = CgAccessModifier.Family
            };
            testResult = testCgMem.GetMyStartEnclosure(TestFileContent);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(354, testResult.Item1);
            Assert.AreEqual(1, testResult.Item2);
            System.Diagnostics.Debug.WriteLine(string.Format("{0},{1}", testResult.Item1, testResult.Item2));
        }

        [TestMethod]
        public void TestGetMyEndEnclosure()
        {
            var testCgMem = new CgMember()
            {
                Name = "ddlScreeningLocation_SelectedIndexChanged",
                Args =
                    new List<CgArg>
                    {
                        new CgArg {ArgName = "sender", ArgType = "object"},
                        new CgArg {ArgName = "e", ArgType = "EventArgs"}
                    },
                IsMethod = true,
                PdbModuleSymbols = new ModuleSymbols() { firstLine = new PdbLineNumber() { lineNumber = 356 }, lastLine = new PdbLineNumber() { lineNumber = 359 } },
                AccessModifier = CgAccessModifier.Family
            };

            var testSrcFile = TestFileContent;

            var testResult = testCgMem.GetMyEndEnclosure(testSrcFile);
            System.Diagnostics.Debug.WriteLine(string.Format("{0},{1}", testResult.Item1, testResult.Item2));
        }

    }
}
