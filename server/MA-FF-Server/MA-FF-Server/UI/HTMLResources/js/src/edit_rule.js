if (!control.creatingNewRule()) {
    console.log("initialize rule data...");

    $('#case-sensitive').prop("checked", control.getCaseSensitive());

    // root
    $('#main-table').attr("uid", control.getRootUID());
    $("#rule-root").val(control.getRootType());

    buildHTMLForChildNodes(control.getRootUID());

    $("#inner-rules .delete").off("click", "**");

    $('#inner-rules .delete').click(onDelete);

    $("#inner-rules .add-condition").off("click", "**");
    $("#inner-rules .add-value").off("click", "**");

    $("#inner-rules .add-condition").click(onAddCondition);

    $("#inner-rules .add-value").click(onAddValue);
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
    var template = _.template($('#condition-template tr')[0].outerHTML);
    $("#inner-rules").append(template({ uid: "" }));

    $("#inner-rules .delete").off("click", "**");

    $('#inner-rules .delete').click(onDelete);

    $("#inner-rules .add-condition").off("click", "**");
    $("#inner-rules .add-value").off("click", "**");

    $("#inner-rules .add-condition").click(onAddCondition);

    $("#inner-rules .add-value").click(onAddValue);
});

$('.main-table.add-value').click(function () {
    //add value
    var template = _.template($('#value-template tr')[0].outerHTML);
    $("#inner-rules").append(template({ uid: "" }));

    $("#inner-rules .delete").off("click", "**");

    $('#inner-rules .delete').click(onDelete);
});


function onAddCondition(event) {
    event.stopImmediatePropagation();
    var tbody = ($($(this).closest('td')).find('tbody')[0]);

    console.log("tbody: ", tbody);


    //add condition
    var template = _.template($('#condition-template tr')[0].outerHTML);
    $(tbody).append(template({ uid: "" }));

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

    var template = _.template($('#value-template tr')[0].outerHTML);
    $(tbody).append(template({ uid: "" }));

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

function buildHTMLForChildNodes(parentUid) {
    var childUIDS = control.getChildUIDs(parentUid);

    var conditionTemplate = _.template($('#condition-template tr')[0].outerHTML);

    var valueTemplate = _.template($('#value-template tr')[0].outerHTML);

    var parentTable = $("table[uid=" + parentUid + "] tbody");

    for (var i = 0; i < childUIDS.length; i++) {
        var childUID = childUIDS[i];
        
        var nodeType = control.getNodeType(childUID);

        console.log("NodeType: ", nodeType);

        if ("and" == nodeType || "or" == nodeType) {
            //add condition
            $(parentTable).append(conditionTemplate({ uid: childUID }));
            $("table[uid=" + childUID + "] thead select.condition-select").val(nodeType);

            buildHTMLForChildNodes(childUID);
        } else if ("not" == nodeType) {
            console.log("not found!");

            var valueChild = control.getChildUIDs(childUID);

            console.log("not children: ", valueChild);

            if (valueChild.length > 0) {
                var nodeType = control.getNodeType(valueChild[0]);

                if ("value" == nodeType) {
                    var nodeData = control.getValueNodeData(valueChild[0]);

                    console.log(nodeData);

                    $(parentTable).append(valueTemplate({ uid: valueChild[0] }));

                    $("tr[uid=" + valueChild[0] + "] .value-condition-select").val("not");

                    $("tr[uid=" + valueChild[0] + "] .value-type-select").val(nodeData[0]);

                    $("tr[uid=" + valueChild[0] + "] .value").val(nodeData[1]);
                }
            }

        } else if ("value" == nodeType) {
            var nodeData = control.getValueNodeData(childUID);

            console.log(nodeData);

            $(parentTable).append(valueTemplate({ uid: childUID }));

            $("tr[uid=" + childUID + "] .value-type-select").val(nodeData[0]);

            $("tr[uid=" + childUID + "] .value").val(nodeData[1]);
        }
    }

}