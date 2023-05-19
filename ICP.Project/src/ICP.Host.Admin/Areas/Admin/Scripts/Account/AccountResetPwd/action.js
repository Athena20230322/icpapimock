(function () {
    libs.alert.validationSummary();

    $('#lnkSubmit').click(function () {
        var Pwd = $('#Pwd').val();
        var ConfirmPwd = $('#ConfirmPwd').val();
        if (Pwd != ConfirmPwd) {
            libs.alert.popup('兩次新密碼輸入有誤，請重新輸入');
            return false;
        }
    });
})();