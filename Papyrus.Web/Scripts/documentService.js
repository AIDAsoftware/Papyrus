var papyrus = papyrus || {};

(function(ns) {
    function documentService() {
        var documentsURL = "documents/";
        var restClient = papyrus.restClient();

        function allDocuments() {
            return restClient.get(documentsURL);
        };

        function getDocument(documentId) {
            return restClient.get(documentsURL + documentId);
        };

        function createDocument(document) {
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
