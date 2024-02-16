(function () {
    
    
})();

function checkFields() {

    $('#BuyerICPMID').val("");
    $('#BuyerCName').val("");
    $('#TradeNo').val("");
    $('#MerchantTradeNo').val("");
    $('#SellerICPMID').val("");
    $('#SellerCName').val("");

    var BuyerType = $('#BuyerType').val();
    var BuyerContent = $('#BuyerContent').val();

    var SellerType = $('#SellerType').val();
    var SellerContent = $('#SellerContent').val();

    var TradeNoType = $('#TradeNoType').val();
    var TradeNoContent = $('#TradeNoContent').val();

    if ($.trim(BuyerContent) != '') {
        if (BuyerType == "1") {
            $('#BuyerICPMID').val(BuyerContent);
        } else {
            $('#BuyerCName').val(BuyerContent);
        }
    }

    if ($.trim(TradeNoContent) != '') {
        if (TradeNoType == "1") {
            $('#TradeNo').val(TradeNoContent);
        } else {
            $('#MerchantTradeNo').val(TradeNoContent);
        }
    }

    if ($.trim(SellerContent) != '') {
        if (SellerType == "1") {
            $('#SellerICPMID').val(SellerContent);
        } else {
            $('#SellerCName').val(SellerContent);
        }
    }

    //### 檢核日期在6個月
    if(!chkDiffOfMonth($('#StartDate').val(), $('#EndDate').val(), 6)) {
        if (!confirm('所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間!')) {
            return false;
        }
    }

    //### 檢核電支帳戶格式
    var regICPMID = /^[0-9]{16}$/;

    //### 檢核英數字
    var regEngOrInt = /^[\d|a-zA-Z]+$/;

    //### 付款方資訊審核
    var buyerICPMID = $('#BuyerICPMID').val();

    if (BuyerType == "1" && buyerICPMID != '' && !regICPMID.test(buyerICPMID, regICPMID)) {
        alert('(付款方)電支帳號格式錯誤。');
        return false;
    }

    //### 訂單類別判斷
    var tradeNo = $('#TradeNo').val();
    if (tradeNo != '' && (tradeNo.length > 20 || !regEngOrInt.test(tradeNo, regEngOrInt))) {
        alert('icashpay訂單編號格式錯誤。');
        return false;
    }

    var merchantTradeNo = $('#MerchantTradeNo').val();
    if (merchantTradeNo != '' && (merchantTradeNo.length > 64 || !regEngOrInt.test(merchantTradeNo, regEngOrInt))) {
        alert('特店訂單編號格式錯誤。');
        return false;
    }

    //### 收款方資訊審核
    var sellerICPMID = $('#SellerICPMID').val();
    if (BuyerType == "1" && sellerICPMID != '' && !regICPMID.test(sellerICPMID, regICPMID)) {
        alert('(收款方)電支帳號格式錯誤。');
        return false;
    }

    $('#form0').submit();
}

function resetFields() {
    $('#form0').resetForm();

    var now = new Date();
    var sdt = now.getFullYear() + "-" + ("0" + (now.getMonth() + 1)).slice(-2) + "-" + ("0" + now.getDate()).slice(-2);
    var edt = sdt;

    $("#StartDate").flatpickr({
        allowInput: false,
        defaultDate: sdt,
        altInputClass: "flatpickr flatpickr-input active minD",
        locale: "zh",
        maxDate: edt
    });

    $("#EndDate").flatpickr({
        allowInput: false,
        defaultDate: edt,
        altInputClass: "flatpickr flatpickr-input active maxD",
        locale: "zh",
        minDate: sdt
    });
}

function chkDiffOfMonth(start, end, i) {

    if (start == "" || end == "") {
        return fasle;
    }

    var diff;
    var startArr = start.split('-');
    var endArr = end.split('-');

    if (startArr[0] == endArr[0]) {
        diff = endArr[1] - startArr[1];
    } else {
        diff = (endArr[0] - startArr[0]) * 12 + (endArr[1] - startArr[1]);
    }

    var chk = false;

    if (new Date(start) > new Date(end)) {
        chk = false;
    } else if (diff < i) {
        chk = true;
    } else if (diff == i && endArr[2] < startArr[2]) {
        chk = true;
    }

    return chk;
}
