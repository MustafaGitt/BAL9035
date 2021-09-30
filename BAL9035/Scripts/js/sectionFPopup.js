/*
 * All Methods related to Section F Popup
 * Validate, Populate and on Radio Change Fields Methods
 * Create Json Object Method, Reset Value methods
 */

// Populate Section F Modal Popup Input fields
function SectionFModalPopUp() {
    var chkCount = validateSectionFModal();
    if (chkCount == 0) {
        var jsonSaveValue = createSectionFObj();
        if ($('#parentTableRowNo').val().length > 0) {
            var tablRowId = parseInt($('#parentTableRowNo').val());
            $('#locTable tr').eq(tablRowId).find('#jsonValue').val(jsonSaveValue);
            // empty the td with Area Value and highlight td
            var getTDs = $('#locTable tr').eq(tablRowId);
            var countyTD = getTDs[0].children[4];
            countyTD.innerHTML = "";
            $(countyTD).addClass('highlight');
            $('#parentTableRowNo').val('');
        }
        else {
            AddLocationTable(jsonSaveValue);
            $('#LocPopUp').modal();
        }
        if ($('#locTable tr').eq(tablRowId).find('.redAddSection').length > 0) {
            $('#locTable tr').eq(tablRowId).find('.redAddSection').removeClass('redAddSection').addClass('greenAddSection');
        }
        $('#locTable tr').eq(tablRowId).find('.redAddSection').removeClass('redAddSection');
        $('#sectionFPopUp').modal('hide');
    }
}
// F13,F14 RadioChange
function onRadioChange() {
    $("#reqModal").empty();
    var value = $("input[name='FPopUp']:checked").val();
    if (value == 'F13') {
        $('#divF13').removeClass('inactive');
        $('#divF14').find('select').val('0');
        $('#divF14').find('input:text').val('');
        $('#divF14').find('input:radio').prop('checked', false);
        if (!$('#divF14').hasClass('inactive')) {
            $('#divF14').addClass('inactive');
        }
    }
    if (value == 'F14') {
        $('#divF14').removeClass('inactive');
        $('#divF13').find('select').val('0');
        $('#divF13').find('input:radio').prop('checked', false);
        if (!$('#divF13').hasClass('inactive')) {
            $('#divF13').addClass('inactive');
        }
    }
}
// Reset fields on modal hide
$('#sectionFPopUp').on('hidden.bs.modal', function () {
    $("#sectionFPopUp form")[0].reset();
    $('#divF13').addClass('inactive');
    $('#divF14').addClass('inactive');
});
// Create Section F Fields JSON Object and return as string
function createSectionFObj() {
    var checkSecFRadio = $("input[name='FPopUp']:checked").val();
    //saving SectionFModal data into table row hidden field
    var SectionFModal = {};
    var strVal = "";
    if (checkSecFRadio == "F13") {
        SectionFModal.FPopUp = "F13";
        SectionFModal.F13 = true;
        SectionFModal.F14 = false;
        SectionFModal.F13a = UndefinedToNull($("input[name=F13a]:checked").val());
        SectionFModal.CollectionType = convertToBool($("input[name=CType]:checked").val());
        SectionFModal.AreaBasedOn = convertToBool($("input[name=AB]:checked").val());
        SectionFModal.RnDPosition = convertToBool($("input[name=RDP]:checked").val());
        SectionFModal.HCPosition = convertToBool($("input[name=HCP]:checked").val());
        SectionFModal.Per = $("#FModalPer").val();
    }
    if (checkSecFRadio == "F14") {
        SectionFModal.FPopUp = "F14";
        SectionFModal.F13 = false;
        SectionFModal.F14 = true;
        SectionFModal.F14a = $("input[name=F14a]:checked").val();
        SectionFModal.F14b = $("select[name=F14b]").val();
        SectionFModal.F14c = $("input[name=F14c]").val();
        SectionFModal.F14d = $("input[name=F14d]").val();
    }
    strVal = JSON.stringify(SectionFModal);
    return strVal;
}
// Validate Section F Modal Fields
function validateSectionFModal() {
    var errMsg = "";
    var secFerrCount = 0;
    //var number = ID.split('_')[1];
    $("#reqModal").empty();
    var checkRadio = $("input[name='FPopUp']:checked").val();
    ////F13 Inputs
    var F13a = $("input[name=F13a]:checked").val();
    var valCType = $("input[name=CType]:checked").val();
    var valAB = $("input[name=AB]:checked").val();
    var valRDP = $("input[name=RDP]:checked").val();
    var valHCP = $("input[name=HCP]:checked").val();
    var valPer = $("select[name='FModalPer']").val();
    // F14 Inputs
    var SType = $("input[name=F14a]:checked").val();
    var SYear = $("select[name=F14b]").val();
    var F14c = $("input[name=F14c]").val();
    var F14d = $("input[name=F14d]").val();

    if (checkRadio == "F13") {
        if (F13a == null) {
            secFerrCount++;
            errMsg = " Section F Question F13-A is Required.  ";
            $("#reqModal").append('<li>' + errMsg + '</li>');
        }
        if (valCType == null) {
            secFerrCount++;
            errMsg = " Section F 'Collection Type' is Required.  ";
            $("#reqModal").append('<li>' + errMsg + '</li>');

        }
        if (valAB == null) {
            secFerrCount++;
            errMsg = " Section F 'Area Based On' is Required.  ";
            $("#reqModal").append('<li>' + errMsg + '</li>');

        }
        if (valRDP == null) {
            secFerrCount++;
            errMsg = " Section F 'Research and Development Position' is Required.  ";
            $("#reqModal").append('<li>' + errMsg + '</li>');

        }
        if (valHCP == null) {
            secFerrCount++;
            errMsg = " Section F 'Highly Compensated Position' is Required.  ";
            $("#reqModal").append('<li>' + errMsg + '</li>');

        }
        if (valPer == "0") {
            secFerrCount++;
            errMsg = " Section F  'Per' is Required.  ";
            $("#reqModal").append('<li>' + errMsg + '</li>');

        }
    }
    else if (checkRadio == "F14") {
        if (SType == null) {
            secFerrCount++;
            errMsg = "  Section F Question : F14-A 'Source Type' is Required.  ";
            $("#reqModal").append('<li>' + errMsg + '</li>');
        }
        if (SType != null) {
            if (SYear == "0") {
                secFerrCount++;
                errMsg = " Section F Question HF4-B : 'Source Year' is Required.  ";
                $("#reqModal").append('<li>' + errMsg + '</li>');
            }
            if (SType == "Other") {
                if (F14c.length <= 0) {
                    secFerrCount++;
                    errMsg = "  Section F Question : F14-C is Required.  ";
                    $("#reqModal").append('<li>' + errMsg + '</li>');
                }
                if (F14d.length <= 0) {
                    secFerrCount++;
                    errMsg = "  Section F Question : F14-D is Required.  ";
                    $("#reqModal").append('<li>' + errMsg + '</li>');
                }
            }
            if (F14c.length > 0) {
                if (F14c.length > 60) {
                    secFerrCount++;
                    errMsg = " Section F Question 14 C : Length must be less or equal to 60 characters.";
                    $("#reqModal").append('<li>' + errMsg + '</li>');
                }
            }
            if (F14d.length > 0) {
                if (F14d.length > 60) {
                    secFerrCount++;
                    errMsg = " Section F Question 14 D : Length must be less or equal to 60 characters.";
                    $("#reqModal").append('<li>' + errMsg + '</li>');
                }
            }
        }
    }
    else {
        secFerrCount++;
        errMsg = "F.13 or F.14 must be selected.";
        $("#reqModal").append('<li>' + errMsg + '</li>');
    }
    return secFerrCount;
}
// Copy First PopUp Values into all Popup against every location ROW
function copyValuesFunction() {  
    toastr.options = {
        "closeButton": true,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    var windowHeight = $(window).height() - 90;
    if (!$('#btnCopyValues').hasClass('header')) {
        var errCount = validateSectionFModal();
        if (errCount == 0) {
            //$('#locTable tr').length  gives length of rows including header row
            if ($('#locTable tr').length > 2) {
                var totalRows = $('#locTable tr').length - 1;
                var valuesToCopy = $('#locTable tr').eq(1).find('#jsonValue').val();
                for (var i = 2; i <= totalRows; i++) {
                    $('#locTable tr').eq(i).find('#jsonValue').val('');
                    $('#locTable tr').eq(i).find('#jsonValue').val(valuesToCopy);
                    $('#locTable tr').eq(i).find('.redAddSection').removeClass('redAddSection').addClass('greenAddSection');
                    $('#locTable tr').eq(i).find('td').eq(4).html('');
                    $('#locTable tr').eq(i).find('td').eq(4).addClass('highlight');
                }
                $('#sectionFPopUp').modal('hide');
                $('#parentTableRowNo').val('');
                toastr.success('Your changes has been made successfully.').css({
                    "width": "500px",
                    "margin-top": windowHeight / 2
                });
            }
        }
        else {
            toastr.error("An Error Occurred. Please Fill up the PopUp Fields and Click Save Changes first ").css({
                "width": "500px",
                "margin-top": windowHeight / 2
            });
        }
    }
    else {
        addSection(1);
    }
}