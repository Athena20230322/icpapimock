(function () {

    var $lnkSave = $('#lnkSave');

    var $form = $lnkSave.parents('form:first');

    $form.find('input.checkAll').change(function () {
        $(this).parents('tr:first').find('input[type="checkbox"][value]').prop('checked', this.checked);
    });

    $('#checkAll').click(function () {
        $form.find('input.checkAll').prop('checked', true).change();
        return false;
    });

    $form.find('input[type="checkbox"]:not(.checkAll)').change(function () {
        var $tr = $(this).parents('tr:first');
        if (this.checked) {
            checkAll.call($tr[0]);
        }
        else {
            $tr.find('input.checkAll').prop('checked', false);
        }
    });

    function checkAll() {
        var checkAll = true;
        var $t = $(this);
        var $chks = $t.find('input[type="checkbox"]:not(.checkAll)').each(function () {
            if (!this.checked) {
                checkAll = false;
                return false;
            }
        });
        if ($chks.length && checkAll) {
            $t.find('input.checkAll').prop('checked', true);
        }
    }

    $form.find('tr').each(checkAll);

    $lnkSave.click(function () {
        var data = [];
        $form.find('tr[FunctionID]').each(function () {
            var $tr = $(this);
            var obj = {
                FunctionID: parseInt($tr.attr('FunctionID'), 10),
                ActionSum: 0
            };
            $tr.find('input[type="checkbox"][value]:checked').each(function () {
                obj.ActionSum += parseInt(this.value, 10);
            });
            data.push(obj);
        });

        $.ajax({
            url: $form.attr('action'),
            method: $form.attr('method'),
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (result) {
                if (result.RtnCode != 1) {
                    libs.alert.popup(result.RtnMsg);
                    return;
                }

                libs.alert.popup('成功');
            }
        });

        return false;
    });

})();