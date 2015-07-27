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
        for (var i = 0; i < documents.length; i++) {
            $("#documents-list").append('<a href="#" class="list-group-item">' + documents[i].Title + '</a>');
        }
    });
}