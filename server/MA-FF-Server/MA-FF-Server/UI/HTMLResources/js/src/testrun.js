$("#participant-identifier").text(control.getParticipantIdentifier());

$("#protocol").val(control.getProtocol());

$("#label").val(control.getLabel());

$("#test-control-button").click(function () {
    if (control.testRunning()) {
        control.stopTestrun();
        control.addTestrunData();
        $('#select-participant-button').attr('disabled', false);
        $('#close-button').attr('disabled', false);

        $(this).text("Start Test");
    } else {
        if ($("#participant-identifier").text() == "") {
            alert("Es muss ein Experimentteilnehmer ausgewählt werden.");
            return;
        }

        $('#select-participant-button').attr('disabled', true);
        $('#close-button').attr('disabled', true);

        control.startTestrun();
        $(this).text("Stop Test");
    }
});

$('#close-button').click(function () {
    control.addTestrunData();
    control.endTest();
});

$('#select-participant-button').click(function () {
    if (!control.testRunning()) {
        control.selectParticipant();
    }
});

$("#protocol").on('change keyup paste', function () {
    control.updateProtocol($(this).val());
});


$("#label").on('change keyup paste', function () {
    control.updateLabel($(this).val());
});

function showSaveIndicator() {
    jQuery('#saving-wrapper').show();
};

function hideSaveIndicator() {
    jQuery('#saving-wrapper').hide();
};