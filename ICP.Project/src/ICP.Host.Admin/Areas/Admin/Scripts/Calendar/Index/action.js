function query() {
    $('#form0').submit();
}

(function () {
    $('#queryResult').on('click', 'a.del-group', function () {
        var a = this;
        var content = '確定要刪除此則休假 / 補班設定嗎？';
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