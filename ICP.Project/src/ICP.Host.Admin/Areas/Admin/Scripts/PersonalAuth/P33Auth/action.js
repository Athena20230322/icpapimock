function IDTypesChange(value) {
    if (value == '0') {
        $('#IDNO_TextBlock').show();
        $('#UnifiedBusinessNo_TextBlock').hide();
        //清空值
        $('#queryUnifiedBusinessNo').val('');
    }
    else {
        $('#IDNO_TextBlock').hide();
        $('#UnifiedBusinessNo_TextBlock').show();
        $('#queryIDNO').val('');
    }
}
(function () {
    //查詢
    $('#lnkQuery').click(function () {
        if (QueryCheck()) {
            var $btnQuery = $('#btnQuery');
            //將 查詢條件 複製至 更新條件
            var $queryFields = $btnQuery.parents('form:first').find('[name]');
            var $refreshFields = $('#refreshForm [name]');
            $queryFields.each(function () {
                $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
            });
            $refreshFields.find('input[name="PageNo"]').val('1');
            $btnQuery.click();
            return false;
        }
    });

    //清除條件
    $('#lnkReset').click(function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });
})();

function QueryCheck() {
    var IDNO = $('#queryIDNO').val();
    if (IDNO != '') {
        var regex = /^[A-Z]{1}[0-9]{9}$/;
        if (!regex.test(IDNO)) {
            var content = '身分證字號格式錯誤';
            libs.alert.popup(content);
            return false;
        }
    }
    var UnifiedBusinessNo = $('#queryUnifiedBusinessNo').val();
    if (UnifiedBusinessNo != '') {
        var regex = /^[0-9]{8}$/;
        if (!regex.test(UnifiedBusinessNo)) {
            var content = '統一編號格式錯誤';
            libs.alert.popup(content);
            return false;
        }
    }
    return true;
}