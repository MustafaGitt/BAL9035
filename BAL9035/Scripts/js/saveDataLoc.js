/* Contians all the methods which deal with the LOCATION Lists Section Question 3 */
var EditId = 0;
// Validate location inputs
function validateLocation() {
    var errMsg = "";
    var errCount = 0;
    $("#reqLoc").empty();
    var valF4 = $("input[name=F4]").val();
    var valF5 = $("input[name=F5]").val();
    var valF6 = $("input[name=F6]").val();
    var valF7 = $("select[name=F7]").val();
    var valF8 = $("select[name=F8]").val();
    var valF9 = $("input[name=F9]").val();
    var wageFrom = $('#LocWageFrom').val();
    var wageTo = $('#LocWageTo').val();
    var secondryEntName = $('#SecondEntityName').val();
    var isCheckedF10 = $('#F10CheckBox').is(':checked');
    var isCheckedF3 = $('#F3CheckBox').is(':checked');

    if (!valF4) {
        errCount++;
        errMsg = " Section F Address 1 : Address 1 is required";
        $("#reqLoc").append('<li>' + errMsg + '</li>');
    }

    if (valF4.length > 0) {
        if (valF4.length < 5) {
            errCount++;
            errMsg = " Section F Address 1 : Length must be less or equal to 5 characters.";
            $("#reqLoc").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF5.length > 0) {
        if (valF5.length < 5) {
            errCount++;
            errMsg = " Section F Address 2 : Length must be less or equal to 5 characters.";
            $("#reqLoc").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF4.length > 60) {
        errCount++;
        errMsg = " Section F Address 1 : Length must be less or equal to 60 characters.";
        $("#reqLoc").append('<li>' + errMsg + '</li>');

    }
    //Len Check
    if (valF5.length > 0) {
        if (valF5.length > 60) {
            errCount++;
            errMsg = " Section F Address 2 : Length must be less or equal to 60 characters.";
            $("#reqLoc").append('<li>' + errMsg + '</li>');
        }
    }
    if (!valF6) {
        errCount++;
        errMsg = " Section F City : City is required";
        $("#reqLoc").append('<li>' + errMsg + '</li>');
    }
    //Len Check
    if (valF6.length > 0) {
        if (valF6.length > 35) {
            errCount++;
            errMsg = " Section F City : Length must be less or equal to 35 characters.";
            $("#reqLoc").append('<li>' + errMsg + '</li>');
        }
    }

    if (valF7 == '0') {
        errCount++;
        errMsg = " Section F State : State is required";
        $("#reqLoc").append('<li>' + errMsg + '</li>');
    }
    if (valF8 == '0') {
        errCount++;
        errMsg = " Section F County : County is required";
        $("#reqLoc").append('<li>' + errMsg + '</li>');
    }

    if (!valF9) {
        errCount++;
        errMsg = " Section F Postal Code : Postal Code is required";
        $("#reqLoc").append('<li>' + errMsg + '</li>');
    }
    if (valF9.length > 0) {
        if (valF9.length > 5) {
            errCount++;
            errMsg = " Section F Postal Code : Length must be less or equal to 5 characters.";
            $("#reqLoc").append('<li>' + errMsg + '</li>');
        }
    }

    if (!isCheckedF10) {
        if (!wageFrom) {
            errCount++;
            errMsg = " Section F Wage Range From Is Required";
            $("#reqLoc").append('<li>' + errMsg + '</li>');
        }
        //if (!wageTo) {
        //    errCount++;
        //    errMsg = " Section F Wage Range To Is Required";
        //    $("#reqLoc").append('<li>' + errMsg + '</li>');
        //}
    }
    if (!isCheckedF3) {
        if (!secondryEntName) {
            errCount++;
            errMsg = " Section F Secondary Entity Business Name Is Required";
            $("#reqLoc").append('<li>' + errMsg + '</li>');
        }
    }
    return errCount;
}
// Close button functionality
function btnLocOnClose() {
    $("input[name=F4]").val("");
    $("input[name=F5]").val("");
    $("input[name=F6]").val("");
    $("select[name=F7]").val("0");
    $("select[name=F8]").val("0");
    $("input[name=F9]").val("");
    $("#LocWageFrom").val("");
    $("#LocWageTo").val("");
    $("#SecondEntityName").val("");
}
// Add or Update the Location Table Values
function UpdateLocationTable() {
    $("#reqLoc").empty();
    var valF4 = $("input[name=F4]").val();
    var valF5 = $("input[name=F5]").val();
    var valF6 = $("input[name=F6]").val();
    var valF7 = $("select[name=F7] option:selected").text()
    var valF8 = $("select[name=F8]").val();
    var valF9 = $("input[name=F9]").val();


    if (valF4 != "" || valF5 != "" || valF6 != "" || valF7 != "-- SELECT --" || valF8 != "0" || valF9 != "") {
        // check validations
        var errCount = validateLocation();
        if (errCount == 0) {
            if ($("#btnLocSav1").text() == "Update Location") {
                var jsonLastTD = $('#locTable tr').eq(EditId).find('#jsonValue').val()
                productUpdateInTable(EditId);
                $('#locTable tr').eq(EditId).find('#jsonValue').val(jsonLastTD);
            }
            else {
                // Add the new row values in Location table
                var lastTr = $('#locTable').find("tr").last().attr("id").split('-');
                EditId = lastTr[1];
                var jsonLastTD = $('#locTable').find("tr").last().find('#jsonValue').val();
                productUpdateInTable(EditId);
                $('#locTable tr').eq(EditId).find('#jsonValue').val(jsonLastTD);
            }
            btnLocOnClose();
            $('#LocPopUp').modal('hide');
        }
    }
    else {
        $("#reqLoc").append('<li>' + "Section F Location : At Lease one value should be filled to add Location." + '</li>');
    }
}
// Delete the row of location table
function productDelete(ctl) {
    $(ctl).parents("tr").remove();
}
// Display the Location table row in a popup
function productDisplay(ctl) {
    var row = $(ctl).parents("tr");
    var cols = row.children("td");


    EditId=$("#locTable tr").index(ctl.closest('tr'));
    //EditId = row.attr('id').split('-')[1];
    $("input[name=F4]").val(cols[0].innerText);
    $("input[name=F5]").val(cols[1].innerText);
    $("input[name=F6]").val(cols[2].innerText);
    $("select[name=F7]").removeAttr("selected");
    if (cols[3].innerText == "") {
        $("select[name=F7]").val("0");
    }
    else {
        $("#F7 option").filter(function (index) { return $(this).text() === cols[3].innerText; }).prop("selected", true);
        // JIRA ISSUE BUG-BAL-155
        //$("select[name=F7] option:contains(" + cols[3].innerText + ")").prop("selected", true);
    }
    //ddlTextSelection("F7", cols[3].innerText);
    FetchCounties(cols[4].innerText, cols[7].children[0].value);
    //$("select[name=F8]").val();
    $("input[name=F9]").val(cols[5].innerText);

    $("input[name=LocWageFrom]").val(cols[8].innerText);
    $("input[name=LocWageTo]").val(cols[9].innerText);
    $("input[name=SecondEntityName]").val($(row).find('#tblSecondEntityName').text());
    $('#LocPopUp').modal();
    // Change Update Button Text
    $("#btnLocSav1").text("Update Location");
}
function AddLocationTable(jsonObject) {
    var valF4 = $("input[name=F4]").val();
    var valF5 = $("input[name=F5]").val();
    var valF6 = $("input[name=F6]").val();
    var valF7 = $("select[name=F7] option:selected").text();
    var valF8 = $("select[name=F8]").val();
    var valF9 = $("input[name=F9]").val();
    var valLocWageFrom = $("input[name=LocWageFrom]").val();
    var valLocWageTo = $("input[name=LocWageTo]").val();
    var valLocBusnsName = $("input[name=SecondEntityName]").val();

    var highlight = "<td>";
    if (valF8 == "0") {
        valF8 = "";
        highlight = "<td  class='highlight'>";
    }
    if ($("select[name=F7]").val() == "0") {
        valF7 = "";
    }
    if ($("#locTable tbody").length == 0) {
        $("#locTable").append("<tbody></tbody>");
    }
    var tblID = 0;
    if ($('#locTable tr').length > 1) {
        //var lastTD = $('#locTable tr:last').attr('id').split('-');
        //tblID = lastTD[1];


        // -1 to remove headers name row from count
        tblID = $('#locTable tr').length - 1;
    }
    else {
        tblID = 0;
    }
    if (jsonObject != null) {
        var inc = parseInt(tblID) + 1;
        $('#locTable > tbody:last-child').append('<tr id="Row-' + inc + '" style="display:none;"><td>' + valF4 + '</td><td>' + valF5 + '</td><td>' + valF6 + '</td><td>' + valF7 + '</td>' + highlight + valF8 + '</td><td>' + valF9 +
            '</td><td>' +
            '<button type="button" onclick="productDisplay(this);" class="btn btn-default btn-tbl-custom"><span class="glyphicon glyphicon-edit" /></button> | ' +
            '<button type="button" onclick="productDelete(this);" class="btn btn-default btn-tbl-custom"><span class="glyphicon glyphicon-remove" /></button> | ' +
            '<button type="button" onclick="addSection(this);" class="btn btn-default btn-tbl-custom redAddSection"><span class="glyphicon glyphicon-plus" /></button>' +
            '</td><td style="display:none;"><input type="text" name="jsonValue" id="jsonValue" value=""></td><td style="display:none;" id="wageFrom">' + valLocWageFrom + '</td><td style="display:none;" id="wageTo">' + valLocWageTo + '</td><td style="display:none;" id="tblSecondEntityName">' + valLocBusnsName +'</td></tr>');
        $('#locTable tr').eq(inc).find('#jsonValue').val(jsonObject);
    }

}
function productUpdateInTable(id) {
    // Find Product in <table>
    //var row = $("#Row-" + id + "").closest("tr");
    var row = $('#locTable').find('tr').eq(id);;
    // Add changed product to table
    $(row).after(productBuildTableRow(id));
    // Remove original product
    $(row).remove();
}
// Creates a new Table Row
function productBuildTableRow(id) {
    debugger;
    var valF4 = $("input[name=F4]").val();
    var valF5 = $("input[name=F5]").val();
    var valF6 = $("input[name=F6]").val();
    var valF7 = $("select[name=F7] option:selected").text();
    var valF8 = $("select[name=F8]").val();
    var valF9 = $("input[name=F9]").val();
    var valLocWageFrom = $("input[name=LocWageFrom]").val();
    var valLocWageTo = $("input[name=LocWageTo]").val();
    var valLocBusnsName = $("input[name=SecondEntityName]").val();

    var highlight = '<td style="width: 100px; max-width: 100px; text-overflow: ellipsis; overflow: hidden; white-space: nowrap; ">';
    if (valF8 == "0") {
        valF8 = "";
        highlight = '<td  class="highlight" style="width: 100px; max-width: 100px; text-overflow: ellipsis; overflow: hidden; white-space: nowrap;">';
    }
    if ($("select[name=F7]").val() == "0") {
        valF7 = "";
    }
    //handle color class
    var resultString = $('#locTable tr').eq(id).find('#jsonValue').val();
    var classResult = 'redAddSection';
    if (resultString) {
        var result = $.parseJSON(resultString);
        if (result.F13) {
            classResult = 'greenAddSection';
        }
        else if (result.F14a && result.F14b) {
            if (result.F14a.toLowerCase() != "other") {
                classResult = 'greenAddSection';
            }
            else {
                if (result.F14c && result.F14d) {
                    classResult = 'greenAddSection';
                }
            }
        }
    }
    else {
        classResult = 'redAddSection';
    }
    var ret = '<tr id="Row-' + id + '"><td style="width: 100px; max-width: 100px; text-overflow: ellipsis; overflow: hidden; white-space: nowrap; ">' + valF4 + '</td><td style="width: 100px; max-width: 100px; text-overflow: ellipsis; overflow: hidden; white-space: nowrap; ">' + valF5 + '</td><td style="width:100px;max-width:100px;text-overflow:ellipsis;overflow:hidden;white-space:nowrap;">' + valF6 + '</td><td style="width:100px;max-width:100px;text-overflow:ellipsis;overflow:hidden;white-space:nowrap;">' + valF7 + '</td>' + highlight + valF8 + '</td><td style="width: 100px; max-width: 100px; text-overflow: ellipsis; overflow: hidden; white-space: nowrap; ">' + valF9 +
        '</td><td style="width:20%;">' +
        '<button type="button" onclick="productDisplay(this);" data-id=' + id + ' class="btn btn-default btn-tbl-custom"><span class="glyphicon glyphicon-edit" /></button> | ' +
        '<button type="button" onclick="productDelete(this);" data-id=' + id + ' class="btn btn-default btn-tbl-custom"><span class="glyphicon glyphicon-remove" /></button> | ' +
        '<button type="button" onclick="addSection(this);" data-id=' + id + ' class="btn btn-default btn-tbl-custom ' + classResult + '"><span class="glyphicon glyphicon-plus" /></button>' +
        '</td><td style="display:none;"><input type="text" name="jsonValue" id="jsonValue" value=""></td><td style="display:none;" id="wageFrom">' + valLocWageFrom + '</td><td style="display:none;" id="wageTo">' + valLocWageTo + '</td><td style="display:none;" id="tblSecondEntityName">' + valLocBusnsName +'</td></tr>';
    return ret;
}
//create a new Section (F13,F14)
function addSection(ctl) {
    var $tr = ctl.closest('tr');
    var tableRowNo = $("#locTable tr").index($tr);
    if (Number.isInteger(tableRowNo)) {
        if (!$("#btnCopyValues").hasClass("header")) {
            // below line send the table row number to modal so modal data can be stored in that row 
            $("#sectionFPopUp #parentTableRowNo").val(tableRowNo);
        }
        if ($('#locTable tr').eq(tableRowNo).find('#jsonValue').val() && $('#locTable tr').eq(tableRowNo).find('#jsonValue').val() != "") {
            //populate modal with values
            var result = JSON.parse($('#locTable tr').eq(tableRowNo).find('#jsonValue').val());
            if (result) {
                //result.F14 == true ? $('#sectionFPopUp input[name=FPopUp][value=F14]').prop('checked', true) : $('#sectionFPopUp input[name=FPopUp][value=F13]').prop('checked', true);
                if (result.F14 == true) {
                    $('#sectionFPopUp input[name=FPopUp][value=F14]').prop('checked', true);
                    $('#sectionFPopUp input[name=F14a][value=' + result.F14a + ']').prop('checked', true);
                    $('#sectionFPopUp #F14b').val(result.F14b);
                    $('#sectionFPopUp #F14c').val(result.F14c);
                    $('#sectionFPopUp #F14d').val(result.F14d);
                    $('#divF14').removeClass('inactive');
                    if (!$('#divF13').hasClass('inactive')) {
                        $('#divF13').addClass('inactive');
                    }
                }
                else if (result.F13 == true) {
                    $('#sectionFPopUp input[name=FPopUp][value=F13]').prop('checked', true);
                    $('#sectionFPopUp input[name=F13a][value=' + result.F13a + ']').prop('checked', true);
                    if (result.CollectionType != null) {
                        $('#sectionFPopUp input[name=CType][value=' + boolToString(result.CollectionType) + ']').prop('checked', true);
                    }
                    if (result.AreaBasedOn != null) {
                        $('#sectionFPopUp input[name=AB][value=' + boolToString(result.AreaBasedOn) + ']').prop('checked', true);
                    }
                    if (result.RnDPosition != null) {
                        $('#sectionFPopUp input[name=RDP][value=' + boolToString(result.RnDPosition) + ']').prop('checked', true);
                    }
                    if (result.HCPosition != null) {
                        $('#sectionFPopUp input[name=HCP][value=' + boolToString(result.HCPosition) + ']').prop('checked', true);
                    }
                    if (result.Per != null) {
                        $("#FModalPer").val(result.Per);
                    }
                    $('#divF13').removeClass('inactive');
                    if (!$('#divF14').hasClass('inactive')) {
                        $('#divF14').addClass('inactive');
                    }
                }
            }

            if (tableRowNo == 1) {
                if (result.F14a != null || result.StateOrTerritory != null) {
                    $('#btnCopyValues').removeClass('hidden');
                    $('#btnCopyValues').removeClass('header');
                }
            }
            else {
                $('#btnCopyValues').addClass('hidden');
            }
        }

    }

    //open modal
    $('#sectionFPopUp').modal();
}

function boolToString(value) {
    if (value == true) {
        return 'yes';
    }
    else {
        return 'no';
    }
}

