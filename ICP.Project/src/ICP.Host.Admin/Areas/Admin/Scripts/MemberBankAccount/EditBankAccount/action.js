(function () {
    // 顯示輸入文字長度
    $('#MemberBankAccount_AuthMsg').keyup(function (e) {
        $(this).next().find('span').text(this.value.length);
    });

    // 選擇圖片 銀行存摺封面
    $('#divBankAccountFiles').on('click', 'a[file], input[file]', function () {
        var $t = $(this);
        var fileSelector = $t.attr('file');
        $(fileSelector).trigger('click');
        return false;
    });

    //上傳顯示縮圖
    $('input[type=file]').change(function () {
        var $t = $(this);
        var img = $t.attr('img');
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $(img).attr('src', e.target.result);
                $(img).show();
            }
            reader.readAsDataURL(this.files[0]);
        }
    });

    $('#MemberBankAccount_BankCode').change(function () {
        var val = this.value;
        var href = $(this).attr('href');

        var data = {
            BankCode: val
        };

        $.ajax({
            url: href,
            method: 'POST',
            data: data,
            success: function (result) {
                if (result.length > 0) {
                    $('#MemberBankAccount_BankBranchCode').empty();

                    $.each(result, function (i, item) {
                        $('#MemberBankAccount_BankBranchCode').append($('<option></option>').val(item.BankBranchCode).text(item.BankName));
                    });
                }
            }
        });
    });

    $('#lnkEditBankAccount').click(function () {
        let numberPattern = /^\d+$/;
        let BankAccount = $('#MemberBankAccount_BankAccount').val();
        if (!numberPattern.test(BankAccount)) {
            var content = '銀行帳號格式錯誤';
            libs.alert.popup(content);
            return false;
        }
    });
})();