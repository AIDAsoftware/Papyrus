$(document).ready(function () {
    var id = getIdFromUrl();
    new papyrus.RestClient().getDocument(id);
});

function getIdFromUrl() {
    var url = window.location.href;
    return url.substring(url.lastIndexOf("/") + 1);
}