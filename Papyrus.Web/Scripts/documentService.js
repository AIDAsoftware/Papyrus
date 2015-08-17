var papyrus = papyrus || {};

(function(ns) {
    function DocumentService() {
        var ajaxContentType = "application/json; charset=utf-8";
        var apiUrl = "http://localhost:8888/papyrusapi/";
        var documents = "documents/";

        var allDocuments = function () {
            return RestClient.get(documents);
        };

        var getDocument = function (documentId) {
            return RestClient.get(documents + documentId);
        };

        var createDocument = function (document) {
            return $.ajax({
                type: "POST",
                contentType: ajaxContentType,
                url: apiUrl + documents,
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

    ns.DocumentService = DocumentService;
})(papyrus);
