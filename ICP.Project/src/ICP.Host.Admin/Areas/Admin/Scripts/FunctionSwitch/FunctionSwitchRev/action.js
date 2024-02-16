(function () {
    $('a.btn-delete').click(function () {
        $.ajax({
            url: this.href,
            method: 'POST',
            success: function (result) {
                if (result.RtnCode == 1) {
                    if (!result.RtnMsg) result.RtnMsg = '刪除成功';
                    libs.alert.popup(result.RtnMsg, false);
                    refresh();
                }
                else {
                    if (!result.RtnMsg) result.RtnMsg = '刪除失敗';
                    libs.alert.popup(result.RtnMsg);
                }
                $('.mfp-close').click();
            }
        });
        return false;
    });
})();