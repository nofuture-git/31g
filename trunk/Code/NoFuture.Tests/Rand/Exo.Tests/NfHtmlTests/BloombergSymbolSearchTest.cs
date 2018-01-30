﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Exo.NfHtml;

namespace NoFuture.Rand.Exo.Tests.NfHtmlTests
{
    [TestClass]
    public class BloombergSymbolSearchTest
    {
        #region

        private string _testData = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN""
        ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:og=""http://opengraphprotocol.org/schema/"" xmlns:fb=""http://www.facebook.com/2008/fbml"">
<head>

<script type=""text/javascript"">
//<![CDATA[
  if(defaultSectionCookie = document.cookie.match(/defaultSection=(\S+)\;/)){
    if(document.location.pathname === '/' && defaultSectionCookie[1] !== '' && document.referrer === ''){
      window.location = defaultSectionCookie[1];
    }
  }

//]]>
</script>
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>

    <meta http-equiv=""X-UA-Compatible"" content=""IE=10; IE=9; IE=8""/>
<script type=""text/javascript"">window.NREUM||(NREUM={});NREUM.info={""beacon"":""bam.nr-data.net"",""errorBeacon"":""bam.nr-data.net"",""licenseKey"":""32a39e099b"",""applicationID"":""1368463"",""transactionName"":""JVwPEhAJDlRSQ0tUBxRYBBIRSRFBWlMLVTkVVgAUAQ5NUVlVAUE="",""queueTime"":10,""applicationTime"":59,""ttGuid"":"""",""agentToken"":null,""agent"":""""}</script>
<script type=""text/javascript"">(window.NREUM||(NREUM={})).loader_config={xpid:""VgMEUVZACQAGXFVUBA==""};window.NREUM||(NREUM={}),__nr_require=function(t,e,n){function r(n){if(!e[n]){var o=e[n]={exports:{}};t[n][0].call(o.exports,function(e){var o=t[n][1][e];return r(o||e)},o,o.exports)}return e[n].exports}if(""function""==typeof __nr_require)return __nr_require;for(var o=0;o<n.length;o++)r(n[o]);return r}({1:[function(t,e,n){function r(t){try{s.console&&console.log(t)}catch(e){}}var o,i=t(""ee""),a=t(14),s={};try{o=localStorage.getItem(""__nr_flags"").split("",""),console&&""function""==typeof console.log&&(s.console=!0,-1!==o.indexOf(""dev"")&&(s.dev=!0),-1!==o.indexOf(""nr_dev"")&&(s.nrDev=!0))}catch(c){}s.nrDev&&i.on(""internal-error"",function(t){r(t.stack)}),s.dev&&i.on(""fn-err"",function(t,e,n){r(n.stack)}),s.dev&&(r(""NR AGENT IN DEVELOPMENT MODE""),r(""flags: ""+a(s,function(t,e){return t}).join("", "")))},{}],2:[function(t,e,n){function r(t,e,n,r,o){try{d?d-=1:i(""err"",[o||new UncaughtException(t,e,n)])}catch(s){try{i(""ierr"",[s,(new Date).getTime(),!0])}catch(c){}}return""function""==typeof f?f.apply(this,a(arguments)):!1}function UncaughtException(t,e,n){this.message=t||""Uncaught error with no additional information"",this.sourceURL=e,this.line=n}function o(t){i(""err"",[t,(new Date).getTime()])}var i=t(""handle""),a=t(15),s=t(""ee""),c=t(""loader""),f=window.onerror,u=!1,d=0;c.features.err=!0,t(1),window.onerror=r;try{throw new Error}catch(l){""stack""in l&&(t(8),t(7),""addEventListener""in window&&t(5),c.xhrWrappable&&t(9),u=!0)}s.on(""fn-start"",function(t,e,n){u&&(d+=1)}),s.on(""fn-err"",function(t,e,n){u&&(this.thrown=!0,o(n))}),s.on(""fn-end"",function(){u&&!this.thrown&&d>0&&(d-=1)}),s.on(""internal-error"",function(t){i(""ierr"",[t,(new Date).getTime(),!0])})},{}],3:[function(t,e,n){t(""loader"").features.ins=!0},{}],4:[function(t,e,n){function r(t){}if(window.performance&&window.performance.timing&&window.performance.getEntriesByType){var o=t(""ee""),i=t(""handle""),a=t(8),s=t(7);t(""loader"").features.stn=!0,t(6);var c=NREUM.o.EV;o.on(""fn-start"",function(t,e){var n=t[0];n instanceof c&&(this.bstStart=Date.now())}),o.on(""fn-end"",function(t,e){var n=t[0];n instanceof c&&i(""bst"",[n,e,this.bstStart,Date.now()])}),a.on(""fn-start"",function(t,e,n){this.bstStart=Date.now(),this.bstType=n}),a.on(""fn-end"",function(t,e){i(""bstTimer"",[e,this.bstStart,Date.now(),this.bstType])}),s.on(""fn-start"",function(){this.bstStart=Date.now()}),s.on(""fn-end"",function(t,e){i(""bstTimer"",[e,this.bstStart,Date.now(),""requestAnimationFrame""])}),o.on(""pushState-start"",function(t){this.time=Date.now(),this.startPath=location.pathname+location.hash}),o.on(""pushState-end"",function(t){i(""bstHist"",[location.pathname+location.hash,this.startPath,this.time])}),""addEventListener""in window.performance&&(window.performance.clearResourceTimings?window.performance.addEventListener(""resourcetimingbufferfull"",function(t){i(""bstResource"",[window.performance.getEntriesByType(""resource"")]),window.performance.clearResourceTimings()},!1):window.performance.addEventListener(""webkitresourcetimingbufferfull"",function(t){i(""bstResource"",[window.performance.getEntriesByType(""resource"")]),window.performance.webkitClearResourceTimings()},!1)),document.addEventListener(""scroll"",r,!1),document.addEventListener(""keypress"",r,!1),document.addEventListener(""click"",r,!1)}},{}],5:[function(t,e,n){function r(t){for(var e=t;e&&!e.hasOwnProperty(u);)e=Object.getPrototypeOf(e);e&&o(e)}function o(t){s.inPlace(t,[u,d],""-"",i)}function i(t,e){return t[1]}var a=t(""ee"").get(""events""),s=t(16)(a),c=t(""gos""),f=XMLHttpRequest,u=""addEventListener"",d=""removeEventListener"";e.exports=a,""getPrototypeOf""in Object?(r(document),r(window),r(f.prototype)):f.prototype.hasOwnProperty(u)&&(o(window),o(f.prototype)),a.on(u+""-start"",function(t,e){if(t[1]){var n=t[1];if(""function""==typeof n){var r=c(n,""nr@wrapped"",function(){return s(n,""fn-"",null,n.name||""anonymous"")});this.wrapped=t[1]=r}else""function""==typeof n.handleEvent&&s.inPlace(n,[""handleEvent""],""fn-"")}}),a.on(d+""-start"",function(t){var e=this.wrapped;e&&(t[1]=e)})},{}],6:[function(t,e,n){var r=t(""ee"").get(""history""),o=t(16)(r);e.exports=r,o.inPlace(window.history,[""pushState"",""replaceState""],""-"")},{}],7:[function(t,e,n){var r=t(""ee"").get(""raf""),o=t(16)(r);e.exports=r,o.inPlace(window,[""requestAnimationFrame"",""mozRequestAnimationFrame"",""webkitRequestAnimationFrame"",""msRequestAnimationFrame""],""raf-""),r.on(""raf-start"",function(t){t[0]=o(t[0],""fn-"")})},{}],8:[function(t,e,n){function r(t,e,n){t[0]=a(t[0],""fn-"",null,n)}function o(t,e,n){this.method=n,this.timerDuration=""number""==typeof t[1]?t[1]:0,t[0]=a(t[0],""fn-"",this,n)}var i=t(""ee"").get(""timer""),a=t(16)(i);e.exports=i,a.inPlace(window,[""setTimeout"",""setImmediate""],""setTimer-""),a.inPlace(window,[""setInterval""],""setInterval-""),a.inPlace(window,[""clearTimeout"",""clearImmediate""],""clearTimeout-""),i.on(""setInterval-start"",r),i.on(""setTimer-start"",o)},{}],9:[function(t,e,n){function r(t,e){d.inPlace(e,[""onreadystatechange""],""fn-"",s)}function o(){var t=this,e=u.context(t);t.readyState>3&&!e.resolved&&(e.resolved=!0,u.emit(""xhr-resolved"",[],t)),d.inPlace(t,v,""fn-"",s)}function i(t){w.push(t),h&&(g=-g,b.data=g)}function a(){for(var t=0;t<w.length;t++)r([],w[t]);w.length&&(w=[])}function s(t,e){return e}function c(t,e){for(var n in t)e[n]=t[n];return e}t(5);var f=t(""ee""),u=f.get(""xhr""),d=t(16)(u),l=NREUM.o,p=l.XHR,h=l.MO,m=""readystatechange"",v=[""onload"",""onerror"",""onabort"",""onloadstart"",""onloadend"",""onprogress"",""ontimeout""],w=[];e.exports=u;var y=window.XMLHttpRequest=function(t){var e=new p(t);try{u.emit(""new-xhr"",[e],e),e.addEventListener(m,o,!1)}catch(n){try{u.emit(""internal-error"",[n])}catch(r){}}return e};if(c(p,y),y.prototype=p.prototype,d.inPlace(y.prototype,[""open"",""send""],""-xhr-"",s),u.on(""send-xhr-start"",function(t,e){r(t,e),i(e)}),u.on(""open-xhr-start"",r),h){var g=1,b=document.createTextNode(g);new h(a).observe(b,{characterData:!0})}else f.on(""fn-end"",function(t){t[0]&&t[0].type===m||a()})},{}],10:[function(t,e,n){function r(t){var e=this.params,n=this.metrics;if(!this.ended){this.ended=!0;for(var r=0;l>r;r++)t.removeEventListener(d[r],this.listener,!1);if(!e.aborted){if(n.duration=(new Date).getTime()-this.startTime,4===t.readyState){e.status=t.status;var i=o(t,this.lastSize);if(i&&(n.rxSize=i),this.sameOrigin){var a=t.getResponseHeader(""X-NewRelic-App-Data"");a&&(e.cat=a.split("", "").pop())}}else e.status=0;n.cbTime=this.cbTime,u.emit(""xhr-done"",[t],t),c(""xhr"",[e,n,this.startTime])}}}function o(t,e){var n=t.responseType;if(""json""===n&&null!==e)return e;var r=""arraybuffer""===n||""blob""===n||""json""===n?t.response:t.responseText;return i(r)}function i(t){if(""string""==typeof t&&t.length)return t.length;if(""object""==typeof t){if(""undefined""!=typeof ArrayBuffer&&t instanceof ArrayBuffer&&t.byteLength)return t.byteLength;if(""undefined""!=typeof Blob&&t instanceof Blob&&t.size)return t.size;if(!(""undefined""!=typeof FormData&&t instanceof FormData))try{return JSON.stringify(t).length}catch(e){return}}}function a(t,e){var n=f(e),r=t.params;r.host=n.hostname+"":""+n.port,r.pathname=n.pathname,t.sameOrigin=n.sameOrigin}var s=t(""loader"");if(s.xhrWrappable){var c=t(""handle""),f=t(11),u=t(""ee""),d=[""load"",""error"",""abort"",""timeout""],l=d.length,p=t(""id""),h=t(13),m=window.XMLHttpRequest;s.features.xhr=!0,t(9),u.on(""new-xhr"",function(t){var e=this;e.totalCbs=0,e.called=0,e.cbTime=0,e.end=r,e.ended=!1,e.xhrGuids={},e.lastSize=null,h&&(h>34||10>h)||window.opera||t.addEventListener(""progress"",function(t){e.lastSize=t.loaded},!1)}),u.on(""open-xhr-start"",function(t){this.params={method:t[0]},a(this,t[1]),this.metrics={}}),u.on(""open-xhr-end"",function(t,e){""loader_config""in NREUM&&""xpid""in NREUM.loader_config&&this.sameOrigin&&e.setRequestHeader(""X-NewRelic-ID"",NREUM.loader_config.xpid)}),u.on(""send-xhr-start"",function(t,e){var n=this.metrics,r=t[0],o=this;if(n&&r){var a=i(r);a&&(n.txSize=a)}this.startTime=(new Date).getTime(),this.listener=function(t){try{""abort""===t.type&&(o.params.aborted=!0),(""load""!==t.type||o.called===o.totalCbs&&(o.onloadCalled||""function""!=typeof e.onload))&&o.end(e)}catch(n){try{u.emit(""internal-error"",[n])}catch(r){}}};for(var s=0;l>s;s++)e.addEventListener(d[s],this.listener,!1)}),u.on(""xhr-cb-time"",function(t,e,n){this.cbTime+=t,e?this.onloadCalled=!0:this.called+=1,this.called!==this.totalCbs||!this.onloadCalled&&""function""==typeof n.onload||this.end(n)}),u.on(""xhr-load-added"",function(t,e){var n=""""+p(t)+!!e;this.xhrGuids&&!this.xhrGuids[n]&&(this.xhrGuids[n]=!0,this.totalCbs+=1)}),u.on(""xhr-load-removed"",function(t,e){var n=""""+p(t)+!!e;this.xhrGuids&&this.xhrGuids[n]&&(delete this.xhrGuids[n],this.totalCbs-=1)}),u.on(""addEventListener-end"",function(t,e){e instanceof m&&""load""===t[0]&&u.emit(""xhr-load-added"",[t[1],t[2]],e)}),u.on(""removeEventListener-end"",function(t,e){e instanceof m&&""load""===t[0]&&u.emit(""xhr-load-removed"",[t[1],t[2]],e)}),u.on(""fn-start"",function(t,e,n){e instanceof m&&(""onload""===n&&(this.onload=!0),(""load""===(t[0]&&t[0].type)||this.onload)&&(this.xhrCbStart=(new Date).getTime()))}),u.on(""fn-end"",function(t,e){this.xhrCbStart&&u.emit(""xhr-cb-time"",[(new Date).getTime()-this.xhrCbStart,this.onload,e],e)})}},{}],11:[function(t,e,n){e.exports=function(t){var e=document.createElement(""a""),n=window.location,r={};e.href=t,r.port=e.port;var o=e.href.split(""://"");!r.port&&o[1]&&(r.port=o[1].split(""/"")[0].split(""@"").pop().split("":"")[1]),r.port&&""0""!==r.port||(r.port=""https""===o[0]?""443"":""80""),r.hostname=e.hostname||n.hostname,r.pathname=e.pathname,r.protocol=o[0],""/""!==r.pathname.charAt(0)&&(r.pathname=""/""+r.pathname);var i=!e.protocol||"":""===e.protocol||e.protocol===n.protocol,a=e.hostname===document.domain&&e.port===n.port;return r.sameOrigin=i&&(!e.hostname||a),r}},{}],12:[function(t,e,n){function r(t,e){return function(){o(t,[(new Date).getTime()].concat(a(arguments)),null,e)}}var o=t(""handle""),i=t(14),a=t(15);""undefined""==typeof window.newrelic&&(newrelic=NREUM);var s=[""setPageViewName"",""setCustomAttribute"",""finished"",""addToTrace"",""inlineHit""],c=[""addPageAction""],f=""api-"";i(s,function(t,e){newrelic[e]=r(f+e,""api"")}),i(c,function(t,e){newrelic[e]=r(f+e)}),e.exports=newrelic,newrelic.noticeError=function(t){""string""==typeof t&&(t=new Error(t)),o(""err"",[t,(new Date).getTime()])}},{}],13:[function(t,e,n){var r=0,o=navigator.userAgent.match(/Firefox[\/\s](\d+\.\d+)/);o&&(r=+o[1]),e.exports=r},{}],14:[function(t,e,n){function r(t,e){var n=[],r="""",i=0;for(r in t)o.call(t,r)&&(n[i]=e(r,t[r]),i+=1);return n}var o=Object.prototype.hasOwnProperty;e.exports=r},{}],15:[function(t,e,n){function r(t,e,n){e||(e=0),""undefined""==typeof n&&(n=t?t.length:0);for(var r=-1,o=n-e||0,i=Array(0>o?0:o);++r<o;)i[r]=t[e+r];return i}e.exports=r},{}],16:[function(t,e,n){function r(t){return!(t&&""function""==typeof t&&t.apply&&!t[a])}var o=t(""ee""),i=t(15),a=""nr@original"",s=Object.prototype.hasOwnProperty,c=!1;e.exports=function(t){function e(t,e,n,o){function nrWrapper(){var r,a,s,c;try{a=this,r=i(arguments),s=""function""==typeof n?n(r,a):n||{}}catch(u){d([u,"""",[r,a,o],s])}f(e+""start"",[r,a,o],s);try{return c=t.apply(a,r)}catch(l){throw f(e+""err"",[r,a,l],s),l}finally{f(e+""end"",[r,a,c],s)}}return r(t)?t:(e||(e=""""),nrWrapper[a]=t,u(t,nrWrapper),nrWrapper)}function n(t,n,o,i){o||(o="""");var a,s,c,f=""-""===o.charAt(0);for(c=0;c<n.length;c++)s=n[c],a=t[s],r(a)||(t[s]=e(a,f?s+o:o,i,s))}function f(e,n,r){if(!c){c=!0;try{t.emit(e,n,r)}catch(o){d([o,e,n,r])}c=!1}}function u(t,e){if(Object.defineProperty&&Object.keys)try{var n=Object.keys(t);return n.forEach(function(n){Object.defineProperty(e,n,{get:function(){return t[n]},set:function(e){return t[n]=e,e}})}),e}catch(r){d([r])}for(var o in t)s.call(t,o)&&(e[o]=t[o]);return e}function d(e){try{t.emit(""internal-error"",e)}catch(n){}}return t||(t=o),e.inPlace=n,e.flag=a,e}},{}],ee:[function(t,e,n){function r(){}function o(t){function e(t){return t&&t instanceof r?t:t?s(t,a,i):i()}function n(n,r,o){t&&t(n,r,o);for(var i=e(o),a=l(n),s=a.length,c=0;s>c;c++)a[c].apply(i,r);var u=f[v[n]];return u&&u.push([w,n,r,i]),i}function d(t,e){m[t]=l(t).concat(e)}function l(t){return m[t]||[]}function p(t){return u[t]=u[t]||o(n)}function h(t,e){c(t,function(t,n){e=e||""feature"",v[n]=e,e in f||(f[e]=[])})}var m={},v={},w={on:d,emit:n,get:p,listeners:l,context:e,buffer:h};return w}function i(){return new r}var a=""nr@context"",s=t(""gos""),c=t(14),f={},u={},d=e.exports=o();d.backlog=f},{}],gos:[function(t,e,n){function r(t,e,n){if(o.call(t,e))return t[e];var r=n();if(Object.defineProperty&&Object.keys)try{return Object.defineProperty(t,e,{value:r,writable:!0,enumerable:!1}),r}catch(i){}return t[e]=r,r}var o=Object.prototype.hasOwnProperty;e.exports=r},{}],handle:[function(t,e,n){function r(t,e,n,r){o.buffer([t],r),o.emit(t,e,n)}var o=t(""ee"").get(""handle"");e.exports=r,r.ee=o},{}],id:[function(t,e,n){function r(t){var e=typeof t;return!t||""object""!==e&&""function""!==e?-1:t===window?0:a(t,i,function(){return o++})}var o=1,i=""nr@id"",a=t(""gos"");e.exports=r},{}],loader:[function(t,e,n){function r(){if(!m++){var t=h.info=NREUM.info,e=u.getElementsByTagName(""script"")[0];if(t&&t.licenseKey&&t.applicationID&&e){c(l,function(e,n){t[e]||(t[e]=n)});var n=""https""===d.split("":"")[0]||t.sslForHttp;h.proto=n?""https://"":""http://"",s(""mark"",[""onload"",a()],null,""api"");var r=u.createElement(""script"");r.src=h.proto+t.agent,e.parentNode.insertBefore(r,e)}}}function o(){""complete""===u.readyState&&i()}function i(){s(""mark"",[""domContent"",a()],null,""api"")}function a(){return(new Date).getTime()}var s=t(""handle""),c=t(14),f=window,u=f.document;NREUM.o={ST:setTimeout,CT:clearTimeout,XHR:f.XMLHttpRequest,REQ:f.Request,EV:f.Event,PR:f.Promise,MO:f.MutationObserver},t(12);var d=""""+location,l={beacon:""bam.nr-data.net"",errorBeacon:""bam.nr-data.net"",agent:""js-agent.newrelic.com/nr-943.min.js""},p=window.XMLHttpRequest&&XMLHttpRequest.prototype&&XMLHttpRequest.prototype.addEventListener&&!/CriOS/.test(navigator.userAgent),h=e.exports={offset:a(),origin:d,features:{},xhrWrappable:p};u.addEventListener?(u.addEventListener(""DOMContentLoaded"",i,!1),f.addEventListener(""load"",r,!1)):(u.attachEvent(""onreadystatechange"",o),f.attachEvent(""onload"",r)),s(""mark"",[""firstbyte"",a()],null,""api"");var m=0},{}]},{},[""loader"",2,10,4,3]);</script>
<title>Symbol Lookup - Bloomberg</title>
    <meta name=""keywords"" content=""financial news, business news, stock quotes, markets quotes, finance stocks, personal finance, personal finance advice, mutual funds, financial calculators, world business, small business, financial trends, forex trading, technology news, bloomberg financial news"" />

<meta name=""description"" content=""Stay up to date on the latest business news, stock market data and financial trends. Get personal finance advice from leading experts."" />
<meta property=""fb:admins"" content=""1502271052,1617206,500566238,616620419,18776,1396618572,501161086,100001111898866,1223836""/>
<meta property=""og:title"" content=""Symbol Lookup""/>
<meta property=""og:site_name"" content=""Bloomberg""/>
<meta property=""og:description"" content=""Stay up to date on the latest business news, stock market data and financial trends. Get personal finance advice from leading experts."" />

<meta name=""twitter:card"" content=""summary""/>
<meta name=""twitter:site"" content=""@business""/>
<meta name=""twitter:creator"" content=""@business""/>
<meta property=""og:url"" content=""http://www.bloomberg.com/markets/symbolsearch?query=Waste+Connections/""/>
<link rel=""shortcut icon"" href=""/favicon.ico"" type=""image/x-icon"" />


            <link href=""http://www.bloomberg.com/blp2/v/20160511_134940/stylesheets/main-min-v3_1.css"" media=""all"" rel=""stylesheet"" type=""text/css"" />        <link href=""http://www.bloomberg.com/blp2/v/20160511_134940/stylesheets/main-min-v3_2.css"" media=""all"" rel=""stylesheet"" type=""text/css"" />        <link href=""http://www.bloomberg.com/blp2/v/20160511_134940/stylesheets/compiled/reskin/site.css"" media=""all"" rel=""stylesheet"" type=""text/css"" />    
<!--[if IE 6]>
                  <link href=""http://www.bloomberg.com/blp2/v/20160511_134940/stylesheets/ie6-all-v2-min.css"" media=""all"" rel=""stylesheet"" type=""text/css"" />                <![endif]-->
<!--[if lt IE 8]>
                  <link href=""http://www.bloomberg.com/blp2/v/20160511_134940/stylesheets/blocks_ltie8-min.css"" media=""all"" rel=""stylesheet"" type=""text/css"" />                <![endif]-->
<!--[if lt IE 9]>
        <link href=""http://www.bloomberg.com/blp2/v/20160511_134940/stylesheets/compiled/reskin/site_ie8.css"" media=""all"" rel=""stylesheet"" type=""text/css"" />        <![endif]-->
  <link href=""http://www.bloomberg.com/blp2/v/20160511_134940/stylesheets/markets/main-min.css"" media=""all"" rel=""stylesheet"" type=""text/css"" />
<link href=""http://www.bloomberg.com/blp2/v/20160511_134940/stylesheets/compiled/symbol_search.css"" media=""screen"" rel=""stylesheet"" type=""text/css"" />
  

      <link href=""http://fonts.gotraffic.net/k/bnj8lwv-d.css?3bb2a6e53c9684ffdc9a9bf4135b2a625d2f4b913c6083f29a411497d8b4eac39ad4819781f1eca20753dbd17073ba609b8e0f1a465859937e32d67cedf7f7dcf30a1f705d91f3da53fc6487d459dc3be571c10d1f0a4df5e89172ff28d158d59a1a457f5d36f64eb37907cfa914606662a47a9f90bc066eb670e86d96dd857c5e"" media=""screen"" rel=""stylesheet"" />
<link href=""//www.bbthat.com/assets/v1.1.0/that.css"" media=""all"" rel=""stylesheet"" />
<link href=""https://nav.bloomberg.com/public/stylesheets/bb-global-nav-7c10d8c1cd.css"" media=""all"" rel=""stylesheet"" />
<link href=""https://nav.bloomberg.com/public/stylesheets/bb-global-footer-a141107103.css"" media=""all"" rel=""stylesheet"" />
<link href=""http://assets.bwbx.io/s3/bcom/assets/application-8942603b7296fa1a8dd93282e6e3e4dc.css"" media=""all"" rel=""stylesheet"" />
<!--[if lt IE 9]>
<link href=""http://assets.bwbx.io/s3/bcom/assets/adaptive/full/global/global-b88dd4edd4e3e587e9df9981cffce4f1.css"" media=""all"" rel=""stylesheet"" />

<link href=""http://assets.bwbx.io/s3/bcom/assets/site_ie8-3dbe32a5cf49e8dd7014035ef6120a5a.css"" media=""all"" rel=""stylesheet"" />
<![endif]-->
<!--[if lte IE 7]>
<link href=""http://assets.bwbx.io/s3/bcom/assets/site_ie7-cb8f804fe90cf13844235944ea7342ac.css"" media=""all"" rel=""stylesheet"" />
<![endif]-->
<!--[if lte IE 9]>
<link href=""http://assets.bwbx.io/s3/bcom/assets/site_ie9-6184cf7d1630c000ab56e461a7efd75f.css"" media=""all"" rel=""stylesheet"" />
<![endif]-->
<!--[if gte IE 9]>
<link href=""http://assets.bwbx.io/s3/bcom/assets/adaptive/full/global/global-b88dd4edd4e3e587e9df9981cffce4f1.css"" media=""all"" rel=""stylesheet"" />


<script src=""//fonts.gotraffic.net/bnj8lwv.js""></script>
<script>
  try{Typekit.load();}catch(e){}
</script>

<link href=""http://assets.bwbx.io/s3/bcom/assets/adaptive/desktop/global/global/ie_drop_nav-744c14caeb844aaf2a7e09e1b8153a7f.css"" media=""all"" rel=""stylesheet"" />
<![endif]-->
<!--[if !IE]> -->
<link href=""http://assets.bwbx.io/s3/bcom/assets/adaptive/full/global/global-b88dd4edd4e3e587e9df9981cffce4f1.css"" media=""all"" rel=""stylesheet"" />


<script src=""//fonts.gotraffic.net/bnj8lwv.js""></script>
<script>
  try{Typekit.load();}catch(e){}
</script>

<!-- <![endif]-->

<link href=""http://assets.bwbx.io/s3/bcom/assets/site_full-4d9cca0cc6a6d09f67e7fc04e925754b.css"" media=""all"" rel=""stylesheet"" />

    <style type='text/css'>
  /*<![CDATA[*/
    .home.has_video_module .video_module .cassette .track .share_container{top: 31px;}.home.has_video_module .video_module .cassette .track.first .share_container{top: 35px;}
    #hot_dog_social.custom_top_box_content .custom_content_meta{width:100%;}
    body.home.has_video_module .video_module{z-index:1;position:relative;}
    
    .promoted_event .details .title, #comments .title{font-size:16px}
    .promoted_event .with_discussion{overflow-y:hidden;}
    #ifc_bottom{width:620px;}
    
    body #fancybox-wrap{z-index:100000005;}
    
    #story .byline{ position:static !important; }
    
    .view_article #story_content .assets{margin-right:10px}
    .politics-home #primary_content hr{clear:left;}
    #wide_tout_container.on_article_page .wide_tout img{margin-left:0;}
    .bw_most_popular_news{border-bottom:2px solid #6b7b84;}
    .podcast #footer_container_v3 .subsection.premium {padding: 0;text-transform: none;padding-left: 10px;margin: 0;font-size: 13px;}
    
    #wide_tout_container.on_article_page .wide_tout img{height:auto;}
    #wide_tout_container{margin-bottom:0}
    #wide_tout_container .wide_tout .custom_content_text{bottom:10px;}
    .call_tout_with_hot_dog_container{margin-bottom:0;padding-bottom:0;margin-top:0;}
    #taboola-autosized-1h-homepage { margin-top: 10px; }
    
    body.v2_style #primary_content h3 {text-transform: none !important;}
    
    body.view_section #container #content #primary_content .stories .story.imaged .art { overflow: hidden !important; }
    
    .european_debt_crisis .primary_taboola_container { float: left; }
    body.home #backgroundInterstitial{z-index:9999;}
    
    body.home #ifc_top_right_rail{float:left;}
    body.home .dfp_ad_box.ad_box.new_ad_box{float:left;width:100%}
    #ext_navigation.eye_brow_nav_reskin_wrapper {margin: 0 auto;}
  /*]]>*/
</style>

  <noscript>
  <style>
    #noscript {
      background-color: #fff;
      font-weight: bold;
      line-height: 30px;
      text-align: center;
    }
    .reskin .footer_nav_contain {
      max-height: none;
    }
    .reskin .footer_nav_toggle {
      display: none;
    }
    .reskin .section_nav ul.top_level li a {
      font-size: 13.4px;
    }
  </style>
</noscript>
  <script src=""/rapi/js_config.js"" type=""text/javascript""></script>


  <script type=""text/javascript"">
//<![CDATA[
    BLOOMBERG.global_var.enable_reskin = false;
    BLOOMBERG.global_var.reskin_global = true;
  
//]]>
</script>
      <script>
  window.BcomServerVars = window.BcomServerVars || {};
  (function(h){
    h.asset_host = ""http://assets.bwbx.io/s3/bcom/"";
    h.ad_test_domain = ""bb20ad"";
    h.bregUrl = ""https://login.bloomberg.com"";
    h.chartCloseButton = ""http://assets.bwbx.io/s3/bcom/assets/chart/x-button-11442310dae37f398025959a586f9986.png""
    h.chartStoryMarker = ""http://assets.bwbx.io/s3/bcom/assets/chart/story-marker-ac885aaa0139c9cea53e395e4c30a8cb.png""
    h.forceTrusteConsentBar = false;
    h.pageName = ""snippet_page"";
    h.qUrl = ""https://q.bloomberg.com"";
    h.symautocompleteUrl = ""http://www.bloomberg.com/apps/data?pid=symautocomplete&Query="";
    h.utcOffset = -14400000;
    h.watchlistUrl = ""http://www.bloomberg.com/markets/watchlist"";
    h.watchlistSslProxyUrl = ""https://login.bloomberg.com/watchlist/markets/watchlist/proxy"";
    h.refreshIfOriginChanged = false;
    h.bucketRange = {""start"":0,""end"":0};
    h.update_api_version = ""20160518_174528"";
    h.mediaVoicePropertyId = ""NA-BLOOCOM-11236026"";
    h.mediaVoicePlacementSwitch = ""nativewell1 nativewell2 nativedata nativelead"";
    h.contentSegment = """";
  
    if(true) {
      h.disableAnalytics = false;
      h.deviceType = ""full"";
      delete h.qRegCssJs;
    }
  })(window.BcomServerVars);
  
  window.mobileIOS = function(){
    return (/(iPhone|iPad)/i).test(navigator.userAgent);
  }
</script>

<script data-exclude=""true"" src=""//www.bbthat.com/assets/v1.1.0/that.js""></script>
<script data-exclude=""true"" src=""https://nav.bloomberg.com/public/javascripts/bb-global-nav-45a812ff2e.js""></script>
<script src=""http://assets.bwbx.io/s3/bcom/assets/snippet/application_snippet-4d9d1339fda208804bc0358da9492999.js""></script>
<script src=""//assets.bwbx.io/s3/reg/v/20160412_165520/javascripts/breg-min.js""></script>
<noscript>
<style>
  #bcom_homepage_headlines_top_news {
    display: block;
    margin-top: 12px;
  }
  .article_content .container {
    margin-top: 0;
  }
  
  .adsense,
  .advertisements,
  .article_interaction .save,
  .footer_nav_toggle,
  .headlines .filter,
  .related_links,
  .search_container,
  .sidebar_panel_trigger {
    display: none;
  }
  .b_nav {
    cursor: default;
  }
  .footer_nav_contain {
    max-height: none;
  }
  .homepage_content {
    overflow: visible;
  }
  .mini_player,
  .on_btv_now {
    display: none;
  }
  .top_nav .drop_arrow {
    display: none !important;
  }
  .quick_view nav .change_menu {
    cursor: default;
  }
  .quick_view nav .change_menu h2:after {
    content: '';
  }
  .quick_view nav .change_menu h2:hover {
    color: #0c0c0c;
  }
  
  @media screen and (min-width: 600px) {
    .related_links {
      display: block;
    }
  
    .footer_nav .mobile_apps .mobile_icon {
      background-image: url(""http://assets.bwbx.io/s3/bcom/assets/mobile-sprites-75405e6058e949c9d788ceaae5daf902.png"");
    }
  }
  
  @media screen and (min-width: 768px) {
    .article_interaction .save {
      display: block;
    }
    .article_interaction .save .icon {
      color: transparent;
      cursor: default;
    }
    .article_interaction .save .icon:after {
      background: none;
    }
    .article_interaction .save:hover .icon {
      background-color: #545454;
    }
  }
  
  @media screen and (min-width: 970px) {
    .section_nav ul.top_level li a {
      font-size: 13.4px;
    }
  }
  
  @media screen and (min-width: 1366px) {
    .section_nav ul.top_level li a {
      font-size: 16px;
    }
  }
</style>
</noscript>


  
        <script src=""http://www.bloomberg.com/blp2/v/20160511_134940/javascripts/main_f-min-v2-jq1.7.2.js"" type=""text/javascript""></script>    <script type='text/javascript'>
  //<![CDATA[
    window.Krux||((Krux=function(){Krux.q.push(arguments)}).q=[]);
    (function(){
      var k=document.createElement('script');k.type='text/javascript';k.async=true;
      var m,src=(m=location.href.match(/\bkxsrc=([^&]+)/))&&decodeURIComponent(m[1]);
      k.src = /^https?:\/\/([^\/]+\.)?krxd\.net(:\d{1,5})?\//i.test(src) ? src : src === ""disable"" ? """" :
              (location.protocol===""https:""?""https:"":""http:"")+""//cdn.krxd.net/controltag?confid=IDeZBlqQ"";
      var s=document.getElementsByTagName('script')[0];s.parentNode.insertBefore(k,s);
    
      //  see https://gist.github.com/dbrans/351e23af05a35a887039
      function retrieve(n){
        var m, k='kx'+n;
    
        var myLocalStorage;
        try {
          myLocalStorage = window.localStorage;
        } catch (e) {
        }
    
        if (myLocalStorage) {
            return myLocalStorage[k] || """";
        } else if (navigator.cookieEnabled) {
            m = document.cookie.match(k+'=([^;]*)');
            return (m && unescape(m[1])) || """";
        } else {
            return '';
        }
      }
    
      Krux.retrieve = retrieve;
    
      Krux.user = retrieve('user');
    
      Krux.segments = retrieve('segs') ? retrieve('segs').split(',') : [];
    
      Krux.injectDartKeyValues = function(url) {
        // 1a. inject kuid as ""kx"" sub-parameter of ""u"" parameter after the ""sz"" sub-parameter,
        var dfpp = [];
    
        if(Krux.user) {
          dfpp.push('khost=' + encodeURIComponent(location.hostname));
          dfpp.push('kuid=' + Krux.user)
        }
    
        for (var i = 0; i < Krux.segments.length; i++){
          dfpp.push('ksg=' + Krux.segments[i])
        }
        Krux.dfppKeyValues = dfpp.length ? ';' + dfpp.join(';') : '';
    
        url = url.replace(/(;u=)/i, Krux.dfppKeyValues + ""$1"");
        return url;
      };
    
      Krux.params = function() {
        var params = {};
        params['khost'] = encodeURIComponent(location.hostname);
        params['kuid']  = Krux.user;
        params['ksg'] = Krux.segments;
        return params;
      }
    
    })();
  //]]>
</script>

  <script type=""text/javascript"">
//<![CDATA[
    var reg_enabled = true || (BLOOMBERG.util.Cookie.get('reg_enabled') == 'true');
    var q_enabled = true || (BLOOMBERG.util.Cookie.get('q_enabled') == 'true');
  
//]]>
</script>
  
        
      <script src=""http://www.bloomberg.com/blp2/v/20160511_134940/javascripts/foresee/foresee-alive.js"" type=""text/javascript""></script>    <script type=""text/javascript"">
      //<![CDATA[
                      //]]>
  </script>

    <script type=""text/javascript"">
          //<![CDATA[
        //For omniture
        Description = ""blp.persfin/invest/ticker/lookup"";
        // Setup for surround session
        var adid="""";
        var surroundTagVal = """";
        var surroundTag=BLOOMBERG.util.Cookie.get(""surroundId""); // Get surround session id from the cookie
        if(surroundTag == ""undefined"" || surroundTag == null || surroundTag == """"){ // check if there was a surround session id in the cookie
            surroundTag="""";
        }else{
            surroundTagVal=""srnd=""+surroundTag+"";""; // finish setting up the key-value for the ad tags
        }

        if (typeof surroundTagVal == ""undefined"" || surroundTagVal == ""null"") surroundTagVal = """";

        //Market status
        if (typeof gblvlty == ""undefined""){ var gblvlty = 0; }
        market_status_val = ""status=marketstatus"" + gblvlty;
        HCat = """";

        //Setting topic value for topic pages(Lingospot)
        var topic_referrer = BLOOMBERG.referrer.topic_referrer();
        if(topic_referrer != null && topic_referrer != """"){
            topic_refer_key = ""topic="" + topic_referrer + "";"";
            topic_refer_name = ""blp.news/topic/"" + topic_referrer;
        }else{
            topic_refer_key = """";
            topic_refer_name = ""blp.news/topic;"";
        }

        sponsor_key = """";
        var sponsor = BLOOMBERG.referrer.sponsor_referrer();
        if(sponsor != null && sponsor != """"){
            sponsor_key = ""sponsor="" + sponsor + "";"";
        }
        var ticker_key = ''
        

        //Page URL to pass url to ad call
        var page_url = window.location.pathname;
        //Getting sumdomain for test site
        var host_name_list = window.location.hostname.split('.');
        var sub_domain_name= host_name_list[0];

        HCat = HCat + topic_refer_key + sponsor_key + ticker_key; //Value to be passed to the ad call
        var no_dfp_interstitial = false;
                            //]]>
    </script>

    
    

    <script type=""text/javascript"">
          //<![CDATA[
        var _sf_startpt=(new Date()).getTime()
    //]]></script>


<script type='text/javascript'>
  //<![CDATA[
    $(document).ready(function () {
      var id = setInterval(function () {
        var $ac = $('.ui-autocomplete');
        if ($ac.length > 0) {
          var $body = $(document.body);
          if ($body.hasClass('quote_snapshot') || $body.hasClass('watchlist')) {
            $('#container').after($ac.detach());
          }
          clearInterval(id);
        }
      }, 500);
    });
  //]]>
</script>

<!--[if lt IE 9]>
  <script src=""http://www.bloomberg.com/blp2/v/20160511_134940/javascripts/polyfills/html5shiv.js"" type=""text/javascript""></script><![endif]-->



</head>
<body class=""markets_v2 default-layout markets-section-front reskin_global"">

    <div class='reskin' id='global_header_reskin'>
<div id=""bb-that"">
    <nav class=""bb-that"">
        <header class=""bb-that-header bb-that--container"">
        <a class=""bb-that-header__link bb-that-header--sitemap-trigger"" href=""http://www.bloomberg.com/company/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg the Company &amp; Its Products</a><a class=""bb-that-header__link"" href=""https://bba.bloomberg.net/?utm_source=bloomberg-menu&amp;utm_medium=bcom""><span>Bloomberg Anywhere Remote Login</span><span>Bloomberg Anywhere Login</span></a><a class=""bb-that-header__link"" href=""http://www.bloomberg.com/professional/bcom-demo/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-CORE-bbgmenu-demo"">Bloomberg Terminal Demo Request</a>    </header>
        <div class=""bb-that__sitemap bb-that--container"">
    <div class=""bb-that__close""></div>

        <ul class=""bb-that__sitemap-sections bb-that--visible-md bb-that--visible-lg"">
                    <li class=""bb-that__col bb-that__col--md"">
            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Bloomberg</h2>
    <div class=""bb-that-category__content""><p class=""bb-that-category__text"">Connecting decision makers to a dynamic network of information, people and ideas, Bloomberg quickly and accurately delivers business and financial information, news and insight around the world.</p></div></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Customer Support</h2>
    <div class=""bb-that-category__content""><p class=""bb-that-category__text"">Americas<br />
+1 212 318 2000</p><p class=""bb-that-category__text"">Europe, Middle East, &amp; Africa<br />
+44 20 7330 7500</p><p class=""bb-that-category__text"">Asia Pacific<br />
+65 6212 1000</p></div></section>    </li>
                    <li class=""bb-that__col bb-that__col--md"">
            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Company</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/careers/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Careers</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/diversity-inclusion/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Diversity &amp; Inclusion</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/bcause/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Sustainability</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/technology/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Technology</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/bloomberg-facts/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">History &amp; Facts</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/philanthropy/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Philanthropy &amp; Engagement</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Communications</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/announcements/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Press Announcements</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/press-contacts/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Press Contacts</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Follow Us</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a href=""http://www.facebook.com/Bloomberglp"" class=""bb-that-category__social-link bb-that--icon-facebook"" target=""_blank"">Facebook</a></li><li class=""bb-that-category__item""><a href=""https://twitter.com/bloomberg"" class=""bb-that-category__social-link bb-that--icon-twitter"" target=""_blank"">Twitter</a></li><li class=""bb-that-category__item""><a href=""https://www.linkedin.com/company/2494"" class=""bb-that-category__social-link bb-that--icon-linkedin"" target=""_blank"">LinkedIn</a></li><li class=""bb-that-category__item""><a href=""https://instagram.com/bloomberg"" class=""bb-that-category__social-link bb-that--icon-instagram"" target=""_blank"">Instagram</a></li></ul></section>    </li>
                    <li class=""bb-that__col bb-that__col--md"">
            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Financial Products</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/professional/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-CORE-bbgmenu"">Bloomberg Terminal</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergtradebook.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-02-13-TBOOK-bbgmenu"">Bloomberg Tradebook</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergbriefs.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Briefs</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergindices.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-05-13-Indices-bbgmenu"">Bloomberg Indices</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergsef.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-08-13-SEF-bbgmenu"">Bloomberg SEF</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://about.bloomberginstitute.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Institute</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Bloomberg Customers</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""https://bba.bloomberg.net/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Anywhere Remote Login</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/professional/downloads/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Download Software</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://service.bloomberg.com/portal/sessions/new?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Service Center</a></li></ul></section>    </li>
                    <li class=""bb-that__col bb-that__col--md"">
            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Enterprise Products</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/enterprise/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-09-13-ES-bbgmenu"">Enterprise Solutions</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/trading-solutions/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-09-13-TS-bbgmenu"">Trading Solutions</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/vault/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-09-13-ES-BVAULT-bbgmenu"">Bloomberg Vault</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergpolarlake.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg PolarLake</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Industry Products</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://about.bgov.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Government</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bna.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Law/BNA</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""https://bol.bna.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Big Law</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://about.bnef.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg New Energy Finance</a></li></ul></section>    </li>
                    <li class=""bb-that__col bb-that__col--md"">
            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Media</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg.com</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/politics/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Politics</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergview.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg View</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/gadfly/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Gadfly</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/live?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Television</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/radio/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Radio</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/mobile/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Mobile Apps</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/news-bureaus/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">News Bureaus</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Media Services</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberglive.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Live Conferences</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/content-service/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Content Service</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergmedia.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Advertising</a></li></ul></section>    </li>
            </ul>

        <ul class=""bb-that__sitemap-sections bb-that--visible-sm"">
                    <li class=""bb-that__col bb-that__col--sm"">
            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Bloomberg</h2>
    <div class=""bb-that-category__content""><p class=""bb-that-category__text"">Connecting decision makers to a dynamic network of information, people and ideas, Bloomberg quickly and accurately delivers business and financial information, news and insight around the world.</p></div></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Customer Support</h2>
    <div class=""bb-that-category__content""><p class=""bb-that-category__text"">Americas<br />
+1 212 318 2000</p><p class=""bb-that-category__text"">Europe, Middle East, &amp; Africa<br />
+44 20 7330 7500</p><p class=""bb-that-category__text"">Asia Pacific<br />
+65 6212 1000</p></div></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Communications</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/announcements/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Press Announcements</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/press-contacts/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Press Contacts</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Follow Us</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a href=""http://www.facebook.com/Bloomberglp"" class=""bb-that-category__social-link bb-that--icon-facebook"" target=""_blank"">Facebook</a></li><li class=""bb-that-category__item""><a href=""https://twitter.com/bloomberg"" class=""bb-that-category__social-link bb-that--icon-twitter"" target=""_blank"">Twitter</a></li><li class=""bb-that-category__item""><a href=""https://www.linkedin.com/company/2494"" class=""bb-that-category__social-link bb-that--icon-linkedin"" target=""_blank"">LinkedIn</a></li><li class=""bb-that-category__item""><a href=""https://instagram.com/bloomberg"" class=""bb-that-category__social-link bb-that--icon-instagram"" target=""_blank"">Instagram</a></li></ul></section>    </li>
                    <li class=""bb-that__col bb-that__col--sm"">
            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Company</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/careers/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Careers</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/diversity-inclusion/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Diversity &amp; Inclusion</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/philanthropy/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Philanthropy &amp; Engagement</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/bcause/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Sustainability</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/technology/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Technology</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/bloomberg-facts/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">History &amp; Facts</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Media</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg.com</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/politics/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Politics</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergview.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg View</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/live?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Television</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/radio/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Radio</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/mobile/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Mobile Apps</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/company/news-bureaus/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">News Bureaus</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Media Services</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergmedia.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Advertising</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/content-service/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Content Service</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberglive.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Live Conferences</a></li></ul></section>    </li>
                    <li class=""bb-that__col bb-that__col--sm"">
            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Financial Products</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/professional/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-CORE-bbgmenu"">Bloomberg Terminal</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergtradebook.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-02-13-TBOOK-bbgmenu"">Bloomberg Tradebook</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergbriefs.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Briefs</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergindices.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-05-13-Indices-bbgmenu"">Bloomberg Indices</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergsef.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-08-13-SEF-bbgmenu"">Bloomberg SEF</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://about.bloomberginstitute.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Institute</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Enterprise Products</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/enterprise/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-09-13-ES-bbgmenu"">Enterprise Solutions</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/trading-solutions/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-09-13-TS-bbgmenu"">Trading Solutions</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/vault/?utm_source=bloomberg-menu&amp;utm_medium=bcom&amp;bbgsum=DG-WS-09-13-ES-BVAULT-bbgmenu"">Bloomberg Vault</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloombergpolarlake.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg PolarLake</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Industry Products</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://about.bgov.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Government</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bna.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Law/BNA</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""https://bol.bna.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Big Law</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://about.bnef.com/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg New Energy Finance</a></li></ul></section>            <section class=""bb-that-category"">
    <h2 class=""bb-that-category__title"">Bloomberg Customers</h2>
    <ul class=""bb-that-category__content""><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""https://bba.bloomberg.net/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Bloomberg Anywhere Remote Login</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://www.bloomberg.com/professional/downloads/?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Download Software</a></li><li class=""bb-that-category__item""><a class=""bb-that-category__link"" href=""http://service.bloomberg.com/portal/sessions/new?utm_source=bloomberg-menu&amp;utm_medium=bcom"">Service Center</a></li></ul></section>    </li>
            </ul>
</div>    </nav>
</div>
<div class=""bb-nav-root""><link rel=""import"" href=""https://nav.bloomberg.com/public/vendor/polymer/polymer.html""/><link rel=""import"" href=""https://nav.bloomberg.com/public/components/bb_ad-421ac31a5c.html""/><nav is=""bb-nav"" class=""bb-nav"" id=""bb-nav"" data-nav-version=""1.12.2"" data-theme=""default"" data-mode=""index"" data-progress=""0"" data-headline="""" data-base=""https://nav.bloomberg.com"" data-tracker-category=""nav"" data-tracker-events=""click"" data-tracker-label=""header"" data-site=""global""><div class=""bb-nav__container""><div class=""bb-nav__content""><h1 class=""bb-nav-logo"" is=""bb-nav-logo""><div class=""bb-nav-logo__rubix"" data-tracker-label=""bomb.open.rubix_button"" data-tracker-action=""click""></div><div class=""bb-nav-logo__menu"" data-tracker-label=""bomb.open.menu_button"" data-tracker-action=""click""><div class=""bb-nav-logo__menu-link"">MENU</div></div><a href=""//www.bloomberg.com"" class=""bb-nav-logo__link"" data-tracker-label=""logo"" data-tracker-action=""click""></a><div class=""bb-nav-logo__arrow""></div></h1><div class=""bb-nav-content-logo"" is=""bb-nav-content-logo""><h1 class=""bb-nav-content-logo__headline"" data-tracker-label=""content""><a href=""//www.bloomberg.com"" class=""bb-nav-content-logo__site"" data-tracker-label=""logo"" data-tracker-action=""click""></a></h1><div class=""bb-nav-content-logo__down-arrow"" data-tracker-label=""expand"" data-tracker-action=""click""></div></div><h2 class=""bb-nav-headline""></h2><link rel=""import"" href=""https://nav.bloomberg.com/public/components/bb_experience_select-b508856dc1.html""/><bb-experience-select class=""bb-nav-experience-select""></bb-experience-select><div class=""bb-nav-categories""><ul class=""bb-nav-categories__container""><li class=""bb-nav-categories__home""><a href=""//www.bloomberg.com"" class=""bb-nav-categories__home-link"">Homepage</a></li><li class=""bb-nav-categories__category has-submenu "" data-tracker-label=""markets"" data-vertical=""markets""><div class=""bb-nav-categories__category-container""><div class=""bb-nav-categories__notch""></div><a href=""http://www.bloomberg.com/markets"" class=""bb-nav-categories__link"" data-category-path=""/markets"" data-category-vertical=""markets"" target=""_self"" data-tracker-action=""click"">Markets</a></div><div class=""bb-nav-submenu"" is=""bb-nav-submenu""><div class=""bb-nav-submenu__content""><div class=""bb-nav-submenu__inner""><ul class=""bb-nav-submenu__categories""><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/markets_stocks"" href=""http://www.bloomberg.com/markets/stocks"" data-tracker-label=""stocks"" data-tracker-action=""click"">Stocks</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/markets_currencies"" href=""http://www.bloomberg.com/markets/currencies"" data-tracker-label=""currencies"" data-tracker-action=""click"">Currencies</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/markets_commodities"" href=""http://www.bloomberg.com/markets/commodities"" data-tracker-label=""commodities"" data-tracker-action=""click"">Commodities</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/markets_rates_bonds"" href=""http://www.bloomberg.com/markets/rates-bonds"" data-tracker-label=""rates_plus_bonds"" data-tracker-action=""click"">Rates + Bonds</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/markets_magazine"" href=""http://www.bloomberg.com/markets/markets-magazine"" data-tracker-label=""magazine"" data-tracker-action=""click"">Magazine</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/markets_benchmark"" href=""http://www.bloomberg.com/markets/benchmark"" data-tracker-label=""benchmark"" data-tracker-action=""click"">Benchmark</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/markets_watchlist"" href=""http://www.bloomberg.com/markets/watchlist"" data-tracker-label=""watchlist"" data-tracker-action=""click"">Watchlist</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/markets_economic_calendar"" href=""http://www.bloomberg.com/markets/economic-calendar"" data-tracker-label=""economic_calendar"" data-tracker-action=""click"">Economic Calendar</a></li><li class=""bb-nav-submenu__ad""><bb-ad ad-code=""bloomberg/nav"" position=""marketsnav1"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></li></ul><div class=""bb-nav-submenu__cards"" hidden></div></div></div></div></li><li class=""bb-nav-categories__category has-submenu "" data-tracker-label=""technology"" data-vertical=""technology""><div class=""bb-nav-categories__category-container""><div class=""bb-nav-categories__notch""></div><a href=""http://www.bloomberg.com/technology"" class=""bb-nav-categories__link"" data-category-path=""/technology"" data-category-vertical=""technology"" target=""_self"" data-tracker-action=""click"">Tech</a></div><div class=""bb-nav-submenu"" is=""bb-nav-submenu""><div class=""bb-nav-submenu__content""><div class=""bb-nav-submenu__inner""><ul class=""bb-nav-submenu__categories""><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/technology_companies"" href=""http://www.bloomberg.com/topics/silicon-valley"" data-tracker-label=""silicon_valley"" data-tracker-action=""click"">Silicon Valley</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/technology_gadgets"" href=""http://www.bloomberg.com/topics/global-tech"" data-tracker-label=""global_tech"" data-tracker-action=""click"">Global Tech</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/technology_start_ups"" href=""http://www.bloomberg.com/topics/startups"" data-tracker-label=""venture_capital"" data-tracker-action=""click"">Venture Capital</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/technology_security"" href=""http://www.bloomberg.com/topics/cybersecurity"" data-tracker-label=""hacking"" data-tracker-action=""click"">Hacking</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/technology_gaming"" href=""http://www.bloomberg.com/topics/entertainment"" data-tracker-label=""digital_media"" data-tracker-action=""click"">Digital Media</a></li><li class=""bb-nav-submenu__ad""><bb-ad ad-code=""bloomberg/nav"" position=""technav1"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></li></ul><div class=""bb-nav-submenu__cards"" hidden></div></div></div></div></li><li class=""bb-nav-categories__category has-submenu "" data-tracker-label=""pursuits"" data-vertical=""pursuits""><div class=""bb-nav-categories__category-container""><div class=""bb-nav-categories__notch""></div><a href=""http://www.bloomberg.com/pursuits"" class=""bb-nav-categories__link"" data-category-path=""/pursuits"" data-category-vertical=""pursuits"" target=""_self"" data-tracker-action=""click"">Pursuits</a></div><div class=""bb-nav-submenu"" is=""bb-nav-submenu""><div class=""bb-nav-submenu__content""><div class=""bb-nav-submenu__inner""><ul class=""bb-nav-submenu__categories""><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/pursuits_autos"" href=""http://www.bloomberg.com/pursuits/cars-bikes"" data-tracker-label=""cars_and_bikes"" data-tracker-action=""click"">Cars & Bikes</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/pursuits_style"" href=""http://www.bloomberg.com/pursuits/style-grooming"" data-tracker-label=""style_and_grooming"" data-tracker-action=""click"">Style & Grooming</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/pursuits_scene"" href=""http://www.bloomberg.com/pursuits/spend"" data-tracker-label=""spend"" data-tracker-action=""click"">Spend</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/pursuits_timepieces"" href=""http://www.bloomberg.com/pursuits/watches-gadgets"" data-tracker-label=""watches_and_gadgets"" data-tracker-action=""click"">Watches & Gadgets</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/pursuits_food"" href=""http://www.bloomberg.com/pursuits/food-drinks"" data-tracker-label=""food_and_drinks"" data-tracker-action=""click"">Food & Drinks</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/pursuits_travel"" href=""http://www.bloomberg.com/pursuits/travel"" data-tracker-label=""travel"" data-tracker-action=""click"">Travel</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/pursuits_property"" href=""http://www.bloomberg.com/pursuits/real-estate"" data-tracker-label=""real_estate"" data-tracker-action=""click"">Real Estate</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/pursuits_art"" href=""http://www.bloomberg.com/pursuits/art-design"" data-tracker-label=""art_and_design"" data-tracker-action=""click"">Art & Design</a></li><li class=""bb-nav-submenu__ad""><bb-ad ad-code=""bloomberg/nav"" position=""pursuitsnav1"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></li></ul><div class=""bb-nav-submenu__cards"" hidden></div></div></div></div></li><li class=""bb-nav-categories__category has-submenu "" data-tracker-label=""politics"" data-vertical=""politics""><div class=""bb-nav-categories__category-container""><div class=""bb-nav-categories__notch""></div><a href=""http://www.bloomberg.com/politics"" class=""bb-nav-categories__link"" data-category-path=""/politics"" data-category-vertical=""politics"" target=""_self"" data-tracker-action=""click"">Politics</a></div><div class=""bb-nav-submenu"" is=""bb-nav-submenu""><div class=""bb-nav-submenu__content""><div class=""bb-nav-submenu__inner""><ul class=""bb-nav-submenu__categories""><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/politics_with_all_due_respect"" href=""http://www.bloomberg.com/politics/topics/wadr-full-episode"" data-tracker-label=""with_all_due_respect"" data-tracker-action=""click"">With All Due Respect</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/politics_delegate_tracker"" href=""http://www.bloomberg.com/politics/graphics/2016-delegate-tracker/"" data-tracker-label=""delegate_tracker"" data-tracker-action=""click"">Delegate Tracker</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/politics_culture_caucus_podcast"" href=""http://www.bloomberg.com/podcasts/culture_caucus"" data-tracker-label=""culture_caucus_podcast"" data-tracker-action=""click"">Culture Caucus Podcast</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/politics_masters_in_politics_podcast"" href=""http://www.bloomberg.com/podcasts/masters_in_politics"" data-tracker-label=""masters_in_politics_podcast"" data-tracker-action=""click"">Masters In Politics Podcast</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/politics_what_the_voters_are_streaming"" href=""http://www.bloomberg.com/politics/graphics/2016-voter-spotify-listens/"" data-tracker-label=""what_the_voters_are_streaming"" data-tracker-action=""click"">What The Voters Are Streaming</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/editors_picks"" href=""http://www.bloomberg.com/politics/topics/editors-picks"" data-tracker-label=""editors'_picks"" data-tracker-action=""click"">Editors' Picks</a></li><li class=""bb-nav-submenu__ad""><bb-ad ad-code=""bloomberg/nav"" position=""politicsnav1"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></li></ul><div class=""bb-nav-submenu__cards"" hidden></div></div></div></div></li><li class=""bb-nav-categories__category has-submenu "" data-tracker-label=""opinion"" data-vertical=""opinion""><div class=""bb-nav-categories__category-container""><div class=""bb-nav-categories__notch""></div><span class=""bb-nav-categories__link"" data-category-path=""/opinion"" data-category-vertical=""opinion"" target=""_self"" data-tracker-action=""click"">Opinion</span></div><div class=""bb-nav-submenu"" is=""bb-nav-submenu""><div class=""bb-nav-submenu__content""><div class=""bb-nav-submenu__inner""><ul class=""bb-nav-submenu__categories""><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/opinion_view"" href=""http://www.bloomberg.com/view"" data-tracker-label=""view"" data-tracker-action=""click"">View</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/opinion_gadfly"" href=""http://www.bloomberg.com/gadfly"" data-tracker-label=""gadfly"" data-tracker-action=""click"">Gadfly</a></li><li class=""bb-nav-submenu__ad""><bb-ad ad-code=""bloomberg/nav"" position=""opinionnav1"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></li></ul><div class=""bb-nav-submenu__cards"" hidden></div></div></div></div></li><li class=""bb-nav-categories__category has-submenu "" data-tracker-label=""businessweek"" data-vertical=""businessweek""><div class=""bb-nav-categories__category-container""><div class=""bb-nav-categories__notch""></div><a href=""http://www.bloomberg.com/businessweek"" class=""bb-nav-categories__link"" data-category-path=""/businessweek"" data-category-vertical=""businessweek"" target=""_self"" data-tracker-action=""click"">Businessweek</a></div><div class=""bb-nav-submenu"" is=""bb-nav-submenu""><div class=""bb-nav-submenu__content""><div class=""bb-nav-submenu__inner""><ul class=""bb-nav-submenu__categories""><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/businessweek_subscribe"" href=""https://subscribe.businessweek.com/servlet/OrdersGateway?cds_mag_code=BWK&cds_page_id=200920"" data-tracker-label=""subscribe"" data-tracker-action=""click"">Subscribe</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/businessweek_cover_stories"" href=""http://www.bloomberg.com/topics/cover-story"" data-tracker-label=""cover_stories"" data-tracker-action=""click"">Cover Stories</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/businessweek_opening_remarks"" href=""http://www.bloomberg.com/topics/opening-remarks"" data-tracker-label=""opening_remarks"" data-tracker-action=""click"">Opening Remarks</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/businessweek_etc"" href=""http://www.bloomberg.com/topics/etc"" data-tracker-label=""etc"" data-tracker-action=""click"">Etc</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/businessweek_features"" href=""http://www.bloomberg.com/topics/features"" data-tracker-label=""features"" data-tracker-action=""click"">Features</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/businessweek_85th_anniversary_issue"" href=""http://www.businessweek.com/features/85ideas/"" data-tracker-label=""85th_anniversary_issue"" data-tracker-action=""click"">85th Anniversary Issue</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute=""https://nav.bloomberg.com/global/api/businessweek_behind_the_cover"" href=""http://www.bloomberg.com/topics/behind-the-cover"" data-tracker-label=""behind_the_cover"" data-tracker-action=""click"">Behind The Cover</a></li><li class=""bb-nav-submenu__ad""><bb-ad ad-code=""bloomberg/nav"" position=""bweeknav1"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></li></ul><div class=""bb-nav-submenu__cards"" hidden></div></div></div></div></li><li class=""bb-nav-categories__category has-submenu "" data-tracker-label=""footer"" data-vertical=""footer""><div class=""bb-nav-categories__category-container""><div class=""bb-nav-categories__notch""></div><span class=""bb-nav-categories__link"" data-category-path=""/footer"" data-category-vertical=""footer"" target=""_self"" data-tracker-action=""click"">More</span></div><div class=""bb-nav-submenu"" is=""bb-nav-submenu""><div class=""bb-nav-submenu__content""><div class=""bb-nav-submenu__inner""><ul class=""bb-nav-submenu__categories""><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/news/industries"" data-tracker-label=""industries"" data-tracker-action=""click"">Industries</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/news/science-energy"" data-tracker-label=""science_plus_energy"" data-tracker-action=""click"">Science + Energy</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/graphics"" data-tracker-label=""graphics"" data-tracker-action=""click"">Graphics</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/game-plan"" data-tracker-label=""game_plan"" data-tracker-action=""click"">Game Plan</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/small-business"" data-tracker-label=""small_business"" data-tracker-action=""click"">Small Business</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/personal-finance"" data-tracker-label=""personal_finance"" data-tracker-action=""click"">Personal Finance</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/segments/inspire-go"" data-tracker-label=""inspire_go"" data-tracker-action=""click"">Inspire GO</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/news/special-reports/bloomberg-board-directors-forum"" data-tracker-label=""board_directors_forum"" data-tracker-action=""click"">Board Directors Forum</a></li><li class=""bb-nav-submenu__category""><a class=""bb-nav-submenu__category-link"" data-subroute="""" href=""http://www.bloomberg.com/sponsor/partnerships/"" data-tracker-label=""sponsored_content"" data-tracker-action=""click"">Sponsored Content</a></li><li class=""bb-nav-submenu__ad""><bb-ad ad-code=""bloomberg/nav"" position=""footernav1"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></li></ul><div class=""bb-nav-submenu__cards"" hidden></div></div></div></div></li><li class=""bb-nav-categories__category bb-nav-categories__experience""><bb-experience-select class=""bb-nav-mobile-experience-select""></bb-experience-select></li></ul></div><div class=""bb-nav-touts"" is=""bb-nav-touts"" data-tracker-label=""tout""><a target=""_self"" class=""bb-nav-touts__link"" href=""https://login.bloomberg.com/"" data-tracker-label=""sign_in"" data-tracker-action=""click"">Sign In</a><a target=""_blank"" class=""bb-nav-touts__subscribe-link"" href=""https://subscribe.businessweek.com/servlet/OrdersGateway?cds_mag_code=BWK&cds_page_id=200920"" data-vertical-target=""businessweek"" data-tracker-label=""subscribe"" data-tracker-action=""click"">Subscribe</a></div><link rel=""import"" href=""https://nav.bloomberg.com/public/components/bb_social-f857cf4583.html""/><div class=""bb-nav-social-container""><bb-social class=""bb-nav-social""></bb-social></div><div class=""bb-nav-search"" is=""bb-nav-search"" data-tracker-label=""search""><form class=""bb-nav-search__form"" action=""/search"" method=""get"" autocomplete=""false""><input type=""text"" class=""bb-nav-search__input"" placeholder=""What are you searching for?"" name=""query"" autocomplete=""off""/></form><link rel=""import"" href=""https://nav.bloomberg.com/public/components/bb_quick_links-9234af6693.html""/><link rel=""import"" href=""https://nav.bloomberg.com/public/components/bb_search_suggestions-fc70f5f1ca.html""/><div class=""bb-search-drawer"" tabindex=""0""><div class=""bb-search-drawer__content""><bb-quick-links></bb-quick-links><bb-search-suggestions></bb-search-suggestions></div></div></div><span class=""bb-nav-search__icon"" data-tracker-label=""search.icon"" data-tracker-action=""click""></span></div></div><div class=""bb-progress""><div class=""bb-progress__status""></div></div><bb-bomb-content><dom-module id=""bb-bomb-content""><template><section class=""bb-nav-bomb"" is=""bb-nav-bomb"" data-tracker-events=""click"" data-tracker-category=""nav"" data-tracker-label=""bomb"" data-bomb=""initial""><div class=""bb-nav-bomb__container""><div class=""bb-nav-bomb__header""><div class=""bb-nav-bomb__logo""><a href=""http://www.bloomberg.com"" class=""bb-nav-bomb__logo-link"" data-tracker-label=""logo"" data-tracker-action=""click"">Bloomberg</a></div><div class=""bb-nav-bomb__close-button""><div class=""bb-nav-bomb__close-button-link"" on-click=""closeClickHandler"" data-tracker-label=""close_button"" data-tracker-action=""click"">CLOSE</div></div></div><div class=""bb-nav-bomb__featured-stories""><div class=""bb-nav-bomb__audio-video""><div class=""bb-nav-bomb__audio-video-container""><div class=""bb-nav-bomb-module"" data-module-vertical=""video"" data-tracker-label=""video""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/live"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Video</a></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__video-player""><div is=""bb-video-player""id$=""[[playerId]]""class=""bb-video-player""data-stream=""3858f0ac-f022-4404-848c-c17783cd066a""data-poster=""https://assets.bwbx.io/images/users/iqjWHBFdfxIU/i1lG1tHvj1Uw/v5/400x225.jpg""></div></div><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloomberg.com/news/videos/2016-05-18/dynamic-s-cohen-no-gold-euphoria-in-canada"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">Dynamic's Cohen: No Gold Euphoria in Canada</a></div></div></div></div><div class=""bb-nav-bomb-module"" data-module-vertical=""audio"" data-tracker-label=""audio""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/audio"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Audio</a></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloomberg.com/news/articles/2016-04-04/odd-lots-the-unbearable-brightness-of-being-a-shadow-bank"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">Odd Lots: The Unbearable Brightness of Being a Shadow Bank</a></div></div></div></div><div class=""bb-nav-bomb__live-tv""><a href=""http://www.bloomberg.com/live"" class=""bb-nav-bomb__live-tv-link"" data-tracker-label=""live_tv"" data-tracker-action=""click"">Live TV</a><a href=""http://www.bloomberg.com/live/schedule-shows"" class=""bb-nav-bomb__live-tv-link"" data-tracker-label=""schedule_shows"" data-tracker-action=""click"">Schedules+Shows</a></div></div></div><div class=""bb-nav-bomb__latest-news""><div class=""bb-nav-bomb-module"" data-module-vertical=""latest_news"" data-tracker-label=""latest_news""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/businessweek"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Businessweek</a></div><div class=""bb-nav-bomb-module__stories""><a href=""//www.bloomberg.com/businessweek"" data-tracker-label=""image"" data-tracker-action=""click""><img class=""bb-nav-bomb-module__image"" src=""https://assets.bwbx.io/images/users/iqjWHBFdfxIU/i2hqmilctIu8/v1/190x-1.jpg""/></a><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloomberg.com/news/articles/2016-05-12/how-to-pull-the-world-economy-out-of-its-rut"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">How to Pull the World Economy Out of Its Rut</a><time datetime=""2016-05-13T17:58:52.701Z"" class=""bb-nav-timestamp"">Updated 5 days ago</time><a href=""https://subscribe.businessweek.com/servlet/OrdersGateway?cds_mag_code=BWK&cds_page_id=200922"" class=""bb-nav-bomb-module-story__subscribe-link"" data-tracker-label=""bw_subscribe_tout"" data-tracker-action=""click"">Subscribe</a></div></div></div></div></div></div><div class=""bb-nav-bomb__menu""><div class=""bb-nav-bomb__menu-container""><div class=""bb-nav-bomb__menu-verticals""><div class=""bb-nav-bomb-module"" data-module-vertical=""markets"" data-tracker-label=""markets""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/markets"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Markets</a></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloomberg.com/news/articles/2016-05-18/banks-sued-over-manipulation-on-9-trillion-agency-bond-market"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">Banks Sued Over Manipulation on $9 Trillion Agency-Bond Market</a><time datetime=""2016-05-18T21:47:18.082Z"" class=""bb-nav-timestamp"">May 18, 2016</time></div></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_1""><a href=""http://www.bloomberg.com/markets/stocks"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Stocks</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_2""><a href=""http://www.bloomberg.com/markets/currencies"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Currencies</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_3""><a href=""http://www.bloomberg.com/markets/commodities"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Commodities</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_4""><a href=""http://www.bloomberg.com/markets/rates-bonds"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Rates + Bonds</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_5""><a href=""http://www.bloomberg.com/markets/markets-magazine"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Magazine</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_6""><a href=""http://www.bloomberg.com/markets/benchmark"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Benchmark</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_7""><a href=""http://www.bloomberg.com/markets/watchlist"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Watchlist</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_8""><a href=""http://www.bloomberg.com/markets/economic-calendar"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Economic Calendar</a></div><div class=""bb-nav-bomb-module-ad""><bb-ad ad-code=""bloomberg/nav"" position=""navmarkets"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></div></div></div><div class=""bb-nav-bomb-module"" data-module-vertical=""technology"" data-tracker-label=""technology""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/technology"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Technology</a></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloomberg.com/news/articles/2016-05-18/tesla-says-12-200-model-3-orders-were-cancelled"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">Tesla Says 12,200 Model 3 Orders Were Canceled</a><time datetime=""2016-05-18T21:51:40.579Z"" class=""bb-nav-timestamp"">May 18, 2016</time></div></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_1""><a href=""http://www.bloomberg.com/topics/silicon-valley"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Silicon Valley</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_2""><a href=""http://www.bloomberg.com/topics/global-tech"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Global Tech</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_3""><a href=""http://www.bloomberg.com/topics/startups"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Venture Capital</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_4""><a href=""http://www.bloomberg.com/topics/cybersecurity"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Hacking</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_5""><a href=""http://www.bloomberg.com/topics/entertainment"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Digital Media</a></div><div class=""bb-nav-bomb-module-ad""><bb-ad ad-code=""bloomberg/nav"" position=""navtech"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></div></div></div><div class=""bb-nav-bomb-module"" data-module-vertical=""politics"" data-tracker-label=""politics""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/politics"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Politics</a></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloomberg.com/politics/videos/2016-05-18/granite-state-poll-clinton-and-trump-neck-and-neck"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">Granite State Poll: Clinton and Trump Neck-and-Neck</a><time datetime=""2016-05-18T21:51:18.082Z"" class=""bb-nav-timestamp"">May 18, 2016</time></div></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_1""><a href=""http://www.bloomberg.com/politics/topics/wadr-full-episode"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">With All Due Respect</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_2""><a href=""http://www.bloomberg.com/politics/graphics/2016-delegate-tracker/"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Delegate Tracker</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_3""><a href=""http://www.bloomberg.com/podcasts/culture_caucus"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Culture Caucus Podcast</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_4""><a href=""http://www.bloomberg.com/podcasts/masters_in_politics"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Masters In Politics Podcast</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_5""><a href=""http://www.bloomberg.com/politics/graphics/2016-voter-spotify-listens/"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">What The Voters Are Streaming</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_6""><a href=""http://www.bloomberg.com/politics/topics/editors-picks"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Editors' Picks</a></div><div class=""bb-nav-bomb-module-ad""><bb-ad ad-code=""bloomberg/nav"" position=""navpolitics"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></div></div></div><div class=""bb-nav-bomb-module"" data-module-vertical=""pursuits"" data-tracker-label=""pursuits""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/pursuits"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Pursuits</a></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloomberg.com/news/articles/2016-05-18/tesla-says-12-200-model-3-orders-were-cancelled"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">Tesla Says 12,200 Model 3 Orders Were Canceled</a><time datetime=""2016-05-18T21:51:40.579Z"" class=""bb-nav-timestamp"">May 18, 2016</time></div></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_1""><a href=""http://www.bloomberg.com/pursuits/cars-bikes"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Cars & Bikes</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_2""><a href=""http://www.bloomberg.com/pursuits/style-grooming"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Style & Grooming</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_3""><a href=""http://www.bloomberg.com/pursuits/spend"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Spend</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_4""><a href=""http://www.bloomberg.com/pursuits/watches-gadgets"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Watches & Gadgets</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_5""><a href=""http://www.bloomberg.com/pursuits/food-drinks"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Food & Drinks</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_6""><a href=""http://www.bloomberg.com/pursuits/travel"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Travel</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_7""><a href=""http://www.bloomberg.com/pursuits/real-estate"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Real Estate</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_8""><a href=""http://www.bloomberg.com/pursuits/art-design"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Art & Design</a></div><div class=""bb-nav-bomb-module-ad""><bb-ad ad-code=""bloomberg/nav"" position=""navpursuits"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></div></div></div><div class=""bb-nav-bomb-module"" data-module-vertical=""opinion"" data-tracker-label=""opinion""><div class=""bb-nav-bomb-module__title""><span class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Opinion</span></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloombergview.com/articles/2016-05-18/trump-tries-to-soothe-the-womenfolk"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">Trump Tries to Comfort the Skittish Womenfolk: Margaret Carlson</a><time datetime=""2016-05-18T21:28:41.117Z"" class=""bb-nav-timestamp"">May 18, 2016</time></div></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_1""><a href=""http://www.bloomberg.com/view"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">View</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_2""><a href=""http://www.bloomberg.com/gadfly"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Gadfly</a></div><div class=""bb-nav-bomb-module-ad""><bb-ad ad-code=""bloomberg/nav"" position=""navopinion"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></div></div></div><div class=""bb-nav-bomb-module"" data-module-vertical=""businessweek"" data-tracker-label=""businessweek""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/businessweek"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Businessweek</a></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-story"" data-tracker-label=""story_0""><div class=""bb-nav-bomb-module-story__info""><a href=""http://www.bloomberg.com/features/2016-martha-beck-life-coach/"" class=""bb-nav-bomb-module-story__headline"" data-tracker-label=""headline"" data-tracker-action=""click"">Even the World's Top Life Coaches Need a Life Coach. Meet Martha Beck</a><time datetime=""2016-05-18T15:15:05.647Z"" class=""bb-nav-timestamp"">May 18, 2016</time></div></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_1""><a href=""https://subscribe.businessweek.com/servlet/OrdersGateway?cds_mag_code=BWK&cds_page_id=200920"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Subscribe</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_2""><a href=""http://www.bloomberg.com/topics/cover-story"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Cover Stories</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_3""><a href=""http://www.bloomberg.com/topics/opening-remarks"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Opening Remarks</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_4""><a href=""http://www.bloomberg.com/topics/etc"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Etc</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_5""><a href=""http://www.bloomberg.com/topics/features"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Features</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_6""><a href=""http://www.businessweek.com/features/85ideas/"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">85th Anniversary Issue</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_7""><a href=""http://www.bloomberg.com/topics/behind-the-cover"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Behind The Cover</a></div><div class=""bb-nav-bomb-module-ad""><bb-ad ad-code=""bloomberg/nav"" position=""navbweek"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></div></div></div></div><div class=""bb-nav-bomb__menu-footer"" data-tracker-label=""footer""><div class=""bb-nav-bomb-module"" data-module-vertical=""footer"" data-tracker-label=""footer""><div class=""bb-nav-bomb-module__title""><a href=""http://www.bloomberg.com/footer"" class=""bb-nav-bomb-module__link"" data-tracker-label=""title"" data-tracker-action=""click"">Footer</a></div><div class=""bb-nav-bomb-module__stories""><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_0""><a href=""http://www.bloomberg.com/news/industries"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Industries</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_1""><a href=""http://www.bloomberg.com/news/science-energy"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Science + Energy</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_2""><a href=""http://www.bloomberg.com/graphics"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Graphics</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_3""><a href=""http://www.bloomberg.com/game-plan"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Game Plan</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_4""><a href=""http://www.bloomberg.com/small-business"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Small Business</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_5""><a href=""http://www.bloomberg.com/personal-finance"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Personal Finance</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_6""><a href=""http://www.bloomberg.com/segments/inspire-go"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Inspire GO</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_7""><a href=""http://www.bloomberg.com/news/special-reports/bloomberg-board-directors-forum"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Board Directors Forum</a></div><div class=""bb-nav-bomb-module-link"" data-tracker-label=""story_8""><a href=""http://www.bloomberg.com/sponsor/partnerships/"" class=""bb-nav-bomb-module-link__label"" data-tracker-label=""link"" data-tracker-action=""click"">Sponsored Content</a></div><div class=""bb-nav-bomb-module-ad""><bb-ad ad-code=""bloomberg/nav"" position=""navbottom"" target=""bloomberg/nav"" dimensions='{""tablet"":[[5,14]],""small_desktop"":[[5,14]],""large_desktop"":[[5,14]]}'></bb-ad></div></div></div></div></div></div></div></section></template></dom-module id=""bb-bomb""></bb-bomb-content><script type=""application/javascript"" src=""//cdn.gotraffic.net/projector/latest/bplayer.js"" data-exclude=""true""></script></nav><script type=""text/javascript"">(function(d) {var s = d.createElement(""script"");s.async = false;s.src = ""https://nav.bloomberg.com/public/javascripts/webcomponents-lite-f60d42edea.js"";s.type = ""text/javascript"";d.head.appendChild(s);s = null;})(document);</script></div>
<script>
  var bbNav = document.querySelector('#bb-nav');
  bbNav.addEventListener('search', function(event) {
    document.location = 'http://www.bloomberg.com/search?query=' + encodeURIComponent(event.detail.query);
  });
</script>

<script>
  Bcom.currentPage = new Bcom.Pages.Snippet();
</script>

<div class='clearfix' id='header_bottom'>
<div class='eye_brow_nav_reskin_wrapper' id='ext_navigation'>

</div>
<script type='text/javascript'>
  //<![CDATA[
    if (""true"" !== ""true"") {
      BLOOMBERG.register_ext_nav_events();
    } else {
      // using code from BCOM and depends on snippet svc being on
      BLOOMBERG.setup_ext_nav_tracking();
    }
  //]]>
</script>


</div>
<div class='reskin' id='leader_board_container'>
<div id='leader_board'>







      <div class=""gpt_ad  "" id=""gpt-ad-""><script type=""text/javascript"">
//<![CDATA[
BLOOMBERG.gpt.render({""ad_description"":""blp.persfin/invest//ticker//lookup"",""position"":null,""size"":[[728,90],[970,66],[970,90],[1,1]],""tile"":2,""el_classes"":"""",""id"":""gpt-ad-""});
//]]>
</script></div>
  


</div>
</div>
</div>

    <div id=""container"" class=""module"">
            
            
      
      <div id=""content"" class=""clearfix "">
                        <div id=""primary_content"">
          <div class='symbol_search'>
<h1>Symbol Lookup</h1>
<form action=""/markets/symbolsearch"" method=""get"">
<div class='ticker_search'>
<input id=""query"" name=""query"" size=""32"" type=""text"" value=""Waste Connections"" />
<input name=""commit"" type=""submit"" value=""Find Symbols"" />
</div>
</form>
<div class='ticker_matches'>
1 - 2 of 2
</div>
<table class='dual_border_data_table alt_rows_stat_table tabular_table'>
<tr class='data_header'>
<th class='symbol'>Symbol</th>
<th class='name'>Name</th>
<th class='country'>Country</th>
<th class='type'>Type</th>
<th>Industry/Objective</th>
</tr>
<tr class='odd'>
<td class='symbol'><a href=""/quote/WCN:US"">WCN:US</a></td>
<td class='name'>Waste Connections Inc</td>
<td>USA</td>
<td>Common Stock</td>
<td>Non-hazardous Waste Disp</td>
</tr>
<tr class='even'>
<td class='symbol'><a href=""/quote/WAJ:GR"">WAJ:GR</a></td>
<td class='name'>Waste Connections Inc</td>
<td>Germany</td>
<td>Common Stock</td>
<td>Non-hazardous Waste Disp</td>
</tr>
</table>
<div class=""text_ad dfp_ad_box ad_box""><div class=""ad_box_title"">Sponsored Link</div>




      <div class=""gpt_ad bottom "" id=""gpt-ad-bottom""><script type=""text/javascript"">
//<![CDATA[
BLOOMBERG.gpt.render({""ad_description"":""blp.persfin/invest//ticker//lookup"",""position"":""bottom"",""size"":[[150,1]],""tile"":3,""el_classes"":"""",""id"":""gpt-ad-bottom""});
//]]>
</script></div>
  
</div>

</div>
<div class='clearfix'></div>
        </div>
        <div id=""secondary_content"">
          

<div class=""dfp_ad_box ad_box "">
  <div class=""ad_box_title"">Advertisement</div>
  



      <div class=""gpt_ad right2 "" id=""gpt-ad-right2""><script type=""text/javascript"">
//<![CDATA[
BLOOMBERG.gpt.render({""ad_description"":""blp.persfin/invest//ticker//lookup"",""position"":""right2"",""size"":[[300,250],[300,600]],""tile"":2,""el_classes"":"""",""id"":""gpt-ad-right2""});
//]]>
</script></div>
  
 </div>


<div class=""ad_box ""><div class=""ad_box_title"">Sponsored Links</div>
    <div class=""FPO_medium_rectangle_ad"" id=""right_google_1"">
    <script type=""text/javascript""><!--
        google_ad_client = ""ca-pub-1979187633561026"";
        google_ad_slot = ""7597199020"";
        google_ad_width = 300;
        google_ad_height = 250;
        //-->
    </script>
    <script type=""text/javascript"" src=""http://pagead2.googlesyndication.com/pagead/show_ads.js"">
    </script>
    </div>
</div>

        </div>
        
      </div>
            
      <div id=""symac_suggest"" class=""reskin""></div>
    </div>
    <script type='text/javascript'>
  //<![CDATA[
    var ga_page_name = ((typeof(Description) != 'undefined') ? Description : '').replace(/\//g,"":"");
    Bcom.Tracker.Page.setDescription(ga_page_name);
  //]]>
</script>
<div class='reskin'>
<script>
  Bcom.currentPage = new Bcom.Pages.Snippet();
  if(Bcom.Tracker.Page.getDescription() == '') {
    Bcom.Tracker.Page.setDescription('');
  }
</script>
<div id='global-footer'>
<footer class=""bb-global-footer""><div class=""bb-global-footer__content""><div class=""bb-global-footer__group""><a class=""bb-global-footer__link"" href=""//www.bloomberg.com/tos"">Terms of Service</a><a class=""bb-global-footer__link"" href=""//www.bloomberg.com/trademarks"">Trademarks</a><a class=""bb-global-footer__link"" href=""//www.bloomberg.com/privacy"">Privacy Policy</a><small class=""bb-global-footer__copyright"">©2016 Bloomberg L.P. All Rights Reserved</small></div><div class=""bb-global-footer__group""><a class=""bb-global-footer__link"" href=""//www.bloomberg.com/careers/?utm_source=dotcom&utm_medium=footer"">Careers</a><a class=""bb-global-footer__link"" href=""http://nytm.org/made-in-nyc"">Made in NYC</a><a class=""bb-global-footer__link"" href=""http://bloombergmedia.com/"">Advertise</a><a class=""bb-global-footer__link bb-global-footer__ad-choices"" href=""//www.bloomberg.com/privacy#advertisements"">Ad Choices</a><a class=""bb-global-footer__link"" href=""//www.bloomberg.com/feedback"">Website Feedback</a><a class=""bb-global-footer__link"" href=""//www.bloomberg.com/help"">Help</a></div></div></footer>
</div>

<script>
  <!-- Chartbeat -->
  var _sf_async_config={};
  _sf_async_config.uid = 15087;
  _sf_async_config.domain = 'bloomberg.com';
  _sf_async_config.useCanonical = true;
  _sf_async_config.authors = '';
  (function(){
    function loadChartbeat() {
      window._sf_endpt=(new Date()).getTime();
      var e = document.createElement(""script"");
      e.setAttribute(""language"", ""javascript"");
      e.setAttribute(""type"", ""text/javascript"");
      e.setAttribute('src', '//static.chartbeat.com/js/chartbeat.js');
      document.body.appendChild(e);
    }
    var oldonload = window.onload;
    window.onload = (typeof window.onload != ""function"") ?
      loadChartbeat : function() { oldonload(); loadChartbeat(); };
  })();
</script>
<script>
  (function() {
    // Native article pages don't have the correct tracking info until the contents are loaded through
    // Polar's API call. The tracking is delayed here and will be fired on native article render event.
    if(Bcom.global_var.IS_NATIVE) {
      return;
    }
    Bcom.Tracker.Page.trackToComScore();
  })();
</script>
<noscript>
<p>
<img alt=""*"" height=""1"" src=""http://b.scorecardresearch.com/p?c1=2&amp;c2=3005059"" width=""1"" />
</p>
</noscript>
<script>
  window.Utils.AB.setExperimentsChosen();
  
  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-11413116-1']);
  Bcom.Tracker.Page.trackToGoogleAnalytics();
  _gaq.push(['_trackPageview']);
  
  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();
</script>
<script>
  $(document).ready(function () {
    Bcom.Tracker.Events.setupTrackingExternalLinks();
  });
</script>
<!-- START Nielsen Online SiteCensus V6.0 -->
<!-- COPYRIGHT 2010 Nielsen Online -->
<script>
  $(window).load(function () {
    Bcom.Tracker.Events.trackNielsenEvent('0');
  });
</script>
<noscript>
<div>
<img alt="""" height=""1"" src=""//secure-us.imrworldwide.com/cgi-bin/m?ci=us-905417h&amp;cg=0&amp;cc=1&amp;ts=noscript"" width=""1"" />
</div>
</noscript>
<!-- END Nielsen Online SiteCensus V6.0 -->


</div>
    
        



<div id=""third_party_trackers"">



          <script src=""http://www.bloomberg.com/blp2/v/20160511_134940/onlineopinionOO4S/oo_engine_c_all-min.js"" type=""text/javascript""></script>  
  



        <script type='text/javascript'>
          var _sf_async_config={};
          _sf_async_config.uid = 15087;
          _sf_async_config.domain = 'bloomberg.com';
          _sf_async_config.sections = '';
          _sf_async_config.authors = '';
          (function(){
            function loadChartbeat() {
              window._sf_endpt=(new Date()).getTime();
              var e = document.createElement('script');
              e.setAttribute('language', 'javascript');
              e.setAttribute('type', 'text/javascript');
              e.setAttribute('src',
                 (('https:' == document.location.protocol) ? 'https://a248.e.akamai.net/chartbeat.download.akamai.com/102508/' : 'http://static.chartbeat.com/') +
                 'js/chartbeat.js');
              document.body.appendChild(e);
            }
            var oldonload = window.onload;
            window.onload = (typeof window.onload != 'function') ?
               loadChartbeat : function() { oldonload(); loadChartbeat(); };
          })();
      </script>
  
  </div>
 <script type=""text/javascript"">
 $(Bcom.init);
 </script>

<!--
  production
  0.053586 : 2016-05-19 16:16:17 -0400
-->
                    <script charset=""windows-1252"" src=""http://www.bloomberg.com/blp2/v/20160511_134940/onlineopinionv5/oo_engine_all-min.js"" type=""text/javascript""></script>    <link href=""http://cdn.gotraffic.net/projector/v0.7.97.1/bvp.css"" media=""screen"" rel=""stylesheet"" type=""text/css"" /><script src=""http://cdn.gotraffic.net/projector/v0.7.97.1/video.js"" type=""text/javascript""></script>     
    <script src=""http://www.bloomberg.com/blp2/v/20160511_134940/javascripts/foresee/foresee-trigger.js"" type=""text/javascript""></script>
  <script type=""text/javascript"">
      (function(w, d) { var a = function() { var a = d.createElement('script'); a.type = 'text/javascript';
          a.async = 'async'; a.src = '//' + ((w.location.protocol === 'https:') ? 's3.amazonaws.com/cdx-radar/' :
                  'radar.cedexis.com/') + '01-12403-radar10.min.js'; d.body.appendChild(a); };
          if (w.addEventListener) { w.addEventListener('load', a, false); }
          else if (w.attachEvent) { w.attachEvent('onload', a); }
      }(window, document));
  </script>
</body>
</html>

";
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var testSubject = new BloombergSymbolSearch(null);
            var testResult = testSubject.ParseContent(_testData);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Any());

        }

        [TestMethod]
        public void TestPreParser()
        {
            var testFile = TestAssembly.UnitTestsRoot + @"\Rand\BloombergSearchRslt_multiple.html";

            var testInput = System.IO.File.ReadAllText(testFile);
            var testResult = NfHtmlDynDataBase.PreParser(testInput);

            Assert.AreNotEqual(testInput.Length, testResult.Length);

            System.IO.File.WriteAllText(TestAssembly.UnitTestsRoot + @"\Rand\BloombergSearchRslt_tr.html", testResult);

        }
    }
}
