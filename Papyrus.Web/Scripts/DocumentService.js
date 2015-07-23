function DocumentService(apiClient) {
    this.api = apiClient;
}

DocumentService.prototype = {
    allDocuments: function () {
        return this.api.allDocuments().map(function (obj) {
            return new PapyrusDocument(obj.Id, obj.Title, obj.Description, obj.Content, obj.Language);
        });
    },

    GetDocument: function (documentId) {

    }
};

