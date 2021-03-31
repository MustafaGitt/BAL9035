// Create Data Asset
function CreateAssets(isSubmit, sysid, balNo, email) {
    // validate inputs
    var errCount = validateInputMethod();
    if (errCount == 0) {
        // Fetch Values and Call the Data Asset API
        fetchValues(sysid, balNo, isSubmit, email);
    }
    else {
        document.getElementById("btnDlgOK").style.display = "none";
        document.getElementById("btnDlgClose").style.display = "inline";
        if (isSubmit == false) {
            document.getElementById("btnDlgSL").style.display = "inline";
        }
        else {
            document.getElementById("btnDlgSL").style.display = "none";
        }
        $("#errorModal").modal();

    }

}
// Call the Asset Data do not check the validation
function SaveLater(isSubmit, sysid, balNo) {
    fetchValues(sysid, balNo, isSubmit);
    document.getElementById("btnDlgSL").style.display = "none";
}
// Fetch all the HTML INPUTS, Convert it into model object and calls the API
function fetchValues(balNo, sysid, isSubmit, email) {
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
    // Section E
    var valE14 = $("input[name='E14']").val();
    //Section F
    var valF1 = $("input[name=F1]").val();
    var valF2 = $("input[name=F2]:checked").val();
    var valF3 = $("input[name=F3]").val();
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
    //Section H
    var valH1 = $("input[name=H1]:checked").val();
    var valH2 = $("input[name=H2]:checked").val();
    var valH3 = $("input[name=H3]:checked").val();
    var valH4 = $("input[name=H4]:checked").val();
    var valH5 = $("input[name=H5]:checked").val();
    var valH6 = $("input[name=H6]:checked").val();
    //Section I
    var valI1a = $("input[name=I1a]:checked").val();
    var valI1b = $("input[name=I1b]:checked").val();
    //Section J
    var valJ1 = $("input[name=J1]").val();
    var valJ2 = $("input[name=J2]").val();
    var valJ3 = $("input[name=J3]").val();
    var valJ4 = $("input[name=J4]").val();
    //Section K
    var valK1 = $("input[name=K1]").val();
    var valK2 = $("input[name=K2]").val();
    var valK3 = $("input[name=K3]").val();
    var valK4 = $("input[name=K4]").val();
    var valK5 = $("input[name=K5]").val();
    //Section FModal
    //var valFModalState = $("select[name=FModalState]").val();
    //var valCType = $("input[name=CType]:checked").val();
    //var valAB = $("input[name=AB]:checked").val();
    //var valFModalArea = $("select[name=SectionFArea]").val();
    //var valRDP = $("input[name=RDP]:checked").val();
    //var valHCP = $("input[name=HCP]:checked").val();
    //var valFModalPer = $("select[name=FModalPer]").val();

    var F10From = valF10from;
    if (valF10fromP.length > 0) {
        F10From = valF10from + "." + valF10fromP;
    }

    var F10To = valF10To;
    if (valF10ToP.length > 0) {
        F10To = valF10To + "." + valF10ToP;
    }
    var F11 = valF11;
    if (valF11P.length > 0) {
        F11 = valF11 + "." + valF11P;
    }
    var I1 = "";
    if (valI1a != null) {
        I1 += valI1a;
    }
    if (valI1b != null) {
        if (I1.length < 0) {
            I1 += valI1b;
        }
        else {
            I1 += "-" + valI1b;
        }

    }
    var SectionB = {
        'B1': valB1,
        'B2': valB2,
        'B3': valB3,
        'B4': convertToBool(valB4),
        'B5': valB5,
        'B6': valB6,
        'B7': valB7,
        'B7a': valB7a,
        'B7b': valB7b,
        'B7c': valB7c,
        'B7d': valB7d,
        'B7e': valB7e,
        'B7f': valB7f,
    }
    var SectionD = {
        'D0': valD0,
    }
    var SectionF = {
        'F1': valF1,
        'F2': convertToBool(valF2),
        'F3': valF3,
        'F10From': F10From,
        'F10To': F10To,
        'F10a': UndefinedToNull(valF10A),
        'F11': F11,
        'F11a': UndefinedToNull(valF11A),

    }
    var SectionH = {
        'H1': convertToBool(valH1),
        'H2': convertToBool(valH2),
        'H3': convertToBool(valH3),
        'H4': valH4,
        'H5': convertBoolToEnum(valH5),
        'H6': convertToBool(valH6),
    }
    var SectionJ = {
        'J1': valJ1,
        'J2': valJ2,
        'J3': valJ3,
        'J4': valJ4,
    }
    var SectionK = {
        'K1': valK1,
        'K2': valK2,
        'K3': valK3,
        'K4': valK4,
        'K5': valK5,
    }
    var caseSubType = $("#casetype li").map(function () {
        return $(this).text();
    }).get();
    var PcaseSubType = $("#Pcasetype li").map(function () {
        return $(this).text();
    }).get();
    var LocationsList = $("#locTable tr").map(function (id) {
        if (id > 0) {
            var jsonObj = $.parseJSON($('#locTable tr').eq(id).find('#jsonValue').val());
            if (isSubmit == true) {
                if (jsonObj.StateOrTerritory != "0") {
                    $('select[name=FModalState]').val(jsonObj.StateOrTerritory);
                    jsonObj.StateOrTerritory = $("select[name=FModalState] option:selected").text();
                }
            }
            return {
                'LocationId': id,
                "Address1": $("td", this).eq(0).text(),
                "Address2": $("td", this).eq(1).text(),
                "City": $("td", this).eq(2).text(),
                "State": $("td", this).eq(3).text(),
                "County": $.trim($("td", this).eq(4).text()),
                "PostalCode": $("td", this).eq(5).text(),
                //'FmodalObject': $("td", this).eq(7).text(),
                'FmodalObject': jsonObj
            };
        }
    }).get();
    var Lists = {
        'BALNumber': balNo,
        'LocationsList': LocationsList,
        'caseSubTypes': caseSubType,
        'parentCaseSubTypes': PcaseSubType
    }
    var Form9035 = {
        'isSubmit': isSubmit,
        'SectionB': SectionB,
        'SectionD': SectionD,
        'E14': valE14,
        'SectionF': SectionF,
        'SectionH': SectionH,
        'I1': I1,
        'SectionJ': SectionJ,
        'SectionK': SectionK,
        'otherValues': ''

    };
    var DataModel = {
        'Form9035': Form9035,
        'Lists': Lists
    }
    var Responsebody = {
        'BalNumber': balNo,
        'Sysid': sysid,
        'JsonString': JSON.stringify(Form9035),
        'ListJsonString': JSON.stringify(Lists),
        'isSubmit': isSubmit,
        'Email': email,
        'EsIdNo': ''
    }
    var appUrl = window.location.href;
    appUrl = appUrl.slice(0, appUrl.lastIndexOf('/'));

    $.ajax({

        type: "POST",
        url: appUrl + "/api/Save9035/CreateAsset",
        dataType: 'json',
        data: Responsebody,
        success: function (response) {
            var json = $.parseJSON(response);
            $("#req").empty();
            $("#req").append('<li>' + json.message + '</li>');

            if (json.success == true) {
                $("#regForm :input").prop("disabled", true);
                $("#btnNext").prop("disabled", false);
                $("#btnprev").prop("disabled", false);
                $("#btnDlgClose").text("Close");
                document.getElementById("btnDlgOK").style.display = "none";
                document.getElementById("btnDlgSL").style.display = "none";
                // If it is Submission Case Get the Secret Key and show POPUP
                if (isSubmit == true) {
                    $.ajax({
                        type: "GET",
                        url: appUrl + "/api/AddCredentials/GetESKey?username=" + email,
                        dataType: 'json',
                        success: function (response) {
                            var json = $.parseJSON(response);
                            // Sets the valus of key if find in HTML Input field
                            if (json.success == true) {
                                $("#key").val(json.message);
                                $("#credPopUp").modal();
                            }

                        },
                        error: function (error) {
                            console.log(error)
                        }

                    });
                }
                else {
                    $("#errorModal").modal();
                }
                $("#exampleModalLongTitle").text("");
            }
            else {
                document.getElementById("btnDlgOK").style.display = "none";
                document.getElementById("btnDlgSL").style.display = "none";
                $("#errorModal").modal();
                $("#exampleModalLongTitle").text("");
            }

        },
        error: function (error) {
            console.log(error)
        }
    });
}
// Enum
var NaSelection = {
    Yes: 1,
    No: 2,
    NA: 3
};
// Convert input value to bool
function convertToBool(str) {
    if (str != null && str != "") {
        if (str.toLowerCase() == "yes") {
            return true
        }
        else if (str.toLowerCase() == "no") {
            return false
        }
    }
    else {
        str = "";
    }
    return str
}
// Convert Bool value to ENUM
function convertBoolToEnum(str) {
    if (str != null && str != "") {
        if (str.toLowerCase() == "yes") {
            str = NaSelection.Yes
        }
        else if (str.toLowerCase() == "no") {
            str = NaSelection.No
        }
        else if (str.toLowerCase() == "na") {
            str = NaSelection.NA
        }
    }
    else {
        str = 0;
    }
    return str

}
// Convert input checkbox value to bool
function convertCheckboxToBool(str) {
    if (str != null && str != "") {
        if (str.toLowerCase() == "yes") {
            return true
        }
        else {
            return false
        }
    }
    else {
        return false
    }
    return str
}
// Convert Undefined to NULL
function UndefinedToNull(str) {
    if (typeof str == "undefined") {
        str = null;
    }
    return str;
}