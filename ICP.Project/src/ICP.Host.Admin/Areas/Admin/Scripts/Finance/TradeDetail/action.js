$(function () {
    //清除條件
    $('#lnkReset').on('click', function () {
        $(this).closest('form').find("input[type=text]").val("");
        $(this).closest('form').find("select").each(function () {
            this.selectedIndex = 0;
        });

        var dateStart = new Date();
        dateStart.setDate(dateStart.getDate() - 3);
        $("#DateStart").val(dateStart.toISOString().substr(0, 10));
        $("#DateEnd").val(new Date().toISOString().substr(0, 10));
    });

    //查詢
    $("#lnkQuery").on("click", function () {
        querySubmit();
    });

    //查詢(預設電支帳號)
    if ($("#ICPMID").val() != "") {
        querySubmit();
    }

});

//查詢條件
var Condition = {
    DateType: 1,
    DateStart: new Date(),
    DateEnd: new Date(),
    TradeNoType: 1,
    TradeNo: "",
    PaymentStatus: 0,
    TradeStatus: 0,
    ICPMIDType: 1,
    ICPMID: "",
    AllocateStatus: 0,
    PaymentType: 0,
    PlatformID: null,
    PageNo: 1
}

function querySubmit() {

    if (checkValidation()) {
        //記錄查詢條件
        Condition = {
            DateType: parseInt($("#DateType :selected").val()),
            DateStart: $("#DateStart").val(),
            DateEnd: $("#DateEnd").val(),
            TradeNoType: parseInt($("#TradeNoType :selected").val()),
            TradeNo: $("#TradeNo").val(),
            PaymentStatus: parseInt($("#PaymentStatus :selected").val()),
            TradeStatus: parseInt($("#TradeStatus :selected").val()),
            ICPMIDType: parseInt($("#ICPMIDType :selected").val()),
            ICPMID: $("#ICPMID").val(),
            AllocateStatus: parseInt($("#AllocateStatus :selected").val()),
            PaymentType: parseInt($("#PaymentType :selected").val()),
            PlatformID: $("#PlatformID").val(),
            PageNo: 1
        }

        $("#queryForm").submit();
    }
}

//查詢條件檢查
function checkValidation() {
    var midReg = new RegExp('^[0-9]{16}$');
    var iNoReg = new RegExp('^[0-9]{20}$');
    var mNoReg = new RegExp('^[A-Za-z0-9]{1,50}$');

    var dateStart = new Date($('#DateStart').val());
    var dateEnd = new Date($('#DateEnd').val());
    var dateDuration = new Date(dateEnd - dateStart);

    var month = (dateDuration.getFullYear() - 1970) * 12 + dateDuration.getMonth();

    if (month >= 6) {
        if (!confirm("所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間!")) {
            return false;
        }
    } else if (month < 0) {
        alert('結束日期不可小於起始日期');
        return false;
    }

    if ($('#ICPMID').val() != '') {
        if (!midReg.test($('#ICPMID').val())) {
            alert('電支帳號格式錯誤');
            return false;
        }
    }

    if ($('#TradeNo').val() != '') {
        var tradeNoType = $("#TradeNoType :selected").val();
        if (tradeNoType == 1 && !iNoReg.test($('#TradeNo').val())) {
            alert('icashpay訂單編號格式錯誤');
            return false;
        } else if (tradeNoType == 2 && !mNoReg.test($('#TradeNo').val())) {
            alert('特店訂單編號格式錯誤');
            return false;
        }
    }

    return true;
}

//匯出excel
function Export() {
    var path = "/Finance/TradeDetailExportExcel?";
    var qryStr = $.param(Condition);
    location.href = path + qryStr;
}