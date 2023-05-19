(function () {
    $('input.flatpickr').each(function () {
        flatpickr_init(this);
    });

    $('#lnkSave').click(function () {
        $(this).closest("form").submit();
    });
})();