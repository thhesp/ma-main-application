if (!control.creatingNewDomainSetting()) {
    console.log("initialize data...");

    $('#identifier').val(control.getIdentifier());

    var identifiers = control.getAOIIdentifiers();
    var uids = control.getAOIUIDs();

    for (var i = 0; i < identifiers.length; i++) {
        var html = "<tr uid='" + uids[i] + "><td>" + identifiers[i] + "</td><td><i class='edit fa fa-pencil-square-o'></i></td><td><i class='copy fa fa-files-o'></i></td><td><i class='delete fa fa-times'></i></td></tr>";

        $('#aoi-table').append(html);
    }

    $('#aoi-table .edit').click(function () {
        console.log($(this));
        
    });


    $('#aoi-table .copy').click(function () {
        console.log($(this));
        
    });

    $('#aoi-table .delete').click(function () {
        console.log($(this));
        $(this).closest('tr').remove();
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


$('#add-aoi').click(function () {
    //show window
});