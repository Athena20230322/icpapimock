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
    //檢查時間+其中任一條件有值
    var StartDate = $('#querStartDate').val();
    var EndDate = $('#queryEndDate').val();
    if(StartDate == '' || EndDate =='')
        return false;
    var Account = $('#queryAccount').val();
    var UserIP = $('#queryUserIP').val();
    var ICPMID = $('#queryICPMID').val();
    var CellPhone = $('#queryCellPhone').val();
    var DeviceID = $('#queryDeviceID').val();
    if (Account == '' && UserIP == '' && ICPMID == '' && CellPhone == '' && DeviceID == '') {
        alert('查詢時間需搭配至少一種搜尋條件');
        return false;
    }
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
