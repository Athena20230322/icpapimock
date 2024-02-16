function query() {
    var minD = $('#queryCreateDateBegin').val();
    var maxD = $('#queryCreateDateEnd').val();
    var $CellPhone = $('#queryCellPhone').val();
    var $Email = $('#queryEmail').val();
    var $IDNO = $('#queryIDNO').val();

    $('#refreshCellPhone').val($CellPhone);
    $('#refreshEmail').val($Email);
    $('#refreshIDNO').val($IDNO);
    $('#refreshCreateDateBegin').val(minD);
    $('#refreshCreateDateEnd').val(maxD);
    

    //var IP = $('#IP').val();
    $('#btnQuery').click();

}


//新增完畫面顯示新增的那筆資料
function trigger(data) {
    var $CellPhone = data.CellPhone;
    $('#refreshCellPhone').val($CellPhone);
    $('#refreshForm input[type="submit"]:first').click();
}

function refresh() {
    $('#refreshForm input[type="submit"]:first').click();
}

(function () {
    function monthDiff(d1, d2) {
        var months;
        months = (d2.getFullYear() - d1.getFullYear()) * 12;
        months -= d1.getMonth() + 1;
        months += d2.getMonth();
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

    //查詢
    $('#lnkQuery').click(function () {
        var minD = $('#queryCreateDateBegin').val();
        var maxD = $('#queryCreateDateEnd').val();
        var $CellPhone = $('#queryCellPhone').val();
        var $Email = $('#queryEmail').val();
        var $IDNO = $('#queryIDNO').val();
        $('#refreshCellPhone').val($CellPhone);
        $('#refreshEmail').val($Email);
        $('#refreshIDNO').val($IDNO);
        $('#refreshCreateDateBegin').val(minD);
        $('#refreshCreateDateEnd').val(maxD);

        //檢核輸入的欄位
        if (monthDiff(new Date(minD), new Date(maxD)) >= 6) {
            var confirmCallback = query;
            var content = '所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間！';
            var isHtml = true;

            libs.alert.confirm(content, confirmCallback, isHtml);
            return false;
        }
        else if ($CellPhone != "") {
            var regex = /^09[0-9]{8}$/;
            if (!regex.test($CellPhone)) {
                var content = '請輸入正確的手機號碼';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($Email != "") {
            var regex = /^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})*$/;
            if (!regex.test($Email)) {
                var content = '請輸入正確的電子郵件';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($IDNO != "")
        {
            var regex = /^[A-Z]{1}[0-9]{9}$/;
            if (!regex.test($IDNO)) {
                var content = '請輸入正確的身分證字號';
                libs.alert.popup(content);
                return false;
            }
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

    $('#ExportCSV').click(function () {
        
        var StartDate = $('#queryCreateDateBegin').val();
        var EndDate = $('#queryCreateDateEnd').val();
        var CellPhone = $('#queryCellPhone').val();
        var IDNO = $('#queryIDNO').val();
        var Email = $('#queryEmail').val();

        location.href = '/CustomerSecurityManage/OTPBlackListExportCSV?StartDate=' + StartDate + '&EndDate=' + EndDate + '&CellPhone=' + CellPhone + '&IDNO=' + IDNO + '&Email=' + Email;
        
    });

    //清除條件
    $('#lnkReset').click(function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });

    $('#queryResult')
        //將 頁碼 複製至 更新條件
        .on('click', 'sc-pagenum-box a[href]', function () {
            var search = this.search.substring(1);
            var query = queryStr2Obj(search);
            $('#refreshForm input[name="PageNo"]').val(query.PageNo);
        })
})();