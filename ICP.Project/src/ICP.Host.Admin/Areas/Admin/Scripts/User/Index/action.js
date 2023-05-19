function query() {

    //var FilterType = $('#FilterType').val();
    var FilterKeyWord = $('#FilterKeyWord').val();
    var CName = '';
    var UserID = '';
    var Account = '';

    //if (FilterType == '0') CName = FilterKeyWord;
    //else if (FilterType == '1') UserID = FilterKeyWord;
    Account = FilterKeyWord;

    $('#queryCName').val(CName);
    $('#queryUserID').val(UserID);
    $('#queryAccount').val(Account);
    $('#btnQuery').click();
}

(function () {
    //$('#FilterType').change(function () {
    //    $('#FilterKeyWord').val('');
    //    query();
    //});

    $('#queryUserGroupID, #FilterKeyWord').change(query);

    $('#queryResult').on('click', 'a.del-user', function () {
        var a = this;
        var content = '確定要刪除此人員嗎? 刪除後<br/>群組使用者將一併移除該人員。';
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