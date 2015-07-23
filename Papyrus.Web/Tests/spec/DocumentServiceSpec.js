describe("DocumentService", function(){
    it("should return a list of documents when there are documents and try to get all", function(){
		var apiRest = new DocumentApiClient();
        spyOn(apiRest, 'allDocuments').and.returnValue([
            {"Title": "Any", "Content": "Any", "Description": "Any", "Language": "es"}
        ]);
		var documentService = new DocumentService(apiRest);

		var documents = documentService.allDocuments();

        var papyrusDocument = new PapyrusDocument("Any", "Any", "Any", "Any");
        var expectedList = [papyrusDocument];
		expect(documents).toEqual(expectedList);
	});
});