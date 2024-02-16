function ListAndValues(value, max) {
    var results = [];
    var andValue = 1;
    while (andValue <= max) {
        if ((value & andValue) == andValue) {
            results.push(andValue);
        }
        andValue *= 2;
    }
    return andValue;
}

function SetCommoditiyType() {
    var result = 0;
    $('#CommoditiyTypeChecks input[type="checkbox"]:checked').each(function () {
        result += parseInt(this.value, 10)
    });
    $('#basic_CommoditiyType').val(result || '').change();
}

function SetCommoditiyType_Checks() {
    var max = 128;
    var value = $('#basic_CommoditiyType').val();
    var results = ListAndValues(value, max);
    var $container = $('#CommoditiyTypeChecks');
    $(results).each(function () {
        $container.find('input[type="checkbox"][value="' + this + '"]:first').prop(true);
    });
}

function SetGatewayItem() {
    var result = 0;
    $('#GatewayItemChecks input[type="checkbox"]:checked').each(function () {
        result += parseInt(this.value, 10)
    });
    $('#basic_GatewayItem').val(result || '').change();
}

function SetGatewayItem_Checks() {
    var max = 128;
    var value = $('#basic_GatewayItem').val();
    var results = ListAndValues(value, max);
    var $container = $('#GatewayItemChecks');
    $(results).each(function () {
        $container.find('input[type="checkbox"][value="' + this + '"]:first').prop(true);
    });
}

function cbAccountRepeat(result) {
    var $dom = $('#statusAccountRepeat');
    var _class = 'icon-ic_fin_svg';
    if (result.RtnCode == 1) {
        $dom.addClass(_class);
    }
    else {
        $dom.removeClass(_class);
        if (result.RtnMsg) libs.alert.popup(result.RtnMsg);
    }
}

function unique(arr) {
    var i,
        len = arr.length,
        out = [],
        obj = {};

    for (i = 0; i < len; i++) {
        obj[arr[i]] = 0;
    }
    for (i in obj) {
        out.push(i);
    }
    return out;
};

function IgnoreFields() {
    var fields = [];

    //提領規則 0:不啟用
    if ($('input[name="detail.TransferSchedule"]:checked').val() == 'false') {
        fields.push('detail.ScheduleType');
        fields.push('detail.AMTransferType');
        fields.push('detail.TransferAmount');
        fields.push('detail.ScheduleValue');
    }
    else if ($('input[name="detail.ScheduleType"]:checked').val() == '1') {
        fields.push('detail.ScheduleValue');
    }

    //特店身份 1:公司戶
    var CustomerType = $('#basic_CustomerType').val();
    if (CustomerType == '1') {
        //負責人類型 0:自然人
        var PrincipalType = $('#detail_PrincipalType').val();
        if (PrincipalType == '0') {
            fields.push('detail.PrincipalCompanyName');
            fields.push('detail.PrincipalUnifiedBusinessNo');
        }
        //負責人類型 1:法人
        else if (PrincipalType == '1') {
            fields.push('detail.NationalityID');
            fields.push('detail.IDNO');
            fields.push('detail.UniformID');
            fields.push('detail.OverSeasIDNO');
            fields.push('detail.CName');
            fields.push('detail.CName_EN');
            fields.push('detail.PrincipalBirthday');
            fields.push('detail.OccupationID');
        }
    }
    //特店身份 2:個人戶
    else if (CustomerType == '2') {
        fields.push('detail.CompanyName');
        fields.push('detail.CompanyType');
        fields.push('detail.UnifiedBusinessNo');
        fields.push('detail.IncorporationDate');
        fields.push('detail.CapitalAmount');
        fields.push('detail.PrincipalCompanyName');
        fields.push('detail.PrincipalUnifiedBusinessNo');
    }

    //國籍 1206 台灣
    var NationalityID = $('#detail_NationalityID').val();
    NationalityID = parseInt(NationalityID, 10);
    if (NationalityID == 1206) {
        fields.push('detail.UniformID');
        fields.push('detail.OverSeasIDNO');
    }
    //國籍 外國
    else if (NationalityID > 0) {
        fields.push('detail.IDNO');
    }

    fields = unique(fields);

    var ignoreClass = 'validignore';
    $('.' + ignoreClass).removeClass(ignoreClass);
    $(fields).each(function () {
        $('[name="' + this + '"]').addClass(ignoreClass);
    });
}

$(function () {

    var $form = $('.link-submit').parents('form:first');

    $('#CommoditiyTypeChecks input[type="checkbox"]').change(SetCommoditiyType);

    $('#GatewayItemChecks input[type="checkbox"]').change(SetGatewayItem);

    $('#ScheduleType_4_Custom_Values').change(function () {
        $('#detail_ScheduleValue').val(this.value);
    });

    $('#ScheduleType_2_Values, #ScheduleType_4_Values').change(function () {
        $form.attr('ScheduleValue', this.value);
        if (this.value != 'custom') {
            $('#detail_ScheduleValue').val(this.value);
        }
        else {
            $('#ScheduleType_4_Custom_Values').change();
        }
    });

    $('input[name="detail.ScheduleType"]').change(function () {
        $form.attr('ScheduleType', this.value);
        if (this.value && this.value != '1')
            $('#ScheduleType_' + this.value + '_Values').change();
    }).filter(':checked').change();

    $('input[name="detail.TransferSchedule"]').change(function () {
        $form.attr('TransferSchedule', this.value);
    }).filter(':checked').change();

    $('#basic_CustomerType').change(function () {
        $form.attr('CustomerType', this.value);
        if (this.value == '2') $('#detail_PrincipalType').val('0').change();
    }).change();

    $('#detail_PrincipalType').change(function () {
        $form.attr('PrincipalType', this.value);
    }).change();

    var $NationalityID = $('#detail_NationalityID').change(function () {
        if (this.value)
            $form.attr('NationalityID', this.value);
        else
            $form.removeAttr('NationalityID');
    });
    if ($NationalityID.val()) $NationalityID.change();

    $('#lnkCheckAccountRepeat').click(function () {
        var $t = $(this);
        var selector = $t.attr('check');
        var value = $(selector).val();
        if (!value) return false;
        var url = $t.attr('href');
        var errorMsg = $t.attr('errorMsg');
        var successMsg = $t.attr('successMsg');
        var cb = $t.attr('callback');
        cb = window[cb];
        $.ajax({
            url: url,
            data: { value: value },
            success: function (result) {
                if (cb) { cb(result); return; }
                var msg = '';
                if (result.RtnCode != 1) {
                    msg = errorMsg;
                }
                else {
                    msg = successMsg;
                }
                if (!msg) msg = result.RtnMsg;
                libs.alert.popup(msg);
            }
        });
        return false;
    });

    SetCommoditiyType_Checks();
    SetGatewayItem_Checks();

    $form.data("validator").settings.ignore = '.validignore';
    $('input[name="detail.TransferSchedule"], #basic_CustomerType, #detail_PrincipalType, #detail_NationalityID').change(IgnoreFields);
    IgnoreFields();
});