(function () {
    countWords();
})();

function formSubmit() {
    if (!$('#upd_submit').is('.disabled')) {
        $('#cal_upd_form').submit();
    }
}

function checkFields(dayDescriptionWordsLength) {
    if (dayDescriptionWordsLength == undefined) {
        dayDescriptionWordsLength = $('#DayDescription').val().length;
    }

    let isDayTypeChecked = ($('#day_holiday').prop("checked") || $('#day_makeup').prop("checked"));
    let isDayDescriptionFillIn = dayDescriptionWordsLength >= 5 && dayDescriptionWordsLength <= 100;

    if (isDayTypeChecked && isDayDescriptionFillIn) {
        $('#upd_submit').removeClass("disabled");
    } else {
        $('#upd_submit').addClass("disabled");
    }
}

function countWords() {
    let dayDescriptionWordsLength = $('#DayDescription').val().length;
    $('#upd_desc_length').text(dayDescriptionWordsLength);
    checkFields(dayDescriptionWordsLength);
}