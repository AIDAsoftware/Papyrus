describe("DocumentService", function(){

    /**
     * TODO:
     *
     */

    it("should return a list of documents when there are documents and try to get all", function(){
		var apiRest = new DocumentApiClient();
        spyOn(apiRest, 'allDocuments').and.returnValue([
            {"Title": "Any", "Content": "Any", "Description": "Any", "Language": "es"}
        ]);
		var documentService = new DocumentService(apiRest);

		var documents = documentService.allDocuments();

        var papyrusDocument = new PapyrusDocument("Any", "Any", "Any", "es");
        var expectedList = [papyrusDocument];
		expect(documents).toEqual(expectedList);
	});

    it("should return a document when it exist and try to get it", function(){
        var apiRest = new DocumentApiClient();
        spyOn(apiRest, 'GetDocument').and.returnValue([
            {"Id": "AnyId", "Title": "Any", "Content": "Any", "Description": "Any", "Language": "es"}
        ]);
        var documentService = new DocumentService(apiRest);

        var document = documentService.GetDocument("AnyId");

        var expectedPapyrusDocument = new PapyrusDocument("AnyId", "Any", "Any", "Any", "es");
        expect(document).toEqual(expectedPapyrusDocument);
    });
});