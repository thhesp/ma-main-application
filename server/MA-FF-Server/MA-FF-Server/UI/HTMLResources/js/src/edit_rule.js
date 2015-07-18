if (!control.creatingNewRule()) {
    console.log("initialize rule data...");

    
}

$('#save-button').click(function () {
    console.log('save rule setting...');

    

    control.saveRule();
});

$('#cancel-button').click(function () {
    console.log('cancel rule...');
    control.cancel();
});


$('#add-subrule').click(function () {
    //show window
    //var html = "<tr><td><select><option value='not'>NOT</option><option value='and'>AND</option><option value='or'>OR</option></select></td><td><select><option value='tag'>Tag</option><option value='id'>ID</option><option value='class'>Class</option></select></td><td><input class='value' type='text' placeholder='Wert'/></td></tr>";

    var template = $('#subrule-template tr')[0].outerHTML
    $("#rule-table").html(_.template(template));
});