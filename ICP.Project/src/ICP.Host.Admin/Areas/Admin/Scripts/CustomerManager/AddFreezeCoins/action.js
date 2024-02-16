(function () {
    //libs.alert.validationSummary();

    $('#Remark').on("keyup", function () {
        countChar( "#Remark", "#input-tip-txt", 200); 
    }).on("keydown", function () {
        countChar( "#Remark", "#input-tip-txt", 200);
        });

    //$('.link-submit').on('click', function () {
    //    $('form').validate();
    //    if ($('form').valid()) {
    //        $('form').submit();
    //    } else {
    //        return false;
    //    }
    //});
  
})();


