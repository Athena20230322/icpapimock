(function () {
    $('#AuthIDNO_IssueDate, #AuthIDNO_Birthday').each(function () {
        if (!this.value) return;
        var $div = $(this).parent();
        var $selects = $div.find('select');
        var date = new Date(this.value);
        $selects.eq(0).val(date.getFullYear());
        $selects.eq(1).val(date.getMonth() + 1);
        $selects.eq(2).val(date.getDate());
        $selects.change(function () {
            var selector = '#' + $(this).attr('data-change-for');
            var $input = $(selector);
            var year = $selects.eq(0).val();
            var month = $selects.eq(1).val();
            var day = $selects.eq(2).val();
            var sDate;
            if (year && month && day) {
                if (month.length == 1) month = '0' + month;
                if (day.length == 1) day = '0' + day;
                sDate = year + '-' + month + '-' + day;
                if (!libs.check.checkDate(sDate)) sDate = null;
            }
            $input.val(sDate || '').change().valid();
        });
    });

    // 顯示輸入文字長度
    $('#AuthIDNO_AuthMsg').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });

    // 選擇圖片 身份證
    $('#divAuthIDNOFiles').on('click', 'a[file], input[file]', function () {
        var $t = $(this);
        var fileSelector = $t.attr('file');
        $(fileSelector).trigger('click');
        return false;
    });

    //上傳顯示縮圖
    $('input[type=file]').change(function () {
        var $t = $(this);
        var img = $t.attr('img');
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $(img).attr('src', e.target.result);
                $(img).show();
            }
            reader.readAsDataURL(this.files[0]);
        }
    });
})();