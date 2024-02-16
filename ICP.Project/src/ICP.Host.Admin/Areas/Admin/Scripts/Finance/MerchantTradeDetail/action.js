$(function () {
    //清除條件
    $("#lnkReset").on("click", function () {
        $(this).closest("form").find("input[type=text]").val("");
        $(this).closest("form").find("select").each(function () {
            this.selectedIndex = (this.id == "UserType"? 1 : 0);
        });

        var dateStart = new Date();
        dateStart.setDate(dateStart.getDate() - 3);
        $("#DateStart").val(dateStart.toISOString().substr(0, 10));
        $("#DateEnd").val(new Date().toISOString().substr(0, 10));
    });

    //查詢
    $("#lnkQuery").on("click", function () {
        if (checkValidation()) {
            //記錄查詢條件
            Condition = {
                DateStart: $("#DateStart").val(),
                DateEnd: $("#DateEnd").val(),
                UserType: parseInt($("#UserType :selected").val()),
                User: $("#User").val(),
                TradeModeID: parseInt($("#TradeModeID :selected").val()),
                PaymentTypeID: parseInt($("#PaymentTypeID :selected").val()),
                PaymentSubTypeID: parseInt($("#PaymentSubTypeID :selected").val()),
                PageNo: 1
            }

            $("#queryForm").submit();
        }
    });

});

//查詢條件
var Condition = {
    DateStart: new Date(),
    DateEnd: new Date(),
    UserType: 1,
    User: "",
    TradeModeID: 0,
    PaymentTypeID: 0,
    PaymentSubTypeID: 0,
    PageNo: 1
}

//繫結交易類型
function BindPaymentTypeList() {
    var tradeModeID = $("#TradeModeID :selected").val();
    $.ajax({
        url: "/Finance/BindPaymentTypeList",
        data: { tradeModeID: tradeModeID},
        type: "post",
        dataType: "json",
        success: function (result) {
            $("#PaymentTypeID").html("");
            $("#PaymentSubTypeID").html("");
            $.each(result.PaymentTypeList, function () {
                $("#PaymentTypeID").append($("<option></option>").val(this.Value).text(this.Text));
            })  
            $.each(result.PaymentSubTypeList, function () {
                $("#PaymentSubTypeID").append($("<option></option>").val(this.Value).text(this.Text));
            })  
        },
        error: function () {
            alert("程式出錯，請聯絡資訊人員!");
        }
    });
}

//繫結交易子類型
function BindPaymentSubTypeList() {
    var paymentTypeID = $("#PaymentTypeID :selected").val();
    $.ajax({
        url: "/Finance/BindPaymentSubTypeList",
        data: { paymentTypeID: paymentTypeID },
        type: "post",
        dataType: "json",
        success: function (result) {
            $("#PaymentSubTypeID").html("");
            $.each(result.PaymentSubTypeList, function () {
                $("#PaymentSubTypeID").append($("<option></option>").val(this.Value).text(this.Text));
            })  
        },
        error: function () {
            alert("程式出錯，請聯絡資訊人員!");
        }
    });
}

//查詢條件檢查
function checkValidation() {
    var midReg = new RegExp("^[0-9]{16}$");

    var dateStart = new Date($("#DateStart").val());
    var dateEnd = new Date($("#DateEnd").val());
    var dateDuration = new Date(dateEnd - dateStart);

    var month = (dateDuration.getFullYear() - 1970) * 12 + dateDuration.getMonth();

    if (month >= 6) {
        if (!confirm("所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間!")) {
            return false;
        }
    } else if (month < 0) {
        alert("結束日期不可小於起始日期");
        return false;
    }

    if ($("#User").val() != "") {
        if ($("#UserType :selected").val() == "1" && !midReg.test($("#User").val())) {
            alert("電支帳號格式錯誤");
            return false;
        }
    }

    return true;
}

//匯出excel
function Export() {
    var path = "/Finance/MerchantTradeDetailExportExcel?";
    var qryStr = $.param(Condition);
    location.href = path + qryStr;
}