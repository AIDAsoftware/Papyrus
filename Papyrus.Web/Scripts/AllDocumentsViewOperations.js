$(document).ready(function () {
    refreshDocumentsList();
});

function refreshDocumentsList() {
    $("#documents-list").html("");
    new papyrus.DocumentService().allDocuments()    ;
}