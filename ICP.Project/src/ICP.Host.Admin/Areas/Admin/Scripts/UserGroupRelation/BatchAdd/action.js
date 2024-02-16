function checkSubmit() {
    if (!$('#ulSelected').has('li').length) {
        $('#lnkSubmit').addClass('disabled');
    }
    else {
        $('#lnkSubmit').removeClass('disabled');
    }
}

function submitArray($form, name, array) {
    $(array).each(function (i) {
        var str = '<input type="hidden" name="' + name + '[' + i + ']" value="' + this + '"/>';
        $form.append(str);
    });
}

(function () {
    libs.alert.validationSummary();

    $('#ulSelect, #ulSelected').on('click', 'li', function () {
        $(this).toggleClass('aml-act');
    })

    $('#queryDeptID').change(function () {
        if (!this.value) {
            $('#ulSelect, #ulSelected').html('');
            checkSubmit();
            return;
        }

        var url = $(this).attr('url');
        $.ajax({
            url: url + '?DeptID=' + this.value,
            method: "POST",
            success: function (result) {
                $('#ulSelected').html('');
                checkSubmit();

                var html = '';
                $(result).each(function () {
                    html += '<li UserID="' + this.UserID + '">' + this.CName + '／' + this.Account + '</li>'
                });
                $('#ulSelect').html(html);
            }
        });
    });

    $('#lnkAddUsers').click(function () {
        var $lis = $('#ulSelect li.aml-act');
        $('#ulSelected').append($lis);
        checkSubmit();
    });

    $('#lnkDelUsers').click(function () {
        var $lis = $('#ulSelected li.aml-act');
        $('#ulSelect').append($lis);
        checkSubmit();
    });

    $('#lnkSubmit').click(function () {
        var UserIDs = [];
        $('#ulSelected li').each(function () {
            var UserID = parseInt($(this).attr('UserID'), 10);
            UserIDs.push(UserID);
        });

        var $form = $(this).parents('form:first');
        submitArray($form, 'UserIDs', UserIDs);
    });
})();