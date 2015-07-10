$(document).ready(function () {
    $('#import-experiment-button').attr('disabled', true);
    $('#import-aoi').attr('disabled', true);
    $('#import-participants').attr('disabled', true);
    $('#import-experiment').text("");
});

$('#import-values').change(function () {
    if (!this.checked) {
        $('#import-experiment-button').attr('disabled', true);
        $('#import-aoi').attr('disabled', true);
        $('#import-participants').attr('disabled', true);
        $('#import-experiment').text("");
    } else {
        $('#import-experiment-button').attr('disabled', false);
        $('#import-aoi').attr('disabled', false);
        $('#import-participants').attr('disabled', false);
    }
});

$('#import-experiment-button').click(function () {
    var path = expWizard.selectExperimentToImportData();

    $('#import-experiment').text(path);
    // save path as hidden and use it for the import and only display the experiment name?
});

$('#create-experiment-button').click(function () {
    console.log('create experiment...');
    var name = $('#experiment-name').val();

    if ($('#import-values').prop('checked')) {
        var path = $('#import-experiment').text();
        var importAOI = $('#import-aoi').prop('checked');
        var importParticipants = $('#import-participants').prop('checked');

        console.log("importAOI: ", importAOI);
        console.log("importParticipants: ", importParticipants);

        expWizard.createExperimentWithImport(name, path, importAOI, importParticipants);
    } else {
        expWizard.createExperiment(name);
    }
});

$('#back-button').click(function () {
    expWizard.back();
});