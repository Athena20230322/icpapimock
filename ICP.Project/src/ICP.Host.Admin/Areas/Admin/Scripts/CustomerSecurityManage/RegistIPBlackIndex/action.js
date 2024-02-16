function query() {

    $('#btnQuery').click();

}

//function AddOK()
//{    
//    if (data.RtnCode == 1)
//    {
//        $('#IP').val(data.responseText);
//        $('#btnQuery').click();
//    }
    
//}

(function () {


    function refresh() {
        $('#refreshForm input[type="submit"]:first').click();
    }

    function queryStr2Obj(search) {
        return JSON.parse('{"' + search.replace(/&/g, '","').replace(/=/g, '":"') + '"}', function (key, value) { return key === "" ? value : decodeURIComponent(value) });
    }

    //檢查必填欄位
    function checkQry() {
        var $IP = $('#queryIP').val();
        if ($IP == "") {
            var content = '請輸入完整的IP';
            var isHtml = true;

            libs.alert.popup(content, isHtml, false);
            return false;
        }

        $('#refreshIP').val($IP);

        return true;
    }       

    //查詢
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

    $('#ExportCSV').click(function () {
        if (!checkQry()) {
            return false;
        }

        var IP = $('#refreshIP').val();  

        location.href = '/CustomerSecurityManage/RegistBlackListExportCSV?IP=' + IP ;
        
    });

    //$('a.btn-add').click(function () {
    //    $.ajax({
    //        url: "CustomerSecurityManage/IPBlackAdd",
    //        type: 'post',
    //        datatype: 'application/json',
    //        data: form.serialize(),
    //        success: function (data) {
    //            $('#IP').val(data.responseText);
    //            $('#btnQuery').click();
    //        },
    //        error: function () {
    //            alert('Ajax error!');
    //        }
    //    });
    //});

    $('#queryResult')
        //將 頁碼 複製至 更新條件
        .on('click', 'sc-pagenum-box a[href]', function () {
            var search = this.search.substring(1);
            var query = queryStr2Obj(search);
            $('#refreshForm input[name="PageNo"]').val(query.PageNo);
        })

})();