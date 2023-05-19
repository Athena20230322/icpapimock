(function () {
    $(document)
    .on(
        'touchstart mouseenter click',
        'a, div, button, .tabs > li, .lcl-box > dl, .mvc-list li',
        function () { $(this).addClass('on-touch'); }
    )
    .on(
        'touchend mouseleave click',
        'a, div, button, .tabs > li, .lcl-box > dl, .mvc-list li',
        function () { $(this).removeClass('on-touch'); }
    );

    //子選單
    $('.lnb-subtitle').on('click', function (event) {
        event.preventDefault();
        /* Act on the event */
        var $li = $(this).parent('li');
        if ($li.hasClass('lnl-active')) {
            $li.removeClass('lnl-active');
        }
        else {
            $li.addClass('lnl-active');
        }
        $(this).next('.lnb-list-sec').slideToggle(400);
    });

    //子選單 click
    $('.lnb-list-sec a').click(function () {
        $('li.lnl-active').removeClass('lnl-active');
        $('li.lls-active').removeClass('lls-active');
        var $t = $(this);
        $t
            .parents('li:first').addClass('lls-active')
            .parents('li:first').addClass('lnl-active');

        if (window.parent && window.parent.setTitle) {
            window.parent.setTitle($t.attr('parentName'), $t.text());
        }
    });
})();