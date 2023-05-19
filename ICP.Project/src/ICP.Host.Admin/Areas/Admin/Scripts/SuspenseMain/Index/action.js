function query() {
    var $btnQuery = $('#btnQuery');

    //將 查詢條件 複製至 更新條件
    var $queryFields = $btnQuery.parents('form:first').find('[name]');
    var $refreshFields = $('#refreshForm [name]');
    $queryFields.each(function () {
        $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
    });
    $refreshFields.find('input[name="PageNo"]').val('1');

    let cellPhonePattern = /^09[0-9]{8}$/;
    let cellPhone = $('#CellPhone:first').val();
    if (cellPhone !== '' && !cellPhonePattern.test(cellPhone)) {
        var content = '請輸入正確的手機號碼';
        libs.alert.popup(content);
        return false;
    }

    let emailPattern = /^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})*$/;
    let email = $('#Email:first').val();
    if (email !== '' && !emailPattern.test(email)) {
        var content = '請輸入正確的電子郵件';
        libs.alert.popup(content);
        return false;
    }

    let idnoPattern = /^[A-Z]{1}[0-9]{9}$/;
    let uniformPattern = /^(?=.*[A-Z]{1}[A-Z]{1}[0-9]{8}).{10}$/;
    let idno = $('#IDNO:first').val();
    if (idno !== '' && !idnoPattern.test(idno) && !uniformPattern.test(idno)) {
        var content = '請輸入正確的身分證字號';
        libs.alert.popup(content);
        return false;
    }

    $('#QueryForm').submit();
}

function refresh() {
    $('#refreshForm input[type="submit"]:first').click();
}

(function () {
    function monthDiff(d1, d2) {
        var months;
        months = (d2.getFullYear() - d1.getFullYear()) * 12;
        months -= d1.getMonth() + 1;
        months += d2.getMonth() + 1;
        return months <= 0 ? 0 : months;
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
            var confirmCallback = query;
            var content = '所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間！';
            var isHtml = true;

            libs.alert.confirm(content, confirmCallback, isHtml);
            return false;
        }

        confirmCallback();
        return false;
    });

    $('#btnExport').click(function () {
        var startDate = $('#queryStartDate').val();
        var endDate = $('#queryEndDate').val();
        var cellPhone = $('#CellPhone').val();
        var email = $('#Email').val();
        var idno = $('#IDNO').val();

        if (!startDate && !endDate && !cellPhone && !email && !idno) {
            libs.alert.popup('請先輸入查詢條件');
            return false;
        }

        location.href = '/SuspenseMain/GetSuspenseMainExport?StartDate=' + startDate + '&EndDate=' + endDate + '&CellPhone' + cellPhone + '&Email' + email + '&IDNO' + idno
    });

    $('#queryResult')
        //將 頁碼 複製至 更新條件
        .on('click', '.sc-pagenum-box a[href]', function () {
            var search = this.search.substring(1);
            var query = queryStr2Obj(search);
            $('#refreshForm input[name="PageNo"]').val(query.PageNo);
        })
        .on('click', 'a.btn-unlock', function () {
            href2AjaxPost(this.href, lnkCallBack);
            return false;
        });
})();