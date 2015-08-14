

$('#save-button').click(function () {
    console.log('save settings...');


    
    control.saveParticipant();
});

$('#cancel-button').click(function () {
    console.log('cancel form...');
    control.cancel();
});