(function () {
    $('#lnkEditCName').click(function () {
        let cName = $('#CName:first').val();
        if (cName === "") {
            var content = '請輸入姓名';
            libs.alert.popup(content);
            return false;
        }
        else if (cName !== "") {
            var regex = /^['•\u4E00-\u9FA5\uF900-\uFA2D]{2,20}/;

            if (!regex.test(cName)) {
                var content = '會員姓名格式錯誤，請輸入2-20個字，不可包含空格，符號僅可接受「•」';
                libs.alert.popup(content);
                return false;
            }
            else {
                var arry = cName.split("");
                console.log(arry.length);
                if (arry.length >= 3) {
                    for (var i = 0; i < arry.length - 2; i++) {
                        if (arry[i] == arry[i + 1] && arry[i + 1] == arry[i + 2]) {
                            var content = '會員姓名格式錯誤，不接受連續三個重複字';
                            libs.alert.popup(content);
                            return false;
                        }
                    }
                }
            }
        }
    });
})();