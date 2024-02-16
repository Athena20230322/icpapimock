(function () {

    //### 全部勾選
    $("#queryResult").on('click', "#clickAll", function () {

        $("input[name='sld_check']").each(function () {
            if ($(this).prop("checked") == false) {
                $(this).prop("checked", true);
            } 
        });

        $("#clickAll").hide();
        $("#cancelAll").show();     
    });   

    //### 全部取消
    $("#queryResult").on('click', "#cancelAll", function () {

        $("input[name='sld_check']").each(function () {
            if ($(this).prop("checked") == true) {
                $(this).prop("checked", false);
            }
        });

        $("#clickAll").show();
        $("#cancelAll").hide();
    });   

    //### 勾選送出
    $("#queryResult").on('click', "#selectSubmit", function () {

        var flag = false;
        var listMID = '';

        $("input[name='sld_check']").each(function () {          

            if ($(this).prop("checked") == true) {                
                listMID += $(this).val() + '|';              
            }
        });

        if (listMID == '') {
            alert('尚未選取檢視紀錄');
        } else {
            SelectSubmit(listMID.slice(0, -1));
        }

    });

    function SelectSubmit(listMID) {
        $.ajax({
            url: "/PaymentStatistics/AddTimingMonitorLog",
            data: { listMID: listMID, startDate: $("#StartDate").val() },
            type: "post",
            dataType: 'json',
            success: function (result) {
                if (result.RtnCode == 1) {
                    //alert("更新完成");
                    if (result.RtnData != null) {
                        alert(result.RtnMsg.slice(0, -1) + "\n以上MID更新失敗，請聯絡資訊人員!");
                    }

                    $("#lnkQuery").trigger('click');

                } else {
                    alert("程式出錯，請聯絡資訊人員!");
                }
            },
            error: function () {
                alert("程式出錯，請聯絡資訊人員!");
            }
        });

    }


})();


