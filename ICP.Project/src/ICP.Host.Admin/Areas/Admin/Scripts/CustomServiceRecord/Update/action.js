(function () {
    $('#updateCustomServiceRecord').click(function () {
        $('form').validate();
        if (!$('form').valid())
            return false;
        }
    )
    $('#Note').on("keyup", function () {
        countChar( "#Note", "#input-tip-txt", 1000); 
    }).on("keydown", function () {
        countChar( "#Note", "#input-tip-txt", 1000);
    });
})();

function countChar(textareaName, spanName, Length) {
    $(spanName).text($(textareaName).val().length + "/" + Length );
} 