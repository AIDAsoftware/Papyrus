$(document).ready(function () {
    refreshDocumentsList();
});

function refreshDocumentsList() {
    $("#documents-list").html("");
    papyrus.documentService().allDocuments().done(function (documents) {
        documents.forEach(function (document) {
            paintDocumentRowFor(document);
        });
    });
}

function paintDocumentRowFor(document) {
    var url = "http://localhost:8888/papyrus/documents/details/" + document.Id;
    $("#documents-list").append(
        '<a href="' + url + '" class="list-group-item">'
            + document.Title +
        "</a>");
}