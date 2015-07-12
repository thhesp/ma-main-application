if(!newParticipant){
    console.log("initialize data...");
}

$('#save-button').click(function () {
    console.log('save participant...');

    control.setIdentifier($('#identifier').val());
    control.setBirthyear(parseInt($('#birthyear').val()));
    control.setEducation($('#education').val());

    console.log(control.getIdentifier(), control.getBirthyear(), control.getEducation());
    
    control.saveParticipant();
});

$('#cancel-button').click(function () {
    console.log('cancel participant...');
    control.cancel();
});