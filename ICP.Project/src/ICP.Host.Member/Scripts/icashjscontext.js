/*
icashJSContext
2019.06
*/
var iCashJSContext = iCashJSContext || {};

(function (window , document) {
    'use strict';


    var version = "1";
    var namespace = "JSContext"
    window["__functionIndexMap"] = {};
    function trace(msg){
        if(window.console)
            window.console.log.apply(console, arguments);

    };
    var isMobile = {
        Android: function() {
            return navigator.userAgent.match(/Android/i);
        },
        BlackBerry: function() {
            return navigator.userAgent.match(/BlackBerry/i);
        },
        iOS: function() {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i);
        },
        Opera: function() {
            return navigator.userAgent.match(/Opera Mini/i);
        },
        Windows: function() {
            return navigator.userAgent.match(/IEMobile/i);
        },
        any: function() {
            return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
        }
    };
    iCashJSContext.callScheme = function(scheme_url, download_url, timeout = 300) {
        window.location = scheme_url;
    }
    iCashJSContext.openOpenPoint = function() {
        iCashJSContext.callScheme("openpoint://")
    }

    iCashJSContext.call = function(functionName , args , callback ){

        var wrap = {
                "method": functionName,
                "params": args || null
        };

        if (callback) {
            var callbackFuncName;
            if (typeof callback == 'function') {
                callbackFuncName = createCallbackFunction(functionName + "_" + "callback", callback);
            } else {
                callbackFuncName = callback;
            }
            wrap["callback"] = callbackFuncName
        }else{
            wrap["callback"] = null
        }

        try {
            // 
            //iOS
            if(isMobile.iOS()){
                window.webkit.messageHandlers[namespace].postMessage(JSON.stringify(wrap));
            }
            //Android
            if(isMobile.Android()){
                // var str = functionName + "("+JSON.stringify(wrap["params"])+","+wrap["callback"]+")"
                var str;
                if(typeof wrap["params"] === 'string' || typeof wrap["params"]=== 'number'){
                    str = wrap["params"].toString()
                }else{
                    str = JSON.stringify(wrap["params"])
                }
                window[namespace][functionName]( str , wrap["callback"]);
                
            }
            
        }catch(error){
            console.log(error)
        }

    }

    function createCallbackFunction(funcName, callbackFunc) {
        if (callbackFunc && callbackFunc.name != null && callbackFunc.name.length > 0) {
            return callbackFunc.name;
        }

        if (typeof window[funcName + 0] != 'function') {
            window[funcName + 0] = callbackFunc;
            __functionIndexMap[funcName] = 0;
            return funcName + 0
        } else {
            var maxIndex = __functionIndexMap[funcName];
            var newIndex = ++maxIndex;
            window[funcName + newIndex] = callbackFunc;
            return funcName + newIndex;
        }
    }


    /*-------------
    * 快取處理
    * 可自行調整快取方案
    * 參考資料 https://blog.patw.me/archives/821/talking-about-the-practice-of-third-party-script-cache-to-automatically-upgrade/
    * -------------*/
    // function insertScript(url) {
    //   var s1 = document.createElement("script");
    //   s1.async = true;
    //   s1.src = url;
    //   var s0 = document.getElementsByTagName("script")[0];
    //   s0.parentNode.insertBefore(s1, s0);
    // }
    //JS載入 init處理
    var pCallBack = window.window["iCashJSContextAsyncInit"];
    if(pCallBack && typeof pCallBack == "function" ){
        pCallBack();
    };


    var doUpdate = function() {
        if ( "undefined" === typeof(document.body) || !document.body ) {
          setTimeout(doUpdate, 500);
        }
        else {
            //這段就會每次檢查快取
            var scripts= document.getElementsByTagName('script');
            var tscript = scripts["icash-JSContext"]; //scripts[scripts.length-1]

            if(tscript){
                var path= tscript.src.split('?')[0];      // remove any ?query
                var myjsdir= path.split('/').slice(0, -1).join('/')+'/';

                var html = '<!doctype html><html lang="tw"><head><script src="'+tscript.src+'"></script></head><body><script>if (location.hash === "") { location.hash = "check"; location.reload(true);}</script></body></html>';
                // IFrame
                var iframe = document.createElement('iframe');
                iframe.src = 'data:text/html;charset=utf-8,' + encodeURI(html);
                iframe.style.display = "none";
                document.body.appendChild(iframe);
            }
        }
    };
    doUpdate();

}(typeof window === 'undefined' ? this : window , document));