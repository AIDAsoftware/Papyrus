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
        documents.forEach(function(document) {
            $("#documents-list").append('<a href="http://localhost:8888/papyrus/document/detail/' + document.Id
                + '" class="list-group-item">' + document.Title + '</a>');
        })
    });
}