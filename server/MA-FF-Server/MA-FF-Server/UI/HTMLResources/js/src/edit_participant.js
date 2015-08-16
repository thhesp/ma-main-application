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

    if (validateData()) {
        control.setIdentifier($('#identifier').val());
        control.setBirthyear(parseInt($('#birthyear').val()));
        control.setEducation($('#education').val());

        var sex = $("#sex input[type='radio']:checked").attr('id');

        control.setSex(sex);

        extractExtraData();


        console.log(control.getIdentifier(), control.getBirthyear(), control.getEducation());

        control.saveParticipant();
    }
});

function validateData() {
    var valid = true;

    valid = validateExtraData();

    return valid;
}

function validateExtraData() {
    var keys = [];

    $('#extra-data-table tr .key').each(function () {
        $(this).removeClass('invalid');
        keys.push($(this).val());
    });

    var duplicates = getDuplicates(keys);

    if (duplicates.length == 0) {
        return true;
    } else {
        console.log('duplicates found', duplicates);
        //mark duplicates
        $('#extra-data-table tr .key').each(function () {
            if (duplicates.indexOf($(this).val()) != -1) {
                $(this).addClass('invalid');
            }
        });

        alert("Bezeichnungen für Zusatzdaten dürfen pro Teilnehmer nur einmal verwendet werden.");

        return false;
    }
}

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

function getDuplicates(array) {
    var sorted_arr = array.sort(); 
    var results = [];
    for (var i = 0; i < sorted_arr.length - 1; i++) {
        if (sorted_arr[i + 1] == sorted_arr[i]) {
            results.push(sorted_arr[i]);
        }
    }

    return results;
}