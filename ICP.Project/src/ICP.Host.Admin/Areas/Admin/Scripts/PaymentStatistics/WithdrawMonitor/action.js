function query() {
    if (checkValidation()) {
        if ($('.tabs.sc-tabs li').eq(0).hasClass("active")) {
            $('#RuleMode').val(1);
        }
        else {
            $('#RuleMode').val(2)
        }
    }

    $('#btnQuery').click();
}

function checkValidation() {
    var intReg = new RegExp('^[0-9]*$');
    var midReg = new RegExp('^[0-9]{16}$');

    if ($('#StartDate').val() == '') {
        alert('請選擇日期');
        return false;
    }
    if ($('#MID').val() != '') {
        if (!midReg.test($('#MID').val())) {
            alert('請輸入正確的電支帳號');
            return false;
        }
    }
    if ($('#WithdrawAmount').val() != '') {
        if (!intReg.test($('#WithdrawAmount').val())) {
            alert('金額區間數字格式錯誤');
            return false;
        }
    }
    if ($('#WithdrawCount').val() != '') {
        if (!intReg.test($('#WithdrawCount').val())) {
            alert('金額區間數字格式錯誤');
            return false;
        }
    }
    if ($('#Day7TransferCount').val() != '') {
        if (!intReg.test($('#Day7TransferCount').val())) {
            alert('前七天綁定銀行帳戶數數字格式錯誤');
            return false;
        }
    }

    return true;
}

function reset() {
    $('#StartDate').val('');
    $('#lwt_1').prop('checked', false);
    $('#lwt_2').prop('checked', false);
    $('#MID').val('');
    $('#MerchantName').val('');
    $('.tabs.sc-tabs li').eq(0).addClass("active");
    $('.tabs.sc-tabs li').eq(1).removeClass("active");
    $('.st-box.ldwm-rule1').show();
    $('.st-box.ldwm-rule2').hide();
    $('#TradeType').val(1);
    $('#SortType1').val(1);
    $('#WithdrawAmount').val('');
    $('#WithdrawCount').val('');
    $('#lws1_1').prop('checked', false);
    $('#lws1_2').prop('checked', false);
    $('#lpt1_1').prop('checked', false);
    $('#lpt1_2').prop('checked', false);
    $('#lpt1_3').prop('checked', false);
    $('#lpt1_4').prop('checked', false);
    $('#Day7TransferCount').val('');
    $('#SortType2').val(1);
    $('#lws2_1').prop('checked', false);
    $('#lws2_2').prop('checked', false);
    $('#lpt2_1').prop('checked', false);
    $('#lpt2_2').prop('checked', false);
    $('#lpt2_3').prop('checked', false);
    $('#lpt2_4').prop('checked', false);
}

function SetInspectData() {
    var listMID = '';
    $('.checkInspect').each(function () {
        if ($(this).prop('checked')) {
            listMID += $(this).val() + '|';
        }
    });

    if (listMID != '') {
        SetInspectType(listMID.slice(0, -1));
    } else {
        alert('尚未選取檢視紀錄');
    }
}

function SetInspectType(listMID) {
    $.ajax({
        url: "/PaymentStatistics/AddWithdrawInspectLog",
        data: { listMID: listMID, startDate: $("#SelectDate").text()},
        type: "post",
        dataType: 'json',
        success: function (result) {
            if (result.RtnCode == 1) {
                alert("更新完成");
            } else {
                if (result.RtnData != null) {
                    alert(result.RtnMsg.slice(0, -1) + "\n以上MID更新失敗，請聯絡資訊人員!");
                } else {
                    alert("程式出錯，請聯絡資訊人員!");
                }
            }
        },
        error: function () {
            alert("程式出錯，請聯絡資訊人員!");
        }
    });
}

function InspectDataSelectAll() {
    if ($('#selectAll').text() == "全部勾選") {
        $('#selectAll').text('全部取消');
        $('.checkInspect').each(function () {
            $(this).prop('checked', true);
        });
    }
    else {
        $('#selectAll').text('全部勾選');
        $('.checkInspect').each(function () {
            $(this).prop('checked', false);
        });
    }
}