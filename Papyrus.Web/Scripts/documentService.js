var papyrus = papyrus || {};

(function(ns) {
    function DocumentService() {
        var ajaxContentType = "application/json; charset=utf-8";
        var apiUrl = "http://localhost:8888/papyrusapi/";
        var documents = "documents/";
        var restClient = new papyrus.RestClient();

        var allDocuments = function () {
            return restClient.get(documents);
        };

        var getDocument = function (documentId) {
            return restClient.get(documents + documentId);
        };

        var createDocument = function (document) {
            return restClient.post(documents, document);
        }

        return {
            getDocument: getDocument,
            createDocument: createDocument,
            allDocuments: allDocuments
        }
    }

    ns.DocumentService = DocumentService;
})(papyrus);
