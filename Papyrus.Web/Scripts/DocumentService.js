function DocumentService(apiClient) {
    this.api = apiClient;
}

DocumentService.prototype = {
    allDocuments: function () {
        return this.api.allDocuments();
    },

    GetDocument: function (documentId) {
        return this.api.GetDocument(documentId);
    },

    createDocument: function (document) {
        this.api.saveDocument(document);
    }
};

