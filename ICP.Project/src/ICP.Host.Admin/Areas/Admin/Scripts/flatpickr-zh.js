/* flatpickr v4.5.2, @license MIT */
(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
    typeof define === 'function' && define.amd ? define(['exports'], factory) :
    (factory((global.zh = {})));
}(this, (function (exports) { 'use strict';

    var fp = typeof window !== "undefined" && window.flatpickr !== undefined ? window.flatpickr : {
      l10ns: {}
    };
    var Mandarin = {
      weekdays: {
        shorthand: ["日", "一", "二", "三", "四", "五", "六"],
        longhand: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"]
      },
      months: {
        shorthand: ["1 月", "2 月", "3 月", "4 月", "5 月", "6 月", "7 月", "8 月", "9 月", "10 月", "11 月", "12 月"],
        longhand: ["一月", "二 月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"]
      },
      rangeSeparator: " 至 ",
      weekAbbreviation: "週",
      scrollTitle: "滾動切換",
      toggleTitle: "點擊切換 12/24 小時時制"
    };
    fp.l10ns.zh = Mandarin;
    var zh = fp.l10ns;

    exports.Mandarin = Mandarin;
    exports.default = zh;

    Object.defineProperty(exports, '__esModule', { value: true });

})));