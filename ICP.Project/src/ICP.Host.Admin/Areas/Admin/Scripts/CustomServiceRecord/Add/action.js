(function () {
    $('#addCustomServiceRecord').click(function () {
        var Type = $('#addType').val();
        var GateWay = $('#addGateWay').val();
        var Status = $('#addStatus').val();
        var CName = $('#addCName').val();
        var CellPhone = $('#addCellPhone').val();
        var ICPMID = $('#addICPMID').val();
        var Email = $('#addEmail').val();
        var TradeNo = $('#addTradeNo').val();
        var Note = $('#addNote').val();
        if (Type == '') {
            var content = '請選擇問題類別';
            libs.alert.popup(content);
            return false;
        }
        if (GateWay == '') {
            var content = '請選擇進線管道';
            libs.alert.popup(content);
            return false;
        }
        if (Status == '') {
            var content = '請選擇案件進度';
            libs.alert.popup(content);
            return false;
        }
        if (CName == '') {
            var content = '回報者姓名不可空白';
            libs.alert.popup(content);
            return false;
        }
        if (CName != '') {
            var regex = /^['•\u4E00-\u9FA5\uF900-\uFA2D]+$/;
            if (!regex.test(CName)) {
                var content = '姓名格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
            if (CName.length < 2) {
                var content = '姓名格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
            if (CName.length > 20) {
                var content = '姓名格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        if (CellPhone != '') {
            var regex = /^[0-9]+$/;
            if (!regex.test(CellPhone)) {
                var content = '手機號碼格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        if (ICPMID != '') {
            var regex = /[0-9]{16}$/;
            if (!regex.test(ICPMID)) {
                var content = '電支帳號格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        if (Email != '') {
            var regex = /^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})*$/;
            if (!regex.test(Email)) {
                var content = 'E-mail格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        if (TradeNo != '') {
            var regex = /^[a-zA-Z0-9]+$/;
            if (!regex.test(TradeNo)) {
                var content = '訂單編號格式錯誤，請重新輸入';
                libs.alert.popup(content);
                return false;
            }
        }
        if (Note == '') {
            var content = '紀錄內容不可為空';
            libs.alert.popup(content);
            return false;
        }
        if (Note != '') {
            if (Note.length < 1) {
                var content = '有長度限制1~1000';
                libs.alert.popup(content);
                return false;
            }
            else if (Note.length > 1000) {
                var content = '有長度限制1~1000';
                libs.alert.popup(content);
                return false;
            }
        }
        return true;
    })
    $('#addNote').on("keyup", function () {
        countChar( "#addNote", "#input-tip-txt", 1000); 
    }).on("keydown", function () {
        countChar( "#addNote", "#input-tip-txt", 1000);
    });
})();

function countChar(textareaName, spanName, Length) {
    $(spanName).text($(textareaName).val().length + "/" + Length );
}