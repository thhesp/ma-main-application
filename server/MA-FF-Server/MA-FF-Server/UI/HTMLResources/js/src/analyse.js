$('#select-participant-button').click(function () {
    analysisExportControl.selectParticipant();
});

$('#select-testrun-button').click(function () {
    if (jQuery('#participant-identifier').text() != "") {
        analysisExportControl.selectTestrun();
    } else {
        alert("Es muss zu erst ein Teilnehmer ausgewählt werden.");
    }

});

$('#select-folder-button').click(function () {
    analysisExportControl.selectFolder();
});

$('#export-button').click(function () {
    //extract export-type
    //extract filename

    //extract algorithm informations

    //analysisExportControl.analyse();
});

$("#export-format input[type='radio']").on('change', function () {
    $("#export-format input[type='radio']").prop('checked', false);
    $(this).prop('checked', true);
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