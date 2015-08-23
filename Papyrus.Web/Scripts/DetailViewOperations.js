$(document).ready(function () {
    $("#documents-list").html("");
    var id = getIdFromUrl();
    papyrus.documentService().getDocument(id).done(function (document) {
        paintDocumentDetailsFor(document);
    });
});

function 
    paintDocumentDetailsFor(document) {
    $("#document-title").html(document.Title);
    $("#document-content").html(document.Content);
    $("#document-description").html(document.Description);
}

function getIdFromUrl() {
    var url = window.location.href;
    return url.substring(url.lastIndexOf("/") + 1);
}