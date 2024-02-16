function query() {    
    $('#btnQuery').click();
}

//新增完畫面顯示新增的那筆資料
function trigger(data) {
    var $IDNO = data.IDNO;
    $('#refreshIDNO').val($IDNO);
    $('#refreshForm input[type="submit"]:first').click();
}

function refresh() {
    $('#refreshForm input[type="submit"]:first').click();
}

(function () {
    
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

    function checkQry() {
        var $IDNO = $('#queryIDNO').val();
        if ($IDNO == "") {
            var content = '請輸入正確的身分證字號/居留證';
            var isHtml = true;

            libs.alert.popup(content, isHtml, false);
            return false;
        }
        else
        {
            var regex = /^[A-Z]{1}[0-9]{9}$/;
            var regexUniformID = /^(?=.*[A-Z]{1}[A-Z]{1}[0-9]{8}).{10}$/;
            if (!regex.test($IDNO) && !regexUniformID.test($IDNO)) {
                var content = '請輸入正確的身分證字號/居留證';
                libs.alert.popup(content);
                return false;
            }            
        }

        $('#refreshIDNO').val($IDNO);

        return true;
    }    

    $('#ExportCSV').click(function () {
        if (!checkQry()) {
            return false;
        }

        var IDNO = $('#refreshIDNO').val();

        location.href = '/CustomerSecurityManage/IDNOBlackListExportCSV?IDNO=' + IDNO;

    });

    $('#lnkQuery').click(function () {

        if (!checkQry()) {
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
})();