function DocumentService(apiClient) {
    this.api = apiClient;
}

DocumentService.prototype = {
    allDocuments: function () {
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:8888/papyrusapi/api/documents"
        });
    },

    GetDocument: function (documentId) {
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:8888/papyrusapi/api/documents/" + documentId
        });
    },

    createDocument: function (document) {
        this.api.saveDocument(document);
    }
};

