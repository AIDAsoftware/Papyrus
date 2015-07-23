function DocumentService(apiClient) {}

DocumentService.prototype = {
    allDocuments: function () {
        return [new PapyrusDocument("Any", "Any", "Any", "Any")];
    }
};

