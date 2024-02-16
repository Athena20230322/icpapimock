function query() {
    $('#QueryForm').submit();
}

function refresh() {
    $('#refreshForm input[type="submit"]:first').click();
}

function monthDiff(d1, d2) {
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth() + 1;
    months += d2.getMonth() + 1;
    return months <= 0 ? 0 : months;
}

function queryStr2Obj(search) {
    return JSON.parse('{"' + search.replace(/&/g, '","').replace(/=/g, '":"') + '"}', function (key, value) { return key === "" ? value : decodeURIComponent(value) });
}

function lnkCallBack(result) {
    if (result.RtnCode == 1) {
        if (!result.RtnMsg) result.RtnMsg = '更新成功';
        libs.alert.popup(result.RtnMsg, false, refresh);
    }
    else {
        if (!result.RtnMsg) result.RtnMsg = '更新失敗';
        libs.alert.popup(result.RtnMsg);
    }
}

function QueryCheck() {
    var CellPhone = $('#queryCellPhone').val();
    var ICPMID = $('#queryICPMID').val();
    var CName = $('#queryCName').val();
    var Email = $('#queryEmail').val();
    var TradeNo = $('#queryTradeNo').val();
    var CaseNo = $('#queryCaseNo').val();
    //檢核輸入的欄位        
    if (CName != "") {
        var regex = /^['•\u4E00-\u9FA5\uF900-\uFA2D]+$/;
        if (!regex.test(CName)) {
            var content = '姓名格式錯誤，請重新輸入';
            libs.alert.popup(content);
            return false;
        }
        if (CName.length < 2) {
            var content = '姓名格式錯誤，請重新輸入';
            libs.alert.popup(content);
            return false;
        }
        if (CName.length > 20) {
            var content = '姓名格式錯誤，請重新輸入';
            libs.alert.popup(content);
            return false;
        }
    }
    if (CellPhone != "") {
        var regex = /^[0-9]+$/;
        if (!regex.test(CellPhone)) {
            var content = '手機號碼格式錯誤，請重新輸入';
            libs.alert.popup(content);
            return false;
        }
    }
    if (ICPMID != "") {
        var regex = /[0-9]{16}$/;
        if (!regex.test(ICPMID)) {
            var content = '電支帳號格式錯誤，請重新輸入';
            libs.alert.popup(content);
            return false;
        }
    }
    if (Email != "") {
        var regex = /^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})*$/;
        if (!regex.test(Email)) {
            var content = 'E-mail格式錯誤，請重新輸入';
            libs.alert.popup(content);
            return false;
        }
    }
    if (TradeNo != "") {
        var regex = /^[a-zA-Z0-9]+$/;
        if (!regex.test(TradeNo)) {
            var content = '訂單編號格式錯誤，請重新輸入';
            libs.alert.popup(content);
            return false;
        }
    }
    if (CaseNo != "") {
        var regex = /^[0-9]+$/;
        if (!regex.test(CaseNo)) {
            var content = '案件編號格式錯誤，請重新輸入';
            libs.alert.popup(content);
            return false;
        }
    }
    return true;
}
(function () {
    $('#lnkQuery').click(function () {
        //檢查是否超過六個月
        var minD = $('.minD').val();
        var maxD = $('.maxD').val();
        if (monthDiff(new Date(minD), new Date(maxD)) >= 6) {
            var confirmCallback = query;
            var content = '所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間！';
            var isHtml = true;
            libs.alert.confirm(content, confirmCallback, isHtml);
            return false;
        }
        var $btnQuery = $('#btnQuery');

        //將 查詢條件 複製至 更新條件
        var $queryFields = $btnQuery.parents('form:first').find('[name]');
        var $refreshFields = $('#refreshForm [name]');
        $queryFields.each(function () {
            $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
        });
        $refreshFields.find('input[name="PageNo"]').val('1');
        $btnQuery.click();
        return false;
    });

    $('#queryResult')
        //將 頁碼 複製至 更新條件
        .on('click', '.sc-pagenum-box a[href]', function () {
            var search = this.search.substring(1);
            var query = queryStr2Obj(search);
            $('#refreshForm input[name="PageNo"]').val(query.PageNo);
        })
        //審核失敗, 審核成功
        .on('click', 'a.btn-LPAuth-fail, a.btn-LPAuth-success', function () {
            href2AjaxPost(this.href, lnkCallBack);
            return false;
        });

    //清除條件
    $('#lnkReset').click(function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });
})();
