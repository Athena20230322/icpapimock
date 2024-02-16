(function () {
    //查詢
    $('#lnkQuery').click(function () {

        var query = function () {
            var $btnQuery = $('#btnQuery');

            //將 查詢條件 複製至 更新條件
            var $queryFields = $btnQuery.parents('form:first').find('[name]');
            var $refreshFields = $('#refreshForm [name]');
            $queryFields.each(function () {
                $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
            });

            $btnQuery.click();
        }
        
        var createDateBegin = $('#querStartDate').val();
        var createDateEnd = $('#queryEndDate').val();
        var createBegin = new Date(createDateBegin);
        var createEnd = new Date(createDateEnd);
        if (createBegin && createEnd) {
            var plus6MonDate = new Date(createBegin.setMonth(createBegin.getMonth() + 6));
            if (plus6MonDate < createEnd) {
                var confirmMsg = '所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間!';
                libs.alert.confirm(confirmMsg, query);
                return false;
            }
        }

        query();
        return false;
    });

    //清除條件
    $('#lnkReset').click(function () {
        $(this).parents('form:first')[0].reset();
        return false;
    });

})();