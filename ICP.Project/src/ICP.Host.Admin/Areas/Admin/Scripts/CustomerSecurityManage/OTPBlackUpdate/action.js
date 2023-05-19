(function () {
    libs.alert.validationSummary();

    // 顯示輸入文字長度
    $('#Memo').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });

    
})();


