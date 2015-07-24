function DocumentService() {
    this.documentsURL = function () {
        return "http://localhost:8888/papyrusapi/documents/";
    }
}

DocumentService.prototype = {
    allDocuments: function () {
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: this.documentsURL()
        });
    },

    GetDocument: function (documentId) {
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: this.documentsURL() + documentId
        });
    },

    createDocument: function (document) {
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: this.documentsURL(),
            data: {
                Title: document.Title,
                Description: document.Description,
                Content: document.Content,
                Language: document.Language
            }
        });
    }
};

