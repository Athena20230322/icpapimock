(function () {
    $('#MessageTypes').change(function () {
        var messageType = $(this).val();
        var message = $(this).find('option:selected').text();
        if (messageType === '5') {
            $('.reason').show();
            message = '';
        }
        else {
            $('.reason').hide();
        }
        $('#Reason').val(message);
        $('#Reason').keyup();
    });

    $('#SuspenseType').change(function () {
        var message = $(this).find('option:selected').text();
        $('.pf-regular-box').text(message);
    });

    $('#Reason').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });

    $('#Note').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });

    $('.link-submit').click(function () {
        if ($(this).hasClass('disabled')) return false;

        $('form').validate();
        if (!$('form').valid()) return false;
    });
})();