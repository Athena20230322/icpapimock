$(function () {
    //清除條件
    $('#lnkReset').on('click', function () {
        $(this).closest('form').find("input[type=text]").val("");
        $(this).closest('form').find("input[type=checkbox]").prop("checked", false);
        $(this).closest('form').find("select").each(function () {
            this.selectedIndex = 0;
        });
        $(this).closest('form').find(".sf-radio-box").each(function () {
            $("input[type=radio]", this).eq(0).prop("checked", true);
        });

        var now = new Date();
        now.setDate(now.getDate() - 1)
        $("#Date").val(now.toISOString().substr(0, 10));

        $("input[type=checkbox][name^=MerchantTypeChkBox]").prop("checked", true);
        $("#Amount").val(0);
        $("#Count").val(0);
    });

    //查詢
    $("#lnkQuery").on("click", function () {
        if (checkValidation()) {
            var personalStatus = $("#Personal:checked").val() === undefined ? false : true
            var legalPersonStatus = $("#LegalPerson:checked").val() === undefined ? false : true

            //記錄查詢條件
            Condition = {
                Date: $("#Date").val(),
                ICPMID: $("#ICPMID").val(),
                MerchantName: $("#MerchantName").val(),
                IncomeStaus: $("#IncomeStaus:checked").val() === undefined ? false : true,
                PaymentStatus: $("#PaymentStatus:checked").val() === undefined ? false : true,
                TradeType: parseInt($("#TradeType :selected").val()),
                MerchantType: ((personalStatus && legalPersonStatus) || (!personalStatus && !legalPersonStatus)) ? 0 : (personalStatus?1:2),
                Amount: parseInt($("#Amount").val()),
                Count: parseInt($("#Count").val()),
                SortType: parseInt($("#SortType :selected").val()),
                SortKind: parseInt($("[name='SortKind']:checked").val()),
                PageNo: 1
            }

            $("#queryForm").submit();
        }
    });

});

//查詢條件
var Condition = {
    Date: new Date(),
    ICPMID: "",
    MerchantName: "",
    IncomeStaus: 0,
    PaymentStatus: 0,
    TradeType: 1,
    MerchantType: { Personal: true, LegalPerson: true},
    Amount: 0,
    Count: 0,
    SortType: 1,
    SortKind: 1,
    PageNo: 1
}

//查詢條件檢查
function checkValidation() {
    var intReg = new RegExp('^[0-9]*$');
    var midReg = new RegExp('^[0-9]{16}$');

    if ($('#ICPMID').val() != '') {
        if (!midReg.test($('#ICPMID').val())) {
            alert('請輸入正確的電支帳號');
            return false;
        }
    }
    if ($('#Amount').val() != '') {
        if (!intReg.test($('#Amount').val())) {
            alert('金額區間數字格式錯誤');
            return false;
        }
    }
    if ($('#Count').val() != '') {
        if (!intReg.test($('#Count').val())) {
            alert('金額區間數字格式錯誤');
            return false;
        }
    }

    return true;
}

//檢視狀態-全部勾選
var selectAll = false;
function SelectAll() {
        selectAll = !selectAll;
        var title = selectAll ? "全部取消" : "全部勾選";
        $(".inspectStatus").prop("checked", selectAll);
        $("#SelectAll").text(title).addBack().attr("title", title);
    };

//檢視狀態-勾選送出
function UpdateStatus() {
        var listMID = "";
        $(".inspectStatus:checked").each(function () {
            listMID += $(this).val() + ($(this).is($(".inspectStatus:checked:last")) ? "" : "|");
        });

        if (listMID == "") {
            alert("尚未選取檢視紀錄");
        } else {
            Condition.PageNo = parseInt($(".spb-list .active a").text());
            SetInspectType(listMID);
        }
    };

//檢視狀態-更新檢視記錄
function SetInspectType(listMID) {
    $.ajax({
        url: "/PaymentStatistics/AddPaymentInspectLog",
        data: { listMID: listMID, request: Condition},
        type: "post",
        dataType: 'json',
        success: function (result) {
            if (result.RtnCode == 1) {
                alert("更新完成");
                ReSearch(result.RtnData);
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

//檢視狀態-更新後重新查詢
function ReSearch(query) {
    $("#Date").val(query.Date);
    $("#ICPMID").val(query.ICPMID);
    $("#MerchantName").val(query.MerchantName);
    $("#IncomeStaus").prop("checked", query.IncomeStaus);
    $("#PaymentStatus").prop("checked", query.PaymentStatus);
    $("#TradeType").eq(TradeType).prop("selected", true);
    $("#Personal").prop("checked", query.MerchantType == 0 || query.MerchantType == 1);
    $("#LegalPerson").prop("checked", query.MerchantType == 0 || query.MerchantType == 2);
    $("#Amount").val(query.Amount);
    $("#Count").val(query.Count);
    $("#SortType").eq(query.SortType).prop("selected", true);
    $("[name='SortKind']").eq(query.SortKind).prop("checked", true);
    $("#PageNo").val(query.PageNo);

    $("#queryForm").submit();
}

//匯出excel
function Export() {
    var path = "/PaymentStatistics/PaymentMonitorExportExcel?";
    var qryStr = $.param(Condition);
    location.href = path + qryStr;
}
