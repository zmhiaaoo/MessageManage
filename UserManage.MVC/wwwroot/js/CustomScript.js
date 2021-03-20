function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = "deleteMessage_" + uniqueId;
    var confirmDeleteSpan = "confirmDelete_" + uniqueId;

    if (isDeleteClicked) {
        $("#" + deleteSpan).hide();
        $("#" + confirmDeleteSpan).show();
    } else {
        $("#" + deleteSpan).show();
        $("#" + confirmDeleteSpan).hide();
    }
}