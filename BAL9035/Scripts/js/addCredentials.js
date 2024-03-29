﻿// Validate Credential inputs
function validateCredentialsInputs() {
    var errMsg = "";
    var errCount = 0;
    username = $('#username').val();
    password = $('#password').val();

    if (username.length <= 0) {
        errCount++;
        errMsg += " Please Enter User Name";
        $('#reqCred').html(errMsg)
        return errCount
    }
    if (password.length <= 0) {
        errCount++;
        errMsg += " Please Enter Password ";
        $('#reqCred').html(errMsg)
        return errCount
    }
    if (password.length > 0 && password.length < 4) {
        errCount++;
        errMsg += " Please Enter at least 4 characters for Password ";
        $('#reqCred').html(errMsg)
        return errCount
    }
    return errCount

}
// validate Secret Key Input
function SetESKey(email) {
    $('#reqCred').html("");
    var key = $('#key').val();
    var appUrl = window.location.href;
    appUrl = appUrl.slice(0, appUrl.lastIndexOf('/'));
    if (key != null && key != "") {
        if (key.length > 6) {
            $("#divKey").addClass("inactive");
            $("#btnAddKey").addClass("inactive");
            $("#divUser").removeClass("inactive");
            $("#divPass").removeClass("inactive");
            $("#btnAdd").removeClass("inactive");
        }
        else {
            $('#reqCred').html(" It looks like you entered a temporary authentication code instead of your secret key. If you aren't sure where to find your secret key, click on the link for How to Find Your Secret Key or contact #automationinfo.");
        }
    }
    else {
        $('#reqCred').html("Please enter Secret Key.");
    }

}
// Call Save Credential Asset API
function CreateCredentialAsset(id, bal_no, email) {
    $('#reqCred').html("");
    // Validate inputs
    var errCount = validateCredentialsInputs();
    if (errCount == 0) {
        //Fetching values
        var username = $('#username').val();
        var password = $('#password').val();
        var key = $('#key').val();
        var Number = id;
        // Making web api input object
        var param = {

            'Name': Number,
            'Username': username,
            'Password': password,
            'SecretKey': key,
            'Email': email
        }
        var appUrl = window.location.href;
        appUrl = appUrl.slice(0, appUrl.lastIndexOf('/'));
        //  Calling an API
        $.ajax({
            type: "POST",
            url: appUrl + "/api/AddCredentials",
            dataType: 'json',
            data: param,
            success: function (response) {
                var json = $.parseJSON(response);
                // If success then add a queue item
                if (json.success == true) {
                    var esKeyId = json.message;
                    var Responsebody = {
                        'BalNumber': bal_no,
                        'Sysid': Number,
                        'JsonString': "",
                        'isSubmit': true,
                        'Email': email,
                        'EsIdNo': esKeyId
                    }
                    // Adding a queue item by calling an API
                    $.ajax({
                        type: "POST",
                        url: appUrl + "/api/Save9035/AddQueueItem",
                        dataType: 'json',
                        data: Responsebody,
                        success: function (response) {
                            var json = $.parseJSON(response);
                            $("#req").empty();
                            $("#req").append('<li>' + json.message + '</li>');
                            $("#credPopUp").modal('hide');
                            $("#errorModal").modal();
                        },
                        error: function (error) {
                            console.log(error)
                        }
                    });

                }

            },
            error: function (error) {
                console.log(error)
            }

        });
    }
    else {
        $('.alert-msg').addClass('altShow');
        setTimeout(function () { $(".alert-msg").removeClass('altShow'); }, 2000);
    }

}