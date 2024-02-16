function query() {
    $('#QueryForm').submit();
}

function IDTypesChange(value) {
    if (value == '0') {
        $('#IDNO_TextBlock').show();
        $('#UnifiedBusinessNo_TextBlock').hide();
        //清空值
        $('#queryUnifiedBusinessNo').val('');
    }
    else {
        $('#IDNO_TextBlock').hide();
        $('#UnifiedBusinessNo_TextBlock').show();
        $('#queryIDNO').val('');
    }
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

function QueryCheck() {
    var ICPMID = $('#queryICPMID').val();
    if (ICPMID != '') {
        var regex = /^[0-9]{16}$/;
        if (!regex.test(ICPMID)) {
            var content = '電支帳號格式錯誤';
            libs.alert.popup(content);
            return false;
        }
    }
    var IDNO = $('#queryIDNO').val();
    if (IDNO != '') {
        var regex = /^[A-Z]{1}[0-9]{9}$/;
        if (!regex.test(IDNO)) {
            var content = '身分證字號格式錯誤';
            libs.alert.popup(content);
            return false;
        }
    }
    var UnifiedBusinessNo = $('#queryUnifiedBusinessNo').val();
    if (UnifiedBusinessNo != '') {
        var regex = /^[0-9]{8}$/;
        if (!regex.test(UnifiedBusinessNo)) {
            var content = '統一編號格式錯誤';
            libs.alert.popup(content);
            return false;
        }
    }
    return true;
}

(function () {
    $('#lnkQuery').click(function () {
        if (QueryCheck()) {
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
        }
    });

    $('#queryResult')
        //將 頁碼 複製至 更新條件
        .on('click', '.sc-pagenum-box a[href]', function () {
            var search = this.search.substring(1);
            var query = queryStr2Obj(search);
            $('#refreshForm input[name="PageNo"]').val(query.PageNo);
        });

    //清除條件
    $('#lnkReset').click(function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });
})();
