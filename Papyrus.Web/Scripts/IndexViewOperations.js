$(document).ready(function () {
    refreshDocumentsList();
});

function refreshDocumentsList() {
    $("#documents-list").html("");
    new DocumentService().allDocuments().done(function (documents) {
        console.log(documents);
        documents.forEach(function (document) {
            paintDocumentRowFor(document);
        });
    });
}

function paintDocumentRowFor(document) {
    //TODO: change static url
    var url = "http://localhost:8888/papyrus/documents/details/" + document.Id;
    $("#documents-list").append(
        '<a href="' + url + '" class="list-group-item">'
            + document.Title +
        "</a>");
}