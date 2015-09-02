$(document).ready(function () {
    $('#import-experiment-button').attr('disabled', true);
    $('#import-settings').attr('disabled', true);
    $('#import-participants').attr('disabled', true);
    $('#import-experiment').text("");
});

$('#import-values').change(function () {
    if (!this.checked) {
        $('#import-experiment-button').attr('disabled', true);
        $('#import-settings').attr('disabled', true);
        $('#import-participants').attr('disabled', true);
        $('#import-experiment').text("");
    } else {
        $('#import-experiment-button').attr('disabled', false);
        $('#import-settings').attr('disabled', false);
        $('#import-participants').attr('disabled', false);
    }
});

$('#import-experiment-button').click(function () {
    expWizard.selectExperimentToImportData();
});

function updateImportExperiment(path) {
    console.log("Import path: " + path);
    $('#import-experiment').text(path);
}

$('#create-experiment-button').click(function () {
    console.log('create experiment...');
    var name = $('#experiment-name').val();

    if ($('#import-values').prop('checked')) {
        var importSettings = $('#import-settings').prop('checked');
        var importParticipants = $('#import-participants').prop('checked');

        console.log("importSettings: ", importSettings);
        console.log("importParticipants: ", importParticipants);

        expWizard.createExperimentWithImport(name, importSettings, importParticipants);
    } else {
        expWizard.createExperiment(name);
    }
});

$('#back-button').click(function () {
    expWizard.back();
});