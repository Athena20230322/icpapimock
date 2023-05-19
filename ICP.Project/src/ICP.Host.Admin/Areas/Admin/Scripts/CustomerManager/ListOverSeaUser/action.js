function query() {
    var minD = $('#queryStartDate').val();
    var maxD = $('#queryEndDate').val();
    var $CellPhone = $('#queryCellPhone').val();
    var $CName = $('#queryCName').val();
    var $UniformID = $('#queryUniformID').val();

    $('#refreshCellPhone').val($CellPhone);
    $('#refreshCName').val($CName);
    $('#refreshUniformID').val($UniformID);
    $('#refreshStartDate').val(minD);
    $('#refreshEndDate').val(maxD);
    

    $('#btnQuery').click();

}

function refresh() {
    var pageNo = $('.spb-list li.active a').html();
    $('#refreshForm input[name="PageNo"]').val(pageNo);
    $('#refreshForm input[type="submit"]:first').submit();
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

    function monthDiff(d1, d2) {
        var months;
        months = (d2.getFullYear() - d1.getFullYear()) * 12;
        months -= d1.getMonth() + 1;
        months += d2.getMonth();
        return months <= 0 ? 0 : months;
    }

    //查詢
    $('#lnkQuery').click(function () {
        var minD = $('#queryStartDate').val();
        var maxD = $('#queryEndDate').val();
        var $CellPhone = $('#queryCellPhone').val();
        var $CName = $('#queryCName').val();
        var $UniformID = $('#queryUniformID').val();
        $('#refreshCellPhone').val($CellPhone);
        $('#refreshCName').val($CName);
        $('#refreshIDNO').val($UniformID);
        $('#refreshStartDate').val(minD);
        $('#refreshEndDate').val(maxD);

        //檢核輸入的欄位
        if (monthDiff(new Date(minD), new Date(maxD)) >= 6) {
            var confirmCallback = query;
            var content = '所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間！';
            var isHtml = true;

            libs.alert.confirm(content, confirmCallback, isHtml);
            return false;
        }

        if ($CellPhone != "") {
            var regex = /^09[0-9]{8}$/;
            if (!regex.test($CellPhone)) {
                var content = '請輸入正確的手機號碼';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($UniformID != "") {
            var regex = /^(?=.*[A-Z]{1}[A-Z]{1}[0-9]{8}).{10}$/;
            if (!regex.test($UniformID)) {
                var content = '請輸入正確的居留證字號';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($CName != "") {
            var regex = /^['•\u4E00-\u9FA5\uF900-\uFA2D]{2,20}/;
            
            if (!regex.test($CName)) {
                var content = '會員姓名格式錯誤，請輸入2-20個字，不可包含空格，符號僅可接受「•」';
                libs.alert.popup(content);
                return false;
            }
            else {
                var arry = $CName.split("");
                console.log(arry.length);
                if (arry.length >= 3)
                {
                    for (var i = 0; i < arry.length-2; i++) {
                        if (arry[i] == arry[i + 1] && arry[i + 1] == arry[i + 2])
                        {
                            var content = '會員姓名格式錯誤，不接受連續三個重複字';
                            libs.alert.popup(content);
                            return false;
                        }
                    }
                }    
                
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


    $("#queryResult").on('change', "#si_all", function () {
        if (this.checked) {
            $(this).parents('.sc-box').find("input[type='checkbox']").prop("checked", true);
        }
        else {
            $(this).parents('.sc-box').find("input[type='checkbox']").prop("checked", false);
        }
    });

    $("#queryResult").on('change', "input[type='checkbox']", function () {

        if ($('#si_all').prop("checked") == true && this.id != "si_all" && this.checked == false) {
            $('#si_all').prop("checked", false);
        }

    });

    $('#queryResult').on('click', ".btn-box .btn", function () {
        var $thisChkBox = $(this).parents('.st-function-group').find("input[type='checkbox']");

        


        if ($thisChkBox.prop("checked") == true) {
            var idList = "";
            // 記錄被勾選的 MID
            if ($thisChkBox.prop("id") == "si_all") {
                //全部勾選
                $('.scb-table-body tr').each(function () {
                    var hasCheckBox = $(this).find(".def-check");
                    if (hasCheckBox.length > 0) {
                        idList += $(this).find("input[name='MID']").val() + ",";
                    }

                });

                idList = idList.substring(0, idList.length - 1);
                console.log('a:'+idList);

            }
            else {
                //單一勾選
                idList = $thisChkBox.prop("id").split('_')[1];

                console.log('b:' +idList);
            }
        

            var content = '是否確定已通過身分驗證？';
            var isHtml = true;
            var confirmCallback = function () {
                updateUniformIDStatus(idList);
            };
            libs.alert.confirm(content, confirmCallback, isHtml);
            return false;         
            
        }
        else {
            //alert 請勾選要變更的資料
            alert("請勾選要變更的資料列");
        }
    });


    function updateUniformIDStatus(idList) {
        $.ajax({
            url: "/CustomerManager/UpdateUniformIDStatus?idList=" + idList,
            type: "POST",
            success: function (result) {
                if (result.RtnCode != 1) {
                    alert(result.RtnMsg);
                    return false;
                }
                else
                {
                    refresh();
                }
            }
        });

    };

    
    
})();


