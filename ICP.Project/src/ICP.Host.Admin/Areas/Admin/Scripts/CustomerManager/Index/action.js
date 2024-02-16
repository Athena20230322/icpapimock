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

(function () {
    //清除條件
    $('#lnkReset').click(function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });    

    //查詢
    $('#lnkQuery').click(function () {
        var $CellPhone = $('#queryCellPhone').val();
        var $Account = $('#queryAccount').val();
        var $IDNO = $('#queryIDNO').val();
        var $ICPMID = $('#queryICPMID').val();
        var $UnifiedBusinessNo = $('#queryUnifiedBusinessNo').val();
        var $CompanyName = $('#queryCompanyName').val();
        var $UniformID = $('#queryUniformID').val();
        var $WebSiteName = $('#queryWebSiteName').val();
        var $CName = $('#queryCName').val();
        var $Email = $('#queryEmail').val();

        $('#refreshCellPhone').val($CellPhone);
        $('#refreshAccount').val($Account);
        $('#refreshIDNO').val($IDNO);
        $('#refreshICPMID').val($ICPMID);
        $('#refreshUnifiedBusinessNo').val($UnifiedBusinessNo);
        $('#refreshCompanyName').val($CompanyName);
        $('#refreshUniformID').val($UniformID);
        $('#refreshWebSiteName').val($WebSiteName);
        $('#refreshCName').val($CName);
        $('#refreshEmail').val($Email);
        

        //檢核輸入的欄位        
        if ($CellPhone != "") {
            var regex = /^09[0-9]{8}$/;
            if (!regex.test($CellPhone)) {
                var content = '手機號碼格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($Account != "") {
            var regex = /^(?!.*[^0-9a-zA-Z])(?=.*\d)(?=.*[a-zA-Z]).{6,12}$/;
            if (!regex.test($Account)) {
                var content = '登入帳號格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($IDNO != "") {
            var regex = /^[A-Z]{1}[0-9]{9}$/;
            if (!regex.test($IDNO)) {
                var content = '身分證字號格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($ICPMID != "") {
            var regex = /[0-9]{16}$/;
            if (!regex.test($ICPMID)) {
                var content = '電支帳號格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($UnifiedBusinessNo != "") {
            var regex = /[0-9]{8}$/;
            if (!regex.test($UnifiedBusinessNo)) {
                var content = '統一編號格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($UniformID != "") {
            var regex = /^(?=.*[A-Z]{1}[A-Z]{1}[0-9]{8}).{10}$/;
            if (!regex.test($UniformID)) {
                var content = '居留證字號格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($CName != "") {
            var regex = /^['•\u4E00-\u9FA5\uF900-\uFA2D]{2,20}/;
            var regex1 = /(.){1,2}/;
            if (!regex.test($CName)) {
                var content = '會員姓名格式錯誤，請輸入2-20個字，不可包含空格，符號僅可接受「•」';
                libs.alert.popup(content);
                return false;
            }
            else {
                var arry = $CName.split("");
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
        else if ($Email != "") {
            var regex = /^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})*$/;
            if (!regex.test($Email)) {
                var content = 'E-mail格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        else if ($CompanyName == "" && $WebSiteName == "")
        {
            var content = '請輸入至少一種搜尋條件';
            libs.alert.popup(content);
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

})();


