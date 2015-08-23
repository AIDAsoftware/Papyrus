$(document).ready(function() {
    $("#documents-list").html("");
    var id = getIdFromUrl();
    papyrus.documentService().getDocument(id);
});

function getIdFromUrl() {
    var url = window.location.href;
    return url.substring(url.lastIndexOf("/") + 1);
}