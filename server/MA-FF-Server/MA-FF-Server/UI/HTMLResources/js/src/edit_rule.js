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


$('#add-tag-subrule').click(function () {
    //show window
    var template = $('#subrule-template tr')[0].outerHTML;
    $("#tag-rule-table").append(_.template(template));

    disableOptions($('#tag-rule-table'));

    if ($("#tag-rule-table tr").length == 1) {
        $("#tag-rule-table tr:first select").val('none');
    }
});

$('#add-id-subrule').click(function () {
    //show window
    var template = $('#subrule-template tr')[0].outerHTML;
    $("#id-rule-table").append(_.template(template));

    disableOptions($('#id-rule-table'));

    if ($("#id-rule-table tr").length == 1) {
        $("#id-rule-table tr:first select").val('none');
    }
});

$('#add-class-subrule').click(function () {
    //show window
    var template = $('#subrule-template tr')[0].outerHTML;
    $("#class-rule-table").append(_.template(template));

    disableOptions($('#class-rule-table'));

    if ($("#class-rule-table tr").length == 1) {
        $("#class-rule-table tr:first select").val('none');
    }
});

function disableOptions(el) {
    var $el = $(el);

    $el.find('option').removeAttr('disabled');

    var firstSelect = $($el.children('tr:first')).find('select');

    firstSelect.find('option.disable-first').attr('disabled', 'true');

    var laterSelects = $($el.children('tr:not(:first)')).find('select');

    laterSelects.find('option.disable-nth').attr('disabled', 'true');
};