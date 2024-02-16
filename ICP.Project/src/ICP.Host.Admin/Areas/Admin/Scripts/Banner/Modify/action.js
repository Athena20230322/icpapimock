$(function () {
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

    $('#BannerContent')
        .ckeditor(config)
        .editor.on('change', function () {
            $(this.element.$).trigger('change');
            $('#contentError').hide();
        });

    isUseContentSetting()
})

function submitConfirm() {
    var chk = false;
    $('.bannerSiteList').each(function () {
        if ($(this).prop('checked')) {
            chk = true;
        }
    });
    if (!chk) {
        $('#bannerSiteListError').show();
    } else {
        $('#bannerSiteListError').hide();
    }

    if ($('#StartDate').val() == '' || $('#StartDateTime').val() == '' ||
        $('#EndDate').val() == '' || $('#EndDateTime').val() == '') {
        $('#timeError').show();
        chk = false;
    }
    else {
        $('#timeError').hide();
        var startDate = $('#StartDate').val() + ' ' + $('#StartDateTime').val();
        var endDate = $("#EndDate").val() + ' ' + $('#EndDateTime').val();
        if (new Date(endDate) <= new Date(startDate)) {
            $('#timeError').text('起迄時間錯誤');
            $('#timeError').show();
            chk = false;
        }
    }
    if ($('#preViewImagePath li').length == 0) {
        $('#imageFile1Error').show();
        chk = false;
    } else {
        $('#imageFile1Error').hide();
    }

    if ($('#OrderID').val() == '') {
        $('#orderIDError').show();
        chk = false;
    } else {
        $('#orderIDError').hide();
    }
    if ($('#OpenNewWindow1').val() == '') {
        $('#openNewWindow1Error').show();
        chk = false;
    } else {
        $('#openNewWindow1Error').hide();
    }

    if ($("input[name='IsUseContent']:checked").val() == "1") {
        //有內頁
        if ($('#BannerContent').val() == "") {
            $('#contentError').show();
            chk = false;
        } else {
            $('#contentError').hide();
        }
        if ($('#Title').val() == "") {
            $('#titleError').show();
            chk = false;
        } else {
            if ($('#Title').val().length < 2) {
                $('#titleError').show();
                chk = false;
            } else {
                $('#titleError').hide();
            }
        }
        if ($('#UrlLink2').val() != "") {
            if ($("input[name='OpenNewWindow2']:checked").val() == undefined) {
                $('#openNewWindow2Error').show();
                chk = false;
            } else {
                $('#openNewWindow2Error').hide();
            }
        } else {
            $('#openNewWindow2Error').hide();
        }
    }
    else {
        //無內頁
        if ($('#UrlLink1').val() == "") {
            $('#urlLink1Error').show();
            chk = false;
        }
        else {
            $('#urlLink1Error').hide();
        }
    }

    return chk;
}

function isUseContentSetting() {
    var value = $("input[name='IsUseContent']:checked").val()
    if (value == '1') {
        $('#secondPart').show();
        $('#urlPart').hide();
        $('#openNewWindowPart').hide();
    }
    else {
        $('#secondPart').hide();
        $('#urlPart').show();
        $('#openNewWindowPart').show();
    }
}

$('#Title').on('keyup', function () {
    $('#titleLength').text(this.value.length + '/20');
}); 

$('#ImageFile1').on('change', function (e) {
    var data = new FormData();
    var files = $(this)[0].files;

    if (files.length > 0) {
        data.append("ImageFile", files[0]);
    } else {
        return false;
    }

    if (!files[0].size > 2097152) {
        alert("檔案大小最大2M")
        return false;
    }

    if (!files[0].type.match('image.*')) {
        alert("檔案格式僅能使用 jpg / jpeg / png 三種檔案格式");
        return false;
    }

    var reader = new FileReader();    
    reader.readAsDataURL(files[0]);
    reader.onload = function (e) {
        var img = new Image();
        img.src = e.target.result;
        img.onload = function () {
            var height = this.height;
            var width = this.width;
            if (height > 949 || width > 728) {
                if (confirm("尺寸不符是否確定上傳？ (W728/H949)")) {
                    postFile1(data);
                }
                else {
                    return false;
                }
            }
            else {
                postFile1(data);
            }
        };
    }
});

function postFile1(data) {
    $.ajax({
        type: "POST",
        url: "/Banner/UploadImage",
        contentType: false,
        processData: false,
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.RtnCode == 1) {
                renderImg1(result.RtnData);
            } else {
                alert(result.RtnMsg);
            }

            $('#ImageFile1').val(null);
        },
        error: function () {
            alert('請稍候在試 !!');
        }
    });
}

$('#ImageFile2').on('change', function () {
    var data = new FormData();
    var files = $(this)[0].files;
    if (files.length > 0) {
        data.append("ImageFile", files[0]);
    } else {
        return false;
    }

    if (!files[0].size > 2097152) {
        alert("檔案大小最大2M")
        return false;
    }

    if (!files[0].type.match('image.*')) {
        alert("檔案格式僅能使用 jpg / jpeg / png 三種檔案格式");
        return false;
    }

    var reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = function (e) {
        var img = new Image();
        img.src = e.target.result;
        img.onload = function () {
            var width = this.width;
            if (width > 1865) {
                if (confirm("尺寸不符是否確定上傳？ (W185/H不限)")) {
                    postFile2(data);
                }
                else {
                    return false;
                }
            }
            else {
                postFile2(data);
            }
        };
    }
});

function postFile2(data) {
    $.ajax({
        type: "POST",
        url: "/Banner/UploadImage",
        contentType: false,
        processData: false,
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.RtnCode == 1) {
                renderImg2(result.RtnData);
            } else {
                alert(result.RtnMsg);
            }

            $('#ImageFile2').val(null);
        },
        error: function () {
            alert('請稍候在試 !!');
        }
    });
}

function renderImg1(rtnData) {
    var input = $('<input type="hidden" />').attr('name', "ImagePath").val(rtnData.path);
    $('#ImagePathList1').find('input').remove();
    $('#ImagePathList1').append(input);

    var preViewImg = $('<img />').attr('src', rtnData.url).attr('alt', rtnData.path);
    var imgDiv = $('<div class="dfp-holder" />').append(preViewImg);
    var imgCloseDiv = $('<div class="dfp-close" />').append('<span class="icon-ic_fail_svg dpc-ic del"></span>')
    var li = $('<li />').append(imgDiv).append(imgCloseDiv);
    $('#preViewImagePath').empty().append(li); 

    $('#ImageFileUrl1').html(rtnData.path);
}

function renderImg2(rtnData) {
    var inputName = 'ImagePathList[' + $('#ImagePathList2').find('input').length + ']';
    var input = $('<input type="hidden" />').attr('name', inputName).val(rtnData.path);
    $('#ImagePathList2').append(input);

    var preViewImg = $('<img />').attr('src', rtnData.url).attr('alt', rtnData.path);
    var imgDiv = $('<div class="dfp-holder" />').append(preViewImg);
    var imgCloseDiv = $('<div class="dfp-close" />').append('<span class="icon-ic_fail_svg dpc-ic del"></span>')
    var li = $('<li />').append(imgDiv).append(imgCloseDiv);
    $('#preViewImageList').append(li);

    $('#ImageFileUrl2').html(rtnData.path);
}

$('#preViewImagePath').on('click', '.del', function () {
    if (confirm("確定刪除？")) {
        var imgUrl = $(this).closest('li').children().find('img').attr('src');
        $('#ImagePathList1').find('input').filter(function () {
            return this.value === imgUrl;
        }).remove();
        $(this).closest('li').remove();
        $('#ImageFileUrl1').html('未選擇任何檔案');
    }
});

$('#preViewImageList').on('click', '.del', function () {
    if (confirm("確定刪除？")) {
        var imgUrl = $(this).closest('li').children().find('img').attr('src');
        $('#ImagePathList2').find('input').filter(function () {
            return this.value === imgUrl;
        }).remove();
        $(this).closest('li').remove();
        $('#ImageFileUrl2').html('未選擇任何檔案');
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

function chkOrderID() {
    if (submitConfirm()) {
        var data = {
            model: {
                StartDate: $('#StartDate').val(),
                StartDateTime: $('#StartDateTime').val(),
                EndDate: $('#EndDate').val(),
                EndDateTime: $('#EndDateTime').val(),
                OrderID: $('#OrderID').val(),
                BannerID: $('#BannerID').val()
            }
        };
        $.ajax({
            url: "/Banner/CheckBannerOrderID",
            type: "POST",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.RtnCode != 1) {
                    chkPopup();
                } else {
                    $('#btnSubmit').click();
                }
            }
        });
    }
}

function chkPopup() {
    var content = '廣告起迄時間內已有其他廣告用相同排序了，如使用此排序，原廣告排序將自動遞延，是否確定要設定此順序?';
    var isHtml = true;
    var confirmCallback = function () {
        $('#btnSubmit').click();
    };
    libs.alert.confirm(content, confirmCallback, isHtml);
    return false;
}