function DocumentService() {}

DocumentService.prototype = {
    allDocuments: function () {
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:8888/papyrusapi/documents"
        });
    },

    GetDocument: function (documentId) {
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:8888/papyrusapi/documents/" + documentId
        });
    },

    createDocument: function (document) {
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:8888/papyrusapi/documents",
            data: {
                Title: document.Title,
                Description: document.Description,
                Content: document.Content,
                Language: document.Language
            }
        });
    }
};

