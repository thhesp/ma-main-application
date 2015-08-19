$("#participant-identifier").text(control.getParticipantIdentifier());

$("#test-control-button").click(function () {
    if (control.testRunning()) {
        control.stopTestrun();

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

$('#close-button').click(function() {
    control.endTest();
});

$('#select-participant-button').click(function () {
    if (!control.testRunning()) {
        control.selectParticipant();
    }
});