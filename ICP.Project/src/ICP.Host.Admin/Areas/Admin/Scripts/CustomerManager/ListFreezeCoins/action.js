function refresh() {   
    var pageNo = $('.spb-list li.active a').html();    
    $('#refreshForm input[name="PageNo"]').val(pageNo);
    $('#refreshForm input[type="submit"]:first').submit();
}

(function () {    

    $('#form0').submit();


    function queryStr2Obj(search) {
        return JSON.parse('{"' + search.replace(/&/g, '","').replace(/=/g, '":"') + '"}', function (key, value) { return key === "" ? value : decodeURIComponent(value) });
    }
       
    //查詢
    $('#btnQuery').click(function () {

        //var $btnQuery = $('#btnQuery');

        //將 查詢條件 複製至 更新條件
        var $queryFields = $btnQuery.parents('form:first').find('[name]');
        var $refreshFields = $('#refreshForm [name]');
        $queryFields.each(function () {
            $refreshFields.filter('[name="' + this.name + '"]:first').val(this.value);
        });
        $refreshFields.find('input[name="PageNo"]').val('1');
        $('form')[1].submit();
        return false;
    });

    $('#queryResult')
        //將 頁碼 複製至 更新條件
        .on('click', 'sc-pagenum-box a[href]', function () {
            var search = this.search.substring(1);
            var query = queryStr2Obj(search);
            $('#refreshForm input[name="PageNo"]').val(query);
        })

})();