

(function () {
    //清除條件
    $('.clear-form').on('click', function () {
        $(this).closest('form').find("input[type=text], textarea").val("");  
        $(this).addClass('disabled');
        $('#btnQuery').addClass('disabled');


    });


    //未輸入任何查詢條件
    var count = 0;
    $(":input").each(function () {
        count += 1;
    });

    var check = 0;

    $(":input").each(function () {
        var value = $(this).val();
        if ($(this).val() == '' | $(this).val() == '0') { check += 1; }
    });

    if (count == check) {
        $('a.link-submit, a.clear-form').addClass('disabled');
    }

})();


