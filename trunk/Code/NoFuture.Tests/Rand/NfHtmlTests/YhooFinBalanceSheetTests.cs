﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand.NfHtmlTests
{
    [TestClass]
    public class YhooFinBalanceSheetTests
    {
        #region

        private string _testData = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">
<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""><title>YHOO Balance Sheet | Yahoo! Inc. Stock - Yahoo! Finance</title><script type=""text/javascript"" src=""http://l.yimg.com/a/i/us/fi/03rd/yg_csstare_nobgcolor.js""></script><link rel=""stylesheet"" href=""http://l.yimg.com/zz/combo?kx/yucs/uh3/uh/1138/css/uh_non_mail-min.css&amp;kx/yucs/uh3s/atomic/84/css/atomic-min.css&amp;kx/yucs/uh_common/meta/3/css/meta-min.css&amp;kx/yucs/uh3/top-bar/366/css/no_icons-min.css&amp;kx/yucs/uh3/search/css/588/blue_border-min.css&amp;kx/yucs/uh3/get-the-app/151/css/get_the_app-min.css&amp;bm/lib/fi/common/p/d/static/css/2.0.356981/2.0.0/mini/yfi_yoda_legacy_lego_concat.css&amp;bm/lib/fi/common/p/d/static/css/2.0.356981/2.0.0/mini/yfi_symbol_suggest.css&amp;bm/lib/fi/common/p/d/static/css/2.0.356981/2.0.0/mini/yui_helper.css&amp;bm/lib/fi/common/p/d/static/css/2.0.356981/2.0.0/mini/yfi_theme_teal.css&amp;bm/lib/fi/common/p/d/static/css/2.0.356981/2.0.0/mini/yfi_follow_quote.css&amp;bm/lib/fi/common/p/d/static/css/2.0.356981/2.0.0/mini/yfi_follow_stencil.css"" type=""text/css""><script language=""javascript"">
	ll_js = new Array();
    </script><script type=""text/javascript"" src=""http://l1.yimg.com/bm/combo?fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yui-min-3.9.1.js&amp;fi/common/p/d/static/js/2.0.356981/yui_2.8.0/build/yuiloader-dom-event/2.0.0/mini/yuiloader-dom-event.js&amp;fi/common/p/d/static/js/2.0.356981/yui_2.8.0/build/container/2.0.0/mini/container.js&amp;fi/common/p/d/static/js/2.0.356981/yui_2.8.0/build/datasource/2.0.0/mini/datasource.js&amp;fi/common/p/d/static/js/2.0.356981/yui_2.8.0/build/autocomplete/2.0.0/mini/autocomplete.js""></script><script language=""javascript"">
	YUI.YUICfg = {""base"":""http:\/\/l.yimg.com\/"",""comboBase"":""http:\/\/l.yimg.com\/zz\/combo?"",""combine"":true,""allowRollup"":true,""maxURLLength"":""2000""}
	YUI.YUICfg.root = 'yui:'+YUI.version+'/build/';
	YUI.applyConfig(YUI.YUICfg); 
    </script><script language=""javascript"">
	ll_js.push({
	    'success_callback' : function() {
                YUI().use('stencil', 'follow-quote', 'node', function (Y) {
                    var conf = {'xhrBase': '/', 'lang': 'en-US', 'region': 'US', 'loginUrl': 'https://login.yahoo.com/config/login_verify2?&.done=http://finance.yahoo.com/q?s=YHOO&.intl=us'};

                    Y.Media.FollowQuote.init(conf, function () {
                        var exchNode = null,
                            followSecClass = """",
                            followHtml = """",
                            followNode = null;

                        followSecClass = Y.Media.FollowQuote.getFollowSectionClass();
                        followHtml = Y.Media.FollowQuote.getFollowBtnHTML({ ticker: 'YHOO', addl_classes: ""follow-quote-always-visible"", showFollowText: true });
                        followNode = Y.Node.create(followHtml);
                        exchNode = Y.one("".wl_sign"");
                        if (!Y.Lang.isNull(exchNode)) { 
                            exchNode.append(followNode);
                            
                        }

                    });
                });
	    }
	});
    </script><style>
       	    /* [bug 3856904]*/
            #ygma.ynzgma #teleban {width:100%;}
            
            /* Style to override message boards template CSS */
            .mboard div#footer {width: 970px !important;}
            .mboard #screen {width: 970px !important; text-align:left !important;}
            .mboard div#screen {width: 970px !important; text-align:left !important;}
            .mboard table.yfnc_modtitle1 td{padding-top:10px;padding-bottom:10px;} 
	</style><meta name=""keywords"" content=""balance sheet,asset,liabilities,shareholders equity,return on equity,cash equivalents,retained earning""><meta name=""description"" content=""Get the detailed balance sheet for Yahoo! Inc. (YHOO). Check out the financial snapshot for possessions, debts and capital invested at a particular date.""><script type=""text/javascript""><!--
    var yfid    = document;
    var yfiagt  = navigator.userAgent.toLowerCase();
    var yfidom  = yfid.getElementById;
    var yfiie   = yfid.all;
    var yfimac  = (yfiagt.indexOf('mac')!=-1);
    var yfimie  = (yfimac&&yfiie);
    var yfiie5  = (yfiie&&yfidom&&!yfimie&&!Array.prototype.pop);
    var yfiie55 = (yfiie&&yfidom&&!yfimie&&!yfiie5);
    var yfiie6  = (yfiie55&&yfid.compatMode);
    var yfisaf  = ((yfiagt.indexOf('safari')>-1)?1:0);
    var yfimoz  = ((yfiagt.indexOf('gecko')>-1&&!yfisaf)?1:0);
    var yfiopr  = ((yfiagt.indexOf('opera')>-1&&!yfisaf)?1:0);
            //--></script></head><body class="" intl-us yfin_gs gsg-0""><div id=""masthead""><div class=""yfi_doc yog-hd"" id=""yog-hd""><div class=""""><style>#header,#y-hd,#hd .yfi_doc,#yfi_hd{background:#fff !important}#yfin_gs #yfimh #yucsHead,#yfin_gs #yfi_doc #yucsHead,#yfin_gs #yfi_fp_hd #yucsHead,#yfin_gs #y-hd #yucsHead,#yfin_gs #yfi_hd #yucsHead,#yfin_gs #yfi-doc #yucsHead{-webkit-box-shadow:0 0 9px 0 #490f76 !important;-moz-box-shadow:0 0 9px 0 #490f76 !important;box-shadow:0 0 9px 0 #490f76 !important;border-bottom:1px solid #490f76 !important}#yog-hd,#yfi-hd,#ysp-hd,#hd,#yfimh,#yfi_hd,#yfi_fp_hd,#masthead,#yfi_nav_header #navigation,#y-nav #navigation,.ad_in_head{background-color:#fff;background-image:none}#header,#hd .yfi_doc,#y-hd .yfi_doc,#yfi_hd .yfi_doc{width:100% !important}#yucs{margin:0 auto;width:970px}#yfi_nav_header,.y-nav-legobg,#y-nav #navigation{margin:0 auto;width:970px}#yucs .yucs-avatar{height:22px;width:22px}#yucs #yucs-profile_text .yuhead-name-greeting{display:none}#yucs #yucs-profile_text .yuhead-name{top:0;max-width:65px}#yucs-profile_text{max-width:65px}#yog-bd .yom-stage{background:transparent}#yog-hd{height:84px}.yog-bd,.yog-grid{padding:4px 10px}.nav-stack ul.yog-grid{padding:0}#yucs #yucs-search.yucs-bbb .yucs-button_theme{background:-moz-linear-gradient(top, #01a5e1 0, #0297ce 100%);background:-webkit-gradient(linear, left top, left bottom, color-stop(0%, #01a5e1), color-stop(100%, #0297ce));background:-webkit-linear-gradient(top, #01a5e1 0, #0297ce 100%);background:-o-linear-gradient(top, #01a5e1 0, #0297ce 100%);background:-ms-linear-gradient(top, #01a5e1 0, #0297ce 100%);background:linear-gradient(to bottom, #01a5e1 0, #0297ce 100%);-webkit-box-shadow:inset 0 1px 3px 0 #01c0eb;box-shadow:inset 0 1px 3px 0 #01c0eb;background-color:#019ed8;background-color:transparent\0/IE9;background-color:transparent\9;*background:none;border:1px solid #595959;padding-left:0px;padding-right:0px}#yucs #yucs-search.yucs-bbb #yucs-prop_search_button_wrapper .yucs-gradient{filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#01a5e1', endColorstr='#0297ce',GradientType=0 );-ms-filter:""progid:DXImageTransform.Microsoft.gradient( startColorstr='#01a5e1', endColorstr='#0297ce',GradientType=0 )"";background-color:#019ed8\0/IE9}#yucs #yucs-search.yucs-bbb #yucs-prop_search_button_wrapper{*border:1px solid #595959}#yucs #yucs-search .yucs-button_theme{background:#0f8ed8;border:0;box-shadow:0 2px #044e6e}@media all{#yucs.yucs-mc,#yucs-top-inner{width:auto !important;margin:0 !important}#yucsHead{_text-align:left !important}#yucs-top-inner,#yucs.yucs-mc{min-width:970px !important;max-width:1240px !important;padding-left:10px !important;padding-right:10px !important}#yucs.yucs-mc{_width:970px !important;_margin:0 !important}#yucsHead #yucs .yucs-fl-left #yucs-search{position:absolute;left:190px !important;max-width:none !important;margin-left:0;_left:190px;_width:510px !important}.yog-ad-billboard #yucs-top-inner,.yog-ad-billboard #yucs.yucs-mc{max-width:1130px !important}#yucs .yog-cp{position:inherit}}#yucs #yucs-logo{width:150px !important;height:34px !important}#yucs #yucs-logo div{width:94px !important;margin:0 auto !important}.lt #yucs-logo div{background-position:-121px center !important}#yucs-logo a{margin-left:-13px !important}</style><style>#yog-hd .yom-bar, #yog-hd .yom-nav, #y-nav, #hd .ysp-full-bar,  #yfi_nav_header, #hd .mast {
float: none;
width: 970px;
margin: 0 auto;
}

#yog-bd .yom-stage {
background: transparent;
}

#y-nav .yom-nav {
padding-top: 0px;
}

#ysp-search-assist .bd {
display:none;
}

#ysp-search-assist h4 {
padding-left: 8px;
}


    #yfi-portfolios-multi-quotes #y-nav, #yfi-portfolios-multi-quotes #navigation, #yfi-portfolios-multi-quotes .y-nav-legobg, 
    #yfi-portfolios-my-portfolios #y-nav, #yfi-portfolios-my-portfolios #navigation, #yfi-portfolios-my-portfolios .y-nav-legobg {
        width : 100%;
    }</style> <div id=""yucsHead"" class=""yucs-finance yucs-en-us  yucs-standard""><!-- meta --><div id=""yucs-meta"" data-authstate=""signedout"" data-cobrand=""standard"" data-crumb="""" data-mc-crumb="""" data-gta="""" data-device=""desktop"" data-experience=""GS"" data-firstname="""" data-style="""" data-flight=""1463752577"" data-forcecobrand=""standard"" data-guid="""" data-host=""finance.yahoo.com"" data-https=""0"" data-languagetag=""en-us"" data-property=""finance"" data-protocol=""http"" data-shortfirstname="""" data-shortuserid="""" data-status=""active"" data-spaceid="""" data-test_id="""" data-userid="""" data-stickyheader=""true"" data-headercollapse="""" data-uh-test=""acctswitch"" ></div><!-- /meta --><div id=""yucs-comet"" style=""display:none;""></div><div id=""yucs-disclaimer"" class=""yucs-disclaimer yucs-activate yucs-hide yucs-property-finance yucs-fcb- "" data-cobrand=""standard"" data-cu = ""0"" data-dsstext=""Want a better search experience? {dssLink}Set your Search to Yahoo{linkEnd}"" data-dsstext-mobile=""Search Less, Find More"" data-dsstext-mobile-ok=""OK"" data-dsstext-mobile-set-search=""Set Search to Yahoo"" data-dssstbtext=""Yahoo is the preferred search engine for Firefox. Switch now."" data-dssstb-ok=""Yes"" data-dssstb-no=""Not Now"" data-ylt-link=""https://search.yahoo.com/searchset?pn="" data-ylt-dssbarclose=""/"" data-ylt-dssbaropen=""/"" data-ylt-dssstb-link=""https://downloads.yahoo.com/sp-firefox"" data-ylt-dssstbbarclose=""/"" data-ylt-dssstbbaropen=""/"" data-ylt-dssCookieCleanedSuccess=""/"" data-ylt-dssCookieCleanedFailed=""/"" data-linktarget=""_top"" data-lang=""en-us"" data-property=""finance"" data-device=""Desktop"" data-close-txt=""Close this window"" data-maybelater-txt = ""Maybe Later"" data-killswitch = ""0"" data-host=""finance.yahoo.com"" data-spaceid="""" data-pn="""" data-dss-cookie-cleanup="""" data-pn-en-ca-mobile-frontpage="""" data-pn-de-de-mobile-frontpage="""" data-pn-es-es-mobile-frontpage="""" data-pn-fr-fr-mobile-frontpage="""" data-pn-en-in-mobile-frontpage="""" data-pn-it-it-mobile-frontpage="""" data-pn-en-us-mobile-frontpage="""" data-pn-en-sg-mobile-frontpage="""" data-pn-en-gb-mobile-frontpage="""" data-pn-en-us-mobile-mail="""" data-pn-en-ca-mobile-mail="""" data-pn-de-de-mobile-mail="""" data-pn-es-es-mobile-mail="""" data-pn-fr-fr-mobile-mail="""" data-pn-en-in-mobile-mail="""" data-pn-it-it-mobile-mail="""" data-pn-en-sg-mobile-mail="""" data-pn-en-gb-mobile-mail="""" data-pn-pt-br-mobile-mail="""" data-pn-en-us-tablet-frontpage="""" data-pn-en-us-tablet-mail="""" data-pn-en-ca-tablet-mail="""" data-pn-de-de-tablet-mail="""" data-pn-es-es-tablet-mail="""" data-pn-fr-fr-tablet-mail="""" data-pn-en-in-tablet-mail="""" data-pn-it-it-tablet-mail="""" data-pn-en-sg-tablet-mail="""" data-pn-en-gb-tablet-mail="""" data-pn-pt-br-tablet-mail="""" data-news-search-yahoo-com="""" data-answers-search-yahoo-com="""" data-finance-search-yahoo-com="""" data-images-search-yahoo-com="""" data-video-search-yahoo-com="""" data-sports-search-yahoo-com="""" data-shopping-search-yahoo-com="""" data-shopping-yahoo-com="""" data-us-qa-trunk-news-search-yahoo-com ="""" data-dss=""1""></div>  <div id=""yucs-top-bar"" class='yucs-ps' ><div id='yucs-top-inner'><ul id=""yucs-top-list""><li id=""yucs-top-home""><a href=""https://www.yahoo.com/"" ><span class=""sp yucs-top-ico""></span>Home</a></li><li id=""yucs-top-mail""><a href=""https://mail.yahoo.com/?.intl=us&.lang=en-US&.src=ym"" >Mail</a></li><li id=""yucs-top-search""><a href=""https://search.yahoo.com/search"" >Search</a></li><li id=""yucs-top-news""><a href=""https://www.yahoo.com/news"" >News</a></li><li id=""yucs-top-sports""><a href=""http://sports.yahoo.com/"" >Sports</a></li><li id=""yucs-top-finance""><a href=""http://finance.yahoo.com/"" >Finance</a></li><li id=""yucs-top-celebrity""><a href=""https://celebrity.yahoo.com/"" >Celebrity</a></li><li id=""yucs-top-weather""><a href=""https://weather.yahoo.com/"" >Weather</a></li><li id=""yucs-top-answers""><a href=""https://answers.yahoo.com/"" >Answers</a></li><li id=""yucs-top-flickr""><a href=""https://www.flickr.com/"" >Flickr</a></li><li id=""yucs-top-mobile""><a href=""https://mobile.yahoo.com/"" >Mobile</a></li><li id='yucs-more' class='yucs-menu yucs-more-activate' data-ylt=""/""><a href=""http://everything.yahoo.com/"" id='yucs-more-link'>More<span class=""sp yucs-top-ico""></span></a><div id='yucs-top-menu'><div class=""yui3-menu-content""><ul class=""yucs-hide yucs-leavable""><li id='yucs-top-politics'><a href=""https://www.yahoo.com/politics"" >Politics</a></li><li id='yucs-top-movies'><a href=""https://www.yahoo.com/movies"" >Movies</a></li><li id='yucs-top-music'><a href=""https://www.yahoo.com/music"" >Music</a></li><li id='yucs-top-tv'><a href=""https://www.yahoo.com/tv"" >TV</a></li><li id='yucs-top-groups'><a href=""https://groups.yahoo.com/"" >Groups</a></li><li id='yucs-top-style'><a href=""https://www.yahoo.com/style"" >Style</a></li><li id='yucs-top-beauty'><a href=""https://www.yahoo.com/beauty"" >Beauty</a></li><li id='yucs-top-tech'><a href=""https://www.yahoo.com/tech"" >Tech</a></li><li id='yucs-top-shopping'><a href=""http://shopping.yahoo.com/"" >Shopping</a></li></ul></div></div></li></ul></div><style>#yucs-top-ff-promo { position:absolute; right:0; right:auto\9; left:950px\9; right:0\9\0; left:auto\9\0; margin-left:18px;}#yucs-top-ff-promo a span { text-decoration: none; display: inline-block;}@media screen and (max-width: 1124px) { #UH.yucs-mail #yucs-top-ff-promo { display:none; }}@media screen and (max-width:1150px) { #yucs-top-ff-promo { right:auto; }}</style><li id='yucs-top-ff-promo' class=""D(ib) Zoom Va(t) Mend(18px) Pstart(14px) D(n)""><a class=""D(b) C(#fff)! Pstart(4px) Td(n)! Td(u)!:h Fz(13px)"" href=""https://www.mozilla.org/firefox/new/?utm_source=yahoo&utm_medium=referral&utm_campaign=y-uh&utm_content=y-finance-try"" data-ylk=""t5:ff-promo;slk:ff-promo;t4:pty-mu;"" target=""_blank""><img id=""yucs-ff-img"" class=""Pend(4px) yucs_Va(m)"" src='https://s.yimg.com/kx/yucs/uh3s/promo-ff/1/images/ff_icon-compressed.png' width=""15"" height=""15"" alt=""Firefox"" />Try Yahoo Finance on Firefox<span>&nbsp;&raquo;</span></a></li><script> var s = false, ts,re2,sdts,v2= null, cookies = ""; "" + document.cookie, dss = cookies.split(""; DSS=""), m, ua = window.navigator.userAgent.toLowerCase(); m = ua.match(/firefox\/(\d+)/); if (!m || (m && m[1] && parseInt(m[1]) < 34)) { if (ua.indexOf('version') >= 0 && ua.indexOf('crios') < 0) { s = true; } if (!!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0) { s = true; } if (dss && dss.length === 2) { re2 = new RegExp('sdts=(\\d+)'); v2 = re2.exec(dss[1]); if (v2 && v2.length === 2) { sdts = v2[1]; } if (sdts && (parseInt(new Date().getTime()/1000) - sdts) < 604800) { s = true; } } if (!s) { m = document.getElementById('yucs-top-ff-promo'); m.className = m.className.replace(/D\(n\)/g,''); } }</script></div><div id=""yucs"" class=""yucs yucs-mc  yog-grid"" data-lang=""en-us"" data-property=""finance"" data-flight=""1463752577"" data-linktarget=""_top"" data-uhvc=""/""> <div class=""yucs-fl-left yog-cp"">   <div id=""yucs-logo""> <style> #yucs #yucs-logo-ani { width:120px ; height:34px; background-image:url(https://s.yimg.com/rz/l/yahoo_finance_en-US_f_pw_119x34.png) ; _background-image:url(https://s.yimg.com/rz/l/yahoo_finance_en-US_f_pw_119x34.gif) ; *left: 0px; display:block ; visibility: visible; position: relative; clip: auto; } .lt #yucs-logo-ani { background-position: 100% 0px !important; } .lt #yucs[data-property='mail'] #yucs-logo-ani { background-position: -350px 0px !important; } #yucs-logo { margin-top:0px!important; padding-top: 11px; width: 120px; } #yucs[data-property='homes'] #yucs-logo { width: 102px; } .advisor #yucs-link-ani { left: 21px !important; } #yucs #yucs-logo a {margin-left: 0!important;}#yucs #yucs-link-ani {width: 100% !important;} @media only screen and (-webkit-min-device-pixel-ratio: 2), only screen and ( min--moz-device-pixel-ratio: 2), only screen and ( -o-min-device-pixel-ratio: 2/1), only screen and ( min-device-pixel-ratio: 2), only screen and ( min-resolution: 192dpi), only screen and ( min-resolution: 2dppx) { #yucs #yucs-logo-ani { background-image: url(https://s.yimg.com/rz/l/yahoo_finance_en-US_f_pw_119x34_2x.png) !important; background-size: 235px 34px; } } </style> <div> <a id=""yucs-logo-ani"" class="""" href=""http://finance.yahoo.com"" target=""_top"" data-alg=""""> Yahoo Finance </a> </div> <img id=""imageCheck"" src=""http://l.yimg.com/os/mit/media/m/base/images/transparent-1093278.png"" alt=""""/> </div><noscript><style>#yucs #yucs-logo-ani {visibility: visible;position: relative;clip: auto;}</style></noscript> <div id=""yucs-search"" style=""width: 570px; display: block;"" class=' yucs-search-activate'> <form role=""search"" class=""yucs-search yucs-activate"" target=""_top"" data-webaction=""https://search.yahoo.com/search"" action=""http://finance.yahoo.com/q"" method=""get""> <table role=""presentation""> <tbody role=""presentation""> <tr role=""presentation""> <td class=""yucs-form-input"" role=""presentation""> <input autocomplete=""off"" class=""yucs-search-input"" name=""s"" type=""search"" aria-describedby=""mnp-search_box"" data-yltvsearch=""http://finance.yahoo.com/q"" data-yltvsearchsugg=""/"" data-satype=""mini"" data-gosurl=""https://s.yimg.com/aq/autoc"" data-pubid=""666"" data-enter-ylt=""http://finance.yahoo.com/q"" data-enter-fr="""" data-maxresults="""" id=""mnp-search_box"" data-rapidbucket=""""/> </td><td NOWRAP class=""yucs-form-btn"" role=""presentation""><div id=""yucs-prop_search_button_wrapper"" class=""yucs-search-buttons""><div class=""yucs-shadow""><div class=""yucs-gradient""></div></div><button id=""yucs-sprop_button"" class=""yucs-action_btn yucs-button_theme yucs-vsearch-button"" type=""submit"" data-vfr=""uh3_finance_vert_gs"" onclick=""var vfr = this.getAttribute('data-vfr'); if(vfr){document.getElementById('fr').value = vfr}"" data-vsearch=""http://finance.yahoo.com/q"">Search Finance</button></div><div id=""yucs-web_search_button_wrapper"" class=""yucs-search-buttons""><div class=""yucs-shadow""><div class=""yucs-gradient""></div></div><button id=""yucs-search_button"" class=""yucs-action_btn yucs-wsearch-button"" onclick=""var form=document.getElementById('yucs-search').children[0];var wa=form.getAttribute('data-webaction');form.setAttribute('action',wa);var searchbox=document.getElementById('mnp-search_box');searchbox.setAttribute('name','p');"" type=""submit"">Search Web</button></div></td></tr> </tbody> </table> <input type=""hidden"" id=""uhb"" name=""uhb"" value=""uhb2"" /> <input type=""hidden"" id=""fr"" name=""fr"" value=""uh3_finance_web_gs"" />   </form><div id=""yucs-satray"" class=""sa-tray sa-hidden"" data-wstext=""Search Web for: "" data-wsearch=""https://search.yahoo.com/search"" data-vfr=""uh3_finance_vert_gs"" data-vsearchAll=""/"" data-vsearch=""http://finance.yahoo.com/q"" data-vstext= ""Search news for: "" data-vert_fin_search=""https://finance.search.yahoo.com/search/""></div> </div></div><div class=""yucs-fl-right""> <div id=""yucs-profile"" class=""yucs-profile yucs-signedout""> <a id=""yucs-menu_link_profile_signed_out"" href=""https://login.yahoo.com/config/login?.src=quote&.intl=us&.lang=en-US&.done=http://finance.yahoo.com/q/bs%3fs=YHOO%26annual"" target=""_top"" rel=""nofollow"" class=""sp yucs-fc"" aria-label=""Profile""> </a> <div id=""yucs-profile_text"" class=""yucs-fc""> <a id=""yucs-login_signIn"" href=""https://login.yahoo.com/config/login?.src=quote&.intl=us&.lang=en-US&.done=http://finance.yahoo.com/q/bs%3fs=YHOO%26annual"" target=""_top"" rel=""nofollow"" class=""yucs-fc""> Sign in </a> </div></div><div class=""yucs-mail_link yucs-mailpreview-ancestor""><a id=""yucs-mail_link_id"" class=""sp yltasis yucs-fc"" href=""https://mail.yahoo.com/?.intl=us&.lang=en-US&.src=ym"" rel=""nofollow"" target=""_top""> Mail <span class=""yucs-activate yucs-mail-count yucs-hide yucs-alert-count-con"" data-uri-scheme=""https"" data-uri-path=""mg.mail.yahoo.com/mailservices/v1/newmailcount"" data-authstate=""signedout"" data-crumb="""" data-mc-crumb=""""><span class=""yucs-alert-count""></span></span></a><div class=""yucs-mail-preview-panel yucs-menu yucs-hide"" data-mail-txt=""Mail"" data-uri-scheme=""http"" data-uri-path=""ucs.query.yahoo.com/v1/console/yql"" data-mail-view=""Go to Mail"" data-mail-help-txt=""Help"" data-mail-help-url=""http://help.yahoo.com/l/us/yahoo/mail/ymail/"" data-mail-loading-txt=""Loading..."" data-languagetag=""en-us"" data-mrd-crumb="""" data-authstate=""signedout"" data-middleauth-signin-text=""Click here to view your mail"" data-popup-login-url=""https://login.yahoo.com/config/login_verify2?.pd=c%3DOIVaOGq62e5hAP8Tv..nr5E3&.src=sc"" data-middleauthtext=""You have {count} new messages."" data-yltmessage-link=""https://mrd.mail.yahoo.com/msg?mid={msgID}&fid=Inbox&src=uh&.crumb="" data-yltviewall-link=""https://mail.yahoo.com/"" data-yltpanelshown=""/"" data-ylterror=""/"" data-ylttimeout=""/"" data-generic-error=""We're unable to preview your mail.<br>Go to Mail."" data-nosubject=""[No Subject]"" data-timestamp='short'></div></div> <div id=""yucs-help"" class=""yucs-activate yucs-help yucs-menu_nav""> <a id=""yucs-help_button"" class=""sp yltasis"" href=""javascript:void(0);"" aria-label=""Help"" rel=""nofollow""> <em class=""yucs-hide yucs-menu_anchor"">Help</em> </a> <div id=""yucs-help_inner"" class=""yucs-hide yucs-menu yucs-hm-activate"" data-yltmenushown=""/""> <span class=""sp yucs-dock""></span> <ul id=""yuhead-help-panel""> <li><a class=""yucs-acct-link"" href=""https://login.yahoo.com/account/personalinfo?.intl=us&.lang=en-US&.done=http://finance.yahoo.com/q/bs%3fs=YHOO%26annual&amp;.src=quote&amp;.intl=us&amp;.lang=en-US"" target=""_top"">Account Info</a></li> <li><a href=""https://help.yahoo.com/l/us/yahoo/finance/"" rel=""nofollow"" >Help</a></li> <span class=""yucs-separator"" role=""presentation"" style=""display: block;""></span><li><a href=""http://feedback.yahoo.com/forums/207809"" rel=""nofollow"" >Suggestions</a></li>  </ul> </div></div>  <div id=""yucs-network_link""><a id=""yucs-home_link"" href=""https://www.yahoo.com/"" rel=""nofollow"" target=""_top""><em class=""sp"">Yahoo</em><span class=""yucs-fc"">Home</span></a></div>  <!-- empty breaking news component -->     </div>    </div> <div id=""yucs-location-js"" class=""yucs-hide yucs-offscreen yucs-location-activate"" data-appid=""yahoo.locdrop.ucs.desktop"" data-crumb=""""><!-- empty for ie --></div><div id=""yUnivHead"" class=""yucs-hide""><!-- empty --></div><div id=""yhelp_container"" class=""yui3-skin-sam""></div></div><!-- alert --><!-- /alert --></div><script type=""text/javascript"">
		var yfi_dd = 'finance.yahoo.com';
		
			ll_js.push({
			'file':'http://l.yimg.com/zz/combo?kx/yucs/uh3/uh/1078/js/uh-min.js&kx/yucs/uh3/uh/1078/js/gallery-jsonp-min.js&kx/yucs/uh3/uh/1134/js/menu_utils_v3-min.js&kx/yucs/uh3/uh/1078/js/localeDateFormat-min.js&kx/yucs/uh3/uh/1078/js/timestamp_library_v2-min.js&kx/yucs/uh3/uh/1104/js/logo_debug-min.js&kx/yucs/uh3/switch-theme/6/js/switch_theme-min.js&kx/yucs/uhc/meta/66/js/meta-min.js&kx/yucs/uh_common/beacon/18/js/beacon-min.js&kx/yucs/uh2/comet/84/js/cometd-yui3-min.js&kx/yucs/uh2/comet/84/js/conn-min.js&kx/yucs/uh2/comet/84/js/dark-test-min.js&kx/yucs/uh3/disclaimer/388/js/disclaimer_seed-min.js&kx/yucs/uh3/top-bar/321/js/top_bar_v3-min.js&kx/yucs/uh3/search/598/js/search-min.js&kx/yucs/uh3/search/611/js/search_plugin-min.js&kx/yucs/uh2/common/145/js/jsonp-super-cached-min.js&kx/yucs/uh3/avatar/40/js/avatar-min.js&kx/yucs/uh3/mail-link/96/js/mailcount_ssl-min.js&kx/yucs/uh3/help/83/js/help_menu_v3-min.js&kx/yucs/uhc/rapid/49/js/uh_rapid-min.js&kx/yucs/uh3/get-the-app/148/js/inputMaskClient-min.js&kx/yucs/uh3/get-the-app/160/js/get_the_app-min.js&kx/yucs/uh3/location/10/js/uh_locdrop-min.js'
			});
		
              if(window.LH) {
                  LH.init({spaceid:'95941443'});
              }
	</script><style>
	.yfin_gs #yog-hd{
	    position: relative;
	}
        html {
            padding-top: 0 !important;
        }

        @media screen and (min-width: 806px) {
                html {
                    padding-top:83px !important;
                }
                .yfin_gs #yog-hd{
                    position: fixed;
                }
        }
        /* bug 6768808 - allow UH scrolling for smartphone */
        @media  only screen and (device-width: 320px) and (device-height: 480px) and (-webkit-device-pixel-ratio: 2),
            only screen and (device-width: 320px) and (device-height: 568px) and (-webkit-min-device-pixel-ratio: 1),
            only screen and (device-width: 320px) and (device-height: 534px) and (-webkit-device-pixel-ratio: 1.5),
            only screen and (device-width: 720px) and (device-height: 1280px) and (-webkit-min-device-pixel-ratio: 2),
            only screen and (device-width: 480px) and (device-height: 800px) and (-webkit-device-pixel-ratio: 1.5 ),
            only screen and (device-width: 384px) and (device-height: 592px) and (-webkit-device-pixel-ratio: 2),
            only screen and (device-width: 360px) and (device-height: 640px) and (-webkit-device-pixel-ratio: 3){
                html {
                    padding-top: 0 !important;
                }
                .yfin_gs #yog-hd{
                    position: relative;
                    border-bottom: 0 none !important;
                    box-shadow: none !important;
                }
            }
        </style><script type=""text/javascript"">
	YUI().use('node',function(Y){
	    var gs_hdr_elem = Y.one('.yfin_gs #yog-hd');
	    var _window = Y.one('window');
	    
	    if(gs_hdr_elem) {
		_window.on('scroll', function() {
		    var scrollTop   = _window.get('scrollTop');

		    if (scrollTop === 0 ) {
			gs_hdr_elem.removeClass('yog-hd-border');
		    }
		    else {
			gs_hdr_elem.addClass('yog-hd-border');
		    }
		});
	    }
	});
	</script><script type=""text/javascript"">
                if(typeof YAHOO == ""undefined""){YAHOO={};}
                if(typeof YAHOO.Finance == ""undefined""){YAHOO.Finance={};}
                if(typeof YAHOO.Finance.ComscoreConfig == ""undefined""){YAHOO.Finance.ComscoreConfig={};}
                YAHOO.Finance.ComscoreConfig={ ""config"" : [
                        {
                        c5: ""95941443"",
                        c7: ""http://finance.yahoo.com/q/bs?s=YHOO&annual""
                        }
                ] }
                

                ll_js.push({
                            'file': 'bm/lib/fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_comscore.js',
                            'combo' : '1'
                });
        </script><noscript><img src=""http://b.scorecardresearch.com/p?c1=2&amp;c2=7241469&amp;c4=http://finance.yahoo.com/q/bs?s=YHOO&amp;annual&amp;c5=95941443&amp;cv=2.0&amp;cj=1""></noscript></div></div><div class=""ad_in_head""><div class=""yfi_doc""></div><table width=""752"" id=""yfncmkttme"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td></td></tr></table></div><div id=""y-nav""><div id=""navigation"" class=""yom-mod yom-nav"" role=""navigation""><div class=""bd""><div class=""nav""><div class=""y-nav-pri-legobg""><div id=""y-nav-pri"" class=""nav-stack nav-0 yfi_doc""><ul class=""yog-grid"" id=""y-main-nav""><li id=""finance%2520home""><div class=""level1""><a href=""/""><span>Finance Home</span></a></div></li><li class=""nav-fin-portfolios""><div id=""y-my-portfolios"" class=""level1""><a id=""yfmya"" href=""/portfolios.html""><span>My Portfolio</span></a><div class=""arrow""><em></em></div><div class=""pointer""></div><ul class=""nav-sub""><li><a href=""/portfolios/"">Sign in to access My Portfolios</a></li><li><a href=""http://billing.finance.yahoo.com/realtime_quotes/signup?.src=quote&amp;.refer=nb"">Free trial of Real-Time Quotes</a></li></ul></div></li><li id=""my%2520quotes%2520news""><div class=""level1""><a href=""/my-quotes-news/""><span>My Quotes News</span></a></div></li><li id=""market%2520data""><div class=""level1""><a href=""/market-overview/""><span>Market Data</span></a><div class=""arrow""><em></em></div><div class=""pointer""></div><ul class=""nav-sub""><li><a href=""/stock-center/"">Stocks</a></li><li><a href=""/funds/"">Mutual Funds</a></li><li><a href=""/options/"">Options</a></li><li><a href=""/etf/"">ETFs</a></li><li><a href=""/bonds"">Bonds</a></li><li><a href=""/futures"">Commodities</a></li><li><a href=""/currency-investing"">Currencies</a></li><li><a href=""http://biz.yahoo.com/research/earncal/today.html"">Calendars</a></li></ul></div></li><li id=""yahoo%2520originals""><div class=""level1""><a href=""/yahoofinance/""><span>Yahoo Originals</span></a><div class=""arrow""><em></em></div><div class=""pointer""></div><ul class=""nav-sub""><li><a href=""/brklivestream/"">Berkshire Hathaway</a></li><li><a href=""/news/marketmovers/"">Market Movers</a></li><li><a href=""/news/middaymovers/"">Midday Movers</a></li><li><a href=""/news/thefinalround"">The Final Round</a></li><li><a href=""/news/sportsbook"">Sportsbook</a></li><li><a href=""/yahoofinance/business/"">Business</a></li><li><a href=""/yahoofinance/investing"">Investing</a></li><li><a href=""/yahoofinance/personalfinance"">Personal Finance</a></li><li><a href=""/blogs/author/andy-serwer"">Andy Serwer, Editor</a></li><li><a href=""https://finance.yahoo.com/blogs/author/sam-ro/"">Sam Ro</a></li><li><a href=""/blogs/author/rick-newman/"">Rick Newman</a></li><li><a href=""/blogs/author/mandi-woodruff/"">Mandi Woodruff</a></li><li><a href=""/blogs/author/daniel-roberts/"">Daniel Roberts</a></li><li><a href=""/blogs/author/nicole-sinclair/"">Nicole Sinclair</a></li><li><a href=""/blogs/author/melody-hahm-20151026/"">Melody Hahm</a></li><li><a href=""/blogs/author/brittany-jones-cooper/"">Brittany Jones-Cooper</a></li><li><a href=""http://finance.yahoo.com/blogs/author/julia-la-roche/"">Julia La Roche</a></li></ul></div></li><li id=""business%2520%2526%2520finance""><div class=""level1""><a href=""/news/""><span>Business &amp; Finance</span></a><div class=""arrow""><em></em></div><div class=""pointer""></div><ul class=""nav-sub""><li><a href=""/corporate-news/"">Company News</a></li><li><a href=""/economic-policy-news/"">Economic News</a></li><li><a href=""/investing-news/"">Market News</a></li></ul></div></li><li id=""personal%2520finance""><div class=""level1""><a href=""/personal-finance/""><span>Personal Finance</span></a><div class=""arrow""><em></em></div><div class=""pointer""></div><ul class=""nav-sub""><li><a href=""/career-education/"">Career & Education</a></li><li><a href=""/real-estate/"">Real Estate</a></li><li><a href=""/retirement/"">Retirement</a></li><li><a href=""/credit-debt/"">Credit & Debt</a></li><li><a href=""https://taxes.yahoo.com/"">Taxes</a></li><li><a href=""/lifestyle/"">Health & Lifestyle</a></li><li><a href=""/videos/"">Featured Videos</a></li><li><a href=""/rates/"">Rates in Your Area</a></li><li><a href=""/calculator/index/"">Calculators</a></li></ul></div></li><li id=""cnbc""><div class=""level1""><a href=""/cnbc/""><span>CNBC</span></a></div></li><li id=""contributors""><div class=""level1""><a href=""/contributors/""><span>Contributors</span></a></div></li></ul></div></div>      </div></div></div><script>
        
        (function(el) {
            function focusHandler(e){
                if (e && e.target){
                    e.target == document ? null : e.target;
                    document.activeElement = e.target;
                }
            }
            // Handle !IE browsers that do not support the .activeElement property
            if(!document.activeElement){
                if (document.addEventListener){ 
                    document.addEventListener(""focus"", focusHandler, true);
                }
            }
        })(document);
        
        </script><div class=""y-nav-legobg""><div class=""yfi_doc""><div id=""y-nav-sec"" class=""clear""><div id=""searchQuotes"" class=""ticker-search mod"" mode=""search""><div class=""hd""></div><div class=""bd""><form id=""quote"" name=""quote"" action=""/q""><h2 class=""yfi_signpost"">Search for share prices</h2><label for=""txtQuotes"">Search for share prices</label><input id=""txtQuotes"" name=""s"" value="""" type=""text"" autocomplete=""off"" placeholder=""Enter Symbol""><input id=""get_quote_logic_opt"" name=""ql"" value=""1"" type=""hidden"" autocomplete=""off""><div id=""yfi_quotes_submit""><span><span><span><input type=""submit"" value=""Look Up"" id=""btnQuotes"" class=""rapid-nf""></span></span></span></div></form></div><div class=""ft""><a href=""http://finance.search.yahoo.com?fr=fin-v1"">Finance Search</a><!-- this is required only for the yoda pages, /page/settings/@page is available only on yoda pages --><div class=""report_issue""><a href=""https://yahoo.uservoice.com/forums/207809-finance-gs/category/68435-data-accuracy"" class=""Fz-xs"" target=""_blank"">Report an Issue</a></div><div class=""mkt_time""><p><span id=""yfs_market_time"">Fri, May 20, 2016, 9:56AM EDT - U.S. Markets close in 6 hrs 4 mins</span></p></div></div></div></div></div></div></div><div id=""screen""><span id=""yfs_ad_"" ></span><style type=""text/css"">
     .yfi-price-change-up{
        color:#008800
     }
     
    .yfi-price-change-down{
        color:#cc0000
    }
    </style><div id=""marketindices"">&nbsp;<a href=""/q?s=%5EDJI"">Dow</a>&nbsp;<span id=""yfs_pp0_^dji""><img width=""10"" height=""14"" style=""margin-right:-2px;"" border=""0"" src=""http://l.yimg.com/os/mit/media/m/base/images/transparent-1093278.png"" class=""pos_arrow"" alt=""Up""><b style=""color:#008800;"">0.60%</b></span>&nbsp;<a href=""/q?s=%5EIXIC"">Nasdaq</a>&nbsp;<span id=""yfs_pp0_^ixic""><img width=""10"" height=""14"" style=""margin-right:-2px;"" border=""0"" src=""http://l.yimg.com/os/mit/media/m/base/images/transparent-1093278.png"" class=""pos_arrow"" alt=""Up""><b style=""color:#008800;"">0.97%</b></span></div><div id=""companynav""><table cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td height=""5""></td></tr></table></div><div id=""leftcol""><div id=""yfi_investing_nav""><div class=""hd""><h2>More On YHOO</h2></div><div class=""bd""><h3>Quotes</h3><ul><li><a href=""/q?s=YHOO"">Summary</a></li><li><a href=""/q/ecn?s=YHOO+Order+Book"">Order Book</a></li><li><a href=""/q/op?s=YHOO+Options"">Options</a></li><li><a href=""/q/hp?s=YHOO+Historical+Prices"">Historical Prices</a></li></ul><h3>Charts</h3><ul><li><a href=""/echarts?s=YHOO+Interactive#symbol=YHOO;range="">Interactive</a></li></ul><h3>News &amp; Info</h3><ul><li><a href=""/q/h?s=YHOO+Headlines"">Headlines</a></li><li><a href=""/q/p?s=YHOO+Press+Releases"">Press Releases</a></li><li><a href=""/q/ce?s=YHOO+Company+Events"">Company Events</a></li><li><a href=""/mb/YHOO/"">Message Boards</a></li><li><a href=""/marketpulse/YHOO"">Market Pulse</a></li></ul><h3>Company</h3><ul><li><a href=""/q/pr?s=YHOO+Profile"">Profile</a></li><li><a href=""/q/ks?s=YHOO+Key+Statistics"">Key Statistics</a></li><li><a href=""/q/sec?s=YHOO+SEC+Filings"">SEC Filings</a></li><li><a href=""/q/co?s=YHOO+Competitors"">Competitors</a></li><li><a href=""/q/in?s=YHOO+Industry"">Industry</a></li><li><a href=""/q/ct?s=YHOO+Components"">Components</a></li></ul><h3>Analyst Coverage</h3><ul><li><a href=""/q/ao?s=YHOO+Analyst+Opinion"">Analyst Opinion</a></li><li><a href=""/q/ae?s=YHOO+Analyst+Estimates"">Analyst Estimates</a></li></ul><h3>Ownership</h3><ul><li><a href=""/q/mh?s=YHOO+Major+Holders"">Major Holders</a></li><li><a href=""/q/it?s=YHOO+Insider+Transactions"">Insider Transactions</a></li><li><a href=""/q/ir?s=YHOO+Insider+Roster"">Insider Roster</a></li></ul><h3>Financials</h3><ul><li><a href=""/q/is?s=YHOO+Income+Statement&amp;annual"">Income Statement</a></li><li class=""selected"">Balance Sheet</li><li><a href=""/q/cf?s=YHOO+Cash+Flow&amp;annual"">Cash Flow</a></li></ul></div><div class=""ft""></div></div></div><div id=""rightcol""><table border=""0"" cellspacing=""0"" cellpadding=""0"" width=""580"" id=""yfncbrobtn"" style=""padding-top:1px;""><tr><td align=""center""><!-- SpaceID=0 robot -->
</td></tr></table><br><div class=""rtq_leaf""><div class=""rtq_div""><div class=""yui-g""><div class=""yfi_rt_quote_summary"" id=""yfi_rt_quote_summary""><div class=""hd""><div class=""title""><h2>Yahoo! Inc. (YHOO)</h2> <span class=""rtq_exch""><span class=""rtq_dash"">-</span>NasdaqGS  </span><span class=""wl_sign""></span></div></div><div class=""yfi_rt_quote_summary_rt_top sigfig_promo_1""><div> <span class=""time_rtq_ticker""><span id=""yfs_l84_yhoo"">36.13</span></span> <span class=""down_r time_rtq_content""><span id=""yfs_c63_yhoo""><img width=""10"" height=""14"" style=""margin-right:-2px;"" border=""0"" src=""http://l.yimg.com/os/mit/media/m/base/images/transparent-1093278.png"" class=""neg_arrow"" alt=""Down"">   0.89</span><span id=""yfs_p43_yhoo"">(2.40%)</span> </span><span class=""time_rtq""> <span id=""yfs_t53_yhoo""><span id=""yfs_t53_yhoo"">9:56AM EDT</span></span></span> - Nasdaq Real Time Price</div><div></div></div><div id=""yfi_toolbox_mini_rtq""><span class=""fb-like-button""></span><script type=""text/javascript"">
                            ll_js.push({
                                'script':function(){
                                    YUI().use('node', function (Y) {
                                        var iframe = Y.Node.create('<iframe src=""http://www.facebook.com/plugins/like.php?href=http://finance.yahoo.com%2Fq%3Fs%3DYHOO&amp;layout=button_count&amp;show_faces=false&amp;action=like&amp;font=arial&amp;colorscheme=light&amp;height=21&amp;width=100&amp;locale=en_US"" scrolling=""no"" frameborder=""0"" allowTransparency=""true"" style=""height:21px; width:95px;""></iframe>');
                                        Y.one("".fb-like-button"").append(iframe);
                                    });
                                }
                            });
                        </script></div></div></div></div></div><table width=""580"" id=""yfncsumtab"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td colspan=""3""><table border=""0"" cellspacing=""0"" class=""yfnc_modtitle1"" style=""border-bottom:1px solid #dcdcdc; margin-bottom:10px; width:100%;""><form action=""/q/bs"" accept-charset=""utf-8""><tr><td><big><b>Balance Sheet</b></big></td><td align=""right""><small><b>Get Balance Sheet for:</b> <input name=""s"" id=""pageTicker"" size=""10"" /></small><input id=""get_quote_logic_opt"" name=""ql"" value=""1"" type=""hidden"" autocomplete=""off""><input value=""GO"" type=""submit"" style=""margin-left:3px;"" class=""rapid-nf""></td></tr></form></table></td></tr><tr><td><table border=""0"" cellpadding=""2"" cellspacing=""1"" width=""100%""><tr><td>View: <strong>Annual Data</strong> | <a href=""/q/bs?s=YHOO"">Quarterly Data</a></td><td align=""right""><SMALL>All numbers in thousands</SMALL></td></tr></table><TABLE class=""yfnc_tabledata1"" width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><TR><TD><TABLE width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0""><TR><TD class=""yfnc_modtitle1"" colspan=""2""><small><span class=""yfi-module-title"">Period Ending</span></small></TD><TD class=""yfnc_modtitle1"" align=""right""><b>Dec 31, 2015</b></TD><TD class=""yfnc_modtitle1"" align=""right""><b>Dec 31, 2014</b></TD><TD class=""yfnc_modtitle1"" align=""right""><b>Dec 31, 2013</b></TD></TR><tr><td colspan=""5"" style=""height:0;padding:0; ""><span style=""display:block; width:5px; height:10px;"" ></span></td></tr><tr><td class=""yfnc_d"" colspan=""5""><strong>Assets</strong></td></tr><tr><td colspan=""5"">Current Assets</td></tr><tr>
                    <td width=""30"" class=""yfnc_tabledata1""><spacer type=""block"" width=""30"" height=""1""/></td>
                <td>Cash And Cash Equivalents</td><td align=""right"">1,631,911&nbsp;&nbsp;</td><td align=""right"">2,664,098&nbsp;&nbsp;</td><td align=""right"">2,077,590&nbsp;&nbsp;</td></tr><tr>
                    <td width=""30"" class=""yfnc_tabledata1""><spacer type=""block"" width=""30"" height=""1""/></td>
                <td>Short Term Investments</td><td align=""right"">4,225,112&nbsp;&nbsp;</td><td align=""right"">5,327,412&nbsp;&nbsp;</td><td align=""right"">1,330,304&nbsp;&nbsp;</td></tr><tr>
                    <td width=""30"" class=""yfnc_tabledata1""><spacer type=""block"" width=""30"" height=""1""/></td>
                <td>Net Receivables</td><td align=""right"">1,047,504&nbsp;&nbsp;</td><td align=""right"">1,032,704&nbsp;&nbsp;</td><td align=""right"">979,559&nbsp;&nbsp;</td></tr><tr>
                    <td width=""30"" class=""yfnc_tabledata1""><spacer type=""block"" width=""30"" height=""1""/></td>
                <td>Inventory</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td></tr><tr>
                    <td width=""30"" class=""yfnc_tabledata1""><spacer type=""block"" width=""30"" height=""1""/></td>
                <td>Other Current Assets</td><td align=""right"">602,792&nbsp;&nbsp;</td><td align=""right"">420,207&nbsp;&nbsp;</td><td align=""right"">638,404&nbsp;&nbsp;</td></tr><tr><td colspan=""5""  style=""height:0;padding:0; border-top:3px solid #333;""><span style=""display:block; width:5px; height:1px;"" ></span></td></tr><tr><td colspan=""2"">
                            <strong>
                        Total Current Assets
                            </strong>
                        </td><td align=""right"">
                                <strong>
                            7,507,319&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            9,444,421&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            5,025,857&nbsp;&nbsp;
                                </strong>
                            </td></tr><tr><td colspan=""2"">Long Term Investments</td><td align=""right"">34,651,551&nbsp;&nbsp;</td><td align=""right"">44,588,259&nbsp;&nbsp;</td><td align=""right"">5,015,847&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Property Plant and Equipment</td><td align=""right"">1,547,323&nbsp;&nbsp;</td><td align=""right"">1,487,684&nbsp;&nbsp;</td><td align=""right"">1,488,518&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Goodwill</td><td align=""right"">808,114&nbsp;&nbsp;</td><td align=""right"">5,152,570&nbsp;&nbsp;</td><td align=""right"">4,679,648&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Intangible Assets</td><td align=""right"">347,269&nbsp;&nbsp;</td><td align=""right"">470,842&nbsp;&nbsp;</td><td align=""right"">417,808&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Accumulated Amortization</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td></tr><tr><td colspan=""2"">Other Assets</td><td align=""right"">342,390&nbsp;&nbsp;</td><td align=""right"">563,560&nbsp;&nbsp;</td><td align=""right"">177,281&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Deferred Long Term Asset Charges</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td></tr><tr><td colspan=""5""  style=""height:0;padding:0; border-top:3px solid #333;""><span style=""display:block; width:5px; height:1px;"" ></span></td></tr><tr><td colspan=""2"">
                            <strong>
                        Total Assets
                            </strong>
                        </td><td align=""right"">
                                <strong>
                            45,203,966&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            61,707,336&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            16,804,959&nbsp;&nbsp;
                                </strong>
                            </td></tr><tr><td colspan=""5"" style=""height:0;padding:0; ""><span style=""display:block; width:5px; height:10px;"" ></span></td></tr><tr><td class=""yfnc_d"" colspan=""5""><strong>Liabilities</strong></td></tr><tr><td colspan=""5"">Current Liabilities</td></tr><tr>
                    <td width=""30"" class=""yfnc_tabledata1""><spacer type=""block"" width=""30"" height=""1""/></td>
                <td>Accounts Payable</td><td align=""right"">1,143,349&nbsp;&nbsp;</td><td align=""right"">4,178,020&nbsp;&nbsp;</td><td align=""right"">1,045,813&nbsp;&nbsp;</td></tr><tr>
                    <td width=""30"" class=""yfnc_tabledata1""><spacer type=""block"" width=""30"" height=""1""/></td>
                <td>Short/Current Long Term Debt</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td></tr><tr>
                    <td width=""30"" class=""yfnc_tabledata1""><spacer type=""block"" width=""30"" height=""1""/></td>
                <td>Other Current Liabilities</td><td align=""right"">134,031&nbsp;&nbsp;</td><td align=""right"">336,963&nbsp;&nbsp;</td><td align=""right"">294,499&nbsp;&nbsp;</td></tr><tr><td colspan=""5""  style=""height:0;padding:0; border-top:3px solid #333;""><span style=""display:block; width:5px; height:1px;"" ></span></td></tr><tr><td colspan=""2"">
                            <strong>
                        Total Current Liabilities
                            </strong>
                        </td><td align=""right"">
                                <strong>
                            1,277,380&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            4,514,983&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            1,340,312&nbsp;&nbsp;
                                </strong>
                            </td></tr><tr><td colspan=""2"">Long Term Debt</td><td align=""right"">1,233,485&nbsp;&nbsp;</td><td align=""right"">1,170,423&nbsp;&nbsp;</td><td align=""right"">1,110,585&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Other Liabilities</td><td align=""right"">118,689&nbsp;&nbsp;</td><td align=""right"">143,095&nbsp;&nbsp;</td><td align=""right"">116,605&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Deferred Long Term Liability Charges</td><td align=""right"">13,494,992&nbsp;&nbsp;</td><td align=""right"">17,093,243&nbsp;&nbsp;</td><td align=""right"">1,106,860&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Minority Interest</td><td align=""right"">35,883&nbsp;&nbsp;</td><td align=""right"">43,755&nbsp;&nbsp;</td><td align=""right"">55,688&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Negative Goodwill</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td></tr><tr><td colspan=""5""  style=""height:0;padding:0; border-top:3px solid #333;""><span style=""display:block; width:5px; height:1px;"" ></span></td></tr><tr><td colspan=""2"">
                            <strong>
                        Total Liabilities
                            </strong>
                        </td><td align=""right"">
                                <strong>
                            16,160,429&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            22,965,499&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            3,730,050&nbsp;&nbsp;
                                </strong>
                            </td></tr><tr><td colspan=""5"" style=""height:0;padding:0; ""><span style=""display:block; width:5px; height:10px;"" ></span></td></tr><tr><td class=""yfnc_d"" colspan=""5""><strong>Stockholders' Equity</strong></td></tr><tr><td colspan=""2"">Misc Stocks Options Warrants</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td></tr><tr><td colspan=""2"">Redeemable Preferred Stock</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td></tr><tr><td colspan=""2"">Preferred Stock</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td><td align=""right"">
            -
            &nbsp;</td></tr><tr><td colspan=""2"">Common Stock</td><td align=""right"">959&nbsp;&nbsp;</td><td align=""right"">945&nbsp;&nbsp;</td><td align=""right"">1,015&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Retained Earnings</td><td align=""right"">4,570,807&nbsp;&nbsp;</td><td align=""right"">8,934,244&nbsp;&nbsp;</td><td align=""right"">4,267,429&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Treasury Stock</td><td align=""right"">(911,533)</td><td align=""right"">(712,455)</td><td align=""right"">(200,228)</td></tr><tr><td colspan=""2"">Capital Surplus</td><td align=""right"">8,807,273&nbsp;&nbsp;</td><td align=""right"">8,499,475&nbsp;&nbsp;</td><td align=""right"">8,688,304&nbsp;&nbsp;</td></tr><tr><td colspan=""2"">Other Stockholder Equity</td><td align=""right"">16,576,031&nbsp;&nbsp;</td><td align=""right"">22,019,628&nbsp;&nbsp;</td><td align=""right"">318,389&nbsp;&nbsp;</td></tr><tr><td colspan=""5""  style=""height:0;padding:0; border-top:3px solid #333;""><span style=""display:block; width:5px; height:1px;"" ></span></td></tr><tr><td colspan=""2"">
                            <strong>
                        Total Stockholder Equity
                            </strong>
                        </td><td align=""right"">
                                <strong>
                            29,043,537&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            38,741,837&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            13,074,909&nbsp;&nbsp;
                                </strong>
                            </td></tr><tr><td colspan=""5"" style=""height:0;padding:0; border-top:3px solid #333;""><span style=""display:block; width:5px; height:1px;"" ></span></td></tr><tr><td colspan=""2"">
                            <strong>
                        Net Tangible Assets
                            </strong>
                        </td><td align=""right"">
                                <strong>
                            27,888,154&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            33,118,425&nbsp;&nbsp;
                                </strong>
                            </td><td align=""right"">
                                <strong>
                            7,977,453&nbsp;&nbsp;
                                </strong>
                            </td></tr></TABLE></TD></TR></TABLE></td></tr><tr><td colspan=""2""><br><center><table border=""0"" cellpadding=""1"" cellspacing=""0"" width=""80%"" class=""yfnc_promooutline1""><tr><td><table border=""0"" cellpadding=""3"" cellspacing=""0"" width=""100%""><tr><td class=""yfnc_promofill1"" align=""center""><font face=""arial"" size=""-1""><b>Sign Up for a Free Trial to EDGAR Online Premium!</b><br>Get the critical business and financial information you need for more than 15,000 U.S. public companies.<br><a href=""http://www.edgar-online.com/trial.asp?src=yahooglimpse"">Sign Up Now</a> - <a href=""http://www.edgar-online.com/corporate/"">Learn More</a></font></td></tr></table></td></tr></table><br></center></td></tr><tr><td><p class=""yfi_disclaimer"">Currency in USD.</p></td></tr></table> <div id=""yfi_media_net"" style=""width:475px;height:200px;""></div><script id=""mNCC"" type=""text/javascript""> 
                    medianet_width='475';
                    medianet_height= '200';
                    medianet_crid='625102783';
                    medianet_divid = 'yfi_media_net';
                </script><script type=""text/javascript"">
                ll_js.push({
                    'file':'//mycdn.media.net/dmedianet.js?cid=8CUJ144F7'
                });
                </script></div> <div class=""yfi_ad_s""></div></div><div class=""footer_copyright""><div class=""yfi_doc""><div id=""footer"" style=""clear:both;width:100% !important;border:none;""><hr noshade size=""1""><table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%""><tr><td class=""footer_legal"">Copyright &amp;copy; 2009 Yahoo! All rights reserved.</td></tr><tr><td><div class=""footer_legal""></div><div class=""footer_disclaimer""><p>Quotes are <strong>real-time</strong> for NASDAQ, NYSE, and NYSE MKT. See also delay times for <a href=""http://help.yahoo.com/l/us/yahoo/finance/quotes/fitadelay.html"">other exchanges</a>. All information provided ""as is"" for informational purposes only, not intended for trading purposes or advice. Neither Yahoo! nor any of independent providers is liable for any informational errors, incompleteness, or delays, or for any actions taken in reliance on information contained herein. By accessing the Yahoo! site, you agree not to redistribute the information found therein.</p><p>US Financials data provided by <a href=""http://www.edgar-online.com/"">Edgar Online</a> and all other Financials provided by <a href=""http://www.capitaliq.com"">Capital IQ</a>.</p></div></td></tr></table></div></div></div><!-- SpaceID=0 robot -->
<script type=""text/javascript"">
            ( function() {
                var nav = document.getElementById(""yfi_investing_nav"");
                if (nav) {
                    var content = document.getElementById(""rightcol"");
                    if ( content && nav.offsetHeight < content.offsetHeight) {
                        nav.style.height = content.offsetHeight + ""px"";
                    }
                }
             }());
        </script><div id=""spaceid"" style=""display:none;"">95941443</div><script type=""text/javascript"">
                if(typeof YAHOO == ""undefined""){YAHOO={};}
                if(typeof YAHOO.Finance == ""undefined""){YAHOO.Finance={};}
                if(typeof YAHOO.Finance.SymbolSuggestConfig == ""undefined""){YAHOO.Finance.SymbolSuggestConfig=[];}

                    YAHOO.Finance.SymbolSuggestConfig.push({
                        dsServer:'http://d.yimg.com/aq/autoc',
                        dsRegion:'US',
                        dsLang:'en-US',
                        dsFooter:'<div class=""moreresults""><a class=""[[tickdquote]]"" href=""http://finance.yahoo.com/lookup?s=[[link]]"">Show all results for [[tickdquote]]</a></div><div class=""tip""><em>Tip:</em> Use comma (,) to separate multiple quotes. <a href=""http://help.yahoo.com/l/us/yahoo/finance/quotes/quotelookup.html"">Learn more...</a></div>',
                        acInputId:'pageTicker',
                        acInputFormId:'quote2',
                        acContainerId:'quote2Container',
                        acModId:'bsheetget',
			acInputFocus:'0'
                    });
				</script></body><div id=""spaceid"" style=""display:none;"">95941443</div><script type=""text/javascript"">
                if(typeof YAHOO == ""undefined""){YAHOO={};}
                if(typeof YAHOO.Finance == ""undefined""){YAHOO.Finance={};}
                if(typeof YAHOO.Finance.SymbolSuggestConfig == ""undefined""){YAHOO.Finance.SymbolSuggestConfig=[];}

                    YAHOO.Finance.SymbolSuggestConfig.push({
                        dsServer:'http://d.yimg.com/aq/autoc',
                        dsRegion:'US',
                        dsLang:'en-US',
                        dsFooter:'<div class=""moreresults""><a class=""[[tickdquote]]"" href=""http://finance.yahoo.com/lookup?s=[[link]]"">Show all results for [[tickdquote]]</a></div><div class=""tip""><em>Tip:</em> Use comma (,) to separate multiple quotes. <a href=""http://help.yahoo.com/l/us/yahoo/finance/quotes/quotelookup.html"">Learn more...</a></div>',
                        acInputId:'txtQuotes',
                        acInputFormId:'quote',
                        acContainerId:'quoteContainer',
                        acModId:'mediaquotessearch',
			acInputFocus:'0'
                    });
				</script><script src=""http://l.yimg.com/zz/combo?os/mit/td/stencil-0.1.150/stencil/stencil-min.js&amp;os/mit/td/mjata-0.4.2/mjata-util/mjata-util-min.js&amp;os/mit/td/stencil-0.1.150/stencil-source/stencil-source-min.js&amp;os/mit/td/stencil-0.1.150/stencil-tooltip/stencil-tooltip-min.js&amp;os/mit/td/stencil-0.1.150/stencil-imageloader/stencil-imageloader-min.js&amp;ss/rapid-3.38.js""></script><script type=""text/javascript"" src=""http://l1.yimg.com/bm/combo?fi/common/p/d/static/js/2.0.356981/2.0.0/mini/ylc_1.9.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_loader.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_symbol_suggest.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_init_symbol_suggest.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_nav_topnav_init.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_nav_topnav.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_nav_portfolio.js&amp;fi/common/p/d/static/js/2.0.356981/yui_2.8.0/build/get/2.0.0/mini/get.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_lazy_load.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_related_videos.js&amp;fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfi_follow_quote.js""></script><span id=""yfs_params_vcr"" style=""display:none"">{""yrb_token"" : ""YFT_MARKET_WILL_CLOSE"", ""tt"" : ""1463752578"", ""s"" : ""yhoo"", ""k"" : ""a00,a50,b00,b60,c10,c63,c64,c85,c86,g00,g53,h00,h53,l10,l84,l85,l86,p20,p43,p44,t10,t53,t54,v00,v53"", ""o"" : ""^dji,^ixic"", ""j"" : ""c10,l10,p20,t10"", ""version"" : ""1.0"", ""market"" : {""NAME"" : ""U.S."", ""ID"" : ""us_market"", ""TZ"" : ""EDT"", ""TZOFFSET"" : ""-14400"", ""open"" : """", ""close"" : """", ""flags"" : {}} , ""market_status_yrb"" : ""YFT_MARKET_WILL_CLOSE"" , ""portfolio"" : { ""fd"" : { ""txns"" : [ ]},""dd"" : """",""pc"" : """",""pcs"" : """"}, ""STREAMER_SERVER"" : ""//streamerapi.finance.yahoo.com"", ""DOC_DOMAIN"" : ""finance.yahoo.com"", ""localize"" : ""0"" , ""throttleInterval"" : ""1000"" , ""arrowAsChangeSign"" : ""true"" , ""up_arrow_icon"" : ""http://l.yimg.com/a/i/us/fi/03rd/up_g.gif"" , ""down_arrow_icon"" : ""http://l.yimg.com/a/i/us/fi/03rd/down_r.gif"" , ""up_color"" : ""green"" , ""down_color"" : ""red"" , ""pass_market_id"" : ""0"" , ""mu"" : ""1"" , ""lang"" : ""en-US"" , ""region"" : ""US"" }</span><span style=""display:none"" id=""yfs_enable_chrome"">1</span><input type=""hidden"" id="".yficrumb"" name="".yficrumb"" value=""""><script type=""text/javascript"">
				ll_js.push({
				    'file':'bm/lib/fi/common/p/d/static/js/2.0.356981/2.0.0/mini/yfs_concat.js&bm/lib/fi/common/p/d/static/js/2.0.356981/translations/2.0.0/mini/yfs_l10n_en-US.js',
				    'combo' : '1',
				    'success_callback' : function() {
					YAHOO.Finance.Streaming.init();
				    }
				});

			</script><script type=""text/javascript"">
            window.RAPID_ULT ={
                tracked_mods:{
                    'navigation':'Navigation',
                    'marketindices' : 'Market Indices',
                    'yfi_investing_nav' : 'Left nav',
                    'searchQuotes':'Quote Bar',
                    'yfncsumtab' : 'Balance Sheet',
                    'yfi_ft' : 'Footer'
                },
                page_params:{
                    'pstcat' : 'Quotes',
                    'pt' : 'Quote Leaf Pages',
                    'pstth' : 'Quotes Balance Sheet'
                }
            }
        </script><script type=""text/javascript"">
                if(window.RAPID_ULT) {
                    var conf = {
                        compr_type:'deflate',
                        tracked_mods:window.RAPID_ULT.tracked_mods,
                        keys:window.RAPID_ULT.page_params,
                        spaceid:95941443,
                        client_only:0,
                        webworker_file:""\/__rapid-worker-1.2.js"",
                        nofollow_class:'rapid-nf',
                        test_id:'',
                        ywa: {
                            project_id:1000911397279,
                            document_group:"""",
                            document_name:'YHOO',
                            host:'y.analytics.yahoo.com'
                        }
                    };
                    YAHOO.i13n.YWA_CF_MAP = {""q"":118,""qlen"":29,""symbol"":119,""symbolLen"":30,""assetCls"":120,""_p"":20,""ad"":58,""authfb"":11,""bpos"":24,""camp"":54,""cat"":25,""code"":55,""cpos"":21,""ct"":23,""dcl"":26,""dir"":108,""domContentLoadedEventEnd"":44,""elm"":56,""elmt"":57,""f"":40,""ft"":51,""grpt"":109,""ilc"":39,""itc"":111,""loadEventEnd"":45,""ltxt"":17,""mpos"":110,""mrkt"":12,""pcp"":67,""pct"":48,""pd"":46,""pkgt"":22,""pos"":20,""prov"":114,""pst"":68,""pstcat"":47,""pt"":13,""rescode"":27,""responseEnd"":43,""responseStart"":41,""rspns"":107,""sca"":53,""sec"":18,""site"":42,""slk"":19,""sort"":28,""t1"":121,""t2"":122,""t3"":123,""t4"":124,""t5"":125,""t6"":126,""t7"":127,""t8"":128,""t9"":129,""tar"":113,""test"":14,""v"":52,""ver"":49,""x"":50};
                    YAHOO.i13n.YWA_ACTION_MAP = {""click"":12,""drag"":21,""drop"":106,""error"":99,""hover"":17,""hswipe"":19,""hvr"":115,""key"":13,""rchvw"":100,""scrl"":104,""scrolldown"":16,""scrollup"":15,""secview"":18,""secvw"":116,""svct"":14,""swp"":103};
                    YAHOO.i13n.YWA_OUTCOME_MAP = {""abuse"":51,""close"":34,""cmmt"":128,""cnct"":127,""comment"":49,""connect"":36,""cueauthview"":43,""cueconnectview"":46,""cuedcl"":61,""cuehpset"":50,""cueinfoview"":45,""cueloadview"":44,""cueswipeview"":42,""cuetop"":48,""dclent"":101,""dclitm"":102,""drop"":22,""dtctloc"":118,""end"":31,""entitydeclaration"":40,""exprt"":122,""favorite"":56,""fetch"":30,""filter"":35,""flagcat"":131,""flagitm"":129,""follow"":52,""hpset"":27,""imprt"":123,""insert"":28,""itemdeclaration"":37,""lgn"":125,""lgo"":126,""login"":33,""msgview"":47,""navigate"":25,""open"":29,""pa"":111,""pgnt"":113,""pl"":112,""prnt"":124,""reauthfb"":24,""reply"":54,""retweet"":55,""rmct"":32,""rmloc"":120,""rmsvct"":117,""sbmt"":114,""setlayout"":38,""sh"":107,""share"":23,""slct"":121,""slctfltr"":133,""slctloc"":119,""sort"":39,""srch"":134,""svct"":109,""top"":26,""undo"":41,""unflagcat"":132,""unflagitm"":130,""unfollow"":53,""unsvct"":110};
                    window.rapidTracker = new YAHOO.i13n.Rapid(conf); //Making rapiTracker a global variable because it might be needed by other module js
                }
        </script><script>
    var aft_beacon_callback = function() {
        if (window.LH !== undefined && Math.floor(Math.random()*100) <= 3) {
            if (!window.LH.isInitialized) {
                window.LH.init(
                {
                    spaceid:'95941443', //your space id goes here
                    serverip:'',
                    pvid:'',
                    crumb:''
                });
            }
        } 
        if (window.YAFT !== undefined) {
            var show_report = (window.location.href.indexOf('showaft=') != -1) ? 1 : 0;
            var aft_mods = [],
            each_mod = '';
            var mods = window.RAPID_ULT.tracked_mods; //Reusing the module list from rapid
            for (each_mod in mods) {
                aft_mods.push(each_mod)
            }
            window.YAFT.init(
            {
                modules: aft_mods,
                modulesExclude: ['yfi_ft'],
                useNativeStartRender: true,
                canShowVisualReport: show_report, //make it configurable and 1 for YNET users
                maxWaitTime: 5000 // make it configurable
            }, function (data, error) {
                if (!error) {
                    var rapidPerfData = {
                        'perf_navigationtime':2,
                        'perf_commontime': {
                            'initialPageLoad' : {
                                'AFT': Math.round(data.aft),
                                'AFT1': Math.round(data.aft),
                                'STR': Math.round(data.startRender),
                                'VIC': Math.round(data.visuallyComplete),
                                'DOMC': Math.round(data.domElementsCount),
                                'HTTPC': Math.round(data.httpRequests.count),
                                'CP': Math.round(data.totalCoveragePercentage),
                                'NCP': Math.round(data.normTotalCoveragePercentage)
                            }
                        },
                    };
                    window.rapidTracker.beaconPerformanceData(rapidPerfData, window.RAPID_ULT.page_params)
                    if (window.LH !== undefined && window.LH.isInitialized) {
                        //UH might initilized LH or above cases satisfied. Now configure YAFT
                        window.LH.tag(""b"",
                        {
                            val: '' //set your bucket id
                        });
                        window.LH.record('AFT', {
                            name: 'AFT', type: 'mark', startTime: Math.round(data.aft), duration: 0
                        });
                        window.LH.record('VIC', {
                            name: 'VIC', type: 'mark', startTime: Math.round(data.visuallyComplete), duration: 0
                        });
                        //this is for darla ads
                        if ( window.___adLT___ !== undefined && window.___adLT___.length && window.___adLT___.length > 0) {
                            for (var __i__ = 0; __i__ < ___adLT___.length; __i__++) {
                                window.LH.record(___adLT___[__i__][0], {
                                    name: ___adLT___[__i__][0], type: 'mark', startTime: ___adLT___[__i__][1], duration: 0
                                });
                            }
                        }
                        //finally fire all LH beacons
                        window.LH.beacon( {
                            clearMarks:false, clearMeasures: false, clearCustomEntries: true, clearTags: false
                        });
                    }
                }
            });
        }
    }
    YAHOO.util.Get.script('http://l.yimg.com/zz/combo?aj/lh-0.9.js&os/yaft/yaft-0.3.6.min.js&os/yaft/yaft-plugin-aftnoad-0.1.3.min.js', {
        onSuccess: aft_beacon_callback
    });
</script></html><!--c19.finance.bf1.yahoo.com-->
<!-- xslt3.finance.bf1.yahoo.com uncompressed/chunked Fri May 20 13:56:18 UTC 2016 -->
<!-- c19.finance.bf1.yahoo.com uncompressed/chunked Fri May 20 09:56:17 EDT 2016 -->

";
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var testSubject = new NoFuture.Rand.Data.NfHtml.YhooFinBalanceSheet(null);
            var testResult = testSubject.ParseContent(_testData);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach (var t in testResult)
                System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(t));

        }
    }
}