function initialize() {

    $('#experiment-name').text(experimentObj.getName());


    $('#websocket-server-status').addClass(experimentObj.getWSConnectionStatus());

    $('#eyetracker-status').addClass(experimentObj.getTrackingConnectionStatus());

    $('#websocket-connection-count').text(experimentObj.getConnectionCount());
};

initialize();