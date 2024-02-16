(function () {
    window.libs = window.libs || {};

    var changedKey = 'changed';
    $.magnificPopup.instance.close = function () {
        var $c = this.contentContainer;
        if ($c.context.hasAttribute(changedKey)) {
            if (!confirm("您的異動尚未儲存，確定要離開?")) {
                return;
            }
        }
        $.magnificPopup.proto.close.call(this);
    }

    libs.dialog = {

        _bindAjaxFormSubmit: function (contentContainer, updateCallback) {

            var successCallback = function (result) {

                contentContainer.removeAttr(changedKey);

                if (result.RtnCode == 1) {
                    $(contentContainer)
                        .prop('data-update-result', result)
                        .attr('data-update-success', '1')
                        .attr('data-update-msg', result.RtnMsg)
                        .attr('data-update-callback', updateCallback);

                    $.magnificPopup.close();
                }
                else if (result.RtnMsg) {
                    alert(result.RtnMsg);
                }
                else {
                    //$(contentContainer).html(result);
                    var magnificPopup = $.magnificPopup.instance;
                    magnificPopup.items[0].type = "inline";
                    magnificPopup.items[0].src = result;
                    magnificPopup.updateItemHTML();

                    // Enable client side validation
                    $.validator.unobtrusive.parse(contentContainer);

                    libs.dialog._bindAjaxFormSubmit(contentContainer, updateCallback);
                }
            };

            //bind ajax form submit
            $('form[target!="_blank"]', contentContainer).submit(function () {
                $(this).ajaxSubmit({
                    success: successCallback
                });
                return false;
            });
        },
        form: function (a, option) {
            option = option || {};
            var class_init = 'libs-dialog-form-init';
            var $a = $(a);
            if ($a.is('.' + class_init)) return;
            var callback = option['data-update-callback'] || $a.attr('data-update-callback');

            $a.magnificPopup({
                type: 'ajax',
                midClick: true,
                closeBtnInside: true,
                showCloseBtn: true,
                fixedContentPos: true,
                mainClass: 'mfp-zoom-in',
                removalDelay: 0,
                closeOnBgClick: false,
                callbacks: {
                    ajaxContentAdded: function () {
                        // Enable client side validation
                        $.validator.unobtrusive.parse(this.contentContainer);

                        libs.dialog._bindAjaxFormSubmit(this.contentContainer, callback);
                    },
                    afterClose: function () {
                        var $c = $(this.contentContainer);
                        if ($c.attr('data-update-success') != '1') return;

                        var cb;
                        var updateCallback = $c.attr('data-update-callback');
                        if (updateCallback) {
                            cb = window[updateCallback];
                        }

                        var _cb;
                        if (cb) _cb = function () { cb(result); };

                        var result = $c.prop('data-update-result');

                        var RtnMsg = $c.attr('data-update-msg')
                        if (RtnMsg)
                            libs.alert.popup(RtnMsg, false, _cb);
                        else if (_cb)
                            _cb();
                    }
                }
            }).magnificPopup('open');
            $a.addClass(class_init);
            return false;
        }
    };
})();
