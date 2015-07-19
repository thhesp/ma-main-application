if (!control.creatingNewAOISetting()) {
    console.log("initialize data...");

    $('#identifier').val(control.getIdentifier());

    var uids = control.getRuleUIDs();

    var template = _.template($('script#aoi-template').html());

    for (var i = 0; i < uids.length; i++) {
        $("#rule-table").append(template({ uid: uids[i], number: i }));
    }

    $('#rule-table .edit').click(function () {
        console.log($(this));
        var uid = $(this).closest('tr').attr('uid');
        control.editRule(uid);
    });


    $('#rule-table .copy').click(function () {
        console.log($(this));
        var uid = $(this).closest('tr').attr('uid');
        control.copyRule(uid);
    });

    $('#rule-table .delete').click(function () {
        console.log($(this));
        var uid = $(this).closest('tr').attr('uid');
        control.deleteRule(uid);
    });
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
    control.createRule();
});