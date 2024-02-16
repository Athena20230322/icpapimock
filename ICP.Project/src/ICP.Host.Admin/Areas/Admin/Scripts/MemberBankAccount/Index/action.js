function query() {
    var $btnQuery = $('#btnQuery');

    //將 查詢條件 複製至 更新條件
    var $queryFields = $btnQuery.parents('form:first').find('[name]');
    var $refreshFields = $('#refreshForm [name]');
    $queryFields.each(function () {
        $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
    });
    $refreshFields.find('input[name="PageNo"]').val('1');

    let numberPattern = /^\d+$/;
    let ICPMID = $('#ICPMID:first').val();
    if (ICPMID !== '' && !numberPattern.test(ICPMID)) {
        var content = '電支帳號格式錯誤';
        libs.alert.popup(content);
        return false;
    }

    let cName = $('#CName:first').val();
    if (cName != "") {
        var regex = /^['•\u4E00-\u9FA5\uF900-\uFA2D]{2,20}/;

        if (!regex.test(cName)) {
            var content = '會員姓名格式錯誤，請輸入2-20個字，不可包含空格，符號僅可接受「•」';
            libs.alert.popup(content);
            return false;
        }
        else {
            var arry = cName.split("");
            console.log(arry.length);
            if (arry.length >= 3) {
                for (var i = 0; i < arry.length - 2; i++) {
                    if (arry[i] == arry[i + 1] && arry[i + 1] == arry[i + 2]) {
                        var content = '會員姓名格式錯誤，不接受連續三個重複字';
                        libs.alert.popup(content);
                        return false;
                    }
                }
            }
        }
    }

    $('#QueryForm').submit();
}

function refresh() {
    $('#refreshForm input[type="submit"]:first').click();
}

(function () {
    if ($('queryICPMID').val != "") {
        $('form').submit();
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

    function href2AjaxPost(href, cb) {
        $.ajax({
            url: href,
            method: 'POST',
            success: cb
        });
    }

    $('.clear-form').on('click', function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });

    $('#lnkQuery').click(function () {
        var minD = $('.minD').val();
        var maxD = $('.maxD').val();
        var confirmCallback = query;

        if (monthDiff(new Date(minD), new Date(maxD)) >= 6) {
            var content = '所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間！';
            var isHtml = true;

            libs.alert.confirm(content, confirmCallback, isHtml);
            return false;
        }
        
        confirmCallback();
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
})();
