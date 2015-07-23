describe("DocumentService", function(){

    /**
     * TODO:
     *  Get a document by Id
     *  Create a document
     */
    
    const anyId = "AnyId";
    const anyTitle = "AnyTitle";
    const anyContent = "AnyContent";
    const anyDescription = "AnyDescription";
    const anyLanguage = "es";

    it("should return a list of documents when there are documents and try to get all", function(){
		var apiRest = new DocumentApiClient();
        spyOn(apiRest, 'allDocuments').and.returnValue([
            {"Id": anyId, "Title": anyTitle, "Content": anyContent, "Description": anyDescription, "Language": anyLanguage}
        ]);
		var documentService = new DocumentService(apiRest);

		var documents = documentService.allDocuments();

        var papyrusDocument = anyPapyrusDocument();
        var expectedList = [papyrusDocument];
		expect(documents).toEqual(expectedList);
	});

    it("should return a document when it exist and try to get it", function(){
        var apiRest = new DocumentApiClient();
        spyOn(apiRest, 'GetDocument').and.returnValue(
            {"Id": anyId, "Title": anyTitle, "Content": anyContent, "Description": anyDescription, "Language": anyLanguage}
        );
        var documentService = new DocumentService(apiRest);

        var document = documentService.GetDocument(anyId);

        expect(apiRest.GetDocument).toHaveBeenCalledWith(anyId);
        var expectedPapyrusDocument = anyPapyrusDocument();
        expect(document).toEqual(expectedPapyrusDocument);
    });

    function anyPapyrusDocument() {
        return new PapyrusDocument()
            .withId(anyId)
            .withTitle(anyTitle)
            .withContent(anyContent)
            .withDescription(anyDescription)
            .forLanguage(anyLanguage);
    }
});