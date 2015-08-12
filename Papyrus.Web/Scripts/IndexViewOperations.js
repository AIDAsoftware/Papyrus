$(document).ready(function () {
    refreshDocumentsList();
});

function createButtonClick() {
    var document = {
        Title: $("#inputTitle").val(),
        Description: $("#inputDescription").val(),
        Content: $("#inputContent").val(),
        Language: $("#inputLanguage").val()
    };
    new DocumentService().createDocument(document).done(function() {
        refreshDocumentsList();
    });
}

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
    var url = "http://localhost:8888/papyrus/documents/details/" + document.Id;
    $("#documents-list").append(
        '<a href="' + url + '" class="list-group-item">'
            + document.Title +
        "</a>");
}