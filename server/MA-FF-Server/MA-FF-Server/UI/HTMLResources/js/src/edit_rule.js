if (!control.creatingNewRule()) {
    console.log("initialize data...");

    
}

$('#save-button').click(function () {
    console.log('save domain setting...');

    

    control.saveRule();
});

$('#cancel-button').click(function () {
    console.log('cancel domain...');
    control.cancel();
});


$('#add-rule').click(function () {
    //show window
    control.createRule();
});