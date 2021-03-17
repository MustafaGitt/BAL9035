/*
 * This JS METHOD is filtering the characters which are not required in some specific Field
 * It will allow the characters which are placed in a REGEX
 * Also Remove the highlighted CSS when user enter some text
 */
function filterValidations() {
    // JIRA ISSUE NO BAL-154
    //$('#B1').bind('keyup blur', function () {
    //    var node = $(this);
    //    node.val(node.val().replace(/[^A-Za-z0-9 ]/g, ''));
    //});
    $('#B2').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9-]/g, ''));
    });
    $('#B3').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z ]/g, ''));
    });
    $('#B5').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9/]/g, ''));
    });
    $('#B6').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9/]/g, ''));
    });
    $('#B7').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
        var value = $(node).val();
        if (value.length <= 0) {
            $(node).addClass("highlight");
        }
        else {
            $(node).removeClass("highlight");
        }
    });
    $('#B7-A').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
        var value = $(node).val();
        if (value.length <= 0) {
            $(node).addClass("highlight");
        }
        else {
            $(node).removeClass("highlight");
        }
    });
    $('#B7-B').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
        var value = $(node).val();
        if (value.length <= 0) {
            $(node).addClass("highlight");
        }
        else {
            $(node).removeClass("highlight");
        }
    });
    $('#B7-C').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
        var value = $(node).val();
        if (value.length <= 0) {
            $(node).addClass("highlight");
        }
        else {
            $(node).removeClass("highlight");
        }
    });
    $('#B7-D').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
        var value = $(node).val();
        if (value.length <= 0) {
            $(node).addClass("highlight");
        }
        else {
            $(node).removeClass("highlight");
        }
    });
    $('#B7-E').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
        var value = $(node).val();
        if (value.length <= 0) {
            $(node).addClass("highlight");
        }
        else {
            $(node).removeClass("highlight");
        }
    });
    $('#B7-F').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
        var value = $(node).val();
        if (value.length <= 0) {
            $(node).addClass("highlight");
        }
        else {
            $(node).removeClass("highlight");
        }
    });
    $('#F1').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
    });
    $('#F3').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z0-9 ]/g, ''));
    });
    $('#F4').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z0-9 ]/g, ''));
    });
    $('#F5').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z0-9 ]/g, ''));
    });
    $('#F6').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z ]/g, ''));
    });
    $('#F9').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
    });
    $('#F10from').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
    });
    $('#F10fromP').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
    });
    $('#F10To').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
    });
    $('#F10ToP').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
    });
    $('#F11').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
    });
    $('#F11P').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^0-9]/g, ''));
    });
    $('#F14c').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z0-9&() ]/g, ''));
    });
    $('#F14d').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z0-9&() ]/g, ''));
    });
    // restriction removed as per client requirements (JIRA-ISSUE-127)
    //$('#J1').bind('keyup blur', function () {
    //    var node = $(this);
    //    node.val(node.val().replace(/[^A-Za-z ]/g, ''));
    //});
    //$('#J2').bind('keyup blur', function () {
    //    var node = $(this);
    //    node.val(node.val().replace(/[^A-Za-z ]/g, ''));
    //});
    //$('#J3').bind('keyup blur', function () {
    //    var node = $(this);
    //    node.val(node.val().replace(/[^A-Za-z ]/g, ''));
    //});
    //$('#J4').bind('keyup blur', function () {
    //    var node = $(this);
    //    node.val(node.val().replace(/[^A-Za-z0-9 ]/g, ''));
    //});
    $('#K1').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z ]/g, ''));
    });
    $('#K2').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z ]/g, ''));
    });
    $('#K3').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^A-Za-z ]/g, ''));
    });
    // JIRA ISSUE NO BAL-153
    //$('#K4').bind('keyup blur', function () {
    //    var node = $(this);
    //    node.val(node.val().replace(/[^A-Za-z0-9 ]/g, ''));
    //});
    
}
