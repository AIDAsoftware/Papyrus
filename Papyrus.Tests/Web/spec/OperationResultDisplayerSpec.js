// The Jasmine Test Framework
/// <reference path="~/../Papyrus.Web/Scripts/jquery-1.11.3.min.js"/>
/// <reference path="~/../Papyrus.Web/Scripts/OperationResultDisplayer.js"/>
/// <reference path="~/Web/lib/jasmine-2.3.4/jasmine.js" />

describe("OperationResultDisplayer", function () {

    beforeEach(function () {
        $("body").append('<div id="message-notifier" style="display: none"><h3></h3></div>');
    });

    it("show green confirmation message when a document is created", function() {
        var displayer = new papyrus.OperationResultDisplayer();
        var message = {
            title: "Document created",
            type: "success"
        }
        
        displayer.displayMessage(message);
        var message = $("#message-notifier"); //TODO: think a name
        expect(message.children("h3").text()).toEqual("Document created");
        expect(message.css("background-color")).toEqual("rgb(166, 215, 133)");
        expect(message.css("display")).toEqual("inline-block");
    });

    it("show error message in red when a document is not created", function () {
        var displayer = new papyrus.OperationResultDisplayer();
        var message = {
            title: "Cant create the document",
            type: "fail"
        }

        displayer.displayMessage(message);
        var message = $("#message-notifier");
        expect(message.children("h3").text()).toEqual("Cant create the document");
        expect(message.css("background-color")).toEqual("rgb(204, 0, 13)");
        expect(message.css("display")).toEqual("inline-block");
    });

    afterEach(function() {                    
        $("#message-notifier").remove();
    });
});