(function () {

})();

function checkFields() {
    let $PaymentSideDataContent = $('#PaymentSideDataContent').val();
    if ($.trim($PaymentSideDataContent) !== "") {
        if ($('#PaymentSideDataType').val() === "1" && (!$.isNumeric($PaymentSideDataContent) || $PaymentSideDataContent.length != 16)) {
            alert("付款方電支帳號格式錯誤");
            return false;
        }
    }

    let $ReceiptSideDataContent = $('#ReceiptSideDataContent').val();
    if ($.trim($ReceiptSideDataContent) !== "") {
        if ($('#ReceiptSideDataType').val() === "1" && (!$.isNumeric($ReceiptSideDataContent) || $ReceiptSideDataContent.length != 16)) {
            alert("收款方電支帳號格式錯誤");
            return false;
        }
    }

    let $TradeNo = $('#TradeNo').val();
    if ($.trim($TradeNo) !== "") {
        if (!$.isNumeric($TradeNo) || $TradeNo.length != 20) {
            alert("icash pay 訂單編號格式錯誤");
            return false;
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
        //alert("超過半年");
        if (!window.confirm("所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間!")) {
            return false;
        }
    }

    //alert("pass");
    //return false;
    $('#form0').submit();
}

function resetFields() {
    $('#form0').resetForm();
}