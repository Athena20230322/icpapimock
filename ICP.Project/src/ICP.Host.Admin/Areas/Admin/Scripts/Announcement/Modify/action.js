$(function () {
    setDefaultSetting();

    var config = {
        height: 260,
        width: 782,
        enterMode: CKEDITOR.ENTER_BR,
        allowedContent: true,
        extraPlugins: 'wordcount',
        wordcount: {
            showParagraphs: false,
            showWordCount: false,
            showCharCount: true,
            countSpacesAsChars: false,
            countHTML: false,
            maxWordCount: -1,
            maxCharCount: 1000,
            charCountGreaterThanMaxLengthEvent: function (currentLength, maxLength) {
                $("#informationchar").css("background-color", "crimson").css("color", "white").text(currentLength + "/" + maxLength + " - char").show();
            },
            charCountLessThanMaxLengthEvent: function (currentLength, maxLength) {
                $("#informationchar").css("background-color", "white").css("color", "black").hide();
            }
        }
    };

    $('#AnnounceContent')
        .ckeditor(config)
        .editor.on('change', function () {
            $(this.element.$).trigger('change');
            $('#contentError').hide();
        });
})

function submitConfirm(isSubmit) {
    var chk = true;

    if ($('#AnnounceContent').val() == "") {
        $('#contentError').show();
        chk = false;
    }

    if ($("input[name='IsTop']:checked").val() == "1") {
        if ($('#IsTopStartDate').val() == '' || $('#IsTopStartDateTime').val() == '' ||
            $('#IsTopEndDate').val() == '' || $('#IsTopEndDateTime').val() == '') {
            $('#isTopTimeError').show();
            chk = false;
        }
        else {
            var startDate = $('#IsTopStartDate').val() + ' ' + $('#IsTopStartDateTime').val();
            var endDate = $("#IsTopEndDate").val() + ' ' + $('#IsTopEndDateTime').val() ;
            if (new Date(endDate) <= new Date(startDate)) {
                $('#isTopTimeError').text('置頂起迄時間錯誤');
                $('#isTopTimeError').show();
                chk = false;
            }
        }
    }

    if ($("input[name='AnnounceType']:checked").val() == "1") {
        if ($('#CsvPathList').find('input').length == 0) {
            $('#MidFileError').show();
            chk = false;
        }
    }

    if (chk && isSubmit && $('#IsTest').val() == "False") {
        if (!confirm("您尚未測試發送，確定要直接送出訊息嗎?")) {
            chk = false;
        }
    }

    return chk;
}

$('#preViewImageList').on('click', '.del', function () {
    if (confirm("確定刪除？")) {
        var imgUrl = $(this).closest('li').children().find('img').attr('src');
        $('#ImagePathList').find('input').filter(function () {
            return this.value === imgUrl;
        }).remove();
        $(this).closest('li').remove();
        $('#ImageFileUrl').html('未選擇任何檔案');
    }
});

$('#preViewImageList').on('click', '.dfp-holder', function () {
    var imgUrl = $(this).find('img').attr('src');
    var clipboard = new Clipboard('img', {
        text: function (trigger) {
            alert('已複製圖片位置');
            clipboard.destroy();
            return imgUrl;
        }
    });
});

function renderImg(rtnData) {
    var inputName = 'ImagePathList[' + $('#ImagePathList').find('input').length + ']';
    var input = $('<input type="hidden" />').attr('name', inputName).val(rtnData.path);
    $('#ImagePathList').append(input);

    var preViewImg = $('<img />').attr('src', rtnData.url).attr('alt', rtnData.path);
    var imgDiv = $('<div class="dfp-holder" />').append(preViewImg);
    var imgCloseDiv = $('<div class="dfp-close" />').append('<span class="icon-ic_fail_svg dpc-ic del"></span>')
    var li = $('<li />').append(imgDiv).append(imgCloseDiv);
    $('#preViewImageList').append(li);

    $('#ImageFileUrl').html(rtnData.path);
}

function renderCsv(rtnData) {
    var index = $('#CsvPathList').find('.FileID').length;
    $('#CsvPathList').append($('<input type="hidden" />').addClass('item' + index).addClass('FileID').attr('name', 'CsvPathList[' + index + '].FileID').val(0));
    $('#CsvPathList').append($('<input type="hidden" />').addClass('item' + index).addClass('FileName').attr('name', 'CsvPathList[' + index + '].FileName').val(rtnData.path));
    $('#CsvPathList').append($('<input type="hidden" />').addClass('item' + index).addClass('Status').attr('name', 'CsvPathList[' + index + '].Status').val(1));

    var preViewUrl = $('<a class="dfl-name" target = "_blank" />').attr('href', rtnData.path).append('發送名單檔' + (index + 1));
    var preViewUrlDelete = $('<a class="dfl-delete" />').append('<span>刪除</span>');
    var li = $('<li />').addClass('item' + index).append(preViewUrl).append(preViewUrlDelete);
    $('#preViewCsvList').append(li);

    $('#CsvFileUrl').html(rtnData.path);
    $('#MidFileError').hide();
}

$('#preViewCsvList').on('click', '.dfl-delete', function () {
    if (confirm("確定刪除？")) {
        var thisClass = $(this).closest('li').attr('class');
        $('#CsvPathList').find('.' + thisClass + '.Status').val(0);
        $(this).closest('li').remove();
        $('#CsvFileUrl').html('未選擇任何檔案');
    }
});

$('#Title').on('keyup', function () {
    $('#titleLength').text(this.value.length + '/20');
}); 

$('#AnnounceContent').on('keyup', function () {
    $('#contentLength').text(this.value.length + '/1000');
});  

$('#TestMidList').on('change', function () {
    $('#btnTest').removeClass('disabled');
});

function setDefaultSetting() {
    isTopSetting();
    announceTypeSetting();
}

function isTopSetting() {
    var value = $("input[name='IsTop']:checked").val()
    if (value == '1') {
        $('#IsTopStartDate').attr("disabled", false);
        $('#IsTopStartDateTime').attr("disabled", false);
        $('#IsTopEndDate').attr("disabled", false);
        $('#IsTopEndDateTime').attr("disabled", false);
    }
    else {
        $('#IsTopStartDate').attr("disabled", true);
        $('#IsTopStartDateTime').attr("disabled", true);
        $('#IsTopEndDate').attr("disabled", true);
        $('#IsTopEndDateTime').attr("disabled", true);
        $('#isTopTimeError').hide();
    }
}

function announceTypeSetting() {
    var value = $("input[name='AnnounceType']:checked").val()
    if (value == '1') {
        $('#MidFile').attr("disabled", false);
        $('#MidFile').closest('div').children('.btn').removeClass('disabled')
    }
    else {
        $('#MidFile').attr("disabled", true);
        $('#MidFile').closest('div').children('.btn').addClass('disabled')
    }
}

function getSendTestData() {
    var data = {
            model: {
                CategoryID: $('#CategoryID').val(),
                Title: $('#Title').val(),
                AnnounceContent: $('#AnnounceContent').val(),
                TestMidList: $('#TestMidList').val()
            }
    };
    return data;
}