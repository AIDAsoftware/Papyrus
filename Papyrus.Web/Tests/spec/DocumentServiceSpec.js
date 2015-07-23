describe("DocumentService", function(){

    /**
     * TODO:
     *  Get a document by Id
     *  Create a document
     */

    it("should return a list of documents when there are documents and try to get all", function(){
		var apiRest = new DocumentApiClient();
        spyOn(apiRest, 'allDocuments').and.returnValue([
            {"Id": "AnyId", "Title": "Any", "Content": "Any", "Description": "Any", "Language": "es"}
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
            {"Id": "AnyId", "Title": "Any", "Content": "Any", "Description": "Any", "Language": "es"}
        );
        var documentService = new DocumentService(apiRest);

        var document = documentService.GetDocument("AnyId");

        expect(apiRest.GetDocument).toHaveBeenCalledWith("AnyId");
        var expectedPapyrusDocument = anyPapyrusDocument();
        expect(document).toEqual(expectedPapyrusDocument);
    });

    function anyPapyrusDocument() {
        return new PapyrusDocument()
            .withId("AnyId")
            .withTitle("Any")
            .withContent("Any")
            .withDescription("Any")
            .forLanguage("es");
    }
});