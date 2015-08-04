function DocumentService() {
    this.documentsURL = function () {
        return "http://localhost:8888/papyrusapi/documents/";
    };
    this.ajaxContentType = function () {
        return "application/json; charset=utf-8";
    }
}

DocumentService.prototype = {
    allDocuments: function () {
        return $.ajax({
            type: "GET",
            contentType: this.ajaxContentType(),
            url: this.documentsURL(),
            success: function() {},
            error: function (request, status, error) {}
        });
    },

    GetDocument: function (documentId) {
        return $.ajax({
            type: "GET",
            contentType: this.ajaxContentType(),
            url: this.documentsURL() + documentId,
            success: function() {},
            error: function (request, status, error) {}
        });
    },

    createDocument: function (document) {
        return $.ajax({
            type: "POST",
            contentType: this.ajaxContentType(),
            url: this.documentsURL(),
            data: JSON.stringify(document),
            success: function() {
                console.log("document created");
            },
            error: function (request, status, error) {
                console.log(request.responseText);
            }
        });
    }
};

