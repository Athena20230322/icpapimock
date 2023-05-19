function query() {    
    $('#btnQuery').click();
}


//新增完畫面顯示新增的那筆資料
function trigger(data) {
    var $ICPMID = data.ICPMID;
    $('#refreshICPMID').val($ICPMID);
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
   
    //清除條件
    $('#lnkReset').click(function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });

    //function checkQry() {
    //    var $IDNO = $('#queryIDNO').val();
    //    if ($IDNO == "") {
    //        var content = '請輸入正確的身分證字號/居留證';
    //        var isHtml = true;

    //        libs.alert.popup(content, isHtml, false);
    //        return false;
    //    }

    //    $('#refreshIDNO').val($IDNO);

    //    return true;
    //}    

    $('#ExportCSV').click(function () {
        var $CellPhone = $('#queryCellPhone').val();
        var $Email = $('#queryEmail').val();
        var $IDNO = $('#queryIDNO').val();
        var $ICPMID = $('#queryICPMID').val();
        var $StartDate = $('#refreshCreateDateBegin').val();
        var $EndDate = $('#refreshCreateDateEnd').val();

        $('#refreshCellPhone').val($CellPhone);
        $('#refreshEmail').val($Email);
        $('#refreshIDNO').val($IDNO);
        $('#refreshICPMID').val($ICPMID);

        location.href = '/CustomerSecurityManage/TakeCashLimitListExportCSV?StartDate=' + $StartDate + '&EndDate=' + $EndDate + '&CellPhone=' + $CellPhone + '&Email=' + $Email + '&IDNO=' + $IDNO + '&ICPMID=' + $ICPMID;

    });

    $('#lnkQuery').click(function () {


        var $CellPhone = $('#queryCellPhone').val();
        var $Email = $('#queryEmail').val();
        var $IDNO = $('#queryIDNO').val();
        var $ICPMID = $('#queryICPMID').val();
        var minD = $('#queryCreateDateBegin').val();
        var maxD = $('#queryCreateDateEnd').val();


        $('#refreshCellPhone').val($CellPhone);
        $('#refreshEmail').val($Email);
        $('#refreshIDNO').val($IDNO);
        $('#refreshICPMID').val($ICPMID);
        $('#refreshCreateDateBegin').val(minD);
        $('#refreshCreateDateEnd').val(maxD);

        if (monthDiff(new Date(minD), new Date(maxD)) >= 6) {
            var confirmCallback = query;
            var content = '所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間！';
            var isHtml = true;

            libs.alert.confirm(content, confirmCallback, isHtml);
            return false;
        }
        else if ($ICPMID != "")
        {
            var regex = /[0-9]{16}$/;
            if (!regex.test($ICPMID)) {
                var content = '請輸入正確的電支帳號';
                libs.alert.popup(content);
                return false;
            }
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
        else if ($IDNO != "") {
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

    $('#queryResult')
        //將 頁碼 複製至 更新條件
        .on('click', '.sc-pagenum-box a[href]', function () {
            var search = this.search.substring(1);
            var query = queryStr2Obj(search);
            $('#refreshForm input[name="PageNo"]').val(query.PageNo);
        })
})();