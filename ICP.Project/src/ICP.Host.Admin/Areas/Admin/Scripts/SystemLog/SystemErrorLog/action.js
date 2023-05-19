(function () {


})();

function checkFields() {
    
    //### 檢核日期在6個月
    if (!chkDiffOfMonth($('#StartDate').val(), $('#EndDate').val(), 6)) {
        if (!confirm('所下的查詢日期區間超過6個月，請問是否真的要查詢，請注意此一查詢可能會花很長的時間!')) {
            return false;
        }
    }

    $('#form0').submit();
}

function resetFields() {
    $('#form0').resetForm();

    var now = new Date();
    var sdt = now.getFullYear() + "-" + ("0" + (now.getMonth() + 1)).slice(-2) + "-" + ("0" + now.getDate()).slice(-2);
    var edt = sdt;

    $("#StartDate").flatpickr({
        allowInput: false,
        defaultDate: sdt,
        altInputClass: "flatpickr flatpickr-input active minD",
        maxDate: edt
    });

    $("#EndDate").flatpickr({
        allowInput: false,
        defaultDate: edt,
        altInputClass: "flatpickr flatpickr-input active maxD",
        minDate: sdt
    });
}

function chkDiffOfMonth(start, end, i) {

    if (start == "" || end == "") {
        return fasle;
    }

    var diff;
    var startArr = start.split('-');
    var endArr = end.split('-');

    if (startArr[0] == endArr[0]) {
        diff = endArr[1] - startArr[1];
    } else {
        diff = (endArr[0] - startArr[0]) * 12 + (endArr[1] - startArr[1]);
    }

    var chk = false;

    if (new Date(start) > new Date(end)) {
        chk = false;
    } else if (diff < i) {
        chk = true;
    } else if (diff == i && endArr[2] < startArr[2]) {
        chk = true;
    }

    return chk;
}
