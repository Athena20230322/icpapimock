function query() {

    $('#btnQuery').click();
}

function Reload() {   
    location.reload();
};   


(function () {
    libs.alert.validationSummary();

    $('#Remark').on("keyup", function () {
        countChar( "#Remark", "#input-tip-txt", 100); 
    }).on("keydown", function () {
        countChar( "#Remark", "#input-tip-txt", 100);
    });
        
})();


function countChar(textareaName, spanName, Length) {
    $(spanName).text($(textareaName).val().length + "/" + Length );
} 