(function () {
    // 顯示輸入文字長度
    $('#Note').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });
})();