(function () {
    window.libs = window.libs || {};
    libs.alert = {
        //get popup template html with div
        getTemplate: function (key) {
            return $('<div>').append($('#popups > div[popup="' + key + '"]:first').clone());
        },



        //alert message
        popup: function (content, isHtml, callback) {

            if ($.magnificPopup.instance.isOpen) {
                alert((!isHtml ? content : content.replace(/<\/?[^>]+>/ig, " ")));
                return;
            }

            var $div = this.getTemplate('general');
            if (isHtml) $div.find('.pp-content').html(content);
            else $div.find('.pp-content').text(content);
            var html = $div.html();

            $.magnificPopup.open({
                items: {
                    src: html,
                    type: 'inline'
                },
                type: 'inline',
                midClick: true,
                closeBtnInside: true,
                showCloseBtn: false,
                fixedContentPos: true,
                mainClass: 'mfp-zoom-in',
                removalDelay: 0,
                closeOnBgClick: false,
                callbacks: {
                    afterClose: callback
                }
            });
            $div = null;
        },
        //confirm message with callback event
        confirm: function (content, callback, isHtml) {

            if ($.magnificPopup.instance.isOpen) {
                if (confirm((!isHtml ? content : content.replace(/<\/?[^>]+>/ig, " ")))) {
                    if (callback) callback();
                }
                return;
            }

            var $div = this.getTemplate('confirm');
            if (isHtml) $div.find('.pp-content').html(content);
            else $div.find('.pp-content').text(content);
            var html = $div.html();

            $.magnificPopup.open({
                items: {
                    src: html,
                    type: 'inline'
                },
                type: 'inline',
                midClick: true,
                closeBtnInside: true,
                showCloseBtn: false,
                fixedContentPos: true,
                mainClass: 'mfp-zoom-in',
                removalDelay: 0,
                closeOnBgClick: false,
                callbacks: {
                    open: function () {
                        //綁定確認事件回呼
                        $(this.content[0]).find('.mp-btn.yes:first').click(function () {
                            $.magnificPopup.close();
                            if (callback) callback();
                            return false;
                        });
                    }
                }
            });
            $div = null;
        },

		//Alert ModelState Errors Message
		validationSummary: function () {
			var errors = [];
			$('.validation-summary-errors:first li').each(function () {
				errors.push($(this).text());
			});
			if (errors.length) {
                //alert(errors.join('\n'));
                var content = errors.join('<br/>');
                this.popup(content, true);
			}
		}
	}
})();