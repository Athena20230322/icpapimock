(function () {
    $('#Note').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });

    $('.link-authstatus').click(function () {
        var authStatus = $(this).attr('AuthStatus');
        if (authStatus === '1') {
            var c = confirm('確定要放行此名單?');
            if (!c) return;
        }
        else if (authStatus === '2') {
            var c = confirm('確定要退件此名單?');
            if (!c) return;
        }

        $('#AuthStatus').val(authStatus);

        $('#btnSubmit').click();
    });
})();