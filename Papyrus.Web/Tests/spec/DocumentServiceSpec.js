describe("DocumentService", function(){
    
    const anyId = "AnyId",
          anyTitle = "AnyTitle",
          anyContent = "AnyContent",
          anyDescription = "AnyDescription",
          anyLanguage = "es",
          documentsURL = "http://localhost:8888/papyrusapi/documents/";

    it("should return a list of documents when there are documents and try to get all", function(){
        spyOn($, 'ajax').and.returnValue([anyPapyrusDocument()]);
		var documentService = new DocumentService();

		var documents = documentService.allDocuments();

        expect($.ajax).toHaveBeenCalledWith({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: documentsURL
        });
        var expectedList = [anyPapyrusDocument()];
		expect(documents).toEqual(expectedList);
	});

    it("should return a document when it exist and try to get it", function(){
        spyOn($, 'ajax').and.returnValue(anyPapyrusDocument());
        var documentService = new DocumentService();

        var document = documentService.GetDocument(anyId);

        expect($.ajax).toHaveBeenCalledWith({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: documentsURL + anyId
        });
        var expectedPapyrusDocument = anyPapyrusDocument();
        expect(document).toEqual(expectedPapyrusDocument);
    });

    it("should save a document when try to create it", function(){
        spyOn($, 'ajax');
        var documentService = new DocumentService();
        const document = anyDocumentWithoutId();

        documentService.createDocument(document);

        expect($.ajax).toHaveBeenCalledWith({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: documentsURL,
            data: {
                Title: anyTitle,
                Description: anyDescription,
                Content: anyContent,
                Language: anyLanguage
            }
        });
    });

    function anyDocumentWithoutId() {
        return {
            Title: anyTitle,
            Content: anyContent,
            Description: anyDescription,
            Language: anyLanguage
        }
    }

    function anyPapyrusDocument() {
        var document = anyDocumentWithoutId();
        document["Id"] = anyId;
        return document;
    }

    function getAllDocumentAjaxRequest() {
        return {
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:8888/papyrusapi/documents/"
        };
    }
});