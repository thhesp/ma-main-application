$(document).ready(function () {
    
    var names = control.getExperimentNames();
    var paths = control.getExperimentPaths();

    var template = _.template($('script#experiment-template').html());

    for (var i = 0; i < names.length; i++) {
        $("#experiment-list").append(template({ name: names[i], path: paths[i] }));
    }

    $('#experiment-list .select-experiment-button').click(function () {
        console.log($(this));
        var path = $(this).attr('path');
        control.selectExperiment(path);
    });
});
