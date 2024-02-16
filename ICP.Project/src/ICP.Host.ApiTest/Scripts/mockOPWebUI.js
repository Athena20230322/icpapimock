$.ajaxSetup({
    cache: false,
    type: 'POST',
    error: function (e) {
        alert(e.responseText);
    }
});

var controller = 'MockOPWebUI';
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

        var i = parseInt($li.parents('li:first').attr('index'), 10);

        var j = parseInt($li.attr('index'), 10);

        var api = ApiListData[i].list[j];

        $('#req_url').val(api.url);

        $('#txtJSON').val(JSON.stringify(api.json, null, '\t'));

        $('#ulApiSpec').html(GenerateApiSpec(api.specs, api.json));
    });
}

function ListApi() {
    $.ajax({
        url: '/' + controller + '/ListApi',
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

$('form:first').submit(function () {

    var req_env = $('#req_env').val();

    var req_url = $('#req_url').val();
    if (!req_url) {
        $('#labErr').text(' Url empty');
        return false;
    }

    var req_json = $('#req_Json').val();

    $('#h3Request span.time').text(GetTimeString(new Date()));

    $('#h3Request span.time').text(GetTimeString(new Date()));

    var url = '/' + controller + '/CallApi';

    var data = new FormData(this);
    data.append('env', req_env);
    data.append('url', req_url);
    data.append('json', req_json);

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
            $('#txtResult').val(JSON.stringify(res.result, null, '\t'));
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


ListApi();
BindApiListClickEvt();