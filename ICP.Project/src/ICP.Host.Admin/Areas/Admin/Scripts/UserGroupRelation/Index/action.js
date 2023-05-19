function query() {
    $('#btnQuery').click();
}

function queryComplete() {
    $('#queryResult2').attr('url', '').html('');
}

function queryUsers() {
    var $target = $('#queryResult2');
    var url = $target.attr('url');
    if (!url) {
        $target.html('');
        return;
    }

    $.ajax({
        url: url,
        success: function (result) {
            $target.html(result);
        }
    });
}

(function () {
    $('#queryUserGroupName').change(query);

    $('#queryResult').on('click', 'a.query-users', function () {
        var url = $(this).attr('url');
        $('#queryResult2').attr('url', url);
        queryUsers();
        return false;
    });

    $('#queryResult2').on('click', 'a.del-user', function () {
        var a = this;
        var content = '確定要刪除此成員嗎? 刪除後<br/>此群組設定的權限將無法使用。';
        var isHtml = true;
        var confirmCallback = function () {
            $.ajax({
                url: $(a).attr('url'),
                type: "POST",
                success: function (result) {
                    if (result.RtnCode != 1) {
                        alert(result.RtnMsg);
                        return false;
                    }
                    queryUsers();
                }
            });
        };
        libs.alert.confirm(content, confirmCallback, isHtml);
        return false;
    });

    query();
})();