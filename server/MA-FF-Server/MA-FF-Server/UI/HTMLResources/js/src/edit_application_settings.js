$('#websocket-port').val(control.getWSPort());

$('#use-mouse-tracking').prop("checked", control.getUseMouseTracking());

$('#connect-local').prop("checked", control.getETConnectLocal());

$('#et-sent-ip').val(control.getETSentIP());

$('#et-sent-port').val(control.getETSentPort());

$('#et-receive-ip').val(control.getETReceiveIP());

$('#et-receive-port').val(control.getETReceivePort());

// disable & enable stuff
if ($('#use-mouse-tracking').prop('checked')) {
    $('#connect-local').attr('disabled', true);

    $('#et-sent-ip').attr('disabled', true);

    $('#et-sent-port').attr('disabled', true);

    $('#et-receive-ip').attr('disabled', true);

    $('#et-receive-port').attr('disabled', true);
} else if ($('#connect-local').prop('checked')) {
    $('#et-sent-ip').attr('disabled', true);

    $('#et-sent-port').attr('disabled', true);

    $('#et-receive-ip').attr('disabled', true);

    $('#et-receive-port').attr('disabled', true);
}

$('#save-button').click(function () {

    if (validateData()) {
        saveData();
    }

});

$('#cancel-button').click(function () {
    console.log('cancel form...');
    control.cancel();
});

$('#use-mouse-tracking').on('change', function () {
    if ($(this).prop('checked')) {
        $('#connect-local').attr('disabled', true);

        $('#et-sent-ip').attr('disabled', true);

        $('#et-sent-port').attr('disabled', true);

        $('#et-receive-ip').attr('disabled', true);

        $('#et-receive-port').attr('disabled', true);
    } else {
        $('#connect-local').attr('disabled', false);

        if (!$('#connect-local').prop('checked')) {
            $('#et-sent-ip').attr('disabled', false);

            $('#et-sent-port').attr('disabled', false);

            $('#et-receive-ip').attr('disabled', false);

            $('#et-receive-port').attr('disabled', false);
        }

    }
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

    control.setWSPort($('#websocket-port').val());

    control.setUseMouseTracking($('#use-mouse-tracking').prop("checked"));

    control.setETConnectLocal($('#connect-local').prop("checked"));

    control.setETSentIP($('#et-sent-ip').val());

    control.setETSentPort($('#et-sent-port').val());

    control.setETReceiveIP($('#et-receive-ip').val());

    control.setETReceivePort($('#et-receive-port').val());
    
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