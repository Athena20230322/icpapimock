// 日期選擇器 初始化
function flatpickr_init(input) {
    if (input._flatpickr) return;
    var $t = $(input);
    var option;
    if (!$t.is('.time')) {
        option = {
            dateFormat: "Y-m-d",
            locale: "zh"
        };
    }
    else {
        option = {
            enableTime: true,
            noCalendar: true,
            dateFormat: "H:i",
            time_24hr: true
        };
    }

    var dateFormat = $t.attr('flatpickr-dateFormat');
    var enableTime = $t.attr('flatpickr-enableTime');
    var noCalendar = $t.attr('flatpickr-noCalendar');
    var time_24hr = $t.attr('flatpickr-time_24hr');
    var minDate = $t.attr('flatpickr-minDate');
    var maxDate = $t.attr('flatpickr-maxDate');
    var minDate_input = $t.attr('flatpickr-minDate_input');
    var maxDate_input = $t.attr('flatpickr-maxDate_input');

    if (dateFormat) option.dateFormat = dateFormat;
    if (enableTime != undefined) option.enableTime = enableTime == '1' || enableTime == 'true';
    if (noCalendar != undefined) option.noCalendar = noCalendar == '1' || noCalendar == 'true';
    if (time_24hr != undefined) option.time_24hr = time_24hr == '1' || time_24hr == 'true';
    if (minDate) option.minDate = minDate;
    if (maxDate) option.maxDate = maxDate;

    $t.flatpickr(option);

    if (minDate_input) {
        var $minDate = $(minDate_input).blur(function () {
            $t[0]._flatpickr.config.minDate = new Date(this.value);
        });
        if ($minDate.val()) $minDate.blur();
    }

    if (maxDate_input) {
        var $maxDate = $(maxDate_input).blur(function () {
            $t[0]._flatpickr.config.maxDate = new Date(this.value);
        });
        if ($maxDate.val()) $maxDate.blur();
    }
}
(function ($) {

    $.ajaxSetup({
        cache: false,
        beforeSend: function () {
            libs.loading.open();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("HttpCode: " + jqXHR.status + "  Error: " + textStatus + ": " + errorThrown);
            alert('發生錯誤');
            libs.loading.close();
        },
        complete: function () {
            libs.loading.close();
        }
    });

    //loading
    libs.loading.open();
    $(window).load(function () {
        libs.loading.close(1300);
    });

    // 表格標頭定位
    var tableHeaderSelector = '#queryResult .st-control > .scb-table-header:first';
    var tableTop = $(tableHeaderSelector).offset();
    var tableTopFn = function () {
        var $t = $(this);
        var winTop = $t.scrollTop();
        var winLeft = $t.scrollLeft();
        var $tableTop = $('.st-control .scb-table-header:first');
        var $table = $('.st-control .scb-table-body:first');
        var $tableBottom = $('.sbb-control:first');
        if (tableTop.top < winTop) {
            $tableTop.addClass('sth-fixed');
        } else {
            $tableTop.removeClass('sth-fixed');
        }
        var tableTop2 = $table.offset().top + $table.height() - $t.height();
        if (tableTop2 > winTop) {
            $tableBottom.addClass('sbb-fixed');
        } else {
            $tableBottom.removeClass('sbb-fixed');
        }
        if (winLeft > 0) {
            $tableBottom.css({
                transform: 'translateX(-' + winLeft + 'px)'
            });
        } else if (winLeft === 0) {
            $tableBottom.css({
                transform: 'translateX(0px)'
            });
        }
        if (winLeft > 0 && tableTop.top < winTop) {
            $tableTop.css({
                transform: 'translateX(-' + winLeft + 'px)'
            });
        } else if (winLeft === 0 || tableTop.top > winTop) {
            $tableTop.css({
                transform: 'translateX(0px)'
            });
        }
        return false;
    };
    if (tableTop) $(window).scroll(tableTopFn);
    var queryResult = document.getElementById('queryResult');
    if (queryResult) {
        var observer = new MutationObserver(function (mutationsList, observer) {
            var hasTableTop = false;
            for (var mutation of mutationsList) {
                for (var addedNode of mutation.addedNodes) {
                    var $addedNode = $(addedNode);
                    if ($addedNode.is('div.st-control:has(.scb-table-header)')) {
                        hasTableTop = true;
                        break;
                    }
                    else {
                        var $tableHeader = $addedNode.find('div.st-control:has(.scb-table-header)');
                        if ($tableHeader.length) {
                            hasTableTop = true;
                            break;
                        }
                    }
                }
                if (hasTableTop) break;
            }
            if (hasTableTop && tableTop) {
                tableTop = null;
                $(window).unbind('scroll', tableTopFn);
            }
            if (hasTableTop && !tableTop) {
                tableTop = $(tableHeaderSelector).offset();
                $(window).bind('scroll', tableTopFn);
            }
            else if (!hasTableTop && tableTop) {
                $(window).unbind('scroll', tableTopFn);
                tableTop = null;
            }
        });
        observer.observe(queryResult, { childList: true });
    }



    //global dynamic dom event bind
    $(document)
        //手機觸控事件
        .on('touchstart mouseenter click', 'a, div, button, .tabs > li, .lcl-box > dl, .mvc-list li', function () {
            $(this).addClass('on-touch');
        })
        .on('touchend mouseleave click', 'a, div, button, .tabs > li, .lcl-box > dl, .mvc-list li', function () {
            $(this).removeClass('on-touch');
        })
        //popup close
        .on('click', '.popup-close', function () {
            $.magnificPopup.close();
            return false;
        })
        //popup
        .on('click', '.open-popup-link', function () {
            $(this).magnificPopup({
                type: 'inline',
                midClick: true,
                closeBtnInside: true,
                showCloseBtn: false,
                fixedContentPos: true,
                mainClass: 'mfp-with-anim',
                removalDelay: 500,
                closeOnBgClick: false,
                callbacks: {
                    beforeOpen: function () {
                        this.st.mainClass = this.st.el.attr('data-effect');
                    }
                }
            }).magnificPopup('open');
            return false;
        })
        //popup (include close botton)
        .on('click', '.open-popup-link2', function () {
            $(this).magnificPopup({
                type: 'inline',
                midClick: true,
                closeBtnInside: true,
                showCloseBtn: true,
                fixedContentPos: true,
                mainClass: 'mfp-with-anim',
                removalDelay: 500,
                callbacks: {
                    beforeOpen: function () {
                        this.st.mainClass = this.st.el.attr('data-effect');
                    }
                }
            }).magnificPopup('open');
            return false;
        })
        .on('click', '.libs-dialog-form', function () {
            libs.dialog.form(this);
            return false;
        })
    ;

    //瀏覽檔案
    $('input.dff-input').change(function () {
        $(this).siblings('.dff-txt').text(this.value);
    });

    // 視窗滾動
    $(window).scroll(function(){
        if ($(this).scrollTop() > 150) {
            $('.scroll-top:first').addClass('visible');
        } else {
            $('.scroll-top:first').removeClass('visible');
        }
    });

    // 回到最上頁
    $('.scroll-top:first').click(function(){
        $('html, body').animate({scrollTop : 0},1000);
        return false;
    });

    // 日期選擇器
    /* <input 
     *  flatpickr-dateFormat="日期格式, 預設(Y-m-d)"
     *  flatpickr-enableTime="顯示時間, 預設否(0)"
     *  flatpickr-noCalendar="隱藏日曆, 預設否(0)"
     *  flatpickr-time_24hr="24小時制, 預設否(0)"
     *  flatpickr-minDate="最小日期"
     *  flatpickr-maxDate="最大日期"
     *  flatpickr-minDate_input="最小日期控件(#id)"
     *  flatpickr-maxDate_input="最大日期控件(#id)" />
     */
    $('input.flatpickr').each(function () {
        flatpickr_init(this);

    });

    //定時監控 觀察名單切換
    function lhpSwitch() {
        if ($('#lps_1').is(':checked')) {
            $('.pb-open').show().siblings('.pld-box').hide();
        } else if ($('#lps_2').is(':checked')) {
            $('.pb-close').show().siblings('.pld-box').hide();
        } else {
            $('.pld-box').hide();
        }
    }
    lhpSwitch();
    $('.lhp-switch').find("input:radio").on('change', function () {
        lhpSwitch();
    });

    //tab
    $('.tab-wrap').each(function () {
        var $tab = $(this);
        var $def_tab = $('.tabs li', $tab).eq(0).addClass('active');
        $($def_tab.data('tab')).siblings().hide();
        $('.tabs li', $tab).click(function () {
            var dataTab = $(this).data('tab');
            $(this).addClass('active').siblings('.active').removeClass('active');
            $(dataTab).stop(false, true).fadeIn(600).siblings().hide();
            return false;
        }).focus(function () {
            this.blur();
        });
    });

    //選擇轉複選
    $('.adb-btn').on('click', function (event) {
        if ($('.ama-dsele-box').hasClass('adb-act')) {
            $('.ama-dsele-box').removeClass('adb-act');
            $('.ama-dsele').removeAttr('multiple');
        } else {
            $('.ama-dsele-box').addClass('adb-act');
            $('.ama-dsele').attr('multiple', 'multiple');
        }
    });
})(jQuery);