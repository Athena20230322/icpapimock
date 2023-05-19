function query() {
    $('#btnQuery').click();
}

(function () {
    $('#queryName').change(query);

    $('#queryResult').on('click', 'a.del-group', function () {
        var a = this;
        var content = '確定要刪除此類別嗎？<br>刪除後原先使用該功能類別的訊息公告，將自動移轉至 「其他」';
        var isHtml = true;
        var confirmCallback = function () {
            $.ajax({
                url: a.href,
                type: "POST",
                success: function (result) {
                    if (result.RtnCode != 1) {
                        alert(result.RtnMsg);
                        return false;
                    }
                    query();
                }
            });
        };
        libs.alert.confirm(content, confirmCallback, isHtml);
        return false;
    });

    query();
})();

var inputValid = function () {
    var $t = $(this);
    var $form = $t.parents('form:first');
    if (typeof $t.attr('data-val') !== 'undefined' && !$t.valid()) {
        $t.addClass('input-error');
        $form.find('link-submit2').addClass('disabled');
    }
    else {
        $t.removeClass('input-error');
        if (!$form.has('[data-val].input-validation-error').length) $form.find('a.link-submit2, a.clear-form').removeClass('disabled');
    }

    $form.parents('div[class=mfp-content]:first').attr('changed', '');
};