if (!control.creatingNewAOISetting()) {
    console.log("initialize data...");

    $('#identifier').val(control.getIdentifier());
}

$('#save-button').click(function () {
    console.log('save domain setting...');

    control.setIdentifier($('#identifier').val());

    control.saveAOISetting();
});

$('#cancel-button').click(function () {
    console.log('cancel domain...');
    control.cancel();
});


$('#add-rule').click(function () {
    //show window


});