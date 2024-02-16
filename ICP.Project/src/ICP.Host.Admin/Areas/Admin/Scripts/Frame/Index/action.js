$(function () {
    $('.nav-mobile-icon:first').click(function () {
        $(this).toggleClass('nmi-act');
        $('body').toggleClass('left-nav-slide');
    });
});