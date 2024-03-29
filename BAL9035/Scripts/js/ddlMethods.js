﻿// Creating Area DropDown Options with selected option 
function FetchAreas(selectedvalue) {
    if (selectedvalue == null || selectedvalue == "") {
        selectedvalue = "0";
    }
    $("#SectionFArea").empty().append('<option value="0">-- SELECT --</option>');
    var stateVal = $("select[name=FModalState] option:selected").text();
    var chkStateCode = stateVal.charCodeAt(0);
    let areas = []
    //fetching counties values which are in the following state
    areas = DDLCounty.Common.ddl_childValue.filter(function (area) {
        return area.PValue == stateVal;
    });
    // Appending options into DDL
    if (areas.length > 0) {
        $.each(areas, function (i) {
            $("#SectionFArea").append("<option value='" + $.trim(areas[i].Value) + "'>" + $.trim(areas[i].Text) + "</option>");
            $("#SectionFArea").val(selectedvalue);
        });
    }
}
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