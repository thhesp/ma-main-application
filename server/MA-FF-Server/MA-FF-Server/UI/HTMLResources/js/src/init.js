$('#experiment-name').text(experimentObj.getName());


$('#websocket-server-status').addClass(experimentObj.getWSConnectionStatus());

$('#eyetracker-status').addClass(experimentObj.getTrackingConnectionStatus());