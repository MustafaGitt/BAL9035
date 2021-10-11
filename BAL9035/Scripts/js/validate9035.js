/*
 * Validate all the Input HTML Fields tab by tab which are Required
 * Validate Max Lengths of HTML Fields
 */

// Validate all 9035 HTML Input fields
function validateInputMethod() {
    var errMsg = "";
    var errCount = 0;
    $("#req").empty();

    errCount = tab1(errCount, errMsg);
    errCount = tab2(errCount, errMsg);
    errCount = tab3(errCount, errMsg);
    errCount = tab5(errCount, errMsg);
    errCount = tab6(errCount, errMsg);

    return errCount;
}
// Validate inputs Page by Page
function validateInputsByTab(tabNo) {
    var errMsg = "";
    var errCount = 0;
    $("#req").empty();

    if (tabNo == 0) {
        errCount = tab1(errCount, errMsg);
    }

    if (tabNo == 1) {
        errCount = tab1(errCount, errMsg);
        errCount = tab2(errCount, errMsg);
    }

    if (tabNo == 2) {
        errCount = tab1(errCount, errMsg);
        errCount = tab2(errCount, errMsg);
        errCount = tab3(errCount, errMsg);
    }

    if (tabNo == 3) {
        errCount = tab1(errCount, errMsg);
        errCount = tab2(errCount, errMsg);
        errCount = tab3(errCount, errMsg);
    }

    if (tabNo == 4) {
        errCount = tab1(errCount, errMsg);
        errCount = tab2(errCount, errMsg);
        errCount = tab3(errCount, errMsg);
        errCount = tab5(errCount, errMsg);
    }

    if (tabNo == 5) {
        errCount = tab1(errCount, errMsg);
        errCount = tab2(errCount, errMsg);
        errCount = tab3(errCount, errMsg);
        errCount = tab5(errCount, errMsg);
        errCount = tab6(errCount, errMsg);
    }

    return errCount;
}

function tab1(errCount, errMsg) {
    var valA1 = $("input[name='A1']").val();
    var valB1 = $("input[name='B1']").val();
    var valB2 = $("input[name='B2']").val();
    var valB3 = $("input[name='B3']").val();
    var valB4 = $("input[name='B4']:checked").val();
    var valB5 = $("input[name='B5']").val();
    var valB6 = $("input[name='B6']").val();
    var valB7 = $("input[name='B7']").val();
    var valB7a = $("input[name='B7-A']").val();
    var valB7b = $("input[name='B7-B']").val();
    var valB7c = $("input[name='B7-C']").val();
    var valB7d = $("input[name='B7-D']").val();
    var valB7e = $("input[name='B7-E']").val();
    var valB7f = $("input[name='B7-F']").val();
    var valD0 = $("input[name='D0']").val();

    if (valB1.length > 0) {
        if (valB1.length < 5) {
            errCount++;
            errMsg = " Section B Question 1 Length should be at least 5 characters. ";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valB1.length > 50) {
        errCount++;
        errMsg = " Section B Question 1 Length should be less or equal to 50 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (!valB2) {
        errCount++;
        errMsg = "Section B Question 2: SOC (ONET/OES) Code is required";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB2.length > 10) {
        errCount++;
        errMsg = " Section B Question 2 Length should be less or equal to 10 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (!valB3) {
        errCount++;
        errMsg = " Section B Question 3 SOC Occupation Title is required.";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB3.length > 60) {
        errCount++;
        errMsg = " Section B Question 3 Length should be less or equal to 60 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (!valB4) {
        errCount++;
        errMsg = "Section B Question 4: Is this a full-time position must be selected";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (!valB5) {
        errCount++;
        errMsg = "Section B Question 5: Begin Date is required";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    // greater than today's date
    if (valB5.length > 0) {
        var fullDate = new Date();
        var DateB5 = new Date(valB5);
        if (DateB5 < fullDate) {
            errCount++;
            errMsg = " Section B Question 5 Date should be greater than today's date  ";
            $("#req").append('<li>' + errMsg + '</li>');

        }
    }
    // within 183 days
    if (valB5.length > 0) {
        var fullDate = new Date();
        var DateB5 = new Date(valB5);
        var diff = daydiff(DateB5, fullDate);
        if (parseInt(diff) > 183) {
            errCount++;
            errMsg = " Section B Question 5 Date should be with 183 days  ";
            $("#req").append('<li>' + errMsg + '</li>');

        }
    }
    if (valB5.length > 0 && valB6.length > 0) {
        var DateB5 = new Date(valB5);
        var DateB6 = new Date(valB6);
        var diff = daydiff(DateB6, DateB5);
        if (valA1 == 'H-1B1' || valA1 == 'H-1B') {
            if (parseInt(diff) > 1095) {
                errCount++;
                errMsg = " Section B Question 6 : The end date for the workers period of employment must be less than or equal to three years."
                $("#req").append('<li>' + errMsg + '</li>');
            }
        }
        if (valA1 == 'E-3') {
            if (parseInt(diff) > 730) {
                errCount++;
                errMsg = " Section B Question 6 : The end date for the workers period of employment must be less than or equal to two years."
                $("#req").append('<li>' + errMsg + '</li>');
            }
        }
    }

    if (valB5.length > 10) {
        errCount++;
        errMsg = " Section B Question 5 Length should be less or equal to 10 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (!valB6) {
        errCount++;
        errMsg = "Section B Question 6: End Date is required";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB6.length > 10) {
        errCount++;
        errMsg = " Section B Question 6 Length should be less or equal to 10 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7.length > 5) {
        errCount++;
        errMsg = " Section B Question 7 Length should be less or equal to 5 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7a.length <= 0) {
        errCount++;
        errMsg = " Section B Question 7a is Required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7b.length <= 0) {
        errCount++;
        errMsg = " Section B Question 7b is Required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7c.length <= 0) {
        errCount++;
        errMsg = " Section B Question 7c is Required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7d.length <= 0) {
        errCount++;
        errMsg = " Section B Question 7d is Required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7e.length <= 0) {
        errCount++;
        errMsg = " Section B Question 7e is Required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7f.length <= 0) {
        errCount++;
        errMsg = " Section B Question 7f is Required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7a.length > 5) {
        errCount++;
        errMsg = " Section B Question 7a Length should be less or equal to 5 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7b.length > 5) {
        errCount++;
        errMsg = " Section B Question 7b Length should be less or equal to 5 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7c.length > 5) {
        errCount++;
        errMsg = " Section B Question 7c Length should be less or equal to 5 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7d.length > 5) {
        errCount++;
        errMsg = " Section B Question 7d Length should be less or equal to 5 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7e.length > 5) {
        errCount++;
        errMsg = " Section B Question 7e Length should be less or equal to 5 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7f.length > 5) {
        errCount++;
        errMsg = " Section B Question 7f Length should be less or equal to 5 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valB7.length > 0 && valB7a.length > 0 && valB7b.length > 0 && valB7c.length > 0 && valB7d.length > 0 && valB7e.length > 0 && valB7f.length > 0) {
        let total = parseInt(valB7a) + parseInt(valB7b) + parseInt(valB7c) + parseInt(valB7d) + parseInt(valB7e) + parseInt(valB7f);
        if (parseInt(valB7) > total) {
            errCount++;
            errMsg = " The Sum of B7a-B7f must be equal to or greater than B7 worker positions.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
        //if D0 checkbox is not checked then set empty value
        if (!$('#FlagProfileChkBox').is(":checked")) {
            $('#D0').val('');
        }


        //if (valB7a.length || valB7b.length || valB7c.length || valB7d.length || valB7e.length || valB7f.length) {
        //if (valB7a.length == 0) {
        //    valB7a = 0;
        //}
        //if (valB7b.length == 0) {
        //    valB7b = 0;
        //}
        //if (valB7c.length == 0) {
        //    valB7c = 0;
        //}
        //if (valB7d.length == 0) {
        //    valB7d = 0;
        //}
        //if (valB7e.length == 0) {
        //    valB7e = 0;
        //}
        //if (valB7f.length == 0) {
        //    valB7f = 0;
        //}

        //}
    }
    return errCount
}

function tab2(errCount, errMsg) {
    var valE14 = $("input[name='E14']").val();
    if (valE14.length > 0) {
        if (valE14.toLowerCase() != "populated by flag") {
            var chkEmail = isEmail(valE14);
            if (chkEmail == false) {
                errCount++;
                errMsg = " Section E Question 14 :  E.14 is not a valid email address. ";
                $("#req").append('<li>' + errMsg + '</li>');
            }
        }
    }
    return errCount;
}

function tab3(errCount, errMsg) {
    var valB7 = $("input[name='B7']").val();
    var valF1 = $("input[name=F1]").val();
    var valF2 = $("input[name=F2]:checked").val();
    var valF3 = $("input[name=F3]").val();
    var valF3CheckBox = $('#F3CheckBox').is(':checked');
    var valF4 = $("input[name=F4]").val();
    var valF5 = $("input[name=F5]").val();
    var valF6 = $("input[name=F6]").val();
    var valF7 = $("input[name=F7]").val();
    var valF8 = $("input[name=F8]").val();
    var valF9 = $("input[name=F9]").val();
    var valF10from = $("input[name=F10from]").val();
    var valF10fromP = $("input[name=F10fromP]").val();
    var valF10To = $("input[name=F10To]").val();
    var valF10ToP = $("input[name=F10ToP]").val();
    var valF10A = $("input[name=F10-A]:checked").val();
    var valF11A = $("input[name=F11-A]:checked").val();
    var valF11 = $("input[name=F11]").val();
    var valF11P = $("input[name=F11P]").val();
    var valF13 = $("input[name=F13]:checked").val();
    var valF13A = $("input[name=F13-A]:checked").val();
    var valF14 = $("input[name=F14]:checked").val();
    var valF14A = $("input[name=F14-A]:checked").val();
    var valF14B = $("select[name=F14-B]").val();
    var valF14c = $("input[name=F14c]").val();
    var valF14d = $("input[name=F14d]").val();
    var valH1 = $('input[name=H1]:checked').val();
    var valH2 = $('input[name=H2]:checked').val();
    var valH3 = $('input[name=H3]:checked').val();
    var valH4 = $('input[name=H4]:checked').val();
    var valH5 = $('input[name=H5]:checked').val();

    if (!valF1) {
        errCount++;
        errMsg = " Section F Question 1 : Enter the estimated number of works that will perform work at this place of employment under the LCA is required";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    //Len Check
    if (valF1.length > 0) {
        if (valF1.length > 15) {
            errCount++;
            errMsg = " Section F Question 1 :  Length must be less or equal to 15 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF1.length > 0 && valB7.length > 0) {
        var intF1 = parseInt(valF1);
        var intB7 = parseInt(valB7);
        if (intF1 > intB7) {
            errCount++;
            errMsg = " Section F Question 1 : The number of workers at this place of employment may not exceed the total number of workers in B.7";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (!valF2) {
        errCount++;
        errMsg = " Section F Question 2 : Please select one option for Indicate whether the work(s) subject to this LCA will be placed with a secondary entity at this place of employment";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (valF2 && valF2 == 'yes') {
        if (valF3CheckBox && !valF3) {
            errCount++;
            errMsg = " Section F Question 3 : provide the legal business name of the secondary entity’ is required.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }

    //Len Check
    if (valF3.length > 0) {
        if (valF3.length < 5) {
            errCount++;
            errMsg = " Section F Question 3 : Legal Business Name of secondary entity must contain at least 5 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF3.length > 0) {
        if (valF3.length > 100) {
            errCount++;
            errMsg = " Section F Question 3 : Length must be less or equal to 100 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (!valF10from) {
        errCount++;
        errMsg = " Section F Question 10 : Wage Rate Paid to Nonimmigrant Workers From is required";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valF10from.length > 0) {
        if (valF10from.length > 9) {
            errCount++;
            errMsg = " Section F Question 10 From : Length must be less or equal to 9 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF10fromP.length > 0) {
        if (valF10fromP.length > 2) {
            errCount++;
            errMsg = " Section F Question 10 From after Point : Length must be less or equal to 2 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF10To.length > 0) {
        if (valF10To.length > 9) {
            errCount++;
            errMsg = " Section F Question 10 To : Length must be less or equal to 9 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF10ToP.length > 0) {
        if (valF10ToP.length > 2) {
            errCount++;
            errMsg = " Section F Question 10 To after Point : Length must be less or equal to 2 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF11.length > 0) {
        if (valF11.length > 9) {
            errCount++;
            errMsg = " Section F Question 11 : Length must be less or equal to 9 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF11P.length > 0) {
        if (valF11P.length > 2) {
            errCount++;
            errMsg = " Section F Question 11 after Point : Length must be less or equal to 2 characters.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF10from.length > 0 && valF10To.length > 0) {
        var f10 = valF10from;
        if (valF10fromP.length > 0) {
            f10 += "." + valF10fromP;
        }
        var f10T = valF10To;
        if (valF10ToP.length > 0) {
            f10T += "." + valF10ToP;
        }
        if (parseFloat(f10) > parseFloat(f10T)) {
            errCount++;
            errMsg = " Section F : The 'To' value must be Higher then 'From' F10 Wage Rate.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    if (valF10To.length > 0 && valF11.length > 0) {
        var f10T = valF10To;
        if (valF10ToP.length > 0) {
            f10T += "." + valF10ToP;
        }
        var f11 = valF11;
        if (valF11P.length > 0) {
            f11 += "." + valF11P;
        }
        if (parseFloat(f11) > parseFloat(f10T)) {
            errCount++;
            errMsg = " Section F : F10 'To' value must be higher than F11 Prevailing Wage";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    else if (valF10To.length <= 0 && valF10from.length > 0 && valF11.length > 0) {
        var f10 = valF10from;
        if (valF10fromP.length > 0) {
            f10 += "." + valF10fromP;
        }
        var f11 = valF11;
        if (valF11P.length > 0) {
            f11 += "." + valF11P;
        }
        if (parseFloat(f11) > parseFloat(f10)) {
            errCount++;
            errMsg = " Section F : F10 'From' value must be higher than F11 Prevailing Wage";
            $("#req").append('<li>' + errMsg + '</li>');
        }
    }
    //if (valF10from.length > 0 && valF11.length > 0) {
    //    var f10 = valF10from;
    //    if (valF10fromP.length > 0) {
    //        f10 += "." + valF10fromP;
    //    }
    //    var f11 = valF11;
    //    if (valF11P.length > 0) {
    //        f11 += "." + valF11P;
    //    }
    //    if (parseFloat(f11) > parseFloat(f10)) {
    //        errCount++;
    //        errMsg = " Section F : F10 Wage Rate From value should be Greater then F11 Prevailing Wage";
    //        $("#req").append('<li>' + errMsg + '</li>');
    //    }
    //}
    var table = $("#locTable tbody");
    table.find('tr').each(function () {
        var breakLoop = 0;
        var $tds = $(this).find('td');
        var County = $tds.eq(4).text();
        var locWageFrom = $(this).find('#wageFrom').text();
        var locSecEntBName = $(this).find('#tblSecondEntityName').text();
        if ($tds.eq(7).find("input").val().length > 0) {
            var popUpObj = $.parseJSON($tds.eq(7).find("input").val());
            if (popUpObj.F13 == true) {
                if (popUpObj.StateOrTerritory == "0" || popUpObj.RnDPosition == null || popUpObj.CollectionType == null || popUpObj.HCPosition == null || popUpObj.Per == "0" || popUpObj.AreaBasedOn == null || popUpObj.Area == "0") {
                    errCount++;
                    breakLoop++;
                    errMsg = "Section F : Click on the '+' icon button in Location Table to provide additional required fields of Section F.13";
                    $("#req").append('<li>' + errMsg + '</li>');
                }
            }
            else if (popUpObj.F14 == true) {
                if (popUpObj.F14a == null || popUpObj.F14b == "0") {
                    errCount++;
                    breakLoop++;
                    errMsg = "Section F : Click on the '+' icon button in Location Table to provide additional required fields of Section F.13";
                    $("#req").append('<li>' + errMsg + '</li>');
                }
                else if (popUpObj.F14a.toLowerCase() == "other" && (popUpObj.F14c == null || popUpObj.F14d == null)) {
                    errCount++;
                    breakLoop++;
                    errMsg = "Section F : Click on the '+' icon button in Location Table to provide additional required fields of Section F.13";
                    $("#req").append('<li>' + errMsg + '</li>');
                }

                if (!valF10A) {
                    errCount++;
                    breakLoop++;
                    errMsg = "Section F Question 10a: Per value is required";
                    $("#req").append('<li>' + errMsg + '</li>');
                }
                if (!valF11) {
                    errCount++;
                    breakLoop++;
                    errMsg = "Section F Question 11: Prevailing Wage Rate value is required.";
                    $("#req").append('<li>' + errMsg + '</li>');
                }
                if (!valF11A) {
                    errCount++;
                    breakLoop++;
                    errMsg = "Section F Question 11a: Per value is required";
                    $("#req").append('<li>' + errMsg + '</li>');
                }

            }

        }
        else {
            errCount++;
            breakLoop++;
            errMsg = " Select F.13 or F.14 for Section F to be populated on FLAG.  Click the '+' icon next to the work location";
            $("#req").append('<li>' + errMsg + '</li>');
        }
        if (County == "") {
            errCount++;
            breakLoop++;
            errMsg = " County must be provided for Section F to be populated on FLAG.  Click the Edit icon next to the work location and select the County from the drop down.";
            $("#req").append('<li>' + errMsg + '</li>');
        }

        if (!locWageFrom) {
            errCount++;
            breakLoop++;
            errMsg = " Wage Range From must be provided for Section F. Click on the Edit icon for each location to provide the location-specific wage range.";
            $("#req").append('<li>' + errMsg + '</li>');
        }
        if (!valF3CheckBox && !locSecEntBName) {
            errCount++;
            breakLoop++;
            errMsg = " Secondary Entity Business Name must be provided for Section F. Click on the Edit icon for each location to provide the location-specific Secondary Entity Business Name";
            $("#req").append('<li>' + errMsg + '</li>');
        }
        if (breakLoop > 0) {
            return false;
        }
    });



    if (!valH1) {
        errCount++;
        errMsg = " Section H Question 1 :  At the time of filing this LCA, is the employer H-1B dependent value is Required";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (!valH2) {
        errCount++;
        errMsg = " Section H Question 2 :  At the time of filing this LCA, is the employer a willfull violator value is Required";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (valH1 == 'yes' && valH2 == 'yes' && !valH3) {
        errCount++;
        errMsg = " Section H Question 3 value is Required";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (valH3 && valH3 == 'yes' && !valH4) {
        errCount++;
        errMsg = " Section H Question 4 value is Required";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (valH1 == 'yes' && valH2 == 'yes' && !valH5) {
        errCount++;
        errMsg = " Section H Question 5 value is Required";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    return errCount
}

function tab5(errCount, errMsg) {
    var valJ1 = $("input[name=J1]").val();
    var valJ2 = $("input[name=J2]").val();
    var valJ3 = $("input[name=J3]").val();
    var valJ4 = $("input[name=J4]").val();

    if (!valJ1) {
        errCount++;
        errMsg = " Section J Question 1: Last (family) name of hiring or designated official Value is required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (!valJ2) {
        errCount++;
        errMsg = " Section J Question 2:  First (given) name of hiring or designated official Value is required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (!valJ4) {
        errCount++;
        errMsg = " Section J Question 4:  Hiring or designated official title Value is required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (valJ1.length > 35) {
        errCount++;
        errMsg = " Section J Question 1 : Length should be less or equal to 35 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valJ2.length > 35) {
        errCount++;
        errMsg = " Section J Question 2 : Length should be less or equal to 35 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valJ3.length > 3) {
        errCount++;
        errMsg = " Section J Question 3 : Length should be less or equal to 3 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valJ4.length > 60) {
        errCount++;
        errMsg = " Section J Question 4 : Length should be less or equal to 60 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    return errCount;
}

function tab6(errCount, errMsg) {
    // Section K
    var valK1 = $("input[name=K1]").val();
    var valK2 = $("input[name=K2]").val();
    var valK3 = $("input[name=K3]").val();
    var valK4 = $("input[name=K4]").val();
    var valK5 = $("input[name=K5]").val();

    if (!valK1) {
        errCount++;
        errMsg = " Section K Question 1 : Last (family) Name is required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (!valK2) {
        errCount++;
        errMsg = " Section K Question 2 : First (given) Name is required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (!valK4) {
        errCount++;
        errMsg = " Section K Question 4 : Firm/Business Name is required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    if (!valK5) {
        errCount++;
        errMsg = " Section K Question 5 : Email Address is required ";
        $("#req").append('<li>' + errMsg + '</li>');
    }

    // K SECTION
    if (valK1.length > 35) {
        errCount++;
        errMsg = " Section K Question 1 : Length should be less or equal to 35 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valK2.length > 35) {
        errCount++;
        errMsg = " Section K Question 2 : Length should be less or equal to 35 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valK3.length > 3) {
        errCount++;
        errMsg = " Section K Question 3 : Length should be less or equal to 3 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valK4.length > 100) {
        errCount++;
        errMsg = " Section K Question 4 : Length should be less or equal to 100 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    if (valK5.length > 100) {
        errCount++;
        errMsg = " Section K Question 5 : Length should be less or equal to 100 characters. ";
        $("#req").append('<li>' + errMsg + '</li>');
    }
    return errCount
}
// parse date
function parseDate(str) {
    var mdy = str.split('/')
    return new Date(mdy[2], mdy[0] - 1, mdy[1]);
}
// get the days between the date range
function daydiff(first, second) {
    var diff = new Date(first - second);
    var days = Math.round(diff / (1000 * 60 * 60 * 24));
    return days
}
// Validate Email
function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}