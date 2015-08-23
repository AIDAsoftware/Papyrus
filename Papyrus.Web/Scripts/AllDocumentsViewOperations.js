$(document).ready(function () {
    refreshDocumentsList();
});

function refreshDocumentsList() {
    $("#documents-list").html("");
    papyrus.documentService().allDocuments();
}