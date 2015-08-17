var papyrus = papyrus || {};

(function(ns) {
    function restClient() {
        var documentsUrl = "http://localhost:8888/papyrusapi/documents/";
        var ajaxContentType = "application/json; charset=utf-8";

        var allDocuments = function () {
            return $.ajax({
                type: "GET",
                contentType: ajaxContentType,
                url: documentsUrl,
                success: function () { },
                error: function (request, status, error) { }
            });
        };

        var getDocument = function (documentId) {
            return $.ajax({
                type: "GET",
                contentType: ajaxContentType,
                url: documentsUrl + documentId,
                success: function () { },
                error: function (request, status, error) { }
            });
        };

        var createDocument = function (document) {
            return $.ajax({
                type: "POST",
                contentType: ajaxContentType,
                url: documentsUrl,
                data: JSON.stringify(document),
                success: function () {
                    console.log("document created");
                },
                error: function (request, status, error) {
                    console.log(request.responseText);
                }
            });
        }

        return {
            getDocument: getDocument,
            createDocument: createDocument,
            allDocuments: allDocuments
        }
    }
    ns.RestClient = restClient;
})(papyrus);
