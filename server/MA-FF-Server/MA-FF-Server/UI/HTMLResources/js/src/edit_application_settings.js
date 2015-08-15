$('#websocket-port').val(control.getWSPort());

$('#use-mouse-tracking').prop("checked", control.getUseMouseTracking());

$('#connect-local').prop("checked", control.getETConnectLocal());

$('#et-sent-ip').val(control.getETSentIP());

$('#et-sent-port').val(control.getETSentPort());

$('#et-receive-ip').val(control.getETReceiveIP());

$('#et-receive-port').val(control.getETReceivePort());

$('#save-button').click(function () {
    console.log('save settings...');

    control.setWSPort($('#websocket-port').val());

    control.setUseMouseTracking($('#use-mouse-tracking').prop("checked"));

    control.setETConnectLocal($('#connect-local').prop("checked"));

    control.setETSentIP($('#et-sent-ip').val());

    control.setETSentPort($('#et-sent-port').val());

    control.setETReceiveIP($('#et-receive-ip').val());

    control.setETReceivePort($('#et-receive-port').val());
    
    control.saveSettings();
});

$('#cancel-button').click(function () {
    console.log('cancel form...');
    control.cancel();
});