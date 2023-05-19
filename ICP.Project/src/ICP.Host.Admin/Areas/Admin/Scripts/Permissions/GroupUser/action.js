(function () {
    $('#queryUserGroupID').change(function () {
        var select = document.getElementById('queryUserID');
        if (!this.value) {
            select.disabled = true;
            select.selectedIndex = 0;
            select.options.length = 1;
            return;
        }

        var url = $(this).attr('url');
        $.ajax({
            url: url + '/' + this.value,
            method: "POST",
            success: function (result) {
                select.selectedIndex = 0;
                select.options.length = result.length + 1;
                $(result).each(function (i) {
                    var item = new Option(this.CName + '／' + this.Account, this.UserID);
                    select.options[i + 1] = item;
                });
                select.disabled = false;
            }
        });
    });
})();