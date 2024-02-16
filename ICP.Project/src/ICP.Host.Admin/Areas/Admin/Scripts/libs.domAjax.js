(function () {
    // libs.domAjax
    window.libs = window.libs || {};
    libs.domAjax = {
        subSelectByMaster: function (masterid, subid, urlSubFormat, masterValue, subValue) {
            var $area = $('#' + masterid).change(function () {
                var $city = $('#' + subid);
                var city = $city[0];
                city.selectedIndex = 0;
                if (!this.value) {
                    city.options.length = 1;
                    $city.trigger('change').trigger('blur');
                    return;
                }
                $.getJSON(urlSubFormat.replace('{0}', encodeURI(this.value)), null, function (data) {
                    city.options.length = 1 + data.length;
                    $(data).each(function (i) {
                        var o = city.options[i + 1];
                        o.value = this.Value;
                        o.text = this.Text;
                    });
                    if (subValue) {
                        $city[0].value = subValue;
                        subValue = null;
                    }
                    $city.trigger('change').trigger('blur');
                });
            });
            if (masterValue) {
                $area[0].value = masterValue;
                $area.change();
                masterValue = null;
            }
        }
    };
})();