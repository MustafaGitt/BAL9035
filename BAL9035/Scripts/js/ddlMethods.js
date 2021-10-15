
// Creating County DropDown Options with selected option 
function FetchCounties(selectedvalue, jsonValue) {
    $("#reqLoc").empty();
    if (selectedvalue == null || selectedvalue == "") {
        selectedvalue = "0";
    }
    $("#F8").empty().append('<option value="0">-- SELECT --</option>');
    var locStateVal = $("select[name=F7] option:selected").text();
    let counties = [];
    if (jsonValue == null) {
        // Get Last TD
        var LastTDjson = $('#locTable').find("tr").last().find('#jsonValue').val();
        // For Add Location Button (new row)
        var SectionFModal = $.parseJSON(LastTDjson);
        counties = GetCounties(SectionFModal, locStateVal);
    }
    else {
        var valFPopUp = JSON.parse(jsonValue);
        counties = GetCounties(valFPopUp, locStateVal);
    }
    // Appending options into DDL
    if (counties.length > 0) {
        $.each(counties, function (i) {
            $("#F8").append("<option value='" + $.trim(counties[i].Value) + "'>" + $.trim(counties[i].Text) + "</option>");
            $("#F8").val(selectedvalue);
        });
    }
}

function GetCounties(parsedObj, stateVal) {
    let countiesList = [];
    //fetching counties values on the basis of F13,F14
    if (parsedObj.F13 == true) {
        //fetching counties values which are in the following state
        countiesList = DDLCounty.Common.ddl_childValue.filter(function (county) {
            return county.PValue == stateVal;
        });
    }
    else if (parsedObj.F14 == true) {
        //fetching counties values which are in the following state
        countiesList = DDLSCounty.Common.ddl_childValue.filter(function (county) {
            return county.PValue == stateVal;
        });
    }
    else {
        $("#reqLoc").append('<li>' + " Section F Question F7 County : Please Select at least one F13 or F14 First to populate the County List." + '</li>');
    }
    return countiesList;
}

function highlightHRadios() {
    $("input[name=H3]").addClass("customRadio");
    $("input[name=H5]").addClass("customRadio");
    $("input[name=H6]").addClass("customRadio");
    $("input[name=H3]").prop('checked', false);
    $("input[name=H5]").prop('checked', false);
    $("input[name=H6]").prop('checked', false);
}
// HTML FIELD H1 Dependencies
function RadioH1() {
    var valH1 = $("input[name=H1]:checked").val();
    var valH2 = $("input[name=H2]:checked").val();
    if (valH1 != null && valH2 != null) {
        if (valH1 == "yes") {
            highlightHRadios();
        }
        else if (valH2 == "yes") {
            highlightHRadios();
        }
        else {
            $("input[name=H3]").removeClass("customRadio");
            $("input[name=H5]").removeClass("customRadio");
            $("input[name=H6]").removeClass("customRadio");
        }
    }
    else if (valH1 != null && valH2 == null) {
        if (valH1 == "yes") {
            highlightHRadios();
        }
        else {
            $("input[name=H3]").removeClass("customRadio");
            $("input[name=H5]").removeClass("customRadio");
            $("input[name=H6]").removeClass("customRadio");
        }
    }
    else if (valH1 == null && valH2 != null) {
        if (valH2 == "yes") {
            highlightHRadios();
        }
        else {
            $("input[name=H3]").removeClass("customRadio");
            $("input[name=H5]").removeClass("customRadio");
            $("input[name=H6]").removeClass("customRadio");
        }
    }
    else {
        $("input[name=H3]").removeClass("customRadio");
        $("input[name=H5]").removeClass("customRadio");
        $("input[name=H6]").removeClass("customRadio");
    }
}
// Enable and Disable Section F13, F14
function EnableDisableSectionF() {
    let valF13 = $("input[name=F13]:checked").val();
    let valF14 = $("input[name=F14]:checked").val();
    if (valF13 != null) {
        if (valF13 == "yes") {
            $("#divF14 :input").prop("disabled", true);
        }
    }
    if (valF14 != null) {
        if (valF14 == "yes") {
            $("input[name=F13-A]").prop('disabled', true);
        }
    }
}
// Enable the fields of Section F13
function EnablebtnF13() {
    $("#divF14 :input").prop("disabled", true);
    $("input[name=F14-A]").prop('checked', false);
    $("input[name=F14]").prop('checked', false);
    $("select[name=F14-B]").val("0");
    $("input[name=F14c]").val("");
    $("input[name=F14d]").val("");
    $("input[name=F13-A]").prop('disabled', false);
    $("input[name=F13]").prop('checked', true);
    $("input[name=F13]").val("yes");
    $("#sectionFPopUp").modal();
    ResetCountiestbl("F13");
}
// Select DDL Value by Text
function ddlTextSelection(id, text) {
    $("#" + id + " option").filter(function (index) { return $(this).text() === text; }).prop("selected", true);
}
// Reset Location GRID Table
function ResetCountiestbl(FieldName) {

    if (previousValue != FieldName) {
        var table = $("#locTable tbody");
        table.find('tr').each(function () {
            var $tds = $(this).find('td'),
                County = $tds.eq(4).text("");
        });
        previousValue = FieldName;
    }
}

function F10CheckBoxFunc() {
    var isChecked = $('#F10CheckBox').is(':checked')
    if (!isChecked) {
        //update multiplePurposeModal Header and body Text to show in modal
        $('#multiModalHeaderTitle').html($.parseHTML('<span class="glyphicon glyphicon-alert"></span>  Location Specific Wage Range'));
        $('#multiModalBody').html($.parseHTML('Click on the Edit icon <span class="glyphicon glyphicon-edit"></span> for each location to provide the location-specific wage range.')).contents();
        $('#multiplePurposeModal').modal('show');
        $("#F10from").prop("disabled", true); 
        $("#F10fromP").prop("disabled", true); 

        $("#F10To").prop("disabled", true);
        $("#F10ToP").prop("disabled", true); 
    }
    else {
        $("#F10from").prop("disabled", false);
        $("#F10fromP").prop("disabled", false); 
        $("#F10To").prop("disabled", false);
        $("#F10ToP").prop("disabled", false); 
    }

    UpdateWagesInLocTbl(isChecked);
}

function UpdateWagesInLocTbl(isChecked) {
    var fromWageResult = $('#F10fromP').val() ? $('#F10from').val() + "." + $('#F10fromP').val() : $('#F10from').val();
    var toWageResult = $('#F10ToP').val() ? $('#F10To').val() + "." + $('#F10ToP').val() : $('#F10To').val();
    $('#locTable > tbody  > tr').each(function (index, tr) {
        debugger;
        if (isChecked) {
            $(tr).find('#wageFrom')[0].innerText = fromWageResult;
            $(tr).find('#wageTo')[0].innerText = toWageResult;
        }
        else {
            $(tr).find('#wageFrom')[0].innerText = "";
            $(tr).find('#wageTo')[0].innerText = "";
        }
    });
}

function F3CheckBoxFunc() {
    var isChecked = $('#F3CheckBox').is(':checked');
    if (!isChecked) {
        //update multiplePurposeModal Header and body Text to show in modal
        $('#multiModalHeaderTitle').html($.parseHTML('<span class="glyphicon glyphicon-alert"></span> Location Specific Secondary Business Entity Name'));
        $('#multiModalBody').html($.parseHTML('Click on the Edit icon <span class="glyphicon glyphicon-edit"></span> for each location to provide the location-specific Secondary Entity Business Name'));
        $('#multiplePurposeModal').modal('show');
        $("#F3").prop("disabled", true);
    }
    else {
        $("#F3").prop("disabled", false);
    }
    UpdateBusinessEntityInLocTbl(isChecked);
}

function UpdateBusinessEntityInLocTbl(isChecked) {
    var entityBusinessNameResult = $('#F3').val();
    $('#locTable > tbody  > tr').each(function (index, tr) {
        if (isChecked) {
            $(tr).find('#tblSecondEntityName')[0].innerText = entityBusinessNameResult;
        }
        else {
            $(tr).find('#tblSecondEntityName')[0].innerText = "";
        }
    });
}

function tblRowsUpdateF3() {
    var isChecked = $('#F3CheckBox').is(':checked');
    var F2Radio = $("input[name=F2]:checked").val();
    if (F2Radio == 'yes' && isChecked) {
        $('#locTable > tbody  > tr').each(function (index, tr) {
            $(tr).find('#tblSecondEntityName')[0].innerText = $('#F3').val();
        });
    }
}

