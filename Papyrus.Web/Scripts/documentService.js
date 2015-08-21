var papyrus = papyrus || {};

(function(ns) {
    function documentService() {
        var documentsURL = "documents/";
        var restClient = new papyrus.RestClient();

        var allDocuments = function () {
            return restClient.get(documentsURL);
        };

        var getDocument = function (documentId) {
            return restClient.get(documentsURL + documentId);
        };

        var createDocument = function (document) {
            return restClient.post(documentsURL, document);
        }

        return {
            getDocument: getDocument,
            createDocument: createDocument,
            allDocuments: allDocuments
        }
    }

    ns.documentService = documentService;
})(papyrus);
