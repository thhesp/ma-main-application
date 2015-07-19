if (!control.creatingNewDomainSetting()) {
    console.log("initialize data...");

    $('#domain').val(control.getDomain());

    $('#include-subdomains').prop("checked", control.getIncludeSubdomains());

    var identifiers = control.getAOIIdentifiers();
    var uids = control.getAOIUIDs();

    var template =  _.template($('script#aoi-template').html());

    for (var i = 0; i < identifiers.length; i++) {
        $("#aoi-table").append(template( { uid: uids[i], identifier: identifiers[i] }));
    }

    $('#aoi-table .edit').click(function () {
        console.log($(this));
        var uid = $(this).closest('tr').attr('uid');
        control.editAOI(uid);
    });


    $('#aoi-table .copy').click(function () {
        console.log($(this));
        var uid = $(this).closest('tr').attr('uid');
        control.copyAOI(uid);
    });

    $('#aoi-table .delete').click(function () {
        console.log($(this));
        var uid = $(this).closest('tr').attr('uid');
        control.deleteAOI(uid);
    });
}

$('#save-button').click(function () {
    console.log('save domain setting...');

    control.setDomain($('#domain').val());
    control.setIncludeSubdomains($('#include-subdomains').prop("checked"));

    control.saveDomainSetting();
});

$('#cancel-button').click(function () {
    console.log('cancel domain...');
    control.cancel();
});


$('#add-aoi').click(function () {
    //show window
    control.createAOI();
});