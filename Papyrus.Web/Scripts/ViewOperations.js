$(document).ready(function () {
    new DocumentService().allDocuments().done(function(documents) {
        for (var i = 0; i < documents.length; i++) {
            $("#documents-list").append('<a href="#" class="list-group-item">' + documents[i].Title + '</a>');
        }
    });
});

function createButtonClick() {
    var document = {
        Title: $("#inputTitle").val(),
        Description: $("#inputDescription").val(),
        Content: $("#inputContent").val(),
        Language: $("#inputLanguage").val()
    };
    new DocumentService().createDocument(document);
}