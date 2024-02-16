(function () {

})();

function formSubmit() {
    if (!$('#add_submit').is('.disabled')) {
        $('#cal_add_form').submit();
    }
}

function checkFields(dayDescriptionWordsLength) {
    if (dayDescriptionWordsLength == undefined) {
        dayDescriptionWordsLength = $('#DayDescription').val().length;
    }

    let isDayTypeChecked = ($('#day_holiday').prop("checked") || $('#day_makeup').prop("checked"));
    let isDayDescriptionFillIn = dayDescriptionWordsLength >= 5 && dayDescriptionWordsLength <= 100;

    if (isDayTypeChecked && isDayDescriptionFillIn) {
        $('#add_submit').removeClass("disabled");
    } else {
        $('#add_submit').addClass("disabled");
    }
}

function countWords() {
    let dayDescriptionWordsLength = $('#DayDescription').val().length;
    $('#add_desc_length').text(dayDescriptionWordsLength);
    checkFields(dayDescriptionWordsLength);
}