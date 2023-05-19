(function () {
    $('#QueryDataType').change(function () {
        $('#QueryDataContent').val("");
        if ($(this).val() === "2") {
            $('#QueryDataContent').attr("maxlength", "14");
        } else {
            $('#QueryDataContent').attr("maxlength", "20");
        }
    });
})();

function checkFields() {
    let $memberDataContent = $('#MemberDataContent').val();
    if ($.trim($memberDataContent) !== "") {
        switch ($('#MemberDataType').val()) {
            case "1":
                if (!$.isNumeric($memberDataContent) || $memberDataContent.length != 16) {
                    alert("電支帳號格式錯誤");
                    return false;
                }
                break;
            case "2":
                let chineseRex = /[\u4E00-\u9FA5•]{2}/;
                if (!chineseRex.test($memberDataContent)) {
                    alert("會員姓名格式錯誤，請輸入2-20個中文字，不可包含英數字及空格，符號僅可接受「•」");
                    return false;
                }

                //let chineseDuplicateRex = /([\u4E00-\u9FA5]){1}/;
                //if (chineseDuplicateRex.test($memberDataContent)) {
                //    alert("會員姓名格式錯誤，不接受連續三個重複字");
                //    return false;
                //}
                break;
            case "3":
                if (!$.isNumeric($memberDataContent)) {
                    alert("手機號碼格式錯誤");
                    return false;
                }
                break;
        }
    }

    let $queryDataContent = $('#QueryDataContent').val();
    if ($.trim($queryDataContent) !== "") {
        switch ($('#QueryDataType').val()) {
            case "1":
                if (!$.isNumeric($queryDataContent) || $queryDataContent.length != 20) {
                    alert("icash pay 訂單編號格式錯誤");
                    return false;
                }
                break;
            case "2":
                if (!$.isNumeric($queryDataContent) || $queryDataContent.length > 14) {
                    alert("銀行轉帳虛擬帳號格式錯誤");
                    return false;
                }
                break;
            case "3":
                if ($queryDataContent.length > 20) {
                    alert("超商店號格式錯誤");
                    return false;
                }
                break;
        }
    }

    let selfSDate = $.trim($('#StartDate').val());
    let selfEDate = $.trim($('#EndDate').val());
    let selfSDateMs = Date.parse(selfSDate.replace(/-/g, "/"));
    let selfEDateMs = Date.parse(selfEDate.replace(/-/g, "/"));

    if (selfSDateMs > selfEDateMs) {
        alert("開始日期不可大於結束日期");
        return false;
    }

    if ((selfEDateMs - selfSDateMs) > (180 * 24 * 60 * 60 * 1000)) {
        if (!window.confirm("所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間!")) {
            return false;
        }
    }

    $('#form0').submit();
}

function resetFields() {
    $('#form0').resetForm();
    $('#QueryDataType').trigger("change");
}
