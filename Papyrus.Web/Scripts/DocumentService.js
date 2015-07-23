function DocumentService(apiClient) {
    this.api = apiClient;
}

DocumentService.prototype = {
    allDocuments: function () {
        return this.api.allDocuments().map(this.parseJsonToDocument);
    },

    GetDocument: function (documentId) {
        var documentAsJson = this.api.GetDocument(documentId);
        return this.parseJsonToDocument(documentAsJson);
    },

    createDocument: function (document) {
        this.api.saveDocument(document);
    },

    parseJsonToDocument: function (documentAsJson) {
        return new PapyrusDocument()
            .withId(documentAsJson.Id)
            .withTitle(documentAsJson.Title)
            .withDescription(documentAsJson.Description)
            .withContent(documentAsJson.Content)
            .forLanguage(documentAsJson.Language);
    }
};

