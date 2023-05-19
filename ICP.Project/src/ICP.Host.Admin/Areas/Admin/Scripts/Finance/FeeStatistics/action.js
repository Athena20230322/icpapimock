function query() {
    $('#QueryForm').submit();
}

function refresh() {
    $('#refreshForm input[type="submit"]:first').click();
}

(function () {
    function monthDiff(d1, d2) {
        var months;
        months = (d2.getFullYear() - d1.getFullYear()) * 12;
        months -= d1.getMonth();
        months += d2.getMonth();
        return months <= 0 ? 0 : months;
    }

    function queryStr2Obj(search) {
        return JSON.parse('{"' + search.replace(/&/g, '","').replace(/=/g, '":"') + '"}', function (key, value) { return key === "" ? value : decodeURIComponent(value) });
    }

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

    $('.sf-radio-box').on('change', function () {

        var StatisticsType_1 = $('#StatisticsType_1');
        var StatisticsType_2 = $('#StatisticsType_2');

        if (StatisticsType_1.prop("checked") == true) {
            $('#date').show();
            $('#ym').hide();
        } else if (StatisticsType_2.prop("checked") == true) {
            $('#date').hide();
            $('#ym').show();
        }
        
        
    });

    $('#MerchantQueryType').on('change', function () {

        var MerchantQueryType = $('#MerchantQueryType').val();

        $("#MerchantQueryValue").val("");

        if (MerchantQueryType == "1") {
            $("#MerchantQueryValue").attr('maxlength', '16');
        } else if (MerchantQueryType == "2") {
            $("#MerchantQueryValue").attr('maxlength', '20');
        }


    });

    $('.minD').blur(function (event) {
        var maxD = $('.maxD').val();

        $('.maxD').flatpickr({
            minDate: $('.minD').val()
        });
        $('.maxD').val(maxD);
    });

    $('.maxD').on('change', function () {
        var minD = new Date();
        var maxD = new Date($('.maxD').val());
        var dateDuration = new Date(minD - maxD);
        var diff = (dateDuration.getFullYear() - 1970) * 12 + dateDuration.getMonth();

        if (diff < 0) {
            alert('查詢時間迄日不可大於今日');
            $('.maxD').val(new Date().toISOString().substr(0, 10));
            return false;
        }

    });

    $('.clear-form').on('click', function () {
        $(this).parents('form:first')[0].reset();
        $('#ym').hide();
        $('#date').show();
        return false;
    });

    $('#btnQuery').click(function () {
        var minD = new Date($('.minD').val());
        var maxD = new Date($('.maxD').val());
        var dateDuration = new Date(maxD - minD);

        var month = (dateDuration.getFullYear() - 1970) * 12 + dateDuration.getMonth();
        
        if (month >= 6) {
            var confirmCallback = query;
            var content = '所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間！';
            var isHtml = true;

            libs.alert.confirm(content, confirmCallback, isHtml);
            return false;
        } else if (month < 0) {
            alert('結束日期不可小於起始日期');
            return false;
        }
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
    });

    $('#queryResult')
        //將 頁碼 複製至 更新條件
        .on('click', '.sc-pagenum-box a[href]', function () {
            var search = this.search.substring(1);
            var query = queryStr2Obj(search);
            $('#refreshForm input[name="PageNo"]').val(query.PageNo);
        })
        //審核失敗, 審核成功
        .on('click', 'a.btn-LPAuth-fail, a.btn-LPAuth-success', function () {
            href2AjaxPost(this.href, lnkCallBack);
            return false;
        });
})();
