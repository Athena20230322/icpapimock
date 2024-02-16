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
    $('#Note').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });

    // 新增上傳
    $('#uploadForms input[type="file"]').change(function (e) {
        var $file = $(this);
        var UploadedType = parseInt($file.attr('UploadedType'), 10);
        if (UploadedType < 5) return;
        var fileName = e.target.files[0].name;
        var fileSelector = '#' + this.id;
        var $li;
        var $button = $('[file="' + fileSelector + '"]:first');
        // button 不存在新增
        if (!$button.length) {
            var $div = $($file.attr('detail'));
            var html =
                '<li>' +
                '<a class="pfl-link"></a>' +
                '<div class="def-file pfl-btn">' +
                '<input type="button" file="' + fileSelector + '" class="dff-input">' +
                '<div class="btn dff-btn">修改</div>' +
                '</div>' +
                '</li>';
            
            $li = $(html).appendTo($div.find('ul.ulLegalFiles:first'));
            
            if ($li.parent().length >= 6) $div.find('.btnAddFile:first').hide();
        }
        else
            $li = $button.parents('li:first');

        $li.find('a.pfl-link').removeAttr('href').text(fileName);
    });

    // 選擇圖片 身份證
    $('#divAuthIDNOFiles, div.divLegalAuthIDNOFiles, ul.ulLegalFiles').on('click', 'a[file], input[file]', function () {
        var $t = $(this);
        var fileSelector = $t.attr('file');
        $(fileSelector).trigger('click');
        return false;
    });

    // 新增資料(圖片)
    $('input.btnAddFile').click(function () {
        var $t = $(this);
        var $div = $t.parents('.legalDetail:first');
        var $ul = $div.find('ul.ulLegalFiles:first');
        var length = $ul.children().length;
        if (length >= 6) return false;
        var MID = $t.attr('MID');
        var UploadType = 4 + length + 1;
        var fileSelector = '#fileUpload' + UploadType + '_' + MID;
        $(fileSelector).trigger('click');
    });

    //駐列上傳
    function queueSubmit($forms, callback) {
        if (!$forms.length) {
            if (callback) callback();
            return;
        }
        var $form = $forms[0];
        $form.ajaxSubmit({
            success: function (result) {
                if (result.RtnCode != 1) {
                    libs.alert.popup(result.RtnMsg || 'Upload File Error');
                    return;
                }
                $forms.shift();
                queueSubmit($forms, callback);
            }
        })
    }

    // 儲存 (先上傳圖檔 + 再儲存資料)
    $('#lnkSave').click(function () {
        var $uploadImgForms = [];
        $('#uploadForms input[type="file"]').each(function () {
            if (!this.value) return;
            var $form = $(this).parents('form:first');
            $uploadImgForms.push($form);
        });

        var $dataForm = $(this).parents('form:first');
        var submitDataForm = function () {
            $dataForm.submit();
        };

        // 依序上傳圖片, 最後再提交資料更新
        queueSubmit($uploadImgForms, submitDataForm);

        return false;
    });
})();