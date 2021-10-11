// Modal events

$('#multiplePurposeModal').on("hidden.bs.modal", function () {
    // hide btnConfirm buttin, because this modal is used for other purpose as well.
    $('#btnConfirm').css("display", "none");
});

