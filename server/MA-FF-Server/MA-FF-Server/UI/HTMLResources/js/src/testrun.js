$("#participant-identifier").text(control.getParticipantIdentifier());

$("#test-control-button").click(function () {
    if(control.testRunning()){
        control.stopTestrun();
        $(this).text("Start Test");
    } else {
        control.startTestrun();
        $(this).text("Stop Test");
    }
});

$('#close-button').click(function() {
    control.endTest();
});

$('#select-participant-button').click(function () {
    control.selectParticipant();
});