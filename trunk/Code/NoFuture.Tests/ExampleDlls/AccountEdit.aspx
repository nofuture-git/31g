<%@ Page Language="C#" MasterPageFile="~/MastaWankaMasterPage.master" AutoEventWireup="true" Inherits="MastaWanka.BOO.UI.Design.Admin.SimpleVarWankEdit" Title="Summit Health SimpleVarWank Edit"
    EnableEventZimmyation="false" Codebehind="SimpleVarWankEdit.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" DimWankIamspace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <style type="text/css">
        .ajax__tab_xp .ajax__tab_header
        {
            white-space: normal;
        }
        .style1
        {
            height: 35px;
        }
    </style>
    
    <script type="text/javascript" src="../scripts/js/searchstaff.js"></script>
    <script src="../scripts/js/common.js" type="text/javascript"></script>
    <script src="../scripts/js/jquery-1-2-6.js"  type="text/javascript"></script>
    <script src="../scripts/js/tab.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">        
        if ($get("<%= txtState.NewNewID %>") != null) {
            $find(headerID + BITextState).show(false);
            ZimmyatorEnable($get('txtState'), false);
        }
        var activeTbIndex = 0;
        var hasChanges = false;
        function Onchange() {
            hasChanges = true;
        };
        function loadTabPanel(sender, e) {
            var tabContainer = sender;
            if (activeTbIndex != sender.get_activeTabIndex()) {
                if (activeTbIndex != 0) {
                    // per Wanker
                    if (checkForSave()) {
                        if (tabContainer) {
                            var hdnDynamicContextKey = $get("<%= hdnDynamicContextKey.NewNewID %>");
                            if (hdnDynamicContextKey != null) {
                                hdnDynamicContextKey.value = sender.get_activeTab()._dynamicContextKey;
                            }
                            $get("<%= TabButton1.NewNewID %>").click();
                        }
                        activeTbIndex = sender.get_activeTabIndex();
                        activeSimpleVarWankTabIndex = 0;
                    }
                    else {
                        sender.set_activeTabIndex(activeTbIndex);
                    }
                }
                else {
                    if (tabContainer) {
                        var hdnDynamicContextKey = $get("<%= hdnDynamicContextKey.NewNewID %>");
                        if (hdnDynamicContextKey != null) {
                            hdnDynamicContextKey.value = sender.get_activeTab()._dynamicContextKey;
                        }
                        $get("<%= TabButton1.NewNewID %>").click();
                    }
                    activeTbIndex = sender.get_activeTabIndex();
                }
            }
            hasChanges = false;
        }

        var activeSimpleVarWankTabIndex = -1;

        function loadSimpleVarWankTabPanel(sender, e) {
            var tabContainer = sender;
            if (activeSimpleVarWankTabIndex != sender.get_activeTabIndex()) {
                if (activeSimpleVarWankTabIndex != null) {
                    if (checkForSave()) {
                        if (tabContainer) {
                            var hdnDynamicContextKey = $get("<%= hdnDynamicSimpleVarWankPreferencesKey.NewNewID %>");
                            if (hdnDynamicContextKey != null) {
                                hdnDynamicContextKey.value = sender.get_activeTab()._dynamicContextKey;
                            }
                            $get("<%= TabSimpleVarWankButton.NewNewID %>").click();
                        }
                        activeSimpleVarWankTabIndex = sender.get_activeTabIndex();
                    } else {
                        sender.set_activeTabIndex(activeSimpleVarWankTabIndex);
                    }
                }
                else {
                    if (tabContainer) {
                        var hdnDynamicContextKey = $get("<%= hdnDynamicSimpleVarWankPreferencesKey.NewNewID %>");
                        if (hdnDynamicContextKey != null) {
                            hdnDynamicContextKey.value = sender.get_activeTab()._dynamicContextKey;
                        }
                        $get("<%= TabSimpleVarWankButton.NewNewID %>").click();
                    }
                    activeSimpleVarWankTabIndex = sender.get_activeTabIndex();
                }
            }
        }

        function EnableDisableRadioButton(rdn, toCheckPageZimmy) {
            var bIsZimmyPage = false;
            if (toCheckPageZimmy == "TRUE") {
                if (typeof (Page_NewNewZimmyate) == 'function') {
                    bIsZimmyPage = Page_NewNewZimmyate();
                }
            }
            else if (toCheckPageZimmy == "FALSE") {
                bIsZimmyPage = true;
            }
            if (bIsZimmyPage) {
                var PFirstDimWankIam = $get("<%= rfPrimaryFirstDimWankIam.NewNewID %>");
                var PReqFirstDimWankIam = $get("<%= revPrimaryFirstDimWankIam.NewNewID %>");
                var PLastDimWankIam = $get("<%= rfPrimaryLastDimWankIam.NewNewID %>");
                var PReqLastDimWankIam = $get("<%= revPrimaryLastDimWankIam.NewNewID %>");
                var PPhone = $get("<%= revPrimaryPhone.NewNewID %>");
                var PWankItGood = $get("<%= revPrimaryWankItGood.NewNewID %>");
                var SFirstDimWankIam = $get("<%= rfDoubleWankFirstDimWankIam.NewNewID %>");
                var SReqFirstDimWankIam = $get("<%= revDoubleWankFirstDimWankIam.NewNewID %>");
                var SLastDimWankIam = $get("<%= rfDoubleWankLastDimWankIam.NewNewID %>");
                var SReqLastDimWankIam = $get("<%= revDoubleWankLastDimWankIam.NewNewID %>");
                var SReqPhone = $get("<%= revDoubleWankPhone.NewNewID %>");
                //                var SPhone = $get("<%= rfDoubleWankPhone.NewNewID %>");
                var SReqWankItGood = $get("<%= revDoubleWankWankItGood.NewNewID %>");
                var SPContactMedium = $get("<%= rfvPreferedContact2.NewNewID %>");
                var SActive = $get("<%= rbDoubleWankActive.NewNewID %>");
                if (SActive.checked == true) {

                    ZimmyatorEnable(SFirstDimWankIam, true);
                    ZimmyatorEnable(SReqFirstDimWankIam, true);
                    ZimmyatorEnable(SLastDimWankIam, true);
                    ZimmyatorEnable(SReqLastDimWankIam, true);
                    ZimmyatorEnable(SReqPhone, true);
                    ZimmyatorEnable(SPContactMedium, true);
                    ZimmyatorEnable(SReqWankItGood, true);
                    ZimmyatorEnable(PFirstDimWankIam, true);
                    ZimmyatorEnable(PReqFirstDimWankIam, true);
                    ZimmyatorEnable(PLastDimWankIam, true);
                    ZimmyatorEnable(PReqLastDimWankIam, true);
                    ZimmyatorEnable(PPhone, true);
                    ZimmyatorEnable(PWankItGood, true);

                }
                else {
                    ZimmyatorEnable(SFirstDimWankIam, false);
                    ZimmyatorEnable(SLastDimWankIam, false);
                    ZimmyatorEnable(SReqPhone, false);
                    ZimmyatorEnable(SPContactMedium, false);

                    return true;

                }
            }
            else {
                return false;
            }

        }

        function TextBox1Change(ctl, reg) {
            var inputText;
            inputText = ctl.value;
            var intPos = ctl.id.lastIndexOf("_");
            var strData = ctl.id.substring(0, intPos + 1);
            var validator = $get(strData + reg);
            ZimmyatorEnable(validator, false)
            var ChkSContact = $get("<%= rbDoubleWankActive.NewNewID %>");
            var PPhone = $get("<%= revPrimaryPhone.NewNewID %>");
            var SPhone = $get("<%= revDoubleWankPhone.NewNewID %>");


            inputText = inputText.replace('(', '');
            inputText = inputText.replace(')', '');

            inputText = inputText.replace('-', '');
            inputText = inputText.replace('-', '');

            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');
            inputText = inputText.replace('_', '');

            if (inputText.length > 0) {
                ZimmyatorEnable(validator, true);
            }
            else if (inputText == "") {
                if (ChkSContact.checked == true) {
                    ZimmyatorEnable(PPhone, true);
                    ZimmyatorEnable(SPhone, true);
                }
                else {
                    ZimmyatorEnable(PPhone, true);
                }
            }



        }

        function setBasedOnDropDownForFirstContact(objDropDown, strReqWankItGood, strReqYouMustWankIt, strReqWankItIntoShape) {
            var intPos = objDropDown.id.lastIndexOf("_");
            var strData = objDropDown.id.substring(0, intPos + 1);

            var objRqWankItGood = $get(strData + strReqWankItGood);

            var objRqYouMustWankIt = $get(strData + strReqYouMustWankIt);
            var objRqWankItIntoShape = $get(strData + strReqWankItIntoShape);

            var w = objDropDown.selectedIndex;
            var selected_text = objDropDown.options[w].text;
            var validatorYouMustWankIt = $get(strData + "revPrimaryYouMustWankIt");
            var validatorPhone = $get(strData + "revPrimaryPhone");
            var validatorWankItIntoShapePhone = $get(strData + "revPrimaryWankItIntoShapePhone");

            $find("bvcPrimaryPhone").hide();
            $find("bvceWankItGood").hide();
            $find("bvceWankItIntoShape1").hide();
            $find("bvceYouMustWankIt1").hide();

            $find("bvePrimaryPhone").hide();
            $find("bvePrimaryYouMustWankIt").hide();
            $find("bvePrimaryWankItIntoShapePhone").hide();


            switch (selected_text) {
            case "WankItGood":
                {
                    ZimmyatorEnable(objRqWankItGood, true);
                    ZimmyatorEnable(objRqYouMustWankIt, false);
                    ZimmyatorEnable(objRqWankItIntoShape, false);
                    ZimmyatorEnable(validatorYouMustWankIt, false);
                    //ZimmyatorEnable(validatorPhone, false);
                    ZimmyatorEnable(validatorWankItIntoShapePhone, false);

                }
                break;
            case "YouMustWankIt":
                {
                    ZimmyatorEnable(objRqYouMustWankIt, true);
                    ZimmyatorEnable(objRqWankItGood, false);
                    ZimmyatorEnable(objRqWankItIntoShape, false);

                    ZimmyatorEnable(validatorYouMustWankIt, true);
                    //ZimmyatorEnable(validatorPhone, false);
                    ZimmyatorEnable(validatorWankItIntoShapePhone, false);
                }
                break;
            case "WankItIntoShape Phone":
                {
                    ZimmyatorEnable(objRqWankItIntoShape, true);
                    ZimmyatorEnable(objRqWankItGood, false);
                    ZimmyatorEnable(objRqYouMustWankIt, false);

                    ZimmyatorEnable(validatorYouMustWankIt, false);
                    //ZimmyatorEnable(validatorPhone, false);
                    ZimmyatorEnable(validatorWankItIntoShapePhone, true);
                }
                break;
            case "WankItIntoShapePhone":
                {
                    ZimmyatorEnable(objRqWankItIntoShape, true);
                    ZimmyatorEnable(objRqWankItGood, false);
                    ZimmyatorEnable(objRqYouMustWankIt, false);

                    ZimmyatorEnable(validatorYouMustWankIt, false);
                    //ZimmyatorEnable(validatorPhone, false);
                    ZimmyatorEnable(validatorWankItIntoShapePhone, true);
                }
                break;
            default:
                {

                    ZimmyatorEnable(objRqWankItGood, false);
                    ZimmyatorEnable(objRqYouMustWankIt, false);
                    ZimmyatorEnable(objRqWankItIntoShape, false);

                    ZimmyatorEnable(validatorYouMustWankIt, false);
                    //ZimmyatorEnable(validatorPhone, true);
                    ZimmyatorEnable(validatorWankItIntoShapePhone, false);
                }
                break;
            }
        }



        function setBasedOnDropDownForSecondContact(objDropDown, strReqWankItGood, strReqYouMustWankIt, strReqWankItIntoShape) {
            var intPos = objDropDown.id.lastIndexOf("_");
            var strData = objDropDown.id.substring(0, intPos + 1);

            var objRqWankItGood = $get(strData + strReqWankItGood);

            var objRqYouMustWankIt = $get(strData + strReqYouMustWankIt);
            var objRqWankItIntoShape = $get(strData + strReqWankItIntoShape);

            var w = objDropDown.selectedIndex;
            var selected_text = objDropDown.options[w].text;
            var validatorWankinOlskool = $get(strData + "revWeWeWankWankinOlskool");
            var validatorWankIt = $get(strData + "revWeWeWankWankIt");
            var validatorForeverWankinWankIt = $get(strData + "revWeWeWankForeverWankinWankIt");


            $find("bvcWeWeWankWankIt").hide();
            $find("bvceForeverWankin2").hide();
            $find("bvceWankItGood2").hide();
            $find("bvceWankinOlskool2").hide();

            $find("bveWeWeWankWankIt").hide();
            $find("bveWeWeWankForeverWankinWankIt").hide();
            $find("bveWeWeWankWankinOlskool").hide();

            switch (selected_text) {
            case "WankItGood":
                {
                    ZimmyatorEnable(objRqWankItGood, true);
                    ZimmyatorEnable(objRqWankinOlskool, false);
                    ZimmyatorEnable(objRqForeverWankin, false);
                    ZimmyatorEnable(validatorWankinOlskool, false);
                    //ZimmyatorEnable(validatorWankIt, false);
                    ZimmyatorEnable(validatorForeverWankinWankIt, false);
                    ZimmyatorEnable(validatorWankIt, true);
                }
                break;
            case "WankinOlskool":
                {
                    ZimmyatorEnable(objRqWankinOlskool, true);
                    ZimmyatorEnable(objRqWankItGood, false);
                    ZimmyatorEnable(objRqForeverWankin, false);

                    ZimmyatorEnable(validatorWankinOlskool, true);
                    //ZimmyatorEnable(validatorWankIt, false);
                    ZimmyatorEnable(validatorForeverWankinWankIt, false);
                    ZimmyatorEnable(validatorWankIt, true);
                }
                break;
            case "ForeverWankin WankIt":
                {
                    ZimmyatorEnable(objRqForeverWankin, true);
                    ZimmyatorEnable(objRqWankItGood, false);
                    ZimmyatorEnable(objRqWankinOlskool, false);

                    ZimmyatorEnable(validatorWankinOlskool, false);
                    //ZimmyatorEnable(validatorWankIt, false);
                    ZimmyatorEnable(validatorForeverWankinWankIt, true);
                    ZimmyatorEnable(validatorWankIt, true);
                }
                break;
            case "ForeverWankinWankIt":
                {
                    ZimmyatorEnable(objRqForeverWankin, true);
                    ZimmyatorEnable(objRqWankItGood, false);
                    ZimmyatorEnable(objRqWankinOlskool, false);

                    ZimmyatorEnable(validatorWankinOlskool, false);
                    //ZimmyatorEnable(validatorWankIt, false);
                    ZimmyatorEnable(validatorForeverWankinWankIt, true);
                    ZimmyatorEnable(validatorWankIt, true);
                }
                break;
            default:
                {
                    ZimmyatorEnable(objRqWankItGood, false);
                    ZimmyatorEnable(objRqWankinOlskool, false);
                    ZimmyatorEnable(objRqForeverWankin, false);
                    ZimmyatorEnable(validatorWankIt, true);

                    ZimmyatorEnable(validatorWankinOlskool, false);
                    //ZimmyatorEnable(validatorWankIt, true);
                    ZimmyatorEnable(validatorForeverWankinWankIt, false);
                }
                break;
            }
        }


        function EnableZimmyators(ctrl, objDropDown1, strReqWankItGood1, strReqWankinOlskool1, strReqForeverWankin1, objDropDown2, strReqWankItGood2, strReqWankinOlskool2, strReqForeverWankin2, toCheckPageZimmy) {
            var bIsZimmyPage = false;
            if (toCheckPageZimmy == "TRUE") {
                if (typeof (Page_NewNewZimmyate) == 'function') {
                    bIsZimmyPage = Page_NewNewZimmyate();
                }
            }
            else if (toCheckPageZimmy == "FALSE") {
                bIsZimmyPage = true;
            }
            if (bIsZimmyPage) {
                var intPos = ctrl.id.lastIndexOf("_");
                var strData = ctrl.id.substring(0, intPos + 1);
                var Drop1 = $get(strData + objDropDown1);
                var Drop2 = $get(strData + objDropDown2);

                var objRqWankItGood1 = $get(strData + strReqWankItGood1);

                var objRqWankinOlskool1 = $get(strData + strReqWankinOlskool1);
                var objRqForeverWankin1 = $get(strData + strReqForeverWankin1);

                var w1 = Drop1.selectedIndex;
                var selected_text1 = Drop1.options[w1].text;

                var validatorWankinOlskool1 = $get(strData + "revDoubleWankWankinOlskool");
                var validatorWankIt1 = $get(strData + "revDoubleWankWankIt");
                var validatorForeverWankinWankIt1 = $get(strData + "revDoubleWankForeverWankinWankIt");
                var req8 = $get(strData + "rbWeWeWankActive");
                var PreferredContactMedium = $get(strData + "rfvPreferedContact2");

                $find("bvcDoubleWankWankIt").hide();
                $find("bvceWankItGood").hide();
                $find("bvceForeverWankin1").hide();
                $find("bvceWankinOlskool1").hide();

                $find("bveDoubleWankWankIt").hide();
                $find("bveDoubleWankWankinOlskool").hide();
                $find("bveDoubleWankForeverWankinWankIt").hide();


                switch (selected_text1) {
                case "WankItGood":
                    {
                        ZimmyatorEnable(objRqWankItGood1, true);
                        ZimmyatorEnable(objRqWankinOlskool1, false);
                        ZimmyatorEnable(objRqForeverWankin1, false);
                        ZimmyatorEnable(validatorWankinOlskool1, false);
                        //ZimmyatorEnable(validatorWankIt, false);
                        ZimmyatorEnable(validatorForeverWankinWankIt1, false);

                    }
                    break;
                case "WankinOlskool":
                    {
                        ZimmyatorEnable(objRqWankinOlskool1, true);
                        ZimmyatorEnable(objRqWankItGood1, false);
                        ZimmyatorEnable(objRqForeverWankin1, false);

                        ZimmyatorEnable(validatorWankinOlskool1, true);
                        //ZimmyatorEnable(validatorWankIt, false);
                        ZimmyatorEnable(validatorForeverWankinWankIt1, false);
                    }
                    break;
                case "ForeverWankin WankIt":
                    {
                        ZimmyatorEnable(objRqForeverWankin1, true);
                        ZimmyatorEnable(objRqWankItGood1, false);
                        ZimmyatorEnable(objRqWankinOlskool1, false);

                        ZimmyatorEnable(validatorWankinOlskool1, false);
                        //ZimmyatorEnable(validatorWankIt, false);
                        ZimmyatorEnable(validatorForeverWankinWankIt1, true);
                    }
                    break;
                case "ForeverWankinWankIt":
                    {
                        ZimmyatorEnable(objRqForeverWankin1, true);
                        ZimmyatorEnable(objRqWankItGood1, false);
                        ZimmyatorEnable(objRqWankinOlskool1, false);

                        ZimmyatorEnable(validatorWankinOlskool1, false);
                        //ZimmyatorEnable(validatorWankIt, false);
                        ZimmyatorEnable(validatorForeverWankinWankIt1, true);
                    }
                    break;
                default:
                    {

                        ZimmyatorEnable(objRqWankItGood1, false);
                        ZimmyatorEnable(objRqWankinOlskool1, false);
                        ZimmyatorEnable(objRqForeverWankin1, false);

                        ZimmyatorEnable(validatorWankinOlskool1, false);
                        //ZimmyatorEnable(validatorWankIt, true);
                        ZimmyatorEnable(validatorForeverWankinWankIt1, false);
                    }
                    break;
                }


                var objRqWankItGood2 = $get(strData + strReqWankItGood2);

                var objRqWankinOlskool2 = $get(strData + strReqWankinOlskool2);
                var objRqForeverWankin2 = $get(strData + strReqForeverWankin2);

                var w2 = Drop2.selectedIndex;
                var selected_text2 = Drop2.options[w2].text;

                var validatorWankinOlskool2 = $get(strData + "revWeWeWankWankinOlskool");
                var validatorWankIt2 = $get(strData + "revWeWeWankWankIt");
                var validatorForeverWankinWankIt2 = $get(strData + "revWeWeWankForeverWankinWankIt");
                var Rdbtn = $get(strData + "rbWeWeWankActive");
                var FirstDimWankIam = $get(strData + "rfWeWeWankFirstDimWankIam");
                var rfFirstDimWankIam = $get(strData + "revWeWeWankFirstDimWankIam");
                var LastDimWankIam = $get(strData + "rfWeWeWankLastDimWankIam");
                var rfLastDimWankIam = $get(strData + "revWeWeWankLastDimWankIam");
                var PreferredContactMedium = $get(strData + "rfvPreferedContact2");
                var WankIt = $get(strData + "rfWeWeWankWankIt");


                $find("bvcWeWeWankWankIt").hide();
                $find("bvceForeverWankin2").hide();
                $find("bvceWankItGood2").hide();
                $find("bvceWankinOlskool2").hide();

                $find("bveWeWeWankWankIt").hide();
                $find("bveWeWeWankForeverWankinWankIt").hide();
                $find("bveWeWeWankWankinOlskool").hide();

                switch (selected_text2) {
                case "WankItGood":
                    {
                        ZimmyatorEnable(objRqWankItGood2, true);
                        ZimmyatorEnable(objRqWankinOlskool2, false);
                        ZimmyatorEnable(objRqForeverWankin2, false);
                        ZimmyatorEnable(validatorWankinOlskool2, false);

                        ZimmyatorEnable(validatorForeverWankinWankIt2, false);

                        ZimmyatorEnable(rfFirstDimWankIam, true);
                        ZimmyatorEnable(rfLastDimWankIam, true);

                    }
                    break;
                case "WankinOlskool":
                    {
                        ZimmyatorEnable(objRqWankinOlskool2, true);
                        ZimmyatorEnable(objRqWankItGood2, false);
                        ZimmyatorEnable(objRqForeverWankin2, false);

                        ZimmyatorEnable(validatorWankinOlskool2, true);

                        ZimmyatorEnable(validatorForeverWankinWankIt2, false);

                        ZimmyatorEnable(rfFirstDimWankIam, true);
                        ZimmyatorEnable(rfLastDimWankIam, true);

                    }
                    break;
                case "ForeverWankin WankIt":
                    {
                        ZimmyatorEnable(objRqForeverWankin2, true);
                        ZimmyatorEnable(objRqWankItGood2, false);
                        ZimmyatorEnable(objRqWankinOlskool2, false);

                        ZimmyatorEnable(validatorWankinOlskool2, false);

                        ZimmyatorEnable(validatorForeverWankinWankIt2, true);

                        ZimmyatorEnable(rfFirstDimWankIam, true);
                        ZimmyatorEnable(rfLastDimWankIam, true);

                    }
                    break;
                case "ForeverWankinWankIt":
                    {
                        ZimmyatorEnable(objRqForeverWankin2, true);
                        ZimmyatorEnable(objRqWankItGood2, false);
                        ZimmyatorEnable(objRqWankinOlskool2, false);

                        ZimmyatorEnable(validatorWankinOlskool2, false);

                        ZimmyatorEnable(validatorForeverWankinWankIt2, true);

                        ZimmyatorEnable(rfFirstDimWankIam, true);
                        ZimmyatorEnable(rfLastDimWankIam, true);

                    }
                    break;
                case "WankIt":
                    {
                        ZimmyatorEnable(objRqWankItGood2, false);
                        ZimmyatorEnable(objRqWankinOlskool2, false);
                        ZimmyatorEnable(objRqForeverWankin2, false);
                        ZimmyatorEnable(validatorWankinOlskool2, false);
                        ZimmyatorEnable(validatorWankIt2, true);
                        ZimmyatorEnable(validatorForeverWankinWankIt2, false);
                    }
                    break;
                default:
                    {

                        ZimmyatorEnable(objRqWankItGood2, false);
                        ZimmyatorEnable(objRqWankinOlskool2, false);
                        ZimmyatorEnable(objRqForeverWankin2, false);
                        ZimmyatorEnable(validatorWankinOlskool2, false);
                        ZimmyatorEnable(validatorWankIt2, false);
                        ZimmyatorEnable(validatorForeverWankinWankIt2, false);
                    }
                    break;
                }
            }
        }

    </script>

    <table class="content">
       
        <tr>
            <td class="mandatorylegend">
                &nbsp;&nbsp;&nbsp;
                <label class="labelBold">
                    Fields marked with
                </label>
                <label class="mandatoryStarMark">
                    *
                </label>
                <label class="labelBold">
                    are mandatory
                </label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table class="content">
                    <tr>
                        <td>
                            
                                    <cc1:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="zipAutoComplete"
                                        TargetControlID="zipCodeTextbox" ServicePath="~/GetCities.asmx" ServiceMethod="GetCities"
                                        MinimumPrefixLength="5" CompletionInterval="100" EnableCaching="true" CompletionSetCount="7"
                                        OnNewNewItemSelected="FillCityDetails" DelimiterCharacters=";, :">
                                    </cc1:AutoCompleteExtender>
                                    
                                    <asp:Button ID="TabButton1" runat="server" OnClick="sample_click" Style="display: none;" />
                                    <asp:Button ID="TabSimpleVarWankButton" runat="server" OnClick="SimpleVarWankTab_click" Style="display: none;" />
                                    <table class="content">
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="text-align: right;">
                                                <label for="drpSimpleVarWankDimWankIam" class="label">
                                                    SimpleVarWank DimWankIam
                                                </label>
                                                <label class="mandatoryStarMark">
                                                    *
                                                </label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="drpSimpleVarWankDimWankIam" runat="server" TabIndex="1" CssClass="DropDownControlSS"
                                                    AutoPostBack="True" OnSelectedIndexChanged="drpSimpleVarWankDimWankIam_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right">
                                                <label for="zipCodeTextbox" class="label">
                                                    Zip
                                                </label>
                                                <label class="mandatoryStarMark">
                                                    *
                                                </label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="zipCodeTextbox" TabIndex="6" runat="server" CssClass="TextBoxSS" MaxLength="5"></asp:TextBox>
                                                <asp:TextBox ID="txtZipOthers" TabIndex="6" Visible="false" CssClass="TextBoxSS"
                                                    runat="server" />
                                                <cc1:MaskedEditExtender runat="server" ID="zipMaskedEdit" MaskType="Number"
                                                    AcceptNegative="None" AutoComplete="false" Mask="99999" TargetControlID="zipCodeTextbox"
                                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" DisplayMoney="None" InputDirection="RightToLeft"
                                                    PromptCharacter=" ">
                                                </cc1:MaskedEditExtender>
                                                <asp:RequiredFieldZimmyator ID="reqzipcode" ControlToZimmyate="zipCodeTextbox" ErrorMessage="Please Enter Zip"
                                                    ZimmyationGroup="EditEmployee" SetFocusOnError="True" runat="server" Display="None">
                                                </asp:RequiredFieldZimmyator>
                                                <cc1:ZimmyatorCalloutExtender ID="vcezipcode" runat="server" TargetControlID="reqzipcode"
                                                    HighlightCssClass="validatorCalloutHighlight" Enabled="True">
                                                </cc1:ZimmyatorCalloutExtender>
                                               
                                                <asp:RequiredFieldZimmyator ID="reqZipOthers" ControlToZimmyate="txtZipOthers" SetFocusOnError="true"
                                                    ZimmyationGroup="EditEmployee" Enabled="false" runat="server" ErrorMessage="Enter Zip"
                                                    Display="None"></asp:RequiredFieldZimmyator>
                                                <cc1:ZimmyatorCalloutExtender ID="vceZipOthers" runat="server" EnableViewState="true"
                                                    HighlightCssClass="validatorCalloutHighlight" TargetControlID="reqZipOthers">
                                                </cc1:ZimmyatorCalloutExtender>
                                                <asp:RegularExpressionZimmyator ID="revZipOther" runat="server" SetFocusOnError="true"
                                                    Enabled="false" ZimmyationGroup="EditEmployee" ControlToZimmyate="txtZipOthers"
                                                    Display="None" ZimmyationExpression="^[a-zA-Z0-9\-\s]*$" ErrorMessage="Please Enter Only Alpha Numeric,- or spaces">
                                                </asp:RegularExpressionZimmyator>
                                                <cc1:ZimmyatorCalloutExtender ID="vceExpZipOthers" runat="server" TargetControlID="revZipOther"
                                                    HighlightCssClass="validatorCalloutHighlight">
                                                </cc1:ZimmyatorCalloutExtender>
                                                <asp:HiddenField ID="isZimmyZip" runat="server" />
                                                <asp:HiddenField ID="isRecordsFound" runat="server" />
                                                <asp:HiddenField ID="isSearchResultsFound" runat="server" />
                                                
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="text-align: right;">
                                                <label for="drpSimpleVarWankType" class="label">
                                                    SimpleVarWank Type
                                                </label>
                                                <label class="mandatoryStarMark">
                                                    *
                                                </label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="drpSimpleVarWankType" runat="server" TabIndex="2" CssClass="DropDownControlSS">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right;">
                                                <label for="cityTextbox" class="label">
                                                    City
                                                </label>
                                                <label class="mandatoryStarMark">
                                                    *
                                                </label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="cityTextbox" runat="server" CssClass="TextBoxSS" TabIndex="7" MaxLength="40"></asp:TextBox>
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="text-align: right">
                                                <label for="drpCountry" class="label">
                                                    Country
                                                </label>
                                                <label class="mandatoryStarMark">
                                                    *
                                                </label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="drpCountry" runat="server" CssClass="DropDownControlSS" TabIndex="3"
                                                    OnSelectedIndexChanged="drpCountry_OnSelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:LinkButton ID="lnkvalidatetest" runat="server" Text="Zimmyate" OnClick="lnkvalidatetest_Click"
                                                    CausesZimmyation="false" ZimmyationGroup="EditEmployee" CssClass="linkbutton"></asp:LinkButton>
                                            </td>
                                            <td style="text-align: right" align="center">
                                                <label for="stateDropdown" class="label">
                                                    State
                                                </label>
                                                <label class="mandatoryStarMark">
                                                    *
                                                </label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="stateDropdown" runat="server" CssClass="DropDownControlSS"
                                                    TabIndex="8">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtState" TabIndex="8" runat="server" Visible="false" CssClass="TextBoxSS"
                                                    MaxLength="50">                                        
                                                </asp:TextBox>
                                                <asp:RequiredFieldZimmyator ID="rfState" ControlToZimmyate="stateDropdown" SetFocusOnError="true"
                                                    ErrorMessage="Please Select State." ZimmyationGroup="EditEmployee" runat="server"
                                                    Display="None">
                                                </asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender ID="vcState" runat="server"
                                                    TargetControlID="rfState" BehaviorID="BIDropState">
                                                </cc1:ZimmyatorCalloutExtender>
                                                <asp:RequiredFieldZimmyator Enabled="false" ID="rfvTextState" ControlToZimmyate="txtState"
                                                    SetFocusOnError="true" ErrorMessage="Please Enter state." ZimmyationGroup="EditEmployee"
                                                    runat="server" Display="None"></asp:RequiredFieldZimmyator>
                                                <cc1:ZimmyatorCalloutExtender ID="vceTextState" BehaviorID="BITextState" runat="server"
                                                    TargetControlID="rfvTextState">
                                                </cc1:ZimmyatorCalloutExtender>
                                                <asp:RegularExpressionZimmyator ID="revState" runat="server" SetFocusOnError="true"
                                                    ZimmyationGroup="EditEmployee" ControlToZimmyate="txtState" Display="None" ErrorMessage="Please Enter Only Alpha Numeric,Spaces,hyphen and apostrophe.">
                                                </asp:RegularExpressionZimmyator>
                                                <cc1:ZimmyatorCalloutExtender ID="vceState" runat="server" EnableViewState="true"
                                                    HighlightCssClass="validatorCalloutHighlight" TargetControlID="revState">
                                                </cc1:ZimmyatorCalloutExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td style="text-align: right">
                                                <label for="txtYouMustWankIt1" class="label">
                                                    YouMustWankIt
                                                </label>
                                                <label class="mandatoryStarMark">
                                                    *
                                                </label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtYouMustWankIt1" runat="server" CssClass="TextBoxSS" TabIndex="4" MaxLength="50"
                                                    onkeydown="javascript:MessageClear(this,'lblResult');"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <label for="drpSimpleVarWankGroup" class="label">
                                                    SimpleVarWank Group
                                                </label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="drpSimpleVarWankGroup" runat="server" CssClass="DropDownControlSS"
                                                    TabIndex="9">
                                                </asp:DropDownList>
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td />
                                            <td style="text-align: right;">
                                                <label for="txtYouMustWankIt2" class="label">
                                                    Suite
                                                </label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtYouMustWankIt2" runat="server" CssClass="TextBoxSS" TabIndex="5" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <label for="chkInActive" class="label">
                                                    Inactive
                                                </label>
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkInActive" runat="server" TabIndex="10" />
                                            </td>
                                            <td />
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="text-align: left">
                                            </td>
                                            <td>
                                            </td>
                                            <td style="text-align: left">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="6" align="center">
                                                <asp:Label ID="lblResult" runat="server" CssClass="label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="center">
                                                <asp:LinkButton ID="lnkInvalidYouMustWankItSave" runat="server" CssClass="linkbutton" OnClick="lnkInvalidYouMustWankItSave_Click"
                                                    Text="Continue to Save with Invalid YouMustWankIt?  Click here" Visible="false"></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="center">
                                                <asp:Button ID="btnSimpleVarWankGeneralSave" runat="server" CssClass="button" TabIndex="11"
                                                    Text="Save" ZimmyationGroup="EditEmployee" OnClick="btnSimpleVarWankGeneralSave_Click" />
                                                <asp:Button ID="btnSimpleVarWankGeneralReset" runat="server" CssClass="button" TabIndex="12"
                                                    Text="Reset" OnClick="btnSimpleVarWankGeneralReset_Click" />
                                                <asp:Button ID="LinkButton1" runat="server" Visible="true" CssClass="button" Text="Back"
                                                    OnClick="btnSimpleVarWankBack_Click" TabIndex="12" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:RequiredFieldZimmyator ID="rfSimpleVarWankWankin" runat="server" ControlToZimmyate="drpSimpleVarWankWankin"
                                        ErrorMessage="Please Enter SimpleVarWank Wankin." ZimmyationGroup="EditEmployee" Display="None"
                                        SetFocusOnError="true"></asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender
                                            ID="vcSimpleVarWankWankin" runat="server" TargetControlID="rfSimpleVarWankWankin">
                                        </cc1:ZimmyatorCalloutExtender>
                                    <asp:RequiredFieldZimmyator ID="rfSimpleVarWankType" runat="server" ControlToZimmyate="drpSimpleVarWankType"
                                        ErrorMessage="Please Select SimpleVarWank Type." ZimmyationGroup="EditEmployee" Display="None"
                                        SetFocusOnError="true"></asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender
                                            ID="vcSimpleVarWankType" runat="server" TargetControlID="rfSimpleVarWankType">
                                        </cc1:ZimmyatorCalloutExtender>
                                    <asp:RequiredFieldZimmyator ID="rfCity" runat="server" ControlToZimmyate="cityTextbox"
                                        ErrorMessage="Please Enter City." ZimmyationGroup="EditEmployee" Display="None"
                                        SetFocusOnError="true"></asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender
                                            ID="vcCity" runat="server" TargetControlID="rfCity">
                                        </cc1:ZimmyatorCalloutExtender>
                                    <asp:RequiredFieldZimmyator ID="rfCountry" runat="server" ControlToZimmyate="drpCountry"
                                        ErrorMessage="Please Select Country." ZimmyationGroup="EditEmployee" Display="None"
                                        SetFocusOnError="true"></asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender
                                            ID="vcCountry" runat="server" TargetControlID="rfCountry">
                                        </cc1:ZimmyatorCalloutExtender>
                                    <asp:RequiredFieldZimmyator ID="rfYouMustWankIt1" runat="server" ControlToZimmyate="txtYouMustWankIt1"
                                        ErrorMessage="Please Enter YouMustWankIt." ZimmyationGroup="EditEmployee" Display="None"
                                        SetFocusOnError="true"></asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender
                                            ID="vcYouMustWankIt1" runat="server" TargetControlID="rfYouMustWankIt1">
                                        </cc1:ZimmyatorCalloutExtender>
                                    <asp:RegularExpressionZimmyator ID="revCity" runat="server" SetFocusOnError="true"
                                        ZimmyationGroup="EditEmployee" ControlToZimmyate="cityTextbox" Display="None"
                                        ErrorMessage="Invalid City." ZimmyationExpression="^[a-zA-Z ]+$">
                                    </asp:RegularExpressionZimmyator>
                                    <cc1:ZimmyatorCalloutExtender ID="vceCityRev" runat="server" EnableViewState="true"
                                        HighlightCssClass="validatorCalloutHighlight" TargetControlID="revCity">
                                    </cc1:ZimmyatorCalloutExtender>
                                    <asp:RequiredFieldZimmyator ID="reqzip" ControlToZimmyate="zipCodeTextbox" SetFocusOnError="true"
                                        ErrorMessage="Please Enter Zip code." ZimmyationGroup="EditEmployee" runat="server"
                                        Display="None"></asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender ID="vdezip"
                                            runat="server" TargetControlID="reqzip">
                                        </cc1:ZimmyatorCalloutExtender>
                                    <asp:RegularExpressionZimmyator ID="revYouMustWankIt1" runat="server" SetFocusOnError="true"
                                        ZimmyationGroup="EditEmployee" ControlToZimmyate="txtYouMustWankIt1" Display="None"
                                        ErrorMessage="Invalid YouMustWankIt." ZimmyationExpression="(?!^[0-9]*$)^([a-zA-Z0-9_\.\#\-\/\, ]*)$">
                                    </asp:RegularExpressionZimmyator>
                                    <cc1:ZimmyatorCalloutExtender ID="vcYouMustWankIt" runat="server" EnableViewState="true"
                                        HighlightCssClass="validatorCalloutHighlight" TargetControlID="revYouMustWankIt1">
                                    </cc1:ZimmyatorCalloutExtender>
                                    <asp:RegularExpressionZimmyator ID="revYouMustWankIt2" runat="server" SetFocusOnError="true"
                                        ZimmyationGroup="AddNewEmployee" ControlToZimmyate="txtYouMustWankIt2" Display="None"
                                        ErrorMessage="Please Enter Only Alpha Numeric,Spaces and Allowed Characters # , - / _ ."
                                        ZimmyationExpression="^([a-zA-Z0-9_\.\#\-\/\, ]*)$">
                                    </asp:RegularExpressionZimmyator>
                                    <cc1:ZimmyatorCalloutExtender ID="vcYouMustWankIt2" runat="server" EnableViewState="true"
                                        HighlightCssClass="validatorCalloutHighlight" TargetControlID="revYouMustWankIt2">
                                    </cc1:ZimmyatorCalloutExtender>
                              
                            <asp:HiddenField ID="hdnDynamicContextKey" runat="server" Value="" />
                            <asp:HiddenField ID="hdnDynamicSimpleVarWankPreferencesKey" runat="server" Value="" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="color: #327586">
            <td style="width: 75%">
                <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnNewNewActiveTabChanged="loadTabPanel"
                    Height='320px' ScrollBars="Vertical" CssClass="ajax__tab_xp" Width="100%">
                    <cc1:TabPanel ID="TabHome" runat="server" Height="320px" DynamicContextKey="Home">
                        <HeaderTemplate>
                            Home
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" style="text-align: center; vertical-align: middle; height: 100%">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr valign="bottom">
                                    <td align="center" valign="baseline">
                                        <label class="successInformLabel">
                                            Please use the Segregated Tabs for filling specific information.
                                        </label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel1" DynamicContextKey="SimpleVarWankExecutive" runat="server"
                        Visible='<%# DisplayItem("468","canEdit") %>'>
                        <HeaderTemplate>
                            SimpleVarWank Executive Information
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <asp:Panel ID="panel1" runat="server" Visible="false">
                                                    <asp:UpdatePanel ID="upSimpleVarWankExecutiveInformation" runat="server" >
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr align="center">
                                                                    <td style="width: 430px">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td colspan="2" align="center" style="text-align: center">
                                                                                    <b>
                                                                                        <label class="label">
                                                                                            DoubleWank</label></b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <label for="drpTitle" class="label">
                                                                                        Title</label>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:DropDownList ID="drpTitle" runat="server" TabIndex="13" CssClass="DropDownControlSS" onclick ="javascript:Onchange();">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtDoubleWankFirstWankin" class="label">
                                                                                        First Wankin
                                                                                    </label>
                                                                                    <label class="mandatoryStarMark">
                                                                                        *
                                                                                    </label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDoubleWankFirstWankin" runat="server" CssClass="TextBoxSS" TabIndex="14"
                                                                                        MaxLength="30" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtDoubleWankLastWankin" class="label">
                                                                                        Last Wankin
                                                                                    </label>
                                                                                    <label class="mandatoryStarMark">
                                                                                        *
                                                                                    </label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDoubleWankLastWankin" runat="server" CssClass="TextBoxSS" TabIndex="15"
                                                                                        MaxLength="30" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtDoubleWankWankIt" class="label">
                                                                                        WankIt
                                                                                    </label>
                                                                                    <label class="mandatoryStarMark">
                                                                                        *
                                                                                    </label>
                                                                                </td>
                                                                                <td nowrap="nowrap">
                                                                                    <asp:TextBox ID="txtDoubleWankWankIt" runat="server" CssClass="TextBoxOffPh" TabIndex="16"
                                                                                        MaxLength="15" onblur="javascript:TextBox1Change(this,'revDoubleWankWankIt');" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                    <asp:Label ID="lblContactOneWankItExtn" runat="server" CssClass="label" Text="Extn"></asp:Label>
                                                                                    <asp:TextBox ID="txtContactOneWankItExtn" MaxLength="5" TabIndex="30" runat="server"
                                                                                        CssClass="TextBoxOffPhExtn" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtDoubleWankForeverWankinWankIt" class="label">
                                                                                        ForeverWankin WankIt
                                                                                    </label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDoubleWankForeverWankinWankIt" runat="server" CssClass="TextBoxSS" TabIndex="17"
                                                                                        MaxLength="15" onblur="javascript:TextBox1Change(this,'revDoubleWankForeverWankinWankIt');" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtDoubleWankWankItGood" class="label">
                                                                                        WankItGood
                                                                                    </label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDoubleWankWankItGood" runat="server" CssClass="TextBoxSS" TabIndex="18"
                                                                                        MaxLength="50" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtDoubleWankWankinOlskool" class="label">
                                                                                        WankinOlskool
                                                                                    </label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDoubleWankWankinOlskool" runat="server" CssClass="TextBoxSS" TabIndex="19"
                                                                                        MaxLength="15" onblur="javascript:TextBox1Change(this,'revDoubleWankWankinOlskool');" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="drpDoubleWankPreferredContactMedium" class="label">
                                                                                        Preferred Contact Medium
                                                                                    </label>
                                                                                    <label class="mandatoryStarMark">
                                                                                        *
                                                                                    </label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="drpDoubleWankPreferredContactMedium" runat="server" CssClass="DropDownControlSS"
                                                                                        onChange="setBasedOnDropDownForFirstContact(this,'rvWankItGood1','rvWankinOlskool1','rvForeverWankin1'); javascript:Onchange()"
                                                                                        TabIndex="20">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="rbDoubleWankActive" class="label">
                                                                                        Active
                                                                                    </label>
                                                                                    
                                                                                </td>
                                                                                <td style="text-align: left;">
                                                                                    <asp:RadioButton ID="rbDoubleWankActive" runat="server" Checked="true" TabIndex="21"
                                                                                        GroupWankin="Active" onclick="javascript:EnableDisableRadioButton(this,'FALSE'); javascript:Onchange();" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="width: 400px">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td align="center" style="text-align: center" colspan="2">
                                                                                    <b>
                                                                                        <label class="label">
                                                                                            WeWeWank</label>
                                                                                    </b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <label for="drpTitle1" class="label">
                                                                                        Title</label>
                                                                                </td>
                                                                                <td style="text-align: left">
                                                                                    <asp:DropDownList ID="drpTitle1" runat="server" TabIndex="22" CssClass="DropDownControlSS" onclick ="javascript:Onchange();">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtWeWeWankFirstWankin" class="label">
                                                                                        First Wankin</label>
                                                                                    <asp:Label ID="Label24" runat="server" CssClass="mandatoryStarMark" Text="*" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtWeWeWankFirstWankin" runat="server" CssClass="TextBoxSS" TabIndex="23"
                                                                                        MaxLength="30" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtWeWeWankLastWankin" class="label">
                                                                                        Last Wankin</label>
                                                                                    <asp:Label ID="Label25" runat="server" CssClass="mandatoryStarMark" Text="*" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtWeWeWankLastWankin" runat="server" CssClass="TextBoxSS" TabIndex="24"
                                                                                        MaxLength="30" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtWeWeWankWankIt" class="label">
                                                                                        WankIt</label>
                                                                                    <asp:Label ID="Label26" runat="server" CssClass="mandatoryStarMark" Text="*" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtWeWeWankWankIt" runat="server" CssClass="TextBoxOffPh" TabIndex="25"
                                                                                        MaxLength="15" onblur="javascript:TextBox1Change(this,'revWeWeWankWankIt');" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                    <asp:Label ID="lblContactTwoWankItExtn" runat="server" CssClass="label" Text="Extn"></asp:Label>
                                                                                    <asp:TextBox ID="txtContactTwoWankItExtn" MaxLength="5" TabIndex="30" runat="server"
                                                                                        CssClass="TextBoxOffPhExtn" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtWeWeWankForeverWankinWankIt" class="label">
                                                                                        ForeverWankin WankIt</label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtWeWeWankForeverWankinWankIt" runat="server" CssClass="TextBoxSS" TabIndex="26"
                                                                                        MaxLength="15" onblur="javascript:TextBox1Change(this,'revWeWeWankForeverWankinWankIt');" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtWeWeWankWankItGood" class="label">
                                                                                        WankItGood</label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtWeWeWankWankItGood" runat="server" CssClass="TextBoxSS" TabIndex="27"
                                                                                        MaxLength="50" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="txtWeWeWankWankinOlskool" class="label">
                                                                                        WankinOlskool</label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtWeWeWankWankinOlskool" runat="server" CssClass="TextBoxSS" TabIndex="28"
                                                                                        MaxLength="15" onblur="javascript:TextBox1Change(this,'revWeWeWankWankinOlskool');" onchange="javascript:Onchange();"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="drpWeWeWankPreferedContactMedium" class="label">
                                                                                        Preferred Contact Medium</label>
                                                                                    <asp:Label ID="Label29" runat="server" CssClass="mandatoryStarMark" Text="*" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="drpWeWeWankPreferedContactMedium" runat="server" CssClass="DropDownControlSS"
                                                                                        onChange="setBasedOnDropDownForSecondContact(this,'rvWankItGood2','rvWankinOlskool2','rvForeverWankin2'); javascript:Onchange();"
                                                                                        TabIndex="29">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <label for="rbWeWeWankActive" class="label">
                                                                                        Active</label>
                                                                                </td>
                                                                                <td style="text-align: left;">
                                                                                    <asp:RadioButton ID="rbWeWeWankActive" runat="server" TabIndex="30" GroupWankin="Active"
                                                                                        onclick="javascript:EnableDisableRadioButton(this,'FALSE'); javascript:Onchange();" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="6" align="center">
                                                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upSimpleVarWankExecutiveInformation">
                                                                            <ProgressTemplate>
                                                                                <img src="../images/ajax-loader.gif" alt="" />
                                                                                Processing.....
                                                                            </ProgressTemplate>
                                                                        </asp:UpdateProgress>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table align="center">
                                                                <tr>
                                                                    <td colspan="2" align="right">
                                                                        &nbsp;<asp:Button ID="btnSimpleVarWankExecutiveInformationSave" runat="server" TabIndex="31"
                                                                            CssClass="button" ZimmyationGroup="AddSimpleVarWankExecutiveInformation" Text="Save"
                                                                            OnNewNewClick="EnableZimmyators(this,'drpDoubleWankPreferredContactMedium','rvWankItGood1','rvWankinOlskool1','rvForeverWankin1','drpWeWeWankPreferedContactMedium','rvWankItGood2','rvWankinOlskool2','rvForeverWankin2','TRUE')"
                                                                            OnClick="btnSimpleVarWankExecutiveInformationSave_Click" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Button ID="btnSimpleVarWankExecutiveInformationReset" runat="server" TabIndex="32"
                                                                            CssClass="button" Text="Reset" OnClick="btnSimpleVarWankExecutiveInformationReset_Click" />
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" align="center">
                                                                        <asp:Label ID="lblResult2" runat="server" CssClass="label"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:RequiredFieldZimmyator ID="rfDoubleWankFirstWankin" runat="server" ControlToZimmyate="txtDoubleWankFirstWankin"
                                                                ErrorMessage="Please Enter First Wankin." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Display="None" SetFocusOnError="true"></asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcDoubleWankFirstWankin" runat="server" TargetControlID="rfDoubleWankFirstWankin"
                                                                Width="2px">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rfDoubleWankLastWankin" runat="server" ControlToZimmyate="txtDoubleWankLastWankin"
                                                                ErrorMessage="Please Enter Last Wankin." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Display="None" SetFocusOnError="true"></asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcDoubleWankLastWankin" runat="server" TargetControlID="rfDoubleWankLastWankin"
                                                                Width="2px">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rfDoubleWankWankIt" runat="server" ControlToZimmyate="txtDoubleWankWankIt"
                                                                ErrorMessage="Please Enter WankIt." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Display="None" SetFocusOnError="true"></asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcDoubleWankWankIt" runat="server" TargetControlID="rfDoubleWankWankIt"
                                                                Width="2px" BehaviorID="bvcDoubleWankWankIt">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revDoubleWankWankIt" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtDoubleWankWankIt"
                                                                Display="None" ErrorMessage="Please Enter WankIt Number in (999)-999-9999 Format."
                                                                ZimmyationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceDoubleWankWankIt" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revDoubleWankWankIt"
                                                                BehaviorID="bveDoubleWankWankIt">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <cc1:MaskedEditExtender ID="meDoubleWankWankIt" runat="server" TargetControlID="txtDoubleWankWankIt"
                                                                Mask="(999)-999-9999" MessageZimmyatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="None" ClearMaskOnLostFocus="false">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:MaskedEditZimmyator ID="mevDoubleWankWankIt" runat="server" ControlToZimmyate="txtDoubleWankWankIt"
                                                                Display="None" ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlExtender="meDoubleWankWankIt"
                                                                InvalidValueMessage="Please Enter WankIt in (999)-999-9999 format.">
                                                            </cc1:MaskedEditZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcevWankIt" runat="server" TargetControlID="mevDoubleWankWankIt">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revDoubleWankWankItExtn" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtContactOneWankItExtn"
                                                                Display="None" ErrorMessage="Please Enter valid Extension. Example 33333." ZimmyationExpression="^[0-9]+$">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceDoubleWankWankItExtn" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revDoubleWankWankItExtn">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revDoubleWankForeverWankinWankIt" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtDoubleWankForeverWankinWankIt"
                                                                Display="None" ErrorMessage="Please Enter ForeverWankin WankIt Number in (999)-999-9999 Format."
                                                                ZimmyationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$"
                                                                Enabled="false">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceDoubleWankForeverWankinWankIt" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revDoubleWankForeverWankinWankIt"
                                                                BehaviorID="bveDoubleWankForeverWankinWankIt">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDoubleWankForeverWankinWankIt"
                                                                Mask="(999)-999-9999" MessageZimmyatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="None" ClearMaskOnLostFocus="false">
                                                            </cc1:MaskedEditExtender>
                                                            <asp:RegularExpressionZimmyator ID="revDoubleWankWankinOlskool" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtDoubleWankWankinOlskool"
                                                                Display="None" ErrorMessage="Please Enter WankinOlskool in (999)-999-9999 Format." ZimmyationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$"
                                                                Enabled="false">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceDoubleWankWankinOlskool" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revDoubleWankWankinOlskool"
                                                                BehaviorID="bveDoubleWankWankinOlskool">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtDoubleWankWankinOlskool"
                                                                Mask="(999)-999-9999" MessageZimmyatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="None" ClearMaskOnLostFocus="false">
                                                            </cc1:MaskedEditExtender>
                                                            <asp:RegularExpressionZimmyator ID="revDoubleWankWankItGood" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtDoubleWankWankItGood"
                                                                Display="None" ErrorMessage="Please Enter Zimmy WankItGood YouMustWankIt (eg)kumar@gmail.com">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceDoubleWankWankItGood" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revDoubleWankWankItGood">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rfWeWeWankFirstWankin" runat="server" ControlToZimmyate="txtWeWeWankFirstWankin"
                                                                ErrorMessage="Please Enter First Wankin." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Enabled="false" Display="None" SetFocusOnError="true"></asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender
                                                                    ID="vcWeWeWankFirstWankin" runat="server" TargetControlID="rfWeWeWankFirstWankin">
                                                                </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rfWeWeWankLastWankin" runat="server" ControlToZimmyate="txtWeWeWankLastWankin"
                                                                ErrorMessage="Please Enter Last Wankin." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Display="None" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldZimmyator><cc1:ZimmyatorCalloutExtender
                                                                    ID="vcWeWeWankLastWankin" runat="server" TargetControlID="rfWeWeWankLastWankin">
                                                                </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rfWeWeWankWankIt" runat="server" ControlToZimmyate="txtWeWeWankWankIt"
                                                                ErrorMessage="Please Enter WankIt." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Display="None" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcWeWeWankWankIt" runat="server" TargetControlID="rfWeWeWankWankIt"
                                                                Width="2px" BehaviorID="bvcWeWeWankWankIt">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revWeWeWankWankIt" runat="server" SetFocusOnError="true"
                                                                Enabled="false" ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtWeWeWankWankIt"
                                                                Display="None" ErrorMessage="Please Enter WankIt Number in (999)-999-9999 Format."
                                                                ZimmyationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceWeWeWankWankIt" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revWeWeWankWankIt"
                                                                BehaviorID="bveWeWeWankWankIt">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <cc1:MaskedEditExtender ID="meWeWeWankWankIt" runat="server" TargetControlID="txtWeWeWankWankIt"
                                                                Mask="(999)-999-9999" MessageZimmyatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="None" ClearMaskOnLostFocus="false">
                                                            </cc1:MaskedEditExtender>
                                                            <cc1:MaskedEditZimmyator ID="mevWeWeWankWankIt" runat="server" ControlToZimmyate="txtWeWeWankWankIt"
                                                                Display="None" ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlExtender="meWeWeWankWankIt"
                                                                InvalidValueMessage="Please Enter WankIt in (999)-999-9999 Format.">
                                                            </cc1:MaskedEditZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="ZimmyatorCalloutExtender3" runat="server" TargetControlID="mevWeWeWankWankIt">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revContactTwoWankItExtn" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtContactTwoWankItExtn"
                                                                Display="None" ErrorMessage="Please Enter valid Extension. Example 33333." ZimmyationExpression="^[0-9]+$">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceContactTwoWankItExtn" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revContactTwoWankItExtn">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revWeWeWankForeverWankinWankIt" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtWeWeWankForeverWankinWankIt"
                                                                Display="None" ErrorMessage="Please Enter ForeverWankin WankIt Number in (999)-999-9999 Format."
                                                                ZimmyationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$"
                                                                Enabled="false">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceWeWeWankForeverWankinWankIt" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revWeWeWankForeverWankinWankIt"
                                                                BehaviorID="bveWeWeWankForeverWankinWankIt">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtWeWeWankForeverWankinWankIt"
                                                                Mask="(999)-999-9999" MessageZimmyatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="None" ClearMaskOnLostFocus="false">
                                                            </cc1:MaskedEditExtender>
                                                            <asp:RegularExpressionZimmyator ID="revWeWeWankWankinOlskool" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtWeWeWankWankinOlskool"
                                                                Display="None" ErrorMessage="Please Enter WankinOlskool in (999)-999-9999 Format." ZimmyationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$"
                                                                Enabled="false">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceWeWeWankWankinOlskool" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revWeWeWankWankinOlskool"
                                                                BehaviorID="bveWeWeWankWankinOlskool">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtWeWeWankWankinOlskool"
                                                                Mask="(999)-999-9999" MessageZimmyatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                                OnInvalidCssClass="MaskedEditError" MaskType="None" ClearMaskOnLostFocus="false">
                                                            </cc1:MaskedEditExtender>
                                                            <asp:RegularExpressionZimmyator ID="revWeWeWankWankItGood" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtWeWeWankWankItGood"
                                                                Display="None" ErrorMessage="Please Enter Zimmy WankItGood YouMustWankIt (eg)kumar@gmail.com">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceWeWeWankWankItGood" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revWeWeWankWankItGood">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rvWankItGood1" Display="None" runat="server" ControlToZimmyate="txtDoubleWankWankItGood"
                                                                ErrorMessage="Please Enter WankItGood YouMustWankIt." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Font-Size="8" SetFocusOnError="True" Enabled="False">
                                                            </asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceWankItGood" runat="server" TargetControlID="rvWankItGood1"
                                                                Width="2px" BehaviorID="bvceWankItGood">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rvForeverWankin1" Display="None" runat="server" ControlToZimmyate="txtDoubleWankForeverWankinWankIt"
                                                                ErrorMessage="Please Enter ForeverWankin WankIt Number." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Font-Size="8" SetFocusOnError="True" Enabled="False">
                                                            </asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceForeverWankin1" runat="server" TargetControlID="rvForeverWankin1"
                                                                Width="2px" BehaviorID="bvceForeverWankin1">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rvWankinOlskool1" Display="None" runat="server" ControlToZimmyate="txtDoubleWankWankinOlskool"
                                                                ErrorMessage="Please Enter WankinOlskool Number." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Font-Size="8" SetFocusOnError="True" Enabled="False">
                                                            </asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcervWankinOlskool1" runat="server" TargetControlID="rvWankinOlskool1"
                                                                Width="2px" BehaviorID="bvceWankinOlskool1">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rvForeverWankin2" Display="None" runat="server" ControlToZimmyate="txtWeWeWankForeverWankinWankIt"
                                                                ErrorMessage="Please Enter ForeverWankin WankIt Number." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Font-Size="8" SetFocusOnError="True" Enabled="False">
                                                            </asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcervForeverWankin2" runat="server" TargetControlID="rvForeverWankin2"
                                                                Width="2px" BehaviorID="bvceForeverWankin2">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rvWankItGood2" Display="None" runat="server" ControlToZimmyate="txtWeWeWankWankItGood"
                                                                ErrorMessage="Please Enter WankItGood YouMustWankIt." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Font-Size="8" SetFocusOnError="True" Enabled="False">
                                                            </asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcervWankItGood2" runat="server" TargetControlID="rvWankItGood2"
                                                                Width="2px" BehaviorID="bvceWankItGood2">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rvWankinOlskool2" Display="None" runat="server" ControlToZimmyate="txtWeWeWankWankinOlskool"
                                                                ErrorMessage="Please Enter WankinOlskool Number." ZimmyationGroup="AddSimpleVarWankExecutiveInformation"
                                                                Font-Size="8" SetFocusOnError="True" Enabled="False">
                                                            </asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcervWankinOlskool2" runat="server" TargetControlID="rvWankinOlskool2"
                                                                Width="2px" BehaviorID="bvceWankinOlskool2">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="revContact1" InitialValue="" runat="server" ControlToZimmyate="drpDoubleWankPreferredContactMedium"
                                                                Font-Size="8" ZimmyationGroup="AddSimpleVarWankExecutiveInformation" Display="None"
                                                                ErrorMessage="Please Select the Preferred Contact Medium." SetFocusOnError="True"></asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceContact1" runat="server" TargetControlID="revContact1"
                                                                Width="2px">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RequiredFieldZimmyator ID="rfvPreferedContact2" InitialValue="" runat="server"
                                                                ControlToZimmyate="drpWeWeWankPreferedContactMedium" Font-Size="8" Display="None"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ErrorMessage="Please Select the Preferred Contact Medium."
                                                                SetFocusOnError="True" Enabled="false"></asp:RequiredFieldZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vcePreferedContact2" runat="server" TargetControlID="rfvPreferedContact2"
                                                                Width="2px">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revDoubleWankFirstWankin" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtDoubleWankFirstWankin"
                                                                Display="None" ErrorMessage="Please Enter Only Alpha Numeric and Allowed Characters & , ' - _ . &quot;.">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceDoubleWankFirstWankin" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revDoubleWankFirstWankin">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revDoubleWankLastWankin" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtDoubleWankLastWankin"
                                                                Display="None" ErrorMessage="Please Enter Only Alpha Numeric and Allowed Characters & , ' - _ . &quot;.">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceDoubleWankLastWankin" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revDoubleWankLastWankin">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revWeWeWankFirstWankin" runat="server" SetFocusOnError="true"
                                                                Enabled="false" ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtWeWeWankFirstWankin"
                                                                Display="None" ErrorMessage="Please Enter Only Alpha Numeric and Allowed Characters & , ' - _ . &quot;.">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceWeWeWankFirstWankin" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revWeWeWankFirstWankin">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                            <asp:RegularExpressionZimmyator ID="revWeWeWankLastWankin" runat="server" SetFocusOnError="true"
                                                                ZimmyationGroup="AddSimpleVarWankExecutiveInformation" ControlToZimmyate="txtWeWeWankLastWankin"
                                                                Enabled="false" Display="None" ErrorMessage="Please Enter Only Alpha Numeric and Allowed Characters & , ' - _ . &quot;.">
                                                            </asp:RegularExpressionZimmyator>
                                                            <cc1:ZimmyatorCalloutExtender ID="vceWeWeWankLastWankin" runat="server" EnableViewState="true"
                                                                HighlightCssClass="validatorCalloutHighlight" TargetControlID="revWeWeWankLastWankin">
                                                            </cc1:ZimmyatorCalloutExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel2" DynamicContextKey="NewNew" runat="server" Visible='<%# DisplayItem("470","canView") %>'>
                        <HeaderTemplate>
                            NewNew
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Panel Visible="false" ID="panel2" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblResult3" runat="server" CssClass="label" Visible="false" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                                                        <ProgressTemplate>
                                                            <img src="../images/ajax-loader.gif" alt="" />
                                                            Processing.....
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:GridView ID="GridView1" CssClass="grdv" HeaderStyle-CssClass="header" AlternatingRowStyle-CssClass="alternating"
                                                        RowStyle-CssClass="normal" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                                        AllowSorting="true" OnPageIndexChanging="GridView1_PageIndexChanging" OnSorting="GridView1_Sorting"
                                                        OnPreRender="GridView1_PreRender" OnRowCommand="GridView1_RowCommand" Width="98%" OnRowDataBound="GridView1_RowDataBound" >
                                                        <PagerStyle CssClass="cssPager" />
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNewNewId" runat="server" Text='<%# Eval("clientId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="NewNew Wankin" SortExpression="clientWankin">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGdNewNewWankin" Text='<%# Eval("clientWankin") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SimpleVarWank Number" SortExpression="SimpleVarWankNumber">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="grdlblSimpleVarWankNumber" Text='<%# Eval("SimpleVarWankNumber") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Contact 1" SortExpression="Contact1">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGdContact1" Text='<%# Eval("Contact1") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Contact 2" SortExpression="Contact2">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="chkGdIsActive" Text='<%# Eval("Contact2") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DoubleWank YouMustWankIt" SortExpression="DoubleWankYouMustWankIt">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="grdlblDoubleWankYouMustWankIt" Text='<%# Eval("DoubleWankYouMustWankIt") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Zip" SortExpression="Zip">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="grdlblZip" Text='<%# Eval("Zip") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="City" SortExpression="City">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="grdlblCity" Text='<%# Eval("City") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="State" SortExpression="State">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="grdlblState" Text='<%# Eval("State") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                              <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIsNewNewFromWE" runat="server" Text='<%# Eval("ExternalKey") %>' Visible="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:ButtonField ButtonType="Link" Text="Edit" CommandWankin="Edit" ItemStyle-Width="5%" />
                                                            <asp:ButtonField ButtonType="Link" Text="View" CommandWankin="View" ItemStyle-Width="5%" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabNewNewPreferences" runat="server" DynamicContextKey="NewNewPreferences"
                        Visible='<%# DisplayItem("575,576,577,578","canEdit") %>'>
                        <HeaderTemplate>
                            SimpleVarWank Preferences
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upSimpleVarWankPreferences" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlSimpleVarWankPreferences" runat="server" Visible="false">
                                        <cc1:TabContainer ID="tabSimpleVarWankPreference" runat="server" ActiveTabIndex="0" Height='310px'
                                            Visible='<%# DisplayItem("575,576,577,578,2121","canEdit") %>' OnNewNewActiveTabChanged="loadSimpleVarWankTabPanel">
                                            <cc1:TabPanel ID="tabBillingInvoice" runat="server" Visible='<%# DisplayItem("575","canEdit") %>'
                                                DynamicContextKey="BillingInvoice">
                                                <HeaderTemplate>
                                                    Billing Invoice
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpdatePaneBillingInvoice" runat="server">
                                                        <ContentTemplate>
                                                            <iframe id="iframeBillimgInvoice" src="SimpleVarWank_Billing_Pricing_New.aspx" width="98%"
                                                                height="310px" frameborder="0" runat="server" visible="false"></iframe>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="tabITApplications" runat="server" Visible='<%# DisplayItem("577","canEdit") %>'
                                                DynamicContextKey="ITApplications">
                                                <HeaderTemplate>
                                                    IT Applications
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpdatePaneITapplication" runat="server">
                                                        <ContentTemplate>
                                                            <iframe id="iframe6" src="SimpleVarWankITApplication_New.aspx" width="98%" height="310px"
                                                                frameborder="0" runat="server" visible="false"></iframe>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="tabScreeningOptions" runat="server" Visible='<%# DisplayItem("578","canEdit") %>'
                                                DynamicContextKey="ScreeningOptions">
                                                <HeaderTemplate>
                                                    Screening Options
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpdatePanelScreeningOptions" runat="server">
                                                        <ContentTemplate>
                                                            <iframe id="iframe5" src="SimpleVarWankScreeningOption.aspx" width="98%" height="310px"
                                                                frameborder="0" runat="server" visible="false"></iframe>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="tabGeneral" runat="server" DynamicContextKey="SimpleVarWankGeneral" Visible='<%# DisplayItem("1202","canEdit") %>'>
                                                <HeaderTemplate>
                                                    SimpleVarWank General
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="upSimpleVarWankGeneral" runat="server">
                                                        <ContentTemplate>
                                                            <iframe runat="server" id="iframeSimpleVarWankGeneral" src="SimpleVarWankGeneral.aspx" width="98%"
                                                                height="350px" frameborder="0" runat="server" visible="false"></iframe>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="tabWellnessWankItGoodSetup" runat="server" DynamicContextKey="WellnessWankItGoodSetup"
                                                Visible='<%# DisplayItem("2121","canEdit") %>'>
                                                <HeaderTemplate>
                                                    Wellness WankItGood Setup
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="upWellnessWankItGoodSetup" runat="server">
                                                        <ContentTemplate>
                                                            <iframe runat="server" id="iframeWellnessWankItGoodSetup" src="../Common/WellnessWankItGoodSetup.aspx"
                                                                width="98%" height="350px" visible="false" frameborder="0"></iframe>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                             <cc1:TabPanel ID="tbImmunizationWankItGoodSetUp" runat="server" DynamicContextKey="ImmunizationWankItGoodSetUp"
                                                Visible='<%# DisplayItem("2122","canEdit") %>'>
                                                <HeaderTemplate>
                                                    Immunization WankItGoods Setup
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpanelImmuWankItGoodSetUp" runat="server">
                                                        <ContentTemplate>
                                                            <iframe runat="server" id="iframeImmunizationWankItGoodSetUp" src="../Common/ImmunizationWankItGoodSetUp.aspx"
                                                                width="98%" height="350px" frameborder="0"></iframe>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabContractInformation" runat="server" DynamicContextKey="ContractInformation"
                        Visible='<%# DisplayItem("579","canEdit") %>'>
                        <HeaderTemplate>
                            Contract Information
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upContractInformation" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeContractInformation" src="SimpleVarWankContractPreference.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                    <%--visible="false"--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabCommission" runat="server" DynamicContextKey="Commission" Visible='<%# DisplayItem("580","canEdit") %>'>
                        <HeaderTemplate>
                            Commission
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upCommission" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeCommission" src="SimpleVarWankCommission.aspx" width="98%"
                                        height="200px" frameborder="0" visible="false"></iframe>
                                    <%--visible="false"--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabVerilyManagementTeam" runat="server" DynamicContextKey="VerilyManagementTeam"
                        Visible='<%# DisplayItem("731","canEdit") %>'>
                        <HeaderTemplate>
                            Verily Management Team
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpPanelPMTeam" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframePMTeam" src="SimpleVarWankVerilyManagementTeam.aspx"
                                        width="98%" height="530px" frameborder="0" visible="false"></iframe>
                                    <%--visible="false"--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabAccessSimpleVarWankPortal" runat="server" DynamicContextKey="AccessSimpleVarWankPortal"
                        Visible='<%# DisplayItem("1206","canEdit") %>'>
                        <HeaderTemplate>
                            Access SimpleVarWank Portal
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upAccessSimpleVarWankPortal" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeAccessSimpleVarWankPortal" src="AccessSimpleVarWankPortal.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabLibraryOfDocuments" runat="server" DynamicContextKey="LibraryOfDocuments"
                        Visible='<%# DisplayItem("1204","canEdit") %>'>
                        <HeaderTemplate>
                            Library of Documents
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upLibraryOfDocuments" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframetabLibraryOfDocuments" src="AccLibraryOfDocs.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabImmunizationPreference" runat="server" DynamicContextKey="ImmunizationPreference"
                        Visible='<%# DisplayItem("2033","canEdit") %>'>
                        <HeaderTemplate>
                            Immunization Preference
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeImmunizationPreference" src="ImmunizationPreference.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                    <%--visible="false"--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabHomeTestKitQuestions" runat="server" DynamicContextKey="HomeTestKitQuestions"
                        Visible="true">
                        <HeaderTemplate>
                            Home Test Kit Preferences
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upHTKQuestions" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeHTKQuestions" src="../Common/HTKPreferences.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPSCPreferences" runat="server" DynamicContextKey="PSCPreferences"
                        Visible="true">
                        <HeaderTemplate>
                            PSC Preferences
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upPSCPreferences" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframePSCPreferences" src="../Common/PSCPreferences.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabDVPreferences" runat="server" DynamicContextKey="DVPreferences"
                        Visible="true">
                        <HeaderTemplate>
                            Doctor Office Preferences
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upDVPreferences" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeDVPreferences" src="../DoctorOffice/DVPreferences.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabHVPreferences" runat="server" DynamicContextKey="HVPreferences"
                        Visible="true">
                        <HeaderTemplate>
                            Home Visit Preferences
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upHVPreferences" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeHVPreferences" src="../Common/HVPreferences.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabCallCenterScripts" runat="server" DynamicContextKey="CallCenterScripts"
                        Visible='<%# DisplayItem("2087","canEdit") %>'>
                        <HeaderTemplate>
                            Call Center Scripts
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upCallCenter" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeSimpleVarWankCallcenterScripts" src="SimpleVarWankCallcenterScripts.aspx"
                                        width="98%" height="350px" frameborder="0" visible="false"></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
                    <cc1:TabPanel ID="tabSatisfactionSurvey" runat="server" DynamicContextKey="Satisfactionsurvey">
                        
                        <HeaderTemplate>
                            Satisfaction Survey
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanelSatisfactionSurvey" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel3" runat="server">
                                        <cc1:TabContainer ID="TCSatisfactionSurvey" runat="server" ActiveTabIndex="0" Height='310px'
                                            ScrollBars="Vertical" CssClass="ajax__tab_xp" Width="100%">
                                            
                                            <cc1:TabPanel ID="tpSiteCoordinatorSurvey" runat="server">
                                                
                                                <HeaderTemplate>
                                                    Site Coordinator Survey
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UPSiteCoordinatorSurvey" runat="server">
                                                        <ContentTemplate>
                                                            <iframe id="iframeSiteCoordinatorSurvey" runat="server" src="SiteCoordinatorSatisfactionSurvey.aspx"
                                                                width="98%" height="310px" frameborder="0" visible="false"></iframe>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="tpParticipantSurvey" runat="server">
                                                
                                                <HeaderTemplate>
                                                    Participant Survey
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="UpParticipantSurvey" runat="server">
                                                        <ContentTemplate>
                                                            <iframe id="iframeParticipantSurvey" runat="server" src="ParticipantSatisfactionSurvey.aspx"
                                                                width="98%" height="310px" frameborder="0" visible="false"></iframe>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabRTDCPHQ" runat="server" DynamicContextKey="RTDCPHQ">
                        
                        <HeaderTemplate>
                            RTDC PHQ
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpanelRTDCPHQ" runat="server">
                                <ContentTemplate>
                                    <iframe runat="server" id="iframeRTDCPHQ" src="RTDCPHQ.aspx" width="98%" height="350px"
                                        frameborder="0" visible="false"></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                
            </td>
        </tr>
      
    </table>
    <!-- 
******* The following script is to prevent double postback on any of the buttons while an async post back is happening****
******* DO NOT REMOVE, IT MUST BE AT THE END OF THE CONTENT**********
******* script found on asp.net forum -- http://forums.asp.net/p/1173891/1976235.aspx#1976235
-->

    <script type="text/javascript" language="javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endRequest);

        var postbackElement;
        // checks the PageRequestManager if there is already a postback being processed
        // and aborts the postback (question: which postback does it abort?)
        // See details here: http://microsoftmiles.blogspot.com/2006/11/maintaining-gridview-scroll-position-in.html
        // and http://geekswithblogs.net/rashid/archive/2007/08/08/Asp.net-Ajax-UpdatePanel-Simultaneous-Update---A-Remedy.aspx
        function initializeRequest(sender, args) {
            //document.body.style.cursor = "wait";
            if (prm.get_isInAsyncPostBack()) {
                //debugger
                args.set_cancel(true);
            }
        }

        function endRequest(sender, args) {
            //document.body.style.cursor = "default";
        }
    </script>

</asp:Content>
