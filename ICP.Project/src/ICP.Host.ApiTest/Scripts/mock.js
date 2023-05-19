$.ajaxSetup({
    cache: false,
    type: 'POST',
    error: function (jqXHR, textStatus, errorThrown) {
        //alert(e.responseText);
    }
});

var async = true;
var ApiListData = [];

function GenerateApiList() {
    var html = '';

    $(ApiListData).each(function (i) {

        html +=
            '<li class="category" index="' + i + '">' +
            '<span class="categoryname">' + this.category + '</span>' +
            '<ul>';

        $(this.list).each(function (j) {
            var api = this.name.split(' ')[0];
            html +=
                '<li class="api" api="' + api + '" index="' + j + '">' +
                '<span class="name">' + this.name + '</span>' +
                '<span class="url">' + this.url + '</span>' +
                '</li>';
        });

        html +=
            '</ul>' +
            '</li>';

        $('#ulApiList').html(html);
    });
}

function GenerateApiSpec(specs, demo) {
    if (!specs) return '';

    var html = '';

    for (var key in specs) {

        var spec = specs[key];
        var specType = typeof (spec);

        if (specType != 'string' && specType != 'object') {
            continue;
        }

        var type;
        var value;
        var array;
        if (demo) {
            value = demo[key];
            if (typeof (value) != "undefined") {
                if (Array.isArray(value)) {
                    array = value;
                    if (array.length) {
                        value = array[0];
                    } else {
                        value = null;
                    }
                }
            }
        }
        if (typeof (value) != "undefined") {
            if (array) {
                type = 'array';
                if (array.length) type += '[' + typeof(value) + ']';
            }
            else
                type = typeof (value);
        }
        else {
            type = '';
        }

        var content =
            '<span class="field">' + key + '</span>' +
            '<span class="type">' + type + '</span>';

        if (specType === 'string') {
            content +=
                '<span class="spec">' + spec + '</span>';
        }
        else if (specType === 'object') {
            content +=
                '<span class="spec">' + (spec.name || '') + '</span>' +
                '<ul>' + GenerateApiSpec(spec.specs, value) + '</ul>';
        }

        html += '<li>' + content + '</li>';
    }

    return html;
}

function BindApiListClickEvt() {

    $('#ulApiList > li.category > span.categoryname').click(function(){
        $(this).parent().toggleClass('hideList');
    });

    $('#ulApiList li.api').click(function () {

        var $li = $(this);

        var _class = 'actived';
        $('#ulApiList > li > ul > li.' + _class + ':first').removeClass(_class);
        $li.addClass(_class);

        var $files = $('#files');
        var html = $('#files')[0].outerHTML;
        var $prev = $files.prev();
        $files.remove();
        $(html).insertAfter($prev);

        var i = parseInt($li.parents('li:first').attr('index'), 10);

        var j = parseInt($li.attr('index'), 10);

        var api = ApiListData[i].list[j];

        $('#req_host').val(api.host);

        $('#req_url').val(api.url);

        var $fileCols = $('#fileCols');

        if (api.json.fileCols) {
            $fileCols.val(api.json.fileCols);
            delete api.json.fileCols;
        }
        else
            $fileCols.val('');

        $('#txtJSON').val(JSON.stringify(api.json, null, '\t'));

        $('#ulApiSpec').html(GenerateApiSpec(api.specs, api.json));
    });
}

function ListApi() {
    $.ajax({
        url: '/Mock/ListApi',
        async: false,
        success: function (data) {
            ApiListData = data;
            GenerateApiList();
        }
    });
}

function GetTimeString(d) {
    if (!d) d = new Date();
    var date =
    [
        d.getFullYear(),
        d.getDate(),
        d.getMonth() + 1
    ].join('/');
    var time =
    [
        d.getHours(),
        d.getMinutes(),
        d.getSeconds()
    ].join(':') + '.' + d.getMilliseconds();
    return date + ' ' + time;
}

$('#req_env').change(function () {
    var req_env = this.value;
    $.ajax({
        url: '/Mock/GetCertStatus',
        data: JSON.stringify({
            env: req_env
        }),
        cache: false,
        type: 'POST',
        contentType: "application/json",
        dataType: "json",
        async: true,
        success: function (res) {
            $('#divLogin').attr('login', res.MID > 0 ? '1' : '0');
            $('#labAccount').text(res.UserCode);
            $('#labMID').text(res.MID);
        }
    });
});

$('form:first').submit(function () {

    var req_env = $('#req_env').val();

    var req_url = $('#req_url').val();
    if (!req_url) {
        $('#labErr').text(' Url empty');
        return false;
    }

    var req_host = $('#req_host').val();
    if (!req_host) {
        $('#labErr').text(' Host empty');
        return false;
    }

    var req_json = $('#req_Json').val();

    $('#h3Request span.time').text(GetTimeString(new Date()));

    var req_file = $('#fileCols').val();

    $('#h3Request span.time').text(GetTimeString(new Date()));

    var url = '/Mock/CallNormalApi';

    var data = new FormData(this);
    data.append('env', req_env);
    data.append('host', req_host);
    data.append('url', req_url);
    data.append('json', req_json);
    data.append('fileCols', req_file);

    var opts = {
        url: url,
        data: data,
        cache: false,
        type: 'POST',
        contentType: false,
        processData: false,
        dataType: "json",
        async: async,
        success: function (res) {
            $('#h3Response span.time').text(GetTimeString(new Date()) + ' (' + res.delag + 'ms)');
            $('#txtResult').val(JSON.stringify(JSON.parse(res.result), null, '\t'));
        }
    };

    if (data.fake) {
        // Make sure no text encoding stuff is done by xhr
        opts.xhr = function () { var xhr = jQuery.ajaxSettings.xhr(); xhr.send = xhr.sendAsBinary; return xhr; }
        opts.contentType = "multipart/form-data; boundary=" + data.boundary;
        opts.data = data.toString();
    }

    $.ajax(opts);

    return false;
});

$('#btnQuery').click(function () {

    var $txtJSON = $('#txtJSON');

    var json = $txtJSON.val();
    if (!json) return;

    var obj;
    try {
        obj = JSON.parse(json);
    }
    catch (e) {
        $('#labErr').text(e.message);
        return;
    }

    $txtJSON.val(JSON.stringify(obj, null, '\t'));
    $('#labErr').text('');
    $('#req_Json').val(JSON.stringify(obj));
    $('form:first').submit();
    return false;
});

$('#btnLogin').click(function () {
    var UserCode = $('#txtUserCode').val();
    var UserPwd = $('#txtUserPwd').val();
    if (!UserCode || !UserPwd) return false;

    async = false;
    var $ulApiList = $('#ulApiList');
    var request = { AuthV: "mock", UserCode: UserCode };
    $ulApiList.find('li[api="M0001"]:first').click();
    $('#txtJSON').val(JSON.stringify(request));
    $('#btnQuery').click();
    
    var res = JSON.parse($('#txtResult').val());
    if (res.RtnCode == 1) {
        request = {
            LoginTokenID: res.EncData.LoginTokenID,
            LoginType: 1,
            UserCode: UserCode,
            UserPwd: UserPwd
        };
        $ulApiList.find('li[api="M0005"]:first').click();
        $('#txtJSON').val(JSON.stringify(request));
        $('#btnQuery').click();

        res = JSON.parse($('#txtResult').val());
        if (res.RtnCode == 1) {
            $('#req_env').change();
        }
    }
    async = true;
    return false;
});

$('#btnLogout').click(function () {
    $('#req_host').val('member');
    $('#req_url').val('/api/Member/MemberInfo/UserCodeLogout');
    $('#txtJSON').val('{}');
    async = false;
    $('#btnQuery').click();
    $('#req_env').change();
    async = true;
    return false;
});

ListApi();
BindApiListClickEvt();
$('#req_env').change();