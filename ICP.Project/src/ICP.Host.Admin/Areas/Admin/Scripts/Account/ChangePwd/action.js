(function () {
    libs.alert.validationSummary();

    let Expire = $('#Expire').val();
    if (Expire === '1') {
        libs.alert.popup('密碼已到期，請修改密碼');
    }

    $('#lnkSubmit').click(function () {
        var OriginPwd = $('#OriginPwd').val();
        var Pwd = $('#Pwd').val();
        var ConfirmPwd = $('#ConfirmPwd').val();
        if (Pwd != ConfirmPwd) {
            $(this).parents('form:first')[0].reset();
            libs.alert.popup('兩次新密碼輸入有誤，請重新輸入');
            return false;
        }
    });
})();