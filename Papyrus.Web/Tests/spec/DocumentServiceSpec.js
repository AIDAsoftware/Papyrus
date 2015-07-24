describe("DocumentService", function(){
    
    const anyId = "AnyId",
          anyTitle = "AnyTitle",
          anyContent = "AnyContent",
          anyDescription = "AnyDescription",
          anyLanguage = "es",
          documentsURL = "http://localhost:8888/papyrusapi/documents/";

    it("should return a list of documents when there are documents and try to get all", function(){
        spyOn($, 'ajax').and.returnValue([anyDocument()]);
		var documentService = new DocumentService();

		var documents = documentService.allDocuments();

        const lastAjaxArgument = $.ajax.calls.mostRecent().args[0];
        expect(lastAjaxArgument.type).toEqual("GET");
        expect(lastAjaxArgument.contentType).toEqual("application/json; charset=utf-8");
        expect(lastAjaxArgument.url).toEqual(documentsURL);
        expect(documents).toEqual([anyDocument()]);
	});

    it("should return a document when it exist and try to get it", function(){
        spyOn($, 'ajax').and.returnValue(anyDocument());
        var documentService = new DocumentService();

        var document = documentService.GetDocument(anyId);

        const lastAjaxArgument = $.ajax.calls.mostRecent().args[0];
        expect(lastAjaxArgument.type).toEqual("GET");
        expect(lastAjaxArgument.contentType).toEqual("application/json; charset=utf-8");
        expect(lastAjaxArgument.url).toEqual(documentsURL + anyId);
        expect(document).toEqual(anyDocument());
    });

    it("should save a document when try to create it", function(){
        spyOn($, 'ajax');
        var documentService = new DocumentService();
        const documentToSave = anyDocumentWithoutId();

        documentService.createDocument(documentToSave);

        const lastAjaxArgument = $.ajax.calls.mostRecent().args[0];
        expect(lastAjaxArgument.type).toEqual("POST");
        expect(lastAjaxArgument.contentType).toEqual("application/json; charset=utf-8");
        expect(lastAjaxArgument.url).toEqual(documentsURL);
        expect(lastAjaxArgument.data).toEqual(documentToSave);
    });

    function anyDocumentWithoutId() {
        return {
            Title: anyTitle,
            Content: anyContent,
            Description: anyDescription,
            Language: anyLanguage
        }
    }

    function anyDocument() {
        var document = anyDocumentWithoutId();
        document["Id"] = anyId;
        return document;
    }
});