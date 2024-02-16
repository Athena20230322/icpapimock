(function () {

    //標單送出鈕 disabled 切換
    var fnInputValid = function () {
        var $t = $(this);
        var $form = $t.parents('form:first');
        if (typeof $t.attr('data-val') !== 'undefined' && !$t.valid()) {
            $t.addClass('input-error');
            $form.find('a.link-submit').addClass('disabled');
        }
        else {
            $t.removeClass('input-error');
            if (!$form.has('[data-val].input-validation-error:not(.validignore)').length) $form.find('a.link-submit, a.clear-form').removeClass('disabled');
        }

        $form.parents('div[class=mfp-content]:first').attr('changed', '');
    };

    $(document)
    //.on('change', 'form input[name], form select[name]', fnInputValid)
    .on('keydown', 'form input[name], form textarea[name]', function (e) {
        this.value_old = this.value;
    })
    .on('keyup', 'form input[name], form textarea[name]', function (e) {
        if (this.value == this.value_old) return;
        fnInputValid.call(this);
    })
    .on('click', 'a.link-submit', function () {
        //click to submit 放最後, 前面 return false 才能停止 submit
        var $t = $(this);
        if ($t.is('.disabled')) return false;
        $(this).parents('form:first').submit();
        return false;
    });
})();