if (!control.creatingNewRule()) {
    console.log("initialize rule data...");

    $('#case-sensitive').prop("checked", control.getCaseSensitive());
}

$('#save-button').click(function () {
    console.log('save rule setting...');

    control.setCaseSensitive($('#case-sensitive').prop("checked"));

    //control.saveRule();
});

$('#cancel-button').click(function () {
    console.log('cancel rule...');
    control.cancel();
});

$('.delete').click(onDelete);

$('.main-table.add-condition').click(function () {
    //add condition
    var template = $('#condition-template tr')[0].outerHTML;
    $("#inner-rules").append(_.template(template));

    $("#inner-rules .delete").off("click", "**");

    $('#inner-rules .delete').click(onDelete);

    $("#inner-rules .add-condition").off("click", "**");
    $("#inner-rules .add-value").off("click", "**");

    $("#inner-rules .add-condition").click(onAddCondition);

    $("#inner-rules .add-value").click(onAddValue);
});

$('.main-table.add-value').click(function () {
    //add value
    var template = $('#value-template tr')[0].outerHTML;
    $("#inner-rules").append(_.template(template));

    $("#inner-rules .delete").off("click", "**");

    $('#inner-rules .delete').click(onDelete);
});


function onAddCondition(event) {
    event.stopImmediatePropagation();
    var tbody = ($($(this).closest('td')).find('tbody')[0]);

    console.log("tbody: ", tbody);


    //add condition
    var template = $('#condition-template tr')[0].outerHTML;
    $(tbody).append(_.template(template));

    $("#inner-rules .delete").off("click", "**");

    $('#inner-rules .delete').click(onDelete);

    $("#inner-rules .add-condition").off("click", "**");
    $("#inner-rules .add-value").off("click", "**");

    $("#inner-rules .add-condition").click(onAddCondition);

    $("#inner-rules .add-value").click(onAddValue);
}


function onAddValue(event) {
    event.stopImmediatePropagation();
    var tbody = ($($(this).closest('td')).find('tbody')[0]);

    console.log("tbody: ", tbody);

    var template = $('#value-template tr')[0].outerHTML;
    $(tbody).append(_.template(template));

    $("#inner-rules .delete").off("click", "**");

    $('#inner-rules .delete').click(onDelete);
}

function onDelete() {
    $(this).closest('tr').remove();
}