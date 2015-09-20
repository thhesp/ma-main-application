$('input[name=tracking-model]').on('change', function () {
    console.log("change...");
    $('.additional-settings').hide();

    var activeSetting = $('input[name=tracking-model]:checked').attr('id');

    console.log("activeSettings: ", activeSetting);

    $('.additional-settings.' + activeSetting).show();
});


$('#connect-local').on('change', function () {
    if ($(this).prop('checked')) {
        $('#et-sent-ip').attr('disabled', true);

        $('#et-sent-port').attr('disabled', true);

        $('#et-receive-ip').attr('disabled', true);

        $('#et-receive-port').attr('disabled', true);
    } else {
        $('#et-sent-ip').attr('disabled', false);

        $('#et-sent-port').attr('disabled', false);

        $('#et-receive-ip').attr('disabled', false);

        $('#et-receive-port').attr('disabled', false);
    }
});

$('#websocket-port').val(control.getWSPort());

$('#' + control.getTrackingModelType()).prop("checked", true);

$('input[name=tracking-model]').trigger('change');

$('#connect-local').prop("checked", control.getETConnectLocal());

$('#connect-local').trigger('change');

$('#et-sent-ip').val(control.getETSentIP());

$('#et-sent-port').val(control.getETSentPort());

$('#et-receive-ip').val(control.getETReceiveIP());

$('#et-receive-port').val(control.getETReceivePort());

$('#save-button').click(function () {

    if (validateData()) {
        saveData();
    }

});

$('#cancel-button').click(function () {
    console.log('cancel form...');
    control.cancel();
});


function validateData() {

    $('#websocket-port').removeClass("invalid");
    $('#et-sent-ip').removeClass("invalid");
    $('#et-sent-port').removeClass("invalid");
    $('#et-receive-ip').removeClass("invalid");
    $('#et-receive-port').removeClass("invalid");

    var valid = true;

    if (!validatePort($('#websocket-port').val())) {
        $('#websocket-port').addClass("invalid");
        valid = false;
    }

    if(!validateIP($('#et-sent-ip').val())){
        $('#et-sent-ip').addClass("invalid");
        valid = false;
    }

    if (!validatePort($('#et-sent-port').val())) {
        $('#et-sent-port').addClass("invalid");
        valid = false;
    }

    if(!validateIP($('#et-receive-ip').val())){
        $('#et-receive-ip').addClass("invalid");
        valid = false;
    }

    if (!validatePort($('#et-receive-port').val())) {
        $('#et-receive-port').addClass("invalid");
        valid = false;
    }

    return valid;
}

function saveData() {

    console.log('save settings...');

    control.setWSPort(parseInt($('#websocket-port').val()));

    control.setTrackingModelType($('input[name=tracking-model]:checked').attr('id'));

    control.setETConnectLocal($('#connect-local').prop("checked"));

    control.setETSentIP($('#et-sent-ip').val());

    control.setETSentPort(parseInt($('#et-sent-port').val()));

    control.setETReceiveIP($('#et-receive-ip').val());

    control.setETReceivePort(parseInt($('#et-receive-port').val()));
    
    control.saveSettings();
}

function validateIP(string) {

    if (/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(string)) {
        return (true);
    }
    return (false);
}

function validatePort(string) {
    if (/^\d{3,}$/.test(string)) {
        return (true);
    }
    return (false);
}