if (!control.creatingNewRule()) {
    console.log("initialize rule data...");

    $('#case-sensitive').prop("checked", control.getCaseSensitive());
}

$('#save-button').click(function () {
    console.log('save rule setting...');

    control.setCaseSensitive($('#case-sensitive').prop("checked"));

    $('#tag-rule-table tr').each(function(){
        control.addTagConstraint($(this).find('select').val(), $(this).find('input.value').val());
    });

    $('#id-rule-table tr').each(function () {
        control.addIDConstraint($(this).find('select').val(), $(this).find('input.value').val());
    });

    $('#class-rule-table tr').each(function () {
        control.addClassConstraint($(this).find('select').val(), $(this).find('input.value').val());
    });

    //control.saveRule();
});

$('#cancel-button').click(function () {
    console.log('cancel rule...');
    control.cancel();
});


$('#add-tag-subrule').click(function () {
    //show window
    var template = $('#subrule-template tr')[0].outerHTML;
    $("#tag-rule-table").append(_.template(template));

    disableOptions($('#tag-rule-table'), true);

    if ($("#tag-rule-table tr").length == 1) {
        $("#tag-rule-table tr:first select").val('none');
    } else {
        $("#tag-rule-table tr:first select").val('or');
    }
});

$('#add-id-subrule').click(function () {
    //show window
    var template = $('#subrule-template tr')[0].outerHTML;
    $("#id-rule-table").append(_.template(template));

    disableOptions($('#id-rule-table'), true);

    if ($("#id-rule-table tr").length == 1) {
        $("#id-rule-table tr:first select").val('none');
    } else {
        $("#id-rule-table tr:first select").val('or');
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

function disableOptions(el, singular) {
    var $el = $(el);

    $el.find('option').removeAttr('disabled');

    var firstSelect = $($el.children('tr:first')).find('select');

    firstSelect.find('option.disable-first').attr('disabled', 'true');

    var laterSelects = $($el.children('tr:not(:first)')).find('select');

    laterSelects.find('option.disable-nth').attr('disabled', 'true');

    if (singular != undefined && singular == true) {
        $el.find('option.disable-singular').attr('disabled', 'true');
    }
};