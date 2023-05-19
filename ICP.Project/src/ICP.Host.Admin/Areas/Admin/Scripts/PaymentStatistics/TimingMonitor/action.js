(function () {

    function refresh() {
        $('#refreshForm input[type="submit"]:first').click();
    }

    function queryStr2Obj(search) {
        return JSON.parse('{"' + search.replace(/&/g, '","').replace(/=/g, '":"') + '"}', function (key, value) { return key === "" ? value : decodeURIComponent(value) });
    }

    function href2AjaxPost(href, cb) {
        $.ajax({
            url: href,
            method: 'POST',
            success: cb
        });
    }

    //### 初始化
    Init();
    
    //查詢
    $('#lnkQuery').click(function () {        

        //### 檢核電支帳戶格式
        var regICPMID = /^[0-9]{16}$/;

        if ($('#ICPMID').val() != '' && !regICPMID.test($('#ICPMID').val(), regICPMID)) {
            alert('請輸入正確的電支帳號');
            return false;
        }

        //### 檢核需為數字格式的欄位
        var regOnlyNumber = /^[0-9]+$/;

        if (
            ($('#Day1Waring').val() != '' && !regOnlyNumber.test($('#Day1Waring').val(), regOnlyNumber)) ||
            ($('#Day10Waring').val() != '' && !regOnlyNumber.test($('#Day10Waring').val(), regOnlyNumber)) ||
            ($('#Day30Waring').val() != '' && !regOnlyNumber.test($('#Day30Waring').val(), regOnlyNumber)) ||
            ($('#Day1Amount').val() != '' && !regOnlyNumber.test($('#Day1Amount').val(), regOnlyNumber)) ||
            ($('#Day10Amount').val() != '' && !regOnlyNumber.test($('#Day10Amount').val(), regOnlyNumber)) ||
            ($('#Day30Amount').val() != '' && !regOnlyNumber.test($('#Day30Amount').val(), regOnlyNumber)) ||
            ($('#TradeContent').val() != '' && !regOnlyNumber.test($('#TradeContent').val(), regOnlyNumber))
        ) {
            alert('監控條件數字格式錯誤');
            return false;
        }

        $('#SortType').val($('input[name=qrySortType]:checked').val());
        $('#SelectMode').val($('#qrySelectMode').is(":checked"));
        $('#MonitorStatus').val($('#qryMonitorStatus').is(":checked"));
        $('#WithdrawStatus').val($('#qryWithdrawStatus').is(":checked"));

        var $btnQuery = $('#btnQuery');

        //將 查詢條件 複製至 更新條件
        var $queryFields = $btnQuery.parents('form:first').find('[name]');
        var $refreshFields = $('#refreshForm [name]');
        $queryFields.each(function () {
            //alert(this.name + ';' + this.value);
            $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
        });
        $refreshFields.find('input[name="PageNo"]').val('1');
        $btnQuery.click();
        return false;
    });

    //清除條件
    $('#lnkReset').click(function () {

        $(this).parents('form:first')[0].reset();

        var now = new Date();
        var preDay = new Date();
        preDay.setDate(now.getDate() - 1);              
        var preday = preDay.getFullYear() + "-" + ("0" + (preDay.getMonth() + 1)).slice(-2) + "-" + ("0" + preDay.getDate()).slice(-2);
        
        $("#queryStartDate").flatpickr({
            allowInput: false,
            defaultDate: preday,
            altInputClass: "flatpickr flatpickr-input active normalD",
            locale: "zh"
        });

        return false;
    });

    $('#queryResult')
    //將 頁碼 複製至 更新條件
    .on('click', 'sc-pagenum-box a[href]', function () {
        var search = this.search.substring(1);
        var query = queryStr2Obj(search);
        $('#refreshForm input[name="PageNo"]').val(query.PageNo);
    })

    //定時監控 觀察名單切換
    function lhpSwitch() {
        if ($('#lps_1').is(':checked')) {
            $('.pb-open').show().siblings('.pld-box').hide();
        } else if ($('#lps_2').is(':checked')) {
            $('.pb-close').show().siblings('.pld-box').hide();
        } else {
            $('.pld-box').hide();
        }
    }
    lhpSwitch();
    $('.lhp-switch').find("input:radio").on('change', function () {
        lhpSwitch();
    });


    //tab
    $('.tab-wrap').each(function () {
        var $tab = $(this);
        var $def_tab = $('.tabs li', $tab).eq(0).addClass('active');
        $($def_tab.data('tab')).siblings().hide();
        $('.tabs li', $tab).click(function () {
            var dataTab = $(this).data('tab');
            $(this).addClass('active').siblings('.active').removeClass('active');
            $(dataTab).stop(false, true).fadeIn(600).siblings().hide();
            return false;
        }).focus(function () {
            this.blur();
        });
    });
    
})();

//### 控制項初始化
function Init() {
    $("input[name='qrySortType'][value='1']").attr("checked", true);
    SetRuleMode(1);
}

function SetRuleMode(val) {
    $('#RuleMode').val(val);
    return false;
}

