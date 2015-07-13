if(!newParticipant){
    console.log("initialize data...");

    $('#identifier').val(control.getIdentifier());

    $('#birthyear').val(control.getBirthyear());

    $('#education').val(control.getEducation());

    $('#sex #' + control.getSex()).prop("checked", true);
}

$('#save-button').click(function () {
    console.log('save participant...');

    control.setIdentifier($('#identifier').val());
    control.setBirthyear(parseInt($('#birthyear').val()));
    control.setEducation($('#education').val());

    var sex = $("#sex input[type='radio']:checked").attr('id');

    control.setSex(sex);
    

    console.log(control.getIdentifier(), control.getBirthyear(), control.getEducation());
    
    control.saveParticipant();
});

$('#cancel-button').click(function () {
    console.log('cancel participant...');
    control.cancel();
});