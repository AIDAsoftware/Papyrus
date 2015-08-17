var papyrus = papyrus || {};

(function(ns) {
    function restClient() {
        var ajaxContentType = "application/json; charset=utf-8";
        var apiUrl = "http://localhost:8888/papyrusapi/";
        var documents = "documents/";

        var allDocuments = function () {
            return get(documents);
        };

        var getDocument = function (documentId) {
            return get(documents + documentId);
        };

        function get (url, successCallBack) {
            return $.ajax({
                type: "GET",
                contentType: ajaxContentType,
                url: apiUrl + url,
                success: successCallBack,
                error: function (request, status, error) { }
            });
        }

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
    ns.RestClient = restClient;
})(papyrus);
