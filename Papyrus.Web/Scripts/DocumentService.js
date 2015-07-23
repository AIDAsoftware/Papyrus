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

    parseJsonToDocument: function (documentAsJson) {
        return new PapyrusDocument()
            .withId(documentAsJson.Id)
            .withTitle(documentAsJson.Title)
            .withDescription(documentAsJson.Description)
            .withContent(documentAsJson.Content)
            .forLanguage(documentAsJson.Language);
    }
};

