// The Jasmine Test Framework
/// <reference path="/Scripts/jquery-1.11.3.min.js"/>
/// <reference path="/Scripts/DocumentService.js"/>
/// <reference path="~/Tests/lib/jasmine-2.3.4/jasmine.js" />

describe("DocumentService", function () {
    
    var anyId = "AnyId",
          anyTitle = "AnyTitle",
          anyContent = "AnyContent",
          anyDescription = "AnyDescription",
          anyLanguage = "es",
          documentsURL = "http://localhost:8888/papyrusapi/documents/"
    var documentService;

    beforeEach(function() {
        documentService = new papyrus.DocumentService();
    });

    it("should return a list of documents when there are documents and try to get all", function(){
        spyOn($, 'ajax').and.returnValue([anyDocument()]);

		var documents = documentService.allDocuments();

        var lastAjaxArgument = $.ajax.calls.mostRecent().args[0];
        expect(lastAjaxArgument.type).toEqual("GET");
        expect(lastAjaxArgument.contentType).toEqual("application/json; charset=utf-8");
        expect(lastAjaxArgument.url).toEqual(documentsURL);
        expect(documents).toEqual([anyDocument()]);
    });

    it("should return a document when it exist and try to get it", function(){
        spyOn($, 'ajax').and.returnValue(anyDocument());

        var document = documentService.getDocument(anyId);

        var lastAjaxArgument = $.ajax.calls.mostRecent().args[0];
        expect(lastAjaxArgument.type).toEqual("GET");
        expect(lastAjaxArgument.contentType).toEqual("application/json; charset=utf-8");
        expect(lastAjaxArgument.url).toEqual(documentsURL + anyId);
        expect(document).toEqual(anyDocument());
    });

    it("should save a document when try to create it", function(){
        spyOn($, 'ajax');
        var documentToSave = anyDocumentWithoutId();

        documentService.createDocument(documentToSave);

        var lastAjaxArgument = $.ajax.calls.mostRecent().args[0];
        expect(lastAjaxArgument.type).toEqual("POST");
        expect(lastAjaxArgument.contentType).toEqual("application/json; charset=utf-8");
        expect(lastAjaxArgument.url).toEqual(documentsURL);
        expect(lastAjaxArgument.data).toEqual(JSON.stringify(documentToSave));
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