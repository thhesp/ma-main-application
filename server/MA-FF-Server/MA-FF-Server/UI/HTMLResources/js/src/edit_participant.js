if(!control.creatingNewParticipant()){
    console.log("initialize data...");

    $('#identifier').val(control.getIdentifier());

    $('#birthyear').val(control.getBirthyear());

    $('#education').val(control.getEducation());

    $('#sex #' + control.getSex()).prop("checked", true);

    var keys = control.getExtraDataKeys();
    var values = control.getExtraDataValues();

    var template = _.template($('script#extra-data-template').html());

    for(var i = 0; i < keys.length; i++){
        $("#extra-data-table").append(template({ key: keys[i], value: values[i] }));
    }

    $('#extra-data-table .delete').click(function () {
        console.log($(this));
        $(this).closest('tr').remove();
    });
}

$('#save-button').click(function () {
    console.log('save participant...');

    control.setIdentifier($('#identifier').val());
    control.setBirthyear(parseInt($('#birthyear').val()));
    control.setEducation($('#education').val());

    var sex = $("#sex input[type='radio']:checked").attr('id');

    control.setSex(sex);

    extractExtraData();
    

    console.log(control.getIdentifier(), control.getBirthyear(), control.getEducation());
    
    control.saveParticipant();
});

function extractExtraData() {
    console.log("extract extra data...");
    $('#extra-data-table tr').each(function () {
        var key = $(this).find('.key').val();
        var value = $(this).find('.value').val();

        console.log("Key: ", key, "Value: ", value);

        if (key != "" && key != undefined && value != "" && value != undefined) {
            control.addExtraData(key, value);
        }
    });
};

$('#cancel-button').click(function () {
    console.log('cancel participant...');
    control.cancel();
});


$('#add-data').click(function () {
    var template = _.template($('script#extra-data-template').html());

    $("#extra-data-table").append(template({ key: '', value: '' }));

    $("#extra-data-table .delete").off("click", "**");

    $('#extra-data-table .delete').click(function () {
        console.log($(this));
        $(this).closest('tr').remove();
    });
});

$("#sex input[type='radio']").on('change', function () {
    $("#sex input[type='radio']").prop('checked', false);
    $(this).prop('checked', true);
});