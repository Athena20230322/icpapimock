(function () {
    //function refresh() {
    //    $('#refreshForm input[type="submit"]:first').click();
    //}

    function queryStr2Obj(search) {
        return JSON.parse('{"' + search.replace(/&/g, '","').replace(/=/g, '":"') + '"}', function (key, value) { return key === "" ? value : decodeURIComponent(value) });
    }

    //function lnkCallBack(result) {
    //    if (result.RtnCode == 1) {
    //        if (!result.RtnMsg) result.RtnMsg = '更新成功';
    //        libs.alert.popup(result.RtnMsg, false, refresh);
    //    }
    //    else {
    //        if (!result.RtnMsg) result.RtnMsg = '更新失敗';
    //        libs.alert.popup(result.RtnMsg);
    //    }
    //}

    function href2AjaxPost(href, cb) {
        $.ajax({
            url: href,
            method: 'POST',
            success: cb
        });
    }

    //查詢
    $('#lnkQuery').click(function () {
        var $btnQuery = $('#btnQuery');
        $('#queryPageNo').val('1');
        //將 查詢條件 複製至 更新條件
        //var $queryFields = $btnQuery.parents('form:first').find('[name]');
        //var $refreshFields = $('#refreshForm [name]');
        //$queryFields.each(function () {
        //    $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
        //});
        //$refreshFields.find('input[name="PageNo"]').val('1');
        $btnQuery.click();
        return false;
    });

    //清除條件
    $('#lnkReset').click(function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });

    //$('#queryResult')
    //    //將 頁碼 複製至 更新條件
    //    .on('click', '.sc-pagenum-box a[href]', function () {
    //        var search = this.search.substring(1);
    //        var query = queryStr2Obj(search);
    //        $('#refreshForm input[name="PageNo"]').val(query.PageNo);
    //    })
    //    //審核失敗, 審核成功, 身份驗證
    //    .on('click', 'a.btn-LPAuth-fail, a.btn-LPAuth-success, a.btn-IDN-verify', function () {
    //        href2AjaxPost(this.href, lnkCallBack);
    //        return false;
    //    })
    //    ;



    $('#lnkQuery').click();
})();