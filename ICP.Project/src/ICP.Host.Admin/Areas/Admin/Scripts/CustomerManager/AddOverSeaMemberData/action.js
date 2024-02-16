//function query() {
//    //$('#queryCName').val(CName);
//    //$('#queryUserID').val(UserID);
//    $('#btnQuery').click();
//}


(function () {
    libs.alert.validationSummary();

    $('input[type=file]').change(function () {
        var AllowExtension = ".csv";

        $in = $(this);
        var inVal = $in.val().toLowerCase();
        var ValLen = inVal.length;
        var LastIndex = inVal.lastIndexOf(".");
        var SubStr = inVal.substr(LastIndex, ValLen - LastIndex);

        $('.fileName').text($('#file').val().split('\\')[$('#file').val().split('\\').length - 1]);

        if (AllowExtension.match(SubStr) == null) {
            alert("檔案格式僅接受.csv, 請重新選擇.")
            if ($.browser.msie) {
                $in.replaceWith($in.clone());
            }
            else {
                $in.val('');
            }
            window.location.reload();
        }
    });


    $("#btSend").click(function (e) {
        e.preventDefault();

        var fileData = $("#file").val();
        if (fileData == "") {
            alert("請選擇檔案")
            return false;
        }
        $("form").submit();
    });
  
})();


