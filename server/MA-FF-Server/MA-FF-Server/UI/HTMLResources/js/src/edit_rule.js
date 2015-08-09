if (!control.creatingNewRule()) {
    console.log("initialize rule data...");

    $('#case-sensitive').prop("checked", control.getCaseSensitive());
}

$('#save-button').click(function () {
    console.log('save rule setting...');

    control.setCaseSensitive($('#case-sensitive').prop("checked"));

    var rootUid = control.createRuleRoot($("#rule-root").val());

    $('#main-table').attr("uid", rootUid);

    extractTreeData();

    control.saveRule();
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

function extractTreeData() {
    
    extractConditions();
    extractValues();
}

function extractConditions() {
    // add conditions
    var conditions = $('#inner-rules .inner-condition-table');

    for (var i = 0; i < conditions.length; i++) {

        var parentUID = $($(conditions[i]).closest('table')).attr('uid');

        var conditionType = $($(conditions[i]).find('select.condition-select')).val();

        var innerTable = $(conditions[i]).find('table');

        var uid = control.addConditionNodeToParent(parentUID, conditionType);

        $(innerTable).attr("uid", uid);

    }
}

function extractValues() {
    // add values
    var values = $('#inner-rules .value-cell');

    for (var i = 0; i < values.length; i++) {
        var parentUID = $($(values[i]).closest('table')).attr('uid');

        var valueCondition = $($(values[i]).find('select.value-condition-select')).val();

        var valueType = $($(values[i]).find('select.value-type-select')).val();

        var value = $($(values[i]).find('input.value')).val();

        if (valueCondition == "not") {
            //create not node and put it inbetween
            var nodeNodeUID = control.addConditionNodeToParent(parentUID, "not");

            control.addValueNodeToParent(nodeNodeUID, valueType, value);
        } else {
            control.addValueNodeToParent(parentUID, valueType, value);
        }
    }
}