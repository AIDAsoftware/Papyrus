$(document).ready(function () {
    var url = window.location.href;
    var id = url.substring(url.lastIndexOf("/") + 1);
    new DocumentService().GetDocument(id).done(function(document) {
        $("#document-title").html(document.Title);
        $("#document-content").html(document.Content);
        $("#document-description").html(document.Description);
    });
});