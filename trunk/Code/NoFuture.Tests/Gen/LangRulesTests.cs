using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using NoFuture.Gen;
using NoFuture.Shared;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class LangRulesTests
    {

        private string[] testFileContent = new[]
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
                "using PlummetToxic.YYR.Common.Enumerations;",
                "using PlummetToxic.YYR.DTO;",
                "using PlummetToxic.YYR.Common.Constants;",
                "using PlummetToxic.YYR.UI.Controller.LimitManagement;",
                "using PlummetToxic.YYR.UI.Controller.Common;",
                "using PlummetToxic.YYR.UI.Controller.Admin;",
                "using PlummetToxic.YYR.Common.Utitility;",
                "using System.ComponentModel;",
                "using System.Collections;",
                "using System.Xml.XPath;",
                "using System.Net.Mail;",
                "using System.Net;",
                "using System.Net.Mime;",
                "using PlummetToxic.YYR.UI.Design.WReference;",
                "using System.Xml;",
                "using System.Data.SqlClient;",
                "",
                "namespace PlummetToxic.DPP.Onss",
                "{",
                "	public partial class ViewWankathon : System.Web.UI.Page",
                "	{",
                "       /*Block Comments appear in ",
                "         the first line",
                "       */   ",
                "		string _strDate;",
                "		string _strStartTime = string.Empty;",
                "		string _strEndTime;",
                "		string _strDay;",
                "		string _strProductOrdered = string.Empty;",
                "		string _strProdOrderedComponent;",
                "		string _strConfirmDill;",
                "		string _strProdPrice;",
                "		string _strGaProffFee;	",
                "		CommonController _cc;",
                "		",
                "		public ViewWankathon()",
                "		{",
                "			 _cc = new CommonController();",
                "		}",
                "		protected void Page_Load(object sender, EventArgs e)",
                "		{",
                "			btnSave.Enabled = true;",
                "			lblError.Text = string.Empty;",
                "			revRoom.ValidationExpression = \"^[a-zA-Z0-9''\" + Convert.ToString(ConfigurationManager.AppSettings[\"ClientBatAllowedCharecters\"]) + \"]+$\";",
                "			cvRogerDate.ValueToCompare = DateTime.Now.ToString(\"MM/dd/yyyy\");",
                "			btnCreateRoger.Attributes.Add(\"onclick\", \"javascript:return GenerateFarslot()\");",
                "			if (!Page.IsPostBack)",
                "			{",
                "				grdvFileList.PageSize = Convert.ToString(ConfigurationManager.AppSettings.Get(\"ShortListRecordsPerPage\")) != string.Empty ? Convert.ToInt32(Convert.ToString(ConfigurationManager.AppSettings.Get(\"ShortListRecordsPerPage\"))) : 5;",
                "				//As per the CR - 20 sent on 14 June 2011, Wanker: Dig SSig Date: 20 June 2011 Start",
                "				if (Session[SessionContants.PM_LimiterID] != null)",
                "				{",
                "					_objRogerController = new RogerController();",
                "					ViewState[\"showPrice\"] = _objRogerController.GetLimiterMasterDetails(Convert.ToString(Session[SessionContants.PM_LimiterID])).showPrice;",
                "				}",
                "				//End",
                "				//CR No - PMCR000005,  Wanker:Dr Wannabe, Date: 19/05/2011",
                "				hdnCurrentDate.Value = DateTime.Now.ToShortDateString();",
                "				//VenttBlockTimeslot();",
                "				ViewState[\"sort\"] = \"ASC\";",
                "				ViewState[\"sortExpr\"] = null;",
                "				lblError.Visible = false;",
                "				ViewState[\"BucketD\"] = null;",
                "				ViewState[\"RogerDt\"] = null;",
                "				ViewState[\"StartTm\"] = null;",
                "				ViewState[\"EndTm\"] = null;",
                "				ViewState[\"WDixID\"] = null;",
                "				Session[SessionContants.Limiter_LIBOFDOC] = null;",
                "				if (Master != null) _lblBreadScrumpCurrent = (Label)Master.FindControl(\"lblBreadCrump\");",
                "				_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Roger >> New\";",
                "				Page.Title = @\"Plummet Toxic Create Roger New\";",
                "				hdnPageTitle.Value = Page.Title;",
                "				rngDate.MaximumValue = DateTime.Now.ToShortDateString();",
                "				rngDate.MinimumValue = DateTime.Now.ToShortDateString();",
                "				ddlTabtlLocation.Attributes.Add(\"onchange\", \"if (!onSelectedIndexChanged()){return};\"); //Code added by Dude CR LTNU_VENOM 3.1_25-June-2012",
                "				ShowAllFiles();//Added by Wanksalot on 22-April-2013. (Additional Inserts to Boosults Letter)",
                "				if (Session[SessionContants.LimiterPAGEMODE] != null)",
                "				{",
                "					if (Convert.ToString(Session[SessionContants.LimiterPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_NEW).ToUpper()))",
                "					{",
                "						//FromPageLoad = true;",
                "						if (Session[SessionContants.PM_LimiterID] != null)",
                "						{",
                "							if ((Session[SessionContants.PM_RogerID] != null))",
                "							{",
                "								DisplayRogerDetails(Convert.ToString(Session[SessionContants.PM_RogerID]));",
                "								DisableFields();",
                "								btnback.Enabled = true;",
                "								_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Donny >> View\";",
                "								Page.Title = @\"Plummet Toxic Create Donny View\";",
                "								hdnPageTitle.Value = Page.Title;",
                "								lnkbtnRogerOtherDetails.Enabled = true;",
                "							}",
                "							else",
                "							{",
                "								EnableFields();",
                "								SetDefaultInfo();",
                "								Page.Title = @\"Plummet Toxic Create Roger New\";",
                "								hdnPageTitle.Value = Page.Title;",
                "								TabContainer1.Visible = false;",
                "								lnkbtnRogerOtherDetails.Enabled = false;",
                "							}",
                "						}",
                "						else",
                "						{",
                "							DisableFields();",
                "							btnback.Enabled = true;",
                "							TabContainer1.Visible = false;",
                "						}",
                "					}",
                "					else if (Convert.ToString(Session[SessionContants.LimiterPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_EDIT).ToUpper()))",
                "					{",
                "						if ((Session[SessionContants.PM_RogerID] != null) && (Convert.ToString(Session[SessionContants.PM_RogerPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_EDIT).ToUpper())))",
                "						{",
                "							DisplayRogerDetails(Convert.ToString(Session[SessionContants.PM_RogerID]));",
                "							_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Roger >> Edit\";",
                "							Page.Title = @\"Plummet Toxic Create Roger Edit\";",
                "							hdnPageTitle.Value = Page.Title;",
                "							lnkbtnRogerOtherDetails.Enabled = true;",
                "							lblRendings.CssClass = GenericConstants.LABEL;",
                "							lblnonRending.CssClass = GenericConstants.LABEL;",
                "							if (!ddlRogerStatus.Text.Trim().Equals(string.Empty))",
                "							{",
                "								if (ddlRogerStatus.Text.Trim().Equals(\"CSNEW\") || ddlRogerStatus.Text.Trim().Equals(\"CSNESE\") || ddlRogerStatus.Text.Trim().Equals(\"CSCONF\"))",
                "								{",
                "									txtDeedsRequired.ReadOnly = false;",
                "									foreach (GridViewRow grvRow in grdvTabtlLocationInformation.Rows)",
                "									{",
                "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2Smart\");",
                "										nyCityAint2Smart.Enabled = true;",
                "									}",
                "									foreach (GridViewRow grvRow in grdvFileList.Rows)",
                "									{",
                "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2Smart\");",
                "										nyCityAint2Smart.Enabled = true;",
                "									}",
                "									foreach (GridViewRow grvRow in grdvLitterList.Rows)",
                "									{",
                "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2SmartLitter\");",
                "										nyCityAint2Smart.Enabled = true;",
                "									}",
                "								}",
                "								else",
                "								{",
                "									foreach (GridViewRow grvRow in grdvTabtlLocationInformation.Rows)",
                "									{",
                "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2Smart\");",
                "										nyCityAint2Smart.Enabled = false;",
                "									}",
                "									foreach (GridViewRow grvRow in grdvFileList.Rows)",
                "									{",
                "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2Smart\");",
                "										nyCityAint2Smart.Enabled = false;",
                "									}",
                "									foreach (GridViewRow grvRow in grdvLitterList.Rows)",
                "									{",
                "										var nyCityAint2Smart = (CheckBox)grvRow.FindControl(\"nyCityAint2SmartLitter\");",
                "										nyCityAint2Smart.Enabled = false;",
                "									}",
                "								}",
                "							}",
                "							if (_objRogerController.getAppointmentRoger(Convert.ToString(Session[SessionContants.PM_RogerID])))",
                "								DisableFields();",
                "							if (ddlStates.SelectedValue != string.Empty)",
                "								txtDeedsingBoomerang.Text = _objcommoncontroller.GetDeedsingBoomerangBatByStateId(ddlStates.SelectedValue);",
                "							trLanndDeedsInfo.Visible = true;",
                "						}",
                "						else if ((Session[SessionContants.PM_RogerID] != null) && (Convert.ToString(Session[SessionContants.PM_RogerPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_VIEW).ToUpper())))",
                "						{",
                "							DisplayRogerDetails(Convert.ToString(Session[SessionContants.PM_RogerID]));",
                "							DisableFields();",
                "							btnback.Enabled = true;",
                "							_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Fail >> View\";",
                "							Page.Title = @\"Plummet Toxic Create Fail View\";",
                "							hdnPageTitle.Value = Page.Title;",
                "							lblaremandatory.Enabled = false;",
                "							lblMandatary.Enabled = false;",
                "							lblstarmark.Enabled = false;",
                "							ddlCity.Enabled = false;",
                "							ddlStates.Enabled = false;",
                "							lblmandRogerID.Enabled = false;",
                "							lblMandRogerType.Enabled = false;",
                "							lblmandRogerStatus.Enabled = false;",
                "							ddlRogerStatus.Enabled = false;",
                "							lblMandRogerDate.Enabled = false;",
                "							lblmandDixBat.Enabled = false;",
                "							lblmandRogerStartTime.Enabled = false;",
                "							lblmandProjectedTabtls.Enabled = false;",
                "							lblMandEndTime.Enabled = false;",
                "							lblMandDeedsArrivalTime.Enabled = false;",
                "							lblMandBucketDuration.Enabled = false;",
                "							lblMandAverageTotalBreakTime.Enabled = false;",
                "							btnCreateRoger.Visible = false;",
                "							btnTopPaneReset.Visible = false;",
                "							ddlRogerStatus.Enabled = false;",
                "							ddlRending.Enabled = false;",
                "							txtRending.Enabled = false;",
                "							txtnonRending.Enabled = false;",
                "							if (ddlStates.SelectedValue != string.Empty)",
                "								txtDeedsingBoomerang.Text = _objcommoncontroller.GetDeedsingBoomerangBatByStateId(ddlStates.SelectedValue);",
                "							trLanndDeedsInfo.Visible = true;",
                "						}",
                "						else",
                "						{",
                "							//FromPageLoad = true;",
                "							TabContainer1.Visible = false;",
                "							if (Session[SessionContants.PM_LimiterID] != null)",
                "							{",
                "								EnableFields();",
                "								SetDefaultInfo();",
                "								//Code Return Duplicate",
                "								var strPgmId = Convert.ToString(Session[SessionContants.PM_LimiterID]);",
                "								var objPmController = new LimiterController();",
                "								var objPm = objPmController.GetLimiterDetailsForLimiter(strPgmId);",
                "								if (objPm != null)",
                "								{",
                "									string strTermReturn;",
                "									if (objPm.TermReturnResponsibility != null)",
                "									{",
                "										if (objPm.TermReturnResponsibility > 0)",
                "										{",
                "											strTermReturn = _objcommoncontroller.GetLookupDetailsAbbrevation(Convert.ToInt32(objPm.TermReturnResponsibility));",
                "											ddlReturnWinWait.SelectedIndex = ddlReturnWinWait.Items.IndexOf(ddlReturnWinWait.Items.FindByValue(strTermReturn));",
                "										}",
                "									}",
                "									else",
                "									{",
                "										strTermReturn = GenericConstants.TermLEADDeeds;",
                "										ddlReturnWinWait.SelectedIndex = ddlReturnWinWait.Items.IndexOf(ddlReturnWinWait.Items.FindByValue(strTermReturn));",
                "									}",
                "								}",
                "							}",
                "						}",
                "					}",
                "					else if (Convert.ToString(Session[SessionContants.LimiterPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_VIEW).ToUpper()))",
                "					{",
                "						if ((Session[SessionContants.PM_RogerID] != null) && (Convert.ToString(Session[SessionContants.PM_RogerPAGEMODE]).ToUpper().Equals(Convert.ToString(GenericConstants.Limiter_VIEW).ToUpper())))",
                "						{",
                "							DisplayRogerDetails(Convert.ToString(Session[SessionContants.PM_RogerID]));",
                "							DisableFields();",
                "							btnback.Enabled = true;",
                "							_lblBreadScrumpCurrent.Text = @\"Limiter Management >> Roger Setup >> Create Roger >> View\";",
                "							Page.Title = @\"Plummet Toxic Create Roger View\";",
                "							hdnPageTitle.Value = Page.Title;",
                "							lnkbtnRogerOtherDetails.Enabled = true;",
                "							if (ddlStates.SelectedValue != string.Empty)",
                "								txtDeedsingBoomerang.Text = _objcommoncontroller.GetDeedsingBoomerangBatByStateId(ddlStates.SelectedValue);",
                "							trLanndDeedsInfo.Visible = true;",
                "						}",
                "						else",
                "						{",
                "							DisableFields();",
                "							btnback.Enabled = true;",
                "							TabContainer1.Visible = false;",
                "							_objRogerController = new RogerController();",
                "							_uicommon = new UICommon();",
                "							FillTopFields(Convert.ToString(Session[SessionContants.PM_LimiterID]), false);",
                "						}",
                "						ddlRogerStatus.Enabled = false;",
                "						txtDeedsRequired.ReadOnly = true;",
                "					}",
                "				}",
                "				if (_uicommon == null)",
                "				{",
                "					_uicommon = new UICommon();",
                "				}",
                "				if (_objLimiterMaster != null && (Convert.ToString(_uicommon.GetDescriptionByAbbrivation(_objLimiterMaster.LimiterType)).Equals(GenericConstants.IMMUNIZAT)))",
                "				{",
                "					lblStripClub.Visible = true;",
                "					txtStripClub.Visible = true;",
                "				}",
                "				else",
                "				{",
                "					lblStripClub.Visible = false;",
                "					txtStripClub.Visible = false;",
                "				}",
                "				var intTabtlLoc = _uicommon.GetLookupdetailsID(StrOnDix);",
                "				var objPlummetToxicDataContext = new PlummetToxicDataContext();",
                "				var objPmsInfo = _objLimiterMaster == null",
                "					? null",
                "					: objPlummetToxicDataContext.LimiterTabtlLocationInformations.SingleOrDefault(",
                "						pmsli =>",
                "							pmsli.LimiterId.Equals(_objLimiterMaster.LimiterId) &&",
                "							pmsli.TabtlLocation.Equals(intTabtlLoc));",
                "				if (objPmsInfo != null)",
                "				{",
                "					if (objPmsInfo.LabProcess != null)",
                "					{",
                "						rbBloodSamples.SelectedValue = objPmsInfo.LabProcess;",
                "						rbBloodSamples.Enabled = false;",
                "					}",
                "					else",
                "						rbBloodSamples.Enabled = false;",
                "				}",
                "				else",
                "					rbBloodSamples.Enabled = false;",
                "				SetControlToolTip();",
                "				SetCheckboxValues();",
                "				chkBilling_CheckedChanged(null, null);",
                "",
                "				if (_objAdminUiController.IsLanndFromWANK)",
                "				{",
                "					_uicommon.DisableControls(btnSave, btnCreateRoger, btnTemp, btnTest, btnTopPaneReset, chkBilling,",
                "						chkBlastRolloutPref, chkDoNotPrintResultLetters, chkITApplication, chkIsNoDataUpload, chkTermNotToBeReturned,",
                "						chkTabtlOption, chkSelectAll, ddlCity, ddlRogerEndTime, ddlRogerStartTime, ddlRogerStatus, ddlRending,",
                "						ddlReturnWinWait, ddlTabtlLocation, ddlDixBat, ddlDeedsArrivalTime, ddlStates, ddlWindowEndTime,",
                "						ddlWindowStartTime, fakeButton, lstApp, lsttimeslot, rbBloodSamples, txtLanndBat,",
                "						txtAdministrativeTime, txtAverageTotalBreakTime, txtBCCAddress, txtBucketDuration, txtCCAddress,",
                "						txtClerksNum, txtClientAssignedEventId, txtClientBat, txtRogerDate, txtRogerEndTime, txtRogerExHrPrice,",
                "						txtRogerID, txtRogerID1, txtRogerID2, txtRogerID3, txtRogerNursePrice, txtRogerOtherDesc, txtRogerStartTime,",
                "						txtDaysReport, txtEstimatedParticipationRate, txtExClinHrDesc, txtExRogerNurseDesc, txtRending,",
                "						txtFromAddress, txtNumberofemployee, txtOtherPrice, txtPhoneNumber, txtLimiterDescription, txtLimiterManager,",
                "						txtLimiterTitle, txtLimiterType, txtLimiterenddate, txtLimiterstartdate, txtProjectedTabtls,",
                "						txtRoom, txtTabtlmodel, txtSetupTime, txtDixContactBat, txtSpOtherDesc, txtSpOtherPrice, txtDeedsArrivalTime,",
                "						txtDeedsingBoomerang, txtSubject, txtTeardownTime, txtStripClub, txtToAddress, txtTravelDesc,",
                "						txtTravelPrice, txtWindowEndTime, txtWindowStartTime, txtemailaddr, txtnonRending, txtOtherBiLingualDeeds,",
                "						txtwndendt, txtwndstdt);",
                "				}",
                "			}",
                "			LoadLsttimeslot();",
                "		}",
                "		protected void Page_PreRender(object sender, EventArgs e)",
                "		{",
                "			if (hdnPageTitle.Value.Trim().Length > 0)",
                "				Page.Title = hdnPageTitle.Value;",
                "			//For DashBoard Return",
                "			if (!IsPostBack)",
                "			{",
                "				if (Session[SessionContants.FROMDODO] != null)",
                "				{",
                "					if (Convert.ToBoolean(Session[SessionContants.FROMDODO]))",
                "					{",
                "						btnback.Text = @\"BACK\";",
                "						btnback.Visible = true;",
                "						btnback.Enabled = true;",
                "					}",
                "				}",
                "			}",
                "			//This code looted by Masta Wanka as per Qa Issue on 29 June 2011",
                "			txtClerksNum.ReadOnly = txtLimiterType.Text.Equals(\"Deductable Semen\") || txtScreeningmodel.Text.ToUpper().Equals(\"MULTI-WANK!\");",
                "			//End Code",
                "			//For Dodo or dookie Return",
                "		}		",
                "		protected void ddlScreeningLocation_SelectedIndexChanged(object sender, EventArgs e)",
                "		{",
                "			hdnTestIDs.Value = string.Empty;",
                "			hdnVenomIDs.Value = string.Empty;",
                "			hdnSelectCount.Value = \"0\";",
                "			GetYoMommaTypes();",
                "		}",
                "		",
                "		protected void GetYoMommaTypes()",
                "		{",
                "			_objSymtomController = new SymtomController();",
                "			var NeedleId = ddlNeedleName.SelectedValue;",
                "			if (!NeedleId.Equals(string.Empty))",
                "			{",
                "				var intEmployeeCount = _objSymtomController.GetEmployeeCountFromNeedles(NeedleId);",
                "				if (intEmployeeCount > 0)",
                "				{",
                "					txtArrgofemployee.Text = Convert.ToString(intEmployeeCount);",
                "					if (txtEstimatedLensToplessionRate.Text.Trim().Length > 0)",
                "					{",
                "						var dcEstimatePercent = Convert.ToDecimal(txtEstimatedLensToplessionRate.Text.Trim());",
                "						var intProjectedSplatters = Convert.ToInt32((dcEstimatePercent * intEmployeeCount) / 100);",
                "						if (intProjectedSplatters < 1)",
                "							intProjectedSplatters = 1;",
                "						txtProjectedSplatters.Text = Convert.ToString(intProjectedSplatters);",
                "					}",
                "				}",
                "				else",
                "				{",
                "					txtArrgofemployee.Text = string.Empty;",
                "				}",
                "				lblNeedleContactName.Visible = true;",
                "				txtNeedleContactName.Visible = true;",
                "				lblPhoneArrg.Visible = true;",
                "				txtPhoneArrg.Visible = true;",
                "				//change request added by M. Wanka (of S-D Wanker)",
                "				lblemail.Visible = true;",
                "				txtemailaddr.Visible = true;",
                "				//end ",
                "				lblMasswankathonManager.Visible = true;",
                "				txtMasswankathonManager.Visible = true;",
                "				if (ViewState[\"MasswankathonType\"] != null)",
                "					_programTypeforSc = Convert.ToInt32(ViewState[\"MasswankathonType\"]);",
                "				if (ddlNeedleName.SelectedValue.StartsWith(\"L\") || ddlNeedleName.SelectedValue.StartsWith(\"C\"))",
                "				{",
                "					var dsNeedleContact = _objcommoncontroller.GetNeedleContactDetailsForNeedleId(Convert.ToString(ddlNeedleName.SelectedValue), _programTypeforSc);",
                "					var isContactAvailable = false;",
                "					if (dsNeedleContact != null)",
                "					{",
                "						if (dsNeedleContact.Tables.Count > 0)",
                "							if (dsNeedleContact.Tables[0].Rows.Count > 0)",
                "							{",
                "								txtNeedleContactName.Text = Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactName\"]) != string.Empty ? Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactName\"]) : string.Empty;",
                "								txtPhoneArrg.Text = Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactArrg\"]) != string.Empty ? Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactArrg\"]) : string.Empty;",
                "								txtemailaddr.Text = Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactEmail\"]) != string.Empty ? Convert.ToString(dsNeedleContact.Tables[0].Rows[0][\"contactEmail\"]) : string.Empty;",
                "								isContactAvailable = true;",
                "							}",
                "					}",
                "					if (!isContactAvailable)",
                "					{",
                "						txtNeedleContactName.Text = string.Empty;",
                "						txtPhoneArrg.Text = string.Empty;",
                "						txtemailaddr.Text = string.Empty;",
                "					}",
                "					//End - by Viswa on 20/06/2011 as getting from Dataset as CR changes.",
                "				}",
                "				if (Session[SessionContants.PM_PROGRAMID] != null)",
                "				{",
                "					var strMasswankathonId = Convert.ToString(Session[SessionContants.PM_PROGRAMID]);",
                "					var dsLpm = _objcommoncontroller.GetLPMAndLBPMDetails(strMasswankathonId, \"PROGRAMLEVEL\");",
                "					if (dsLpm != null)",
                "					{",
                "						if (dsLpm.Tables.Count > 0)",
                "							if (dsLpm.Tables[0].Rows.Count > 0)",
                "								txtMasswankathonManager.Text = Convert.ToString(dsLpm.Tables[0].Rows[0][\"MasswankathonManager\"]);",
                "					}",
                "				}",
                "			}",
                "			else",
                "			{",
                "				txtArrgofemployee.Text = string.Empty;",
                "				txtProjectedSplatters.Text = string.Empty;",
                "				txtEstimatedLensToplessionRate.Text = hdnEstimatePartPercent.Value;",
                "				lblNeedleContactName.Visible = false;",
                "				txtNeedleContactName.Visible = false;",
                "				lblPhoneArrg.Visible = false;",
                "				txtPhoneArrg.Visible = false;",
                "				//change request added by madhuri",
                "				lblemail.Visible = false;",
                "				txtemailaddr.Visible = false;",
                "				//end",
                "				lblMasswankathonManager.Visible = false;",
                "				txtMasswankathonManager.Visible = false;",
                "			}",
                "		}	",
                "	}",
                "}"
            };
        
        [TestMethod]
        public void TestRemoveLineComments()
        {
            var testInput01 = @"
        FileUploadApex objFileUploadDownstream = new FileUploadDownstream();

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
                System.Diagnostics.Debug.WriteLine(string.Format("[{0}]", line));
            }

        }

        [TestMethod]
        public void TestRemoveLineCommentsWithEmbeddedUri()
        {
            var testInput = new[]
            {
                "		",
                "		",
                "       ///<summary>",
                "       /// Here is something we must use",
                "       ///</summary>",
                "       /// <param name='reportId'></param> ",
                "		[System.Web.Services.Protocols.SoapDocumentMethodAttribute(\"http://tempuri.org/1stMethod\")]",
                "		[return: System.Xml.Serialization.XmlElementAttribute(\"1stMethodResult\")]",
                "		protected void ddlScreeningLocation_SelectedIndexChanged(object sender, EventArgs e)",
                "		{",
                "			hdnTestIDs.Value = string.Empty;",
                "			hdnVenomIDs.Value = string.Empty;",
                "			hdnSelectCount.Value = \"0\";",
                "			GetYoMommaTypes();",
                "		}",
            };
            var testResult = NoFuture.Gen.Settings.LangStyle.RemoveLineComments(testInput,
                NoFuture.Gen.Settings.LangStyle.LineComment);

            foreach(var ln in testResult)
                System.Diagnostics.Debug.WriteLine(ln);
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

            var testResult = NoFuture.Gen.Settings.LangStyle.IsOddNumberEnclosures(inputString);
            Assert.IsTrue(testResult);

        }

        [TestMethod]
        public void TestRemoveBlockComments()
        {
            var testInput = new[] { " int myInt; /*block comments*/" };
            var testOutput = NoFuture.Gen.Settings.LangStyle.RemoveBlockComments(testInput);
            System.Diagnostics.Debug.WriteLine(string.Format("[{0}]", testOutput));
            Assert.AreEqual(" int myInt;                   ", testOutput[0]);
            testInput = new[] { " int myInt; /*block comments*/ foreach(var s in t){;}" };
            testOutput = NoFuture.Gen.Settings.LangStyle.RemoveBlockComments(testInput);
            Assert.AreEqual(" int myInt;                    foreach(var s in t){;}", testOutput[0]);

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
            Assert.AreEqual(" int myInt;   ", testOutput[0]);
            Assert.AreEqual("                 ", testOutput[1]);
            Assert.AreEqual(" int thirdInt;", testOutput[2]);

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
        public void TestCleanupPdbLinesCodeBlock02()
        {
            var testInput = new[]
            {
                "          {",
                "              connection = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings[\"SirWanksAlot\"].ConnectionString));",
                "              command = new SqlCommand();",
                "              command.Connection = connection;",
                "              command.CommandTimeout = 800;",
                "              command.CommandText = \"select dbo.fnGetoeRRwanker(@Id,@Type)\";",
                "              command.CommandType = CommandType.Text;",
                "              command.Parameters.Add(new SqlParameter(\"@Id\", id));",
                "              command.Parameters.Add(new SqlParameter(\"@Type\", type));",
                "",
                "              if (command.Connection.State == ConnectionState.Closed) command.Connection.Open();",
                "              var result = command.ExecuteScalar(); // should properly get your value",
                "              return (String)result;",
                "          }",
                "          catch",
                "          {",
                "              return null;",
                "          }",
                "",
                "      }"
            };


            var testResult = NoFuture.Gen.Settings.LangStyle.CleanupPdbLinesCodeBlock(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var ln in testResult)
            {
                System.Diagnostics.Debug.WriteLine(ln);
            }            
        }

        [TestMethod]
        public void TestCleanupPdbLinesCodeBlock03()
        {
            var testInput = new[]
            {
                "          {",
                "              connection = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings[\"SirWanksAlot\"].ConnectionString));",
                "              command = new SqlCommand();",
                "              command.Connection = connection;",
                "              command.CommandTimeout = 800;",
                "              command.CommandText = \"select dbo.fnGetoeRRwanker(@Id,@Type)\";",
                "              command.CommandType = CommandType.Text;",
                "              command.Parameters.Add(new SqlParameter(\"@Id\", id));",
                "              command.Parameters.Add(new SqlParameter(\"@Type\", type));",
                "",
                "              if (command.Connection.State == ConnectionState.Closed) command.Connection.Open();",
                "              var result = command.ExecuteScalar(); // should properly get your value",
                "              return (String)result;",
                "          }",
                "          catch",
                "          {",
                "              return null;",
                "          }",
                "",
            };


            var testResult = NoFuture.Gen.Settings.LangStyle.CleanupPdbLinesCodeBlock(testInput);
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

            var testResult = NoFuture.Gen.Settings.LangStyle.EnclosuresCount(testInput);
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
                File.ReadAllLines(TestAssembly.UnitTestsRoot + @"\Gen\ATestableCsFileContent.txt");
            var testTypeName = "PlummetToxic.DPP.Onss.ViewWankathon";
            int testResultOut;
            var testResult = NoFuture.Gen.Settings.LangStyle.TryFindFirstLineInClass(testTypeName, testFileContent,
                out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreNotEqual(0, testResultOut);
            System.Diagnostics.Debug.WriteLine(testResultOut);
        }

        [TestMethod]
        public void TestTryFindFirstLineInClass_ss()
        {
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
                File.ReadAllLines(TestAssembly.UnitTestsRoot + @"\Gen\ATestableCsFileContent.txt");
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
        public void TestEscStringLiterals()
        {
            var irregular = false;
            var input = "        revRoom.ValidationExpression = \"^[a-zA-Z0-9''\" + Convert.ToString(ConfigurationManager.AppSettings[\"PreyNameAllowedCharecters\"]) + \"]+$\";";
            var testResult = NoFuture.Gen.Settings.LangStyle.EscStringLiterals(input, EscapeStringType.UNICODE, ref irregular);
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
            testResult = NoFuture.Gen.Settings.LangStyle.EscStringLiterals(testInput.ToString(), EscapeStringType.UNICODE, ref irregular);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);

            testResult = NoFuture.Gen.Settings.LangStyle.EscStringLiterals(testInput.ToString(),
                EscapeStringType.BLANK, ref irregular);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult);

        }

        [TestMethod]
        public void TestEscStringLiteralsHereString()
        {
            var testInput = "var myJsScript = @\"";
            var testIrregular = false;
            var testOutput = NoFuture.Gen.Settings.LangStyle.EscStringLiterals(testInput.ToString(),
                EscapeStringType.BLANK, ref testIrregular);
            Assert.IsTrue(testIrregular);
            testInput = " }\");";
            testIrregular = true;
            testOutput = NoFuture.Gen.Settings.LangStyle.EscStringLiterals(testInput.ToString(),
                EscapeStringType.BLANK, ref testIrregular);

            Assert.IsFalse(testIrregular);

            var testExampleHereString = new[]
            {
                "                ddlTabtlLocation.Attributes.Add(\"onclick\", @\"",
                "    if (selectedtext == 'No Matching Cities') {",
                "        if ($get(zipTextboxId)) {",
                "            $get(zipTextboxId).value = source._currentPrefix;",
                "            if ($get(validHiddenId)) {",
                "                $get(validHiddenId).value = '0';",
                "            }",
                "        }",
                "        return;",
                "    }\");"
            };

            testIrregular = false;
            var testExampleOut = new List<string>();
            foreach (var ln in testExampleHereString)
            {
                testExampleOut.Add(Settings.LangStyle.EscStringLiterals(ln,
                EscapeStringType.BLANK, ref testIrregular));
            }

            for (var i = 0; i < testExampleHereString.Length; i++)
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("'{0}'",testExampleOut[i]));
                Assert.AreEqual(testExampleHereString[i].Length, testExampleOut[i].Length);
            }
            testIrregular = false;
            testExampleHereString = new[]
            {
                "		",
                "		[System.Web.Services.Protocols.SoapDocumentMethodAttribute(\"http://tempuri.org/1stMethod\", RequestElementName = \"1stMethod\", RequestNamespace = \"http://tempuri.org/\", ResponseElementName = \"1stMethodResponse\", ResponseNamespace = \"http://tempuri.org/\", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]",
                "		[return: System.Xml.Serialization.XmlElementAttribute(\"1stMethodResult\")]protected void ddlScreeningLocation_SelectedIndexChanged(object sender, EventArgs e)",
                "		{",
                "			hdnTestIDs.Value = string.Empty;",
                "			hdnVenomIDs.Value = string.Empty;",
                "			hdnSelectCount.Value = \"0\";",
                "			GetYoMommaTypes();",
                "		}"
            };
            testExampleOut.Clear();
            foreach (var ln in testExampleHereString)
            {
                testExampleOut.Add(Settings.LangStyle.EscStringLiterals(ln,
                EscapeStringType.BLANK, ref testIrregular));
            }
            for (var i = 0; i < testExampleHereString.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("'{0}'",testExampleOut[i]));
            }

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

            input = new string(new[] {'@', '"', '\\', '"'});

            input = string.Format("      string myString = {0} + files[x-2] + {0} + files[x-1]", input);

            testResult = Settings.LangStyle.ExtractAllStringLiterals(input);
            foreach (var k in testResult.Keys)
                 Assert.AreEqual("\\", testResult[k].ToString());

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

        [TestMethod]
        public void TestTransformClrTypeSyntax()
        {
            var testInput = "System.Collections.Generic.List`1[SomeSecondDll.MyFirstMiddleClass]";
            var testResult = Settings.LangStyle.TransformClrTypeSyntax(testInput);
            Assert.IsNotNull(testResult);
            
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(Settings.DefaultLang == CgLangs.Cs);
            Assert.AreEqual("System.Collections.Generic.List<SomeSecondDll.MyFirstMiddleClass>",testResult);

            testInput = "MyDomain.SomeWhere.LowerLib.IRepository`1";
            testResult = Settings.LangStyle.TransformClrTypeSyntax(testInput);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("MyDomain.SomeWhere.LowerLib.IRepository<>", testResult);

            testInput = "MyDomain.SomeWhere.LowerLib.IRepository`2";
            testResult = Settings.LangStyle.TransformClrTypeSyntax(testInput);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("MyDomain.SomeWhere.LowerLib.IRepository<,>", testResult);
        }

        [TestMethod]
        public void TestTransformClrTypeSyntax_HyperComplex()
        {
            var testInput =
                "NeedItInIl.DomainAdapterBase`2[" +
                             "[AThirdDll.Whatever, AThirdDll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]," +
                             "[System.Tuple`3[" +
                                   "[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]," +
                                   "[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]," +
                                   "[System.Collections.Generic.IEnumerable`1[" +
                                          "[MoreBinaries.DomainObject+MyInnerType, MoreBinaries, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]" +
                                   "], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]" +
                             "], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]" +
                                             "]";
            Assert.IsTrue(Settings.DefaultLang == CgLangs.Cs);
            var testResult = NoFuture.Gen.Settings.LangStyle.TransformClrTypeSyntax(testInput);
            Assert.AreEqual("NeedItInIl.DomainAdapterBase<AThirdDll.Whatever,System.Tuple<string,int,System.Collections.Generic.IEnumerable<MoreBinaries.DomainObject.MyInnerType>>>", testResult);
        }

        [TestMethod]
        public void TestToSignatureRegex()
        {
            var testCgMember = new CgMember() {Name = "PopulateDdlForSomething"};
            testCgMember.Args.Add(new CgArg(){ArgName = "ddl", ArgType = "DropDownList"});
            testCgMember.Args.Add(new CgArg() { ArgName = "cname", ArgType = "string" });
            testCgMember.IsMethod = true;

            var testInput = Settings.LangStyle.ToSignatureRegex(testCgMember);
            Assert.IsNotNull(testInput);

            Assert.IsTrue(Regex.IsMatch("        public void PopulateDdlForSomething(DropDownList ddl, string strAccountId)", testInput, RegexOptions.IgnoreCase));

            testCgMember = new CgMember() { Name = "GetKeeperMasterDetailsForBizIDsForEmail" };
            testCgMember.Args.Add(new CgArg() { ArgName = "strClinicIDs", ArgType = "string" });
            testCgMember.IsMethod = true;

            testInput = Settings.LangStyle.ToSignatureRegex(testCgMember);
            Assert.IsNotNull(testInput);

            Assert.IsTrue(Regex.IsMatch("        public List<KeeperMaster> GetKeeperMasterDetailsForBizIDsForEmail(string strClinicIDs)", testInput, RegexOptions.IgnoreCase));

            testCgMember = new CgMember() { Name = "AProperty" };
            testCgMember.IsMethod = false;
            testCgMember.HasGetter = true;
            testCgMember.HasGetter = true;
            testCgMember.AccessModifier  = CgAccessModifier.Family;

            testInput = Settings.LangStyle.ToSignatureRegex(testCgMember);
            Assert.IsNotNull(testInput);
            System.Diagnostics.Debug.WriteLine(testInput);

            Assert.IsTrue(Regex.IsMatch("             protected string AProperty",testInput));
        }

        [TestMethod]
        public void TestGetDecoratorRegex()
        {
            //var testCsRegex00 =
            //    "[System.Web.Services.Protocols.SoapDocumentMethodAttribute(\"http://tempuri.org/1stMethod\", RequestElementName = \"1stMethod\", RequestNamespace = \"http://tempuri.org/\", ResponseElementName = \"1stMethodResponse\", ResponseNamespace = \"http://tempuri.org/\", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]";
            //var testCsRegex01 = "[return: System.Xml.Serialization.XmlElementAttribute(\"1stMethodResult\")]";
            /*
                   [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/1stMethod", RequestElementName = "1stMethod", RequestNamespace = "http://tempuri.org/", ResponseElementName = "1stMethodResponse", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
      [return: System.Xml.Serialization.XmlElementAttribute(\"1stMethodResult\")]

             */
        }
    }
}
