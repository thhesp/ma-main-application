﻿jQuery(document).ready(function () {
    var algorithm = jQuery('#analysis-algorithm').val();

    var template= _.template($('script#distance-algorithm-template').html());

    $("#algorithm-settings").append(template);
});

jQuery('#select-participant-button').click(function () {
    analysisExportControl.selectParticipant();
});

jQuery('#select-testrun-button').click(function () {
    if (jQuery('#participant-identifier').text() != "") {
        analysisExportControl.selectTestrun();
    } else {
        alert("Es muss zu erst ein Teilnehmer ausgewählt werden.");
    }

});

jQuery('#select-folder-button').click(function () {
    analysisExportControl.selectFolder();
});

jQuery('#export-button').click(function () {

    if (validateData()) {
        //extract export-format
        var exportFormat = jQuery("#export-format input[type='radio']:checked").attr('id');
        analysisExportControl.setExportFormat(exportFormat);
        //extract filename
        analysisExportControl.setFilename(jQuery('#filename').val());

        //contain gaze data
        analysisExportControl.setContainGazeData(jQuery('#contain-gaze-data').prop('checked'));

        analysisExportControl.analyse();
    }
});

function validateData() {
    var valid = true;

    jQuery('#select-participant-button').remove('invalid');
    jQuery('#select-testrun-button').remove('invalid');
    jQuery('#select-folder-button').remove('invalid');
    jQuery('#filename').remove('invalid');

    if (jQuery('#participant-identifier').text() == "") {
        jQuery('#select-participant-button').addClass('invalid');
        valid = false;
    }

    if (jQuery('#testrun-created').text() == "") {
        jQuery('#select-testrun-button').addClass('invalid');
        valid = false;
    }

    if (jQuery('#folder-path').text() == "") {
        jQuery('#select-folder-button').addClass('invalid');
        valid = false;
    }

    if (jQuery('#filename').val() == "") {
        jQuery('#filename').addClass('invalid');
        valid = false;
    }

    return valid;
}

$("#export-format input[type='radio']").on('change', function () {
    jQuery("#export-format input[type='radio']").prop('checked', false);
    jQuery(this).prop('checked', true);
});

function showSaveIndicator() {
    jQuery('#saving-wrapper').show();
};

function hideSaveIndicator() {
    jQuery('#saving-wrapper').hide();
};

function setParticipant(identifier) {
    jQuery('#participant-identifier').text(identifier);
};

function setTestrun(created) {
    jQuery('#testrun-created').text(created);
};

function setFolderPath(path) {
    jQuery('#folder-path').text(path);
};

function createDistanceAlgorithm() {
    var minimumDuration = parseFloat(jQuery('#algorithm-settings .duration').val());

    var acceptableDeviation = parseFloat(jQuery('#algorithm-settings .acceptable-deviation').val());

    analysisExportControl.useDistanceAlgorithm(minimumDuration, acceptableDeviation);
}

jQuery('#analysis-algorithm').on('change', function () {
    $("#algorithm-settings").empty();

    var algorithm = jQuery('#analysis-algorithm').val();

    analysisExportControl.setAlgorithmType(algorithm);

    var template;

    switch (algorithm) {
        case 'distance':
            template = _.template($('script#distance-algorithm-template').html());
            break;
    }

    $("#algorithm-settings").append(template);
});