(function () {
	//libs.loading
	window.libs = window.libs || {};
	libs.loading = {
		loading: false,
        open: function () {
            if (this.loading) return;
            this.loading = true;
			$('body').prepend('<div class="loading-wrap"><div class="scb-box"><div class="lds-spinner"><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div></div></div>');
		},
        close: function (duration_easing) {
            $('body').css('overflow', 'visible');
            var $wrap = $('.loading-wrap:first');
            if (!duration_easing || duration_easing <= 0) {
                //$wrap.remove();
                //libs.loading.loading = false;
                //return;//太快跟 ajax popup 效果 搭起來怪怪的, 改300ms
                duration_easing = 300;
            }
            $wrap.fadeOut(duration_easing, function () {
                $(this).remove()
                libs.loading.loading = false;
			});
		}
	};
})();