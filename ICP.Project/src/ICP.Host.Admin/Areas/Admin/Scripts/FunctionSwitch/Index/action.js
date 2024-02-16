function refresh() {
    $('#refreshForm input[type="submit"]:first').click();
}

(function () {
    function lnkCallBack(result) {
        if (result.RtnCode == 1) {
            if (!result.RtnMsg) result.RtnMsg = '更新成功';
            libs.alert.popup(result.RtnMsg, false, refresh);
        }
        else {
            if (!result.RtnMsg) result.RtnMsg = '更新失敗';
            libs.alert.popup(result.RtnMsg);
        }
    }

    function href2AjaxPost(href, cb) {
        $.ajax({
            url: href,
            method: 'POST',
            success: cb
        });
    }

    $('.icon-ic-search').click(function () {
        var $btnQuery = $('#btnQuery');

        //將 查詢條件 複製至 更新條件
        var $queryFields = $btnQuery.parents('form:first').find('[name]');
        var $refreshFields = $('#refreshForm [name]');
        $queryFields.each(function () {
            $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
        });

        $btnQuery.click();
    });

    $('#queryResult').on('click', 'a.swb-control-box', function () {
        href2AjaxPost(this.href, lnkCallBack);
        return false;
    });
    
    $('#btnQuery').click();
})();