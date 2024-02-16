(function () {
    libs.alert.validationSummary();

    // 顯示輸入文字長度
    $('#LockMemo').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });    


    $('.link-submit').on('click', function () {
        $('form').validate();
        if ($('form').valid()) {
            $('btnSubmit').click();
        } else {
            return false;
        }

    });


})();

